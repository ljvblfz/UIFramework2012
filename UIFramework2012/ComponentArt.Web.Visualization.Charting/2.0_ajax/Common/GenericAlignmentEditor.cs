using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	#region --- ContentAlignmentControl ---

	internal delegate void AlignmentChangedHandler(object sender, ContentAlignment newAlignment);

	internal class ContentAlignmentControl : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// Alignment point is one of 9 possible alignment alternatives.
		/// </summary>
		private class AlignmentPoint
		{
			// indices
			int ix, iy;
			// parent control
			ContentAlignmentControl parent;

			ContentAlignment alignment;
			
			bool selected = false;
			bool visible = true;

			// rectangle
			int x0,x1, y0,y1;

			internal AlignmentPoint(ContentAlignmentControl parent, int ix, int iy, ContentAlignment alignment)
			{
				this.parent = parent;
				this.ix = ix;
				this.iy = iy;
				this.alignment = alignment;
			}

			public bool SelectedAtPoint(Point point)
			{
				if(!visible)
					return false;
				GetRectangle();
				selected = x0 < point.X && point.X < x1 && y0 < point.Y && point.Y < y1;
				return selected;
			}

			public bool Selected { get { return selected; } set { selected = value; } }
			public bool Visible { get { return visible; } set { visible = value; } }
			public ContentAlignment Alignment { get { return alignment; } set { alignment = value; } }

			private void GetRectangle()
			{
				int dOuter = (int)(0.25*(Math.Min(parent.Width,parent.Height)));
				switch(ix)
				{
					case 0: x0 = 0; x1 = dOuter; break;
					case 1: x0 = dOuter+2; x1 = parent.Width-dOuter-3; break;
					default: x0 = parent.Width-dOuter-1; x1 = parent.Width - 1; break;
				}
				switch(iy)
				{
					case 0: y0 = 0; y1 = dOuter; break;
					case 1: y0 = dOuter+2; y1 = parent.Height-dOuter-3; break;
					default: y0 = parent.Height-dOuter-1; y1 = parent.Height - 1; break;
				}
			}

			internal void Paint(Graphics g)
			{
				if(!visible)
					return;

				GetRectangle();
				using (Pen lightPen = new Pen(Color.FromKnownColor(KnownColor.ControlLightLight),1F),
						   darkPen = new Pen(Color.FromKnownColor(KnownColor.ControlDark),1F))
				{
					if(selected)
					{
						g.DrawLine(darkPen,x0,y0,x0,y1); // left
						g.DrawLine(darkPen,x0,y0,x1,y0); // top
						g.DrawLine(lightPen,x1,y0,x1,y1); // right
						g.DrawLine(lightPen,x0,y1,x1,y1); // bottom
					}
					else
					{
						g.DrawLine(lightPen,x0,y0,x0,y1); // left
						g.DrawLine(lightPen,x0,y0,x1,y0); // top
						g.DrawLine(darkPen,x1,y0,x1,y1); // right
						g.DrawLine(darkPen,x0,y1,x1,y1); // bottom
					}
				}
			}
		}


		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private AlignmentPoint[] points = new AlignmentPoint[9];

		public event AlignmentChangedHandler AlignmentChanged;

		public ContentAlignmentControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			InitializeComponent2();
		}

		public bool CentralPointVisible 
		{
			get { return points[4].Visible; }
			set 
			{
				if(points[4].Visible != value)
				{
					points[4].Visible = value; 
					Invalidate();
				}
			}
		}

		public ContentAlignment ContentAlignment 
		{
			get
			{
				foreach(AlignmentPoint aPoint in points)
				{
					if(aPoint.Selected)
						return aPoint.Alignment;
				}
				// shouldn't happen, but anyway...
				points[0].Selected = true;
				return points[0].Alignment;
			}
			set
			{
				foreach(AlignmentPoint aPoint in points)
				{
					aPoint.Selected = (aPoint.Alignment == value);
				}
				Invalidate();
			}
		}

		#region --- Legend position handling ---
		
		private class PositionMapping
		{
			public ContentAlignment   contentAlignment;
			public LegendPositionKind legendPosition;
			public PositionMapping(ContentAlignment   contentAlignment, LegendPositionKind legendPosition)
			{
				this.contentAlignment = contentAlignment;
				this.legendPosition = legendPosition;
			}
		}

		private PositionMapping[] map = new PositionMapping[] 
		{
			new PositionMapping(ContentAlignment.BottomLeft, LegendPositionKind.BottomLeft),
			new PositionMapping(ContentAlignment.BottomCenter, LegendPositionKind.BottomCenter),
			new PositionMapping(ContentAlignment.BottomRight, LegendPositionKind.BottomRight),

			new PositionMapping(ContentAlignment.MiddleCenter, LegendPositionKind.BottomCenter),
			new PositionMapping(ContentAlignment.MiddleLeft, LegendPositionKind.CenterLeft),
			new PositionMapping(ContentAlignment.MiddleRight, LegendPositionKind.CenterRight),
		
			new PositionMapping(ContentAlignment.TopLeft, LegendPositionKind.TopLeft),
			new PositionMapping(ContentAlignment.TopCenter, LegendPositionKind.TopCenter),
			new PositionMapping(ContentAlignment.TopRight, LegendPositionKind.TopRight)
		};

		public LegendPositionKind GetLegendPosition()
		{
			ContentAlignment align = this.ContentAlignment;
			foreach(PositionMapping pm in map)
			{
				if(pm.contentAlignment == align)
					return pm.legendPosition;
			}
			// shouldn't be used...
			return LegendPositionKind.TopRight;
		}

		public void SetLegendPosition(LegendPositionKind legendPosition)
		{
			ContentAlignment align = this.ContentAlignment;
			foreach(PositionMapping pm in map)
			{
				if(pm.legendPosition == legendPosition)
				{
					align = pm.contentAlignment;
					break;
				}
			}
			ContentAlignment = align;
			Invalidate();
		}

		#endregion

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		void InitializeComponent2()
		{
			points[0] = new AlignmentPoint(this,0,0,ContentAlignment.TopLeft);
			points[1] = new AlignmentPoint(this,1,0,ContentAlignment.TopCenter);
			points[2] = new AlignmentPoint(this,2,0,ContentAlignment.TopRight);

			points[3] = new AlignmentPoint(this,0,1,ContentAlignment.MiddleLeft);
			points[4] = new AlignmentPoint(this,1,1,ContentAlignment.MiddleCenter);
			points[5] = new AlignmentPoint(this,2,1,ContentAlignment.MiddleRight);
		
			points[6] = new AlignmentPoint(this,0,2,ContentAlignment.BottomLeft);
			points[7] = new AlignmentPoint(this,1,2,ContentAlignment.BottomCenter);
			points[8] = new AlignmentPoint(this,2,2,ContentAlignment.BottomRight);

			points[0].Selected = true;
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ContentAlignmentControl
			// 
			this.Name = "ContentAlignmentControl";
			this.Resize += new System.EventHandler(this.ContentAlignmentControl_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DoPaint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContentAlignmentControl_MouseDown);

		}
		#endregion

		#region --- Painting and mouse handling ---
		private void DoPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromKnownColor(KnownColor.Control));

			foreach(AlignmentPoint aPoint in points)
				aPoint.Paint(e.Graphics);
		}

		private void ContentAlignmentControl_Resize(object sender, System.EventArgs e)
		{
			Invalidate();
		}

		private void ContentAlignmentControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			AlignmentPoint oldPoint = null, newPoint = null;
			foreach(AlignmentPoint ap in points)
			{
				if(ap.Selected)
					oldPoint = ap;
				if(ap.SelectedAtPoint(new Point(e.X,e.Y)))
					newPoint = ap;
			}
			if(newPoint == null)
				oldPoint.Selected = true;

			if(newPoint != null && AlignmentChanged != null)
				AlignmentChanged(this,newPoint.Alignment);

			Invalidate();
		}
		#endregion
	}
	
	
	#endregion

	internal class GenericAlignmentEditor : UITypeEditor
	{
		// This editor edits both ContentAlignment and LegendPosition values.
		// The Edit() function uses common functionality and adapts to the type of input value

		private ContentAlignmentControl contentAC;
		private IWindowsFormsEditorService service;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
 
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
				if (service == null)
				{
					return value;
				}
				if (contentAC == null)
				{
					contentAC = new ContentAlignmentControl();
					contentAC.AlignmentChanged += new AlignmentChangedHandler(OnAlignmentChanged);
				}
				if(value is ContentAlignment)
				{
					contentAC.ContentAlignment = (ContentAlignment)value;
					contentAC.CentralPointVisible = true; 
				}
				else
				{
					contentAC.SetLegendPosition((LegendPositionKind)value);
					contentAC.CentralPointVisible = false;
				}

				contentAC.Size = new Size(100,75); 
				service.DropDownControl(contentAC);
				if(value is ContentAlignment)
				{
					return contentAC.ContentAlignment;
				}
				else
				{
					return contentAC.GetLegendPosition();
				}
			}
			return value;
		}
 
		void OnAlignmentChanged(object sender, ContentAlignment align)
		{
			service.CloseDropDown();
			service = null;
		}
	}
}
