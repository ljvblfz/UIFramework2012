using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for Vector3DControl.
	/// </summary>
	[DefaultEvent("VectorChanged")]
	internal class Vector3DTextBoxesControlHorizontal : WizardElementWithHint
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		[WizardHint("Vector3DTextBoxesControlHorizontalY")]
		private System.Windows.Forms.TextBox textBoxY;
		[WizardHint("Vector3DTextBoxesControlHorizontalZ")]
		private System.Windows.Forms.TextBox textBoxZ;
		[WizardHint("Vector3DTextBoxesControlHorizontalX")]
		private System.Windows.Forms.TextBox textBoxX;
		private ComponentArt.Win.UI.Internal.BorderPanel borderPanel1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Vector3DTextBoxesControlHorizontal()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_defaultSize = Size;
			m_sizeset = true;
			
		}

		bool m_sizeset = false;
		Size m_defaultSize;

		protected override void OnResize ( System.EventArgs e ) 
		{
			base.OnResize(e);
			if (m_sizeset && Size != m_defaultSize) 
				Size = m_defaultSize;
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


		Vector3D m_vector = new Vector3D(0,0,0);

		public Vector3D Vector 
		{
			get 
			{
				return m_vector;
			}
			set 
			{
				Vector3D oldVector = m_vector;
				m_vector = value;

				if (m_vector.X != oldVector.X)
					textBoxX.Text = m_vector.X.ToString("0.0");
				if (m_vector.Y != oldVector.Y)
					textBoxY.Text = m_vector.Y.ToString("0.0");
				if (m_vector.Z != oldVector.Z)
					textBoxZ.Text = m_vector.Z.ToString("0.0");
			}
		}
		
	

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxY = new System.Windows.Forms.TextBox();
			this.textBoxZ = new System.Windows.Forms.TextBox();
			this.textBoxX = new System.Windows.Forms.TextBox();
			this.borderPanel1 = new ComponentArt.Win.UI.Internal.BorderPanel();
			this.borderPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(128, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(16, 16);
			this.label3.TabIndex = 26;
			this.label3.Text = "Z:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(64, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 16);
			this.label2.TabIndex = 25;
			this.label2.Text = "Y:";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(0, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 16);
			this.label1.TabIndex = 24;
			this.label1.Text = "X:";
			// 
			// textBoxY
			// 
			this.textBoxY.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxY.Location = new System.Drawing.Point(80, 3);
			this.textBoxY.Name = "textBoxY";
			this.textBoxY.Size = new System.Drawing.Size(30, 13);
			this.textBoxY.TabIndex = 22;
			this.textBoxY.Text = "0";
			this.textBoxY.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
			// 
			// textBoxZ
			// 
			this.textBoxZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxZ.Location = new System.Drawing.Point(144, 3);
			this.textBoxZ.Name = "textBoxZ";
			this.textBoxZ.Size = new System.Drawing.Size(30, 13);
			this.textBoxZ.TabIndex = 23;
			this.textBoxZ.Text = "0";
			this.textBoxZ.TextChanged += new System.EventHandler(this.textBoxZ_TextChanged);
			// 
			// textBoxX
			// 
			this.textBoxX.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxX.Location = new System.Drawing.Point(16, 3);
			this.textBoxX.Name = "textBoxX";
			this.textBoxX.Size = new System.Drawing.Size(30, 13);
			this.textBoxX.TabIndex = 21;
			this.textBoxX.Text = "0";
			this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
			// 
			// borderPanel1
			// 
			this.borderPanel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.label2,
																					   this.textBoxZ,
																					   this.label3,
																					   this.label1,
																					   this.textBoxY,
																					   this.textBoxX});
			this.borderPanel1.Name = "borderPanel1";
			this.borderPanel1.Size = new System.Drawing.Size(178, 20);
			this.borderPanel1.TabIndex = 57;
			// 
			// Vector3DTextBoxesControlHorizontal
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.borderPanel1});
			this.Name = "Vector3DTextBoxesControlHorizontal";
			this.Size = new System.Drawing.Size(178, 20);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Vector3DTextBoxesControlHorizontal_Paint);
			this.borderPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void textBoxZ_TextChanged(object sender, System.EventArgs e)
		{
			if (textBoxZ.Text != "") 
			{
				m_vector.Z = double.Parse(textBoxZ.Text);
				OnVectorChanged(EventArgs.Empty);
			}
		}

		private void textBoxY_TextChanged(object sender, System.EventArgs e)
		{
			if (textBoxY.Text != "") 
			{
				m_vector.Y = double.Parse(textBoxY.Text);
				OnVectorChanged(EventArgs.Empty);
			}
		}

		private void textBoxX_TextChanged(object sender, System.EventArgs e)
		{
			if (textBoxX.Text != "") 
			{
				m_vector.X = double.Parse(textBoxX.Text);
				OnVectorChanged(EventArgs.Empty);
			}
		}


		public event EventHandler VectorChanged;

		/// <summary>
		/// Invoke the Changed event; called whenever list changes:
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnVectorChanged(EventArgs e) 
		{
			if (VectorChanged != null)
				VectorChanged(this,e);
		}

		private void Vector3DTextBoxesControlHorizontal_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.Clear(Color.White);
		}
	}
}
