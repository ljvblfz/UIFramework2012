using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Summary description for TreeViewAutoFormatEditorForm.
  /// </summary>
  internal class ToolBarItemsEditorForm : System.Windows.Forms.Form
  {
    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.PropertyGrid propertyGrid1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.Button button3;

    private ComponentArt.Web.UI.ToolBar _toolBar;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.PictureBox pictureBox2;

    public ToolBarItemCollection Items;


    private void LoadNodes(TabStripTab oItem, TreeNode oTreeNode)
    {
      oTreeNode.Tag = oItem;

      foreach(TabStripTab oChild in oItem.Tabs)
      {
        TreeNode oChildNode = new TreeNode(oChild.Text);
        LoadNodes(oChild, oChildNode);

        oTreeNode.Nodes.Add(oChildNode);
      }
    }

    public ToolBarItemsEditorForm(ComponentArt.Web.UI.ToolBar oToolBar)
    {
      InitializeComponent();

      _toolBar = oToolBar;
      Items = oToolBar.Items;

      // add pre-existing items
      foreach (ToolBarItem oItem in Items)
      {
        TreeNode oNewTreeNode = new TreeNode(oItem.Text);
        oNewTreeNode.Tag = oItem;
        treeView1.Nodes.Add(oNewTreeNode);
      }

      treeView1.HideSelection = false;
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if(components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

		#region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ToolBarItemsEditorForm));
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.Location = new System.Drawing.Point(10, 104);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(256, 288);
      this.treeView1.TabIndex = 0;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      // 
      // propertyGrid1
      // 
      this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
      this.propertyGrid1.Location = new System.Drawing.Point(280, 64);
      this.propertyGrid1.Name = "propertyGrid1";
      this.propertyGrid1.Size = new System.Drawing.Size(272, 328);
      this.propertyGrid1.TabIndex = 1;
      this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_ValueChanged);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(360, 408);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(88, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Done";
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(464, 408);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(88, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Cancel";
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button3
      // 
      this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
      this.button3.Location = new System.Drawing.Point(10, 64);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(32, 32);
      this.button3.TabIndex = 4;
      this.toolTip1.SetToolTip(this.button3, "Add item");
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button5
      // 
      this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
      this.button5.Location = new System.Drawing.Point(50, 64);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(32, 32);
      this.button5.TabIndex = 6;
      this.toolTip1.SetToolTip(this.button5, "Remove item");
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(568, 56);
      this.pictureBox1.TabIndex = 7;
      this.pictureBox1.TabStop = false;
      // 
      // pictureBox2
      // 
      this.pictureBox2.Location = new System.Drawing.Point(0, 440);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(584, 16);
      this.pictureBox2.TabIndex = 8;
      this.pictureBox2.TabStop = false;
      // 
      // ToolBarItemsEditorForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(566, 451);
      this.Controls.Add(this.pictureBox2);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.propertyGrid1);
      this.Controls.Add(this.treeView1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ToolBarItemsEditorForm";
      this.Text = "ComponentArt ToolBar Designer";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
      this.ResumeLayout(false);

    }
		#endregion


    // Add new item
    private void button3_Click(object sender, System.EventArgs e)
    {
      ToolBarItem oItem = new ToolBarItem();
      oItem.Text = "New Item";

      Items.Add(oItem);

      TreeNode oNewTreeNode = new TreeNode(oItem.Text);
      oNewTreeNode.Tag = oItem;
      treeView1.Nodes.Add(oNewTreeNode);

      treeView1.SelectedNode = treeView1.Nodes[treeView1.Nodes.Count - 1];
    }

    // Ok
    private void button1_Click(object sender, System.EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    // Cancel
    private void button2_Click(object sender, System.EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    // Click on node
    private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
    {
      propertyGrid1.SelectedObject = e.Node.Tag;
    }

    // Delete node
    private void button5_Click(object sender, System.EventArgs e)
    {
      if(treeView1.SelectedNode != null)
      {
        ToolBarItem oItem = (ToolBarItem)treeView1.SelectedNode.Tag;
        
        oItem.ParentToolBar.Items.Remove(oItem);
        treeView1.SelectedNode.Remove();
      }
    }

    private void propertyGrid1_ValueChanged(object sender, PropertyValueChangedEventArgs e)
    {
      // if we've just changed text, we need to update the visible tree
      if(e.ChangedItem.Label == "Text")
      {
        treeView1.SelectedNode.Text = (string)e.ChangedItem.Value;
      }
    }
  }
}
