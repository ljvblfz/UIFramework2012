using System;
using System.Drawing;
using System.IO;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// This interface defines core functionality of charting component. This interface is implemented by 
	/// ComponentArt products WinChart, WebChart and SqlChart.
	/// </summary>
	public interface IChart
	{
		#region ==== COMMON =====

		#region ======== Design Time Properties ========

		#region --- Appearance Properties ---

		ChartFrame Frame { get; set; }
		StringAlignment TextAlignment { get; set; }

		/// <summary>
		/// Overriden base class property.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		///		Gets or sets the frame title position.
		/// </summary>
		FrameTextPosition TextPosition { get; set; }

		/// <summary>
		///		Gets or sets the value indicating whether the frame title text has a shade.
		/// </summary>
		bool TextShade { get; set; }

		/// <summary>
		///		Gets or sets the background gradient ending color.
		/// </summary>
		/// <remarks>
		///		When the chart background is rendered as gradient, i.e. the <see cref="BackGradientKind"/>
		///		is not set to <see cref="GradientKind.None"/>, two colors are used: <b>BackColor</b> and this color. Default setting
		///		for these colors is <see cref="Color.Transparent"/>. In that case, special <see cref="Palette"/> colors <see cref="Palette.BackgroundColor"/>
		///		and <see cref="Palette.BackgroundEndingColor"/> are used for gradient. To override
		///		palette color, set this property to value different from <see cref="Color.Transparent"/>.
		/// </remarks>
		Color BackGradientEndingColor {	get; set; }

		/// <summary>
		///		Gets or sets the background gradient kind.
		/// </summary>
		/// <remarks>
		///   If this property is <see cref="GradientKind.None"/>, <see cref="Windows.BackColor"/> is used 
		///   to paint the background, otherwise <see cref="GradientKind.None"/> and
		///   <see cref="WinChart.BackGradientEndingColor"/> are used for gradient. If any of these
		///   two properties is set to <see cref="Color.Transparent"/>, the corresponding special color from the
		///   <see cref="Palette"/> is used.
		/// </remarks>
		GradientKind BackGradientKind {	get; set; }

		#endregion
	
		#region --- Styles Properties ---

		/// <summary>
		/// Gets the collection of label styles contained within the chart.
		/// </summary>
		LabelStyleCollection LabelStyles {	get; }

		/// <summary>
		/// Gets the collection of text styles contained within the chart.
		/// </summary>
		TextStyleCollection TextStyles { get; }

		/// <summary>
		/// Gets the collection of data-point label styles contained within the chart.
		/// </summary>
		DataPointLabelStyleCollection DataPointLabelStyles { get; }

		/// <summary>
		/// Gets the collection of series styles contained within the chart.
		/// </summary>
		SeriesStyleCollection SeriesStyles { get; }

		/// <summary>
		/// Gets the collection of color palettes contained within the chart.
		/// </summary>
		PaletteCollection Palettes { get; }

		/// <summary>
		/// Gets or sets the palette used in the chart.
		/// </summary>
		string SelectedPaletteName { get; set; } 

		/// <summary>
		/// Gets the collection of line styles contained within the chart.
		/// </summary>
		LineStyleCollection LineStyles { get; }

		/// <summary>
		/// Gets the collection of 2D line styles contained within the chart.
		/// </summary>
		LineStyle2DCollection LineStyles2D { get; }
		
		/// <summary>
		/// Gets the collection of gradient styles contained within the chart.
		/// </summary>
		GradientStyleCollection GradientStyles { get; }
		
		/// <summary>
		/// Gets the collection of marker styles contained within the chart.
		/// </summary>
		MarkerStyleCollection MarkerStyles { get; }
		
		/// <summary>
		/// Gets the collection of tickmark styles contained within the chart.
		/// </summary>
		TickMarkStyleCollection TickMarkStyles { get; }
		
		/// <summary>
		/// Gets the collection of tickmark styles contained within the chart.
		/// </summary>
		TextBoxStyleCollection TextBoxStyles { get; }

#endregion

		#region --- Chart Contents Properties ---
		#region --- Free Annotations ---

		/// <summary>
		/// Creates free annotation containing given text, using the specified style and hooked to the 
		/// specified anchor point.
		/// </summary>
		ChartTextBox CreateAnnotation(string text, string styleName, TextAnchor anchorPoint);

		/// <summary>
		/// Creates free annotation containing given text, using the specified style and hooked to the 
		/// specified anchor point.
		/// </summary>
		ChartTextBox CreateAnnotation(string text, TextBoxStyleKind styleKind, TextAnchor anchorPoint);

		#endregion

		/// <summary>
		/// Gets or sets the <see cref="Legend"/> object of the chart.
		/// </summary>
		Legend Legend { get; set; }

		/// <summary>
		/// Gets or sets the collection of secondary legends.
		/// </summary>
		LegendCollection SecondaryLegends { get; set; }

		/// <summary>
		/// Gets the collection of input variables contained within the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is collection of <see cref="InputVariable"/> objects populated by methods
		///     <see cref="WinChart.DefineValue"/>,
		///     <see cref="WinChart.DefineAsExpression"/> and
		///     <see cref="WinChart.DefineValuePath"/>.
		///   </para>
		///   <para>
		///     For more on input variables see topic "Data Binding" in "Basic Concepts"
		///     and "Advanced Data Binding" in "Advanced Concepts".
		///   </para>
		/// </remarks>
		InputVariableCollection InputVariables { get; }

		/// <summary>
		/// Gets the collection of series contained in the first level of the series tree.
		/// </summary>
		SeriesCollection Series { get; }

		/// <summary>
		/// Gets or sets the number of data points simulated in design time.
		/// </summary>
		int NumberOfSimulatedDataPoints { get; set; }

		/// <summary>
		/// Default style of the chart.
		/// </summary>
		string MainStyle { get; set; }
		/// <summary>
		///   Gets or sets the composition kind of the root series.
		/// </summary>
		/// <remarks>
		///    Composition kind defines the way series that belong to the root composite series
		///    are combined in the chart. See <see cref="CompositionKind"/> for more information 
		///    on series composition.
		/// </remarks>
		CompositionKind CompositionKind { get; set; }

		/// <summary>
		///		Gets or sets the main coordinate system of the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///		The main coordinate system of the chart is the <see cref="CompositeSeries.CoordSystem"/>
		///		property of the root <see cref="CompositeSeries"/>. Often this is the only coordinate system in the chart.
		///		It is available for modification in design time using wizard or property view.
		///	  </para>
		///	  <para>
		///	    When <see cref="CompositionKind"/> is <see cref="CompositionKind.MultiSystem"/> the main coordinate system
		///	    hosts multiple coordinate systems, one for each <see cref="Series"/> or
		///	    <see cref="CompositeSeries"/> child of the root series hierarchy node. In that case
		///	    position and size of children systems are expressed in coordinates of the main system and 
		///	    the main system is invisible.
		///	  </para>
		///	  <para>
		///	    When <see cref="CompositionKind"/> is <see cref="CompositionKind.MultiArea"/>
		///	    each area has its own coordinate system and the main coordinate system is ignored.
		///	  </para>
		/// </remarks>
		/// <summary>
		///		Main coordinate system of the chart.
		/// </summary>
		CoordinateSystem CoordinateSystem { get; set; }
		/// <summary>
		/// Gets or sets the value indicating whether the y-axis of the main coordinate system is logarithmic.
		/// </summary>
		bool IsLogarithmic { get; set ; }

		/// <summary>
		/// Gets or sets the logarithm base of this chart.
		/// </summary>
		int LogBase { get; set; }

		/// <summary>
		/// Gets the collection of lights contained within the chart.
		/// </summary>
		LightCollection Lights { get; }

		/// <summary>
		/// Gets or sets the rendering precision of the chart.
		/// </summary>
		/// <remarks>
		/// Rendering precision is the maximum distance between a rendered smooth surface or line and
		/// its theoretical position in coordinate system of the target bitmap. The unit is 1 pixel. 
		/// Values above 0.5 give coarse and faster rendering, value 0.1 is considered fine. (Note that
		/// ComponentArt chart uses sub-pixel sampling where needed to obtain smooth lines and surfaces.)
		/// </remarks>
		double RenderingPrecision { get; set; }

		/// <summary>
		///		Gets or sets a <see cref="Mapping"/> object to the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///		The <see cref="Mapping"/> object defines mapping from thw World Coordinate System (WCS)
		///		into Target Coordinate System (TCS). We also call this "projection". Read more on coordinates
		///		in topic "Coordinates and Coordinate Systems" in section "Advanced Concepts".
		///	  </para>
		/// </remarks>
		Mapping View { get; set; }

		/// <summary>
		/// Collection of chart titles.
		/// </summary>
		ChartTitleCollection Titles { get; }
		
		#endregion

		#endregion

		/// <summary>
		/// Checks if the chart object supports the specified feature.
		/// </summary>
		/// <param name="featureName">Feature name.</param>
		/// <returns>True if the feature is supported.</returns>
		/// <remarks>
		/// The feature set supported by a chart depends on the geometric engine. There are two features
		/// supported by the HighQualityRendering engine but not 
		/// supported by the HighSpeedRendering engine: "VariablePieHeight" and "PieLift".
		/// </remarks>
		bool SupportsFeature(string featureName);
 
        /// <summary>
        /// Gets or sets automatic margins resizing to fit axis labels.
        /// </summary>
        bool ResizeMarginsToFitLabels { get; set; }
		
		#region --- Data Binding ---

		/// <summary>
		/// 
		/// </summary>
		SeriesStyleKind MainStyleKind { get; set; }

		/// <summary>
		/// Defines value of an input variable.
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="obj"> Value <see cref="object"/>.</param>
		/// <remarks>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		/// </remarks>
		void DefineValue(string name, object obj);

		/// <summary>
		/// Defines value of an input variable.
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="obj"> Value <see cref="object"/>.</param>
		/// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
		/// <remarks>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		///   </para>
		///   <para>
		///     For non-standard input data the <see cref="DataDimension"/> object handling 
		///     input values is provided in this function call. The <see cref="DataDimension"/>
		///     object maps input values (in Data Coordinate System - DCS) into Logical 
		///     Coordinate System (LCS), which is the first step in allocating space that thata 
		///     points will ocupy in the charting coordinate system. For more about this, see
		///     "Coordinates and Coordinate Systems" in "Advanced Topics".
		///   </para>
		/// </remarks>
		void DefineValue(string name, object obj, DataDimension dimension);
		
		/// <summary>
		///		Defines value of an input variable as expression.
		/// </summary>
		/// <param name="name">Input variable name.</param>
		/// <param name="expression">String representing the expression.</param>
		/// <remarks>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		/// </remarks>
		void DefineAsExpression(string name, string expression);

		/// <summary>
		///		Defines value of an input variable as expression and provides <see cref="DataDimension"/>
		///		that handles values of the input variable.
		/// </summary>
		/// <param name="name">Input variable name.</param>
		/// <param name="expression">String representing the expression.</param>
		/// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
		/// <remarks>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		///   </para>
		///   <para>
		///     For non-standard input data the <see cref="DataDimension"/> object handling 
		///     input values is provided in this function call. The <see cref="DataDimension"/>
		///     object maps input values (in Data Coordinate System - DCS) into Logical 
		///     Coordinate System (LCS), which is the first step in allocating space that thata 
		///     points will ocupy in the charting coordinate system. For more about this, see
		///     "Coordinates and Coordinate Systems" in "Advanced Topics".
		///   </para>
		/// </remarks>
		/// 
		void DefineAsExpression(string name, string expression, DataDimension dimension);

		/// <summary>
		///		Defines value path of an input variable within data source object.
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="valuePath">String representing the value path.</param>
		/// <remarks>
		///   <para>
		///     This method is used when the <see cref="WinChart.DataSource"/> object is used
		///     to provide data for the chart. Since data source may have multiple tables
		///     and each table may have multiple columns, the value path is used to select table
		///     and column in format "tablename.columnname".
		///   </para>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts".
		///   </para>
		/// </remarks>
		void DefineValuePath(string name, string valuePath);

		/// <summary>
		/// Defines value path of an input variable within data source object and variable dimension
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="valuePath">String representing the value path.</param>
		/// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
		/// <remarks>
		///   <para>
		///     This method is used when the <see cref="WinChart.DataSource"/> object is used
		///     to provide data for the chart. Since data source may have multiple tables
		///     and each table may have multiple columns, the value path is used to select table
		///     and column in format "tablename.columnname".
		///   </para>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts".
		///   </para>
		///   <para>
		///     For non-standard input data the <see cref="DataDimension"/> object handling 
		///     input values is provided in this function call. The <see cref="DataDimension"/>
		///     object maps input values (in Data Coordinate System - DCS) into Logical 
		///     Coordinate System (LCS), which is the first step in allocating space that thata 
		///     points will ocupy in the charting coordinate system. For more about this, see
		///     "Coordinates and Coordinate Systems" in "Advanced Topics".
		///   </para>
		/// </remarks>
		void DefineValuePath(string name, string valuePath, DataDimension dimension);

        /// <summary>
        /// Performs data binding and builds chart object model
        /// </summary>
        void DataBind();

#endregion

		#region --- Reference value ---
		/// <summary>
		/// Gets or sets the reference value.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is a base value along the y-axis of the charts. All bars, as well as areas
		///     are rendered with this base values. The type of this object has to be the type 
		///     of y-values of the chart.
		///   </para>
		///   <para>
		///     Often the y-values are numeric and default reference value is 0. If values
		///     are from a narrow range, say between 200 and 210, you may choose reference value
		///     200. The same kind of operation makes sense for other types of y-values.
		///   </para>
		///   <para>
		///     Reference value is handy when there is a baseline value (like average) and we want
		///     values below the baseline to be drawn downwards.
		///   </para>
		/// </remarks>
		object ReferenceValue { get; set; }

		/// <summary>
		/// The reference value.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This property can be edited in design time. In code it is equivalent to 
		///     the <see cref="ReferenceValue"/> property.
		///   </para>
		///   <para>
		///     This is a base value along the y-axis of the charts. All bars, as well as areas
		///     are rendered with this base value. The reference value has to be the same type 
		///     as the type of the y-values in a chart.
		///   </para>
		///   <para>
		///     Often the y-values are numeric and the default reference value is 0. If values
		///     are from a narrow range, say between 200 and 210, you may choose a reference value of
		///     200. The same kind of operation makes sense for other types of y-values.
		///   </para>
		///   <para>
		///     Reference value is handy when there is a baseline value (like average) and we want
		///     values below the baseline to be drawn downwards.
		///   </para>
		/// </remarks>
		GenericType Reference { get; set; }
		
		/// <summary>
		/// Gets or sets a value that indicates whether reference value should be adjusted to the y-value range.
		/// </summary>
		bool AdjustReferenceValue  { get; set; }
		#endregion

		#region --- Chart Template Handling ---

		/// <summary>
		///		Loads chart from an XML template.
		/// </summary>
		/// <param name="templateFileName">Name of the input XML file</param>
		/// <remarks>
		///   <para>
		///     Loads chart settings from an XML file. The input file might be created by 
		///     <see cref="WinChart.StoreTemplate"/> functions and maybe hand-edited afterwards.
		///     After the chart is loaded from a template, data definition functions are needed
		///     to set real data and <see cref="WinChart.DataBind"/> is supposed to be called.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		void LoadTemplate(string templateFileName);

		/// <summary>
		///		Loads chart from an XML template.
		/// </summary>
		/// <param name="templateStream">The input stream object</param>
		/// <remarks>
		///   <para>
		///     Loads chart settings from an XML file. The input file might be created by 
		///     <see cref="WinChart.StoreTemplate"/> functions and maybe hand-edited afterwards.
		///     After the chart is loaded from a template, data definition functions are needed
		///     to set real data and <see cref="WinChart.DataBind"/> is supposed to be called.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		void LoadTemplate(Stream templateStream);

		/// <summary>
		///		Stores chart to an XML template.
		/// </summary>
		/// <param name="templateFileName">Name of the output XML file</param>
		/// <remarks>
		///   <para>
		///     Stores chart settings to an XML file. The output file might be used in
		///     <see cref="WinChart.LoadTemplate"/> function to restore chart settings.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		void StoreTemplate(string templateFileName);

		/// <summary>
		///		Stores chart to an XML template.
		/// </summary>
		/// <param name="templateFileName">The output stream</param>
		/// <remarks>
		///   <para>
		///     Stores chart settings to a stream. The output might be used in
		///     <see cref="WinChart.LoadTemplate"/> function to restore chart settings.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		void StoreTemplate(Stream templateStream);
		#endregion

		#region --- Missing data point handling ---
		/// <summary>
		/// Gets or sets the missing points style name.
		/// </summary>
		string MissingPointsStyleName { get; set; }

		/// <summary>
		/// Gets or sets the missing points style kind.
		/// </summary>
		SeriesStyleKind MissingPointsStyleKind { get; set; }

		/// <summary>
		/// The missing point handler kind.
		/// </summary>
		MissingPointHandlerKind MissingPointHandlerKind { get; set; }

		/// <summary>
		/// Sets the custom missing point handler.
		/// </summary>
		/// <param name="mph"></param>
		void SetCustomMissingPointHandler(MissingPointHandler mph);

		GeometricEngineKind GeometricEngineKind { get; set; } 


		#endregion

		#endregion (COMMON)
	}
}
