using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
#if __BUILDING_CRI_DESIGNER__
using Microsoft.ReportDesigner.Design;
#endif

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardTitlesDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.TabControl tabControl1;
		private ComponentArt.Win.UI.Internal.ComboBox m_refPointComboBox;
		private ComponentArt.Win.UI.Internal.ComboBox m_positionComboBox;
		private System.Windows.Forms.NumericUpDown m_rightTextMarginUpDown;
		private System.Windows.Forms.NumericUpDown m_leftTextMarginUpDown;
		private System.Windows.Forms.NumericUpDown m_bottomTextMarginUpDown;
		private System.Windows.Forms.NumericUpDown m_topTextMarginUpDown;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControlWithDots m_titleOutlineColor;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.NumericUpDown m_outlineWidthUpDown;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown m_shadeWidthUpDown;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControlWithDots m_titleForeColor;
		private System.Windows.Forms.Label m_fontLabel;
		private ComponentArt.Win.UI.Internal.Button m_fontButton;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControlWithDots m_titleShadeColor;
		private ComponentArt.Web.Visualization.Charting.Design.WizardLegendDialog wizardLegendDialog1;
		private ComponentArt.Win.UI.Internal.TabPage m_titlesTabPage;
		private ComponentArt.Win.UI.Internal.TabPage m_legendTabPage;
		private ComponentArt.Win.UI.Internal.TabPage m_marginsTabPage;
		private ComponentArt.Win.UI.Internal.BorderPanel m_titleOptionsPanel;
		private System.Windows.Forms.TrackBar m_rightMarginTrackBar;
		private System.Windows.Forms.TrackBar m_bottomMarginTrackBar;
		private System.Windows.Forms.TrackBar m_leftMarginTrackBar;
		private System.Windows.Forms.Panel panel1;
		private ComponentArt.Win.UI.Internal.Button m_moreSettingsButton;
		private System.Windows.Forms.Label m_bottomMarginLabel;
		private System.Windows.Forms.Label m_rightMarginLabel;
		private System.Windows.Forms.Label m_leftMarginLabel;
		private ComponentArt.Win.UI.Internal.Separator separator3;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		private ComponentArt.Win.UI.Internal.Separator separator4;
		private ComponentArt.Win.UI.Internal.Separator separator5;
		private ComponentArt.Win.UI.Internal.ListBox listBox;
		private ComponentArt.Win.UI.Internal.Button m_removeButton;
		private ComponentArt.Win.UI.Internal.Button m_addButton;
		[WizardHint(typeof(ChartTitle), "Font")]
		private ComponentArt.Win.UI.Internal.GroupBox m_fontGroupBox;
		private ComponentArt.Win.UI.Internal.GroupBox m_titleCollectionGroupBox;
		[WizardHint(typeof(MappingMargins), "Bottom")]
		private ComponentArt.Win.UI.Internal.GroupBox m_bottomMarginGroupBox;
		[WizardHint(typeof(MappingMargins), "Left")]
		private ComponentArt.Win.UI.Internal.GroupBox m_leftMarginGroupBox;
		[WizardHint(typeof(MappingMargins), "Right")]
		private ComponentArt.Win.UI.Internal.GroupBox m_rightMarginGroupBox;
		[WizardHint(typeof(ChartTitle), "Text")]
		private ComponentArt.Win.UI.Internal.GroupBox m_textGroupBox;
		[WizardHint(typeof(ChartTitle), "Position")]
		private ComponentArt.Win.UI.Internal.GroupBox m_positionGroupBox;
		[WizardHint(typeof(ChartTitle), "RefPoint")]
		private ComponentArt.Win.UI.Internal.GroupBox m_refPointGroupBox;
		private ComponentArt.Win.UI.Internal.GroupBox m_textMarginsGroupBox;
		[WizardHint(typeof(ChartTitle), "TextTopMargin")]
		private ComponentArt.Win.UI.Internal.GroupBox m_topTextMarginGroupBox;
		[WizardHint(typeof(ChartTitle), "TextBottomMargin")]
		private ComponentArt.Win.UI.Internal.GroupBox m_bottomTextMarginGroupBox;
		[WizardHint(typeof(ChartTitle), "TextLeftMargin")]
		private ComponentArt.Win.UI.Internal.GroupBox m_leftTextMarginGroupBox;
		[WizardHint(typeof(ChartTitle), "TextRightMargin")]
		private ComponentArt.Win.UI.Internal.GroupBox m_rightTextMarginGroupBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

#if !__BUILDING_CRI_DESIGNER__
        private System.Windows.Forms.TextBox m_titleText;
        private ComponentArt.Win.UI.Internal.BorderPanel borderPanel1;
#else
        private ComponentArt.Win.UI.Internal.ComboBox m_titleComboBox;
#endif

		private ComponentArt.Win.UI.Internal.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox m_autoResizeMarginsCheckBox;
		[WizardHint(typeof(MappingMargins), "Top")]
		private ComponentArt.Win.UI.Internal.GroupBox m_topMarginGroupBox;
		private System.Windows.Forms.Label m_topMarginLabel;
		private System.Windows.Forms.TrackBar m_topMarginTrackBar;
		private ComponentArt.Win.UI.Internal.Separator separator6;

        public WizardTitlesDialog()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			listBox.SelectedIndexChanged += new EventHandler(SelectedIndexChangedHandler);

			m_positionComboBox.Items.AddRange(Enum.GetNames(typeof(ComponentArt.Web.Visualization.Charting.TitlePosition)));
			m_refPointComboBox.Items.AddRange(Enum.GetNames(typeof(ComponentArt.Web.Visualization.Charting.TextReferencePoint)));


            Control textControl;
#if !__BUILDING_CRI_DESIGNER__
            this.borderPanel1 = new ComponentArt.Win.UI.Internal.BorderPanel();
			this.borderPanel1.SuspendLayout();
            			this.m_textGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.borderPanel1});
			// 
			// borderPanel1
			// 
			this.borderPanel1.Location = new System.Drawing.Point(42, 7);
			this.borderPanel1.Name = "borderPanel1";
			this.borderPanel1.Size = new System.Drawing.Size(106, 15);
			this.borderPanel1.TabIndex = 1;
			this.borderPanel1.ResumeLayout(false);


            this.m_titleText = new System.Windows.Forms.TextBox();
            textControl = this.m_titleText;
            this.m_titleText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_titleText.TextChanged += new System.EventHandler(this.textControl_TextChanged);
            this.m_titleText.Location = new System.Drawing.Point(1, 1);
            textControl.Size = new System.Drawing.Size(104, 13);
            this.borderPanel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   textControl});
