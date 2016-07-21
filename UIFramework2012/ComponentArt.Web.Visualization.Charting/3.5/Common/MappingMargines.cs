using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents the margins of the chart.
	/// </summary>
	[Serializable()]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class MappingMargins
	{
		private const double defaultMargimn = 10;
		private double	marginLeft=defaultMargimn;
		private double	marginTop=defaultMargimn;
		private double	marginRight=defaultMargimn;
		private double	marginBottom=defaultMargimn;

		private bool	hasChanged = false;

		/// <summary>
		/// Initialises a new instance of <see cref="MappingMargins"/> class with default parameters.
		/// </summary>
		public MappingMargins()
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="MappingMargins"/> class with specified left, top right and left margins.
		/// </summary>
		/// <param name="left">Left margin in percentages. This value is stored in <see cref="MappingMargins.Left"/> property.</param>
		/// <param name="top">Top margin in percentages. This value is stored in <see cref="MappingMargins.Top"/> property.</param>
		/// <param name="right">Right margin in percentages. This value is stored in <see cref="MappingMargins.Right"/> property.</param>
		/// <param name="bottom">Bottom margin in percentages. This value is stored in <see cref="MappingMargins.Bottom"/> property.</param>
		public MappingMargins(double left, double top, double right, double bottom)
		{
			marginLeft = left;
			marginTop = top;
			marginRight = right;
			marginBottom = bottom;
		}

		internal bool	  HasChanged	{ get { return hasChanged; } }

		internal bool	  MarginsInPercentages	{ get { return true; } }

		/// <summary>
		/// Gets or sets the left margin size in % of the chart width.
		/// </summary>
		[
		Description("Left margin size in % of the chart width"), 
		Category("Margins"),
		DefaultValue(defaultMargimn),
		NotifyParentProperty(true)
		]
		public double Left			{ get { return marginLeft; }   set { if(marginLeft != value) hasChanged = true; marginLeft = Math.Min(value,90-marginRight); } }

		/// <summary>
		/// Gets or sets the right margin size in % of the chart width.
		/// </summary>
		[
		Description("Right margin size in % of the chart width"), 
		Category("Margins"),
		DefaultValue(defaultMargimn),
		NotifyParentProperty(true)
		]
		public double Right			{ get { return marginRight; }  set { if(marginRight != value) hasChanged = true; marginRight = Math.Min(value,90-marginLeft); } }

		/// <summary>
		/// Gets or sets the top margin size in % of the chart width.
		/// </summary>
		[
		Description("Top margin size in % of the chart height"), 
		Category("Margins"),
		DefaultValue(defaultMargimn),
		NotifyParentProperty(true)
		]
		public double Top				{ get { return marginTop; }    set { if(marginTop != value) hasChanged = true; marginTop = Math.Min(value,90-marginBottom); } }

		/// <summary>
		/// Gets or sets the bottom margin size in % of the chart width.
		/// </summary>
		[
		Description("Bottom margin size in % of the chart height"), 
		Category("Margins"),
		DefaultValue(defaultMargimn),
		NotifyParentProperty(true)
		]
		public double Bottom			{ get { return marginBottom; } set { if(marginBottom != value) hasChanged = true; marginBottom = Math.Min(value,90-marginTop); } }

		private static string[] propertiesOrder = new string[]
			{
				"Left",
				"Right",
				"Top",
				"Bottom"
			};
	}
}
