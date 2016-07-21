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
	/// Summary description for StateColorDialog.
	/// </summary>
#if DEBUG
	public
#else
	internal
#endif
	class StateColorDialog : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ComponentArt.WinUI.Button buttonAddColorStop;
		private ComponentArt.Web.Visualization.Gauges.Core.WinUI.StateColorStopList colorStopList1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

		private StateColorCollection m_stateColor = new StateColorCollection();

		public event EventHandler OnChanged;

		// gray drawing colors
		private Color gray1 = Color.FromArgb(170,170,170);
		private Color gray2 = Color.FromArgb(204,204,204);
		private Color gray3 = Color.FromArgb(221,221,221);

		public StateColorDialog()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			//this.BackColor = Color.FromArgb(238,238,238);

			colorStopList1.OnChanged += new EventHandler(HandleChangedColorStops);
			buttonAddColorStop.Click += new EventHandler(HandleAddButtonClick);

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

		public StateColorCollection StateColor
		{
			get
			{
				return m_stateColor;
			}
			set
			{
				m_stateColor = value;
				UpdateControls();
			}
		}

		private void UpdateControls()
		{
			colorStopList1.ColorStops = m_stateColor;
			ShowHideHelpText();
		}

		private void HandleChangedColorStops(object sender, EventArgs ea)
		{
			m_stateColor.Clear();
			StateColorCollection colorStops = colorStopList1.ColorStops;
			foreach(StateColor sc in colorStops)
			{
				m_stateColor.Add(sc);
			}
			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
			Invalidate();
			ShowHideHelpText();
		}

		private void HandleAddButtonClick(object sender, EventArgs ea)
		{
			colorStopList1.AddNewStop();
			ShowHideHelpText();
		}

		private void ShowHideHelpText()
		{
			if (colorStopList1.ColorStops.Count == 0 && colorStopList1.Controls.Count == 0)
			{
				this.label3.Visible = true;
			}
			else
			{
				this.label3.Visible = false;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonAddColorStop = new ComponentArt.WinUI.Button();
			this.colorStopList1 = new ComponentArt.Web.Visualization.Gauges.Core.WinUI.StateColorStopList();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonAddColorStop
			// 
			this.buttonAddColorStop.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.buttonAddColorStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.buttonAddColorStop.Location = new System.Drawing.Point(72, 114);
			this.buttonAddColorStop.Name = "buttonAddColorStop";
			this.buttonAddColorStop.Size = new System.Drawing.Size(80, 18);
			this.buttonAddColorStop.TabIndex = 5;
			this.buttonAddColorStop.Text = "Add a State";
			// 
			// colorStopList1
			// 
			this.colorStopList1.AutoScroll = true;
			this.colorStopList1.Location = new System.Drawing.Point(4, 24);
			this.colorStopList1.Name = "colorStopList1";
			this.colorStopList1.Size = new System.Drawing.Size(154, 85);
			this.colorStopList1.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(52, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Color";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "No states available.";
			// 
			// StateColorDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.colorStopList1,
																		  this.buttonAddColorStop});
			this.Name = "StateColorDialog";
			this.Size = new System.Drawing.Size(160, 136);
			this.Resize += new System.EventHandler(this.StateColorDialog_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.StateColorDialog_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		private void StateColorDialog_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rect = ClientRectangle;
			Pen pen = new Pen(gray2,1);
			e.Graphics.DrawRectangle(pen,0,0,rect.Width-1,rect.Height-1);
			pen.Dispose();
		}

		private void StateColorDialog_Resize(object sender, System.EventArgs e)
		{
			// Color stop list layout
			this.colorStopList1.Width = this.Width - 10;
			this.colorStopList1.Height = this.Height - 48;
//			this.colorStopList1.Location = new Point(3, this.Height - 3 - colorStopList1.Height);
		}
	}
}