#else
            this.m_textGroupBox.DrawBorderAroundControl = false;
            this.m_titleComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
            textControl = this.m_titleComboBox;
            this.m_titleComboBox.SelectedIndexChanged += new EventHandler(m_titleComboBox_SelectedIndexChanged);
            this.m_titleComboBox.SelectionChangeCommitted += new EventHandler(m_titleComboBox_SelectionChangeCommitted);
            this.m_titleComboBox.Location = new System.Drawing.Point(40, 1);
            this.m_titleComboBox.Size = new System.Drawing.Size(112, 21);
            this.m_titleComboBox.TextChanged += new EventHandler(textControl_TextChanged);
            this.m_textGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   textControl});
#endif
            textControl.Name = "m_titleText";
            textControl.TabIndex = 0;
            textControl.Text = "";
		}

#if __BUILDING_CRI_DESIGNER__
        private bool m_launchEditor = false;
        private String m_oldComboValue;

        void m_titleComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            m_oldComboValue = combo.Text;
            m_launchEditor = true;
        }
        
        void m_titleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_titleComboBox.SelectedIndex == 0 && m_launchEditor)
            {
                m_launchEditor = false;
                ExpressionEditor editor = new ExpressionEditor();
                string newValue;
                SqlChartDesigner rschartDesigner = ((SqlChartDesigner)WinChart.Chart.Owner);
                newValue = (string)editor.EditValue(null, rschartDesigner.Site, m_oldComboValue);
                m_titleComboBox.Items[0] = newValue;
                SetChartTitleText(newValue);
            }
            else
            {
                SetChartTitleText(m_titleComboBox.Text);
            }
        }
