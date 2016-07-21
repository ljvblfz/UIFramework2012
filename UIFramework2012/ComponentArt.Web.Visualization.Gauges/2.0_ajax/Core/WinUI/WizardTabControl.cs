using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing.Drawing2D;

namespace ComponentArt.WinUI
{
	/// <summary>
	/// Summary description for WizardTabControl.
	/// </summary>
	[Designer(typeof(ComponentArt.WinUI.TabControlDesigner))]
	[DefaultEvent("SelectedIndexChanged")]
#if DEBUG
	public
#else
	internal 
#endif
		class TabControl : System.Windows.Forms.Control
	{
		internal static readonly int HeaderHeight = 24;
		internal static readonly int TabPadding = 2;
		internal static readonly int TabButtonInternalPadding = 8;
		internal static readonly Color DarkColor = Color.FromArgb(238,238,238);
		internal static readonly Color LightColor = Color.FromArgb(246,246,246);
		private Color colorBehind = Color.FromArgb(238, 238, 238);

		private bool multiLine = false;
		private System.Windows.Forms.TabSizeMode sizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
		private Rectangle[] tabButtonRectangles;

		int m_lastButtonX, m_lastButtonY;

		int m_selectedIndex = -1;

		public TabControl() 
		{
			m_selectedIndex = 0;
			//m_panelCollection = new WizardTabControl.PanelCollection(this);
		}

		[Category("Apearance")]
		[Description("Color behind this control, in area not covered by page tabs")]
		public Color ColorBehind { get { return colorBehind; } set { colorBehind = value; Invalidate(); } }

		internal int LastButtonX 
		{
			get 
			{
				return m_lastButtonX;
			}
		}

		internal int LastButtonY 
		{
			get 
			{
				return m_lastButtonY;
			}
		}

		public bool Multiline { get { return multiLine; } set { multiLine = value; } }
		public System.Windows.Forms.TabSizeMode SizeMode { get { return sizeMode; } set { sizeMode = value; } }

		protected override void OnResize ( System.EventArgs e ) 
		{
			base.OnResize(e);
			foreach (Panel p in Controls) 
			{
				SetPanelProperties(p);
			}
			Invalidate();
		}

		protected override void OnKeyDown(KeyEventArgs ke)
		{
			if ((ke.KeyCode == Keys.Tab) && ((ke.KeyData & Keys.Control) != Keys.None))
			{
				bool flag1 = (ke.KeyData & Keys.Shift) == Keys.None;
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex != -1)
				{
					if (flag1)
					{
						selectedIndex = (selectedIndex + 1) % TabPages.Count;
					}
					else
					{
						selectedIndex = ((selectedIndex + TabPages.Count) - 1) % TabPages.Count;
					}
					//this.UISelection = true;
					this.SelectedIndex = selectedIndex;
					UpdateSelection();
					Invalidate();
					ke.Handled = true;
				}
			}

			if (Focused) 
			{

				if (ke.KeyCode == Keys.Left) 
				{
					if (SelectedIndex > 0)
					{
						--SelectedIndex;
						UpdateSelection();
						Invalidate();
					}
				}

				if (ke.KeyCode == Keys.Right) 
				{

					if (SelectedIndex < this.TabPages.Count-1)
					{
						++SelectedIndex;
						UpdateSelection();
						Invalidate();
					}
				}
			}

