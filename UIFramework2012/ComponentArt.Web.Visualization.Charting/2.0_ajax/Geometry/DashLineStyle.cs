using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines a dashed line style.
	/// </summary>
	// ---------------------------------------------------------------------------------------------------
		
	//[TypeConverter(typeof(DashLineStyleConverter))]
	public class DashLineStyle : LineStyle
	{
		DashLineStyleSegmentCollection m_lineSegments;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyle"/> class with a specified name and segments.
		/// </summary>
		/// <param name="name">The name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="lineStyles">An array of segment line style names of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="lengths">An array of segment lengths of this <see cref="DashLineStyle"/> object.</param>
		public DashLineStyle(string name, string[] lineStyles, double[] lengths) : this(name)
		{
			if (lineStyles.Length != lengths.Length)
				throw new ArgumentException("Arrays for DashLineStyle must be of equal size");

			for (int i=0; i<lineStyles.Length; ++i) 
			{
				m_lineSegments.Add(new DashLineStyleSegment(lineStyles[i], lengths[i]));
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyle"/> class with a specified name and a specified segment collection.
		/// </summary>
		/// <param name="name">The name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="lineStyles">The segment collection of this <see cref="DashLineStyle"/> object.</param>
		internal DashLineStyle(string name, DashLineStyleSegmentCollection lineStyles) : base(name)
		{
			m_lineSegments = lineStyles;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyle"/> class with a specified name and 2 specified segments.
		/// </summary>
		/// <param name="name">The name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="lineStyle1">First segment line style name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="lineStyle2">Second segment line style name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="length1">First segment length of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="length2">Second segment length of this <see cref="DashLineStyle"/> object.</param>
		public DashLineStyle(string name, string lineStyle1, string lineStyle2, double length1, double length2) 
			: base(name)
		{
			m_lineSegments = new DashLineStyleSegmentCollection(this);
			m_lineSegments.Add(new DashLineStyleSegment(lineStyle1, length1));
			m_lineSegments.Add(new DashLineStyleSegment(lineStyle2, length2));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyle"/> class with a specified name and 2 specified segments.
		/// </summary>
		/// <param name="name">The name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="lineStyle1">First segment line style name of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="length1">First segment length of this <see cref="DashLineStyle"/> object.</param>
		/// <param name="length2">Second segment length of this <see cref="DashLineStyle"/> object.</param>
		public DashLineStyle(string name, string lineStyle1, double length1, double length2) 
			: this(name, lineStyle1, "NoLine", length1, length2)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="DashLineStyle"/> object.</param>
		public DashLineStyle(string name) : this(name,"Default", "NoLine",8,5) 
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyle"/> class.
		/// </summary>
		public DashLineStyle() : this(null) {}

		/// <summary>
		/// Gets the collection of line segments of this <see cref="DashLineStyle"/> object.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[SRDescription("DashLineStyleSegmentsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DashLineStyleSegmentCollection Segments
		{
			get {return m_lineSegments;}
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override double Width 
		{
			get {return 0;}
			set {}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override double Height 
		{
			get {return 0;}
			set {}
		}

		#region --- XML Serialization ---

		internal override void Serialize(XmlCustomSerializer S)
		{
			base.Serialize(S);
			if(S.Reading)
			{
				DashLineStyleSegment DLSE;
				m_lineSegments.Clear();
				if(S.GoToFirstChild("LineSegment"))
				{
					DLSE = new DashLineStyleSegment();
					S.AttributeProperty(DLSE,"LineStyleName");
					S.AttributeProperty(DLSE,"SegmentLength");
					m_lineSegments.Add(DLSE);
					while(S.GoToNext("LineSegment"))
					{
						DLSE = new DashLineStyleSegment();
						S.AttributeProperty(DLSE,"LineStyleName");
						S.AttributeProperty(DLSE,"SegmentLength");
						m_lineSegments.Add(DLSE);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach (DashLineStyleSegment DLSE in m_lineSegments)
				{
					if(S.BeginTag("LineSegment"))
					{
						S.AttributeProperty(DLSE,"LineStyleName");
						S.AttributeProperty(DLSE,"SegmentLength");
						S.EndTag();
					}
				}
			}
		}
		#endregion
	}

}
