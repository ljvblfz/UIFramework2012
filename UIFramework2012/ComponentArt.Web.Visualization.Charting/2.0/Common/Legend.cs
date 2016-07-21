using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Specifies the position of the <see cref="Legend"/> in the chart.
	/// </summary>
#if !__BUILDING_CRI__
	[Editor(typeof(GenericAlignmentEditor), typeof(System.Drawing.Design.UITypeEditor))] 
#endif
	public enum LegendPositionKind
	{
		TopLeft,
		TopCenter, 
		TopRight,
		CenterLeft,
		CenterRight,
		BottomLeft, 
		BottomCenter,
		BottomRight
	}

	/// <summary>
	/// Specifies the positioning of items in the <see cref="Legend"/>.
	/// </summary>
	public enum LegendKind
	{
		/// <summary>
		/// Items are in column.
		/// </summary>
		Column,
		/// <summary>
		/// Items are in row.
		/// </summary>
		Row
	}

	public enum LegendItemKind
	{
		RectangleItem,
		LineItem,
		MarkerItem,
		Unknown
	}

	/// <summary>
	/// Represents the legend of the chart
	/// </summary>
	[TypeConverter(typeof(LegendConverter))]
	public class Legend : IDisposable, INamedObject 
	{
		private ChartBase	owningChart;
		private int		interColumnSpace = 10;
		private float		scaleFactor = 1.0f;
        private double dpi = 1;

		// NOTE: If you change any of the following initial values ,
		//		 make sure that the corresponding "ShouldSerialize...()" function
		//		 is updated !!!

		private LegendPositionKind	legendPosition = LegendPositionKind.TopRight;
		private LegendKind			kind = LegendKind.Column;
		private int					maxNumberOfItemsInColumn = 0;
		private int					maxNumberOfItemsInRow = 0;
		// Background Rectangle
		private bool		drawBackgroundRectangle = true;
		private Color		backColor = Color.Transparent;
		private Color		borderColor = Color.Transparent;
		private int			borderWidth = 1;
		private int			borderShadeWidth = 3;
		// Item Rectangle
		private Color		itemBackColor = Color.White;
		private int			itemBorderWidth = 1;
		private int			itemBorderShadeWidth = 2;

		private Font		font = new Font("Arial",10f);
		private Color		fontColor = Color.Transparent;

		private int			inRectangleMargin = 10;
		private SizeF		locationOffset = new SizeF(10,10);

		private LegendItemCollection legendItems;
		
		private ArrayList	drawItems;
		private Rectangle	legendRect;
		private Size		rectSize; // Symbol rectangle size
		private int			rectYOffset;
		private TargetArea	targetArea = null;

		private string		name;
		private NamedCollection owningCollection;

		private bool		visible = false;

		private bool		ordered = false;
		private int			numberOfItems = int.MaxValue;

		private bool		sharesChartArea = true;

		#region --- Constructor ---
		/// <summary>
		/// Initializes a new instance of <see cref="Legend"/> class and sets its name.
		/// </summary>
		public Legend(string name) 
		{
			this.name = name;
			legendItems = new LegendItemCollection(this);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Legend"/> class with default parameters.
		/// </summary>
		public Legend() : this("<Default>") { }

		#endregion

		#region --- INamedObject I/F Implementation ---

		public string Name {get { return name; } set { name = value; } }
		/// <summary>
		/// Sets or gets the NamedCollection object the element belongs to
		/// </summary>
		[Browsable(false)]
		public NamedCollection OwningCollection { get { return owningCollection; } set { owningCollection = value; } }
		#endregion

		#region --- Properties ---

		public override string ToString() { return Name; }
		
		internal ChartBase OwningChart { get { return OwningTargetArea.OwningChart; } }

		internal LegendItemCollection LegendItems { get { return legendItems; } set { legendItems = value; } }

		public bool Ordered { get { return ordered; } set { ordered = value; }}
		public int  NumberOfItems { get { return numberOfItems; } set { numberOfItems = value; }}

		/// <summary>
		/// Sets or gets a value indicating whether this <see cref="Legend"/> object and the 
		/// chart area are in sharing mode.
		/// </summary>
		/// <remarks>
		/// When set to <see langword="true"/>, chart and legend share the same drawing area. This may cause 
		/// legend being drawn on top of the chart. To prevent overlapping, set set this property to <see langword="false"/>
		/// </remarks>
		[Description("Chart area sharing mode")]
		[DefaultValue(true)]
		public bool SharesChartArea { get { return sharesChartArea; } set { sharesChartArea = value; } }

			/// <summary>
			/// Gets or sets the type of this <see cref="Legend"/> object.
			/// </summary>
			[DefaultValue(LegendKind.Column)]
			[Description("The type of this legend object.")]
			[RefreshProperties(RefreshProperties.All)]
			[NotifyParentProperty(true)]
			public LegendKind	LegendLayout			{ get { return kind; } set { kind = value; } }
		/// <summary>
		/// Gets or sets the maximum number of items in a column of this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(0)]
		[Description("The maximum number of items in a column.")]
		[NotifyParentProperty(true)]
		public int			NumberOfItemsInColumn	{ get { return maxNumberOfItemsInColumn; } set { maxNumberOfItemsInColumn = value; } }
		/// <summary>
		/// Gets or sets the maximum number of items in a row of this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(0)]
		[Description("The maximum number of items in a row.")]
		[NotifyParentProperty(true)]
		public int			NumberOfItemsInRow		{ get { return maxNumberOfItemsInRow; } set { maxNumberOfItemsInRow = value; } }

		/// <summary>
		/// Gets or sets a value indcating whether the outer rectangle should be drawn fot this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(true)]
		[Description("Indcating whether the outer rectangle should be drawn.")]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		public bool		DrawBackgroundRectangle { get { return drawBackgroundRectangle; } set { drawBackgroundRectangle = value; } }

		/// <summary>
		/// Gets or sets the background color used in this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(typeof(Color), "Transparent")]
		[Description("The background color of the legend.")]
		[NotifyParentProperty(true)]
		public Color	BackColor				{ get { return backColor; } set { backColor = value; } }
		/// <summary>
		/// Gets or sets the border color used in this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(typeof(Color), "Transparent")]
		[Description("The border color of the legend.")]
		[NotifyParentProperty(true)]
		public Color	BorderColor				{ get { return borderColor; } set { borderColor = value; } }
		/// <summary>
		/// Gets or sets the border width used in this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(1)]
		[Description("The border width of the legend.")]
		[NotifyParentProperty(true)]
		public int		BorderWidth				{ get { return borderWidth; } set { borderWidth = value; } }
		/// <summary>
		/// Gets or sets the border shade width used in this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(3)]
		[Description("The border shade width of the legend.")]
		[NotifyParentProperty(true)]
		public int		BorderShadeWidth		{ get { return borderShadeWidth; } set { borderShadeWidth = value; } }
		/// <summary>
		/// Gets or sets the item border width used in this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(1)]
		[Description("The item border width of the legend.")]
		[NotifyParentProperty(true)]
		public int		ItemBorderWidth			{ get { return itemBorderWidth; } set { itemBorderWidth = value; } }
		/// <summary>
		/// Gets or sets the item border shade width used in this <see cref="Legend"/> object.
		/// </summary>
		[DefaultValue(2)]
		[Description("The item border shade width of the legend.")]
		[NotifyParentProperty(true)]
		public int		ItemBorderShadeWidth	{ get { return itemBorderShadeWidth; } set { itemBorderShadeWidth = value; } }

		/// <summary>
		/// Gets or sets the font used to display the text of legend items.
		/// </summary>
		[DefaultValue(typeof(Font), "Arial, 8.25pt")]
		[Description("The font used to display the text of legend items.")]
		[NotifyParentProperty(true)]
		public Font		Font					{ get { return font; } set { font = value; } }
		/// <summary>
		/// Gets or sets the foreground color used to display text in the legend.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		[Description("Foreground color used to display text in the legend.")]
		[NotifyParentProperty(true)]
		public Color	FontColor				{ get { return fontColor; } set { fontColor = value; } }

		/// <summary>
		/// Gets or sets the location of the legend inside the chart control.
		/// </summary>
		[DefaultValue(LegendPositionKind.TopRight)]
		[Description("Determines the location of the legend inside the chart control.")]
		[NotifyParentProperty(true)]
		public LegendPositionKind LegendPosition { get { return legendPosition; } set { legendPosition = value; } }

		/// <summary>
		/// Gets os sets a value that determines whether the control is visible or hidden.
		/// </summary>
		[DefaultValue(false)]
		[Description("Determines whether the legend is visible")]
		[NotifyParentProperty(true)]
		public bool		Visible					{ get { return visible; } set { visible = value; } }
		/// <summary>
		/// Gets or sets horizontal distance of this <see cref="Legend"/> object from the chart border.
		/// </summary>
		[DefaultValue((float) 10)]
		[Description("Horizontal distance of the legend from the chart border.")]
		[NotifyParentProperty(true)]
		public float	LocationOffsetHorizontal{ get { return locationOffset.Width; } set { locationOffset.Width = value; } }
		/// <summary>
		/// Gets or sets vertical distance of this <see cref="Legend"/> object from the chart border.
		/// </summary>
		[DefaultValue((float) 10)]
		[Description("Vertical distance of the legend from the chart border.")]
		[NotifyParentProperty(true)]
		public float	LocationOffsetVertical  { get { return locationOffset.Height; } set { locationOffset.Height = value; } }

		internal TargetArea OwningTargetArea { get { return targetArea; } set { targetArea = value; } }


		#endregion

		#region --- Construction and Drawing ---
		public int Add(ILegendItemProvider legendProvider)
		{
			if(legendItems == null)
				return -1;
			//LegendItem item = new LegendItem(legendProvider);
			for(int i=0; i<legendItems.Count; i++)
			{
					if(legendProvider.LegendItemText == legendItems[i].LegendItemText)
						return i;
			}
			return legendItems.Add(legendProvider);
		}
		
		/// <summary>
		/// Clears the items from this <see cref="Legend"/> object.
		/// </summary>
		public void Clear() 
		{
			if(legendItems != null)
				legendItems.Clear();
		}

		class DrawingItem
		{
			public ILegendItemProvider	legendItem = null;
			public Size			size;
			public Point		position;
			public string		text;
			public DrawingItem() { }
		}


		/// <summary>
		/// Retrieves the rectangle of this <see cref="Legend"/> object.
		/// </summary>
		/// <returns>rectangle of this <see cref="Legend"/> object.</returns>
		public Rectangle GetRectangle()
		{
			if(!Visible)
				return new Rectangle(0,0,0,0);

			Graphics g = OwningChart.WorkingGraphics;
			if(g != null)
			{
				OwningChart.GetTargetRectangle(g);
				ComputeLayout(g);
			}
			else
			{
				Bitmap bmp = new Bitmap(10,10);
				g = Graphics.FromImage(bmp);
				OwningChart.GetTargetRectangle(g);
				this.dpi = g.DpiX;
				ComputeLayout(g);
				g.Dispose();
				bmp.Dispose();
			}
			GetScaleFactor();
			float sf = scaleFactor;
			return new Rectangle((int)(legendRect.X*sf),(int)(legendRect.Y*sf),(int)(legendRect.Width*sf),(int)(legendRect.Height*sf));
		}

		private void GetScaleFactor()
		{
			scaleFactor = (float)(OwningChart.ScaleToNativeSize * 96/dpi);//*OwningChart.ReducedSamplingStep;
            scaleFactor = ((int)(scaleFactor * 8)) * 0.125f;
		}

		internal void ComputeLayout(Graphics g)
		{
			if(!visible || legendItems == null || targetArea == null)
				return;

			dpi = g.DpiX;
			GetScaleFactor();
			Rectangle rect = targetArea.EffectiveOuterTarget;
			double xT=0,yT=0,wT=1,hT=1;
			targetArea.GetParametersReducedByTitles(ref xT,ref yT,ref wT,ref hT);
			
			rect = new Rectangle(
				(int)(rect.X + xT*rect.Width),
				(int)(rect.Y + yT*rect.Height),
				(int)(wT*rect.Width),
				(int)(hT*rect.Height));

			LegendItemCollection effectiveItems = GetEffectiveLegendItems();
			drawItems = new ArrayList();

			GraphicsContainer container = g.BeginContainer();

			float sf = scaleFactor;
			g.ScaleTransform(sf,sf);
			rect.X		= (int)(rect.X/sf);
			rect.Y		= (int)(rect.Y/sf);
			rect.Width	= (int)(rect.Width/sf);
			rect.Height	= (int)(rect.Height/sf);

			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			// Items metrics

			SizeF rectSizeF = g.MeasureString("aa",font);	// Symbol rectangle size
			rectYOffset = (int)(rectSizeF.Height/6.0f);
			rectSize = new Size((int)(rectSizeF.Width + 0.9),(int)(rectSizeF.Height*2/3 + 0.9));

			// Spacing
			interColumnSpace = rectSize.Height;
			
			int maxWidth = 0;
			foreach(ILegendItemProvider item in effectiveItems)
			{
				if(item.LegendItemVisible)
				{
					DrawingItem di = new DrawingItem();
					di.legendItem = item;
					di.text = item.LegendItemText;
					di.size = g.MeasureString(di.text,font).ToSize();
					di.size.Width += rectSize.Width;
					di.size.Height = Math.Max(di.size.Height,rectSize.Height);
					maxWidth = Math.Max(maxWidth,di.size.Width);
					drawItems.Add(di);
				}
			}

			// Items layout

			Size legendRectSize = new Size(0,0);
			Size maxLegendSize = new Size((int)(rect.Width - 2*locationOffset.Height), (int)(rect.Height - 2*locationOffset.Height));
			switch(kind)
			{
				case LegendKind.Column:
					legendRectSize = ColumnLayout(drawItems, maxWidth, maxLegendSize);
					break;
				case LegendKind.Row:
					legendRectSize = RowLayout(drawItems, maxWidth, maxLegendSize);
					break;
			}

			// Drawing

			// Drawing legend box
			legendRect = new Rectangle(0,0,legendRectSize.Width, legendRectSize.Height);

			switch(legendPosition)
			{
				case LegendPositionKind.BottomCenter:
					legendRect.X = (int)(rect.X + (rect.Width - legendRect.Width)/2);
					legendRect.Y = (int)(rect.Y + rect.Height - legendRect.Height - locationOffset.Width);
					break;
				case LegendPositionKind.BottomLeft:
					legendRect.X = (int)(rect.X + locationOffset.Width);
					legendRect.Y = (int)(rect.Y + rect.Height - legendRect.Height - locationOffset.Height);
					break;
				case LegendPositionKind.BottomRight:
					legendRect.X = (int)(rect.X + rect.Width - locationOffset.Width - legendRect.Width);
					legendRect.Y = (int)(rect.Y + rect.Height - legendRect.Height - locationOffset.Height);
					break;
				case LegendPositionKind.CenterLeft:
					legendRect.X = (int)(rect.X + locationOffset.Width);
					legendRect.Y = (int)(rect.Y + (rect.Height - legendRect.Height)/2);
					break;
				case LegendPositionKind.CenterRight:
					legendRect.Y = (int)(rect.Y + (rect.Height - legendRect.Height)/2);
					legendRect.X = (int)(rect.X + rect.Width - locationOffset.Width - legendRect.Width);
					break;
				case LegendPositionKind.TopCenter:
					legendRect.X = (int)(rect.X + (rect.Width - legendRect.Width)/2);
					legendRect.Y = (int)(rect.Y + locationOffset.Height);
					break;
				case LegendPositionKind.TopLeft:
					legendRect.X = (int)(rect.X + locationOffset.Width);
					legendRect.Y = (int)(rect.Y + locationOffset.Height);
					break;
				case LegendPositionKind.TopRight:
					legendRect.X = (int)(rect.X + rect.Width - locationOffset.Width - legendRect.Width);
					legendRect.Y = (int)(rect.Y + locationOffset.Height);
					break;
			}
			
			g.EndContainer(container);
		}

		private LegendItemCollection GetEffectiveLegendItems()
		{
			// Effective drawing items

			LegendItemCollection effectiveLegendItems;

			if(!ordered && numberOfItems >= LegendItems.Count)
				effectiveLegendItems = legendItems;
			else
			{
				int nItems = Math.Min(numberOfItems,legendItems.Count);
				effectiveLegendItems = new LegendItemCollection(this);
				for(int i=0; i<legendItems.Count; i++)
					effectiveLegendItems.Add(legendItems[i]);
				if(ordered)
				{
					for(int i=0; i<nItems-1; i++)
						for(int j=i+1; j<legendItems.Count; j++)
						{
							if(effectiveLegendItems[i].LegendItemCharacteristicValue < 
								effectiveLegendItems[j].LegendItemCharacteristicValue)
							{
								ILegendItemProvider da = effectiveLegendItems[i];
								effectiveLegendItems[i] = effectiveLegendItems[j];
								effectiveLegendItems[j] = da;
							}
						}
				}
				int count = legendItems.Count;
				while(nItems < count)
				{
					effectiveLegendItems.RemoveAt(nItems);
					count--;
				}
			}
			return effectiveLegendItems;
		}

		internal void Render(Graphics g)
		{
			if(!visible || legendItems == null || targetArea == null)
				return;
            dpi = g.DpiX;

			ComputeLayout(g);

			GraphicsContainer container = g.BeginContainer();
			GetScaleFactor();

			float sf = scaleFactor;
            g.ScaleTransform(sf, sf);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

			// Spacing
			interColumnSpace = rectSize.Height;

			// Drawing

			// Drawing legend box
			
			Brush shadeBrush = new SolidBrush(Color.FromArgb(128,0,0,0));
			Color bColor = borderColor;
			if(bColor.A == 0)
				bColor = OwningChart.Palette.LegendBorderColor;
			Pen penBorder = new Pen(bColor,borderWidth);
			if(drawBackgroundRectangle)
			{
				if(BorderShadeWidth>0)
				{
					g.FillRectangle(shadeBrush,
						new Rectangle(
						legendRect.X+BorderShadeWidth,
						legendRect.Y+legendRect.Height,
						legendRect.Width,BorderShadeWidth)
						);
					g.FillRectangle(shadeBrush,
						new Rectangle(
						legendRect.X+legendRect.Width,
						legendRect.Y+BorderShadeWidth,
						BorderShadeWidth,legendRect.Height-BorderShadeWidth)
						);
				}
				Color bkColor = backColor;
				if(bkColor.A == 0)
					bkColor = OwningChart.Palette.LegendBackgroundColor;
				Brush brush = new SolidBrush(bkColor);
				g.FillRectangle(brush,legendRect);
				brush.Dispose();
				g.DrawRectangle(penBorder,legendRect);
			}

			// Drawing items
			Color fColor = fontColor;
			if(fColor.A == 0)
				fColor = OwningChart.Palette.LegendFontColor;
			Brush fontBrush = new SolidBrush(fColor);
			Brush brushItemBack = new SolidBrush(itemBackColor);

			foreach(DrawingItem di in drawItems)
			{
				int xOff = legendRect.X;
				int yOff = legendRect.Y;
				// Symbol rectangle
				Rectangle symbolRect = new Rectangle(xOff+di.position.X,yOff+di.position.Y,rectSize.Width,rectSize.Height);
				if(OwningChart.ObjectTrackingEnabled)
				{
					int x0 = symbolRect.X;
					int x1 = x0+symbolRect.Width;
					int y0 = OwningChart.TargetSize.Height - symbolRect.Y-symbolRect.Height;
					int y1 = y0 + symbolRect.Height;
					x0 = (int)(sf*x0);
					x1 = (int)(sf*x1);
					y0 = (int)(sf*y0);
					y1 = (int)(sf*y1);
					Point[] pts = new Point[]
						{
							new Point(x0,y0),
							new Point(x0,y1),
							new Point(x1,y1),
							new Point(x1,y0),
							new Point(x0,y0)

						};
					OwningChart.ObjectMapper.AddMapAreaPolygon(pts,0,0,di.legendItem);
				}
				// Shade
				if(ItemBorderShadeWidth > 0)
					g.FillRectangle(shadeBrush,
						new Rectangle(symbolRect.X + ItemBorderShadeWidth, symbolRect.Y + ItemBorderShadeWidth, symbolRect.Width, symbolRect.Height));
				// Background
				g.FillRectangle(brushItemBack,symbolRect);
				// Symbol
				di.legendItem.DrawLegendItemRectangle(g,symbolRect);
				// Frame
				g.DrawRectangle(penBorder,symbolRect);
				// Text
                g.DrawString(di.legendItem.LegendItemText, font, fontBrush, di.position.X + rectSize.Width + xOff + 3, di.position.Y + yOff);
			}

            penBorder.Dispose();
			brushItemBack.Dispose();
			shadeBrush.Dispose();
			g.EndContainer(container);

		}

		private Size ColumnLayout(ArrayList drawItems, int maxWidth, Size maxLegendSize)
		{
			// Computes size of the surrounding rectangle and positions of items
			// relative to the point (0,0) of the rectangle.
            int inRectangleMarginPxl = (int)(inRectangleMargin * dpi / 96.0 + 0.5);
            int interColumnSpacePxl = (int)(interColumnSpace * dpi / 96.0 + 0.5);

            Size size = new Size(maxWidth + inRectangleMarginPxl, 0);
            int x = inRectangleMarginPxl;
            int y = inRectangleMarginPxl;
			int i = 0;
			int count = 0;
			foreach (DrawingItem di in drawItems)
			{
				bool sizeExceeded = y > maxLegendSize.Height - 2*locationOffset.Height - di.size.Height;
				bool countExceeded = maxNumberOfItemsInColumn > 0 && count >= maxNumberOfItemsInColumn;
				if(sizeExceeded || countExceeded) 
				{
                    y = inRectangleMarginPxl;
					x += (int)(maxWidth + interColumnSpace);
					size.Width += maxWidth + interColumnSpace;
					count = 0;
				}
				di.position = new Point(x,y);
				y += di.size.Height;
				size.Height  = Math.Max(y,size.Height);
				count ++;
				i ++;
			}

            size.Width += inRectangleMarginPxl;
            size.Height += inRectangleMarginPxl;
			return size;
		}

		private Size RowLayout(ArrayList drawItems, int maxWidth, Size maxLegendSize)
		{
			// Computes size of the surrounding rectangle and positions of items
			// relative to the point (0,0) of the rectangle.

			Size size = new Size(0,0);
			int x = inRectangleMargin;
			int y = inRectangleMargin;
			int i = 0;
			int count = 0;
			foreach (DrawingItem di in drawItems)
			{
				bool sizeExceeded = x > maxLegendSize.Width - 2*locationOffset.Width - maxWidth;
				bool countExceeded = maxNumberOfItemsInRow > 0 && count >= maxNumberOfItemsInRow;
				if(sizeExceeded || countExceeded) 
				{
					x = inRectangleMargin;
					y += di.size.Height;
					size.Height = y + di.size.Height;
					count = 0;
				}
				di.position = new Point(x,y);
				x += maxWidth + interColumnSpace;
				size.Width  = Math.Max(x,size.Width);
				size.Height = Math.Max(y + di.size.Height,size.Height);
				count ++;
				i ++;
			}

			size.Width += inRectangleMargin;	// adding ending gaps
			size.Height += inRectangleMargin;
			return size;
		}

		#endregion

		#region --- Serialization and Browsing ---

		private bool ShouldBrowseNumberOfItemsInColumn() { return kind == LegendKind.Column;; }
		private bool ShouldBrowseNumberOfItemsInRow() { return kind == LegendKind.Row;; }
		private bool ShouldBrowseBackColor() { return drawBackgroundRectangle; }
		private bool ShouldBrowseBorderColor() { return drawBackgroundRectangle; }
		private bool ShouldBrowseBorderWidth() { return drawBackgroundRectangle; }
		private bool ShouldBrowseBorderShadeWidth() { return drawBackgroundRectangle; }

		private bool ShouldSerializeLegendLayout() { return kind != LegendKind.Column; }
		private bool ShouldSerializeNumberOfItemsInColumn() { return maxNumberOfItemsInColumn != 0; }
		private bool ShouldSerializeNumberOfItemsInRow() { return maxNumberOfItemsInRow != 0; }
		private bool ShouldSerializeBackColor() { return backColor != Color.FromArgb(255,255,196); }
		private bool ShouldSerializeDrawBackgroundRectangle() { return !drawBackgroundRectangle; }
		private bool ShouldSerializeBorderColor() { return borderColor != Color.FromArgb(128,0,0,0);; }
		private bool ShouldSerializeBorderWidth() { return borderWidth != 1; }
		private bool ShouldSerializeBorderShadeWidth() { return borderShadeWidth != 3; }
		private bool ShouldSerializeItemBorderWidth() { return itemBorderWidth != 1; }
		private bool ShouldSerializeItemBorderShadeWidth() { return itemBorderShadeWidth != 2; }
		private bool ShouldSerializeFontColor() { return fontColor != Color.Black; }
		private bool ShouldSerializeLegendPosition() { return legendPosition != LegendPositionKind.TopRight; }
		private bool ShouldSerializeVisible() { return true; }
		private bool ShouldSerializeLocationOffsetHorizontal() { return locationOffset.Width != 10; }
		private bool ShouldSerializeLocationOffsetVertical() { return locationOffset.Height != 10; }

		internal void Serialize(XmlCustomSerializer S)
		{
			if(!Visible && !S.Reading)	// if writing non-visible...
				return;
			S.Comment("    ======    ");
			S.Comment("    Legend    ");
			S.Comment("    ======    ");

			S.AttributeProperty(this,"Visible");
			S.AttributeProperty(this,"LegendLayout");
			S.AttributeProperty(this,"NumberOfItemsInColumn");
			S.AttributeProperty(this,"NumberOfItemsInRow");
			S.AttributeProperty(this,"BackColor");
			S.AttributeProperty(this,"DrawBackgroundRectangle");
			S.AttributeProperty(this,"BorderColor");
			S.AttributeProperty(this,"BorderWidth");
			S.AttributeProperty(this,"BorderShadeWidth");
			//S.AttributeProperty(this,"ItemBorderColor");
			S.AttributeProperty(this,"ItemBorderWidth");
			S.AttributeProperty(this,"ItemBorderShadeWidth");
			S.AttributeProperty(this,"FontColor");
			S.AttributeProperty(this,"LegendPosition");
			S.AttributeProperty(this,"LocationOffsetHorizontal");
			S.AttributeProperty(this,"LocationOffsetVertical");
		}

		private static string[] PropertiesOrder = new string[] 
			{
				"LegendPosition",
				"LegendLayout",
				"NumberOfItemsInColumn",
				"NumberOfItemsInRow",
				"LocationOffsetHorizontal",
				"LocationOffsetVertical",
				"Font",
				"FontColor",
				"DrawBackgroundRectangle",
				"BackColor",
				"BorderColor",
				"BorderWidth",
				"BorderShadeWidth",
				//"ItemBorderColor",
				"ItemBorderWidth",
				"ItemBorderShadeWidth",
				"Visible"
			};

		#endregion


		internal void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (font != null) 
				{
					font.Dispose();
					font = null;
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
	
	/// <summary>
	/// A collection of <see cref="Legend"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class LegendCollection : NamedCollection
	{
		internal LegendCollection(Object owner) : base(typeof(Legend), owner)
		{ }

		internal LegendCollection() : this(null) { }

		/// <summary>
		/// Adds a specified object to a <see cref="CollectionWithType"/> object.
		/// </summary>
		/// <param name="value">Object to be added to <see cref="CollectionWithType"/>.</param>
		/// <returns>The index of the newly added object or -1 if the the object could not be added.</returns>
		public override int Add( object value )  
		{
			if (value == null)
				return -1;
			Legend legend = value as Legend;
			if (legend == null)
				return -1;

			legend.OwningCollection = this;
			legend.OwningTargetArea = this.Owner as TargetArea;
			return( List.Add( legend ) );
		}

		/// <summary>
		/// Indicates the <see cref="Legend"/> at the specified indexed location in the <see cref="LegendCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="Legend"/> from the <see cref="LegendCollection"/> object.</param>
		public new Legend this[object index]   
		{ 
			get { return ((Legend)base[index]); } 
			set { base[index] = value; value.OwningTargetArea = this.Owner as TargetArea; value.OwningCollection = this; } 
		}
	}

}
