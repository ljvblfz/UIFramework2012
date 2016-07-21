using System;
using System.Collections;
using System.ComponentModel;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	internal enum BoolExt
	{
		Yes,
		No,
		NotSet
	};

	// ===============================================================================================
	//		Series Base Class
	// ===============================================================================================

	/// <summary>
	///     <see cref="SeriesBase"/> class is a base class for the <see cref="Series"/> or 
	///     the <see cref="CompositeSeries"/> chart series classes.
	/// </summary>
	/// <remarks>
	/// <para>
	///		The charting object model is made up of two parts: the style part (implemented as aset of style 
	///		collections) and the data part implemented as a series hierarchy.
	///		The <see cref="SeriesBase"/> objects are the building blocks of the series hierarchy. 
	///	</para>
	///	<para>
	///     <see cref="SeriesBase.OwningSeries"/> navigates to the parent <see cref="CompositeSeries"/>.
	///     The <see cref="SeriesBase.Name"/> property can be used as the index to the parent composite series
	///     property <see cref="CompositeSeries.SubSeries"/> to navigate from the parent to the child in the 
	///     series hierarchy.
	/// </para>
	/// <para>
	///     In the data binding process, depending on the composition kind of the parent composite series, a
	///     <see cref="CoordinateSystem"/> object may be automatically created and assigned to the 
	///     <see cref="SeriesBase.OwnCoordSystem"/> property. Not all nodes have a non-null 
	///     <see cref="SeriesBase.OwnCoordSystem"/> property, but all nodes a have non-null
	///     <see cref="SeriesBase.CoordSystem"/> property defining the coordinate system within which
	///     the node and all subordinated nodes (and their data points) are rendered. If a node doesn't have
	///     the <see cref="SeriesBase.OwnCoordSystem"/> it takes the coordinate system of the first parent node
	///     in the series hierarchy.
	/// </para>
	/// <para>
	///     The <see cref="SeriesBase.TargetArea"/> property represents a rectangular region within the target 
	///     bitmap where the <see cref="SeriesBase"/> object is rendered. 
	///     It is automatically created in the root node of the series hierarchy and in 
	///     those internal nodes whose parent <see cref="CompositeSeries"/> has the "MultiArea" composition kind.
	/// </para>
	/// </remarks>
	[TypeConverter(typeof(SeriesBaseConverter))]
	[Serializable]
	public abstract class SeriesBase : ChartObject, INamedObject
	{
		private	Hashtable		pointProperties = new Hashtable();
		private	Hashtable		parameters = new Hashtable();

		private CoordinateSystem  coordinateSystem = null;
		private bool			isThisLogarithmic = false;
		private int				logBase = 10;
 
		private	const double defaultZ0 = 0.0;
		private const string defaultStyleName = "Cylinder";

		private string	    styleName="";//defaultStyleName;
		private string      missingPointsStyleName="";
		private MissingPointHandlerKind missingPointHandlerKind = MissingPointHandlerKind.Auto;
		private MissingPointHandler customMissingPointHandler = null;
		private	string	    cashedStyleName=defaultStyleName;
		private	SeriesStyle cashedStyle=null;
		private	double		x0 = 0, x1 = 1; // for positioning within x-interval
		private	object		referenceValue = null;
		private	double		offsetXICS = 0, offsetYICS = 0, offsetZICS = 0;

		// Data point labels
		private SeriesLabelsCollection		seriesLabels;

		// Multi-y-axis support

		private bool		hasOwnYAxis = false;

		private bool renderAreaMap = false;

		// ICS Coordinates
		//		Sizes
		private	double		dXICS, dYICS, dZICS=10;

		// Target area
		private TargetArea	targetArea = null;

		// Label (in coordinate system and legend)
		private string label = null;
		private bool removeParentNameFromLabel = false;

		private BoolExt adjustReferenceValue = BoolExt.NotSet;

		/// <summary>
		/// Initializes a new instance of the <see cref="SeriesBase"/> class. 
		/// </summary>
		/// <param name="name">The name of the new <see cref="SeriesBase"/> object.</param>
		protected SeriesBase(string name) 
		{ 
			Name = name; 
			seriesLabels = new SeriesLabelsCollection(this);
			genericReference.ValueSet += new EventHandler(HandleReferenceChanged);
		}

		internal abstract DataDimension XDimension { get; }
		internal abstract DataDimension YDimension { get; }
		internal abstract DataDimension ZDimension { get; }

		internal Mapping Mapping { get { return TargetArea.Mapping; } }

        internal string highlightedImageName = null;

		// (ED)
		/// <summary>
		/// Series label used in coordinate system annotation and legend 
		/// </summary>
		[DefaultValue(null)]
		[Description("Series label used in coordinate system annotation and legend")]
		[Category("General")]
		public string Label { get { return label; } set { label = value; } }

		// (ED)
		/// <summary>
		/// Sets or gets whether the parent series name should be removed from this series label
		/// when it is constructed from the series name.
		/// </summary>
		[DefaultValue(false)]
		[Description("Should parent series name be removed from this series label")]
		[Category("General")]
		public bool RemoveParentNameFromLabel { get { return removeParentNameFromLabel; } set { removeParentNameFromLabel = value; } }

		internal string GetEffectiveLabel()
		{
			if (label != null)
				return label;
			string lbl = Name;
			if (OwningSeries != null)
			{
				bool nameHasParentSufix = lbl.EndsWith(OwningSeries.Name);
				if (removeParentNameFromLabel && nameHasParentSufix)
				{
					lbl = lbl.Substring(0,lbl.Length - (OwningSeries.Name.Length+1)).Trim();
				}
			}
			return lbl;
		}

		#region --- Datapoint Parameters ---

		/// <summary>
		/// Gets the collection of <see cref="SeriesLabels"/> that belong to this <see cref="Series"/> object.
		/// </summary>
		[Category("General")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SeriesLabelsCollection			Labels		{ get { return seriesLabels; } }

		internal void GetEffectiveLabels()
		{
			if(seriesLabels == null)
				seriesLabels = new SeriesLabelsCollection();
			if(seriesLabels.Count == 0)
			{
				SeriesBase s = OwningSeries;
				while(s != null)
				{
					if(s.Labels != null && s.Labels.Count != 0)
					{
						for(int i=0; i<s.Labels.Count; i++)
						{
							SeriesLabels labels = new SeriesLabels();
							labels.LabelStyleName = s.Labels[i].LabelStyleName;
							labels.LabelExpression = s.Labels[i].LabelExpression;
							labels.Visible = s.Labels[i].Visible;
							labels.Inherited = true;
							seriesLabels.Add(labels);
						}
						break;
					}
					else
						s = s.OwningSeries;
				}
			}
			return;
		}

			/// <summary>
			/// Gets the parameters of <see cref="DataPoint"/>s belonging to this <see cref="SeriesBase"/> object.
			/// </summary>
			/// <remarks>
			/// Use this property to assign parameters to data points.
			/// </remarks>
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			public Hashtable DataPointParameters { get { return pointProperties; } }
		
		internal Hashtable AccumulatedDataPointParameters()
		{
			Hashtable ht;
			SeriesBase parent = OwningSeries;
			if(parent != null)
			{
				ht = parent.AccumulatedDataPointParameters();
				IDictionaryEnumerator de = pointProperties.GetEnumerator();
				while (de.MoveNext())
				{
					ht.Remove(de.Key);
					ht.Add(de.Key,de.Value);
				}
			}
			else
				ht = new Hashtable(pointProperties);
			return ht;
		}

		#endregion

		#region --- Series Parameters ---

		/// <summary>
		/// Gets or sets a value that indicates whether this <see cref="SeriesBase"/> object has and independent y-axis.
		/// </summary>
		/// <remarks>
        /// Independent y-axis may have different scale and range of values from other series in the same
		/// coordinate system. This feature is commonly known ar "Multiple Y-axis" feature.
		/// </remarks>
		public bool HasIndependentYAxis { get { return hasOwnYAxis; } set { hasOwnYAxis = value; } }

		/// <summary>
		/// Gets the Parameter hashtable of this <see cref="SeriesBase"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Hashtable Parameters { get { return parameters; } }
		
		/// <summary>
		/// Retrieves a parameter value of this <see cref="SeriesBase"/> object.
		/// </summary>
		/// <param name="key">Parameter key</param>
		/// <returns>Value of the parameter.</returns>
		/// <remarks>
		/// 
		/// </remarks>
		public object Parameter(object key) 
		{
			object param = parameters[key];
			if(param == null && OwningSeries != null)
				param = OwningSeries.Parameter(key);
			return param;
		}

		internal abstract int TotalNumberOfDataPoints { get; }

		#endregion

		#region --- Navigation in the series hierarchy ---

		/// <summary>
		/// Gets the next sibling of this <see cref="SeriesBase"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal SeriesBase Next		{ get { return OwningSeries.NextTo(this); } }
		/// <summary>
		/// Gets the previous sibling of this <see cref="SeriesBase"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal SeriesBase Previous	{ get { return OwningSeries.PreviousOf(this); } }
		internal abstract SeriesBase FirstChild { get ; }

		internal abstract SeriesBase NextTo(SeriesBase s);
		internal abstract SeriesBase PreviousOf(SeriesBase s);

        /// <summary>
        /// Moves the series to the first visual (last logical) position in its parent <see cref="CompositeSeries"/>.
        /// </summary>
        public void MoveToFront()
        {
            Coordinate seriesNode = ((EnumeratedDataDimension)this.OwningChart.Series.CoordSystem.ZAxis.Dimension)[this.Name];
            Coordinate parentNode = ((EnumeratedDataDimension)this.OwningChart.Series.CoordSystem.ZAxis.Dimension)[this.OwningSeries.Name];
            
            if (seriesNode == null)
                throw new Exception("Expecting an EnumeratedDataDimension in the Z Axis");
            else
            {
                seriesNode.MoveToBack(this.OwningChart);
                CoordSystem.ZAxis.MinValue = parentNode.FirstChild.Value;
                CoordSystem.ZAxis.MaxValue = parentNode.LastChild.Value;
            }
        }

        /// <summary>
        /// Moves the series to the last visual (first logical) position in its parent <see cref="CompositeSeries"/>.
        /// </summary>
        public void MoveToBack()
        {
            Coordinate seriesNode = ((EnumeratedDataDimension)this.OwningChart.Series.CoordSystem.ZAxis.Dimension)[this.Name];
            Coordinate parentNode = ((EnumeratedDataDimension)this.OwningChart.Series.CoordSystem.ZAxis.Dimension)[this.OwningSeries.Name];
            
            if (seriesNode == null)
                throw new Exception("Expecting an EnumeratedDataDimension in the Z Axis");
            else
            {
                seriesNode.MoveToFront(this.OwningChart);
                CoordSystem.ZAxis.MinValue = parentNode.FirstChild.Value;
                CoordSystem.ZAxis.MaxValue = parentNode.LastChild.Value;
            }

        }

		#endregion

		#region --- Z -positioning ---

		/// <summary>
		/// Gets the owning series of this <see cref="SeriesBase"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public		CompositeSeries	OwningSeries    { get { return Owner as CompositeSeries; } }
		internal	double			X0				{ get { return x0; }	set { x0 = value; } }
		internal	double			X1				{ get { return x1; }	set { x1 = value; } }
		internal	double			ReducedX0
		{
			// x-span start reduced with respect of parents x-span.
			get
			{
				if(OwningSeries == null)
					return x0;
				double x0p = OwningSeries.ReducedX0;
				double x1p = OwningSeries.ReducedX1;
				return x0*x1p + (1-x0)*x0p;
			}
		}
		internal	double			ReducedX1
		{
			// x-span end reduced with respect of parents x-span.
			get
			{
				if(OwningSeries == null)
					return x1;
				double x0p = OwningSeries.ReducedX0;
				double x1p = OwningSeries.ReducedX1;
				return x1*x1p + (1-x1)*x0p;
			}
		}

		#endregion

		#region --- Target Area ---

		/// <summary>
		/// Gets the <see cref="TargetArea"/> of this <see cref="SeriesBase"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TargetArea TargetArea 
		{
			get 
			{
				// If this is root series, it has to have the tarhet area
				CompositeSeries ps = OwningSeries;
				if(ps==null)
				{
					if(targetArea == null)
					{
						targetArea = new TargetArea();
						targetArea.Series = this;
					}

					return targetArea;
				}
				// We take this.targetArea if the parent composite series has multiarea composition kind. 
				// Otherwise, we ignore own target area and search through the parent series.
				if(ps.CompositionKind == CompositionKind.MultiArea)
				{
					if(targetArea == null) // Parent series didn't create children's areas - force it to do so
						ps.CreateChildernTargetAreas();
					return targetArea;
				}
				else
					return ps.TargetArea;
			}
		}

		internal TargetArea OwnTargetArea { get { return targetArea; } set { targetArea = value; if(value != null) targetArea.Series = this; } }

		internal void SetTargetArea()
		{
			if(OwnTargetArea==null && OwningSeries!=null)
				return;
			TargetArea.SetCoordinates();
		}

		#endregion

		
		/// <summary>
		/// Gets or sets a value indicating whether area maps should be rendered for this <see cref="SeriesBase"/>.
		/// </summary>
		[SRDescription("SeriesRenderAreaMapDescr")]
		[DefaultValue(false)]
		public bool RenderAreaMap	
		{ 
			get { return renderAreaMap; } 
			set { renderAreaMap = value; }
		}

		internal bool ObjectTrackingEnabled 
		{ 
			get 
			{
				if(RenderAreaMap)
					return true;
				if(OwningSeries != null)
					return OwningSeries.ObjectTrackingEnabled;
				return OwningChart.ObjectTrackingEnabled;
			}
		}

		#region --- Coordinate System ---

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SeriesBase"/> object is logarithmic.
		/// </summary>
		[SRDescription("SeriesIsLogarithmicDescr")]
		[DefaultValue(false)]
		public virtual bool IsLogarithmic	
		{ 
			get { return isThisLogarithmic; } 
			set { isThisLogarithmic = value; }
		}

		/// <summary>
		/// The base of the log.
		/// </summary>
		/// <remarks>
		/// Use the <see cref="SeriesBase.IsLogarithmic"/> property to turn on the logarithmic scale.
		/// </remarks>
		[SRDescription("SeriesLogBaseDescr")]
		[DefaultValue(10)]
		public int LogBase	
		{ 
			get { return logBase; } 
			set { if(value>1) logBase = value; }
		}

		internal DataDimension GetOrCreateZDimension()
		{
			if(OwnCoordSystem != null && !OwnCoordSystem.IsEmbeded)
			{
				DataDimension zDDim = OwnCoordSystem.ZAxis.Dimension;
				if(zDDim != null)
				{
					EnumeratedDataDimension eZDDim = zDDim as EnumeratedDataDimension;
					if(eZDDim != null)
						SyncronizeZDimension(); // Synchronize dimension with series structure					
				}
				if(OwnCoordSystem.ZAxis.Dimension == null)
					OwnCoordSystem.ZAxis.SetDimension(new EnumeratedDataDimension(Name,typeof(string)));
				return OwnCoordSystem.ZAxis.Dimension;
			}
			else
				return OwningSeries.GetOrCreateZDimension();
		}

		internal void AddThisToZDimensionHierarchy()
		{
			EnumeratedDataDimension zDimension = GetOrCreateZDimension() as EnumeratedDataDimension;
			if(zDimension != null)
			{
				if(zDimension[Name] != null)
					return;
				Coordinate parent = null;
				if(OwningSeries != null)
					parent = zDimension[OwningSeries.Name];
				if(parent != null)
					parent.Add(Name);
			}
		}

		internal CoordinateSystem CreateCoordinateSystem()
		{
			CoordSystem = new CoordinateSystem();
			CoordSystem.HasChanged = false;
			CoordSystem.SetOwner(this);
			return CoordSystem;
		}

		internal Axis XAxis { get { return CoordSystem.XAxis; } }
		internal Axis YAxis { get { return CoordSystem.YAxis; } }
		internal Axis ZAxis { get { return CoordSystem.ZAxis; } }

		GenericType genericReference = new GenericType();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DefaultValue(typeof(GenericType),"")]
			/// <summary>
			/// Gets or sets the reference value of this <see cref="SeriesBase"/> object.
			/// </summary>
		public GenericType Reference
		{
			get 
			{
				return genericReference; 
			}
			set 
			{
				genericReference = value; 
				referenceValue = genericReference.InternalValue;
			}
		}

		private void HandleReferenceChanged(object genMinimum,EventArgs ea)
		{
			if(OwningChart==null || !OwningChart.InDesignMode)
			{
				referenceValue = genericReference.InternalValue;
			}
		}

		internal virtual void HandleMissingDataPoints() { } // should be overriden


		/// <summary>
		/// The reference point for this <see cref="SeriesBase"/> object.
		/// </summary>
		/// <remarks>
		/// This is used to determine where the bar starts from, for example.
		/// </remarks>
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object ReferenceValue
		{
			get 
			{ 	
				if(referenceValue == null)
				{
					object rv = null;
					if(OwningSeries != null && !HasIndependentYAxis)
						rv = OwningSeries.ReferenceValue;
					if(rv == null && YDimension != null)
						rv = YDimension.ReferenceValue;
					if(IsLogarithmic && (rv==null || !(rv is double) || ((double)rv <= 0.0)))
						return 1.0;
					return rv;
				}
				else
					return referenceValue;
			}
			set 
			{
				referenceValue = value; 
			} 
		}

		internal virtual void AdjustReferenceValuesToYRange(object yMinValue, object yMaxValue)
		{
			// Adjust reference value to be in the given y range
			object rv = ReferenceValue;
			if(yMinValue != null && YDimension.Compare(yMinValue,rv) > 0)
				ReferenceValue = yMinValue;
			else if(yMaxValue != null && YDimension.Compare(yMaxValue,rv) < 0)
				ReferenceValue = yMaxValue;
		}

		internal virtual void GetReferenceValuesRange(ref object minRef, ref object maxRef, bool useImpliedValues)
		{
			object rv;
			if(useImpliedValues)
				rv = ReferenceValue;
			else
				rv = RawReferenceValue;
			if(rv == null)
				return;

			if(minRef == null) // then maxRef == null too!
			{
				minRef = rv;
				maxRef = rv;
				return;
			}

			// NB: here rv, minRef and maxRef are != null !
			if(YDimension.Compare(rv,minRef) < 0)
				minRef = rv;
			if(YDimension.Coordinate(rv) + YDimension.Width(rv) > 
				YDimension.Coordinate(maxRef) + YDimension.Width(maxRef))
				maxRef = rv;
		}

		/// <summary>
		/// A property that indicates whether a reference value should be adjusted to the y-value range.
		/// </summary>
		[Description("Should reference value be adjusted to the y-value range")]
		[Category("General")]
		[DefaultValue(false)]
		public bool AdjustReferenceValue
		{
			get
			{
				if(adjustReferenceValue == BoolExt.NotSet && !OwningChart.InSerialization)
				{
					if(OwningSeries != null)
						return OwningSeries.AdjustReferenceValue;
					else
						return false;
				}
				else
					return (adjustReferenceValue==BoolExt.Yes);
			}
			set
			{
				adjustReferenceValue = (value? BoolExt.Yes:BoolExt.No);
			}
		}

		// The value defined at this series object or somewhere in the ancestor nodes
		internal object RawReferenceValue
		{
			get
			{
				if(referenceValue != null)
					return referenceValue;
				else if(OwningSeries != null)
					return OwningSeries.RawReferenceValue;
				else
					return null;
			}
		}



		internal object GetReferenceValue()
		{
			return referenceValue;
		}

		/// <summary>
		/// Gets or sets the <see cref="CoordinateSystem"/> of this <see cref="SeriesBase"/> object.
		/// </summary>
		[SRDescription("SeriesBaseCoordSystemDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public abstract CoordinateSystem CoordSystem { get; set; }
		
		/// <summary>
		/// Gets or sets the coordinate system object that is owned to this <see cref="SeriesBase"/> object.
		/// </summary>
		[DefaultValue(null)]
		[Browsable(false)]
		[NotifyParentProperty(true)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public CoordinateSystem OwnCoordSystem { get { return coordinateSystem; } set { coordinateSystem = value; if(value != null) coordinateSystem.SetOwner(this); } }

		internal void CreateCSIfSeriesHasIndependentCS()
		{
			if(OwnCoordSystem == null && HasIndependentYAxis)
			{
				OwnCoordSystem = new CoordinateSystem();
				OwnCoordSystem.SetEmbeded(true);
			}
		}
		#endregion

		#region --- Coordinates ---

		// DCS - Coordinates

		internal abstract object MinXDCS();
		internal abstract object MaxXDCS();
		internal abstract object MinYDCS();
		internal abstract object MaxYDCS();
		internal abstract object MinZDCS();
		internal abstract object MaxZDCS();
		internal abstract object MinDCS(string param);
		internal abstract object MaxDCS(string param);

		// LCS - Coordinates

		internal abstract double MinXLCS();
		internal abstract double MaxXLCS();
		internal abstract double MinYLCS();
		internal abstract double MaxYLCS();
		internal abstract double MinZLCS();
		internal abstract double MaxZLCS();
		internal abstract double MinLCS(string param);
		internal abstract double MaxLCS(string param);

		// ICS Coordinates

		// Sizes
		internal double DXICS { get { return dXICS; } set { dXICS = value; } }
		internal double DYICS { get { return dYICS; } set { dYICS = value; } }
		internal double DZICS { get { return dZICS; } set { dZICS = value; } }
		
		/// <summary>
		/// Gets or sets the width of this <see cref="SeriesBase"/> object along the z-axis in Intermediate Coordinate System.
		/// </summary>
		[DefaultValue(10.0)]
		public   double Depth { get { return dZICS; } set { dZICS = value; } }

		// Offsets. Note that these are offsets in the parent coordinate system and may not
		// be colinear with this system orientation
		[DefaultValue(0.0)]
		internal double OffsetXICS	{ get { return offsetXICS; } set { offsetXICS = value; } }
		[DefaultValue(0.0)]
		internal double OffsetYICS	{ get { return offsetYICS; } set { offsetYICS = value; } }
		[DefaultValue(0.0)]
		internal double OffsetZICS	{ get { return offsetZICS; } set { offsetZICS = value; } }
		internal double TotalOffsetZICS	
		{ 
			get 
			{ 
				if(OwningSeries != null)
					return OwningSeries.TotalOffsetZICS + offsetZICS;
				else
					return offsetZICS; 
			}
		}

		internal double EffectiveYByXRatio
		{
			get 
			{
				double r = TargetArea.GetEffectiveYXRatio();
				if(CoordSystem.Orientation == CoordinateSystemOrientation.Horizontal)
						return 1.0/r;
					else if(CoordSystem.Orientation == CoordinateSystemOrientation.Default)
						return r;
					else
						return 1.0;
			}
		}
		#endregion

		#region --- NamedStyle Interface ---

		internal NamedStyleInternal m_nsi = new NamedStyleInternal();

		/// <summary>
		/// Gets the parent collection of this <see cref="SeriesBase"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual NamedCollection OwningCollection 
		{
			get {return m_nsi.NamedCollection;}
			set {m_nsi.NamedCollection = value;}
		}
		/// <summary>
		/// Gets or sets the name of this <see cref="SeriesBase"/> object.
		/// </summary>
		[Category("General")]
		[SRDescription("SeriesBaseNameDescr")]
		[NotifyParentProperty(true)]
		public string Name 
		{ 
			get { return m_nsi.Name; } 
			set 
			{
                if (m_nsi.Name != value)
                {
                    if (m_nsi.Name != "" && value != "")
                    {
                        if (OwningChart != null &&
                            (OwningChart.Series.FindSeries(value) != null || OwningChart.Series.FindCompositeSeries(value) != null))
                            throw new ArgumentException("Another series has name '" + value + "'. Series names must be unique.");
                        if (CoordSystem != null && CoordSystem.ZAxis.Dimension != null)
                            CoordSystem.ZAxis.Dimension.Rename(m_nsi.Name, value);
                        if (OwningChart != null)
                            OwningChart.DataProvider.RenameSeries(m_nsi.Name, value);
                    }
                    m_nsi.Name = value;
                }
			}
		}
		#endregion

		internal abstract void RegisterVariables();
		internal abstract void AdjustXCoordinate();
		internal virtual void FillLegend(Legend legend) { }
		internal virtual void SyncronizeZDimension() { }
		internal abstract int NumberOfSimpleSeriesBefore(Series s, out bool found); 
		internal abstract Series[] SimpleSubseriesList { get; }
		internal abstract bool HasBarSeries();

		internal abstract void BindParameters();
		internal virtual void DataBind()
		{
			if(OwningChart.InitializeOnDataBind)
			{
				cashedStyleName = null;
			}
			if(HasIndependentYAxis)
			{
				if(OwnCoordSystem == null)
					OwnCoordSystem = new CoordinateSystem();
				OwnCoordSystem.SetEmbeded(true);
			}
		}
		internal abstract void WCSRange(out double x0, out double y0, out double z0, out double x1, out double y1, out double z1);
		internal abstract bool IsEmpty();
		internal abstract void RenderTitles();
		internal abstract void ComputeDefaultICSSize();

		#region --- Rendering ---
		internal abstract void Render(bool toSetupArea);

		internal override void Render()
		{
			Render(true);
		}

		internal virtual void RenderLegends()
		{
			if(OwnTargetArea != null)
				OwnTargetArea.RenderLegend();
		}

		internal void PushTargetArea()
		{
			if(OwnTargetArea != null)
			{
				TargetAreaBox tab = new TargetAreaBox();
				GE.Add(tab);
				GE.Push(tab);
				tab.Tag = OwnTargetArea;
			}
		}

		internal void PushCoordinateSystem()
		{
			if(OwnCoordSystem != null)
			{
				if(Style.IsRadar)
					GE.Push(new RadarBox());
				else if(Style.ChartKind == ChartKind.Pie || Style.ChartKind == ChartKind.Doughnut)
					GE.Push(new PieDoughnutBox());
				else
				{
					CoordinateSystemBox csb = new CoordinateSystemBox();
					csb.Tag = OwnCoordSystem;
					GE.Push(csb);
				}
			}
		}

		internal void PopTargetArea()
		{
			if(OwnTargetArea != null)
				GE.Pop(typeof(TargetAreaBox));				
		}

		internal void PopCoordinateSystem()
		{
			if(OwnCoordSystem != null)
				GE.Pop(typeof(ChartBox));
		}

		#endregion

		internal bool InVisibleParent
		{
			get
			{
				if(OwningSeries == null)
					return true;
				else
					return OwningSeries.Visible && OwningSeries.InVisibleParent;
			}
		}

		#region --- Size and Position ---

		internal double ICoordinateZ
		{
			get
			{
				double icz = ZAxis.ICoordinate(Name);
				return icz;
			}
		}

		internal double IWidthZ
		{
			get
			{
				double width = ZAxis.IWidth(Name);
				return width;
			}
		}

		internal abstract void ComputeSize();

		#endregion

		#region --- Style ---
		/// <summary>
		/// Gets or sets the style name to be used with this <see cref="SeriesBase"/> object.
		/// </summary>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
#if !__BUILDING_CRI__
		[Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor))]
#endif
		[DefaultValue(defaultStyleName)]
		[SRDescription("SeriesBaseStyleNameDescr")]
		public virtual string StyleName		
		{ 
			get 
			{ 
				if(styleName == "" || styleName == null)
				{
					if(OwningChart.InSerialization)
						return defaultStyleName;
					if(OwningSeries != null)
						return OwningSeries.StyleName;
					else return defaultStyleName;
				}
				return styleName; 
			} 
			set 
			{ 
				styleName = value; 
			}
		}

		/// <summary>
		/// The series style kind. 
		/// </summary>
		/// <remarks>
		/// This property is used to get or set the series style when the style is one of the predefined series styles.
		/// Using the value <see cref="SeriesStyleKind.Custom"/> is wrong, unless there is a user created style named "Custom".
		/// For all user created styles, this property gets the value <see cref="SeriesStyleKind.Custom"/>.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SeriesStyleKind StyleKind
		{
			get { return SeriesStyle.StyleKindOf(StyleName); }
			set { StyleName = value.ToString(); }
		}


		#region --- Missing data point handling ---
		/// <summary>
		/// Gets or sets the missing points style name to be used with this <see cref="SeriesBase"/> object.
		/// </summary>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
#if !__BUILDING_CRI__
		[Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor))]
