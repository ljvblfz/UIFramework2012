using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	[DefaultEvent("ColorChanged")]
	internal class ColorControl : Control
	{
		
		public ColorControl() 
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true); 
		}

		bool m_readonly = false;
		
		/// <summary>
		/// Calls a color selection dialog
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseDown(e);
			
			if (ReadOnly)
				return;

			if (e.Button == MouseButtons.Left) 
			{
				PerformClick();
			}
		}

		public void PerformClick()
		{
			ColorDialog cd = new ColorDialog();
			cd.Color = BackColor;
			DialogResult dr = cd.ShowDialog();
			Color = cd.Color;
		}


		public event EventHandler ColorChanged;

		protected virtual void OnColorChanged(EventArgs e) 
		{
			if (ColorChanged != null)
				ColorChanged(this, e);
		}

		public Color Color 
		{
			get {return BackColor;}
			set 
			{
				if (BackColor == value)
					return;
				
				BackColor = value;

				OnColorChanged(EventArgs.Empty);
				Invalidate();
			}
		}
		
	
		[DefaultValue(false)]
		[Category("Behavior")]
		[Description("Controls whether the color in the color control can be changed or not.")]
		public bool ReadOnly 
		{
			get {return m_readonly;}
			set 
			{
				m_readonly = value;
			}
		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e ) 
		{
			base.OnPaint(e);

			if (this.DesignMode) 
			{
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				e.Graphics.DrawLine(new Pen(Color.Blue), e.Graphics.VisibleClipBounds.Width, e.Graphics.VisibleClipBounds.Height, 0, 0);
				e.Graphics.DrawLine(new Pen(Color.Blue), e.Graphics.VisibleClipBounds.Width, 0, 0, e.Graphics.VisibleClipBounds.Height);
			} 
			else 
			{
				e.Graphics.DrawRectangle(new Pen(Color.Black), e.Graphics.VisibleClipBounds.X, e.Graphics.VisibleClipBounds.Y, e.Graphics.VisibleClipBounds.Width-1, e.Graphics.VisibleClipBounds.Height-1);
			}
		}


		protected override void OnResize ( System.EventArgs e ) 
		{
			base.OnResize(e);
			Invalidate();
		}
	}
}
