using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting
{
    /// <summary>
    ///     Represents a series of data points.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///		Instances of the <see cref="Series"/> class are leaves in series hierarchy. This class provides access
    ///		to individual data points through the <see cref="Series.DataPoints"/> property. The value of this 
    ///		property is a <see cref="DataPointCollection"/> containing individual data points. 
    ///		This collection is created during the "DataBind()" operation and therefore
    ///		it is not available before the control completes the "DataBind()" operation. For a detailed
    ///		description of data binding see topics "Data Binding" in "Basic Concepts"
    ///		and "Advanced Data Binding" in "Advanced Concepts".
    /// </para>
    /// <para>
    ///     <see cref="Series.CreateAnnotation"/> can be used to create data point annotations. This function
    ///     can be used multiple times, so that data points may have multiple annotations
    ///     stored in <see cref="SeriesLabelsCollection"/> property <see cref="Series.Labels"/>.
    /// </para>
    /// <para>
    ///     The <see cref="Series.StyleName"/> property can be set to override the chart style inherited
    ///     from the parent node in the series hierarchy. 
    /// </para>
    /// </remarks>
    // (ED)
    [TypeConverter(typeof(GenericExpandableObjectConverter))]
	[NotIncludedInTemplate]
	public sealed class Series : SeriesBase,ILegendItemProvider
    {
		internal const string temporaryGradientStyleName = "<<TemporaryGradientStyleName>>";

		private const int			defaultNumberOfPoints = 6;
        private DataDescriptorCollection dataDescriptors;
        private DataPointCollection dataPoints;
        private DataPointCollection nonMissingDataPoints;
        private int numberOfPoints = defaultNumberOfPoints;
		private bool				dataBindingValid;
        private bool                isRange = false;

		private string				legendText = "";
		private bool				addToLegend = true;

		// Z Coordinate of points in this series
		private bool				dzSetForPieDoughnut;

		// X,Y extremes and scatter point width
		private object				minXDCS,maxXDCS, minYDCS,maxYDCS;
		private double				scatterXPointSize, scatterYPointSize;

		// Scatter points size data
		private double scaterPointSize;
		private bool autoScatteredPointSize = true;

		// Marker size in points
		private double markerSizePts = 0;

		private	const double defaultDepth = 10.0; 

		// Transparency
		private double				transparency = 0.0; // Opaque, by default

		// Dimension objects
		private DataDimension		xDimension = null;
		private DataDimension		yDimension = null;

		// WOrking data
		private LineKind			styleLowerBoundLineKind;
		private LineKind			styleUpperBoundLineKind;
		
		#region --- Navigation ---

		internal override SeriesBase FirstChild { get { return null; } }

		internal override SeriesBase NextTo(SeriesBase s) { return null; }

		internal override SeriesBase PreviousOf(SeriesBase s) { return null; }

		#endregion

		#region --- Coordinates ---

		// DCS - Coordinates

		internal override object MinXDCS()	
		{
			if(Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
				return 0.0;
			return minXDCS; 
		}

		internal override object MaxXDCS()	
		{
			if(Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
				return Style.Radius*2;
			return maxXDCS; 
		}

		internal override object MinYDCS()
		{
			if(Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
				return 0.0;

			object val = null;
			double dval = double.MaxValue;
			DataDimension yDim = YDimension;
			if(RawReferenceValue != null) // TODO Raw or no Raw?
			{
				val = ReferenceValue;
				dval = yDim.Coordinate(ReferenceValue);
			}
			if(minYDCS != null && dval < yDim.Coordinate(minYDCS))
				return val;
			else
				return minYDCS;

		}

		internal override object MaxYDCS() 
		{
			if(Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
			{
				double v = 0.0;
				for(int i=0;i<NonMissingDataPoints.Count;i++)
				{
                    double curVal = this.NonMissingDataPoints[i].EffectiveHeight + this.NonMissingDataPoints[i].EffectiveLift;
					v = Math.Max(v,curVal);
				}
				return v;
			}
			
			object val = null;
			double dval = double.MinValue;
			DataDimension yDim = YDimension;
			if(ReferenceValue != null)
			{
				val = ReferenceValue;
				dval = yDim.Coordinate(ReferenceValue);
			}
			if(maxYDCS != null && dval > yDim.Coordinate(maxYDCS))
				return val;
			else
				return maxYDCS;
		}

		internal override object MinZDCS()
		{
			return Name;
		}

		internal override object MaxZDCS()
		{
			return Name;
		}

		internal override object MinDCS(string param)
		{
			if(numberOfPoints == 0)
				return null;
            DataPointCollection dataPoints = this.NonMissingDataPoints;
            if (dataPoints.Count == 0)
                return null;
			object val = dataPoints[0].DCS(param);
			double dval = dataPoints[0].MinLCS(param);
			for(int i=1; i<dataPoints.Count; i++)
			{
				object v = dataPoints[i].DCS(param);
				double d = dataPoints[i].MinLCS(param);
				if(d < dval)
				{
					dval = d;
					val = v;
				}
			}
			return val;
		}

		internal override object MaxDCS(string param)
		{
			if(numberOfPoints == 0)
				return null;
            DataPointCollection dataPoints = this.NonMissingDataPoints;
            if (dataPoints.Count == 0)
                return null;
            object val = dataPoints[0].DCS(param);
			double dval = dataPoints[0].MaxLCS(param);
			for(int i=1; i<dataPoints.Count; i++)
			{
				object v = dataPoints[i].DCS(param);
				double d = dataPoints[i].MaxLCS(param);
				if(d > dval)
				{
					dval = d;
					val = v;
				}
			}
			return val;
		}

		// LCS - Coordinates
		internal override double MinXLCS()
		{
			if(numberOfPoints == 0)
				return double.MaxValue;
			return MinLCS("x");
		}

		internal override double MaxXLCS()
		{
			if(numberOfPoints == 0)
				return double.MinValue;
			return MaxLCS("x");
		}

		internal override double MinYLCS()
		{
			if(numberOfPoints == 0)
				return double.MaxValue;
			object val = MinYDCS();
			if(val == null)
				return double.NaN;
			else
				return YDimension.Coordinate(val);
		}

		internal override double MaxYLCS()
		{
			if(numberOfPoints == 0)
				return double.MinValue;
			object val = MaxYDCS();
			if(val == null)
				return double.NaN;
			else
				return YDimension.Coordinate(val) + YDimension.Width(val);
		}

		internal override double MinZLCS()
		{
			if(numberOfPoints == 0)
				return double.MaxValue;
			
			return CoordSystem.ZAxis.LCoordinate(ZCoordinate);
		}

		internal override double MaxZLCS()
		{
			if(numberOfPoints == 0)
				return double.MinValue;
			
			return CoordSystem.ZAxis.LCoordinate(ZCoordinate)+CoordSystem.ZAxis.Dimension.Width(ZCoordinate);
		}

		internal override double MinLCS(string param)
		{			
			if(NonMissingDataPoints.Count == 0)
				return double.MaxValue;
            DataPointCollection dataPoints = this.NonMissingDataPoints;
            double dval = dataPoints[0].MinLCS(param);
			for(int i=1; i<dataPoints.Count; i++)
				dval = Math.Min(dval,this.dataPoints[i].MinLCS(param));
			return dval;
		}

		internal override double MaxLCS(string param)
		{
			if(numberOfPoints == 0)
				return double.MinValue;
            DataPointCollection dataPoints = this.NonMissingDataPoints;
            if (dataPoints.Count == 0)
                return double.MinValue;
            double dval = dataPoints[0].MaxLCS(param);
			for(int i=1; i<dataPoints.Count; i++)
				dval = Math.Max(dval,dataPoints[i].MaxLCS(param));
			return dval;
		}
		
		internal override void ComputeSize()
		{
			// We keep default ICS Size computed in parameters binding,
			// unless the size has been changed externally
		}

		#endregion

        #region --- Construction and Setup ---

		/// <summary>
		/// Initializes a new instance of the <see cref="Series"/> class. 
		/// </summary>
		/// <param name="name">The name of the new <see cref="Series"/> object.</param>
        public Series(string name) : base(name) 
        { 
            dataDescriptors = new DataDescriptorCollection(this);
            dataPoints = new DataPointCollection();
            nonMissingDataPoints = new DataPointCollection();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Series"/> class with default parameters.
		/// </summary>
		public Series() : this("") {}

		#endregion

		#region --- Legend ---
		
		// ILegendItemProvider interface implementation
		bool ILegendItemProvider.LegendItemVisible
		{ 
			get
			{
				if(!addToLegend)
					return false;
				if(Style.ChartKind == ChartKind.Pie || Style.ChartKind == ChartKind.Doughnut)
				{
					DataDescriptor legendDes = dataDescriptors["legend"];
					if(legendDes == null)
						return false;
				}
				return true; 
			}
		}

		double ILegendItemProvider.LegendItemCharacteristicValue
		{
			get
			{
				double sum = 0;
				foreach(DataPoint dp in DataPoints)
					sum += (dp as ILegendItemProvider).LegendItemCharacteristicValue;
				return sum;
			}
		}

		LegendItemKind ILegendItemProvider.LegendItemKind
		{
			get
			{
				if(Style.IsLinear)
					return LegendItemKind.RectangleItem;
				else if(Style.IsLine)
					return LegendItemKind.LineItem;
				else
					return LegendItemKind.MarkerItem;
			}
		}
		
		string ILegendItemProvider.LegendItemText
		{
			get
			{
				return LegendText; 
			}
		}
		
		void ILegendItemProvider.DrawLegendItemRectangle	(Graphics g, Rectangle rect)
		{
			if(Style.IsLinear)
			{
				ChartColor surf = EffectiveSurface;
				if(surf == null)
					return;
				Color color = surf.Color;
				
				// Check data point colors; they may override the series color
				// If all data point colors are the same, the data point color will be used
				Color dpColor = Color.Empty;
				bool allTheSame = true;
				for (int i = 0; i < dataPoints.Count; i++)
				{
					DataPoint dp = dataPoints[i];
					Color col;
					if (dp.ColorProperty("color", out col))
					{
						if (dpColor.IsEmpty)
							dpColor = col;
						else
						{
							if (dpColor != col)
							{
								allTheSame = false;
								break;
							}
						}
					}
				}
				if (allTheSame && !dpColor.IsEmpty)
					color = dpColor;
				
				Brush brush = new SolidBrush(color);
				g.FillRectangle(brush,rect);
				brush.Dispose();
			}
		}

		internal override void FillLegend(Legend legend) 
		{
			if(Style.IsLinear)
				legend.Add(this);
			if(Style.ChartKind == ChartKind.Pie || Style.ChartKind == ChartKind.Doughnut)
			{
				if(AddToLegend)
				{
					for(int i=0;i<NonMissingDataPoints.Count;i++)
					{
                        legend.Add(this.NonMissingDataPoints[i]);
					}
				}
			}
		}

		#endregion

		#region --- Properties ---

		#region --- Browsable Properties ---

		#region --- Category "General"
		
		/// <summary>
		/// Gets or sets the style name to be used with this <see cref="Series"/> object.
		/// </summary>
		[Category("General")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[System.ComponentModel.TypeConverter(typeof(SelectedSeriesStyleConverter))]
		public override string StyleName { get { return base.StyleName; } set { base.StyleName = value; } }	

		[Category("General")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
#if !__BUILDING_CRI__
        [Editor(typeof(NamedConstCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		internal DataDescriptorCollection    DataDescriptors			    
		{
			get 
			{
				// We return filtered collection if we are in serialization
				if(OwningChart != null && OwningChart.InSerialization)
				{
					DataDescriptorCollection C = new DataDescriptorCollection(this);
					foreach(DataDescriptor D in dataDescriptors)
						if(D.HasChanged)
							C.Add(D); 
					return C;
				}
				else
					return dataDescriptors; 
			} 
		}

		/// <summary>
		/// Gets or sets marker size in points.
		/// </summary>
		/// <remarks>
		/// When this value is default (zero) the width of markers is 2/3 of distance between data points along x-axis.
		/// In some cases, especially scattered data points, the default size might not appropriate and this property 
		/// could be used to override it.
		/// </remarks>
		[DefaultValue(0)]
		[Category("General")]
		[Description("Marker size in points.")]
		public double MarkerSizePts { get { return markerSizePts; } set { markerSizePts = value; } }

		/// <summary>
		/// Gets or sets the value indicating if this <see cref="Series"/> object is a range series.
		/// </summary>
		[DefaultValue(false)]
		[Category("General")]
		[Description("Determines whether this series is a range series. ")]
		public bool			IsRange					{ get { return isRange; } set { isRange = value; } }

		//Fixme: Design time only?

		/// <summary>
		/// Gets or sets the number of points used in design time data simulation.
		/// </summary>
		[Category("General")]
		[Description("Number of points used in design time data simulation")]
		[DefaultValue(defaultNumberOfPoints)]
		[Browsable(false)]
		public int NumberOfPoints
		{
			get { return numberOfPoints; } 
			set { numberOfPoints = value; }			
		}
		#endregion

		#region --- Category "Legend"

		/// <summary>
		/// Gets or sets the text to be displayed in the legend for this <see cref="Series"/> object.
		/// </summary>
		[Category("Legend")]
		[ DefaultValue("")]
		public string LegendText	
		{
			get 
			{
				if(legendText=="")
				{
                    if (OwningChart == null || OwningChart.InSerialization)
                        return "";
                    else
						return GetEffectiveLabel();
				}
				else
					return legendText;  
			} 
			set { legendText = value; } 
		}

		/// <summary>
		/// Gets or sets a value that indicates whether this <see cref="Series"/> object should be added to the legend of the chart.
		/// </summary>
		[Category("Legend")]
		[DefaultValue(true)]
		public bool AddToLegend		{ get { return addToLegend; } set { addToLegend = value; } }

		#endregion

		#endregion

		/// <summary>
		/// Gets the Z-coodrinate in Data Coordinate System (DCS) for all points in this <see cref="Series"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object		ZCoordinate				{ get { return Name; } }

		internal DataPointCollection NonMissingDataPoints
		{
            get { return this.nonMissingDataPoints; } 
		}

			/// <summary>
			/// Gets the <see cref="DataPointCollection"/> object that contains <see cref="DataPoint"/>s assiciated with this <see cref="Series"/> object.
			/// </summary>
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[NotifyParentProperty(true)]
			public	  DataPointCollection  DataPoints 
		{
			get { return this.dataPoints; } 
		}


		[DefaultValue(0.0)]
		internal double InternalTransparency { get { return transparency; } set { transparency = Math.Max(0, Math.Min(1,value)); } }

		internal override bool IsEmpty() { return !dataBindingValid; }

		/// <summary>
		/// Gets or sets transparency of this <see cref="Series"/> object. Possible values are between 0 and 100.
		/// </summary>
		[DefaultValue(0.0)]
		[Description("The level of transparency for individual series.")]
		public double Transparency 
		{
			get 
			{
				return transparency * 100; 
			}
			set 
			{
				transparency = Math.Max(0, Math.Min(1,value/100.0)); 
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="CoordinateSystem"/> associated with this <see cref="Series"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]		
		public override CoordinateSystem        CoordSystem             
		{
			get 
			{
				CreateCSIfSeriesHasIndependentCS();
				if(OwnCoordSystem != null 
					&& OwningSeries.CompositionKind != CompositionKind.Stacked 
					&& OwningSeries.CompositionKind != CompositionKind.Stacked100 )
					return OwnCoordSystem;
				return OwningSeries.CoordSystem; 
			}
			set { OwnCoordSystem = value; } 
		}
		
		// --- Internal Properties ---

		internal double ReferenceValueNumeric
		{
			get 
			{ 
				if(ReferenceValue != null)
					return YDimension.Coordinate(ReferenceValue);
				else
					return 0.0;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates the reference value of this <see cref="Series"/> object.
		/// </summary>
		[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object ReferenceValue
		{
			get 
			{ 
				if(base.ReferenceValue == null)
				{
					object rv = YAxis.MinValue;
					if(rv == null)
					{
						if(YAxis.IsLogarithmic && YDimension is NumericDataDimension)
						{
							// Find the minimum and adjust reference value
							double min = double.MaxValue;
							foreach(DataPoint dp in dataPoints)
								min = Math.Min(min,(double)(dp.MinYDCS()));

							if(min > 0)
							{
								min = Math.Pow(Math.Floor(Math.Log(min,YAxis.LogBase)),YAxis.LogBase);
								rv = min;
							}

						}
						if(rv == null)
							rv = YDimension.ReferenceValue;
					}
					return rv;
				}
				else
					return base.ReferenceValue;
			}
			set
			{
				base.ReferenceValue = value;
			}
		}

        internal DrawingBoard CreateDrawingBoard()
		{
			// Give a small push to the relative front space and relative back space to get
			// areas that don't share the same, but slightly different z-coordinate
			double rfs = Style.RelativeFrontSpace;
			double rbs = Style.RelativeBackSpace;
			double displacement = 0;
			if(OwningSeries.CompositionKind == CompositionKind.Merged)
			{
				CompositeSeries parent = OwningSeries;
				int n = parent.SubSeries.Count;
				int ix = parent.GetSequenceNumber(this);
				displacement = ((double)(n-ix))/n*0.01;
			}
            DrawingBoard drawingBoard = OwningChart.GeometricEngine.CreateDrawingBoard();// new Geometry.DrawingBoard();
			double zPos = (rbs - rfs + 1)*0.5-displacement;
			double z0ICS = ICoordinateZ;
			drawingBoard.V0 = CoordSystem.ICS2WCS(new Vector3D(0,0,z0ICS+zPos*IWidthZ));
			drawingBoard.Vx = CoordSystem.ICS2WCS(new Vector3D(CoordSystem.XAxis.MaxValueICS,0,z0ICS+zPos*IWidthZ)) - drawingBoard.V0;
			drawingBoard.Vy = CoordSystem.ICS2WCS(new Vector3D(0,CoordSystem.YAxis.MaxValueICS,z0ICS+zPos*IWidthZ)) - drawingBoard.V0;
			drawingBoard.Reflection = 0.5;
			drawingBoard.LogPhong = 6;
			return drawingBoard;
		}
		
		internal override DataDimension XDimension
		{
			get
			{
				return xDimension;
			}
		}

		internal override DataDimension YDimension
		{
			get
			{				
				return yDimension;
			}
		}

		internal override DataDimension ZDimension
		{
			get
			{
				return OwningSeries.ZDimension; 
			}
		}

		internal override int TotalNumberOfDataPoints  { get { return DataPoints.Count; } }

		public override string ToString()
		{
			return "Series '"+Name+"'";
		}

		#endregion

		#region --- Size and Position ---

		internal double ScatterPointSize 
		{
			get 
			{
				return scaterPointSize;
				// we don't want the scatter point size to be too small:
				double minSPS = ZDimension.Width(Name)/10;
				minSPS = ZAxis.LCS2ICS(minSPS) - ZAxis.LCS2ICS(0);
				return Math.Max(scaterPointSize,minSPS); 
			}
		}

		internal DataPoint DataPointAt(object xCoordinate)
		{
			for(int i=0; i< DataPoints.Count; i++)
			{
				DataPoint dp = this.DataPoints[i];
				if(dp.Parameters["x"] == xCoordinate)
					return dp;
			}
			return null;
		}

		#endregion

		#region --- Annotations ---

		internal SeriesLabels CreateAnnotation(string labelExpression, DataPointLabelStyle dataPointLabelStyle)
		{
			SeriesLabels annotation = new SeriesLabels();
			Labels.Add(annotation);
			annotation.LabelExpression = labelExpression;
			if(dataPointLabelStyle != null)
				annotation.LabelStyle = dataPointLabelStyle;
			if(DataPoints != null && DataPoints.Count > 0)
				annotation.DataBind();
			return annotation;
		}


		/// <summary>
		/// Creates a <see cref="SeriesLabels"/> object with the specified label expression and label style name and adds it to the <see cref="Labels"/> colletion.
		/// </summary>
		/// <param name="labelExpression">The label expression to be used in the labels created.</param>
		/// <param name="labelStyleName">The data point style name to be used in the labels.</param>
		/// <returns>The newly created <see cref="SeriesLabels"/> object.</returns>
		public SeriesLabels CreateAnnotation(string labelExpression,string labelStyleName)
		{
			SeriesLabels annotation = new SeriesLabels();
			Labels.Add(annotation);
			annotation.LabelExpression = labelExpression;
			annotation.LabelStyleName = labelStyleName;
			if(DataPoints != null && DataPoints.Count > 0)
				annotation.DataBind();
			return annotation;
		}

		/// <summary>
		/// Creates a <see cref="SeriesLabels"/> object with the specified label expression and label style and adds it to the <see cref="Labels"/> colletion.
		/// </summary>
		/// <param name="labelExpression">The label expression to be used in the labels created.</param>
		/// <param name="labelStyleKind"><see cref="DataPointLabelStyleKind"/> to be used in the labels.</param>
		/// <returns>The newly created <see cref="SeriesLabels"/> object.</returns>
		public SeriesLabels CreateAnnotation(string labelExpression,DataPointLabelStyleKind labelStyleKind)
		{
			return CreateAnnotation(labelExpression,labelStyleKind.ToString());
		}

		/// <summary>
		/// Creates a default <see cref="SeriesLabels"/> object and adds it to the <see cref="Labels"/> colletion.
		/// </summary>
		/// <returns>The newly created <see cref="SeriesLabels"/> object.</returns>
		public SeriesLabels CreateAnnotation()
		{
			SeriesLabels annotation = new SeriesLabels();
			Labels.Add(annotation);
			if(DataPoints != null && DataPoints.Count > 0)
				annotation.DataBind();
			return annotation;
		}

		#endregion

		#region --- Data Binding ---

		internal override void AdjustXCoordinate()
		{
			Debug.WriteLine(Name + " adjusting x to " + X0.ToString("0.00 - ") + X1.ToString("0.00"));
			foreach(DataPoint p in dataPoints)
				p.AdjustXCoordinate(X0,X1);
		}

		internal override Series[] SimpleSubseriesList 
		{ 
			get
			{
				return new Series[] { this };
			}
		}
		private static string[] yParamNames = new string[]
			{
				"y",
				"from",
				"to",
				"open",
				"close",
				"High",
				"low"
			};

		internal override void BindDimensions()
		{
			// Bind to live database if possible

			if((OwningChart.InDesignMode || OwningChart.InitializeOnDataBind) &&
				OwningChart.DataSource != null && OwningChart.DataSource.ToString() != "" &&
				OwningChart.DataProvider.InputVariables[Name + ":x"] == null)
				OwningChart.DataProvider.DefaultDataBindingToDatabase(this);

			RegisterVariables();

			if(OwningChart.DataProvider.SeriesNameInInputData && OwningChart.InDesignMode)
			{
				InputVariableCollection inVars = OwningChart.DataProvider.InputVariables;
				InputVariable varSeries = inVars["series"];
				varSeries.VariableKind = InputVariableKind.SeriesName;
				varSeries.Evaluate();
				int np = varSeries.NumberOfPoints;
				foreach(DataDescriptor des in dataDescriptors)
				{
					if(des.Required && (inVars[des.Name] == null || inVars[des.Name].EvaluatedValue.Length != np))
					{
						string valuePath = "";
						if(inVars[des.Name] != null)
							valuePath = inVars[des.Name].ValuePath;
						InputVariable inVar = new InputVariable(des.Name);
						inVar.VariableKind = DataDescriptor.VariableKind(des.Name);
						inVar.ValuePath = valuePath;
						inVar.NumberOfPoints = np;
						inVars.Add(inVar);
						inVar.Evaluate();
					}
				}
			}

			foreach (DataDescriptor des in dataDescriptors)
			{
				des.EvaluateValue();
				Variable v = des.Value;
				if(v != null)
				{
					if(des.Name == "x")
						xDimension = v.Dimension;
					bool firstYParameterSeen = false;
					for(int i=0;i<yParamNames.Length;i++)
					{
						if(des.Name == yParamNames[i])
						{
							if(yDimension == null || !firstYParameterSeen)
							{
								firstYParameterSeen = true;
								yDimension = v.Dimension;
								YAxis.SetDimension(yDimension);
							}
							else
								yDimension = yDimension.Merge(v.Dimension);
						}
					}
				}
			}
		}

		internal override void PropagateDimensions(DataDimension xDim, DataDimension yDim)
		{
			xDimension = xDim;
			if(!HasIndependentYAxis)
				yDimension = yDim;
			foreach (DataDescriptor des in dataDescriptors)
			{
				Variable v = des.Value;
				if(v != null)
				{
					if(des.Name == "x")
						v.Dimension = XDimension;
					for(int i=0;i<yParamNames.Length;i++)
					{
						if(des.Name == yParamNames[i])
							v.Dimension = yDimension;
					}
				}
			}
		}

		internal override void BindParameters()
		{
            dataPoints.Clear();
            nonMissingDataPoints.Clear();

			// Determine number of data points
			//  Note: some variables associated to data descriptors may be constants,
			//        therefore number of data points is minimum length > 1 of variables.

			int nPts = 0;
			dataBindingValid = true;
			foreach (DataDescriptor des in dataDescriptors)
			{
				des.EvaluateValue();
				Variable var = des.Value;
				if(var == null)
				{
					if(des.Required)
					{
						dataBindingValid = false;
						OwningChart.RegisterErrorMessage("Series '" + Name + "': value of '" + des.Name + "' (required for style '" + Style.Name + "') is not known.");
					}
				}
				else
				{
					int L = var.Length;
					if(L>0) 
					{
						if(nPts == 0)
							nPts = L;
						else
							nPts = Math.Min(L,nPts);
					}
				}
			}

			if(!dataBindingValid)
			{
				numberOfPoints = 0;
				return;
			}


			numberOfPoints = nPts;
			if(nPts == 0)
				return;

			// Create data points

            //Go through all the points to determine which points fall within the X range and
            //therefore need to be created.
            //an array representing all the points is created so that each element of the array represents a point with:
            // 0 - point is OUT of the X range
            // 1 - point is IN the X range
            // 2 - point is next to the point which is in X range (usefull for line and area chart edges)
            short[] pointsInRange = new short[numberOfPoints];
            for (int i = 0; i<numberOfPoints; i++)
            {
                pointsInRange[i]=1;
            }

            if (XAxis.MinValue != null || XAxis.MaxValue != null)
            {
                double min = (XAxis.MinValue != null)?xDimension.Coordinate(XAxis.MinValue):double.MinValue;
                double max = (XAxis.MaxValue != null)?xDimension.Coordinate(XAxis.MaxValue):double.MaxValue;

                foreach (DataDescriptor des in dataDescriptors)
                {
                    if (des.Name.ToLower().Equals("x"))
                    {
                        des.EvaluateValue();
                        Variable var = des.Value;
                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            double pointValue = this.xDimension.Coordinate(var.ItemAt(i));
							if(i>0 && pointValue >= min && pointsInRange[i-1] == 0)
								pointsInRange[i-1] = 2;
							if(i<numberOfPoints-1 && pointValue <= max && pointsInRange[i+1] == 0)
								pointsInRange[i+1] = 2;
                            if (pointValue >= min && pointValue <= max)
								pointsInRange[i] = 1;
							else
								pointsInRange[i] = 0;
						}
						break;
                    }
                }
            }

            int index = 0;
			for(int i=0; i<nPts; i++)
			{
                if ((pointsInRange[i] == 1)
                            || (pointsInRange[i] == 2
                                && (Style.ChartKindCategory == ChartKindCategory.Area
                                    || Style.ChartKindCategory == ChartKindCategory.Line)))
                {
                    DataPoint DP = new DataPoint();
                    DP.Index = index;
					DP.OriginalIndex = i;
                    dataPoints.Add(DP);
                    DP.SetOwner(this);
                    index++;
                }
			}

            numberOfPoints = index;

			// Set parameters to data points, compute x,y extremes and scatterPoint size
			minXDCS = null;
			maxXDCS = null;
			minYDCS = null;
			maxYDCS = null;
			scatterXPointSize = 0;
			scatterYPointSize = 0;
			
			foreach (DataDescriptor des in dataDescriptors)
			{
				Variable v = des.Value;
				if(v != null)
				{
                    index = 0;
					for(int i=0;i<nPts;i++)
					{
                        //do not add points if the range is specified and they are not in the range
                        //except for points next to the included ones in Area and Line charts
                        if ((pointsInRange[i] == 1)
                            || (pointsInRange[i] == 2
                                && (Style.ChartKindCategory == ChartKindCategory.Area
                                    || Style.ChartKindCategory == ChartKindCategory.Line)))
                        {


                            if (des.Required && v.MissingAt(i))
                                dataPoints[index].IsMissing = true;

                            dataPoints[index][des.Name] = v.ItemAt(i);
                            index++;
                        }
					}
				}
			}

			foreach (DataDescriptor des in dataDescriptors)
			{
				Variable v = des.Value;
				if(v == null)
					continue;
				Variable vv = Variable.CreateVariable(v.Name,v.ValType);
				vv.SetConstant = false;
				vv.Clear();
                for (int i = 0; i < numberOfPoints; i++)
				{
					if(!v.IsMissing[i])
						vv.Add(DataPoints[i][des.Name]);
				}
				
				if(des.Name == "x")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minXDCS,ref maxXDCS, ref scatterXPointSize, true);
				}
				else if(des.Name == "y")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
				else if(des.Name == "from")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
				else if(des.Name == "to")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
				else if(des.Name == "open")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
				else if(des.Name == "close")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
				else if(des.Name == "high")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
				else if(des.Name == "low")
				{
					v.Dimension.GetExtremesAndPointSize(vv, ref minYDCS,ref maxYDCS, ref scatterYPointSize, false);
				}
			}

			for(int i=0; i<numberOfPoints; i++)
				this.DataPoints[i].BindParameters();
			AdjustYRangeBasedOnXRange();
			
			// Handle scatter chart
			if(autoScatteredPointSize)
			{
				double xPoint = 0, xPointPrev = 0, dMin = double.MaxValue;
				DataDimension xDim = XDimension;
				if(Style.ChartKindCategory == ChartKindCategory.Bar ||
					Style.ChartKindCategory == ChartKindCategory.Financial)
				{
					if(numberOfPoints > 1)
					{
						double xMin = double.MaxValue;
						double xMax = double.MinValue;
						for(int i=0; i<numberOfPoints; i++)
						{
							object xObj = this.DataPoints[i].Parameters["x"];
							xPoint = xDim.Coordinate(xObj);
							if(i>0)
								dMin = Math.Min(dMin,Math.Abs(xPoint-xPointPrev));
							xMin = Math.Min(xMin,xPoint);
							xMax = Math.Max(xMax,xPoint);
							xPointPrev = xPoint;
						}
						// Make sure that the scattered point size is not too small; Defect 5194
						scaterPointSize = Math.Max(dMin,(xMax-xMin)/numberOfPoints*0.3);
					}
					else
						scaterPointSize = xDim.SingleScatterPointSize;
				}
				else
					scaterPointSize = 5; // TODO: base this value on IWidthZ
			}

            // Compute non-missing data points
            for (int i = 0; i < numberOfPoints; i++)
            {
                if (!DataPoints[i].IsMissing)
                    nonMissingDataPoints.Add(DataPoints[i]);
            }
		}

		internal void AdjustYRangeBasedOnXRange()
		{
			if(OwningChart.InDesignMode || OwningChart.InWizard)
				return;
			string[] valueNames = new String[] { "y","from","to", "open","high","low","close" };
			double minLCS = double.MinValue;
			double maxLCS = double.MaxValue;
			if(XAxis.MinValue != null)
				minLCS = XDimension.Coordinate(XAxis.MinValue);
			if(XAxis.MaxValue != null)
				maxLCS = XDimension.Coordinate(XAxis.MaxValue);

			object min = null;
			object max = null;
			double minD = double.MaxValue;
			double maxD = double.MinValue;
			for(int i=0;i<DataPoints.Count; i++)
			{
				DataPoint dp = DataPoints[i];
				double xLCS = XDimension.Coordinate(dp.Parameter("x"));

				if(minLCS <= xLCS && xLCS <= maxLCS && !dp.IsMissing)
				{
					object y = null;
					for(int j=0; j<valueNames.Length; j++)
					{
						y = dp.Parameters[valueNames[j]];
						if(y != null)
						{
							double yD = YDimension.Coordinate(y);
							if(min == null || minD > yD)
							{
								min = y;
								minD = yD;
							}
							if(max == null || maxD < yD)
							{
								max = y;
								maxD = yD;
							}
						}
					}
				}
			}

			if(min == null)
			{
				// If there is no point in the x range, use all points
				for(int i=0;i<DataPoints.Count; i++)
				{
					DataPoint dp = DataPoints[i];
					double xLCS = XDimension.Coordinate(dp.Parameter("x"));

					if(!dp.IsMissing)
					{
						object y = null;
						for(int j=0; j<valueNames.Length; j++)
						{
							y = dp.Parameters[valueNames[j]];
							if(y != null)
							{
								double yD = YDimension.Coordinate(y);
								if(min == null || minD > yD)
								{
									min = y;
									minD = yD;
								}
								if(max == null || maxD < yD)
								{
									max = y;
									maxD = yD;
								}
							}
						}
					}
				}
			}

			minYDCS = min;
			maxYDCS = max;

		}

		internal override void HandleMissingDataPoints()
		{
			MissingPointHandler mph = GetEffectiveMissingPointHandler();
			if(mph != null)
			{
				mph.SetData(DataPoints);
				for(int i=0; i<DataPoints.Count; i++)
				{
					if(DataPoints[i].IsMissing)
						DataPoints[i].Parameters["styleName"] = EffectiveMissingPointsStyleName;
				}
				mph.ComputeMissingPoints();
			}
		}
		
		internal override void ComputeDefaultICSSize()
		{
			if(Style.IsLinear)
			{
				if(DZICS == 0.0 || dzSetForPieDoughnut)
					DZICS = defaultDepth;
				
				// Find the predecessor series that owns the coordinate system 
				// and take the number of points

				SeriesBase s = this;
				double lcsSize = 0.0;
				while(s != null && s.OwnCoordSystem == null)
					s = s.OwningSeries;
				if(s != null)
					lcsSize = s.MaxXLCS()-s.MinXLCS();
				if(lcsSize <= 0.0)
					lcsSize = DataPoints.Count;
				else
				{
					if(XDimension is DateTimeDataDimension)
							lcsSize += 1;
				}
			
				if(XDimension is NumericDataDimension)
				{
					if(scatterXPointSize != 0.0)
						lcsSize = lcsSize/scatterXPointSize + 1;
				}

				if(lcsSize <= 10)
					DXICS = lcsSize*defaultDepth;
				else
					DXICS = 10*defaultDepth;

				

				DYICS = DXICS*EffectiveYByXRatio;

				dzSetForPieDoughnut = false;
			}
			else
			{
				DXICS = 100;
				DZICS = 100;
				DYICS = Math.Min(DXICS,DZICS)/2*Style.RelativeHeight;
				dzSetForPieDoughnut = true;
			}	
		}

		internal override void DataBind()
		{
			base.DataBind();
			AddThisToZDimensionHierarchy();

			EnumeratedDataDimension zDimension = ZAxis.Dimension as EnumeratedDataDimension;
			if(zDimension != null)
			{
				Coordinate ddi = zDimension[Name];
				if(OwningSeries.CompositionKind == CompositionKind.Sections)
					ddi.Offset = zDimension[OwningSeries.Name].Width;
				if(ZAxis.MinValue == null)
					ZAxis.MinValue = ddi.Value;
				ZAxis.MaxValue = ddi.Value;
			}

			// Default layout
			ComputeSize();

			// Bind the coordinate system
			if(OwnCoordSystem != null)
				OwnCoordSystem.DataBind();

			// Datapoint labels bind
		
			RemoveInheritedLabels();
			GetEffectiveLabels();
			foreach(SeriesLabels sl in Labels)
				sl.DataBind();
		}

		private void RemoveInheritedLabels()
		{
			ArrayList lst = new ArrayList();
			foreach(SeriesLabels sl in Labels)
			{
				if(sl.Inherited)
					lst.Add(sl);
			}
			foreach(SeriesLabels sl in lst)
				Labels.Remove(sl);
		}
		#endregion

		#region --- Building ---

		internal override void Build()
        {
			if(!dataBindingValid)
				return;

			// Make sure that x and y dimensions exist in the owning chart

			if(OwningChart.DataDimensions[XDimension.Name] == null)
				OwningChart.DataDimensions.Add(XDimension);
			if(OwningChart.DataDimensions[YDimension.Name] == null)
				OwningChart.DataDimensions.Add(YDimension);

			// Build points

			numberOfPoints = this.dataPoints.Count;
			for(int i=0;i<numberOfPoints;i++)
			{
				this.DataPoints[i].Build();
			}

			// Set data point properties (by copying from this series)

			Hashtable ht = AccumulatedDataPointParameters();
			IDictionaryEnumerator de = ht.GetEnumerator();
			
			while (de.MoveNext())
			{
				if(de.Key is string)
				{
					string sKey = (string)de.Key;
					object val = de.Value;
					
					for(int i=0;i<numberOfPoints;i++)
					{
						// Do not override parameter already assigned to the DataPoint
						if(this.DataPoints[i][sKey] == null)
							this.DataPoints[i][sKey] = val;
					}
				}
			}
			// Redefine "color" and "secondaryColor" data point properties
			//	for doughnut and pie

			Palette palette = OwningChart.Palette;

			// Redefine Y0 and Y1 applying "stacking" for doughnut/pie styles;
			if(Style.ChartKind == ChartKind.Pie || Style.ChartKind == ChartKind.Doughnut)
			{
				double sum = 0.0;
				for(int i=0;i<numberOfPoints;i++)
				{
					int iColor = i;
					if(Style.ChartKindCategory == ChartKindCategory.PieDoughnut &&
						i%palette.PrimaryColors.Count == 0 && i==numberOfPoints-1)
						iColor = i+1;
					double y;
					if(this.DataPoints[i].NumericValue("y",out y))
						sum += y;
					if(this.DataPoints[i]["color"] == null)
						this.DataPoints[i]["color"] = palette.GetPrimaryColor(iColor);
					if(this.DataPoints[i]["secondaryColor"] == null)
						this.DataPoints[i]["secondaryColor"] = palette.GetSecondaryColor(iColor);
				}
				for(int i=1;i<numberOfPoints;i++)
					dataPoints[i].StackOn(dataPoints[i-1]);
				for(int i=0;i<numberOfPoints;i++)
					dataPoints[i].SetLCSRange(0,sum);
			}

			// Build the coordinate system
			if(OwnCoordSystem != null)
				OwnCoordSystem.Build();

			// Build labels

			GetEffectiveLabels();
			Labels.Build();
        }

		internal bool StackOn(Series s)
		{
			numberOfPoints = dataPoints.Count;
			if(s.NumberOfPoints != numberOfPoints)
				throw new Exception("Cannot apply stacking composition on series '" + s.Name + "' and '" + Name + "': Different number of points");
			bool allPointsZeroHeight = true;
			int[] ix = AlignIndices(s);
			for(int i=0;i<numberOfPoints;i++)
			{
				if(!this.DataPoints[i].StackOn(s.DataPoints[ix[i]]))
					return false;
				if(DataPoints[i].Y0LCSE != DataPoints[i].Y1LCSE)
					allPointsZeroHeight = false;
			}
			if(allPointsZeroHeight)
			{
				this.Visible = false;
				this.addToLegend = false;
			}
			return true;
		}

		private int[] AlignIndices(Series s)
		{
			// Align indices if x-coordinates of s don't match in order x coordinates of this series
			int[] ix = new int[numberOfPoints];
			if(XDimension is EnumeratedDataDimension)
			{
				for(int i=0; i<numberOfPoints; i++)
				{
					string thisX = DataPoints[i].Parameter("x") as string;
					string sX = s.DataPoints[i].Parameter("x") as string;
					if(thisX == sX)
						ix[i] = i;
					else
					{
						for(int j=0; j<numberOfPoints; j++)
						{
							sX = s.DataPoints[j].Parameter("x") as string;
							if(thisX == sX)
							{
								ix[i] = j;
								break;
							}
						}
					}
				}
			}
			else
				for(int i=0; i<numberOfPoints; i++)
					ix[i] = i;
			return ix;
		}

		internal void Scale100(Series s)
		{
			numberOfPoints = dataPoints.Count;
			if(s.NumberOfPoints != numberOfPoints)
				throw new Exception("Cannot apply stacking composition on series '" + s.Name + "' and '" + Name + "': Different number of points");
			for(int i=0;i<numberOfPoints;i++)
				this.DataPoints[i].Scale100(s.DataPoints[i]);
		}

		internal void IgnoreReferenceValue()
		{
			for(int i=0;i<NumberOfPoints;i++)
				DataPoints[i].IgnoreReferenceValue();
		}

		internal void SetYLCSRange(Series first, Series last)
		{
			numberOfPoints = dataPoints.Count;
			if(first.NumberOfPoints != numberOfPoints)
				throw new Exception("Cannot apply stacking composition on series '" + first.Name + "' and '" + Name + "': Different number of points");
			if(last.NumberOfPoints != numberOfPoints)
				throw new Exception("Cannot apply stacking composition on series '" + last.Name + "' and '" + Name + "': Different number of points");
			for(int i=0;i<numberOfPoints;i++)
				DataPoints[i].SetLCSRange(first.dataPoints[i].Y0LCSE,last.DataPoints[i].Y1LCSE);
		}

		internal string[] RequiredParameterNames
		{
			get
			{
				string[] required = null;
				if(isRange)
				{
					switch (Style.ChartKind)
					{
						case ChartKind.Bubble:				required = new string[] { "x","y" };				break;
						case ChartKind.Bubble2D:			required = new string[] { "x","y" };				break;
						case ChartKind.Line:				required = new string[] { "x","y" };				break;
						case ChartKind.Line2D:				required = new string[] { "x","y" };				break;
						case ChartKind.Marker:				required = new string[] { "x","y" };				break;
						case ChartKind.Doughnut:			required = new string[] { "x","y" };				break;
						case ChartKind.Pie:					required = new string[] { "x","y" };				break;
						case ChartKind.HighLowOpenClose:    required = new string[] { "x","high","low","open","close" };        break;
						case ChartKind.CandleStick:			required = new string[] { "x","high","low","open","close" };        break;
						default:							required = new string[] { "x","from","to" };        break;
					}
				}
				else
				{
					switch (Style.ChartKind)
					{
						case ChartKind.Doughnut:			required = new string[] { "x","y" };		break;
						case ChartKind.Pie:					required = new string[] { "x","y" };		break;
						case ChartKind.CandleStick:			required = new string[] { "x","high","low","open","close" };        break;
						case ChartKind.HighLowOpenClose:    required = new string[] { "x","high","low","open","close" };        break;
						default:							required = new string[] { "x","y" };        break;
					}
				}
				return required;
			}
		}
		
		/// <summary>
		///  This is a list used from SqlChart reading sequence. 
		/// </summary>
		static internal string[] paramNamesWithType = new string[] { "y","transparency", "s:action", "width", "height", "shift", "lift", "s:legend", "s:color", "s:secondaryColor", "s:styleName", "s:legend", "s:toolTip" };
        
		internal string[] OptionalParameterNamesWithType
		{
			get
			{
				string[] optional = null;
				switch (Style.ChartKind)
				{   // List here all string-type parameters with "s:" prefix
                    case ChartKind.Bubble: optional = new string[] {"isMissing", "transparency", "s:action", "size", "s:color", "s:secondaryColor", "s:styleName", "s:legend", "s:toolTip" }; break;
                    case ChartKind.Bubble2D: optional = new string[] {"isMissing", "transparency", "s:action", "size", "s:color", "s:secondaryColor", "s:styleName", "s:legend", "s:toolTip" }; break;
                    case ChartKind.Line: optional = new string[] {"isMissing", "transparency", "s:action", "s:color", "s:secondaryColor", "s:styleName", "s:legend", "s:toolTip" }; break;
                    case ChartKind.Line2D: optional = new string[] {"isMissing", "transparency", "s:action", "s:color", "s:styleName", "s:legend", "s:toolTip" }; break;
                    case ChartKind.Marker: optional = new string[] {"isMissing", "transparency", "s:action", "size", "s:color", "s:secondaryColor", "s:styleName", "s:markerStyle", "s:legend", "s:toolTip" }; break;
                    case ChartKind.Doughnut: optional = new string[] {"isMissing", "transparency", "s:action", "width", "height", "shift", "lift", "s:legend", "s:color", "s:secondaryColor", "s:styleName", "s:legend", "s:toolTip" }; break;
                    case ChartKind.Pie: optional = new string[] {"isMissing", "transparency", "s:action", "width", "height", "shift", "lift", "s:legend", "s:color", "s:secondaryColor", "s:styleName", "s:legend", "s:toolTip" }; break;
                    case ChartKind.HighLowOpenClose: optional = new string[] {"isMissing", "transparency", "s:action", "s:color", "s:secondaryColor", "s:legend", "s:toolTip" }; break;
                    case ChartKind.CandleStick: optional = new string[] {"isMissing", "transparency", "s:action", "color", "secondaryColor", "legend", "toolTip" }; break;
                    default: optional = new string[] {"isMissing", "transparency", "s:action", "width", "depth", "s:color", "s:secondaryColor", "s:styleName", "s:gradientStyle", "s:legend", "s:toolTip" }; break;
				}
				return optional;
			}
		}
        
        internal string[] OptionalParameterNames
        {
            get
            {
                string[] list = OptionalParameterNamesWithType;
                if(list == null)
                    return null;
                list = (string[])list.Clone();
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].IndexOf(':') > 0)
                        list[i] = list[i].Split(':')[1];
                }
                return list;
            }
        }

        internal string[] OptionalNumericalParameters
        {
            get
            {
                string[] list = OptionalParameterNamesWithType;
                if (list == null)
                    return null;

                int n = 0;
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].IndexOf(':') < 0)
                        n++;
                }
                if (n == 0)
                    return null;

                string[] list2 = new string[n];
                n = 0;
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].IndexOf(':') < 0)
                    {
                        list2[n] = list[i];
                        n++;
                    }
                }
                return list2;
            }
        }

        internal string[] OptionalStringParameters
        {
            get
            {
                string[] list = OptionalParameterNamesWithType;
                if (list == null)
                    return null;

                int n = 0;
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].IndexOf(':') > 0)
                        n++;
                }
                if (n == 0)
                    return null;

                string[] list2 = new string[n];
                n = 0;
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].IndexOf(':') > 0)
                    {
                        list2[n] = list[i].Split(':')[1];
                        n++;
                    }
                }
                return list2; 
            }
        }

        internal bool IsRequiredParameter(string paramName)
        {
            string[] required = RequiredParameterNames;
            foreach (string param in required)
                if (param == paramName)
                    return true;
            return false;
        }

        internal bool IsOptionalParameter(string paramName)
        {
            string[] optional = OptionalParameterNames;
            foreach (string param in optional)
                if (param == paramName)
                    return true;
            return false;
        }



		internal bool UsesParameter(string paramName)
		{
            if (IsRequiredParameter(paramName) || IsOptionalParameter(paramName))
                return true;

			GetEffectiveLabels();
            string[] required;
            for (int i = 0; i < Labels.Count; i++)
            {
                required = Labels[i].RequiredParameters;
                if (required != null)
                {
                    foreach (string param in required)
                    {
                        if (paramName == param)
                            return true;
                    }
                }
            }

			return false;
		}

		internal override void RegisterVariables()
        {
            string[] required = RequiredParameterNames, optional = OptionalParameterNames;

			dataDescriptors.Clear();

			foreach(string name in required)
			{
				DataDescriptor DVD = new DataDescriptor(name);
				DVD.SetRequired(true);
				dataDescriptors.Add(DVD);
			}
			foreach(string name in optional)
			{
				DataDescriptor DVD = new DataDescriptor(name);
				DVD.SetRequired(false);
				dataDescriptors.Add(DVD);
			}

			GetEffectiveLabels(); 
            for (int i = 0; i < Labels.Count; i++)
            {
                Labels[i].RegisterVariables();
            }
		}

  
		internal double MaxValue(string varName)
		{
			double max = double.MinValue;
			foreach(DataPoint P in dataPoints)
			{
				double v;
				if(!P.NumericValue(varName,out v))
					throw new Exception("Variable '" + varName + "' does not exist or it is not numeric");
				max = Math.Max(max,v);
			}
			return max;
		}

		internal override int NumberOfSimpleSeriesBefore(Series s, out bool found)
		{
			if(s == this)
			{
				found = true;
				return 0;
			}
			else
			{
				found = false;
				return 1;
			}
		}
		internal override bool HasBarSeries()
		{
			return Style.IsBar;
		}
		
		internal override void WCSRange(out double x0, out double y0, out double z0, out double x1, out double y1, out double z1)
		{
			x0 = double.MaxValue;
			y0 = double.MaxValue;
			z0 = double.MaxValue;

			x1 = double.MinValue;
			y1 = double.MinValue;
			z1 = double.MinValue;

			for(int i=0;i<2;i++)
			{
				double x = i*DXICS;
				for(int j=0;j<2;j++)
				{
					double y = j*DYICS;
					for(int k=0;k<2;k++)
					{
						double z = k*DZICS + TotalOffsetZICS;
						Vector3D v = CoordSystem.ICS2WCS(new Vector3D(x,y,z));
						x0 = Math.Min(x0,v.X);
						y0 = Math.Min(y0,v.Y);
						z0 = Math.Min(z0,v.Z);
						x1 = Math.Max(x1,v.X);
						y1 = Math.Max(y1,v.Y);
						z1 = Math.Max(z1,v.Z);
					}
				}
			}
		}

		#endregion
   
		#region --- Rendering ---

		internal override void Render(bool toSetupArea)
		{
			if(!Visible || !InVisibleParent)
				return;

			if (TotalNumberOfDataPoints == 0)
				return;

			if(!dataBindingValid)
				return;

            styleLowerBoundLineKind = Style.LowerBoundLineKind;
			styleUpperBoundLineKind = Style.UpperBoundLineKind;
			if(dataPoints.Count == 1)
			{
				styleLowerBoundLineKind = LineKind.Step;
				styleUpperBoundLineKind = LineKind.Step;
			}

			PushTargetArea();

			if(OwnTargetArea != null)
				OwnTargetArea.Render();

			PushCoordinateSystem();
			if(OwnCoordSystem != null && Style.ChartKindCategory != ChartKindCategory.PieDoughnut)
				OwnCoordSystem.Render();
            
			SectionBox sb = new SectionBox();
			sb.Tag = this;
			GE.Push(sb);
			GE.SetActiveObject(this);

			if(Style.IsRadar && Style.IsArea)
				RenderRadarArea();
			else if(Style.IsLinear)
			{
				// Partial rendering of sequences of "similar" data points

				int ip0 = 0;
				while(ip0 < DataPoints.Count)
				{
					int ip1 = GetSimilarPointsEndIndex(ip0);
					RenderPartial(ip0,ip1);
					ip0 = ip1+1;
				}
			}
			else
			{
				// Traditional rendering of pie/doughnut and radar types

				if(Style.IsLine)
					RenderLine();
				else if(Style.IsArea)
					RenderArea();
				else 
					RenderDataPoints();
			}

			GE.SetActiveObject(null);
			Labels.Render();

			GE.Pop(sb.GetType());
			PopCoordinateSystem();
			PopTargetArea();
		}

		#region --- Partial series rendering ---

		private void RenderPartial(int iPoint0, int iPoint1)
		{
			double x0 = CoordSystem.XAxis.MinValueLCS;
			double x1 = CoordSystem.XAxis.MaxValueLCS;
			for(int i=iPoint0; i<=iPoint1; i++)
			{
				DataPoint dataPoint = DataPoints[i];
				dataPoint.ToRenderLabels = dataPoint.X0LCSE < x1 && dataPoint.X1LCSE > x0;
			}
			SeriesStyle style = DataPoints[iPoint0].EffectiveStyle;

			// Checking the number of non-missing points,
			// in case when line or area has to turn into marker
			DataPointCollection nonMissing = NonMissingDataPoints;
			int firstNonMissing = -1;
			int nNonMissing = 0;
			for(int i=iPoint0; i<=iPoint1; i++)
			{
				if(!DataPoints[i].IsMissing)
				{
					nNonMissing++;
					if(nNonMissing == 1)
						firstNonMissing = i;
				}
			}
			if(style.IsArea || style.IsLine)
			{
				if(nNonMissing == 0)
					return;
				else if(nNonMissing == 1)
				{
					// One point on line/area
					style = OwningChart.SeriesStyles[SeriesStyleKind.CircleMarker];
					style.MarkerStyleKind = MarkerStyleKind.Circle;
					DataPoints[firstNonMissing].Parameters["size"] = 10;
					DataPoints[firstNonMissing].StyleKind = SeriesStyleKind.CircleMarker;
					iPoint0 = firstNonMissing;
					iPoint1 = firstNonMissing;
				}
			}
			ChartColor pColor = DataPoints[iPoint0].EffectiveSurface;
			ChartColor sColor = DataPoints[iPoint0].EffectiveSecondarySurface;
			if(style.IsLine)
				RenderLinePartial(style,pColor,sColor,iPoint0,iPoint1);
			else if(style.IsArea)
				RenderAreaPartial(style,pColor,sColor,iPoint0,iPoint1);
			else 
				RenderDataPointsPartial(iPoint0,iPoint1);
		}
		
		private void RenderDataPointsPartial(int iPoint0, int iPoint1)
		{
			DrawingBoard drawingBoard = null;
			for(int i=iPoint0; i<=iPoint1; i++)
			{
				if(DataPoints[i].IsMissing)
					continue;
				ColumnBox cb = new ColumnBox();
				GE.Push(cb);
				double x0w,x1w, y0w,y1w, z0w,z1w;			
				bool invertedY;
				DataPoints[i].ComputeBoundingBox(out x0w,out x1w, out y0w, out y1w,out z0w, out z1w, out invertedY);
				double yc = (OwningSeries.CoordSystem.YAxis.MinValueICS+OwningSeries.CoordSystem.YAxis.MaxValueICS)/2;
				cb.Tag = new Vector3D((x0w+x1w)/2,yc,(z0w+z1w)/2);

				SubColumnBox scb = new SubColumnBox();
				GE.Push(scb);
				DataPoints[i].Render(ref drawingBoard);
				scb.Tag = DataPoints[i];
				GE.Pop(scb.GetType());

				GE.Pop(cb.GetType());
			}
		}

		private void RenderLinePartial(SeriesStyle style, ChartColor pColor, ChartColor sColor, int iPoint0, int iPoint1)
		{
			ColumnBox cb = new ColumnBox();
			GE.Push(cb);
			double x0LCS = double.MaxValue;
			double x1LCS = double.MinValue;
			for(int i=iPoint0; i<=iPoint1; i++)
			{
				x0LCS = Math.Min(x0LCS,DataPoints[i].MinXLCS());
				x1LCS = Math.Max(x1LCS,DataPoints[i].MaxXLCS());
			}
			cb.Tag = new Vector3D(
				CoordSystem.XAxis.LCS2ICS((x0LCS+x1LCS)/2),
				(CoordSystem.YAxis.MinValueICS+CoordSystem.YAxis.MaxValueICS)/2,
				CoordSystem.ZAxis.LCS2ICS(ZDimension.Coordinate(ZCoordinate)+ZDimension.Width(ZCoordinate)/2)
				);
			RenderLineInRange(x0LCS,x1LCS,style,pColor,sColor);
			GE.Pop(typeof(ColumnBox));
		}

		private void RenderAreaPartial(SeriesStyle style, ChartColor pColor, ChartColor sColor, int iPoint0, int iPoint1)
		{
			ColumnBox cb = new ColumnBox();
			GE.Push(cb);
			double x0LCS = double.MaxValue;
			double x1LCS = double.MinValue;
			for(int i=iPoint0; i<=iPoint1; i++)
			{
				x0LCS = Math.Min(x0LCS,DataPoints[i].MinXLCS());
				x1LCS = Math.Max(x1LCS,DataPoints[i].MaxXLCS());
			}
			cb.Tag = new Vector3D(
				CoordSystem.XAxis.LCS2ICS((x0LCS+x1LCS)/2),
				(CoordSystem.YAxis.MinValueICS+CoordSystem.YAxis.MaxValueICS)/2,
				CoordSystem.ZAxis.LCS2ICS(ZDimension.Coordinate(ZCoordinate)+ZDimension.Width(ZCoordinate)/2)
				);
			RenderLinearAreaInRange(x0LCS,x1LCS,style,pColor,sColor);
			GE.Pop(typeof(ColumnBox));
		}


		private int GetSimilarPointsEndIndex(int pointStartIndex)
		{
			int i;

			if(pointStartIndex == DataPoints.Count-1)
				return pointStartIndex;

			DataPoint sp0 = DataPoints[pointStartIndex];
			SeriesStyle sp0Style = sp0.EffectiveStyle;
			ChartColor sp0Color = sp0.EffectiveSurface;
			for(i=pointStartIndex+1; i<DataPoints.Count; i++)
			{
				SeriesStyle sp1Style = DataPoints[i].EffectiveStyle;
				ChartColor sp1Color = DataPoints[i].EffectiveSurface;
				if(sp1Style != sp0Style)
					break;
				if(sp1Color.Alpha != sp0Color.Alpha ||
					sp1Color.Red != sp0Color.Red ||
					sp1Color.Green != sp0Color.Green ||
					sp1Color.Blue != sp0Color.Blue)
					break;
				//if(DataPoints[i].Visible != sp0.Visible)
				//	break;
			}
			return i-1;
		}

		#endregion
				
		internal override void RenderTitles()
		{
			if(OwnTargetArea != null)
				OwnTargetArea.Render();
		}

		private void RenderDataPoints()
		{
			DataPointCollection dataPoints = NonMissingDataPoints;
            DrawingBoard drawingBoard = null;
			foreach(DataPoint DP in dataPoints)
			{
				DP.ToRenderLabels = true;
				DP.Render(ref drawingBoard);
			}
		}

		
		private void RenderLine()
		{
			RenderLineInRange(double.MinValue, double.MaxValue, Style, EffectiveSurface, EffectiveSecondarySurface);
		}
		
		private void RenderLineInRange(double x0LCS, double x1LCS, SeriesStyle style, ChartColor pColor, ChartColor sColor)
		{
			// Check number of points
			DataPointCollection dataPoints = NonMissingDataPoints;
			if(dataPoints.Count <= 1)
			{
				OwningChart.RegisterErrorMessage("Series '" + Name + "' with " + dataPoints.Count + " point(s) cannot be displayed as line");
				return;
			}

			if(Style.IsRadar)
			{
				LineStyle2D ls2D = OwningChart.LineStyles2D[Style.LineStyle2DName];
				ls2D = (LineStyle2D)ls2D.Clone();
				ls2D.Color = pColor.Color;
                DrawingBoard B = CoordSystem.PlaneXY.CreateDrawingBoard(); 
				Vector3D V0 = new Vector3D(0,0,0);
				GraphicsPath path = new GraphicsPath();
				PointF[] points = new PointF[dataPoints.Count];

				for(int i=0; i<NonMissingDataPoints.Count; i++)
				{
                    DataPoint DP = NonMissingDataPoints[i];
					Vector3D V = CoordSystem.PlaneXY.LogicalToWorld(CoordSystem.XAxis.LCS2ICS(DP.X0LCSE),CoordSystem.YAxis.LCS2ICS(DP.Y1LCSE));
					points[i].X = (float)V.X;
					points[i].Y = (float)V.Y;
					if(ObjectTrackingEnabled && !OwningChart.InDesignMode)
					{
						Vector3D VP = Mapping.Map(V);
						OwningChart.ObjectMapper.AddDataPointHotSpot((int)(VP.X),(int)(VP.Y),DP);
					}

				}
				path.AddPolygon(points);
				path.CloseFigure();
				B.DrawPath(ls2D,path,true,true,true);
			}
			else
			{
				Vector3D Xe = (CoordSystem.ICS2WCS(new Vector3D(1,0,0))-CoordSystem.ICS2WCS(new Vector3D(0,0,0))).Unit();
				Vector3D Ye = (CoordSystem.ICS2WCS(new Vector3D(0,1,0))-CoordSystem.ICS2WCS(new Vector3D(0,0,0))).Unit();
				if(style.ChartKind == ChartKind.Line)
				{
					bool closed = false;
					SimpleLine L = CreateLinkedLine(true,style.LineKind); 
					Vector3D P = CoordSystem.ICS2WCS(new Vector3D(0,0,ZAxis.ICoordinate(Name)+ZAxis.IWidth(Name)*0.5));
					// Setting the style
					LineStyle ls1 = OwningChart.LineStyles[style.LineStyleName];
					if(ls1 == null)
						return;

					double depth = ZAxis.IWidth(Name);
					if(ls1 is StripLineStyle)
						ls1.Height = 
							(
							CoordSystem.ICS2WCS(new Vector3D(0,0,depth*(1-style.RelativeBackSpace-style.RelativeFrontSpace)))-
							CoordSystem.ICS2WCS(new Vector3D(0,0,0))
							).Abs/Mapping.FromPointToWorld;
					// Constructing the line
					double x0ICS = Math.Max(CoordSystem.XAxis.MinValueICS, CoordSystem.XAxis.LCS2ICS(x0LCS));
					double x1ICS = Math.Min(CoordSystem.XAxis.MaxValueICS, CoordSystem.XAxis.LCS2ICS(x1LCS));
					if(CoordSystem.XAxis.Reverse)
					{
						double a = x0ICS;
						x0ICS = x1ICS;
						x1ICS= a;
					}
					if(x1ICS <= x0ICS)
						return;
					SimpleLine[] sections = L.CutInXRange(x0ICS,x1ICS);
					//GE.Top.Tag = sections;
					if(sections == null)
						return;
					for(int lineX = 0; lineX<sections.Length; lineX++)
					{
						L = sections[lineX];
						ChartLine line = GE.CreateLine(style.LineStyleName,P,Xe,Ye);
						line.IsSmooth = (style.LineKind == LineKind.Smooth);
						line.ChartColor = pColor;// EffectiveSurface;
						//line.SetOwner(this);
						line.AddPoints(L);
						line.Closed = closed;
						//line.Render();
						if(ObjectTrackingEnabled && !OwningChart.InDesignMode)
						{
							for(int i = 0;i<NonMissingDataPoints.Count; i++)
							{
                                DataPoint DP = NonMissingDataPoints[i];
								double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS()) + CoordSystem.XAxis.IWidth(DP.XDCS())*0.5;
								double yy = CoordSystem.YAxis.LCS2ICS(DP.Y1LCSE);
								Vector3D PP = Mapping.Map(P+Xe*xx+Ye*yy);
                                OwningChart.ObjectMapper.AddDataPointHotSpot((int)(PP.X), (int)(PP.Y), (DataPoint)(NonMissingDataPoints[i]));
							}
						}
					}
				}
				else	// Line2D 
				{
					LineStyle2D LS2D = OwningChart.LineStyles2D[style.LineStyle2DName];
					if(LS2D == null)
						LS2D = new LineStyle2D(style.LineStyle2DName,2,Color.Black,DashStyle.Solid);
					else
						LS2D = (LineStyle2D)LS2D.Clone();
					// Handle transparent line color
					if(LS2D.Color.A == 0)
						LS2D.Color = GetEffectiveSurface(0).Color;
                    DrawingBoard drawingBoard = CreateDrawingBoard();
					Xe = drawingBoard.Vx;
					Ye = drawingBoard.Vy;
					SimpleLine L = CreateLinkedLine(true,style.LineKind); // TODO: implement "closed" feature			
					double x0ICS = Math.Max(CoordSystem.XAxis.MinValueICS, CoordSystem.XAxis.LCS2ICS(x0LCS));
					double x1ICS = Math.Min(CoordSystem.XAxis.MaxValueICS, CoordSystem.XAxis.LCS2ICS(x1LCS));
					if(x1ICS <= x0ICS)
						return;
					SimpleLine[] sections = L.CutInXRange(x0ICS,x1ICS);
					//GE.Top.Tag = sections;
					if(sections == null)
						return;
					for(int lineX = 0; lineX<sections.Length; lineX++)
					{
						L = sections[lineX];
						int N = L.Length;
						PointF[] pts = new PointF[N];
						SimpleLine LL = L;
						for(int i=0;i<N;i++,LL=LL.next)
						{
							pts[i].X = (float)LL.x;
							pts[i].Y = (float)LL.y;
						}
						if(ObjectTrackingEnabled && !OwningChart.InDesignMode)
						{
							double zICS = OffsetZICS+DZICS*0.5;
							for(int i = 0;i<NonMissingDataPoints.Count; i++)
							{
                                DataPoint DP = NonMissingDataPoints[i];
								double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS()) + CoordSystem.XAxis.IWidth(DP.XDCS())*0.5;
								double yy = CoordSystem.YAxis.LCS2ICS(DP.Y1LCS);
								Vector3D PW = CoordSystem.ICS2WCS(new Vector3D(xx,yy,zICS));
								Vector3D PP = Mapping.Map(PW);
                                OwningChart.ObjectMapper.AddDataPointHotSpot((int)(PP.X), (int)(PP.Y), (DataPoint)(NonMissingDataPoints[i]));
							}
						}

						drawingBoard.DrawLines(LS2D,pts,true,true);	
					}

					if(LS2D.Width<3)
						GE.SetActiveObject(null);
				}
			}
		}

		private void RenderRadarArea()
		{
			Brush brush = new SolidBrush(EffectiveSurface.Color);
            DrawingBoard B = CoordSystem.PlaneXY.CreateDrawingBoard();
			Vector3D V0 = new Vector3D(0,0,0);
			GraphicsPath path = new GraphicsPath();
			PointF[] points = new PointF[dataPoints.Count];
			for(int i=0; i<dataPoints.Count; i++)
			{
				DataPoint DP = this.DataPoints[i];
				Vector3D V = CoordSystem.PlaneXY.LogicalToWorld(CoordSystem.XAxis.LCS2ICS(DP.X0LCSE),CoordSystem.YAxis.LCS2ICS(DP.Y1LCSE));
				points[i].X = (float)V.X;
				points[i].Y = (float)V.Y;
			}
			path.AddPolygon(points);
			path.CloseFigure();
			LineStyle2D ls2D = OwningChart.LineStyles2D[Style.LineStyle2DName];
			B.DrawPath(ls2D,path,false,true,false);
			B.SetActiveObject(this);
			B.FillPath(brush,path,true,false);
			B.SetActiveObject(null);
			B.DrawPath(ls2D,path,true,false, true);
			if(ObjectTrackingEnabled && !OwningChart.InDesignMode)
			{
				GraphicsPath tPath = B.BoardToBmp(path);
				OwningChart.ObjectMapper.PrependMapAreaPolygon(tPath,B.Ix0,B.Iy0,this);
				for(int i=0; i<NonMissingDataPoints.Count; i++)
				{
                    DataPoint DP = NonMissingDataPoints[i];
					Vector3D V = CoordSystem.PlaneXY.LogicalToWorld(CoordSystem.XAxis.LCS2ICS(DP.X0LCSE),CoordSystem.YAxis.LCS2ICS(DP.Y1LCSE));
					Vector3D VP = Mapping.Map(V);
					OwningChart.ObjectMapper.AddDataPointHotSpot((int)(VP.X),(int)(VP.Y),DP);
				}
			}
			B.LiftZ = 1.5;

			return;
		}

		private void RenderArea()
		{
			// Check number of points
			if(dataPoints.Count <= 1)
			{
				Style.LineKind = LineKind.Step;
				if(dataPoints.Count == 0)
				{
					OwningChart.RegisterErrorMessage("Series '" + Name + "' with " + dataPoints.Count + " point(s) cannot be displayed as area");
					return;
				}
			}
			if(Style.IsRadar)
				RenderRadarArea();
			else
				RenderLinearArea();
		}

		private void RenderLinearArea()
		{
			RenderLinearAreaInRange(double.MinValue,double.MaxValue,Style,EffectiveSurface,EffectiveSecondarySurface);
		}

        private void RenderLinearAreaInRange(double xRangeMinLCS, double xRangeMaxLCS, SeriesStyle style,
			ChartColor primary, ChartColor secondary)
        {
			double xRangeMinICS;
			double xRangeMaxICS;
			if(XAxis.Reverse)
			{
				xRangeMinICS = XAxis.LCS2ICS(xRangeMaxLCS);
				xRangeMaxICS = XAxis.LCS2ICS(xRangeMinLCS);
			}
			else
			{
				xRangeMinICS = XAxis.LCS2ICS(xRangeMinLCS);
				xRangeMaxICS = XAxis.LCS2ICS(xRangeMaxLCS);
			}
            // Lines

            double minValueICSX = CoordSystem.XAxis.MinValueICS;
            double maxValueICSX = CoordSystem.XAxis.MaxValueICS;
			minValueICSX = Math.Max(minValueICSX,xRangeMinICS);
			maxValueICSX = Math.Min(maxValueICSX,xRangeMaxICS);
			if(minValueICSX >= maxValueICSX)
				return;

            double dValueICSX = maxValueICSX - minValueICSX;
            minValueICSX += dValueICSX * 0.001;
            maxValueICSX -= dValueICSX * 0.001;

			if(xRangeMinICS >= maxValueICSX || xRangeMaxICS <= minValueICSX)
				return;
            
            SimpleLine L1, L2;

			L1 = CreateLinkedLine(true, styleUpperBoundLineKind, false);
			L2 = CreateLinkedLine(false, styleLowerBoundLineKind, false);//.Reverse();

            L1.SetSmoothLine(styleLowerBoundLineKind == LineKind.Smooth);
            L2.SetSmoothLine(styleUpperBoundLineKind == LineKind.Smooth);
			SimpleLineCollection LC1;
			if(L1.IsClosed())
			{
				LC1 = new SimpleLineCollection();
				LC1.Add(L1);
			}
			else
			{
				LC1 = SimpleLineCollection.CreateFromLowerAndUpperBounds(L2,L1);
			}

			// We have to crop one line at a time because they may have different orientation

			SimpleLineCollection LC = new SimpleLineCollection();

			foreach(SimpleLine SL in LC1)
			{
				SimpleLineCollection LCC = new SimpleLineCollection();
				LCC.Add(SL);
				LCC = LCC.CropAreaAtX(minValueICSX, false);
				LCC = LCC.CropAreaAtX(maxValueICSX, true);
				if(xRangeMinICS > minValueICSX)
					LCC = LCC.CropAreaAtX(xRangeMinICS,false);
				if(xRangeMaxICS < maxValueICSX)
					LCC = LCC.CropAreaAtX(xRangeMaxICS,true);
				foreach(SimpleLine L in LCC)
					LC.Add(L);
			}
			LC1 = LC;

            // Wall plane origin in ICS
            double z0w, dz;
            z0w = CoordSystem.ZAxis.ICoordinate(this.Name);
            dz = CoordSystem.ZAxis.IWidth(this.Name);
            if (dz < 0)
            {
                z0w += dz;
                dz = -dz;
            }

            // Give a small push to the relative front space and relative back space to get
            // areas that don't share the same, but slightly different z-coordinate
            double rfs = Style.RelativeFrontSpace;
            double rbs = Style.RelativeBackSpace;
            double displacement = 0;
            if (OwningSeries.CompositionKind == CompositionKind.Merged)
            {
                CompositeSeries parent = OwningSeries;
				int n = parent.SubSeries.Count;
				int ix = parent.GetSequenceNumber(this);
				displacement = ((double)(n - 1 - ix)) / n * 0.05;
                rfs = rfs + displacement;
                rbs = rbs + displacement;
            }

            // Origin in ICS
            Vector3D PICS = new Vector3D(-displacement, 0, z0w + dz * (1.0 - rfs));

            // Origin in WCS

            Vector3D P = CoordSystem.ICS2WCS(PICS);
            Vector3D Xe = (CoordSystem.ICS2WCS(new Vector3D(1, 0, 0)) - CoordSystem.ICS2WCS(new Vector3D(0, 0, 0))).Unit();
            Vector3D Ye = (CoordSystem.ICS2WCS(new Vector3D(0, 1, 0)) - CoordSystem.ICS2WCS(new Vector3D(0, 0, 0))).Unit();
            double h =
                (
                CoordSystem.ICS2WCS(new Vector3D(0, 0, dz * (1.0 - rbs - rfs))) -
                CoordSystem.ICS2WCS(new Vector3D(0, 0, 0))
                ) * (Xe.CrossProduct(Ye));
            
            // Creating hot-spot points

            if (ObjectTrackingEnabled && !OwningChart.InDesignMode)
            {
                for (int i = 0; i < NonMissingDataPoints.Count; i++)
                {
                    DataPoint DP = NonMissingDataPoints[i];
                    if (DP.Visible)
                    {
						double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS()) + CoordSystem.XAxis.IWidth(DP.XDCS()) * 0.5;
						double yy = CoordSystem.YAxis.LCS2ICS(DP.Y1LCSE);
						Vector3D pointWCS = P + Xe * xx + Ye * yy;
						if (style.ChartKind != ChartKind.Area)
						{
							double zz = OffsetZICS + DZICS*0.5;
							pointWCS = CoordSystem.ICS2WCS(new Vector3D(xx,yy,zz));
						}
                        Vector3D PP = TargetArea.Mapping.Map(pointWCS);
                        OwningChart.ObjectMapper.AddDataPointHotSpot((int)(PP.X), (int)(PP.Y), DP);
                    }
                }
            }

			// Computing orientation for area segments

			bool coordSystemSign = true;
			switch(CoordSystem.Orientation)
			{
				case CoordinateSystemOrientation.Default:coordSystemSign = true; break;
				case CoordinateSystemOrientation.Horizontal:coordSystemSign = false; break;
				case CoordinateSystemOrientation.XZY:coordSystemSign = false; break;
				case CoordinateSystemOrientation.YZX:coordSystemSign = true; break;
				case CoordinateSystemOrientation.ZXY:coordSystemSign = true; break;
				case CoordinateSystemOrientation.ZYX:coordSystemSign = false; break;
			}

			bool xyReversed = CoordSystem.XAxis.Reverse != CoordSystem.YAxis.Reverse;
			bool[] toReverse = new bool[LC1.Count];
			bool[] positiveSeg = new bool[LC1.Count];
			for(int i=0; i<LC1.Count; i++)
			{
				positiveSeg[i] = LC1[i].IsPositiveArea();
				toReverse[i] = (positiveSeg[i]!=coordSystemSign);
				positiveSeg[i] = (positiveSeg[i]!=xyReversed);
			}
		
			GE.SetActiveObject(this);
			if (style.ChartKind == ChartKind.Area)
            {
				AdjustAreaOrientation(LC1,ref Xe, ref Ye, ref P, ref h);
				for(int i=0; i<LC1.Count; i++)
				{
					double xC, yC;
					SimpleLineCollection SLC = new SimpleLineCollection();
					if(toReverse[i])
						SLC.Add(LC1[i].Reverse());
					else
						SLC.Add(LC1[i]);
					ColumnBox scb = new ColumnBox();
					LC1[i].GetAreaCenter(out xC, out yC);
					Vector3D center = new Vector3D(P+xC*Xe+yC*Ye) - h*0.5*Xe.CrossProduct(Ye);
					scb.Tag = center;
					GE.Push(scb);
					GE.CreateArea(SLC, h, P, Xe, Ye, positiveSeg[i]?primary:secondary, primary).Tag = this;
					GE.Pop();
				}
			}
            else
            {
                LineStyle2D LS = OwningChart.LineStyles2D[EffectiveLine2DStyleName];
                if (LS == null) return;

                if (LS.Color.A == 0)
                {
                    LS = (LineStyle2D)LS.Clone();
                    LS.Color = OwningChart.Palette.TwoDObjectBorderColor;
                }

                object gradientStyleObj = DataPointParameters["gradientStyle"];
                string gradientStyleName;
                if (gradientStyleObj != null && gradientStyleObj is string)
                    gradientStyleName = (string)gradientStyleObj;
                else
                    gradientStyleName = Style.GradientStyleName;
                GradientStyle gradientStyle = OwningChart.GradientStyles[gradientStyleName];
                GradientStyle secGradientStyle = null;
                if (gradientStyle == null)
                {
                    ChartColor srf = primary;
                    ChartColor srfSec = secondary;
                    gradientStyle = new GradientStyle(gradientStyleName, GradientKind.None, srf.Color, srf.Color);
                    secGradientStyle = new GradientStyle(gradientStyleName, GradientKind.None, srfSec.Color, srfSec.Color);
                }
                else
                {
                    gradientStyle = (GradientStyle)gradientStyle.Clone();
                    if (gradientStyle.EndColor.A == 0)
                        gradientStyle.EndColor = secondary.Color;
                    if (gradientStyle.StartColor.A == 0)
                        gradientStyle.StartColor = primary.Color;
                    secGradientStyle = gradientStyle;
                }

                DrawingBoard dBoard = CreateDrawingBoard();
				dBoard.SetActiveObject(this);
				for(int i=0; i<LC1.Count;i++)
				{
					SimpleLine SL = LC1[i];
					int np = SL.Length;
					PointF[] points = new PointF[np];
					for(int j=0;j<np;j++)
					{
						points[j].X = (float)SL.x;
						points[j].Y = (float)SL.y;
						SL = SL.next;
					}
					dBoard.DrawArea(LS,gradientStyle,points);
				}
            }
        }
		private void AdjustAreaOrientation(SimpleLineCollection LC,ref Vector3D Xe, ref Vector3D Ye, ref Vector3D P0, 
			ref double h)
		{
			bool revX = Xe.X < 0 || Xe.Y < 0 || Xe.Z < 0;
			bool revY = Ye.X < 0 || Ye.Y < 0 || Ye.Z < 0;
			Vector3D H = h*Xe.CrossProduct(Ye);
			bool revZ = H.X < 0 || H.Y < 0 || H.Z < 0;
			if(!revX && !revY && !revZ)
				return;

			if(revX) 
				Xe = -Xe;
			if(revY)
				Ye = -Ye;

			if(revZ)
			{
				P0 = P0+H;
				h = -h;
			}

			foreach(SimpleLine L in LC)
			{
				SimpleLine LL = L;
				while(LL != null)
				{
					if(revX) 
						LL.x = -LL.x;
					if(revY)
						LL.y = -LL.y;
					LL = LL.next;
					if(LL == L)
						break;
				}
			}

		}

 
		private SimpleLine[] CutInXRangeICS(SimpleLine L, double minValueICSX,double maxValueICSX)
		{
			return L.CutInXRange(minValueICSX,maxValueICSX);
		}

		private SimpleLine CreateLinkedLine(bool useHighValue, LineKind lineKind)
		{
			// Used for lines
			DataPointCollection dataPoints = NonMissingDataPoints;

			SimpleLine L = null;
			
			int nPoints = dataPoints.Count;

			for(int i=0;i<nPoints; i++)
			{
				DataPoint DP;
				DP = dataPoints[i];
				double yy = DP.Y1LCSE;
				yy = CoordSystem.YAxis.LCS2ICS(yy);
				if(lineKind != LineKind.Step)
				{
					double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS()) + CoordSystem.XAxis.IWidth(DP.XDCS())*0.5;
					if(L==null)
						L = new SimpleLine(xx,yy);
					else
						L.Append(xx,yy);
				}
				else // StepLine
				{
					double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS());
					if(L == null)
						L = new SimpleLine(xx,yy);
					else
						L.Append(xx,yy);
					xx = xx + CoordSystem.XAxis.IWidth(DP.XDCS());
					L.Append(xx,yy);
				}
			}

			if (lineKind == LineKind.Smooth)
				L = L.SmoothLine(false);
			

			return L;
		}

		private SimpleLine CreateLinkedLine(bool useHighValue, LineKind lineKind, bool closed)
		{
			// Used for areas

			DataPointCollection dataPoints = NonMissingDataPoints;
			SimpleLine L = null;
			
			int nPoints = dataPoints.Count;

			double refLCS = YDimension.Coordinate(ReferenceValue);

			for(int i=0;i<nPoints; i++)
			{
				double yy;
				DataPoint DP;
				DP = dataPoints[i];
				if(useHighValue)
					yy = DP.Y1LCSE;
				else
					yy = DP.Y0LCSE;
                bool pushYValue = (yy == ReferenceValueNumeric);
                yy = CoordSystem.YAxis.LCS2ICS(yy);
                if(pushYValue)
                    yy += 0.001*Math.Abs(YAxis.MaxValueICS - YAxis.MinValueICS);
				if(lineKind != LineKind.Step)
				{
					double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS()) + CoordSystem.XAxis.IWidth(DP.XDCS())*0.5;
					if(L==null)
						L = new SimpleLine(xx,yy);
					else
						L.Append(xx,yy);
				}
				else // StepLine
				{
					double xx = CoordSystem.XAxis.ICoordinate(DP.XDCS());
					if(L == null)
						L = new SimpleLine(xx,yy);
					else
						L.Append(xx,yy);
					xx = xx + CoordSystem.XAxis.IWidth(DP.XDCS());
					L.Append(xx,yy);
				}
			}

			if (lineKind == LineKind.Smooth)
				L = L.SmoothLine(closed);
			

			return L;
		}

		#endregion

		#region --- Effective Property Values ---

		internal ChartColor EffectiveSurface { get { return GetEffectiveSurface(0); } }

		internal double EffectiveTransparency
		{
			get
			{
                if (!OwningChart.InDesignMode)
                {
                    object tObj = DataDescriptors["transparency"];
                    if (tObj != null && tObj is double)
                        return (double)tObj;
                }
                return transparency;
			}
		}

		internal static Color ModifyTransparency(Color inputColor, double transparency)
		{
			if(transparency == 0.0)
				return inputColor;
			if(transparency == 1.0)
				return Color.FromArgb(0,inputColor.R,inputColor.G,inputColor.B);
			return Color.FromArgb((int)((1.0-transparency)*inputColor.A),inputColor.R,inputColor.G,inputColor.B);
		}

		internal ChartColor GetEffectiveSurface(int dataPointIndex)
		{
			if(Style.ChartKind == ChartKind.Pie || Style.ChartKind == ChartKind.Doughnut)
				return new ChartColor(0.5f,6,
					ModifyTransparency(OwningChart.Palette.GetPrimaryColor(dataPointIndex),EffectiveTransparency));

			object srf;

			srf = DataDescriptors["surface"];
			if(srf == null)
				srf = DataDescriptors["color"];

			if(srf != null && srf is ChartColor)
			{
				ChartColor s = srf as ChartColor;
				s = (ChartColor)s.Clone();
				s.Color = ModifyTransparency(s.Color,EffectiveTransparency);
				return s;
			}

			if(srf != null && (srf is Color))
				return new ChartColor(0.5f,6,ModifyTransparency((Color)srf,EffectiveTransparency));

			return new ChartColor(0.5f,6,
				ModifyTransparency(OwningChart.Palette.GetPrimaryColor(OwningSeries.GetSequenceNumber(this)),EffectiveTransparency));
		}

		internal ChartColor EffectiveSecondarySurface{ get { return GetEffectiveSecondarySurface(0); } }

		internal ChartColor GetEffectiveSecondarySurface(int dataPointIndex)
		{
			if(Style.ChartKind == ChartKind.Pie || Style.ChartKind == ChartKind.Doughnut)
				return new ChartColor(0.5f,6,OwningChart.Palette.GetPrimaryColor(dataPointIndex));

			object srf;

			srf = DataDescriptors["secondarySurface"];
			if(srf != null && srf is ChartColor)
				return srf as ChartColor;

			srf = DataDescriptors["secondaryColor"];
			if(srf != null && (srf is Color))
				return new ChartColor(0.5f,6,(Color)srf);


			if(srf != null && srf is ChartColor)
			{
				ChartColor s = srf as ChartColor;
				s = (ChartColor)s.Clone();
				s.Color = ModifyTransparency(s.Color,EffectiveTransparency);
				return s;
			}

			if(srf != null && (srf is Color))
				return new ChartColor(0.5f,6,ModifyTransparency((Color)srf,EffectiveTransparency));

			return new ChartColor(0.5f,6,
				ModifyTransparency(OwningChart.Palette.GetSecondaryColor(OwningSeries.GetSequenceNumber(this)),EffectiveTransparency));
		}

		internal string EffectiveGradientStyleName
		{
			get
			{
				string name;
				object nameP = DataDescriptors["gradientStyleName"];
				if(nameP == null || nameP.GetType() != typeof(string))
				{
					name = temporaryGradientStyleName;
					OwningChart.GradientStyles.Remove(name);
					GradientStyle S = new GradientStyle(name,GradientKind.None,EffectiveSurface.Color,EffectiveSurface.Color);
					OwningChart.GradientStyles.Add(S);
					return name;
				}
				else
					return nameP as string;
			}
		}

		internal string EffectiveLine2DStyleName
		{
			get
			{
				object nameP = DataDescriptors["line2DStyleName"];
				if(nameP != null && nameP.GetType() == typeof(string))
					return nameP as string;
				else
					return Style.BorderLineStyleName;
			}
		}
		#endregion
	
		#region --- Serialization & browsing Control ---
		private bool ShouldSerializeNumberOfPoints()	{ return false; }

		private static string[] PropertiesOrder = new string[] 
			{
				"Name",
				"StyleName",
				"Parameters",
				"Labels",
				"IsRange",
				"NumberOfPoints",
		  
				"LegendText",
				"AddToLegend",
		
				"Depth",
				"Z0",
				"Visible"
			};

		#endregion
	}
}
