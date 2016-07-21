using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
#if WEB
using System.Web;
#endif


namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies the type of indicator to render.
    /// </summary>
	public enum IndicatorKind
	{
		Circle,
		Rectangle,
		Text,
		CustomImage,
		Multiple
	}

    /// <summary>
    /// Specifies how the value of an indicator is set or retrieved.
    /// </summary>
	public enum IndicatorValueKind
	{
		Numeric,
		State,
		CustomSetColor
	}

    /// <summary>
    /// Specifies how indicators with <see cref="IndicatorKind"/> set to Multiple are rendered.
    /// </summary>
	public enum IndicatorLayoutKind
	{
		Row,
		Column
	}

    /// <summary>
    /// Specifies how indicator labels are rendered.
    /// </summary>
	public enum IndicatorLabelPositionKind
	{
		LeftOfImage,
		RightOfImage,
		AboveImage,
		BelowImage
	}

	/// <summary>
	/// Represents a visual indicator element of a <see cref="Gauge"/> object.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(IndicatorTypeConverter))]
	public class Indicator : NamedObject, ISizePositionRangeProvider
	{
		// Kind related mambers

		private IndicatorKind kind = IndicatorKind.Circle;
		private Image indicatorImage = null; // for kind=CustomImage
		private String indicatorImagePath = null; // for kind=CustomImage
		private IndicatorCollection indicatorList = new IndicatorCollection(); // for kind=Multiple

		// Value related members

		private IndicatorValueKind indicatorValueKind = IndicatorValueKind.Numeric;
		// Numeric value kind:
		private double minValue = 0;
		private double maxValue = 100;
		private double val = 0;
		private MultiColor valueColors = new MultiColor(Color.Green);
		private string valueObjectPath = "";

		// State value kind:
		private string state = "";
		private StateColorCollection stateColorCollection = new StateColorCollection();

		// Custom set color kind
		private Color customColor = Color.Green;

		// Layout data
		
		private IndicatorLayoutKind indicatorListLayoutKind = IndicatorLayoutKind.Column;
		private Point2D location = new Point2D(50,25);
		private Size2D imageSize = new Size2D(8,8);
		private IndicatorLabelPositionKind indicatorLabelPositionKind = IndicatorLabelPositionKind.RightOfImage;

		// Annotation

		private string label = "";
		private string labelStyleName = "DefaultIndicatorLabelStyle"; // NB: create predefined style for indicator labels

		// Working data, set and valid during rendering operation
		private SubGauge topGauge = null;
		private SubGauge gauge = null;   
		private Point2D targetLocation;
		private Size2D targetSize;
		private Size2D labelTargetSize;
		private Point2D labelTargetLocation;
		private Size2D imageTargetSize;
		private Point2D imageTargetLocation;
		private TextRenderingContext textRenderingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Indicator"/> class.
        /// </summary>
		public Indicator()
		{ 
			(indicatorList as IObjectModelNode).ParentNode = this;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Indicator"/> class with the specified name.
        /// </summary>
        public Indicator(string name) : base(name)
		{ 
			(indicatorList as IObjectModelNode).ParentNode = this;
		}

		#region --- Properties ---

		#region --- Kind Related Properties ---

        /// <summary>
        /// Gets or set the indicator type.
        /// </summary>
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(IndicatorKind.Circle)]
		public IndicatorKind IndicatorKind { get { return kind; } set { kind = value; } }

        /// <summary>
        /// Gets or sets an image used as the indicator.
        /// </summary>
		[DefaultValue(null)]
		public Image IndicatorImage 
		{ 
			get { return indicatorImage; } 
			set 
			{ 
				indicatorImage = value;
				indicatorImagePath = null;
			} 
		}

		/// <summary>
		/// Path of the image used for the indicator.
		/// </summary>
		[DefaultValue(null)]
		public string IndicatorImagePath
		{
			get { return indicatorImagePath; }
			set
			{
                indicatorImagePath = value.Trim();
            
                SubGauge topmostGauge = ObjectModelBrowser.GetOwningTopmostGauge(this);
                if (!topmostGauge.InDesignMode)
                {
                    try
                    {
#if WEB
                        Gauge thisGauge = topmostGauge.gaugeWrapper as Gauge;
                        string directory = Utils.ConvertUrl(HttpContext.Current, thisGauge.TemplateSourceDirectory, indicatorImagePath);
                        string absolutePath = Utils.MapPhysicalPath(directory);
                        indicatorImage = Image.FromFile(absolutePath);
#else
        				indicatorImage = Image.FromFile(indicatorImagePath);
#endif
                    }
                    catch (Exception)
                    {
                        indicatorImage = null;
                    }
                }
			}
		}

        /// <summary>
        /// Gets a collection of sub-indicators.
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public IndicatorCollection IndicatorList { get { return indicatorList; } }
		#endregion

		#region --- Value Related Properties ---

        /// <summary>
        /// Gets or sets the indicator value type.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
		[DefaultValue(IndicatorValueKind.Numeric)]
		public IndicatorValueKind IndicatorValueKind { get { return indicatorValueKind; } set { indicatorValueKind = value; } }

		// Numeric value kind:
		
		/// <summary>
		/// Gets or sets the minimum value of the indicator.
		/// </summary>
		[DefaultValue(0)]
		public double MinValue { get { return minValue; } set { minValue = value; } }

        /// <summary>
        /// Gets or sets the maximum value of the indicator.
        /// </summary>
        [DefaultValue(100)]
		public double MaxValue { get { return maxValue; } set { maxValue = value; } }

        /// <summary>
        /// Gets or sets the value of the indicator.
        /// </summary>
        [DefaultValue(0)]
		public double Value { get { return val; } set { val = value; } }

        /// <summary>
        /// Gets or sets the colors used to represent the value.
        /// </summary>
        [DefaultValue(typeof(MultiColor), "true,0=Green,1=Green")]
        [TypeConverter(typeof(MultiColorTypeConverter))]

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public MultiColor ValueColors { get { return valueColors; } set { valueColors = value; } }

        /// <summary>
        /// Gets or sets a path to a gauge (or child) object to retrieve a value from.
        /// </summary>		
		[DefaultValue("")]
		public string ValueObjectPath { get { return valueObjectPath; } set { valueObjectPath = value; } }

		// State value kind:
		
		/// <summary>
		/// Gets or sets the state of the current indicator.
		/// </summary>
		[DefaultValue("")]
		public string State { get { return state; } set { state = value; } }

        /// <summary>
        /// Gets a collection of all available states.
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public StateColorCollection StateColors { get { return stateColorCollection; } }
		
		// Custom set color kind:
		/// <summary>
		/// Gets or set the custom color of the indicator.
		/// </summary>
		[DefaultValue(typeof(Color),"Green")]
        public Color CustomColor { get { return customColor; } set { customColor = value; ObjectModelBrowser.NotifyChanged(this); } }
		
		#endregion

		#region --- Layout data ---

        /// <summary>
        /// Gets or sets the position of the indicator label.
        /// </summary>
		[Category("Indicator Layout Data")]
		[Description("Indicator text position")]
		[DefaultValue(IndicatorLabelPositionKind.RightOfImage)]
		public IndicatorLabelPositionKind LabelPositionKind { get { return indicatorLabelPositionKind; } set { indicatorLabelPositionKind = value; } }
		
		/// <summary>
        /// Gets or sets the layout of the indicator. (IndicatorKind.Multiple)
		/// </summary>
		[Category("Indicator Layout Data")]
		[DefaultValue(IndicatorLayoutKind.Column)]
		public IndicatorLayoutKind IndicatorListLayoutKind { get { return indicatorListLayoutKind; } set { indicatorListLayoutKind = value; } }
		
		/// <summary>
		/// Gets or sets the location of the indicator relative to the containing <see cref="Gauge"/> object.
		/// </summary>
		[Category("Indicator Layout Data")]
		[DefaultValue(typeof(Point2D),"50,25")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public Point2D Location { get { return location; } set { location = value; } }

        /// <summary>
        /// Gets or sets the size of the indicator relative to the containing <see cref="Gauge"/> object.
        /// </summary>
        [Category("Indicator Layout Data")]
		[DefaultValue(typeof(Size2D),"8,8")]
        [Editor(typeof(SizePositionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public Size2D ImageSize { get { return imageSize; } set { imageSize = value; } }

		#endregion

        /// <summary>
        /// Gets or sets the label of the indicator.
        /// </summary>
		[Category("Indicator Label Data")]
		[DefaultValue("")]
		public string Label { get { return label; } set { label = value; } }

        /// <summary>
        /// Gets or sets the style name of the indicator label.
        /// </summary>
		[Category("Indicator Label Data")]
		[DefaultValue("DefaultIndicatorLabelStyle")]
		public string LabelStyleName { get { return labelStyleName; } set { labelStyleName = value; } }

        /// <summary>
        /// Gets the style of the indicator label.
        /// </summary>
		[Category("Indicator Label Data")]
		[Description("Shortcut to the indicator text style")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyle LabelStyle 
		{
			get 
			{
				return ObjectModelBrowser.GetOwningTopmostGauge(this).TextStyles[labelStyleName]; 
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

		#endregion

		#region --- Rendering ---
		private bool visible = true;
		
		/// <summary>
		/// Gets or sets the visibility of the indicator.
		/// </summary>
		[Category("General")]
		[Description("Indicator visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

		internal void Render(RenderingContext context)
		{
			if(!Visible)
				return;

			gauge = ObjectModelBrowser.GetOwningGauge(this);
			textRenderingContext = context.Engine.Factory.CreateTextRenderingContext(LabelStyle,context, GaugeGeometry.LinearSize(gauge));
			topGauge = gauge.TopmostGauge;
			ComputeTargetSize(context);
			ComputeTargetLocation(context);

			switch(kind)
			{
				case IndicatorKind.Multiple:
					for(int i=0; i<indicatorList.Count; i++)
						indicatorList[i].Render(context);
					break;
				case IndicatorKind.Circle:
					RenderSimple(context.Engine.IndicatorCircleImage(),context);
					break;
				case IndicatorKind.Rectangle:
					RenderSimple(context.Engine.IndicatorRectImage(),context);
					break;
				case IndicatorKind.CustomImage:
				{
					if(indicatorImage == null)
					{
						LayerVisualPart vp = context.Engine.Factory.CreateLayerVisualPart("CustomIndicatorBitmap",null);
						RenderSimple(vp,context);				
					}
					else
					{
						LayerVisualPart vp = context.Engine.Factory.CreateLayerVisualPart("CustomIndicatorBitmap",indicatorImage);
						RenderSimple(vp,context);				
					}
				}
					break;
				case IndicatorKind.Text:
					RenderSimple(null,context);
					break;
				default:
					throw new Exception("Indicator kind = '" + kind.ToString() + "' rendering not implemented.");
			}		

			gauge = null;
			topGauge = null;
			textRenderingContext.Dispose();
			textRenderingContext = null;
		}

		private void RenderSimple(LayerVisualPart vp, RenderingContext context)
		{
			switch(indicatorLabelPositionKind)
			{
				case IndicatorLabelPositionKind.AboveImage:
					labelTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-labelTargetSize.Width)/2,targetLocation.Y + targetSize.Height-labelTargetSize.Height);
					imageTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-imageTargetSize.Width)/2,targetLocation.Y);
					break;
				case IndicatorLabelPositionKind.BelowImage:
					imageTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-imageTargetSize.Width)/2,targetLocation.Y + targetSize.Height - imageTargetSize.Height);
					labelTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-labelTargetSize.Width)/2,targetLocation.Y);
					break;
				case IndicatorLabelPositionKind.RightOfImage:
					imageTargetLocation = new Point2D(targetLocation.X, targetLocation.Y - (targetSize.Height-imageTargetSize.Height)/2);
					labelTargetLocation = new Point2D(targetLocation.X + targetSize.Width - labelTargetSize.Width,targetLocation.Y + (targetSize.Height-labelTargetSize.Height)/2);
					break;
				case IndicatorLabelPositionKind.LeftOfImage:
					labelTargetLocation = new Point2D(targetLocation.X, targetLocation.Y + (targetSize.Height-labelTargetSize.Height)/2);
					imageTargetLocation = new Point2D(targetLocation.X + targetSize.Width - imageTargetSize.Width,targetLocation.Y + (targetSize.Height-imageTargetSize.Height)/2);
					break;
				default: break;
			}

			Color color = GetIndicatorColor();

			RenderingContext myContext = null;
			RenderingContext mapAreaContext = null;
			// Rendering image
			if(kind != IndicatorKind.Text)
			{
                myContext = context.SetTargetArea(new Rectangle2D(imageTargetLocation,imageTargetSize));
                vp.RenderAfterSettingColor(myContext,color);
                mapAreaContext = myContext.SetAreaMapping(new Rectangle2D(new Point2D(), new Size2D(1,1)),true);
			}

			// Rendering label
			if(textRenderingContext != null)
			{
				myContext = context.SetTargetArea(new Rectangle2D(labelTargetLocation,labelTargetSize));
				if(kind == IndicatorKind.Text)
					textRenderingContext.FontColor = color;
				if(label != null && label != "")
				{
					context.Engine.DrawText(label,LabelStyle,myContext,textRenderingContext);
					// Map area handling
					if(topGauge.RenderIndicatorsMapAreas && kind == IndicatorKind.Text)
					{
						Size2D sz = textRenderingContext.MeasureString(label);
						mapAreaContext = myContext.SetAreaMapping(new Rectangle2D(new Point2D(), new Size2D(1,1)),true);
					}
				}
			}

			// Map area handling

			if(topGauge.RenderIndicatorsMapAreas)
			{
				PointF[] mapAreaPoints = new PointF[]
					{
						new PointF(0,0),
						new PointF(0,1),
						new PointF(1,1),
						new PointF(1,0)
					};
				Point[] maPoints = new Point[4];

				for(int i=0; i<4; i++)
				{
					PointF T = mapAreaContext.TransformPoint(mapAreaPoints[i]);
					maPoints[i] = new Point((int)(T.X+0.5f),(int)(T.Y+0.5f));
				}
				MapAreaPolygon mapArea = new MapAreaPolygon(maPoints);
				mapArea.SetObject(this);
				topGauge.MapAreas.Add(mapArea);
			}
		}

		private bool IsMemberOfMultipleIndicator 
		{
			get
			{
				return ((this as IObjectModelNode).ParentNode.ParentNode is Indicator);
			}
		}

		#region --- Size and Position ---

		private void ComputeTargetLocation(RenderingContext context)
		{
			if(IsMemberOfMultipleIndicator)
			{
				Indicator parent = (this as IObjectModelNode).ParentNode.ParentNode as Indicator;
				IndicatorCollection parentCollection = parent.IndicatorList;
				int ix = parentCollection.IndexOf(this.Name);
				int n = parentCollection.Count;
				if(ix == 0)
				{
					if(parent.IndicatorListLayoutKind == IndicatorLayoutKind.Row)
						targetLocation = parent.targetLocation + new Size2D(0,(parent.targetSize.Height-targetSize.Height)/2);
					else
						targetLocation = parent.targetLocation + new Size2D((parent.targetSize.Width-targetSize.Width)/2,parent.targetSize.Height-targetSize.Height);
				}
				else
				{
					Point2D prevLocation = parentCollection[ix-1].targetLocation;
					Size2D prevSize = parentCollection[ix-1].targetSize;
					if(parent.IndicatorListLayoutKind == IndicatorLayoutKind.Column)
						targetLocation =  new Point2D(parent.targetLocation.X + (parent.targetSize.Width-targetSize.Width)/2,prevLocation.Y-targetSize.Height);
					else
						targetLocation =  new Point2D(prevLocation.X+prevSize.Width,parent.targetLocation.Y + (parent.targetSize.Height-targetSize.Height)/2);
				}
			}
			else
			{
				float xc = context.TargetArea.X + context.TargetArea.Width * Location.X * 0.01f;
				float yc = context.TargetArea.Y + context.TargetArea.Height * Location.Y * 0.01f;
				targetLocation = new Point2D(xc-targetSize.Width/2,yc-targetSize.Height/2);
				if(kind != IndicatorKind.Multiple)
				{
					switch(indicatorLabelPositionKind)
					{
						case IndicatorLabelPositionKind.AboveImage:
							labelTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-labelTargetSize.Width)/2,targetLocation.Y + targetSize.Height-labelTargetSize.Height);
							imageTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-imageTargetSize.Width)/2,targetLocation.Y);
							break;
						case IndicatorLabelPositionKind.BelowImage:
							imageTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-imageTargetSize.Width)/2,targetLocation.Y);
							labelTargetLocation = new Point2D(targetLocation.X + (targetSize.Width-labelTargetSize.Width)/2,targetLocation.Y + (targetSize.Height-labelTargetSize.Height));
							break;
						case IndicatorLabelPositionKind.RightOfImage:
							imageTargetLocation = new Point2D(targetLocation.X, targetLocation.Y - (targetSize.Height-imageTargetSize.Height)/2);
							labelTargetLocation = new Point2D(targetLocation.X + targetSize.Width - labelTargetSize.Width,targetLocation.Y - (targetSize.Height-labelTargetSize.Height)/2);
							break;
						case IndicatorLabelPositionKind.LeftOfImage:
							labelTargetLocation = new Point2D(targetLocation.X, targetLocation.Y - (targetSize.Height-labelTargetSize.Height)/2);
							imageTargetLocation = new Point2D(targetLocation.X + targetSize.Width - imageTargetSize.Width,targetLocation.Y - (targetSize.Height-imageTargetSize.Height)/2);
							break;
						default: break;
					}
				}
			}
		}

		private void ComputeTargetSize(RenderingContext context)
		{
			if(kind == IndicatorKind.Multiple)
			{
				float w = 0;
				float h = 0;
				for(int i=0; i<indicatorList.Count; i++)
				{
					indicatorList[i].textRenderingContext = this.textRenderingContext;
					indicatorList[i].ComputeTargetSize(context);
					Size2D memberSize = indicatorList[i].targetSize;
					if(indicatorListLayoutKind == IndicatorLayoutKind.Row)
					{
						h = Math.Max(memberSize.Height,h);
						w = w + memberSize.Width;
					}
					else
					{
						w = Math.Max(memberSize.Width,w);
						h = h + memberSize.Height;
					}
				}
				targetSize = new Size2D(w,h);
			}
			else
			{
				ComputeLabelTargetSize(context);
				ComputeImageTargetSize(context);
				if(indicatorLabelPositionKind == IndicatorLabelPositionKind.AboveImage ||
					indicatorLabelPositionKind == IndicatorLabelPositionKind.BelowImage)
					targetSize = new Size2D(Math.Max(labelTargetSize.Width,imageTargetSize.Width), labelTargetSize.Height + imageTargetSize.Height*1.2f);
				else
					targetSize = new Size2D(labelTargetSize.Width + imageTargetSize.Width*1.2f, Math.Max(labelTargetSize.Height,imageTargetSize.Height));
			}
		}

		private void ComputeLabelTargetSize(RenderingContext context)
		{
			labelTargetSize = textRenderingContext.MeasureString(label);
		}

		private void ComputeImageTargetSize(RenderingContext context)
		{
			float w,h;
			if(kind == IndicatorKind.Text)
			{ // there is no image; size = 0
				w = 0;
				h = 0;
			}
			else
			{
				w = context.TargetArea.Width * ImageSize.Width * 0.01f;
				h = context.TargetArea.Height * ImageSize.Height * 0.01f;
			}
			imageTargetSize = new Size2D(w,h);
		}
		
		#endregion

		private Color GetIndicatorColor()
		{
			if(indicatorValueKind == IndicatorValueKind.CustomSetColor)
				return CustomColor;
			else if (indicatorValueKind == IndicatorValueKind.State)
			{
				StateColor sc = stateColorCollection[state];
				if(sc == null)
					return Color.Transparent;
				else
					return sc.Color;
			}

			// Numeric value kind
			double v = 0, vMin = 0, vMax = 0;
			if(valueObjectPath != null && valueObjectPath != "")
			{
				Pointer ptr = null;
				try
				{
					object obj = ObjectModelBrowser.GetObjectFromExpression(this,valueObjectPath);
					if(obj == null)
						throw new Exception("The value evaluated is null");
					ptr = obj as Pointer;
					if(ptr == null && obj is SubGauge)
						obj = (obj as SubGauge).Scales["Main"];
					if(ptr == null && obj is Scale)
						ptr = (obj as Scale).Pointers["Main"];
					if(ptr == null)
						throw new Exception("Cannot process object type '" + obj.GetType().Name + "'.");
				}
				catch(Exception ex)
				{
					throw new Exception("Indicator '" + Name + "' cannot compute the expression '" + valueObjectPath + "':\n" + ex.Message);
				}

				v = ptr.Value;
				vMin = ptr.Scale.MinValue;
				vMax = ptr.Scale.MaxValue;
			}
			else
			{
				v = val;
				vMin = minValue;
				vMax = maxValue;
			}
			float vRel = (float)((v-vMin)/(vMax-vMin));
			return valueColors.ColorAt(vRel);
		}

		#endregion

		#region ---  ISizePositionRangeProvider implementation ---
		public virtual void GetRangesAndSteps(string propertyName,
			ref float x0, ref float x1, ref float stepX,
			ref float y0, ref float y1, ref float stepY)
		{
			if (propertyName == "ImageSize")
			{
				x0 = 0;
				x1 = 20;
				stepX = 0.2f;

				y0 = 0;
				y1 = 20;
				stepY = 0.2f;
			}
			if (propertyName == "Location")
			{
				x0 = 0;
				x1 = 100;
				stepX = 0.2f;

				y0 = 0;
				y1 = 100;
				stepY = 0.2f;
			}
		}
		#endregion


    #region --- Client-side serialization ---
