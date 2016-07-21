using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardPropertyGridForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PropertyGrid m_propertyGrid;
		private ComponentArt.Win.UI.Internal.Button m_OKButton;
		private System.ComponentModel.Container components = null;

		public WizardPropertyGridForm()
		{
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

		internal PropertyGrid PropertyGrid 
		{
			get 
			{
				return m_propertyGrid;
			}
		}

		protected override void OnVisibleChanged ( System.EventArgs e ) 
		{
			base.OnVisibleChanged(e);
			if (!Visible) 
				return;

			try 
			{
				Point p = RegistryValues.WizardPropertyGridFormLocation;
				StartPosition = FormStartPosition.Manual;
				Location = p;
			} 
			catch
			{
			}
		}


		protected override void OnClosed ( System.EventArgs e ) 
		{
			base.OnClosed(e);
			RegistryValues.WizardPropertyGridFormLocation = Location;
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.m_OKButton = new ComponentArt.Win.UI.Internal.Button();
			this.SuspendLayout();
			// 
			// m_propertyGrid
			// 
			this.m_propertyGrid.CommandsVisibleIfAvailable = true;
			this.m_propertyGrid.LargeButtons = false;
			this.m_propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.m_propertyGrid.Name = "m_propertyGrid";
			this.m_propertyGrid.Size = new System.Drawing.Size(290, 296);
			this.m_propertyGrid.TabIndex = 0;
			this.m_propertyGrid.Text = "propertyGrid1";
			this.m_propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.m_propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// m_OKButton
			// 
			this.m_OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_OKButton.Location = new System.Drawing.Point(96, 304);
			this.m_OKButton.Name = "m_OKButton";
			this.m_OKButton.Size = new System.Drawing.Size(104, 24);
			this.m_OKButton.TabIndex = 42;
			this.m_OKButton.Text = "OK";
			// 
			// WizardPropertyGridForm
			// 
			this.AcceptButton = this.m_OKButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_OKButton;
			this.ClientSize = new System.Drawing.Size(290, 336);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_OKButton,
																		  this.m_propertyGrid});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "WizardPropertyGridForm";
			this.Text = "WizardPropertyGrid";
			this.ResumeLayout(false);

		}
		#endregion

	}
}
