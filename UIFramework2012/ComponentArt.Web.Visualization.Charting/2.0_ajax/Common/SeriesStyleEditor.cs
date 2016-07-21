using System;
using System.Windows.Forms;
using System.Drawing.Design;

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// This is editor for the "MainStyle" control property. It uses a SeriesStyleTreeView control in a dropdown window.
	/// </summary>
	internal class SeriesStyleEditor : UITypeEditor
	{
		SeriesStyleCollection styles;
		SeriesStyleTreeView view;

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value) 
		{
			styles = null;
			ChartBase chart = ChartBase.GetChartFromObject(context.Instance);
			if(chart != null)
				styles = chart.SeriesStylesX;
			else if (context.Instance is SeriesBase)
				styles = ((SeriesBase)context.Instance).OwningChart.SeriesStyles;
			if(styles == null)
				return value;

			// get the editor service.
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
			
			if (edSvc == null) 
			{
				return value;
			}

			// create our UI
			if (view == null) 
			{
				view = new SeriesStyleTreeView();
				view.BorderStyle = BorderStyle.None;
				view.Scrollable = true;
			}

			view.Populate(styles);
			view.EditorService = edSvc;
			bool userSelect = view.UserSelect;
			view.UserSelect = false;
			if (context.Instance is SeriesBase) 
			{
				view.SelectedStyle = ((SeriesBase)context.Instance).Style;
			} 
			else 
			{
				view.SelectedStyle = (styles.Owner as ChartBase).Series.Style;
			}
			view.UserSelect = userSelect;

			// instruct the editor service to display the control as a dropdown.
			
			edSvc.DropDownControl(view);
		
			// return the updated value;
			if(view.SelectedStyle != null)
				return view.SelectedStyle.Name;
			else
				return value;
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}
	}
}
