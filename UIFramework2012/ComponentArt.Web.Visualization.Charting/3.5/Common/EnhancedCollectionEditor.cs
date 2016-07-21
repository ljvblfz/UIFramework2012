using System;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class EnhancedCollectionEditor : CollectionEditor
	{
		public EnhancedCollectionEditor(Type type): base(type)
		{
		}

		protected CollectionForm m_collectionForm; 

		/// <summary>
		/// Overriden, so that we can change the properties of thePropertyGrid. This is not done
		/// via Reflection.
		/// </summary>
		/// <returns>The form</returns>
		protected override CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm form = base.CreateCollectionForm();

			foreach (Control c in form.Controls)
			{
				if (c.GetType() == typeof(PropertyGrid))
				{
					PropertyGrid propertyGrid =(PropertyGrid)c;
					if (propertyGrid != null)
					{
						propertyGrid.ToolbarVisible = true;
						propertyGrid.HelpVisible = true;
						propertyGrid.BackColor = System.Drawing.SystemColors.Control;
					}
					break;
				}
			}
			return form;
		}
		
		/// <summary>
		/// Overriden, so that we can store the reference to collection
		/// </summary>
		/// 
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if(m_collectionForm != null && m_collectionForm.Visible) 
			{ 
				// If the CollectionForm is already visible, then create a new instance 
				// of the editor and delegate this call to it. 
				ConstructorInfo ci = this.GetType().GetConstructor(new Type[] {typeof(System.Type)});
				CollectionWithTypeEditor editor = (CollectionWithTypeEditor)ci.Invoke(new object [] {this.CollectionType});
				return editor.EditValue(context, provider, value); 
			}

			return base.EditValue(context, provider, value);
		}
	}
}
