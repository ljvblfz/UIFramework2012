using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using System.Reflection;
using ComponentArt.Win.UI.Internal;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardSeriesStyleDialog : WizardDialog
	{
		private ListBoxForChartTypes m_compositionListBox;
		private ComponentArt.Win.UI.Internal.ComboBox m_optionsComboBox;
		private System.ComponentModel.IContainer components;
		private ComponentArt.Win.UI.Internal.ComboBox m_compositionKindComboBox;

		private ComponentArt.Win.UI.Internal.GroupBox groupBox1;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox m_filterComboBox;

		private ComponentArt.Win.UI.Internal.CheckBox m_3DCheckBox;
		private System.Windows.Forms.Label label2;

		Bitmap [] m_chartTypeBitmaps;
		private ComponentArt.Win.UI.Internal.GroupBox m_generatingChartsGroupBox;
		private ComponentArt.Win.UI.Internal.ProgressBar m_ourChartGenerationProgressBar;
		ChartTypeThumbnail [] m_thumbnails;

		bool m_doNotSetupListBox = false;

		public WizardSeriesStyleDialog() : this (null) { }

		public WizardSeriesStyleDialog(WinChart chart) : base (chart)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_chartTypeBitmaps = new Bitmap [] 
				{
					GetChartTypeListImages("barGanttCube"),
					GetChartTypeListImages("area"),
					GetChartTypeListImages("line"),
					GetChartTypeListImages("pieDoughnut"),
					GetChartTypeListImages("bubbleMarker"),
					GetChartTypeListImages("financial"),
					GetChartTypeListImages("radar")
				};


			m_filterComboBox.Items.AddRange( new String [] {
															   "Bar",
															   "Area",
															   "Line",
															   "Pie-Doughnut",
															   "Bubble-Marker",
															   "Financial" ,
															   "Radar"
														   });


			m_filterComboBox.SelectedIndex = -1;

			m_compositionListBox.Invalidate();

		}

		private Bitmap GetChartTypeListImages(string filename) 
		{
			return (Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream(filename + ".png"));
		}

		private SeriesStyle m_chartStyle;		

		internal SeriesStyle SeriesStyle 
		{
			set 
			{
				m_chartStyle = value;
			}

			get 
			{
				return m_chartStyle;
			}
		}

		ProjectionKind m_default3DProjectionKind = ProjectionKind.CentralProjection;

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (Wizard == null)
				return;

			Wizard.ToolTip.SetToolTip(m_filterComboBox, "Main type");
			Wizard.ToolTip.SetToolTip(m_optionsComboBox, "Sub-type");
			Wizard.ToolTip.SetToolTip(m_compositionKindComboBox, "Composition of multiple series");

			if (WinChart == null)
				return;

			ArrayList l = new ArrayList(Enum.GetValues(typeof(CompositionKind)));
			
			m_compositionKindComboBox.Items.AddRange((object [])l.ToArray(typeof(object)));
			m_compositionKindComboBox.Text = WinChart.CompositionKind.ToString();

            UpdateGUI();
		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			Wizard.DT.CheckPoint("Start Painting WizardSeriesStyleDialog");
			base.OnPaint(e);
			Wizard.DT.CheckPoint("End Painting WizardSeriesStyleDialog");
		}

		protected override void OnVisibleChanged ( System.EventArgs e ) 
		{
			Wizard.DT.CheckPoint("Start WizardSeriesStyleDialog::OnVisibleChanged");
			base.OnVisibleChanged(e);

			if (WinChart == null || Visible == false)
				return;

            UpdateGUI();

			Wizard.DT.CheckPoint("End WizardSeriesStyleDialog::OnVisibleChanged");
		}

        void UpdateGUI()
        {
            m_filterComboBox.SelectedIndex =
    (int)((ComponentArt.Web.Visualization.Charting.SeriesStyle)WinChart.SeriesStyles[WinChart.MainStyle]).ChartKindCategory;

            if (WinChart.Mapping.Kind != ProjectionKind.TwoDimensional)
                m_default3DProjectionKind = WinChart.Mapping.Kind;

            // Images
            m_thumbnails = WizardImageGenerator.Generate(WinChart, m_compositionListBox.ImageList);

            m_3DCheckBox.Checked = (WinChart.Mapping.Kind != ProjectionKind.TwoDimensional);
            SeriesStyle = WinChart.RootSeries.Style;

            SetupListBoxItems();

        }

		internal class ChartTypeThumbnail  
		{
			int m_imageIndex;

			SeriesStyle m_style;
			ProjectionKind m_pk;
			ChartKind m_chartKind;
			CompositionKind m_compositionKind;
			Bitmap m_bitmapInListBox = null;

			public ChartTypeThumbnail(
				int imageIndex, 
				ProjectionKind pk, 
				ChartKind chartKind, 
				CompositionKind compositionKind,
				SeriesStyle style) 
			{
				m_imageIndex = imageIndex;
				m_chartKind = chartKind;
				m_pk = pk;
				m_compositionKind = compositionKind;
				m_style = style;
			}

			public Bitmap BitmapInListBox 
			{
				set 
				{
					m_bitmapInListBox = value;
				}
				get 
				{
					return m_bitmapInListBox;
				}
			}

			public int ImageIndex
			{
				get {return m_imageIndex;}
				set {m_imageIndex = value;}
			}

			public CompositionKind CompositionKind 
			{
				get {return m_compositionKind;}
				set {m_compositionKind = value;}
			}


			public ChartKind ChartKind 
			{
				get {return m_chartKind;}
				set {m_chartKind = value;}
			}

			public ChartKindCategory ChartKindCategory
			{
				get {return m_style.ChartKindCategory;}
			}

			public string StyleName 
			{
				get {return m_style.Name;}
			}

			public SeriesStyle SeriesStyle 
			{
				get {return m_style;}
			}

			public ProjectionKind ProjectionKind 
			{
				get {return m_pk;}
				set {m_pk = value;}
			}

			public ListBoxThumbnail ToListBoxThumbnail() 
			{
				string shortComposition = "";
				switch (CompositionKind) 
				{
					case CompositionKind.Concentric:
						shortComposition = "Conc.";
						break;
					case CompositionKind.Sections:
						shortComposition = "Sect.";
						break;
					case CompositionKind.Merged:
						shortComposition = "Merg.";
						break;
					case CompositionKind.Stacked:
						shortComposition = "Stcd.";
						break;
					case CompositionKind.Stacked100:
						shortComposition = "S100.";
						break;
					case CompositionKind.MultiSystem:
						shortComposition = "MSys.";
						break;
					case CompositionKind.MultiArea:
						shortComposition = "MArea.";
						break;
				}

				string shortStyleName = StyleName.Length > 10 ? StyleName.Substring(0, 9) + "..." : StyleName;

				ListBoxThumbnail lbt = new ListBoxThumbnail(StyleName + ", " + CompositionKind.ToString(), shortStyleName + ", " + shortComposition, ImageIndex, this);
				return lbt;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_3DCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_compositionListBox = new ComponentArt.Web.Visualization.Charting.Design.ListBoxForChartTypes();
			this.m_optionsComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_compositionKindComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.groupBox1 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_generatingChartsGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_ourChartGenerationProgressBar = new ComponentArt.Win.UI.Internal.ProgressBar();
			this.groupBox2 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.m_filterComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.m_generatingChartsGroupBox.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_3DCheckBox
			// 
			this.m_3DCheckBox.BackColor = System.Drawing.Color.White;
			this.m_3DCheckBox.Checked = true;
			this.m_3DCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_3DCheckBox.Location = new System.Drawing.Point(536, 36);
			this.m_3DCheckBox.Name = "m_3DCheckBox";
			this.m_3DCheckBox.Size = new System.Drawing.Size(40, 16);
			this.m_3DCheckBox.TabIndex = 6;
			this.m_3DCheckBox.Text = "3D";
			this.m_3DCheckBox.CheckedChanged += new System.EventHandler(this.m_3DCheckBox_CheckedChanged);
			// 
			// m_compositionListBox
			// 
			this.m_compositionListBox.BackColor = System.Drawing.Color.White;
			this.m_compositionListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_compositionListBox.IntegralHeight = true;
			this.m_compositionListBox.ItemHeight = 90;
			this.m_compositionListBox.Location = new System.Drawing.Point(2, 30);
			this.m_compositionListBox.MultiColumn = true;
			this.m_compositionListBox.Name = "m_compositionListBox";
			this.m_compositionListBox.SelectedIndex = -1;
			this.m_compositionListBox.SelectedItem = null;
			this.m_compositionListBox.Size = new System.Drawing.Size(580, 208);
			this.m_compositionListBox.TabIndex = 10;
			this.m_compositionListBox.HighlightedIndexChanged += new System.EventHandler(this.m_compositionListBox_HighlightedIndexChanged);
			this.m_compositionListBox.SelectedIndexChanged += new System.EventHandler(this.m_compositionListBox_SelectedIndexChanged);
			// 
			// m_optionsComboBox
			// 
			this.m_optionsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_optionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_optionsComboBox.ItemHeight = 18;
			this.m_optionsComboBox.Location = new System.Drawing.Point(216, 30);
			this.m_optionsComboBox.Name = "m_optionsComboBox";
			this.m_optionsComboBox.Size = new System.Drawing.Size(114, 24);
			this.m_optionsComboBox.TabIndex = 4;
			this.m_optionsComboBox.SelectedIndexChanged += new System.EventHandler(this.m_optionsComboBox_SelectedIndexChanged);
			// 
			// m_compositionKindComboBox
			// 
			this.m_compositionKindComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_compositionKindComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_compositionKindComboBox.ItemHeight = 18;
			this.m_compositionKindComboBox.Location = new System.Drawing.Point(416, 30);
			this.m_compositionKindComboBox.Name = "m_compositionKindComboBox";
			this.m_compositionKindComboBox.Size = new System.Drawing.Size(114, 24);
			this.m_compositionKindComboBox.TabIndex = 5;
			this.m_compositionKindComboBox.SelectedIndexChanged += new System.EventHandler(this.m_compositionKindComboBox_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_generatingChartsGroupBox,
																					this.m_compositionListBox});
			this.groupBox1.Location = new System.Drawing.Point(0, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.ResizeChildren = false;
			this.groupBox1.SimpleGroupBox = false;
			this.groupBox1.Size = new System.Drawing.Size(584, 240);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Chart Types:";
			// 
			// m_generatingChartsGroupBox
			// 
			this.m_generatingChartsGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									 this.m_ourChartGenerationProgressBar});
			this.m_generatingChartsGroupBox.Location = new System.Drawing.Point(16, 80);
			this.m_generatingChartsGroupBox.Name = "m_generatingChartsGroupBox";
			this.m_generatingChartsGroupBox.ResizeChildren = false;
			this.m_generatingChartsGroupBox.SimpleGroupBox = false;
			this.m_generatingChartsGroupBox.Size = new System.Drawing.Size(552, 64);
			this.m_generatingChartsGroupBox.TabIndex = 39;
			this.m_generatingChartsGroupBox.TabStop = false;
			this.m_generatingChartsGroupBox.Text = "Generating Charts...";
			// 
			// m_ourChartGenerationProgressBar
			// 
			this.m_ourChartGenerationProgressBar.Location = new System.Drawing.Point(8, 32);
			this.m_ourChartGenerationProgressBar.Name = "m_ourChartGenerationProgressBar";
			this.m_ourChartGenerationProgressBar.Size = new System.Drawing.Size(536, 23);
			this.m_ourChartGenerationProgressBar.TabIndex = 5;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_compositionKindComboBox,
																					this.label2,
																					this.m_filterComboBox,
																					this.label1,
																					this.m_3DCheckBox,
																					this.m_optionsComboBox});
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.ResizeChildren = false;
			this.groupBox2.SimpleGroupBox = false;
			this.groupBox2.Size = new System.Drawing.Size(584, 64);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Chart Type Filter:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label2.Location = new System.Drawing.Point(336, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Composition:";
			// 
			// m_filterComboBox
			// 
			this.m_filterComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_filterComboBox.ItemHeight = 18;
			this.m_filterComboBox.Location = new System.Drawing.Point(88, 30);
			this.m_filterComboBox.Name = "m_filterComboBox";
			this.m_filterComboBox.Size = new System.Drawing.Size(121, 24);
			this.m_filterComboBox.TabIndex = 3;
			this.m_filterComboBox.SelectedIndexChanged += new System.EventHandler(this.m_filterComboBox_SelectedIndexChanged);
			this.m_filterComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.m_filterComboBox_DrawItem);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label1.Location = new System.Drawing.Point(8, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Chart Type:";
			// 
			// WizardSeriesStyleDialog
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.groupBox2});
			this.Name = "WizardSeriesStyleDialog";
			this.Size = new System.Drawing.Size(584, 344);
			this.groupBox1.ResumeLayout(false);
			this.m_generatingChartsGroupBox.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		int loopThroughImages(bool render) 
		{
			int needToRender = 0;
			foreach (ChartTypeThumbnail ctt in m_thumbnails) 
			{
				// Check chart type and projection
				if ((int)ctt.ChartKindCategory == m_filterComboBox.SelectedIndex && ((ctt.ProjectionKind == ProjectionKind.TwoDimensional ) ^ m_3DCheckBox.Checked)) 
				{

					// Check sub type
					if (m_optionsComboBox.Text == "All" 
						|| ((ComponentArt.Web.Visualization.Charting.SeriesStyle)WinChart.SeriesStyles[m_optionsComboBox.Text]).Name == ctt.StyleName
						)
					{
						// Check composition
						if (m_compositionKindComboBox.Text == "All" || ctt.CompositionKind.ToString() == m_compositionKindComboBox.Text )
						{
							if (ctt.ImageIndex == -1) 
							{
								++needToRender;
								if (render) 
								{
									// Render image and update the progress bar
									WizardImageGenerator.AssignImage(ctt);
									m_ourChartGenerationProgressBar.PerformStep();
									m_ourChartGenerationProgressBar.Refresh();
									
								}
							}

							if (render) 
							{
								// Add the item to the List
								int addedIndex = m_compositionListBox.Items.Add(ctt.ToListBoxThumbnail());

								// Make is selected if needed
								if (ctt.StyleName == WinChart.MainStyle 
									&& ctt.CompositionKind == WinChart.CompositionKind)
								{
									m_compositionListBox.SelectedIndex = addedIndex;
								}
							}
						}
					}
				}
			}

			return needToRender;
		}

		void SetupListBoxItems() 
		{
			if (m_doNotSetupListBox)
				return;

			if (m_thumbnails == null)
				return;

			m_loadingThumbnails = true;

			m_compositionListBox.Items.Clear();
			m_compositionListBox.ScrollBar.Value = m_compositionListBox.ScrollBar.Minimum;
			m_compositionListBox.SelectedIndex = -1;

				
			int needToRender = loopThroughImages(false);

			if (needToRender > 0) 
			{
				m_compositionListBox.Visible = false;

				m_ourChartGenerationProgressBar.Minimum = 0;
				m_ourChartGenerationProgressBar.Maximum = needToRender;
				m_ourChartGenerationProgressBar.Value = 1;
				m_ourChartGenerationProgressBar.Step = 1;

				// Display the ProgressBar control.
				m_generatingChartsGroupBox.Visible = true;
				if(Wizard != null)
					Wizard.Refresh();
			}

				
			loopThroughImages(true);
			
				
			if (m_ourChartGenerationProgressBar.Visible) 
			{
				m_generatingChartsGroupBox.Visible = false;
				m_compositionListBox.Visible = true;
			}
				
			m_loadingThumbnails = false;

			m_compositionListBox.ScrollBar.Enabled = m_compositionListBox.Items.Count>10;

			m_compositionListBox.Invalidate();
		}

		bool m_settingUpListBox = false;

		private void m_optionsComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!m_settingUpListBox)
				SetupListBoxItems();

			if (m_compositionListBox.SelectedIndex == -1 && m_compositionListBox.Items.Count > 0)
				m_compositionListBox.SelectedIndex = 0;
		}

        bool m_loadingThumbnails = false;

		private void m_compositionListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_compositionListBox.Refreshing)
				return;

			if (m_compositionListBox.SelectedIndex == -1)
				return;

			if (m_loadingThumbnails)
				return;

			ChartTypeThumbnail ctt = ((ChartTypeThumbnail)((ListBoxThumbnail)m_compositionListBox.SelectedItem).Item);

			if (WinChart != null && SeriesStyle != null) 
			{
				foreach (ComponentArt.Web.Visualization.Charting.SeriesStyle ss in WinChart.SeriesStyles) 
				{               
					if (ss.Name == ctt.StyleName) 
					{
						WinChart.MainStyle = ss.Name;
						
						if(Wizard != null)
							Wizard.CoordSysEnabled = !(ss.ChartKind == ChartKind.Pie || ss.ChartKind == ChartKind.Doughnut || ss.IsRadar);

						WinChart.CompositionKind = ctt.CompositionKind;
						break;
					}
				}

				WinChart.Invalidate();
			}
		}

		private bool Match(CompositionKind ck) 
		{
			if (m_compositionKindComboBox.SelectedItem == null)
				return false;

			CompositionKind compositionKindInComboBox = (CompositionKind)m_compositionKindComboBox.SelectedItem;

			if ((string)m_filterComboBox.SelectedItem == "Bar-Gantt-Cube"
				&& m_compositionKindComboBox.SelectedItem != null
				&& ck == CompositionKind.Merged)
				return true;

			if ((string)m_filterComboBox.SelectedItem == "Area" 
				&& m_compositionKindComboBox.SelectedItem != null
				&& compositionKindInComboBox == CompositionKind.Merged
				&& ck == CompositionKind.Sections)
				return true;

			return false;
		}

		private void m_compositionKindComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetupListBoxItems();

			if (m_compositionListBox.SelectedIndex == -1 && m_compositionListBox.Items.Count > 0)
				m_compositionListBox.SelectedIndex = 0;

			m_compositionListBox.Invalidate();
		}
	
		private void m_filterComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (WinChart == null)
				return;

			bool doNotSetupListBoxState = m_doNotSetupListBox;
			m_doNotSetupListBox = true;

			// Clear all styles
			m_optionsComboBox.Items.Clear();
			
			ChartKindCategory ct = (ChartKindCategory)m_filterComboBox.SelectedIndex;

			m_optionsComboBox.Items.Add("All");

			// Put all styles of that chart type
			foreach ( ComponentArt.Web.Visualization.Charting.SeriesStyle ss in WinChart.SeriesStyles ) 
			{
				if (ss.ChartKindCategory == ct) 
				{
					m_optionsComboBox.Items.Add(ss.Name);
				}
			}

			if (m_optionsComboBox.Items.Count != 0)
				m_optionsComboBox.SelectedIndex = 0;

			// Remember the value of the composition box
			object oldComposition = m_compositionKindComboBox.SelectedItem;

			// fill the composition combobox
			m_compositionKindComboBox.Items.Clear();

			if (ct == ChartKindCategory.Bar ) 
			{
				m_compositionKindComboBox.Items.AddRange(new object[] {CompositionKind.Sections, CompositionKind.Stacked, CompositionKind.Stacked100, CompositionKind.Merged, CompositionKind.MultiSystem, CompositionKind.MultiArea});
			} 
			else if (ct == ChartKindCategory.Area)
			{
				m_compositionKindComboBox.Items.AddRange(new object[] {CompositionKind.Sections, CompositionKind.Stacked, CompositionKind.Stacked100, CompositionKind.MultiSystem, CompositionKind.MultiArea });
			}
			else if (ct == ChartKindCategory.PieDoughnut) 
			{
				m_compositionKindComboBox.Items.AddRange(new object[] {CompositionKind.Concentric, CompositionKind.MultiArea});
			}
			else
			{
				CompositionKind [] cks = null;

				if (ct == ChartKindCategory.Line )
					cks = new CompositionKind [] {CompositionKind.Sections, CompositionKind.Stacked, CompositionKind.Stacked100, CompositionKind.Merged, CompositionKind.MultiSystem, CompositionKind.MultiArea};
				else if (ct == ChartKindCategory.Marker)
					cks = new CompositionKind [] {CompositionKind.Sections, CompositionKind.Merged, CompositionKind.MultiSystem, CompositionKind.MultiArea};
				else if (ct == ChartKindCategory.Financial)
					cks = new CompositionKind [] {CompositionKind.Sections, CompositionKind.MultiSystem, CompositionKind.MultiArea};

				if (cks != null)
					foreach (CompositionKind ck in cks)
						m_compositionKindComboBox.Items.Add(ck);
			}

			m_compositionKindComboBox.Items.Insert(0, "All");

			m_compositionKindComboBox.SelectedItem = oldComposition;
			if (m_compositionKindComboBox.SelectedItem == null) 
			{
				if (m_compositionKindComboBox.Items.Count > 1)
					m_compositionKindComboBox.SelectedIndex = 1;
				else if (m_compositionKindComboBox.Items.Count == 1)
					m_compositionKindComboBox.SelectedIndex = 0;
			}
			
			m_doNotSetupListBox = doNotSetupListBoxState;
			
			SetupListBoxItems();
			
			if (m_compositionListBox.SelectedIndex == -1 && m_compositionListBox.Items.Count > 0)
				m_compositionListBox.SelectedIndex = 0;

			m_compositionListBox.Invalidate();
		}

		private void m_filterComboBox_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			if (e.Index == -1)
				return;

			Color backColor = Color.White;
			Color foreColor = Color.Black;
			
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				backColor = Color.FromArgb(221, 52, 9);
				foreColor = Color.White;
			}

			e.Graphics.FillRectangle(new SolidBrush(backColor), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

			Graphics g = e.Graphics;

			Pen pen = new Pen(Color.FromArgb(238,238,238));
			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			g.DrawLine(pen
				, new Point( e.Bounds.X, e.Bounds.Y+e.Bounds.Height-1)
				, new Point( e.Bounds.X+e.Bounds.Width-1, e.Bounds.Y+e.Bounds.Height-1)
				);

			g.DrawImage(m_chartTypeBitmaps[e.Index], e.Bounds.X +1, e.Bounds.Y +1, m_chartTypeBitmaps[e.Index].Width, m_chartTypeBitmaps[e.Index].Height);
			g.DrawString(m_filterComboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(foreColor), e.Bounds.X+20, e.Bounds.Y + 3);

			// If the ListBox has focus, draw a focus rectangle around the selected item.
			e.DrawFocusRectangle();
		}

		private void m_3DCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			SetupListBoxItems();

			if (m_3DCheckBox.Checked)
				WinChart.Mapping.Kind = m_default3DProjectionKind;
			else
				WinChart.Mapping.Kind = ProjectionKind.TwoDimensional;

			m_compositionListBox.Invalidate();

			WinChart.Invalidate();
		}

		private void m_compositionListBox_HighlightedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_compositionListBox.HighlightedIndex != -1 && Wizard != null)
				Wizard.ToolTip.SetToolTip(m_compositionListBox, ((ListBoxThumbnail)m_compositionListBox.Items[m_compositionListBox.HighlightedIndex]).Caption);
		}

		private void m_loadLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Point old_loc = WinChart.Location;
			Size old_size = WinChart.Size;

			try
			{
				ChartXmlSerializer.OpenXmlTemplate(WinChart);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				MessageBox.Show(null,"Invalid ComponentArt Web.Visualization.Charting color palette\n\n" +
					ex.Message + "\n\nCaution: Chart object is not completely loaded","Error in loading from template");
				throw;
			}
			WinChart.Location = old_loc;
			WinChart.Size = old_size;

			m_thumbnails = WizardImageGenerator.Generate(WinChart, m_compositionListBox.ImageList);
			SetupListBoxItems();

			m_compositionListBox.Invalidate();

			WinChart.Invalidate();
		}

		private void m_saveLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				ChartXmlSerializer.SaveXmlTemplate(WinChart,"Chart");
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, ex.StackTrace);
				throw;
			}
		}
	}

	internal class ListBoxForChartTypes : ComponentArt.Win.UI.Internal.ThumbnailListBox
	{
	}
}