using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Specifies available pre-defined marker styles.
	/// </summary>
	public enum MarkerStyleKind
	{
		Default,
		Rectangle,
		Circle,
		Ellipse,
		Diamond,
		Trapeze,
		TrapezeInverted,
		Triangle,
		TriangleInverted,
		ArcticWhiteMajor,
		ArcticWhiteMinor,
		ArcticWhiteMajorInverted,
		Monochrome,
		Custom
	}

	/// <summary>
	/// Specifies available marker types.
	/// </summary>
	public enum MarkerKind
	{
		Default,
		Rectangle,
        Circle,
		Ellipse,
        Diamond,
		Trapeze,
		TrapezeInverted,
		Triangle,
		TriangleInverted,
		Bitmap
	}
	/// <summary>
    /// Represents a style definition referenced by a <see cref="PointerStyle"/> object.
	/// </summary>
	[Serializable]
	public class MarkerStyle: NamedObject
	{
		private MarkerKind markerKind = MarkerKind.Default;
		private Color baseColor = Color.Empty;
		private Color lineColor = Color.FromArgb(128,0,0,0);
		private float lineWidth = 1;
        private Size2D shadowOffset = new Size2D(10, 10);
		private bool effect3D = true;
		private bool hasShadow = true;

		private Point2D hotSpot = new Point2D(50,50);

		private string markerImageName = "None";

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New MarkerStyle instances should be created with <see cref="MarkerStyleCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public MarkerStyle() : base() { }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New MarkerStyle instances should be created with <see cref="MarkerStyleCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public MarkerStyle(string name) : base (name) { }

		#region --- Properties ---

		/// <summary>
		/// The shape or kind for this marker
		/// </summary>
		public MarkerKind Kind { get { return markerKind; } set { markerKind = value; } }
		
		/// <summary>
		/// The color of the marker
		/// </summary>
		[DefaultValue(typeof(Color),"Empty")]
		public Color BaseColor { get { return baseColor; } set { baseColor = value; } }
		
		/// <summary>
		/// The outline color in the marker
		/// </summary>
		[DefaultValue(typeof(Color),"Empty")]
		public Color LineColor { get { return lineColor; } set { lineColor = value; } }

		/// <summary>
		/// The outline width in the marker
		/// </summary>
		[DefaultValue(1.0f)]
		public float LineWidth { get { return lineWidth; } set { lineWidth = value; } }
		
		/// <summary>
		/// Whether the marker has a 3D effect or not
		/// </summary>
		[DefaultValue(true)]
		public bool Effect3D { get { return effect3D; } set { effect3D = value; } }

		/// <summary>
		/// Whether the marker has a shadow or not
		/// </summary>
		[DefaultValue(true)]
		public bool HasShadow { get { return hasShadow; } set { hasShadow = value; } }
	
		/// <summary>
		/// The relative point in the marker that is placed on the exact pointed value the marker represents.
		/// Default is (50,50) which is the mid point of the marker.
		/// </summary>
		[DefaultValue(typeof(Point2D),"50,50")]
		[Description("Hot spot position in percentages of the marker size")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public Point2D RelativeHotSpot { get { return hotSpot; } set { hotSpot = value; } }
	
		/// <summary>
		/// If marker is a Custom marker, the custom bitmap image for the marker
		/// </summary>
		[TypeConverter(typeof(MarkerLayerNameConverter))]
		[DefaultValue("None")]
		public string MarkerImageName { get { return markerImageName; } set { markerImageName = value; } }

		internal bool IsImage { get { return markerImageName != "" && markerImageName != null && markerImageName != "None"; } }
		
		internal Layer MarkerLayer { get { return ObjectModelBrowser.GetOwningTopmostGauge(this).MarkerLayers[markerImageName,GaugeKind.Circular]; ; } }
       
		[Description("Shadow offset in percentages of tickmark size")]
		[DefaultValue("10,10")]
		internal Size2D ShadowOffset { get { return shadowOffset; } set { shadowOffset = value; } }

		#endregion
    
		#region --- MarkerStyleKind Handling ---

		/// <summary>
		/// The style associated with the Marker.  Default is that of the selected theme.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MarkerStyleKind MarkerStyleKind { get { return NameToKind(Name); } }

		internal static MarkerStyleKind NameToKind(string name)
		{
			try
			{
				string name1 = name.Replace(" ","");
				TypeConverter tc = new EnumConverter(typeof(MarkerStyleKind));
				return (MarkerStyleKind)tc.ConvertFromString(name1);
			}
			catch
			{
				return MarkerStyleKind.Custom;
			}
		}
		internal static string KindToName(MarkerStyleKind kind)
		{
			switch(kind)
			{
				case MarkerStyleKind.ArcticWhiteMajor: return "Arctic White Major"; 
				case MarkerStyleKind.ArcticWhiteMajorInverted: return "Arctic White Major Inverted"; 
				case MarkerStyleKind.ArcticWhiteMinor: return "Arctic White Minor"; 
				default: return kind.ToString();
			}
		}

		#endregion
	}


    /// <summary>
    /// Contains a collection of <see cref="MarkerStyle"/> objects.
    /// </summary>
	[Serializable]
	public class MarkerStyleCollection : NamedObjectCollection
	{
		
		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Default". If member named "Default" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public MarkerStyle AddNewMember(string newMemberName)
		{
			MarkerStyle newMember = AddNewMemberFrom(newMemberName,"Default");
			if(newMember == null)
			{
				newMember = new MarkerStyle(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="MarkerStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new MarkerStyle AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as MarkerStyle;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="MarkerStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="markerStyleKind"><see cref="MarkerStyleKind"/> of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public MarkerStyle AddNewMemberFrom(string newMemberName, MarkerStyleKind markerStyleKind)
		{
			return base.AddNewMemberFrom(newMemberName,MarkerStyle.KindToName(markerStyleKind)) as MarkerStyle;
		}

		#endregion

		internal override NamedObject CreateNewMember()
		{
			MarkerStyle newMember = new MarkerStyle();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;	
		}

		/// <summary>
		/// Retrieve the MarkerStyle based on it's name or MarkerStyleKind
		/// </summary>
		public new MarkerStyle this[object ix]
		{
			get 
			{
				if(ix is MarkerStyleKind)
					ix = MarkerStyle.KindToName((MarkerStyleKind)ix);
				return base[ix] as MarkerStyle; 
			}
			set
			{
				if(ix is MarkerStyleKind)
					ix = MarkerStyle.KindToName((MarkerStyleKind)ix);
				base[ix] = value; 
			}
		}

        internal override void PopulateInitialContents()
		{
            MarkerKind[] kinds = new MarkerKind[]
				{
		        MarkerKind.Default,
		        MarkerKind.Rectangle,
                MarkerKind.Circle,
		        MarkerKind.Ellipse,
                MarkerKind.Diamond,
		        MarkerKind.Trapeze,
		        MarkerKind.TrapezeInverted,
		        MarkerKind.Triangle,
		        MarkerKind.TriangleInverted
                };


            foreach (MarkerKind kind in kinds)
			{
				MarkerStyle style = new MarkerStyle(kind.ToString());
				style.Kind = kind;
				Add(style);
			}
		}
	}

	internal class MarkerStyleNameConverter : SelectedNameConverter 
	{
		public MarkerStyleNameConverter() { }

		protected override NamedObjectCollection GetNamedCollection(SubGauge gauge)
		{
			return gauge.MarkerStyles;
		}
	}
}
