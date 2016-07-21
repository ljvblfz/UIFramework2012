using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	///     The smallest entity representing a single input data item.
	/// </summary>
	/// <remarks>
	///	  <para>
	///     The <see cref="DataPoint"/> object is presented by a single geometrical entity
	///     like cylinder, block, marker etc., depending on the data point style. The graphical presentation
	///     of a data point is affected by a set of parameters stored in the property <see cref="DataPoint.Parameters"/>.
	///     Data points are created and parameters are computed from input data during the "DataBind()" operation and therefore they are not available before
	///     data binding is completed. (For a detailed description of data binding see topics "Data Binding" in "Basic Concepts"
	///		and "Advanced Data Binding" in "Advanced Concepts".) 
	///     Once created, parameters are accessible and may be modified by user code.
	///	  </para>
	///	  <para>
	///	    There are many parameters that affect the data point presentation within the chart. The most important
	///	    parameters are 
	///	    <list type="bullet">
	///	      <item>
	///	        "x" - representing the x-coordinate for a data point in Data Coordinate System (DCS). For more about DCS and 
	///	        other coordinate systems see topic "Coordinates and Coordinate Systems" in "Advanced Topics".
	///	      </item>
	///	      <item>
	///	        "y" - representing a (usually numeric) value from input data. Instead of "y", parameters "from" and
	///	        "to" are used if the owning series is of range type ("IsRange" property is set to "true"). In case of
	///	        financial charts, parameters "open", "close", "low" and "high" are used instead of "y".
	///	      </item>
	///	    </list>
	///	    The role of a parameter depends on the data point styles. For example, some parameters are used 
	///	    only in pie/doughnut styles, some other only for markers and otherwise ignored. For the full list
	///	    of parameters and their roles see the topic "Data Structure" in "Basic Topics".
	///	  </para>
	/// </remarks>
	// (KED)
	public sealed class DataPoint : ChartObject,ILegendItemProvider/*, ICloneable*/
	{
		private Hashtable coordinates = new Hashtable();

		private string		legendText = "";
		private string		toolTipText = "";

		// Index in the containing series

		int	index = 0;			// after series reduction within x-range
		int originalIndex = -1; // before series reduction
		bool toRenderLabels = false;

		double radius = 50;

		// --- DCS coordinates of the data point ---

		// x-coordinate
		private object  xValue;
		// y-coordinates.
		// Values are equal unless the series is range type or financial open-low-hugh-close
		private object  yValue0, yValue1;
		
		// --- LCS coordinates of the data point ---

		// --- x logical coordinates
		
		// Logical x-coordinate range
		double x0LCS, x1LCS;	
		// Effective range, adjusted in case of merged composition
		double x0LCSE, x1LCSE; 

		// -- y logical coordinates
		// The relationships between y-coordinates:
		//		1. y1LCSStacked-y0LCSStacked = y1LCS-y0LCS	(stacking preserves LCS size)
		//		2. y0LCSRange <= y0LCSStacked <= y1LCSStacked <= y1LCSRange
		
		// Logical y-coordinate range
		double y0LCS, y1LCS;	
		// Effective logical y-coordinates of datapoint, representing extent of the rendered datapoint
		//    in logical coordinate system.
		// Initially they are the same as y0LCS, y1LCS, but the may be affected by the reference point
		//    and stacking composition.
		//    Note that relationship y0LCSE > y1LCSE may be valid (for example when from>to)
		double y0LCSE, y1LCSE;	
		// Logical y-coordinate range at the same x-coordinate range for stacked composition
		double y0LCSRange,   y1LCSRange;
		// Minimum and maximum value for object representation.
		// Based on yValue0, yValue1, but might be affected by the series reference point
		object	yMinValue, yMaxValue;
		
		bool y0IsReferenceValue = false;

		// -- z logical coordinates
		double z0LCS, z1LCS;

		// -- Missing mode
		private bool isMissing = false;
		
		#region --- Construction and Cloning ---

		internal DataPoint() { }

		#endregion

		#region --- Public Properties ---

		/// <summary>
		/// The x-coordinate of the data point.
		/// </summary>
		public object X { get { return coordinates["x"]; } }

		/// <summary>
		/// The y-coordinate of the data point.
		/// </summary>
		/// <remarks>
		/// This coordinate is null for the range type or the financial type series.
		/// </remarks>
		public object Y { get { return coordinates["y"]; } }

		/// <summary>
		/// The "from" value of the data point.
		/// </summary>
		/// <remarks>
		/// This value is non-null only for the range type series.
		/// </remarks>
		public object From { get { return coordinates["from"]; } }

		/// <summary>
		/// The "to" value of the data point.
		/// </summary>
		/// <remarks>
		/// This value is non-null only for the range type series.
		/// </remarks>
		public object To { get { return coordinates["to"]; } }

		/// <summary>
		/// The "open" value of the data point.
		/// </summary>
		/// <remarks>
		/// This value is valid only for the financial type series, otherwise it is NaN.
		/// </remarks>
		public double Open { get { return GetDoubleParameter("open"); } }
		
		/// <summary>
		/// The "high" value of the data point.
		/// </summary>
		/// <remarks>
		/// This value is valid only for financial type series, otherwise it is NaN.
		/// </remarks>
		public double High { get { return GetDoubleParameter("high"); } }
		
		
		/// <summary>
		/// The "low" value of the data point.
		/// </summary>
		/// <remarks>
		/// This value is valid only for financial type series, otherwise it is NaN.
		/// </remarks>
		public double Low { get { return GetDoubleParameter("low"); } }
		
		
		/// <summary>
		/// The "close" value of the data point.
		/// </summary>
		/// <remarks>
		/// This value is valid only for financial type series, otherwise it is NaN.
		/// </remarks>
		public double Close { get { return GetDoubleParameter("close"); } }

		/// <summary>
		/// The <see cref="ChartColor"/> of the data point.
		/// </summary>
		/// <remarks>
		/// Both this property and the <see cref="Color"/> property can set the <see cref="ChartColor"/> of a data point. 
        /// If both are used the latter will override the former.
		/// </remarks>
		public ChartColor Surface
		{
			get
			{
				return EffectiveSurface;
			}
			set
			{
				coordinates["surface"] = value;
			}
		}

		/// <summary>
		/// The secondary <see cref="ChartColor"/> of the data point.
		/// </summary>
		/// <remarks>
		/// Both this property and the <see cref="SecondaryColor"/> property can set the 
		/// secondary <see cref="ChartColor"/> of a data point. 
        /// If both are used the latter will override the former.
		/// </remarks>
		public ChartColor SecondarySurface
		{
			get
			{
				return EffectiveSecondarySurface;
			}
			set
			{
				coordinates["secondarySurface"] = value;
			}
		}

		/// <summary>
		/// The <see cref="ChartColor"/> of the data point.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When this property is set, a <see cref="ChartColor"/> is created with the default <see cref="ChartColor.Reflection"/> (=0.5)
		/// and the default <see cref="ChartColor.LogPhong"/> (=6) are used to define the data points <see cref="Surface"/> property.
		/// </para>
		/// <para>
		/// Both this property and the <see cref="Surface"/> property can set the <see cref="ChartColor"/> of a data point. 
		/// If both are used, the latter will override the former.
		/// </para>
		/// </remarks>
		public Color Color
		{
			get
			{
				ChartColor surface = EffectiveSurface;
				return surface.Color;
			}
			set
			{
				coordinates.Remove("surface");
				coordinates["color"] = value;
			}
		}

		
		/// <summary>
		/// The secondary <see cref="ChartColor"/> of the data point.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When this property is set, a <see cref="ChartColor"/> is created with the default <see cref="ChartColor.Reflection"/> (=0.5)
		/// and the default <see cref="ChartColor.LogPhong"/> (=6) are used to define the data points <see cref="SecondarySurface"/> property.
		/// </para>
		/// <para>
		/// Both this property and the <see cref="SecondarySurface"/> property can set the secondary <see cref="ChartColor"/> of a data point. 
		/// If both are used, the later overrides the former.
		/// </para>
		/// </remarks>
		public Color SecondaryColor
		{
			get
			{
				ChartColor surface = EffectiveSecondarySurface;
				return surface.Color;
			}
			set
			{
				coordinates.Remove("secondarySurface");
				coordinates["secondaryColor"] = value;
			}
		}

		/// <summary>
		/// The depth of the data point.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This value is used to determine the data point dimension along the z-coordinate for bar type charts.
        /// If the value of this property is not set it will equal NaN.
		/// </para>
		/// <para>
        /// The depth of the point with the maximum depth value will be equal to the depth of the section 
		/// containing the series. The depths of the other points will be set relative to this point 
        /// (proportional to the ratio of the <see cref="Depth"/> values of the points).
		/// </para>
		/// <para>
		/// For example, if the depth of the point with the maximum depth is 100, then the point with Depth = 50 has 
        /// half the depth of the section, i.e. half the depth of the maximum point. Note that the absolute values of the 
        /// depth aren't important; the same effect could be achieved with depths of 10 and 5.
		/// </para>
		/// </remarks>
		public double Depth 
		{
			get
			{
				return GetDoubleParameter("depth");
			}
			set
			{
				coordinates["depth"] = value;
			}
		}
		
		/// <summary>
		/// The size of the data point.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This value is only used in marker type charts. 
		/// If the value of this property is not set it will equal NaN.
		/// </para>
		/// <para>
		/// The point with the maximum size value will completely fill the space assigned to a data point.
        /// Other points will be resized relative to this point 
        /// (proportional to the ratio of the <see cref="Size"/> values of the points).
		/// </para>
		/// <para>
		/// For example, if the size of the point with the maximum size is 100, then the point with Size = 50 
        /// has half the size of the maximum. Note that absolute values aren't important; the same effect 
        /// could be achieved with the values of 10 and 5.
		/// </para>
		/// </remarks>
		public double Size
		{
			get
			{
				return GetDoubleParameter("size");
			}
			set
			{
				coordinates["size"] = value;
			}
		}

		/// <summary>
		/// The marker style name.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is only used in marker type charts. 
		/// If this property is not set it equals an empty string.
		/// </para>
		/// <para>
		/// This property can be used to assign different marker styles to data points. 
		/// Any marker style can be used. For non-marker styles, this property is ignored.
		/// </para>
		/// </remarks>
		public string MarkerStyleName
		{
			get
			{
				return GetStringParameter("markerStyle");
			}
			set
			{
				coordinates["markerStyle"] = value;
			}
		}
	
		/// <summary>
		/// The gradient style name.
		/// </summary>
		/// <remarks>
		/// <para>
        /// <para>
        /// If this property is not set it equals an empty string.
        /// </para>
		/// This value is only used in 2D area and rectangle chart types. 
        /// Any <see cref="GradientStyle"/> value from the "GradientStyles" property of the chart object 
        /// can be used.
		/// </para>
		/// </remarks>
		public string GradientStyleName
		{
			get
			{
				return GetStringParameter("gradientStyle");
			}
			set
			{
				coordinates["gradientStyle"] = value;
			}
		}

		
		/// <summary>
		/// The style name.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is used to set different styles to individual data points of a series.
		/// Any <see cref="SeriesStyle"/> value from the "SeriesStyles" property of the chart object
        /// can be used.
		/// </para>
		/// </remarks>
		public string StyleName
		{
			get
			{
				string name = GetStringParameter("styleName");
				if(name == string.Empty)
					return EffectiveStyle.Name;
				else
					return name;
			}
			set
			{
				coordinates["styleName"] = value;
			}
		}

		
		/// <summary>
		/// The data point style of the type <see cref="SeriesStyleKind"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This value is used to set different styles to individual data points of a series.
		/// Style setting through this property should be done only for predefined styles.
		/// For other styles, use the <see cref="StyleName"/> property.
		/// </para>
		/// <para>
		/// Any <see cref="SeriesStyle"/> value from the "SeriesStyles" property of the chart object
        /// can be used.
		/// </para>
		/// </remarks>
		public SeriesStyleKind StyleKind
		{
			get
			{
				return SeriesStyle.StyleKindOf(StyleName);
			}
			set
			{
				StyleName = SeriesStyle.StyleNameOf(value);
			}
		}

		
		/// <summary>
		/// The shift amount for this data point in a pie/doughnut chart.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If this property is not set the value equals 0.
		/// </para>
		/// <para>
        /// <para>
        /// If this property is not set the value equals NaN.
        /// </para>
		/// This value is only used in pie/doughnut type charts. 
        /// The amount of shift is relative to the size of the radius. For example,
		/// if Shift = 0.2, the pie/doughnut slice is shifted outwards by 20% of the radius.
		/// </para>
		/// </remarks>
		public double Shift
		{
			get
			{
				return GetDoubleParameter("shift",0);
			}
			set
			{
				coordinates["shift"] = value;
			}
		}
				
		/// <summary>
		/// The lift amount for the data point in a pie/doughnut chart.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If this property is not set it equals 0.
		/// </para>
		/// <para>
		/// This property value is only used in a pie/doughnut charts. 
        /// The amount of lift is relative to the pie/doughnut height. For example,
		/// if Lift = 0.2, then the pie/doughnut slice is lifted upwards by 20% of the pie chart height.
		/// </para>
		/// </remarks>
		public double Lift
		{
			get
			{
				return GetDoubleParameter("lift",0);
			}
			set
			{
				coordinates["lift"] = value;
			}
		}
				
		/// <summary>
		/// The color transparency of the data point.
		/// </summary>
		/// <remarks>
		/// The valid values are between 0 (opaque) and 1 (totally transparent). 
		/// If this property is not set it equals 0.
		/// </remarks>
		public double Transparency
		{
			get
			{
				return GetDoubleParameter("transparency",0);
			}
			set
			{
				coordinates["transparency"] = value;
			}
		}

		#region --- Parameters handling helpers
		private double GetDoubleParameter(string key, double deflt)
		{
			double val = GetDoubleParameter(key);
			if(val.Equals(double.NaN))
				return deflt;
			else
				return val;
		}

		private double GetDoubleParameter(string key)
		{
			object val = coordinates[key];
			if(val == null)
				return double.NaN;
			if (val is double)
				return (double)val;
			TypeConverter tc = TypeDescriptor.GetConverter(val.GetType());
			if(tc != null && tc.CanConvertTo(typeof(double)))
				return (double)tc.ConvertTo(val,typeof(double));
			throw new Exception("Type '" + val.GetType().Name + "' not valid for DataPoint parameter '" + key + "'");
		}

		private string GetStringParameter(string key)
		{
			object val = coordinates[key];
			if(val == null)
				return string.Empty;
			if (val is string)
				return (string)val;
			throw new Exception("Type '" + val.GetType().Name + "' not valid for DataPoint parameter '" + key + "'");
		}


		#endregion
			/// <summary>
			/// Gets the parent <see cref="Series"/> of this <see cref="DataPoint"/> object.
			/// </summary>
			public		Series	OwningSeries	{ get { return (Owner as Series); } }
		/// <summary>
		/// Gets the parameters of this <see cref="DataPoint"/> object.
		/// </summary>
		public		Hashtable	Parameters  { get { return coordinates; } }
		/// <summary>
		/// Gets or sets the mode indicating if x or y coordinate of this <see cref="DataPoint"/> is missing.
		/// </summary>
		public		bool	IsMissing  { get { return isMissing; } set { isMissing = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DataPoint"/> object is visible.
		/// </summary>
		public override bool Visible
		{
			get
			{
				if(!base.Visible)
					return false;
				if(isMissing)
					return false;
				if(OwningSeries != null && OwningSeries.Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
					return true;

				double x0 = X0LCSE;
				double x1 = X1LCSE;
				double d = x1-x0;
				if(OwningSeries != null && OwningSeries.Style.ChartKindCategory == ChartKindCategory.Marker)
				{
					x0 += d/2;
					x1 -= d/2;
				}
				else
				{
					x0 += d/100;
					x1 -= d/100;
				}
				//				return CoordSystem.XAxis.MinValueLCS < x0 && x1 < CoordSystem.XAxis.MaxValueLCS;
				return CoordSystem.XAxis.MinValueLCS < x1 || x0 < CoordSystem.XAxis.MaxValueLCS;
			}
			set
			{
				base.Visible = value;
			}
		}


		/// <summary>
		/// Gets or sets the text in the legend in pie/doughnut chart.
		/// </summary>
		public string	LegendText		
		{ 
			get 
			{ 
				if(legendText != null && legendText != "")
					return legendText;
				if(coordinates["legend"] != null)
					return coordinates["legend"].ToString();
				if(coordinates["x"] != null)
					return coordinates["x"].ToString();
				return "";
			}
			set { legendText = value; } 
		}

		/// <summary>
		/// Gets or sets the tool tip text.
		/// </summary>
		public string	ToolTipText		
		{ 
			get 
			{ 
				if(toolTipText != null && legendText != "")
					return toolTipText;
				if(coordinates["tooltip"] != null)
					return coordinates["tooltip"].ToString();
				return LegendText;
			}
			set { toolTipText = value; } 
		}

		#endregion

		#region --- Values and Properties ---
		internal double X0LCSE { get { return x0LCSE; } }
		internal double X1LCSE { get { return x1LCSE; } }
		// These coordinates present the LCS range
		internal double Y0LCS { get { return y0LCS; } }
		internal double Y1LCS { get { return y1LCS; } }
		// These y coordinates are affected by reference value
		internal double Y0LCSE 
		{
			get 
			{
				if(y0IsReferenceValue)
				{
					yMinValue = OwningSeries.ReferenceValue;
					DataDimension yDim = OwningSeries.DataDescriptors["y"].Value.Dimension;
					y0LCSE = yDim.Coordinate(yMinValue);
				}
				return y0LCSE; 
			} 
		}
		internal double Y1LCSE { get { return y1LCSE; } }
		internal double Y0LCSRange { get { return y0LCSRange; } }
		internal double Y1LCSRange { get { return y1LCSRange; } }
		/// <summary>
		/// Retrieves a value of a parameter of this <see cref="DataPoint"/> object.
		/// </summary>
		/// <param name="key">The key of the parameter.</param>
		/// <returns>The value of the parameter corresponding to this <paramref name="key" />.</returns>
		/// <remarks>
		/// If the parameter wasn't found on the <see cref="DataPoint"/> level, the <paramref name="key" /> is passed to the <see cref="Series.Parameter"/> property of the parent series.
		/// </remarks>
		public object Parameter(object key)
		{
			object param = coordinates[key];
			if(param == null)
				param  = OwningSeries.Parameter(key);
			return param;
		}

		internal void SetLCSRange(double y0, double y1)
		{
			y0LCSRange = y0;
			y1LCSRange = y1;
		}

		internal TargetArea TargetArea { get { return OwningSeries.TargetArea; } }

		private CoordinateSystem CoordSystem { get { return OwningSeries.CoordSystem; } }

		private DataDimension XDimension { get { return OwningSeries.XDimension; } }

		private DataDimension YDimension
		{
			get
			{
				DataDescriptor des = OwningSeries.DataDescriptors["y"];
				if(des != null)
					return des.Value.Dimension;
				des = OwningSeries.DataDescriptors["from"];
				if(des != null)
					return des.Value.Dimension;
				des = OwningSeries.DataDescriptors["low"];
				if(des != null)
					return des.Value.Dimension;
				des = OwningSeries.DataDescriptors["open"];
				if(des != null)
					return des.Value.Dimension;
				return DataDimension.StandardNumericDimension;
			}
		}


		#endregion

		#region --- Doughnut/Pie specific properties

		private		Vector3D Vx		{ get { return CoordSystem.XAxis.UnitVector;  } }
		private		Vector3D Vy		{ get { return -CoordSystem.ZAxis.UnitVector; } }
		private		Vector3D Vz		{ get { return CoordSystem.YAxis.UnitVector;  } }

		internal int Index { get { return index; } set { index = value; } }
		internal int OriginalIndex { get { return originalIndex; } set { originalIndex = value; } }
		internal bool ToRenderLabels { get { return toRenderLabels; } set { toRenderLabels = value; } }

			internal	Vector3D Center	
		{
			get 
			{
				return CoordSystem.ICS2WCS(new Vector3D(CoordSystem.XAxis.MaxValueICS/2,0,
					CoordSystem.ZAxis.MaxValueICS/2));
			}
		}
		
		internal double Radius			
		{
			get 
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		internal SeriesStyle EffectiveStyle
		{
			get
			{
				string styleName;
				if(StringProperty("styleName",out styleName))
				{
					SeriesStyle style = OwningChart.SeriesStyles[styleName];
					if(style != null)
						return style;
				}
				return OwningSeries.Style;
			}
}
		internal double RelativeShift	
		{
			get
			{
				double shift;
				if(NumericValue("shift",out shift))
					return shift;
				else
					return 0;
			}
		}

		internal double RelativeHeight	
		{
			get
			{
				double height = 1.0;
				if(GE.SupportsFeature("VariablePieHeight"))
				{
					if(NumericValue("height",out height))
					{
						double maxHeight = OwningSeries.MaxValue("height");
						if(maxHeight>0)
							height = height/maxHeight;
					}
				}
				return height;
			}
		}

		internal double RelativeWidth	
		{
			get
			{
				double width = 1.0;
				if(NumericValue("width",out width))
				{
					double maxWidth = OwningSeries.MaxValue("width");
					if(maxWidth>0)
						width = width/maxWidth;
				}
				return width;
			}
		}
	
		private void GetEfectiveRadii(out double innerRadius, out double outerRadius)
		{
			// Inner and outer radii of the whole slice
			SeriesStyle style = EffectiveStyle;
			double oRadWhole = Radius;
			double iRadWhole = style.RelativeInnerRadius*oRadWhole;

			// Normalized radius range
			double radiusRange0 = GetDoubleParameter("radiusRange0",0);
			double radiusRange1 = GetDoubleParameter("radiusRange1",1);

			// Correct for the radius range set for concentric composition
			double iRadCC = radiusRange0*oRadWhole + (1-radiusRange0)*iRadWhole;
			double oRadCC = radiusRange1*oRadWhole + (1-radiusRange1)*iRadWhole;

			// Correct for the relative width
			//	Centerline radius and halfWidth
			double clRad = (iRadCC + oRadCC)*0.5;
			double halfWidth = (oRadCC - iRadCC)*0.5;
			//	Correct the halfwidth and radii
			double relativeWidth = 1.0, width;
			if(NumericValue("width",out width))
			{
				double maxWidth = OwningSeries.MaxValue("width");
				relativeWidth = (maxWidth>0)? width/maxWidth:0;
			}
			halfWidth *= relativeWidth;

			innerRadius = clRad - halfWidth;
			outerRadius = clRad + halfWidth;
		}
	
		internal double EffectiveHeight
		{
			get
			{
				// Height defined by the style
				double effectiveHeight = Radius*EffectiveStyle.RelativeHeight;
				if(GE.SupportsFeature("VariablePieHeight"))
				{
					// If there is variable "height", we take relative height
					double height;
					if(NumericValue("height",out height))
					{
						double maxHeight = OwningSeries.MaxValue("height");
						if(maxHeight > 0)
							effectiveHeight = effectiveHeight*height/maxHeight;
					}
				}
				return effectiveHeight;
			}
		}
	
		internal double EffectiveLift
		{
			get
			{
				double effectiveLift = 0.0;
				if(GE.SupportsFeature("PieLift"))
				{
					// If there is variable "lift", we take relative height
					double lift;
					if(NumericValue("lift",out lift))
						effectiveLift = lift*Radius*EffectiveStyle.RelativeHeight;
				}
				return effectiveLift;
			}
		}
	
		private double EffectiveShift
		{
			get
			{
				double effectiveShift = 0.0;
				// If there is variable "shift", we take relative height
				double shift;
				if(NumericValue("shift",out shift))
					effectiveShift = shift*Radius;
				return effectiveShift;
			}
		}
	
		private double EffectiveInnerRadius
		{
			get
			{
				double iRad,oRad;
				GetEfectiveRadii(out iRad,out oRad);
				return iRad;
			}
		}

		private double EffectiveOuterRadius
		{
			get
			{
				double iRad,oRad;
				GetEfectiveRadii(out iRad,out oRad);
				return oRad;
			}
		}

		internal ChartColor EffectiveSurface
		{
			get
			{
				ChartColor eSurface = null;
				// Try "surface" parameter
				object srf = coordinates["surface"];
				// if not, try "color"
				if(srf==null)
					srf = coordinates["color"];
				if(srf != null && srf is ChartColor)
					eSurface = srf as ChartColor;
				else
				{
					// Try "surface" datapoint property in the owning Series
					srf = OwningSeries.DataPointParameters["surface"];
					if(srf != null && srf is ChartColor)
						eSurface = srf as ChartColor;
					else
					{
						// Try "color" parameter
						srf = coordinates["color"];
                        if (srf != null)
                        {
                            if (srf is string && (srf as string) != "")
                                srf = (new ColorConverter()).ConvertFromString((string)srf);
                            if (srf is Color)
                                eSurface = new ChartColor(0.5f, 6, (Color)srf);
                        }
						if (eSurface == null)
						{
							// If nothing elese worked, get it from the owning Series
							if(OwningSeries.Style.ChartKindCategory == ChartKindCategory.PieDoughnut &&
								index%OwningSeries.DataPoints.Count == 0)
								eSurface = OwningSeries.GetEffectiveSurface(index+1); // def 5232: avoid two consecuteve slices of the same color
							else
								eSurface = OwningSeries.GetEffectiveSurface(index);
						}
					}
				}
				
				object tObject = coordinates["transparency"];
				if(tObject != null && tObject is double && !OwningChart.InDesignMode)
				{
					double transparency = (double)tObject;
					Color color = Series.ModifyTransparency(eSurface.Color,transparency);
					return new ChartColor(eSurface.Reflection,eSurface.LogPhong,color);
				}
				else
					return eSurface;
			}
		}

		internal ChartColor EffectiveSecondarySurface
		{
			get
			{
				ChartColor eSurface = null;
				// Try "secondarySurface" property
				object srf = coordinates["secondarySurface"];
				if(srf != null && srf is ChartColor)
					eSurface = srf as ChartColor;
				else
				{
					// Try "secondaryColor" property
					srf = coordinates["secondaryColor"];
                    if (srf != null)
                    {
                        if (srf is string && (srf as string) != "")
                            srf = (new ColorConverter()).ConvertFromString((string)srf);
                        if (srf is Color)
                            eSurface = new ChartColor(0.5f, 6, (Color)srf);
                    }
                    if(eSurface == null)
                        // If nothing elese worked, get it from the owning Series
                        eSurface = OwningSeries.GetEffectiveSecondarySurface(index);
				}
				
				object tObject = coordinates["transparency"];
				if(tObject != null && tObject is double)
				{
					double transparency = (double)tObject;
					Color color = Series.ModifyTransparency(eSurface.Color,transparency);
					return new ChartColor(eSurface.Reflection,eSurface.LogPhong,color);
				}
				else
					return eSurface;
			}
		}

	#endregion

		#region --- Coordinates and Points ---
		#region --- DCS Coordinates ---

		internal object XDCS()			{ return xValue;  }
		internal object MinYDCS()			{ return yValue0; }
		internal object MaxYDCS()			{ return yValue1; }
		internal object ZDCS()			{ return OwningSeries.MinZDCS();  }
		internal object DCS(string param)	{ return Parameters[param];  }
		
		#endregion

		#region --- LCS Coordinates ---

		internal double MinXLCS()	{ return x0LCS;	}
		internal double MaxXLCS()	{ return x1LCS;	}
		internal double MinYLCS()	{ return y0LCS;	}
		internal double MaxYLCS()	{ return y1LCS;	}
		internal double MinZLCS()	{ return z0LCS;	}
		internal double MaxZLCS()	{ return z1LCS;	}
		internal double MinLCS(string param)	
		{
			DataDescriptor par = OwningSeries.DataDescriptors[param];
			if(par == null)
				throw new Exception("Series '" + OwningSeries.Name + "' doesn't have parameter '" + param +"'");
			return par.Value.Dimension.Coordinate(DCS(param));
		}
		
		internal double MaxLCS(string param)
		{
			DataDescriptor par = OwningSeries.DataDescriptors[param];
			if(par == null)
				throw new Exception("Series '" + OwningSeries.Name + "' doesn't have parameter '" + param +"'");
			object val = DCS(param);
			return par.Value.Dimension.Coordinate(val) + par.Value.Dimension.Width(val);
		}


		#endregion

		internal Vector3D AtLocal(double x, double y, double z)
		{
			if(OwningSeries.Style.IsLinear)
				return AtLocalLinear(x,y,z);
			else
				return AtLocalRadial(x,y,z);
		}

		private Vector3D AtLocalLinear(double xL, double yL, double zL)
		{
			return CoordSystem.ICS2WCS(AtLocalLinearICS(xL,yL,zL));
		}

		internal Vector3D AtLocalLinearICS(double xL, double yL, double zL)
		{
			Vector3D Vx = CoordSystem.XAxis.UnitVector;
			Vector3D Vy = CoordSystem.YAxis.UnitVector;
			Vector3D Vz = CoordSystem.ZAxis.UnitVector;
			double x0w,x1w, y0w,y1w, z0w,z1w;
			bool invertedY;
			ComputeBoundingBox(out x0w,out x1w,out y0w,out y1w, out z0w,out z1w, out invertedY);

			if(EffectiveStyle.ChartKindCategory == ChartKindCategory.Marker)
			{
				if(invertedY)
					y1w = y0w;
				else
					y0w = y1w;
			}
			// Fix zero-size dimension to accomodate direction vectors
			double pxl2 = 0.5/Mapping.Enlargement;
			if(x0w == x1w)
			{
				x0w -= pxl2;
				x1w += pxl2;
			}
			if(y0w == y1w)
			{
				y0w -= pxl2;
				y1w += pxl2;
			}
			if(z0w == z1w)
			{
				z0w -= pxl2;
				z1w += pxl2;
			}
			double xw = x0w*(1.0-xL) + x1w*xL;
			double yw = y0w*(1.0-yL) + y1w*yL;
			double zw = z0w*(1.0-zL) + z1w*zL;
			return new Vector3D(xw,yw,zw);
		}

		private Vector3D AtLocalRadial(double x, double y, double z)
		{
			// x is along radius
			// y is along the angle (circle)
			// z is along the height

			PieSegment PS = GetPieSegment();
			return PS.AtLocal(x,y,z);
		}

		internal Vector3D VxAtLocal(double x, double y, double z)
		{
			Vector3D Vd = AtLocal(x+0.001,y,z) - AtLocal(x-0.001,y,z);
			if(Vd.IsNull)
				return Vd;
			return Vd.Unit();
		}
	
		internal Vector3D VyAtLocal(double x, double y, double z)
		{
			Vector3D Vd = AtLocal(x,y+0.001,z) - AtLocal(x,y-0.001,z);
			if(Vd.IsNull)
				return Vd;
			return Vd.Unit();
		}

		internal Vector3D VzAtLocal(double x, double y, double z)
		{
			Vector3D Vd = AtLocal(x,y,z+0.001) - AtLocal(x,y,z-0.001);
			if(Vd.IsNull)
				return Vd;
			return Vd.Unit();
		}

		internal Vector3D LocalToWorld(Vector3D P)
		{
			return AtLocal(P.X,P.Y,P.Z);
		}

		internal Vector3D LocalToWorldDirection(Vector3D P, Vector3D dir)
		{
			Vector3D diff = LocalToWorld(P + dir*0.001)-LocalToWorld(P);
			if(!diff.IsNull)
				diff = diff.Unit();
			return diff;
		}
		#endregion

		#region --- Legend Interface ---

		bool			ILegendItemProvider.LegendItemVisible{ get {return true; } }
		LegendItemKind	ILegendItemProvider.LegendItemKind	{ get { return LegendItemKind.RectangleItem; } }
		string			ILegendItemProvider.LegendItemText	{ get { return LegendText; } }
		void			ILegendItemProvider.DrawLegendItemRectangle	(Graphics g, Rectangle rect)
		{
			ChartColor surface = EffectiveSurface;
			if(surface == null)
				return;
			Brush brush = new SolidBrush(surface.Color);
			g.FillRectangle(brush,rect);
			brush.Dispose();
		}

		double ILegendItemProvider.LegendItemCharacteristicValue
		{
			get
			{
				return Math.Abs(y1LCSE - y0LCSE);
			}
		}
		#endregion
		
		#region --- Getting and Setting DataValues and dataProperties ---

		internal bool NumericValue(string name, out double val)
		{
			val = 0;
			object v = coordinates[name];
			if(v == null)
				return false;
			if(v is double)
				val = (double)v;
			else if(v is int)
				val = (double)(int)v;
			else
				return false;
			return true;
		}

		internal bool ColorProperty(string name, out Color val)
		{
			val = System.Drawing.Color.FromArgb(0, 0, 0, 0);
			object v = coordinates[name];
			if (v == null)
				return false;

			if (v is Color)
			{
				val = (Color)v;
				return true;
			}
			else if (v is string && (v as string) != "")
			{
				val = (Color)(new ColorConverter()).ConvertFromString((string)v);
				return true;
			}

			return false;
		}

		internal bool StringProperty(string name, out string val)
		{
			val = "";
			object v = coordinates[name];
			if(v == null || !(v is string))
				return false;
			val = (string)v;
			return true;
		}

		/// <summary>
		/// Gets or sets the value of the parameter specified by name.
		/// </summary>
		/// <param name="name">The name.</param>
		public object this[string name] 
		{
			get 
			{ 
				return coordinates[name];
			}
			set 
			{ 
				coordinates.Remove(name);
				coordinates.Add(name,value);
			}
		}

		#endregion

		#region --- Rendering ---

        internal void Render(ref Geometry.DrawingBoard drawingBoard)
		{
			if(!Visible)
				return;
			if(OwningSeries.Style.IsLinear)
			{
				RenderLinear(ref drawingBoard);
			}
			else
			{
				toRenderLabels = true;
				RenderRadial();
			}
		}

		internal void ComputeBoundingBox(out double x0w, out double x1w, out double y0w, out double y1w, out double z0w, out double z1w, out bool invertedY)
		{// This is used for linear shapes only
			SeriesStyle style = EffectiveStyle;
			CoordinateSystem system = CoordSystem;

			double x0i = system.XAxis.LCS2ICS(x0LCSE);
			double x1i = system.XAxis.LCS2ICS(x1LCSE);
			double y0i = system.YAxis.LCS2ICS(y0LCSE);
			double y1i = system.YAxis.LCS2ICS(y1LCSE);

			invertedY = (y0LCSE > y1LCSE);

			x0w = Math.Min(x0i,x1i);
			x1w = Math.Max(x0i,x1i);
			y0w = Math.Min(y0i,y1i);
			y1w = Math.Max(y0i,y1i);

			double dxw = x1w-x0w;
			x0w += dxw*style.RelativeLeftSpace;
			x1w -= dxw*style.RelativeRightSpace;
			dxw = x1w-x0w;

			/* --- z coordinates computation ---
			 * 1. The block is within [z0, z0+zSpan], expressed in world coordinates.
			 * 2. If variableDepth then the depth is z-coordinate in the chart coordinates.
			 * 3. If not variable depth, the interval [z0, z0+zSpan] is corrected for front
			 *		and back space.
			 */
			double zSpan = OwningSeries.DZICS;
			double vDepth = Depth;
			z0w = OwningSeries.ICoordinateZ;
			if(!vDepth.Equals(double.NaN))
			{
				z1w = z0w + vDepth/OwningSeries.MaxValue("depth")*zSpan;
			}
			else
			// Instead of depth, we use the span, reduced for front and back space
			{
				z1w = z0w + OwningSeries.IWidthZ;
				double dzw = OwningSeries.DZICS;
				z0w += dzw*style.RelativeBackSpace;
				z1w -= dzw*style.RelativeFrontSpace;
				if(style.ForceSquareBase && style.ChartKind != ChartKind.Area)
				{
					dxw = x1w-x0w;
					dzw = z1w-z0w;
					if(dxw>dzw)
					{
						double xsw = (x0w+x1w)*0.5;
						x0w = xsw - 0.5*dzw;
						x1w = xsw + 0.5*dzw;
					}
					else
					{
						double zsw = (z0w+z1w)*0.5;
						z0w = zsw - 0.5*dxw;
						z1w = zsw + 0.5*dxw;
					}
				}
			}
			if(z0w>z1w)
			{
				double z=z0w; z0w = z1w; z1w = z;
			}

			// Correction of depth coordinate for 2D features

			if(	style.ChartKind == ChartKind.Area2D ||
				style.ChartKind == ChartKind.Bubble2D ||
				style.ChartKind == ChartKind.CandleStick ||
				style.ChartKind == ChartKind.HighLowOpenClose ||
				style.ChartKind == ChartKind.Line2D ||
				style.ChartKind == ChartKind.Rectangle)
			{
				double dz0 = 0.05*(z1w-z0w);
				z0w = (z0w + z1w)*0.5-dz0;
				z1w = z0w+2*dz0;
			}
			
		}

        private void RenderLinear(ref Geometry.DrawingBoard drawingBoard)
		{
			CoordinateSystem system = OwningSeries.CoordSystem;

			if(ChartBase.GetChartFromObject(OwningSeries)==null)
				return;

			Vector3D Vx = system.XAxis.UnitVector;
			Vector3D Vy = system.YAxis.UnitVector;
			Vector3D Vz = Vx.CrossProduct(Vy);

			SeriesStyle style = EffectiveStyle;

			// Surfaces

			ChartColor surface = EffectiveSurface;
			if(surface == null)
				return;
			if(yMinValue != yMaxValue && Y0LCSE>Y1LCSE)
				surface = EffectiveSecondarySurface;

			// Radius
			
			double radius = 0.0;
			object dppRadius = coordinates["size"];
			bool radiusFromSizeParameter = false;
			if(dppRadius != null)// && (dppRadius is double))
			{
				try // to convert to double
				{
					radius = (double)Convert.ChangeType(dppRadius,TypeCode.Double)/OwningSeries.MaxValue("size");;
					radiusFromSizeParameter = true;
				}
				catch
				{}
			}

			// ICS coordinates

			double x0Axis = CoordSystem.XAxis.MinValueICS;
			double x1Axis = CoordSystem.XAxis.MaxValueICS;
			double x0i,x1i, y0i,y1i, z0i,z1i;
			bool invertedY;

			ComputeBoundingBox(out x0i, out x1i, out y0i, out y1i, out z0i, out z1i, out invertedY);
			x0i = Math.Max(x0i,x0Axis);
			x1i = Math.Min(x1i,x1Axis);
			if(x0i >= x1i)
				return;

			double yLowI  = system.YAxis.LCS2ICS(Y0LCSRange);
			double yHighI = system.YAxis.LCS2ICS(Y1LCSRange);
			if(yLowI == yHighI)
			{
				yLowI -= 1;
				yHighI += 1;
			}

			// Data point types that create special area map objects and therefore
			// do not use standard "active object" strategy
			bool controlActiveObject = // style.ChartKind != ChartKind.Rectangle &&
				style.ChartKind != ChartKind.Bubble2D &&
				style.ChartKind != ChartKind.Marker;

			// Center at YLow in WCS
			Vector3D SLow = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,yLowI,(z0i+z1i)*0.5));
			// Center at YHigh in WCS
			Vector3D SHigh = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,yHighI,(z0i+z1i)*0.5));
			// Block vector
			Vector3D vectorH = SHigh - SLow;

			// Adjust values if top and bottom are too close
			double minimumDistance = 0.005*(system.YAxis.MaxValueLCS - system.YAxis.MinValueLCS);
			double y1lcse = Y1LCSE;
			if(Y0LCSE <= Y1LCSE && Y1LCSE - Y0LCSE < minimumDistance)
				y1lcse = Y0LCSE + minimumDistance;
			else if(Y0LCSE > Y1LCSE && Y0LCSE - Y1LCSE < minimumDistance)
				y1lcse = Y0LCSE - minimumDistance;

			// Center at Y0
			Vector3D S0 = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,system.YAxis.LCS2ICS(Y0LCSE),(z0i+z1i)*0.5));
			// Center at Y1
			Vector3D S1 = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,system.YAxis.LCS2ICS(y1lcse),(z0i+z1i)*0.5));

			// Base radius vectors
			//		x -radius vector
			Vector3D V1 = (system.ICS2WCS(new Vector3D(x1i,y0i,z0i)) - system.ICS2WCS(new Vector3D(x0i,y0i,z0i)))*0.5;
			//		z -radius vector
			Vector3D V2 = (system.ICS2WCS(new Vector3D(x1i,y0i,z0i)) - system.ICS2WCS(new Vector3D(x1i,y0i,z1i)))*0.5;

			double dzw = V2.Abs;
			if(controlActiveObject)
				GE.SetActiveObject(this);

			switch(style.ChartKind)
			{
				case ChartKind.Block:
					GE.CreatePrism(S0,S1,V1*Math.Sqrt(2),V2.Abs*Math.Sqrt(2),4,style.EdgeRadiusInPoints,surface);
					break;
				case ChartKind.Marker:
				{
					if(radius==0.0)
						radius = 0.33*(x1i-x0i)*0.5;
					if(OwningSeries.MarkerSizePts > 0)						
						radius = OwningSeries.CoordSystem.TargetArea.Mapping.FromPointToWorld/
							 (OwningSeries.CoordSystem.ICS2WCS(new Vector3D(1,0,0)) - OwningSeries.CoordSystem.ICS2WCS(new Vector3D(0,0,0))).Abs
							*OwningSeries.MarkerSizePts*0.5;
					if(style.IsRadar)
						S1 = CoordSystem.PlaneXY.LogicalToWorld(system.XAxis.LCS2ICS(X0LCSE),system.YAxis.LCS2ICS(Y1LCSE));
					else
					{
						double tol = radius;
						double xx = OwningSeries.XAxis.ICoordinate(xValue)+OwningSeries.XAxis.IWidth(xValue)*0.5;
						double yy = system.YAxis.LCS2ICS(Y1LCSE);
						double zz = (z0i+z1i)*0.5;
						S1 = new Vector3D(xx,yy,zz);
						if(S1.Y < -tol || 
							S1.Y > OwningSeries.YAxis.MaxValueICS+tol ||
							S1.X < -tol ||
							S1.X > OwningSeries.XAxis.MaxValueICS+tol )
							break;
						S1 = system.ICS2WCS(S1);
						Debug.WriteLine("Marker @WCS = " + S1.ToString());
					}
					if(ObjectTrackingEnabled)
					{
						Vector3D PP = Mapping.Map(S1);
						OwningChart.ObjectMapper.AddDataPointHotSpot((int)(PP.X),(int)(PP.Y),this);
					}
					string markerStyleName = MarkerStyleName;
					if(markerStyleName==string.Empty)
						markerStyleName = style.MarkerStyleName;
					surface = EffectiveSurface;
					double markerHeight = OwningSeries.IWidthZ*(1.0 - style.RelativeBackSpace - style.RelativeFrontSpace);
					//S1 = S1 + OwningSeries.ZAxis.UnitVector*(markerHeight*0.5);
					Marker marker = GE.CreateMarker(markerStyleName,Math.Min(radius,markerHeight/2),S1,EffectiveSurface);
					marker.Vx = CoordSystem.XAxis.UnitVectorWCS;//CoordSystem.LCS2WCS(new Vector3D(2,1,1)) - CoordSystem.LCS2WCS(new Vector3D(1,1,1)).Unit();
					marker.Vy = CoordSystem.YAxis.UnitVectorWCS;//CoordSystem.LCS2WCS(new Vector3D(1,2,1)) - CoordSystem.LCS2WCS(new Vector3D(1,1,1)).Unit();
					marker.Vz = CoordSystem.ZAxis.UnitVectorWCS;//CoordSystem.LCS2WCS(new Vector3D(1,1,2)) - CoordSystem.LCS2WCS(new Vector3D(1,1,1)).Unit();
					marker.Height = markerHeight;
					marker.IsTwoDimensional = TargetArea.IsTwoDimensional;
				}
					break;
				case ChartKind.Bubble:
				{
					double rx=0,ry=0,rz=0;
					if(radiusFromSizeParameter)
					{
						radius *= (x1i-x0i)*0.5;
						rx = radius;
						ry = radius;
						rz = radius;
					}
					else if(OwningSeries.MarkerSizePts > 0)		
					{
						radius = OwningSeries.CoordSystem.TargetArea.Mapping.FromPointToWorld/
							(OwningSeries.CoordSystem.ICS2WCS(new Vector3D(1,0,0)) - OwningSeries.CoordSystem.ICS2WCS(new Vector3D(0,0,0))).Abs
							*OwningSeries.MarkerSizePts;
						rx = radius;
						ry = radius;
						rz = radius;
					}
					else
					{
						MarkerStyle markerStyle = OwningChart.MarkerStyles[style.MarkerStyleName];
						if(markerStyle != null)
						{
							Vector3D vSize = markerStyle.MarkerSize;
							double rFac = OwningSeries.CoordSystem.TargetArea.Mapping.FromPointToWorld/
							(OwningSeries.CoordSystem.ICS2WCS(new Vector3D(1,0,0)) - OwningSeries.CoordSystem.ICS2WCS(new Vector3D(0,0,0))).Abs;
							rx = vSize.X * rFac / 2;
							ry = vSize.Y * rFac / 2;
							rz = vSize.Z * rFac / 2;
						}
						if(rx*ry*rz == 0)
						{
							radius = 0.33* (x1i-x0i)*0.5;
							rx = radius;
							ry = radius;
							rz = radius;
						}
					}

					if(style.IsRadar)
						S1 = CoordSystem.PlaneXY.LogicalToWorld(system.XAxis.LCS2ICS(X0LCSE),system.YAxis.LCS2ICS(Y1LCSE));
					else
					{
						double tol = radius;
						S1 = new Vector3D(OwningSeries.XAxis.ICoordinate(xValue)+OwningSeries.XAxis.IWidth(xValue)*0.5,system.YAxis.LCS2ICS(Y1LCSE),(z0i+z1i)*0.5);
						if(S1.Y < -tol || 
							S1.Y > OwningSeries.YAxis.MaxValueICS+tol ||
							S1.X < -tol ||
							S1.X > OwningSeries.XAxis.MaxValueICS+tol )
							break;
						S1 = system.ICS2WCS(S1);
						Debug.WriteLine("Bubble @WCS = " + S1.ToString());
					}
					GE.CreateEllipsoid(S1,Vx*rx,Vz*rz,ry,surface);
					if(ObjectTrackingEnabled)
					{
						Vector3D PP = Mapping.Map(S1);
						OwningChart.ObjectMapper.AddDataPointHotSpot((int)(PP.X),(int)(PP.Y),this);
					}
				}
					break;
				case ChartKind.Cylinder:
					GE.CreateCylinder(S0,S1,V1,dzw,style.EdgeRadiusInPoints,surface);
					break;
				case ChartKind.Prism3:
					GE.CreatePrism(S0,S1,V1,V2.Abs,3,style.EdgeRadiusInPoints,surface);
					break;
				case ChartKind.Hexagon:
					GE.CreatePrism(S0,S1,V1,V2.Abs,6,style.EdgeRadiusInPoints,surface);
					break;
				case ChartKind.Rectangle:
				{
					Vector3D P1 = system.ICS2WCS(new Vector3D(x0i,y0i,(z0i+z1i)*0.5)); 
					Vector3D P2 = system.ICS2WCS(new Vector3D(x1i,y1i,(z0i+z1i)*0.5)); 
					if(ObjectTrackingEnabled)
					{
						Vector3D PP1 = Mapping.Map(P1);
						Vector3D PP2 = Mapping.Map(P2);
						OwningChart.ObjectMapper.AddDataPointSegment((int)(PP1.X),(int)(PP1.Y),(int)(PP2.X),(int)(PP2.Y),this);
					}
					string gradientStyleName = GradientStyleName;
					if(gradientStyleName == string.Empty)
						gradientStyleName = style.GradientStyleName;
					GradientStyle gradientStyle = OwningChart.GradientStyles[gradientStyleName];
					if(gradientStyle == null)
					{
						ChartColor srf = EffectiveSurface;
						gradientStyle = new GradientStyle(gradientStyleName,GradientKind.None,srf.Color,srf.Color);
					}
					if(gradientStyle != null)
					{
						gradientStyle = (GradientStyle) gradientStyle.Clone();
						if(gradientStyle.EndColor.A == 0)
							gradientStyle.EndColor = EffectiveSecondarySurface.Color;
						if(gradientStyle.StartColor.A == 0)
							gradientStyle.StartColor = EffectiveSurface.Color;
					}
					// NB: This drawing board is too big, since it is created to cover whole x-y range of the series.
					// An optimization is possible to adjust the size of the drawing bord to the size of data point.
					drawingBoard = OwningSeries.CreateDrawingBoard();
					LineStyle2D LS = (LineStyle2D)OwningChart.LineStyles2D[style.BorderLineStyleName].Clone();
					if(LS != null && LS.Color.A == 0)
							LS.Color = OwningChart.Palette.TwoDObjectBorderColor;

					drawingBoard.SetActiveObject(this);
					drawingBoard.DrawRectangle(LS,gradientStyle,P1,P2); 
					drawingBoard.SetActiveObject(null);
				}
					break;
				case ChartKind.Bubble2D:
				{
					Vector3D P1 = S1 - Vx*(radius*0.5) - Vy*(radius*0.5);
					Vector3D P2 = S1 + Vx*(radius*0.5) + Vy*(radius*0.5);;
					if(radius==0.0)
						radius = 0.33;
					radius *= (x1i-x0i)*0.5;
					if(OwningSeries.MarkerSizePts > 0)						
						radius = OwningSeries.CoordSystem.TargetArea.Mapping.FromPointToWorld/
							(OwningSeries.CoordSystem.ICS2WCS(new Vector3D(1,0,0)) - OwningSeries.CoordSystem.ICS2WCS(new Vector3D(0,0,0))).Abs
							*OwningSeries.MarkerSizePts;
					if(drawingBoard == null)
						drawingBoard = OwningSeries.CreateDrawingBoard();
					drawingBoard.ClearDefaultActiveObject();
					drawingBoard.FillEllipse(new SolidBrush(surface.Color),S1,radius,radius,true);
					LineStyle2D LS = OwningChart.LineStyles2D[style.BorderLineStyleName];
					if(LS != null)
					{
						if(LS.Color.A == 0)
							LS.Color = OwningChart.Palette.TwoDObjectBorderColor;
						drawingBoard.DrawEllipse(LS,S1,radius,radius);
					}
					if(ObjectTrackingEnabled)
					{
						Vector3D PP = Mapping.Map(S1);
						Vector3D PPR = Mapping.Map(new Vector3D(S1.X+radius,S1.Y,S1.Z));
						int dist = (int)(PP-PPR).Abs;
						OwningChart.ObjectMapper.AddMarkerSegment((int)(PP.X),(int)(PP.Y),this,dist);
					}					
				}
					break;
				case ChartKind.Cone:
					GE.CreateCone(SLow,SLow+vectorH,V1,dzw,(Y0LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),(Y1LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),surface);
					break;
				case ChartKind.Paraboloid:
					GE.CreateParaboloid(SLow,SLow+vectorH,V1,dzw,(Y0LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),(Y1LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),surface);
					break;
				case ChartKind.Pyramid:
					GE.CreatePyramid(SLow,SLow+vectorH,V1,dzw,(Y0LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),(Y1LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),4,surface);
					break;
				case ChartKind.Pyramid3:
					GE.CreatePyramid(SLow,SLow+vectorH,V1,dzw,(Y0LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),(Y1LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),3,surface);
					break;
				case ChartKind.Pyramid6:
					GE.CreatePyramid(SLow,SLow+vectorH,V1,dzw,(Y0LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),(Y1LCSE-Y0LCSRange)/(Y1LCSRange-Y0LCSRange),6,surface);
					break;	

				case ChartKind.CandleStick:
				{
					double LowValue = Low;
					double HighValue = High;
					if(LowValue.Equals(double.NaN) || HighValue.Equals(double.NaN))
						return;
					Vector3D P1 = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,system.YAxis.LCS2ICS(y0LCSE),(z0i+z1i)*0.5));
					Vector3D P2 = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,system.YAxis.LCS2ICS(y1LCSE),(z0i+z1i)*0.5));
					if(drawingBoard == null)
						drawingBoard = OwningSeries.CreateDrawingBoard();
					DrawCandleStick(P1,P2,x0i,x1i,drawingBoard);					
				}
					break;
				case ChartKind.HighLowOpenClose:
				{
					Vector3D P1 = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,system.YAxis.LCS2ICS(y0LCSE),(z0i+z1i)*0.5));
					Vector3D P2 = system.ICS2WCS(new Vector3D((x0i+x1i)*0.5,system.YAxis.LCS2ICS(y1LCSE),(z0i+z1i)*0.5));
					if(drawingBoard == null)
						drawingBoard = OwningSeries.CreateDrawingBoard();
					DrawHighLowOpenClose(P1,P2,x0i,x1i,drawingBoard);					
				}
					break;
				default: break;
			}

			if(controlActiveObject)
				GE.SetActiveObject(null);
		}

		private bool ObjectTrackingEnabled { get { return OwningSeries.ObjectTrackingEnabled; } }

			private Mapping Mapping	{ get { return CoordSystem.TargetArea.Mapping; } }

		internal void RenderRadial()
		{

			PieSegment PS = GetPieSegment();
            if (PS.Alpha0 >= PS.Alpha1)
                return;
			GE.SetActiveObject(this);
			GE.Add(PS);
			GE.SetActiveObject(null);
			
			if(Mapping.Kind == ProjectionKind.TwoDimensional)
			{
				// Add lines arround the segment
				Vector3D[] innerLine = PS.GetInnerLine(0.2,true);
				Vector3D[] outerLine = PS.GetOuterLine(0.2,true);
				TargetCoordinateRange cr = new TargetCoordinateRange();
				for(int i=0; i<innerLine.Length; i++)
					cr.Include(innerLine[i]);
				for(int i=0; i<outerLine.Length; i++)
					cr.Include(outerLine[i]);
		
				Vector3D BV0 = new Vector3D(cr.X0,cr.Y1+1,cr.Z0);
				Vector3D BVx = new Vector3D(cr.X1-cr.X0,0,0);
				Vector3D BVy = new Vector3D(0,0,cr.Z1-cr.Z0);
				Geometry.DrawingBoard B = GE.CreateDrawingBoard(BV0, BVx, BVy);
				B.Grow(5);
				B.Reflection = 0.0;
				LineStyle2D LS = (LineStyle2D)OwningChart.LineStyles2D[EffectiveStyle.BorderLineStyleName].Clone();
				if(LS != null && LS.Color.A == 0)
					LS.Color = OwningChart.Palette.TwoDObjectBorderColor;

				for(int i=1; i<innerLine.Length; i++)
					B.DrawLine(LS,innerLine[i-1],innerLine[i]);
				for(int i=1; i<outerLine.Length; i++)
					B.DrawLine(LS,outerLine[i-1],outerLine[i]);
				B.DrawLine(LS,innerLine[0],outerLine[0]);
				B.DrawLine(LS,innerLine[innerLine.Length-1],outerLine[outerLine.Length-1]);
				//B.DrawPieSegment(LS,S.X+growAmount/2,sz.Z-S.Z+growAmount/2, effectiveInnerRadius, effectiveOuterRadius,	shift, low, high);
			}

//			Vector3D Vh = Vz*height;
//			GE.CreatePieSegment(low, high, S, S + Vh, Vx.Unit() * effectiveOuterRadius, effectiveInnerRadius,
//				outerEdgeSmoothingRadius, innerEdgeSmoothingRadius, surface);
//			GE.SetActiveObject(null);
			
		}

		
		internal PieSegment GetPieSegment()
		{
			Vector3D S = Center;
			SeriesStyle style = EffectiveStyle;

			Axis Ax = CoordSystem.XAxis;
			Axis Ay = CoordSystem.YAxis;
			Axis Az = CoordSystem.ZAxis;

			// Mapping to the world C.S.
			Vector3D sz = Mapping.DomainSize;

			ChartColor surface = EffectiveSurface;

			double outerRadius = EffectiveOuterRadius;
			double innerRadius = EffectiveInnerRadius;
			double dRadius = outerRadius - innerRadius;
			double height = EffectiveHeight;
			double shift = EffectiveShift;
			double lift = EffectiveLift;

			double innerEdgeSmoothingRadius = style.RelativeInnerEdgeSmoothingRadius*dRadius;
			double outerEdgeSmoothingRadius = style.RelativeOuterEdgeSmoothingRadius*dRadius;
			if(innerEdgeSmoothingRadius<=0.0) innerEdgeSmoothingRadius = 0.1;
			if(outerEdgeSmoothingRadius<=0.0) outerEdgeSmoothingRadius = 0.1;
			innerEdgeSmoothingRadius = Math.Min(innerEdgeSmoothingRadius,height);
			outerEdgeSmoothingRadius = Math.Min(outerEdgeSmoothingRadius,height);
			double sumSmoothingRadius = innerEdgeSmoothingRadius+outerEdgeSmoothingRadius;
			if(sumSmoothingRadius>dRadius)
			{
				innerEdgeSmoothingRadius *= dRadius/sumSmoothingRadius;
				outerEdgeSmoothingRadius *= dRadius/sumSmoothingRadius;
			}

			dRadius = EffectiveOuterRadius - EffectiveInnerRadius;
			// Fix smoothing radius for thin segments
			if(innerEdgeSmoothingRadius + outerEdgeSmoothingRadius > dRadius)
			{
				double inner = innerEdgeSmoothingRadius/(innerEdgeSmoothingRadius + outerEdgeSmoothingRadius)*dRadius;
				double outer = outerEdgeSmoothingRadius/(innerEdgeSmoothingRadius + outerEdgeSmoothingRadius)*dRadius;
				innerEdgeSmoothingRadius = inner;
				outerEdgeSmoothingRadius = outer;
			}

			double low1;
			double high1;
			if(Y1LCSRange <= 0) // all data points == 0
			{
				low1 = 0;
				high1 = 0;
			}
			else
			{
				low1 = Y0LCSE*2*Math.PI/Y1LCSRange+0.01;
				high1 = Y1LCSE*2*Math.PI/Y1LCSRange+0.01;
			}
			double sa = OwningSeries.Style.FirstSegmentStart*Math.PI/180.0;
			double low = sa - high1;
			double high = sa - low1;
			double effectiveInnerRadius = EffectiveInnerRadius;
			double effectiveOuterRadius = EffectiveOuterRadius;
			
			// Process shift and lift params
			double sAlpha = (low+high)/2;
			Vector3D Vs = Vx.Unit()*Math.Cos(sAlpha) + Vy.Unit()*Math.Sin(sAlpha);
			S = S + Vs*shift + Vz.Unit()*lift;

			Vector3D Vh = Vz*height;
			PieSegment PS = new PieSegment(low,high, S, S+Vh, Vx.Unit() * effectiveOuterRadius, effectiveInnerRadius,
				outerEdgeSmoothingRadius, innerEdgeSmoothingRadius, surface);
			return PS;
		}

		private void DrawCandleStick(Vector3D P1, Vector3D P2, double x0w, double x1w, Geometry.DrawingBoard drawingBoard)
		{
			LineStyle2D LS = OwningChart.LineStyles2D[EffectiveStyle.BorderLineStyleName];
			if(LS != null && LS.Color.A == 0)
			{
				LS = (LineStyle2D)LS.Clone();
				LS.Color = OwningChart.Palette.TwoDObjectBorderColor;
			}
			Pen pen = (LS!=null)? LS.Pen:null;
					
			double OpenValue,CloseValue,LowValue,HighValue;
			if(	!NumericValue("open",out OpenValue) ||
				!NumericValue("close",out CloseValue) ||
				!NumericValue("low",out LowValue) ||
				!NumericValue("high",out HighValue)
				)
				return;

			ChartColor ss = (OpenValue < CloseValue)? EffectiveSurface as ChartColor:EffectiveSecondarySurface as ChartColor;
			SolidBrush brush = (ss != null)? new SolidBrush(ss.Color):null;

			Vector3D Vx = OwningSeries.CoordSystem.XAxis.UnitVector;

			double dh = OwningChart.FromPointToWorld*8;
			dh = Math.Min(dh,(x1w-x0w)*0.5);
			
			if(pen != null)
			{
				drawingBoard.DrawLine(pen,P1 - Vx*(dh*0.5), P1 + Vx*(dh*0.5),false);
			}

			if(LowValue == HighValue)
				return;

			double a = (OpenValue-LowValue)/(HighValue-LowValue);
			Vector3D VOpen = P2*a + P1*(1.0-a);
			a = (CloseValue-LowValue)/(HighValue-LowValue);
			Vector3D VClose = P2*a + P1*(1.0-a);

			if(pen!=null)
			{
				drawingBoard.DrawLine(pen,P1,P2,false);
				drawingBoard.DrawLine(pen,P2 - Vx*(dh*0.5), P2 + Vx*(dh*0.5),false);
			}
			
			if(brush != null)
				drawingBoard.FillRectangle(brush,VOpen - Vx*dh, VClose + Vx*dh, true);
			if(pen != null)
				drawingBoard.DrawRectangle(pen,VOpen - Vx*dh, VClose + Vx*dh,false);
		}

        private void DrawHighLowOpenClose(Vector3D P1, Vector3D P2, double x0w, double x1w, Geometry.DrawingBoard drawingBoard)
		{
			LineStyle2D LS = OwningChart.LineStyles2D[EffectiveStyle.BorderLineStyleName];
			if(LS==null)
				return;
			if(LS.Color.A == 0)
			{
				LS = (LineStyle2D)LS.Clone();
				LS.Color = OwningChart.Palette.TwoDObjectBorderColor;
			}
					
			double OpenValue = Open, CloseValue = Close, LowValue=Low, HighValue=High;
			if(	OpenValue.Equals(double.NaN) ||
				CloseValue.Equals(double.NaN) ||
				LowValue.Equals(double.NaN) ||
				HighValue.Equals(double.NaN)
				)
				return;

			Pen pen = LS.Pen;

			Vector3D Vx = OwningSeries.CoordSystem.XAxis.UnitVector;

			double dh = OwningChart.FromPointToWorld*8;
			dh = Math.Min(dh,(x1w-x0w)*0.5);

			if(LowValue == HighValue)
			{
				drawingBoard.DrawLine(pen,P1-Vx*dh,P1+Vx*dh,false);
				return;
			}
		
			double a = (OpenValue-LowValue)/(HighValue-LowValue);
			Vector3D VOpen = P2*a + P1*(1.0-a);
			a = (CloseValue-LowValue)/(HighValue-LowValue);
			Vector3D VClose = P2*a + P1*(1.0-a);

			drawingBoard.DrawLine(pen,P1,P2,false);
			drawingBoard.DrawLine(pen,VOpen - Vx*dh,VOpen,false);
			drawingBoard.DrawLine(pen,VClose,VClose+Vx*dh,false);
		}

		#endregion

		#region --- Building ---

		internal bool StackOn(DataPoint dp)
		{
			if(yMinValue.GetType() != dp.yMinValue.GetType())
				return false;
			y0IsReferenceValue = false;
			
			DataDimension yDim = YDimension;

			if(OwningSeries.IsRange)
				y1LCSE = dp.y1LCSE+(y1LCS-y0LCS);
			else
				y1LCSE = dp.y1LCSE+y0LCS;
			y0LCSE = dp.y1LCSE;
			
			return true;
		}

		internal void IgnoreReferenceValue()
		{
			y0LCSE = 0;
			y0LCSRange = 0;
		}

		internal void Scale100(DataPoint dp)
		{
			double ys = dp.Y1LCSE;
			y0LCSE = y0LCSE/ys*100;
			y1LCSE = y1LCSE/ys*100;
			y0LCSRange = 0;
			y1LCSRange = 100;
		}

		internal void AdjustXCoordinate(double x0P, double x1P)
		{
			// We adjust this to accomodate merged composition kind using
			// series normalized x-coordinates (between 0 and 1)
			x0LCSE = x0LCS; 
			x1LCSE = x1LCS; 
			double dx = x1LCSE-x0LCSE;
			double xp = x0LCSE;
			x0LCSE = xp + x0P*dx;
			x1LCSE = xp + x1P*dx;
		}

		internal void BindParameters()
		{
			// Missing point through parameter
			object isMissingParam = coordinates["isMissing"];
			if(isMissingParam != null && (isMissingParam is bool) && (bool)isMissingParam)
				isMissing = true;

			// By the time this function is called, DCS coordinates are set.
			// This function sets LCS coordinates of the point

			DataDimension xDim, yDim = null;
			y0IsReferenceValue = false;

			// x -LCS coordinates

			xValue = coordinates["x"];
			if(xValue != null)
			{
				xDim = XDimension;
				x0LCS = xDim.Coordinate(xValue);
				x1LCS = x0LCS + xDim.Width(xValue);				
			}
			else
			{
				// The case when x-coordinate is not known will not be considered 
				// invalid since some chart types don't require x.
				x0LCS = 0;
				x1LCS = 0;
			}
			// For now the effective x LCS range has not been changed
			x0LCSE = x0LCS;
			x1LCSE = x1LCS;

			// y -DCS and LCS coordinates

			yValue1 = null;
			if(OwningSeries.IsRange)
			{
				if((yValue0 = coordinates["from"]) == null)
					throw new Exception("The range type series '" + OwningSeries.Name + "': value 'from' is not defined");
				if((yValue1 = coordinates["to"]) == null)
					throw new Exception("The range type series '" + OwningSeries.Name + "': value 'to' is not defined");
				yDim = OwningSeries.DataDescriptors["from"].Value.Dimension;
				y0LCSE = yDim.Coordinate(yValue0) + 0.5*yDim.Width(yValue0);
				y1LCSE = yDim.Coordinate(yValue1) + 0.5*yDim.Width(yValue1);
				if(y0LCSE <= y1LCSE)
				{
					yMinValue = yValue0;
					yMaxValue = yValue1;
				}
				else
				{
					yMaxValue = yValue0;
					yMinValue = yValue1;
				}
				y0LCS = y0LCSE;
				y1LCS = y1LCSE;
			}
			else
			{
				if((yValue0 = coordinates["low"]) != null)
				{
					yDim = OwningSeries.DataDescriptors["low"].Value.Dimension;
					yValue1 = coordinates["high"];
					if(yValue1 == null)
						throw new Exception("Series '" + OwningSeries.Name + "': undefined parameter 'high'");
					yMinValue = yValue0;
					yMaxValue = yValue1;
					y0LCSE = yDim.Coordinate(yValue0);
					y1LCSE = yDim.Coordinate(yValue1);
					y0LCS = y0LCSE;
					y1LCS = y1LCSE;
				}
				else
				{
					if((yValue0 = coordinates["y"]) == null)
						throw new Exception("Series '" + OwningSeries.Name + "': value 'y' is not defined");

					yDim = OwningSeries.DataDescriptors["y"].Value.Dimension;
					// Not a range type of series, so the values are equal
					yValue1 = yValue0;
					y0LCS = yDim.Coordinate(yValue0);
					y1LCS = y0LCS+yDim.Width(yValue0);
					// Calculate the representation values affected by the reference point
					if(OwningSeries.Style.ChartKindCategory != ChartKindCategory.PieDoughnut)
					{
						double yRef = yDim.Coordinate(OwningSeries.ReferenceValue);

						yMinValue = OwningSeries.ReferenceValue;
						yMaxValue = yValue1;
						y0LCSE = yDim.Coordinate(yMinValue);
						y1LCSE = yDim.Coordinate(yMaxValue)+0.5*yDim.Width(yMaxValue);
						y0IsReferenceValue = true;
					}
					else
					{
						yMinValue = 0;
						yMaxValue = yValue1;
						y0LCSE = yDim.Coordinate(yMinValue)+0.5*yDim.Width(yMinValue);
						y1LCSE = yDim.Coordinate(yMaxValue)+0.5*yDim.Width(yMaxValue);
					}
				}
			}

			// For now we don't have knowledge of other series so the range is just initialized.

			y0LCSRange = y0LCSE;
			y1LCSRange = y1LCSE;
		}
		
		internal override void Build()
		{
			if(x1LCS <= x0LCS)
			{
				x0LCS = x0LCS-OwningSeries.ScatterPointSize*0.5;
				x1LCS = x1LCS+OwningSeries.ScatterPointSize*0.5;
			}
			x0LCSE = x0LCS;
			x1LCSE = x1LCS;
			z0LCS = OwningSeries.MinZLCS();
			z1LCS = OwningSeries.MaxZLCS();
		}
	
		#endregion
	}
}
