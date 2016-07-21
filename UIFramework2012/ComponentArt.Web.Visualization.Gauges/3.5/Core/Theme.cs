using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies available pre-defined themes.
    /// </summary>
	public enum ThemeKind
	{
		Default,
		BlackIce,
		ArcticWhite,
		Monochrome,
		Custom
	};


    /// <summary>
    /// Represents a visual theme definition for a <see cref="Gauge"/> object.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(TypeConverterWithDefaultConstructor))]
    public class Theme : NamedObject,ISizePositionRangeProvider
    {
        private string paletteName = "Auto"; // "Auto" means: use the theme name
        private string textStyleName = "Auto";
        private SkinCollection skins;
		private Size2D needleShadowOffset = new Size2D(1,-1);
		private Size2D hubShadowOffset = new Size2D(1.5f,-1.5f);
		private Size2D tickMarkShadowOffset = new Size2D(0.2f,-0.2f);

		private GaugePalette defaultPalette = new GaugePalette();

		public Theme() : this(null) { }

        public Theme(string name) : base(name) 
        {
            skins = new SkinCollection(true);
            (skins as IObjectModelNode).ParentNode = this;
        }
		
		//[Browsable(false)]
		public new string Name { get { return base.Name; } set { base.Name = value; } }
		
		[TypeConverter(typeof(GaugePaletteNameConverter))]
		[DefaultValue("Auto")]
		public string PaletteName { get { return paletteName; } set { paletteName = value; } }
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PaletteKind PaletteKind { get { return GaugePalette.NameToKind(paletteName); } set { paletteName = GaugePalette.KindToName(value); } }

		[TypeConverter(typeof(TextStyleNameConverter))]
		[DefaultValue("Auto")]
		public string TextStyleName { get { return textStyleName; } set { textStyleName = value; } }
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyleKind TextStyleKind { get { return TextStyle.NameToKind(textStyleName); } set { textStyleName = TextStyle.KindToName(value); } }

		internal string EName(string name) { return (name=="Auto")? Name:name; }
		internal SubGauge TopGauge { get { return ObjectModelBrowser.GetOwningTopmostGauge(this); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public SkinCollection Skins { get { return skins; } }

		[DefaultValue(typeof(Size2D),"1,-1")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public Size2D NeedleShadowOffset { get { return needleShadowOffset; } set { needleShadowOffset = value; } }

		[DefaultValue(typeof(Size2D),"1.5f,-1.5f")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public Size2D HubShadowOffset { get { return hubShadowOffset; } set { hubShadowOffset = value; } }

		internal GaugePalette Palette() 
		{
			GaugePalette palette = TopGauge.Palettes[EName(paletteName)]; 
			if(palette == null)
				return defaultPalette;
			else
				return palette;
		}

		#region --- ThemeKind Handling ---

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ThemeKind ThemeKind { get { return NameToKind(Name); } }

			internal static ThemeKind NameToKind(string name)
		{
			try
			{
				string name1 = name.Replace(" ","");
				TypeConverter tc = new EnumConverter(typeof(ThemeKind));
				return (ThemeKind)tc.ConvertFromString(name1);
			}
			catch
			{
				return ThemeKind.Custom;
			}
		}
		internal static string KindToName(ThemeKind kind)
		{
			switch(kind)
			{
				case ThemeKind.BlackIce: return "Black Ice"; 
				case ThemeKind.ArcticWhite: return "Arctic White"; 
				default: return kind.ToString();
			}
		}

		#endregion
		
		#region ---  ISizePositionRangeProvider implementation ---
		public virtual void GetRangesAndSteps(string propertyName,
			ref float x0, ref float x1, ref float stepX,
			ref float y0, ref float y1, ref float stepY)
		{
			x0 = -5;
			x1 = 5;
			stepX = 0.1f;
			y0 = -5;
			y1 = 5;
			stepY = 0.1f;
		}
		#endregion

    }


    /// <summary>
    /// Contains a collection of <see cref="Theme"/> objects.
    /// </summary>
    public class ThemeCollection : NamedObjectCollection
    {
        internal ThemeCollection(bool populateInitialContents) : base(populateInitialContents) { }
        public ThemeCollection() : this(false) { }
        
        internal override NamedObject CreateNewMember()
        {
            Theme member = new Theme();
            SelectGenericNewName(member);
            Add(member);
            return member;
        }
		
		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Default". If member named "Default" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public Theme AddNewMember(string newMemberName)
		{
			Theme newMember = AddNewMemberFrom(newMemberName,"Default");
			if(newMember == null)
			{
				newMember = new Theme(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Theme"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new Theme AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as Theme;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Theme"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="themeKind"><see cref="ThemeKind"/> of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public Theme AddNewMemberFrom(string newMemberName, ThemeKind themeKind)
		{
			return base.AddNewMemberFrom(newMemberName,Theme.KindToName(themeKind)) as Theme;
		}


		#endregion

		public int Add(Theme theme)
        {
            (theme as IObjectModelNode).ParentNode = this;
            return base.Add(theme);
        }
        public new Theme this[object ix]
        {
			get 
			{
				if(ix is ThemeKind)
					ix = Theme.KindToName((ThemeKind)ix);
				return base[ix] as Theme; 
			}
            set 
			{
				if(ix is ThemeKind)
					ix = Theme.KindToName((ThemeKind)ix);
				base[ix] = value; 
			}
        }

        internal override void PopulateInitialContents()
        {
        }
    }

    internal class ThemeNameConverter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ThemeCollection themes = null;
            GaugeKind kind = GaugeKind.Circular;

            object instance = null;
            if (context != null)
                instance = context.Instance;
            if (instance != null)
            {
                if (instance is IGaugeControl)
                {
                    themes = (instance as IGaugeControl).Themes;
                    kind = (instance as IGaugeControl).GaugeKind;
                }
                else if (instance is SubGauge)
                {
                    kind = (instance as SubGauge).GaugeKind;
                    themes = (instance as SubGauge).Themes;
                }
            }

            if (themes == null)
            {
                return new StandardValuesCollection(new string[] { });
            }

            ArrayList themeNames = new ArrayList();

            for (int i = 0; i < themes.Count; i++)
                themeNames.Add(themes[i].Name);

            if (!themeNames.Contains("None"))
                themeNames.Add("None");

            // Return the collection
            return new StandardValuesCollection(themeNames);
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

}
