using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Xml;



namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="Light"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class LightCollection : CollectionWithType 
	{
		private bool hasChanged = true;

		internal LightCollection() : base(typeof(Light)) 
		{ }

		/// <summary>
		/// Indicates the <see cref="Light"/> at the specified indexed location in the <see cref="LightCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="Light"/> from the <see cref="LightCollection"/> object.</param>
		public new Light this[object index]   
		{ 
			get { return ((Light)base[index]); } 
			set { base[index] = value; } 
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the <see cref="LightCollection" /> instance.
		/// </summary>
		/// <param name="value">Object to be added to <see cref="LightCollection"/>.</param>
		/// <returns>The index of the newly added object or -1 if the the object could not be added.</returns>
		public override int Add(object obj)
		{
			hasChanged = true;
			return base.Add(obj);
		}
		
		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="LightCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="object"/> to remove from the <see cref="LightCollection"/>.</param>
		public new void Remove(object obj)
		{
			hasChanged = true;
			base.Remove(obj);
		}
		
		internal bool HasChanged 
		{
			get 
			{ 
				if(hasChanged)
					return true;
				foreach(Light L in this)
					if(L.HasChanged)
						return true;
				return false;
			}
			set { hasChanged = value; } 
		}
	}


	/// <summary>
	/// A collection of <see cref="LabelStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class LabelStyleCollection : NamedCollection
	{
		internal LabelStyleCollection(Object owner, bool initialize) : base(typeof(LabelStyle), owner)
		{ 
			if(initialize)
				InitializeContents();
		}
		internal LabelStyleCollection(Object owner) : this(owner,false)
		{ }

		internal LabelStyleCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="LabelStyle"/> at the specified indexed location in the <see cref="LabelStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or <see cref="LabelStyleKind"/> or name to retrieve a <see cref="LabelStyle"/> from the <see cref="LabelStyleCollection"/> object.</param>
		public new LabelStyle this[object index]   
		{ 
			get 
			{
				if(index is LabelStyleKind)
					index = LabelStyle.NameOf((LabelStyleKind)index);
				return ((LabelStyle)base[index]); 
			} 
			set 
			{
				if(index is LabelStyleKind)
					index = LabelStyle.NameOf((LabelStyleKind)index);
				base[index] = value; 
			} 
		}

		private void InitializeContents()
		{
			LabelStyle ls = new LabelStyle("Default", TextOrientation.Default, TextReferencePoint.Default, 0, 0, 0.0, 0.5).MarkCreatedInternally();
			ls.SetOwningChart(Owner as ChartBase);
			Add(ls);

			ls = new LabelStyle("DefaultAxisLabels", TextOrientation.Default, TextReferencePoint.Default, 0, 0, 0.0, 0.5).MarkCreatedInternally();
			ls.SetOwningChart(Owner as ChartBase);
			ls.ForeColor = Color.FromArgb(0, 0, 0, 0);
			ls.HasChanged = false;
			Add(ls);
		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates a new default <see cref="LabelStyle"/> object and adds it to this <see cref="LabelStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the style.</param>
		/// <returns>Newly created <see cref="LabelStyle"/>.</returns>
		public LabelStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

		/// <summary>
		/// Copies a specified <see cref="LabelStyle"/> and adds it to this <see cref="LabelStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberName">The name of the style to copy.</param>
		/// <returns>Newly created <see cref="LabelStyle"/>.</returns>
		public LabelStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			LabelStyle style = new LabelStyle(newMemberName);
			LabelStyle srcStyle = this[oldMemberName];
			if(srcStyle != null)
			{
				style.LoadFrom(srcStyle);
			}
			Add(style);
			return style;
		}

		
		/// <summary>
		/// Copies a specified <see cref="LabelStyle"/> and adds it to this <see cref="LabelStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberKind">The kind of the style to copy.</param>
		/// <returns>Newly created <see cref="LabelStyle"/>.</returns>
		public LabelStyle CreateFrom(string newMemberName, LabelStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,LabelStyle.NameOf(oldMemberKind));
		}
#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ==============  ");
			S.Comment("    Label Styles  ");
			S.Comment("    ==============  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("LabelStyle"))
				{
					LabelStyle P = new LabelStyle();
					P.Serialize(S);
					Add(P);
					while(S.GoToNext("LabelStyle"))
					{
						P = new LabelStyle();
						P.Serialize(S);
						Add(P);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(LabelStyle P in this)
				{
					if(S.BeginTag("LabelStyle"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}

		internal void CreateDOM(XmlElement parent)
		{
			XmlDocument doc = parent.OwnerDocument;
			XmlElement root = doc.CreateElement("LabelStyles");

			foreach(LabelStyle S in this)
				root.AppendChild(S.CreateDOM(doc));
			parent.AppendChild(root);
		}

		internal static LabelStyleCollection CreateFromDOM(XmlElement root)
		{
			if(root.Name.ToLower() != "labelstyles")
				return null;
			LabelStyleCollection C = new LabelStyleCollection(null);

			foreach (XmlElement e in root.ChildNodes)
				C.Add(LabelStyle.CreateFromDOM(e));

			return C;
		}
		#endregion
	}

	/// <summary>
	/// A collection of <see cref="Palette"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class PaletteCollection : NamedCollection
	{
		internal PaletteCollection() : base(typeof(Palette))
		{
            //string sFileName = "Resources.Palettes.xml";
            string sFileName = "Palettes.xml";

			Stream inputStream = CommonFunctions.GetManifestResourceStream(sFileName);
			XmlCustomSerializer.Read(inputStream, this);
			inputStream.Close();
			foreach(Palette p in this)
				p.SetChanged(false);
		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates a new default <see cref="Palette"/> and adds it to this <see cref="PaletteCollection"/> object.
		/// </summary>
		/// <param name="newPaletteName">The name of the palette.</param>
		/// <returns>Newly created palette.</returns>
		public Palette CreateNew(string newPaletteName)
		{
			return CreateFrom(newPaletteName,"Default");
		}

		/// <summary>
		/// Copies a specified <see cref="Palette"/> and adds it to this <see cref="PaletteCollection"/> object.
		/// </summary>
		/// <param name="newPaletteName">Name of the new palette.</param>
		/// <param name="oldPaletteName">Name of the palette to copy.</param>
		/// <returns></returns>
		public Palette CreateFrom(string newPaletteName, string oldPaletteName)
		{
			Palette p;
			Palette srcP = this[oldPaletteName];
			if(srcP != null)
			{
				p = (Palette)srcP.Clone();
				p.Name = newPaletteName;
			}
			else
				p = new Palette(newPaletteName);
			p.SetChanged(true);
			Add(p);
			return p;
		}

		/// <summary>
		/// Copies a specified <see cref="Palette"/> and adds it to this <see cref="PaletteCollection"/> object.
		/// </summary>
		/// <param name="newPaletteName">Name of the new palette.</param>
		/// <param name="oldPaletteKind"><see cref="PaletteKind"/> of the palette to copy.</param>
		/// <returns></returns>
		public Palette CreateFrom(string newPaletteName, PaletteKind oldPaletteKind)
		{
			return CreateFrom(newPaletteName,Palette.NameOf(oldPaletteKind));
		}

		#endregion
		

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the <see cref="PaletteCollection" /> instance.
		/// </summary>
		/// <param name="value">Object to be added to <see cref="PaletteCollection"/>.</param>
		/// <returns>The index of the newly added object or -1 if the the object could not be added.</returns>
		public override int Add( object value )  
		{
			Palette pal = value as Palette;
			if (pal == null)
				return -1;
			pal.OwningCollection = this;
			for(int i=0;i<List.Count;i++)
			{
				Palette cp = (Palette)List[i];
				if(cp.Name == pal.Name)
				{
					List[i] = pal;
					return i;
				}
			}
			return( List.Add( pal ) );
		}

		/// <summary>
		/// Indicates the <see cref="Palette"/> at the specified indexed location in the <see cref="PaletteCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name or <see cref="PaletteKind"/> to retrieve a <see cref="Palette"/> from the <see cref="PaletteCollection"/> object.</param>
		public new Palette this[object index]   
		{ 
			get 
			{
				if(index is PaletteKind)
					index = Palette.NameOf((PaletteKind)index);
				return ((Palette)base[index]); 
			} 
			set 
			{
				if(index is PaletteKind)
					index = Palette.NameOf((PaletteKind)index);
				base[index] = value; 
			} 
		}

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			Serialize(S,null);
		}

		internal void Serialize(XmlCustomSerializer S, ChartBase chart)
		{
			if(chart != null)
				S.AttributeProperty(chart.Palette,"Active","Name");
			S.Comment(" <<<<<<<<<<<<<<<<<<<<<<<<< Color Palettes >>>>>>>>>>>>>>>>>>>>>>>>> ");
			if(S.Reading)
			{
				if(S.GoToFirstChild("Palette"))
				{
					Palette P = new Palette();
					P.Serialize(S);
					Add(P);
					while(S.GoToNext("Palette"))
					{
						P = new Palette();
						P.Serialize(S);
						Palette oldPalette = this[P.Name];
						bool removable = true;
						if(oldPalette != null)
						{
							removable = oldPalette.Removable;
							oldPalette.Removable = true;
							this.Remove(P.Name);
						}
						P.Removable = removable;
						Add(P);
					}
					S.GoToParent();
				}
				else
				{
					throw new Exception("Input file not in pallete collection format. \nOperation aborted.");
				}

			}
			else
			{
				foreach(Palette P in this)
				{
					if(S.BeginTag("Palette"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}

		#endregion
	}


	/// <summary>
	/// A collection of <see cref="DataDimension"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class DataDimensionCollection : NamedCollection
	{
		internal DataDimensionCollection(object owner) : base(typeof(DataDimension),owner, 
			new Type[] { typeof(NumericDataDimension), typeof(DateTimeDataDimension),
						 typeof(IndexDataDimension),typeof(EnumeratedDataDimension) })
		{ }
		internal DataDimensionCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="DataDimension"/> at the specified indexed location in the <see cref="DataDimensionCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="DataDimension"/> from the <see cref="DataDimensionCollection"/> object.</param>
		public new DataDimension this[object index]   
		{ 
			get { return ((DataDimension)base[index]); } 
			set { base[index] = value; } 
		}

		#region --- XML Serialization ---

		internal XmlElement CreateDOM(XmlDocument doc, string activePalette)
		{
			XmlElement dimensions = doc.CreateElement("Dimensions");
			return dimensions;
		}
		
		internal static DataDimensionCollection CreateFromDOM(XmlElement root)
		{
			DataDimensionCollection dimensions = new DataDimensionCollection(null);
			return dimensions;
		}
		#endregion
	}


	// =====================================================================================================
	//		LineStyles2D Collection
	// =====================================================================================================
	
	/// <summary>
	/// A collection of <see cref="LineStyle2D"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class LineStyle2DCollection : NamedCollection
	{
		internal LineStyle2DCollection(Object owner, bool initialize) 
			: base(typeof(LineStyle2D), owner)
		{ 
			if(initialize)
				InitializeContents();
		}
		internal LineStyle2DCollection(Object owner) : this(owner,false)
		{ }

		internal LineStyle2DCollection() : this(null) { InitializeContents(); }

		/// <summary>
		/// Indicates the <see cref="LineStyle2D"/> at the specified indexed location in the <see cref="LineStyle2DCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="LineStyle2D"/> from the <see cref="LineStyle2DCollection"/> object.</param>
		public new LineStyle2D this[object index]
		{ 
			get 
			{
				if(index is LineStyle2DKind)
					index = LineStyle2D.NameOf((LineStyle2DKind)index);
				return ((LineStyle2D)base[index]); 
			} 
			set 
			{
				if(index is LineStyle2DKind)
					index = LineStyle2D.NameOf((LineStyle2DKind)index);
				base[index] = value; 
			} 
		}

		internal void InitializeContents()
		{//
			LineStyle2D LS;
			
			if(this["DefaultForLine2DSeriesStyle"] == null)
			{
				LS = new LineStyle2D("DefaultForLine2DSeriesStyle",2.0f,Color.Transparent);
				Add(LS);
				LS.HasChanged = false;
			}
			
			if(this["Default"] == null)
			{
				LS = new LineStyle2D("Default",1.0f,Color.Black);
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["TwoDObjectBorder"] == null)
			{
				LS = new LineStyle2D("TwoDObjectBorder",1.0f,Color.FromArgb(0,0,0,0));
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["CoordinateLine"] == null)
			{
				LS = new LineStyle2D("CoordinateLine",1.0f,Color.FromArgb(0,0,0,0),DashStyle.Solid);
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["AxisLine"] == null)
			{
				LS = new LineStyle2D("AxisLine",1.0f,Color.FromArgb(0,0,0,0),DashStyle.Solid);
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["Solid"] == null)
			{
				LS = new LineStyle2D("Solid",1.0f,Color.Black,DashStyle.Solid);
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["Dash"] == null)
			{
				LS = new LineStyle2D("Dash",1.0f,Color.Black,DashStyle.Dash);
				Add(LS);
				LS.HasChanged = false;
			}
			
			if(this["Dot"] == null)
			{
				LS = new LineStyle2D("Dot",1.0f,Color.Black,DashStyle.Dot);
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["DashDot"] == null)
			{
				LS = new LineStyle2D("DashDot",1.0f,Color.Black,DashStyle.DashDot);
				Add(LS);
				LS.HasChanged = false;
			}

			if(this["DashDotDot"] == null)
			{
				LS = new LineStyle2D("DashDotDot",1.0f,Color.Black,DashStyle.DashDotDot);
				Add(LS);
				LS.HasChanged = false;
			}
		}
				
		/// <summary>
		/// Clones and stores the specified <see cref="LineStyle2D"/>.
		/// </summary>
		/// <param name="originalStyleName">Name of the original style.</param>
		/// <param name="clonedStyleName">Name of the cloned style.</param>
		/// <returns>Returns the cloned style.</returns>
		/// <remarks>If the original style does not exist, the function returns null. 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public LineStyle2D Clone(string originalStyleName, string clonedStyleName)
		{
			LineStyle2D original = this[originalStyleName];
			if(original == null)
				return null;
			StyleCloner cloner = new StyleCloner();
			LineStyle2D clonedStyle = cloner.Clone(original) as LineStyle2D;
			clonedStyle.Name = clonedStyleName;
			Add(clonedStyle);

			return clonedStyle;
		}
		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ==============  ");
			S.Comment("    2D Line Styles  ");
			S.Comment("    ==============  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("LineStyle2D"))
				{
					LineStyle2D P = new LineStyle2D();
					P.Serialize(S);
					Add(P);
					while(S.GoToNext("LineStyle2D"))
					{
						P = new LineStyle2D();
						P.Serialize(S);
						Add(P);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(LineStyle2D P in this)
				{
					if(S.BeginTag("LineStyle2D"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}

		#endregion
	}

	// =====================================================================================================
	//		GradientStyle Collection
	// =====================================================================================================
	
	/// <summary>
	/// A collection of <see cref="GradientStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class GradientStyleCollection : NamedCollection
	{
		internal GradientStyleCollection(Object owner,bool initialize) 
			: base(typeof(GradientStyle), owner)
		{ 
			if(initialize)
				InitializeContents();
		}
		internal GradientStyleCollection(Object owner) : this(owner,true) { }

		internal GradientStyleCollection() : this(null) {}

		/// <summary>
		/// Indicates the <see cref="GradientStyle"/> at the specified indexed location in the <see cref="GradientStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or <see cref="GradientStyleKind"/> or name to retrieve a <see cref="GradientStyle"/> from the <see cref="GradientStyleCollection"/> object.</param>
		public new GradientStyle this[object index]   
		{ 
			get 
			{
				if(index is GradientStyleKind)
					index = GradientStyle.NameOf((GradientStyleKind)index);
				return ((GradientStyle)base[index]); 
			} 
			set 
			{
				if(index is GradientStyleKind)
					index = GradientStyle.NameOf((GradientStyleKind)index);
				base[index] = value; 
			} 
		}

		private void InitializeContents()
		{
			AddNonChanged(new GradientStyle("None",					GradientKind.None,					Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("Horizontal",			GradientKind.Horizontal,			Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("Vertical",				GradientKind.Vertical,				Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("DiagonalRight",		GradientKind.DiagonalRight,			Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("DiagonalLeft",			GradientKind.DiagonalLeft,			Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("Center",				GradientKind.Center,				Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("HorizontalCenter",		GradientKind.HorizontalCenter,		Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("VerticalCenter",		GradientKind.VerticalCenter,		Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("DiagonalRightCenter",	GradientKind.DiagonalRightCenter,	Color.Transparent,Color.Transparent));
			AddNonChanged(new GradientStyle("DiagonalLeftCenter",	GradientKind.DiagonalLeftCenter,	Color.Transparent,Color.Transparent));
		}

		private void AddNonChanged(GradientStyle style)
		{
			style.HasChanged = false;
			Add(style);
		}

		#region --- Member Creation Interface ---

        /// <summary>
        /// Creates a new default <see cref="GradientStyle"/> object and adds it to this <see cref="GradientStyleCollection"/> object.
        /// </summary>
        /// <param name="newMemberName">The name of the style.</param>
        /// <returns>Newly created <see cref="GradientStyle"/>.</returns>
		public GradientStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

        /// <summary>
        /// Copies a specified <see cref="GradientStyle"/> and adds it to this <see cref="GradientStyleCollection"/> object.
        /// </summary>
        /// <param name="newMemberName">The name of the new style.</param>
        /// <param name="oldMemberName">The name of the style to copy.</param>
        /// <returns>Newly created <see cref="GradientStyle"/>.</returns>
		public GradientStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			GradientStyle style;
			GradientStyle srcStyle = this[oldMemberName];
			if(srcStyle != null)
			{
				style = (GradientStyle)srcStyle.Clone();
				style.Name = newMemberName;
			}
			else
				style = new GradientStyle(newMemberName);
			Add(style);
			return style;
		}

		
		/// <summary>
		/// Copies a specified <see cref="GradientStyle"/> and adds it to this <see cref="GradientStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberKind">The name of the style to copy.</param>
		/// <returns>Newly created <see cref="GradientStyle"/>.</returns>
		public GradientStyle CreateFrom(string newMemberName, GradientStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,GradientStyle.NameOf(oldMemberKind));
		}
#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ===============  ");
			S.Comment("    Gradient Styles  ");
			S.Comment("    ===============  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("GradientStyle"))
				{
					GradientStyle P = new GradientStyle();
					P.Serialize(S);
					Add(P);
					while(S.GoToNext("GradientStyle"))
					{
						P = new GradientStyle();
						P.Serialize(S);
						Add(P);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(GradientStyle P in this)
				{
					if(S.BeginTag("GradientStyle"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}
		#endregion
	}


	/// <summary>
	/// A collection of <see cref="Grid"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class GridCollection : NamedCollection 
	{
		private bool hasChanged = true;

		internal GridCollection(Object owner) : base(typeof(Grid), owner)
		{ }

		internal GridCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="Grid"/> at the specified indexed location in the <see cref="ConstantLineCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="Grid"/> from the <see cref="ConstantLineCollection"/> object.</param>
		public new Grid this[object index]   
		{ 
			get 
			{
				if(index is GridKind)
					index = Grid.NameOf((GridKind)index);
				return ((Grid)base[index]); 
			} 
			set 
			{
				if(index is GridKind)
					index = Grid.NameOf((GridKind)index);
				base[index] = value; 
			} 
		}

		public override int Add(object obj)
		{
			hasChanged = true;
			(obj as Grid).SetOwner((ChartObject)Owner);
			return base.Add(obj);
		}

		public new void Remove(object obj)
		{
			hasChanged = true;
			base.Remove(obj);
		}

		internal bool HasChanged
		{
			get
			{
				if(hasChanged)
					return true;
				foreach(Grid L in this)
					if(L.HasChanged)
						return true;
				return false;
			}
			set
			{
				hasChanged = value;
			}
		}

		internal override object Owner 
		{
			get 
			{
				return base.Owner;
			}
			set 
			{
				foreach(Grid grid in this)
				{
					grid.SetOwner(value as ChartObject);
				}
				base.Owner = value;
			}
		}			
	};
	
	/// <summary>
	/// A collection of <see cref="StripSet"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class StripSetCollection : NamedCollection 
	{
		private bool hasChanged = true;

		internal StripSetCollection(Object owner) : base(typeof(StripSet), owner)
		{ }

		internal StripSetCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="StripSet"/> at the specified indexed location in the <see cref="StripCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="StripSet"/> from the <see cref="StripCollection"/> object.</param>
		public new StripSet this[object index]   
		{ 
			get 
			{
				if(index is StripsKind)
					index = StripSet.NameOf((StripsKind)index);
				return ((StripSet)base[index]); } 
			set 
			{ 
				if(index is StripsKind)
					index = StripSet.NameOf((StripsKind)index);
				base[index] = value; 
			} 
		}

		public override int Add(object obj)
		{
			hasChanged = true;
			(obj as StripSet).SetOwner((ChartObject)Owner);
			return base.Add(obj);
		}

		public new void Remove(object obj)
		{
			hasChanged = true;
			base.Remove(obj);
		}
	
		internal bool HasChanged
		{
			get
			{
				if(hasChanged)
					return true;
				foreach(StripSet L in this)
					if(L.HasChanged)
						return true;
				return false;
			}
			set
			{
				hasChanged = value;
			}
		}
	}

	/// <summary>
	/// A collection of <see cref="AxisAnnotation"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class AxisAnnotationCollection : NamedCollection  
	{
		private bool hasChanged = true;

		internal AxisAnnotationCollection(Object owner) : base(typeof(AxisAnnotation), owner)
		{ }

		internal AxisAnnotationCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="AxisAnnotation"/> at the specified indexed location in the <see cref="AxisAnnotationCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="AxisAnnotation"/> from the <see cref="AxisAnnotationCollection"/> object.</param>
		public new AxisAnnotation this[object index]   
		{ 
			get 
			{
				if(index is AxisAnnotationKind)
				{
					index = AxisAnnotation.NameOf((AxisAnnotationKind)index);
				}
				return ((AxisAnnotation)base[index]); 
			} 
			set 
			{
				if(index is AxisAnnotationKind)
				{
					// Remove series name part
					string name = AxisAnnotation.NameOf((AxisAnnotationKind)index);
					string[] nameParts = name.Split('.');
					index = nameParts.Length-1;
				}
				base[index] = value; 
			} 
		}

		public override int Add(object obj)
		{
			hasChanged = true;
			AxisAnnotation a = obj as AxisAnnotation;
			if(a != null)
			{
				a.SetOwner((ChartObject)Owner);
			}
			return base.Add(obj);
		}

		public new void Remove(object obj)
		{
			hasChanged = true;
			base.Remove(obj);
		}
	
		internal bool HasChanged
		{
			get
			{
				if(hasChanged)
					return true;
				foreach(AxisAnnotation L in this)
					if(L.HasChanged)
						return true;
				return false;
			}
			set
			{
				hasChanged = value;
			}
		}
	}


	/// <summary>
	/// A collection of <see cref="MultiLineStyleItem"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class MultiLineStyleCollection : CollectionWithType 
	{
		internal MultiLineStyleCollection(Object owner) : base(typeof(MultiLineStyleItem), owner)
		{ }
        
		internal MultiLineStyleCollection() : this(null) {}

		/// <summary>
		/// Indicates the <see cref="MultiLineStyleItem"/> at the specified indexed location in the <see cref="MultiLineStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="MultiLineStyleItem"/> from the <see cref="MultiLineStyleCollection"/> object.</param>
		public new MultiLineStyleItem this[object index]   
		{ 
			get { return ((MultiLineStyleItem)base[index]); } 
			set { base[index] = value; if((Owner as LineStyle)!= null) value.LineStyle = Owner as LineStyle; } 
		}
	}


	/// <summary>
	/// A collection of <see cref="DashLineStyleSegment"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class DashLineStyleSegmentCollection : CollectionWithType 
	{
		internal DashLineStyleSegmentCollection(Object owner) : base(typeof(DashLineStyleSegment), owner)
		{ }
        
		internal DashLineStyleSegmentCollection() : this(null) {}

		/// <summary>
        /// Indicates the <see cref="DashLineStyleSegment"/> at the specified indexed location in this <see cref="DashLineStyleSegmentCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="DashLineStyleSegment"/> from this <see cref="DashLineStyleSegmentCollection"/> object.</param>
		public new DashLineStyleSegment this[object index]   
		{ 
			get { return ((DashLineStyleSegment)base[index]); } 
			set { base[index] = value; } 
		}
	}

}
