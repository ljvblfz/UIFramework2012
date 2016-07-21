using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies available pre-defined palettes.
    /// </summary>
	public enum PaletteKind
	{
		Default,
		BlackIce,
		ArcticWhite,
		MonochromeGreen,
		MonochromeAmber,
		MonochromeWhite,
		MonochromeBlue,
		Custom
	}

	/// <summary>
	/// Represents a complete color palette definition.
	/// </summary>
	[TypeConverter(typeof(NamedObjectConverter))]
	[Serializable]
	public class GaugePalette : NamedObject
	{
		private Color backgroundBaseColor = Color.FromArgb(128,128,128);
        private Color frameBaseColor = Color.FromArgb(128, 128, 128);
        private Color hubBaseColor = Color.FromArgb(128, 128, 128);
        private MultiColor pointerBaseColor = Color.FromArgb(192, 64, 64);
		private MultiColor rangeBaseColor = Color.FromArgb(192,255,255,255);
		private MultiColor annotationBaseColor = Color.FromArgb(192,255,255,255);
		private MultiColor majorTickmarkBaseColor = Color.FromArgb(192,255,255,255);
		private MultiColor minorTickmarkBaseColor = Color.FromArgb(192,255,255,255);
		private MultiColor numericColor = Color.FromArgb(192,0,0,0);
		private MultiColor numericDecimalColor = Color.FromArgb(192,128,0,0);
		private MultiColor numericBackColor = Color.FromArgb(192,255,255,255);
		private MultiColor numericDecimalBackColor = Color.FromArgb(192,255,255,255);
		
		public GaugePalette() : this("") { }
		public GaugePalette(string name) : base(name) 
		{
			(pointerBaseColor as IObjectModelNode).ParentNode = this;
			(rangeBaseColor as IObjectModelNode).ParentNode = this;
			(annotationBaseColor as IObjectModelNode).ParentNode = this;
			(majorTickmarkBaseColor as IObjectModelNode).ParentNode = this;
			(pointerBaseColor as IObjectModelNode).ParentNode = this;
			(minorTickmarkBaseColor as IObjectModelNode).ParentNode = this;
			(numericColor as IObjectModelNode).ParentNode = this;
			(numericDecimalColor as IObjectModelNode).ParentNode = this;
			(numericBackColor as IObjectModelNode).ParentNode = this;
			(numericDecimalBackColor as IObjectModelNode).ParentNode = this;
		}

		public Color BackgroundBaseColor { get { return backgroundBaseColor; } set { backgroundBaseColor = value; } }

        public Color FrameBaseColor { get { return frameBaseColor; } set { frameBaseColor = value; } }

        public Color HubBaseColor { get { return hubBaseColor; } set { hubBaseColor = value; } }

#if WEB
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public MultiColor PointerBaseColor { get { return pointerBaseColor; } set { pointerBaseColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor RangeBaseColor { get { return rangeBaseColor; } set { rangeBaseColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor AnnotationBaseColor { get { return annotationBaseColor; } set { annotationBaseColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor MajorTickmarkBaseColor { get { return majorTickmarkBaseColor; } set { majorTickmarkBaseColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor MinorTickmarkBaseColor { get { return minorTickmarkBaseColor; } set { minorTickmarkBaseColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor NumericColor { get { return numericColor; } set { numericColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if DEBUG		
		// NB: for bacward compatibility with XML templates; not needed in release build
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MultiColor NumericBaseColor { get { return numericColor; } set { numericColor = value; (value as IObjectModelNode).ParentNode = this; } }
		// NB: for bacward compatibility with XML templates; not needed in release build
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MultiColor NumericDecimalBaseColor { get { return numericDecimalColor; } set { numericDecimalColor = value; (value as IObjectModelNode).ParentNode = this; } }
#endif
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MultiColor NumericDecimalColor { get { return numericDecimalColor; } set { numericDecimalColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
	public MultiColor NumericBackColor { get { return numericBackColor; } set { numericBackColor = value; (value as IObjectModelNode).ParentNode = this; } }

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
	public MultiColor NumericDecimalBackColor { get { return numericDecimalBackColor; } set { numericDecimalBackColor = value; (value as IObjectModelNode).ParentNode = this; } }
    
		#region --- PaletteKind Handling ---

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PaletteKind PaletteKind { get { return NameToKind(Name); } }

		internal static PaletteKind NameToKind(string name)
		{
			try
			{
				string name1 = name.Replace(" ","");
				TypeConverter tc = new EnumConverter(typeof(PaletteKind));
				return (PaletteKind)tc.ConvertFromString(name1);
			}
			catch
			{
				return PaletteKind.Custom;
			}
		}
		internal static string KindToName(PaletteKind kind)
		{
			switch(kind)
			{
				case PaletteKind.ArcticWhite: return "Arctic White"; 
				case PaletteKind.BlackIce: return "Black Ice"; 
				case PaletteKind.MonochromeAmber: return "Monochrome Amber"; 
				case PaletteKind.MonochromeBlue: return "Monochrome Blue"; 
				case PaletteKind.MonochromeGreen: return "Monochrome Green"; 
				case PaletteKind.MonochromeWhite: return "Monochrome White"; 
				default: return kind.ToString();
			}
		}

		#endregion

	}

    /// <summary>
    /// Contains a collection of <see cref="GaugePalette"/> objects.
    /// </summary>
	[Serializable]
	public class GaugePaletteCollection : NamedObjectCollection
	{
		internal GaugePaletteCollection(bool populateInitialContents) : base(populateInitialContents) { }

        public GaugePaletteCollection() : this(false) { }

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Default". If member named "Default" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public GaugePalette AddNewMember(string newMemberName)
		{
			GaugePalette newMember = AddNewMemberFrom(newMemberName,"Default");
			if(newMember == null)
			{
				newMember = new GaugePalette(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="GaugePalette"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new GaugePalette AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as GaugePalette;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="GaugePalette"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="paletteKind"><see cref="PaletteKind"/> of the original collection member.</param>
		/// <param name="paletteKind"></param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public GaugePalette AddNewMemberFrom(string newMemberName, PaletteKind paletteKind)
		{
			return base.AddNewMemberFrom(newMemberName,GaugePalette.KindToName(paletteKind)) as GaugePalette;
		}

		#endregion

		internal override NamedObject CreateNewMember()
		{
			GaugePalette gp = new GaugePalette();
			SelectGenericNewName(gp);
			Add(gp);
			return gp;
		}

		public int Add(GaugePalette palette)
		{
			(palette as IObjectModelNode).ParentNode = this;
			return base.Add(palette);
		}

		public new GaugePalette this[object ix]
		{
			get 
			{
				if(ix is PaletteKind)
					ix = GaugePalette.KindToName((PaletteKind)ix);
				return base[ix] as GaugePalette; 
			}
			set 
			{
				if(ix is PaletteKind)
					ix = GaugePalette.KindToName((PaletteKind)ix);
				base[ix] = value; 
			}
		}

        internal override void PopulateInitialContents()
		{
			GaugePalette palette = new GaugePalette("Default");
			Add(palette);
		}
	}

	internal class GaugePaletteNameConverter : SelectedNameConverter 
	{
		public GaugePaletteNameConverter() { }

		protected override NamedObjectCollection GetNamedCollection(SubGauge gauge)
		{
			this.addAuto = true;
			this.addNone = true;
			return gauge.Palettes;
		}
	}
}
