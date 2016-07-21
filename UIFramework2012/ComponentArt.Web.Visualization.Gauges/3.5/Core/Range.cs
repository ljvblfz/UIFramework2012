using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Point of reference for alignment.  Left represents outside on radial geometry gauges.
	/// </summary>
	public enum RangeSideKind
	{
		Left,
		Right,
		Middle
	};

	/// <summary>
	/// Represents a visual layout definition for a <see cref="Range"/> object.
	/// </summary>
	[TypeConverter(typeof(TypeConverterWithDefaultConstructor))]
	[Serializable]
	public class RangeLayout : IObjectModelNode
	{
		private double startWidth = 2.5;
		private double endWidth = 2.5;
		private double offset = 0;
		private RangeSideKind sideKind = RangeSideKind.Middle;

		public RangeLayout() { }
		
		/// <summary>
		/// Starting strip width in percentages of the gauge size.
		/// </summary>
		[Description("Starting strip width in percentages of the gauge size.")]
		[DefaultValue(2.5)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0,30,0.1)]
		public double StartWidth { get { return startWidth; } set { startWidth = value; } }
		
		/// <summary>
		/// Ending strip width in percentages of the gauge size
		/// </summary>
		[Description("Ending strip width in percentages of the gauge size.")]
		[DefaultValue(2.5)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0,30,0.1)]
		public double EndWidth { get { 	return endWidth; } set { endWidth = value; } }
		
		/// <summary>
		/// Range main line offset from the scale in percentages of the gauge size
		/// </summary>
		[Description("Range main line offset from the scale in percentages of the gauge size.")]
		[DefaultValue(0.0)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(-50,50,0.1)]
		public double Offset { get { return offset; } set { offset = value; } }
		
		/// <summary>
		/// Range strip position relative to the range main line
		/// </summary>
		[Description("Range strip position relative to the range main line.")]
		[DefaultValue(RangeSideKind.Middle)]
		public RangeSideKind SideKind { get { return sideKind; } set { sideKind = value; } }
		
		#region --- IObjectModelNode Implementation ---
		private IObjectModelNode parent;
		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}
		#endregion
	}

	/// <summary>
	/// Represents a visual range element of a <see cref="Scale"/> object.
	/// </summary>
	[TypeConverter(typeof(NamedObjectConverter))]
	[Serializable]
	public class Range : NamedObject
	{
		private bool visible = true;

		private double minValue = double.NaN;
		private double maxValue = double.NaN;
		private MultiColor color = new MultiColor(System.Drawing.Color.Empty);
		private AnnotationCollection annotations = new AnnotationCollection();
		private bool stripEnabled = true;
		private bool tickMarksEnabled = true;

		private RangeLayout rangeLayout = new RangeLayout();

		// Tickmarks
		private TickMarkSet majorTickMarks = new TickMarkSet();
		private TickMarkSet minorTickMarks = new TickMarkSet();

		/// <summary>
		/// New ranges should be created thorough the RangeCollection.AddNewMember() method
		/// </summary>
		public Range() : this(string.Empty) 
		{	}
		internal Range(string name) : base(name) 
        {
			(rangeLayout as IObjectModelNode).ParentNode = this;
			(majorTickMarks as IObjectModelNode).ParentNode = this;
			(minorTickMarks as IObjectModelNode).ParentNode = this;
			(annotations as IObjectModelNode).ParentNode = this;
			(color as IObjectModelNode).ParentNode = this;
			CreateDefaultAnnotation();
        }

		internal Range(string name, double minValue, double maxValue) : this(name)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;

			(majorTickMarks as IObjectModelNode).ParentNode = this;
			(minorTickMarks as IObjectModelNode).ParentNode = this;
			(annotations as IObjectModelNode).ParentNode = this;
			(color as IObjectModelNode).ParentNode = this;
			CreateDefaultAnnotation();
		}

		private void CreateDefaultAnnotation()
		{
			Annotation ann = new Annotation("Main");
			ann.IsRequired = true;
			annotations.Add(ann);
		}

		internal SubGauge TopGauge { get { return ObjectModelBrowser.GetOwningTopmostGauge(this); } }
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

		#region --- Properties ---
		/// <summary>
		/// Whether the range is visible or not
		/// </summary>
		[Category("General")]
		[Description("Range visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

		/// <summary>
		/// Holds properties relating to the positioning and look of the range.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DefaultValue(null)]
		public RangeLayout RangeLayout 
		{ 
			get 
			{
                if (IsMain)
				{
					if(ObjectModelBrowser.InSerialization(this))
						return null;
					else
						return Gauge.Skin.RangeLayout;
				}
				else
					return rangeLayout;
			}
			set
			{
				rangeLayout = value;
			}
		}

		/// <summary>
		/// Holds all range annotations
		/// </summary>
        [NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public AnnotationCollection Annotations
		{
		    get 
		    {
                if (ObjectModelBrowser.InSerialization(this))
                    return annotations.GetModifiedCollection() as AnnotationCollection;
                    
                return annotations;
		    }
            //set
            //{
            //    annotations = value;
            //    (annotations as IObjectModelNode).ParentNode = this;
            //}
		}
        
		/// <summary>
		/// Shortcut to the first annotation in the annotation collection
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Annotation MainAnnotation { get { return annotations["Main"]; } set { annotations["Main"] = value; } }
		
		/// <summary>
		/// The value of the parent scale where this range starts
		/// </summary>
		[DefaultValue(double.NaN)]
		[TypeConverter(typeof(DoubleWithAutoConverter))]
		public double MinValue { get { return minValue; } set { minValue = value; } }

		/// <summary>
		/// The value of the parent scale where this range ends
		/// </summary>
		[DefaultValue(double.NaN)]
		[TypeConverter(typeof(DoubleWithAutoConverter))]
		public double MaxValue { get { return maxValue; } set { maxValue = value; } }

		/// <summary>
		/// The Color or MultiColor of this range's strip
		/// </summary>
		[Category("General")]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor Color { get { return color; } set { color = value; (color as IObjectModelNode).ParentNode = this; } }
		
		internal MultiColor EffectiveColor 
		{
			get
			{
				if(color.IsEmpty)
					return ObjectModelBrowser.GetOwningTopmostGauge(this).Palette.RangeBaseColor;
				else
					return color;
			}
		}

		/// <summary>
		/// Indicates whether to display the range strip
		/// </summary>
		[Category("Visibility")]
		[Description("Indicates whether to display the range strip")]
		[DefaultValue(true)]
		public bool StripVisible { get { return stripEnabled; } set { stripEnabled = value; } }

		/// <summary>
		/// Indicates whether to display tickmarks
		/// </summary>
		[Category("Visibility")]
		[Description("Indicates whether to display tickmarks")]
		[DefaultValue(true)]
		public bool TickMarksVisible { get { return tickMarksEnabled; } set { tickMarksEnabled = value; } }

		/// <summary>
		/// Represents the major tickmarks in this range
		/// </summary>
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public TickMarkSet MajorTickMarks { get { return majorTickMarks; } set { majorTickMarks = value; (majorTickMarks as IObjectModelNode).ParentNode = this; } }

		/// <summary>
		/// Represents the minor tickmarks in this range
		/// </summary>
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public TickMarkSet MinorTickMarks { get { return minorTickMarks; } set { minorTickMarks = value; (minorTickMarks as IObjectModelNode).ParentNode = this; } }

		private bool Editing { get { return this.TopGauge != null && this.TopGauge.Editing; } }

        internal bool IsMain { get { return Name == "Main" && Scale != null && Scale.Name == "Main"; } }
		#endregion

		#region --- Obsolete interface NB: Remove ---

		/// <summary>
		/// The starting width of the range's strip, if visible
		/// </summary>
		[Category("Range Width")]
		[DefaultValue(2.5)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0,30)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double StartWidth { get { if(RangeLayout == null) return 2.5; else return RangeLayout.StartWidth; } set { RangeLayout.StartWidth = value; } }

		/// <summary>
		/// The ending width of the range's strip, if visible
		/// </summary>
		[Category("Range Width")]
		[DefaultValue(2.5)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0,30)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double EndWidth { get { if(RangeLayout == null) return 2.5; else return RangeLayout.EndWidth; } set { RangeLayout.EndWidth = value; } }
		#endregion

        #region --- Internal Properties ---

		internal double EffectiveMinValue
		{
			get
			{
				if(double.IsNaN(minValue))
					return Scale.MinValue;
				else
					return minValue;
			}
		}

		internal double EffectiveMaxValue
		{
			get
			{
				if(double.IsNaN(maxValue))
					return Scale.MaxValue;
				else
					return maxValue;
			}
		}

		internal Scale Scale { get { return ObjectModelBrowser.GetAncestorByType(this,typeof(Scale)) as Scale; } }

		internal double StartingPosition
		{
			get
			{
				Scale scale = this.Scale;
				return scale.WorldToRelative(minValue);
			}
		}

		#endregion
	
		#region --- Rendering ---

		internal void RenderStripOnly(RenderingContext context)
		{
			if(stripEnabled)
			{
				if(SubGauge.IsInGroup(this.Gauge.GaugeKind,GaugeKindGroup.Radial))
				{
					if(Scale.ScaleLayout.EffectivePosition(Gauge) <= 0)
						return;
					else
						RenderStripRadial(context);
				}
				else if(SubGauge.IsInGroup(this.Gauge.GaugeKind,GaugeKindGroup.Linear))
					RenderStripLinear(context);
			}
		}

		internal void RenderTickMarksOnly(RenderingContext context)
		{
			if(tickMarksEnabled)
			{
				if(MinorTickMarks != null && MinorTickMarks.Visible)
				{
					if(!MinorTickMarks.CustomValues)
						MinorTickMarks.CreateValues();
					MinorTickMarks.CreateTickmarks(MinorTickMarks.Values);
				}
				if(MajorTickMarks != null && MajorTickMarks.Visible)
				{
					if(!MajorTickMarks.CustomValues)
						MajorTickMarks.CreateValues();
					MajorTickMarks.CreateTickmarks(MajorTickMarks.Values);
				}

				RemoveMinorOverlappedByMajorTickmarks();

				if(MinorTickMarks != null && MinorTickMarks.Visible)
					MinorTickMarks.Render(context);
				if(MajorTickMarks != null && MajorTickMarks.Visible)
					MajorTickMarks.Render(context);
			}
		}

		internal void RenderStrip(RenderingContext context)
		{
			RenderStripOnly(context);
			RenderTickMarksOnly(context);
		}

		internal void RemoveMinorOverlappedByMajorTickmarks()
		{
			if(minorTickMarks == null || majorTickMarks == null ||
				!minorTickMarks.Visible || !majorTickMarks.Visible)
				return;

			Scale sc = this.Scale;
			for(int i=0; i<minorTickMarks.Count; i++)
			{
				double val0 = minorTickMarks[i].Value;
				Point2D p0 = sc.WorldToRenderingCoordinates(val0);
				for(int j=0; j<majorTickMarks.Count; j++)
				{
					double val1 = majorTickMarks[j].Value;
					if((sc.WorldToRenderingCoordinates(val1)-p0).Abs() < 0.001)
					{
						minorTickMarks[i].Visible = false;
						break;
					}
				}
			}
		}

		internal void RenderStripRadial(RenderingContext context)
		{
			if(Scale.ScaleLayout.EffectivePosition(Gauge) <= 0)
				return;

			double val0 = EffectiveMinValue;
			double val1 = EffectiveMaxValue;
			
			// Computing number of points nPts ("RC" = "rendering coordinates")
			Point2D centerRC = Scale.CenterPoint();
			Point2D ptStartRC = WorldToRenderingCoordinates(val0,0);
			Point2D ptEndRC = WorldToRenderingCoordinates(val0*0.75 + val1*0.25,0); // We'll use 1/4 of the full range
			double radiusRC = (centerRC-ptStartRC).Abs();
			Size2D v1 = ptStartRC - centerRC;
			Size2D v1n = v1.Normal();
			Size2D v2 = ptEndRC - centerRC;
			double xx = v1.Width *v2.Width + v1.Height *v2.Height;
			double yy = v1n.Width *v2.Width + v1n.Height *v2.Height;
			double angle = Math.Abs(Math.Atan2(yy,xx));
			// distance between circle and section for angle increment dA:
			//   D = r*(1-cos(dA/2))
			// D/r - 1 = -cos(da/2)
			// dA/2 = arcos(1-D/r)
			// therefore
			//   dA = 2*ArcCos(1 - D/r);
			// Distance in pixels:
			double D = 0.05;
			if(radiusRC < D)
				return;
			double dA = 2* Math.Acos(1-D/radiusRC);
			int nPts = (int)(4*angle/dA); // The factor 4 is to compensate for 1/4 of the range
			nPts = Math.Min(100,Math.Max(nPts,2)); 

			PointF[] points = new PointF[2*nPts+1];
            double[] values = new double[nPts];
			Point2D[] gradientLine = new Point2D[nPts];
            values[0] = val0;
            if (Scale.IsLogarithmic)
            {
                double logV = Math.Log(val0);
                double dv = (Math.Log(val1) - logV) / (nPts - 1);
                for (int i = 1; i < nPts; i++)
                {
                    logV = logV + dv;
                    values[i] = Math.Exp(logV);
                }
            }
            else
            {
                double dv = (val1 - val0) / (nPts - 1);
                for (int i = 1; i < nPts; i++)
                {
                    values[i] = values[i - 1] + dv;
                }
            }
            values[nPts - 1] = val1;

			for(int i=0; i<nPts; i++)
			{
				double pointOffset = this.RangeLayout.StartWidth + (values[i]-val0)/(val1-val0)*(this.RangeLayout.EndWidth-this.RangeLayout.StartWidth);
				double offsetLeft = 0;
				double offsetRight = 0;
				switch(this.RangeLayout.SideKind)
				{
					case RangeSideKind.Left:
						offsetLeft = -pointOffset;
						break;
					case RangeSideKind.Right:
						offsetRight = pointOffset;
						break;
					case RangeSideKind.Middle:
						offsetLeft = -pointOffset/2;
						offsetRight = pointOffset/2;
						break;
				}
				Point2D P = WorldToRenderingCoordinates(values[i],offsetLeft);
                points[i] = P; 
				P =  WorldToRenderingCoordinates(values[i],offsetRight);
                points[nPts * 2 - 1 - i] = P;
			}

			points[2*nPts] = points[0];
			MultiColor mc = EffectiveColor;

			double minOffset = -Math.Max(RangeLayout.EndWidth,RangeLayout.StartWidth)-5;
			for(int i=0; i<nPts; i++)
			{
				gradientLine[i] = WorldToRenderingCoordinates(values[i],minOffset);
			}

			// Radial gradient
			
			context.Engine.FillRadialArea(points,mc,System.Drawing.Color.Transparent,Scale.CenterPoint(),gradientLine,context);

			Pointer pointerParent = ObjectModelBrowser.GetAncestorByType(this,typeof(Pointer)) as Pointer;
			if(pointerParent != null && TopGauge.RenderPointersMapAreas || TopGauge.RenderRangesMapAreas)
			{
				int n = points.Length;
				Point[] pts = new Point[n];
				for(int i=0; i<n; i++)
				{
					PointF T = context.TransformPoint(points[i]);
					pts[i] = new Point((int)(T.X + 0.5f),(int)(T.Y + 0.5f));
				}
				MapAreaPolygon ma = new MapAreaPolygon(pts);
				// This may be regular range strip, or pointer strip. 
				if(pointerParent != null)
					ma.SetObject(pointerParent);
				else
                    ma.SetObject(this);
				TopGauge.MapAreas.Add(ma);
			}
		}

		internal void RenderStripLinear(RenderingContext context)
		{
			double val0 = EffectiveMinValue;
			double val1 = EffectiveMaxValue;
			PointF[] points = new PointF[5];

			double val = val0;
			double pointOffset = this.RangeLayout.StartWidth;
			for(int i=0; i<2; i++)
			{
				double offsetLeft = 0;
				double offsetRight = 0;
				switch(this.RangeLayout.SideKind)
				{
					case RangeSideKind.Left:
						offsetLeft = -pointOffset;
						break;
					case RangeSideKind.Right:
						offsetRight = pointOffset;
						break;
					case RangeSideKind.Middle:
						offsetLeft = -pointOffset/2;
						offsetRight = pointOffset/2;
						break;
				}
				points[i] = WorldToRenderingCoordinates(val,offsetLeft);
				points[3-i] =  WorldToRenderingCoordinates(val,offsetRight);
				val = val1;
				pointOffset = this.RangeLayout.EndWidth;
			}

			points[4] = points[0];
			MultiColor mc = EffectiveColor;

			// Linear gradient
			context.Engine.FillLinearArea(points,mc,WorldToRenderingCoordinates(val0),WorldToRenderingCoordinates(val1),context);
		}

		// Computes the width of the strip on the left or right side of range center line
		// ocupied by the range strip and tickmarks.
		// Used to position annotations.
		// Result in percentages of gauge size (=min(w,h))
		
		internal double OccupiedWidthPerc(double worldValue, bool leftSide)
		{
			double a = (worldValue - EffectiveMinValue)/(EffectiveMaxValue-EffectiveMinValue);
			double w = a*RangeLayout.EndWidth + (1-a)*RangeLayout.StartWidth;
			double offsetLeft = 0;
			double offsetRight = 0;
			switch(this.RangeLayout.SideKind)
			{
				case RangeSideKind.Left:
					offsetLeft = w;
					break;
				case RangeSideKind.Right:
					offsetRight = w;
					break;
				case RangeSideKind.Middle:
					offsetLeft = w/2;
					offsetRight = w/2;
					break;
			}
			if(leftSide)
			{
				if(MajorTickMarks != null && MajorTickMarks.Visible)
					w = Math.Max(offsetLeft,MajorTickMarks.Size.Height*0.5 + MajorTickMarks.EffectiveOffset);
				if(MinorTickMarks != null && MinorTickMarks.Visible)
					w = Math.Max(offsetLeft,MinorTickMarks.Size.Height*0.5 + MinorTickMarks.EffectiveOffset);
			}
			else
			{
				if(MajorTickMarks != null && MajorTickMarks.Visible)
					w = Math.Max(offsetRight,MajorTickMarks.Size.Height*0.5 - MajorTickMarks.EffectiveOffset);
				if(MinorTickMarks != null && MinorTickMarks.Visible)
					w = Math.Max(offsetRight,MinorTickMarks.Size.Height*0.5 - MinorTickMarks.EffectiveOffset);
			}
			return w;
		}

		#endregion

		#region --- Utilities ---

		internal double AngleInRadians(double val)
		{
			Scale scale = this.Scale;
			double min = EffectiveMinValue;
			double max = EffectiveMaxValue;

			double scalePercent = 100-scale.EffectiveStartingMargin-scale.EffectiveEndingMargin;
			// angles in percentages of full circle
			double angle0 = scale.EffectiveStartingMargin + scale.WorldToRelative(min)*scalePercent;
			double angle1 = scale.EffectiveStartingMargin + scale.WorldToRelative(max)*scalePercent;
			double totalAngleFactpr = GaugeGeometry.TotalAngle(this.TopGauge.GaugeKind)/360;
			angle0 *= totalAngleFactpr;
			angle1 *= totalAngleFactpr;

			angle0 += scale.StartingAngle*100/360;
			angle1 += scale.StartingAngle*100/360;
			// angles in radians
			angle0 = 0.02*angle0*Math.PI;
			angle1 = 0.02*angle1*Math.PI;

			double a = (val - min)/(max - min);
			return a*angle1 + (1-a)*angle0;

		}

		// Coordinate on the range main line
		internal Point2D WorldToRenderingCoordinates(double worldValue)
		{
			return WorldToRenderingCoordinates(worldValue,0);
		}

		internal Point2D WorldToRenderingCoordinates(double worldValue, double offset)
		{
			GaugeKind kind = this.Scale.Gauge.GaugeKind;
			bool isRadial = SubGauge.IsInGroup(kind,GaugeKindGroup.Radial);
			Point2D scalePoint = Scale.WorldToRenderingCoordinates(worldValue);
			double linearSize = GaugeGeometry.LinearSize(this.Gauge);

			Size2D vec = new Size2D(0,0);
			if(isRadial)
				vec = (scalePoint-Scale.CenterPoint()).Unit();
			else if (kind ==  GaugeKind.LinearHorizontal)
				vec = new Size2D(0,1);
			else if (kind ==  GaugeKind.LinearVertical)
				vec = new Size2D(-1,0);
			else
				throw new Exception("Implementation: Range.'WorldToRenderingCoordinates()' should not be used for gauge kind '" + kind.ToString() + "'.");

			return scalePoint + vec*(RangeLayout.Offset-offset)*linearSize*0.01;
		}

		#endregion

    #region --- Client-side serialization ---
#if WEB 

    internal Hashtable ExportJsObject()
    {
      Hashtable range = new Hashtable();

      range.Add("name", Name);
      range.Add("minValue", MinValue);
      range.Add("maxValue", MaxValue);
      range.Add("visible", Visible);

      return range;
    }

    internal void ImportJsObject(Hashtable range)
    {
      if (range.ContainsKey("minValue"))
        MinValue = (double)range["minValue"];

      if (range.ContainsKey("maxValue"))
        MaxValue = (double)range["maxValue"];

      if (range.ContainsKey("visible"))
        Visible = (bool)range["visible"];
    }

#endif
    #endregion

	}

	/// <summary>
	/// Contains a collection of <see cref="Range"/> objects.
	/// </summary>
	[Serializable]
	public class RangeCollection : NamedObjectCollection
	{
		internal override NamedObject CreateNewMember()
		{
			Range newMember = new Range();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;
		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Main". If member named "Main" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public Range AddNewMember(string newMemberName)
		{
			Range newMember = AddNewMemberFrom(newMemberName,"Main");
			if(newMember == null)
			{
				newMember = new Range(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Range"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new Range AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as Range;
		}

		#endregion
		
		/// <summary>
		/// Retrieves the range from the collection based on its order of addition to the collection.  Main range is 0.
		/// </summary>
		/// <param name="ix">the number or the name of the range as it was being added</param>
		/// <returns>the range</returns>
		public new Range this[object ix]
		{
			get { return base[ix] as Range; }
			set { base[ix] = value; }
		}
//
//		/// <summary>
//		/// Retrieves the range from the collection based on its name, given when the range was created.
//		/// </summary>
//		/// <param name="name">the name of the requested range</param>
//		/// <returns>the range</returns>
//		public new Range this[string name] 
//		{ 
//			get { return base[name] as Range; }
//			set { base[name] = value; }
//		}

    #region --- Client-side serialization ---
#if WEB 

    internal ArrayList ExportJsArray()
    {
      ArrayList ranges = new ArrayList();

      for (int i = 0; i < this.Count; i++)
        ranges.Insert(i, this[i].ExportJsObject());

      return ranges;
    }

    internal void ImportJsArray(IEnumerable ranges)
    {
      foreach (Hashtable range in ranges)
        this[(string)range["name"]].ImportJsObject(range);
    }

#endif
    #endregion

  }
}
