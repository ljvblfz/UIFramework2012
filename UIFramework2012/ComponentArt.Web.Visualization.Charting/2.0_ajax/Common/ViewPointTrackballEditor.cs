using System;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for ViewPointTrackballEditor.
	/// </summary>
	internal class ViewPointTrackballEditor : UITypeEditor
	{

		ViewPointTrackballControl ui;
		Vector3D m_viewPoint;

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value) 
		{
			m_viewPoint = (Vector3D)value;

			// get the editor service.
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));

			if (edSvc == null) 
			{
				// uh oh.
				return value;
			}

			// create our UI
			if (ui == null) 
			{
				ui = new ViewPointTrackballControl();
			}

			ChartCloner vpcc = new ChartCloner();
            
            ChartBase origChart;
            if (context.Instance is Mapping)
                origChart = ((Mapping)context.Instance).Chart;
            else
                origChart = ChartBase.GetChartFromObject(context.Instance);

			ui.WinChart.Chart = (ChartBase)vpcc.Clone(origChart);
            ui.WinChart.Chart.Owner = origChart.Owner;
			ui.WinChart.Chart.InWizard = true;
			
			// initialize the ui with the settings for this vertex
			ui.WinChart.Mapping.ViewDirection = m_viewPoint;
			ui.WinChart.Invalidate();
			ui.EditorService = edSvc;

			// instruct the editor service to display the control as a dropdown.
			edSvc.DropDownControl(ui);
			ui.WinChart.Chart.InWizard = false;
		
			Vector3D ret_val = ui.WinChart.Mapping.ViewDirection;

			// Cut out some insignificant digits
			int significantDigits = 2;
			double factor = Math.Pow(10, significantDigits);

			ret_val.X = (double)((int)(ret_val.X * factor))/factor;
			ret_val.Y = (double)((int)(ret_val.Y * factor))/factor;			
			ret_val.Z = (double)((int)(ret_val.Z * factor))/factor;

			// return the updated value;
			return ret_val;
		}

		/// <summary>
		/// Specify that this editor is the drop down style (instead of the [...] popup style)
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}


	}
}