#endif


		ChartTitle ChartTitle 
		{
			get 
			{
				return listBox.SelectedIndex == -1 ? null : WinChart.Titles[listBox.SelectedIndex];
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
			this.m_titlesTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.m_removeButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_addButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_titleCollectionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.listBox = new ComponentArt.Win.UI.Internal.ListBox();
			this.m_titleOptionsPanel = new ComponentArt.Win.UI.Internal.BorderPanel();
			this.separator5 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator4 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_textMarginsGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_topTextMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_topTextMarginUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_bottomTextMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_bottomTextMarginUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_leftTextMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_leftTextMarginUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_rightTextMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_rightTextMarginUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_fontGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.m_shadeWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_outlineWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_titleOutlineColor = new ComponentArt.Web.Visualization.Charting.Design.ColorControlWithDots();
			this.m_titleShadeColor = new ComponentArt.Web.Visualization.Charting.Design.ColorControlWithDots();
			this.label14 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.m_titleForeColor = new ComponentArt.Web.Visualization.Charting.Design.ColorControlWithDots();
			this.m_fontLabel = new System.Windows.Forms.Label();
			this.m_fontButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_textGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_positionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_positionComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_refPointGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_refPointComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_moreSettingsButton = new ComponentArt.Win.UI.Internal.Button();
			this.tabControl1 = new ComponentArt.Win.UI.Internal.TabControl();
			this.m_legendTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.wizardLegendDialog1 = new ComponentArt.Web.Visualization.Charting.Design.WizardLegendDialog();
			this.m_marginsTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.separator6 = new ComponentArt.Win.UI.Internal.Separator();
			this.groupBox1 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_autoResizeMarginsCheckBox = new System.Windows.Forms.CheckBox();
			this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator3 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_topMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_topMarginLabel = new System.Windows.Forms.Label();
			this.m_topMarginTrackBar = new System.Windows.Forms.TrackBar();
			this.m_bottomMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_bottomMarginLabel = new System.Windows.Forms.Label();
			this.m_bottomMarginTrackBar = new System.Windows.Forms.TrackBar();
			this.m_leftMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_leftMarginLabel = new System.Windows.Forms.Label();
			this.m_leftMarginTrackBar = new System.Windows.Forms.TrackBar();
			this.m_rightMarginGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_rightMarginLabel = new System.Windows.Forms.Label();
			this.m_rightMarginTrackBar = new System.Windows.Forms.TrackBar();
			this.m_titlesTabPage.SuspendLayout();
			this.m_titleCollectionGroupBox.SuspendLayout();
			this.m_titleOptionsPanel.SuspendLayout();
			this.m_textMarginsGroupBox.SuspendLayout();
			this.m_topTextMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_topTextMarginUpDown)).BeginInit();
			this.m_bottomTextMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_bottomTextMarginUpDown)).BeginInit();
			this.m_leftTextMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_leftTextMarginUpDown)).BeginInit();
			this.m_rightTextMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_rightTextMarginUpDown)).BeginInit();
			this.m_fontGroupBox.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_shadeWidthUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_outlineWidthUpDown)).BeginInit();
			this.m_positionGroupBox.SuspendLayout();
			this.m_refPointGroupBox.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.m_legendTabPage.SuspendLayout();
			this.m_marginsTabPage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.m_topMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_topMarginTrackBar)).BeginInit();
			this.m_bottomMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_bottomMarginTrackBar)).BeginInit();
			this.m_leftMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_leftMarginTrackBar)).BeginInit();
			this.m_rightMarginGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_rightMarginTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// m_titlesTabPage
			// 
			this.m_titlesTabPage.BackColor = System.Drawing.Color.White;
			this.m_titlesTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this.m_removeButton,
																						  this.m_addButton,
																						  this.m_titleCollectionGroupBox,
																						  this.m_titleOptionsPanel,
																						  this.m_moreSettingsButton});
			this.m_titlesTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_titlesTabPage.Name = "m_titlesTabPage";
			this.m_titlesTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_titlesTabPage.TabIndex = 2;
			this.m_titlesTabPage.Text = "Titles";
			// 
			// m_removeButton
			// 
			this.m_removeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_removeButton.Location = new System.Drawing.Point(72, 256);
			this.m_removeButton.Name = "m_removeButton";
			this.m_removeButton.Size = new System.Drawing.Size(59, 21);
			this.m_removeButton.TabIndex = 20;
			this.m_removeButton.Text = "Remove";
			this.m_removeButton.Click += new System.EventHandler(this.m_removeButton_Click);
			// 
			// m_addButton
			// 
			this.m_addButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_addButton.Location = new System.Drawing.Point(8, 256);
			this.m_addButton.Name = "m_addButton";
			this.m_addButton.Size = new System.Drawing.Size(59, 21);
			this.m_addButton.TabIndex = 10;
			this.m_addButton.Text = "Add";
			this.m_addButton.Click += new System.EventHandler(this.m_addButton_Click);
			// 
			// m_titleCollectionGroupBox
			// 
			this.m_titleCollectionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.listBox});
			this.m_titleCollectionGroupBox.DrawBorderAroundControl = true;
			this.m_titleCollectionGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_titleCollectionGroupBox.Name = "m_titleCollectionGroupBox";
			this.m_titleCollectionGroupBox.ResizeChildren = false;
			this.m_titleCollectionGroupBox.Size = new System.Drawing.Size(120, 240);
			this.m_titleCollectionGroupBox.TabIndex = 0;
			this.m_titleCollectionGroupBox.TabStop = false;
			this.m_titleCollectionGroupBox.Text = "All Titles";
			// 
			// listBox
			// 
			this.listBox.BackColor = System.Drawing.Color.White;
			this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox.DisplayMember = "Name";
			this.listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.listBox.ItemHeight = 18;
			this.listBox.Location = new System.Drawing.Point(2, 24);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(116, 198);
			this.listBox.TabIndex = 35;
			// 
			// m_titleOptionsPanel
			// 
			this.m_titleOptionsPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.separator5,
																							  this.separator4,
																							  this.m_textMarginsGroupBox,
																							  this.m_fontGroupBox,
																							  this.m_textGroupBox,
																							  this.m_positionGroupBox,
																							  this.m_refPointGroupBox});
			this.m_titleOptionsPanel.Enabled = false;
			this.m_titleOptionsPanel.Location = new System.Drawing.Point(128, 27);
			this.m_titleOptionsPanel.Name = "m_titleOptionsPanel";
			this.m_titleOptionsPanel.Size = new System.Drawing.Size(192, 225);
			this.m_titleOptionsPanel.TabIndex = 30;
			// 
			// separator5
			// 
			this.separator5.Location = new System.Drawing.Point(16, 146);
			this.separator5.Name = "separator5";
			this.separator5.Size = new System.Drawing.Size(168, 3);
			this.separator5.TabIndex = 50;
			this.separator5.TabStop = false;
			// 
			// separator4
			// 
			this.separator4.Location = new System.Drawing.Point(8, 82);
			this.separator4.Name = "separator4";
			this.separator4.Size = new System.Drawing.Size(176, 3);
			this.separator4.TabIndex = 30;
			this.separator4.TabStop = false;
			// 
			// m_textMarginsGroupBox
			// 
			this.m_textMarginsGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.m_topTextMarginGroupBox,
																								this.m_bottomTextMarginGroupBox,
																								this.m_leftTextMarginGroupBox,
																								this.m_rightTextMarginGroupBox});
			this.m_textMarginsGroupBox.Location = new System.Drawing.Point(8, 150);
			this.m_textMarginsGroupBox.Name = "m_textMarginsGroupBox";
			this.m_textMarginsGroupBox.ResizeChildren = false;
			this.m_textMarginsGroupBox.Size = new System.Drawing.Size(182, 78);
			this.m_textMarginsGroupBox.TabIndex = 50;
			this.m_textMarginsGroupBox.TabStop = false;
			this.m_textMarginsGroupBox.Text = "Text Margins";
			// 
			// m_topTextMarginGroupBox
			// 
			this.m_topTextMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								  this.m_topTextMarginUpDown});
			this.m_topTextMarginGroupBox.Location = new System.Drawing.Point(14, 25);
			this.m_topTextMarginGroupBox.Name = "m_topTextMarginGroupBox";
			this.m_topTextMarginGroupBox.ResizeChildren = false;
			this.m_topTextMarginGroupBox.Size = new System.Drawing.Size(75, 24);
			this.m_topTextMarginGroupBox.TabIndex = 50;
			this.m_topTextMarginGroupBox.TabStop = false;
			this.m_topTextMarginGroupBox.Text = "Top:";
			// 
			// m_topTextMarginUpDown
			// 
			this.m_topTextMarginUpDown.BackColor = System.Drawing.Color.White;
			this.m_topTextMarginUpDown.Location = new System.Drawing.Point(34, 4);
			this.m_topTextMarginUpDown.Name = "m_topTextMarginUpDown";
			this.m_topTextMarginUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_topTextMarginUpDown.TabIndex = 50;
			this.m_topTextMarginUpDown.ValueChanged += new System.EventHandler(this.m_topTextMarginUpDown_ValueChanged);
			// 
			// m_bottomTextMarginGroupBox
			// 
			this.m_bottomTextMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									 this.m_bottomTextMarginUpDown});
			this.m_bottomTextMarginGroupBox.Location = new System.Drawing.Point(-5, 49);
			this.m_bottomTextMarginGroupBox.Name = "m_bottomTextMarginGroupBox";
			this.m_bottomTextMarginGroupBox.ResizeChildren = false;
			this.m_bottomTextMarginGroupBox.Size = new System.Drawing.Size(96, 24);
			this.m_bottomTextMarginGroupBox.TabIndex = 70;
			this.m_bottomTextMarginGroupBox.TabStop = false;
			this.m_bottomTextMarginGroupBox.Text = "Bottom:";
			// 
			// m_bottomTextMarginUpDown
			// 
			this.m_bottomTextMarginUpDown.BackColor = System.Drawing.Color.White;
			this.m_bottomTextMarginUpDown.Location = new System.Drawing.Point(53, 4);
			this.m_bottomTextMarginUpDown.Name = "m_bottomTextMarginUpDown";
			this.m_bottomTextMarginUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_bottomTextMarginUpDown.TabIndex = 70;
			this.m_bottomTextMarginUpDown.ValueChanged += new System.EventHandler(this.m_bottomTextMarginUpDown_ValueChanged);
			// 
			// m_leftTextMarginGroupBox
			// 
			this.m_leftTextMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.m_leftTextMarginUpDown});
			this.m_leftTextMarginGroupBox.Location = new System.Drawing.Point(102, 25);
			this.m_leftTextMarginGroupBox.Name = "m_leftTextMarginGroupBox";
			this.m_leftTextMarginGroupBox.ResizeChildren = false;
			this.m_leftTextMarginGroupBox.Size = new System.Drawing.Size(77, 24);
			this.m_leftTextMarginGroupBox.TabIndex = 60;
			this.m_leftTextMarginGroupBox.TabStop = false;
			this.m_leftTextMarginGroupBox.Text = "Left:";
			// 
			// m_leftTextMarginUpDown
			// 
			this.m_leftTextMarginUpDown.BackColor = System.Drawing.Color.White;
			this.m_leftTextMarginUpDown.Location = new System.Drawing.Point(34, 4);
			this.m_leftTextMarginUpDown.Name = "m_leftTextMarginUpDown";
			this.m_leftTextMarginUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_leftTextMarginUpDown.TabIndex = 60;
			this.m_leftTextMarginUpDown.ValueChanged += new System.EventHandler(this.m_leftTextMarginUpDown_ValueChanged);
			// 
			// m_rightTextMarginGroupBox
			// 
			this.m_rightTextMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.m_rightTextMarginUpDown});
			this.m_rightTextMarginGroupBox.Location = new System.Drawing.Point(94, 49);
			this.m_rightTextMarginGroupBox.Name = "m_rightTextMarginGroupBox";
			this.m_rightTextMarginGroupBox.ResizeChildren = false;
			this.m_rightTextMarginGroupBox.Size = new System.Drawing.Size(84, 24);
			this.m_rightTextMarginGroupBox.TabIndex = 80;
			this.m_rightTextMarginGroupBox.TabStop = false;
			this.m_rightTextMarginGroupBox.Text = "Right:";
			// 
			// m_rightTextMarginUpDown
			// 
			this.m_rightTextMarginUpDown.BackColor = System.Drawing.Color.White;
			this.m_rightTextMarginUpDown.Location = new System.Drawing.Point(42, 4);
			this.m_rightTextMarginUpDown.Name = "m_rightTextMarginUpDown";
			this.m_rightTextMarginUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_rightTextMarginUpDown.TabIndex = 80;
			this.m_rightTextMarginUpDown.ValueChanged += new System.EventHandler(this.m_rightTextMarginUpDown_ValueChanged);
			// 
			// m_fontGroupBox
			// 
			this.m_fontGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.panel1,
																						 this.m_titleForeColor,
																						 this.m_fontLabel,
																						 this.m_fontButton});
			this.m_fontGroupBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_fontGroupBox.Location = new System.Drawing.Point(8, 86);
			this.m_fontGroupBox.Name = "m_fontGroupBox";
			this.m_fontGroupBox.ResizeChildren = false;
			this.m_fontGroupBox.Size = new System.Drawing.Size(176, 56);
			this.m_fontGroupBox.TabIndex = 45;
			this.m_fontGroupBox.TabStop = false;
			this.m_fontGroupBox.Text = "Font";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.m_shadeWidthUpDown,
																				 this.m_outlineWidthUpDown,
																				 this.m_titleOutlineColor,
																				 this.m_titleShadeColor,
																				 this.label14,
																				 this.label9});
			this.panel1.Location = new System.Drawing.Point(144, 48);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(152, 56);
			this.panel1.TabIndex = 60;
			this.panel1.Visible = false;
			// 
			// m_shadeWidthUpDown
			// 
			this.m_shadeWidthUpDown.Location = new System.Drawing.Point(112, 32);
			this.m_shadeWidthUpDown.Name = "m_shadeWidthUpDown";
			this.m_shadeWidthUpDown.Size = new System.Drawing.Size(40, 21);
			this.m_shadeWidthUpDown.TabIndex = 58;
			this.m_shadeWidthUpDown.ValueChanged += new System.EventHandler(this.m_shadeWidthUpDown_ValueChanged);
			// 
			// m_outlineWidthUpDown
			// 
			this.m_outlineWidthUpDown.Location = new System.Drawing.Point(112, 8);
			this.m_outlineWidthUpDown.Name = "m_outlineWidthUpDown";
			this.m_outlineWidthUpDown.Size = new System.Drawing.Size(40, 21);
			this.m_outlineWidthUpDown.TabIndex = 53;
			this.m_outlineWidthUpDown.ValueChanged += new System.EventHandler(this.m_outlineWidthUpDown_ValueChanged);
			// 
			// m_titleOutlineColor
			// 
			this.m_titleOutlineColor.BackColor = System.Drawing.Color.White;
			this.m_titleOutlineColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_titleOutlineColor.Location = new System.Drawing.Point(80, 8);
			this.m_titleOutlineColor.Name = "m_titleOutlineColor";
			this.m_titleOutlineColor.Size = new System.Drawing.Size(24, 11);
			this.m_titleOutlineColor.TabIndex = 56;
			this.m_titleOutlineColor.ColorChanged += new System.EventHandler(this.m_titleOutlineColor_ColorChanged);
			// 
			// m_titleShadeColor
			// 
			this.m_titleShadeColor.BackColor = System.Drawing.Color.White;
			this.m_titleShadeColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_titleShadeColor.Location = new System.Drawing.Point(80, 32);
			this.m_titleShadeColor.Name = "m_titleShadeColor";
			this.m_titleShadeColor.Size = new System.Drawing.Size(24, 11);
			this.m_titleShadeColor.TabIndex = 59;
			this.m_titleShadeColor.ColorChanged += new System.EventHandler(this.m_titleShadeColor_ColorChanged);
			// 
			// label14
			// 
			this.label14.BackColor = System.Drawing.Color.White;
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(72, 24);
			this.label14.TabIndex = 54;
			this.label14.Text = "Outline Color and Width:";
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.White;
			this.label9.Location = new System.Drawing.Point(0, 24);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 24);
			this.label9.TabIndex = 57;
			this.label9.Text = "Shade Color and Width:";
			// 
			// m_titleForeColor
			// 
			this.m_titleForeColor.BackColor = System.Drawing.Color.White;
			this.m_titleForeColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_titleForeColor.Location = new System.Drawing.Point(128, 29);
			this.m_titleForeColor.Name = "m_titleForeColor";
			this.m_titleForeColor.TabIndex = 40;
			this.m_titleForeColor.ColorChanged += new System.EventHandler(this.m_titleForeColor_ColorChanged);
			// 
			// m_fontLabel
			// 
			this.m_fontLabel.BackColor = System.Drawing.Color.White;
			this.m_fontLabel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_fontLabel.ForeColor = System.Drawing.Color.Black;
			this.m_fontLabel.Location = new System.Drawing.Point(2, 28);
			this.m_fontLabel.Name = "m_fontLabel";
			this.m_fontLabel.Size = new System.Drawing.Size(98, 16);
			this.m_fontLabel.TabIndex = 45;
			// 
			// m_fontButton
			// 
			this.m_fontButton.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_fontButton.Location = new System.Drawing.Point(104, 28);
			this.m_fontButton.Name = "m_fontButton";
			this.m_fontButton.Size = new System.Drawing.Size(20, 14);
			this.m_fontButton.TabIndex = 30;
			this.m_fontButton.Text = "...";
			this.m_fontButton.TextLocation = new System.Drawing.Point(5, 2);
			this.m_fontButton.Click += new System.EventHandler(this.m_fontButton_Click);
			// 
			// m_textGroupBox
			// 
			this.m_textGroupBox.DrawBorderAroundControl = true;
			this.m_textGroupBox.Location = new System.Drawing.Point(32, 0);
			this.m_textGroupBox.Name = "m_textGroupBox";
			this.m_textGroupBox.ResizeChildren = false;
			this.m_textGroupBox.Size = new System.Drawing.Size(152, 24);
			this.m_textGroupBox.TabIndex = 0;
			this.m_textGroupBox.TabStop = false;
			this.m_textGroupBox.Text = "Text:";
			// 
			// m_positionGroupBox
			// 
			this.m_positionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							 this.m_positionComboBox});
			this.m_positionGroupBox.Location = new System.Drawing.Point(13, 27);
			this.m_positionGroupBox.Name = "m_positionGroupBox";
			this.m_positionGroupBox.ResizeChildren = false;
			this.m_positionGroupBox.Size = new System.Drawing.Size(174, 24);
			this.m_positionGroupBox.TabIndex = 10;
			this.m_positionGroupBox.TabStop = false;
			this.m_positionGroupBox.Text = "Position:";
			// 
			// m_positionComboBox
			// 
			this.m_positionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_positionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_positionComboBox.Location = new System.Drawing.Point(59, 1);
			this.m_positionComboBox.Name = "m_positionComboBox";
			this.m_positionComboBox.Size = new System.Drawing.Size(112, 21);
			this.m_positionComboBox.TabIndex = 10;
			this.m_positionComboBox.SelectedIndexChanged += new System.EventHandler(this.m_positionComboBox_SelectedIndexChanged);
			// 
			// m_refPointGroupBox
			// 
			this.m_refPointGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							 this.m_refPointComboBox});
			this.m_refPointGroupBox.Location = new System.Drawing.Point(9, 53);
			this.m_refPointGroupBox.Name = "m_refPointGroupBox";
			this.m_refPointGroupBox.ResizeChildren = false;
			this.m_refPointGroupBox.Size = new System.Drawing.Size(177, 24);
			this.m_refPointGroupBox.TabIndex = 20;
			this.m_refPointGroupBox.TabStop = false;
			this.m_refPointGroupBox.Text = "Ref Point:";
			// 
			// m_refPointComboBox
			// 
			this.m_refPointComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_refPointComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_refPointComboBox.Location = new System.Drawing.Point(63, 1);
			this.m_refPointComboBox.Name = "m_refPointComboBox";
			this.m_refPointComboBox.Size = new System.Drawing.Size(112, 21);
			this.m_refPointComboBox.TabIndex = 20;
			this.m_refPointComboBox.SelectedIndexChanged += new System.EventHandler(this.m_refPointComboBox_SelectedIndexChanged);
			// 
			// m_moreSettingsButton
			// 
			this.m_moreSettingsButton.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_moreSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.m_moreSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_moreSettingsButton.Location = new System.Drawing.Point(184, 256);
			this.m_moreSettingsButton.Name = "m_moreSettingsButton";
			this.m_moreSettingsButton.Size = new System.Drawing.Size(96, 23);
			this.m_moreSettingsButton.TabIndex = 40;
			this.m_moreSettingsButton.Text = "More settings...";
			this.m_moreSettingsButton.Click += new System.EventHandler(this.m_moreSettingsButton_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.m_legendTabPage,
																					  this.m_titlesTabPage,
																					  this.m_marginsTabPage});
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Size = new System.Drawing.Size(324, 312);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// m_legendTabPage
			// 
			this.m_legendTabPage.BackColor = System.Drawing.Color.White;
			this.m_legendTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this.wizardLegendDialog1});
			this.m_legendTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_legendTabPage.Name = "m_legendTabPage";
			this.m_legendTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_legendTabPage.TabIndex = 3;
			this.m_legendTabPage.Text = "Legend";
			// 
			// wizardLegendDialog1
			// 
			this.wizardLegendDialog1.BackColor = System.Drawing.Color.White;
			this.wizardLegendDialog1.DefaultHint = "Set legend visibility, font, position, layout, background, and item border settin" +
				"gs. ";
			this.wizardLegendDialog1.DefaultHintTitle = "Legend";
			this.wizardLegendDialog1.Name = "wizardLegendDialog1";
			this.wizardLegendDialog1.Size = new System.Drawing.Size(324, 296);
			this.wizardLegendDialog1.TabIndex = 0;
			// 
			// m_marginsTabPage
			// 
			this.m_marginsTabPage.BackColor = System.Drawing.Color.White;
			this.m_marginsTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.separator6,
																						   this.groupBox1,
																						   this.separator2,
																						   this.separator1,
																						   this.separator3,
																						   this.m_topMarginGroupBox,
																						   this.m_bottomMarginGroupBox,
																						   this.m_leftMarginGroupBox,
																						   this.m_rightMarginGroupBox});
			this.m_marginsTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_marginsTabPage.Name = "m_marginsTabPage";
			this.m_marginsTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_marginsTabPage.TabIndex = 5;
			this.m_marginsTabPage.Text = "Margins";
			// 
			// separator6
			// 
			this.separator6.BackColor = System.Drawing.Color.White;
			this.separator6.Location = new System.Drawing.Point(8, 40);
			this.separator6.Name = "separator6";
			this.separator6.Size = new System.Drawing.Size(304, 3);
			this.separator6.TabIndex = 63;
			this.separator6.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.White;
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_autoResizeMarginsCheckBox});
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.ResizeChildren = false;
			this.groupBox1.Size = new System.Drawing.Size(248, 19);
			this.groupBox1.TabIndex = 62;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Auto-resize margins to fit labels";
			// 
			// m_autoResizeMarginsCheckBox
			// 
			this.m_autoResizeMarginsCheckBox.BackColor = System.Drawing.Color.White;
			this.m_autoResizeMarginsCheckBox.Location = new System.Drawing.Point(224, 5);
			this.m_autoResizeMarginsCheckBox.Name = "m_autoResizeMarginsCheckBox";
			this.m_autoResizeMarginsCheckBox.Size = new System.Drawing.Size(16, 16);
			this.m_autoResizeMarginsCheckBox.TabIndex = 3;
			this.m_autoResizeMarginsCheckBox.CheckedChanged += new System.EventHandler(this.m_autoResizeMarginsCheckBox_CheckedChanged);
			// 
			// separator2
			// 
			this.separator2.Location = new System.Drawing.Point(8, 200);
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(304, 3);
			this.separator2.TabIndex = 52;
			this.separator2.TabStop = false;
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(8, 144);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(304, 3);
			this.separator1.TabIndex = 51;
			this.separator1.TabStop = false;
			// 
			// separator3
			// 
			this.separator3.Location = new System.Drawing.Point(8, 88);
			this.separator3.Name = "separator3";
			this.separator3.Size = new System.Drawing.Size(304, 3);
			this.separator3.TabIndex = 50;
			this.separator3.TabStop = false;
			// 
			// m_topMarginGroupBox
			// 
			this.m_topMarginGroupBox.BackColor = System.Drawing.Color.White;
			this.m_topMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.m_topMarginLabel,
																							  this.m_topMarginTrackBar});
			this.m_topMarginGroupBox.Location = new System.Drawing.Point(36, 48);
			this.m_topMarginGroupBox.Name = "m_topMarginGroupBox";
			this.m_topMarginGroupBox.ResizeChildren = false;
			this.m_topMarginGroupBox.Size = new System.Drawing.Size(280, 36);
			this.m_topMarginGroupBox.TabIndex = 0;
			this.m_topMarginGroupBox.TabStop = false;
			this.m_topMarginGroupBox.Text = "Top Margin:";
			// 
			// m_topMarginLabel
			// 
			this.m_topMarginLabel.BackColor = System.Drawing.Color.White;
			this.m_topMarginLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_topMarginLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(170)), ((System.Byte)(170)));
			this.m_topMarginLabel.Location = new System.Drawing.Point(217, 8);
			this.m_topMarginLabel.Name = "m_topMarginLabel";
			this.m_topMarginLabel.Size = new System.Drawing.Size(40, 16);
			this.m_topMarginLabel.TabIndex = 34;
			this.m_topMarginLabel.Text = "0";
			// 
			// m_topMarginTrackBar
			// 
			this.m_topMarginTrackBar.AutoSize = false;
			this.m_topMarginTrackBar.BackColor = System.Drawing.Color.White;
			this.m_topMarginTrackBar.Location = new System.Drawing.Point(71, 0);
			this.m_topMarginTrackBar.Maximum = 100;
			this.m_topMarginTrackBar.Name = "m_topMarginTrackBar";
			this.m_topMarginTrackBar.Size = new System.Drawing.Size(144, 30);
			this.m_topMarginTrackBar.TabIndex = 33;
			this.m_topMarginTrackBar.TickFrequency = 10;
			this.m_topMarginTrackBar.Scroll += new System.EventHandler(this.m_topMarginTrackBar_Scroll);
			// 
			// m_bottomMarginGroupBox
			// 
			this.m_bottomMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								 this.m_bottomMarginLabel,
																								 this.m_bottomMarginTrackBar});
			this.m_bottomMarginGroupBox.Location = new System.Drawing.Point(16, 104);
			this.m_bottomMarginGroupBox.Name = "m_bottomMarginGroupBox";
			this.m_bottomMarginGroupBox.ResizeChildren = false;
			this.m_bottomMarginGroupBox.Size = new System.Drawing.Size(296, 36);
			this.m_bottomMarginGroupBox.TabIndex = 10;
			this.m_bottomMarginGroupBox.TabStop = false;
			this.m_bottomMarginGroupBox.Text = "Bottom Margin:";
			// 
			// m_bottomMarginLabel
			// 
			this.m_bottomMarginLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_bottomMarginLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(170)), ((System.Byte)(170)));
			this.m_bottomMarginLabel.Location = new System.Drawing.Point(237, 8);
			this.m_bottomMarginLabel.Name = "m_bottomMarginLabel";
			this.m_bottomMarginLabel.Size = new System.Drawing.Size(40, 16);
			this.m_bottomMarginLabel.TabIndex = 35;
			this.m_bottomMarginLabel.Text = "0";
			// 
			// m_bottomMarginTrackBar
			// 
			this.m_bottomMarginTrackBar.AutoSize = false;
			this.m_bottomMarginTrackBar.BackColor = System.Drawing.Color.White;
			this.m_bottomMarginTrackBar.Location = new System.Drawing.Point(91, 0);
			this.m_bottomMarginTrackBar.Maximum = 100;
			this.m_bottomMarginTrackBar.Name = "m_bottomMarginTrackBar";
			this.m_bottomMarginTrackBar.Size = new System.Drawing.Size(144, 30);
			this.m_bottomMarginTrackBar.TabIndex = 40;
			this.m_bottomMarginTrackBar.TickFrequency = 10;
			this.m_bottomMarginTrackBar.Scroll += new System.EventHandler(this.m_bottomMarginTrackBar_Scroll);
			// 
			// m_leftMarginGroupBox
			// 
			this.m_leftMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_leftMarginLabel,
																							   this.m_leftMarginTrackBar});
			this.m_leftMarginGroupBox.Location = new System.Drawing.Point(36, 216);
			this.m_leftMarginGroupBox.Name = "m_leftMarginGroupBox";
			this.m_leftMarginGroupBox.ResizeChildren = false;
			this.m_leftMarginGroupBox.Size = new System.Drawing.Size(280, 36);
			this.m_leftMarginGroupBox.TabIndex = 30;
			this.m_leftMarginGroupBox.TabStop = false;
			this.m_leftMarginGroupBox.Text = "Left Margin:";
			// 
			// m_leftMarginLabel
			// 
			this.m_leftMarginLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_leftMarginLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(170)), ((System.Byte)(170)));
			this.m_leftMarginLabel.Location = new System.Drawing.Point(217, 8);
			this.m_leftMarginLabel.Name = "m_leftMarginLabel";
			this.m_leftMarginLabel.Size = new System.Drawing.Size(40, 16);
			this.m_leftMarginLabel.TabIndex = 35;
			this.m_leftMarginLabel.Text = "0";
			// 
			// m_leftMarginTrackBar
			// 
			this.m_leftMarginTrackBar.AutoSize = false;
			this.m_leftMarginTrackBar.BackColor = System.Drawing.Color.White;
			this.m_leftMarginTrackBar.Location = new System.Drawing.Point(71, 0);
			this.m_leftMarginTrackBar.Maximum = 100;
			this.m_leftMarginTrackBar.Name = "m_leftMarginTrackBar";
			this.m_leftMarginTrackBar.Size = new System.Drawing.Size(144, 30);
			this.m_leftMarginTrackBar.TabIndex = 60;
			this.m_leftMarginTrackBar.TickFrequency = 10;
			this.m_leftMarginTrackBar.Scroll += new System.EventHandler(this.m_leftMarginTrackBar_Scroll);
			// 
			// m_rightMarginGroupBox
			// 
			this.m_rightMarginGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.m_rightMarginLabel,
																								this.m_rightMarginTrackBar});
			this.m_rightMarginGroupBox.Location = new System.Drawing.Point(28, 160);
			this.m_rightMarginGroupBox.Name = "m_rightMarginGroupBox";
			this.m_rightMarginGroupBox.ResizeChildren = false;
			this.m_rightMarginGroupBox.Size = new System.Drawing.Size(280, 36);
			this.m_rightMarginGroupBox.TabIndex = 20;
			this.m_rightMarginGroupBox.TabStop = false;
			this.m_rightMarginGroupBox.Text = "Right Margin:";
			// 
			// m_rightMarginLabel
			// 
			this.m_rightMarginLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_rightMarginLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(170)), ((System.Byte)(170)));
			this.m_rightMarginLabel.Location = new System.Drawing.Point(225, 8);
			this.m_rightMarginLabel.Name = "m_rightMarginLabel";
			this.m_rightMarginLabel.Size = new System.Drawing.Size(40, 16);
			this.m_rightMarginLabel.TabIndex = 35;
			this.m_rightMarginLabel.Text = "0";
			// 
			// m_rightMarginTrackBar
			// 
			this.m_rightMarginTrackBar.AutoSize = false;
			this.m_rightMarginTrackBar.BackColor = System.Drawing.Color.White;
			this.m_rightMarginTrackBar.Location = new System.Drawing.Point(79, 0);
			this.m_rightMarginTrackBar.Maximum = 100;
			this.m_rightMarginTrackBar.Name = "m_rightMarginTrackBar";
			this.m_rightMarginTrackBar.Size = new System.Drawing.Size(144, 30);
			this.m_rightMarginTrackBar.TabIndex = 50;
			this.m_rightMarginTrackBar.TickFrequency = 10;
			this.m_rightMarginTrackBar.Scroll += new System.EventHandler(this.m_rightMarginTrackBar_Scroll);
			// 
			// WizardTitlesDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.DefaultHint = "Set legend visibility, font, position, layout, background, and item border settin" +
				"gs. ";
			this.DefaultHintTitle = "Legend";
			this.Name = "WizardTitlesDialog";
			this.Size = new System.Drawing.Size(324, 312);
			this.m_titlesTabPage.ResumeLayout(false);
			this.m_titleCollectionGroupBox.ResumeLayout(false);
			this.m_titleOptionsPanel.ResumeLayout(false);
			this.m_textMarginsGroupBox.ResumeLayout(false);
			this.m_topTextMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_topTextMarginUpDown)).EndInit();
			this.m_bottomTextMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_bottomTextMarginUpDown)).EndInit();
			this.m_leftTextMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_leftTextMarginUpDown)).EndInit();
			this.m_rightTextMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_rightTextMarginUpDown)).EndInit();
			this.m_fontGroupBox.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_shadeWidthUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_outlineWidthUpDown)).EndInit();
			this.m_positionGroupBox.ResumeLayout(false);
			this.m_refPointGroupBox.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.m_legendTabPage.ResumeLayout(false);
			this.m_marginsTabPage.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.m_topMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_topMarginTrackBar)).EndInit();
			this.m_bottomMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_bottomMarginTrackBar)).EndInit();
			this.m_leftMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_leftMarginTrackBar)).EndInit();
			this.m_rightMarginGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_rightMarginTrackBar)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion



		void PopulateTitlesList() 
		{
			listBox.Items.Clear();
			foreach (ComponentArt.Web.Visualization.Charting.ChartTitle ct in WinChart.Titles) 
			{
				listBox.Items.Add(ct.Text);
			}

		}


		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (WinChart == null) 
				return;

			PopulateTitlesList();

			if (listBox.Items.Count > 0)
				listBox.SelectedIndex = 0;

			m_rightMarginTrackBar.Value = (int)WinChart.View.Margins.Right;
			m_leftMarginTrackBar.Value = (int)WinChart.View.Margins.Left;
			m_bottomMarginTrackBar.Value = (int)WinChart.View.Margins.Bottom;
			m_topMarginTrackBar.Value = (int)WinChart.View.Margins.Top;

			m_bottomMarginLabel.Text = WinChart.View.Margins.Bottom.ToString();
			m_rightMarginLabel.Text = WinChart.View.Margins.Right.ToString();
			m_topMarginLabel.Text = WinChart.View.Margins.Top.ToString();
			m_leftMarginLabel.Text = WinChart.View.Margins.Left.ToString();

			m_autoResizeMarginsCheckBox.Checked = WinChart.ResizeMarginsToFitLabels;

