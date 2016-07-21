using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Determines the position of <see cref="ChartTitle"/>.
	/// </summary>
	public enum TitlePosition
	{
		Top,
		Bottom,
		LeftUpwards,
		LeftDownwards,
		RightUpwards,
		RightDownwards
	};

	/// <summary>
	/// Represents the title of the chart.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class ChartTitle : ChartObject
	{
		const double	defaultRectMargin = 2;
		const double	defaultTextMargin = 2;
		
		// Text Rectangle
		const bool			default_backgroundRectangleVisible = false;
		const bool			default_backgroundBorderVisible = true;
		const GradientKind	default_backgroundGradientKind = GradientKind.None;
		const string		default_backgroundColor1 = "White";
		const string		default_backgroundColor2 = "White";

		bool			backgroundRectangleVisible = default_backgroundRectangleVisible;
		bool			backgroundBorderVisible = default_backgroundBorderVisible;
		GradientKind	backgroundGradientKind = default_backgroundGradientKind;
		Color			backgroundColor1 = Color.FromName(default_backgroundColor1);
		Color			backgroundColor2 = Color.FromName(default_backgroundColor2);


		const string		default_borderColor = "Black";
		const double		default_borderWidth = 1;
		const DashStyle		default_borderDashStyle = DashStyle.Solid;
		const string		default_borderShadeColor = "196, 0, 0, 0";
		const double		default_borderShadeWidthPts = 2;

		Color			borderColor = Color.FromName(default_borderColor);
		double			borderWidth = default_borderWidth;
		DashStyle		borderDashStyle = default_borderDashStyle;
		Color			borderShadeColor = (Color)new System.Drawing.ColorConverter().ConvertFromInvariantString(default_borderShadeColor);
		double			borderShadeWidthPts = default_borderShadeWidthPts;


		const TitlePosition	default_titlePosition = TitlePosition.Top;
		TitlePosition	titlePosition = default_titlePosition;

		const double			default_rectangleLeftMargin = defaultRectMargin;
		const double			default_rectangleTopMargin = defaultRectMargin;
		const double			default_rectangleRightMargin = defaultRectMargin;
		const double			default_rectangleBottomMargin = defaultRectMargin;
		
		double			rectangleLeftMargin = default_rectangleLeftMargin;
		double			rectangleTopMargin = default_rectangleTopMargin;
		double			rectangleRightMargin = default_rectangleRightMargin;
		double			rectangleBottomMargin = default_rectangleBottomMargin;
		
		SizeF			rectangleSize = new SizeF(0,0);

		// Text Position
		const TextReferencePoint	default_refPoint = TextReferencePoint.Default;
		const double				default_textLeftMargin = defaultTextMargin;
		const double				default_textTopMargin = defaultTextMargin;
		const double				default_textRightMargin = defaultTextMargin;
		const double				default_textBottomMargin = defaultTextMargin;
		const string				default_textFont = "Arial, 12pt";
		const string				default_text = "";
		const string				default_foreColor = "Transparent";

		TextReferencePoint	refPoint = default_refPoint;
		double				textLeftMargin = default_textLeftMargin;
		double				textTopMargin = default_textTopMargin;
		double				textRightMargin = default_textRightMargin;
		double				textBottomMargin = default_textBottomMargin;
		Font				textFont = (Font)new FontConverter().ConvertFromInvariantString(default_textFont);
		string				text = default_text;
		Color				foreColor = Color.FromName(default_foreColor);

		// Outline
		const string		default_outlineColor = "Black";
		const double		default_outlineWidth = 0;

		Color				outlineColor = Color.FromName(default_outlineColor);
		double				outlineWidth = default_outlineWidth;

		// Shadow
		const double		default_textShadeWidthPts = 0;						
		const string		default_textShadowColor = "128,0,0,0";
		double				textShadeWidthPts = default_textShadeWidthPts;						
		Color				textShadowColor = (Color)new System.Drawing.ColorConverter().ConvertFromInvariantString(default_textShadowColor);

		// Working data

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of <see cref="ChartTitle"/> with "Title" as the title text.
		/// </summary>
		public ChartTitle() : this("Title") { }

		/// <summary>
		/// Initializes a new instance of <see cref="ChartTitle"/> class with specified title text.
		/// </summary>
		/// <param name="text">The text of the title. This value is stored in the <see cref="ChartTitle.Text"/> property.</param>
		public ChartTitle(string text)
		{
			if(text != null)
				this.text = text;
			else
				this.text = "";
		}
		#endregion

		#region --- Properties ---

		/// <summary>
		/// Gets or sets a value indicating whether the title background is visible.
		/// </summary>
		[Category("Background rectangle")]
		[DefaultValue(default_backgroundRectangleVisible)]
		[Description("Indicating whether the title background is visible")]
		public bool			BackgroundVisible	{ get { return backgroundRectangleVisible; } set { backgroundRectangleVisible = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether the title border is visible.
		/// </summary>
		[Description("Indicating whether the title border is visible")]
		[DefaultValue(default_backgroundBorderVisible)]
		[Category("Background rectangle")]
		public bool			BorderVisible		{ get { return backgroundBorderVisible; } set { backgroundBorderVisible = value; } }

		/// <summary>
		/// Gets or sets the <see cref="GradientKind"/> of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("The gradient kind of the background of this title.")]
		[DefaultValue(default_backgroundGradientKind)]
		[Category("Background rectangle")]
		public GradientKind	GradientKind		{ get { return backgroundGradientKind; } set { backgroundGradientKind = value; } }

		/// <summary>
		/// Gets or sets the background <see cref="Color"/> of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Color of the background or this title.")]
		[DefaultValue(typeof(Color), default_backgroundColor1)]
		[Category("Background rectangle")]
		public Color		BackgroundColor1	{ get { return backgroundColor1; } set { backgroundColor1 = value; } }

		/// <summary>
		/// Gets or sets the gradient background <see cref="Color"/> of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Gradient color of the background or this title.")]
		[DefaultValue(typeof(Color), default_backgroundColor2)]
		[Category("Background rectangle")]
		public Color		BackgroundColor2	{ get { return backgroundColor2; } set { backgroundColor2 = value; } }


		/// <summary>
		/// Gets or sets the color the border is drawn with in this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Color the title border is drawn with.")]
		[DefaultValue(typeof(Color), default_borderColor)]
		[Category("Background rectangle")]
		public Color		BorderColor			{ get { return borderColor; } set { borderColor = value; } }

		/// <summary>
		/// Gets or sets the width of the border drawn around this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("The width of the border drawn around this ChartTitle")]
		[Category("Background rectangle")]
		[DefaultValue(default_borderWidth)]
		public double		BorderWidth			{ get { return borderWidth; } set { borderWidth = value; } }

		/// <summary>
		/// Gets or sets the style used for dashed lines for drawing the border around this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Style of dashed lines to be used when drawing the border.")]
		[Category("Background rectangle")]
		[DefaultValue(default_borderDashStyle)]
		public DashStyle	BorderDashStyle		{ get { return borderDashStyle; } set { borderDashStyle = value; } }

		/// <summary>
		/// Gets or sets the color the shade of the border is drawn with in this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Color the title shade of the border is drawn with.")]
		[Category("Background rectangle")]
		[DefaultValue(typeof(Color), default_borderShadeColor)]
		public Color		BorderShadeColor	{ get { return borderShadeColor; } set { borderShadeColor = value; } }


		/// <summary>
		/// Gets or sets the shade width of the border drawn around this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("The shade width of the border drawn around this ChartTitle")]
		[Category("Background rectangle")]
		[DefaultValue(default_borderShadeWidthPts)]
		public double		BorderShadeWidthPts { get { return borderShadeWidthPts; } set { borderShadeWidthPts = value; } }


		/// <summary>
		/// Gets or sets the margin on the left of the rectangle surrounding this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the left of the rectangle surrounding this chart title.")]
		[Category("Background rectangle")]
		[DefaultValue(default_rectangleLeftMargin)]
		public double		RectangleLeftMargin { get { return rectangleLeftMargin; } set { rectangleLeftMargin = value; } }

		/// <summary>
		/// Gets or sets the margin on the top of the rectangle surrounding this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the top of the rectangle surrounding this chart title.")]
		[Category("Background rectangle")]
		[DefaultValue(default_rectangleTopMargin)]
		public double		RectangleTopMargin	{ get { return rectangleTopMargin; } set { rectangleTopMargin = value; } }

		/// <summary>
		/// Gets or sets the margin on the right of the rectangle surrounding this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the right of the rectangle surrounding this chart title.")]
		[Category("Background rectangle")]
		[DefaultValue(default_rectangleRightMargin)]
		public double		RectangleRightMargin { get { return rectangleRightMargin; } set { rectangleRightMargin = value; } }

		/// <summary>
		/// Gets or sets the margin on the bottom of the rectangle surrounding this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the bottom of the rectangle surrounding this chart title.")]
		[Category("Background rectangle")]
		[DefaultValue(default_rectangleBottomMargin)]
		public double		RectangleBottomMargin { get { return rectangleBottomMargin; } set { rectangleBottomMargin = value; } }
		
        /// <summary>
        /// Gets or sets the size of the title rectangle.
        /// </summary>
		[SRDescription("ChartTitleRectangleSizeDescr")]
		[Category("Background rectangle")]
		[DefaultValue(typeof(Size), "0, 0")]
		public Size		RectangleSize		{ get { return rectangleSize.ToSize(); } set { rectangleSize = value; } }

		// Text Position
		
		/// <summary>
		/// Gets or sets the position of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Position of the title")]
		[Category("Title Text")]
		[DefaultValue(default_titlePosition)]
		public TitlePosition	Position	{ get { return titlePosition; } set { titlePosition = value; } }
		
		/// <summary>
		/// Gets or sets the reference point of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Reference point of the title")]
		[Category("Title Text")]
		[DefaultValue(default_refPoint)]
		public TextReferencePoint	RefPoint	{ get { return refPoint; } set { refPoint = value; } }
		
		/// <summary>
		/// Gets or sets the margin on the left of the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the left of the text of this chart title.")]
		[Category("Title Text")]
		[DefaultValue(default_textLeftMargin)]
		public double		TextLeftMargin		{ get { return textLeftMargin; } set { textLeftMargin = value; } }
		
		/// <summary>
		/// Gets or sets the margin on the top of the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the top of the text of this chart title.")]
		[Category("Title Text")]
		[DefaultValue(default_textTopMargin)]
		public double		TextTopMargin		{ get { return textTopMargin; } set { textTopMargin = value; } }
		
		/// <summary>
		/// Gets or sets the margin on the right of the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the right of the text of this chart title.")]
		[Category("Title Text")]
		[DefaultValue(default_textRightMargin)]
		public double		TextRightMargin		{ get { return textRightMargin; } set { textRightMargin = value; } }
		
		/// <summary>
		/// Gets or sets the margin on the bottom of the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Margin on the bottom of the text of this chart title.")]
		[Category("Title Text")]
		[DefaultValue(default_textBottomMargin)]
		public double		TextBottomMargin	{ get { return textBottomMargin; } set { textBottomMargin = value; } }
		
		/// <summary>
		/// Gets or sets the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Text of the title")]
		[Category("Title Text")]
		[DefaultValue(default_text)]
		public string		Text				{ get { return text; } set { text = value; } }
		
		/// <summary>
		/// Gets or sets the font of the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Font of the text of the title")]
		[Category("Font")]
		[DefaultValue(typeof(Font), default_textFont)]
		public Font			Font				{ get { return textFont; } set { textFont = value; } }
		
		/// <summary>
		/// Gets or sets the color of the text of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Color of the text of the title")]
		[Category("Font")]
		[DefaultValue(typeof(Color), default_foreColor)]
		public Color		ForeColor			{ get { return foreColor; } set { foreColor = value; } }
		// Outline
		
		/// <summary>
		/// Gets or sets the color of the text outline of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Color of the text outline of the title")]
		[Category("Font")]
		[DefaultValue(typeof(Color), default_outlineColor)]
		public Color		OutlineColor		{ get { return outlineColor; } set { outlineColor = value; } }
		
		/// <summary>
		/// Gets or sets the width of the text outline of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Width of the text outline of the title")]
		[Category("Font")]
		[DefaultValue(default_outlineWidth)]
		public double		OutlineWidth		{ get { return outlineWidth; } set { outlineWidth = value; } }
		// Shadow
		
		/// <summary>
		/// Gets or sets the width of the text shade of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Width of the text shade of the title")]
		[Category("Font")]
		[DefaultValue(default_textShadeWidthPts)]
		public double		TextShadeWidthPts	{ get { return textShadeWidthPts; } set { textShadeWidthPts = value; } }
		
		/// <summary>
		/// Gets or sets the color of the text shade of this <see cref="ChartTitle"/> object.
		/// </summary>
		[Description("Color of the text shade of the title")]
		[Category("Font")]
		[DefaultValue(typeof(Color), default_textShadowColor)]
		public Color		TextShadowColor		{ get { return textShadowColor; } set { textShadowColor = value; } }


		#endregion

		#region --- Rendering ---

		internal RectangleF ComputeRectangle(Rectangle rect)
		{
			// Scaling factor
			double pt2W = OwningChart.FromPointToTarget*OwningChart.ReducedSamplingStep;

			// Get the path
			
			GraphicsPath path = new GraphicsPath(FillMode.Alternate);
			float scaledSize = (float)(textFont.SizeInPoints*pt2W);
			path.AddString(text,textFont.FontFamily,(int)textFont.Style,scaledSize,new PointF(0,0),StringFormat.GenericTypographic);

			// Get extremes and shift
			
			float xMin = float.MaxValue;
			float yMin = float.MaxValue;
			float xMax = float.MinValue;
			float yMax = float.MinValue;
			float x,y;
			PointF[] pts = path.PathPoints;
			byte[] types = path.PathTypes;

			int i;
			for (i=0;i<pts.Length;i++)
			{
				xMin = Math.Min(xMin,pts[i].X);
				xMax = Math.Max(xMax,pts[i].X);
				yMin = Math.Min(yMin,pts[i].Y);
				yMax = Math.Max(yMax,pts[i].Y);
			}
			for(i=0; i<pts.Length;i++)
			{
				pts[i].X -= xMin;
				pts[i].Y -= yMin;
			}
			float dxTxt = xMax - xMin;
			float dyTxt = yMax - yMin;

			// Coordinates of the minimum rectangle, including text margins (between 
			//	text and the rectangle borders)
			xMin -= (float)(textLeftMargin*pt2W);
			yMin -= (float)(textTopMargin*pt2W);
			xMax += (float)(textRightMargin*pt2W);
			yMax += (float)(textBottomMargin*pt2W);
			float width = xMax - xMin;
			float height = yMax - yMin;

			// Real rectangle dimensions
			SizeF rectangleSize = this.rectangleSize;

			if(titlePosition == TitlePosition.Top || titlePosition == TitlePosition.Bottom)
			{
				if(rectangleSize.IsEmpty)
					rectangleSize = new SizeF(10000,0);
				float sx = rectangleSize.Width;
				sx = (float)Math.Min(sx,rect.Width - (rectangleLeftMargin + rectangleRightMargin)*pt2W);
				width = Math.Max(width,sx);
				float sy = rectangleSize.Height;
				sy = (float)Math.Min(sy,rect.Height - (rectangleTopMargin + rectangleBottomMargin)*pt2W);
				height = Math.Max(height,sy);
			}
			else
			{
				if(rectangleSize.IsEmpty)
					rectangleSize = new SizeF(0,10000);
				float sx = rectangleSize.Height;
				sx = (float)Math.Min(sx,rect.Height - (rectangleLeftMargin + rectangleRightMargin)*pt2W);
				width = Math.Max(width,sx);
				float sy = rectangleSize.Width;
				sy = (float)Math.Min(sy,rect.Width - (rectangleTopMargin + rectangleBottomMargin)*pt2W);
				height = Math.Max(height,sy);
			}

			// Shift text according to the reference point

			TextReferencePoint eRefPoint = refPoint;
			if(eRefPoint == TextReferencePoint.Default)
			{
				switch(titlePosition)
				{
					case TitlePosition.Bottom:
						eRefPoint = TextReferencePoint.CenterBottom;
						break;
					case TitlePosition.Top:
						eRefPoint = TextReferencePoint.CenterTop;
						break;
					case TitlePosition.LeftDownwards:
						eRefPoint = TextReferencePoint.CenterBottom;
						break;
					case TitlePosition.LeftUpwards:
						eRefPoint = TextReferencePoint.CenterTop;
						break;
					case TitlePosition.RightDownwards:
						eRefPoint = TextReferencePoint.CenterTop;
						break;
					case TitlePosition.RightUpwards:
						eRefPoint = TextReferencePoint.CenterBottom;
						break;
				}
			}
			float shiftX = 0, shiftY = 0;
			switch(eRefPoint)
			{
				case TextReferencePoint.Center:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
				case TextReferencePoint.CenterBottom:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(height - textBottomMargin*pt2W - dyTxt);
					break;
				case TextReferencePoint.CenterTop:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(textTopMargin*pt2W);
					break;
				case TextReferencePoint.LeftCenter:
					shiftX = (float)(textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
				case TextReferencePoint.LeftBottom:
					shiftX = (float)(textRightMargin*pt2W);
					shiftY = (float)(height - textBottomMargin*pt2W - dyTxt);
					break;
				case TextReferencePoint.LeftTop:
					shiftX = (float)(textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W);
					break;
				case TextReferencePoint.RightCenter:
					shiftX = (float)(width - dxTxt - textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
				case TextReferencePoint.RightBottom:
					shiftX = (float)(width - dxTxt - textRightMargin*pt2W);
					shiftY = (float)(height - textBottomMargin*pt2W - dyTxt);
					break;
				case TextReferencePoint.RightTop:
					shiftX = (float)(width - dxTxt - textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W);
					break;
				default:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
			}

			for (i=0;i<pts.Length;i++)
			{
				pts[i].X += shiftX;
				pts[i].Y += shiftY;
			}

			// Process orientation 

			if(titlePosition == TitlePosition.LeftDownwards || titlePosition == TitlePosition.RightDownwards)
			{
				x = width; width = height; height = x;
				for (i=0;i<pts.Length;i++)
				{
					x = width-pts[i].Y;
					y = pts[i].X;
					pts[i].X = x;
					pts[i].Y = y;
				}
			}
			else if(titlePosition == TitlePosition.LeftUpwards || titlePosition == TitlePosition.RightUpwards)
			{
				x = width; width = height; height = x;
				for (i=0;i<pts.Length;i++)
				{
					x = pts[i].Y;
					y = height-pts[i].X;
					pts[i].X = x;
					pts[i].Y = y;
				}
			}
		
			width += (float) outlineWidth;
			height += (float) outlineWidth;

			// Drawing rectangle

			float x0=0, y0=0;
			switch(titlePosition)
			{
				case TitlePosition.Bottom:
					x0 = rect.X + rect.Width/2 - width/2;
					y0 = (float)(rect.Y + rect.Height - rectangleBottomMargin*pt2W - height);
					break;
				case TitlePosition.Top:
					x0 = rect.X + rect.Width/2 - width/2;
					y0 = (float)(rect.Y + rectangleTopMargin*pt2W);
					break;
				case TitlePosition.LeftDownwards:
					x0 = (float)(rect.X + rectangleBottomMargin*pt2W);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleLeftMargin*pt2W - height/2);
					break;
				case TitlePosition.LeftUpwards:
					x0 = (float)(rect.X + rectangleTopMargin*pt2W);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleLeftMargin*pt2W - height/2);
					break;
				case TitlePosition.RightDownwards:
					x0 = (float)(rect.X + rect.Width - rectangleTopMargin*pt2W - width);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleRightMargin*pt2W - height/2);
					break;
				case TitlePosition.RightUpwards:
					x0 = (float)(rect.X + rect.Width - rectangleTopMargin*pt2W - width);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleRightMargin*pt2W - height/2);
					break;
			};

			return new RectangleF(x0,y0,width,height);
		}

		internal void Render(Graphics G, Rectangle rect)
		{
			// Scaling factor
			double pt2W = OwningChart.FromPointToTarget*OwningChart.ReducedSamplingStep;

			// Get the path
			
			GraphicsPath path = new GraphicsPath(FillMode.Alternate);
			float scaledSize = (float)(textFont.SizeInPoints*pt2W);
			path.AddString(text,textFont.FontFamily,(int)textFont.Style,scaledSize,new PointF(0,0),StringFormat.GenericTypographic);

			// Get extremes and shift
			
			float xMin = float.MaxValue;
			float yMin = float.MaxValue;
			float xMax = float.MinValue;
			float yMax = float.MinValue;
			float x,y;
			PointF[] pts = path.PathPoints;
			byte[] types = path.PathTypes;

			int i;
			for (i=0;i<pts.Length;i++)
			{
				xMin = Math.Min(xMin,pts[i].X);
				xMax = Math.Max(xMax,pts[i].X);
				yMin = Math.Min(yMin,pts[i].Y);
				yMax = Math.Max(yMax,pts[i].Y);
			}
			for(i=0; i<pts.Length;i++)
			{
				pts[i].X -= xMin;
				pts[i].Y -= yMin;
			}
			float dxTxt = xMax - xMin;
			float dyTxt = yMax - yMin;

			// Coordinates of the minimum rectangle, including text margins (between 
			//	text and the rectangle borders)
			xMin -= (float)(textLeftMargin*pt2W);
			yMin -= (float)(textTopMargin*pt2W);
			xMax += (float)(textRightMargin*pt2W);
			yMax += (float)(textBottomMargin*pt2W);
			float width = xMax - xMin;
			float height = yMax - yMin;

			// Real rectangle dimensions
			SizeF rectangleSize = this.rectangleSize;

			if(titlePosition == TitlePosition.Top || titlePosition == TitlePosition.Bottom)
			{
				if(rectangleSize.IsEmpty)
					rectangleSize = new SizeF(10000,0);
				float sx = rectangleSize.Width;
				sx = (float)Math.Min(sx,rect.Width - (rectangleLeftMargin + rectangleRightMargin)*pt2W);
				width = Math.Max(width,sx);
				float sy = rectangleSize.Height;
				sy = (float)Math.Min(sy,rect.Height - (rectangleTopMargin + rectangleBottomMargin)*pt2W);
				height = Math.Max(height,sy);
			}
			else
			{
				if(rectangleSize.IsEmpty)
					rectangleSize = new SizeF(0,10000);
				float sx = rectangleSize.Height;
				sx = (float)Math.Min(sx,rect.Height - (rectangleLeftMargin + rectangleRightMargin)*pt2W);
				width = Math.Max(width,sx);
				float sy = rectangleSize.Width;
				sy = (float)Math.Min(sy,rect.Width - (rectangleTopMargin + rectangleBottomMargin)*pt2W);
				height = Math.Max(height,sy);
			}

			// Shift text according to the reference point


			TextReferencePoint eRefPoint = refPoint;
			if(eRefPoint == TextReferencePoint.Default)
			{
				switch(titlePosition)
				{
					case TitlePosition.Bottom:
						eRefPoint = TextReferencePoint.CenterBottom;
						break;
					case TitlePosition.Top:
						eRefPoint = TextReferencePoint.CenterTop;
						break;
					case TitlePosition.LeftDownwards:
						eRefPoint = TextReferencePoint.CenterBottom;
						break;
					case TitlePosition.LeftUpwards:
						eRefPoint = TextReferencePoint.CenterTop;
						break;
					case TitlePosition.RightDownwards:
						eRefPoint = TextReferencePoint.CenterTop;
						break;
					case TitlePosition.RightUpwards:
						eRefPoint = TextReferencePoint.CenterBottom;
						break;
				}
			}
			float shiftX = 0, shiftY = 0;
			switch(eRefPoint)
			{
				case TextReferencePoint.Center:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
				case TextReferencePoint.CenterBottom:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(height - textBottomMargin*pt2W - dyTxt);
					break;
				case TextReferencePoint.CenterTop:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(textTopMargin*pt2W);
					break;
				case TextReferencePoint.LeftCenter:
					shiftX = (float)(textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
				case TextReferencePoint.LeftBottom:
					shiftX = (float)(textRightMargin*pt2W);
					shiftY = (float)(height - textBottomMargin*pt2W - dyTxt);
					break;
				case TextReferencePoint.LeftTop:
					shiftX = (float)(textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W);
					break;
				case TextReferencePoint.RightCenter:
					shiftX = (float)(width - dxTxt - textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
				case TextReferencePoint.RightBottom:
					shiftX = (float)(width - dxTxt - textRightMargin*pt2W);
					shiftY = (float)(height - textBottomMargin*pt2W - dyTxt);
					break;
				case TextReferencePoint.RightTop:
					shiftX = (float)(width - dxTxt - textRightMargin*pt2W);
					shiftY = (float)(textTopMargin*pt2W);
					break;
				default:
					shiftX = (float)(textRightMargin*pt2W + (width-textRightMargin*pt2W-textLeftMargin*pt2W-dxTxt)/2);
					shiftY = (float)(textTopMargin*pt2W + (height-textTopMargin*pt2W-textBottomMargin*pt2W-dyTxt)/2);
					break;
			}

			for (i=0;i<pts.Length;i++)
			{
				pts[i].X += shiftX;
				pts[i].Y += shiftY;
			}

			// Process orientation 

			if(titlePosition == TitlePosition.LeftDownwards || titlePosition == TitlePosition.RightDownwards)
			{
				x = width; width = height; height = x;
				for (i=0;i<pts.Length;i++)
				{
					x = width-pts[i].Y;
					y = pts[i].X;
					pts[i].X = x;
					pts[i].Y = y;
				}
			}
			else if(titlePosition == TitlePosition.LeftUpwards || titlePosition == TitlePosition.RightUpwards)
			{
				x = width; width = height; height = x;
				for (i=0;i<pts.Length;i++)
				{
					x = pts[i].Y;
					y = height-pts[i].X;
					pts[i].X = x;
					pts[i].Y = y;
				}
			}
		
			width += (float) outlineWidth;
			height += (float) outlineWidth;

			// Graphics object

			G.SmoothingMode = SmoothingMode.AntiAlias;

			// Drawing rectangle

			float x0=0, y0=0;
			switch(titlePosition)
			{
				case TitlePosition.Bottom:
					x0 = rect.X + rect.Width/2 - width/2;
					y0 = (float)(rect.Y + rect.Height - rectangleBottomMargin*pt2W - height);
					break;
				case TitlePosition.Top:
					x0 = rect.X + rect.Width/2 - width/2;
					y0 = (float)(rect.Y + rectangleTopMargin*pt2W);
					break;
				case TitlePosition.LeftDownwards:
					x0 = (float)(rect.X + rectangleBottomMargin*pt2W);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleLeftMargin*pt2W - height/2);
					break;
				case TitlePosition.LeftUpwards:
					x0 = (float)(rect.X + rectangleTopMargin*pt2W);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleLeftMargin*pt2W - height/2);
					break;
				case TitlePosition.RightDownwards:
					x0 = (float)(rect.X + rect.Width - rectangleTopMargin*pt2W - width);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleRightMargin*pt2W - height/2);
					break;
				case TitlePosition.RightUpwards:
					x0 = (float)(rect.X + rect.Width - rectangleTopMargin*pt2W - width);
					y0 = (float)(rect.Y + rect.Height/2 - rectangleRightMargin*pt2W - height/2);
					break;
			};

			if(backgroundRectangleVisible)
			{
				float borderShadeWidthPxl = (float)(borderShadeWidthPts*pt2W);
				// Shadow
				if(borderShadeWidthPts > 0)
				{
					Brush b = new SolidBrush(borderShadeColor);
					G.FillRectangle(b,x0+borderShadeWidthPxl,y0+borderShadeWidthPxl,width,height);
					b.Dispose();
				}
				// Interior
				GradientStyle gs = new GradientStyle("bgd",backgroundGradientKind,backgroundColor1,backgroundColor2);
				Rectangle rec = new Rectangle((int)x0,(int)y0,(int)width,(int)height);
				Brush brush = gs.CreateBrush(rect);
				G.FillRectangle(brush,x0,y0,width,height);

				// Border
				if(backgroundBorderVisible)
				{
					Pen p = new Pen(borderColor, (float)borderWidth);
					p.DashStyle = borderDashStyle;
					G.DrawRectangle(p,x0,y0,width,height);
				}
			}

			// Text
			
			Brush tBrush;

			if(textShadeWidthPts > 0)
			{	// Shadow
				float p = (float)(textShadeWidthPts*pt2W);
				for (i=0;i<pts.Length;i++)
				{
					pts[i].X += x0 + p ;
					pts[i].Y += y0 + p;
				}
				path = new GraphicsPath(pts,types);
				tBrush = new SolidBrush(TextShadowColor);
				G.FillPath(tBrush,path);
				for (i=0;i<pts.Length;i++)
				{
					pts[i].X -= p ;
					pts[i].Y -= p;
				}
			}
			else
			{
				for (i=0;i<pts.Length;i++)
				{
					pts[i].X += x0;
					pts[i].Y += y0;
				}
			}

			// Text face
			Color fColor = foreColor;
			if(foreColor.A == 0)
				fColor = OwningChart.Palette.TitleFontColor;
			path = new GraphicsPath(pts,types);
			tBrush = new SolidBrush(fColor);
			G.FillPath(tBrush,path);

			// Outline
			if(outlineWidth > 0)
			{
				Pen pen = new Pen(outlineColor,(float)outlineWidth);
				G.DrawPath(pen,path);
			}

		}

		#endregion

		#region --- Browsing Control ---

		private static string[] PropertiesOrder = new string[] 
		{
			"Text",
			"Font",
			"ForeColor",
			"OutlineColor",
			"OutlineWIdth",
			"TextShadeColor",
			"TextShadeWidthPts",
			"TextBottomMargin",
			"TextLeftMargin",
			"TextTopMargin",
			"TextRightMargin",
			"RefPoint",

			"BsckgroundVisible",
			"RectangleSize",
			"Position",
			"GradientKind",
			"BackgroundColor1",
			"BackgroundColor2",
			"BorderVisible",
			"BorderWidth",
			"BorderColor",
			"BorderShadeColor",
			"BorderDashStyle",
			"RectangleBottomMargin",
			"RectangleLeftMargin",
			"RectangleTopMargin",
			"RectangleRightMargin"
		};


		#endregion
	}

	/// <summary>
	/// Collection of <see cref="ChartTitle"/> objects.
	/// </summary>
	public sealed class ChartTitleCollection : CollectionWithType
	{
		internal ChartTitleCollection() : base(typeof(ChartTitle)) 
		{ }

		/// <summary>
		/// Gets or sets the <see cref="ChartTitle"/> object at the specified zero-based index.
		/// </summary>
		/// <param name="i">The zero-based index of the <see cref="ChartTitle"/> to find.</param>
		public ChartTitle this[int i] 
		{
			get { return (ChartTitle)(base[i]); }
			set { base[i] = value; }
		}

		internal void Render(Graphics G, Rectangle rec)
		{
			foreach (ChartTitle title in this)
				title.Render(G,rec);
		}
	}
}
