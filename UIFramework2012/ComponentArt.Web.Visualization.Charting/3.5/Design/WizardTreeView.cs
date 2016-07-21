using System;
using System.Drawing;
using System.Windows.Forms;

using System.Globalization;
using System.Resources;
using System.Collections;
using System.Diagnostics;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	/// <summary>
	/// Presents a treeview of the presentation objects.
	/// </summary>
	internal class WizardTreeView : TreeView
	{
		/// <summary>
		/// Tree node that contains presentation object
		/// </summary>

		private bool mouseDown = false;
		private Point mouseLocation;
		private TreeNode movingNode;
		private Bitmap nodeBitmap;
		private Rectangle nodeRectangle;

		// Use of this is necessary because a strange "OnMouseUp()" is generated
		// by the framework immediately after "OnMouseDown()".
		private bool thereWasMouseMove = false;

		class SeriesTreeNode : TreeNode 
		{
			SeriesBase m_series;

			public SeriesTreeNode(SeriesBase s, string text) : base (text)
			{
				m_series = s;
			}

			public SeriesBase SeriesBase
			{
				get {return m_series;}
			}
		}

		public WizardTreeView(WinChart w)
		{
			InitializeComponent();
			m_winchart = w;

			// Allow editing the name of the presentation
			LabelEdit = true;

			// Assign image list
			ImageList = imageList1;

			// Set the TreeView control's default image and selected image indexes.
			ImageIndex = 0;
			SelectedImageIndex = 0;
		}

		private bool IsValidTargetNode(TreeNode node)
		{
			if (node == null || (node as SeriesTreeNode).SeriesBase is Series)
				return false;
			TreeNode loopNode = node;
			while (loopNode != null)
			{
				if (loopNode == movingNode)
				{
					return false;
				}
				loopNode = loopNode.Parent;
			}
			return true;

		}
		
		private void NotifyNodeMoved(TreeNode movingNode,TreeNode targetNode)
		{
			// Update the series hierarchy
			SeriesTreeNode sMovingNode = movingNode as SeriesTreeNode;
			SeriesTreeNode sTargetNode = targetNode as SeriesTreeNode;
			SeriesBase series = sMovingNode.SeriesBase;
			CompositeSeries cSeries = sTargetNode.SeriesBase as CompositeSeries;
			series.OwningSeries.SubSeries.Remove(series);
			cSeries.SubSeries.Add(series);
		}

		public WizardTreeView() : this(null) {}

		private System.Windows.Forms.ContextMenu TreeNodeMenu;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;

		TreeNode m_selectedNode;

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WizardTreeView));
			this.TreeNodeMenu = new System.Windows.Forms.ContextMenu();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;

		}
	
		/// <summary>
		/// Builds the list of menu items for each situation
		/// </summary>
		private void LoadList(bool addingOnly) 
		{
			TreeNodeMenu.MenuItems.Clear();

			// Can only add a child series if dealing with composite series
			if (((SeriesTreeNode)m_selectedNode).SeriesBase is CompositeSeries) 
			{
				// simple series item
				MenuItem menuItem2 = new MenuItem("Add Series", new System.EventHandler(MenuItem_OnClick));
				TreeNodeMenu.MenuItems.Add(menuItem2);

				// composite series item
				MenuItem menuItem1 = new MenuItem("Add Composite Series", new System.EventHandler(MenuItem_OnClick));
				TreeNodeMenu.MenuItems.Add(menuItem1);
			}

			if (!addingOnly) 
			{
				// remove item
				MenuItem menuItem3 = new MenuItem("Remove", new System.EventHandler(MenuItem_OnClick));
				TreeNodeMenu.MenuItems.Add(menuItem3);
			}
		}

		/// <summary>
		/// Stores the new name of the presentation
		/// </summary>
		protected override void OnAfterLabelEdit ( System.Windows.Forms.NodeLabelEditEventArgs e ) 
		{
            if (!e.CancelEdit && e.Label != null)
                ((SeriesTreeNode)e.Node).SeriesBase.Name = e.Label;

            base.OnAfterLabelEdit(e);
		}

		/// <summary>
		/// Handles the click on the context menu
		/// </summary>
		public void MenuItem_OnClick(object sender, EventArgs e) 
		{
			if (((MenuItem)sender).Text.IndexOf("Remove") != -1)  
			{
				Remove();
			} 
			else 
			{
				// Add menu item was clicked
				SeriesBase cur_ser = ((SeriesTreeNode)m_selectedNode).SeriesBase;
				if (cur_ser is CompositeSeries) 
				{

					SeriesBase s;

					if (((MenuItem)sender).Text.IndexOf("Composite") != -1) 
						s = new CompositeSeries();
					else 
						s = new Series();

					((CompositeSeries)cur_ser).SubSeries.Add(s);
					PopulateTree(m_selectedNode, s);
				}
			}

			WinChart.Invalidate();
		}

		#region --- Mouse events handling ---

		/// <summary>
		/// Invokes the menu on right button click or prepares for dragging if left button
		/// </summary>
		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Right) 
			{
				m_selectedNode = GetNodeAt(e.X, e.Y);
				if (m_selectedNode != null)
				{
					SelectedNode = m_selectedNode;
					LoadList(false);
					TreeNodeMenu.Show(this, new System.Drawing.Point(e.X, e.Y));
				}
			}
			else
			{	// Prepare for node dragging
				base.OnMouseDown(e);
				mouseDown = true;
				thereWasMouseMove = false;
				mouseLocation = new Point(e.X,e.Y);
				movingNode = GetNodeAt(mouseLocation);
				if (movingNode != null)
				{
					Rectangle r = movingNode.Bounds;
					nodeRectangle = r;
					nodeBitmap = new Bitmap(r.Width,r.Height);
					Graphics gr = Graphics.FromImage(nodeBitmap);
					gr.Clear(Color.FromArgb(64,Color.Blue));
					PointF point = new PointF(2,2);
					gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
					gr.DrawString(movingNode.Text, this.Font, Brushes.Blue, point);
					gr.Dispose();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (movingNode == null || !mouseDown)
				return;

			if(!thereWasMouseMove)
				return;

			TreeNode targetNode = GetNodeAt(e.X,e.Y);

			if (IsValidTargetNode(targetNode))
			{                
				movingNode.Remove();
				targetNode.Nodes.Add(movingNode);
				targetNode.Expand();
				SelectedNode = movingNode;

				NotifyNodeMoved(movingNode,targetNode);
				if(m_winchart != null)
					m_winchart.Refresh();
			}

			Cursor = Cursors.Default;

			Refresh();
          
            if(nodeBitmap != null)
			    nodeBitmap.Dispose();
			nodeBitmap = null;
			mouseDown = false;
			movingNode = null;
			thereWasMouseMove = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (movingNode == null || !mouseDown)
				return;

			// This strange case had to be done this way because a strange "OnMouseUp()" is generated
			// by the framework immediately after "OnMouseDown()".
			if(e.Button != MouseButtons.Left)
			{
				OnMouseUp(e);
				return;
			}

			thereWasMouseMove = true;
			TreeNode thisNode = GetNodeAt(e.X,e.Y);
            if (IsValidTargetNode(thisNode))
            {
                Cursor = Cursors.Hand;
                this.SelectedNode = thisNode;
            }
            else
            {
                Cursor = Cursors.No;
                this.SelectedNode = null;
            }
			Refresh();

			Point newLocation = new Point(e.X,e.Y);
			Graphics gr = CreateGraphics();
			gr.DrawImage(nodeBitmap,
				new Point(nodeRectangle.Left + newLocation.X - mouseLocation.X,
				nodeRectangle.Top + newLocation.Y - mouseLocation.Y));
			gr.Dispose();

		}
		#endregion

		public void Add (Control addButton)
		{
			m_selectedNode = SelectedNode;
			LoadList(true);
			TreeNodeMenu.Show(addButton, new System.Drawing.Point(0, addButton.Size.Height));
		}
	
		/// <summary>
		/// Removes the presentation and the node
		/// </summary>
		public void Remove()
		{
			m_selectedNode = SelectedNode;

			SeriesBase cur_pres = ((SeriesTreeNode)m_selectedNode).SeriesBase;
			if (cur_pres.OwningSeries == null)
				return;

			cur_pres.OwningSeries.SubSeries.Remove(cur_pres);
            cur_pres.OwningChart.DataProvider.RemoveSeriesVariables(cur_pres);
			m_selectedNode.Remove();
			m_selectedNode = null;
		}


		/// <summary>
		/// Tells if the selected presentation is composite
		/// </summary>
		public bool IsSelectedCompositeSeries() 
		{
			if (SelectedNode == null) 
				return false;

			return ((SeriesTreeNode)SelectedNode).SeriesBase is CompositeSeries;
		}

		public SeriesBase SeriesBase 
		{
			get 
			{
				if (SelectedNode == null)
					return null;

				return ((SeriesTreeNode)SelectedNode).SeriesBase;
			}
		}


		WinChart m_winchart;

		
		/// <summary>
		/// Sets and gets the winchart
		/// </summary>
		public WinChart WinChart 
		{
			get {return m_winchart;}
			set 
			{
				m_winchart = value;

				if (m_winchart != null) 
					LoadNodes();
			}
		}


		/// <summary> 
		/// Populates the tree with presentations
		/// </summary>
		private void PopulateTree(TreeNode parent, SeriesBase seriesBase) 
		{
			PopulateTreeRecursive(parent, seriesBase);
			ExpandAll();
		}

		private void PopulateTreeRecursive(TreeNode parent, SeriesBase seriesBase)
		{
			SeriesTreeNode ptn = new SeriesTreeNode(seriesBase, seriesBase.Name);

			ptn.ImageIndex = seriesBase is CompositeSeries ? 0 : 1;
			ptn.SelectedImageIndex = ptn.ImageIndex;

			if (parent == null)
				Nodes.Add(ptn);
			else
				parent.Nodes.Add(ptn);



			if (seriesBase is CompositeSeries) 
			{
				foreach (SeriesBase child in ((CompositeSeries)seriesBase).SubSeries) 
				{
					PopulateTreeRecursive(ptn, child);
				}
			}
		}


		bool FocusOnFirstSimpleSeries(TreeNodeCollection tnc)
		{
			foreach (SeriesTreeNode stn in tnc) 
			{
				if (stn.SeriesBase is Series) 
				{
					SelectedNode = stn;
					return true;
				} 
				else 
				{
					if (FocusOnFirstSimpleSeries(stn.Nodes))
						return true;
				}
			}
			return false;
		}


		private void LoadNodes() 
		{
			BeginUpdate();

			// Clear the TreeView each time the method is called.
			Nodes.Clear();

			// Populate the root node(s)
			CompositeSeries series = m_winchart.RootSeries;
			PopulateTree(null, series);

			EndUpdate();

			SelectedNode = Nodes[0];
		}				

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (m_winchart == null) 
				return;

			LoadNodes();
		}
	}
}
