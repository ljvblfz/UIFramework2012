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

namespace ComponentArt.Win.Demos
{
	public partial class IntroContent : UserControl
    {
		internal PageNew pn;
		
        public IntroContent()
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
                pn.IntroContentComplete();
        }

        public void ResetStoryboard()
        {
            Storyboard oAnimation = (Storyboard)this.Resources["RootStoryboard"];
            oAnimation.Seek(new TimeSpan(0));
        }
    }
}