using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents "free annotation", i.e. a box with text associated to a specific point on the chart.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Free annotations are used to insert additional text and/or an image serving as explanation 
	/// of the chart contents. Annotations may have call-outs and they are rendered as flat 2D
	/// content on top of the rest of the chart.
	/// </para>
	/// <para>
	///Free annotation may be used to comment on the extreme data values or other data features. 
	///They may also be used to mark some special values on x and/or y-axis, 
	///such as a date when an event (affecting the values on the chart) took place.
	///</para>
	///<para>
	///Free annotations are implemented by the following classes:
	///	    <list type="bullet">
	///	      <item>
	///			ChartTextBox represents a rectangle with text, 
	///		  </item>
	///		  <item>
	///			TextAnchor defines the position of the annotation, i.e. the point in the chart 
	///			that the annotation is related to,
	///		  </item>
	///		  <item>
	///			TextBoxStyle defines the style of the annotation, and
	///		  </item>
	///		  <item>
	///			TextBoxStyleCollection contains predefined and user defined TextBoxStyle-s.
	///		  </item>
	///</para>
	/// </remarks>
	public class ChartTextBox 
	{
		private ChartTextBoxCollection owningCollection;
		private TextAnchor anchorPoint;
		private string text = "";
		private string styleName = "Default";
		private double initXOffset;
		private double initYOffset;
		private double maxWidthPts;

		private double xOffset;
		private double yOffset;

		private Color textColor = Color.Transparent;
		private PointF tipPoint;

		float ctsPts = 5; // width in points of the cartoon pointer triangle

		// Working data
		private Canvace canvace;
		private object font;
		private TextBoxStyle boxStyle;
		private RectangleF boxRect, textRect;
		
		
		internal ChartTextBox(string text, TextAnchor anchorPoint, string styleName,
			double initXOffset, double initYOffset, double maxWidthPts)
		{
			this.text = text;
			this.styleName = styleName;
			this.anchorPoint = anchorPoint;
			this.initXOffset = initXOffset;
			this.initYOffset = initYOffset;
			this.maxWidthPts = maxWidthPts;
			xOffset = initXOffset;
			yOffset = initYOffset;
		}

		internal ChartTextBoxCollection OwningCollection { get { return owningCollection; } set { owningCollection = value; } }

		#region --- Properties ---

		/// <summary>
		/// The style of the <see cref="ChartTextBox"/>.
		/// </summary>
		public TextBoxStyle Style
		{
			get 
			{
				if(boxStyle == null)
				{
					boxStyle = OwningChart.TextBoxStyles[styleName].Clone();
				}
				return boxStyle;

			}
			set { boxStyle = value; } 
		}


		/// <summary>
		/// The style name of the <see cref="ChartTextBox"/>.
		/// </summary>
		/// <remarks>
		/// In the setter, the name may be any predefined or custom style name.
		/// </remarks>
		public string StyleName { get { return styleName; } set { styleName = value; } }

		/// <summary>
		/// The style kind of the <see cref="ChartTextBox"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// In the setter, the kind may be any predefined kind. The getter returns "Custom" for all the custom styles.
		/// </para>
		/// <para>
		/// This is an alternative to the <see cref="StyleName"/> property which works with intelisense.
		/// However, it can only be used for predefined styles.
		/// </para>
		/// </remarks>
		public TextBoxStyleKind StyleKind { get { return TextBoxStyle.KindOf(styleName); } set { styleName = TextBoxStyle.NameOf(value); } }
		
        /// <summary>
		/// The text of the annotation.
		/// </summary>
		public string Text { get { return text; } set { text = value; } }
		
        /// <summary>
		/// The text color of the annotation.
		/// </summary>
		public Color TextColor { get { return textColor; } set { textColor = value; } }

		/// <summary>
		/// X offset (in points) of the anchor point from the text reference point of the annotation box.
		/// </summary>
		/// <remarks>
		/// By default, the text reference point is the mid-point of the bottom line of the box. Usually this point doesn't
		/// coincide with the anchor point. The reference point and the anchor point are sometimes connected by a
		/// line to indicate association between the annotation and the point in the chart.
		/// </remarks>
		public double XOffset { get { return xOffset; } set { xOffset = value; } }

		/// <summary>
		/// Y offset (in points) of the anchor point from the text reference point of the annotation box.
		/// </summary>
		/// <remarks>
        /// By default, the text reference point is the mid-point of the bottom line  of the box. Usually this point doesn't
		/// coincide with the anchor point. The reference point and the anchor point are sometimes connected by a
		/// line to indicate association between the annotation and the point in the chart.
		/// </remarks>
		public double YOffset { get { return yOffset; } set { yOffset = value; } }
	
        /// <summary>
		/// Object describing the point in the chart that this <see cref="ChartTextBox"/> is associated to.
		/// </summary>
		public TextAnchor AnchorPoint { get { return anchorPoint; } set { anchorPoint = value; } }

		internal ChartBase OwningChart { get { return OwningCollection.Owner as ChartBase; } }
	
		#region --- Conversion Factors ---
		private float ScaleFactor { get { return (float)Chart.ScaleToNativeSize; } }
		private float PointToPixel { get { return (float)Chart.FromPointToTarget; } }
		#endregion

		private ChartBase Chart { get { return owningCollection.Owner as ChartBase; } }

		#endregion

		#region --- Rendering ---

		internal void Render(Canvace canvace)
		{
			this.canvace = canvace;
			font = canvace.CreateFont(Style.FontName,Style.FontSize*ScaleFactor,Style.FontStyle);

			SizeF textSize = GetTextSize();
			boxRect = GetOuterRectangle(textSize);
			
			textRect = new RectangleF((float)(boxRect.X+Style.LeftMargin*PointToPixel), (float)(boxRect.Y+Style.TopMargin*PointToPixel),
				textSize.Width,textSize.Height);
			RenderBackground();
			RenderText();
		}

		private SizeF GetTextSize()
		{
			SizeF textSize;
			if(maxWidthPts <= 0)
				textSize = canvace.MeasureString(text,font,Style.Orientation);
			else
				textSize = canvace.MeasureString(text,font,(int)(maxWidthPts*PointToPixel),Style.Orientation);
			return textSize;
		}

		private RectangleF GetOuterRectangle(SizeF textSize)
		{
			// Rectangle containing the text rectangle and text margins
			PointF anchorPt = anchorPoint.Position;
			double refPtX = anchorPt.X - xOffset*PointToPixel;
			double refPtY = anchorPt.Y - yOffset*PointToPixel;
			double boxWidth = textSize.Width + (Style.LeftMargin + Style.RightMargin)*PointToPixel;
			double boxHeight = textSize.Height + (Style.TopMargin + Style.BottomMargin)*PointToPixel;
			double x0 = 0, y0 = 0;
			switch(Style.TextReferencePoint)
			{
				case TextReferencePoint.CenterBottom: x0 = refPtX - boxWidth/2; y0 = refPtY; break;
				case TextReferencePoint.Center: x0 = refPtX - boxWidth/2; y0 = refPtY + boxHeight/2; break;
				case TextReferencePoint.CenterTop: x0 = refPtX - boxWidth/2; y0 = refPtY - boxHeight; break;
				case TextReferencePoint.LeftBottom: x0 = refPtX; y0 = refPtY; break;
				case TextReferencePoint.LeftCenter: x0 = refPtX; y0 = refPtY - boxHeight/2; break;
				case TextReferencePoint.LeftTop: x0 = refPtX; y0 = refPtY - boxHeight; break;
				case TextReferencePoint.RightBottom: x0 = refPtX - boxWidth; y0 = refPtY; break;
				case TextReferencePoint.RightCenter: x0 = refPtX - boxWidth; y0 = refPtY - boxHeight/2; break;
				case TextReferencePoint.RightTop: x0 = refPtX - boxWidth; y0 = refPtY - boxHeight; break;
				default: x0 = refPtX - boxWidth/2; y0 = refPtY - boxHeight/2; break;
			}

			return new RectangleF((float)x0, (float)y0, (float)boxWidth, (float)boxHeight);
		}

		private void RenderBackground()
		{
			if(Style.TextBoxBorderKind == TextBoxBorderKind.None)
				return;

			Color borderColor = Style.BorderColor;
			float borderWidth = Style.BorderWidth;
			DashStyle borderDashStyle = Style.BorderDashStyle;
			object pen = canvace.CreatePen(borderColor,borderWidth,borderDashStyle,LineCap.Flat);

			if(Style.TextBoxBorderKind == TextBoxBorderKind.OneSideLine)
			{
				switch(Style.TextReferencePoint)
				{
					case TextReferencePoint.LeftBottom:
						tipPoint = new PointF(boxRect.X,boxRect.Y); break;
					case TextReferencePoint.CenterBottom:
						tipPoint = new PointF(boxRect.X+boxRect.Width*0.5f,boxRect.Y); break;
					case TextReferencePoint.RightBottom:
						tipPoint = new PointF(boxRect.X+boxRect.Width,boxRect.Y); break;
					case TextReferencePoint.LeftCenter:
						tipPoint = new PointF(boxRect.X,boxRect.Y+boxRect.Height*0.5f); break;
					case TextReferencePoint.RightCenter:
						tipPoint = new PointF(boxRect.X+boxRect.Width,boxRect.Y+boxRect.Height*0.5f); break;
					case TextReferencePoint.LeftTop:
						tipPoint = new PointF(boxRect.X,boxRect.Y+boxRect.Height); break;
					case TextReferencePoint.CenterTop:
						tipPoint = new PointF(boxRect.X+boxRect.Width*0.5f,boxRect.Y+boxRect.Height); break;
					case TextReferencePoint.RightTop:
						tipPoint = new PointF(boxRect.X+boxRect.Width,boxRect.Y+boxRect.Height); break;
					default:
						tipPoint = new PointF(boxRect.X+boxRect.Width*0.5f,boxRect.Y+boxRect.Height*0.5f); break;
				}
				PointF point1 = PointF.Empty, point2 = PointF.Empty;
				if(Style.BorderVisible)
				{
					switch(Style.TextReferencePoint)
					{
						case TextReferencePoint.LeftBottom:
						case TextReferencePoint.CenterBottom:
						case TextReferencePoint.RightBottom:
							point1 = new PointF(boxRect.X,boxRect.Y);
							point2 = new PointF(boxRect.X+boxRect.Width,boxRect.Y);
							break;
						case TextReferencePoint.LeftCenter:
							point1 = new PointF(boxRect.X,boxRect.Y);
							point2 = new PointF(boxRect.X,boxRect.Y+boxRect.Height);
							break;
						case TextReferencePoint.RightCenter:
							point1 = new PointF(boxRect.X+boxRect.Width,boxRect.Y);
							point2 = new PointF(boxRect.X+boxRect.Width,boxRect.Y+boxRect.Height);
							break;
						case TextReferencePoint.LeftTop:
						case TextReferencePoint.CenterTop:
						case TextReferencePoint.RightTop:
							point1 = new PointF(boxRect.X,boxRect.Y+boxRect.Height);
							point2 = new PointF(boxRect.X+boxRect.Width,boxRect.Y+boxRect.Height);
							break;
						default:
							point1 = new PointF(0,0);
							point2 = point1;
							break;
					}
				}
				if(point1 != point2)
					canvace.DrawLine(pen, point1, point2);
			}
			else // Rectangle or rounded rectangle
			{
				GraphicsPath path = GetBackgroundPath(0);
				PointF tipPt = tipPoint; // Save it
				if(Style.FillBackground)
				{
					double shadeWidth = Style.ShadeWidth;
					if(shadeWidth > 0)
					{
						GraphicsPath bPath = GetBackgroundPath((float)(shadeWidth*PointToPixel));
						canvace.FillPath(Color.FromArgb(64,0,0,0),bPath);
						bPath.Dispose();
						tipPoint = tipPt; // Restore it
					}
					object background;
					if(Style.GradientKind == GradientKind.None)		
						background = Style.BackgroundColor;
					else
						background = canvace.CreateBrush(Style.GradientKind,path,Style.BackgroundColor,Style.BackgroundEndingColor);
					canvace.FillPath(background,path);
				}
				// Render border
				if(Style.BorderVisible)
					canvace.DrawPath(pen,path);
				path.Dispose();
			}
			// Render pointer
			RenderPointer();
		}

		#region --- creating the box sections

		private void AddLBPoint(GraphicsPath path,float offset)
		{// Left-bottom point
			if(Style.TextReferencePoint == TextReferencePoint.LeftBottom)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+offset-cts*1.41f,boxRect.Y-offset-cts*1.41f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+offset+cts/1.41f,boxRect.Y-offset),
						tipPoint,
						new PointF(boxRect.X+offset,boxRect.Y-offset+cts/1.41f)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+offset,boxRect.Y-offset);
				}
			}
			else
			{
				float lm = 2*(float)(Style.LeftMargin*PointToPixel);
				float bm = 2*(float)(Style.BottomMargin*PointToPixel);
				float tm = 2*(float)(Style.TopMargin*PointToPixel);
				float rm = 2*(float)(Style.RightMargin*PointToPixel);
				switch(Style.TextBoxBorderKind)
				{
					case TextBoxBorderKind.RoundedRectangle:
						path.AddArc(boxRect.X+offset,boxRect.Y-offset,2*lm,2*bm,270,-90);
						break;
					default:
						path.AddLine(new PointF(boxRect.X+offset+1,boxRect.Y-offset),new PointF(boxRect.X+offset,boxRect.Y-offset));
						break;
				}
			}
		}

		private void AddLCPoint(GraphicsPath path,float offset)
		{// Left-center point
			if(Style.TextReferencePoint == TextReferencePoint.LeftCenter)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+offset-1.5f*cts,boxRect.Y-offset + boxRect.Height/2);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+offset,boxRect.Y-offset + (boxRect.Height-cts)/2),
						tipPoint,
						new PointF(boxRect.X+offset,boxRect.Y-offset+ (boxRect.Height+cts)/2)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+offset,boxRect.Y-offset+boxRect.Height/2);
				}
			}
		}

		private void AddLTPoint(GraphicsPath path,float offset)
		{// Left-top point
			if(Style.TextReferencePoint == TextReferencePoint.LeftTop)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+offset-cts*1.41f,boxRect.Y+boxRect.Height-offset+cts*1.41f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+offset,boxRect.Y+boxRect.Height-offset-cts/1.41f),
						tipPoint,
						new PointF(boxRect.X+offset+cts/1.41f,boxRect.Y+boxRect.Height-offset)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+offset,boxRect.Y+boxRect.Height-offset);
				}
			}
			else
			{
				float lm = 2*(float)(Style.LeftMargin*PointToPixel);
				float bm = 2*(float)(Style.BottomMargin*PointToPixel);
				float tm = 2*(float)(Style.TopMargin*PointToPixel);
				float rm = 2*(float)(Style.RightMargin*PointToPixel);

				switch(Style.TextBoxBorderKind)
				{
					case TextBoxBorderKind.RoundedRectangle:
						path.AddArc(boxRect.X+offset,boxRect.Y-offset+boxRect.Height-2*tm,2*lm,2*bm,180,-90);
						break;
					default:
						path.AddLine(new PointF(boxRect.X+offset,boxRect.Y+boxRect.Height-offset-1),new PointF(boxRect.X+offset,boxRect.Y+boxRect.Height-offset));
						break;
				}
			}
		}

		private void AddCTPoint(GraphicsPath path,float offset)
		{// Center-top point
			if(Style.TextReferencePoint == TextReferencePoint.CenterTop)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)				
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+offset+boxRect.Width/2,boxRect.Y+boxRect.Height-offset+cts*1.5f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+offset+(boxRect.Width-cts)/2,boxRect.Y+boxRect.Height-offset),
						tipPoint,
						new PointF(boxRect.X+offset+(boxRect.Width+cts)/2,boxRect.Y+boxRect.Height-offset)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+offset+boxRect.Width/2,boxRect.Y+boxRect.Height-offset);
				}
			}
			// else add nothing
		}

		private void AddRTPoint(GraphicsPath path,float offset)
		{// Right-top point
			if(Style.TextReferencePoint == TextReferencePoint.RightTop)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+boxRect.Width+offset+cts*1.41f,boxRect.Y+boxRect.Height-offset+cts*1.41f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+boxRect.Width+offset-cts/1.41f,boxRect.Y+boxRect.Height-offset),
						tipPoint,
						new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y+boxRect.Height-offset-cts/1.41f)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y+boxRect.Height-offset);
				}
			}
			else
			{
				float lm = 2*(float)(Style.LeftMargin*PointToPixel);
				float bm = 2*(float)(Style.BottomMargin*PointToPixel);
				float tm = 2*(float)(Style.TopMargin*PointToPixel);
				float rm = 2*(float)(Style.RightMargin*PointToPixel);

				switch(Style.TextBoxBorderKind)
				{
					case TextBoxBorderKind.RoundedRectangle:
						path.AddArc(boxRect.X+offset+boxRect.Width-2*rm,boxRect.Y-offset+boxRect.Height-2*tm,2*lm,2*bm,90,-90);
						break;
					default:
						path.AddLine(new PointF(boxRect.X+boxRect.Width+offset-1,boxRect.Y+boxRect.Height-offset),new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y+boxRect.Height-offset));
						break;
				}
			}
		}

		private void AddRCPoint(GraphicsPath path,float offset)
		{// Right-center point
			if(Style.TextReferencePoint == TextReferencePoint.RightCenter)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+boxRect.Width+offset+cts*1.5f,boxRect.Y-offset + boxRect.Height*0.5f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y-offset+(boxRect.Height+cts)/2),
						tipPoint,
						new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y-offset+(boxRect.Height-cts)/2)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y-offset + boxRect.Height*0.5f);
				}
			}
			// else add nothing
		}

		private void AddRBPoint(GraphicsPath path,float offset)
		{// Right-Bottom point
			if(Style.TextReferencePoint == TextReferencePoint.RightBottom)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+boxRect.Width+offset+cts*1.41f,boxRect.Y-offset-cts*1.41f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y-offset+cts/1.41f),
						tipPoint,
						new PointF(boxRect.X+boxRect.Width+offset-cts/1.41f,boxRect.Y-offset)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y-offset);
				}
			}
			else
			{
				float lm = 2*(float)(Style.LeftMargin*PointToPixel);
				float bm = 2*(float)(Style.BottomMargin*PointToPixel);
				float tm = 2*(float)(Style.TopMargin*PointToPixel);
				float rm = 2*(float)(Style.RightMargin*PointToPixel);

				switch(Style.TextBoxBorderKind)
				{
					case TextBoxBorderKind.RoundedRectangle:
						path.AddArc(boxRect.X+offset+boxRect.Width-2*rm,boxRect.Y-offset,2*lm,2*bm,0,-90);
						break;
					default:
						path.AddLine(new PointF(boxRect.X+boxRect.Width+offset-1,boxRect.Y-offset),new PointF(boxRect.X+boxRect.Width+offset,boxRect.Y-offset));
						break;
				}
			}
		}

		private void AddCBPoint(GraphicsPath path,float offset)
		{// Center-bottom point
			if(Style.TextReferencePoint == TextReferencePoint.CenterBottom)
			{
				if(Style.TextBoxAnchorKind == TextBoxAnchorKind.Cartoon)
				{
					float cts = ctsPts*PointToPixel;
					tipPoint = new PointF(boxRect.X+boxRect.Width/2+offset,boxRect.Y-offset-cts*1.5f);
					path.AddLines(new PointF[] 
					{
						new PointF(boxRect.X+offset+(boxRect.Width+cts)/2,boxRect.Y-offset),
						tipPoint,
						new PointF(boxRect.X+offset+(boxRect.Width-cts)/2,boxRect.Y-offset)
					}
						);
				}
				else
				{
					tipPoint = new PointF(boxRect.X+boxRect.Width/2+offset,boxRect.Y-offset);
				}
			}
			// else add nothing
		}


		#endregion

		private GraphicsPath GetBackgroundPath(float offset)
		{
			GraphicsPath path = new GraphicsPath();
			AddCBPoint(path,offset);
			AddLBPoint(path,offset);
			AddLCPoint(path,offset);
			AddLTPoint(path,offset);
			AddCTPoint(path,offset);
			AddRTPoint(path,offset);
			AddRCPoint(path,offset);
			AddRBPoint(path,offset);
			path.CloseFigure();
			return path;
		}

		private PointF GetRefPoint()
		{
			switch(Style.TextReferencePoint)
			{
				case TextReferencePoint.LeftBottom:	return new PointF(boxRect.X, boxRect.Y);
				case TextReferencePoint.LeftCenter:	return new PointF(boxRect.X, boxRect.Y+0.5f*boxRect.Height);
				case TextReferencePoint.LeftTop:	return new PointF(boxRect.X, boxRect.Y+boxRect.Height);
				case TextReferencePoint.Center:	return new PointF(boxRect.X+0.5f*boxRect.Width, boxRect.Y+0.5f*boxRect.Height);
				case TextReferencePoint.CenterBottom:	return new PointF(boxRect.X+0.5f*boxRect.Width, boxRect.Y);
				case TextReferencePoint.CenterTop:	return new PointF(boxRect.X+0.5f*boxRect.Width, boxRect.Y+boxRect.Height);
				case TextReferencePoint.RightBottom:	return new PointF(boxRect.X+boxRect.Width, boxRect.Y);
				case TextReferencePoint.RightCenter:	return new PointF(boxRect.X+boxRect.Width, boxRect.Y+0.5f*boxRect.Height);
				case TextReferencePoint.RightTop:	return new PointF(boxRect.X+boxRect.Width, boxRect.Y+boxRect.Height);
				default: return new PointF(boxRect.X+0.5f*boxRect.Width, boxRect.Y);
			}
		}

		private void RenderPointer()
		{
			LineCap cap;
			switch(Style.TextBoxAnchorKind)
			{
				case TextBoxAnchorKind.LineArrow: cap = LineCap.ArrowAnchor; break;
				case TextBoxAnchorKind.LineCircle: cap = LineCap.RoundAnchor; break;
				case TextBoxAnchorKind.LineSquare: cap = LineCap.SquareAnchor; break;
				case TextBoxAnchorKind.LineDiamond: cap = LineCap.DiamondAnchor; break;
				default: cap = LineCap.NoAnchor; break;
			}
			
			canvace.DrawLine(canvace.CreatePen(Style.BorderColor,Style.BorderWidth,Style.BorderDashStyle,cap), 
				tipPoint,anchorPoint.Position);			
		}

		private void RenderText()
		{
			Color color = TextColor;
			if(color.A == 0)
				color = Style.TextColor;
			canvace.DrawString(text,font,color,textRect,Style.Alignment,Style.Orientation);
		}

		#endregion

	}

	internal class ChartTextBoxCollection : CollectionWithType
	{
		public ChartTextBoxCollection() : this (null) {}
		public ChartTextBoxCollection(object owner) : base (typeof(ChartTextBox),owner) {}
		internal ChartTextBox this[int index]
		{
			get { return (ChartTextBox)(base[index]); }
			set { base[index] = value; }
		}
		
		internal int Add(ChartTextBox ctb)
		{
			if(ctb != null)
			{
				ctb.OwningCollection = this;
				return base.Add(ctb);
			}
			else
				return -1;
		}
	}
	

}
