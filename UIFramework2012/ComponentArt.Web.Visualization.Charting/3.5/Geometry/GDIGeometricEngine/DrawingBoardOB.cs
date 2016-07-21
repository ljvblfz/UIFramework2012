using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for DrawingPanel.
	/// </summary>
	internal class DrawingBoardOB : DrawingBoard, IDisposable
	{

		protected Bitmap	bmp;
		private Graphics gs;

		#region --- Construction and Initialization ---
		public DrawingBoardOB() : base(){}
		public DrawingBoardOB(Vector3D V0, Vector3D Vx, Vector3D Vy) : base (V0,Vx,Vy) { }

		#region --- Memory Management ---

		public void Dispose()
		{
			if(bmp != null)
			{
#if DEBUG
				ChartBase.objTracker.Deleted(bmp);
#endif
				bmp.Dispose();
			}
			bmp = null;
			if(g != null)
				g.Dispose();
			g = null;
#if DEBUG
			disposed++;
			ChartBase.objTracker.Deleted(this);
#endif
		}

		#endregion

		public override void Grow(double deltaWorld)
		{
			growth = 0;
			Clear();
		}


		internal override void Clear()
		{
		}

		internal void SetGraphics(Graphics g)
		{
			this.gs = g;
		}

		internal Bitmap BMP { get { return bmp; } }

		internal override void PrepareToRenderContents()
		{
			/*
			 *             v3
			 *            /  \
			 *           /vy  \
			 *          /      \
			 *        v0        v2
			 *          \      /
			 *           \vx  /
			 *            \  /
			 *             v1
			 * 
			 */
			g = gs;

			// Unit vectors in directions vx and vy

			Vx1 = vx.Unit();
			Vy1 = vy.Unit();

			// Find projected coordinates range
			
			v1 = v0 + vx;
			v2 = v1 + vy;
			v3 = v0 + vy;
			Vector3D v0p,v1p,v2p,v3p;
			Mapping map = this.Mapping;
			map.Map(v0 ,out v0p);
			map.Map(v1 ,out v1p);
			map.Map(v2 ,out v2p);
			map.Map(v3 ,out v3p);

			ix0 = (int)Math.Min(v0p.X,v1p.X); ix0 = (int)Math.Min(ix0,v2p.X); ix0 = (int)Math.Min(ix0,v3p.X); 
			iy0 = (int)Math.Min(v0p.Y,v1p.Y); iy0 = (int)Math.Min(iy0,v2p.Y); iy0 = (int)Math.Min(iy0,v3p.Y); 
			ix1 = (int)Math.Max(v0p.X,v1p.X); ix1 = (int)Math.Max(ix1,v2p.X); ix1 = (int)Math.Max(ix1,v3p.X); 
			iy1 = (int)Math.Max(v0p.Y,v1p.Y); iy1 = (int)Math.Max(iy1,v2p.Y); iy1 = (int)Math.Max(iy1,v3p.Y);
			ix1 -= ix0;
			iy1 -= iy0;
			ix0 = 0;
			iy0 = 0;
		}
		#endregion
	
		internal override double OrderingZ()
		{
			return Mapping.Map(V0+(Vx+Vy)*0.5).Z+liftZ;
		}

		internal override void ProcessObjectTrackingArea(GraphicsPath bmpPath)
		{
			GeometricEngineBasedOnRenderingOrder gebro = GE as GeometricEngineBasedOnRenderingOrder;
			ObjectTrackingData otd = gebro.ObjectTrackingData;
			if(otd == null)
				return;
			if(otd.ActiveObjectIndex > 0)
				otd.ProcessRegion(bmpPath);
		}

	}
}
