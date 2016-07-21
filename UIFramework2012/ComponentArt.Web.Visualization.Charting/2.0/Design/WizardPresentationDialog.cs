using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using ComponentArt.Win.UI.Internal;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardPresentationDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.Button RemoveButton;
		private ComponentArt.Win.UI.Internal.Button AddButton;
		private System.Windows.Forms.CheckBox m_isRangeCheckBox;
		private ComponentArt.Win.UI.Internal.TabPage m_mainPresentationTabPage;
		private ComponentArt.Win.UI.Internal.TabControl m_presentationTabControl;
		private ComponentArt.Web.Visualization.Charting.Design.WizardTreeView seriesTreeView;
		private ComponentArt.Web.Visualization.Charting.SeriesStyleTreeView m_seriesStylesTreeView;
		[WizardHint("SeriesStyle")]
		private ComponentArt.Win.UI.Internal.GroupBox m_styleGroupBox;
		[WizardHint(typeof(CompositeSeries), "CompositionKind")]
		private ComponentArt.Win.UI.Internal.GroupBox m_compositeGroupBox;
		private System.Windows.Forms.ComboBox m_compositeComboBox;
		[WizardHint("SeriesTree")]
		private ComponentArt.Win.UI.Internal.GroupBox m_seriesTreeGroupBox;
		private ComponentArt.Win.UI.Internal.TabPage tabPage1;
		private ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox m_DPLStyleNameComboBox;
		[WizardHint("SeriesDataPointLabelStyle")]
		private ComponentArt.Win.UI.Internal.GroupBox m_dataPointLabelStyleGroupBox;
		private ComponentArt.Win.UI.Internal.TrackBar m_seriesTransparencyTrackBar;
		[WizardHint(typeof(Series), "Transparency")]
		private ComponentArt.Win.UI.Internal.GroupBox m_seriesTransparencyGroupBox;
		private System.Windows.Forms.Label m_seriesTransparencyValueLabel;
		[WizardHint(typeof(Series), "IsRange")]
		private ComponentArt.Win.UI.Internal.GroupBox m_isRangeGroupBox;
		private ComponentArt.Web.Visualization.Charting.Design.WizardDataBindingDialog wizardDataBindingDialog1;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox m_independentYCheckBox;
		private System.ComponentModel.IContainer components;

		public WizardPresentationDialog()
		{
			InitializeComponent();

            m_presentationTabControl.SelectedIndexChanged += new EventHandler(m_presentationTabControl_SelectedIndexChanged);
            FurtherInitializations();
		}

        void m_presentationTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

#if !__BUILDING_CRI_DESIGNER__
		private ComponentArt.Win.UI.Internal.TabPage m_advancedTabPage;
		private ComponentArt.Web.Visualization.Charting.Design.WizardSeriesAdvancedDialog m_wizardSeriesAdvancedDialog;
#else
        private ComponentArt.Win.UI.Internal.TabPage m_filterTabPage;
        internal ComponentArt.Charting.FiltersControl m_wizardFiltersControl;
        private ComponentArt.Win.UI.Internal.TabPage m_sortingTabPage;
        internal ComponentArt.Charting.SortingControl m_wizardSortingControl;
#endif

		void FurtherInitializations() 
		{
#if !__BUILDING_CRI_DESIGNER__
			this.m_advancedTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			
			this.m_wizardSeriesAdvancedDialog = new ComponentArt.Web.Visualization.Charting.Design.WizardSeriesAdvancedDialog();

			this.m_advancedTabPage.SuspendLayout();

			
			this.m_advancedTabPage.BackColor = System.Drawing.Color.White;
			this.m_advancedTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_wizardSeriesAdvancedDialog});
			this.m_advancedTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_advancedTabPage.Name = "m_advancedTabPage";
			this.m_advancedTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_advancedTabPage.TabIndex = 2;
			this.m_advancedTabPage.Text = "Advanced";


			this.m_wizardSeriesAdvancedDialog.BackColor = System.Drawing.Color.White;
			this.m_wizardSeriesAdvancedDialog.DefaultHint = "Manage advanced series properties";
			this.m_wizardSeriesAdvancedDialog.DefaultHintTitle = "Series";
			this.m_wizardSeriesAdvancedDialog.Name = "m_wizardSeriesAdvancedDialog";
			this.m_wizardSeriesAdvancedDialog.Size = new System.Drawing.Size(320, 272);
			this.m_wizardSeriesAdvancedDialog.TabIndex = 0;


			this.m_presentationTabControl.Controls.Add(m_advancedTabPage);
			this.m_advancedTabPage.ResumeLayout(false);
