using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace ComponentArt.Silverlight.Demos
{
	public partial class IntroMenuBar : UserControl
    {
		internal PageNew pn;
		
        public IntroMenuBar()
		{
            InitializeComponent();
        }
		
		public void BeginAnimation()
		{
			Storyboard oAnimation = (Storyboard)this.Resources["RootStoryboard"];
      		oAnimation.Begin();
		}
		
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            if (pn != null)
                pn.IntroMenuBarComplete();
        }
    }
}