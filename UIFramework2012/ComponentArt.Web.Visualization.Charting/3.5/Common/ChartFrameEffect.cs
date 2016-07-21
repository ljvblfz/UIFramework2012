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
	internal abstract class ChartFrameEffect
	{
		#region --- Obsolete ---
		/// <summary>
		/// Creates path from list of points, shifting and growing the original shape.
		/// </summary>
		/// <param name="pts"> List of points</param>
		/// <param name="nPts">Number of points</param>
		/// <param name="xShift">Shift amount along x -axis</param>
		/// <param name="yShift">Shift amount along y -axis</param>
		/// <param name="grow">Growing amount</param>
		/// <returns></returns>
		/// <remarks>All values in pixels in the original GDI+ coordinate system.</remarks>
		internal static GraphicsPath CreatePath(PointF[] pts, int nPts, float xShift, float yShift, float grow)
		{
			PointF[] p = new PointF[nPts];
			for(int i=0;i<nPts;i++)
			{
				int j1 = (i+nPts-1)%nPts;
				int j2 = (i+1)%nPts;
				double dx = pts[i].X - pts[j1].X;
				double dy = pts[i].Y - pts[j1].Y;
				double a = Math.Sqrt(dx*dx+dy*dy);
				double n1x = dy/a;
				double n1y = -dx/a;
				dx = pts[j2].X - pts[i].X;
				dy = pts[j2].Y - pts[i].Y;
				a = Math.Sqrt(dx*dx+dy*dy);
				double n2x = dy/a;
				double n2y = -dx/a;

				double nx = n1x+n2x;
				double ny = n1y+n2y;
				double f = grow/(nx*n1x + ny*n1y);

				p[i].X = (float)(pts[i].X + nx*f) + xShift;
				p[i].Y = (float)(pts[i].Y + ny*f) + yShift;
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(p);
			return path;			
		}
		#endregion

		#region --- Low Level Shading Primitives ---
		/// <summary>
		/// Creates open polygon for the right-bottom side of the rectangle.
		/// </summary>
		/// <param name="rect">Original rectangle</param>
		/// <param name="roundTop">Indicates if top corners are round</param>
		/// <param name="roundBottom">Indicates if bottom corners are round</param>
		/// <param name="cornerRadius">Corner radius</param>
		/// <param name="pts">Array of points. Has to be allocated by the caller and big enough to accept all points (22 at most) </param>
		/// <param name="nPts">Output parameter presenting number of points</param>
		/// <remarks>All measures are in pixels in GDI+ coordinate system.</remarks>
		public static void CreateRightBottomPolygon(Rectangle rect, bool roundTop, bool roundBottom, double cornerRadius, PointF[]	pts,out int nPts)
		{
			CreateRightBottomPolygon(rect,roundTop, roundBottom, cornerRadius, cornerRadius, pts, out nPts);
		}

		public static void CreateRightBottomPolygon(Rectangle rect, bool roundTop, bool roundBottom, double cornerRadius, double cornerRadiusEnd, PointF[]	pts,out int nPts)
		{
			double cx,cy,alpha,dAlpha = Math.PI/20; // 10 steps for 90 degrees

			nPts = 0;

			// Frame Top

			if(roundTop)
			{
				// top-right (xMax,0)
				cx = rect.X + rect.Width-cornerRadiusEnd-1;
				cy = rect.Y + cornerRadiusEnd;
				alpha = 1.75*Math.PI;
				for(int i=0;i<=5;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadiusEnd);
				}
			}
			else
			{
				pts[0].X = (float)(rect.X + rect.Width-1 - cornerRadiusEnd + cornerRadius);
				pts[0].Y = rect.Y;
				nPts = 1;
			}

			// Frame Bottom

			if(roundBottom)
			{
				// Bottom-Right (xMax,yMax)
				cx = rect.X + rect.Width - cornerRadiusEnd-1;
				cy = rect.Y + rect.Height- cornerRadiusEnd-1;
				alpha = 0;
				dAlpha = Math.PI/20.0;
				for(int i=0;i<=10;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
				// bottom-left (0,yMax)
				cx = rect.X + cornerRadiusEnd;
				cy = rect.Y + rect.Height-cornerRadiusEnd-1;
				alpha = 0.5*Math.PI;
				for(int i=0;i<=5;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadiusEnd);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
			}
			else
			{
				pts[nPts].X = (float)(rect.X + rect.Width-1 - cornerRadiusEnd + cornerRadius);
				pts[nPts].Y = (float)(rect.Y + rect.Height-1 - cornerRadiusEnd + cornerRadius);;
				nPts++;
				pts[nPts].X = rect.X;
				pts[nPts].Y = (float)(rect.Y + rect.Height-1 - cornerRadiusEnd + cornerRadius);;
				nPts++;
			}
		}
	

		public static void CreateTopLeftPolygon(Rectangle rect, bool roundTop, bool roundBottom, double cornerRadius, PointF[]	pts,out int nPts)
		{
			CreateTopLeftPolygon(rect,roundTop,roundBottom,cornerRadius,cornerRadius,pts,out nPts);
		}

		public static void CreateTopLeftPolygon(Rectangle rect, bool roundTop, bool roundBottom, double cornerRadius, double cornerRadiusEnd, PointF[]	pts,out int nPts)
		{
			double cx, cy, alpha,dAlpha = Math.PI/20; // 10 steps per 90 degrees

			nPts = 0;

			// Bottom-Left

			if(roundBottom)
			{
				cx = rect.X + cornerRadiusEnd;
				cy = rect.Y + rect.Height-cornerRadiusEnd-1;
				alpha = 0.75*Math.PI;
				for(int i=0;i<=5;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadiusEnd);
				}
			}
			else
			{
				pts[0].X = (float)(rect.X + cornerRadiusEnd - cornerRadius);
				pts[0].Y = (float)(rect.Y + rect.Height-1);
				nPts = 1;
			}

			// Frame Top

			if(roundTop)
			{
				// top-left (0,0)
				cx = rect.X + cornerRadiusEnd;
				cy = rect.Y + cornerRadiusEnd;
				alpha = Math.PI;
				for(int i=0;i<=10;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
				// top-right (xMax,0)
				cx = rect.X + rect.Width-cornerRadiusEnd-1;
				cy = rect.Y + cornerRadiusEnd;
				alpha = 1.5*Math.PI;
				for(int i=0;i<=5;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadiusEnd);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
			}
			else
			{
				pts[nPts].X = (float)(rect.X + cornerRadiusEnd - cornerRadius);
				pts[nPts].Y = (float)(rect.Y + cornerRadiusEnd - cornerRadius);
				nPts++;
				pts[nPts].X = rect.X + rect.Width-1;
				pts[nPts].Y = (float)(rect.Y + cornerRadiusEnd - cornerRadius);
				nPts++;
			}
		}


		public static GraphicsPath CreatePathFromSlidingOpenLine(PointF[] pts, int nPts, float xShift0, float yShift0, float xShift1, float yShift1)
		{
			PointF [] P = new PointF[2*nPts];
			int j = 2*nPts-1;
			for (int i=0;i<nPts;i++,j--)
			{
				P[i] = new PointF(pts[i].X + xShift0,pts[i].Y + yShift0);
				P[j] = new PointF(pts[i].X + xShift1,pts[i].Y + yShift1);
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(P);
			return path;
		}


		public static void CreateClosedPolygon(Rectangle rect, bool roundTop, bool roundBottom, double cornerRadius, PointF[]	pts,out int nPts)
		{
			nPts = 0;

			// Frame Top

			if(roundTop)
			{
				// top-left (0,0)
				double cx = rect.X + cornerRadius;
				double cy = rect.Y + cornerRadius;
				double alpha = Math.PI;
				double dAlpha = Math.PI/20.0;
				for(int i=0;i<=10;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
				// top-right (xMax,0)
				cx = rect.X + rect.Width-cornerRadius-1;
				cy = rect.Y + cornerRadius;
				alpha = 1.5*Math.PI;
				for(int i=0;i<=10;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
			}
			else
			{
				pts[0].X = rect.X;
				pts[0].Y = rect.Y;
				pts[1].X = rect.X + rect.Width-1;
				pts[1].Y = rect.Y;
				nPts = 2;
			}

			// Frame Bottom

			if(roundBottom)
			{
				// Bottom-Right (xMax,yMax)
				double cx = rect.X + rect.Width - cornerRadius-1;
				double cy = rect.Y + rect.Height- cornerRadius-1;
				double alpha = 0;
				double dAlpha = Math.PI/20.0;
				for(int i=0;i<=10;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
				// bottom-left (0,yMax)
				cx = rect.X + cornerRadius;
				cy = rect.Y + rect.Height-cornerRadius-1;
				alpha = 0.5*Math.PI;
				for(int i=0;i<=10;i++,alpha += dAlpha,nPts++)
				{
					double c = Math.Cos(alpha);
					double s = Math.Sin(alpha);
					pts[nPts].X = (float)(cx + c*cornerRadius);
					pts[nPts].Y = (float)(cy + s*cornerRadius);
				}
			}
			else
			{
				pts[nPts].X = rect.X + rect.Width-1;
				pts[nPts].Y = rect.Y + rect.Height-1;
				nPts++;
				pts[nPts].X = rect.X;
				pts[nPts].Y = rect.Y + rect.Height-1;
				nPts++;
			}
		}


		#endregion

		#region --- High Level Shading Primitives ---
		internal static GraphicsPath CreateFrameArea(Rectangle rect0, Rectangle rect1, double t0, double t1, bool roundTop, bool roundBottom, double cornerRadius)
		{
			PointF[] pts0 = new PointF[50];
			PointF[] pts1 = new PointF[50];
			int nPts0,nPts1;
			CreateClosedPolygon(rect0,roundTop,roundBottom,cornerRadius,pts0,out nPts0);
			CreateClosedPolygon(rect1,roundTop,roundBottom,cornerRadius,pts1,out nPts1);

			PointF[] pts0x = new PointF[nPts0];
			PointF[] pts1x = new PointF[nPts0];
			for(int i=0;i<nPts0;i++)
			{
				pts0x[i].X = (float)(t0*pts1[i].X + (1.0-t0)*pts0[i].X);
				pts0x[i].Y = (float)(t0*pts1[i].Y + (1.0-t0)*pts0[i].Y);
				pts1x[i].X = (float)(t1*pts1[i].X + (1.0-t1)*pts0[i].X);
				pts1x[i].Y = (float)(t1*pts1[i].Y + (1.0-t1)*pts0[i].Y);
			}

			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(pts0x);
			path.AddPolygon(pts1x);

			return path;
		}

		internal static GraphicsPath CreateRightBottomArea(Rectangle rect, double width0, double width1, bool roundTop, bool roundBottom, double cornerRadius)
		{
			PointF[] pts = new PointF[50];
			int nPts;
			CreateRightBottomPolygon(rect,roundTop,roundBottom,cornerRadius,pts,out nPts);
			return CreatePathFromSlidingOpenLine(pts,nPts,(float)width0,(float)width0,(float)width1,(float)width1);
		}
		
		internal static GraphicsPath CreateRightBottomInnerArea(Rectangle rect, double width0, double width1, bool roundTop, bool roundBottom, double cornerRadius)
		{
			PointF[] pts0 = new PointF[50];
			PointF[] pts1 = new PointF[50];
			int nPts0,nPts1;
			CreateRightBottomPolygon(rect,roundTop,roundBottom,cornerRadius-width0,cornerRadius,pts0,out nPts0);
			CreateRightBottomPolygon(rect,roundTop,roundBottom,cornerRadius-width1,cornerRadius,pts1,out nPts1);
			
			PointF[] pts = new PointF[nPts0*2];
			int j = nPts0*2-1;
			for(int i=0;i<nPts0;i++,j--)
			{
				pts[i] = pts0[i];
				pts[j] = pts1[i];
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(pts);
			return path;
		}
		
		internal static GraphicsPath CreateTopLeftInnerArea(Rectangle rect, double width0, double width1, bool roundTop, bool roundBottom, double cornerRadius)
		{
			PointF[] pts0 = new PointF[50];
			PointF[] pts1 = new PointF[50];
			int nPts0,nPts1;
			CreateTopLeftPolygon(rect,roundTop,roundBottom,cornerRadius-width0,cornerRadius,pts0,out nPts0);
			CreateTopLeftPolygon(rect,roundTop,roundBottom,cornerRadius-width1,cornerRadius,pts1,out nPts1);
			
			PointF[] pts = new PointF[nPts0*2];
			int j = nPts0*2-1;
			for(int i=0;i<nPts0;i++,j--)
			{
				pts[i] = pts0[i];
				pts[j] = pts1[i];
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(pts);
			return path;
		}

		internal static GraphicsPath CreateTopLeftArea(Rectangle rect, double width0, double width1, bool roundTop, bool roundBottom, double cornerRadius)
		{
			PointF[] pts = new PointF[50];
			int nPts;
			CreateTopLeftPolygon(rect,roundTop,roundBottom,cornerRadius,pts,out nPts);
			return CreatePathFromSlidingOpenLine(pts,nPts,(float)width0,(float)width0,(float)width1,(float)width1);
		}
		
		#endregion

		abstract internal void Render(Graphics g);
	}
}
