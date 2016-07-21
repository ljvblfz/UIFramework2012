using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

#if !__BUILDING_ComponentArt_Win_UI_Internal__
namespace ComponentArt.Win.UI.Internal
#else
namespace ComponentArt.Win.UI.WinChartSamples
#endif
{
#if !__BUILDING_FOR_SAMPLES__
	internal
#else
	public
#endif
	class UserControl : System.Windows.Forms.UserControl
	{

		public UserControl() 
		{
			base.BackColor = Color.FromArgb(243,243,243);
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException("Cannot set background image on a(n) '" + this.GetType().FullName + "'");
			}
		}
	}
}
