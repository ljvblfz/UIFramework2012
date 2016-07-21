using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing.Drawing2D;

#if !__BUILDING_ComponentArt_Win_UI_Internal__
namespace ComponentArt.Win.UI.Internal
#else
namespace ComponentArt.Win.UI.WinChartSamples
#endif
{
	/// <summary>
	/// Summary description for WizardTabControl.
	/// </summary>
#if DEBUG 
	[Designer(typeof(
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControlDesigner
#else
ComponentArt.Win.UI.WinChartSamples.TabControlDesigner
#endif
		))]
#endif
	[DefaultEvent("SelectedIndexChanged")]
#if __BUILDING_ComponentArt_Win_UI_Internal__
	public
#else
	internal 
#endif
		class TabControl : System.Windows.Forms.Control
	{

		private ArrayList m_buttons = new ArrayList();

		internal static readonly int HeaderHeight = 24;
		internal static readonly int TabPadding = 2;
		internal static readonly int TabButtonInternalPadding = 10;
		internal static readonly Color DarkColor = Color.FromArgb(238,238,238);
		internal static readonly Color LightColor = Color.FromArgb(246,246,246);

		int m_lastButtonX;

		int m_selectedIndex = -1;

		public TabControl() 
		{
            m_selectedIndex = 0;
			//m_panelCollection = new WizardTabControl.PanelCollection(this);
		}

		internal int LastButtonX 
		{
			get 
			{
				return m_lastButtonX;
			}
		}

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
 
		protected override bool ProcessKeyPreview(ref Message m)
		{
			if (this.ProcessKeyEventArgs(ref m))
			{
				return true;
			}
			return base.ProcessKeyPreview(ref m);
		}
 
		internal ArrayList TabButtons 
		{
			get 
			{
				return m_buttons;
			}
		}

		class TabButton 
		{
			Rectangle m_buttonArea;
            
			public Rectangle ButtonArea 
			{
				get {return m_buttonArea;}
				set {m_buttonArea = value;}
			}
		}

		internal void SetPanelProperties(Panel p) 
		{
			p.Location = new Point(TabPadding, HeaderHeight+1);
			p.Size = new Size( Size.Width - 2 * TabPadding, Size.Height - HeaderHeight - TabPadding-1);
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
				if (DesignMode) 
				{
					WaterMark.Draw(pevent.Graphics, this, "ComponentArt TabControl", "for use within the charting product only");
				}


				return;
			}

			int firstX = 1;
			int currentX = firstX;
			Font font = new Font("Verdana", 11, FontStyle.Bold, GraphicsUnit.Pixel);

			TabButtons.Clear();

			Pen greyPen = new Pen(new SolidBrush(Color.FromArgb(153, 153, 153)));
			Pen darkPen = new Pen(new SolidBrush(DarkColor));
			foreach (Control c in Controls) 
			{
				TabButton tb = new TabButton();

				bool selected = c == SelectedTab;

				Panel p = (Panel)c;
				string panelText = (p is 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabPage
#else
ComponentArt.Win.UI.WinChartSamples.TabPage
#endif
					)? p.Text : p.Name;
				SizeF panelTextSize = g.MeasureString(panelText, font);

				int tabButtonWidth = TabButtonInternalPadding*2 + (int)panelTextSize.Width;
				int nextX = currentX + tabButtonWidth;

				Color textColor = (selected ? Color.FromArgb(221,52,9) : Color.FromArgb(102, 102, 102));
				tb.ButtonArea = new Rectangle(currentX, 0, TabButtonInternalPadding*2 + (int)panelTextSize.Width, HeaderHeight-1);
				TabButtons.Add(tb);

				g.DrawLine(greyPen, nextX, 1, nextX, HeaderHeight-2);

				int buttonGradientStartY = 3;
				int buttonGradientFinishY = ((HeaderHeight-2)-buttonGradientStartY)/2;

				if (selected) 
				{
					Skin.PaintVerticalGradient(g, 
						new Rectangle(currentX + 2, buttonGradientStartY, tabButtonWidth - 3, buttonGradientFinishY - buttonGradientStartY), LightColor, Color.White);

					g.DrawLine(darkPen, currentX+2, buttonGradientStartY, nextX-2, buttonGradientStartY);
					Pen pen = new Pen(new LinearGradientBrush(new Point(currentX+2, buttonGradientStartY), new Point(currentX+2, HeaderHeight-3), DarkColor, Color.White));
					g.DrawLine(pen, currentX+2, buttonGradientStartY, currentX+2, HeaderHeight-3);
					g.DrawLine(pen, nextX-2, buttonGradientStartY, nextX-2, HeaderHeight-3);

					if (Focused) 
					{
						Rectangle r = new Rectangle(currentX + 4, 4, TabButtonInternalPadding*2 + (int)panelTextSize.Width - 8, HeaderHeight-8);
						Pen focusPen = new Pen(Color.DarkGray);
						focusPen.DashStyle = DashStyle.Dot;
						focusPen.LineJoin = LineJoin.Round;
						g.DrawRectangle(focusPen, r);
					}
					pen.Dispose();
				} 
				else 
				{
					// Bottom line of the button
					g.DrawLine(greyPen, currentX, HeaderHeight-2, nextX, HeaderHeight-2);
					g.DrawLine(darkPen, currentX, HeaderHeight-1, nextX, HeaderHeight-1);
					Skin.PaintVerticalGradient(g, 
						new Rectangle(currentX + 1, buttonGradientStartY, tabButtonWidth - 1, buttonGradientFinishY - buttonGradientStartY), Color.White, DarkColor);
					g.FillRectangle(new SolidBrush(DarkColor), currentX + 1, buttonGradientFinishY, tabButtonWidth - 1, HeaderHeight- 2 - buttonGradientFinishY);
					g.DrawLine(darkPen, currentX+1, buttonGradientStartY-1, nextX-1, buttonGradientStartY-1);
					g.DrawLine(darkPen, currentX+1, buttonGradientStartY, currentX+1, HeaderHeight-3);
					g.DrawLine(darkPen, nextX-1, buttonGradientStartY, nextX-1, HeaderHeight-3);
				}
				SolidBrush sb = new SolidBrush(textColor);
				g.DrawString(panelText, font, sb, currentX + TabButtonInternalPadding, 6);
				sb.Dispose();

				m_lastButtonX = currentX = nextX;
			}			

			g.DrawLine(greyPen, 1, 1, m_lastButtonX, 1);
			g.DrawLine(greyPen, 1, 1, 1, height-2);
			g.DrawLine(greyPen, 1, height-2, width-2, height-2);
			g.DrawLine(greyPen, width-2, height-2, width-2, HeaderHeight-1);
			g.DrawLine(greyPen, width-2, HeaderHeight-2, m_lastButtonX, HeaderHeight-2);
			g.DrawLine(darkPen, width-3, HeaderHeight-1, m_lastButtonX, HeaderHeight-1);
			SolidBrush sbDark = new SolidBrush(DarkColor);
			g.FillRectangle(sbDark, m_lastButtonX+2, 0, width-(m_lastButtonX+2), HeaderHeight-3);
			sbDark.Dispose();
		}

		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseDown(e);

			int index = -1;

			foreach (TabButton tb in TabButtons) 
			{
				if (tb.ButtonArea.IntersectsWith(new Rectangle(new Point(e.X, e.Y), new Size(1,1)))) 
				{
					index = TabButtons.IndexOf(tb);
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
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.ControlCollection(this);
		}


		public 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
				ComponentArt.Win.UI.Internal.TabControl.ControlCollection 
#else
			ComponentArt.Win.UI.WinChartSamples.TabControl.ControlCollection 
#endif
			TabPages 
		{
			get 
			{
				return (
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
					ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
					.ControlCollection)Controls;
			}
		}


		class TabPagesCollectionEditor : CollectionEditor 
		{
			public TabPagesCollectionEditor(Type type): base(type) 
			{
			}

			protected override object CreateInstance(Type itemType)
			{
				return base.CreateInstance(typeof(
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabPage
#else
					ComponentArt.Win.UI.WinChartSamples.TabPage
#endif
					));
			}
		}

		internal void PageTextChanged(object sender, EventArgs e) 
		{
			Invalidate(new Rectangle(0, 0, Width, HeaderHeight));
		}

	}
}