#if WEB 

    internal Hashtable ExportJsObject()
    {
      Hashtable indicator = new Hashtable();

      indicator.Add("name", Name);
      indicator.Add("valueObjectPath", ValueObjectPath);

      indicator.Add("minValue", MinValue);
      indicator.Add("maxValue", MaxValue);
      indicator.Add("value", Value);
      indicator.Add("state", State);
      indicator.Add("customColor", CustomColor.ToArgb());
      indicator.Add("visible", Visible);

      indicator.Add("indicatorKind", IndicatorKind.ToString());
      indicator.Add("indicatorValueKind", IndicatorKind.ToString());

      indicator.Add("indicatorList", IndicatorList.ExportJsArray());

      return indicator;
    }

    internal void ImportJsObject(Hashtable indicator)
    {
      if (indicator.ContainsKey("minValue"))
        MinValue = (double)indicator["minValue"];

      if (indicator.ContainsKey("maxValue"))
        MaxValue = (double)indicator["maxValue"];

      if (indicator.ContainsKey("value"))
        Value = (double)indicator["value"];

      if (indicator.ContainsKey("state"))
        State = (string)indicator["state"];

      if (indicator.ContainsKey("customColor"))
        CustomColor = Color.FromArgb(Convert.ToInt32((double)indicator["customColor"]));

      if (indicator.ContainsKey("visible"))
        Visible = (bool)indicator["visible"];

      if (indicator.ContainsKey("indicatorKind"))
        IndicatorKind = (IndicatorKind)Enum.Parse(typeof(IndicatorKind), (string)indicator["indicatorKind"]);

      if (indicator.ContainsKey("indicatorValueKind"))
          IndicatorValueKind = (IndicatorValueKind)Enum.Parse(typeof(IndicatorValueKind), (string)indicator["indicatorValueKind"]);

      if (indicator.ContainsKey("indicatorList"))
        IndicatorList.ImportJsArray((IEnumerable)indicator["indicatorList"]);
    }

