using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class NamedConstCollectionEditor : NamedCollectionEditor
	{
		public NamedConstCollectionEditor(Type type): base(type) {}

		protected override CollectionForm CreateCollectionForm() 
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			
			m_removeB.Enabled = false;
			m_copyB.Enabled = false;
			m_addB.Enabled = false;
			
			m_removeB.EnabledChanged += new EventHandler(ButtonNeedsToBeDisabled);
			m_addB.EnabledChanged += new EventHandler(ButtonNeedsToBeDisabled);

			if (addB_copy != null) 
				addB_copy.Enabled = false;

			if (m_dropB != null) 
				m_dropB.Enabled = false;
			
			return collectionForm;
		
		}

		void ButtonNeedsToBeDisabled(object sender, EventArgs e) 
		{
			if (((ButtonBase)sender).Enabled == true)
				((ButtonBase)sender).Enabled = false;
		}
	}
}