			base.OnKeyDown(ke);
		}
 

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			switch ((keyData & Keys.KeyCode))
			{
				case Keys.Prior:
				case Keys.Next:
				case Keys.End:
				case Keys.Home:
				case Keys.Left:
				case Keys.Right:
				{
					return true;
				}
			}
			return base.IsInputKey(keyData);
		}
 



		//[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			if (this.ProcessKeyEventArgs(ref m))
			{
				return true;
			}
			return base.ProcessKeyPreview(ref m);
		}
 
		internal void SetPanelProperties(Panel p) 
		{
			p.Location = new Point(TabPadding, m_lastButtonY+1);
			Size sz = ClientSize;
			p.Size = new Size( sz.Width - 2 * TabPadding, sz.Height - m_lastButtonY - TabPadding-2);
			p.Invalidate();
		}

		internal void AddTab(Panel page1) 
		{
			Controls.Add(page1);
			Invalidate();
		}



		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (this.ShowFocusCues)
			{
				Invalidate();
			}
		}
 

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}
 


		protected override void OnPaint( System.Windows.Forms.PaintEventArgs pevent )
		{
			base.OnPaint(pevent);

			Graphics g = pevent.Graphics;

			g.Clear(Color.White);

			int height =  Height;
			int width =  Width;

			if (Controls.Count == 0) 
			{
				g.DrawRectangle(Pens.White, 0, 0, width-2, height-2);				
				g.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(204, 204, 204))), 1, 1, width-3, height-3);
				return;
			}

			Font font = base.Font;
			SolidBrush brushBehind = new SolidBrush(colorBehind);
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			// Planning layout
			int nPages = Controls.Count;
			tabButtonRectangles = new Rectangle[nPages];
			int x = 1;
			int y = 0;
			int w = 0;
			int h = HeaderHeight-1;
			int i = 0;
			int nRows = 1;
			int selectedX = -1;
			foreach (Control c in Controls) 
			{
				TabPage page = c as TabPage;
				if(page == null)
					continue;
				if(c == SelectedTab)
					selectedX = i;
				w = TabButtonInternalPadding*2 + (int)g.MeasureString(page.Text, font).Width;
				if(x + w >= ClientRectangle.Width)
				{
					y ++;
					x = 1;
					nRows ++;
				}
				// we use y-coordinate as row index for now
				tabButtonRectangles[i++] = new Rectangle(x, y, w, h);
				x += w;
			}
			int[] rowExpand = new int[nRows];

			if(nRows > 1)
			{
				// moving the row of the selected to the bottom
				if(selectedX >= 0)
				{
					int rowOfSelectedTab = tabButtonRectangles[selectedX].Y;
					if(rowOfSelectedTab < nRows-1 || true)
					{
						// exchange position of rows
						for(i=0; i<nPages; i++)
						{
							if(tabButtonRectangles[i].Y == rowOfSelectedTab)
								tabButtonRectangles[i].Y = nRows-1;
							else if (tabButtonRectangles[i].Y == nRows-1)
								tabButtonRectangles[i].Y = rowOfSelectedTab;
						}
						// sort the pages by the row
						int[] px = new int[nPages];
						int k = 0;
						for(int j = 0; j<nPages; j++)
						{
							for(i=0; i<nPages; i++)
							{
								if(tabButtonRectangles[i].Y == j)
									px[k++] = i;
							}
						}
						// stretch the lower rows, if needed (note that .Y is the row index)
						int[] rowWidth = new int[nRows];
						for(i=0; i<nPages; i++)
						{
							rowWidth[tabButtonRectangles[i].Y] = tabButtonRectangles[i].X + tabButtonRectangles[i].Width;
						}
						// expanded width
						rowExpand[0] = rowWidth[0];
						for(i=1; i<nRows; i++)
						{
							rowExpand[i] = Math.Max(rowExpand[i-1],rowWidth[i]);
						}
						for(i=0; i<nPages; i++)
						{
							int ix = px[i];
							int ixp = (i>0? px[i-1]:-1);
							int ixs = (i<nPages-1? px[i+1]:-1);
							if(ixp >=0 && tabButtonRectangles[ixp].Y != tabButtonRectangles[ix].Y)
								ixp = -1;
							if(ixs >=0 && tabButtonRectangles[ixs].Y != tabButtonRectangles[ix].Y)
								ixs = -1;
							if(ixp >= 0) // there is previous in this row
								tabButtonRectangles[ix].X = tabButtonRectangles[ixp].X + tabButtonRectangles[ixp].Width; // fix the starting x
							else
								tabButtonRectangles[ix].X = 1;
							int iRow = tabButtonRectangles[ix].Y;
							if(ixs >= 0) // there is next in this row
							{
								int iexp = (tabButtonRectangles[ix].Width * (rowExpand[iRow] - rowWidth[iRow]))/rowWidth[iRow];
								tabButtonRectangles[ix].Width += iexp;
							}
							else
							{
								tabButtonRectangles[ix].Width = rowExpand[iRow] - tabButtonRectangles[ix].X;
							}
						}
					}
				}
			}

			// Rendering
			Pen greyPen = new Pen(new SolidBrush(Color.FromArgb(153, 153, 153)));
			Pen darkPen = new Pen(new SolidBrush(DarkColor));
			m_lastButtonY = (y + 1)*h;
			g.DrawRectangle(greyPen,1,m_lastButtonY,ClientRectangle.Width-2,ClientRectangle.Height-m_lastButtonY-1);

			i = 0;
			foreach (Control c in Controls) 
			{
				TabPage page = c as TabPage;
				if(page != null)
				{
					tabButtonRectangles[i].Y *= h;
					PaintTabButton(page,g,font,
						tabButtonRectangles[i].X,
						tabButtonRectangles[i].Y,
						tabButtonRectangles[i].Width,
						tabButtonRectangles[i].Height,
						c == SelectedTab);
				}
				i++;
			}

			m_lastButtonX = x;

			foreach(Panel p in Controls)
				SetPanelProperties(p);

			if(nRows > 1)
			{
				for(i=0;i<nRows;i++)
				{
					int x0 = rowExpand[i]+2;
					int y0 = i*(h-1);
					if(i>0)
						y0 += 1;
					g.FillRectangle(brushBehind, x0, y0, ClientSize.Width-(x0), h);
				}
			}
			else
				g.FillRectangle(brushBehind, 
					tabButtonRectangles[nPages-1].X + tabButtonRectangles[nPages-1].Width + 2,0,ClientSize.Width - (tabButtonRectangles[nPages-1].X + tabButtonRectangles[nPages-1].Width),h);
			
			brushBehind.Dispose();
		}

		private void PaintTabButton(TabPage p, Graphics g, Font font, int x, int y , int w, int h, bool selected)
		{
			string panelText = p.Text;
			SizeF panelTextSize = g.MeasureString(panelText, font);

			int tabButtonWidth = w;
			int nextX = x + tabButtonWidth;

			Pen greyPen = new Pen(new SolidBrush(Color.FromArgb(153, 153, 153)));
			Pen darkPen = new Pen(new SolidBrush(DarkColor));
			Color textColor = (selected ? Color.FromArgb(221,52,9) : Color.FromArgb(102, 102, 102));
			Brush brush = null;
			
			g.DrawLine(greyPen, nextX, y+1, nextX, y+h-2);

			int buttonGradientStartY = y+3;
			int buttonGradientFinishY = y+h/3;

			if (selected) 
			{

				Skin.PaintVerticalGradient(g, 
					new Rectangle(x + 2, buttonGradientStartY, w - 3, buttonGradientFinishY - buttonGradientStartY), LightColor, Color.White);

				g.DrawLine(darkPen, x+2, buttonGradientStartY, nextX-2, buttonGradientStartY);
				brush = new LinearGradientBrush(new Point(x+2, buttonGradientStartY), new Point(x+2, buttonGradientFinishY), DarkColor, Color.White);
				Pen pen = new Pen(brush);
				g.DrawLine(pen, x+2, buttonGradientStartY, x+2, buttonGradientFinishY);
				g.DrawLine(pen, nextX-2, buttonGradientStartY, nextX-2, buttonGradientFinishY);
				brush.Dispose();
				pen.Dispose();

				if (Focused) 
				{
					Rectangle r = new Rectangle(x + 4, y + 4, TabButtonInternalPadding*2 + (int)panelTextSize.Width - 8, h-8);
					Pen focusPen = new Pen(Color.DarkGray);
					focusPen.DashStyle = DashStyle.Dot;
					focusPen.LineJoin = LineJoin.Round;
					g.DrawRectangle(focusPen, r);
					focusPen.Dispose();
				}

				g.DrawRectangle(greyPen,x,y+1,w,h+5);
				g.FillRectangle(Brushes.White,x+1,y+h-3,w-1,5);
			} 
			else 
			{
				g.DrawLine(darkPen, x, y+h-1, nextX, y+h-1);
				Skin.PaintVerticalGradient(g, 
					new Rectangle(x + 1, buttonGradientStartY, w - 1, buttonGradientFinishY - buttonGradientStartY), Color.White, DarkColor);
				brush = new SolidBrush(DarkColor);
				g.FillRectangle(brush, x + 1, buttonGradientFinishY, w - 1, y + h - buttonGradientFinishY);
				brush.Dispose();
				g.DrawLine(darkPen, x+1, buttonGradientStartY-1, nextX-1, buttonGradientStartY-1);
				g.DrawLine(darkPen, x+1, buttonGradientStartY, x+1, y+h-3);
				g.DrawLine(darkPen, nextX-1, buttonGradientStartY, nextX-1, y+h-3);
				g.DrawRectangle(greyPen,x,y+1,w,h-1);
			}
			brush = new SolidBrush(textColor);
			g.DrawString(panelText, font, brush, x + TabButtonInternalPadding, y + 6);

			brush.Dispose();
			greyPen.Dispose();
			darkPen.Dispose();
		}

		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseDown(e);

			int index = -1;

			for(int i=0; i<	tabButtonRectangles.Length; i++)
			{
				if (tabButtonRectangles[i].Contains(e.X, e.Y))
				{
					index = i;
					break;
				}
			}
			
			if (index == -1)
				return;

			SelectedIndex = index;

			UpdateSelection();

		}

		internal void UpdateSelection() 
		{
			for (int i=0; i<Controls.Count; ++i) 
			{
				Controls[i].Visible = (i == SelectedIndex);
			}
			Invalidate();
		}	


		[Browsable(false), DefaultValue(-1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				return this.m_selectedIndex;
			}
			set 
			{
				if (m_selectedIndex == value)
					return;

				m_selectedIndex = value;
				OnSelectedIndexChanged(EventArgs.Empty);
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Panel SelectedTab
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex == -1)
				{
					return null;
				}
				return (Panel)Controls[selectedIndex];
			}
			set
			{
				this.SelectedIndex = Controls.IndexOf(value);
			}
		}
 
		protected virtual void OnSelectedIndexChanged(EventArgs ea) 
		{
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, ea);
		}

		public event EventHandler SelectedIndexChanged;


		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new 
				ComponentArt.WinUI.TabControl.ControlCollection(this);
		}

		[Editor(typeof(TabPagesCollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public 	ComponentArt.WinUI.TabControl.ControlCollection TabPages 
		{
			get 
			{
				return (ComponentArt.WinUI.TabControl.ControlCollection)Controls;
			}
		}


		class TabPagesCollectionEditor : CollectionEditor 
		{
			private TabControl control = null;
			public TabPagesCollectionEditor(Type type): base(type) 
			{
			}

			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
			{
				control = context.Instance as TabControl;
				return base.EditValue(context,provider,value);
			}

			protected override object CreateInstance(Type itemType)
			{
				TabPage newPage = base.CreateInstance(typeof(ComponentArt.WinUI.TabPage)) as TabPage;
				if(control != null)
				{
					control.SetPanelProperties(newPage);
					control.Invalidate();
				}
				return newPage;
			}
		}

		internal void PageTextChanged(object sender, EventArgs e) 
		{
			Invalidate(new Rectangle(0, 0, Width, HeaderHeight));
		}

	}
}
