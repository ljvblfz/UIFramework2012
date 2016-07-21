using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies available styles for annotations.
    /// </summary>
	public enum ScaleAnnotationStyleKind
	{
		Default,
		BlackIce,
		ArcticWhite,
		ArcticWhiteLinear,
		Monochrome,
		MonochromeLinear,
		Custom
	}

    /// <summary>
    /// Specifies how annotations will be created for a <see cref="Range"/> object.
    /// </summary>
	public enum AnnotationValueSetKind
	{
		AtMajorTickMarks,
		AtMinAndMaxValues,
		DefinedByStep,
		Custom
	}

    /// <summary>
    /// Specifies how annotation labels are rendered.
    /// </summary>
	public enum LabelSideKind
	{
		ToTheLeft,
		ToTheRight
	}

	/// <summary>
	/// Represents a visual annotation set of a <see cref="Range"/> object.
	/// </summary>
    [TypeConverter(typeof(NamedObjectConverter))]
    [Serializable]
	public class Annotation : NamedObject
	{
		private AnnotationValueSetKind valuesKind = AnnotationValueSetKind.AtMajorTickMarks;
        private double[] values;
		private bool customValues = false;
        private TextAnnotation[] texts;

		private string scaleAnnotationStyleName = "Auto";

		private string formattingString = "G";

		private bool enabled = true;

        private double step = 0;
		private double scaleFactor = 1.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Annotation"/> class.
        /// </summary>
        public Annotation() : this(string.Empty) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Annotation"/> class with the specified name.
        /// </summary>
        public Annotation(string name) : base(name) { }

        /// <summary>
        /// Gets all the <see cref="TextAnnotation"/> objects belonging to this object.
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]		
		public TextAnnotation[] Texts 
		{
			get
			{
				double[] vals = Values;
				if(texts != null && texts.Length == vals.Length) // maybe the texts are good. Check if values are equal
				{
					bool textsGood = true;
					for(int i=0; i<texts.Length; i++)
					{
						if(texts[i].Tag == null || (double)(texts[i].Tag)!= vals[i])
						{
							textsGood = false;
							break;
						}
					}
					if(textsGood)
						return texts;
				}
				CreateTexts();
				return texts;
			}
		}

        /// <summary>
        /// Gets or sets the <see cref="AnnotationValueSetKind"/> for this object.
        /// </summary>
		[Category("General")]
		[Description("Set of values")]
		[NotifyParentProperty(true)]
		[DefaultValue(typeof(AnnotationValueSetKind),"AtMajorTickMarks")]
		public AnnotationValueSetKind AnnotationValueSetKind 
		{
			get { return valuesKind; } 
			set 
			{
				if(valuesKind != value && value != AnnotationValueSetKind.Custom) 
					values = null; 
				valuesKind = value;
			}
		}

        /// <summary>
        /// Gets or sets the visibility of this annotation set.
        /// </summary>
		[Category("General")]
		[Description("Annotation visible")]
        [NotifyParentProperty(true)]
        [DefaultValue(true)]
		public bool Visible { get { return enabled; } set { enabled = value; ObjectModelBrowser.NotifyChanged(this); } }

        /// <summary>
        /// Gets or sets the formatting string for this annotation set.
        /// </summary>
		[Category("General")]
		[Description("Value formatting string")]
        [NotifyParentProperty(true)]
        [DefaultValue("G")]
        public string Format { get { return formattingString; } set { formattingString = value; ObjectModelBrowser.NotifyChanged(this); } }

        /// <summary>
        /// Gets or sets the increment for this annotation set.
        /// </summary>
        [Category("General")]
        [Description("Annotation step used when AnnotationValueSetKind = DefinedByStep. Must be > 0")]
        [NotifyParentProperty(true)]
        [DefaultValue(0.0)]
        public double Step { get { return step; } set { step = value; ObjectModelBrowser.NotifyChanged(this); } }
		
		/// <summary>
		/// Gets or sets the scaling factor for this annotation set.
		/// </summary>
		[Category("General")]
		[Description("Annotation scale factor")]
        [NotifyParentProperty(true)]
        [DefaultValue(1.0)]
        public double ScaleFactor { get { return scaleFactor; } set { scaleFactor = value; texts = null; ObjectModelBrowser.NotifyChanged(this); } }
		
		/// <summary>
		/// Gets or sets the style name of the <see cref="ScaleAnnotationStyle"/> to be used.
		/// </summary>
		[Category("General")]
		[Description("Annotation style name")]
		[DefaultValue("Auto")]
		public string ScaleAnnotationStyleName { get { return scaleAnnotationStyleName; } set { scaleAnnotationStyleName = value; texts = null; ObjectModelBrowser.NotifyChanged(this); } }

        /// <summary>
        /// Gets or sets the <see cref="ScaleAnnotationStyleKind"/> to be used.
        /// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]	
		public ScaleAnnotationStyleKind ScaleAnnotationStyleKind 
		{
			get { return ScaleAnnotationStyle.NameToKind(scaleAnnotationStyleName); } 
			set { scaleAnnotationStyleName = ScaleAnnotationStyle.KindToName(value); texts = null; ObjectModelBrowser.NotifyChanged(this); } 
		}

        /// <summary>
        /// Gets or sets the values of the corresponding <see cref="TextAnnotation"/> objects.
        /// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double[] Values 
		{
			get 
			{
				if (values == null || !customValues)
					values = CreateValues();
				return values; 
			} 
			set 
			{
				values = value; customValues = (value != null); 
			}
		}

		internal ScaleAnnotationStyle ScaleAnnotationStyle 
		{
			get
			{
				SubGauge gauge = ObjectModelBrowser.GetOwningGauge(this);
				if(scaleAnnotationStyleName == "Auto")
				{
					ScaleAnnotationStyle style = gauge.ScaleAnnotationStyles[gauge.Theme.EName(scaleAnnotationStyleName)];
					if(style == null)
						return gauge.ScaleAnnotationStyles["Default"];
					else
						return style;
				}
				return gauge.ScaleAnnotationStyles[scaleAnnotationStyleName];
			}
		}

		private void CreateTexts()
		{
			double[] vals = Values;
			if(vals == null || vals.Length == 0)
			{
				texts = null;
				return;
			}

			texts = new TextAnnotation[vals.Length];

			for(int i=0; i<vals.Length; i++)
			{
				double val = vals[i]/scaleFactor;
				if(formattingString.ToLower() == "g")
					val = Math.Round(val,12); // This is supposed to fix the floating point error accumulation
				string str = val.ToString(formattingString);
				texts[i] = new TextAnnotation("",str,ScaleAnnotationStyle.TextStyle);
				texts[i].Tag = vals[i];
			}
		}

		private double[] CreateValues()
		{
			double[] vals;
            AnnotationValueSetKind lsk = valuesKind;
            if (lsk == AnnotationValueSetKind.DefinedByStep && Step <= 0)
                lsk = AnnotationValueSetKind.AtMinAndMaxValues;
			switch(lsk)
			{
				case AnnotationValueSetKind.AtMajorTickMarks:
				{
					if(Range.MajorTickMarks == null)
						return new Double[0];
					int n = Range.MajorTickMarks.Count;
					if(n == 0)
						return null;
					vals = new double[n];
					for(int i=0; i<n; i++)
						vals[i] = Range.MajorTickMarks[i].Value;
					return vals;
				}
				case AnnotationValueSetKind.AtMinAndMaxValues:
				{
					vals = new double[2];
					vals[0] = Range.EffectiveMinValue;
					vals[1] = Range.EffectiveMaxValue;
					return vals;
				}
                case AnnotationValueSetKind.DefinedByStep:
                {
                    int i0 = (int)(Range.EffectiveMinValue / Step + 0.999);
                    int i1 = (int)(Range.EffectiveMaxValue / Step + 0.001);
                    int n = i1 - i0 + 1;
                    vals = new double[n];
                    for (int i = 0; i < n; i++)
                        vals[i] = (i0 + i) * Step;
                    return vals;
                }
				case AnnotationValueSetKind.Custom:
					return values;
				default:
					return null;
			}
		}

		internal Range Range { get { return ObjectModelBrowser.GetAncestorByType(this,typeof(Range)) as Range; } }

		internal void Render(RenderingContext context)
		{
			if(!Visible)
				return;
			Range range = ObjectModelBrowser.GetAncestorByType(this,typeof(Range)) as Range;
			if(range == null || !range.Visible)
				return;
			Scale scale = range.Scale;
			if(scale == null || !scale.Visible)
				return;
			SubGauge gauge = scale.Gauge;
			
			TextAnnotation[] texts = this.Texts;
			if(texts == null)
				return;

			if(SubGauge.IsInGroup(gauge.GaugeKind,GaugeKindGroup.Radial) && scale.ScaleLayout.EffectivePosition(gauge) <= 0)
					return;

			GaugeKind kind = gauge.GaugeKind;
			TextStyle textStyle = ScaleAnnotationStyle.TextStyle;
			TextRenderingContext trContext = context.Engine.Factory.CreateTextRenderingContext(textStyle,context, GaugeGeometry.LinearSize(gauge));

			double rMin = range.EffectiveMinValue;
			double rMax = range.EffectiveMaxValue;

			double angleDeg = 0;
			Size2D textDisp = new Size2D(0,0);

			for(int i=0; i<Texts.Length; i++)
			{
				Size2D txSize = trContext.MeasureString(texts[i].Text);

				Point2D pt1 = range.WorldToRenderingCoordinates(values[i]);

				// Text displacement relative to the reference point defined by the range

				Size2D dir;
				if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial))
				{
					dir = scale.WorldToRenderingCoordinates(values[i])-scale.CenterPoint(); 
					dir = dir.Unit();
					switch(ScaleAnnotationStyle.LabelOrientation)
					{
						case LabelOrientation.Horizontal:
							textDisp = 0.5*(txSize.Width * dir.Width*dir.Width + txSize.Height*dir.Height*dir.Height)*dir;
							break;
						case LabelOrientation.Vertical:
							textDisp = 0.5*(txSize.Height * dir.Width*dir.Width + txSize.Width*dir.Height*dir.Height)*dir;
							angleDeg = 90;
							break;
						case LabelOrientation.NormalToScale:
							angleDeg = 180/Math.PI*Math.Atan2(dir.Height,dir.Width);
							textDisp = txSize.Width*0.5f*dir;
							break;
						case LabelOrientation.ScaleDirection:
							angleDeg = 180/Math.PI*Math.Atan2(dir.Height,dir.Width) - 90;
							textDisp = (txSize.Height*0.5f)*dir;
							break;
						default:
							throw new Exception("LabelOrientation." + ScaleAnnotationStyle.LabelOrientation.ToString() + " not handled.");
					}
				}
				else
				{
					bool scaleVertical = (kind == GaugeKind.LinearVertical);
					if(scaleVertical)
						dir = new Size2D(-1,0);
					else
						dir = new Size2D(0,1);
					switch(ScaleAnnotationStyle.LabelOrientation)
					{
						case LabelOrientation.Horizontal:
							textDisp = 0.5*(txSize.Width * dir.Width*dir.Width + txSize.Height*dir.Height*dir.Height)*dir;
							break;
						case LabelOrientation.Vertical:
							textDisp = 0.5*(txSize.Height * dir.Width*dir.Width + txSize.Width*dir.Height*dir.Height)*dir;
							angleDeg = scaleVertical? 90:-90;
							break;
						case LabelOrientation.NormalToScale:
							angleDeg = scaleVertical? 0:-90;
							textDisp = txSize.Width*0.5f*dir;
							break;
						case LabelOrientation.ScaleDirection:
							angleDeg = scaleVertical? 90:0;
							textDisp = (txSize.Height*0.5f)*dir;
							break;
						default:
							throw new Exception("LabelOrientation." + ScaleAnnotationStyle.LabelOrientation.ToString() + " not handled.");
					}
				}
				if(angleDeg < -90 || angleDeg > 90)
					angleDeg = angleDeg + 180;
				texts[i].AngleDegrees = angleDeg;

				// Text location
				float scalingFactor = GaugeGeometry.LinearSize(gauge);
				switch(ScaleAnnotationStyle.LabelSideKind)
				{
					case LabelSideKind.ToTheRight:
					{
						double totalOffset = range.OccupiedWidthPerc(values[i],false) + ScaleAnnotationStyle.Offset;
						// Reference point defined by the range;
						Point2D refPt = pt1 - totalOffset*0.01f*scalingFactor*dir;
						texts[i].Location = refPt - textDisp;
					}
						break;
					case LabelSideKind.ToTheLeft:
					{
						double totalOffset = range.OccupiedWidthPerc(values[i],true) + ScaleAnnotationStyle.Offset;
						// Reference point defined by the range;
						Point2D refPt = pt1 + totalOffset*0.01f*scalingFactor*dir; 
						texts[i].Location = refPt + textDisp;
					}
						break;
					default:
						throw new Exception("LabelSideKind." + ScaleAnnotationStyle.LabelSideKind.ToString() + " not handled.");
				}
				
				if(texts[i].TextStyle.FontColor.IsEmpty)
				{
					double a = (values[i] - rMin)/(rMax-rMin);
					trContext.FontColor = ObjectModelBrowser.GetOwningTopmostGauge(this).Palette.AnnotationBaseColor.ColorAt((float)a);
				}

				texts[i].Render(context,trContext);
			}

			trContext.Dispose();
		}

	}


    /// <summary>
    /// Represents a style definiation for <see cref="Annotation"/> objects.
    /// </summary>
	[Serializable]
    public class ScaleAnnotationStyle : NamedObject, ISizePositionRangeProvider
	{
		private LabelOrientation labelOrientation = LabelOrientation.NormalToScale;
		private LabelSideKind labelSideKind = LabelSideKind.ToTheRight;
		private string textStyleName = "Default";

        private string majorTickMarkStyleName = "Default";
        private string minorTickMarkStyleName = "Default";

        private Size2D majorTickmarkSize = new Size2D(2, 4);
        private Size2D minorTickmarkSize = new Size2D(0.5f, 3);
        private double majorTickmarkOffset = 0;
        private double minorTickmarkOffset = 0;

		private double offset = 1; // % of gauge size

        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleAnnotionStyle"/> class.
        /// </summary>
		public ScaleAnnotationStyle() : base() { }
		
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleAnnotionStyle"/> class with the specified name.
        /// </summary>
        public ScaleAnnotationStyle(string name) : base(name) { }
		
        #region --- Tickmarks Properties ---

        /// <summary>
        /// Gets or sets the style name for major tick marks.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Description("Major tickmarks style")]
        [TypeConverter(typeof(MarkerStyleNameConverter))]
        [DefaultValue("Default")]
        public string MajorTickMarkStyleName { get { return majorTickMarkStyleName; } set { majorTickMarkStyleName = value; } }

        /// <summary>
        /// Gets or sets the style name for minor tick marks.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Description("Minor tickmarks style")]
        [TypeConverter(typeof(MarkerStyleNameConverter))]
        [DefaultValue("Default")]
        public string MinorTickMarkStyleName { get { return minorTickMarkStyleName; } set { minorTickMarkStyleName = value; } }

        /// <summary>
        /// Gets or sets the size of minor ticks marks relative to the size of the containing <see cref="qwe Gauge ghj"/> object.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Description("Minor tickmarks size in percentages of gauge size")]
        [Editor(typeof(SizePositionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[DefaultValue(typeof(Size2D),"0.5, 3")]
		public Size2D MinorTickmarkSize { get { return minorTickmarkSize; } set { minorTickmarkSize = value; } }

        /// <summary>
        /// Gets or sets the size of major ticks marks relative to the size of the containing <see cref="Gauge"/> object.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Description("Major tickmarks size in percentages of gauge size")]
        [Editor(typeof(SizePositionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[DefaultValue(typeof(Size2D),"2,4")]
        public Size2D MajorTickmarkSize { get { return majorTickmarkSize; } set { majorTickmarkSize = value; } }

        /// <summary>
        /// Gets or sets the offset of major ticks marks relative to the containing <see cref="Range"/> object.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Description("Major tickmarks offset from range center line in percentages of control size")]
        [DefaultValue(0.0)]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(-20, 20)]
        public double MajorTickmarkOffset { get { return majorTickmarkOffset; } set { majorTickmarkOffset = value; } }

        /// <summary>
        /// Gets or sets the offset of minor ticks marks relative to the containing <see cref="Range"/> object.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [Description("Minor tickmarks offset from range center line in percentages of control size")]
        [DefaultValue(0.0)]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(-20, 20)]
        public double MinorTickmarkOffset { get { return minorTickmarkOffset; } set { minorTickmarkOffset = value; } }

        /// <summary>
        /// Gets or sets the style for major tick marks.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarkerStyle MajorTickmarkStyle { get { return TopGauge.MarkerStyles[MajorTickMarkStyleName]; } }

        /// <summary>
        /// Gets or sets the style for minor tick marks.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarkerStyle MinorTickmarkStyle { get { return TopGauge.MarkerStyles[MinorTickMarkStyleName]; } }
        
        #endregion
	
	    /// <summary>
	    /// Gets or sets the orientation of the annotation labels.
	    /// </summary>
		[Category("General")]
		[Description("Label orientation")]
		[DefaultValue(typeof(LabelOrientation),"NormalToScale")]
		public LabelOrientation LabelOrientation { get { return labelOrientation; } set { labelOrientation = value; } }

        /// <summary>
        /// Gets or sets the position of the annotation labels.
        /// </summary>
		[Category("General")]
		[Description("Side of the range where the label should be displayed")]
		[DefaultValue(typeof(LabelSideKind),"ToTheRight")]
        public LabelSideKind LabelSideKind { get { return labelSideKind; } set { labelSideKind = value; ObjectModelBrowser.NotifyChanged(this); } }

        /// <summary>
        /// Gets or sets the style name for the annotation labels.
        /// </summary>
		[Category("General")]
		[Description("Text style name")]
		[TypeConverter(typeof(TextStyleNameConverter))]
		[DefaultValue("Default")]
		public string TextStyleName { get { return textStyleName; } set { textStyleName = value; } }

        /// <summary>
        /// Gets or sets the style for the annotation labels.
        /// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyleKind TextStyleKind { get { return TextStyle.NameToKind(textStyleName); } set { textStyleName = TextStyle.KindToName(value); } }

		[Category("General")]
		[Description("Text style used in this annotation")]
		internal TextStyle TextStyle 
		{
			get
			{
				SubGauge topGauge = ObjectModelBrowser.GetOwningTopmostGauge(this);
				if(topGauge != null)
					return topGauge.TextStyles[textStyleName];
				else
					return null;
			}
		}

        /// <summary>
        /// Gets or sets offset of the annotation labels relative to the containing <see cref="Range"/> object.
        /// </summary>
		[Category("General")]
		[Description("Annotation offset from the range in percentages of the control size")]
		[DefaultValue(1.0)]
		[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor)), ValueRange(0, 10, 0.5)]
        public double Offset { get { return offset; } set { offset = value; ObjectModelBrowser.NotifyChanged(this); } }

        private SubGauge TopGauge { get { return ObjectModelBrowser.GetOwningTopmostGauge(this); } }


        #region ---  ISizePositionRangeProvider implementation ---
        public virtual void GetRangesAndSteps(string propertyName,
            ref float x0, ref float x1, ref float stepX,
            ref float y0, ref float y1, ref float stepY)
        {
            if (propertyName == "MajorTickmarkSize" || propertyName == "MinorTickmarkSize")
            {
                x0 = 0;
                x1 = 20;
                stepX = 0.2f;

                y0 = 0;
                y1 = 20;
                stepY = 0.2f;
            }
        }
        #endregion
 
	
		#region --- ScaleAnnotationStyleKind Handling ---

        /// <summary>
        /// Gets the style type of this object.
        /// </summary>
		public ScaleAnnotationStyleKind ScaleAnnotationStyleKind { get { return NameToKind(Name); } }

		internal static ScaleAnnotationStyleKind NameToKind(string name)
		{
			try
			{
				string name1 = name.Replace(" ","");
				TypeConverter tc = new EnumConverter(typeof(ScaleAnnotationStyleKind));
				return (ScaleAnnotationStyleKind)tc.ConvertFromString(name1);
			}
			catch
			{
				return ScaleAnnotationStyleKind.Custom;
			}
		}
		internal static string KindToName(ScaleAnnotationStyleKind kind)
		{
			switch(kind)
			{
				case ScaleAnnotationStyleKind.BlackIce: return "BlackIce"; 
				case ScaleAnnotationStyleKind.ArcticWhite: return "Arctic White"; 
				case ScaleAnnotationStyleKind.ArcticWhiteLinear: return "Arctic White Linear"; 
				case ScaleAnnotationStyleKind.MonochromeLinear: return "Monochrome Linear"; 
				default: return kind.ToString();
			}
		}

		#endregion
	}
	

    /// <summary>
    /// Contains a collection of <see cref="Annotation"/> objects.
    /// </summary>
    [Serializable]
    public class AnnotationCollection : NamedObjectCollection
    {
        /// <summary>
        /// Adds an <see cref="Annotation"/> object to this collection.
        /// </summary>
        /// <param name="a">The <see cref="Annotation"/> object to add to this collection</param>
        /// <returns>The index at which the <see cref="Annotation"/> object was added.</returns>
        public int Add(Annotation a)
        {
            (a as IObjectModelNode).ParentNode = this;
            return base.Add(a);
        }

        internal override NamedObject CreateNewMember()
        {
            Annotation newMember = new Annotation();
            SelectGenericNewName(newMember);
            Add(newMember);
            return newMember;
        }

        /// <summary>
        /// Gets or sets the <see cref="Annotation"/> object at the specified index.
        /// </summary>
        /// <param name="ix">The index of the <see cref="Annotation"/> object to get or set.</param>
        /// <returns>The <see cref="Annotation"/> object at the specified index.</returns>
        public new Annotation this[object ix]
        {
            get { return base[ix] as Annotation; }
            set { base[ix] = value; }
        }
    }

    /// <summary>
    /// Contains a collection of <see cref="ScaleAnnotationStyle"/> objects.
    /// </summary>
	[Serializable]
	public class ScaleAnnotationStyleCollection : NamedObjectCollection
	{
        internal ScaleAnnotationStyleCollection(bool populateInitialContents) : base(populateInitialContents) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleAnnotationStyleCollection"/> class.
        /// </summary>
        public ScaleAnnotationStyleCollection() : this(false) { }
        
        /// <summary>
        /// Adds a <see cref="ScaleAnnotationStyle"/> object to this collection.
        /// </summary>
        /// <param name="a">The <see cref="ScaleAnnotationStyle"/> object to add to this collection.</param>
        /// <returns>The index at which the <see cref="ScaleAnnotationStyle"/> was added.</returns>
		public int Add(ScaleAnnotationStyle a)
		{
			(a as IObjectModelNode).ParentNode = this;
			return base.Add(a);
		}
        
        internal override void PopulateInitialContents()
        {
            Add(new ScaleAnnotationStyle("Default"));
        }

		internal override NamedObject CreateNewMember()
		{
			ScaleAnnotationStyle newMember = new ScaleAnnotationStyle();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;
		}

        /// <summary>
        /// Gets or sets the <see cref="ScaleAnnotationStyle"/> object at the specified index.
        /// </summary>
        /// <param name="ix">The index of the <see cref="ScaleAnnotationStyle"/> object to get or set.</param>
        /// <returns>The <see cref="ScaleAnnotationStyle"/> object at the specified index.</returns>		
		public new ScaleAnnotationStyle this[object ix]
		{
			get 
			{
				if(ix is ScaleAnnotationStyleKind)
					ix = ScaleAnnotationStyle.KindToName((ScaleAnnotationStyleKind)ix);
				return base[ix] as ScaleAnnotationStyle; 
			}
			set 
			{
				if(ix is ScaleAnnotationStyleKind)
					ix = ScaleAnnotationStyle.KindToName((ScaleAnnotationStyleKind)ix);
				base[ix] = value; 
			}
		}
		
		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Default". If member named "Default" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public ScaleAnnotationStyle AddNewMember(string newMemberName)
		{
			ScaleAnnotationStyle newMember = AddNewMemberFrom(newMemberName,"Default");
			if(newMember == null)
			{
				newMember = new ScaleAnnotationStyle(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="ScaleAnnotationStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new ScaleAnnotationStyle AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as ScaleAnnotationStyle;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="ScaleAnnotationStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="scaleAnnotationStyleKind"><see cref="ScaleAnnotationStyleKind"/> of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public ScaleAnnotationStyle AddNewMemberFrom(string newMemberName, ScaleAnnotationStyleKind scaleAnnotationStyleKind)
		{
			return base.AddNewMemberFrom(newMemberName,ScaleAnnotationStyle.KindToName(scaleAnnotationStyleKind)) as ScaleAnnotationStyle;
		}

		#endregion

	}

	
	internal class ScaleAnnotationStyleNameConverter : SelectedNameConverter 
	{
		public ScaleAnnotationStyleNameConverter() { }

		protected override NamedObjectCollection GetNamedCollection(SubGauge gauge)
		{
			return gauge.ScaleAnnotationStyles;
		}
	}

}
