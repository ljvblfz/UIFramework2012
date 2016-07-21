using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

#if !__BUILDING_ComponentArt_Win_UI_Internal__
namespace ComponentArt.Win.UI.Internal
#else
namespace ComponentArt.Win.UI.WinChartSamples
#endif
{
	[ToolboxItem(false)/*, DesignTimeVisible(false)*/]
#if __BUILDING_ComponentArt_Win_UI_Internal__
	public
#else
	internal
#endif		
		class TabPage : Panel
	{
		public TabPage()
		{
		}

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
 
		protected override void OnLocationChanged ( System.EventArgs e ) 
		{
			base.OnLocationChanged(e);
			if (Location != new Point(
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.TabPadding, 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.HeaderHeight+1)) 
			{
				Location = new Point(
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
					ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
					.TabPadding, 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
					ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
					.HeaderHeight+1);
			}
		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs pevent )
		{
			base.OnPaint(pevent);

			if (DesignMode) 
			{
				WaterMark.Draw(pevent.Graphics, this, "ComponentArt TabControl", "for use within the charting product only");
			}
		}

		protected override void OnResize ( System.EventArgs e )
		{
			base.OnResize(e);

			if (Parent != null && Size != new Size( Parent.Size.Width - 2 * 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.TabPadding, Parent.Size.Height - 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.HeaderHeight - 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.TabPadding-1)) 
			{
				Size = new Size( Parent.Size.Width - 2 * 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
					ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
					.TabPadding, Parent.Size.Height - 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
					ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
					.HeaderHeight - 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
					ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
					.TabPadding-1);
			}
		}
	}
}
