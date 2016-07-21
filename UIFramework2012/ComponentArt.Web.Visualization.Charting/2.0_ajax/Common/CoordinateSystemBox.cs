using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	// =======================================================================================

	internal class TargetAreaBox : GeometricObject
	{
        internal TargetAreaBox() { }

		internal override void Add(GeometricObject gpObject)
		{
			if (gpObject is TargetAreaBox || gpObject is ChartBox)
				base.Add(gpObject);
			else
				throw new Exception ("Cannot add a(n) '" + gpObject.GetType().Name + 
					"' to a(n) '" + GetType().Name + "'");
		}

		internal void AdjustMapping()
		{
			TargetArea ta = Tag as TargetArea;
			if(ta == null)
				throw new Exception("Implementation: TargetBox doesn't have valid TargetArea tag");
			ta.AdjustMapping(this);
		}

	}

	// =======================================================================================

	internal abstract class ChartBox : GeometricObject
	{
		// Central box point
		internal abstract Vector3D CenterICS();
		// Ordering based on projection of the central point
		internal override double OrderingZ()
		{
			CoordinateSystemBox csb = Owning(typeof(CoordinateSystemBox)) as CoordinateSystemBox;
			if(csb == null) // Possible in multiarea composition
				return 0;
			CoordinateSystem cs = csb.Tag as CoordinateSystem;
			Vector3D cWCS = cs.ICS2WCS(CenterICS());
			TargetAreaBox tab = Owning(typeof(TargetAreaBox)) as TargetAreaBox;
			return tab.Mapping.Map(cWCS).Z;
		}
		public override string ToString()
		{
			return this.GetType().Name + " C=" +  CenterICS().ToString() + 
				((Tag==null)? "":", tag="+Tag.ToString());
		}
	}

	// =======================================================================================

	internal class PieDoughnutBox : ChartBox
	{
		GeometricObject bases;
		GeometricObject	innerWalls;
		GeometricObject outerWalls;
		GeometricObject tops;

		internal PieDoughnutBox()
		{
			bases = new GeometricObject();
			innerWalls = new GeometricObject();
			outerWalls = new GeometricObject();
			tops = new GeometricObject();

			Add(bases);
			Add(innerWalls);
			Add(outerWalls);
			Add(tops);
		}

		internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			TargetCoordinateRange tcr = base.CoordinateRange(usingTexts);
			return tcr;
		}

		internal override void Add(GeometricObject gpObject)
		{
			base.Add(gpObject);
		}
	
		internal void AddBase(GeometricObject obj)
		{
			bases.Add(obj);
		}
	
		internal void AddInnerWall(GeometricObject obj)
		{
			innerWalls.Add(obj);
		}
	
		internal void AddOuterWall(GeometricObject obj)
		{
			outerWalls.Add(obj);
		}
	
		internal void AddTop(GeometricObject obj)
		{
			tops.Add(obj);
		}

		internal override Vector3D CenterICS()
		{
			// This is not used...
			return Vector3D.Null;
		}
	}

	// =======================================================================================

	internal class RadarBox : ChartBox
	{
        internal RadarBox() : base() { }
		internal override Vector3D CenterICS()
		{
			// This is not used...
			return Vector3D.Null;
		}
        internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
        {
			CoordinatePlane cPlane = Tag as CoordinatePlane;
			if(cPlane == null || !cPlane.IsRadial)
				return base.CoordinateRange(usingTexts);

			// It's enough to take 4 points along x-range, for y=max

			TargetCoordinateRange tcr = new TargetCoordinateRange();
			double xMax = cPlane.XAxis.MaxValueLCS;
			double yMax = cPlane.YAxis.MaxValueLCS;

			Vector3D ptC = cPlane.LogicalToWorld(0,0);
			Vector3D ptTargetC = Mapping.Map(ptC);
			Vector3D pt = cPlane.LogicalToWorld(xMax,yMax);
			Vector3D ptTarget = Mapping.Map(pt);
			double a = (pt-ptC).Abs;
			tcr.Include(ptTargetC + a*(new Vector3D(1,0,0)));
			tcr.Include(ptTargetC + a*(new Vector3D(0,1,0)));
			tcr.Include(ptTargetC + a*(new Vector3D(-1,0,0)));
			tcr.Include(ptTargetC + a*(new Vector3D(0,-1,0)));

			if(!usingTexts)
				return tcr;
			else
				return base.CoordinateRange(usingTexts);
        }
	}

	// =======================================================================================

	internal class CoordinateSystemBox : ChartBox
	{
		private GeometricObject coordinatePlanes;
		private CoordinateSystemInterior interior;
		private GeometricObject subSystemBoxes;

		public CoordinateSystemBox()
		{
			coordinatePlanes = new GeometricObject();
			interior = new CoordinateSystemInterior();
			subSystemBoxes = new GeometricObject();
			base.Add(coordinatePlanes);
			base.Add(interior);
		}

		internal override void Add(GeometricObject gpObject)
		{
			if(gpObject is CoordinatePlaneBox)
			{
				coordinatePlanes.Add(gpObject);
				base.Add(gpObject);
			}
			else if (gpObject is CoordinateSystemBox)
			{
				subSystemBoxes.Add(gpObject);
				base.Add(gpObject);
			}
			else
				interior.Add(gpObject);
		}

		internal void RearrangeInternalCoordinateSystemBoxes()
		{
			// Process all interior section boxes that have coordinate system boxes at top
			if(interior.SubObjects == null)
				return;
			
			bool tryRearranging = true;

			int i0 = 0;
			while(tryRearranging)
			{
				tryRearranging = false;
				CoordinateSystemBox csb = null;
				SectionBox sb = null;
				for(int i=i0; i<interior.SubObjects.Count; i++)
				{
					sb = interior.SubObjects[i] as SectionBox;
					if(sb == null || sb.SubObjects == null)
						continue;
					CompositeSeries cs = sb.Tag as CompositeSeries;
					if(cs != null && cs.CompositionKind == CompositionKind.MultiSystem)
						continue;
					for(int j=0; j<sb.SubObjects.Count; j++)
					{
						csb = sb.SubObjects[j] as CoordinateSystemBox;
						if(csb != null)
						{
							i0 = i;
							break;
						}
					}
				}
				if(csb != null)
				{
					csb.RearrangeInternalCoordinateSystemBoxes();
					sb.SubObjects.Remove(csb);
					tryRearranging = true;
					// Merge this coordinate system
					// -- Coordinate planes
					if(CoordinatePlanes.SubObjects != null)
					{
						for(int j=0; j<CoordinatePlanes.SubObjects.Count; j++)
						{
							(CoordinatePlanes.SubObjects[j] as GeometricObject)
								.Merge(csb.CoordinatePlanes.SubObjects[j] as GeometricObject);
						}
					}
					// -- Interior
					sb.Merge(csb.Interior);
				}
			}
		}

		internal GeometricObject CoordinatePlanes { get { return coordinatePlanes; } }
		internal GeometricObject SubSystems { get { return subSystemBoxes; } }
		internal CoordinateSystemInterior Interior { get { return interior; } }
		internal override Vector3D CenterICS()
		{
			CoordinateSystem cs = Tag as CoordinateSystem;
			if(cs == null)
				return Vector3D.Null;
			double xMin = cs.XAxis.MinValueICS;
			double xMax = cs.XAxis.MaxValueICS;
			double yMin = cs.YAxis.MinValueICS;
			double yMax = cs.YAxis.MaxValueICS;
			double zMin = cs.ZAxis.MinValueICS;
			double zMax = cs.ZAxis.MaxValueICS;
			return new Vector3D((xMin+xMax)/2,(yMin+yMax)/2,(zMin+zMax)/2);
		}

        internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
        {
            // The range is computed from the coordinate system corners, except the (1,1,1) corner (because it is not visible)
            //    regardles coordinate planes visibility.
            // Computing might be improved by providing range computation of the data points, so that base method doesn't return empty range.

            TargetCoordinateRange tcr = base.CoordinateRange(usingTexts);
            CoordinateSystem cs = Tag as CoordinateSystem;
            Mapping map = this.Mapping;
            double xMin = cs.XAxis.MinValueICS;
            double xMax = cs.XAxis.MaxValueICS;
            double yMin = cs.YAxis.MinValueICS;
            double yMax = cs.YAxis.MaxValueICS;
            double zMin = cs.ZAxis.MinValueICS;
            double zMax = cs.ZAxis.MaxValueICS;
            for(int ix = 0; ix<=1; ix++)
                for(int iy = 0; iy <=1; iy++)
                    for (int iz = 0; iz <= 1; iz++)
                    {
                        if (ix == 1 && iy == 1 && iz == 1)
                            continue;
                        Vector3D pWCS = cs.ICS2WCS(new Vector3D(ix * xMax + (ix - 1) * xMin, iy * yMax + (iy - 1) * yMin, iz * zMax + (iz - 1) * zMin));
                        tcr.Include(map.Map(pWCS));
                    }
            return tcr;
        }
    }

	// =======================================================================================

	internal class CoordinateSystemInterior : ChartBox
	{
		internal override Vector3D CenterICS()
		{
			CoordinateSystemBox csb = Owning(typeof(CoordinateSystemBox)) as CoordinateSystemBox;
			return csb.CenterICS();
		}
	}

	// =======================================================================================

	internal class CoordinatePlaneBox : ChartBox
	{
		internal override Vector3D CenterICS()
		{
			CoordinatePlane plane = Tag as CoordinatePlane;
			CoordinateSystem cs = plane.CoordinateSystem;
			double xMin = cs.XAxis.MinValueICS;
			double xMax = cs.XAxis.MaxValueICS;
			double yMin = cs.YAxis.MinValueICS;
			double yMax = cs.YAxis.MaxValueICS;
			double zMin = cs.ZAxis.MinValueICS;
			double zMax = cs.ZAxis.MaxValueICS;
			if(plane == cs.PlaneXY)
				return new Vector3D((xMin+xMax)/2, (yMin+yMax)/2,zMin);
			else if(plane == cs.PlaneYZ)
				return new Vector3D(xMin, (yMin+yMax)/2,(zMin+zMax)/2);
			else //(plane == cs.PlaneZX)
				return new Vector3D((xMin+xMax)/2, yMin,(zMin+zMax)/2);
		}

		internal bool FrontSideVisible()
		{
			CoordinatePlane plane = Tag as CoordinatePlane;
			if(plane == null)
				return false;
			CoordinateSystem CS = plane.CoordinateSystem;

			Vector3D S = Mapping.Map(new Vector3D(0,0,0));
			Vector3D Px = Mapping.Map(plane.XAxis.UnitVector) - S;
			Vector3D Py = Mapping.Map(plane.YAxis.UnitVector) - S;
			S.Z = 0;
			Px.Z = 0;
			Py.Z = 0;
			Vector3D cp = Px.CrossProduct(Py);
			return CS.IsPositive? cp.Z > 0 : cp.Z < 0;
		}

	}

	// =======================================================================================

	internal class SectionBox : ChartBox
	{
        internal SectionBox() { }

		internal override Vector3D CenterICS()
		{
			SeriesBase series = Tag as SeriesBase;

			double xMin = series.CoordSystem.XAxis.MinValueICS;
			double xMax = series.CoordSystem.XAxis.MaxValueICS;
			double yMin = series.CoordSystem.YAxis.MinValueICS;
			double yMax = series.CoordSystem.YAxis.MaxValueICS;
			double zCtr = series.CoordSystem.ZAxis.Dimension.Coordinate(series.Name);
			double wCtr = series.CoordSystem.ZAxis.Dimension.Width(series.Name);
			zCtr = series.CoordSystem.ZAxis.LCS2ICS(zCtr+wCtr*0.5);
			return new Vector3D((xMin+xMax)/2,(yMin+yMax)/2,zCtr);
		}

		internal Vector3D ProjectToFrontFace(Vector3D pointICS)
		{
			// Projects a point to the front (towards the viewer) face of the section
			SeriesBase series = Tag as SeriesBase;

			if(series.Style.ChartKindCategory == ChartKindCategory.PieDoughnut || 
				series.Style.ChartKindCategory == ChartKindCategory.Radar)
				return pointICS;

			return pointICS;
		}
	}

	// =======================================================================================

	// Column box may span multiple consecutive x-coordinates in case of area or line segment
	internal class ColumnBox : ChartBox
	{
		internal override Vector3D CenterICS()
		{
			return (Vector3D)Tag ;
		}
	}

	// =======================================================================================

	internal class SubColumnBox : ChartBox
	{
        internal SubColumnBox() :base() { }

		internal override Vector3D CenterICS()
		{
			DataPoint dp = Tag as DataPoint;
			double x0i,x1i, y0i,y1i, z0i,z1i;
			bool invertedY;
			dp.ComputeBoundingBox(out x0i, out x1i, out y0i, out y1i, out z0i, out z1i, out invertedY);

			return new Vector3D((x0i+x1i)/2,(y0i+y1i)/2,(z0i+z1i)/2);
		}
	}

}
