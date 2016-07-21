using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Specifies the composition kind of the <see cref="CompositeSeries"/>.
	/// </summary>
	public enum CompositionKind
	{
		/// <summary>
		/// Values of different series are drawn side by sude, in the same chart section.
		/// </summary>
		Merged,
		/// <summary>
		/// Series are stacked on top of each other.
		/// </summary>
		Stacked,
		/// <summary>
		/// Series are stacked and values are stretched to 100%.
		/// </summary>
		Stacked100,
		/// <summary>
		/// Values of different series are drawn in the different series space.
		/// </summary>
		Sections,
		/// <summary>
		/// Series are positioned in circle around the main axis.
		/// </summary>
		Concentric,
		/// <summary>
		/// Values of different series are drawn in independent coordinate systems. 
		/// Applicable only at the root of the series hierarchy
		/// </summary>
		MultiSystem,
		/// <summary>
		/// Values of different series are drawn in independent chart areas. 
		/// </summary>
		MultiArea
};

	/// <summary>
	///     Container for <see cref="Series"/> or <see cref="CompositeSeries"/> object.
	/// </summary>
	/// 
	/// <remarks>
	///   <para>
	///     The <see cref="SeriesCollection"/> property <see cref="CompositeSeries.Subseries"/> provides
	///     access to other <see cref="CompositeSeries"/> or <see cref="Series"/> objects which are
	///     children nodes in the series hierarchy. The main function of <see cref="CompositeSeries"/> is to
	///     create presentation of its subnodes according to the property <see cref="CompositeSeries.CompositionKind"/>.
	///   </para>
	/// </remarks>
	// (KED)
	[NotIncludedInTemplate]
	public sealed class CompositeSeries : SeriesBase
	{
		private	SeriesCollection	seriesCollection;
		private	CompositionKind		compositionKind = CompositionKind.Sections;
		private	DataDimension		xDimension = null;
		private	DataDimension		yDimension = null;

		private	const double defaultDepth = 10.0;

        // Dynamic source series data
        // Dynamic source series is the series used to dynamically create series
        private string dynamicSourceSeriesName = "";
        private string dynamicSourceSeriesLegendText = "";


		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeSeries"/> class.
		/// </summary>
		/// <param name="name">The name of the <see cref="CompositeSeries"/> object.</param>
		public CompositeSeries(string name) : base(name) 
		{
			seriesCollection = new SeriesCollection(this);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeSeries"/> class.
		/// </summary>
		public CompositeSeries() : this("") { }
		#endregion

		#region --- Properties ---

        internal string DynamicSourceSeriesName { get { return dynamicSourceSeriesName; } set { dynamicSourceSeriesName = value; } }
        internal string DynamicSourceSeriesLegendText { get { return dynamicSourceSeriesLegendText; } set { dynamicSourceSeriesLegendText = value; } }
        
		/// <summary>
		/// Gets the <see cref="SeriesCollection"/> of the <see cref="SeriesBase"/> objects belonging to this <see cref="CompositeSeries"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
		public	  SeriesCollection  SubSeries 
		{
			get { return seriesCollection; } 
		}

		/// <summary>
		/// Gets or sets the composition of the series.
		/// </summary>
		[Description("The way multiple series are displayed.")]
		[DefaultValue(CompositionKind.Sections)]
		public CompositionKind CompositionKind 
		{
			get 
			{
				FixCompositionKind();
				return compositionKind; 
			} 
			set 
			{ compositionKind = value; } 
		}
		
		internal override int TotalNumberOfDataPoints 
		{ 
			get
			{
				int n = 0;
				foreach(SeriesBase ser in SubSeries)
					n += ser.TotalNumberOfDataPoints;
				return n;
			}
		}

		public override string ToString()
		{
			return "CompositeSeries '"+Name+"'," + CompositionKind.ToString();
		}

		internal bool MultiCS
		{
			get
			{
				return CompositionKind == CompositionKind.MultiSystem;
			}
		}

		internal bool MultiArea
		{
			get
			{
				return CompositionKind == CompositionKind.MultiArea;
			}
		}

		internal override void BindDimensions()
		{
			for(int i=0; i<SubSeries.Count; i++)
				SubSeries[i].BindDimensions();
			ComputeXDimension();
			ComputeYDimension();
			if(xDimension == null || yDimension == null)
				return;
			if(OwningSeries == null)
				PropagateDimensions(xDimension,yDimension);

			CoordinateSystem s = OwnCoordSystem;
			if(s != null)
			{
				s.XAxis.SetDimension(xDimension);
				s.YAxis.SetDimension(yDimension);
			}
		}

		private void ComputeXDimension()
		{
			if(CompositionKind == CompositionKind.MultiArea &&
				CompositionKind == CompositionKind.MultiSystem)
				xDimension = null;
			else
			{
				DataDimension dim = null;
				for(int i=0; i<SubSeries.Count; i++)
				{
					DataDimension dimNext = SubSeries[i].XDimension;
					if(dimNext != null)
					{
						if(dim == null)
							dim = dimNext;
						else
							dim = dim.Merge(dimNext);
					}
				}
				xDimension = dim;
			}
		}

		private void ComputeYDimension()
		{
			if(CompositionKind == CompositionKind.MultiArea ||
				CompositionKind == CompositionKind.MultiSystem)
				yDimension = null;
			else
			{
				DataDimension dim = null;
				for(int i=0; i<SubSeries.Count; i++)
				{
					if (SubSeries[i].HasIndependentYAxis)
					{
						if (dim == null && SubSeries[i].YDimension != null)
							dim = SubSeries[i].YDimension;
						continue;
					}
					DataDimension dimNext = SubSeries[i].YDimension;
					if(dimNext != null)
					{
						if(dim == null)
							dim = dimNext;
						else
							dim = dim.Merge(dimNext);
					}
					
				}
				yDimension = dim;
			}
		}


		internal override void PropagateDimensions(DataDimension xDim, DataDimension yDim)
		{
			if(OwningSeries != null &&
				OwningSeries.CompositionKind != CompositionKind.MultiArea &&
				OwningSeries.CompositionKind != CompositionKind.MultiSystem)
			{
				xDimension = xDim;
				if(!HasIndependentYAxis)
					yDimension = yDim;
			}
			foreach (SeriesBase ser in SubSeries)
			{				
				ser.PropagateDimensions(xDimension,yDimension);
			}
			
		}

		internal override DataDimension XDimension { get { return xDimension; } }
		internal override DataDimension YDimension { get { return yDimension; } }

		internal override DataDimension ZDimension 
		{
			get 
			{
				if(CoordSystem != null)
				{
					DataDimension zDimension = CoordSystem.ZAxis.Dimension;
					if(zDimension == null)
					{
						zDimension = new EnumeratedDataDimension(Name, typeof(string));
						CoordSystem.ZAxis.SetDimension(zDimension);
					}
					return zDimension;
				}
				return null;
			}
		}

		#endregion

		#region --- Navigation ---

		internal override SeriesBase FirstChild { get { return SubSeries[0]; } }

		internal override SeriesBase NextTo(SeriesBase s)
		{
			for(int i=0; i<SubSeries.Count-1; i++)
				if(SubSeries[i] == s)
					return SubSeries[i+1];
			return null;
		}

		internal override SeriesBase PreviousOf(SeriesBase s)
		{
			for(int i=1; i<SubSeries.Count; i++)
				if(SubSeries[i] == s)
					return SubSeries[i-1];
			return null;
		}

		/// <summary>
		/// Retrieves a <see cref="Series"/> object with a specified name.
		/// </summary>
		/// <param name="name">The name of the <see cref="Series"/> object.</param>
		/// <returns><see cref="Series"/> object with name 'name'; null if not found.</returns>
		public Series FindSeries(string name)
		{
			foreach(SeriesBase SB in SubSeries)
			{
				if(SB is Series)
				{
					if(SB.Name == name)
						return (Series)SB;
				}
				else
				{
					Series S = ((CompositeSeries)SB).FindSeries(name);
					if(S != null)
						return S;
				}
			}
			return null;
		}

		internal SeriesBase FindSeriesBase(string name)
		{
			if (Name == name)
				return this;
			for (int i = 0; i < SubSeries.Count; i++)
			{
				if (SubSeries[i].Name == name)
					return SubSeries[i];
				CompositeSeries child = SubSeries[i] as CompositeSeries;
				if (child != null)
				{
					SeriesBase sb = child.FindSeriesBase(name);
					if (sb != null)
						return sb;
				}
			}
			return null;
		}

		internal void NotifyMemberRemoved(SeriesBase series)
		{
			this.OwningChart.DataProvider.RemoveSeriesVariables(series);
		}

		/// <summary>
		/// Retrieves a <see cref="CompositeSeries"/> object with a specified name.
		/// </summary>
		/// <param name="name">The name of the <see cref="CompositeSeries"/> object.</param>
		/// <returns><see cref="CompositeSeries"/> object with name 'name'; null if not found.</returns>
		public CompositeSeries FindCompositeSeries(string name)
		{
			if(Name == name)
				return this;
			foreach(SeriesBase SB in SubSeries)
			{
				if(SB is CompositeSeries)
				{
					CompositeSeries rs = ((CompositeSeries)SB).FindCompositeSeries(name);
					if(rs != null)
						return rs;
				}
			}
			return null;
		}


		#endregion

		#region --- Coordinate system ---

		/// <summary>
		/// Gets or sets the <see cref="CoordinateSystem"/> associated with this <see cref="CompositeSeries"/> object.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [DefaultValue(null)]
		public override CoordinateSystem CoordSystem 
		{
			get
			{
				if(OwnCoordSystem != null)
					return OwnCoordSystem;
				CompositeSeries parent = this.Owner as CompositeSeries;
				if(parent != null)
					return parent.CoordSystem;
				else
				{
					if(OwningChart == null || OwningChart.InSerialization && !OwnCoordSystem.ShouldSerializeMe)
						return null;
					else
					{
						if(OwnCoordSystem == null)
							CreateCoordinateSystem();
						return OwnCoordSystem;
					}
				}
			} 
			set
			{
				OwnCoordSystem = value;
				if (OwnCoordSystem != null) 
					OwnCoordSystem.SetOwner(this);
					
			}
		}

		internal void AdjustAxisOrientationToProjectionKind()
		{
			// For 2D and isometric projections we have to override
			// non-standard axis orientations
			if(CoordSystem != null && (
				TargetArea.Mapping.Kind == ProjectionKind.Isometric ||
				TargetArea.Mapping.Kind == ProjectionKind.TwoDimensional))
			{
				CoordSystem.Orientation = CoordinateSystemOrientation.Default;
			}
		}

		internal override void SyncronizeZDimension() 
		{
			EnumeratedDataDimension zDim = ZDimension as EnumeratedDataDimension;
			if(zDim == null)
				return;
			Coordinate coord = zDim[this.Name];
			if(coord == null)
				return;

			// Add all missing series
			foreach(SeriesBase s in SubSeries)
			{
				Coordinate cc = coord.FirstChild;
				while(cc != null)
				{
					if((cc.Value as String) == s.Name)
						break;
					cc = cc.Next;
				}
				if(cc== null)
				{
					coord.Add(s.Name);
				}
				if(s is CompositeSeries)
					coord[s.Name].Width = 0;
			}

			Coordinate[] children = new Coordinate[SubSeries.Count];
			Coordinate c = coord.FirstChild;
			int n = 0;
			while(c != null)
			{
				if(SubSeries[c.Value as string] != null)
				{
					children[n] = c;
					n++;
				}
				c = c.Next;
			}

			// Recreate the subtree in the series order

			Coordinate previous = null;
			for(int i=0; i<n; i++)
			{
				string seriesName = SubSeries[i].Name;
				for(int j=0; j<n; j++)
				{
					if((children[j].Value as string) == seriesName)
					{
						if(previous == null)
							coord.FirstChild = children[j];
						else
							previous.Next = children[j];
						previous = children[j];
						previous.Next = null;
					}
				}
			}
			
			return;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CompositeSeries"/> object is logarithmic.
		/// </summary>
		[SRDescription("SeriesIsLogarithmicDescr")]
		[DefaultValue(false)]
		public override bool IsLogarithmic	
		{ 
			get 
			{ 
				if(base.IsLogarithmic || OwningChart.InSerialization)
					return base.IsLogarithmic; 
				// This is not declared logarithmic, but if all subseries are logarithmic,
				// this one will be as well!
				bool itIs = true;
				foreach(SeriesBase s in SubSeries)
					itIs = itIs && s.IsLogarithmic;
				return itIs;
			} 
			set { base.IsLogarithmic = value; }
		}

		
		#endregion

		#region --- Coordinates ---

		// DCS - Coordinates

		internal override object MinXDCS()
		{
			if(SubSeries.Count == 0)
				return null;
			DataDimension dim = XDimension;
			object val = null;
			double dval = double.MaxValue;
			for(int i=0; i<SubSeries.Count; i++)
			{
				Series ss = SubSeries[i] as Series;
				if(ss == null || ss.DataPoints.Count > 0)
				{
					double d = SubSeries[i].MinXLCS();
					if(d<dval)
					{
						dval = d;
						val = SubSeries[i].MinXDCS();
					}
				}
			}
			return val;
		}

		internal override object MaxXDCS()
		{
			if(SubSeries.Count == 0)
				return null;
			object val = null;
			double dval = double.MinValue;
			for(int i=0; i<SubSeries.Count; i++)
			{
				Series ss = SubSeries[i] as Series;
				if(ss == null || ss.DataPoints.Count > 0)
				{
					double d = SubSeries[i].MaxXLCS();
					if(d>dval)
					{
						dval = d;
						val = SubSeries[i].MaxXDCS();
					}
				}
			}
			return val;
		}

		internal override object MinYDCS()
		{
			if(SubSeries.Count == 0)
				return null;

			if(CompositionKind == CompositionKind.Stacked100)
			{
				return 0.0;
			}
			if(CompositionKind == CompositionKind.Stacked)
			{
				return SubSeries[0].MinYDCS();
			}

			object val = null;
			double dval = double.MaxValue;
			for(int i=0; i<SubSeries.Count; i++)
			{
				if(i>0 && SubSeries[i].HasIndependentYAxis)
					continue;
				Series ss = SubSeries[i] as Series;
				if(ss == null || ss.DataPoints.Count > 0)
				{
					double d = SubSeries[i].MinYLCS();
					if(d != double.NaN && d<dval)
					{
						dval = d;
						val = SubSeries[i].MinYDCS();
					}
				}
			}
			return val;
		}

		internal override object MaxYDCS()
		{
			if(SubSeries.Count == 0)
				return null;
			object val = null;
			if(CompositionKind == CompositionKind.Stacked100)
			{
				int i, n = SubSeries.Count;
				for(i=0; i<n; i++)
				{
					Series ser = SubSeries[i] as Series;
					if(ser == null)
						throw new Exception("Composite series '" + SubSeries[i].Name + "' cannot be stacked");
					if(ser.DataPoints.Count == 0) 
						continue;
					DataDescriptor yPar = ser.DataDescriptors["y"];
					if(yPar == null)
						throw new Exception("Stacked series '" + SubSeries[i].Name + "' doesn't have 'y' parameter defined");
					NumericVariable nVar = yPar.Value as NumericVariable;
					if(nVar == null)
						throw new Exception("Stacked series '" + SubSeries[i].Name + "' should be numeric");
				}
				return 100.0;
			}

			if(CompositionKind == CompositionKind.Stacked)
			{
				int i, n = SubSeries.Count;
				NumericVariable s = null;
				for(i=0; i<n; i++)
				{
					Series ser = SubSeries[i] as Series;
					if(ser == null)
						throw new Exception("Composite series '" + SubSeries[i].Name + "' cannot be stacked");
					if(ser.DataPoints.Count == 0) 
						continue;
					DataDescriptor yPar = ser.DataDescriptors["y"];
					if(yPar == null)
						throw new Exception("Stacked series '" + SubSeries[i].Name + "' doesn't have 'y' parameter defined");
					NumericVariable nVar = yPar.Value as NumericVariable;
					if(nVar == null)
						throw new Exception("Stacked series '" + SubSeries[i].Name + "' should be numeric");
					if(i==0)
						s = nVar;
					else
						s = (s + nVar).Value as NumericVariable;
				}

				double dVal = double.MinValue;
				for(i=0; i<s.Length; i++)
					dVal = Math.Max(dVal, s[i]);
				return dVal;

			}
			else
			{
				val = null;
				double dval = double.MinValue;
				if(YDimension != null)
				{
					for(int i=0; i<SubSeries.Count; i++)
					{
						if(i>0 && SubSeries[i].HasIndependentYAxis)
							continue;
						Series ss = SubSeries[i] as Series;
						if(ss == null || ss.DataPoints.Count > 0)
						{
							object m = SubSeries[i].MaxYDCS();
							if(m != null)
							{
								double d = YDimension.Coordinate(m) + YDimension.Width(m);
								if(d > dval)
								{
									dval = d;
									val = m;
								}
							}
						}
					}
				}
			}
			return val;
		}

		internal override object MinZDCS()
		{
			object val = null;
			if(SubSeries.Count == 0)
				return null;
			val = SubSeries[0].MinZDCS();
			double dval = SubSeries[0].MinZLCS();
			for(int i=1; i<SubSeries.Count; i++)
			{
				double d = SubSeries[i].MinZLCS();
				if(d<dval)
				{
					dval = d;
					val = SubSeries[i].MinZDCS();
				}
			}
			return val;
		}

		internal override object MaxZDCS()
		{
			if(SubSeries.Count == 0)
				return null;
			object val = SubSeries[0].MaxZDCS();
			double dval = SubSeries[0].MaxZLCS();
			for(int i=1; i<SubSeries.Count; i++)
			{
				double d = SubSeries[i].MaxZLCS();
				if(d>dval)
				{
					dval = d;
					val = SubSeries[i].MaxZDCS();
				}
			}
			return val;
		}

		internal override object MinDCS(string param)
		{
			if(SubSeries.Count == 0)
				return null;
			object val = SubSeries[0].MinDCS(param);
			double dval = SubSeries[0].MinLCS(param);
			for(int i=1; i<SubSeries.Count; i++)
			{
				double d = SubSeries[i].MinLCS(param);
				if(d<dval)
				{
					dval = d;
					val = SubSeries[i].MinDCS(param);
				}
			}
			return val;
		}

		internal override object MaxDCS(string param)
		{
			if(SubSeries.Count == 0)
				return null;
			object val = SubSeries[0].MaxDCS(param);
			double dval = SubSeries[0].MaxLCS(param);
			for(int i=1; i<SubSeries.Count; i++)
			{
				double d = SubSeries[i].MaxLCS(param);
				if(d>dval)
				{
					dval = d;
					val = SubSeries[i].MaxDCS(param);
				}
			}
			return val;
		}

		// LCS - Coordinates
		internal override double MinXLCS()
		{
			return MinLCS("x");
		}

		internal override double MaxXLCS()
		{
			return MaxLCS("x");
		}

		internal override double MinYLCS()
		{
			if(SubSeries.Count == 0)
				return double.MaxValue;
			double dval = SubSeries[0].MinYLCS();
			for(int i=1; i<SubSeries.Count; i++)
				dval = Math.Min(dval,SubSeries[i].MinYLCS());
			return dval;
		}

		internal override double MaxYLCS()
		{
			if(SubSeries.Count == 0)
				return double.MinValue;
			double dval = SubSeries[0].MaxYLCS();
			for(int i=1; i<SubSeries.Count; i++)
				dval = Math.Max(dval,SubSeries[i].MaxYLCS());
			return dval;
		}

		internal override double MinZLCS()
		{
			if(SubSeries.Count == 0)
				return double.MaxValue;
			double dval = SubSeries[0].MinZLCS();
			for(int i=1; i<SubSeries.Count; i++)
				dval = Math.Min(dval,SubSeries[i].MinZLCS());
			return dval;
		}

		internal override double MaxZLCS()
		{
			if(SubSeries.Count == 0)
				return double.MinValue;
			double dval = SubSeries[0].MaxZLCS();
			for(int i=1; i<SubSeries.Count; i++)
				dval = Math.Max(dval,SubSeries[i].MaxZLCS());
			return dval;
		}

		internal override double MinLCS(string param)
		{
			if(SubSeries.Count == 0)
				return double.MaxValue;
			if(CompositionKind == CompositionKind.MultiArea)
				return 0;
			double dval = SubSeries[0].MinLCS(param);
			for(int i=1; i<SubSeries.Count; i++)
				dval = Math.Min(dval,SubSeries[i].MinLCS(param));
			return dval;
		}

		internal override double MaxLCS(string param)
		{
			if(SubSeries.Count == 0)
				return double.MinValue;
			if(CompositionKind == CompositionKind.MultiArea)
				return 0;
			double dval = SubSeries[0].MaxLCS(param);
			for(int i=1; i<SubSeries.Count; i++)
				dval = Math.Max(dval,SubSeries[i].MaxLCS(param));
			return dval;
		}
		
		// WCS Coordinates

		internal void ComputeDefaultLayout()
		{
			DXICS = 0;
			DYICS = 0;
			DZICS = 0;

			for(int i=0; i<SubSeries.Count; i++)
			{
				SeriesBase subSeries = SubSeries[i];
				// We assume that initial layout has been computed for subseries
				double subSeriesDX = subSeries.DXICS; 
				double subSeriesDY = subSeries.DYICS; 
				double subSeriesDZ = subSeries.DZICS; 
				// Permutate values if there is CS at the child subseries
				CoordinateSystem SSCS = subSeries.OwnCoordSystem;
				if(SSCS != null)
				{
					Vector3D v = 
						SSCS.XAxis.UnitVector * subSeriesDX +
						SSCS.YAxis.UnitVector * subSeriesDY +
						SSCS.ZAxis.UnitVector * subSeriesDZ ;
					subSeriesDX = v.X;
					subSeriesDY = v.Y;
					subSeriesDZ = v.Z;
				}
				subSeries.OffsetZICS = 0;
				if(i == 0)
				{
					DXICS = subSeriesDX + subSeries.OffsetXICS;
					DYICS = subSeriesDY + subSeries.OffsetYICS;
					DZICS = subSeriesDZ + subSeries.OffsetZICS;
					if(CompositionKind == CompositionKind.MultiSystem)
					{
						int ix = XIndexInMultiSystem(SubSeries.Count,i);
						int iy = YIndexInMultiSystem(SubSeries.Count,i);
						subSeries.CoordSystem.OffsetYICS = subSeries.CoordSystem.DYICS*(iy+(iy>0? 0.2:0.0));
						subSeries.CoordSystem.OffsetXICS = subSeries.CoordSystem.DXICS*(ix+(ix>0? 0.2:0.0));
					}
				}
				else
				{
					switch(CompositionKind)
					{
						case CompositionKind.Sections:
							DXICS = Math.Max(DXICS,subSeriesDX);
							DYICS = Math.Max(DYICS,subSeriesDY);
							subSeries.OffsetZICS = DZICS;
							DZICS = DZICS + subSeriesDZ;
							break;
						case CompositionKind.MultiSystem:
						{
							int ix = XIndexInMultiSystem(SubSeries.Count,i);
							int iy = YIndexInMultiSystem(SubSeries.Count,i);
							subSeries.CoordSystem.OffsetYICS = subSeries.CoordSystem.DYICS*(iy+(iy>0? 0.2:0.0));
							subSeries.CoordSystem.OffsetXICS = subSeries.CoordSystem.DXICS*(ix+(ix>0? 0.2:0.0));
							DXICS = Math.Max(DXICS,subSeries.CoordSystem.OffsetXICS+subSeries.CoordSystem.DXICS);
							DYICS = Math.Max(DYICS,subSeries.CoordSystem.OffsetYICS+subSeries.CoordSystem.DYICS);
							DZICS = Math.Max(DZICS,subSeriesDZ);
						}
							break;
						default:
							DXICS = Math.Max(DXICS,subSeriesDX + subSeries.OffsetXICS);
							DYICS = Math.Max(DYICS,subSeriesDY + subSeries.OffsetYICS);
							DZICS = Math.Max(DZICS,subSeriesDZ + subSeries.OffsetZICS);
							break;
					}
				}
			}
		}

		private int XIndexInMultiSystem(int n, int i)
		{
			int nx = (int)(Math.Sqrt(n-1)+1);
			int ny = (n-1)/nx + 1;
			int row = i/nx;
			return i-nx*row;
		}

		private int YIndexInMultiSystem(int n, int i)
		{
			int nx = (int)(Math.Sqrt(n-1)+1);
			int ny = (n-1)/nx + 1;
			int row = i/nx;
			return ny-1-row;
		}

		#endregion

        #region --- Style Handling ---

		/// <summary>
		/// Gets or sets the style name to be used with this <see cref="CompositeSeries"/> object.
		/// </summary>
		/// <remarks>Sets this style to all the children series.</remarks>
		[Category("General")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[System.ComponentModel.TypeConverter(typeof(SelectedSeriesStyleConverter))]
		public override string StyleName { get { return base.StyleName; } set { base.StyleName = value; SetStyleNameToChildren(value); } }	

        internal override void SetStyleNameToChildren(string name) 
        {
            foreach (SeriesBase S in seriesCollection)
                S.StyleName = name;
        }

        #endregion

        #region --- Building ---

		internal override void RegisterVariables()
		{
			foreach(SeriesBase sb in SubSeries)
				sb.RegisterVariables();
		}

		internal override void DataBind()
		{
			base.DataBind();

			if(seriesCollection == null || SubSeries.Count == 0)
				return;

			// Create coordinate systems and target areas for children series
			if(MultiCS || MultiArea)
			{
				int i = 0;
				foreach(SeriesBase child in SubSeries)
				{
					if(child.OwnCoordSystem == null)
						child.OwnCoordSystem = new CoordinateSystem();
					if(MultiArea && (child.OwnTargetArea == null || OwningChart.InDesignMode || OwningChart.InitializeOnDataBind))
					{
						child.OwnTargetArea = TargetArea.GetSubSection(i,SubSeries.Count);
						child.OwnTargetArea.Mapping = (Mapping)(TargetArea.Mapping.Clone());
						i++;
					}
				}
			}
			else
			{
				foreach(SeriesBase child in SubSeries)
				{
					child.OwnTargetArea = null;
					child.OwnCoordSystem = null;
				}
			}
			
			SyncronizeZDimension();

			AddThisToZDimensionHierarchy();

			foreach(SeriesBase S in seriesCollection)
			{
				S.DataBind();
				if(OwnCoordSystem != null && ZAxis.Dimension is EnumeratedDataDimension)
				{
					if(ZAxis.MinValue == null)
						ZAxis.MinValue = S.Name;
					ZAxis.MaxValue = S.Name;
				}
			}

			if(OwningChart.HasErrors)
				return;
			
			ComputeDefaultLayout();

			if(OwnCoordSystem != null)
			{
				OwnCoordSystem.DataBind();
			}

		}


		internal void CreateChildernTargetAreas()
		{
			if(!MultiArea)
				return;
			
			int n = SubSeries.Count;
			for(int i = 0; i<n; i++)
			{
				SeriesBase child = SubSeries[i];
				if(child.OwnTargetArea == null)
				{
					child.OwnTargetArea = TargetArea.GetSubSection(i,n);
					child.OwnTargetArea.Mapping = (Mapping)(TargetArea.Mapping.Clone());
				}
			}
		}
		internal override void ComputeDefaultICSSize()
		{
			double firstDYICS = 0;
			bool firstItem = true;
			foreach(SeriesBase item in SubSeries)
			{
				item.ComputeDefaultICSSize();
				if(firstItem)
				{
					firstDYICS = item.DYICS;
					firstItem = false;
				}
				else
				{
					if(item.HasIndependentYAxis)
						item.YAxis.SetMaxValueICS(firstDYICS);
				}
			}
		}

		internal override void BindParameters()
		{
			foreach(SeriesBase S in seriesCollection)
			{
				S.BindParameters();
			}
			if(CompositionKind == CompositionKind.Stacked)
				BindStackedSeries(false);
			if(CompositionKind == CompositionKind.Stacked100)
				BindStackedSeries(true);
		}

		private void BindStackedSeries(bool percent100)
		{
			int n = seriesCollection.Count;
			if(n == 0)
				return;
			Series prev = null;
			Series first = null;
			foreach(SeriesBase s in seriesCollection)
			{
				Series ss = s as Series;
				if(ss == null)
					return;
				if(prev == null)
				{
					// First series: reference value should be ignored
					ss.IgnoreReferenceValue();
					first = ss;
				}
				else
				{
					if(!ss.StackOn(prev))
						throw new Exception("Cannot apply stacking composition on series '"  + Name + "' and '" +
							s.Name + "' because of data type");
				}
				prev = ss;
			}
			// now the prev is the topmost series
			foreach(Series s in seriesCollection)
				s.SetYLCSRange(first,prev);

			if(percent100)
			{
				foreach(Series s in seriesCollection)
					s.Scale100(prev);
			}
		}

		internal override void ComputeSize()
		{
			int n = seriesCollection.Count;
			if(n > 0)
			{
				double xMax = double.MinValue, yMax = double.MinValue, zMax = double.MinValue;
				foreach(SeriesBase S in seriesCollection)
				{
					S.ComputeSize();
					Vector3D sSize = new Vector3D(S.DXICS+S.OffsetXICS ,S.DYICS+S.OffsetYICS ,S.DZICS+S.OffsetZICS);
					CoordinateSystem CS = S.OwnCoordSystem;
					if(CS != null)
					{
						sSize = 
							CS.XAxis.UnitVector*sSize.X +
							CS.YAxis.UnitVector*sSize.Y +
							CS.ZAxis.UnitVector*sSize.Z +
							new Vector3D(CS.OffsetXICS,CS.OffsetYICS,CS.OffsetZICS);
					}
					yMax = Math.Max(yMax,sSize.Y);
					xMax = Math.Max(xMax,sSize.X);
					zMax = Math.Max(zMax,sSize.Z);
				}

				DXICS = xMax;
				DYICS = yMax;
				DZICS = zMax;
			}
			else
			{
				DXICS = 1;
				DYICS = 1;
				DZICS = 1;
			}
		}

		internal override void WCSRange(out double x0, out double y0, out double z0, out double x1, out double y1, out double z1)
		{
			x0 = double.MaxValue;
			y0 = double.MaxValue;
			z0 = double.MaxValue;

			x1 = double.MinValue;
			y1 = double.MinValue;
			z1 = double.MinValue;

			ComputeSize();

			//double xx0, yy0, zz0, xx1, yy1, zz1;
			for(int i=0; i<2; i++)
			{
				double x = i*XAxis.MaxValueICS;
				for (int j=0; j<2; j++)
				{
					double y = j*YAxis.MaxValueICS;
					for(int k=0; k<2; k++)
					{
						double z = k*ZAxis.MaxValueICS;
						Vector3D s = CoordSystem.ICS2WCS(new Vector3D(x,y,z));
						x0 = Math.Min(x0,s.X);
						y0 = Math.Min(y0,s.Y);
						z0 = Math.Min(z0,s.Z);
						x1 = Math.Max(x1,s.X);
						y1 = Math.Max(y1,s.Y);
						z1 = Math.Max(z1,s.Z);
					}
				}
			}
			return;
		}

        internal override void Build()
        {
			if(IsEmpty())
				return;
			
			double zOffset = 0;
			EnumeratedDataDimension zDim = (ZDimension as EnumeratedDataDimension);
			if(zDim != null)
			{
				Coordinate myCoord = zDim[Name];
				if(myCoord != null)
				{
					myCoord.HasMergedMembers = 
						compositionKind == CompositionKind.Merged ||
						compositionKind == CompositionKind.Stacked ||
						compositionKind == CompositionKind.Stacked100;
					zOffset = myCoord.Offset;
				}
				else
					zOffset = 0;
			}
			foreach(SeriesBase S in seriesCollection)
			{
				S.Build();
				if(Style.ChartKindCategory != ChartKindCategory.PieDoughnut && !MultiCS && !MultiArea)
				{
					Coordinate ddi = (ZDimension as EnumeratedDataDimension)[S.Name];
					if(ddi != null)
					{
						ddi.Offset = zOffset;
						if(CompositionKind == CompositionKind.Sections)
							zOffset += ddi.Width;
					}
				}
			}

			try
			{
				switch(CompositionKind)
				{
					case CompositionKind.MultiSystem:		BuildMultiCSInColumn();	break;
					case CompositionKind.Merged:			BuildMerged();			break;
					case CompositionKind.Sections:			BuildSections();		break;
					case CompositionKind.Stacked:			BuildStacked();			break;
					case CompositionKind.Stacked100:		BuildStacked100();		break;
					case CompositionKind.Concentric:		BuildConcentric();		break;
					default: break;
				}
			}
			catch
			{
				CompositionKind = CompositionKind.Sections;
				BuildSections();
			}

			if(CoordSystem == OwnCoordSystem) // this owns the coordinate system, so we can build it now
			{
					OwnCoordSystem.Build();
			}
        }

		internal override bool IsEmpty()
		{
			foreach(SeriesBase s in SubSeries)
				if(!s.IsEmpty())
					return false;
			return true;
		}

		internal void BuildMultiCSInColumn()
		{
		}

		internal void BuildMerged()
		{
			AdjustXCoordinate();
		}

		internal override void AdjustXCoordinate()
		{
			Debug.WriteLine(Name + " adjusting x to " + X0.ToString("0.00 - ") + X1.ToString("0.00"));

			if(compositionKind != CompositionKind.Merged)
			{
				foreach(SeriesBase s in seriesCollection)
				{
					s.X0 = X0;
					s.X1 = X1;
					s.AdjustXCoordinate();
				}
			}
			else
			{
				// Count the number of bar-style 
				int n = NumberOfSubSeriesToBeMerged();
				Debug.WriteLine("In " + Name + " number of series to be merged = " + n);
				double dx = X1 - X0;
				if(n > 0)
					dx = (X1-X0)/n;
				double x = X0;
				foreach(SeriesBase s in seriesCollection)
				{
					if(s is Series)
					{
						if(s.Style.IsBar)
						{
							s.X0 = x;
							x += dx;
							s.X1 = x;
						}
					}
					else
					{
						CompositeSeries cs = s as CompositeSeries;
						if(cs.HasBarSeries())
						{
							s.X0 = x;
							x += dx;
							s.X1 = x;
						}
					}
					s.AdjustXCoordinate();
				}
			}
		}

		internal int NumberOfSubSeriesToBeMerged()
		{
			// Count number of bar-style subseries that share the x-coordinate range
			if(CompositionKind != CompositionKind.Merged)
				return 0;
			int n = 0;
			foreach(SeriesBase s in seriesCollection)
			{
				if(s.HasBarSeries())
					n++;
			}
			return n;
		}

		internal override bool HasBarSeries()
		{
			foreach(SeriesBase s in SubSeries)
			{
				if(s.HasBarSeries())
					return true;
			}
			return false;
		}

		internal void BuildOverlay()
		{
		}

		internal void BuildSections()
		{
		}

		internal void BuildStacked()
		{
		}

		internal void BuildStacked100()
		{
		}

		internal void BuildConcentric()
		{
			BuildConcentric(0.0,1.0);
		}

		internal void BuildConcentric(double a0, double a1)
		{
			int n = seriesCollection.Count;
			if(n == 0)
				return;
			double radiusRange0 = a0, radiusRangeStep = (a1-a0)/n;
			foreach(SeriesBase s in seriesCollection)
			{
				if(s is Series)
				{
					Series ser = s as Series;
					for(int i=0;i<ser.DataPoints.Count;i++)
					{
						ser.DataPoints[i]["radiusRange0"] = radiusRange0;
						ser.DataPoints[i]["radiusRange1"] = radiusRange0+radiusRangeStep;
					}
				}
				else
				{
					CompositeSeries cser = s as CompositeSeries;
					cser.BuildConcentric(radiusRange0,radiusRange0+radiusRangeStep);
				}
				radiusRange0 += radiusRangeStep;
			}
		}

		internal int GetSequenceNumber(Series s)
		{
			// Here we maintain unique sequence numbers for subseries, used to
			// construct names of required variables

			if(OwningSeries != null)
				return OwningSeries.GetSequenceNumber(s);
			bool found;
			return NumberOfSimpleSeriesBefore(s,out found);
		}

		internal int NumberOfSimpleSeries
		{
			get
			{
				bool found;
				return NumberOfSimpleSeriesBefore(null,out found);
			}
		}

		internal override int NumberOfSimpleSeriesBefore(Series s, out bool found)
		{
			int n = 0;
			found = false;
			for(int i=0;i<SubSeries.Count;i++)
			{
				n += SubSeries[i].NumberOfSimpleSeriesBefore(s,out found);
				if(found)
					break;
			}
			return n;
		}

		internal override Series[] SimpleSubseriesList
		{
			get
			{
				ArrayList list = new ArrayList();
				foreach(SeriesBase sb in SubSeries)
				{
					Series[] lst = sb.SimpleSubseriesList;
					list.AddRange(lst);

				}

				Series[] result = new Series[list.Count];
				for(int i=0;i<list.Count; i++)
					result[i] = (Series)list[i];
				return result;
			}
		}

        #endregion

		#region --- Rendering ---

		internal override void HandleMissingDataPoints()
		{
			// This is recursive. Except for the recursive descent, this should be called only
			// on the root node.
			foreach(SeriesBase series in SubSeries)
				series.HandleMissingDataPoints();
		}

		internal override void Render(bool toSetupArea)
		{
			if(IsEmpty())
				return;
			
			int cnt = seriesCollection.Count;
			if(cnt == 0)
				return;

			if (TotalNumberOfDataPoints == 0)
				return;

			CheckIfCompositionKindCompatibleWithParent();

			PushTargetArea();
			if(CompositionKind == CompositionKind.MultiArea)
			{
				for(int i=1; i<=cnt; i++)
				{
					SeriesBase childSeries = seriesCollection[cnt-i];
					if(childSeries.OwnTargetArea != null)
					{
						childSeries.OwnTargetArea.SetCoordinates();
					}
					childSeries.Render();
				}
			}
			else
			{
				if(YAxis.MinValue == null)
					return;
				PushCoordinateSystem();
				if(OwnCoordSystem != null &&
					Style.ChartKind != ChartKind.Pie &&
					Style.ChartKind != ChartKind.Doughnut &&
					!MultiArea)
					OwnCoordSystem.Render();

				SectionBox sb = new SectionBox();
				GE.Push(sb);
				sb.Tag = this;

				try
				{
					for(int i=1; i<=cnt; i++)
					{
						SeriesBase childSeries = seriesCollection[cnt-i];
						childSeries.Render();
					}

					ConsolidateBoxStructure();
					GE.Pop(sb.GetType());
					PopCoordinateSystem();
				}
				catch(Exception e)
				{
#if DEBUG
					Debug.WriteLine(e.Message + "\n" + e.StackTrace);
					GE.Root.Dump(0);
#endif
					throw;
				}
			}

			PopTargetArea();
		}

		private void ConsolidateBoxStructure()
		{
			// At the topmost level we consolidate rearrange coord system boxes

			if(OwningSeries == null)
			{
				CoordinateSystemBox csb = (GE.Root.SubObjects[0] as TargetAreaBox).SubObjects[0] as CoordinateSystemBox;
				if(csb == null)
					return;
				csb.RearrangeInternalCoordinateSystemBoxes();
			}

			if(this.CompositionKind == CompositionKind.Merged)
			{
				// move all column boxes to the same section box

				SectionBox resultSection = new SectionBox();
				resultSection.Tag = this;
				GeometricObject parent = null; // where the resulting section belongs
				ArrayList container = null; // where the sections are taken from
				if(GE.Top is CoordinateSystemBox)
				{
					parent = (GE.Top as CoordinateSystemBox).Interior;
					container = (GE.Top as CoordinateSystemBox).Interior.SubObjects;

				}
				else if(GE.Top is SectionBox)
				{
					parent = GE.Top;
					container = GE.Top.SubObjects;
				}
				else
					throw new Exception("Implementation: Cannot consolidate box structure; unexpected object on top of stack, '" +
						GE.Top.GetType().Name + "'");

				int cnt = 0;

				cnt = container.Count;
				for(int i=0; i<cnt; i++)
				{
					// NOTE: We always take from index 0, because in the next step the object will be removed
					// so the next pop to the 0 index!
					SectionBox sb = container[0] as SectionBox;
					container.RemoveAt(0);
					if(sb != null)
					{
						for(int j=0; j<sb.SubObjects.Count;j++)
							resultSection.Add(sb[j]);
					}
				}
				parent.Add(resultSection);
			}
			else if(this.CompositionKind == CompositionKind.Stacked ||
				this.CompositionKind == CompositionKind.Stacked100)
			{
				// move sub-column boxes to the same column box
				// Get coordinate system box currently on top
				SectionBox resultSection = new SectionBox();
				resultSection.Tag = this;
				GeometricObject parent = null; // where the resulting section belongs
				ArrayList container = null; // where the sections are taken from
				if(GE.Top is CoordinateSystemBox)
				{
					parent = (GE.Top as CoordinateSystemBox).Interior;
					container = (GE.Top as CoordinateSystemBox).Interior.SubObjects;

				}
				else if(GE.Top is SectionBox)
				{
					parent = GE.Top.Parent;
					container = GE.Top.SubObjects;
				}
				else
					throw new Exception("Implementation: Cannot consolidate box structure; unexpected object on top of stack, '" +
						GE.Top.GetType().Name + "'");

				for(int i=0; i<container.Count; i++)
				{
					SectionBox sb = container[i] as SectionBox;
					if(sb != null && sb.SubObjects != null)
					{
						bool emptyResultSection = (resultSection.SubObjects == null);
						for(int j=0; j<sb.SubObjects.Count;j++)
						{
							if(emptyResultSection || sb[j] is ChartText)
								resultSection.Add(sb[j]);
							else if(sb[j].SubObjects != null)
							{
								for(int k=0; k<sb[j].SubObjects.Count; k++)
									resultSection[j].Add(sb[j][k]);
							}
						}
						emptyResultSection=false;
					}
				}
				container.Clear();
				parent.Add(resultSection);
			}
		}

		internal override void RenderLegends()
		{
			base.RenderLegends();
			foreach(SeriesBase sb in SubSeries)
				sb.RenderLegends();
		}
	
		internal override void RenderTitles()
		{
			if(OwnTargetArea != null)
				OwnTargetArea.Render();
			foreach(SeriesBase child in SubSeries)
				child.RenderTitles();
		}

		private void CheckIfCompositionKindCompatibleWithParent()
		{
			CompositeSeries parent = OwningSeries;
			if(parent == null)
				return;

			CompositionKind parentKind = parent.CompositionKind;
			bool compatible = true;
			switch(CompositionKind)
			{
				case CompositionKind.Concentric:
					compatible = (parentKind == CompositionKind.Concentric);
					break;
				case CompositionKind.Merged:
					compatible = (parentKind != CompositionKind.Stacked) && 
						(parentKind != CompositionKind.Stacked100);
					break;
				case CompositionKind.MultiArea:
					compatible = (parentKind == CompositionKind.MultiArea);
					break;
				case CompositionKind.MultiSystem:
					compatible = (parentKind == CompositionKind.MultiArea) ||
						 (parentKind == CompositionKind.MultiSystem);
					break;
				case CompositionKind.Sections:
					compatible = (parentKind != CompositionKind.Stacked) && 
						(parentKind != CompositionKind.Stacked100) &&
						(parentKind != CompositionKind.Merged);
					break;
				case CompositionKind.Stacked:
					compatible = (parentKind != CompositionKind.Stacked) && 
						(parentKind != CompositionKind.Stacked100);
					break;
				case CompositionKind.Stacked100:
					compatible = (parentKind != CompositionKind.Stacked) && 
						(parentKind != CompositionKind.Stacked100);
					break;
				default:
					throw new Exception("Composition kind " + 
						CompositionKind + " not handled in CheckIfCompositionKindCompatibleWithParent()");
			}
			if(!compatible)
				throw new Exception("Composition kind '" + CompositionKind.ToString() +
					"' of series '" + Name + "' is not compatible with composition kind '" +
					parentKind.ToString() + "' of the parent series '" + parent.Name + "'");
		}
		#endregion

		#region --- Legend Handling ---

		internal override void FillLegend(Legend legend) 
		{
			if(compositionKind == CompositionKind.Stacked || compositionKind == CompositionKind.Stacked100)
			{
				// Backwards to show legend in the same vertical order
				for(int i=SubSeries.Count-1;i>=0;i--)
				{
					SubSeries[i].FillLegend(legend);
				}
			}
			else
			{
				for(int i=0;i<SubSeries.Count;i++)
				{
					SubSeries[i].FillLegend(legend);
					if(CompositionKind == CompositionKind.Concentric)
						break; // for pie and doughnut only the first set shows in the legend
				}
			}
		}

		#endregion

		#region --- Serialization & Browsing Control ---

		private bool ShouldSerializeReferenceValue()	{ return ReferenceValue != null ; }
		#endregion
	
		#region --- Handling y range and reference values ---
		
		internal override void AdjustReferenceValuesToYRange(object yMinValue, object yMaxValue)
		{
			base.AdjustReferenceValuesToYRange(yMinValue,yMaxValue);
			foreach(SeriesBase sb in SubSeries)
			{
				if(!sb.HasIndependentYAxis)
					sb.AdjustReferenceValuesToYRange(yMinValue,yMaxValue);
			}
		}

		internal override void GetReferenceValuesRange(ref object minRef, ref object maxRef, bool useImpliedValues)
		{
			// Note: we never use implied values of composite series
			base.GetReferenceValuesRange(ref minRef,ref maxRef,false);
			foreach(SeriesBase sb in SubSeries)
			{
				if(!sb.HasIndependentYAxis)
					sb.GetReferenceValuesRange(ref minRef,ref maxRef,useImpliedValues);
			}
		}


		#endregion

		internal bool IsApplicable(CompositionKind ck) 
		{
			SeriesStyle S = this.Style;
			if (S.ChartKind == ChartKind.Pie || S.ChartKind == ChartKind.Doughnut) 
				return (ck == CompositionKind.Concentric || ck == CompositionKind.MultiArea);
			else
				return (ck != CompositionKind.Concentric);
		}

		private void FixCompositionKind()
		{
			SeriesStyle S = this.Style;
			if(S.ChartKind == ChartKind.Pie || S.ChartKind == ChartKind.Doughnut)
			{
				if(compositionKind != CompositionKind.MultiArea)
					compositionKind = CompositionKind.Concentric;
			}
			else if (compositionKind == CompositionKind.Concentric)
				compositionKind = CompositionKind.Sections;
		}
	}
}
