using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges.Core.WinUI
{
	/// <summary>
	/// Summary description for ColorItemControl.
	/// </summary>
#if DEBUG
	public
#else
	internal
#endif
		class StateColorStopControl : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.Button buttonColorDialog;
		private System.ComponentModel.IContainer components;


		private System.Windows.Forms.TextBox textBoxPosition;
		private System.Windows.Forms.TextBox textBoxColor;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolTip toolTip2;
		private ComponentArt.WinUI.Button buttonDelete;

		public event EventHandler OnChange;
		public event EventHandler OnDelete;

		private StateColor colorStop;

		public StateColorStopControl()
		{
			// This call is required by the Windows.Forms Form Designer.

			InitializeComponent();
			DoControlLayout();
			ColorStop = new StateColor("state0", Color.Green);

			toolTip1.SetToolTip(buttonDelete,"Delete state");
			toolTip2.SetToolTip(buttonColorDialog,"Select color");
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
			this.components = new System.ComponentModel.Container();
			this.textBoxColor = new System.Windows.Forms.TextBox();
			this.buttonColorDialog = new ComponentArt.WinUI.Button();
			this.buttonDelete = new ComponentArt.WinUI.Button();
			this.textBoxPosition = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// textBoxColor
			// 
			this.textBoxColor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxColor.Location = new System.Drawing.Point(39, 2);
			this.textBoxColor.Name = "textBoxColor";
			this.textBoxColor.Size = new System.Drawing.Size(72, 13);
			this.textBoxColor.TabIndex = 2;
			this.textBoxColor.Text = "";
			this.textBoxColor.Leave += new System.EventHandler(this.textBoxColor_Leave);
			// 
			// buttonColorDialog
			// 
			this.buttonColorDialog.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.buttonColorDialog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonColorDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.buttonColorDialog.Location = new System.Drawing.Point(125, 0);
			this.buttonColorDialog.Name = "buttonColorDialog";
			this.buttonColorDialog.Size = new System.Drawing.Size(20, 17);
			this.buttonColorDialog.TabIndex = 3;
			this.buttonColorDialog.Text = "...";
			this.buttonColorDialog.TextLocation = new System.Drawing.Point(4, -2);
			this.buttonColorDialog.Click += new System.EventHandler(this.buttonColorDialog_Click);
			// 
			// buttonDelete
			// 
			this.buttonDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.buttonDelete.Location = new System.Drawing.Point(147, 0);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new System.Drawing.Size(20, 17);
			this.buttonDelete.TabIndex = 4;
			this.buttonDelete.Text = "x";
			this.buttonDelete.TextLocation = new System.Drawing.Point(5, 0);
			this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
			// 
			// textBoxPosition
			// 
			this.textBoxPosition.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxPosition.Location = new System.Drawing.Point(2, 2);
			this.textBoxPosition.Name = "textBoxPosition";
			this.textBoxPosition.Size = new System.Drawing.Size(35, 13);
			this.textBoxPosition.TabIndex = 1;
			this.textBoxPosition.Text = "";
			this.textBoxPosition.Leave += new System.EventHandler(this.textBoxPosition_Leave);
			// 
			// ColorStopControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBoxPosition,
																		  this.textBoxColor,
																		  this.buttonColorDialog,
																		  this.buttonDelete});
			this.Name = "ColorStopControl";
			this.Size = new System.Drawing.Size(168, 16);
			this.Resize += new System.EventHandler(this.ColorItemControl_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorItemControl_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		internal void DisableDeleteAndValueEdit()
		{
			textBoxPosition.Enabled = false;
			buttonDelete.Enabled = false;
		}

		private void DoControlLayout()
		{
			int w = Size.Width;
			if(w <= 0)
				return;
			Size btnSize = buttonColorDialog.Size;

			buttonDelete.Location = new Point(w-buttonDelete.Width-1,0);
			buttonColorDialog.Location = new Point(buttonDelete.Location.X - btnSize.Width,0);

			int textH = textBoxPosition.Size.Height;
			textBoxPosition.Location = new Point(2,2);
			textBoxPosition.Size = new Size(35,textH);
			textBoxColor.Location = new Point(textBoxPosition.Location.X + textBoxPosition.Width + 3,2);
			textBoxColor.Size = new Size(buttonColorDialog.Location.X - textBoxColor.Location.X - 3,2);

		}
		private void buttonColorDialog_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.AllowFullOpen = true;
			dlg.AnyColor = true;
			dlg.FullOpen = true;
			dlg.Color = colorStop.Color;
			if(dlg.ShowDialog(this) == DialogResult.OK)
			{
				ColorStop = new StateColor(colorStop.Name, dlg.Color);
				if(OnChange != null)
					OnChange(this,EventArgs.Empty);
				Invalidate();
			}
		}

		private void textBoxPosition_Leave(object sender, System.EventArgs e)
		{
			try
			{
				//double position = double.Parse(textBoxPosition.Text);
				if(textBoxPosition.Text != colorStop.Name)
				{
					colorStop.Name = textBoxPosition.Text;
					if(OnChange != null)
						OnChange(this,EventArgs.Empty);
				}
			}
			catch
			{
				MessageBox.Show("Invalid data format");
				return;
			}
		}

		private void textBoxColor_Leave(object sender, System.EventArgs e)
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color));
			Color color =  (Color)tc.ConvertFrom(textBoxColor.Text);
			if(color != colorStop.Color)
			{
				this.ColorStop = new StateColor(colorStop.Name, color);
				if(OnChange != null)
					OnChange(this,EventArgs.Empty);
			}
		}

		private void ColorItemControl_Resize(object sender, System.EventArgs e)
		{
			DoControlLayout();
		}

		private void buttonDelete_Click(object sender, System.EventArgs e)
		{
			if(OnDelete != null)
				OnDelete(this,EventArgs.Empty);
		}

		private void ColorItemControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Pen p1 = new Pen(Color.FromArgb(170,170,170));
			Pen p2 = new Pen(Color.FromArgb(221,221,221));
			int x0,y0,x1,y1;
			// Position box
			x0 = 0; y0 = 0; x1 = x0+textBoxPosition.Width+2; y1 = y0 + textBoxPosition.Height + 2;
			e.Graphics.DrawLine(p1,x0,y0,x0,y1);
			e.Graphics.DrawLine(p1,x0,y0,x1,y0);
			e.Graphics.DrawLine(p2,x0+1,y1,x1,y1);
			e.Graphics.DrawLine(p2,x1,y0+1,x1,y1);
			// Position box
			x0 = textBoxColor.Left-1; y0 = textBoxColor.Top-2; x1 = x0+textBoxColor.Width+2; y1 = y0 + textBoxColor.Height + 2;
			e.Graphics.DrawLine(p1,x0,y0,x0,y1);
			e.Graphics.DrawLine(p1,x0,y0,x1,y0);
			e.Graphics.DrawLine(p2,x0+1,y1,x1,y1);
			e.Graphics.DrawLine(p2,x1,y0+1,x1,y1);

			p1.Dispose();
			p2.Dispose();
		}

		public StateColor ColorStop
		{
			get
			{
				return colorStop;
			}
			set
			{
				this.colorStop = value;
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color));
				this.textBoxColor.Text = tc.ConvertToString(colorStop.Color);
				this.buttonColorDialog.BackColor = colorStop.Color;
				this.buttonColorDialog.Invalidate();
				this.textBoxPosition.Text = colorStop.Name;
			}
		}
	}
}
