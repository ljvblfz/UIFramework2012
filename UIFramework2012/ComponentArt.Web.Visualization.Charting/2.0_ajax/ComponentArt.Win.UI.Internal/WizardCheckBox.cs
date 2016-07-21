using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace ComponentArt.Win.UI.Internal
{
	/// <summary>
	/// Summary description for WizardCheckBox.
	/// </summary>
	internal class CheckBox : System.Windows.Forms.CheckBox
	{
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
			}
		}
    }
}
