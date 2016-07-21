using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Specifies available pre-defined pointer styles.
	/// </summary>
	public enum PointerStyleKind
	{
		Default,
		BlackIce,
		BlackIce2,
		BlackIceLinear,
		ArcticWhite,
		ArcticWhite2,
		MarkerPointer,
		BarPointer,
		Monochrome,
		MonochromeLinear,
		Custom
	}

	/// <summary>
	/// Specifies available pointer types.
	/// </summary>
	public enum PointerKind
	{
		Needle,
		Marker,
		Bar
	}

    /// <summary>
    /// Represents a style definition for <see cref="Pointer"/> objects.
    /// </summary>
	[Serializable] 
	public class PointerStyle : NamedObject
	{
		private Point2D relativeCenterPoint = new Point2D(0,50);
		private Point2D relativeEndPoint = new Point2D(100,50);

		private MultiColor pointerBackColor = new MultiColor(Color.Empty);

		// Kind
		private PointerKind kind = PointerKind.Needle;

		// Needle/hub kind
        private Color hubBackColor = Color.Empty;
		private Color effectivePointerBackColor = Color.Empty;
        private string hubStyleName = "Default";
		private string needleStyleName = "Default";
		private double relativeHubRadius = 10; // %
		private bool hubEnabled = true;

		// Marker kind
		private string markerStyleName = "Default";
		private Size2D markerSize = new Size2D(5,5); // size in %
		private double markerOffset = 0;

		// Bar kind
		private bool barAboveTickMarks = true;
		private double barStartWidth = 2.5;
		private double barEndWidth = 2.5;

		private bool enabled = true;

		private bool hubAboveNeedle = true;

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New PointerStyle instances should be created with <see cref="PointerStyleCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public PointerStyle() : base() { (pointerBackColor as IObjectModelNode).ParentNode = this; }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New PointerStyle instances should be created with <see cref="PointerStyleCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public PointerStyle(string name) : base(name) { (pointerBackColor as IObjectModelNode).ParentNode = this; }

		/// <summary>
		/// The type of pointer this is.  Can be needle, bar or marker
		/// </summary>
		[DefaultValue(typeof(PointerKind),"Needle")]
		public PointerKind PointerKind { get { return kind; } set { kind = value; } }

		/// <summary>
		/// Whether the pointer is visible or not
		/// </summary>
		[DefaultValue(true)]
		public bool Visible { get { return enabled; } set { enabled = value; } }

		/// <summary>
		/// The background color of the pointer
		/// </summary>
		public MultiColor PointerBackColor { get { return pointerBackColor; } set { pointerBackColor = value;  ObjectModelBrowser.NotifyChanged(this); (pointerBackColor as IObjectModelNode).ParentNode = this; } }
		
		/// <summary>
		/// An (x,y) value pair representing the percentage of the pointer graphic that intersects the range mid-point.
		/// </summary>
		public Point2D RelativeEndPoint { get { return relativeEndPoint; } set { relativeEndPoint = value ; } }

		internal Color EffectivePointerBackColor { get { return effectivePointerBackColor; } set { effectivePointerBackColor = value; } }

		#region --- Marker type pointer properties ---

		/// <summary>
		/// If the pointer is of Marker kind, the MarkerStyle name that will be applied to this pointer
		/// </summary>
		[Category("Marker type pointer")]
		[Description("Marker style")]
		[DefaultValue("Default")]
		[TypeConverter(typeof(MarkerStyleNameConverter))]
		public string MarkerStyleName { get { return markerStyleName; } set { markerStyleName = value; } }
		
		/// <summary>
		/// If the pointer is of Marker kind, the predefined MarkerStyle that will be applied to this pointer
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MarkerStyleKind MarkerStyleKind { get { return MarkerStyle.NameToKind(markerStyleName); } set { markerStyleName = MarkerStyle.KindToName(value); } }

		/// <summary>
		/// Marker offset from the centre of the scale, as percentage of the entire gauge size
		/// </summary>
		[Category("Marker type pointer")]
		[Description("Marker offset in percentages of gauge size")]
		[DefaultValue(0.0)]
		public double MarkerOffset { get { return markerOffset; } set { markerOffset = value; } }
		
		/// <summary>
		/// The size of the marker as percentage of entire gauge size
		/// </summary>
		[Category("Marker type pointer")]
		[Description("Marker size in percentages of gauge size")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		[DefaultValue(typeof(Size2D),"5,5")]
		public Size2D MarkerSize { get { return markerSize; } set { markerSize = value; } }

		#endregion

		#region --- Bar type pointer properties ---

		/// <summary>
		/// When pointer is of Bar kind, whether it is drawn over the range tick marks or under.
		/// </summary>
		[Category("Bar type pointer")]
		[Description("Render pointer bar above tickmarks")]
		[DefaultValue(true)]
		public bool BarAboveTickMarks { get { return barAboveTickMarks; } set { barAboveTickMarks = value; } }

		/// <summary>
		/// The start witdh of the Bar type pointer
		/// </summary>
		[Category("Bar type pointer")]
		[Description("Initial bar width in percentages")]
		[DefaultValue(2.5)]
		public double BarStartWidth { get { return barStartWidth; } set { barStartWidth = value; } }

		/// <summary>
		/// The start witdh of the Bar type pointer
		/// </summary>
		[Category("Bar type pointer")]
		[Description("Ending bar width in percentages")]
		[DefaultValue(2.5)]
		public double BarEndWidth { get { return barEndWidth; } set { barEndWidth = value; } }

		#endregion
		
		#region --- Needle type pointer properties ---

		/// <summary>
		/// Name of the HubStyle that describes hub properties when PointerKind is Needle
		/// </summary>
		public string HubStyleName { get { return hubStyleName; } set { hubStyleName = value; } }

		/// <summary>
		/// Name of the NeedleStyle that describes pointer properties when PointerKind is Needle
		/// </summary>
		public string NeedleStyleName { get { return needleStyleName; } set { needleStyleName = value; } }

		/// <summary>
		/// A relative point on the needle graphic between (0,0) and (100,100) that describes the point around which the needle rotates.
		/// </summary>
		public Point2D RelativeCenterPoint { get { return relativeCenterPoint; } set { relativeCenterPoint = value; } }

		/// <summary>
		/// Radius of the hub graphic relative to the size of the entire gauge
		/// </summary>
		[DefaultValue(10.0)]
		[Editor(typeof(SliderEditor),typeof(UITypeEditor)),ValueRange(0,100)]
		public double RelativeHubRadius { get { return relativeHubRadius; } set { relativeHubRadius = value; } }

		/// <summary>
		/// Whether the hub is visible or not
		/// </summary>
		[DefaultValue(true)]
		public bool HubVisible { get { return hubEnabled; } set { hubEnabled = value; } }

		/// <summary>
		/// Whether the hub graphic is drawn on top of the pointer graphic or vice versa
		/// </summary>
		[DefaultValue(true)]
		public bool HubAboveNeedle { get { return hubAboveNeedle; } set { hubAboveNeedle = value ; } }
		
		/// <summary>
		/// The background color for the hub
		/// </summary>
		public Color HubBackColor { get { return hubBackColor; } set { hubBackColor = value; } }

        internal Layer NeedleLayer { get { return TopGauge.NeedleLayers[needleStyleName,GaugeKind.Circular]; } }
        internal Layer HubLayer { get { return TopGauge.HubLayers[hubStyleName,GaugeKind.Circular]; } }

		#endregion

		#region --- PointerStyleKind Handling ---

		/// <summary>
		/// The style associated with the Pointer.  Default is that of the selected theme.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PointerStyleKind PointerStyleKind { get { return NameToKind(Name); } }

		internal static PointerStyleKind NameToKind(string name)
		{
			try
			{
				string name1 = name.Replace(" ","");
				TypeConverter tc = new EnumConverter(typeof(PointerStyleKind));
				return (PointerStyleKind)tc.ConvertFromString(name1);
			}
			catch
			{
				return PointerStyleKind.Custom;
			}
		}

		internal static string KindToName(PointerStyleKind kind)
		{
			switch(kind)
			{
				case PointerStyleKind.BlackIce: return "Black Ice"; 
				case PointerStyleKind.BlackIce2: return "Black Ice 2"; 
				case PointerStyleKind.BlackIceLinear: return "Black Ice Linear"; 
				case PointerStyleKind.ArcticWhite: return "Arctic White"; 
				case PointerStyleKind.ArcticWhite2: return "Arctic White 2"; 
				case PointerStyleKind.MonochromeLinear: return "Monochrome Linear"; 
				default: return kind.ToString();
			}
		}

		#endregion

		private SubGauge TopGauge { get { return ObjectModelBrowser.GetOwningTopmostGauge(this); } }
	}

    /// <summary>
    /// Contains a collection of <see cref="PointerStyle"/> objects.
    /// </summary>
	[Serializable] 
	public class PointerStyleCollection : NamedObjectCollection
	{
		/// <summary>
		/// In most cases there is no need to create another PointerStyleCollection, the existing one should be sufficient
		/// </summary>
        public PointerStyleCollection(bool populateInitialContents) : base(populateInitialContents) 
		{
			if(populateInitialContents)
				PopulateInitialContents();
		}

		/// <summary>
		/// In most cases there is no need to create another PointerStyleCollection
		/// </summary>
        public PointerStyleCollection() : this(false) { }
        
        internal override NamedObject CreateNewMember() 
		{
			PointerStyle newMember = new PointerStyle();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;
		}
		
		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Default". If member named "Default" doesn't exist, a new
		/// instance of PointerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public PointerStyle AddNewMember(string newMemberName)
		{
			PointerStyle newMember = AddNewMemberFrom(newMemberName,"Default");
			if(newMember == null)
			{
				newMember = new PointerStyle(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="PointerStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new PointerStyle AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as PointerStyle;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="PointerStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="pointerStyleKind"><see cref="PointerStyleKind"/> of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public PointerStyle AddNewMemberFrom(string newMemberName, PointerStyleKind pointerStyleKind)
		{
			return base.AddNewMemberFrom(newMemberName,PointerStyle.KindToName(pointerStyleKind)) as PointerStyle;
		}

		#endregion

        internal override void PopulateInitialContents()
        {
            LayerCollection needleLayers = ObjectModelBrowser.GetOwningTopmostGauge(this).NeedleLayers;

            PointerStyle style;
            for (int i = 0; i < needleLayers.Count; i++)
            {
                string name = needleLayers[i].Name;
                style = new PointerStyle(name);
                style.NeedleStyleName = name;
                style.HubStyleName = name;
                Add(style);
            }

            style = new PointerStyle("MarkerPointer");
            style.PointerKind = PointerKind.Marker;
            Add(style);

            style = new PointerStyle("BarPointer");
            style.PointerKind = PointerKind.Bar;
            Add(style);
        }

		/// <summary>
		/// Retrieve the PointerStyle based on it's name or PointerStyleKind
		/// </summary>
		public new PointerStyle this[object ix]
		{
			get 
			{
				if(ix is PointerStyleKind)
					ix = PointerStyle.KindToName((PointerStyleKind)ix);
				return base[ix] as PointerStyle; 
			}
			set 
			{
				if(ix is PointerStyleKind)
					ix = PointerStyle.KindToName((PointerStyleKind)ix);
				base[ix] = value; 
			}
		}
	}

	internal class PointerStyleNameConverter : SelectedNameConverter 
	{
		public PointerStyleNameConverter() 
		{
			addNone = true;
			addAuto = true;
		}

		protected override NamedObjectCollection GetNamedCollection(SubGauge gauge)
		{
			return gauge.PointerStyles;
		}

	}
}
