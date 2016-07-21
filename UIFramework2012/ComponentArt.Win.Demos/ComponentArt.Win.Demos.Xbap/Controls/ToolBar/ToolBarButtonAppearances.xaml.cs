using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Win.UI.Navigation;
using System.ComponentModel;

namespace ComponentArt.Win.Demos 
{
    public partial class ToolBarButtonAppearances : UserControl, IDisposable
    {
        public ToolBarButtonAppearances() 
        {
            InitializeComponent();
        }

        private void Appearances_Checked(object sender, RoutedEventArgs e)
        {
            if (ToolBar1 == null) return;

            switch (((RadioButton)sender).Name)
            {
                case "ImageAboveText":
                    ToolBar1.ItemTextImageRelation = ComponentArt.Win.UI.Navigation.ToolBar.ToolBarTextImageRelation.ImageAboveText;
                    break;
                case "TextAboveImage":
                    ToolBar1.ItemTextImageRelation = ComponentArt.Win.UI.Navigation.ToolBar.ToolBarTextImageRelation.TextAboveImage;
                    break;
                case "TextOnly":
                    ToolBar1.ItemTextImageRelation = ComponentArt.Win.UI.Navigation.ToolBar.ToolBarTextImageRelation.TextOnly;
                    break;
                case "ImageOnly":
                    ToolBar1.ItemTextImageRelation = ComponentArt.Win.UI.Navigation.ToolBar.ToolBarTextImageRelation.ImageOnly;
                    break;
                case "ImageBeforeText":
                    ToolBar1.ItemTextImageRelation = ComponentArt.Win.UI.Navigation.ToolBar.ToolBarTextImageRelation.ImageBeforeText;
                    break;
                case "TextBeforeImage":
                    ToolBar1.ItemTextImageRelation = ComponentArt.Win.UI.Navigation.ToolBar.ToolBarTextImageRelation.TextBeforeImage;
                    break;
            }

        }
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
