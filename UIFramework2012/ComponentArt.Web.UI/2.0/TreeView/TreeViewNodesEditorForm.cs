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
  internal class TreeViewNodesEditorForm : System.Windows.Forms.Form
  {
    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.PropertyGrid propertyGrid1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.Button button3;

    private ComponentArt.Web.UI.TreeView _treeView;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.PictureBox pictureBox2;
    private System.Windows.Forms.PictureBox pictureBox1;

    public TreeViewNodeCollection Nodes;


    private void LoadNodes(TreeViewNode oNode, TreeNode oTreeNode)
    {
      oTreeNode.Tag = oNode;

      foreach(TreeViewNode oChild in oNode.Nodes)
      {
        TreeNode oChildNode = new TreeNode(oChild.Text);
        LoadNodes(oChild, oChildNode);

        oTreeNode.Nodes.Add(oChildNode);
      }
    }

		public TreeViewNodesEditorForm(ComponentArt.Web.UI.TreeView oTreeView)
    {
      InitializeComponent();

      _treeView = oTreeView;
      Nodes = oTreeView.Nodes;

      // add pre-existing nodes
      foreach(TreeViewNode oRoot in Nodes)
      {
        TreeNode oRootNode = new TreeNode(oRoot.Text);
        LoadNodes(oRoot, oRootNode);
        treeView1.Nodes.Add(oRootNode);
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TreeViewNodesEditorForm));
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.ImageIndex = -1;
      this.treeView1.Location = new System.Drawing.Point(10, 104);
      this.treeView1.Name = "treeView1";
      this.treeView1.SelectedImageIndex = -1;
      this.treeView1.Size = new System.Drawing.Size(256, 288);
      this.treeView1.TabIndex = 0;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      // 
      // propertyGrid1
      // 
      this.propertyGrid1.CommandsVisibleIfAvailable = true;
      this.propertyGrid1.LargeButtons = false;
      this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
      this.propertyGrid1.Location = new System.Drawing.Point(280, 64);
      this.propertyGrid1.Name = "propertyGrid1";
      this.propertyGrid1.Size = new System.Drawing.Size(272, 328);
      this.propertyGrid1.TabIndex = 1;
      this.propertyGrid1.Text = "propertyGrid1";
      this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
      this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
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
      this.button3.Image = ((System.Drawing.Bitmap)(resources.GetObject("button3.Image")));
      this.button3.Location = new System.Drawing.Point(10, 64);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(32, 32);
      this.button3.TabIndex = 4;
      this.toolTip1.SetToolTip(this.button3, "Add a root node");
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button4
      // 
      this.button4.Image = ((System.Drawing.Bitmap)(resources.GetObject("button4.Image")));
      this.button4.Location = new System.Drawing.Point(50, 64);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(32, 32);
      this.button4.TabIndex = 5;
      this.toolTip1.SetToolTip(this.button4, "Add a child node");
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button5
      // 
      this.button5.Image = ((System.Drawing.Bitmap)(resources.GetObject("button5.Image")));
      this.button5.Location = new System.Drawing.Point(90, 64);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(32, 32);
      this.button5.TabIndex = 6;
      this.toolTip1.SetToolTip(this.button5, "Remove node");
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // pictureBox2
      // 
      this.pictureBox2.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox2.Image")));
      this.pictureBox2.Location = new System.Drawing.Point(0, 442);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(584, 16);
      this.pictureBox2.TabIndex = 9;
      this.pictureBox2.TabStop = false;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(568, 56);
      this.pictureBox1.TabIndex = 10;
      this.pictureBox1.TabStop = false;
      // 
      // TreeViewNodesEditorForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(568, 453);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.pictureBox1,
                                                                  this.pictureBox2,
                                                                  this.button5,
                                                                  this.button4,
                                                                  this.button3,
                                                                  this.button2,
                                                                  this.button1,
                                                                  this.propertyGrid1,
                                                                  this.treeView1});
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "TreeViewNodesEditorForm";
      this.Text = "ComponentArt TreeView Designer";
      this.ResumeLayout(false);

    }
		#endregion


    // New root
    private void button3_Click(object sender, System.EventArgs e)
    {
      TreeViewNode oNode = new TreeViewNode();
      oNode.Text = "New Root";
      oNode.navigator = _treeView;

      Nodes.Add(oNode);

      TreeNode oNewTreeNode = new TreeNode("New Root");
      oNewTreeNode.Tag = oNode;
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

    // New child
    private void button4_Click(object sender, System.EventArgs e)
    {
      if(treeView1.SelectedNode == null)
      {
        return;
      }

      TreeViewNode oNode = (TreeViewNode)treeView1.SelectedNode.Tag;

      TreeViewNode oNewNode = new TreeViewNode();
      oNewNode.Text = "New Node";

      oNode.Nodes.Add(oNewNode);
      
      TreeNode oNewTreeNode = new TreeNode("New Node");
      oNewTreeNode.Tag = oNewNode;
      treeView1.SelectedNode.Nodes.Add(oNewTreeNode);
      treeView1.SelectedNode.Expand();
    }

    // Delete node
    private void button5_Click(object sender, System.EventArgs e)
    {
      if(treeView1.SelectedNode != null)
      {
        TreeViewNode oNode = (TreeViewNode)treeView1.SelectedNode.Tag;
        
        if(oNode.ParentNode == null)
        {
          // we have a root
          oNode.ParentTreeView.Nodes.Remove(oNode);
        }
        else
        {
          // we have a child
          oNode.ParentNode.Nodes.Remove(oNode);
        }
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
