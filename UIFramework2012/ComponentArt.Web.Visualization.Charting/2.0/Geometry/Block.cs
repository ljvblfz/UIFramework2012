using System;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Block.
	/// </summary>
	// ---------------------------------------------------------------------------------------

	internal class Block : Prism
	{
        public Block() { }

		public Block(Vector3D P, Vector3D Height, Vector3D Side1, 
			double side2, double edgeSmoothingRadius, ChartColor surface) 
			: base(P + Side1*0.5 + Height.CrossProduct(Side1).Unit()*(side2*0.5),
			P + Side1*0.5 + Height.CrossProduct(Side1).Unit()*(side2*0.5)+Height,
			Side1,side2,4,edgeSmoothingRadius,surface) { }
	}
}
