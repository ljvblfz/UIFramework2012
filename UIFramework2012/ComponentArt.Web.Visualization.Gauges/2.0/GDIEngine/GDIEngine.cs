using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	/// <summary>
	/// Summary description for GDIEngine.
	/// </summary>
	internal class GDIEngine : IEngine, IDisposable
	{
		private Factory factory;
		private GDIBitmapVisualPart[] LCDDigits = new GDIBitmapVisualPart[15];
		private GDIBitmapVisualPart indicatorCircleImage;
		private GDIBitmapVisualPart indicatorRectImage;

		private ArrayList bitmaps = new ArrayList();

		public GDIEngine()
		{
			CreateImages();
		}

		#region --- Handling GDI Resources ---
		
		public void Dispose()
		{
			foreach(Bitmap bmp in bitmaps)
				bmp.Dispose();
			bitmaps.Clear();
			for(int i=0; i<LCDDigits.Length; i++)
				if(LCDDigits[i] != null)
				{
					LCDDigits[i].Dispose();
					LCDDigits[i] = null;
				}
		}

		internal void RegisterBitmap(Bitmap bmp)
		{
			if(bmp != null)
				bitmaps.Add(bmp);
		}

		#endregion

		internal void SetFactory(Factory factory)
		{
			this.factory = factory;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		virtual public Factory Factory { get { return factory; } }

		private void CreateImages()
		{
			/// LCD Digits
			for(int i = 0; i<10; i++)
			{
				string name = "LCD" + i.ToString() + ".png";
				LCDDigits[i] = GDIBitmapVisualPart.CreateFromResourcePath(i.ToString(),name);
			}
			LCDDigits[10] = GDIBitmapVisualPart.CreateFromResourcePath("Dot","LCDdot.png");
			LCDDigits[11] = GDIBitmapVisualPart.CreateFromResourcePath("Empty","LCDempty.png");
			LCDDigits[12] = GDIBitmapVisualPart.CreateFromResourcePath("Empty","LCDdash.png");

			// Indicator images
			indicatorCircleImage = GDIBitmapVisualPart.CreateFromResourcePath("Circle","IndicatorCircle.png");
			indicatorRectImage = GDIBitmapVisualPart.CreateFromResourcePath("Circle","IndicatorRect.png");
		}

		public virtual LayerVisualPart IndicatorCircleImage() { return indicatorCircleImage; } 
		public virtual LayerVisualPart IndicatorRectImage() { return indicatorRectImage; } 

		#region --- Rendering ---
		#region --- Drawing Background Image ---

		public virtual void DrawBackground(object backgroundImage,ImageLayout backgroundImageLayout,RenderingContext context)
		{
			if(backgroundImage == null)
				return;

			Graphics g = context.RenderingTarget as Graphics;
			Image bgImage = backgroundImage as Image;
			if(g == null || bgImage == null)
				return;

			int h = bgImage.Height;
			int w = bgImage.Width;
			int gh = (int)context.Size.Height;
			int gw = (int)context.Size.Width;

			// Now we have to turn y coordinate upside-down to comply with image rendering in GDI+
			Matrix mtx = new Matrix();
			mtx.Scale(1.0f,-1.0f);
			Matrix mtx1 = g.Transform;
			Matrix mtx2 = mtx1.Clone();
			mtx2.Multiply(mtx,MatrixOrder.Prepend);
			mtx2.Translate(0,-gh,MatrixOrder.Prepend);
			g.Transform = new Matrix();//mtx2;
			
			Rectangle srcRect = new Rectangle(0,0,w,h);
			Rectangle destRect = Rectangle.Empty;
			switch (backgroundImageLayout)
			{
				case ImageLayout.Center:
					destRect = new Rectangle((gw-w)/2,(gh-h)/2,w,h);
					break;
				case ImageLayout.None:
					destRect = srcRect;
					break;
				case ImageLayout.Stretch:
					destRect = new Rectangle(0,0,gw,gh);
					break;
				case ImageLayout.Zoom:
				{
					float fx = ((float)gw)/w;
					float fy = ((float)gh)/h;
					float f = Math.Min(fx,fy); // this is "zoom to fit in"; use max for "zoom to cover"
					int w1 = (int)(f*w+0.5);
					int h1 = (int)(f*h+0.5);
					destRect = new Rectangle((gw-w1)/2,(gh-h1)/2,w1,h1);
				}
					break;
				case ImageLayout.Tile:
				{
					for(int ix = 0; ix < gw; ix += w)
						for(int iy = 0; iy < gh; iy += h)
						{
							g.DrawImage(bgImage,ix,iy);
						}
					// restore transformation
					g.Transform = mtx1;
					return;
				}
			}
			g.DrawImage(bgImage,destRect,srcRect,GraphicsUnit.Pixel);
			// restore transformation
			g.Transform = mtx1;
		}

		#endregion

		#region --- Rendering Image ---
		public virtual void DrawImage(object imageObject, Size2D relativeSize, Point2D location, RenderingContext context)
		{
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				return;
				
			float ww = context.TargetArea.Width*relativeSize.Width*0.01f;
			float hh = context.TargetArea.Height*relativeSize.Height*0.01f;
            PointF centerPoint = context.TransformPoint(location); 
			int gw = (int)ww;
			int gh = (int)hh;

			Image image = imageObject as Image;

			Matrix mtx = g.Transform;
			g.Transform = new Matrix();
			if(image == null) // Create placeholder
			{
				gw = Math.Max(4,gw);
				gh = Math.Max(4,gw);
				int x = (int)(centerPoint.X-gw/2);
				int y = (int)(centerPoint.Y-gh/2);
				g.DrawRectangle(Pens.Red,x,y,gw,gh);
			}
			else
			{
				int h = image.Height;
				int w = image.Width;
				if(relativeSize == null || relativeSize.Width == 0 || relativeSize.Height == 0)
				{
					gh = h;
					gw = w;
				}

				// Now we have to turn y coordinate upside-down to comply with image rendering in GDI+
				g.DrawImage(image,(int)(centerPoint.X-gw/2f),(int)(centerPoint.Y-gh/2f),gw,gh);
			}
			g.Transform = mtx;
		}

		public virtual object GetImage(string imageFile)
		{
			try
			{
				Bitmap bmp = new Bitmap(imageFile);
				RegisterBitmap(bmp);
				return bmp;
			}
			catch
			{
				return null;
			}
		}


		#endregion

		#region --- Filling Areas ---

		public virtual void FillArea(PointF[] points,Color color, RenderingContext context)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Brush brush = new SolidBrush(color);
			Graphics g = context.RenderingTarget as Graphics;
			g.FillPath(brush,path);
			path.Dispose();
			brush.Dispose();
		}

		public virtual void FillRadialArea(PointF[] points,MultiColor mc, Color centerColor, Point2D centerPoint, Point2D[] gradientLine, RenderingContext context)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Brush brush = CreateRadialBrush(mc,centerColor,centerPoint,gradientLine);
			Graphics g = context.RenderingTarget as Graphics;
			g.FillPath(brush,path);
			path.Dispose();
			brush.Dispose();
		}
		public virtual void FillLinearArea(PointF[] points,MultiColor mc, Point2D gradientStartPoint, Point2D gradientEndPoint, RenderingContext context)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Brush brush = CreateLinearBrush(mc,gradientStartPoint,gradientEndPoint);
			Graphics g = context.RenderingTarget as Graphics;
			g.FillPath(brush,path);
			path.Dispose();
			brush.Dispose();
		}


		private Brush CreateRadialBrush(MultiColor mc, Color centerColor, Point2D centerPoint, Point2D[] gradientLine) 
		{
			if(mc.IsEmpty)
				return null;
			if(mc.IsSolid)
				return new SolidBrush(mc.ColorStops[0].Color);

			// Creating non-trivial path brush

			// We are inserting internal color stops positions, twice for the case of solid multicolor

			int n = gradientLine.Length + 2*(mc.ColorStops.Count-2);
			float[] lengths = new float[n];
			Color[] colors = new Color[n+1];
			PointF[] gp = new PointF[n+1];
			float d = 0;
			for(int i=0; i<gradientLine.Length; i++)
			{
				if(i > 0)
				{
					d = (float)(d + (gradientLine[i]-gradientLine[i-1]).Abs());
				}
				lengths[i] = d;
				gp[i] = new PointF(gradientLine[i].X,gradientLine[i].Y);
			}
			for(int i=0; i<gradientLine.Length; i++)
			{
				lengths[i] = lengths[i]/lengths[gradientLine.Length-1];
				colors[i] = mc.ColorAt(lengths[i]);
			}

			// Add color stops at the end

			int m = gradientLine.Length; // next index
			for(int i=1; i<mc.ColorStops.Count-1; i++)
			{
				lengths[m] = (float)mc.ColorStops[i].Position;
				if(mc.Solid)
					colors[m] = mc.ColorStops[i-1].Color;
				else
					colors[m] = mc.ColorStops[i].Color;
				m++;
				lengths[m] = (float)mc.ColorStops[i].Position;
				colors[m] = mc.ColorStops[i].Color;
				m++;
			}
			for(int i=gradientLine.Length; i<n; i++)
			{
				if(lengths[i] < 0.0001)
					gp[i] = new PointF(gradientLine[0].X,gradientLine[0].Y);
				else if(lengths[i] > 0.9999)
					gp[i] = new PointF(gradientLine[gradientLine.Length-1].X,gradientLine[gradientLine.Length-1].Y);
				else
				{
					// interpolate the point
					int j;
					for(j=1; j<gradientLine.Length; j++)
					{
						if(lengths[j] >= lengths[i])
							break;
						float a = (lengths[i] - lengths[j-1])/(lengths[j] - lengths[j-1]);
						gp[i] = new PointF(
							a*gradientLine[j].X + (1-a)*gradientLine[j-1].X,
							a*gradientLine[j].Y + (1-a)*gradientLine[j-1].Y);
					}
				}
			}

			// Move the color stops to the right position
			for(int k=gradientLine.Length; k<n; k++)
			{
				for(int i=k; i>0; i--)
				{
					if(lengths[i] < lengths[i-1])
					{
						// switch
						float a = lengths[i];
						lengths[i] = lengths[i-1];
						lengths[i-1] = a;
						Color color = colors[i];
						colors[i] = colors[i-1];
						colors[i-1] = color;
						PointF pointF = gp[i];
						gp[i] = gp[i-1];
						gp[i-1] = pointF;
					}
					else
						break;
				}
			}

			// Add the center point
			gp[n] = new PointF(centerPoint.X,centerPoint.Y);
			colors[n] = centerColor;

			PathGradientBrush pgb = new PathGradientBrush(gp);
			pgb.CenterColor = centerColor;
			pgb.CenterPoint = new PointF(centerPoint.X,centerPoint.Y);
			pgb.SurroundColors = colors;

			return pgb;
		}

		private Brush CreateLinearBrush(MultiColor mc, Point2D startPoint, Point2D endPoint)
		{
			if(mc.IsEmpty)
				return null;
			if(mc.IsSolid)
				return new SolidBrush(mc.ColorStops[0].Color);

			// Creating non-trivial linear brush

			float[] positions;
			Color[] colors;
			mc.GetColorsAndPositions(out colors, out positions);

			LinearGradientBrush lgb = new LinearGradientBrush(startPoint,endPoint,Color.White, Color.Black);
			ColorBlend blend = new ColorBlend(colors.Length);
			blend.Colors = colors;
			blend.Positions = positions;
			lgb.InterpolationColors = blend;

			return lgb;
		}

		#endregion

		#region --- Rendering TickMarks and Markers ---

		public virtual MapArea TickMarkMapArea(TickMark marker, float dx, float dy, RenderingContext context)
		{
			GraphicsPath path = GetPath(marker,dx,dy);
			path.Flatten();
			int n = path.PointCount;
			Point[] points = new Point[n];
			for(int i=0; i<n; i++)
			{
				PointF T = context.TransformPoint(path.PathPoints[i]);
				points[i] = new Point((int)(T.X + 0.5f),(int)(T.Y + 0.5f));
			}

			MapAreaPolygon mapArea = new MapAreaPolygon(points);
			mapArea.SetObject(marker);
			return mapArea;
		}

		internal GraphicsPath GetPath(TickMark marker, float dx, float dy)
		{
			GraphicsPath path = new GraphicsPath();
			float r = Math.Min(dx, dy);

			switch(marker.Style.Kind)
			{
				case MarkerKind.Rectangle:
					path.AddRectangle(new RectangleF(0,0,dx,dy));
					break;
				case MarkerKind.Circle:
					path.AddEllipse(new RectangleF((dx-r)/2, (dy-r)/2, r, r));
					break;
				case MarkerKind.Ellipse:
					path.AddEllipse(new RectangleF(0, 0, dx, dy));
					break;
				case MarkerKind.Diamond:
					path.AddPolygon(
						new PointF[] 
							{
								new PointF(0,dy/2),
								new PointF(dx/2,dy),
								new PointF(dx,dy/2),
								new PointF(dx/2,0)
							}
						);
					break;
				case MarkerKind.Trapeze:
					path.AddPolygon(
						new PointF[] 
							{
								new PointF(dx/4,0),
								new PointF(0,dy),
								new PointF(dx,dy),
								new PointF(3*dx/4,0)
							});
					break;
				case MarkerKind.TrapezeInverted:
					path.AddPolygon(
						new PointF[] 
							{
								new PointF(0,0),
								new PointF(dx/4,dy),
								new PointF(3*dx/4,dy),
								new PointF(dx,0)
							});
					break;
				case MarkerKind.Triangle:
					path.AddPolygon(
						new PointF[] 
							{
								new PointF(0,0),
								new PointF(dx/2,dy),
								new PointF(dx,0),
					});
					break;
				case MarkerKind.TriangleInverted:
					path.AddPolygon(
						new PointF[] 
							{
								new PointF(dx/2,0),
								new PointF(0,dy),
								new PointF(dx,dy),
					}
						);
					break;
				default:
					path.AddRectangle(new RectangleF(0,0,dx,dy));
					break;
			}
			return path;
		}

		public virtual void DrawTickMark(TickMark marker, float dx, float dy,RenderingContext context, TickMarkRenderingContext tmContext)
		{
			// Draw tickmark in rectangle (0,0,dx,dy)

			GDITickMarkRenderingContext gdiTmContext = tmContext as GDITickMarkRenderingContext;

			// NB: Shadow handling is missing
			Graphics g = context.RenderingTarget as Graphics;
			GraphicsPath path = GetPath(marker,dx,dy);

			if (path != null)
			{
				if (gdiTmContext.ShadowBrush != null)
				{
					Matrix mtx1 = g.Transform;
					Matrix mtx2 = mtx1.Clone();
					float dd = Math.Max(dx, dy);
					mtx2.Translate(
						gdiTmContext.ShadowOffset.Width * dd * 0.01f, 
						gdiTmContext.ShadowOffset.Width * dd * 0.01f, 
						MatrixOrder.Append);
					g.Transform = mtx2;
					g.FillPath(gdiTmContext.ShadowBrush, path);
					g.Transform = mtx1;
				}
				g.FillPath(gdiTmContext.BackgroundBrush, path);
				if (gdiTmContext.ContourPen.Width > 0)
					g.DrawPath(gdiTmContext.ContourPen,path);
				path.Dispose();
			}
		}

		#endregion

		#region --- Rendering Visual Parts ---

		public virtual void DrawNiddle(LayerVisualPart part, Point2D centerPoint, Point2D tipPoint,
			RenderingContext context)
		{
			Point2D centerS = new Point2D(
				part.RelativeCenterPoint.X*part.Size.Width*0.01f,
				part.RelativeCenterPoint.Y*part.Size.Height*0.01f);
			Point2D tipS = new Point2D(
				part.RelativeEndPoint.X*part.Size.Width*0.01f,
				part.RelativeEndPoint.Y*part.Size.Height*0.01f);
			RenderingContext niddleContext = context.DefineMapping(centerS,tipS,centerPoint,tipPoint);
			part.RenderAsNiddle(niddleContext);	
		}

		public virtual void DrawNiddleRegion(LayerVisualPart part, Point2D centerPoint, Point2D tipPoint,
			RenderingContext context, Color backgroundColor)
		{
			Point2D centerS = new Point2D(
				part.RelativeCenterPoint.X*part.Size.Width*0.01f,
				part.RelativeCenterPoint.Y*part.Size.Height*0.01f);
			Point2D tipS = new Point2D(
				part.RelativeEndPoint.X*part.Size.Width*0.01f,
				part.RelativeEndPoint.Y*part.Size.Height*0.01f);
			RenderingContext niddleContext = context.DefineMapping(centerS,tipS,centerPoint,tipPoint);
			part.RenderAsNiddleRegion(niddleContext, backgroundColor);	
		}

		public virtual void RenderWatermark(RenderingContext context)
		{
			Graphics g = (Graphics)context.RenderingTarget;
			//save the original matrix, and put a blank one in so the image is not transformed
			Matrix mtx = g.Transform;
			g.Transform = new Matrix();			
				
			string name = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources.gauge-watermark.png";
			System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
			Bitmap wMark = (Bitmap)Bitmap.FromStream(stream);
			
			int w = Math.Min(wMark.Width, (int)context.Size.Width - 10);
			int h = Math.Min(wMark.Height, (int)context.Size.Height - 10);
			int x = ((int)context.Size.Width - w) / 2;
			int y = ((int)context.Size.Height - h) / 2;
			g.DrawImage(wMark, x, y, w, h);
			wMark.Dispose();

			name = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources.gauge-watermark-fineprint.png";
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
			wMark = (Bitmap)Bitmap.FromStream(stream);

			w = wMark.Width;
			h = wMark.Height;
			x = ((int)context.Size.Width - wMark.Width) / 2;
			y = (int)context.Size.Height - wMark.Height - 2;
			if (x >= 0 && y >= 0)
				g.DrawImage(wMark, x, y, w, h);
			wMark.Dispose();

			//restore the original graphics matrix
			g.Transform = mtx;
		}

		public virtual void RenderErrorMessage(string message, RenderingContext context)
		{
			Graphics g = (Graphics)context.RenderingTarget;
			Matrix mtx = g.Transform;
			g.Transform = new Matrix();			

			Font font = new Font("Arial",10);
			float h = g.MeasureString(message,font,(int)(context.Size.Width-10)).Height;
			g.DrawString("Exception",font,Brushes.DarkRed,new RectangleF(5,5,context.Size.Width-10,context.Size.Height-10));
			g.DrawString(message,font,Brushes.Red,new RectangleF(10,25,context.Size.Width-10,context.Size.Height-10));
			g.DrawString("Please fix the problem and try again.",font,Brushes.DarkRed,new RectangleF(5,30+h,context.Size.Width-10,context.Size.Height-10));
			font.Dispose();
			g.Transform = mtx;
		}

		#endregion

		#region --- Rendering Texts ---

		public virtual void DrawText(TextAnnotation text, RenderingContext context, TextRenderingContext trContext)
		{
			GDITextRenderingContext gdiTrContext = trContext as GDITextRenderingContext;
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				return;
			// text.Location is center point in relative coordinates
			Point2D center = text.Location;
			// Unit vector of the text direction and normal
			Size2D dir = new Size2D((float)Math.Cos(text.AngleDegrees/180*Math.PI),(float)Math.Sin(text.AngleDegrees/180*Math.PI));
			Size2D norm = dir.Normal();
			// Text size
			Size2D tSize = trContext.MeasureString(text.Text);
			float w = tSize.Width;
			float h = tSize.Height;
			// Two points on the middle line of the text rectangle
			Point2D P0 = center - w*dir*0.5;
			Point2D P1 = P0 + dir*w;
			// Define context so that(0,h/2) --> P0 , (w,h/2) --> P1 where w and h are text dimensions
			RenderingContext textContext = context.DefineMapping(new Point2D(0,0), new Point2D(w,0),P0,P1);
			g = textContext.RenderingTarget as Graphics;
			// Now we have to turn y coordinate upside-down to comply with text rendering in GDI+
			Matrix mtx = new Matrix();
			mtx.Scale(1.0f,-1.0f);
			Matrix mtx1 = g.Transform;
			Matrix mtx2 = mtx1.Clone();
			mtx2.Multiply(mtx,MatrixOrder.Prepend);
			mtx2.Translate(0,-h/2,MatrixOrder.Prepend);
			g.Transform = mtx2;

			TextStyle style = text.TextStyle;
			if(gdiTrContext.ShadowBrush != null)
			{
				float offset = style.ShadeOffsetPerc * gdiTrContext.Font.Size * 0.01f; // style.ShadeOffsetPerc * h * 0.01f;
				g.DrawString(text.Text,gdiTrContext.Font,gdiTrContext.ShadowBrush,offset,offset,StringFormat.GenericTypographic);
			}
			if(gdiTrContext.FontBrush != null)
				g.DrawString(text.Text,gdiTrContext.Font,gdiTrContext.FontBrush,0,0,StringFormat.GenericTypographic);
			// restore transformation
			g.Transform = mtx1;
		}

		public virtual void DrawText(string text, TextStyle style, RenderingContext context, TextRenderingContext trContext)
		{
			GDITextRenderingContext gdiTrContext = trContext as GDITextRenderingContext;
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				return;
            
			// Computing the text rendering context
			Size2D textSize = trContext.MeasureString(text);
			Size2D targSize = context.TargetArea.Size;
			float fx = targSize.Width/textSize.Width;
			float fy = targSize.Height/textSize.Height;
			float f = Math.Min(fx,fy);
			Rectangle2D newTargetArea = new Rectangle2D(
				context.TargetArea.X + targSize.Width - f*textSize.Width,
				context.TargetArea.Y + (targSize.Height - f*textSize.Height)/2,
				f*textSize.Width, f*textSize.Height);
			RenderingContext textContext = context.SetAreaMapping(new Rectangle2D(0,0,textSize.Width,textSize.Height),newTargetArea,true);

			g = textContext.RenderingTarget as Graphics;
			// Now we have to turn y coordinate upside-down to comply with text rendering in GDI+
			Matrix mtx = new Matrix();
			mtx.Scale(1.0f,-1.0f);
			Matrix mtx1 = g.Transform;
			Matrix mtx2 = mtx1.Clone();
			mtx2.Multiply(mtx,MatrixOrder.Prepend);
			mtx2.Translate(0,-textSize.Height,MatrixOrder.Prepend);
			g.Transform = mtx2;

			if(gdiTrContext.ShadowBrush != null)
			{
				float offset = style.ShadeOffsetPerc * gdiTrContext.Font.Size * 0.01f; // NB: style.ShadeOffsetPerc * h * 0.01f;
				g.DrawString(text,gdiTrContext.Font,gdiTrContext.ShadowBrush,offset,offset,StringFormat.GenericTypographic);
			}
			if(gdiTrContext.FontBrush != null)
				g.DrawString(text,gdiTrContext.Font,gdiTrContext.FontBrush,0,0,StringFormat.GenericTypographic);
			// restore transformation
			g.Transform = mtx1;
		}

		public virtual void DrawTextWithCharacterOverlay(string text, TextStyle style, Layer characterOverlayLayer, RenderingContext context, TextRenderingContext trContext)
		{
			GDITextRenderingContext gdiTrContext = trContext as GDITextRenderingContext;
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				return;

			string sep = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			int ixDec = text.IndexOf(sep);
			if(ixDec >= 0)
			{
				if(trContext.DisplayDecimalPoint)
					ixDec++;
				else
					text = text.Remove(ixDec,sep.Length);
			}

			Size2D chSize = characterOverlayLayer.Size;
			float totalW = chSize.Width*text.Length;
			float totalH = chSize.Height;
			RenderingContext totalContext = context.SetAreaMapping(new Rectangle2D(0,0,totalW,totalH),true);

			GDIBitmapVisualPart vpr = characterOverlayLayer.Region as GDIBitmapVisualPart;
			GDIBitmapVisualPart vp = characterOverlayLayer.MainVisualParts[0] as GDIBitmapVisualPart;
			GDIBitmapVisualPart vpBig = new GDIBitmapVisualPart();

			Bitmap bigBmp = null;
			Graphics g1 = null;
			Bitmap bmp = null;
			if (vpr != null)
			{
			    lock (vpr)
			    {
				    bmp = vpr.Bitmap;		
				    bigBmp = new Bitmap(bmp.Width * text.Length, bmp.Height); 
				    vpBig.Bitmap = bigBmp;
				    g1 = Graphics.FromImage(bigBmp);
				    
				    for(int i=0; i<text.Length;i++)
				    {
					    ImageAttributes imageAttributes = null;
					    
					    if(ixDec >= 0 && i >= ixDec)
						    imageAttributes = GetImageAttributes(gdiTrContext.AlternateFontBackColor);
					    else
						    imageAttributes = GetImageAttributes(gdiTrContext.FontBackColor);

                        g1.DrawImage(bmp, new Rectangle(i * bmp.Width, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttributes);
                    }
				    vpBig.Render(totalContext);
				    g1.Clear(Color.Transparent);
				}
			}
			else if (vp != null)
			{
			    lock (vp)
			    {
                    bigBmp = new Bitmap(vp.Bitmap.Width * text.Length, vp.Bitmap.Height);
                    g1 = Graphics.FromImage(bigBmp);
				    vpBig.Bitmap = bigBmp;
				}
			}
			else
				return;

            lock (vp)
            {
			    bmp = vp.Bitmap;
                for (int i = 0; i < text.Length; i++)
                    g1.DrawImage(bmp, new Rectangle(i * bmp.Width, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel);
            }
            
            g1.Dispose();

			for(int i=0; i<text.Length;i++)
			{
				if(text.Substring(i,1) == " ")
					continue;
				Size2D thisCharacterSize = trContext.MeasureString(text.Substring(i,1));
				float w = chSize.Height*thisCharacterSize.Width/thisCharacterSize.Height;
				w = Math.Min(w,chSize.Width);
				RenderingContext chContext = totalContext.SetAreaMapping(
					new Rectangle2D(0,0,w,chSize.Height),
					new Rectangle2D(i*chSize.Width + (chSize.Width-w)/2,0,w,chSize.Height),
//					new Rectangle2D(i*chSize.Width-5,0,chSize.Width+10,chSize.Height),
					true);
				if(ixDec >= 0 && i >= ixDec)
					trContext.FontColor = trContext.AlternateFontColor;
				DrawText(text.Substring(i,1),style,chContext,trContext);
			}
			vpBig.Render(totalContext);
			vpBig.Dispose();
		}
		
		public virtual void DrawLCDText(string text, RenderingContext context, TextRenderingContext trContext)
		{
			GDITextRenderingContext gdiTrContext = trContext as GDITextRenderingContext;
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				return;

			string sep = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			int ixDec = text.IndexOf(sep);
			if(ixDec >= 0)
			{
				text = text.Remove(ixDec,sep.Length);
			}

            lock (LCDDigits)
            {
			    Size2D chSize = LCDDigits[0].Size;
			    int width = (int)chSize.Width;
			    int height = (int)chSize.Height;
			    float totalW = chSize.Width*text.Length;
			    float totalH = chSize.Height;
			    RenderingContext totalContext = context.SetAreaMapping(new Rectangle2D(0,0,totalW,totalH),true);

			    Bitmap bigBmp = new Bitmap(text.Length*width,height);
			    Graphics g1 = Graphics.FromImage(bigBmp);
    			
			    for(int i=0; i<text.Length;i++)
			    {
				    Brush background = null;
				    ImageAttributes lcdAttributes = null;
				    if(ixDec >= 0 && i >= ixDec)
				    {
					    lcdAttributes = GetLCDImageAttributes(gdiTrContext.AlternateFontColor);
					    background = gdiTrContext.AlternateFontBackBrush;
				    }
				    else
				    {
					    lcdAttributes = GetLCDImageAttributes(gdiTrContext.FontColor);
					    background = gdiTrContext.FontBackBrush;
				    }
				    Rectangle destRectangle = new Rectangle(i*width,0,width,height);
				    g1.FillRectangle(background,destRectangle);
				    Bitmap digitBmp = GetLCDDigit(text[i]);
				    g1.DrawImage(digitBmp,destRectangle,0,0,digitBmp.Width,digitBmp.Height,GraphicsUnit.Pixel,lcdAttributes);
				    if(i==ixDec-1 && i<text.Length-1)
					    g1.DrawImage(LCDDigits[10].Bitmap,destRectangle,0,0,digitBmp.Width,digitBmp.Height,GraphicsUnit.Pixel,lcdAttributes);
			    }
			    
                GDIBitmapVisualPart vpBig = new GDIBitmapVisualPart();
                vpBig.Bitmap = bigBmp;
                vpBig.Render(totalContext);
                vpBig.Dispose();
                g1.Dispose();
            }
		}

		private Bitmap GetLCDDigit(char digit)
		{
			if('0' <= digit && digit <= '9')
				return LCDDigits[(int)(digit-'0')].Bitmap;
			if(digit == ',')
				return LCDDigits[10].Bitmap;
			else if(digit == ' ')
				return LCDDigits[11].Bitmap;
			else 
				return LCDDigits[12].Bitmap;
		}
		
		private ImageAttributes GetImageAttributes(Color color)
		{
			// Create image attribute to set color to the region bitmap
			float R = (float)color.R / 255f;
			float G = (float)color.G / 255f;
			float B = (float)color.B / 255f;

			ColorMatrix colorMatrix;

			// Color transformation mode:
			// To use transparency info only and to set the color
			bool useTransparencyOnly = true;

			if (useTransparencyOnly)
			{
				colorMatrix = new ColorMatrix(
					new float[][] {
									  new float[] { 0, 0, 0, 0, 0 },
									  new float[] { 0, 0, 0, 0, 0 },
									  new float[] { 0, 0, 0, 0, 0 },
									  new float[] { 0, 0, 0, 1, 0 },
									  new float[] { R, G, B, 0, 1 }
								  });
			}
			else
			{
				// We assume that the original region color = (0.5,0.5,0.5)
				R = 2 * R / 3;
				G = 2 * G / 3;
				B = 2 * B / 3;

				colorMatrix = new ColorMatrix(
					new float[][] {
									  new float[] { R, G, B, 0, 0 },
									  new float[] { R, G, B, 0, 0 },
									  new float[] { R, G, B, 0, 0 },
									  new float[] { 0, 0, 0, 1, 0 },
									  new float[] { R, G, B, 0, 1 }
								  });

			}
			ImageAttributes imageAttrs = new ImageAttributes();
			imageAttrs.SetColorMatrix(colorMatrix);
			return imageAttrs;
		}	
	
		private ImageAttributes GetLCDImageAttributes(Color color)
		{
			// Create image attribute to set color to the region bitmap
			float R = (float)color.R / 256f;
			float G = (float)color.G / 256f;
			float B = (float)color.B / 256f;
			float A = (float)color.A / 256f;

			float H = 0.50f;
			float V = 0.15f;

			ColorMatrix colorMatrix = new ColorMatrix(
				new float[][] {
								  new float[] { R, G, B, A, 0 }, // Red row
								  new float[] { 0, 0, 0, 0, 0 },
								  new float[] { H, H, H, V, 0 }, // Blue row - inactive cells
								  new float[] { 0, 0, 0, 0, 0 }, // Alpha row
								  new float[] { 0, 0, 0, 0, 1 } // Translation row
							  });
			ImageAttributes imageAttrs = new ImageAttributes();
			imageAttrs.SetColorMatrix(colorMatrix);
			return imageAttrs;
		}

		#endregion

		#endregion

	}
}
