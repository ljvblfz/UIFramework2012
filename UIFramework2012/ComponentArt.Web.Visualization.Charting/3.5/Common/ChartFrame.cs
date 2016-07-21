using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Specifies the shade position.
	/// </summary>
	internal enum ChartFrameShadePosition
	{
		/// <summary>
		/// No shade.
		/// </summary>
		NoShade,
		/// <summary>
		/// Shade is installed at the right-bottom of the chart.
		/// </summary>
		RightBottom,
		/// <summary>
		/// Shade is installed around the chart.
		/// </summary>
		Centered,
		/// <summary>
		/// Shade is installed at the left-top of the chart.
		/// </summary>
		LeftTop
	}

	/// <summary>
	/// Specifies the kind of the frame.
	/// </summary>
	public enum ChartFrameKind
	{
		/// <summary>
		/// No frame
		/// </summary>
		NoFrame,
		/// <summary>
		/// Thin twodimensional frame
		/// </summary>
		Thin2DFrame,
		/// <summary>
		/// Medium twodimensional frame
		/// </summary>
		Medium2DFrame,
		/// <summary>
		/// Thick twodimensional frame
		/// </summary>
		Thick2DFrame,
		/// <summary>
		/// Thick threedimensional frame
		/// </summary>
		Triple2DFrame,
		/// <summary>
		/// Thick threedimensional frame
		/// </summary>
		Custom2DFrame,
		/// <summary>
		/// Thin threedimensional frame
		/// </summary>
		Thin3DFrame,
		/// <summary>
		/// Medium threedimensional frame
		/// </summary>
		Medium3DFrame,
		/// <summary>
		/// Thick threedimensional frame
		/// </summary>
		Thick3DFrame
	}

	/// <summary>
	/// Specifies where the frame text (chart title) is located.
	/// </summary>
	public enum FrameTextPosition
	{
		/// <summary>
		/// No text on the frame.
		/// </summary>
		NoTitle,
		/// <summary>
		/// The text is on top of the frame.
		/// </summary>
		Top,
		/// <summary>
		/// The text is at the bottom of the frame.
		/// </summary>
		Bottom,
		/// <summary>
		/// The text is on the left side of the frame.
		/// </summary>
		Left,
		/// <summary>
		/// The text is on the right side of the frame.
		/// </summary>
		Right
	}
	// =============================================================================================================
	//	ChartFrame Class
	// =============================================================================================================


	/// <summary>
	/// Represents the chart frame.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class ChartFrame
	{
		private ChartBase owningChart;

		const bool	_borderLineDefault = false;
		const bool	_roundTopDefault = false;
		const bool	_roundBottomDefault = false;
		const int	_extraDefault = 0;
		const int	_dropShadeWidthDefault = 10;

		private Rectangle rect;
		private bool		roundTop = _roundTopDefault;
		private bool		roundBottom = _roundBottomDefault;
		private int			cornerRadius = 20;
		private bool		borderLine = _borderLineDefault;
		private ChartFrameKind kind = ChartFrameKind.NoFrame;

		private ChartRectangularFrameInner	innerFrame;
		private ChartFrameDropShade			dropShade;
		private ChartShadeFrameInner		innerShade;

		private FrameTextPosition	textPosition = FrameTextPosition.Top;
		private StringAlignment		textAlignment = StringAlignment.Near;
		private float				scaleFactor = 1.0f;

		private Color[]				lineColors = new Color[] { Color.Gray,Color.White };
		private int[]				lineWidths = new int  [] { 1, 1 };
		private Color				formColor = Color.White;

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartFrame"/> class.
		/// </summary>
		public ChartFrame() : this (new Rectangle(0,0,0,0)) { }

		internal ChartBase OwningChart 
		{
			get { return owningChart; } 
			set 
			{ 
				owningChart = value; 
			} 
		}

		internal ChartFrame(Rectangle rect) 
		{
			this.rect = rect;
			dropShade = new ChartFrameDropShade();
			innerShade = new ChartShadeFrameInner();
			innerFrame = new ChartRectangularFrameInner();
			innerFrame.OwningChart = owningChart;
			innerShade.OwningChart = owningChart;
			innerFrame.ExtraSpaceBottom = 0;
			innerFrame.ExtraSpaceTop = 0;
			innerFrame.ExtraSpaceLeft = 0;
			innerFrame.ExtraSpaceRight = 0;
		}

		#endregion


		/// <summary>
		/// Creates a custom frame from specified colos and widths.
		/// </summary>
		/// <param name="colors">Colors in the frame.</param>
		/// <param name="widths">Widths of the <paramref name="colors" /> in pixels.</param>
		/// <remarks>
		/// The frame is built using rectangles of specified <paramref name="colors" /> and their corresponding <paramref name="widths" /> that will be located around the chart.
		/// </remarks>
		public void SetCustomFrame(Color[] colors, int[] widths) 
		{
			lineWidths = widths;
			lineColors = colors;
			kind = ChartFrameKind.Custom2DFrame;
		}

		#region --- Properties ---

		/// <summary>
		/// Gets or sets the chart frame kind.
		/// </summary>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
#if __BUILDING_CRI_DESIGNER__ || __BUILDING_CRI__
        [DefaultValue(ChartFrameKind.Thin2DFrame)]
#else
        [DefaultValue(ChartFrameKind.NoFrame)]
#endif
        [Description("The overall frame style.")]
		public ChartFrameKind FrameKind			{ get { return kind; }			set { kind = value; } }

		[NotifyParentProperty(true)]
		[DefaultValue(typeof(Color),"White")]
		[Description("Background color of the form that the control belongs to.")]
		public Color	FormColor			{ get { return formColor;	}	set { formColor = value; } }

		/// <summary>
		/// Gets or sets the <see cref="System.Drawing.Color"/> of the frame.
		/// </summary>
		[NotifyParentProperty(true)]
#if __BUILDING_CRI_DESIGNER__ || __BUILDING_CRI__
        [DefaultValue(typeof(Color), "Silver")]
#else
        [DefaultValue(typeof(Color), "Gray")]
#endif
        [Description("The color used to draw the frame")]
		public Color	FrameColor			{ get { return innerFrame.FrameColor;	}	set { innerFrame.FrameColor = value; } }
		
		/// <summary>
		/// Gets or sets the amount of extra frame space on the top in points.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		[Description("The amount of extra frame space on the top in points.")]
		public int		ExtraSpaceTop		{ get { return innerFrame.ExtraSpaceTop;	}	set { innerFrame.ExtraSpaceTop = value; } }
		/// <summary>
		/// Gets or sets the amount of extra frame space on the bottom in points.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		[Description("The amount of extra frame space on the bottom in points.")]
		public int		ExtraSpaceBottom	{ get { return innerFrame.ExtraSpaceBottom; }	set { innerFrame.ExtraSpaceBottom = value; } }
		/// <summary>
		/// Gets or sets the amount of extra frame space on the left in points.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		[Description("The amount of extra frame space on the left in points.")]
		public int		ExtraSpaceLeft		{ get { return innerFrame.ExtraSpaceLeft;	}	set { innerFrame.ExtraSpaceLeft = value; } }
		/// <summary>
		/// Gets or sets the amount of extra frame space on the right in points.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		[Description("The amount of extra frame space on the right in points.")]
		public int		ExtraSpaceRight		{ get { return innerFrame.ExtraSpaceRight;	}	set { innerFrame.ExtraSpaceRight = value; } }

		/// <summary>
		/// Gets or sets a value that indicates whether border lines are shown.
		/// </summary>
		[Description("Indicates represents whether border lines are shown.")]
		[DefaultValue(_borderLineDefault)]
		[NotifyParentProperty(true)]
		public bool		ShowBorderLines			{ get { return borderLine; }	set { borderLine = value; } }

		/// <summary>
		/// Gets or sets a value that indicates whether top corners are rounded.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_roundTopDefault)]
		[Description("Determines whether the top corners are rounded.")]
		public bool		RoundTopCorners		
		{
			get { return roundTop; }		
			set 
			{ 
				roundTop = value; 
				if(innerFrame!= null) 
					innerFrame.RoundTopCorners = value; 
				if(dropShade != null)
					dropShade.RoundTopCorners = value;
			} 
		}
		
		/// <summary>
		/// Gets or sets a value that indicates whether bottom corners are rounded.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_roundBottomDefault)]
		[Description("Determines whether the bottom corners are rounded.")]
		public bool		RoundBottomCorners	
		{ 
			get { return roundBottom; }	
			set 
			{ 
				roundBottom = value;
				if(innerFrame!= null)
					innerFrame.RoundBottomCorners = value; 
				if(dropShade != null)
					dropShade.RoundBottomCorners = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a value that indicates whether the drop shade exists in this <see cref="ChartFrame"/> object.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[Description("Determines whether the drop shade exists")]
		public bool DropShade
		{ 
			get { return dropShade.ShadePosition == ChartFrameShadePosition.RightBottom; }		
			set 
			{ 
				if(value)
					dropShade.ShadePosition = ChartFrameShadePosition.RightBottom;
				else
					dropShade.ShadePosition = ChartFrameShadePosition.NoShade;
			}
		}
		
		/// <summary>
		/// Gets or sets a value that indicates whether the inner shade exists in this <see cref="ChartFrame"/> object.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[Description("Determines whether the inner shade exists")]
		public bool InnerShade
		{ 
			get { return innerShade.ShadePosition == ChartFrameShadePosition.LeftTop; }		
			set 
			{ 
				if(value)
					innerShade.ShadePosition = ChartFrameShadePosition.LeftTop;
				else
					innerShade.ShadePosition = ChartFrameShadePosition.NoShade;
			}
		}

		/// <summary>
		/// Gets or sets the shade width in this <see cref="ChartFrame"/> object.
		/// </summary>
		[NotifyParentProperty(true)]
		[DefaultValue(_dropShadeWidthDefault)]
		[Description("The width of the shade in pixels")]
		public int		ShadeWidth					
		{
			get { return dropShade.ShadeWidth; }
			set 
			{
				dropShade.ShadeWidth = value; 
				innerShade.ShadeWidth = value; 
			}
		}

		#endregion

		#region --- Old Properties ---
		
		internal Rectangle Rectangle { get { return rect; } set { rect = value; } }

		internal FrameTextPosition	TextPosition	{ get { return textPosition; }		set { textPosition = value; } }
		internal StringAlignment	TextAlignment	{ get { return textAlignment; }		set { textAlignment = value; } }
		internal bool				TextShade		{ get { return innerFrame.TextShade; } set { innerFrame.TextShade = value; } }
		/// <summary>
		/// Gets or sets the <see cref="System.Drawing.Font"/> of this <see cref="ChartFrame"/> object.
		/// </summary>
		[Description("Font of the frame text. ")]
		[NotifyParentProperty(true)]
		public Font					Font			{ get { return innerFrame.Font; } set { innerFrame.Font = value; } }
		internal Color				FontColor		{ get { return innerFrame.FontColor; } set { innerFrame.FontColor = value; } }
		internal string				Text			{ get { return innerFrame.Text; } set { innerFrame.Text = value; } }
		#endregion

		#region --- Internal Functions ---

		internal void SetFontColor(Color fontColor)
		{
			FontColor = fontColor;
		}

		internal void SetText(string text)
		{
			Text = text;
		}

		internal Rectangle InternalRectangle(Graphics g)
		{
			if(kind == ChartFrameKind.Thin2DFrame || 
				kind == ChartFrameKind.Medium2DFrame || 
				kind == ChartFrameKind.Thick2DFrame || 
				kind == ChartFrameKind.Custom2DFrame)
			{
				Rectangle r = ExternalRectangle();
				int left = r.Left;
				int top = r.Top;
				int width = r.Width;
				int height = r.Height;

				for(int i=0; i<lineWidths.Length;i++)
				{
					left   += lineWidths[i];	
					top    += lineWidths[i];	
					width  -= 2*lineWidths[i];	
					height -= 2*lineWidths[i];	
				}
				return new Rectangle(left,top,width,height);
			}
			else
			{
				if(innerFrame == null || kind == ChartFrameKind.NoFrame)
					return ExternalRectangle();
				UpdateLayout();
				innerFrame.Rectangle = ExternalRectangle();
				return innerFrame.InternalRectangle(g);
			}
		}

		internal void UpdateLayout()
		{
			Rectangle R = ExternalRectangle();

			// Drop Shade

			if(dropShade != null && dropShade.ShadePosition != ChartFrameShadePosition.NoShade)
			{
				dropShade.CornerRadius = cornerRadius;
				dropShade.Rectangle = R;
			}

			// Frame
			
			if(innerFrame != null && kind != ChartFrameKind.NoFrame)
			{
				switch(kind)
				{
					case ChartFrameKind.NoFrame:
						break;
					case ChartFrameKind.Thick3DFrame:
						innerFrame.ShadeWidth = (int)(5*scaleFactor);
						break;
					case ChartFrameKind.Medium3DFrame:
						innerFrame.ShadeWidth = (int)(2*scaleFactor);
						break;
					case ChartFrameKind.Thin3DFrame:
						innerFrame.ShadeWidth = 0;
						break;
				}

				innerFrame.RoundBottomCorners = RoundBottomCorners;
				innerFrame.RoundTopCorners = RoundTopCorners;
				innerFrame.CornerRadius = (int)(cornerRadius*scaleFactor);
				innerFrame.Rectangle = R;
				innerFrame.TextAlignment = TextAlignment;
				innerFrame.TextPosition = TextPosition;
			}
		}

		internal void SetScaleFactor(double scale)
		{
			scaleFactor = (float)scale;
			dropShade.SetScaleFactor(scaleFactor);
			innerShade.SetScaleFactor(scaleFactor);
			innerFrame.SetScaleFactor(scaleFactor);
		}

		internal void Render(Graphics g)
		{
			if(kind == ChartFrameKind.NoFrame)
				return;

			// Border and Outside Area

			float sf = ((int)(scaleFactor*8))*0.125f;

			GraphicsPath brd = ExternalBorder();

			if(borderLine)
				g.DrawPath(new Pen(formColor,1),brd);
			brd.AddRectangle(rect);
			g.FillPath(new SolidBrush(formColor),brd);
			g.DrawRectangle(new Pen(formColor),rect);

			Rectangle R = ExternalRectangle();

			// Drop Shade

			if(dropShade != null && dropShade.ShadePosition != ChartFrameShadePosition.NoShade)
			{
				dropShade.Rectangle = R;
				dropShade.CornerRadius = cornerRadius;
				dropShade.FormColor = formColor;
				dropShade.Render(g);
			}

			// Inner Shade

			innerShade.OwningChart = OwningChart;

			innerShade.RoundBottomCorners = RoundBottomCorners;
			innerShade.RoundTopCorners = RoundTopCorners;
			innerShade.CornerRadius = (int)(cornerRadius*scaleFactor);
			innerShade.Rectangle = R;
			innerShade.TextAlignment = TextAlignment;
			innerShade.TextPosition = TextPosition;
			
			innerShade.Render(g);

			switch(kind)
			{
				case ChartFrameKind.Thin2DFrame:
					RenderSimpleFrame(g,new int[] { 1,1 } );
					return;
				case ChartFrameKind.Medium2DFrame:
					RenderSimpleFrame(g,new int[] { 2,2 } );
					return;
				case ChartFrameKind.Thick2DFrame:
					RenderSimpleFrame(g,new int[] { 4,1 } );
					return;
				case ChartFrameKind.Triple2DFrame:
					RenderSimpleFrame(g,new int[] { 1,1,2,1 } );
					return;
				case ChartFrameKind.Custom2DFrame:
					RenderSimpleFrame(g);
					return;
			};			
			
			if(innerFrame != null && kind != ChartFrameKind.NoFrame)
			{
				innerFrame.OwningChart = OwningChart;
				switch(kind)
				{
					case ChartFrameKind.Thick3DFrame:
						innerFrame.ShadeWidth = 5;
						innerFrame.ShowSmoothLines = true;
						break;
					case ChartFrameKind.Medium3DFrame:
						innerFrame.ShadeWidth = 2;
						innerFrame.ShowSmoothLines = true;
						break;
					case ChartFrameKind.Thin3DFrame:
						innerFrame.ShadeWidth = 0;
						innerFrame.ShowSmoothLines = false;
						break;
				}

				innerFrame.RoundBottomCorners = RoundBottomCorners;
				innerFrame.RoundTopCorners = RoundTopCorners;
				innerFrame.CornerRadius = cornerRadius;
				innerFrame.Rectangle = R;
				innerFrame.TextAlignment = TextAlignment;
				innerFrame.TextPosition = TextPosition;
				innerFrame.ShowSharpLines = borderLine;
				innerFrame.Render(g);
			}

		}

		private void RenderSimpleFrame(Graphics g, int[] widths)
		{
			lineWidths = widths;
			lineColors = new Color[widths.Length];
			for(int i=0; i<lineColors.Length; i++)
			{
				if(i%2 == 0)
					lineColors[i] = OwningChart.Palette.FrameColor;
				else
					lineColors[i] = OwningChart.Palette.FrameSecondaryColor;
			}
			RenderSimpleFrame(g);
		}

		private void RenderSimpleFrame(Graphics g)
		{
			int i;

			Color[] colors = lineColors;
			int[] widths = lineWidths;
			if(colors == null)
				colors = new Color[] { Color.Black };
			if(widths == null)
			{
				widths = new int[] { colors.Length };
				for(i=0; i<widths.Length;i++)
					widths[i] = 2;
			}
			int radius = cornerRadius;
			int minRadius = 0;
			for(i=0;i<widths.Length;i++)
				minRadius += widths[i];
			minRadius += 5;
			radius = Math.Max(radius,minRadius);

			PointF[] pts = new PointF[50];
			int nPts;
			ChartFrameEffect.CreateClosedPolygon(ExternalRectangle(),roundTop,roundBottom,radius*scaleFactor,pts,out nPts);
			
			int nLines = Math.Min(colors.Length,widths.Length);
			float growth = -widths[0]*0.5f;
			growth = 0;
			for(i=0;i<nLines;i++)
			{
				GraphicsPath path = ChartFrameEffect.CreatePath(pts,nPts,0,0,growth);
				if(i<nLines-1)
					growth = growth - (widths[i] + widths[i+1])*0.5f;
				Pen pen = new Pen(colors[i],widths[i]);
				g.DrawPath(pen,path);
			}

			
		}
		#endregion

		#region --- Private Functions ---
		
		private GraphicsPath ExternalBorder()
		{
			PointF[] pts = new PointF[50];
			int nPts;
			ChartFrameEffect.CreateClosedPolygon(ExternalRectangle(),roundTop,roundBottom,cornerRadius*scaleFactor,pts,out nPts);
			return ChartFrameEffect.CreatePath(pts,nPts,0,0,0);
		}

		private Rectangle ExternalRectangle()
		{
			int x0 = rect.X;
			int x1 = rect.X + rect.Width;
			int y0 = rect.Y;
			int y1 = rect.Y + rect.Height;
			if(dropShade != null && dropShade.ShadePosition != ChartFrameShadePosition.NoShade)
			{
				if(dropShade.ShadePosition != ChartFrameShadePosition.LeftTop)
				{
					x1 -= (int)(dropShade.ShadeWidth*scaleFactor);
					y1 -= (int)(dropShade.ShadeWidth*scaleFactor);
				}
				if(dropShade.ShadePosition != ChartFrameShadePosition.RightBottom)
				{
					x0 += (int)(dropShade.ShadeWidth*scaleFactor);
					y0 += (int)(dropShade.ShadeWidth*scaleFactor);
				}
			}
			if(x0 == 0)
				x0 = 1;
			if(y0 == 0)
				y0 = 1;

			return new Rectangle(x0,y0,x1-x0,y1-y0);
		}
		#endregion

		#region --- Serialization and Browsing Control ---

		private static string[] PropertiesOrder = new string[]
			{
				"FrameKind",
				"FrameColor",
				"ShowBorderLines",

				"DropShade",
				"InnerShade",
				"ShadeWidth",

				"RoundTopCorners",
				"RoundBottomCorners",
  
				"ExtraSpaceBottom",
				"ExtraSpaceLeft",
				"ExtraSpaceTop",
				"ExtraSpaceRight"
			};
		private bool IsSimple 
		{ 
			get 
			{ 
				return 
					FrameKind == ChartFrameKind.Thick2DFrame || 
					FrameKind == ChartFrameKind.Medium2DFrame || 
					FrameKind == ChartFrameKind.Thin2DFrame || 
					FrameKind == ChartFrameKind.Custom2DFrame;
			} 
		}

		private bool ShouldBrowseFrameColor()			{ return FrameKind != ChartFrameKind.NoFrame && !IsSimple; }
		private bool ShouldBrowseShowBorderLines()		{ return FrameKind != ChartFrameKind.NoFrame && !IsSimple; }
		private bool ShouldBrowseDropShade()			{ return FrameKind != ChartFrameKind.NoFrame; }
		private bool ShouldBrowseInnerShade()			{ return FrameKind != ChartFrameKind.NoFrame; }
		private bool ShouldBrowseShadeWidth()			{ return FrameKind != ChartFrameKind.NoFrame && (ShouldBrowseDropShade() || ShouldBrowseInnerShade()); }
		private bool ShouldBrowseRoundTopCorners()		{ return FrameKind != ChartFrameKind.NoFrame; }
		private bool ShouldBrowseRoundBottomCorners()	{ return FrameKind != ChartFrameKind.NoFrame; }
		private bool ShouldBrowseExtraSpaceBottom()		{ return FrameKind != ChartFrameKind.NoFrame && !IsSimple; }
		private bool ShouldBrowseExtraSpaceLeft()		{ return FrameKind != ChartFrameKind.NoFrame && !IsSimple; }
		private bool ShouldBrowseExtraSpaceTop()		{ return FrameKind != ChartFrameKind.NoFrame && !IsSimple; }
		private bool ShouldBrowseExtraSpaceRight()		{ return FrameKind != ChartFrameKind.NoFrame && !IsSimple; }

		private bool ShouldBrowseLineColors()			{ return FrameKind == ChartFrameKind.Custom2DFrame; }
		private bool ShouldBrowseLineWidths()			{ return FrameKind == ChartFrameKind.Custom2DFrame; }
		

		internal void Serialize(XmlCustomSerializer S)
		{
			if(FrameKind == ChartFrameKind.NoFrame && !S.Reading)	// if writing noframe
				return;
			S.Comment("    ===========    ");
			S.Comment("    Chart Frame    ");
			S.Comment("    ===========    ");

			S.AttributeProperty(this,"FrameKind");
			S.AttributeProperty(this,"ShowBorderLines");
			S.AttributeProperty(this,"RoundTopCorners");
			S.AttributeProperty(this,"RoundBottomCorners");
			S.AttributeProperty(this,"TextPosition");
			S.AttributeProperty(this,"TextAlignment");
			S.AttributeProperty(this,"TextShade");
			S.AttributeProperty(this,"Font");
			S.AttributeProperty(this,"FontColor");

			if((innerFrame != null || S.Reading) && S.BeginTag("Border"))
			{
				innerFrame.Serialize(S);
				S.EndTag();
			}
		}
		#endregion
	}
}
