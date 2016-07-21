using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal class AboutDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Rectangle rec = new Rectangle(464,168,64,16); // "close box"

		private bool cursorIn = false;

		public AboutDialog(string text)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			label1.Text = text;
			this.BackgroundImage = new Bitmap(GetType(),"gauge-About.png");
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label1.Location = new System.Drawing.Point(13, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(512, 72);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// AboutDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 198);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutDialog_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AboutDialog_MouseDown);
			this.MouseHover += new System.EventHandler(this.AboutDialog_MouseHover);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AboutDialog_MouseMove);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void AboutDialog_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int x = e.X;
			int y = e.Y;
			
			if(rec.X < x && x < rec.X + rec.Width && rec.Y < y && y < rec.Y + rec.Height)
				Close();
		}

		private void AboutDialog_MouseHover(object sender, System.EventArgs e)
		{
			
		}

		private void AboutDialog_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Escape)
				Close();
		}

		private void AboutDialog_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int x = e.X;
			int y = e.Y;
			
			if(rec.X < x && x < rec.X + rec.Width && rec.Y < y && y < rec.Y + rec.Height)
			{
				if(!cursorIn)
					this.Cursor = Cursors.Hand;
				cursorIn = true;
			}
			else
			{
				if(cursorIn)
					this.Cursor = Cursors.Default;
				cursorIn = false;
			}
	
		}
	}
}
