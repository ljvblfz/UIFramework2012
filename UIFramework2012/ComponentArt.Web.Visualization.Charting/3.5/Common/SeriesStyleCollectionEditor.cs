using System;
using System.Windows.Forms;
using System.Drawing.Design;

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// SeriesStyle collection editor.
	/// </summary>
	internal class SeriesStyleCollectionEditor : UITypeEditor
	{
		SeriesStyleCollection styles;
		SeriesStylesCollectionDialog dlg;
		public SeriesStyleCollectionEditor()
		{ }
		
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value) 
		{
			WinChart winChart = context.Instance as WinChart;
			
			styles = value as SeriesStyleCollection;
			if(styles==null)
				return value;

			// Clone the collection
			StyleCloner cloner = new StyleCloner();
			SeriesStyleCollection copyOfStyles = new SeriesStyleCollection(styles.Owner,false);
			foreach(SeriesStyle style in styles)
				copyOfStyles.Add((SeriesStyle) cloner.Clone(style));

			// get the editor service.
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
			
			if (edSvc == null) 
				return value;
			
			// create our UI
			if (dlg == null || true) 
			{
				dlg = new SeriesStylesCollectionDialog();
				dlg.Populate(styles);
			}
			dlg.EditorService = edSvc;

			// instruct the editor service to display the modal dialog.
			edSvc.ShowDialog(dlg);
		
			// Check the dialog result
			if(dlg.DialogResult == DialogResult.Cancel)
			{
				bool wasInCollectionEditor = styles.InCollectionEditor;
				styles.InCollectionEditor = true;
				styles.Clear();
				foreach (SeriesStyle style in copyOfStyles)
					styles.Add(style);
				styles.InCollectionEditor = wasInCollectionEditor;
			}
			if(winChart != null)
				winChart.Invalidate();
			return value;
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return System.Drawing.Design.UITypeEditorEditStyle.Modal;
		}
	}
}
