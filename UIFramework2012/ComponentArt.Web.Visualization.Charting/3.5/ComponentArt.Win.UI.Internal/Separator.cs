using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace ComponentArt.Win.UI.Internal
{
	/// <summary>
	/// Summary description for Separator.
	/// </summary>
	internal class Separator : Control
	{
		public Separator() 
		{
			this.TabStop = false;
		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e ) 
		{
			base.OnPaint(e);
			Pen p = new Pen(Color.FromArgb(204, 204, 204));
			p.DashStyle = DashStyle.Dot;
			e.Graphics.DrawLine(p, 0, 0, Width-1, 0);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
		public bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}
	}
}
