using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for Renderer.
	/// </summary>
	internal class Renderer
	{
		protected SubGauge gauge;
		protected RenderingContext context;
		protected IEngine engine;
		protected Skin skin;

		internal Renderer(SubGauge gauge, RenderingContext context)
		{
            this.gauge = gauge;
			this.context = context;
			engine = gauge.Factory.CreateEngine();
			skin = gauge.Skin;
		}

		internal virtual void Render() 
		{
			if(gauge.Theme != null)
			{
				RenderControlBackground();
				RenderGaugeBackground();
				RenderContents();
				RenderCover();
				RenderFrame();
			}
			else
				RenderSubgauges();

		}

		#region --- Rendering Steps ---

		protected void RenderControlBackground()
		{
			if((gauge as IObjectModelNode).ParentNode == null)
				engine.DrawBackground(gauge.BackgroundImage,gauge.BackgroundImageLayout,context);
		}

		protected void RenderGaugeBackground()
		{
			// Rendering gauge background
			if(skin != null)
			{
				Layer background = skin.Background();
				if(background != null)
				{
					background.BackgroundColor = skin.BackgroundBaseColor;
					RenderLayer(background);
				}
			}
		}
		public virtual void RenderContents()
		{
			if(SubGauge.IsInGroup(gauge.GaugeKind,GaugeKindGroup.WithScale))
				RenderWithScale();
			else if(gauge.GaugeKind == GaugeKind.Numeric)
				RenderNumeric();
			else
				RenderIndicator();
		}

		public virtual void RenderSubgauges()
		{
			// Rendering subgauges
			foreach(SubGauge g in gauge.SubGauges)
			{
				if((g as IObjectModelNode).ParentNode == null)
					(g as IObjectModelNode).ParentNode = gauge;
				g.Render(context);
			}
		}

		public virtual void RenderCover()
		{					
			if(skin != null)
			{
				// Rendering top
				RenderLayer(skin.Cover());
			}
		}

		public virtual void RenderFrame()
		{						
			if(skin != null)
			{
				// Rendering frame
				Layer frame = skin.Frame();
				if(frame != null)
				{
					frame.BackgroundColor = skin.FrameBaseColor; 
					RenderLayer(frame);
				}
			}
		}

		#endregion

		#region --- Rendering Helpers ---

		protected void RenderLayer(Layer layer)
		{
			if(layer == null)
				return;

			LayerVisualPart region = layer.Region;
			if(region != null)
				region.RenderAsRegion(context,layer.BackgroundColor);

			LayerVisualPartCollection parts = layer.MainVisualParts;
			if(parts != null)
			{
				for(int i=0; i<parts.Count; i++)
					parts[i].Render(context);
			}
		}

		#endregion

		#region --- Rendering Geometries ---

		#region --- Rendering With Scale ---

		private void RenderWithScale()
		{
			// Rendering free annotations
			foreach(ImageAnnotation a in gauge.ImageAnnotations)
				a.Render(context);
			foreach(TextAnnotation ta in gauge.TextAnnotations)
				ta.Render(context,GaugeGeometry.LinearSize(gauge));
			
			// Rendering ranges - strips first
			foreach(Scale scale in gauge.Scales)
			{
				foreach(Range range in scale.Ranges)
					range.RenderStripOnly(context);
			}

			// Rendering pointers below tickmarks
			foreach(Scale scale in gauge.Scales)
			{
				foreach(Pointer pointer in scale.Pointers)
				{
					PointerStyle pStyle = pointer.Style;
					if(pStyle != null && !pStyle.BarAboveTickMarks && pStyle.PointerKind == PointerKind.Bar)
						pointer.Render(context);
				}
			}

			// Rendering ranges - strips first
			foreach(Scale scale in gauge.Scales)
			{
				foreach(Range range in scale.Ranges)
					range.RenderTickMarksOnly(context);
			}

			// Render annotations
			foreach(Scale scale in gauge.Scales)
			{
				foreach(Range range in scale.Ranges)
				{
					foreach(Annotation ann in range.Annotations)
						ann.Render(context);
				}
			}
			// Render indicators
			foreach(Indicator indicator in gauge.Indicators)
				indicator.Render(context);

			RenderSubgauges();
			
			// Rendering pointers
			foreach(Scale scale in gauge.Scales)
			{
				foreach(Pointer pointer in scale.Pointers)
				{
					PointerStyle pStyle = pointer.Style;
					if(pStyle != null && (pStyle.BarAboveTickMarks || pStyle.PointerKind != PointerKind.Bar))
						pointer.Render(context);
				}
			}
		}

		#endregion

		#region --- Rendering Numeric ---

		private void RenderNumeric()
		{
			Rectangle2D numericGaugeRectangle = gauge.Skin.NumericGaugeRectangle;
			RenderingContext numericGaugeContext = null;
			if(numericGaugeRectangle != null && numericGaugeRectangle.Size.Width > 0 && numericGaugeRectangle.Size.Height > 0)
			{
				Size2D ngSize = context.TargetArea.Size;
				numericGaugeContext = context.SetAreaMapping(new Rectangle2D(0,0,
					numericGaugeRectangle.X*ngSize.Width*0.01f,
					numericGaugeRectangle.Y*ngSize.Height*0.01f),
					new Rectangle2D(
					numericGaugeRectangle.X*ngSize.Width*0.01f,
					numericGaugeRectangle.Y*ngSize.Height*0.01f,
					numericGaugeRectangle.Width*ngSize.Width*0.01f,
					numericGaugeRectangle.Height*ngSize.Height*0.01f),true);
			}
			else
				numericGaugeContext = context;

			string text = gauge.Value.ToString(gauge.Skin.NumericFormattingString);
			TextRenderingContext trc = engine.Factory.CreateTextRenderingContext(gauge.Skin.NumericTextStyle,numericGaugeContext,GaugeGeometry.LinearSize(gauge));
			float relValue = (float)gauge.MainScale.WorldToRelative(gauge.Value);

			if(gauge.Skin.NumericTextStyle.FontColor.IsEmpty)
				trc.FontColor = gauge.Palette.NumericColor.ColorAt(relValue); 
			else
				trc.FontColor = gauge.Skin.NumericTextStyle.FontColor; 

			if(gauge.Skin.NumericTextStyle.DecimalFontColor.IsEmpty)
				trc.AlternateFontColor = gauge.Palette.NumericDecimalColor.ColorAt(relValue); 
			else
				trc.AlternateFontColor = gauge.Skin.NumericTextStyle.DecimalFontColor; 

			if(gauge.Skin.NumericTextStyle.FontBackColor.IsEmpty)
				trc.FontBackColor = gauge.Palette.NumericBackColor.ColorAt(relValue);
			else
				trc.FontBackColor = gauge.Skin.NumericTextStyle.FontBackColor; 

			if(gauge.Skin.NumericTextStyle.DecimalFontBackColor.IsEmpty)
				trc.AlternateFontBackColor = gauge.Palette.NumericDecimalBackColor.ColorAt(relValue); 
			else
				trc.AlternateFontBackColor = gauge.Skin.NumericTextStyle.DecimalFontBackColor; 

			// note that in LCD display we never use a place for dec digit
			bool displayDecimalPoint = gauge.Skin.DisplayDecimalPoint && gauge.Skin.NumericDisplayKind!=NumericDisplayKind.LCD;
			trc.DisplayDecimalPoint = displayDecimalPoint;

			int np = gauge.Skin.NumberOfPositions;
			if(np > 0)
			{
				bool thereIsDecPoint = text.IndexOf(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) >= 0;
				int textLength = text.Length;
				if(thereIsDecPoint && !displayDecimalPoint)
					textLength --;
				if(textLength > np)
				{
					text = "";
					for(int i=0; i<np; i++)
						text = text + "*";
				}
				else
				{
					for(int i=textLength; i<gauge.Skin.NumberOfPositions; i++)
						text = " " + text;
				}
			}
			
			switch(gauge.Skin.NumericDisplayKind)
			{
				case NumericDisplayKind.Simple:
					RenderNumericSimple(text,trc,numericGaugeContext);
					break;
				case NumericDisplayKind.Book:
					RenderNumericBook(text,trc,numericGaugeContext);
					break;
				case NumericDisplayKind.Mechanical:
					RenderNumericMechanical(text,trc,numericGaugeContext);
					break;
				case NumericDisplayKind.LCD:
					RenderNumericLCD(text,trc,numericGaugeContext);
					break;
				default: break;
			}
		}

		private void RenderNumericSimple(string text, TextRenderingContext trc, RenderingContext numContext)
		{
			engine.DrawText(text,gauge.Skin.NumericTextStyle,numContext,trc);
		}

		private void RenderNumericBook(string text, TextRenderingContext trc, RenderingContext numContext)
		{
			Layer overlayLayer = gauge.DigitMaskLayers["Book",GaugeKind.Numeric];
			if(overlayLayer != null)
				engine.DrawTextWithCharacterOverlay(text,gauge.Skin.NumericTextStyle,overlayLayer,numContext,trc);
		}

		private void RenderNumericMechanical(string text, TextRenderingContext trc, RenderingContext numContext)
		{
			Layer overlayLayer = gauge.DigitMaskLayers["Mechanical",GaugeKind.Numeric];
			if(overlayLayer != null)
				engine.DrawTextWithCharacterOverlay(text,gauge.Skin.NumericTextStyle,overlayLayer,numContext,trc);
		}

		private void RenderNumericLCD(string text, TextRenderingContext trc, RenderingContext numContext)
		{
			engine.DrawLCDText(text,numContext,trc);
		}

		#endregion

		#region --- Rendering Indicators ---

		private void RenderIndicator()
		{
		}

		#endregion
		
		#endregion
	}

	// ======================================================================================================
	//
	// ======================================================================================================

