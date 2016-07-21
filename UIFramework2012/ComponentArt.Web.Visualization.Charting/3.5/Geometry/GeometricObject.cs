using System;
using System.Collections;
using System.Text;
using System.Diagnostics;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Class containing minimum and maximum values of a GeometricObject in target coordinate 
	/// system.
	/// </summary>
	internal class TargetCoordinateRange
	{
		private double x0 = double.MaxValue;
		private double x1 = double.MinValue;
		private double y0 = double.MaxValue;
		private double y1 = double.MinValue;
		private double z0 = double.MaxValue;
		private double z1 = double.MinValue;

		internal bool IsEmpty
		{
			get
			{
				return x0 > x1 || y0 > y1 || z0 > z1;
			}
		}

		internal TargetCoordinateRange Max(TargetCoordinateRange r)
		{
			TargetCoordinateRange rcr = new TargetCoordinateRange();
			rcr.x0 = Math.Min(this.x0,r.x0);
			rcr.x1 = Math.Max(this.x1,r.x1);
			rcr.y0 = Math.Min(this.y0,r.y0);
			rcr.y1 = Math.Max(this.y1,r.y1);
			rcr.z0 = Math.Min(this.z0,r.z0);
			rcr.z1 = Math.Max(this.z1,r.z1);
			return rcr;
		}

		/// <summary>
		/// Expand the range to include point
		/// </summary>
		/// <param name="point">Point to be included</param>
 
		internal void Include(Vector3D point)
		{
			x0 = Math.Min(x0, point.X);
			x1 = Math.Max(x1, point.X);
			y0 = Math.Min(y0, point.Y);
			y1 = Math.Max(y1, point.Y);
			z0 = Math.Min(z0, point.Z);
			z1 = Math.Max(z1, point.Z);
		}

		internal double X0 { get { return x0; } }
		internal double X1 { get { return x1; } }
		internal double Y0 { get { return y0; } }
		internal double Y1 { get { return y1; } }
		internal double Z0 { get { return z0; } }
		internal double Z1 { get { return z1; } }

		public override string ToString()
		{
			if(x0 > x1 || y0> y1 || z0 > z1)
				return "TargetCoordinateRange = Empty";
			else
				return
				"X: " + x0.ToString("000.0") + " - " + x1.ToString("000.0") + "  " + 
				"Y: " + y0.ToString("000.0") + " - " + y1.ToString("000.0") + "  " + 
				"Z: " + z0.ToString("000.0") + " - " + z1.ToString("000.0");
		}
	}

		/// <summary>
    /// Base class for objects rendered by geometric engine. 
    /// Geometric object may have subobjects of the same type and it therefore implements hierarchical 
    /// structure of geometric objects.
    /// </summary>
    internal class GeometricObject
    {
        private ArrayList subObjects = null;
        private object tag;
		private GeometricObject parent = null;

		public virtual void Dispose()
		{
			if(subObjects == null)
				return;
			for(int i=0; i<subObjects.Count; i++)
				(subObjects[i] as GeometricObject).Dispose();
			subObjects = null;
		}

		#region --- Merging ---

		internal virtual void Merge(GeometricObject obj)
		{
			if(obj.SubObjects == null)
				return;
			SubObjects.AddRange(obj.SubObjects);
			obj.SubObjects.Clear();
		}

		#endregion

		#region --- Object Hierarchy ---

        internal ArrayList SubObjects { get { return subObjects; } }
        internal virtual void Add(GeometricObject gpObject)
        {
            if (subObjects == null)
                subObjects = new ArrayList();
            subObjects.Add(gpObject);
			gpObject.parent = this;
        }

        /// <summary>
        /// Clears the sub-objects collection
        /// </summary>
        internal void Clear()
        {
            subObjects = null;
        }
        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="i"> index.</param>
        /// <returns>Returns geometric object at the given index if the collection is not null and index in valid renge, 
        /// otherwise returns null.</returns>
        internal GeometricObject this[int i]
        {
            get
            {
                if (subObjects == null || i < 0 || i >= subObjects.Count)
                    return null;
                return subObjects[i] as GeometricObject;
            }
        }

		/// <summary>
		/// Navigation: getting the parent object
		/// </summary>
		internal GeometricObject Parent { get { return parent; } set { parent = value; } }

		internal ChartBase OwningChart
		{
			get
			{
				TargetArea ta = TargetArea;
				if(ta != null)
					return ta.OwningChart;
				else
					return null;
			}
		}

		internal GeometricObject Owning(Type type)
		{
			GeometricObject obj = this;
			while(obj != null && obj.GetType() != type)
				obj = obj.Parent;
			return obj;
		}

		internal CoordinateSystem CoordinateSystem()
		{
			return (Owning(typeof(CoordinateSystemBox)) as CoordinateSystemBox).Tag as CoordinateSystem;
		}

		internal TargetArea TargetArea
		{
			get
			{
				TargetAreaBox tab = Owning(typeof(TargetAreaBox)) as TargetAreaBox;
				if(tab == null)
				{
					if(SubObjects != null && SubObjects.Count > 0 && (SubObjects[0] is TargetAreaBox))
					{// This is the case when root object is queried for mapping
						return (SubObjects[0] as GeometricObject).Tag as TargetArea;
					}
					return null;
				}
				else
					return tab.Tag as TargetArea;
			}
		}

		internal Mapping Mapping
		{
			get
			{
				TargetArea ta = TargetArea;
				if(ta == null)
				{
					GeometricObject obj = this;
					Debug.WriteLine("Unable to find mapping at a " + GetType().Name );
					while(obj.Parent != null)
					{
						obj = obj.Parent;
						Debug.Write(" <<< " + obj.ToString());
					}
					Debug.WriteLine(" ");
					obj.Dump(0);
					return null;
				}
				else
					return ta.Mapping;
			}
		}

		internal void Dump(int depth)
		{
			for(int i=0;i<depth;i++)
				Debug.Write("  ");
			Debug.WriteLine(ToString());
			if(subObjects != null)
			{
				for(int i=0;i<subObjects.Count; i++)
				{
					(subObjects[i] as GeometricObject).Dump(depth+1);
				}
			}
		}

		public override string ToString()
		{
			return GetType().Name + ((Tag==null)? "":", tag='"+Tag.GetType() + "'") + " " + CoordinateRange(true).ToString();
		}
		#endregion

		#region --- Coordinate Range ---
		/// <summary>
		/// Z coordinate in WCS to be used for object ordering.
		/// </summary>
		/// <returns>WCS Z coordinate. Default implementation uses the middle value of z coordinate
		/// in the onjects' <see cref="CoordinateRange"/>.</returns>
		internal virtual double OrderingZ()
		{
			double z = 0;
			if(SubObjects == null)
				return 0;

			int k = SubObjects.Count;
			for(int i=0; i<k; i++)
			{
				z += (SubObjects[i] as GeometricObject).OrderingZ();
			}
			if(k > 0)
				return z/k;
			else
				return 0;
		}

		/// <summary>
		/// Default implementation of coordinate range, based on subobjects.
		/// </summary>
		/// <returns>Target coordinate range of this object</returns>
		internal virtual TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			TargetCoordinateRange tcr = new TargetCoordinateRange();
			if(subObjects != null)
			{
				for(int i=0; i<subObjects.Count; i++)
				{
					tcr = tcr.Max(this[i].CoordinateRange(usingTexts));
				}
			}
			return tcr;
		}

		/// <summary>
		/// Expand coordinate range to include given point.
		/// </summary>
		/// <param name="pointWCS">The point to be included in WCS</param>
		/// <param name="range">Range to be expanded</param>
		internal void IncludeWCSPoint(Vector3D pointWCS, TargetCoordinateRange range)
		{
			range.Include(Mapping.Map(pointWCS));
		}

		/// <summary>
		/// Expand coordinate range to include box in WCS defined by two diagonal points
		/// </summary>
		/// <param name="point1WCS">The first diagonal point</param>
		/// <param name="point2WCS">the second diagonal point</param>
		/// <param name="range">The range object to be expanded</param>
		internal void IncludeWCSBox(Vector3D point1WCS, Vector3D point2WCS, TargetCoordinateRange range)
		{
			Vector3D Vd = point2WCS - point1WCS;
			Vector3D Vx = new Vector3D(Vd.X,0,0);
			Vector3D Vy = new Vector3D(0,Vd.Y,0);
			Vector3D Vz = new Vector3D(0,0,Vd.Z);
			range.Include(Mapping.Map(point1WCS));
			for(int ix = 0; ix<2; ix++)
			{
				for(int iy = 0; iy<2; iy++)
				{
					for(int iz = 0; iz<2; iz++)
						range.Include(Mapping.Map(point1WCS + Vx*ix + Vy*iy + Vz*iz));
				}
			}
		}

		#endregion

      	#region --- Sizing ---

        public virtual int NumberOfApproximationPointsForEllipse(double axis1W, double axis2W, double renderingPrecisionInPixelSize)
        {
            double a = axis1W * Mapping.Enlargement;
            double b = axis2W * Mapping.Enlargement;

            return (int)Math.Max(2, 2 * Math.Sqrt((a + b) / renderingPrecisionInPixelSize) + 1);
        }

        internal double FromPointToWorld { get { return Mapping.FromPointToWorld; } }
		#endregion

		internal object Tag { get { return tag; } set { tag = value; } }
    }

}
