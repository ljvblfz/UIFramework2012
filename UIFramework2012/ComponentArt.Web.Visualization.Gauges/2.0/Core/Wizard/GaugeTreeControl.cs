using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal class TreeNodeSelectEventArgs: EventArgs
	{
		public TreeNodeSelectEventArgs(object selectedObject)
		{
			this.selectedObject = selectedObject;
		}

		private object selectedObject;
		public object SelectedObject
		{
			get 
			{
				return selectedObject;
			}
		}
	}
	
	/// <summary>
	/// Summary description for GaugeTreeControl.
	/// </summary>
	internal class GaugeTreeControl : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.Button removeButton;
		private System.Windows.Forms.TreeView treeView;
		private ComponentArt.WinUI.Button addButton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TreeNode previouslySelectedNode = null;

		ImageList treeImages = null;

		public GaugeTreeControl()
		{
			this.BackColor = Color.FromArgb(255,255,255);
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			LoadImages();
		}

		public GaugeTreeControl(IGaugeControl gauge):this()
		{
			m_gauge = gauge;
			treeView.HideSelection = false;
			PopulateTree();
		}

		IGaugeControl m_gauge = null;
		public IGaugeControl Gauge
		{
			set
			{
				m_gauge = value;
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

		private TreeNode m_SelectedNode = null;

		public delegate void GaugeTreeViewEventHandlerWithArgs (object source, TreeNodeSelectEventArgs e);
		public delegate void GaugeTreeViewEventHandler (object source);
		public event GaugeTreeViewEventHandlerWithArgs AfterSelect;
		public event GaugeTreeViewEventHandler Changed;


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.addButton = new ComponentArt.WinUI.Button();
			this.removeButton = new ComponentArt.WinUI.Button();
			this.treeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// addButton
			// 
			this.addButton.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.addButton.Enabled = false;
			this.addButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.addButton.Location = new System.Drawing.Point(24, 275);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(56, 23);
			this.addButton.TabIndex = 13;
			this.addButton.Text = "Add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.removeButton.Enabled = false;
			this.removeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.removeButton.Location = new System.Drawing.Point(96, 275);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(56, 23);
			this.removeButton.TabIndex = 12;
			this.removeButton.Text = "Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// treeView
			// 
			this.treeView.ImageIndex = -1;
			this.treeView.Location = new System.Drawing.Point(4, 4);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = -1;
			this.treeView.Size = new System.Drawing.Size(156, 266);
			this.treeView.TabIndex = 11;
			this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
			this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelect);
			this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
			// 
			// GaugeTreeControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.treeView,
																		  this.removeButton,
																		  this.addButton});
			this.Name = "GaugeTreeControl";
			this.Size = new System.Drawing.Size(164, 303);
			this.ResumeLayout(false);

		}
		#endregion


		private void LoadImages()
		{
			System.IO.Stream stream;

			treeImages = new ImageList();
			treeImages.ImageSize = new Size(16, 16);
			treeImages.TransparentColor = Color.White;

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.gauge.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));		

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.scale-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.indicator-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.textAnnotation-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.imageAnnotation-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.range-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.pointer-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.scale.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.range.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.annotation-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.pointer.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.annotation.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.indicator.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.imageAnnotation.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.textAnnotation.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.treeviewIcons.gauge-collection.png");
			treeImages.Images.Add((Bitmap)Bitmap.FromStream(stream));

			treeView.ImageList = treeImages;
		}


		public void PopulateTree()
		{
			this.SuspendLayout();
			treeView.Nodes.Clear();

			//create root node
			TreeNode gaugeNode = new TreeNode(m_gauge.Name);
			gaugeNode.Tag = m_gauge;
			gaugeNode.ImageIndex = 0;
			gaugeNode.SelectedImageIndex = 0;
			PopulateGauge(gaugeNode);
			treeView.Nodes.Add(gaugeNode);

			treeView.ExpandAll();
			this.ResumeLayout(false);
		}

		public void SelectNode (object tag)
		{
			treeView.Select();
			SelectNodeRecursively(treeView.Nodes[0], tag);
		}
		
		private bool SelectNodeRecursively(TreeNode node, object tag)
		{
			if (node.Tag == tag)
			{
				treeView.SelectedNode = node;
				if (previouslySelectedNode != null)
				{
					previouslySelectedNode.BackColor = Color.White;
					previouslySelectedNode.ForeColor = Color.Black;
				}
				treeView.SelectedNode.BackColor = Color.FromArgb(49, 106, 197);
				treeView.SelectedNode.ForeColor = Color.White;
				previouslySelectedNode = treeView.SelectedNode;
				return true;
			}
			foreach (TreeNode n in node.Nodes)
			{
				if (SelectNodeRecursively(n, tag))
					return true;
			}

			return false;
		}

		private void PopulateGauge(TreeNode gaugeNode)
		{	
			//create scales node
			TreeNode scalesNode = new TreeNode("Scales");
			if (gaugeNode.Tag is IGaugeControl) 
				scalesNode.Tag = ((IGaugeControl)(gaugeNode.Tag)).Scales;
			else if (gaugeNode.Tag is SubGauge)
				scalesNode.Tag = ((SubGauge)(gaugeNode.Tag)).Scales;
			scalesNode.ImageIndex = 1;
			scalesNode.SelectedImageIndex = 1;
			PopulateScales(scalesNode);
			gaugeNode.Nodes.Add(scalesNode);

			//create sub-gauges node
			TreeNode subGaugesNode = new TreeNode("Sub-Gauges");
			if (gaugeNode.Tag is IGaugeControl) 
				subGaugesNode.Tag = ((IGaugeControl)(gaugeNode.Tag)).SubGauges;
			else if (gaugeNode.Tag is SubGauge)
				subGaugesNode.Tag = ((SubGauge)(gaugeNode.Tag)).SubGauges;
			subGaugesNode.ImageIndex = 15;
			subGaugesNode.SelectedImageIndex = 15;
			PopulateSubGauges(subGaugesNode);
			gaugeNode.Nodes.Add(subGaugesNode);

			//create indicators node
			TreeNode indicatorNode = new TreeNode("Indicators");
			if (gaugeNode.Tag is IGaugeControl) 
				indicatorNode.Tag = ((IGaugeControl)(gaugeNode.Tag)).Indicators;
			else if (gaugeNode.Tag is SubGauge)
				indicatorNode.Tag = ((SubGauge)(gaugeNode.Tag)).Indicators;
			indicatorNode.ImageIndex = 2;
			indicatorNode.SelectedImageIndex = 2;
			PopulateIndicators(indicatorNode);
			gaugeNode.Nodes.Add(indicatorNode);

			//create text annotations node
			TreeNode textAnnotationNode = new TreeNode("Text Annotations");
			if (gaugeNode.Tag is IGaugeControl) 
				textAnnotationNode.Tag = ((IGaugeControl)(gaugeNode.Tag)).TextAnnotations;
			else if (gaugeNode.Tag is SubGauge)
				textAnnotationNode.Tag = ((SubGauge)(gaugeNode.Tag)).TextAnnotations;
			textAnnotationNode.ImageIndex = 3;
			textAnnotationNode.SelectedImageIndex = 3;
			PopulateTextAnnotations(textAnnotationNode);
			gaugeNode.Nodes.Add(textAnnotationNode);

			//create image annotations node
			TreeNode imageAnnotationNode = new TreeNode("Image Annotations");
			if (gaugeNode.Tag is IGaugeControl) 
				imageAnnotationNode.Tag = ((IGaugeControl)(gaugeNode.Tag)).ImageAnnotations;
			else if (gaugeNode.Tag is SubGauge)
				imageAnnotationNode.Tag = ((SubGauge)(gaugeNode.Tag)).ImageAnnotations;
			imageAnnotationNode.ImageIndex = 4;
			imageAnnotationNode.SelectedImageIndex = 4;
			PopulateImageAnnotations(imageAnnotationNode);
			gaugeNode.Nodes.Add(imageAnnotationNode);
		}


		private void PopulateSubGauges(TreeNode subGaugesNode)
		{
			foreach(SubGauge gauge in (SubGaugeCollection)(subGaugesNode.Tag))
			{
				//create gauge node
				TreeNode gaugeNode = new TreeNode(gauge.Name);
				gaugeNode.ImageIndex = 0;
				gaugeNode.SelectedImageIndex = 0;
				gaugeNode.Tag = gauge;
				PopulateGauge(gaugeNode);
				subGaugesNode.Nodes.Add(gaugeNode);
			}
		}

		private void PopulateScale(TreeNode scaleNode)
		{
			Scale scale = (Scale)scaleNode.Tag;
			//create ranges node
			TreeNode rangesNode = new TreeNode("Ranges");
			rangesNode.Tag = (scale.Ranges);
			rangesNode.ImageIndex = 5;
			rangesNode.SelectedImageIndex = 5;
			PopulateRanges(rangesNode);
			scaleNode.Nodes.Add(rangesNode);

			//create pointers node
			TreeNode pointersNode = new TreeNode("Pointers");
			pointersNode.Tag = scale.Pointers;
			pointersNode.ImageIndex = 6;
			pointersNode.SelectedImageIndex = 6;
			PopulatePointers(pointersNode);
			scaleNode.Nodes.Add(pointersNode);
		}

		private void PopulateScales(TreeNode scalesNode)
		{
			foreach(Scale scale in (ScaleCollection)(scalesNode.Tag))
			{
				//create scale node
				TreeNode scaleNode = new TreeNode(scale.Name);
				scaleNode.Tag = scale;
				scaleNode.ImageIndex = 7;
				scaleNode.SelectedImageIndex = 7;
				PopulateScale(scaleNode);
				scalesNode.Nodes.Add(scaleNode);
			}
		}

		private void PopulateRanges(TreeNode rangesNode)
		{
			foreach(Range range in (RangeCollection)(rangesNode.Tag))
			{
				//create range node
				TreeNode rangeNode = new TreeNode(range.Name);
				rangeNode.ImageIndex = 8;
				rangeNode.SelectedImageIndex = 8;
				rangeNode.Tag = range;

				//create annotations node
				TreeNode annotationsNode = new TreeNode("Annotations");
				annotationsNode.Tag = range.Annotations;
				annotationsNode.ImageIndex = 9;
				annotationsNode.SelectedImageIndex = 9;
				PopulateAnnotations(annotationsNode);
				rangeNode.Nodes.Add(annotationsNode);

				rangesNode.Nodes.Add(rangeNode);
			}
		}

		private void PopulatePointers(TreeNode pointersNode)
		{
			foreach(Pointer pointer in (PointerCollection)(pointersNode.Tag))
			{
				//create pointer node
				TreeNode pointerNode = new TreeNode(pointer.Name);
				pointerNode.Tag = pointer;
				pointerNode.ImageIndex = 10;
				pointerNode.SelectedImageIndex = 10;
				pointersNode.Nodes.Add(pointerNode);
			}
		}
		
		private void PopulateAnnotations(TreeNode annotationsNode)
		{
			foreach(Annotation annotation in (AnnotationCollection)(annotationsNode.Tag))
			{
				//create annotation node
				TreeNode annotationNode = new TreeNode(annotation.Name);
				annotationNode.Tag = annotation;
				annotationNode.ImageIndex = 11;
				annotationNode.SelectedImageIndex = 11;
				annotationsNode.Nodes.Add(annotationNode);
			}
		}

		private void PopulateIndicators(TreeNode indicatorsNode)
		{
			foreach(Indicator indicator in (IndicatorCollection)(indicatorsNode.Tag))
			{
				//create indicator node
				TreeNode indicatorNode = new TreeNode(indicator.Name);
				indicatorNode.Tag = indicator;
				indicatorNode.ImageIndex = 12;
				indicatorNode.SelectedImageIndex = 12;
				indicatorsNode.Nodes.Add(indicatorNode);
			}
		}


		private void PopulateImageAnnotations(TreeNode imageAnnotationsNode)
		{
			foreach(ImageAnnotation imageAnnotation in (ImageAnnotationCollection)(imageAnnotationsNode.Tag))
			{
				//create image annotation node
				//TODO uncomment the nexct line
				TreeNode imageAnnotationNode = new TreeNode("Image Annotation"/*imageAnnotation.Name*/);
				imageAnnotationNode.Tag = imageAnnotation;
				imageAnnotationNode.ImageIndex = 13;
				imageAnnotationNode.SelectedImageIndex = 13;
				imageAnnotationsNode.Nodes.Add(imageAnnotationNode);
			}
		}

		private void PopulateTextAnnotations(TreeNode textAnnotationsNode)
		{
			foreach(TextAnnotation textAnnotation in (TextAnnotationCollection)(textAnnotationsNode.Tag))
			{
				//create text annotation node
				//TODO uncomment the next line
				TreeNode textAnnotationNode = new TreeNode("Text Annotation"/*textAnnotation.Name*/);
				textAnnotationNode.Tag = textAnnotation;
				textAnnotationNode.ImageIndex = 14;
				textAnnotationNode.SelectedImageIndex = 14;
				textAnnotationsNode.Nodes.Add(textAnnotationNode);
			}
		}

		private void OnSelect(object sender, TreeViewEventArgs e)
		{
			if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
				return;

			if (previouslySelectedNode != null)
			{
				previouslySelectedNode.BackColor = Color.White;
				previouslySelectedNode.ForeColor = Color.Black;
			}
			treeView.SelectedNode.BackColor = Color.FromArgb(49, 106, 197);
			treeView.SelectedNode.ForeColor = Color.White;
			previouslySelectedNode = treeView.SelectedNode;

			EnableAddRemoveButtons();
			
			//If it is not the root node do the AfterSelect event
			if(treeView.SelectedNode.Tag != treeView.Nodes[0].Tag)
			{
				if (AfterSelect != null)
				{
					TreeNodeSelectEventArgs arg = new TreeNodeSelectEventArgs(treeView.SelectedNode.Tag);
					AfterSelect(this, arg);
				}
			}
		}

		//Enable and disable the add/remove buttons
		private void EnableAddRemoveButtons()
		{
			addButton.Enabled = false;
			removeButton.Enabled = false;
			if (treeView.SelectedNode.Tag is NamedObjectCollection)
			{
				addButton.Enabled = true;
			}
			else if (treeView.SelectedNode.Tag is NamedObject && treeView.SelectedNode.Tag != treeView.Nodes[0].Tag)
			{
				if (!((NamedObject)treeView.SelectedNode.Tag).IsRequired)
					removeButton.Enabled = true;
			}
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			if (treeView.SelectedNode != null && treeView.SelectedNode.Tag is NamedObjectCollection)
			{
				NamedObject newObject = ((NamedObjectCollection)treeView.SelectedNode.Tag).CreateNewMember();

				//create scale node
				TreeNode node = new TreeNode(newObject.Name);
				node.Tag = newObject;
				if (newObject is Scale)
				{
					node.ImageIndex = 7;
					node.SelectedImageIndex = 7;
					PopulateScale(node);
				}
				else if (newObject is SubGauge)
				{	
					node.ImageIndex = 0;
					node.SelectedImageIndex = 0;
					PopulateGauge(node);
				}
				else if (newObject is Range)
				{
					node.ImageIndex = 8;
					node.SelectedImageIndex = 8;
				}
				else if (newObject is Pointer)
				{
					node.ImageIndex = 10;
					node.SelectedImageIndex = 10;
				}
				else if (newObject is Indicator)
				{
					node.ImageIndex = 12;
					node.SelectedImageIndex = 12;
				}
				else if (newObject is Annotation)
				{
					node.ImageIndex = 11;
					node.SelectedImageIndex = 11;
				}
				else if (newObject is TextAnnotation)
				{
					node.ImageIndex = 14;
					node.SelectedImageIndex = 14;
				}
				else if (newObject is ImageAnnotation)
				{
					node.ImageIndex = 13;
					node.SelectedImageIndex = 13;
				}

				treeView.SelectedNode.Nodes.Add(node);
				treeView.SelectedNode.ExpandAll();

				treeView.SelectedNode = node;
				if (previouslySelectedNode != null)
				{
					previouslySelectedNode.BackColor = Color.White;
					previouslySelectedNode.ForeColor = Color.Black;
				}
				treeView.SelectedNode.BackColor = Color.FromArgb(49, 106, 197);
				treeView.SelectedNode.ForeColor = Color.White;
				previouslySelectedNode = treeView.SelectedNode;

				if (Changed != null)
				{
					Changed(this);
				}
			}

		}

		private void removeButton_Click(object sender, System.EventArgs e)
		{
			if(treeView.SelectedNode != null && treeView.SelectedNode.Tag is NamedObject)
			{
				if (!((NamedObject)treeView.SelectedNode.Tag).IsRequired)
				{
					((NamedObjectCollection)treeView.SelectedNode.Parent.Tag).Remove((NamedObject)treeView.SelectedNode.Tag);
					treeView.SelectedNode.Remove();
				}
			}
			
			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void treeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_SelectedNode = treeView.GetNodeAt(e.X, e.Y);
		}

		private void treeView_DoubleClick(object sender, System.EventArgs e)
		{
			if (m_SelectedNode == null && m_SelectedNode.Parent == null)
				return;
			treeView.SelectedNode = m_SelectedNode;
		
			if (treeView.SelectedNode.Tag is NamedObject && treeView.SelectedNode.Tag != treeView.Nodes[0].Tag && !((NamedObject)treeView.SelectedNode.Tag).IsRequired)
			{
				treeView.LabelEdit = true;
				treeView.SelectedNode.BeginEdit();
			}
		}

		private void treeView_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if (e.Label != null)
			{
				if(e.Label.Length > 0)
				{
					e.Node.EndEdit(false);
					if (treeView.SelectedNode.Tag is NamedObject)
					{
						((NamedObject)treeView.SelectedNode.Tag).Name = e.Label;
					}
				}
				else
				{
					// Cancel the label edit action, inform the user, and place the node in edit mode again
					e.CancelEdit = true;
					MessageBox.Show("Invalid object label.\n The label cannot be blank.", "Object Label Edit");
					e.Node.BeginEdit();
				}
				this.treeView.LabelEdit = false;
			}	
		}
	}
}