//	internal class RendererNumeric : Renderer
//	{
//		internal RendererNumeric(Gauge gauge, RenderingContext context) : base(gauge,context) { }
//
//		public override void RenderContents()
//		{	
//			switch(gauge.Skin.NumericDisplayKind)
//			{
//				case NumericDisplayKind.Simple:
//					RenderNumericSimple();
//					break;
//				case NumericDisplayKind.Book:
//					RenderNumericBook();
//					break;
//				case NumericDisplayKind.LCD:
//					RenderNumericLCD();
//					break;
//				default: break;
//			}
//		}
//
//		private void RenderNumericSimple()
//		{
//		}
//
//		private void RenderNumericBook()
//		{
//		}
//
//		private void RenderNumericLCD()
//		{
//		}
//	}
//

	//	internal class RendererCircular : RendererWithScale
//	{
//		internal RendererCircular(Gauge gauge, RenderingContext context) : base(gauge,context) { }
//
//		#region --- Rendering Gauge ---
//
//		internal override void Render()
//		{
//			RenderControlBackground();
//			RenderGaugeBackground();
//
//			// Rendering free annotations
//			foreach(ImageAnnotation a in gauge.ImageAnnotations)
//				a.Render(context);
//			foreach(TextAnnotation ta in gauge.TextAnnotations)
//				ta.Render(context,GaugeGeometry.LinearSize(gauge));
//			
//			// Rendering ranges
//			foreach(Scale scale in gauge.Scales)
//			{
//				foreach(Range range in scale.Ranges)
//					range.RenderStrip(context);
//			}
//			// Render annotations
//			foreach(Scale scale in gauge.Scales)
//			{
//				foreach(Range range in scale.Ranges)
//				{
//					foreach(Annotation ann in range.Annotations)
//						ann.Render(context);
//				}
//			}
//
//			// Rendering subgauges
//			foreach(Gauge g in gauge.SubGauges)
//			{
//				if((g as IObjectModelNode).ParentNode == null)
//					(g as IObjectModelNode).ParentNode = gauge;
//				g.Render(context);
//			}
//			
//			// Rendering pointers
//			foreach(Scale scale in gauge.Scales)
//			{
//				foreach(Pointer pointer in scale.Pointers)
//					pointer.Render(context);
//			}
//			
//			if(skin != null)
//			{
//				// Rendering top
//				RenderLayer(skin.Cover(gauge.GaugeKind));
//
//				// Rendering frame
//				Layer frame = skin.Frame(gauge.GaugeKind);
//				if(frame != null)
//				{
//					frame.BackgroundColor = skin.FrameBaseColor; 
//					RenderLayer(frame);
//				}
//			}
//		}
//
//		private void RenderLayer(Layer layer)
//		{
//			if(layer == null)
//				return;
//
//			LayerVisualPart region = layer.Region;
//			if(region != null)
//				region.RenderAsRegion(context,layer.BackgroundColor);
//
//			LayerVisualPartCollection parts = layer.MainVisualParts;
//			if(parts != null)
//			{
//				for(int i=0; i<parts.Count; i++)
//					parts[i].Render(context);
//			}
//		}
//
//		#endregion
//
//	}
}
