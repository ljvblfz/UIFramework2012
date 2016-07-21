using System;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class NonResizableControlDesigner : ControlDesigner
	{
		public override SelectionRules SelectionRules 
		{
			get 
			{
				SelectionRules sr = base.SelectionRules;
				sr &= ~(SelectionRules.AllSizeable);
				return sr;
			}
		}
	}
}
