using System;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for CollectionListBox.
	/// </summary>
	internal class WizardCollectionListBox : ComponentArt.Win.UI.Internal.ListBox
	{
		public WizardCollectionListBox()
		{
			DisplayMember = "Name";
		}

		protected override void OnDisplayMemberChanged ( System.EventArgs e ) 
		{
			base.OnDisplayMemberChanged(e);

			if (DisplayMember != "Name") 
			{
				DisplayMember = "Name";
			}
		}
	}
}
