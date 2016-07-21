using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class SeriesStylesCollectionDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonRemove;

		private IWindowsFormsEditorService edSvc;
		private SeriesStyleCollection styles;

		private SeriesStyleTreeView view;
		private PropertyGrid propertyGrid;
		int dist1 = 5, dist2 = 15, dist3 = 10;
		private System.Windows.Forms.GroupBox groupBox1;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SeriesStylesCollectionDialog()
		{
			InitializeComponent();

			view = new SeriesStyleTreeView();
			view.Location = new Point(label2.Location.X,label2.Location.Y + label2.Height + dist1);
			view.Size = new Size(buttonRemove.Location.X + buttonRemove.Size.Width - label2.Location.X, buttonAdd.Location.Y - view.Location.Y - dist1);
			view.HideSelection = false;
			Controls.Add(view);

			view.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterStyleSelect);

			propertyGrid = new PropertyGrid();
			propertyGrid.CommandsVisibleIfAvailable = true;
			propertyGrid.Location = new Point(label1.Location.X, view.Location.Y);
			propertyGrid.Size = new Size(this.Size.Width - propertyGrid.Location.X - label2.Location.X, groupBox1.Location.Y - propertyGrid.Location.Y - dist1);
			Controls.Add(propertyGrid);

			propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertiesChanged);

		}

		private void AfterStyleSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			SeriesStyle style = view.SelectedStyle;
			propertyGrid.SelectedObject = style;
			buttonAdd.Enabled = !view.IsSelectedCategory;
			buttonRemove.Enabled = false;
			if(style != null)
			{
				buttonRemove.Enabled = style.Removable;
			}
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

		public void PropertiesChanged(object obj,PropertyValueChangedEventArgs pva)
		{
			GridItem item = pva.ChangedItem;
			if(item.Label == "Name")
				view.SelectedNode.Text = item.Value as string;
		}

	
		public IWindowsFormsEditorService EditorService 
		{
			get  
			{
				return edSvc;
			}
			set 
			{
				edSvc = value;
			}
		}
		public void Populate(SeriesStyleCollection styles)
		{
			this.styles = styles;
			view.Populate(styles);
			ChartBase chart = styles.Owner as ChartBase;
			if(chart != null)
				view.SelectedStyle = styles[chart.Series.StyleName];
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonRemove = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(240, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(272, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "SeriesStyle Properties:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(164, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "SeriesStyle Collection:";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(258, 341);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(100, 24);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(363, 341);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(100, 24);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.buttonAdd.Location = new System.Drawing.Point(8, 288);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(100, 24);
			this.buttonAdd.TabIndex = 5;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonRemove
			// 
			this.buttonRemove.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.buttonRemove.Location = new System.Drawing.Point(112, 288);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(100, 24);
			this.buttonRemove.TabIndex = 6;
			this.buttonRemove.Text = "Remove";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(8, 325);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(456, 4);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// SeriesStylesCollectionDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 374);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.buttonRemove,
																		  this.buttonAdd,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.label2,
																		  this.label1});
			this.Name = "SeriesStylesCollectionDialog";
			this.Text = "SeriesStyleCollection Editor";
			this.Resize += new System.EventHandler(this.SeriesStylesEditForm_Resize);
			this.ResumeLayout(false);

		}
		#endregion

		private void SeriesStylesEditForm_Resize(object sender, System.EventArgs e)
		{
			view.Size = new Size(buttonRemove.Location.X + buttonRemove.Size.Width - label2.Location.X, buttonAdd.Location.Y - view.Location.Y - dist1);
			propertyGrid.Size = new Size(buttonCancel.Location.X + buttonCancel.Width - propertyGrid.Location.X, groupBox1.Location.Y - propertyGrid.Location.Y - dist1);
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			SeriesStyle style = view.SelectedStyle;
			if(style == null)
				return;
			string styleName = style.Name;
			int i=1;
			while(styles[styleName + "_" + i] != null)
				i++;
			SeriesStyle clonedStyle = styles.CreateFrom(styleName + "_" + i,styleName);
			clonedStyle.IsDefault = false;
			clonedStyle.Removable = true;
			TreeNode newNode = view.Add(clonedStyle);
			view.Invalidate();
			view.SelectedNode = newNode;
			view.Focus();
		}

		private void buttonRemove_Click(object sender, System.EventArgs e)
		{
			SeriesStyle style = view.SelectedStyle;
			if(style == null)
				return;
			styles.Remove(style.Name);
			TreeNode node = view.SelectedNode;
			view.SelectedNode = view.SelectedNode.Parent;
			node.Remove();
			view.Invalidate();
		}
	}
}
