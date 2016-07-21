using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	[DefaultEvent("ColorChanged")]
	internal class ColorControlWithDots : System.Windows.Forms.UserControl
	{
		private Design.ColorControl m_controlColor;
		private ComponentArt.Win.UI.Internal.Button m_dotsButton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;

		public ColorControlWithDots()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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
			this.m_controlColor = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_dotsButton = new ComponentArt.Win.UI.Internal.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_controlColor
			// 
			this.m_controlColor.Color = System.Drawing.SystemColors.Control;
			this.m_controlColor.Name = "m_controlColor";
			this.m_controlColor.ReadOnly = true;
			this.m_controlColor.Size = new System.Drawing.Size(24, 12);
			this.m_controlColor.TabIndex = 11;
			this.m_controlColor.Text = "colorControl1";
			this.m_controlColor.ColorChanged += new System.EventHandler(this.m_controlColor_ColorChanged);
			// 
			// m_dotsButton
			// 
			this.m_dotsButton.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_dotsButton.Location = new System.Drawing.Point(-1, -1);
			this.m_dotsButton.Name = "m_dotsButton";
			this.m_dotsButton.Size = new System.Drawing.Size(20, 14);
			this.m_dotsButton.TabIndex = 12;
			this.m_dotsButton.Text = "...";
			this.m_dotsButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.m_dotsButton.TextLocation = new System.Drawing.Point(5, 2);
			this.m_dotsButton.Click += new System.EventHandler(this.m_dotsButton_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.m_dotsButton});
			this.panel1.Location = new System.Drawing.Point(25, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(18, 12);
			this.panel1.TabIndex = 13;
			// 
			// ColorControlWithDots
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.m_controlColor});
			this.Name = "ColorControlWithDots";
			this.Size = new System.Drawing.Size(43, 12);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_dotsButton_Click(object sender, System.EventArgs e)
		{
			m_controlColor.PerformClick();
		}

		public event EventHandler ColorChanged;

		protected virtual void OnColorChanged(EventArgs e) 
		{
			if (ColorChanged != null)
				ColorChanged(this, e);
		}


		public Color Color 
		{
			get {return m_controlColor.Color;}
			set {m_controlColor.Color = value; Invalidate();}
		}


		protected override Size DefaultSize 
		{
			get 
			{
				return new Size(43, 12);
			}
		}


		private void m_controlColor_ColorChanged(object sender, System.EventArgs e)
		{
			OnColorChanged(EventArgs.Empty);
		}
	}
}
