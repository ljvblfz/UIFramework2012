using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Xml;

using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	internal class GDIBitmapVisualPart : GDILayerVisualPart, IDisposable
	{
		private Bitmap bmp = null;
		private XmlElement bitmapNode;

		internal GDIBitmapVisualPart(string name) : base(name) { }
		internal GDIBitmapVisualPart() : base() { }
		internal override Size2D Size 
		{ 
			get 
			{
			    if (bmp == null)
                    throw new Exception("BitmapVisualPart '" + Name + "' doesn't have the bitmap defined");

				return base.Size;
			}
			set 
			{
				base.Size = value; 
			}
		}

		public void Dispose()
		{
		    if (bmp != null)
		    {
			    bmp.Dispose();
		        bmp = null;
		    }
		}

		internal void GetFromFile(string filePath)
		{
			try
			{
			    lock (this)
			    {
				    bmp = new Bitmap(filePath);
				    this.Size = new Size2D(bmp.Width, bmp.Height); 
				}
			}
			catch(Exception ex)
			{
				throw new Exception("LayerVisualPart '" + Name + "' cannot be created from file '" + filePath + "'", ex);
			}
		}

		internal void GetFromResource(string resourceName)
		{
			try
			{
				string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
				string name = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources." + resourceName;
				Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
				lock (this) 
				{
				    bmp = (Bitmap)Bitmap.FromStream(stream);
				    this.Size = new Size2D(bmp.Width,bmp.Height); 
				}
			}
			catch(Exception ex)
			{
				string msg = "LayerVisualPart '" + Name + "' cannot be created from resource '" + resourceName + "'";
				Debug.WriteLine(msg);
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
				throw new Exception(msg, ex);
			}
		}

        internal Bitmap Bitmap 
		{
			get 
			{
                if (bmp == null)
                    throw new Exception("BitmapVisualPart '" + Name + "' doesn't have the bitmap defined");
				
				return bmp; 
			} 
			set 
			{
			    lock (this)
			    {
				    bmp = value; 
				    
				    if (value != null)
					    Size = new Size2D(bmp.Size.Width, bmp.Size.Height); 
			    }
			}
		}

		internal XmlElement BitmapNode { get { return bitmapNode; } set { bitmapNode = value; Bitmap = XMLUtils.StringToBmp(value.InnerText); } }

		internal void GetFromResourcePath(string resourcePath)
		{
			try
			{
				Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
				bmp = (Bitmap)Bitmap.FromStream(stream);
			}
			catch(Exception ex)
			{
				string msg = "LayerVisualPart '" + Name + "' cannot be created from resource '" + resourcePath + "'";
				Debug.WriteLine(msg);
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
				throw new Exception(msg, ex);
			}
		}

		internal static GDIBitmapVisualPart CreateFromResourcePath(string name,string resourcePath)
		{
			GDIBitmapVisualPart vp = new GDIBitmapVisualPart(name);
			vp.GetFromResource(resourcePath);
			return vp;
		}

		#region --- Map Areas ---

		internal override MapAreaCollection CreateMapAreas()
		{
	        lock (this) 
	        {		    
		        ObjectMapper om = new ObjectMapper(bmp,null);
		        om.CreateMappingAreas();
		        return om.MapAreas;
		    }
		}

		#endregion
	
		#region --- Rendering ---

		internal override void Render(RenderingContext renderingContext)				
		{
            lock (this)
            {
                Bitmap bmp = Bitmap;

                if (bmp == null)
                    return; 
			
                Image outImg = StretchImage(bmp, renderingContext.TargetArea.Size);

			    Rectangle2D localRect = new Rectangle2D(0,0,outImg.Width,outImg.Height);
			    RenderingContext context = renderingContext.SetAreaMapping(localRect,true);
			    Graphics g = context.RenderingTarget as Graphics;
			    if(g == null)
                    throw new ArgumentException("BitmapVisualPart '" + Name + "' cannot render on target type '" + 
					    renderingContext.RenderingTarget.GetType().Name + "'");

                Rectangle src = new Rectangle(0, 0, outImg.Width, outImg.Height);
                Rectangle dest = new Rectangle(0, outImg.Height, outImg.Width, -outImg.Height);
                g.DrawImage(outImg, dest, src, GraphicsUnit.Pixel);

                if (outImg != bmp)
				    outImg.Dispose();
            }
        }

		#region --- Needle rendering ---

		internal override void RenderAsNiddle(RenderingContext context)
		{
			Rectangle2D localRect = new Rectangle2D(0,0,Size.Width,Size.Height);
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
                throw new ArgumentException("BitmapVisualPart '" + Name + "' cannot render on target type '" + 
					context.RenderingTarget.GetType().Name + "'");
					
            lock (this)
            {
			    Bitmap bmp = Bitmap;
			    
                if (bmp == null)
                    throw new Exception("BitmapVisualPart '" + Name + "' doesn't have the bitmap defined");

                Rectangle src = new Rectangle(0, 0, bmp.Width, bmp.Height);
                Rectangle dest = new Rectangle(0, bmp.Height, bmp.Width, -bmp.Height);
                g.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);
            }
		}

		internal override void RenderAsNiddleRegion(RenderingContext context, Color backColor)
		{
			Rectangle2D localRect = new Rectangle2D(0,0,Size.Width,Size.Height);
			Graphics g = context.RenderingTarget as Graphics;
			
			if(g == null)
				throw new ArgumentException("BitmapVisualPart '" + Name + "' cannot render on target type '" + 
					context.RenderingTarget.GetType().Name + "'");
					
            lock (this)
            {
                Bitmap bmp = Bitmap;

                if (bmp == null)
				    throw new Exception("BitmapVisualPart '" + Name + "' doesn't have the bitmap defined");

			    ImageAttributes imageAttributes = GetImageAttributes(backColor);
                g.DrawImage(bmp, new Rectangle(0, bmp.Height, bmp.Width, -bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttributes);
            }
        }
		
		#endregion

		internal override void RenderAfterSettingColor(RenderingContext renderingContext, Color color)
		{
			ImageAttributes imageAttrs = RedMappingAttributes(color);
			Render(renderingContext,imageAttrs);
			imageAttrs.Dispose();
		}

		internal override void RenderAsRegion(RenderingContext renderingContext, Color backColor)
		{
			ImageAttributes imageAttrs = GetImageAttributes(backColor);
			Render(renderingContext,imageAttrs);
			imageAttrs.Dispose();
		}

		private void Render(RenderingContext renderingContext, ImageAttributes imageAttrs)
		{
			Image outImg = null;
			Rectangle2D localRect;
			
			lock (this) 
			{
    			Bitmap bmp = Bitmap;

                if (bmp == null) // we are rendering placeholder
			    {
                    localRect = new Rectangle2D(0, 0, renderingContext.TargetArea.Size.Width, renderingContext.TargetArea.Size.Height);
			    }
			    else
			    {
                    outImg = StretchImage(bmp, renderingContext.TargetArea.Size);
				    localRect = new Rectangle2D(0,0,outImg.Width,outImg.Height);
			    }

			    RenderingContext context = renderingContext.SetAreaMapping(localRect,true);
			    Graphics g = context.RenderingTarget as Graphics;
			    if(g == null)
				    throw new ArgumentException("BitmapVisualPart '" + Name + "' cannot render on target type '" + 
					    renderingContext.RenderingTarget.GetType().Name + "'");

                if (bmp == null)
			    {
				    renderingContext.Engine.DrawImage(null,new Size2D(100,100),new Point2D(0,0),renderingContext);
			    }
			    else
			    {
				    g.DrawImage(outImg,new Rectangle(0,outImg.Height,outImg.Width,-outImg.Height),0,0,outImg.Width,outImg.Height,GraphicsUnit.Pixel,imageAttrs);

                    if (outImg != bmp)
					    outImg.Dispose();
			    }
			}
		}

		private Image StretchImage(Image img, Size2D outSize) 
		{
			Bitmap bmpIn = img as Bitmap;
			if(bmpIn == null || CornerSize.Abs() == 0)
				return img;
			double fx = outSize.Width/img.Width;
			double fy = outSize.Height/img.Height;
			
			if(0.99 < fx/fy && fx/fy < 1.01)
				return img;

			// With corner > 0 split into 9 sections
			Size2D inCorner = this.CornerSize;
			Size2D otCorner = inCorner*Math.Min(fx,fy);
			int outWidth = (int) outSize.Width;
			int outHeight = (int) outSize.Height;

			int[] xins = new int[] { 0,(int)inCorner.Width,img.Width-(int)inCorner.Width,img.Width };
			int[] yins = new int[] { 0,(int)inCorner.Height,img.Height-(int)inCorner.Height,img.Height };

			int[] xots = new int[] { 0,(int)otCorner.Width,outWidth-(int)otCorner.Width,outWidth };
			int[] yots = new int[] { 0,(int)otCorner.Height,outHeight-(int)otCorner.Height,outHeight };

			Bitmap outImg = new Bitmap(outWidth,outHeight);
			Graphics g = Graphics.FromImage(outImg);
			for(int i=0; i<3; i++)
				for(int j=0; j<3; j++)
				{
					g.DrawImage(img,new Rectangle(xots[i],yots[j],xots[i+1]-xots[i],yots[j+1]-yots[j]),xins[i],yins[j],xins[i+1]-xins[i],yins[j+1]-yins[j],GraphicsUnit.Pixel);
                }
			g.Dispose();
			return outImg;
		}

		internal static ImageAttributes RedMappingAttributes(Color color)
		{
			// Create image attribute to set color to the region bitmap
			float R = (float)color.R / 255f;
			float G = (float)color.G / 255f;
			float B = (float)color.B / 255f;
			float A = (float)color.A / 255f;

			ColorMatrix colorMatrix = new ColorMatrix(
				new float[][] {
								  new float[] { R, G, B, A, 0 },
								  new float[] { 0, 1-G, 0, 0, 0 },
								  new float[] { 0, 0, 1-B, 0, 0 },
								  new float[] { 0, 0, 0, 0, 0 },
								  new float[] { 0, 0, 0, 0, 1 }
							  });
			ImageAttributes imageAttrs = new ImageAttributes();
			imageAttrs.SetColorMatrix(colorMatrix);
			return imageAttrs;
		}

		internal static ImageAttributes GetImageAttributes(Color color)
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
		
		
		#endregion
		
		#region --- XML Export/Import ---

        internal XmlElement GetXmlNode(XmlDocument doc)
        {
            XmlElement imageNode = doc.CreateElement("Bitmap");
			string str = XMLUtils.BmpToString(Bitmap);
			if(str != "" && str != null)
			{
				imageNode.InnerText = str;
				return imageNode;
			}
			else
				return null;
        }

        #endregion
    }
}