#endif
		[DefaultValue("")]
		[Category("Missing Points")]
		[SRDescription("SeriesBaseMissingPointStyleNameDescr")]
		public virtual string MissingPointsStyleName		
		{ 
			get 
			{ 
				return missingPointsStyleName;
			} 
			set 
			{ 
				missingPointsStyleName = value; 
			}
		}

		/// <summary>
		/// The missing points style kind to be used with this <see cref="SeriesBase"/> object.
		/// This property can be used when the missing points style is one of the predefined styles.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SeriesStyleKind MissingPointsStyleKind 
		{
			get { return SeriesStyle.StyleKindOf(MissingPointsStyleName); }
			set { MissingPointsStyleName = SeriesStyle.StyleNameOf(value); }
		}

		internal string EffectiveMissingPointsStyleName
		{
			get
			{
				string r = GetMissingPointsStyleNameRecursive();
				if(r == null)
					return "DefaultMissingPointsStyle";
				else
					return r;
			}
		}

		/// <summary>
        /// Missing point handler kind.
		/// </summary>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[DefaultValue(MissingPointHandlerKind.Auto)]			
		[Description("Missing data point handling kind")]
		public MissingPointHandlerKind MissingPointHandlerKind 
		{
			get { return missingPointHandlerKind; }
			set { missingPointHandlerKind = value; }
		}

		/// <summary>
		/// Sets custom missing point handler.
		/// </summary>
		/// <param name="mph"></param>
		public void SetCustomMissingPointHandler(MissingPointHandler mph)
		{
			customMissingPointHandler = mph;
			missingPointHandlerKind = MissingPointHandlerKind.Custom;
		}

		private string GetMissingPointsStyleNameRecursive()
		{
			if(missingPointsStyleName != null && missingPointsStyleName != "")
				return missingPointsStyleName;
			else if(OwningSeries != null)
				return OwningSeries.GetMissingPointsStyleNameRecursive();
			else
				return null;
		}

		internal MissingPointHandler GetEffectiveMissingPointHandler()
		{
			MissingPointHandler mph = null;
			switch(missingPointHandlerKind)
			{
				case MissingPointHandlerKind.Auto:
					if(OwningSeries != null)
						return OwningSeries.GetEffectiveMissingPointHandler();
					break;
				case MissingPointHandlerKind.Custom:
				{
					mph = customMissingPointHandler;
					SeriesBase sb = OwningSeries;
					while(mph == null && sb != null)
					{
						mph = sb.customMissingPointHandler;
						sb = sb.OwningSeries;
					}
					return mph;
				}
				default:
					return MissingPointHandler.GetHandlerByKind(missingPointHandlerKind);
			}
			return null;
		}

		#endregion

			/// <summary>
			/// Gets the <see cref="SeriesStyle"/> object used with this <see cref="SeriesBase"/> object.
			/// </summary>
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
			[Browsable(false)]
			public SeriesStyle Style			
		{ // Method based on cashed style to avoid frequent search through chart styles dictionary.
			get
			{
				if(StyleName != cashedStyleName || cashedStyle==null || OwningChart.InDesignMode)
				{
					cashedStyleName = StyleName;
					cashedStyle = OwningChart.GetPresentationStyle(StyleName);
					if(cashedStyle == null)
						throw new Exception("SeriesStyle '" + StyleName + "' does not exist.");
				}
				return cashedStyle; 
			}
		}

		internal virtual void SetStyleNameToChildren(string name) { }

		#endregion
 
		#region --- Coordinates Consolidation ---

		internal abstract void BindDimensions();
		internal abstract void PropagateDimensions(DataDimension xDim, DataDimension yDim);

		private struct sms
		{
			public string	s;	   // string
			public int		count; // counts the number of elements that preceede this one
		}
		private string[] MergedStringCoordinates(string[] coord1, string[] coord2)
		{
			string[] order;
			
			// Try alphanumeric sort
			if(AlphanumericSort(coord1,coord2,out order))
				return order;

			// Merging two string sequences while preserving order in both,
			// at least as much as possible

			// Populate the working structure
			sms[] ws = new sms[coord1.Length+coord2.Length];
			int i,j,n;
			for(i=0;i<coord1.Length;i++)
			{
				ws[i].s = coord1[i];
				ws[i].count = i;
			}
			n = coord1.Length;
			for(i=0;i<coord2.Length;i++)
			{
				bool found = false;
				for(j=0;j<n;j++)
				{
					if(ws[j].s == coord2[i])
					{
						ws[j].count = Math.Max(ws[j].count,i);
						found = true;
						break;
					}
				}
				if(!found)
				{
					ws[n].s = coord2[i];
					ws[n].count = i;
					n++;
				}
			}

			// Iterate adjusting

			return null;
		}

		static private bool AlphanumericSort(string[] coord1, string[] coord2, out string[] order)
		{
			int i,j;
			bool ascending = true;
			bool descending = true;

			order = null;
			for(i=1; i<coord1.Length; i++)
			{
				if(coord1[i].CompareTo(coord1[i-1])<0)
					descending = false;
				else if(coord1[i].CompareTo(coord1[i-1])>0)
					ascending = false;
				if(!ascending && !descending)
					return false;
			}
			for(i=1; i<coord2.Length; i++)
			{
				if(coord2[i].CompareTo(coord2[i-1])<0)
					descending = false;
				else if(coord2[i].CompareTo(coord2[i-1])>0)
					ascending = false;
				if(!ascending && !descending)
					return false;				
			}

			// Merge and sort
			int n = coord1.Length + coord2.Length;
			string[] ws = new string[n];

			for(i=0; i<coord1.Length; i++)
				ws[i] = coord1[i];
			for(i=0; i<coord2.Length; i++)
				ws[i+coord1.Length] = coord2[i];
			for(i=0;i<n-1;i++)
				for(j=i+1;j<n;j++)
				{
					if(ascending && ws[i].CompareTo(ws[j])>0 || descending && ws[i].CompareTo(ws[j])<0)
					{
						string s = ws[i];
						ws[i] = ws[j];
						ws[j] = s;
					}
				}

			// Remove duplicates
			int m = 0;
			for(i=1; i<n; i++)
			{
				if(ws[i] != ws[m])
				{
					m++;
					ws[m] = ws[i];
				}
			}
			m++;
			order = new string[m];
			for(i=0; i<m; i++)
				order[i] = ws[i];
			return true;
		}
		#endregion
		#region --- Serialization & Browsing Control ---

		private bool ShouldSerializeReferenceValue()	{ return referenceValue != null ; }
		private bool ShouldSerializeStyleName	  ()	{ return styleName != "" ; }
		private bool ShouldSerializeName		  ()	{ return true; } 
		#endregion
	}
}
