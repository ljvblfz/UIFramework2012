using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using ComponentArt.Win.UI.Internal;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WizardMappingDialog.
	/// </summary>
	internal class WizardMappingDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.ComboBox m_projectionKindComboBox;
		private ComponentArt.Win.UI.Internal.TabControl tabControl1;
		private ComponentArt.Win.UI.Internal.TabPage tabPage1;
		private Design.Vector3DTextBoxesControlHorizontal m_viewDirectionTextBoxes;
		[WizardHint("Wizard3DOptionsDirection")]
		private ComponentArt.Win.UI.Internal.GroupBox m_viewingDirectionGroupBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		const double min = 1.5;
		const double med = 2.5;
		const double max = 20.0;

		//val = f(x) = c+a/(b-x)
		//val-c = a/(b-x)
		//b-x = a/(val-c)
		//x = b-a/(val-c)

		//f(0) = min
		//f(0.5) = med
		//f(1) = max

        // After working out the formula we get the values for a, b and c:

		const double b = 
			(max-med) /
			(max+min-2*med);

		const double a = (max-min)*b*(b-1.0);
		private System.Windows.Forms.Label m_renderingPrecisionLabel;
		private ComponentArt.Win.UI.Internal.TrackBar m_renderingPrecisionTrackBar;
		[WizardHint(typeof(Mapping), "LightsOffOn2D")]
		private ComponentArt.Win.UI.Internal.CheckBox m_lightsCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private ComponentArt.Win.UI.Internal.TrackBar m_perspectiveTrackBar;
		private System.Windows.Forms.Label m_perspectiveLabel;
		private ComponentArt.Win.UI.Internal.ComboBox m_orientationComboBox;
		[WizardHint(typeof(Mapping), "Perspective")]
		private ComponentArt.Win.UI.Internal.GroupBox m_perspectiveGroupBox;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox m_xAxisReduction;
		private System.Windows.Forms.TextBox m_yAxisReduction;
		private ComponentArt.Win.UI.Internal.GroupBox m_zAxisReductionGroupBox;
		[WizardHint(typeof(CoordinateSystem), "Orientation")]
		private ComponentArt.Win.UI.Internal.GroupBox m_orientationGroupBox;
		[WizardHint(ChartBase.MainAssemblyTypeName, "RenderingPrecision")]
		private ComponentArt.Win.UI.Internal.GroupBox m_renderingPrecisionGroupBox;
		[WizardHint(typeof(Mapping), "Kind")]
		private ComponentArt.Win.UI.Internal.GroupBox m_projectionKindGroupBox;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		const double c = min-a/b;

		public WizardMappingDialog()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (WinChart != null) 
			{
				m_projectionKindComboBox.SelectedIndex = (int)WinChart.Mapping.Kind;

				//x = b-a/(val-c)
				double x = b-a/(WinChart.Mapping.PerspectiveFactor-c);

				m_perspectiveTrackBar.Value = WinChart.Mapping.Perspective;
				m_perspectiveTrackBar_Scroll(this, EventArgs.Empty);

				m_renderingPrecisionTrackBar.Value = (int)((0.5-WinChart.RenderingPrecision) / 0.4 * (double)m_renderingPrecisionTrackBar.Maximum);
				m_renderingPrecisionTrackBar_Scroll(this, EventArgs.Empty);

				m_lightsCheckBox.Checked = (WinChart.View.LightsOffOn2D == false);

				m_xAxisReduction.Text = WinChart.Mapping.XReductionOfZAxis.ToString();
				m_yAxisReduction.Text = WinChart.Mapping.YReductionOfZAxis.ToString();

				WinChart.TrackballEnabled = Visible;

				WinChart.Mapping.ViewDirectionChanged += new EventHandler(WinChartDirectionChanged);
				WinChartDirectionChanged(this, EventArgs.Empty);
			}
		}

		protected override void OnVisibleChanged(EventArgs e) 
		{
			if (Visible)
				SetupOrientation();
		}

		void SetupOrientation() 
		{
			m_orientationComboBox.Items.Clear();

			if (WinChart == null)
				return;

			if (WinChart.Chart.Series.Style.IsRadar || WinChart.Chart.Series.Style.ChartKind == ChartKind.Pie
				|| WinChart.Chart.Series.Style.ChartKind == ChartKind.Doughnut) 
			{
				m_orientationComboBox.Items.AddRange(new object [] {CoordinateSystemOrientation.Default});
			}
			else if (WinChart.View.Kind == ProjectionKind.TwoDimensional) 
				m_orientationComboBox.Items.AddRange(new string [] {CoordinateSystemOrientation.Default.ToString(), CoordinateSystemOrientation.Horizontal.ToString()});
			else
				m_orientationComboBox.Items.AddRange(Enum.GetNames(typeof(CoordinateSystemOrientation)));
			m_orientationComboBox.Text = WinChart.CoordinateSystem.Orientation.ToString();
		}

		void WinChartDirectionChanged(object sender, EventArgs e) 
		{
			m_viewDirectionTextBoxes.Vector = WinChart.Mapping.ViewDirection;
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
			this.m_projectionKindComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.tabControl1 = new ComponentArt.Win.UI.Internal.TabControl();
			this.tabPage1 = new ComponentArt.Win.UI.Internal.TabPage();
			this.m_lightsCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_perspectiveGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.m_perspectiveLabel = new System.Windows.Forms.Label();
			this.m_perspectiveTrackBar = new ComponentArt.Win.UI.Internal.TrackBar();
			this.m_zAxisReductionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_yAxisReduction = new System.Windows.Forms.TextBox();
			this.m_xAxisReduction = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.m_orientationGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_orientationComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_renderingPrecisionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.m_renderingPrecisionLabel = new System.Windows.Forms.Label();
			this.m_renderingPrecisionTrackBar = new ComponentArt.Win.UI.Internal.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.m_projectionKindGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_viewingDirectionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_viewDirectionTextBoxes = new ComponentArt.Web.Visualization.Charting.Design.Vector3DTextBoxesControlHorizontal();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.m_perspectiveGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_perspectiveTrackBar)).BeginInit();
			this.m_zAxisReductionGroupBox.SuspendLayout();
			this.m_orientationGroupBox.SuspendLayout();
			this.m_renderingPrecisionGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_renderingPrecisionTrackBar)).BeginInit();
			this.m_projectionKindGroupBox.SuspendLayout();
			this.m_viewingDirectionGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_projectionKindComboBox
			// 
			this.m_projectionKindComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_projectionKindComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_projectionKindComboBox.Items.AddRange(new object[] {
																		  "3D - Perspective",
																		  "3D - Isometric",
																		  "3D - Parallel",
																		  "2D"});
			this.m_projectionKindComboBox.Location = new System.Drawing.Point(2, 26);
			this.m_projectionKindComboBox.Name = "m_projectionKindComboBox";
			this.m_projectionKindComboBox.Size = new System.Drawing.Size(144, 21);
			this.m_projectionKindComboBox.TabIndex = 0;
			this.m_projectionKindComboBox.SelectedIndexChanged += new System.EventHandler(this.m_projectionKindComboBox_SelectedIndexChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1});
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Size = new System.Drawing.Size(324, 312);
			this.tabControl1.TabIndex = 14;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.m_lightsCheckBox,
																				   this.separator2,
																				   this.separator1,
																				   this.m_perspectiveGroupBox,
																				   this.m_zAxisReductionGroupBox,
																				   this.m_orientationGroupBox,
																				   this.m_renderingPrecisionGroupBox,
																				   this.m_projectionKindGroupBox,
																				   this.m_viewingDirectionGroupBox});
			this.tabPage1.Location = new System.Drawing.Point(2, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(320, 285);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "3D Options";
			// 
			// m_lightsCheckBox
			// 
			this.m_lightsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_lightsCheckBox.Checked = true;
			this.m_lightsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_lightsCheckBox.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_lightsCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_lightsCheckBox.Location = new System.Drawing.Point(8, 184);
			this.m_lightsCheckBox.Name = "m_lightsCheckBox";
			this.m_lightsCheckBox.Size = new System.Drawing.Size(96, 16);
			this.m_lightsCheckBox.TabIndex = 45;
			this.m_lightsCheckBox.Text = "Lights in 2D";
			this.m_lightsCheckBox.CheckedChanged += new System.EventHandler(this.m_lightsCheckBox_CheckedChanged);
			// 
			// separator2
			// 
			this.separator2.Location = new System.Drawing.Point(8, 72);
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(304, 1);
			this.separator2.TabIndex = 47;
			this.separator2.TabStop = false;
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(8, 176);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(304, 1);
			this.separator1.TabIndex = 46;
			this.separator1.TabStop = false;
			// 
			// m_perspectiveGroupBox
			// 
			this.m_perspectiveGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.label9,
																								this.label10,
																								this.m_perspectiveLabel,
																								this.m_perspectiveTrackBar});
			this.m_perspectiveGroupBox.Location = new System.Drawing.Point(165, 80);
			this.m_perspectiveGroupBox.Name = "m_perspectiveGroupBox";
			this.m_perspectiveGroupBox.ResizeChildren = false;
			this.m_perspectiveGroupBox.Size = new System.Drawing.Size(147, 88);
			this.m_perspectiveGroupBox.TabIndex = 37;
			this.m_perspectiveGroupBox.TabStop = false;
			this.m_perspectiveGroupBox.Text = "Perspective:";
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.White;
			this.label9.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label9.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(204)), ((System.Byte)(204)), ((System.Byte)(204)));
			this.label9.Location = new System.Drawing.Point(112, 24);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(30, 11);
			this.label9.TabIndex = 7;
			this.label9.Text = "Max";
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.White;
			this.label10.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label10.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(204)), ((System.Byte)(204)), ((System.Byte)(204)));
			this.label10.Location = new System.Drawing.Point(8, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(27, 11);
			this.label10.TabIndex = 6;
			this.label10.Text = "Min";
			// 
			// m_perspectiveLabel
			// 
			this.m_perspectiveLabel.BackColor = System.Drawing.Color.Transparent;
			this.m_perspectiveLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_perspectiveLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_perspectiveLabel.Location = new System.Drawing.Point(104, 6);
			this.m_perspectiveLabel.Name = "m_perspectiveLabel";
			this.m_perspectiveLabel.Size = new System.Drawing.Size(40, 14);
			this.m_perspectiveLabel.TabIndex = 4;
			this.m_perspectiveLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_perspectiveTrackBar
			// 
			this.m_perspectiveTrackBar.Location = new System.Drawing.Point(2, 40);
			this.m_perspectiveTrackBar.Maximum = 100;
			this.m_perspectiveTrackBar.Name = "m_perspectiveTrackBar";
			this.m_perspectiveTrackBar.Size = new System.Drawing.Size(144, 45);
			this.m_perspectiveTrackBar.TabIndex = 2;
			this.m_perspectiveTrackBar.TickFrequency = 10;
			this.m_perspectiveTrackBar.Scroll += new System.EventHandler(this.m_perspectiveTrackBar_Scroll);
			// 
			// m_zAxisReductionGroupBox
			// 
			this.m_zAxisReductionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.m_yAxisReduction,
																								   this.m_xAxisReduction,
																								   this.label11,
																								   this.label12});
			this.m_zAxisReductionGroupBox.Location = new System.Drawing.Point(164, 80);
			this.m_zAxisReductionGroupBox.Name = "m_zAxisReductionGroupBox";
			this.m_zAxisReductionGroupBox.ResizeChildren = false;
			this.m_zAxisReductionGroupBox.Size = new System.Drawing.Size(148, 88);
			this.m_zAxisReductionGroupBox.TabIndex = 38;
			this.m_zAxisReductionGroupBox.TabStop = false;
			this.m_zAxisReductionGroupBox.Text = "z-axis Reduction";
			// 
			// m_yAxisReduction
			// 
			this.m_yAxisReduction.Location = new System.Drawing.Point(96, 56);
			this.m_yAxisReduction.Name = "m_yAxisReduction";
			this.m_yAxisReduction.Size = new System.Drawing.Size(40, 20);
			this.m_yAxisReduction.TabIndex = 9;
			this.m_yAxisReduction.Text = "";
			this.m_yAxisReduction.TextChanged += new System.EventHandler(this.m_yAxisReduction_TextChanged);
			// 
			// m_xAxisReduction
			// 
			this.m_xAxisReduction.Location = new System.Drawing.Point(96, 32);
			this.m_xAxisReduction.Name = "m_xAxisReduction";
			this.m_xAxisReduction.Size = new System.Drawing.Size(40, 20);
			this.m_xAxisReduction.TabIndex = 8;
			this.m_xAxisReduction.Text = "";
			this.m_xAxisReduction.TextChanged += new System.EventHandler(this.m_xAxisReduction_TextChanged);
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.White;
			this.label11.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label11.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label11.Location = new System.Drawing.Point(8, 56);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(80, 16);
			this.label11.TabIndex = 7;
			this.label11.Text = "Along y-axis";
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.White;
			this.label12.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label12.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label12.Location = new System.Drawing.Point(8, 32);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(80, 16);
			this.label12.TabIndex = 6;
			this.label12.Text = "Along x-axis";
			// 
			// m_orientationGroupBox
			// 
			this.m_orientationGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.m_orientationComboBox});
			this.m_orientationGroupBox.Location = new System.Drawing.Point(164, 8);
			this.m_orientationGroupBox.Name = "m_orientationGroupBox";
			this.m_orientationGroupBox.ResizeChildren = true;
			this.m_orientationGroupBox.Size = new System.Drawing.Size(148, 56);
			this.m_orientationGroupBox.TabIndex = 34;
			this.m_orientationGroupBox.TabStop = false;
			this.m_orientationGroupBox.Text = "Orientation";
			// 
			// m_orientationComboBox
			// 
			this.m_orientationComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_orientationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_orientationComboBox.Location = new System.Drawing.Point(2, 26);
			this.m_orientationComboBox.Name = "m_orientationComboBox";
			this.m_orientationComboBox.Size = new System.Drawing.Size(144, 21);
			this.m_orientationComboBox.TabIndex = 0;
			this.m_orientationComboBox.SelectedIndexChanged += new System.EventHandler(this.m_orientationComboBox_SelectedIndexChanged);
			// 
			// m_renderingPrecisionGroupBox
			// 
			this.m_renderingPrecisionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									   this.label2,
																									   this.m_renderingPrecisionLabel,
																									   this.m_renderingPrecisionTrackBar,
																									   this.label1});
			this.m_renderingPrecisionGroupBox.Location = new System.Drawing.Point(8, 80);
			this.m_renderingPrecisionGroupBox.Name = "m_renderingPrecisionGroupBox";
			this.m_renderingPrecisionGroupBox.ResizeChildren = false;
			this.m_renderingPrecisionGroupBox.Size = new System.Drawing.Size(150, 88);
			this.m_renderingPrecisionGroupBox.TabIndex = 35;
			this.m_renderingPrecisionGroupBox.TabStop = false;
			this.m_renderingPrecisionGroupBox.Text = "Rendering Precision:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.White;
			this.label2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(204)), ((System.Byte)(204)), ((System.Byte)(204)));
			this.label2.Location = new System.Drawing.Point(112, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 11);
			this.label2.TabIndex = 6;
			this.label2.Text = "Fine";
			// 
			// m_renderingPrecisionLabel
			// 
			this.m_renderingPrecisionLabel.BackColor = System.Drawing.Color.Transparent;
			this.m_renderingPrecisionLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_renderingPrecisionLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_renderingPrecisionLabel.Location = new System.Drawing.Point(123, 6);
			this.m_renderingPrecisionLabel.Name = "m_renderingPrecisionLabel";
			this.m_renderingPrecisionLabel.Size = new System.Drawing.Size(27, 14);
			this.m_renderingPrecisionLabel.TabIndex = 4;
			// 
			// m_renderingPrecisionTrackBar
			// 
			this.m_renderingPrecisionTrackBar.Location = new System.Drawing.Point(2, 40);
			this.m_renderingPrecisionTrackBar.Maximum = 4;
			this.m_renderingPrecisionTrackBar.Name = "m_renderingPrecisionTrackBar";
			this.m_renderingPrecisionTrackBar.Size = new System.Drawing.Size(144, 45);
			this.m_renderingPrecisionTrackBar.TabIndex = 2;
			this.m_renderingPrecisionTrackBar.Value = 1;
			this.m_renderingPrecisionTrackBar.Scroll += new System.EventHandler(this.m_renderingPrecisionTrackBar_Scroll);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(204)), ((System.Byte)(204)), ((System.Byte)(204)));
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 11);
			this.label1.TabIndex = 5;
			this.label1.Text = "Coarse";
			// 
			// m_projectionKindGroupBox
			// 
			this.m_projectionKindGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.m_projectionKindComboBox});
			this.m_projectionKindGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_projectionKindGroupBox.Name = "m_projectionKindGroupBox";
			this.m_projectionKindGroupBox.ResizeChildren = true;
			this.m_projectionKindGroupBox.Size = new System.Drawing.Size(148, 56);
			this.m_projectionKindGroupBox.TabIndex = 33;
			this.m_projectionKindGroupBox.TabStop = false;
			this.m_projectionKindGroupBox.Text = "Projection Kind";
			// 
			// m_viewingDirectionGroupBox
			// 
			this.m_viewingDirectionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									 this.m_viewDirectionTextBoxes});
			this.m_viewingDirectionGroupBox.Location = new System.Drawing.Point(8, 184);
			this.m_viewingDirectionGroupBox.Name = "m_viewingDirectionGroupBox";
			this.m_viewingDirectionGroupBox.ResizeChildren = false;
			this.m_viewingDirectionGroupBox.Size = new System.Drawing.Size(304, 56);
			this.m_viewingDirectionGroupBox.TabIndex = 45;
			this.m_viewingDirectionGroupBox.TabStop = false;
			this.m_viewingDirectionGroupBox.Text = "Viewing Direction";
			// 
			// m_viewDirectionTextBoxes
			// 
			this.m_viewDirectionTextBoxes.BackColor = System.Drawing.Color.White;
			this.m_viewDirectionTextBoxes.Location = new System.Drawing.Point(8, 32);
			this.m_viewDirectionTextBoxes.Name = "m_viewDirectionTextBoxes";
			this.m_viewDirectionTextBoxes.Size = new System.Drawing.Size(178, 20);
			this.m_viewDirectionTextBoxes.TabIndex = 29;
			this.m_viewDirectionTextBoxes.Vector = new ComponentArt.Web.Visualization.Charting.Vector3D("(0,0,0)");
			this.m_viewDirectionTextBoxes.VectorChanged += new System.EventHandler(this.m_viewDirectionTextBoxes_VectorChanged);
			// 
			// WizardMappingDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.DefaultHint = "Drag the chart image to change the viewing direction. Set the projection kind, or" +
				"ientation, rendering precision, and perspective.";
			this.DefaultHintTitle = "3D Options";
			this.Name = "WizardMappingDialog";
			this.Size = new System.Drawing.Size(324, 312);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.m_perspectiveGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_perspectiveTrackBar)).EndInit();
			this.m_zAxisReductionGroupBox.ResumeLayout(false);
			this.m_orientationGroupBox.ResumeLayout(false);
			this.m_renderingPrecisionGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_renderingPrecisionTrackBar)).EndInit();
			this.m_projectionKindGroupBox.ResumeLayout(false);
			this.m_viewingDirectionGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_projectionKindComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.Mapping.Kind = (ProjectionKind)m_projectionKindComboBox.SelectedIndex;
			SetupOrientation();

			m_viewingDirectionGroupBox.Visible  = 
				(WinChart.Mapping.Kind == ProjectionKind.CentralProjection 
				|| WinChart.Mapping.Kind == ProjectionKind.ParallelProjection);

			m_lightsCheckBox.Visible = (WinChart.Mapping.Kind == ProjectionKind.TwoDimensional);
			m_zAxisReductionGroupBox.Visible = (WinChart.Mapping.Kind == ProjectionKind.Isometric);

			WinChart.Invalidate();
		}


		private void m_viewDirectionTextBoxes_VectorChanged(object sender, System.EventArgs e)
		{
			if (WinChart.View.ViewDirection != m_viewDirectionTextBoxes.Vector) 
			{
				WinChart.View.ViewDirection = m_viewDirectionTextBoxes.Vector;
				WinChart.Invalidate();
			}
		}

		private void m_renderingPrecisionTrackBar_Scroll(object sender, System.EventArgs e)
		{
			double x = (double)m_renderingPrecisionTrackBar.Value/(double)m_renderingPrecisionTrackBar.Maximum;

			double val = 0.5-x*0.4;

			m_renderingPrecisionLabel.Text = val.ToString("0.0", System.Globalization.NumberFormatInfo.CurrentInfo);
			m_renderingPrecisionLabel.Refresh();
			WinChart.RenderingPrecision = val;
			WinChart.Invalidate();

		}

		private void m_lightsCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.View.LightsOffOn2D = !m_lightsCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_perspectiveTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int val = m_perspectiveTrackBar.Value;

			m_perspectiveLabel.Text = val.ToString() + "%";
			m_perspectiveLabel.Refresh();
			WinChart.View.Perspective = val;
			WinChart.Invalidate();
		}

		private void m_orientationComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.CoordinateSystem.Orientation = (CoordinateSystemOrientation)Enum.Parse(typeof(CoordinateSystemOrientation), m_orientationComboBox.Text);
			WinChart.Invalidate();
		}

		private void m_xAxisReduction_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Mapping.XReductionOfZAxis = double.Parse(m_xAxisReduction.Text);
			WinChart.Invalidate();
		}

		private void m_yAxisReduction_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Mapping.YReductionOfZAxis = double.Parse(m_yAxisReduction.Text);
			WinChart.Invalidate();
		}
	}
}
