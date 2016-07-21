using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal class TabSelectEventArgs: EventArgs
	{
		public TabSelectEventArgs(object relatedObject)
		{
			this.relatedObject = relatedObject;
		}

		private object relatedObject;
		public object RelatedObject
		{
			get 
			{
				return relatedObject;
			}
		}
	}

	/// <summary>
	/// Summary description for RangesTab.
	/// </summary>
	internal class RangesTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.TabPage tabPage1;
		private ComponentArt.WinUI.TabPage tabPage2;
		private ComponentArt.WinUI.TabPage tabPage3;
		private ComponentArt.WinUI.TabPage tabPage4;
		private ComponentArt.WinUI.TabPage tabPage5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox maxValueEditBox;
		private System.Windows.Forms.TextBox minValueEditBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.TextBox annotationScaleFactorTextBox;
		private System.Windows.Forms.TextBox annotationStepTextBox;
		private ComponentArt.WinUI.ComboBox annotationKindDropdown;
		private System.Windows.Forms.TextBox annotationFormatTextBox;
		private ComponentArt.Web.Visualization.Gauges.SliderControl colorStripStartWidthSlider;
		private ComponentArt.Web.Visualization.Gauges.SliderControl colorStripEndWidthSlider;
		private ComponentArt.WinUI.ComboBox colorStripPositionDropdown;
		private ComponentArt.Web.Visualization.Gauges.SliderControl colorStripOffsetSlider;
		private System.Windows.Forms.TextBox majorTMMaxValueTextBox;
		private System.Windows.Forms.TextBox majorTMMinValueTextBox;
		private System.Windows.Forms.CheckBox majorTMEnabledCheckBox;
		private ComponentArt.Web.Visualization.Gauges.SliderControl majorTMOffsetSlider;
		private ComponentArt.WinUI.ComboBox majorTMStyleDropdown;
		private System.Windows.Forms.TextBox majorTMSizeXTextBox;
		private System.Windows.Forms.TextBox majorTMSizeYTextBox;
		private System.Windows.Forms.TextBox majorTMStepTextBox;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private ComponentArt.WinUI.TabControl tabControl;
		private System.Windows.Forms.TextBox minorTMStepTextBox;
		private System.Windows.Forms.TextBox minorTMSizeYTextBox;
		private System.Windows.Forms.TextBox minorTMSizeXTextBox;
		private ComponentArt.WinUI.ComboBox minorTMStyleDropdown;
		private ComponentArt.Web.Visualization.Gauges.SliderControl minorTMOffsetSlider;
		private System.Windows.Forms.CheckBox minorTMEnabledCheckBox;
		private System.Windows.Forms.TextBox minorTMMaxValueTextBox;
		private System.Windows.Forms.TextBox minorTMMinValueTextBox;
		private ComponentArt.WinUI.ComboBox annotationStyleDropdown;
		private ComponentArt.Web.Visualization.Gauges.Core.WinUI.MultiColorDialog multiColorDialog1;
		private System.Windows.Forms.CheckBox visibleCheckBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		private Range m_range = null;
		PropertyDescriptorCollection m_rangeDesc = null;
		PropertyDescriptorCollection m_majorTickMarksDesc = null;	
		PropertyDescriptorCollection m_minorTickMarksDesc = null;

		public Range Range
		{
			set
			{
				m_range = value;
				m_rangeDesc = TypeDescriptor.GetProperties(m_range);
				m_majorTickMarksDesc = TypeDescriptor.GetProperties(m_range.MajorTickMarks);	
				m_minorTickMarksDesc = TypeDescriptor.GetProperties(m_range.MinorTickMarks);
				LoadDataFromRange();
				if (this.tabControl.SelectedIndex == 2)
					this.tabControl.SelectedIndex = 0;
				EnableAnnotationControls();

				m_annotation = m_range.MainAnnotation;
				if (m_annotation != null)
					LoadDataFromAnnotation();
			}
			get
			{
				return m_range;
			}
		}

		private Annotation m_annotation = null;
		public Annotation Annotation
		{
			set
			{
				m_annotation = value;
				if (m_annotation != null)
					LoadDataFromAnnotation();
				this.tabControl.SelectedIndex = 2;
				EnableAnnotationControls();
			}
			get
			{
				return m_annotation;
			}
		}

		public delegate void TabEventHandler (object source);
		public delegate void TabEventHandlerWithArgs (object source, TabSelectEventArgs e);
		public event TabEventHandler Changed;
		public event TabEventHandlerWithArgs TabSelected;

		public RangesTab()
		{
			// This call is required by the Windows.Forms Form Designer.
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			ComponentArt.Web.Visualization.Gauges.MultiColor multiColor1 = new ComponentArt.Web.Visualization.Gauges.MultiColor();
			this.tabControl = new ComponentArt.WinUI.TabControl();
			this.tabPage1 = new ComponentArt.WinUI.TabPage();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.maxValueEditBox = new System.Windows.Forms.TextBox();
			this.minValueEditBox = new System.Windows.Forms.TextBox();
			this.visibleCheckBox = new System.Windows.Forms.CheckBox();
			this.tabPage3 = new ComponentArt.WinUI.TabPage();
			this.multiColorDialog1 = new ComponentArt.Web.Visualization.Gauges.Core.WinUI.MultiColorDialog();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.colorStripOffsetSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.colorStripPositionDropdown = new ComponentArt.WinUI.ComboBox();
			this.colorStripEndWidthSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.colorStripStartWidthSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.tabPage2 = new ComponentArt.WinUI.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.annotationScaleFactorTextBox = new System.Windows.Forms.TextBox();
			this.annotationStepTextBox = new System.Windows.Forms.TextBox();
			this.annotationStyleDropdown = new ComponentArt.WinUI.ComboBox();
			this.annotationKindDropdown = new ComponentArt.WinUI.ComboBox();
			this.annotationFormatTextBox = new System.Windows.Forms.TextBox();
			this.tabPage4 = new ComponentArt.WinUI.TabPage();
			this.majorTMStepTextBox = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.majorTMSizeYTextBox = new System.Windows.Forms.TextBox();
			this.majorTMSizeXTextBox = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.majorTMStyleDropdown = new ComponentArt.WinUI.ComboBox();
			this.majorTMOffsetSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.majorTMEnabledCheckBox = new System.Windows.Forms.CheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.majorTMMaxValueTextBox = new System.Windows.Forms.TextBox();
			this.majorTMMinValueTextBox = new System.Windows.Forms.TextBox();
			this.tabPage5 = new ComponentArt.WinUI.TabPage();
			this.minorTMStepTextBox = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.minorTMSizeYTextBox = new System.Windows.Forms.TextBox();
			this.minorTMSizeXTextBox = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.minorTMStyleDropdown = new ComponentArt.WinUI.ComboBox();
			this.minorTMOffsetSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.minorTMEnabledCheckBox = new System.Windows.Forms.CheckBox();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.minorTMMaxValueTextBox = new System.Windows.Forms.TextBox();
			this.minorTMMinValueTextBox = new System.Windows.Forms.TextBox();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.ColorBehind = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.tabPage1,
																					 this.tabPage3,
																					 this.tabPage2,
																					 this.tabPage4,
																					 this.tabPage5});
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.Size = new System.Drawing.Size(256, 328);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl.TabIndex = 10;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_TabChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label3,
																				   this.label1,
																				   this.maxValueEditBox,
																				   this.minValueEditBox,
																				   this.visibleCheckBox});
			this.tabPage1.Location = new System.Drawing.Point(2, 47);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 278);
			this.tabPage1.TabIndex = 11;
			this.tabPage1.Text = "Properties";
			// 
			// visibleCheckBox
			// 
			this.visibleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.visibleCheckBox.Location = new System.Drawing.Point(24, 18);
			this.visibleCheckBox.Name = "visibleCheckBox";
			this.visibleCheckBox.Size = new System.Drawing.Size(88, 24);
			this.visibleCheckBox.TabIndex = 22;
			this.visibleCheckBox.Text = "Visible";
			this.visibleCheckBox.CheckedChanged += new System.EventHandler(this.visibleCheckBox_Changed);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(132, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Max Value";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 68);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Min Value";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// maxValueEditBox
			// 
			this.maxValueEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.maxValueEditBox.Location = new System.Drawing.Point(196, 66);
			this.maxValueEditBox.Name = "maxValueEditBox";
			this.maxValueEditBox.Size = new System.Drawing.Size(32, 21);
			this.maxValueEditBox.TabIndex = 13;
			this.maxValueEditBox.Text = "100";
			this.maxValueEditBox.Leave += new System.EventHandler(this.maxValueEditBox_Leave);
			// 
			// minValueEditBox
			// 
			this.minValueEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minValueEditBox.Location = new System.Drawing.Point(76, 66);
			this.minValueEditBox.Name = "minValueEditBox";
			this.minValueEditBox.Size = new System.Drawing.Size(32, 21);
			this.minValueEditBox.TabIndex = 12;
			this.minValueEditBox.Text = "0";
			this.minValueEditBox.Leave += new System.EventHandler(this.minValueEditBox_Leave);
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.Color.White;
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.multiColorDialog1,
																				   this.label12,
																				   this.label11,
																				   this.label10,
																				   this.label9,
																				   this.label8,
																				   this.colorStripOffsetSlider,
																				   this.colorStripPositionDropdown,
																				   this.colorStripEndWidthSlider,
																				   this.colorStripStartWidthSlider});
			this.tabPage3.Location = new System.Drawing.Point(2, 47);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(252, 278);
			this.tabPage3.TabIndex = 31;
			this.tabPage3.Text = "Color Strip";
			this.tabPage3.Visible = false;
			// 
			// multiColorDialog1
			// 
			this.multiColorDialog1.BackColor = System.Drawing.Color.White;
			this.multiColorDialog1.Location = new System.Drawing.Point(60, 184);
			multiColor1.ColorStops = new ComponentArt.Web.Visualization.Gauges.ColorStopCollection("Empty");
			this.multiColorDialog1.MultiColor = multiColor1;
			this.multiColorDialog1.Name = "multiColorDialog1";
			this.multiColorDialog1.Size = new System.Drawing.Size(182, 88);
			this.multiColorDialog1.TabIndex = 36;
			this.multiColorDialog1.OnChanged += new System.EventHandler(this.multiColorDialog1_OnChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(16, 188);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(48, 16);
			this.label12.TabIndex = 37;
			this.label12.Text = "Color";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(16, 148);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 16);
			this.label11.TabIndex = 38;
			this.label11.Text = "Offset";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 108);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(48, 16);
			this.label10.TabIndex = 39;
			this.label10.Text = "Position";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 68);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(74, 16);
			this.label9.TabIndex = 40;
			this.label9.Text = "End Width";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 28);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(74, 16);
			this.label8.TabIndex = 41;
			this.label8.Text = "Start Width";
			// 
			// colorStripOffsetSlider
			// 
			this.colorStripOffsetSlider.Location = new System.Drawing.Point(96, 132);
			this.colorStripOffsetSlider.Name = "colorStripOffsetSlider";
			this.colorStripOffsetSlider.Size = new System.Drawing.Size(104, 32);
			this.colorStripOffsetSlider.TabIndex = 35;
			this.colorStripOffsetSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.colorStripOffsetSlider_Changed);
			// 
			// colorStripPositionDropdown
			// 
			this.colorStripPositionDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.colorStripPositionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colorStripPositionDropdown.Location = new System.Drawing.Point(96, 104);
			this.colorStripPositionDropdown.Name = "colorStripPositionDropdown";
			this.colorStripPositionDropdown.Size = new System.Drawing.Size(104, 22);
			this.colorStripPositionDropdown.TabIndex = 34;
			this.colorStripPositionDropdown.SelectedIndexChanged += new System.EventHandler(this.colorStripPositionDropdown_Changed);
			// 
			// colorStripEndWidthSlider
			// 
			this.colorStripEndWidthSlider.Location = new System.Drawing.Point(96, 52);
			this.colorStripEndWidthSlider.Name = "colorStripEndWidthSlider";
			this.colorStripEndWidthSlider.Size = new System.Drawing.Size(104, 32);
			this.colorStripEndWidthSlider.TabIndex = 33;
			this.colorStripEndWidthSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.colorStripEndWidthSlider_Changed);
			// 
			// colorStripStartWidthSlider
			// 
			this.colorStripStartWidthSlider.Location = new System.Drawing.Point(96, 12);
			this.colorStripStartWidthSlider.Name = "colorStripStartWidthSlider";
			this.colorStripStartWidthSlider.Size = new System.Drawing.Size(104, 32);
			this.colorStripStartWidthSlider.TabIndex = 32;
			this.colorStripStartWidthSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.colorStripStartWidthSlider_Changed);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.White;
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label7,
																				   this.label6,
																				   this.label5,
																				   this.label4,
																				   this.label2,
																				   this.annotationScaleFactorTextBox,
																				   this.annotationStepTextBox,
																				   this.annotationStyleDropdown,
																				   this.annotationKindDropdown,
																				   this.annotationFormatTextBox});
			this.tabPage2.Location = new System.Drawing.Point(2, 47);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(252, 278);
			this.tabPage2.TabIndex = 21;
			this.tabPage2.Text = "Annotations";
			this.tabPage2.Visible = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(112, 148);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "Scale Factor";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 148);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "Step";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 108);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Style";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 68);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Format";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Kind";
			// 
			// annotationScaleFactorTextBox
			// 
			this.annotationScaleFactorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.annotationScaleFactorTextBox.Location = new System.Drawing.Point(192, 144);
			this.annotationScaleFactorTextBox.Name = "annotationScaleFactorTextBox";
			this.annotationScaleFactorTextBox.Size = new System.Drawing.Size(32, 21);
			this.annotationScaleFactorTextBox.TabIndex = 26;
			this.annotationScaleFactorTextBox.Text = "";
			this.annotationScaleFactorTextBox.Leave += new System.EventHandler(this.annotationScaleFactorTextBox_Leave);
			// 
			// annotationStepTextBox
			// 
			this.annotationStepTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.annotationStepTextBox.Location = new System.Drawing.Point(64, 144);
			this.annotationStepTextBox.Name = "annotationStepTextBox";
			this.annotationStepTextBox.Size = new System.Drawing.Size(32, 21);
			this.annotationStepTextBox.TabIndex = 25;
			this.annotationStepTextBox.Text = "";
			this.annotationStepTextBox.Leave += new System.EventHandler(this.annotationStepTextBox_Leave);
			// 
			// annotationStyleDropdown
			// 
			this.annotationStyleDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.annotationStyleDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.annotationStyleDropdown.Location = new System.Drawing.Point(96, 104);
			this.annotationStyleDropdown.Name = "annotationStyleDropdown";
			this.annotationStyleDropdown.Size = new System.Drawing.Size(104, 22);
			this.annotationStyleDropdown.TabIndex = 24;
			this.annotationStyleDropdown.SelectedIndexChanged += new System.EventHandler(this.annotationStyleDropdown_Changed);
			// 
			// annotationKindDropdown
			// 
			this.annotationKindDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.annotationKindDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.annotationKindDropdown.Location = new System.Drawing.Point(96, 24);
			this.annotationKindDropdown.Name = "annotationKindDropdown";
			this.annotationKindDropdown.Size = new System.Drawing.Size(104, 22);
			this.annotationKindDropdown.TabIndex = 22;
			this.annotationKindDropdown.SelectedIndexChanged += new System.EventHandler(this.annotationKindDropdown_Changed);
			// 
			// annotationFormatTextBox
			// 
			this.annotationFormatTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.annotationFormatTextBox.Location = new System.Drawing.Point(96, 64);
			this.annotationFormatTextBox.Name = "annotationFormatTextBox";
			this.annotationFormatTextBox.Size = new System.Drawing.Size(104, 21);
			this.annotationFormatTextBox.TabIndex = 23;
			this.annotationFormatTextBox.Text = "";
			this.annotationFormatTextBox.Leave += new System.EventHandler(this.annotationFormatTextBox_Leave);
			// 
			// tabPage4
			// 
			this.tabPage4.BackColor = System.Drawing.Color.White;
			this.tabPage4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.majorTMStepTextBox,
																				   this.label21,
																				   this.majorTMSizeYTextBox,
																				   this.majorTMSizeXTextBox,
																				   this.label19,
																				   this.label18,
																				   this.label17,
																				   this.label16,
																				   this.label15,
																				   this.majorTMStyleDropdown,
																				   this.majorTMOffsetSlider,
																				   this.majorTMEnabledCheckBox,
																				   this.label13,
																				   this.label14,
																				   this.majorTMMaxValueTextBox,
																				   this.majorTMMinValueTextBox});
			this.tabPage4.Location = new System.Drawing.Point(2, 47);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(252, 278);
			this.tabPage4.TabIndex = 41;
			this.tabPage4.Text = "Major Tick Marks";
			this.tabPage4.Visible = false;
			// 
			// majorTMStepTextBox
			// 
			this.majorTMStepTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.majorTMStepTextBox.Location = new System.Drawing.Point(200, 184);
			this.majorTMStepTextBox.Name = "majorTMStepTextBox";
			this.majorTMStepTextBox.Size = new System.Drawing.Size(32, 21);
			this.majorTMStepTextBox.TabIndex = 49;
			this.majorTMStepTextBox.Text = "";
			this.majorTMStepTextBox.Leave += new System.EventHandler(this.majorTMStepTextBox_Leave);
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(168, 188);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(32, 16);
			this.label21.TabIndex = 50;
			this.label21.Text = "Step";
			// 
			// majorTMSizeYTextBox
			// 
			this.majorTMSizeYTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.majorTMSizeYTextBox.Location = new System.Drawing.Point(120, 184);
			this.majorTMSizeYTextBox.Name = "majorTMSizeYTextBox";
			this.majorTMSizeYTextBox.Size = new System.Drawing.Size(32, 21);
			this.majorTMSizeYTextBox.TabIndex = 48;
			this.majorTMSizeYTextBox.Text = "";
			this.majorTMSizeYTextBox.Leave += new System.EventHandler(this.majorTMSizeYTextBox_Leave);
			// 
			// majorTMSizeXTextBox
			// 
			this.majorTMSizeXTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.majorTMSizeXTextBox.Location = new System.Drawing.Point(64, 184);
			this.majorTMSizeXTextBox.Name = "majorTMSizeXTextBox";
			this.majorTMSizeXTextBox.Size = new System.Drawing.Size(32, 21);
			this.majorTMSizeXTextBox.TabIndex = 47;
			this.majorTMSizeXTextBox.Text = "";
			this.majorTMSizeXTextBox.Leave += new System.EventHandler(this.majorTMSizeXTextBox_Leave);
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(104, 188);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(8, 16);
			this.label19.TabIndex = 51;
			this.label19.Text = "y";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(48, 188);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(8, 16);
			this.label18.TabIndex = 52;
			this.label18.Text = "x";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(16, 188);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(32, 16);
			this.label17.TabIndex = 53;
			this.label17.Text = "Size";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(16, 148);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(56, 16);
			this.label16.TabIndex = 54;
			this.label16.Text = "Offset";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(16, 108);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(48, 16);
			this.label15.TabIndex = 55;
			this.label15.Text = "Style";
			// 
			// majorTMStyleDropdown
			// 
			this.majorTMStyleDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.majorTMStyleDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.majorTMStyleDropdown.Location = new System.Drawing.Point(96, 104);
			this.majorTMStyleDropdown.Name = "majorTMStyleDropdown";
			this.majorTMStyleDropdown.Size = new System.Drawing.Size(104, 22);
			this.majorTMStyleDropdown.TabIndex = 45;
			this.majorTMStyleDropdown.SelectedIndexChanged += new System.EventHandler(this.majorTMStyleDropdown_SelectedIndexChanged);
			// 
			// majorTMOffsetSlider
			// 
			this.majorTMOffsetSlider.Location = new System.Drawing.Point(96, 132);
			this.majorTMOffsetSlider.Name = "majorTMOffsetSlider";
			this.majorTMOffsetSlider.Size = new System.Drawing.Size(104, 32);
			this.majorTMOffsetSlider.TabIndex = 46;
			this.majorTMOffsetSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.majorTMOffsetSlider_ValueChanged);
			// 
			// majorTMEnabledCheckBox
			// 
			this.majorTMEnabledCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.majorTMEnabledCheckBox.Location = new System.Drawing.Point(16, 24);
			this.majorTMEnabledCheckBox.Name = "majorTMEnabledCheckBox";
			this.majorTMEnabledCheckBox.Size = new System.Drawing.Size(72, 24);
			this.majorTMEnabledCheckBox.TabIndex = 42;
			this.majorTMEnabledCheckBox.Text = "Visible";
			this.majorTMEnabledCheckBox.CheckedChanged += new System.EventHandler(this.majorTMEnabledCheckBox_Changed);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(134, 68);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 16);
			this.label13.TabIndex = 56;
			this.label13.Text = "Max Value";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(18, 68);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(60, 16);
			this.label14.TabIndex = 57;
			this.label14.Text = "Min Value";
			// 
			// majorTMMaxValueTextBox
			// 
			this.majorTMMaxValueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.majorTMMaxValueTextBox.Location = new System.Drawing.Point(198, 64);
			this.majorTMMaxValueTextBox.Name = "majorTMMaxValueTextBox";
			this.majorTMMaxValueTextBox.Size = new System.Drawing.Size(32, 21);
			this.majorTMMaxValueTextBox.TabIndex = 44;
			this.majorTMMaxValueTextBox.Text = "100";
			this.majorTMMaxValueTextBox.Leave += new System.EventHandler(this.majorTMMaxValueTextBox_Leave);
			// 
			// majorTMMinValueTextBox
			// 
			this.majorTMMinValueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.majorTMMinValueTextBox.Location = new System.Drawing.Point(78, 64);
			this.majorTMMinValueTextBox.Name = "majorTMMinValueTextBox";
			this.majorTMMinValueTextBox.Size = new System.Drawing.Size(32, 21);
			this.majorTMMinValueTextBox.TabIndex = 43;
			this.majorTMMinValueTextBox.Text = "0";
			this.majorTMMinValueTextBox.Leave += new System.EventHandler(this.majorTMMinValueTextBox_Leave);
			// 
			// tabPage5
			// 
			this.tabPage5.BackColor = System.Drawing.Color.White;
			this.tabPage5.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.minorTMStepTextBox,
																				   this.label20,
																				   this.minorTMSizeYTextBox,
																				   this.minorTMSizeXTextBox,
																				   this.label22,
																				   this.label23,
																				   this.label24,
																				   this.label25,
																				   this.label26,
																				   this.minorTMStyleDropdown,
																				   this.minorTMOffsetSlider,
																				   this.minorTMEnabledCheckBox,
																				   this.label27,
																				   this.label28,
																				   this.minorTMMaxValueTextBox,
																				   this.minorTMMinValueTextBox});
			this.tabPage5.Location = new System.Drawing.Point(2, 47);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(252, 278);
			this.tabPage5.TabIndex = 51;
			this.tabPage5.Text = "Minor Tick Marks";
			this.tabPage5.Visible = false;
			// 
			// minorTMStepTextBox
			// 
			this.minorTMStepTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minorTMStepTextBox.Location = new System.Drawing.Point(200, 184);
			this.minorTMStepTextBox.Name = "minorTMStepTextBox";
			this.minorTMStepTextBox.Size = new System.Drawing.Size(32, 21);
			this.minorTMStepTextBox.TabIndex = 59;
			this.minorTMStepTextBox.Text = "";
			this.minorTMStepTextBox.Leave += new System.EventHandler(this.minorTMStepTextBox_Leave);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(168, 188);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(32, 16);
			this.label20.TabIndex = 60;
			this.label20.Text = "Step";
			// 
			// minorTMSizeYTextBox
			// 
			this.minorTMSizeYTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minorTMSizeYTextBox.Location = new System.Drawing.Point(120, 184);
			this.minorTMSizeYTextBox.Name = "minorTMSizeYTextBox";
			this.minorTMSizeYTextBox.Size = new System.Drawing.Size(32, 21);
			this.minorTMSizeYTextBox.TabIndex = 58;
			this.minorTMSizeYTextBox.Text = "";
			this.minorTMSizeYTextBox.Leave += new System.EventHandler(this.minorTMSizeYTextBox_Leave);
			// 
			// minorTMSizeXTextBox
			// 
			this.minorTMSizeXTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minorTMSizeXTextBox.Location = new System.Drawing.Point(64, 184);
			this.minorTMSizeXTextBox.Name = "minorTMSizeXTextBox";
			this.minorTMSizeXTextBox.Size = new System.Drawing.Size(32, 21);
			this.minorTMSizeXTextBox.TabIndex = 57;
			this.minorTMSizeXTextBox.Text = "";
			this.minorTMSizeXTextBox.Leave += new System.EventHandler(this.minorTMSizeXTextBox_Leave);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(104, 188);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(8, 16);
			this.label22.TabIndex = 61;
			this.label22.Text = "y";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(48, 188);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(8, 16);
			this.label23.TabIndex = 62;
			this.label23.Text = "x";
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(16, 188);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(32, 16);
			this.label24.TabIndex = 63;
			this.label24.Text = "Size";
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(16, 148);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(56, 16);
			this.label25.TabIndex = 64;
			this.label25.Text = "Offset";
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(16, 108);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(48, 16);
			this.label26.TabIndex = 65;
			this.label26.Text = "Style";
			// 
			// minorTMStyleDropdown
			// 
			this.minorTMStyleDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.minorTMStyleDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.minorTMStyleDropdown.Location = new System.Drawing.Point(96, 104);
			this.minorTMStyleDropdown.Name = "minorTMStyleDropdown";
			this.minorTMStyleDropdown.Size = new System.Drawing.Size(104, 22);
			this.minorTMStyleDropdown.TabIndex = 55;
			this.minorTMStyleDropdown.SelectedIndexChanged += new System.EventHandler(this.minorTMStyleDropdown_SelectedIndexChanged);
			// 
			// minorTMOffsetSlider
			// 
			this.minorTMOffsetSlider.Location = new System.Drawing.Point(96, 132);
			this.minorTMOffsetSlider.Name = "minorTMOffsetSlider";
			this.minorTMOffsetSlider.Size = new System.Drawing.Size(104, 32);
			this.minorTMOffsetSlider.TabIndex = 56;
			this.minorTMOffsetSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.minorTMOffsetSlider_ValueChanged);
			// 
			// minorTMEnabledCheckBox
			// 
			this.minorTMEnabledCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.minorTMEnabledCheckBox.Location = new System.Drawing.Point(16, 24);
			this.minorTMEnabledCheckBox.Name = "minorTMEnabledCheckBox";
			this.minorTMEnabledCheckBox.Size = new System.Drawing.Size(72, 24);
			this.minorTMEnabledCheckBox.TabIndex = 52;
			this.minorTMEnabledCheckBox.Text = "Visible";
			this.minorTMEnabledCheckBox.CheckedChanged += new System.EventHandler(this.minorTMEnabledCheckBox_CheckedChanged);
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(134, 68);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(64, 16);
			this.label27.TabIndex = 66;
			this.label27.Text = "Max Value";
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(18, 68);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(60, 16);
			this.label28.TabIndex = 67;
			this.label28.Text = "Min Value";
			// 
			// minorTMMaxValueTextBox
			// 
			this.minorTMMaxValueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minorTMMaxValueTextBox.Location = new System.Drawing.Point(198, 64);
			this.minorTMMaxValueTextBox.Name = "minorTMMaxValueTextBox";
			this.minorTMMaxValueTextBox.Size = new System.Drawing.Size(32, 21);
			this.minorTMMaxValueTextBox.TabIndex = 54;
			this.minorTMMaxValueTextBox.Text = "100";
			this.minorTMMaxValueTextBox.Leave += new System.EventHandler(this.minorTMMaxValueTextBox_Leave);
			// 
			// minorTMMinValueTextBox
			// 
			this.minorTMMinValueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minorTMMinValueTextBox.Location = new System.Drawing.Point(78, 64);
			this.minorTMMinValueTextBox.Name = "minorTMMinValueTextBox";
			this.minorTMMinValueTextBox.Size = new System.Drawing.Size(32, 21);
			this.minorTMMinValueTextBox.TabIndex = 53;
			this.minorTMMinValueTextBox.Text = "0";
			this.minorTMMinValueTextBox.Leave += new System.EventHandler(this.minorTMMinValueTextBox_Leave);
			// 
			// RangesTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl});
			//this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "RangesTab";
			this.Size = new System.Drawing.Size(256, 328);
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadDataFromRange()
		{
			// Properties Tab
			this.visibleCheckBox.Checked = m_range.Visible;
			this.maxValueEditBox.Text = this.m_rangeDesc["MaxValue"].Converter.ConvertToString(m_range.MaxValue);
			this.minValueEditBox.Text = this.m_rangeDesc["MinValue"].Converter.ConvertToString(m_range.MinValue);
			
			// Color Strip Tab
			this.colorStripOffsetSlider.SliderValue = new SliderValue(-50, 50, m_range.RangeLayout.Offset, 1);
			this.colorStripPositionDropdown.Items.Clear();
			foreach (string rsk in Enum.GetNames(typeof(RangeSideKind)))
			{
				this.colorStripPositionDropdown.Items.Add(rsk);
			}
			this.colorStripPositionDropdown.SelectedItem = m_range.RangeLayout.SideKind.ToString();
			this.colorStripEndWidthSlider.SliderValue = new SliderValue(0, 30, m_range.RangeLayout.EndWidth, 0.5);
			this.colorStripStartWidthSlider.SliderValue = new SliderValue(0, 30, m_range.RangeLayout.StartWidth, 0.5);

			this.multiColorDialog1.MultiColor = m_range.Color;
			
			// Major TickMarks Tab
			this.majorTMStepTextBox.Text = this.m_majorTickMarksDesc["Step"].Converter.ConvertToString(m_range.MajorTickMarks.Step);
			this.majorTMSizeYTextBox.Text = m_range.MajorTickMarks.Size.Height.ToString();
			this.majorTMSizeXTextBox.Text = m_range.MajorTickMarks.Size.Width.ToString();
			this.majorTMStyleDropdown.Items.Clear();
			foreach (MarkerStyle ms in m_range.Gauge.MarkerStyles)
			{
				this.majorTMStyleDropdown.Items.Add(ms.Name);
			}
			this.majorTMStyleDropdown.SelectedItem = m_range.MajorTickMarks.Style.Name;
			
//			this.majorTMOffsetSlider.SliderValue = new SliderValue(-20, 20, (double)m_majorTickMarksDesc["Offset"].Converter.ConvertTo(m_range.MajorTickMarks.Offset, typeof(double)), 1);
			if (double.IsNaN(m_range.MajorTickMarks.Offset))
				this.majorTMOffsetSlider.SliderValue = new SliderValue(-20, 20, m_range.MajorTickMarks.EffectiveOffset, 1);
			else
				this.majorTMOffsetSlider.SliderValue = new SliderValue(-20, 20, m_range.MajorTickMarks.Offset, 1);
			
			this.majorTMEnabledCheckBox.Checked = m_range.MajorTickMarks.Visible;

			this.majorTMMaxValueTextBox.Text = this.m_majorTickMarksDesc["MaxValue"].Converter.ConvertToString(m_range.MajorTickMarks.MaxValue);
			this.majorTMMinValueTextBox.Text = this.m_majorTickMarksDesc["MinValue"].Converter.ConvertToString(m_range.MajorTickMarks.MinValue);

			
			// Minor TickMarks Tab
			this.minorTMStepTextBox.Text = this.m_minorTickMarksDesc["Step"].Converter.ConvertToString(m_range.MinorTickMarks.Step);	
			
			this.minorTMSizeYTextBox.Text = m_range.MinorTickMarks.Size.Height.ToString();
			this.minorTMSizeXTextBox.Text = m_range.MinorTickMarks.Size.Width.ToString();
			this.minorTMStyleDropdown.Items.Clear();
			foreach (MarkerStyle ms in m_range.Gauge.MarkerStyles)
			{
				this.minorTMStyleDropdown.Items.Add(ms.Name);
			}
			this.minorTMStyleDropdown.SelectedItem = m_range.MinorTickMarks.Style.Name;

//			this.minorTMOffsetSlider.SliderValue = new SliderValue(-20, 20, (double)m_minorTickMarksDesc["Offset"].Converter.ConvertTo(m_range.MinorTickMarks.Offset, typeof(double)), 1);		
			if (double.IsNaN(m_range.MinorTickMarks.Offset))
				this.minorTMOffsetSlider.SliderValue = new SliderValue(-20, 20, m_range.MinorTickMarks.EffectiveOffset, 1);
			else
				this.minorTMOffsetSlider.SliderValue = new SliderValue(-20, 20, m_range.MinorTickMarks.Offset, 1);			
			this.minorTMEnabledCheckBox.Checked = m_range.MinorTickMarks.Visible;

			this.minorTMMaxValueTextBox.Text = this.m_minorTickMarksDesc["MaxValue"].Converter.ConvertToString(m_range.MinorTickMarks.MaxValue);
			this.minorTMMinValueTextBox.Text = this.m_minorTickMarksDesc["MinValue"].Converter.ConvertToString(m_range.MinorTickMarks.MinValue);
		}

		private void LoadDataFromAnnotation()
		{
			// Annotations Tab
			this.annotationScaleFactorTextBox.Text = m_annotation.ScaleFactor.ToString();
			this.annotationStepTextBox.Text = m_annotation.Step.ToString();
			this.annotationStyleDropdown.Items.Clear();
			foreach (ScaleAnnotationStyle style in m_annotation.Range.Gauge.ScaleAnnotationStyles)
			{
				this.annotationStyleDropdown.Items.Add(style.Name);
			}
			this.annotationStyleDropdown.SelectedItem = m_annotation.ScaleAnnotationStyle.Name;
			this.annotationKindDropdown.Items.Clear();
			foreach (string avsk in Enum.GetNames(typeof(AnnotationValueSetKind)))
			{
				this.annotationKindDropdown.Items.Add(avsk);
			}
			this.annotationKindDropdown.SelectedItem = m_annotation.AnnotationValueSetKind.ToString();
			this.annotationFormatTextBox.Text = m_annotation.Format;
		}
		
		private void EnableAnnotationControls()
		{
			if (this.tabControl.SelectedIndex == 0)
			{
				this.tabPage1.Visible = true;
				this.tabPage2.Visible = false;
				this.tabPage3.Visible = false;
				this.tabPage4.Visible = false;
				this.tabPage5.Visible = false;
			}
			else if (this.tabControl.SelectedIndex == 2)
			{
				this.tabPage1.Visible = false;
				this.tabPage2.Visible = true;
				this.tabPage3.Visible = false;
				this.tabPage4.Visible = false;
				this.tabPage5.Visible = false;
			}

			this.annotationScaleFactorTextBox.Enabled = (m_annotation != null);
			this.annotationStepTextBox.Enabled = (m_annotation != null);
			this.annotationStyleDropdown.Enabled = (m_annotation != null);
			this.annotationKindDropdown.Enabled = (m_annotation != null);
			this.annotationFormatTextBox.Enabled = (m_annotation != null);
		}

		private void tabControl_TabChanged(object sender, System.EventArgs e)
		{
			TabSelectEventArgs args;
			if (TabSelected != null)
			{
				// if Annotation tab selected
				if (tabControl.SelectedIndex == 2)
					args = new TabSelectEventArgs(m_annotation);
				// any other tab selected
				else
					args = new TabSelectEventArgs(m_range);
				TabSelected(this, args);
			}

		}

		private void DoChangedEvent()
		{
			if (Changed != null)
			{
				Changed(this);
			}
		}
		
		private void visibleCheckBox_Changed(object sender, System.EventArgs e)
		{
			m_range.Visible = this.visibleCheckBox.Checked;
			DoChangedEvent();
		}

		private void minValueEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MinValue = (double)m_rangeDesc["MinValue"].Converter.ConvertFromString(minValueEditBox.Text);
			}
			catch(Exception)
			{
				minValueEditBox.Text = m_rangeDesc["MinValue"].Converter.ConvertToString(m_range.MinValue);
			}
			DoChangedEvent();
		}

		private void maxValueEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MaxValue = (double)m_rangeDesc["MaxValue"].Converter.ConvertFromString(maxValueEditBox.Text);
			}
			catch(Exception)
			{
				maxValueEditBox.Text = m_rangeDesc["MaxValue"].Converter.ConvertToString(m_range.MaxValue);
			}
			DoChangedEvent();		
		}

		private void colorStripStartWidthSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_range.RangeLayout.StartWidth = this.colorStripStartWidthSlider.SliderValue.Value;
			DoChangedEvent();		
		}

		private void colorStripEndWidthSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_range.RangeLayout.EndWidth = this.colorStripEndWidthSlider.SliderValue.Value;			
			DoChangedEvent();		
		}

		private void colorStripPositionDropdown_Changed(object sender, System.EventArgs e)
		{
			m_range.RangeLayout.SideKind = (RangeSideKind)Enum.Parse(typeof(RangeSideKind), (string)this.colorStripPositionDropdown.SelectedItem,false);
			DoChangedEvent();		
		}

		private void colorStripOffsetSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_range.RangeLayout.Offset = this.colorStripOffsetSlider.SliderValue.Value;
			DoChangedEvent();		
		}

		private void multiColorDialog1_OnChanged(object sender, System.EventArgs e)
		{
			DoChangedEvent();
		}

		private void annotationKindDropdown_Changed(object sender, System.EventArgs e)
		{
			m_annotation.AnnotationValueSetKind = (AnnotationValueSetKind)Enum.Parse(typeof(AnnotationValueSetKind), (string)this.annotationKindDropdown.SelectedItem,false);
			DoChangedEvent();		
		}

		private void annotationFormatTextBox_Leave(object sender, System.EventArgs e)
		{
			m_annotation.Format = this.annotationFormatTextBox.Text;
			DoChangedEvent();		
		}

		private void annotationStyleDropdown_Changed(object sender, System.EventArgs e)
		{
			m_annotation.ScaleAnnotationStyleName = (string)this.annotationStyleDropdown.SelectedItem;
			DoChangedEvent();		
		}

		private void annotationStepTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_annotation.Step = double.Parse(this.annotationStepTextBox.Text);
			}
			catch(Exception)
			{
				this.annotationStepTextBox.Text = m_annotation.Step.ToString();
			}
			DoChangedEvent();		
		}

		private void annotationScaleFactorTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_annotation.ScaleFactor = double.Parse(this.annotationScaleFactorTextBox.Text);
			}
			catch(Exception)
			{
				this.annotationScaleFactorTextBox.Text = m_annotation.ScaleFactor.ToString();
			}
			DoChangedEvent();		
		}

		private void majorTMEnabledCheckBox_Changed(object sender, System.EventArgs e)
		{
			m_range.MajorTickMarks.Visible = this.majorTMEnabledCheckBox.Checked;
			DoChangedEvent();		
		}

		private void majorTMMinValueTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MajorTickMarks.MinValue = (double)this.m_majorTickMarksDesc["MinValue"].Converter.ConvertFromString(this.majorTMMinValueTextBox.Text);
			}
			catch(Exception)
			{
				this.majorTMMinValueTextBox.Text = this.m_majorTickMarksDesc["MinValue"].Converter.ConvertToString(m_range.MajorTickMarks.MinValue);
			}
			DoChangedEvent();		
		}

		private void majorTMMaxValueTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MajorTickMarks.MaxValue = (double)this.m_majorTickMarksDesc["MaxValue"].Converter.ConvertFromString(this.majorTMMaxValueTextBox.Text);
			}
			catch(Exception)
			{
				this.majorTMMaxValueTextBox.Text = this.m_majorTickMarksDesc["MaxValue"].Converter.ConvertToString(m_range.MajorTickMarks.MaxValue);
			}
			DoChangedEvent();		
		}

		private void majorTMStyleDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_range.MajorTickMarks.MarkerStyleName = (string)this.majorTMStyleDropdown.SelectedItem;
			DoChangedEvent();		
		}

		private void majorTMOffsetSlider_ValueChanged(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_range.MajorTickMarks.Offset = this.majorTMOffsetSlider.SliderValue.Value;
			DoChangedEvent();		
		}

		private void majorTMSizeXTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MajorTickMarks.Size.Width = float.Parse(this.majorTMSizeXTextBox.Text);
			}
			catch(Exception)
			{
				this.majorTMSizeXTextBox.Text = m_range.MajorTickMarks.Size.Width.ToString();
			}
			DoChangedEvent();		
		}

		private void majorTMSizeYTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MajorTickMarks.Size.Height = float.Parse(this.majorTMSizeYTextBox.Text);
			}
			catch(Exception)
			{
				this.majorTMSizeYTextBox.Text = m_range.MajorTickMarks.Size.Height.ToString();
			}
			DoChangedEvent();		
		}

		private void majorTMStepTextBox_Leave(object sender, System.EventArgs e)
		{			
			try
			{
				m_range.MajorTickMarks.Step = (double)this.m_majorTickMarksDesc["Step"].Converter.ConvertFromString(this.majorTMStepTextBox.Text);
			}
			catch(Exception)
			{
				this.majorTMStepTextBox.Text = this.m_majorTickMarksDesc["Step"].Converter.ConvertToString(m_range.MajorTickMarks.Step);
			}
			DoChangedEvent();		
		}

		private void minorTMEnabledCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			m_range.MinorTickMarks.Visible = this.minorTMEnabledCheckBox.Checked;
			DoChangedEvent();		
		}

		private void minorTMMinValueTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MinorTickMarks.MinValue = (double)this.m_minorTickMarksDesc["MinValue"].Converter.ConvertFromString(this.minorTMMinValueTextBox.Text);
			}
			catch(Exception)
			{
				this.minorTMMinValueTextBox.Text = this.m_minorTickMarksDesc["MinValue"].Converter.ConvertToString(m_range.MinorTickMarks.MinValue);
			}
			DoChangedEvent();		
		}

		private void minorTMMaxValueTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MinorTickMarks.MaxValue = (double)this.m_minorTickMarksDesc["MaxValue"].Converter.ConvertFromString(this.minorTMMaxValueTextBox.Text);
			}
			catch(Exception)
			{
				this.minorTMMaxValueTextBox.Text = this.m_minorTickMarksDesc["MaxValue"].Converter.ConvertToString(m_range.MinorTickMarks.MaxValue);
			}
			DoChangedEvent();		
		}

		private void minorTMStyleDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//m_range.MinorTickMarks.Style = m_range.Gauge.MarkerStyles[this.minorTMStyleDropdown.SelectedItem];
			m_range.MinorTickMarks.MarkerStyleName = (string)this.minorTMStyleDropdown.SelectedItem;
			DoChangedEvent();		
		}

		private void minorTMOffsetSlider_ValueChanged(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_range.MinorTickMarks.Offset = this.minorTMOffsetSlider.SliderValue.Value;
			DoChangedEvent();		
		}

		private void minorTMSizeXTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MinorTickMarks.Size.Width = float.Parse(this.minorTMSizeXTextBox.Text);
			}
			catch(Exception)
			{
				this.minorTMSizeXTextBox.Text = m_range.MinorTickMarks.Size.Width.ToString();
			}
			DoChangedEvent();		
		}

		private void minorTMSizeYTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MinorTickMarks.Size.Height = float.Parse(this.minorTMSizeYTextBox.Text);
			}
			catch(Exception)
			{
				this.minorTMSizeYTextBox.Text = m_range.MinorTickMarks.Size.Height.ToString();
			}
			DoChangedEvent();		
		}

		private void minorTMStepTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_range.MinorTickMarks.Step = (double)this.m_minorTickMarksDesc["Step"].Converter.ConvertFromString(this.minorTMStepTextBox.Text);
			}
			catch(Exception)
			{
				this.minorTMStepTextBox.Text = this.m_minorTickMarksDesc["Step"].Converter.ConvertToString(m_range.MinorTickMarks.Step);
			}
			DoChangedEvent();		
		}
	}
}
