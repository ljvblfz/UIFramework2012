using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WizardAxesDialog.
	/// </summary>
	internal class WizardCoordSystemDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.TabControl tabControl1;
		private ComponentArt.Web.Visualization.Charting.Design.WizardPlaneDialog m_wizardPlaneDialogXY;
		private ComponentArt.Web.Visualization.Charting.Design.WizardPlaneDialog m_wizardPlaneDialogYZ;
		private ComponentArt.Web.Visualization.Charting.Design.WizardPlaneDialog m_wizardPlaneDialogZX;
		private ComponentArt.Win.UI.Internal.TabPage m_axesTabPage;
		private System.Windows.Forms.Label m_titleOffsetLabel;
		private ComponentArt.Win.UI.Internal.Button m_options;
		private ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox m_annotationsLineStyleNameComboBox;
		[WizardHint(typeof(AxisAnnotation), "Visible")]
		private ComponentArt.Win.UI.Internal.CheckBox m_axisAnnotationsVisible;
		private ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox m_titleStyleNameComboBox;
		private ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox m_labelStyleNameComboBox;
		private System.Windows.Forms.TextBox m_axisAnnotationTitleTextBox;
		private ComponentArt.Win.UI.Internal.ComboBox m_annotationsComboBox;
		private System.Windows.Forms.TrackBar m_axisAnnotationTitleOffsetTrackBar;
		private ComponentArt.Win.UI.Internal.RadioButton m_XAxisRadioButton;
		private ComponentArt.Win.UI.Internal.RadioButton m_YAxisRadioButton;
		private ComponentArt.Win.UI.Internal.RadioButton m_ZAxisRadioButton;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		private ComponentArt.Win.UI.Internal.TabPage m_wizardTabPageXY;
		private ComponentArt.Win.UI.Internal.TabPage m_wizardTabPageYZ;
		private ComponentArt.Win.UI.Internal.TabPage m_wizardTabPageZX;
		[WizardHint("AxisAnnotations")]
		private ComponentArt.Win.UI.Internal.GroupBox m_axisAnnotationsGroupBox;
		[WizardHint(typeof(AxisAnnotation), "AxisTitle")]
		private ComponentArt.Win.UI.Internal.GroupBox m_axisTitleGroupBox;
		[WizardHint(typeof(AxisAnnotation), "AxisTitleOffsetPts")]
		private ComponentArt.Win.UI.Internal.GroupBox m_offsetGroupBox;
		[WizardHint(typeof(AxisAnnotation), "LabelStyleName")]
		private ComponentArt.Win.UI.Internal.GroupBox m_labelStyleGroupBox;
		[WizardHint(typeof(AxisAnnotation), "LineStyleName")]
		private ComponentArt.Win.UI.Internal.GroupBox m_lineStyleGroupBox;
		[WizardHint(typeof(AxisAnnotation), "AxisTitleStyleName")]
		private ComponentArt.Win.UI.Internal.GroupBox m_axisTitleStyleGroupBox;
		private ComponentArt.Win.UI.Internal.GroupBox m_chooseYourAxisGroupBox;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox1;
		private ComponentArt.Win.UI.Internal.ComboBox m_angleComboBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardCoordSystemDialog()
		{
			InitializeComponent();
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

		internal CoordinateSystem CoordinateSystem 
		{
			get 
			{
				return WinChart.CoordinateSystem;
			}
		}

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (WinChart == null)
				return;

			// Connect the comboboxes to collections
			m_titleStyleNameComboBox.SetProperty(typeof(AxisAnnotation).GetProperty("AxisTitleStyleName"), "Edit Label Styles...");
			m_labelStyleNameComboBox.SetProperty(typeof(AxisAnnotation).GetProperty("AxisTitleStyleName"), "Edit Label Styles...");
			m_annotationsLineStyleNameComboBox.SetProperty(typeof(AxisAnnotation).GetProperty("LineStyleName"), "Edit Line Styles...");

			m_XAxisRadioButton.Checked = true;

			SetupAnnotations();
		}


		void SetupAnnotations()
		{
			m_annotationsComboBox.Items.Clear();
			m_annotationsComboBox.Items.AddRange(Axis.AxisAnnotations.GetNames());
			if (m_annotationsComboBox.Items.Count>0) 
			{
				m_annotationsComboBox.SelectedIndex = 0;
			}
		}

		Axis m_axis = null;

		internal Axis Axis 
		{
			get 
			{
				return m_axis;
			}
		}

		internal AxisAnnotation AxisAnnotation 
		{
			get 
			{
				return Axis.AxisAnnotations[m_annotationsComboBox.Text];
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new ComponentArt.Win.UI.Internal.TabControl();
			this.m_wizardTabPageXY = new ComponentArt.Win.UI.Internal.TabPage();
			this.m_wizardPlaneDialogXY = new ComponentArt.Web.Visualization.Charting.Design.WizardPlaneDialog();
			this.m_wizardTabPageYZ = new ComponentArt.Win.UI.Internal.TabPage();
			this.m_wizardPlaneDialogYZ = new ComponentArt.Web.Visualization.Charting.Design.WizardPlaneDialog();
			this.m_wizardTabPageZX = new ComponentArt.Win.UI.Internal.TabPage();
			this.m_wizardPlaneDialogZX = new ComponentArt.Web.Visualization.Charting.Design.WizardPlaneDialog();
			this.m_axesTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.groupBox1 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_angleComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_chooseYourAxisGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_ZAxisRadioButton = new ComponentArt.Win.UI.Internal.RadioButton();
			this.m_YAxisRadioButton = new ComponentArt.Win.UI.Internal.RadioButton();
			this.m_XAxisRadioButton = new ComponentArt.Win.UI.Internal.RadioButton();
			this.m_axisAnnotationsVisible = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_options = new ComponentArt.Win.UI.Internal.Button();
			this.m_axisAnnotationsGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_annotationsComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_axisTitleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_axisAnnotationTitleTextBox = new System.Windows.Forms.TextBox();
			this.m_axisTitleStyleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_titleStyleNameComboBox = new ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox();
			this.m_offsetGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_axisAnnotationTitleOffsetTrackBar = new System.Windows.Forms.TrackBar();
			this.m_titleOffsetLabel = new System.Windows.Forms.Label();
			this.m_labelStyleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_labelStyleNameComboBox = new ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox();
			this.m_lineStyleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_annotationsLineStyleNameComboBox = new ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox();
			this.tabControl1.SuspendLayout();
			this.m_wizardTabPageXY.SuspendLayout();
			this.m_wizardTabPageYZ.SuspendLayout();
			this.m_wizardTabPageZX.SuspendLayout();
			this.m_axesTabPage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.m_chooseYourAxisGroupBox.SuspendLayout();
			this.m_axisAnnotationsGroupBox.SuspendLayout();
			this.m_axisTitleGroupBox.SuspendLayout();
			this.m_axisTitleStyleGroupBox.SuspendLayout();
			this.m_offsetGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_axisAnnotationTitleOffsetTrackBar)).BeginInit();
			this.m_labelStyleGroupBox.SuspendLayout();
			this.m_lineStyleGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.m_wizardTabPageXY,
																					  this.m_wizardTabPageYZ,
																					  this.m_wizardTabPageZX,
																					  this.m_axesTabPage});
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Size = new System.Drawing.Size(324, 312);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// m_wizardTabPageXY
			// 
			this.m_wizardTabPageXY.BackColor = System.Drawing.Color.White;
			this.m_wizardTabPageXY.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_wizardPlaneDialogXY});
			this.m_wizardTabPageXY.Location = new System.Drawing.Point(2, 25);
			this.m_wizardTabPageXY.Name = "m_wizardTabPageXY";
			this.m_wizardTabPageXY.Size = new System.Drawing.Size(320, 285);
			this.m_wizardTabPageXY.TabIndex = 0;
			this.m_wizardTabPageXY.Text = "Plane XY";
			// 
			// m_wizardPlaneDialogXY
			// 
			this.m_wizardPlaneDialogXY.BackColor = System.Drawing.Color.White;
			this.m_wizardPlaneDialogXY.DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			this.m_wizardPlaneDialogXY.DefaultHintTitle = "Coordinate System â€“ Plane XY";
			this.m_wizardPlaneDialogXY.Name = "m_wizardPlaneDialogXY";
			this.m_wizardPlaneDialogXY.Size = new System.Drawing.Size(320, 264);
			this.m_wizardPlaneDialogXY.TabIndex = 0;
			// 
			// m_wizardTabPageYZ
			// 
			this.m_wizardTabPageYZ.BackColor = System.Drawing.Color.White;
			this.m_wizardTabPageYZ.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_wizardPlaneDialogYZ});
			this.m_wizardTabPageYZ.Location = new System.Drawing.Point(2, 25);
			this.m_wizardTabPageYZ.Name = "m_wizardTabPageYZ";
			this.m_wizardTabPageYZ.Size = new System.Drawing.Size(320, 285);
			this.m_wizardTabPageYZ.TabIndex = 1;
			this.m_wizardTabPageYZ.Text = "Plane YZ";
			// 
			// m_wizardPlaneDialogYZ
			// 
			this.m_wizardPlaneDialogYZ.BackColor = System.Drawing.Color.White;
			this.m_wizardPlaneDialogYZ.DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			this.m_wizardPlaneDialogYZ.DefaultHintTitle = "Coordinate System â€“ Plane XY";
			this.m_wizardPlaneDialogYZ.Name = "m_wizardPlaneDialogYZ";
			this.m_wizardPlaneDialogYZ.Size = new System.Drawing.Size(320, 264);
			this.m_wizardPlaneDialogYZ.TabIndex = 1;
			// 
			// m_wizardTabPageZX
			// 
			this.m_wizardTabPageZX.BackColor = System.Drawing.Color.White;
			this.m_wizardTabPageZX.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_wizardPlaneDialogZX});
			this.m_wizardTabPageZX.Location = new System.Drawing.Point(2, 25);
			this.m_wizardTabPageZX.Name = "m_wizardTabPageZX";
			this.m_wizardTabPageZX.Size = new System.Drawing.Size(320, 285);
			this.m_wizardTabPageZX.TabIndex = 2;
			this.m_wizardTabPageZX.Text = "Plane ZX";
			// 
			// m_wizardPlaneDialogZX
			// 
			this.m_wizardPlaneDialogZX.BackColor = System.Drawing.Color.White;
			this.m_wizardPlaneDialogZX.DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			this.m_wizardPlaneDialogZX.DefaultHintTitle = "Coordinate System â€“ Plane XY";
			this.m_wizardPlaneDialogZX.Name = "m_wizardPlaneDialogZX";
			this.m_wizardPlaneDialogZX.Size = new System.Drawing.Size(320, 264);
			this.m_wizardPlaneDialogZX.TabIndex = 1;
			// 
			// m_axesTabPage
			// 
			this.m_axesTabPage.BackColor = System.Drawing.Color.White;
			this.m_axesTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						this.groupBox1,
																						this.separator2,
																						this.separator1,
																						this.m_chooseYourAxisGroupBox,
																						this.m_axisAnnotationsVisible,
																						this.m_options,
																						this.m_axisAnnotationsGroupBox,
																						this.m_axisTitleGroupBox,
																						this.m_axisTitleStyleGroupBox,
																						this.m_offsetGroupBox,
																						this.m_labelStyleGroupBox,
																						this.m_lineStyleGroupBox});
			this.m_axesTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_axesTabPage.Name = "m_axesTabPage";
			this.m_axesTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_axesTabPage.TabIndex = 3;
			this.m_axesTabPage.Text = "Axes";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_angleComboBox});
			this.groupBox1.Location = new System.Drawing.Point(208, 134);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.ResizeChildren = false;
			this.groupBox1.Size = new System.Drawing.Size(103, 28);
			this.groupBox1.TabIndex = 92;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Angle:";
			// 
			// m_angleComboBox
			// 
			this.m_angleComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_angleComboBox.Items.AddRange(new object[] {
																 "0",
																 "45",
																 "90"});
			this.m_angleComboBox.Location = new System.Drawing.Point(48, 2);
			this.m_angleComboBox.Name = "m_angleComboBox";
			this.m_angleComboBox.Size = new System.Drawing.Size(49, 21);
			this.m_angleComboBox.TabIndex = 11;
			this.m_angleComboBox.TextChanged += new System.EventHandler(this.m_angleComboBox_TextChanged);
			// 
			// separator2
			// 
			this.separator2.Location = new System.Drawing.Point(12, 180);
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(296, 3);
			this.separator2.TabIndex = 91;
			this.separator2.TabStop = false;
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(8, 64);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(296, 3);
			this.separator1.TabIndex = 48;
			this.separator1.TabStop = false;
			// 
			// m_chooseYourAxisGroupBox
			// 
			this.m_chooseYourAxisGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.m_ZAxisRadioButton,
																								   this.m_YAxisRadioButton,
																								   this.m_XAxisRadioButton});
			this.m_chooseYourAxisGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_chooseYourAxisGroupBox.Name = "m_chooseYourAxisGroupBox";
			this.m_chooseYourAxisGroupBox.ResizeChildren = false;
			this.m_chooseYourAxisGroupBox.Size = new System.Drawing.Size(304, 48);
			this.m_chooseYourAxisGroupBox.TabIndex = 1;
			this.m_chooseYourAxisGroupBox.TabStop = false;
			this.m_chooseYourAxisGroupBox.Text = "Choose your axis:";
			// 
			// m_ZAxisRadioButton
			// 
			this.m_ZAxisRadioButton.BackColor = System.Drawing.Color.White;
			this.m_ZAxisRadioButton.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_ZAxisRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_ZAxisRadioButton.Location = new System.Drawing.Point(216, 28);
			this.m_ZAxisRadioButton.Name = "m_ZAxisRadioButton";
			this.m_ZAxisRadioButton.Size = new System.Drawing.Size(62, 16);
			this.m_ZAxisRadioButton.TabIndex = 2;
			this.m_ZAxisRadioButton.Text = "Z Axis";
			this.m_ZAxisRadioButton.CheckedChanged += new System.EventHandler(this.m_ZAxisRadioButton_CheckedChanged);
			// 
			// m_YAxisRadioButton
			// 
			this.m_YAxisRadioButton.BackColor = System.Drawing.Color.White;
			this.m_YAxisRadioButton.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_YAxisRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_YAxisRadioButton.Location = new System.Drawing.Point(112, 28);
			this.m_YAxisRadioButton.Name = "m_YAxisRadioButton";
			this.m_YAxisRadioButton.Size = new System.Drawing.Size(62, 16);
			this.m_YAxisRadioButton.TabIndex = 1;
			this.m_YAxisRadioButton.Text = "Y Axis";
			this.m_YAxisRadioButton.CheckedChanged += new System.EventHandler(this.m_YAxisRadioButton_CheckedChanged);
			// 
			// m_XAxisRadioButton
			// 
			this.m_XAxisRadioButton.BackColor = System.Drawing.Color.White;
			this.m_XAxisRadioButton.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_XAxisRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_XAxisRadioButton.Location = new System.Drawing.Point(8, 28);
			this.m_XAxisRadioButton.Name = "m_XAxisRadioButton";
			this.m_XAxisRadioButton.Size = new System.Drawing.Size(62, 16);
			this.m_XAxisRadioButton.TabIndex = 0;
			this.m_XAxisRadioButton.Text = "X Axis";
			this.m_XAxisRadioButton.CheckedChanged += new System.EventHandler(this.m_XAxisRadioButton_CheckedChanged);
			// 
			// m_axisAnnotationsVisible
			// 
			this.m_axisAnnotationsVisible.BackColor = System.Drawing.Color.White;
			this.m_axisAnnotationsVisible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_axisAnnotationsVisible.Checked = true;
			this.m_axisAnnotationsVisible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_axisAnnotationsVisible.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_axisAnnotationsVisible.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_axisAnnotationsVisible.Location = new System.Drawing.Point(185, 196);
			this.m_axisAnnotationsVisible.Name = "m_axisAnnotationsVisible";
			this.m_axisAnnotationsVisible.Size = new System.Drawing.Size(58, 16);
			this.m_axisAnnotationsVisible.TabIndex = 70;
			this.m_axisAnnotationsVisible.Text = "Show";
			this.m_axisAnnotationsVisible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_axisAnnotationsVisible.CheckedChanged += new System.EventHandler(this.m_axisAnnotationsVisible_CheckedChanged);
			// 
			// m_options
			// 
			this.m_options.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.m_options.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_options.Location = new System.Drawing.Point(200, 226);
			this.m_options.Name = "m_options";
			this.m_options.Size = new System.Drawing.Size(104, 23);
			this.m_options.TabIndex = 90;
			this.m_options.Text = "More Settings...";
			this.m_options.Click += new System.EventHandler(this.m_options_Click);
			// 
			// m_axisAnnotationsGroupBox
			// 
			this.m_axisAnnotationsGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.m_annotationsComboBox});
			this.m_axisAnnotationsGroupBox.Location = new System.Drawing.Point(6, 70);
			this.m_axisAnnotationsGroupBox.Name = "m_axisAnnotationsGroupBox";
			this.m_axisAnnotationsGroupBox.ResizeChildren = false;
			this.m_axisAnnotationsGroupBox.Size = new System.Drawing.Size(304, 28);
			this.m_axisAnnotationsGroupBox.TabIndex = 10;
			this.m_axisAnnotationsGroupBox.TabStop = false;
			this.m_axisAnnotationsGroupBox.Text = "X Axis Annotations:";
			// 
			// m_annotationsComboBox
			// 
			this.m_annotationsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_annotationsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_annotationsComboBox.Location = new System.Drawing.Point(122, 3);
			this.m_annotationsComboBox.Name = "m_annotationsComboBox";
			this.m_annotationsComboBox.Size = new System.Drawing.Size(176, 21);
			this.m_annotationsComboBox.TabIndex = 10;
			this.m_annotationsComboBox.SelectedIndexChanged += new System.EventHandler(this.m_annotationsComboBox_SelectedIndexChanged);
			// 
			// m_axisTitleGroupBox
			// 
			this.m_axisTitleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.m_axisAnnotationTitleTextBox});
			this.m_axisTitleGroupBox.Location = new System.Drawing.Point(6, 102);
			this.m_axisTitleGroupBox.Name = "m_axisTitleGroupBox";
			this.m_axisTitleGroupBox.ResizeChildren = false;
			this.m_axisTitleGroupBox.Size = new System.Drawing.Size(146, 28);
			this.m_axisTitleGroupBox.TabIndex = 20;
			this.m_axisTitleGroupBox.TabStop = false;
			this.m_axisTitleGroupBox.Text = "Title:";
			// 
			// m_axisAnnotationTitleTextBox
			// 
			this.m_axisAnnotationTitleTextBox.Location = new System.Drawing.Point(40, 3);
			this.m_axisAnnotationTitleTextBox.Name = "m_axisAnnotationTitleTextBox";
			this.m_axisAnnotationTitleTextBox.TabIndex = 20;
			this.m_axisAnnotationTitleTextBox.Text = "";
			this.m_axisAnnotationTitleTextBox.TextChanged += new System.EventHandler(this.m_axisAnnotationTitleTextBox_TextChanged);
			// 
			// m_axisTitleStyleGroupBox
			// 
			this.m_axisTitleStyleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.m_titleStyleNameComboBox});
			this.m_axisTitleStyleGroupBox.Location = new System.Drawing.Point(161, 102);
			this.m_axisTitleStyleGroupBox.Name = "m_axisTitleStyleGroupBox";
			this.m_axisTitleStyleGroupBox.ResizeChildren = false;
			this.m_axisTitleStyleGroupBox.Size = new System.Drawing.Size(146, 28);
			this.m_axisTitleStyleGroupBox.TabIndex = 30;
			this.m_axisTitleStyleGroupBox.TabStop = false;
			this.m_axisTitleStyleGroupBox.Text = "Style:";
			// 
			// m_titleStyleNameComboBox
			// 
			this.m_titleStyleNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_titleStyleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_titleStyleNameComboBox.Location = new System.Drawing.Point(40, 2);
			this.m_titleStyleNameComboBox.Name = "m_titleStyleNameComboBox";
			this.m_titleStyleNameComboBox.Size = new System.Drawing.Size(104, 21);
			this.m_titleStyleNameComboBox.TabIndex = 30;
			this.m_titleStyleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.m_titleStyleNameComboBox_SelectedIndexChanged);
			// 
			// m_offsetGroupBox
			// 
			this.m_offsetGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.m_axisAnnotationTitleOffsetTrackBar,
																						   this.m_titleOffsetLabel});
			this.m_offsetGroupBox.Location = new System.Drawing.Point(6, 134);
			this.m_offsetGroupBox.Name = "m_offsetGroupBox";
			this.m_offsetGroupBox.ResizeChildren = false;
			this.m_offsetGroupBox.Size = new System.Drawing.Size(198, 38);
			this.m_offsetGroupBox.TabIndex = 40;
			this.m_offsetGroupBox.TabStop = false;
			this.m_offsetGroupBox.Text = "Offset";
			// 
			// m_axisAnnotationTitleOffsetTrackBar
			// 
			this.m_axisAnnotationTitleOffsetTrackBar.BackColor = System.Drawing.Color.White;
			this.m_axisAnnotationTitleOffsetTrackBar.Location = new System.Drawing.Point(40, 2);
			this.m_axisAnnotationTitleOffsetTrackBar.Maximum = 100;
			this.m_axisAnnotationTitleOffsetTrackBar.Name = "m_axisAnnotationTitleOffsetTrackBar";
			this.m_axisAnnotationTitleOffsetTrackBar.Size = new System.Drawing.Size(124, 45);
			this.m_axisAnnotationTitleOffsetTrackBar.TabIndex = 40;
			this.m_axisAnnotationTitleOffsetTrackBar.TickFrequency = 10;
			this.m_axisAnnotationTitleOffsetTrackBar.Scroll += new System.EventHandler(this.m_axisAnnotationTitleOffsetTrackBar_Scroll);
			// 
			// m_titleOffsetLabel
			// 
			this.m_titleOffsetLabel.BackColor = System.Drawing.Color.White;
			this.m_titleOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_titleOffsetLabel.Location = new System.Drawing.Point(158, 8);
			this.m_titleOffsetLabel.Name = "m_titleOffsetLabel";
			this.m_titleOffsetLabel.Size = new System.Drawing.Size(28, 16);
			this.m_titleOffsetLabel.TabIndex = 58;
			this.m_titleOffsetLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_labelStyleGroupBox
			// 
			this.m_labelStyleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_labelStyleNameComboBox});
			this.m_labelStyleGroupBox.Location = new System.Drawing.Point(2, 194);
			this.m_labelStyleGroupBox.Name = "m_labelStyleGroupBox";
			this.m_labelStyleGroupBox.ResizeChildren = false;
			this.m_labelStyleGroupBox.Size = new System.Drawing.Size(184, 24);
			this.m_labelStyleGroupBox.TabIndex = 50;
			this.m_labelStyleGroupBox.TabStop = false;
			this.m_labelStyleGroupBox.Text = "Label Style:";
			// 
			// m_labelStyleNameComboBox
			// 
			this.m_labelStyleNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_labelStyleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_labelStyleNameComboBox.Location = new System.Drawing.Point(76, 2);
			this.m_labelStyleNameComboBox.Name = "m_labelStyleNameComboBox";
			this.m_labelStyleNameComboBox.Size = new System.Drawing.Size(108, 21);
			this.m_labelStyleNameComboBox.TabIndex = 50;
			this.m_labelStyleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.m_labelStyleNameComboBox_SelectedIndexChanged);
			// 
			// m_lineStyleGroupBox
			// 
			this.m_lineStyleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.m_annotationsLineStyleNameComboBox});
			this.m_lineStyleGroupBox.Location = new System.Drawing.Point(9, 226);
			this.m_lineStyleGroupBox.Name = "m_lineStyleGroupBox";
			this.m_lineStyleGroupBox.ResizeChildren = false;
			this.m_lineStyleGroupBox.Size = new System.Drawing.Size(184, 24);
			this.m_lineStyleGroupBox.TabIndex = 60;
			this.m_lineStyleGroupBox.TabStop = false;
			this.m_lineStyleGroupBox.Text = "Line Style:";
			// 
			// m_annotationsLineStyleNameComboBox
			// 
			this.m_annotationsLineStyleNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_annotationsLineStyleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_annotationsLineStyleNameComboBox.Location = new System.Drawing.Point(69, 2);
			this.m_annotationsLineStyleNameComboBox.Name = "m_annotationsLineStyleNameComboBox";
			this.m_annotationsLineStyleNameComboBox.Size = new System.Drawing.Size(108, 21);
			this.m_annotationsLineStyleNameComboBox.TabIndex = 60;
			this.m_annotationsLineStyleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.m_annotationsLineStyleNameComboBox_SelectedIndexChanged);
			// 
			// WizardCoordSystemDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			this.DefaultHintTitle = "Coordinate System â€“ Plane XY";
			this.Name = "WizardCoordSystemDialog";
			this.Size = new System.Drawing.Size(324, 312);
			this.tabControl1.ResumeLayout(false);
			this.m_wizardTabPageXY.ResumeLayout(false);
			this.m_wizardTabPageYZ.ResumeLayout(false);
			this.m_wizardTabPageZX.ResumeLayout(false);
			this.m_axesTabPage.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.m_chooseYourAxisGroupBox.ResumeLayout(false);
			this.m_axisAnnotationsGroupBox.ResumeLayout(false);
			this.m_axisTitleGroupBox.ResumeLayout(false);
			this.m_axisTitleStyleGroupBox.ResumeLayout(false);
			this.m_offsetGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_axisAnnotationTitleOffsetTrackBar)).EndInit();
			this.m_labelStyleGroupBox.ResumeLayout(false);
			this.m_lineStyleGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void m_annotationsComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_axisAnnotationsVisible.Checked = AxisAnnotation.Visible;
			m_axisAnnotationTitleOffsetTrackBar.Value = (int)AxisAnnotation.AxisTitleOffsetPts;
			m_titleOffsetLabel.Text = m_axisAnnotationTitleOffsetTrackBar.Value.ToString();
			m_axisAnnotationTitleTextBox.Text = AxisAnnotation.AxisTitle;
			m_titleStyleNameComboBox.Text = AxisAnnotation.AxisTitleStyleName;
			m_labelStyleNameComboBox.Text = AxisAnnotation.LabelStyleName;
			m_annotationsLineStyleNameComboBox.Text = AxisAnnotation.LineStyleName;
			m_angleComboBox.Text = AxisAnnotation.RotationAngle.ToString();
		}

		private void m_titleStyleNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			AxisAnnotation.AxisTitleStyleName = (string)m_titleStyleNameComboBox.Text;	
			WinChart.Invalidate();
		}

		private void m_axisAnnotationsVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			AxisAnnotation.Visible = m_axisAnnotationsVisible.Checked;
			WinChart.Invalidate();
		}

		private void m_annotationsLineStyleNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			AxisAnnotation.LineStyleName = (string)m_annotationsLineStyleNameComboBox.Text;
			WinChart.Invalidate();
		}

		private void m_axisAnnotationTitleTextBox_TextChanged(object sender, System.EventArgs e)
		{
			AxisAnnotation.AxisTitle = m_axisAnnotationTitleTextBox.Text;
			WinChart.Invalidate();
		}

		private void m_labelStyleNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			AxisAnnotation.LabelStyleName = (string)m_labelStyleNameComboBox.Text;
			WinChart.Invalidate();
		}

		private void m_options_Click(object sender, System.EventArgs e)
		{
			int lastIndex = m_annotationsComboBox.SelectedIndex;

			InvokeEditor(Axis.AxisAnnotations);

			m_annotationsComboBox.Items.Clear();
			m_annotationsComboBox.Items.AddRange(Axis.AxisAnnotations.GetNames());

			if (lastIndex < m_annotationsComboBox.Items.Count) 
			{
				m_annotationsComboBox.SelectedIndex = lastIndex;
			} 			
			else if (m_annotationsComboBox.Items.Count>0) 
			{
				m_annotationsComboBox.SelectedIndex = 0;
			} 
			else 
			{
				m_annotationsComboBox.SelectedIndex = -1;
			}

			WinChart.Invalidate();
		}

		private void m_axisAnnotationTitleOffsetTrackBar_Scroll(object sender, System.EventArgs e)
		{
			AxisAnnotation.AxisTitleOffsetPts = (double)m_axisAnnotationTitleOffsetTrackBar.Value;
			m_titleOffsetLabel.Text = m_axisAnnotationTitleOffsetTrackBar.Value.ToString();
			m_titleOffsetLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_XAxisRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_axis = CoordinateSystem.XAxis;
			m_axisAnnotationsGroupBox.Text = "X Axis Annotations:";
			SetupAnnotations();
		}

		private void m_YAxisRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_axis = CoordinateSystem.YAxis;
			m_axisAnnotationsGroupBox.Text = "Y Axis Annotations:";
			SetupAnnotations();
		}

		private void m_ZAxisRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_axis = CoordinateSystem.ZAxis;
			m_axisAnnotationsGroupBox.Text = "Z Axis Annotations:";
			SetupAnnotations();
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControl1.SelectedTab == m_wizardTabPageXY) 
			{
				HintTitle = DefaultHintTitle = "Coordinate System â€“ Plane XY";
				Hint = DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			} 
			else if (tabControl1.SelectedTab == m_wizardTabPageYZ) 
			{
				HintTitle = DefaultHintTitle = "Coordinate System â€“ Plane YZ";
				Hint = DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			} 
			else if (tabControl1.SelectedTab == m_wizardTabPageZX) 
			{
				HintTitle = DefaultHintTitle = "Coordinate System â€“ Plane ZX";
				Hint = DefaultHint = "Set plane visibility, grid lines, grid stripes, plane depth and plane offset.";
			} 
			else 
			{
				HintTitle = DefaultHintTitle = "Axes Annotations";
				Hint = DefaultHint = "Several annotations can be created for each axes.";
			}
		}

		private void m_angleComboBox_TextChanged(object sender, System.EventArgs e)
		{
			try 
			{
				AxisAnnotation.RotationAngle = double.Parse(m_angleComboBox.Text);
				WinChart.Invalidate();
			} 
			catch 
			{
			}
		}
	}
}
