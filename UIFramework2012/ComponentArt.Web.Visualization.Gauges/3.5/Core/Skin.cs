using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Represents a visual skin definition for a <see cref="Theme"/> object.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TypeConverterWithDefaultConstructor))]
	public class Skin : NamedObject 
	{
		private string backgroundName = "Auto";// "Auto" means: use the theme name
		private string frameName = "Auto";
		private string coverName = "Auto";
		private string pointerStyleName = "Default";
		private string digitOverlayName = "Default";

		private string scaleAnnotationStyleName = "Auto";
		private double scalePosition = double.NaN;

		private GaugePalette defaultPalette = new GaugePalette();

		private ScaleLayout scaleLayout = new ScaleLayout();
		private RangeLayout rangeLayout = new RangeLayout();

		// Numeric gauge properties
		private string numericTextStyleName = "Auto";
		private string numericFormattingString = "G";
		private int numberOfPositions = 0;
		private bool displayDecimalPoint = true;
		private Rectangle2D numericGaugeRectangle = new Rectangle2D(10,10,80,80);
		private NumericDisplayKind numericDisplayKind = NumericDisplayKind.Simple;

		public Skin() : base()
		{
			(scaleLayout as IObjectModelNode).ParentNode = this;
			(rangeLayout as IObjectModelNode).ParentNode = this;
		}

		public Skin(string name) : base(name) 
		{
			(scaleLayout as IObjectModelNode).ParentNode = this;
			(rangeLayout as IObjectModelNode).ParentNode = this;
		}

		#region --- Public Properties ---

		#region --- Properties Relevant to All Gauge Kinds ---

		[Browsable(false)]
		public new string Name { get { return base.Name; } set { base.Name = value; } }
		
		[TypeConverter(typeof(BackgroundLayerNameConverter))]
		[DefaultValue("Auto")]
		public string BackgroundName { get { return backgroundName; } set { backgroundName = value; } }
		
		[TypeConverter(typeof(FrameLayerNameConverter))]
		[DefaultValue("Auto")]
		public string FrameName { get { return frameName; } set { frameName = value; } }
		
		[TypeConverter(typeof(CoverLayerNameConverter))]
		[DefaultValue("Auto")]
		public string CoverName { get { return coverName; } set { coverName = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GaugeKind Kind 
		{
			get 
			{
				try
				{
					EnumConverter eConv = new EnumConverter(typeof(GaugeKind));
					return (GaugeKind)eConv.ConvertFromString(Name);
				}
				catch
				{
					throw new Exception("Invalid skin name '" + Name + "'.");
				}
			}
		}
		#endregion

		#region --- Properties Relevant to Gauges with Scale ---

		[TypeConverter(typeof(PointerStyleNameConverter))]
		[DefaultValue("Default")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public string PointerStyleName { get { return pointerStyleName; } set { pointerStyleName = value; } }
			
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PointerStyleKind PointerStyleKind { get { return PointerStyle.NameToKind(pointerStyleName); } set { pointerStyleName = PointerStyle.KindToName(value); } }

		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Scale annotation style name")]
		[TypeConverter(typeof(ScaleAnnotationStyleNameConverter))]
		[DefaultValue("Auto")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public string ScaleAnnotationStyleName { get { return scaleAnnotationStyleName; } set { scaleAnnotationStyleName = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]	
		public ScaleAnnotationStyleKind ScaleAnnotationStyleKind 
		{
			get { return ScaleAnnotationStyle.NameToKind(scaleAnnotationStyleName); } 
			set { scaleAnnotationStyleName = ScaleAnnotationStyle.KindToName(value); ObjectModelBrowser.NotifyChanged(this); } 
		}

		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[RelevantFor(GaugeKindGroup.WithScale)]
		[Description("Main scale layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public ScaleLayout ScaleLayout { get { return scaleLayout; } set { scaleLayout = value; (scaleLayout as IObjectModelNode).ParentNode = this; } }

		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Main range layout")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public RangeLayout RangeLayout { get { return rangeLayout; } set { rangeLayout = value; (rangeLayout as IObjectModelNode).ParentNode = this; } }
		
		#endregion

		#region --- Properties Relevant to Numeric Gauges ---
		
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Text style style name for numeric gauge")]
		[TypeConverter(typeof(TextStyleNameConverter))]
		[DefaultValue("Auto")]
		[RelevantFor(GaugeKindGroup.Numeric)]
		public string NumericTextStyleName { get { return numericTextStyleName; } set { numericTextStyleName = value; } }
#if DEBUG // NB: this is for backward compatibility with working templates
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public
#else
		internal
#endif
		string DigitOverlayName { get { return digitOverlayName; } set { digitOverlayName = value; } }
			
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Digit overlay layer name name for numeric gauge")]
		[DefaultValue(NumericDisplayKind.Simple)]
		[RelevantFor(GaugeKindGroup.Numeric)]
		public NumericDisplayKind NumericDisplayKind { get { return numericDisplayKind; } set { numericDisplayKind = value; } }
			
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Numeric formatting string")]
		[DefaultValue("G")]
		[RelevantFor(GaugeKindGroup.Numeric)]
		public string NumericFormattingString { get { return numericFormattingString; } set { numericFormattingString = value; } }
			
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Numeric gauge display rectangle in percentages of the gauge dimensions")]
		[DefaultValue(typeof(Rectangle2D), "10,10,80,80")]
		[RelevantFor(GaugeKindGroup.Numeric)]
		public Rectangle2D NumericGaugeRectangle { get { return numericGaugeRectangle; } set { numericGaugeRectangle = value; } }
			
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Display decimal point in numeric gauge")]
		[DefaultValue(true)]
		[RelevantFor(GaugeKindGroup.Numeric)]
		public bool DisplayDecimalPoint { get { return displayDecimalPoint; } set { displayDecimalPoint = value; } }
			
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Description("Number of positions in numeric gauge")]
		[DefaultValue(0)]
		[RelevantFor(GaugeKindGroup.Numeric)]
		public int NumberOfPositions { get { return numberOfPositions; } set { numberOfPositions = value; } }

		#endregion

		#endregion

		internal Theme Theme { get { return ObjectModelBrowser.GetAncestorByType(this,typeof(Theme)) as Theme ; }	}
		internal SubGauge TopGauge { get { return ObjectModelBrowser.GetOwningTopmostGauge(this) ; }	}

		internal TextStyle NumericTextStyle
		{
			get
			{
				string sName = this.numericTextStyleName;
				if(sName == "Auto")
					sName = Theme.Name;
				TextStyle style = TopGauge.TextStyles[sName];
				if(style == null)
					style = TopGauge.TextStyles["Default"];
				return style;
			}
		}

		public override string ToString()
		{
			Theme theme = ObjectModelBrowser.GetAncestorByType(this,typeof(Theme)) as Theme;
			if (theme != null)
				return GetType().Name + " '" + theme.Name + " " + Name + "'";
			else
				return base.ToString();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public ScaleAnnotationStyle ScaleAnnotationStyle 
		{ 
			get 
			{ 
				ScaleAnnotationStyle ssa = TopGauge.ScaleAnnotationStyles[EName(scaleAnnotationStyleName)];
				if (ssa == null)
					return TopGauge.ScaleAnnotationStyles["Default"];
				else
					return ssa;
			}
		}
		internal double EffectiveScalePosition(SubGauge gauge)
		{
			if (double.IsNaN(scalePosition))
				return GaugeGeometry.ScalePosition(gauge.GaugeKind);
			else
				return scalePosition;
		}

		internal string EName(string name) { return (name=="Auto")? Name:name; }

		internal GaugePalette Palette() 
		{
			Theme theme = ObjectModelBrowser.GetAncestorByType(this,typeof(Theme)) as Theme;
			return theme.Palette();
		}

		internal Layer Background() 
		{
			SubGauge topGauge = TopGauge;
			if(topGauge == null)
				return null;
			else
				return topGauge.BackgroundLayers[EName(backgroundName),Kind]; 
		} 
		internal Layer Frame() { return TopGauge.FrameLayers[EName(frameName),Kind]; } 
		internal Layer Cover() { return TopGauge.CoverLayers[EName(coverName),Kind]; } 
		internal Layer DigitOverlay() { return TopGauge.DigitMaskLayers[digitOverlayName,GaugeKind.Numeric]; } 

		internal Color BackgroundBaseColor { get { return this.Palette().BackgroundBaseColor; } }
		internal Color FrameBaseColor { get { return this.Palette().FrameBaseColor; } } 

	}

	// =================================================================================================================

    /// <summary>
    /// Contains a collection of <see cref="Skin"/> objects.
    /// </summary>
	[Serializable]
	public class SkinCollection : NamedObjectCollection
	{
		public SkinCollection() : this(false)  { }
        
		internal SkinCollection(bool populateInitialContents) : base(populateInitialContents) { }

		internal override NamedObject CreateNewMember()
		{
			Skin skin = new Skin();
			SelectGenericNewName(skin);
			Add(skin);
			return skin;
		}
		public int Add(Skin skin)
		{
			(skin as IObjectModelNode).ParentNode = this;
			return base.Add(skin);
		}

		public new Skin this[object ix]
		{
			get 
			{ 
				if (ix is GaugeKind)
					return base[((GaugeKind)ix).ToString()] as Skin;
				return base[ix] as Skin; 
			}
			set 
			{ 
				if (ix is GaugeKind)
					base[((GaugeKind)ix).ToString()] = value;
				base[ix] = value; 
			}
		}

//		public new Skin this[string name]
//		{
//			get { return base[name] as Skin; }
//			set { base[name] = value; }
//		}

//		public Skin this[GaugeKind kind]
//		{
//			get { return base[kind.ToString()] as Skin; }
//			set { base[kind.ToString()] = value; }
//		}

		internal override void PopulateInitialContents()
		{
			EnumConverter kindConverter = new EnumConverter(typeof(GaugeKind));
			TypeConverter.StandardValuesCollection stdValues = kindConverter.GetStandardValues() as TypeConverter.StandardValuesCollection;
			for (int i = 0; i < stdValues.Count; i++)
			{
				string skinName = stdValues[i].ToString();
				Add(new Skin(skinName));
			}
		}

	}


	#region --- Type Converters ---
	
	// Base class for layer names depending on current GaugeKind

	internal class LayerNameConverter : StringConverter
	{
		LayerCollection layers = null;
		protected bool checkGaugeKind = true;

		public virtual LayerCollection GetLayerCollection(SubGauge gauge)
		{
			return null;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			SubGauge gauge = null;
			if(context == null || context.Instance == null)
				return new StandardValuesCollection(new string[] { });

			object instance = context.Instance;
			if (instance is IGaugeControl)
			{
				ThemeCollection themes = (instance as IGaugeControl).Themes;
				gauge = ObjectModelBrowser.GetOwningTopmostGauge(themes);
			}
			else if (instance is IObjectModelNode)
			{
				gauge = ObjectModelBrowser.GetOwningTopmostGauge(instance as IObjectModelNode);
			}
            
			if(gauge == null)
			{
				return new StandardValuesCollection(new string[] { });
			}

			layers = GetLayerCollection(gauge);
            
			if(layers == null)
			{
				return new StandardValuesCollection(new string[] { });
			}

			ArrayList names = new ArrayList();

			for(int i=0; i<layers.Count; i++)
			{
				if(layers[i].Kind == gauge.GaugeKind || !checkGaugeKind)
					names.Add(layers[i].Name);
			}

			if (!names.Contains("None"))
				names.Add("None");
			if (!names.Contains("Auto"))
				names.Add("Auto");

			// Return the collection
			return new StandardValuesCollection(names);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}	
	}


	internal class DigitLayerNameConverter : LayerNameConverter
	{
		public override LayerCollection GetLayerCollection(SubGauge gauge)
		{
			checkGaugeKind = false;
			return gauge.DigitMaskLayers;
		}
	}
	internal class BackgroundLayerNameConverter : LayerNameConverter
	{
		public override LayerCollection GetLayerCollection(SubGauge gauge)
		{
			return gauge.BackgroundLayers;
		}
	}

	internal class FrameLayerNameConverter : LayerNameConverter
	{
		public override LayerCollection GetLayerCollection(SubGauge gauge)
		{
			return gauge.FrameLayers;
		}
	}

	internal class CoverLayerNameConverter : LayerNameConverter
	{
		public override LayerCollection GetLayerCollection(SubGauge gauge)
		{
			return gauge.CoverLayers;
		}
	}

	internal class HubLayerNameConverter : LayerNameConverter
	{
		public override LayerCollection GetLayerCollection(SubGauge gauge)
		{
			return gauge.HubLayers;
		}
	}

	internal class MarkerLayerNameConverter : LayerNameConverter
	{
		public override LayerCollection GetLayerCollection(SubGauge gauge)
		{
			checkGaugeKind = false;
			return gauge.MarkerLayers;
		}
	}

	// =============================================================================================================================

	internal class ExpandableConverterWithPropertyRelevance : ExpandableObjectConverter
	{
		public override bool GetPropertiesSupported (ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context,	Object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection props = base.GetProperties(context,value,attributes);
			if(context == null || context.Instance == null)
				return props;
			SubGauge gauge = null;

			object instance = context.Instance;
			PropertyDescriptor pDesInst = TypeDescriptor.GetProperties(instance,true)["SubGauge"];
			if(pDesInst != null)
				instance = pDesInst.GetValue(instance);

			if(instance is SubGauge)
				gauge = instance as SubGauge;
			else if(instance is IObjectModelNode)
				gauge = ObjectModelBrowser.GetOwningGauge(instance as IObjectModelNode);
			else if(instance is IGaugeControl)
			{
				IGaugeControl gControl = instance as IGaugeControl;
				gauge = ObjectModelBrowser.GetOwningTopmostGauge(gControl.Palettes);
			}
			else
				return props;

			 
			PropertyDescriptorCollection outProps = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
			foreach (PropertyDescriptor pDes in props)
			{
				RelevantForAttribute relevantFor = pDes.Attributes[typeof(RelevantForAttribute)] as RelevantForAttribute;
				if(relevantFor != null && !relevantFor.IsRelevantFor(gauge.GaugeKind))
					continue;
				else
					outProps.Add(pDes);
			}
			return outProps;
		}
	}

	#endregion
}
