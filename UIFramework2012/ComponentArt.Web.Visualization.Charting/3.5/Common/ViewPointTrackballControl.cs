using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for ViewPointTrackballControl.
	/// </summary>
	internal class ViewPointTrackballControl : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		WinChart m_propertyGridViewpointChart;

		public ViewPointTrackballControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.m_propertyGridViewpointChart = new ComponentArt.Web.Visualization.Charting.WinChart();
			m_propertyGridViewpointChart.TrackballEnabled = true;
			m_propertyGridViewpointChart.Size = Size;
			this.m_propertyGridViewpointChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewPointControl_MouseUp);
			this.Controls.AddRange(new System.Windows.Forms.Control[] { this.m_propertyGridViewpointChart });
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

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();
			WinChart.Chart.SetDesignMode(true);
		}

		/// <summary>
		/// When the button is released, we've finished.  Update our selected value
		/// and ask the IWindowsFormsEditorService to close the dropdown.
		/// </summary>
		private void ViewPointControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) 
			{
				if (edSvc != null)
					edSvc.CloseDropDown();
			}
		}

		private IWindowsFormsEditorService edSvc;
		public IWindowsFormsEditorService EditorService 
		{
			get {return edSvc;}
			set {this.edSvc = value;}
		}


		public WinChart WinChart 
		{
			get {return this.m_propertyGridViewpointChart;}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ViewPointTrackballControl
			// 
			this.Name = "ViewPointTrackballControl";
			this.Size = new System.Drawing.Size(150, 116);

		}
		#endregion
	}
}
