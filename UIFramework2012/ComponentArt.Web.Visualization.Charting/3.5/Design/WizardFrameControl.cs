using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WizardFrameControl.
	/// </summary>
	internal class WizardFrameControl : WizardElementWithHint
	{
		private ComponentArt.Win.UI.Internal.ComboBox m_frameKindComboBox;
		[WizardHint(typeof(ChartFrame), "RoundBottomCorners")]
		private ComponentArt.Win.UI.Internal.CheckBox m_bottomRoundCornerCheckBox;
		[WizardHint(typeof(ChartFrame), "RoundTopCorners")]
		private ComponentArt.Win.UI.Internal.CheckBox m_topRoundCornerCheckBox;
		[WizardHint(typeof(ChartFrame), "InnerShade")]
		private ComponentArt.Win.UI.Internal.CheckBox m_innerCheckBox;
		[WizardHint(typeof(ChartFrame), "DropShade")]
		private ComponentArt.Win.UI.Internal.CheckBox m_dropCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		//[WizardHint(typeof(System.Windows.Forms.Control), "Text")]
		private System.Windows.Forms.TextBox m_frameTextTextBox;
		[WizardHint(typeof(ChartFrame), "ExtraSpaceRight")]
		private System.Windows.Forms.NumericUpDown m_rightExtraSpaceTextBox;
		[WizardHint(typeof(ChartFrame), "ExtraSpaceTop")]
		private System.Windows.Forms.NumericUpDown m_topExtraSpaceTextBox;
		[WizardHint(typeof(ChartFrame), "ExtraSpaceLeft")]
		private System.Windows.Forms.NumericUpDown m_leftExtraSpaceTextBox;
		[WizardHint(typeof(ChartFrame), "ExtraSpaceBottom")]
		private System.Windows.Forms.NumericUpDown m_bottomExtraSpaceTextBox;
		[WizardHint(ChartBase.MainAssemblyTypeName, "TextShade")]
		private ComponentArt.Win.UI.Internal.CheckBox m_shadeCheckBox;
		private System.Windows.Forms.Label m_fontLabel;
		private ComponentArt.Win.UI.Internal.Button m_fontButton;
		[WizardHint("FrameExtraSpace")]
		private ComponentArt.Win.UI.Internal.GroupBox m_extraSpaceGroupBox;
		[WizardHint("FrameShade")]
		private ComponentArt.Win.UI.Internal.GroupBox m_frameShadeGroupBox;
		[WizardHint("FrameCornerRounding")]
		private ComponentArt.Win.UI.Internal.GroupBox m_frameCornerRoundingGroupBox;
		//[WizardHint("FrameKindAndColor")]
		[WizardHint(typeof(ChartFrame), "FrameKind")]
		private ComponentArt.Win.UI.Internal.GroupBox m_frameKindAndColorGroupBox;
		//[WizardHint("FrameTextProperties")]
		[WizardHint(ChartBase.MainAssemblyTypeName, "Text")]
		private ComponentArt.Win.UI.Internal.GroupBox m_frameTextGroupBox;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		private ComponentArt.Win.UI.Internal.Separator separator3;
		private ComponentArt.Win.UI.Internal.Separator separator4;
		private ComponentArt.Win.UI.Internal.BorderPanel borderPanel1;
		private ComponentArt.Win.UI.Internal.ComboBox m_textPositionComboBox;
		private ComponentArt.Win.UI.Internal.ComboBox m_textAlignmentComboBox;
		[WizardHint("FrameTextPositionGroupBox")]
		private ComponentArt.Win.UI.Internal.GroupBox m_frameTextPositionGroupBox;
		[WizardHint(typeof(ChartFrame), "Font")]
		private ComponentArt.Win.UI.Internal.GroupBox m_fontGroupBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardFrameControl()
		{
			InitializeComponent();

			m_frameKindComboBox.Items.AddRange(Enum.GetNames(typeof(ChartFrameKind)));
			m_textPositionComboBox.Items.AddRange(Enum.GetNames(typeof(FrameTextPosition)));
			m_textAlignmentComboBox.Items.AddRange(Enum.GetNames(typeof(StringAlignment)));
		}

		protected override void OnCreateControl () 
		{
			base.OnCreateControl();
			
			if (WinChart == null)
				return;
			
			ChartFrame cf = WinChart.Frame;
			m_frameKindComboBox.Text = cf.FrameKind.ToString();
			m_textPositionComboBox.Text = cf.TextPosition.ToString();
			m_textAlignmentComboBox.Text = cf.TextAlignment.ToString();

			m_topRoundCornerCheckBox.Checked = cf.RoundTopCorners;
			m_bottomRoundCornerCheckBox.Checked = cf.RoundBottomCorners;
			m_dropCheckBox.Checked = cf.DropShade;
			m_innerCheckBox.Checked = cf.InnerShade;

			m_leftExtraSpaceTextBox.Value = cf.ExtraSpaceLeft;
			m_topExtraSpaceTextBox.Value = cf.ExtraSpaceTop;
			m_rightExtraSpaceTextBox.Value = cf.ExtraSpaceRight;
			m_bottomExtraSpaceTextBox.Value = cf.ExtraSpaceBottom;

			m_frameTextTextBox.Text = WinChart.Text;
			m_shadeCheckBox.Checked = WinChart.TextShade;

			SetFontLabel(m_fontLabel, WinChart.Frame.Font);
		}

		internal static void SetFontLabel(Label label, Font font) 
		{
			string fontText = (string)new FontConverter().ConvertTo(null, null, font, typeof(string));

			Bitmap bmp = new Bitmap(1,1);
			Graphics g = Graphics.FromImage(bmp);

			SizeF size = g.MeasureString(fontText, label.Font);
            
			if (size.Width <= label.Width) 
			{
				label.Text = fontText;
				g.Dispose();
				bmp.Dispose();
				return;
			}
            
			while (g.MeasureString(fontText + "...", label.Font).Width > label.Width) 
			{
				fontText = fontText.Substring(0, fontText.Length - 1);
			}

			label.Text = fontText + "...";
			g.Dispose();
			bmp.Dispose();
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
			this.m_frameTextGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.borderPanel1 = new ComponentArt.Win.UI.Internal.BorderPanel();
			this.m_frameTextTextBox = new System.Windows.Forms.TextBox();
			this.m_textAlignmentComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_fontButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_fontLabel = new System.Windows.Forms.Label();
			this.m_shadeCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_textPositionComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_frameKindAndColorGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_frameKindComboBox = new ComponentArt.Win.UI.Internal.ComboBox();
			this.m_frameCornerRoundingGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_bottomRoundCornerCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_topRoundCornerCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_frameShadeGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_innerCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_dropCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_extraSpaceGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_rightExtraSpaceTextBox = new System.Windows.Forms.NumericUpDown();
			this.m_topExtraSpaceTextBox = new System.Windows.Forms.NumericUpDown();
			this.m_leftExtraSpaceTextBox = new System.Windows.Forms.NumericUpDown();
			this.m_bottomExtraSpaceTextBox = new System.Windows.Forms.NumericUpDown();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator3 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator4 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_frameTextPositionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_fontGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_frameTextGroupBox.SuspendLayout();
			this.borderPanel1.SuspendLayout();
			this.m_frameKindAndColorGroupBox.SuspendLayout();
			this.m_frameCornerRoundingGroupBox.SuspendLayout();
			this.m_frameShadeGroupBox.SuspendLayout();
			this.m_extraSpaceGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_rightExtraSpaceTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_topExtraSpaceTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_leftExtraSpaceTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_bottomExtraSpaceTextBox)).BeginInit();
			this.m_frameTextPositionGroupBox.SuspendLayout();
			this.m_fontGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_frameTextGroupBox
			// 
			this.m_frameTextGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.borderPanel1});
			this.m_frameTextGroupBox.Location = new System.Drawing.Point(0, 68);
			this.m_frameTextGroupBox.Name = "m_frameTextGroupBox";
			this.m_frameTextGroupBox.ResizeChildren = false;
			this.m_frameTextGroupBox.Size = new System.Drawing.Size(152, 44);
			this.m_frameTextGroupBox.TabIndex = 10;
			this.m_frameTextGroupBox.TabStop = false;
			this.m_frameTextGroupBox.Text = "Frame Text:";
			// 
			// borderPanel1
			// 
			this.borderPanel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.m_frameTextTextBox});
			this.borderPanel1.Location = new System.Drawing.Point(2, 22);
			this.borderPanel1.Name = "borderPanel1";
			this.borderPanel1.Size = new System.Drawing.Size(150, 20);
			this.borderPanel1.TabIndex = 0;
			// 
			// m_frameTextTextBox
			// 
			this.m_frameTextTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_frameTextTextBox.Location = new System.Drawing.Point(3, 3);
			this.m_frameTextTextBox.Name = "m_frameTextTextBox";
			this.m_frameTextTextBox.Size = new System.Drawing.Size(143, 13);
			this.m_frameTextTextBox.TabIndex = 0;
			this.m_frameTextTextBox.Text = "";
			this.m_frameTextTextBox.TextChanged += new System.EventHandler(this.m_frameTextTextBox_TextChanged);
			// 
			// m_textAlignmentComboBox
			// 
			this.m_textAlignmentComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_textAlignmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_textAlignmentComboBox.Location = new System.Drawing.Point(80, 25);
			this.m_textAlignmentComboBox.Name = "m_textAlignmentComboBox";
			this.m_textAlignmentComboBox.Size = new System.Drawing.Size(60, 21);
			this.m_textAlignmentComboBox.TabIndex = 20;
			this.m_textAlignmentComboBox.SelectedIndexChanged += new System.EventHandler(this.m_textAlignmentComboBox_SelectedIndexChanged);
			// 
			// m_fontButton
			// 
			this.m_fontButton.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_fontButton.Location = new System.Drawing.Point(206, 6);
			this.m_fontButton.Name = "m_fontButton";
			this.m_fontButton.Size = new System.Drawing.Size(20, 14);
			this.m_fontButton.TabIndex = 22;
			this.m_fontButton.Text = "...";
			this.m_fontButton.TextLocation = new System.Drawing.Point(5, 2);
			this.m_fontButton.Click += new System.EventHandler(this.m_fontButton_Click);
			// 
			// m_fontLabel
			// 
			this.m_fontLabel.BackColor = System.Drawing.Color.White;
			this.m_fontLabel.ForeColor = System.Drawing.Color.Black;
			this.m_fontLabel.Location = new System.Drawing.Point(38, 6);
			this.m_fontLabel.Name = "m_fontLabel";
			this.m_fontLabel.Size = new System.Drawing.Size(158, 16);
			this.m_fontLabel.TabIndex = 42;
			this.m_fontLabel.Text = "zazaza";
			// 
			// m_shadeCheckBox
			// 
			this.m_shadeCheckBox.BackColor = System.Drawing.Color.White;
			this.m_shadeCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_shadeCheckBox.Checked = true;
			this.m_shadeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_shadeCheckBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_shadeCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_shadeCheckBox.Location = new System.Drawing.Point(238, 120);
			this.m_shadeCheckBox.Name = "m_shadeCheckBox";
			this.m_shadeCheckBox.Size = new System.Drawing.Size(63, 17);
			this.m_shadeCheckBox.TabIndex = 23;
			this.m_shadeCheckBox.Text = "Shade:";
			this.m_shadeCheckBox.CheckedChanged += new System.EventHandler(this.m_shadeCheckBox_CheckedChanged);
			// 
			// m_textPositionComboBox
			// 
			this.m_textPositionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_textPositionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_textPositionComboBox.Location = new System.Drawing.Point(0, 25);
			this.m_textPositionComboBox.Name = "m_textPositionComboBox";
			this.m_textPositionComboBox.Size = new System.Drawing.Size(72, 21);
			this.m_textPositionComboBox.TabIndex = 10;
			this.m_textPositionComboBox.SelectedIndexChanged += new System.EventHandler(this.m_textPositionComboBox_SelectedIndexChanged);
			// 
			// m_frameKindAndColorGroupBox
			// 
			this.m_frameKindAndColorGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									  this.m_frameKindComboBox});
			this.m_frameKindAndColorGroupBox.Name = "m_frameKindAndColorGroupBox";
			this.m_frameKindAndColorGroupBox.ResizeChildren = false;
			this.m_frameKindAndColorGroupBox.Size = new System.Drawing.Size(128, 56);
			this.m_frameKindAndColorGroupBox.TabIndex = 1;
			this.m_frameKindAndColorGroupBox.TabStop = false;
			this.m_frameKindAndColorGroupBox.Text = "Frame Style:";
			// 
			// m_frameKindComboBox
			// 
			this.m_frameKindComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_frameKindComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_frameKindComboBox.Location = new System.Drawing.Point(2, 28);
			this.m_frameKindComboBox.Name = "m_frameKindComboBox";
			this.m_frameKindComboBox.Size = new System.Drawing.Size(126, 21);
			this.m_frameKindComboBox.TabIndex = 0;
			this.m_frameKindComboBox.SelectedIndexChanged += new System.EventHandler(this.m_frameKindComboBox_SelectedIndexChanged);
			// 
			// m_frameCornerRoundingGroupBox
			// 
			this.m_frameCornerRoundingGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																										this.m_bottomRoundCornerCheckBox,
																										this.m_topRoundCornerCheckBox});
			this.m_frameCornerRoundingGroupBox.Location = new System.Drawing.Point(0, 152);
			this.m_frameCornerRoundingGroupBox.Name = "m_frameCornerRoundingGroupBox";
			this.m_frameCornerRoundingGroupBox.ResizeChildren = false;
			this.m_frameCornerRoundingGroupBox.Size = new System.Drawing.Size(304, 25);
			this.m_frameCornerRoundingGroupBox.TabIndex = 25;
			this.m_frameCornerRoundingGroupBox.TabStop = false;
			this.m_frameCornerRoundingGroupBox.Text = "Round corners:";
			// 
			// m_bottomRoundCornerCheckBox
			// 
			this.m_bottomRoundCornerCheckBox.BackColor = System.Drawing.Color.White;
			this.m_bottomRoundCornerCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_bottomRoundCornerCheckBox.Checked = true;
			this.m_bottomRoundCornerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_bottomRoundCornerCheckBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_bottomRoundCornerCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_bottomRoundCornerCheckBox.Location = new System.Drawing.Point(181, 4);
			this.m_bottomRoundCornerCheckBox.Name = "m_bottomRoundCornerCheckBox";
			this.m_bottomRoundCornerCheckBox.Size = new System.Drawing.Size(67, 17);
			this.m_bottomRoundCornerCheckBox.TabIndex = 40;
			this.m_bottomRoundCornerCheckBox.Text = "Bottom";
			this.m_bottomRoundCornerCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_bottomRoundCornerCheckBox.CheckedChanged += new System.EventHandler(this.m_bottomRoundCornerCheckBox_CheckedChanged);
			// 
			// m_topRoundCornerCheckBox
			// 
			this.m_topRoundCornerCheckBox.BackColor = System.Drawing.Color.White;
			this.m_topRoundCornerCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_topRoundCornerCheckBox.Checked = true;
			this.m_topRoundCornerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_topRoundCornerCheckBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_topRoundCornerCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_topRoundCornerCheckBox.Location = new System.Drawing.Point(120, 4);
			this.m_topRoundCornerCheckBox.Name = "m_topRoundCornerCheckBox";
			this.m_topRoundCornerCheckBox.Size = new System.Drawing.Size(48, 17);
			this.m_topRoundCornerCheckBox.TabIndex = 39;
			this.m_topRoundCornerCheckBox.Text = "Top";
			this.m_topRoundCornerCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_topRoundCornerCheckBox.CheckedChanged += new System.EventHandler(this.m_topRoundCornerCheckBox_CheckedChanged);
			// 
			// m_frameShadeGroupBox
			// 
			this.m_frameShadeGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_innerCheckBox,
																							   this.m_dropCheckBox});
			this.m_frameShadeGroupBox.Location = new System.Drawing.Point(0, 188);
			this.m_frameShadeGroupBox.Name = "m_frameShadeGroupBox";
			this.m_frameShadeGroupBox.ResizeChildren = false;
			this.m_frameShadeGroupBox.Size = new System.Drawing.Size(304, 25);
			this.m_frameShadeGroupBox.TabIndex = 30;
			this.m_frameShadeGroupBox.TabStop = false;
			this.m_frameShadeGroupBox.Text = "Shade:";
			// 
			// m_innerCheckBox
			// 
			this.m_innerCheckBox.BackColor = System.Drawing.Color.White;
			this.m_innerCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_innerCheckBox.Checked = true;
			this.m_innerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_innerCheckBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_innerCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_innerCheckBox.Location = new System.Drawing.Point(189, 4);
			this.m_innerCheckBox.Name = "m_innerCheckBox";
			this.m_innerCheckBox.Size = new System.Drawing.Size(59, 17);
			this.m_innerCheckBox.TabIndex = 40;
			this.m_innerCheckBox.Text = "Inner";
			this.m_innerCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_innerCheckBox.CheckedChanged += new System.EventHandler(this.m_innerCheckBox_CheckedChanged);
			// 
			// m_dropCheckBox
			// 
			this.m_dropCheckBox.BackColor = System.Drawing.Color.White;
			this.m_dropCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_dropCheckBox.Checked = true;
			this.m_dropCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_dropCheckBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_dropCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_dropCheckBox.Location = new System.Drawing.Point(114, 5);
			this.m_dropCheckBox.Name = "m_dropCheckBox";
			this.m_dropCheckBox.Size = new System.Drawing.Size(54, 17);
			this.m_dropCheckBox.TabIndex = 39;
			this.m_dropCheckBox.Text = "Drop";
			this.m_dropCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_dropCheckBox.CheckedChanged += new System.EventHandler(this.m_dropCheckBox_CheckedChanged);
			// 
			// m_extraSpaceGroupBox
			// 
			this.m_extraSpaceGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_rightExtraSpaceTextBox,
																							   this.m_topExtraSpaceTextBox,
																							   this.m_leftExtraSpaceTextBox,
																							   this.m_bottomExtraSpaceTextBox,
																							   this.label4,
																							   this.label3,
																							   this.label2,
																							   this.label1});
			this.m_extraSpaceGroupBox.Location = new System.Drawing.Point(0, 224);
			this.m_extraSpaceGroupBox.Name = "m_extraSpaceGroupBox";
			this.m_extraSpaceGroupBox.ResizeChildren = false;
			this.m_extraSpaceGroupBox.Size = new System.Drawing.Size(304, 53);
			this.m_extraSpaceGroupBox.TabIndex = 40;
			this.m_extraSpaceGroupBox.TabStop = false;
			this.m_extraSpaceGroupBox.Text = "Extra Space:";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.White;
			this.label4.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label4.Location = new System.Drawing.Point(227, 29);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Right";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.White;
			this.label3.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label3.Location = new System.Drawing.Point(158, 29);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Top";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.White;
			this.label2.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label2.Location = new System.Drawing.Point(87, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Left";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.label1.Location = new System.Drawing.Point(2, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Bottom";
			// 
			// m_rightExtraSpaceTextBox
			// 
			this.m_rightExtraSpaceTextBox.Location = new System.Drawing.Point(264, 27);
			this.m_rightExtraSpaceTextBox.Name = "m_rightExtraSpaceTextBox";
			this.m_rightExtraSpaceTextBox.Size = new System.Drawing.Size(36, 20);
			this.m_rightExtraSpaceTextBox.TabIndex = 41;
			this.m_rightExtraSpaceTextBox.TextChanged += new System.EventHandler(this.m_rightExtraSpaceTextBox_TextChanged);
			// 
			// m_topExtraSpaceTextBox
			// 
			this.m_topExtraSpaceTextBox.Location = new System.Drawing.Point(187, 27);
			this.m_topExtraSpaceTextBox.Name = "m_topExtraSpaceTextBox";
			this.m_topExtraSpaceTextBox.Size = new System.Drawing.Size(36, 20);
			this.m_topExtraSpaceTextBox.TabIndex = 40;
			this.m_topExtraSpaceTextBox.TextChanged += new System.EventHandler(this.m_topExtraSpaceTextBox_TextChanged);
			// 
			// m_leftExtraSpaceTextBox
			// 
			this.m_leftExtraSpaceTextBox.Location = new System.Drawing.Point(116, 27);
			this.m_leftExtraSpaceTextBox.Name = "m_leftExtraSpaceTextBox";
			this.m_leftExtraSpaceTextBox.Size = new System.Drawing.Size(34, 20);
			this.m_leftExtraSpaceTextBox.TabIndex = 39;
			this.m_leftExtraSpaceTextBox.TextChanged += new System.EventHandler(this.m_leftExtraSpaceTextBox_TextChanged);
			// 
			// m_bottomExtraSpaceTextBox
			// 
			this.m_bottomExtraSpaceTextBox.Location = new System.Drawing.Point(48, 27);
			this.m_bottomExtraSpaceTextBox.Name = "m_bottomExtraSpaceTextBox";
			this.m_bottomExtraSpaceTextBox.Size = new System.Drawing.Size(36, 20);
			this.m_bottomExtraSpaceTextBox.TabIndex = 38;
			this.m_bottomExtraSpaceTextBox.TextChanged += new System.EventHandler(this.m_bottomExtraSpaceTextBox_TextChanged);
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(0, 62);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(304, 3);
			this.separator1.TabIndex = 48;
			this.separator1.TabStop = false;
			// 
			// separator2
			// 
			this.separator2.Location = new System.Drawing.Point(0, 144);
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(304, 3);
			this.separator2.TabIndex = 49;
			this.separator2.TabStop = false;
			// 
			// separator3
			// 
			this.separator3.Location = new System.Drawing.Point(0, 182);
			this.separator3.Name = "separator3";
			this.separator3.Size = new System.Drawing.Size(304, 3);
			this.separator3.TabIndex = 50;
			this.separator3.TabStop = false;
			// 
			// separator4
			// 
			this.separator4.Location = new System.Drawing.Point(0, 218);
			this.separator4.Name = "separator4";
			this.separator4.Size = new System.Drawing.Size(304, 3);
			this.separator4.TabIndex = 51;
			this.separator4.TabStop = false;
			// 
			// m_frameTextPositionGroupBox
			// 
			this.m_frameTextPositionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									  this.m_textAlignmentComboBox,
																									  this.m_textPositionComboBox});
			this.m_frameTextPositionGroupBox.Location = new System.Drawing.Point(160, 64);
			this.m_frameTextPositionGroupBox.Name = "m_frameTextPositionGroupBox";
			this.m_frameTextPositionGroupBox.ResizeChildren = false;
			this.m_frameTextPositionGroupBox.Size = new System.Drawing.Size(139, 47);
			this.m_frameTextPositionGroupBox.TabIndex = 10;
			this.m_frameTextPositionGroupBox.TabStop = false;
			this.m_frameTextPositionGroupBox.Text = "Frame Text Position:";
			// 
			// m_fontGroupBox
			// 
			this.m_fontGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.m_fontButton,
																						 this.m_fontLabel});
			this.m_fontGroupBox.Location = new System.Drawing.Point(0, 115);
			this.m_fontGroupBox.Name = "m_fontGroupBox";
			this.m_fontGroupBox.ResizeChildren = false;
			this.m_fontGroupBox.Size = new System.Drawing.Size(232, 28);
			this.m_fontGroupBox.TabIndex = 52;
			this.m_fontGroupBox.TabStop = false;
			this.m_fontGroupBox.Text = "Font:";
			// 
			// WizardFrameControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_shadeCheckBox,
																		  this.separator4,
																		  this.separator3,
																		  this.separator2,
																		  this.separator1,
																		  this.m_frameShadeGroupBox,
																		  this.m_frameKindAndColorGroupBox,
																		  this.m_frameTextGroupBox,
																		  this.m_frameCornerRoundingGroupBox,
																		  this.m_extraSpaceGroupBox,
																		  this.m_frameTextPositionGroupBox,
																		  this.m_fontGroupBox});
			this.Name = "WizardFrameControl";
			this.Size = new System.Drawing.Size(304, 288);
			this.m_frameTextGroupBox.ResumeLayout(false);
			this.borderPanel1.ResumeLayout(false);
			this.m_frameKindAndColorGroupBox.ResumeLayout(false);
			this.m_frameCornerRoundingGroupBox.ResumeLayout(false);
			this.m_frameShadeGroupBox.ResumeLayout(false);
			this.m_extraSpaceGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_rightExtraSpaceTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_topExtraSpaceTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_leftExtraSpaceTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_bottomExtraSpaceTextBox)).EndInit();
			this.m_frameTextPositionGroupBox.ResumeLayout(false);
			this.m_fontGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_frameKindComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.FrameKind = (ChartFrameKind)Enum.Parse(typeof(ChartFrameKind), m_frameKindComboBox.Text);
			WinChart.Invalidate();
		}

		private void m_topRoundCornerCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.RoundTopCorners = m_topRoundCornerCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_bottomRoundCornerCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.RoundBottomCorners = m_bottomRoundCornerCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_dropCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.DropShade = m_dropCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_innerCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.InnerShade = m_innerCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_bottomExtraSpaceTextBox_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.ExtraSpaceBottom = (int)m_bottomExtraSpaceTextBox.Value;
			WinChart.Invalidate();
		}

		private void m_leftExtraSpaceTextBox_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.ExtraSpaceLeft = (int)m_leftExtraSpaceTextBox.Value;
			WinChart.Invalidate();
		}

		private void m_topExtraSpaceTextBox_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.ExtraSpaceTop = (int)m_topExtraSpaceTextBox.Value;
			WinChart.Invalidate();
		}

		private void m_rightExtraSpaceTextBox_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.ExtraSpaceRight = (int)m_rightExtraSpaceTextBox.Value;
			WinChart.Invalidate();
		}

		private void m_frameTextTextBox_TextChanged(object sender, System.EventArgs e)
		{
			WinChart.Text = m_frameTextTextBox.Text;
			WinChart.Invalidate();
		}

		private void m_shadeCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.TextShade = m_shadeCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_fontButton_Click(object sender, System.EventArgs e)
		{
			FontDialog fontDialog1 = new FontDialog();
			fontDialog1.ShowColor = true;

			fontDialog1.Font = WinChart.Frame.Font;
			fontDialog1.Color = WinChart.ForeColor;

			if(fontDialog1.ShowDialog() != DialogResult.Cancel )
			{
				WinChart.Frame.Font = fontDialog1.Font;
				WinChart.ForeColor = fontDialog1.Color;
				SetFontLabel(m_fontLabel, WinChart.Frame.Font);

				WinChart.Invalidate();
			}

		}

		private void m_textPositionComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.TextPosition = (FrameTextPosition)Enum.Parse(typeof(FrameTextPosition), m_textPositionComboBox.Text);
			WinChart.Invalidate();
		}

		private void m_textAlignmentComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.Frame.TextAlignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), m_textAlignmentComboBox.Text);
			WinChart.Invalidate();

		}
	}
}

namespace ComponentArt.Win.UI.Internal 
{
	internal class BorderPanel : System.Windows.Forms.Panel 
	{
		int m_padding = 2;

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			base.OnPaint(e);

			if (Controls.Count == 0)
				return;

			foreach (Control c in Controls) 
			{
				if ( !(c is TextBox) || ((TextBox)c).BorderStyle != BorderStyle.None)
					continue;

				e.Graphics.DrawLines(new Pen(Color.FromArgb(153, 153, 153)), new Point [] 
				{
					new Point(c.Left + c.Width + m_padding, c.Top-1 - m_padding), 
					new Point(c.Left-1 - m_padding, c.Top-1 - m_padding), 
					new Point(c.Left-1 - m_padding, c.Top + c.Height-1 + m_padding), 
				});

				e.Graphics.DrawLines(new Pen(Color.FromArgb(204, 204, 204)), new Point [] 
				{
					new Point(c.Left + c.Width + m_padding, c.Top - m_padding), 
					new Point(c.Left + c.Width + m_padding, c.Top + c.Height + m_padding), 
					new Point(c.Left-1 - m_padding, c.Top + c.Height + m_padding), 
				});
			}
		}
	}
}