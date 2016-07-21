using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class ColorArrayControl : Control
	{
		IList m_colors = null;
		int m_strips;
		double m_step;
		int m_mouseColorIndex = -1;
		bool m_readonly = false;

		bool m_isSetupComplete = false;

		public event EventHandler PaletteChanged;
		public event EventHandler PaletteColorIndexChanged;

		public int ColorIndex  
		{
			get {return m_mouseColorIndex;}
			set {m_mouseColorIndex = value; /*Invalidate();*/}
		}
		
		public IList Colors 
		{
			get {return m_colors;}
			set {m_colors = value; m_isSetupComplete = false;}
		}

		/// <summary>
		/// Determines the width and height of color strips
		/// </summary>
		/// <param name="e"></param>
		void Setup(PaintEventArgs e ) 
		{
			if (m_colors != null) 
			{
				m_strips = m_colors.Count;
				m_step = (double)(Width - 4 ) / (double)m_strips;
			}
			m_isSetupComplete = true;
		}

		/// <summary>
		/// Paints strips of colors
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint ( PaintEventArgs e ) 
		{
			base.OnPaint(e);
			if (m_colors == null) 
			{
				e.Graphics.DrawLine(new Pen(Color.Blue), e.Graphics.VisibleClipBounds.Width-1, e.Graphics.VisibleClipBounds.Height-1, 0, 0);
				e.Graphics.DrawLine(new Pen(Color.Blue), e.Graphics.VisibleClipBounds.Width-1, 0, 0, e.Graphics.VisibleClipBounds.Height-1);
				return;
			}

			if (!m_isSetupComplete) 
			{
				Setup(e);
			}

			for (int i=0; i<m_strips; ++i) 
			{
				e.Graphics.FillRectangle(new SolidBrush(((ChartColor)m_colors[i]).Color), 2 + (int)(i*m_step+0.5), 2, 1+(int)(m_step+0.5), Height-4);				
			}

			// Draw a rectangle over the active color index
			if (m_mouseColorIndex != -1) 
			{
				e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2), e.Graphics.VisibleClipBounds.X+1 + (int)(m_mouseColorIndex*m_step+0.5), e.Graphics.VisibleClipBounds.Y+2, (int)(m_step+0.5), e.Graphics.VisibleClipBounds.Height-4);
			}
			e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width-1, Height-1);

			e.Graphics.DrawRectangle(Pens.White, 1, 1, Width-3, Height-3);

		}

		protected override void OnKeyDown ( System.Windows.Forms.KeyEventArgs e ) 
		{
			base.OnKeyDown(e);

			if (ReadOnly)
				return;

			if (e.KeyCode == Keys.Left) 
			{
				if (m_mouseColorIndex > 0) 
				{
					--m_mouseColorIndex;
					Invalidate();
					OnPaletteColorIndexChanged(EventArgs.Empty);
				}
			}

			if (e.KeyCode == Keys.Right) 
			{
				if (m_mouseColorIndex < m_colors.Count-1) 
				{
					++m_mouseColorIndex;
					Invalidate();
					OnPaletteColorIndexChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Make sure left and right keys are handled by this control
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys key)
		{
			switch (key)
			{
				case Keys.Left:
					return false;
				case Keys.Right:
					return false;
				default:
					return base.ProcessDialogKey(key);
			}
		}

		/// <summary>
		/// Calculates the selected index based on the movement
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseMove(e);

			if (ReadOnly)
				return;

			Cursor = Cursors.Hand;

			if (m_colors == null)
				return;

			int newMouseColorIndex = (int) ((double)e.X / m_step);

			if (newMouseColorIndex >= m_colors.Count)
				newMouseColorIndex = m_colors.Count-1;

			if (m_mouseColorIndex != newMouseColorIndex) 
			{
				m_mouseColorIndex = newMouseColorIndex;
				Invalidate();
				OnPaletteColorIndexChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Calls a color selection dialog
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseDown(e);

			if (ReadOnly)
				return;
			
			if (e.Button == MouseButtons.Right) 
			{
				ColorDialog cd = new ColorDialog();
				cd.Color = (Color)m_colors[m_mouseColorIndex];

				DialogResult dr = cd.ShowDialog();

				m_colors[m_mouseColorIndex] = cd.Color;
				OnPaletteChanged(EventArgs.Empty);
				Invalidate();
			}
		}

		/// <summary>
		/// Invoke the PaletteChanged event; called whenever patelle changes:
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPaletteChanged(EventArgs e) 
		{
			if (PaletteChanged != null)
				PaletteChanged(this,e);
		}

		/// <summary>
		/// Invoke the PaletteColorIndexChanged event; called whenever patelle changes:
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPaletteColorIndexChanged(EventArgs e) 
		{
			if (PaletteColorIndexChanged != null)
				PaletteColorIndexChanged(this,e);
		}

		protected override void OnResize ( System.EventArgs e ) 
		{
			base.OnResize(e);
			Invalidate();
		}

		[DefaultValue(false)]
		[Category("Behavior")]
		[Description("Controls whether the colors in the color control can be changed or not.")]
		public bool ReadOnly 
		{
			get {return m_readonly;}
			set 
			{
				m_readonly = value;
			}
		}
	}
}
