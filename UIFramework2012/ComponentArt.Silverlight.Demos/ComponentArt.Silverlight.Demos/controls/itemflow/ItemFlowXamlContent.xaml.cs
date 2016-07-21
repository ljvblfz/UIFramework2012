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
using ComponentArt.Silverlight.UI.Utils;

namespace ComponentArt.Silverlight.Demos
{
  public partial class ItemFlowXamlContent : UserControl
  {
      public ItemFlowXamlContent()
    {
      InitializeComponent();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
    }

    private void Previous_Click(object sender, RoutedEventArgs e)
    {
        myItemFlow.Previous();
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
        myItemFlow.Next();
    }

    private void refresh(object sender, RoutedEventArgs e)
    {
        myItemFlow.Initialize();
    }

    private void update(object sender, RoutedEventArgs e)
    {
        TextBlock myOutput = (TextBlock)Visual.FindElementByName(myItemFlow, "Output");
        myOutput.Text = "Name: " + ((TextBox)Visual.FindElementByName(myItemFlow, "firstname")).Text + " " + ((TextBox)Visual.FindElementByName(myItemFlow, "lastname")).Text + Environment.NewLine;

        myOutput.Text += "Billing Address:" + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "bill")).Text+Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "bill_street")).Text + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "bill_city")).Text + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "bill_zip")).Text + Environment.NewLine;
        myOutput.Text += "Shipping Address:" + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "ship")).Text + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "ship_street")).Text + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "ship_city")).Text + Environment.NewLine;
        myOutput.Text += " " + ((TextBox)Visual.FindElementByName(myItemFlow, "ship_zip")).Text + Environment.NewLine;
    }

  }
}
