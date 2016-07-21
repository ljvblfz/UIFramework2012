using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{	
	/// <summary>
	/// Represents a scale element of a <see cref="Gauge"/> object.
	/// </summary>
	[TypeConverter(typeof(NamedObjectConverter))]
	[Serializable]
    public class Scale : NamedObject
    {
		private PointerCollection pointers;
		private RangeCollection ranges;
		private bool visible = true;
		private bool isLogarithmic = false;
		private bool isReverse = false;
		private double minValue = 0.0;
		private double maxValue = 100.0;
		private double logBase = 10;

		private ScaleLayout scaleLayout = null;
		
		/// <summary>
		/// New scales should be created thorough the ScaleCollection.AddNewMember() method
		/// </summary>
        public Scale() : this(String.Empty) { }
        internal Scale(string name) : base(name)
        {
			pointers = new PointerCollection();
			Pointer mainPointer = new Pointer("Main");
			pointers.Add(mainPointer);
			mainPointer.IsRequired = true;
			(pointers as IObjectModelNode).ParentNode = this;

			ranges = new RangeCollection();
			ComponentArt.Web.Visualization.Gauges.Range mainRange = new Range("Main");
			ranges.Add(mainRange);
			mainRange.IsRequired = true;

			(ranges as IObjectModelNode).ParentNode = this;
			mainRange.MajorTickMarks.Visible = true;
			mainRange.MinorTickMarks.Visible = true;
        }

		#region --- Geometry ---

		/// <summary>
		/// Holds all positioning and layout related properties of the scale
		/// </summary>
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public ScaleLayout ScaleLayout 
		{
			get 
			{
				if (scaleLayout == null)
				{
					if (Gauge==null || Gauge.Skin == null)
					{
						return null;
					}
					else
					{
						scaleLayout = Gauge.Skin.ScaleLayout.Copy();
						(scaleLayout as IObjectModelNode).ParentNode = this;
					}
				}
				return scaleLayout; 
			}
			set 
			{
					scaleLayout = value;
					(scaleLayout as IObjectModelNode).ParentNode = this;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal ScaleLayout EffectiveScaleLayout 
		{
			get { return ScaleLayout; }
			set { ScaleLayout = value; } 
		}

		private bool Editing { get { return Gauge != null && Gauge.Editing; } }

		internal double StartingAngle { get { return EffectiveScaleLayout.EffectiveStartingAngle(Gauge); } }
        internal double EffectiveStartingMargin { get { return EffectiveScaleLayout.EffectiveStartingMargin(Gauge); } }
        internal double EffectiveEndingMargin { get { return EffectiveScaleLayout.EffectiveEndingMargin(Gauge); } }
        internal double EffectivePosition { get { return EffectiveScaleLayout.EffectivePosition(Gauge); } }
        internal Point2D RelativeCenter { get { return EffectiveScaleLayout.EffectiveRelativeCenter(Gauge); } }

		#endregion

		#region --- Properties ---
		/// <summary>
		/// Minimum value of this scale and all its child ranges
		/// </summary>
		[DefaultValue(0.0)]
		[TypeConverter(typeof(DoubleWithAutoConverter))]
		public double MinValue { get { return minValue; } set { minValue = value; } }

		/// <summary>
		/// Maximum value of this scale and all its child ranges
		/// </summary>
		[DefaultValue(100.0)]
		[TypeConverter(typeof(DoubleWithAutoConverter))]
		public double MaxValue { get { return maxValue; } set { maxValue = value; } }

		/// <summary>
		/// Whether the scale is logarithmic.  MinValue Must be greater than 0.
		/// </summary>
		[DefaultValue(false)]
		public bool IsLogarithmic { get { return isLogarithmic; } set { isLogarithmic = value; } }
		
		/// <summary>
		/// The logarithmic base used when IsLogarthmic is true.
		/// </summary>
		[DefaultValue(10.0)]
		public double LogBase { get { return logBase; } set { logBase = value; } }

		/// <summary>
		/// Primary (first) Pointer in the PointerCollection of this scale.
		/// </summary>
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public Pointer MainPointer
		{
			get { return Pointers["Main"]; }
			set
            {
                value.OverrideName("Main");
                Pointers["Main"] = value;
            }
        }

		/// <summary>
		/// Collection of all the pointers in this scale.
		/// </summary>
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[Editor(typeof(NamedObjectCollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public PointerCollection Pointers 
		{ 
		    get 
		    {
                if (ObjectModelBrowser.InSerialization(this))
                    return pointers.GetModifiedCollection() as PointerCollection;
		    
		        return pointers; 
		    } 
		}

		/// <summary>
		/// Primary (first) Range in the RangeCollection of this scale.
		/// </summary>
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public Range MainRange
		{
            get { return Ranges["Main"]; }
			set
			{
                value.OverrideName("Main");
                Ranges["Main"] = value;
			}
		}
		
		/// <summary>
		/// Collection of all the ranges in this scale. 
		/// </summary>
		[NotifyParentProperty(true)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(NamedObjectCollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public RangeCollection Ranges 
		{
		    get
		    {
                if (ObjectModelBrowser.InSerialization(this))
                    return ranges.GetModifiedCollection() as RangeCollection;
                    
                return ranges;
		    }
		}
		
		/// <summary>
		/// Whether the scale is visible or not.
		/// </summary>
		[Category("General")]
		[Description("Scale marks visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

		#endregion

		#region --- Internal Properties ---

		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

		#endregion

		// Computes relative position (between two scale extremes) a 
		internal double WorldToRelative(double val)
		{
			double x0, x1, a;
            if (IsLogarithmic)
            {
                if (minValue <= 0.0)
                    throw new Exception("Logarithmic scale cannot have min value <= 0");
                if (maxValue <= 0.0)
                    throw new Exception("Logarithmic scale cannot have max value <= 0");
                if (val <= 0.0)
                    throw new Exception("Logarithmic scale cannot have a value <= 0");
                x0 = Math.Log(minValue);
                x1 = Math.Log(maxValue);
                a = Math.Log(val);
            }
            else
            {
                x0 = minValue;
                x1 = maxValue;
                a = val;
            }

			double relValue = (a-x0)/(x1-x0);
			if(isReverse)
				relValue = 1.0-relValue;
			return relValue;
		}

		// Used only in radial gauges
		internal Point2D CenterPoint() 
		{
			Point2D relCenter = RelativeCenter;
			SubGauge gauge = this.Gauge;
            Size2D size = gauge.RenderingSize;

            return new Point2D(size.Width * relCenter.X * 0.01f, size.Height * relCenter.Y * 0.01f);
		}

		// Used only in radial gauges
		internal float MaximumRadius()
		{
			return (float)(GaugeGeometry.ScaleRadiusRelative(this.Gauge.GaugeKind)*0.01*
				GaugeGeometry.LinearSize(this.Gauge));
		}

		// Used only in radial and linear gauges
		internal Point2D WorldToRenderingCoordinates(double worldValue)
		{
			SubGauge gauge = this.Gauge;
			GaugeKind kind = gauge.GaugeKind;
			
			double relValue = WorldToRelative(worldValue);

			if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial))
			{
				Point2D centerPoint = CenterPoint();
				double totalAngleDeg = GaugeGeometry.TotalAngle(kind);
				double scaleStartDeg = StartingAngle - EffectiveStartingMargin*0.01*totalAngleDeg;
				double scaleEndDeg = StartingAngle - (100-EffectiveEndingMargin)*0.01*totalAngleDeg;
				double angleDeg = relValue*scaleEndDeg + (1-relValue)*scaleStartDeg;
				double angleRad = angleDeg/180*Math.PI;
				Size2D unitDir = new Size2D((float)Math.Cos(angleRad),(float)Math.Sin(angleRad));
				return centerPoint + unitDir*0.01*EffectivePosition*MaximumRadius();
			}
			else if(SubGauge.IsInGroup(kind,GaugeKindGroup.Linear))
			{
				float gw = gauge.RenderingSize.Width;
                float gh = gauge.RenderingSize.Height;
				if(kind == GaugeKind.LinearHorizontal)
				{
					double x0 = (float)(gw*0.01*EffectiveStartingMargin);
					double x1 = (float)(1.0 - 0.01*EffectiveEndingMargin)*gw;
					float y = (float)(gh*0.01*EffectivePosition);
					return new Point2D((float)(relValue*x1 + (1f-relValue)*x0),y);
				}
				else
				{
					double y0 = (float)(gh*0.01*EffectiveStartingMargin);
					double y1 = (float)(1.0 - 0.01*EffectiveEndingMargin)*gh;
					float x = (float)(gw*0.01*EffectivePosition);
					return new Point2D(x,(float)(relValue*y1 + (1f-relValue)*y0));
				}
			}
			else
				throw new Exception("Implementation: 'WorldToRenderingCoordinates()' cannot be computed for '" + kind.ToString() + "'.");
		}

		internal Point2D WorldToRenderingCoordinates(double worldValue, double offset)
		{
			SubGauge gauge = Gauge;
			GaugeKind kind = gauge.GaugeKind;
			bool isRadial = SubGauge.IsInGroup(kind,GaugeKindGroup.Radial);
			Point2D scalePoint = WorldToRenderingCoordinates(worldValue);
			double linearSize = GaugeGeometry.LinearSize(gauge);

			Size2D vec = new Size2D(0,0);
			if(isRadial)
				vec = (scalePoint-CenterPoint()).Unit();
			else if (kind ==  GaugeKind.LinearHorizontal)
				vec = new Size2D(0,1);
			else if (kind ==  GaugeKind.LinearVertical)
				vec = new Size2D(-1,0);
			else
				throw new Exception("Implementation: Range.'WorldToRenderingCoordinates()' should not be used for gauge kind '" + kind.ToString() + "'.");

			return scalePoint + vec*offset*linearSize*0.01;
		}


    #region --- Client-side serialization ---
#if WEB 

    internal Hashtable ExportJsObject()
    {
      Hashtable scale = new Hashtable();

      scale.Add("name", Name);
      scale.Add("minValue", MinValue);
      scale.Add("maxValue", MaxValue);
      scale.Add("isLogarithmic", IsLogarithmic);
      scale.Add("logBase", LogBase);
      scale.Add("visible", Visible);

      scale.Add("ranges", Ranges.ExportJsArray());
      scale.Add("pointers", Pointers.ExportJsArray());

      return scale;
    }

    internal void ImportJsObject(Hashtable scale)
    {
      if (scale.ContainsKey("minValue"))
        MinValue = (double)scale["minValue"];

      if (scale.ContainsKey("maxValue"))
        MaxValue = (double)scale["maxValue"];

      if (scale.ContainsKey("isLogarithmic"))
        IsLogarithmic = (bool)scale["isLogarithmic"];

      if (scale.ContainsKey("logBase"))
        LogBase = (double)scale["logBase"];

      if (scale.ContainsKey("visible"))
        Visible = (bool)scale["visible"];

      if (scale.ContainsKey("ranges"))
        Ranges.ImportJsArray((IEnumerable)scale["ranges"]);

      if (scale.ContainsKey("pointers"))
        Pointers.ImportJsArray((IEnumerable)scale["pointers"]);
    }
    
#endif
    #endregion

	}

    /// <summary>
    /// Represents a visual layout definition for a <see cref="Scale"/> object.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(TypeConverterWithDefaultConstructor))]
    public class ScaleLayout : IObjectModelNode 
    {
        private double startingMargin = double.NaN;
        private double endingMargin = double.NaN;
        private double position = double.NaN;

        // radial gauges params
        private double startingAngle = double.NaN;
        private Point2D relativeCenter = null;

        public ScaleLayout() { }

        private SubGauge Gauge { get { return ObjectModelBrowser.GetOwningTopmostGauge(this); } }
        private bool Editing { get { return Gauge != null && Gauge.Editing; } }

		/// <summary>
		/// The margin in clockwise radial degrees from the StartingAngle where the first tick-mark for the MinValue of the scale is drawn.
		/// </summary>
        [DefaultValue(double.NaN)]
        [TypeConverter(typeof(DoubleWithAutoConverter))]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0, 50)]
        public double StartingMargin
		{
			get 
			{
				if(!double.IsNaN(startingMargin) || InSerialization() || !Editing)
					return startingMargin;
				// Editing && value = NaN && !InSerialization
				return GaugeGeometry.ScaleStartingMargin(GaugeKind());
			} 
			set 
			{
				startingMargin = value;
                ObjectModelBrowser.NotifyChanged(this);
			}
		}

		/// <summary>
		/// The margin in counter-clockwise radial degrees from the StartingAngle where the last tick-mark for the MaxValue of the scale is drawn.
		/// </summary>
        [DefaultValue(double.NaN)]
        [TypeConverter(typeof(DoubleWithAutoConverter))]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0, 50)]
		public double EndingMargin 		
		{
            get
            {
				if(!double.IsNaN(endingMargin) || InSerialization() || !Editing)
					return endingMargin;
				// Editing && value = NaN && !InSerialization
				return GaugeGeometry.ScaleEndingMargin(GaugeKind());
			}
			set 
			{
				endingMargin = value;
                ObjectModelBrowser.NotifyChanged(this);
			}
		}

		/// <summary>
		/// The position of the scale, relative to boundaries of the gauge.  The distance of the boundaries of the gauge is equivalent of position set to 100.
		/// </summary>
        [DefaultValue(double.NaN)]
        [TypeConverter(typeof(DoubleWithAutoConverter))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0, 100)]
        public double Position
        {
            get
            {
				if(!double.IsNaN(position) || InSerialization() || !Editing)
					return position;
				// Editing && value = NaN && !InSerialization
				return GaugeGeometry.ScalePosition(GaugeKind());
            }
            set
            {
				position = value;
                ObjectModelBrowser.NotifyChanged(this);
			}
        }

		/// <summary>
		/// The radial angle in degrees, 0-359, that denotes the starting and ending point of the scale (wrt. the margins)
		/// </summary>
		[DefaultValue(double.NaN)]
		[TypeConverter(typeof(DoubleWithAutoConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[NotifyParentProperty(true)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0, 360)]
		public double StartingAngle		
		{
			get 
			{
				if(!double.IsNaN(startingAngle) || InSerialization() || !Editing)
					return startingAngle;
				// Editing && value = NaN && !InSerialization
				return GaugeGeometry.StartingAngle(GaugeKind());
			} 
			set 
			{
				startingAngle = value;
                ObjectModelBrowser.NotifyChanged(this);
			}
		}
        
		/// <summary>
		///  Centerpoint in percentage coordinate (rel to the gauge size)
		/// </summary>
		[Editor(typeof(SizePositionEditor), typeof(UITypeEditor))]
		[DefaultValue(null)]
		public Point2D RelativeCenter
		{
			get 
			{
				if(relativeCenter != null || InSerialization() || !Editing)
					return relativeCenter;
				// Editing && value = NaN && !InSerialization
				return GaugeGeometry.RelativeCenter(GaugeKind());
			} 
			set 
			{ 
				relativeCenter = value;
				ObjectModelBrowser.NotifyChanged(this);
			} 
		}

        internal double EffectiveStartingAngle(SubGauge gauge)
        {
            if (double.IsNaN(startingAngle) && gauge != null)
                return GaugeGeometry.StartingAngle(gauge.GaugeKind);
            else
                return startingAngle;
        }


        internal double EffectiveStartingMargin(SubGauge gauge)
        {
            if (double.IsNaN(startingMargin) && gauge != null)
                return GaugeGeometry.ScaleStartingMargin(gauge.GaugeKind);
            else
                return startingMargin;
        }

        internal double EffectiveEndingMargin(SubGauge gauge)
        {
            if (double.IsNaN(endingMargin) && gauge != null)
                return GaugeGeometry.ScaleEndingMargin(gauge.GaugeKind);
            else
                return endingMargin;
        }

        internal double EffectivePosition(SubGauge gauge)
        {
            if (double.IsNaN(position) && gauge != null)
                return gauge.Skin.EffectiveScalePosition(gauge);
            else
                return Math.Max(1,position);
        }

        internal Point2D EffectiveRelativeCenter(SubGauge gauge)
        {
            if (relativeCenter == null && gauge != null)
                return GaugeGeometry.RelativeCenter(gauge.GaugeKind);
            else
                return relativeCenter;
        }

		private bool InSerialization()
		{
			return ObjectModelBrowser.InSerialization(this);
		}

		private GaugeKind GaugeKind()
		{
			// If this is part of a skin, take the gauge kind from skin,
			// otherwise take it from the owning gauge
			Skin skin = (this as IObjectModelNode).ParentNode as Skin;
			if(skin != null)
				return skin.Kind;
			SubGauge gauge = ObjectModelBrowser.GetOwningGauge(this);
			if(gauge != null)
				return gauge.GaugeKind;
			throw new Exception("Gauge kind not known for a ScaleLayout object.");
		}


        #region --- IObjectModelNode Implementation ---
        private IObjectModelNode parent;
        IObjectModelNode IObjectModelNode.ParentNode
        {
            get { return parent; }
            set { parent = value; }
        }

        #endregion

		internal ScaleLayout Copy()
		{
			ScaleLayout scaleLayout = new ScaleLayout();
			scaleLayout.Position = position;
			if (relativeCenter == null)
			{
				scaleLayout.RelativeCenter = null;
			}
			else
			{
				scaleLayout.RelativeCenter = new Point2D();
				scaleLayout.RelativeCenter.X = relativeCenter.X;
				scaleLayout.RelativeCenter.Y = relativeCenter.Y;
			}
			scaleLayout.StartingAngle = startingAngle;
			scaleLayout.EndingMargin = endingMargin;
			scaleLayout.StartingMargin = startingMargin;
			return scaleLayout;
		}
    }

    // ===============================================================================================

    /// <summary>
    /// Contains a collection of <see cref="Scale"/> objects.
    /// </summary>
	[Serializable]
	public class ScaleCollection : NamedObjectCollection
	{
		internal override NamedObject CreateNewMember()
		{
			Scale newMember = new Scale();
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
		public Scale AddNewMember(string newMemberName)
		{
			Scale newMember = AddNewMemberFrom(newMemberName,"Main");
			if(newMember == null)
			{
				newMember = new Scale(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Scale"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new Scale AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as Scale;
		}

		#endregion

		/// <summary>
		/// Retrieves the Scale from the collection based on its order of addition to the collection.  Main Scale is 0.
		/// </summary>
		/// <param name="ix">the number or the name of the Scale as it was being added</param>
		/// <returns>the Scale</returns>
		public new Scale this[object ix]
		{
			get { return base[ix] as Scale; }
			set { base[ix] = value; }
		}

//
//		/// <summary>
//		/// Retrieves the Scale from the collection based on its name, given when the Scale was created.
//		/// </summary>
//		/// <param name="name">the name of the requested Scale</param>
//		/// <returns>the Scale</returns>
//		public new Scale this[string name] 
//		{ 
//			get { return base[name] as Scale; }
//			set { base[name] = value; }
//		}

      #region --- Client-side serialization ---
#if WEB 

      internal ArrayList ExportJsArray()
      {
        ArrayList scales = new ArrayList();

        for (int i = 0; i < this.Count; i++)
          scales.Insert(i, this[i].ExportJsObject());

        return scales;
      }

      internal void ImportJsArray(IEnumerable scales)
      {
        foreach (Hashtable scale in scales)
          this[(string)scale["name"]].ImportJsObject(scale);
      }

#endif
      #endregion

	}
}
