using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class SeriesStyleTreeView : TreeView
	{
		private SeriesStyleCollection styles;
		private IWindowsFormsEditorService edSvc;

		private bool m_userSelect = true;


		public SeriesStyleTreeView()
		{ 
			InitializeComponent();
		}
	
		internal bool UserSelect 
		{
			get 
			{
				return m_userSelect;
			}
			set 
			{
				m_userSelect = value;
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

		public void Populate(SeriesStyleCollection styles)
		{
			Populate(styles, null);
		}

		public void Populate(SeriesStyleCollection styles, SeriesBase sb)
		{
			this.styles = styles;
			Nodes.Clear();
			// Populate the topmost level: Categories
			foreach (string ckcname in Enum.GetNames(typeof(ChartKindCategory))) 
			{
				this.Nodes.Add(ckcname);
			}

			foreach(SeriesStyle style in styles) 
			{
				if (sb == null || style.IsApplicable(sb))
					Add(style);
			}

			bool removed = true;
			while (removed == true) 
			{
				removed = false;
				foreach (TreeNode tn in Nodes) 
				{
					if (tn.Nodes.Count == 0) 
					{
						Nodes.Remove(tn);
						removed = true;
						break;
					}
				}
			}
		}

		public TreeNode Add(SeriesStyle style)
		{
			string cat = style.ChartKindCategory.ToString();
			TreeNode node = this.Nodes[0];
			while(node.Text != cat && node != null)
				node = node.NextNode;
			if(node != null)
			{
				TreeNode newNode = new TreeNode(style.Name);
				node.Nodes.Add(newNode);
				return newNode;
			}
			return null;
		}

		private void InitializeComponent()
		{
		}

		private void WizardSeriesStyleTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(UserSelect && edSvc != null && e.Node.Parent != null) 
			{
				edSvc.CloseDropDown();
			}
		}

		bool handlerAssigned = false;

		protected override void OnVisibleChanged(EventArgs e) 
		{
			base.OnVisibleChanged(e);
			if (Visible && !handlerAssigned)
			{
				handlerAssigned = true;
				this.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.WizardSeriesStyleTreeView_AfterSelect);
			}
		}

		public bool IsSelectedCategory { get { return this.SelectedNode.Parent == null; } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SeriesStyle SelectedStyle
		{
			get
			{
				if(IsSelectedCategory || SelectedNode == null)
					return null;
				return styles[SelectedNode.Text];
			}
			set
			{
				SelectStyle(value.Name);
			}
		}

		private void SelectStyle(string styleName)
		{
			TreeNode node = this.Nodes[0];
			while(node != null)
			{
				TreeNode child = node.FirstNode;
				while(child != null)
				{
					if(child.Text == styleName)
					{
						node.Expand();
						SelectedNode = child;
						return;
					}
					else
						child = child.NextNode;
				}
				node = node.NextNode;
			}
		}

		internal ChartKindCategory SelectedCategory
		{
			get
			{
				if(SelectedNode == null)
					return (ChartKindCategory)(-1);
				TypeConverter conv = TypeDescriptor.GetConverter(typeof(ChartKindCategory));
				if(IsSelectedCategory)
					return (ChartKindCategory)conv.ConvertFromString(SelectedNode.Text);
				else
					return (ChartKindCategory)conv.ConvertFromString(SelectedNode.Parent.Text);
			}
		}
	}
}
