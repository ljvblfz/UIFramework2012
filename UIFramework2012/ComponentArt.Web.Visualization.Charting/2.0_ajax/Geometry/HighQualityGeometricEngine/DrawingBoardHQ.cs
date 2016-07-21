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
	internal class DrawingBoardHQ : DrawingBoard, IDisposable
	{

		protected Bitmap	bmp;

		#region --- Construction and Initialization ---
		public DrawingBoardHQ() : base(){}
		public DrawingBoardHQ(Vector3D V0, Vector3D Vx, Vector3D Vy) : base (V0,Vx,Vy) { }

		#region --- Memory Management ---

		public override void Dispose()
		{
			if(bmp != null)
			{
				bmp.Dispose();
			}
			bmp = null;
			if(g != null)
				g.Dispose();
			g = null;
		}

		#endregion

		public override void Grow(double deltaWorld)
		{
			growth = deltaWorld-growth;
			Clear();
		}

		internal override void Clear()
		{
			if(bmp != null)
			{
				bmp.Dispose();
				g.Dispose();
				bmp = null;
				g = null;
				vx = vx - Vx1*(growth*2);
				vy = vy - Vy1*(growth*2);
				v0 = v0 + (Vx1+Vy1)*growth;

			}
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

			// Unit vectors in directions vx and vy

			Vx1 = vx.Unit();
			Vy1 = vy.Unit();

			vx = vx + Vx1*(growth*2);
			vy = vy + Vy1*(growth*2);
			vOffset = (Vx1+Vy1)*growth;
			v0 = v0 - vOffset;

			// Find projected coordinates range
			
			v1 = v0 + vx;
			v2 = v1 + vy;
			v3 = v0 + vy;
			Vector3D v0p,v1p,v2p,v3p;
			Mapping map = this.Mapping;
			map.Map(v0 - Vx1*growth - Vy1*growth,out v0p);
			map.Map(v1 + Vx1 *growth,out v1p);
			map.Map(v2 + Vy1 *growth,out v2p);
			map.Map(v3 + Vx1 *growth + Vy1*growth,out v3p);

			ix0 = (int)Math.Min(v0p.X,v1p.X); ix0 = (int)Math.Min(ix0,v2p.X); ix0 = (int)Math.Min(ix0,v3p.X); 
			iy0 = (int)Math.Min(v0p.Y,v1p.Y); iy0 = (int)Math.Min(iy0,v2p.Y); iy0 = (int)Math.Min(iy0,v3p.Y); 
			ix1 = (int)Math.Max(v0p.X,v1p.X); ix1 = (int)Math.Max(ix1,v2p.X); ix1 = (int)Math.Max(ix1,v3p.X); 
			iy1 = (int)Math.Max(v0p.Y,v1p.Y); iy1 = (int)Math.Max(iy1,v2p.Y); iy1 = (int)Math.Max(iy1,v3p.Y);
 
			ix1 +=2; iy1 +=2;

			// Create the graphics object
	
			bmp = new Bitmap(ix1-ix0+1,iy1-iy0+1,PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(bmp);
			
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
		}
		#endregion
	}
}