#if __BUILDING_CRI_DESIGNER__
            m_titleComboBox.Items.Clear();
            m_titleComboBox.Items.Add("<Expression...>");
            SqlChartDesigner rschartDesigner = ((SqlChartDesigner)WinChart.Chart.Owner);
            Microsoft.ReportDesigner.RptDataSet dataset = rschartDesigner.GetDataSet();
            if (dataset != null)
            {
                for (int j = 0; j < dataset.Fields.Count; j++)
                {
                    m_titleComboBox.Items.Add("=Fields!" + dataset.Fields[j].Name + ".Value");
                }
            }

#endif

		}

		private void SelectedIndexChangedHandler(object sender, EventArgs e) 
		{
			if (m_typingText)
				return;

			if (ChartTitle == null) 
			{
				//if (!m_typingText)
				m_titleOptionsPanel.Enabled = false;
				return;
			}
			else 
			{
				m_titleOptionsPanel.Enabled = true;
			}

			m_positionComboBox.SelectedItem = ChartTitle.Position.ToString();
			m_refPointComboBox.SelectedItem = ChartTitle.RefPoint.ToString();
#if !__BUILDING_CRI_DESIGNER__
			m_titleText.Text = ChartTitle.Text;
#else
            m_titleComboBox.Text = ChartTitle.Text;
#endif

			m_topTextMarginUpDown.Value = (decimal)ChartTitle.TextTopMargin;
			m_bottomTextMarginUpDown.Value = (decimal)ChartTitle.TextBottomMargin;
			m_leftTextMarginUpDown.Value = (decimal)ChartTitle.TextLeftMargin;
			m_rightTextMarginUpDown.Value = (decimal)ChartTitle.TextRightMargin;

			WizardFrameControl.SetFontLabel(m_fontLabel, ChartTitle.Font);

			m_titleForeColor.Color = ChartTitle.ForeColor;

			m_shadeWidthUpDown.Value = (decimal)ChartTitle.TextShadeWidthPts;
			m_outlineWidthUpDown.Value = (decimal)ChartTitle.OutlineWidth;
		}

		bool m_typingText = false;

        private void textControl_TextChanged(object sender, System.EventArgs e)
        {
            Control textControl = (Control)sender;
            SetChartTitleText(textControl.Text);
        }

        void SetChartTitleText(string text) {
            if (ChartTitle.Text == text)
				return;

			m_typingText = true;

            ChartTitle.Text = text;
			//m_titlesCollection.Invalidate();
            listBox.Items[listBox.SelectedIndex] = text;
			WinChart.Invalidate();

			m_typingText = false;
		}

        private void m_positionComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ChartTitle.Position = (TitlePosition)Enum.Parse(typeof(TitlePosition), m_positionComboBox.SelectedItem.ToString());
			WinChart.Invalidate();
		}

		private void m_refPointComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ChartTitle.RefPoint = (TextReferencePoint)Enum.Parse(typeof(TextReferencePoint), m_refPointComboBox.SelectedItem.ToString());		
			WinChart.Invalidate();
		}

		private void m_rightTextMarginUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			ChartTitle.TextRightMargin = (double)m_rightTextMarginUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_topTextMarginUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			ChartTitle.TextTopMargin = (double)m_topTextMarginUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_leftTextMarginUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			ChartTitle.TextLeftMargin = (double)m_leftTextMarginUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_bottomTextMarginUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			ChartTitle.TextBottomMargin = (double)m_bottomTextMarginUpDown.Value;
			WinChart.Invalidate();
		}



		private void m_fontButton_Click(object sender, System.EventArgs e)
		{
			FontDialog fontDialog1 = new FontDialog();
			//fontDialog1.ShowColor = true;

			fontDialog1.Font = ChartTitle.Font;
			//fontDialog1.Color = WinChart.ForeColor;

			if(fontDialog1.ShowDialog() != DialogResult.Cancel )
			{
				ChartTitle.Font = fontDialog1.Font;
				//WinChart.ForeColor = fontDialog1.Color;
				WizardFrameControl.SetFontLabel(m_fontLabel, ChartTitle.Font);

				WinChart.Invalidate();
			}
		}

		private void m_titleForeColor_ColorChanged(object sender, System.EventArgs e)
		{
			ChartTitle.ForeColor = m_titleForeColor.Color;
			WinChart.Invalidate();
		}

		private void m_titleOutlineColor_ColorChanged(object sender, System.EventArgs e)
		{
			ChartTitle.OutlineColor = m_titleOutlineColor.Color;
			WinChart.Invalidate();
		}

		private void m_outlineWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			ChartTitle.OutlineWidth = (double)m_outlineWidthUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_titleShadeColor_ColorChanged(object sender, System.EventArgs e)
		{
			ChartTitle.TextShadowColor = m_titleShadeColor.Color;
			WinChart.Invalidate();
		}

		private void m_shadeWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			ChartTitle.TextShadeWidthPts = (double)m_shadeWidthUpDown.Value;
			WinChart.Invalidate();
		}


		private void m_bottomMarginTrackBar_Scroll(object sender, System.EventArgs e)
		{
			WinChart.View.Margins.Bottom = (double)m_bottomMarginTrackBar.Value;
			m_bottomMarginLabel.Text = WinChart.View.Margins.Bottom.ToString();
			m_bottomMarginLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_rightMarginTrackBar_Scroll(object sender, System.EventArgs e)
		{
			WinChart.View.Margins.Right = (double)m_rightMarginTrackBar.Value;
			m_rightMarginLabel.Text = WinChart.View.Margins.Right.ToString();
			m_rightMarginLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_leftMarginTrackBar_Scroll(object sender, System.EventArgs e)
		{
			WinChart.View.Margins.Left = (double)m_leftMarginTrackBar.Value;
			m_leftMarginLabel.Text = WinChart.View.Margins.Left.ToString();
			m_leftMarginLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_topMarginTrackBar_Scroll(object sender, System.EventArgs e)
		{
			WinChart.View.Margins.Top = (double)m_topMarginTrackBar.Value;
			m_topMarginLabel.Text = WinChart.View.Margins.Top.ToString();
			m_topMarginLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_moreSettingsButton_Click(object sender, System.EventArgs e)
		{
			int lastSelected = listBox.SelectedIndex;
			InvokeEditor(WinChart.Titles);

			PopulateTitlesList();

			if (listBox.Items.Count > lastSelected) 
				listBox.SelectedIndex = lastSelected;
			else if (listBox.Items.Count > 0)
				listBox.SelectedIndex = 0;

			WinChart.Invalidate();
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControl1.SelectedTab == m_legendTabPage) 
			{
				HintTitle = DefaultHintTitle = "Legend";
				Hint = DefaultHint = "Set legend visibility, font, position, layout, background, and item border settings.";
			} 
			else if (tabControl1.SelectedTab == m_titlesTabPage) 
			{
				HintTitle = DefaultHintTitle = "Titles";
				Hint = DefaultHint = "To add a title click 'Add'. Set title text, font, position, and margin settings.";
			} 
			else if (tabControl1.SelectedTab == m_marginsTabPage) 
			{
				HintTitle = DefaultHintTitle = "Margins";
				Hint = DefaultHint = "Set top, bottom, left, and right chart margins.";
			} 
		}

		private void m_addButton_Click(object sender, System.EventArgs e)
		{
			ComponentArt.Web.Visualization.Charting.ChartTitle ct = new ChartTitle();
			WinChart.Titles.Add(ct);
			listBox.Items.Add(ct.Text);
			if (listBox.Items.Count == 1)
				listBox.SelectedIndex = 0;

			WinChart.Invalidate();
		}

		private void m_removeButton_Click(object sender, System.EventArgs e)
		{
			int lastSelectedIndex = listBox.SelectedIndex;
			int nextSelectedIndex;

			if (lastSelectedIndex == -1)
				return;

			if (lastSelectedIndex == WinChart.Titles.Count-1 && WinChart.Titles.Count != 0) 
				nextSelectedIndex = WinChart.Titles.Count-2;
			else
				nextSelectedIndex = lastSelectedIndex;

			WinChart.Titles.RemoveAt(lastSelectedIndex);
			listBox.Items.RemoveAt(lastSelectedIndex);

			if (listBox.SelectedIndex != nextSelectedIndex)
				listBox.SelectedIndex = nextSelectedIndex;

			WinChart.Invalidate();
		}

		private void m_autoResizeMarginsCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.ResizeMarginsToFitLabels = m_autoResizeMarginsCheckBox.Checked;
			WinChart.Invalidate();
		}
	}
}