#else
            m_filterTabPage = new ComponentArt.Win.UI.Internal.TabPage();
            m_wizardFiltersControl = new FiltersControl();

            m_filterTabPage.BackColor = System.Drawing.Color.White;
            m_filterTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_wizardFiltersControl});
            m_filterTabPage.Location = new System.Drawing.Point(2, 25);
            m_filterTabPage.Name = "m_filtersTabPage";
            m_filterTabPage.Size = new System.Drawing.Size(320, 285);
            m_filterTabPage.TabIndex = 2;
            m_filterTabPage.Text = "Filters";

            m_wizardFiltersControl.BackColor = System.Drawing.Color.White;
            m_wizardFiltersControl.Name = "m_wizardFiltersControl";
            m_wizardFiltersControl.Size = new System.Drawing.Size(320, 272);
            m_wizardFiltersControl.TabIndex = 0;

            m_presentationTabControl.Controls.Add(m_filterTabPage);

            m_sortingTabPage = new ComponentArt.Win.UI.Internal.TabPage();
            m_wizardSortingControl = new SortingControl();

            m_sortingTabPage.BackColor = System.Drawing.Color.White;
            m_sortingTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_wizardSortingControl});
            m_sortingTabPage.Location = new System.Drawing.Point(2, 25);
            m_sortingTabPage.Name = "m_sortingTabPage";
            m_sortingTabPage.Size = new System.Drawing.Size(320, 285);
            m_sortingTabPage.TabIndex = 3;
            m_sortingTabPage.Text = "Sorting";

            m_wizardSortingControl.BackColor = System.Drawing.Color.White;
            m_wizardSortingControl.Name = "m_wizardSortingControl";
            m_wizardSortingControl.Size = new System.Drawing.Size(320, 272);
            m_wizardSortingControl.TabIndex = 0;

            m_presentationTabControl.Controls.Add(m_sortingTabPage);

