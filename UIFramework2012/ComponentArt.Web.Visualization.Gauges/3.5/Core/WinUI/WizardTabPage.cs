using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ComponentArt.WinUI
{
	[ToolboxItem(false)/*, DesignTimeVisible(false)*/]
#if DEBUG
	public
#else
	internal
#endif		
		class TabPage : Panel
	{
		public TabPage()
		{

		}

		//[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), Bindable(false)]
		[Localizable(true), Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}
 
//		protected override void OnLocationChanged ( System.EventArgs e ) 
//		{
//			base.OnLocationChanged(e);
//			if (Location != new Point(ComponentArt.WinUI.TabControl.TabPadding, 
//				ComponentArt.WinUI.TabControl.HeaderHeight+1)) 
//			{
//				Location = new Point(ComponentArt.WinUI.TabControl.TabPadding, 
//					ComponentArt.WinUI.TabControl.HeaderHeight+1);
//			}
//		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs pevent )
		{
			base.OnPaint(pevent);
		}

//		protected override void OnResize ( System.EventArgs e )
//		{
//			base.OnResize(e);
//
//			if (Parent != null && Size != new Size( Parent.Size.Width - 2 * ComponentArt.WinUI.TabControl
//				.TabPadding, Parent.Size.Height - ComponentArt.WinUI.TabControl
//				.HeaderHeight - ComponentArt.WinUI.TabControl.TabPadding-1)) 
//			{
//				Size = new Size( Parent.Size.Width - 2 * ComponentArt.WinUI.TabControl.TabPadding, 
//					Parent.Size.Height - ComponentArt.WinUI.TabControl.HeaderHeight - 
//					ComponentArt.WinUI.TabControl.TabPadding-1);
//			}
//		}
	}
}
