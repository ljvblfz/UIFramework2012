using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ComponentArt.Win.UI.Internal
{
	internal class TrackBar : System.Windows.Forms.TrackBar
	{
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return Color.White;
			}
			set
			{
			}
		}
	}
}
