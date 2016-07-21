using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Box.
	/// </summary>
	internal class Box : ChartBox
	{
		// WCS corners (NOT ICS!)
		Vector3D p0;
		Vector3D p1;
		ChartColor surface;

        // for cloning
        internal Box()
        {
        }

		internal Box(Vector3D P0, Vector3D P1, ChartColor surface)
		{
			this.p0 = P0;
			this.p1 = P1;
			this.surface = surface;
		}

        internal Vector3D P0 { get { return p0; } }
        internal Vector3D P1 { get { return p1; } }
        internal ChartColor Color { get { return surface; } }

		// Note: We cannot compute CenterICS since p0 and p1 are in WCS!
		// We just override "OrderingZ()" instead...

		internal override double OrderingZ()
		{
			return Mapping.Map((p0+p1)/2).Z;
		}

		internal override Vector3D CenterICS()
		{
			// This sould never be called, because OrderingZ() is implemented!
			return (p0+p1)/2;
		}

		internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			TargetCoordinateRange tcr = new TargetCoordinateRange();
			IncludeWCSBox(p0,p1,tcr);
			return tcr;
		}

    }
}
