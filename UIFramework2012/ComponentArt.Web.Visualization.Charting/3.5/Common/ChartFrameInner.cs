using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// =============================================================================================================
	//	ChartFrameInner Class
	// =============================================================================================================

	internal abstract class ChartFrameInner
	{
		private ChartBase		owningChart;
		private Rectangle	rect;
		private bool		roundTop = false;
		private bool		roundBottom = false;
		private int			cornerRadius = 20;
		private bool		textShade = false;
		protected float		scaleFactor = 0.5f;
		private Font		scaledFont = null;

		private FrameTextPosition	textPosition = FrameTextPosition.Top;
		private StringAlignment		textAlignment = StringAlignment.Near;
		private Color				textColor = Color.Transparent;
		private Font				font = new Font("Arial",12);
		private string	text =		"";

		protected ChartFrameInner() { }

		internal Rectangle	Rectangle			{ get { return rect; }			set { rect = value; } }
		internal bool		RoundTopCorners		{ get { return roundTop; }		set { roundTop = value; } }
		internal bool		RoundBottomCorners	{ get { return roundBottom; }	set { roundBottom = value; } }
		internal int		CornerRadius		{ get { return cornerRadius; }	set { cornerRadius = value; } }

		internal FrameTextPosition	TextPosition	{ get { return textPosition; }		set { textPosition = value; } }
		internal StringAlignment	TextAlignment	{ get { return textAlignment; }		set { textAlignment = value; } }

		internal bool				TextShade		{ get { return textShade; } set { textShade = value; } }
		internal ChartBase OwningChart { get { return owningChart; } set { owningChart = value; } }

		internal Font	ScaledFont				
		{
			get 
			{
				if(scaledFont == null)
					scaledFont = new Font(font.FontFamily,font.Size*scaleFactor,font.Style,GraphicsUnit.Pixel);
				return scaledFont; 
			}
		}

		internal Font	Font				
		{
			get { return font; }
			set { font = value; scaledFont = null; } 
		}
		internal Color	FontColor			
		{
			get 
			{ 
				if(textColor.A == 0 && OwningChart != null && !OwningChart.InSerialization)
					return OwningChart.Palette.FrameFontColor;
				return textColor; 
			}	
			set { textColor = value; } 
		}
		internal string	Text				{ get { return text; }			set { text = value; } }

		internal void SetScaleFactor(float scaleFactor)
		{
			this.scaleFactor = scaleFactor;
			scaledFont = null;
		}
		
		internal float ScaleFactor 
		{
			get {return scaleFactor;}
			set {this.scaleFactor = value; scaledFont = null;}
		}

		public abstract void Render(Graphics g);

		protected GraphicsPath ExternalBorder(Rectangle rect)
		{
			PointF[] pts = new PointF[50];
			int nPts;
			ChartFrameEffect.CreateClosedPolygon(rect,roundTop,roundBottom,cornerRadius*scaleFactor,pts,out nPts);
			return ChartFrameEffect.CreatePath(pts,nPts,0,0,0);
		}

		protected void RenderText(Graphics g, int inputShadeWidth)
		{
			GraphicsContainer container = g.BeginContainer();
			float shadeWidth = inputShadeWidth*scaleFactor;
			// Text metrics
			Font font = ScaledFont;
			SizeF textSize = g.MeasureString(text,font);
			int textH = (int)(textSize.Height+1);

			Brush brush = new SolidBrush(FontColor);
			Brush bBrush = new SolidBrush(Color.FromArgb(150,0,0,0));
			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			PointF point = new PointF(0,0);
			int tOff = (int)(12*scaleFactor); // fixed position offset
			switch(textPosition)
			{
				case FrameTextPosition.Bottom:
					point.X = tOff;
					point.Y = rect.Height - textSize.Height - shadeWidth-1;
					if(textAlignment == StringAlignment.Center)
						point.X = (rect.Width-textSize.Width)/2;
					else if(textAlignment == StringAlignment.Far)
						point.X = rect.Width - textSize.Width - tOff;
					if(textShade)
						g.DrawString(text,font,bBrush,new PointF(point.X+1,point.Y+1));
					g.DrawString(text,font,brush,point);
					break;
				case FrameTextPosition.Top:
					point.X = tOff;
					point.Y = shadeWidth+1;
					if(textAlignment == StringAlignment.Center)
						point.X = (rect.Width-textSize.Width)/2;
					else if(textAlignment == StringAlignment.Far)
						point.X = rect.Width - textSize.Width - tOff;
					if(textShade)
						g.DrawString(text,font,bBrush,new PointF(point.X+1,point.Y+1));
					g.DrawString(text,font,brush,point);
					break;
				case FrameTextPosition.Left:
					point.Y = textSize.Width + tOff;
					point.X = shadeWidth+textSize.Height+1;
					if(textAlignment == StringAlignment.Center)
						point.Y = (rect.Height+textSize.Width)/2;
					else if(textAlignment == StringAlignment.Near)
						point.Y = rect.Height - tOff;					
					g.TranslateTransform(point.X,point.Y);
					g.RotateTransform(180.0f);
					if(textShade)
						g.DrawString(text,font,bBrush,new PointF(-1,1),new StringFormat(StringFormatFlags.DirectionVertical));
					g.DrawString(text,font,brush,new PointF(0,0),new StringFormat(StringFormatFlags.DirectionVertical));
					break;
				case FrameTextPosition.Right:
					point.Y = textSize.Width + tOff;
					point.X = rect.Width;
					if(textAlignment == StringAlignment.Center)
						point.Y = (rect.Height+textSize.Width)/2;
					else if(textAlignment == StringAlignment.Near)
						point.Y = rect.Height - tOff;					
					g.TranslateTransform(point.X,point.Y);
					g.RotateTransform(180.0f);
					if(textShade)
						g.DrawString(text,font,bBrush,new PointF(-1,1),new StringFormat(StringFormatFlags.DirectionVertical));
					g.DrawString(text,font,brush,new PointF(0,0),new StringFormat(StringFormatFlags.DirectionVertical));
					break;
			}
			g.EndContainer(container);
			brush.Dispose();
			bBrush.Dispose();
			g.EndContainer(container);
		}
	}

}