#endif
		}

		void BuildCompositeKindComboBox() 
		{
			CompositeSeries cs = SeriesBase as CompositeSeries;
			if (cs == null)
				return;

			m_compositeComboBox.Items.Clear();

			foreach (CompositionKind ck in Enum.GetValues(typeof(CompositionKind))) 
			{
				if (cs.IsApplicable(ck)) 
				{
					m_compositeComboBox.Items.Add(ck.ToString());
				}
			}
		}

		bool m_updatingStyleTree = false;

		void UpdateTreeStyle() 
		{
			if (seriesTreeView.SeriesBase == null) 
				return;
			
			m_updatingStyleTree = true;
			try 
			{
				m_seriesStylesTreeView.Populate(WinChart.SeriesStyles, seriesTreeView.SeriesBase);
			} 
			catch 
			{
			}
			m_seriesStylesTreeView.SelectedStyle = seriesTreeView.SeriesBase.Style;
			m_updatingStyleTree = false;
			m_seriesStylesTreeView.Focus();
		}

		/// <summary>
		/// Manipulate the state of buttons on selection
		/// </summary>
		public void AfterSeriesSelect(
			object sender,
			TreeViewEventArgs e
			) 
		{
			AddButton.Enabled = seriesTreeView.IsSelectedCompositeSeries();

			UpdateTreeStyle();

			m_isRangeGroupBox.Visible = !seriesTreeView.IsSelectedCompositeSeries();
			m_compositeGroupBox.Visible = seriesTreeView.IsSelectedCompositeSeries();
            m_dataPointLabelStyleGroupBox.Visible = true; // !seriesTreeView.IsSelectedCompositeSeries();
			m_seriesTransparencyGroupBox.Visible = !seriesTreeView.IsSelectedCompositeSeries();

			m_independentYCheckBox.Checked = SeriesBase.HasIndependentYAxis;

			if (m_isRangeGroupBox.Visible) 
			{
                Series series = SeriesBase as Series;
                m_isRangeCheckBox.Checked = series.IsRange;
                m_seriesTransparencyTrackBar.Value = (int)series.Transparency;
				m_seriesTransparencyValueLabel.Text = series.Transparency.ToString() + "%";
			} 
			else 
			{
				BuildCompositeKindComboBox();
				m_compositeComboBox.Text = ((CompositeSeries)SeriesBase).CompositionKind.ToString();
			}
            if (SeriesBase.Labels.Count == 0)
                m_DPLStyleNameComboBox.SelectedIndex = 0;
            else
                m_DPLStyleNameComboBox.Text = SeriesBase.Labels[0].LabelStyleName;
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


		protected override string HintMessage() 
		{
			return "Wizard Presentations Dialog";
		}


		protected override void OnVisibleChanged(EventArgs e) 
		{
			base.OnVisibleChanged(e);

			if (!Visible)
				return;
			
			UpdateTreeStyle();

			if (seriesTreeView.IsSelectedCompositeSeries())
				m_compositeComboBox.Text = ((CompositeSeries)SeriesBase).CompositionKind.ToString();

		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			SeriesStyleCollection styles = m_winchart.SeriesStyles;

			m_DPLStyleNameComboBox.SetProperty(typeof(SeriesLabels).GetProperty("LabelStyleName"), "Edit DataPoint Label Styles...");

            seriesTreeView.Focus();

            if (m_winchart == null)
                return;
				
            seriesTreeView.WinChart = m_winchart;

#if __BUILDING_CRI_DESIGNER__
            SqlChartDesigner sqlChartDesigner = ((SqlChartDesigner)WinChart.Chart.Owner);
            m_wizardFiltersControl.SqlChartDesigner = sqlChartDesigner;
            m_wizardFiltersControl.Filters = Wizard.ExtraWizardParameters.m_topLevelFilters;
            m_wizardSortingControl.SqlChartDesigner = sqlChartDesigner;
            m_wizardSortingControl.Sorting = Wizard.ExtraWizardParameters.m_xSorting;
#endif
        }


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.seriesTreeView = new ComponentArt.Web.Visualization.Charting.Design.WizardTreeView();
			this.RemoveButton = new ComponentArt.Win.UI.Internal.Button();
			this.AddButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_isRangeCheckBox = new System.Windows.Forms.CheckBox();
			this.m_presentationTabControl = new ComponentArt.Win.UI.Internal.TabControl();
			this.m_mainPresentationTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.groupBox1 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_independentYCheckBox = new System.Windows.Forms.CheckBox();
			this.m_styleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_seriesStylesTreeView = new ComponentArt.Web.Visualization.Charting.SeriesStyleTreeView();
			this.m_dataPointLabelStyleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_DPLStyleNameComboBox = new ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox();
			this.m_seriesTreeGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_isRangeGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_compositeGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_compositeComboBox = new System.Windows.Forms.ComboBox();
			this.m_seriesTransparencyGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_seriesTransparencyValueLabel = new System.Windows.Forms.Label();
			this.m_seriesTransparencyTrackBar = new ComponentArt.Win.UI.Internal.TrackBar();
			this.tabPage1 = new ComponentArt.Win.UI.Internal.TabPage();
			this.wizardDataBindingDialog1 = new ComponentArt.Web.Visualization.Charting.Design.WizardDataBindingDialog();
			this.m_presentationTabControl.SuspendLayout();
			this.m_mainPresentationTabPage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.m_styleGroupBox.SuspendLayout();
			this.m_dataPointLabelStyleGroupBox.SuspendLayout();
			this.m_seriesTreeGroupBox.SuspendLayout();
			this.m_isRangeGroupBox.SuspendLayout();
			this.m_compositeGroupBox.SuspendLayout();
			this.m_seriesTransparencyGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_seriesTransparencyTrackBar)).BeginInit();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// seriesTreeView
			// 
			this.seriesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.seriesTreeView.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.seriesTreeView.HideSelection = false;
			this.seriesTreeView.LabelEdit = true;
			this.seriesTreeView.Location = new System.Drawing.Point(2, 26);
			this.seriesTreeView.Name = "seriesTreeView";
			this.seriesTreeView.Size = new System.Drawing.Size(130, 205);
			this.seriesTreeView.TabIndex = 1;
			this.seriesTreeView.WinChart = null;
			this.seriesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSeriesSelect);
			this.seriesTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.seriesTreeView_AfterLabelEdit);
			// 
			// RemoveButton
			// 
			this.RemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.RemoveButton.Location = new System.Drawing.Point(78, 256);
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(59, 21);
			this.RemoveButton.TabIndex = 20;
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.Click += new System.EventHandler(this.Remove_Click);
			// 
			// AddButton
			// 
			this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.AddButton.Location = new System.Drawing.Point(15, 256);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(59, 21);
			this.AddButton.TabIndex = 10;
			this.AddButton.Text = "Add";
			this.AddButton.Click += new System.EventHandler(this.Add_Click);
			// 
			// m_isRangeCheckBox
			// 
			this.m_isRangeCheckBox.BackColor = System.Drawing.Color.White;
			this.m_isRangeCheckBox.Location = new System.Drawing.Point(144, 5);
			this.m_isRangeCheckBox.Name = "m_isRangeCheckBox";
			this.m_isRangeCheckBox.Size = new System.Drawing.Size(16, 16);
			this.m_isRangeCheckBox.TabIndex = 3;
			this.m_isRangeCheckBox.CheckedChanged += new System.EventHandler(this.m_isRangeCheckBox_CheckedChanged);
			// 
			// m_presentationTabControl
			// 
			this.m_presentationTabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.m_mainPresentationTabPage,
																								   this.tabPage1});
			this.m_presentationTabControl.Name = "m_presentationTabControl";
			this.m_presentationTabControl.Size = new System.Drawing.Size(324, 312);
			this.m_presentationTabControl.TabIndex = 8;
			// 
			// m_mainPresentationTabPage
			// 
			this.m_mainPresentationTabPage.BackColor = System.Drawing.Color.White;
			this.m_mainPresentationTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.groupBox1,
																									this.m_styleGroupBox,
																									this.m_dataPointLabelStyleGroupBox,
																									this.m_seriesTreeGroupBox,
																									this.RemoveButton,
																									this.AddButton,
																									this.m_isRangeGroupBox,
																									this.m_compositeGroupBox,
																									this.m_seriesTransparencyGroupBox});
			this.m_mainPresentationTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_mainPresentationTabPage.Name = "m_mainPresentationTabPage";
			this.m_mainPresentationTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_mainPresentationTabPage.TabIndex = 0;
			this.m_mainPresentationTabPage.Text = "Series";
			this.m_mainPresentationTabPage.VisibleChanged += new System.EventHandler(this.m_mainPresentationTabPage_VisibleChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.White;
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_independentYCheckBox});
			this.groupBox1.Location = new System.Drawing.Point(152, 113);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.ResizeChildren = false;
			this.groupBox1.Size = new System.Drawing.Size(160, 19);
			this.groupBox1.TabIndex = 61;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Independent Y";
			// 
			// m_independentYCheckBox
			// 
			this.m_independentYCheckBox.BackColor = System.Drawing.Color.White;
			this.m_independentYCheckBox.Location = new System.Drawing.Point(144, 5);
			this.m_independentYCheckBox.Name = "m_independentYCheckBox";
			this.m_independentYCheckBox.Size = new System.Drawing.Size(16, 16);
			this.m_independentYCheckBox.TabIndex = 3;
			this.m_independentYCheckBox.CheckedChanged += new System.EventHandler(this.m_independentYCheckBox_CheckedChanged);
			// 
			// m_styleGroupBox
			// 
			this.m_styleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this.m_seriesStylesTreeView});
			this.m_styleGroupBox.DrawBorderAroundControl = true;
			this.m_styleGroupBox.Location = new System.Drawing.Point(152, 8);
			this.m_styleGroupBox.Name = "m_styleGroupBox";
			this.m_styleGroupBox.ResizeChildren = true;
			this.m_styleGroupBox.Size = new System.Drawing.Size(160, 102);
			this.m_styleGroupBox.TabIndex = 30;
			this.m_styleGroupBox.TabStop = false;
			this.m_styleGroupBox.Text = "Style";
			// 
			// m_seriesStylesTreeView
			// 
			this.m_seriesStylesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_seriesStylesTreeView.EditorService = null;
			this.m_seriesStylesTreeView.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_seriesStylesTreeView.HideSelection = false;
			this.m_seriesStylesTreeView.ImageIndex = -1;
			this.m_seriesStylesTreeView.Location = new System.Drawing.Point(2, 26);
			this.m_seriesStylesTreeView.Name = "m_seriesStylesTreeView";
			this.m_seriesStylesTreeView.SelectedImageIndex = -1;
			this.m_seriesStylesTreeView.Size = new System.Drawing.Size(156, 74);
			this.m_seriesStylesTreeView.TabIndex = 35;
			this.m_seriesStylesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_seriesStylesTreeView_AfterSelect);
			// 
			// m_dataPointLabelStyleGroupBox
			// 
			this.m_dataPointLabelStyleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																										this.m_DPLStyleNameComboBox});
			this.m_dataPointLabelStyleGroupBox.Location = new System.Drawing.Point(152, 197);
			this.m_dataPointLabelStyleGroupBox.Name = "m_dataPointLabelStyleGroupBox";
			this.m_dataPointLabelStyleGroupBox.ResizeChildren = false;
			this.m_dataPointLabelStyleGroupBox.Size = new System.Drawing.Size(160, 48);
			this.m_dataPointLabelStyleGroupBox.TabIndex = 50;
			this.m_dataPointLabelStyleGroupBox.TabStop = false;
			this.m_dataPointLabelStyleGroupBox.Text = "Data Point Label Style";
			// 
			// m_DPLStyleNameComboBox
			// 
			this.m_DPLStyleNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_DPLStyleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_DPLStyleNameComboBox.Location = new System.Drawing.Point(3, 24);
			this.m_DPLStyleNameComboBox.Name = "m_DPLStyleNameComboBox";
			this.m_DPLStyleNameComboBox.NoneString = "None";
			this.m_DPLStyleNameComboBox.Size = new System.Drawing.Size(156, 21);
			this.m_DPLStyleNameComboBox.TabIndex = 50;
			this.m_DPLStyleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.m_DPLStyleNameComboBox_SelectedIndexChanged);
			// 
			// m_seriesTreeGroupBox
			// 
			this.m_seriesTreeGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.seriesTreeView});
			this.m_seriesTreeGroupBox.DrawBorderAroundControl = true;
			this.m_seriesTreeGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_seriesTreeGroupBox.Name = "m_seriesTreeGroupBox";
			this.m_seriesTreeGroupBox.ResizeChildren = true;
			this.m_seriesTreeGroupBox.Size = new System.Drawing.Size(134, 233);
			this.m_seriesTreeGroupBox.TabIndex = 0;
			this.m_seriesTreeGroupBox.TabStop = false;
			this.m_seriesTreeGroupBox.Text = "Series Tree";
			// 
			// m_isRangeGroupBox
			// 
			this.m_isRangeGroupBox.BackColor = System.Drawing.Color.White;
			this.m_isRangeGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_isRangeCheckBox});
			this.m_isRangeGroupBox.Location = new System.Drawing.Point(152, 254);
			this.m_isRangeGroupBox.Name = "m_isRangeGroupBox";
			this.m_isRangeGroupBox.ResizeChildren = false;
			this.m_isRangeGroupBox.Size = new System.Drawing.Size(160, 19);
			this.m_isRangeGroupBox.TabIndex = 60;
			this.m_isRangeGroupBox.TabStop = false;
			this.m_isRangeGroupBox.Text = "Is Range";
			// 
			// m_compositeGroupBox
			// 
			this.m_compositeGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.m_compositeComboBox});
			this.m_compositeGroupBox.Location = new System.Drawing.Point(152, 136);
			this.m_compositeGroupBox.Name = "m_compositeGroupBox";
			this.m_compositeGroupBox.ResizeChildren = false;
			this.m_compositeGroupBox.Size = new System.Drawing.Size(160, 46);
			this.m_compositeGroupBox.TabIndex = 38;
			this.m_compositeGroupBox.TabStop = false;
			this.m_compositeGroupBox.Text = "Composition Kind";
			// 
			// m_compositeComboBox
			// 
			this.m_compositeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_compositeComboBox.Location = new System.Drawing.Point(4, 24);
			this.m_compositeComboBox.Name = "m_compositeComboBox";
			this.m_compositeComboBox.Size = new System.Drawing.Size(154, 21);
			this.m_compositeComboBox.TabIndex = 0;
			this.m_compositeComboBox.SelectedIndexChanged += new System.EventHandler(this.m_compositeComboBox_SelectedIndexChanged);
			// 
			// m_seriesTransparencyGroupBox
			// 
			this.m_seriesTransparencyGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									   this.m_seriesTransparencyValueLabel,
																									   this.m_seriesTransparencyTrackBar});
			this.m_seriesTransparencyGroupBox.Location = new System.Drawing.Point(152, 136);
			this.m_seriesTransparencyGroupBox.Name = "m_seriesTransparencyGroupBox";
			this.m_seriesTransparencyGroupBox.ResizeChildren = false;
			this.m_seriesTransparencyGroupBox.Size = new System.Drawing.Size(160, 56);
			this.m_seriesTransparencyGroupBox.TabIndex = 41;
			this.m_seriesTransparencyGroupBox.TabStop = false;
			this.m_seriesTransparencyGroupBox.Text = "Transparency";
			this.m_seriesTransparencyGroupBox.Visible = false;
			// 
			// m_seriesTransparencyValueLabel
			// 
			this.m_seriesTransparencyValueLabel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_seriesTransparencyValueLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_seriesTransparencyValueLabel.Location = new System.Drawing.Point(112, 6);
			this.m_seriesTransparencyValueLabel.Name = "m_seriesTransparencyValueLabel";
			this.m_seriesTransparencyValueLabel.Size = new System.Drawing.Size(32, 16);
			this.m_seriesTransparencyValueLabel.TabIndex = 4;
			this.m_seriesTransparencyValueLabel.Text = "0%";
			this.m_seriesTransparencyValueLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_seriesTransparencyTrackBar
			// 
			this.m_seriesTransparencyTrackBar.Location = new System.Drawing.Point(0, 24);
			this.m_seriesTransparencyTrackBar.Maximum = 100;
			this.m_seriesTransparencyTrackBar.Name = "m_seriesTransparencyTrackBar";
			this.m_seriesTransparencyTrackBar.Size = new System.Drawing.Size(152, 45);
			this.m_seriesTransparencyTrackBar.TabIndex = 40;
			this.m_seriesTransparencyTrackBar.TickFrequency = 10;
			this.m_seriesTransparencyTrackBar.Scroll += new System.EventHandler(this.m_seriesTransparencyTrackBar_Scroll);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.wizardDataBindingDialog1});
			this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.tabPage1.Location = new System.Drawing.Point(2, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(320, 285);
			this.tabPage1.TabIndex = 1;
			this.tabPage1.Text = "Data Source";
			// 
			// wizardDataBindingDialog1
			// 
			this.wizardDataBindingDialog1.BackColor = System.Drawing.Color.White;
			this.wizardDataBindingDialog1.DefaultHint = "Build the series structure and set properties for individual series. Select the r" +
				"oot node (or any other composite series node) to add a new series.";
			this.wizardDataBindingDialog1.DefaultHintTitle = "Series";
			this.wizardDataBindingDialog1.Name = "wizardDataBindingDialog1";
			this.wizardDataBindingDialog1.Size = new System.Drawing.Size(320, 280);
			this.wizardDataBindingDialog1.TabIndex = 0;
			// 
			// WizardPresentationDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_presentationTabControl});
			this.DefaultHint = "Build the series structure and set properties for individual series. Select the r" +
				"oot node (or any other composite series node) to add a new series.";
			this.DefaultHintTitle = "Series";
			this.Name = "WizardPresentationDialog";
			this.Size = new System.Drawing.Size(324, 312);
			this.Load += new System.EventHandler(this.WizardPresentationDialog_Load);
			this.m_presentationTabControl.ResumeLayout(false);
			this.m_mainPresentationTabPage.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.m_styleGroupBox.ResumeLayout(false);
			this.m_dataPointLabelStyleGroupBox.ResumeLayout(false);
			this.m_seriesTreeGroupBox.ResumeLayout(false);
			this.m_isRangeGroupBox.ResumeLayout(false);
			this.m_compositeGroupBox.ResumeLayout(false);
			this.m_seriesTransparencyGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_seriesTransparencyTrackBar)).EndInit();
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void Add_Click(object sender, System.EventArgs e)
		{
			seriesTreeView.Add(this.AddButton);
			WinChart.Invalidate();
		}

		private void Remove_Click(object sender, System.EventArgs e)
		{
			seriesTreeView.Remove();
			wizardDataBindingDialog1.SetupDataBindingPairControls();
			WinChart.Invalidate();
		}

		private void WizardPresentationDialog_Load(object sender, System.EventArgs e)
		{
		}

		SeriesBase SeriesBase 
		{
			get 
			{
				return seriesTreeView.SeriesBase;
			}
		}


		private void m_addToLegendCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Invalidate();
		}

		private void m_isRangeCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			((Series)SeriesBase).IsRange = m_isRangeCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_mainPresentationTabPage_VisibleChanged(object sender, System.EventArgs e)
		{
			if (m_presentationTabControl.SelectedTab == m_mainPresentationTabPage)
				seriesTreeView.Focus();
		}

		private void m_seriesStylesTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (seriesTreeView.SeriesBase != null && m_seriesStylesTreeView.SelectedStyle != null && !m_updatingStyleTree) 
			{
				seriesTreeView.SeriesBase.StyleName = m_seriesStylesTreeView.SelectedStyle.Name;
				if (seriesTreeView.SeriesBase == WinChart.RootSeries)
					Wizard.CoordSysEnabled = !(seriesTreeView.SeriesBase.Style.ChartKind == ChartKind.Pie || seriesTreeView.SeriesBase.Style.ChartKind == ChartKind.Doughnut || seriesTreeView.SeriesBase.Style.IsRadar);

				if (SeriesBase is CompositeSeries) 
				{
					BuildCompositeKindComboBox();
					m_compositeComboBox.Text = ((CompositeSeries)SeriesBase).CompositionKind.ToString();
				}

				WinChart.Invalidate();
			}
		}

		private void m_nameSetButton_Click(object sender, System.EventArgs e)
		{
			this.seriesTreeView.SelectedNode.Text = SeriesBase.Name;
		}
		
		private void seriesTreeView_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
            if (!e.CancelEdit)
            {
                WinChart.Invalidate();
                wizardDataBindingDialog1.SetupDataBindingPairControls();
            }
		}

		private void m_compositeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			((CompositeSeries)SeriesBase).CompositionKind = (CompositionKind)Enum.Parse(typeof(CompositionKind), m_compositeComboBox.Text);
			WinChart.Invalidate();
		}

		private void m_DPLStyleNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SeriesBase s = SeriesBase;
			if (m_DPLStyleNameComboBox.SelectedIndex == 0)
				s.Labels.Clear();
			else 
			{
				if (s.Labels.Count == 0)
					s.Labels.Add(new SeriesLabels());
				s.Labels[0].LabelStyleName = m_DPLStyleNameComboBox.Text;
			}

			WinChart.Invalidate();
		}

		private void m_seriesTransparencyTrackBar_Scroll(object sender, System.EventArgs e)
		{
			Series s = ((Series)SeriesBase);
			s.Transparency = m_seriesTransparencyTrackBar.Value;
			m_seriesTransparencyValueLabel.Text = s.Transparency.ToString() + "%";
			m_seriesTransparencyValueLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_independentYCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			SeriesBase.HasIndependentYAxis = m_independentYCheckBox.Checked;
			WinChart.Invalidate();
		}
	}
}
