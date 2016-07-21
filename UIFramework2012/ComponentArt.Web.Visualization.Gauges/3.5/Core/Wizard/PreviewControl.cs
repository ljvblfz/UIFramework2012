using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for PreviewControl.
	/// </summary>
	internal class PreviewControl : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Bitmap m_bitmap = null;
		private IDrawableControl m_control = null;
		public IDrawableControl Control
		{
			set
			{
				m_control = value;
				Invalidate();
			}
		}

		public PreviewControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// PreviewControl
			// 
			this.Name = "PreviewControl";

		}
		#endregion

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);

			if(m_bitmap == null)
				return;
			e.Graphics.DrawImage(m_bitmap, 0, 0);
		}

		public new void Invalidate()
		{
			base.Invalidate();
			CreateBitmap();
		}

		private void CreateBitmap()
		{
			if (m_control == null)
				return;
			m_bitmap = m_control.RenderBitmap(Size.Width, Size.Height);
		}

	}
}
