#define TEST_LAYOUT_
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents the rectangular region within the target bitmap. The owning <see cref="SeriesBase"/>
	/// of this area is rendered within the specified region.
	/// </summary>
	/// <remarks>
	///		The <see cref="TargetArea"/> concept is used to implement "MultiArea" composition kind.
	/// </remarks>
	[Serializable]
	public class TargetArea : ChartObject, IDisposable
	{
		private SeriesBase	series;
		// Relative origin and size parameters within parent area
		private double xr = 0.0, yr = 0.0, wr = 1.0, hr = 1.0;

		private Mapping	mapping = new Mapping();
		private Vector3D		viewDirectionLinear;
		private Vector3D		viewDirectionRadial;
		private bool			lastTimeWasPieDoughnut = false;
		private bool			lastTimeWasRadar = false;
		private ProjectionKind  projectionKind = ProjectionKind.CentralProjection;

		// Titles
		private ChartTitleCollection titles;

		// Legend
		private Legend			legend = null;
		private LegendCollection secondaryLegends;

		/// <summary>
		/// Initializes a new instance of <see cref="TargetArea"/> class.
		/// </summary>
		public TargetArea() 
		{
			// Mapping object initialization

			viewDirectionLinear = Mapping.defaultViewDirectionLinear;
			viewDirectionRadial = Mapping.defaultViewDirectionRadial;
			titles = new ChartTitleCollection();
			titles.SetOwner(this);
			secondaryLegends = new LegendCollection(this);
		}

		internal void Dispose(bool disposing) 
		{
			if (disposing)
			{
				if (legend != null) 
				{
					legend.Dispose();
					legend = null;
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

		internal SeriesBase Series { get { return series; } set { series = value; SetOwner(value); } }		
		internal Mapping Mapping { get { return mapping; } set { mapping = value; mapping.Chart = this.OwningChart; } }	
		/// <summary>
		/// Gets the collection of chart titles belonging to this <see cref="TargetArea"/> object.
		/// </summary>
		public ChartTitleCollection Titles { get { return titles; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="TargetArea"/> object is two-dimensional.
		/// </summary>
		public bool IsTwoDimensional	{ get { return mapping.Kind == ProjectionKind.TwoDimensional; } }
		/// <summary>
		/// Gets the <see cref="Legend"/> of this <see cref="TargetArea"/> object.
		/// </summary>
		[DefaultValue(null)]
		public Legend Legend 
		{ 
			get 
			{ 
				if(OwningChart.InSerialization)
					return legend; 
				if(legend == null)
				{
					legend = new Legend();
					legend.OwningTargetArea = this;
				}
				return legend;
			}
			set 
			{ 
				legend = value;
				if(legend != null)
					legend.OwningTargetArea = this;
			} 
		}

		/// <summary>
		/// Gets the <see cref="Legend"/> of this <see cref="TargetArea"/> object.
		/// </summary>
		[DefaultValue(null)]
		public LegendCollection SecondaryLegends
		{
			get
			{
				if(OwningChart.InSerialization && secondaryLegends.Count == 0)
					return null;
				else
					return secondaryLegends;
			}
			set
			{
				secondaryLegends = value;
			}
		}

		#region --- Margins ---

			/// <summary>
			/// Gets or sets the <see cref="Margins"/> of this <see cref="TargetArea"/> object.
			/// </summary>
		public MappingMargins Margins		{ get { return Mapping.Margins; } set { Mapping.Margins = value; } }
		
		#endregion

		/// <summary>
		/// Sets this <see cref="TargetArea"/> relative to the parent area.
		/// </summary>
		/// <param name="xRelative">x-coordinate of the upper-left corner of the target area to draw in relative units (0-0.99).</param>
		/// <param name="yRelative">y-coordinate of the upper-left corner of the target area to draw in relative units (0-0.99).</param>
		/// <param name="widthRelative">Width of the target area in relative units.</param>
		/// <param name="heightRelative">Height of the target area in relative units.</param>
		public void SetParameters(double xRelative, double yRelative, 
			double widthRelative, double heightRelative)
		{
			xr = xRelative;
			yr = yRelative;
			wr = widthRelative;
			hr = heightRelative;
		}
		internal void GetEffectiveParameters(out double xRelative, out double yRelative, 
			out double widthRelative, out double heightRelative)
		{
			GetEffectiveParameters(out xRelative, out yRelative, out widthRelative, out heightRelative, 1.0);
		}

		internal void GetEffectiveParameters(out double xRelative, out double yRelative, 
			out double widthRelative, out double heightRelative, double marginFraction)
		{	// MarginFraction is part of margin left outside parameters
			// marginFraction = 0 - outer parameters
			// marginFraction = 1 - full margins used

			// Parameters of the target rectangle not including margines
			double xr = Math.Max(0,Math.Min(0.99,this.xr));
			double yr = Math.Max(0,Math.Min(0.99,this.yr));
			double wr = Math.Min(this.wr,1.0-xr);
			double hr = Math.Min(this.hr,1.0-yr);

			double xp = 0.0, yp = 0.0, wp = 1.0, hp = 1.0;
			TargetArea parentArea = OwningTargetArea;
			if(parentArea != null)
				parentArea.GetEffectiveParameters(out xp,out yp,out wp,out hp);
			
			// Outer rectangle, enclosing the margine area

			xRelative = xp + xr*wp;
			yRelative = yp + yr*hp;
			widthRelative = wr*wp;
			heightRelative = hr*hp;
			
			if(marginFraction < 0)
				return;

			// Inner rectangle, inside the margine area
			xRelative += Margins.Left*0.01*widthRelative*marginFraction;
			yRelative += Margins.Top*0.01*heightRelative*marginFraction;
			widthRelative *= (1.0-marginFraction*(Margins.Left*0.01+Margins.Right*0.01));
			heightRelative *= (1.0-marginFraction*(Margins.Top*0.01+Margins.Bottom*0.01));
		}

		internal void GetParametersReducedByTitles(ref double xRelative, ref double yRelative, 
			ref double widthRelative, ref double heightRelative)
		{
			Size sz = OwningChart.TargetSize;
			Rectangle areaRectangle = EffectiveOuterTarget;
			double x0 = xRelative*sz.Width;
			double y0 = yRelative*sz.Height;
			double x1 = x0 + widthRelative*sz.Width;
			double y1 = y0 + heightRelative*sz.Height;

			for(int i=0; i<Titles.Count; i++)
			{
				ChartTitle title = Titles[i];
				RectangleF rect = title.ComputeRectangle(areaRectangle);
				switch(title.Position)
				{
					case TitlePosition.Top:
						y0 = rect.Y + rect.Height;
						break;
					case TitlePosition.LeftDownwards:
					case TitlePosition.LeftUpwards:
						x0 = rect.X + rect.Width;
						break;
					case TitlePosition.RightDownwards:
					case TitlePosition.RightUpwards:
						x1 = rect.X;
						break;
					case TitlePosition.Bottom:
						y1 = rect.Y;
						break;
					default:break;
				}
			}
		
			xRelative = x0/sz.Width;
			yRelative = y0/sz.Height;
			widthRelative = (x1-x0)/sz.Width;
			heightRelative = (y1-y0)/sz.Height;
		}

		internal void GetParametersReducedByLegends(ref double xRelative, ref double yRelative, 
			ref double widthRelative, ref double heightRelative)
		{
			ArrayList leg = new ArrayList();

			if(Legend.Visible && !Legend.SharesChartArea)
				leg.Add(Legend);
			if(secondaryLegends != null)
			{
				foreach(Legend l in secondaryLegends)
				{
					if(l.Visible && !l.SharesChartArea)
						leg.Add(l);
				}
			}

			if(leg.Count == 0)
				return;

			// Reducing output rectangle
			double x, y , w, h;
			GetEffectiveOuterParameters(out x, out y, out w, out h);
			//GetParametersReducedByTitles(ref x,ref y,ref w,ref h);
			double ocWidth = OwningChart.TargetSize.Width;
			double ocHeight = OwningChart.TargetSize.Height;
			double x0 = x*ocWidth;
			double y0 = y*ocHeight;
			double x1 = x0 + w*ocWidth;
			double y1 = y0 + h*ocHeight;

			foreach(ComponentArt.Web.Visualization.Charting.Legend l in leg)
			{
				Rectangle rec = l.GetRectangle();
				switch(l.LegendPosition)
				{
					case LegendPositionKind.BottomCenter:
						y1 = Math.Min(y1,rec.Y);
						break;
					case LegendPositionKind.BottomLeft:
						if(rec.Width > rec.Height)
							y1 = Math.Min(y1,rec.Y);
						else
							x0 = Math.Max(x0,rec.Left + rec.Width);
						break;
					case LegendPositionKind.BottomRight:
						if(rec.Width > rec.Height)
							y1 = Math.Min(y1,rec.Y);
						else
							x1 = Math.Min(x1,rec.Left);
						break;
					case LegendPositionKind.CenterLeft:
						x0 = Math.Max(x0,rec.Left + rec.Width);
						break;
					case LegendPositionKind.CenterRight:
						x1 = Math.Min(x1,rec.Left);
						break;
					case LegendPositionKind.TopCenter:
						y0 = Math.Max(y0,rec.Bottom);
						break;
					case LegendPositionKind.TopLeft:
						if(rec.Width > rec.Height)
							y0 = Math.Max(y0,rec.Bottom);
						else
							x0 = Math.Max(x0,rec.Left + rec.Width);
						break;
					case LegendPositionKind.TopRight:
						if(rec.Width > rec.Height)
							y0 = Math.Max(y0,rec.Bottom);
						else
							x1 = Math.Min(x1,rec.Left);
						break;				}
			}

			x0 = Math.Max(x0, xRelative*ocWidth);
			y0 = Math.Max(y0, yRelative*ocHeight);
			x1 = Math.Min(x1, (xRelative + widthRelative)*ocWidth);
			y1 = Math.Min(y1, (yRelative + heightRelative)*ocHeight);

			xRelative = x0/ocWidth;
			yRelative = (ocHeight-y1)/ocHeight;
			widthRelative = (x1-x0)/ocWidth;
			heightRelative = (y1-y0)/ocHeight;
		}

		internal void GetEffectiveOuterParameters(out double xRelative, out double yRelative, 
			out double widthRelative, out double heightRelative)
		{
			GetEffectiveParameters(out xRelative, out yRelative, out widthRelative, out heightRelative, 0.0);
		}

		internal TargetArea OwningTargetArea
		{
			get
			{
				if(series == null)
					return null;
				SeriesBase pSeries = series.OwningSeries;
				if(pSeries!=null)
					return pSeries.TargetArea;
				else
					return null;
			}
		}

		internal Rectangle EffectiveTarget
		{
			get
			{
				double x,y,w,h;
				GetEffectiveParameters(out x,out y,out w,out h);
				Size ts = Series.OwningChart.TargetSize;
				return new Rectangle((int)(ts.Width*x),(int)(ts.Height*y),(int)(ts.Width*w),(int)(ts.Height*h));
			}
		}

		internal double GetEffectiveYXRatio()
		{
			if(coordSysRectangle.IsEmpty)
				coordSysRectangle = EffectiveTarget;
			return ((double)coordSysRectangle.Height)/coordSysRectangle.Width;
		}

		internal Rectangle EffectiveOuterTarget
		{
			get
			{
				double x,y,w,h;
				GetEffectiveParameters(out x,out y,out w,out h,-1);
				Rectangle tRect = Series.OwningChart.TargetRectangle;
				return new Rectangle(tRect.X+(int)(tRect.Width*x),tRect.Y+(int)(tRect.Height*y),(int)(tRect.Width*w),(int)(tRect.Height*h));
			}
		}

		internal TargetArea GetSubSection(int xSection, int nSections)
		{
			// Default partition of an area in subareas
			// Note that sizes are relative to the size of the parent target area
			int nx = (int)(Math.Sqrt(nSections-1)+1);
			int ny = (nSections-1)/nx + 1;
			double dx = 1.0/nx;
			double dy = 1.0/ny;
			int row = xSection/nx;
			int col = xSection - nx*row;
			row = ny-1-row;
			TargetArea area = new TargetArea();
			area.Mapping.Margins = Mapping.Margins;
			area.SetParameters(col*dx,row*dy,dx,dy);
			return area;
		}

        internal void SetCoordinates()
        {
            Vector3D VD = mapping.ViewDirection;

            // In design time we track pie/doughnut view direction separatelly from linear one since
            // (1) defaults are different and (2) we don't want to keep the view direction when switching
            // from pie/doughnut to linear and vv.

            bool thisTimeIsRadar = Series.Style.ChartKindCategory == ChartKindCategory.Radar;
            bool thisTimeIsPieDoughnut = series.Style.ChartKind == ChartKind.Pie || series.Style.ChartKind == ChartKind.Doughnut;

            bool wiewDirectionWasDefault = false;
            if (OwningChart.InDesignMode)
            {
                if (thisTimeIsRadar)
                {
                    if (!lastTimeWasRadar)
                        projectionKind = Mapping.Kind;
                }
                else if (thisTimeIsPieDoughnut)
                {
                    if (lastTimeWasRadar)
                        Mapping.Kind = projectionKind;
                    else
                        projectionKind = Mapping.Kind;
                    if (lastTimeWasPieDoughnut)
                        viewDirectionRadial = VD; // save
                    else
                        VD = viewDirectionRadial; // restore
                }
                else // Standard, linear
                {
                    if (lastTimeWasRadar)
                        Mapping.Kind = projectionKind;
                    else
                        projectionKind = Mapping.Kind;
                    if (lastTimeWasPieDoughnut || lastTimeWasRadar)
                        VD = viewDirectionLinear; // restore
                    else // save
                        viewDirectionLinear = VD;
                }

                lastTimeWasPieDoughnut = thisTimeIsPieDoughnut;
                lastTimeWasRadar = thisTimeIsRadar;
            }
            else if (Mapping.IsViewDirectionDefault)
            {
                if (thisTimeIsPieDoughnut)
                    VD = Mapping.defaultViewDirectionRadial;
                else
                    VD = Mapping.defaultViewDirectionLinear;
                wiewDirectionWasDefault = true;
            }

            CoordinateSystem CS = series.CoordSystem;

            if (CS == null)
                CS = series.CreateCoordinateSystem();

            CS.PlaneXY.IsRadial = false;
            if (series.Style.IsRadar)
            {
                CS.Orientation = CoordinateSystemOrientation.Default;
                CS.PlaneXY.IsRadial = true;
                mapping.Kind = ProjectionKind.TwoDimensional;
            }
            else if (series.Style.ChartKind == ChartKind.Pie || series.Style.ChartKind == ChartKind.Doughnut)
            {
                CS.Orientation = CoordinateSystemOrientation.Default;
                mapping.ViewDirection = new Vector3D(0, mapping.ViewDirection.Y, mapping.ViewDirection.Z);
            }

            // Doughnut/Pie does not use the same 2D projection plane as the others

            if (mapping.Kind == ProjectionKind.TwoDimensional)
            {
                if (series.Style.ChartKind == ChartKind.Pie || series.Style.ChartKind == ChartKind.Doughnut)
                    mapping.ProjectionPlane = ComponentArt.Web.Visualization.Charting.Mapping.ProjectionPlaneKind.PlaneXZ;
                else
                    mapping.ProjectionPlane = ComponentArt.Web.Visualization.Charting.Mapping.ProjectionPlaneKind.PlaneXY;
            }
            double x0, y0, x1, y1, z0, z1;
            Series.WCSRange(out x0, out y0, out z0, out x1, out y1, out z1);
            double xr, yr, wr, hr;
            GetEffectiveParameters(out xr, out yr, out wr, out hr);
            GetParametersReducedByLegends(ref xr, ref yr, ref wr, ref hr);
            mapping.DPI = 96.0 / OwningChart.ReducedSamplingStep * OwningChart.ScaleToNativeSize;

            mapping.ViewDirection = VD;
            mapping.IsViewDirectionDefault = wiewDirectionWasDefault;

            mapping.TargetSize = Series.OwningChart.EffectiveTargetSize;
            mapping.Margins = this.Margins;
            mapping.IsPieDoughnut = thisTimeIsPieDoughnut;
            mapping.Setup(new Vector3D(x1, y1, z1), xr, yr, wr, hr);

            mapping.ForceRefresh();
            GE.SetMapping(mapping);
        }

		#region --- Target Area Layout ---

		// NOTE: All rectangles are computed with y0 at bottom

		// The rectangle in which this target area is located.
		// It is the outerRectangle of the owning target area, 
		// or full bitmap area in case of root target area
		[DoNotClone]
		private Rectangle owningRectangle;

		// Defined by this target area relative position within owning rectangle
		[DoNotClone]
		private Rectangle outerRectangle;		
		
		// Defined by safety margins. Nothing renders outside this rectangle except maybe
		// titles and/or legends; safetyRectangle <= outerRectangle
		[DoNotClone]
		private Rectangle safetyRectangle;
		
		// Maximum rectange not covered by titles, titleSafeRectangle <= outerRectangle
		[DoNotClone]
		private Rectangle titleSafeRectangle;

		// Defined by margins, then corrected to fit within titleSafeRectangle
		[DoNotClone]
		private Rectangle marginsRectangle;

		// Maximum rectangle not covered by legends not sharing chart area.
		// Depends on titleSafeRectangle and legends layout and not always included
		// in margins rectangle; legendsSafeRectangle <= titleSafeRectangle
		[DoNotClone]
		private Rectangle legendsSafeRectangle;

		// Maximum rectangle where axis annotations are allowed.
		[DoNotClone]
		private Rectangle annotationRectangle;

		// Coordinate system rectangle, filled by the chart coordinate system.
		// The chart is scaled and translated so that the following conditions are satisfied:
		// 1. Coordinate system planes or Pie/Radar objects without text fit within 
		//    intersection of legendsSafeRectangle and marginsRectangle
		// 2. Chart WITH text fits within legendsSafeRectangle
		[DoNotClone]
		private Rectangle coordSysRectangle;

		internal void ComputeLayot()
		{
			if(OwningTargetArea != null)
				OwningTargetArea.ComputeLayot();

			ComputeOwningRectangle();
			ComputeOuterRectangle();
			ComputeSafetyRectangle();
			ComputeTitleSafeRectangle();
			ComputeMarginsRectangle();
			ComputeLegendSafeRectangle();
			annotationRectangle = Intersection(legendsSafeRectangle,safetyRectangle);
			coordSysRectangle = Intersection(legendsSafeRectangle,marginsRectangle);
		
#if TEST_LAYOUT
			testRectangles.Clear();
			testRectangles.Add(new TestAreaRectangle(Color.Blue,coordSysRectangle));
			testRectangles.Add(new TestAreaRectangle(Color.Red,annotationRectangle));
#endif

		}

		private Rectangle Intersection(Rectangle rect1, Rectangle rect2)
		{
			int x0 = Math.Max(rect1.X,rect2.X);
			int y0 = Math.Max(rect1.Y,rect2.Y);
			int x1 = Math.Min(rect1.X+rect1.Width,rect2.X+rect2.Width);
			int y1 = Math.Min(rect1.Y+rect1.Height,rect2.Y+rect2.Height);
			return new Rectangle(x0,y0,x1-x0,y1-y0);
		}

		private void ComputeOwningRectangle()
		{
			if(OwningTargetArea != null)
				owningRectangle = OwningTargetArea.outerRectangle;
			else
				owningRectangle = new Rectangle(0,0,OwningChart.TargetSize.Width,OwningChart.TargetSize.Height);
		}
		private void ComputeOuterRectangle()
		{
			int x0 = (int)(owningRectangle.X + (owningRectangle.Width * xr) + 0.5);
			int x1 = (int)(owningRectangle.X + (owningRectangle.Width * (xr + wr)) + 0.5);
			int y0 = (int)(owningRectangle.Y + (owningRectangle.Height * yr) + 0.5);
			int y1 = (int)(owningRectangle.Y + (owningRectangle.Height * (yr + hr)) + 0.5);
			outerRectangle = new Rectangle(x0,y0,x1-x0,y1-y0);
#if TEST_LAYOUT
			testRectangles.Add(new TestAreaRectangle(Color.Red,outerRectangle));
#endif
		}

		private void ComputeMarginsRectangle()
		{
			Rectangle owningRectangle;
			if(OwningTargetArea != null)
				owningRectangle = OwningTargetArea.outerRectangle;
			else
				owningRectangle = new Rectangle(0,0,this.Mapping.TargetSize.Width,this.Mapping.TargetSize.Height);

			int x0 = (int)(owningRectangle.X + (owningRectangle.Width * (xr + Mapping.Margins.Left*0.01) + 0.5));
			int x1 = (int)(owningRectangle.X + (owningRectangle.Width * (xr + wr - Mapping.Margins.Right*0.01)) + 0.5);
			int y0 = (int)(owningRectangle.Y + (owningRectangle.Height * (yr + Mapping.Margins.Bottom*0.01)) + 0.5);
			int y1 = (int)(owningRectangle.Y + (owningRectangle.Height * (yr + hr - Mapping.Margins.Top*0.01)) + 0.5);

			marginsRectangle = new Rectangle(x0,y0,x1-x0,y1-y0);
			
#if TEST_LAYOUT
			testRectangles.Add(new TestAreaRectangle(Color.Pink,marginsRectangle));
#endif
		}

		private void ComputeSafetyRectangle()
		{
			double sm = OwningChart.SafetyMarginsPercentage;
			int sw = (int)(sm*outerRectangle.Width/100.0 + 0.5);
			int sh = (int)(sm*outerRectangle.Height/100.0 + 0.5);
			safetyRectangle = new Rectangle(outerRectangle.X + sw, outerRectangle.Y+sh, outerRectangle.Width-2*sw, outerRectangle.Height-2*sh);
#if TEST_LAYOUT
			testRectangles.Add(new TestAreaRectangle(Color.Green,safetyRectangle));
#endif
			}

		private void ComputeTitleSafeRectangle()
		{
			int x0 = outerRectangle.X;
			int y0 = outerRectangle.Y;
			int x1 = x0 + outerRectangle.Width;
			int y1 = y0 + outerRectangle.Height;

			int height = OwningChart.TargetSize.Height;

			for(int i=0; i<Titles.Count;i++)
			{
				ChartTitle title = Titles[i];
				RectangleF rect = title.ComputeRectangle(outerRectangle);
				// rect has y0 at the top!
				switch(title.Position)
				{
					case TitlePosition.Top:
						y1 = (int)Math.Min(y1,outerRectangle.Y + height-(rect.Y+rect.Height));
						break;
					case TitlePosition.LeftDownwards:
					case TitlePosition.LeftUpwards:
						x0 = (int)Math.Max(x0,outerRectangle.X+rect.Width);
						break;
					case TitlePosition.RightDownwards:
					case TitlePosition.RightUpwards:
						x1 = (int)Math.Min(x1,outerRectangle.X+outerRectangle.Width-rect.Width);
						break;
					case TitlePosition.Bottom:
						y0 = (int)Math.Max(y0,outerRectangle.Y + rect.Height);
						break;
					default:break;
				}
			}

			titleSafeRectangle = new Rectangle(x0,y0,x1-x0,y1-y0);
#if TEST_LAYOUT
			testRectangles.Add(new TestAreaRectangle(Color.Blue,titleSafeRectangle));
#endif
			}
	
		private void ComputeLegendSafeRectangle()
		{
			int x0 = titleSafeRectangle.X;
			int y0 = titleSafeRectangle.Y;
			int x1 = x0 + titleSafeRectangle.Width;
			int y1 = y0 + titleSafeRectangle.Height;

			ArrayList leg = new ArrayList();

			if(Legend.Visible && !Legend.SharesChartArea)
				leg.Add(Legend);
			if(secondaryLegends != null)
			{
				foreach(Legend l in secondaryLegends)
				{
					if(l.Visible && !l.SharesChartArea)
						leg.Add(l);
				}
			}

			int height = OwningChart.TargetSize.Height;

			foreach(ComponentArt.Web.Visualization.Charting.Legend l in leg)
			{
				Rectangle rec = l.GetRectangle();
				// rec has y0 at top
				switch(l.LegendPosition)
				{
					case LegendPositionKind.BottomCenter:
						y0 = Math.Max(y0,height-rec.Y);
						break;
					case LegendPositionKind.BottomLeft:
						if(rec.Width > rec.Height)
							y0 = Math.Max(y0,height-rec.Y);
						else
							x0 = Math.Max(x0,rec.Left + rec.Width);
						break;
					case LegendPositionKind.BottomRight:
						if(rec.Width > rec.Height)
							y0 = Math.Max(y0,height-rec.Y);
						else
							x1 = Math.Min(x1,rec.Left);
						break;
					case LegendPositionKind.CenterLeft:
						x0 = Math.Max(x0,rec.Left + rec.Width);
						break;
					case LegendPositionKind.CenterRight:
						x1 = Math.Min(x1,rec.Left);
						break;
					case LegendPositionKind.TopCenter:
						y1 = Math.Min(y1,height-(rec.Y + rec.Height));
						break;
					case LegendPositionKind.TopLeft:
						if(rec.Width > rec.Height)
							y1 = Math.Min(y1,height-(rec.Y + rec.Height));
						else
							x0 = Math.Max(x0,rec.Left + rec.Width);
						break;
					case LegendPositionKind.TopRight:
						if(rec.Width > rec.Height)
							y1 = Math.Min(y1,height-(rec.Y + rec.Height));
						else
							x1 = Math.Min(x1,rec.Left);
						break;		
				}
			}
			legendsSafeRectangle = new Rectangle(x0,y0,x1-x0,y1-y0);
#if TEST_LAYOUT
			testRectangles.Add(new TestAreaRectangle(Color.Brown,legendsSafeRectangle));
#endif
		}
		
		
		internal void AdjustMapping(TargetAreaBox tab)
		{
			if(Mapping.Kind != ProjectionKind.TwoDimensional)
				Adjust3DMapping(tab);
			else
				return; // because this has already been handled differently for 2D
		}

		internal static bool Adjust2DAreas(GeometricObject box)
		{
			TargetAreaBox tab = box as TargetAreaBox;
			bool someAreasAdjusted = false;
			if(tab != null)
			{
				TargetArea ta = tab.Tag as TargetArea;
				if(ta != null && ta.Mapping.Kind == ProjectionKind.TwoDimensional)
				{
					ta.Adjust2DMapping(tab);
					someAreasAdjusted = true;
				}
			}

			if(box.SubObjects != null)
			{
				foreach(GeometricObject b in box.SubObjects)
					someAreasAdjusted = someAreasAdjusted || Adjust2DAreas(b);
			}
			return someAreasAdjusted;
		}

		private Rectangle Revert(Rectangle rect)
		{
			int outY = OwningChart.TargetSize.Height - (rect.Y + rect.Height);
			return new Rectangle(rect.X,outY,rect.Width,rect.Height);
		}

		internal void Adjust2DMapping(TargetAreaBox tab)
		{
			if(Series.Style.ChartKindCategory == ChartKindCategory.Radar)
				return;
			// In 2D projections we redefine ICS ranges to get maximum coverage
			// of the coordinateSystemRectangle
			ComputeLayot();
			Mapping.ResetAfterProjectionTransformation();
			TargetCoordinateRange csRange = tab.CoordinateRange(false);
			TargetCoordinateRange annRange = tab.CoordinateRange(true);

			CoordinateSystem cs = Series.CoordSystem;
			Axis xAxis = cs.XAxis;
			Axis yAxis = cs.YAxis;
			Axis hAxis,vAxis;
			if(cs.Orientation == CoordinateSystemOrientation.Horizontal)
			{
				hAxis = yAxis;
				vAxis = xAxis;
			}
			else
			{
				hAxis = xAxis;
				vAxis = yAxis;
			}

			// The new target rectangle for coordinate system
			int x0 = coordSysRectangle.X;
			int y0 = coordSysRectangle.Y;
			int x1 = x0 + coordSysRectangle.Width;
			int y1 = y0 + coordSysRectangle.Height;

			if(OwningChart.ResizeMarginsToFitLabels)
			{
				double height = OwningChart.TargetSize.Height;
				double annRangeY0 = height - annRange.Y1;
				double annRangeY1 = height - annRange.Y0;

				double dx0 = annotationRectangle.X - annRange.X0;
				double dx1 = annRange.X1 - (annotationRectangle.X + annotationRectangle.Width);

				double dy0 = annotationRectangle.Y - annRange.Y0;
				double dy1 = annRange.Y1 - (annotationRectangle.Y + annotationRectangle.Height);

				if(dx0 > 0)
				{
					x0 = Math.Max(x0,(int)(csRange.X0 + dx0));
				}
				if(dx1 > 0)
				{
					x1 = Math.Min(x1,(int)(csRange.X1 - dx1));
				}
				if(dy0 > 0)
				{
					y0 = Math.Max(y0,(int)(csRange.Y0 + dy0));
				}
				if(dy1 > 0)
				{
					y1 = Math.Min(y1,(int)(csRange.Y1 - dy1));
				}
			}

			Rectangle targetRect = new Rectangle(x0,y0,(x1-x0),(y1-y0));

			if(Series.CoordSystem.YAxis.AutoICSRange)
			{
				// Correct ICS size to get the same x/y proportion
				double scaleFactor = ((double)(x1 - x0))/(y1 - y0);
			
				if(cs.Orientation == CoordinateSystemOrientation.Horizontal)
					Series.CoordSystem.YAxis.SetMaxValueICS(Series.CoordSystem.DXICS*scaleFactor);
				else
					Series.CoordSystem.YAxis.SetMaxValueICS(Series.CoordSystem.DXICS/scaleFactor);
			}

			AdjustMappingToFit(targetRect,csRange);
		}


		internal void Adjust3DMapping(TargetAreaBox tab)
		{
			ComputeLayot();
			Mapping.ResetAfterProjectionTransformation();
			TargetCoordinateRange csRange = tab.CoordinateRange(false);
			TargetCoordinateRange annRange = tab.CoordinateRange(true);

			// The new target rectangle for coordinate system
			int x0 = coordSysRectangle.X;
			int y0 = coordSysRectangle.Y;
			int x1 = x0 + coordSysRectangle.Width;
			int y1 = y0 + coordSysRectangle.Height;

			if(OwningChart.ResizeMarginsToFitLabels)
			{
				double height = OwningChart.TargetSize.Height;
				double annRangeY0 = height - annRange.Y1;
				double annRangeY1 = height - annRange.Y0;

				double dx0 = annotationRectangle.X - annRange.X0;
				double dx1 = annRange.X1 - (annotationRectangle.X + annotationRectangle.Width);

				double dy0 = annotationRectangle.Y - annRange.Y0;
				double dy1 = annRange.Y1 - (annotationRectangle.Y + annotationRectangle.Height);

				if(dx0 > 0)
				{
					x0 = Math.Max(x0,(int)(csRange.X0 + dx0));
				}
				if(dx1 > 0)
				{
					x1 = Math.Min(x1,(int)(csRange.X1 - dx1));
				}
				if(dy0 > 0)
				{
					y0 = Math.Max(y0,(int)(csRange.Y0 + dy0));
				}
				if(dy1 > 0)
				{
					y1 = Math.Min(y1,(int)(csRange.Y1 - dy1));
				}
			}

			Rectangle targetRect = new Rectangle(x0,y0,(x1-x0),(y1-y0));

			// Correct ICS size to get the same x/y proportion
			double scaleFactor = ((double)(x1 - x0))/(y1 - y0);

			AdjustMappingToFit(targetRect,csRange);
			coordSysRectangle = targetRect;
		}

		private void AdjustMappingToFit(Rectangle outputRectangle, TargetCoordinateRange cr)
		{
			// The rectangle before transformation
			double x1 = cr.X0;
			double y1 = cr.Y0;
			double w1 = cr.X1-cr.X0;
			double h1 = cr.Y1-cr.Y0;
			// Scaling factor
			double fx = outputRectangle.Width/w1;
			double fy = outputRectangle.Height/h1;
			double f = Math.Min(fx,fy);
			// Output rectangle
			double w2 = w1*f;
			double h2 = h1*f;
			double x2 = outputRectangle.X + (outputRectangle.Width-w2)/2;
			double y2 = outputRectangle.Y + (outputRectangle.Height-h2)/2;
			// Set parameters
			Mapping.SetAfterProjectionParameters(x1,y1,x2,y2,f);
		}

		#endregion --- Target Area Layout ---

		internal void GetIntersectionRectangle(double xa, double ya, double wa, double ha,
			double xb, double yb, double wb, double hb,
			out double x, out double y, out double w, out double h)
		{
			double x0 = Math.Max(xa,xb);
			double y0 = Math.Max(ya,yb);
			double x1 = Math.Min(xa+wa, xb+wb);
			double y1 = Math.Min(ya+ha, yb+hb);
			x = x0;
			y = y0;
			w = x1-x0;
			h = y1-y0;
		}


		internal void RenderLegend()
		{
			Graphics g = OwningChart.WorkingGraphics;
			if(g == null)
				return;
			Rectangle rect = EffectiveOuterTarget;
			titles.Render(g,rect);
			if(legend != null)
				legend.Render(g);
			if(secondaryLegends != null)
			{
				foreach(Legend leg in secondaryLegends)
				{
					leg.Render(g);
				}
			}
#if TEST_LAYOUT
			RenderTestAreaRectangles(g);
#endif
		}

		// --- Test drawing layout rectangles ---

#if TEST_LAYOUT
		private ArrayList testRectangles = new ArrayList();

		private void RenderTestAreaRectangles(Graphics g)
		{
			if(testRectangles == null || testRectangles.Count == 0)
				return;
			foreach(TestAreaRectangle r in testRectangles)
				r.Render(g,(int)(OwningChart.TargetSize.Height));
		}

		private class TestAreaRectangle
		{
			private Color color;
			private double x0, y0, x1, y1;
			public TestAreaRectangle(Color color, Rectangle rect)
			{
				this.color = color;
				this.x0 = rect.X;
				this.x1 = rect.X+rect.Width;
				this.y0 = rect.Y;
				this.y1 = rect.Y +rect.Height;
			}
			public TestAreaRectangle(Color color, double x0, double x1, double y0, double y1)
			{
				this.color = color;
				this.x0 = x0;
				this.x1 = x1;
				this.y0 = y0;
				this.y1 = y1;
			}

			public void Render(Graphics g, int height)
			{
				Pen pen = new Pen(color,1);
				g.DrawRectangle(pen,(int)x0,height-(int)(y1),(int)(x1-x0)-1, (int)(y1-y0)-1);
			}
		}
#endif
	}
}
