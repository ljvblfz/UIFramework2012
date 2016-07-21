using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ComponentArt.WinUI
{
#if DEBUG
	public
#else
	internal
#endif
	class TrackBar : System.Windows.Forms.TrackBar
	{
		//Skin m_skin = Skin.DefaultSkin;

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				//return m_skin.BackColor;
				return Color.White;
			}
			set
			{
			}
		}
	}
}