#endif
    #endregion


	}
	// ========================================================================================================
	
	internal class IndicatorTypeConverter : NamedObjectConverter
	{
		public override bool GetPropertiesSupported (ITypeDescriptorContext context)
		{
			return true;
		}
		
		// Property strings format 
		//	propertyName=kindFlags:valueKindFlags
		// where kind flags are
		//	B = predefined bitmap
		//  C = custom bitmap
		//  T = text
		//  M = multiple
		// value kind flags are
		//  N = numeric
		//  S = state
		//  C = custom color

		string[] propsd = new string[]
			{
				"IndicatorImage=C:NSC",
				"IndicatorList=M",
				"Value=BC:N",
				"MinValue=BC:N",
				"MaxValue=BC:N",
				"ImageSize=BCT",
				"ValueColors=BCT:N",
				"ValueObjectPath=BC:N",
				"State=BCT:S",
				"StateColors=BCT:S",
				"CustomColor=BCT:C",
				"IndicatorValueKind=BCT",
				"IndicatorLabelPositionKind=BCT",
				"IndicatorListLayoutKind=M",
				"Label=BCT",
				"LabelStyleName=BCT",
				"LabelPositionKind=BCT",
				"LabelStyle=BCT"
			};

		public override PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context,	Object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection props = base.GetProperties(context,value,attributes);
			if(context == null || !(context.Instance is Indicator))
				return props;
			
			PropertyDescriptorCollection outProps = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
			int n = propsd.Length;
			string[] names = new String[n];
			string[] kindFlags = new String[n];
			string[] valueKindFlags = new String[n];
			for(int i=0; i<n; i++)
			{
				int ix = propsd[i].IndexOf("=");
				names[i] = propsd[i].Substring(0,ix);
				string tail = propsd[i].Substring(ix+1);
				kindFlags[i] = tail;
				valueKindFlags[i] = "";
				ix = tail.IndexOf(":");
				if(ix>=0)
				{
					kindFlags[i] = tail.Substring(0,ix);
					valueKindFlags[i] = tail.Substring(ix+1);
				}
			}

			Indicator indicator = context.Instance as Indicator;
			string kindFlag;
			switch(indicator.IndicatorKind)
			{
				case IndicatorKind.Circle: kindFlag = "B"; break;
				case IndicatorKind.Rectangle: kindFlag = "B"; break;
				case IndicatorKind.Text: kindFlag = "T"; break;
				case IndicatorKind.CustomImage: kindFlag = "C"; break;
				case IndicatorKind.Multiple: kindFlag = "M"; break;
				default: kindFlag = ""; break;
			}
			string valueKindFlag;
			switch(indicator.IndicatorValueKind)
			{
				case IndicatorValueKind.Numeric: valueKindFlag = "N"; break;
				case IndicatorValueKind.State: valueKindFlag = "S"; break;
				case IndicatorValueKind.CustomSetColor: valueKindFlag = "C"; break;
				default: valueKindFlag = ""; break;
			}

			for(int i=0; i<props.Count; i++)
			{
				bool rejected = false;
				for(int j=0; j<n; j++)
				{
					if(names[j] == props[i].Name)
					{
						rejected = 
							(kindFlags[j].IndexOf(kindFlag) < 0) || 
							(valueKindFlags[j] != "" &&  valueKindFlags[j].IndexOf(valueKindFlag) < 0);
						break;
					}
				}
				if(!rejected)
					outProps.Add(props[i]);
			}

			return outProps;
		}
	}

	// ============================================================================================

    /// <summary>
    /// Represents a color corresponding to a state.
    /// </summary>
	[TypeConverter(typeof(TypeConverterWithDefaultConstructor))]
	[Serializable]
	public class StateColor : NamedObject
	{
		private Color color = Color.Green;

		public StateColor() { }
		public StateColor(string state, Color color) : base(state)
		{
			this.color = color;
		}
		public Color Color { get { return color; } set { color = value; } }
	}

	// =========================================================================================

    /// <summary>
    /// Contains a collection of <see cref="StateColor"/> objects.
    /// </summary>
	public class StateColorCollection : NamedObjectCollection
	{
		public int Add(StateColor a)
		{
			(a as IObjectModelNode).ParentNode = this;
			return base.Add(a);
		}

		internal override NamedObject CreateNewMember()
		{
			StateColor newMember = new StateColor();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;
		}
		public new StateColor this[object ix]
		{
			get { return base[ix] as StateColor; }
			set { base[ix] = value; }
		}
//
//		public new StateColor this[string name] 
//		{ 
//			get { return base[name] as StateColor; }
//			set { base[name] = value; }
//		}
	}

	// =========================================================================================

    /// <summary>
    /// Contains a collection of <see cref="Indicator"/> objects.
    /// </summary>
    [Serializable]
    public class IndicatorCollection : NamedObjectCollection
	{
//        public int Add(Indicator a)
//		{
//			(a as IObjectModelNode).ParentNode = this;
//			return base.Add(a);
//		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public Indicator AddNewMember(string newMemberName)
		{
			Indicator newMember = new Indicator(newMemberName);
			Add(newMember);
			return newMember;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Indicator"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new Indicator AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as Indicator;
		}

		#endregion

		internal override NamedObject CreateNewMember()
		{
			Indicator newMember = new Indicator();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;
		}

		public new Indicator this[object ix]
		{
			get { return base[ix] as Indicator; }
			set { base[ix] = value; }
		}
//
//		public new Indicator this[int ix]
//		{
//			get { return base[ix] as Indicator; }
//			set { base[ix] = value; }
//		}
//
//        public new Indicator this[string name] 
//		{ 
//			get { return base[name] as Indicator; }
//			set { base[name] = value; }
//		}

    #region --- Client-side serialization ---
#if WEB 

    internal ArrayList ExportJsArray()
    {
      ArrayList indicators = new ArrayList();

      for (int i = 0; i < this.Count; i++)
        indicators.Insert(i, this[i].ExportJsObject());

      return indicators;
    }

    internal void ImportJsArray(IEnumerable indicators)
    {
      foreach (Hashtable indicator in indicators)
        this[(string)indicator["name"]].ImportJsObject(indicator);
    }

#endif
    #endregion
	}
}
