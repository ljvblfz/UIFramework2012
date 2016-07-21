using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WizardLegendDialog.
	/// </summary>
	internal class WizardLegendDialog : WizardDialog
	{
		private System.Windows.Forms.ComboBox m_legendPositionComboBox;
		private ComponentArt.Win.UI.Internal.RadioButton m_rowRadioButton;
		private ComponentArt.Win.UI.Internal.RadioButton m_columnRadioButton;
		private System.Windows.Forms.NumericUpDown m_numberOfItemsUpDown;
		private System.Windows.Forms.Label m_fontLabel;
		private System.Windows.Forms.NumericUpDown m_borderWidthUpDown;
		private System.Windows.Forms.NumericUpDown m_borderShadeWidthUpDown;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown m_itemBorderShadeWidthUpDown;
		private System.Windows.Forms.NumericUpDown m_itemBorderWidthUpDown;
		private ComponentArt.Win.UI.Internal.CheckBox m_legendVisibleCheckBox;
		private System.Windows.Forms.Panel m_legendPropertiesPanel;
		[WizardHint(typeof(Legend), "Font")]
		private ComponentArt.Win.UI.Internal.GroupBox m_fontGroupBox;
		private ComponentArt.Win.UI.Internal.Button m_fontButton;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		private ComponentArt.Win.UI.Internal.CheckBox m_isBackgroundVisibleCheckBox;
		[WizardHint(typeof(Legend), "Visible")]
		private ComponentArt.Win.UI.Internal.GroupBox m_visibleGroupBox;
		[WizardHint(typeof(Legend), "LegendPosition")]
		private ComponentArt.Win.UI.Internal.GroupBox m_positionGroupBox;
		[WizardHint(typeof(Legend), "LegendLayout")]
		private ComponentArt.Win.UI.Internal.GroupBox m_layoutGroupBox;
		[WizardHint(typeof(Legend), "NumberOfItemsInRow")]
		private ComponentArt.Win.UI.Internal.GroupBox m_numberOfItemsGroupBox;
		[WizardHint(typeof(Legend), "BorderWidth")]
		private ComponentArt.Win.UI.Internal.GroupBox m_borderWidthGroupBox;
		[WizardHint(typeof(Legend), "BorderShadeWidth")]
		private ComponentArt.Win.UI.Internal.GroupBox m_borderShadeWidthGroupBox;
		[WizardHint(typeof(Legend), "ItemBorderWidth")]
		private ComponentArt.Win.UI.Internal.GroupBox m_itemBorderWidthGroupBox;
		[WizardHint(typeof(Legend), "ItemBorderShadeWidth")]
		private ComponentArt.Win.UI.Internal.GroupBox m_itemBorderShadeWidthGroupBox;
		[WizardHint(typeof(Legend), "DrawBackgroundRectangle")]
		private ComponentArt.Win.UI.Internal.GroupBox m_backgroundGroupBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardLegendDialog()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_legendPositionComboBox.Items.AddRange(Enum.GetNames(typeof(LegendPositionKind)));

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

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (WinChart == null)
				return;

			WizardFrameControl.SetFontLabel(m_fontLabel, WinChart.Legend.Font);
			
			m_legendVisibleCheckBox.Checked = WinChart.Legend.Visible;
			m_legendPositionComboBox.SelectedItem = WinChart.Legend.LegendPosition.ToString();
			
			((RadioButton)(WinChart.Legend.LegendLayout == LegendKind.Column ? m_columnRadioButton : m_rowRadioButton)).Select();
			m_numberOfItemsUpDown.Value = (WinChart.Legend.LegendLayout == LegendKind.Column) ? WinChart.Legend.NumberOfItemsInColumn : WinChart.Legend.NumberOfItemsInRow;
            
            m_isBackgroundVisibleCheckBox.Checked = WinChart.Legend.DrawBackgroundRectangle;
			m_borderWidthUpDown.Value = WinChart.Legend.BorderWidth;
			m_borderShadeWidthUpDown.Value = WinChart.Legend.BorderShadeWidth;

			m_itemBorderShadeWidthUpDown.Value = WinChart.Legend.ItemBorderShadeWidth;
			m_itemBorderWidthUpDown.Value = WinChart.Legend.ItemBorderWidth;
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_positionGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_legendPositionComboBox = new System.Windows.Forms.ComboBox();
			this.m_layoutGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_columnRadioButton = new ComponentArt.Win.UI.Internal.RadioButton();
			this.m_rowRadioButton = new ComponentArt.Win.UI.Internal.RadioButton();
			this.m_numberOfItemsUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_fontGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_fontButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_fontLabel = new System.Windows.Forms.Label();
			this.m_backgroundGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_isBackgroundVisibleCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_borderWidthGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_borderWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_borderShadeWidthGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_borderShadeWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_itemBorderWidthGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_itemBorderWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_itemBorderShadeWidthGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_itemBorderShadeWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_visibleGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_legendVisibleCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_legendPropertiesPanel = new System.Windows.Forms.Panel();
			this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_numberOfItemsGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_positionGroupBox.SuspendLayout();
			this.m_layoutGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_numberOfItemsUpDown)).BeginInit();
			this.m_fontGroupBox.SuspendLayout();
			this.m_backgroundGroupBox.SuspendLayout();
			this.m_borderWidthGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_borderWidthUpDown)).BeginInit();
			this.m_borderShadeWidthGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_borderShadeWidthUpDown)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.m_itemBorderWidthGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_itemBorderWidthUpDown)).BeginInit();
			this.m_itemBorderShadeWidthGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_itemBorderShadeWidthUpDown)).BeginInit();
			this.m_visibleGroupBox.SuspendLayout();
			this.m_legendPropertiesPanel.SuspendLayout();
			this.m_numberOfItemsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_positionGroupBox
			// 
			this.m_positionGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							 this.m_legendPositionComboBox});
			this.m_positionGroupBox.Location = new System.Drawing.Point(144, 30);
			this.m_positionGroupBox.Name = "m_positionGroupBox";
			this.m_positionGroupBox.ResizeChildren = false;
			this.m_positionGroupBox.Size = new System.Drawing.Size(168, 29);
			this.m_positionGroupBox.TabIndex = 50;
			this.m_positionGroupBox.TabStop = false;
			this.m_positionGroupBox.Text = "Position:";
			// 
			// m_legendPositionComboBox
			// 
			this.m_legendPositionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_legendPositionComboBox.Items.AddRange(new object[] {""});
			this.m_legendPositionComboBox.Location = new System.Drawing.Point(57, 2);
			this.m_legendPositionComboBox.Name = "m_legendPositionComboBox";
			this.m_legendPositionComboBox.Size = new System.Drawing.Size(106, 21);
			this.m_legendPositionComboBox.TabIndex = 50;
			this.m_legendPositionComboBox.SelectedIndexChanged += new System.EventHandler(this.m_legendPositionComboBox_SelectedIndexChanged);
			// 
			// m_layoutGroupBox
			// 
			this.m_layoutGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.m_columnRadioButton,
																						   this.m_rowRadioButton});
			this.m_layoutGroupBox.Name = "m_layoutGroupBox";
			this.m_layoutGroupBox.ResizeChildren = false;
			this.m_layoutGroupBox.Size = new System.Drawing.Size(256, 24);
			this.m_layoutGroupBox.TabIndex = 35;
			this.m_layoutGroupBox.TabStop = false;
			this.m_layoutGroupBox.Text = "Layout:";
			// 
			// m_columnRadioButton
			// 
			this.m_columnRadioButton.BackColor = System.Drawing.Color.Transparent;
			this.m_columnRadioButton.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_columnRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_columnRadioButton.Location = new System.Drawing.Point(144, 5);
			this.m_columnRadioButton.Name = "m_columnRadioButton";
			this.m_columnRadioButton.Size = new System.Drawing.Size(80, 17);
			this.m_columnRadioButton.TabIndex = 30;
			this.m_columnRadioButton.Text = "Column";
			this.m_columnRadioButton.CheckedChanged += new System.EventHandler(this.m_columnRadioButton_CheckedChanged);
			// 
			// m_rowRadioButton
			// 
			this.m_rowRadioButton.BackColor = System.Drawing.Color.Transparent;
			this.m_rowRadioButton.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_rowRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_rowRadioButton.Location = new System.Drawing.Point(88, 5);
			this.m_rowRadioButton.Name = "m_rowRadioButton";
			this.m_rowRadioButton.Size = new System.Drawing.Size(64, 17);
			this.m_rowRadioButton.TabIndex = 20;
			this.m_rowRadioButton.Text = "Row";
			this.m_rowRadioButton.CheckedChanged += new System.EventHandler(this.m_rowRadioButton_CheckedChanged);
			// 
			// m_numberOfItemsUpDown
			// 
			this.m_numberOfItemsUpDown.Location = new System.Drawing.Point(88, 4);
			this.m_numberOfItemsUpDown.Name = "m_numberOfItemsUpDown";
			this.m_numberOfItemsUpDown.Size = new System.Drawing.Size(48, 20);
			this.m_numberOfItemsUpDown.TabIndex = 40;
			this.m_numberOfItemsUpDown.ValueChanged += new System.EventHandler(this.m_numberOfItemsUpDown_ValueChanged);
			// 
			// m_fontGroupBox
			// 
			this.m_fontGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.m_fontButton,
																						 this.m_fontLabel});
			this.m_fontGroupBox.Enabled = false;
			this.m_fontGroupBox.Location = new System.Drawing.Point(88, 8);
			this.m_fontGroupBox.Name = "m_fontGroupBox";
			this.m_fontGroupBox.ResizeChildren = false;
			this.m_fontGroupBox.Size = new System.Drawing.Size(232, 26);
			this.m_fontGroupBox.TabIndex = 48;
			this.m_fontGroupBox.TabStop = false;
			this.m_fontGroupBox.Text = "Font:";
			// 
			// m_fontButton
			// 
			this.m_fontButton.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_fontButton.Location = new System.Drawing.Point(206, 5);
			this.m_fontButton.Name = "m_fontButton";
			this.m_fontButton.Size = new System.Drawing.Size(20, 14);
			this.m_fontButton.TabIndex = 10;
			this.m_fontButton.Text = "...";
			this.m_fontButton.TextLocation = new System.Drawing.Point(5, 2);
			this.m_fontButton.Click += new System.EventHandler(this.m_fontButton_Click);
			// 
			// m_fontLabel
			// 
			this.m_fontLabel.BackColor = System.Drawing.Color.White;
			this.m_fontLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_fontLabel.Location = new System.Drawing.Point(40, 6);
			this.m_fontLabel.Name = "m_fontLabel";
			this.m_fontLabel.Size = new System.Drawing.Size(160, 16);
			this.m_fontLabel.TabIndex = 45;
			this.m_fontLabel.Text = "Microsoft Sans Serif, 8.25pt";
			// 
			// m_backgroundGroupBox
			// 
			this.m_backgroundGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_isBackgroundVisibleCheckBox,
																							   this.m_borderWidthGroupBox,
																							   this.m_borderShadeWidthGroupBox});
			this.m_backgroundGroupBox.Location = new System.Drawing.Point(0, 76);
			this.m_backgroundGroupBox.Name = "m_backgroundGroupBox";
			this.m_backgroundGroupBox.ResizeChildren = false;
			this.m_backgroundGroupBox.Size = new System.Drawing.Size(312, 64);
			this.m_backgroundGroupBox.TabIndex = 55;
			this.m_backgroundGroupBox.TabStop = false;
			this.m_backgroundGroupBox.Text = "Background";
			// 
			// m_isBackgroundVisibleCheckBox
			// 
			this.m_isBackgroundVisibleCheckBox.BackColor = System.Drawing.Color.White;
			this.m_isBackgroundVisibleCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_isBackgroundVisibleCheckBox.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_isBackgroundVisibleCheckBox.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_isBackgroundVisibleCheckBox.Location = new System.Drawing.Point(83, 4);
			this.m_isBackgroundVisibleCheckBox.Name = "m_isBackgroundVisibleCheckBox";
			this.m_isBackgroundVisibleCheckBox.Size = new System.Drawing.Size(24, 20);
			this.m_isBackgroundVisibleCheckBox.TabIndex = 60;
			this.m_isBackgroundVisibleCheckBox.CheckedChanged += new System.EventHandler(this.m_isBackgroundVisibleCheckBox_CheckedChanged);
			// 
			// m_borderWidthGroupBox
			// 
			this.m_borderWidthGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.m_borderWidthUpDown});
			this.m_borderWidthGroupBox.Location = new System.Drawing.Point(-1, 34);
			this.m_borderWidthGroupBox.Name = "m_borderWidthGroupBox";
			this.m_borderWidthGroupBox.ResizeChildren = false;
			this.m_borderWidthGroupBox.Size = new System.Drawing.Size(130, 29);
			this.m_borderWidthGroupBox.TabIndex = 70;
			this.m_borderWidthGroupBox.TabStop = false;
			this.m_borderWidthGroupBox.Text = "Border Width:";
			// 
			// m_borderWidthUpDown
			// 
			this.m_borderWidthUpDown.Location = new System.Drawing.Point(89, 4);
			this.m_borderWidthUpDown.Name = "m_borderWidthUpDown";
			this.m_borderWidthUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_borderWidthUpDown.TabIndex = 70;
			this.m_borderWidthUpDown.ValueChanged += new System.EventHandler(this.m_borderWidthUpDown_ValueChanged);
			// 
			// m_borderShadeWidthGroupBox
			// 
			this.m_borderShadeWidthGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									 this.m_borderShadeWidthUpDown});
			this.m_borderShadeWidthGroupBox.Location = new System.Drawing.Point(136, 34);
			this.m_borderShadeWidthGroupBox.Name = "m_borderShadeWidthGroupBox";
			this.m_borderShadeWidthGroupBox.ResizeChildren = false;
			this.m_borderShadeWidthGroupBox.Size = new System.Drawing.Size(172, 29);
			this.m_borderShadeWidthGroupBox.TabIndex = 80;
			this.m_borderShadeWidthGroupBox.TabStop = false;
			this.m_borderShadeWidthGroupBox.Text = "Border Shade Width:";
			// 
			// m_borderShadeWidthUpDown
			// 
			this.m_borderShadeWidthUpDown.Location = new System.Drawing.Point(129, 4);
			this.m_borderShadeWidthUpDown.Name = "m_borderShadeWidthUpDown";
			this.m_borderShadeWidthUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_borderShadeWidthUpDown.TabIndex = 80;
			this.m_borderShadeWidthUpDown.ValueChanged += new System.EventHandler(this.m_borderShadeWidthUpDown_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_itemBorderWidthGroupBox,
																					this.m_itemBorderShadeWidthGroupBox});
			this.groupBox2.Location = new System.Drawing.Point(0, 156);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.ResizeChildren = false;
			this.groupBox2.Size = new System.Drawing.Size(312, 56);
			this.groupBox2.TabIndex = 85;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Item border properties:";
			// 
			// m_itemBorderWidthGroupBox
			// 
			this.m_itemBorderWidthGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.m_itemBorderWidthUpDown});
			this.m_itemBorderWidthGroupBox.Location = new System.Drawing.Point(0, 26);
			this.m_itemBorderWidthGroupBox.Name = "m_itemBorderWidthGroupBox";
			this.m_itemBorderWidthGroupBox.ResizeChildren = false;
			this.m_itemBorderWidthGroupBox.Size = new System.Drawing.Size(128, 29);
			this.m_itemBorderWidthGroupBox.TabIndex = 88;
			this.m_itemBorderWidthGroupBox.TabStop = false;
			this.m_itemBorderWidthGroupBox.Text = "Width:";
			// 
			// m_itemBorderWidthUpDown
			// 
			this.m_itemBorderWidthUpDown.Location = new System.Drawing.Point(88, 4);
			this.m_itemBorderWidthUpDown.Name = "m_itemBorderWidthUpDown";
			this.m_itemBorderWidthUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_itemBorderWidthUpDown.TabIndex = 90;
			this.m_itemBorderWidthUpDown.ValueChanged += new System.EventHandler(this.m_itemBorderWidthUpDown_ValueChanged);
			// 
			// m_itemBorderShadeWidthGroupBox
			// 
			this.m_itemBorderShadeWidthGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																										 this.m_itemBorderShadeWidthUpDown});
			this.m_itemBorderShadeWidthGroupBox.Location = new System.Drawing.Point(160, 26);
			this.m_itemBorderShadeWidthGroupBox.Name = "m_itemBorderShadeWidthGroupBox";
			this.m_itemBorderShadeWidthGroupBox.ResizeChildren = false;
			this.m_itemBorderShadeWidthGroupBox.Size = new System.Drawing.Size(90, 29);
			this.m_itemBorderShadeWidthGroupBox.TabIndex = 101;
			this.m_itemBorderShadeWidthGroupBox.TabStop = false;
			this.m_itemBorderShadeWidthGroupBox.Text = "Shade:";
			// 
			// m_itemBorderShadeWidthUpDown
			// 
			this.m_itemBorderShadeWidthUpDown.Location = new System.Drawing.Point(50, 4);
			this.m_itemBorderShadeWidthUpDown.Name = "m_itemBorderShadeWidthUpDown";
			this.m_itemBorderShadeWidthUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_itemBorderShadeWidthUpDown.TabIndex = 100;
			this.m_itemBorderShadeWidthUpDown.ValueChanged += new System.EventHandler(this.m_itemBorderShadeWidthUpDown_ValueChanged);
			// 
			// m_visibleGroupBox
			// 
			this.m_visibleGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.m_legendVisibleCheckBox});
			this.m_visibleGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_visibleGroupBox.Name = "m_visibleGroupBox";
			this.m_visibleGroupBox.ResizeChildren = false;
			this.m_visibleGroupBox.Size = new System.Drawing.Size(72, 26);
			this.m_visibleGroupBox.TabIndex = 0;
			this.m_visibleGroupBox.TabStop = false;
			this.m_visibleGroupBox.Text = "Visible";
			// 
			// m_legendVisibleCheckBox
			// 
			this.m_legendVisibleCheckBox.BackColor = System.Drawing.Color.White;
			this.m_legendVisibleCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_legendVisibleCheckBox.Location = new System.Drawing.Point(51, 0);
			this.m_legendVisibleCheckBox.Name = "m_legendVisibleCheckBox";
			this.m_legendVisibleCheckBox.Size = new System.Drawing.Size(24, 26);
			this.m_legendVisibleCheckBox.TabIndex = 4;
			this.m_legendVisibleCheckBox.CheckedChanged += new System.EventHandler(this.m_legendVisibleCheckBox_CheckedChanged);
			// 
			// m_legendPropertiesPanel
			// 
			this.m_legendPropertiesPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																								  this.separator2,
																								  this.separator1,
																								  this.m_backgroundGroupBox,
																								  this.groupBox2,
																								  this.m_layoutGroupBox,
																								  this.m_positionGroupBox,
																								  this.m_numberOfItemsGroupBox});
			this.m_legendPropertiesPanel.Enabled = false;
			this.m_legendPropertiesPanel.Location = new System.Drawing.Point(8, 40);
			this.m_legendPropertiesPanel.Name = "m_legendPropertiesPanel";
			this.m_legendPropertiesPanel.Size = new System.Drawing.Size(312, 240);
			this.m_legendPropertiesPanel.TabIndex = 52;
			// 
			// separator2
			// 
			this.separator2.Location = new System.Drawing.Point(0, 148);
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(304, 3);
			this.separator2.TabIndex = 55;
			this.separator2.TabStop = false;
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(0, 68);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(304, 3);
			this.separator1.TabIndex = 54;
			this.separator1.TabStop = false;
			// 
			// m_numberOfItemsGroupBox
			// 
			this.m_numberOfItemsGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								  this.m_numberOfItemsUpDown});
			this.m_numberOfItemsGroupBox.Location = new System.Drawing.Point(0, 30);
			this.m_numberOfItemsGroupBox.Name = "m_numberOfItemsGroupBox";
			this.m_numberOfItemsGroupBox.ResizeChildren = false;
			this.m_numberOfItemsGroupBox.Size = new System.Drawing.Size(136, 29);
			this.m_numberOfItemsGroupBox.TabIndex = 40;
			this.m_numberOfItemsGroupBox.TabStop = false;
			this.m_numberOfItemsGroupBox.Text = "No. of items:";
			// 
			// WizardLegendDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_visibleGroupBox,
																		  this.m_fontGroupBox,
																		  this.m_legendPropertiesPanel});
			this.Name = "WizardLegendDialog";
			this.Size = new System.Drawing.Size(324, 296);
			this.m_positionGroupBox.ResumeLayout(false);
			this.m_layoutGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_numberOfItemsUpDown)).EndInit();
			this.m_fontGroupBox.ResumeLayout(false);
			this.m_backgroundGroupBox.ResumeLayout(false);
			this.m_borderWidthGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_borderWidthUpDown)).EndInit();
			this.m_borderShadeWidthGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_borderShadeWidthUpDown)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.m_itemBorderWidthGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_itemBorderWidthUpDown)).EndInit();
			this.m_itemBorderShadeWidthGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_itemBorderShadeWidthUpDown)).EndInit();
			this.m_visibleGroupBox.ResumeLayout(false);
			this.m_legendPropertiesPanel.ResumeLayout(false);
			this.m_numberOfItemsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void m_legendPositionComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.LegendPosition = (LegendPositionKind)Enum.Parse(typeof(LegendPositionKind), m_legendPositionComboBox.SelectedItem.ToString());
			WinChart.Invalidate();
		}

		private void m_rowRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.LegendLayout = LegendKind.Row;
			m_numberOfItemsUpDown.Value = WinChart.Legend.NumberOfItemsInRow;
			WinChart.Invalidate();
		}

		private void m_columnRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.LegendLayout = LegendKind.Column;
			m_numberOfItemsUpDown.Value = WinChart.Legend.NumberOfItemsInColumn;
			WinChart.Invalidate();
		}

		private void m_numberOfItemsUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			if (WinChart.Legend.LegendLayout == LegendKind.Row) 
			{
				WinChart.Legend.NumberOfItemsInRow = (int)m_numberOfItemsUpDown.Value;
			} 
			else 
			{
				WinChart.Legend.NumberOfItemsInColumn = (int)m_numberOfItemsUpDown.Value;
			}
			WinChart.Invalidate();
		}

		private void m_isBackgroundVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.DrawBackgroundRectangle = m_isBackgroundVisibleCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_borderWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.BorderWidth = (int)m_borderWidthUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_borderShadeWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.BorderShadeWidth = (int)m_borderShadeWidthUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_itemBorderWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.ItemBorderWidth = (int)m_itemBorderWidthUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_itemBorderShadeWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.ItemBorderShadeWidth = (int)m_itemBorderShadeWidthUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_legendVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.Legend.Visible = m_legendVisibleCheckBox.Checked;
			m_fontGroupBox.Enabled = m_legendPropertiesPanel.Enabled = m_legendVisibleCheckBox.Checked;
			WinChart.Invalidate();
		}

		private void m_fontButton_Click(object sender, System.EventArgs e)
		{
			FontDialog fontDialog1 = new FontDialog();

			fontDialog1.Font = WinChart.Legend.Font;

			if(fontDialog1.ShowDialog() != DialogResult.Cancel )
			{
				WinChart.Legend.Font = fontDialog1.Font;
				WizardFrameControl.SetFontLabel(m_fontLabel, WinChart.Legend.Font);
				WinChart.Invalidate();
			}
		}
	}
}
