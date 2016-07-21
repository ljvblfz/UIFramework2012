using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges.Core.WinUI
{
	/// <summary>
	/// Summary description for MultiColorDialog.
	/// </summary>
#if DEBUG
	public
#else
	internal
#endif
	class MultiColorDialog : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkBoxSolid;
		private ComponentArt.Web.Visualization.Gauges.Core.WinUI.ColorStopList colorStopList1;
		private System.Windows.Forms.Panel panel1;

		private MultiColor mc = new MultiColor();

		// gray drawing colors
		private Color gray1 = Color.FromArgb(170,170,170);
		private Color gray2 = Color.FromArgb(204,204,204);
		private Color gray3 = Color.FromArgb(221,221,221);

		public event EventHandler OnChanged;

		public MultiColorDialog()
		{
			this.BackColor = Color.White;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			colorStopList1.OnChanged += new EventHandler(HandleChangedColorStops);
			checkBoxSolid.CheckedChanged += new EventHandler(HandleSolidChanged);

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

		public MultiColor MultiColor
		{
			get
			{
				return mc;
			}
			set
			{
				mc = value;
				UpdateControls();
			}
		}

		private void UpdateControls()
		{
			colorStopList1.ColorStops = mc.ColorStops;
			checkBoxSolid.Checked = mc.Solid;
		}

		private void HandleChangedColorStops(object sender, EventArgs ea)
		{
			mc.ColorStops = colorStopList1.ColorStops;
			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
			panel1.Invalidate();
			Invalidate();
		}

		private void HandleSolidChanged(object sender, EventArgs ea)
		{
			mc.Solid = checkBoxSolid.Checked;
			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
			panel1.Invalidate();
			Invalidate();
		}

		private void HandleAddButtonClick(object sender, EventArgs ea)
		{
			//colorStopList1.AddNewStop();
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkBoxSolid = new System.Windows.Forms.CheckBox();
			this.colorStopList1 = new ComponentArt.Web.Visualization.Gauges.Core.WinUI.ColorStopList();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// checkBoxSolid
			// 
			this.checkBoxSolid.Location = new System.Drawing.Point(104, 7);
			this.checkBoxSolid.Name = "checkBoxSolid";
			this.checkBoxSolid.Size = new System.Drawing.Size(52, 16);
			this.checkBoxSolid.TabIndex = 3;
			this.checkBoxSolid.Text = "Solid";
			// 
			// colorStopList1
			// 
			this.colorStopList1.AutoScroll = true;
			this.colorStopList1.ColorStops = new ComponentArt.Web.Visualization.Gauges.ColorStopCollection("Empty");
			this.colorStopList1.Location = new System.Drawing.Point(8, 28);
			this.colorStopList1.Name = "colorStopList1";
			this.colorStopList1.Size = new System.Drawing.Size(144, 100);
			this.colorStopList1.TabIndex = 6;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.panel1.Location = new System.Drawing.Point(6, 6);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(98, 17);
			this.panel1.TabIndex = 7;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// MultiColorDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.colorStopList1,
																		  this.checkBoxSolid});
			this.Name = "MultiColorDialog";
			this.Size = new System.Drawing.Size(160, 136);
			this.Resize += new System.EventHandler(this.MultiColorDialog_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MultiColorDialog_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		private void MultiColorDialog_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rect = ClientRectangle;
			Pen pen = new Pen(gray2,1);
			e.Graphics.DrawRectangle(pen,0,0,rect.Width-1,rect.Height-1);
			pen.Dispose();
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Color[] colors;
			float[] positions;
			mc.GetColorsAndPositions(out colors,out positions);
			if(colors == null || colors.Length<2)
				return;
			// Gradient/color
			Rectangle r = panel1.ClientRectangle;
			Point startPoint = new Point(r.Left,0);
			Point endPoint = new Point(r.Right,0);
			LinearGradientBrush lgb = new LinearGradientBrush(startPoint,endPoint,Color.White, Color.Black);
			ColorBlend blend = new ColorBlend();
			blend.Colors = colors;
			blend.Positions = positions;
			lgb.InterpolationColors = blend;
			e.Graphics.FillRectangle(lgb,r);
			lgb.Dispose();
			// Color Strip Frame
			Pen penDark = new Pen(gray1,1);
			Pen penLight = new Pen(gray3,1);
			e.Graphics.DrawLine(penDark,0,0,0,r.Height);
			e.Graphics.DrawLine(penDark,0,0,r.Width,0);
			e.Graphics.DrawLine(penLight,1,r.Height-1,r.Width,r.Height-1);
			e.Graphics.DrawLine(penLight,r.Width-1,0,r.Width-1,r.Height-1);
			penDark.Dispose();
			penLight.Dispose();
		}

		private void MultiColorDialog_Resize(object sender, System.EventArgs e)
		{
			// Checkbox layout
			checkBoxSolid.Location = new Point(this.Width - checkBoxSolid.Width - 1, checkBoxSolid.Location.Y);
			// Color strip layout
			panel1.Width = checkBoxSolid.Location.X - panel1.Location.X - 6;
			// Color stop list layout
			this.colorStopList1.Width = this.Width - 10;
			this.colorStopList1.Height = this.Height - 17 - this.panel1.Height;
			this.colorStopList1.Location = new Point(5,this.Height-5-colorStopList1.Height);
		}
	}
}
