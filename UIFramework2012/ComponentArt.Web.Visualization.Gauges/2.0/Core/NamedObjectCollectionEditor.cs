using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal class NamedObjectCollectionEditor : UITypeEditor
	{
		NamedObjectCollection collection;
		NamedObjectCollection savedCollection;
		public NamedObjectCollectionEditor()
		{ }
		
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value) 
		{
			collection = value as NamedObjectCollection;
			if(collection == null)
				return value;

			// Clone the collection
			savedCollection = collection.Clone();

			// get the editor service.
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
			
			if (edSvc == null) 
				return value;
			
			// create our UI
			NamedObjectCollectionDialog dlg = new NamedObjectCollectionDialog();
			dlg.Populate(collection);
			dlg.EditorService = edSvc;

			// instruct the editor service to display the modal dialog.
			edSvc.ShowDialog(dlg);
		
			// Check the dialog result
			if(dlg.DialogResult == DialogResult.Cancel)
			{
				collection.Clear();
				for(int i=0; i<savedCollection.Count; i++)
					collection.Add(savedCollection[i]);
                ObjectModelBrowser.NotifyChanged(collection);
			}
			else if(context != null)
			{
				context.OnComponentChanged();
			}
			return value;
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return System.Drawing.Design.UITypeEditorEditStyle.Modal;
		}
	}

	// =======================================================================================
	/// <summary>
	/// Summary description for SeriesStylesEditForm.
	/// </summary>
	internal class NamedObjectCollectionDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonRemove;

		private IWindowsFormsEditorService edSvc;
		private NamedObjectCollection collection;

		//private SeriesStyleTreeView view;
		private PropertyGrid propertyGrid;
		int dist1 = 5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button buttonCopy;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NamedObjectCollectionDialog()
		{
			InitializeComponent();

			listBox1.Location = new Point(label2.Location.X,label2.Location.Y + label2.Height + dist1);
			listBox1.Size = new Size(buttonCopy.Location.X + buttonCopy.Size.Width - label2.Location.X, buttonAdd.Location.Y - listBox1.Location.Y - dist1);

			propertyGrid = new PropertyGrid();
			propertyGrid.CommandsVisibleIfAvailable = true;
			propertyGrid.Location = new Point(label1.Location.X, listBox1.Location.Y);
			propertyGrid.Size = new Size(buttonCancel.Location.X + buttonCancel.Width - propertyGrid.Location.X, groupBox1.Location.Y - propertyGrid.Location.Y - dist1);
			propertyGrid.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			Controls.Add(propertyGrid);

			propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertiesChanged);

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
			ObjectModelBrowser.GetOwningTopmostGauge(collection).Invalidate();
			if(pva.ChangedItem.PropertyDescriptor.Name == "Name")
			{
				int ix = listBox1.SelectedIndex;
				string newName = pva.ChangedItem.Value.ToString();
				listBox1.Items.RemoveAt(ix);
				listBox1.Items.Insert(ix,newName);
				listBox1.SelectedIndex = ix;
			}
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

		public void Populate(NamedObjectCollection collection)
		{
			this.collection = collection;
			this.Text = collection.GetType().Name + " Editor";
			for(int i=0; i<collection.Count; i++)
			{
				int ix = listBox1.Items.Add(collection[i].Name);
			}
			if(collection.Count > 0)
				listBox1.SelectedIndex = 0;
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.buttonCopy = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(208, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(328, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Member Properties:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(164, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Collection:";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(330, 349);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(100, 24);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(435, 349);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(100, 24);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.buttonAdd.Location = new System.Drawing.Point(8, 304);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(48, 24);
			this.buttonAdd.TabIndex = 5;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonRemove
			// 
			this.buttonRemove.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.buttonRemove.Location = new System.Drawing.Point(64, 304);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(64, 24);
			this.buttonRemove.TabIndex = 6;
			this.buttonRemove.Text = "Remove";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox1.Location = new System.Drawing.Point(8, 333);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(528, 4);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 24);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(184, 264);
			this.listBox1.TabIndex = 8;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// buttonCopy
			// 
			this.buttonCopy.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.buttonCopy.Location = new System.Drawing.Point(136, 304);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new System.Drawing.Size(56, 24);
			this.buttonCopy.TabIndex = 9;
			this.buttonCopy.Text = "Copy";
			this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
			// 
			// NamedObjectCollectionDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(544, 382);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCopy,
																		  this.listBox1,
																		  this.groupBox1,
																		  this.buttonRemove,
																		  this.buttonAdd,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.label2,
																		  this.label1});
			this.Name = "NamedObjectCollectionDialog";
			this.Text = "SeriesStyleCollection Editor";
			this.Resize += new System.EventHandler(this.SeriesStylesEditForm_Resize);
			this.ResumeLayout(false);

		}
		#endregion

		private void SeriesStylesEditForm_Resize(object sender, System.EventArgs e)
		{
			listBox1.Size = new Size(buttonCopy.Location.X + buttonCopy.Size.Width - label2.Location.X, buttonAdd.Location.Y - listBox1.Location.Y - dist1);
			propertyGrid.Size = new Size(buttonCancel.Location.X + buttonCancel.Width - propertyGrid.Location.X, groupBox1.Location.Y - propertyGrid.Location.Y - dist1);
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			int ix = listBox1.Items.Add(collection.CreateNewMember().Name);
			listBox1.SelectedIndex = ix;
            ObjectModelBrowser.NotifyChanged(collection);
		}

		private void buttonRemove_Click(object sender, System.EventArgs e)
		{
			int ix = listBox1.SelectedIndex;
			if(ix < 0)
				return;
			collection.RemoveAt(ix);
			listBox1.Items.RemoveAt(ix);
			if(collection.Count <= ix)
				ix--;
			if(ix>0)
				listBox1.SelectedIndex = ix;
            ObjectModelBrowser.NotifyChanged(collection);
        }

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int ix = listBox1.SelectedIndex;
			if(ix >= 0 && ix < collection.Count)
			{
				buttonRemove.Enabled = !collection[ix].IsRequired;
				propertyGrid.SelectedObject = collection[ix];
			}
			else
				propertyGrid.SelectedObject = null;
		}

		private void buttonCopy_Click(object sender, System.EventArgs e)
		{
			int ix = listBox1.SelectedIndex;
			if(ix < 0 || ix >= collection.Count)
				return;
			string selectedMemberName = listBox1.Items[ix].ToString();
			string newMemberName = "CopyOf" + listBox1.Items[ix].ToString();
			string newName = newMemberName;
			int i = 1;
			while(collection.IndexOf(newName) >= 0)
			{
				i++;
				newName = newMemberName + "_" + i.ToString();
			}
			collection.AddNewMemberFrom(newName,selectedMemberName);
			listBox1.Items.Add(newName);
			listBox1.SelectedIndex = listBox1.Items.Count-1;
			Invalidate();
		}
	}
}
