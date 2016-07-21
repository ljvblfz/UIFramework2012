using System;
using System.Windows.Forms;

using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardPaletteDialog : WizardDialog
	{
		private Design.ColorArrayControl m_primaryColors;
		private Design.ColorArrayControl m_secondaryColors;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private Design.ColorControl m_coordinateLineColorControlWithDots;
		private Design.ColorControl m_coordinatePlaneColorControlWithDots;
		private Design.ColorControl m_coodinateLabelFontColorControlWithDots;
		private Design.ColorControl m_dataLabelFontColorControlWithDots;
		private Design.ColorControl m_coordinatePlaneSecondaryColorControlWithDots;
		private ComponentArt.Win.UI.Internal.GroupBox m_pallettesGroupBox;
		private System.Windows.Forms.ListBox m_paletteListBox;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox1;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox2;
		private ComponentArt.Win.UI.Internal.Button m_paletteCollectionEditButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_backgroundColorControl;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_backgroundEndingColorControl;
		private System.Windows.Forms.Label label3;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_axisLineColorControl;
		private ComponentArt.Win.UI.Internal.Button m_loadPalettesButton;
		private ComponentArt.Win.UI.Internal.Button m_savePalettesButton;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private System.Windows.Forms.Panel m_specialColorsPanel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_legendBorderColorControl;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_legendFontColorControl;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_frameFontColorControl;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_titleFontColorControl;
		private System.Windows.Forms.Label label14;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_twoDObjectBorderColorControl;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_legendBackgroundFontColorControl;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_frameColorControl;
		private System.Windows.Forms.Label label15;
		private ComponentArt.Web.Visualization.Charting.Design.ColorControl m_frameSecondaryColorControl;
		private System.Windows.Forms.Label label16;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox3;
	
		private void InitializeComponent()
		{
			this.m_primaryColors = new ComponentArt.Web.Visualization.Charting.Design.ColorArrayControl();
			this.m_secondaryColors = new ComponentArt.Web.Visualization.Charting.Design.ColorArrayControl();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.m_coordinateLineColorControlWithDots = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_coordinatePlaneColorControlWithDots = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_coodinateLabelFontColorControlWithDots = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_dataLabelFontColorControlWithDots = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_coordinatePlaneSecondaryColorControlWithDots = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_pallettesGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_paletteListBox = new System.Windows.Forms.ListBox();
			this.groupBox1 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.groupBox2 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.groupBox3 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_specialColorsPanel = new System.Windows.Forms.Panel();
			this.m_frameSecondaryColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label16 = new System.Windows.Forms.Label();
			this.m_frameColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.m_twoDObjectBorderColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_legendBorderColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label6 = new System.Windows.Forms.Label();
			this.m_legendFontColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_legendBackgroundFontColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_frameFontColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.m_titleFontColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.m_backgroundColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label3 = new System.Windows.Forms.Label();
			this.m_axisLineColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.m_backgroundEndingColorControl = new ComponentArt.Web.Visualization.Charting.Design.ColorControl();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_paletteCollectionEditButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_loadPalettesButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_savePalettesButton = new ComponentArt.Win.UI.Internal.Button();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_pallettesGroupBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.m_specialColorsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_primaryColors
			// 
			this.m_primaryColors.ColorIndex = -1;
			this.m_primaryColors.Colors = null;
			this.m_primaryColors.Location = new System.Drawing.Point(5, 26);
			this.m_primaryColors.Name = "m_primaryColors";
			this.m_primaryColors.ReadOnly = true;
			this.m_primaryColors.Size = new System.Drawing.Size(142, 20);
			this.m_primaryColors.TabIndex = 5;
			this.m_primaryColors.TabStop = false;
			// 
			// m_secondaryColors
			// 
			this.m_secondaryColors.ColorIndex = -1;
			this.m_secondaryColors.Colors = null;
			this.m_secondaryColors.Location = new System.Drawing.Point(5, 26);
			this.m_secondaryColors.Name = "m_secondaryColors";
			this.m_secondaryColors.ReadOnly = true;
			this.m_secondaryColors.Size = new System.Drawing.Size(142, 20);
			this.m_secondaryColors.TabIndex = 7;
			this.m_secondaryColors.TabStop = false;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(26, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(114, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "Coordinate Plane";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(26, 17);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(114, 13);
			this.label5.TabIndex = 17;
			this.label5.Text = "Coordinate Line";
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.White;
			this.label7.Location = new System.Drawing.Point(26, 1);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(114, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Coordinate Label";
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.White;
			this.label8.Location = new System.Drawing.Point(26, 65);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(114, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "Data Label";
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.White;
			this.label9.Location = new System.Drawing.Point(26, 49);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(114, 13);
			this.label9.TabIndex = 21;
			this.label9.Text = "Coordinate Plane Sec";
			// 
			// m_coordinateLineColorControlWithDots
			// 
			this.m_coordinateLineColorControlWithDots.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coordinateLineColorControlWithDots.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coordinateLineColorControlWithDots.Location = new System.Drawing.Point(1, 17);
			this.m_coordinateLineColorControlWithDots.Name = "m_coordinateLineColorControlWithDots";
			this.m_coordinateLineColorControlWithDots.ReadOnly = true;
			this.m_coordinateLineColorControlWithDots.Size = new System.Drawing.Size(24, 12);
			this.m_coordinateLineColorControlWithDots.TabIndex = 24;
			this.m_coordinateLineColorControlWithDots.TabStop = false;
			// 
			// m_coordinatePlaneColorControlWithDots
			// 
			this.m_coordinatePlaneColorControlWithDots.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coordinatePlaneColorControlWithDots.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coordinatePlaneColorControlWithDots.Location = new System.Drawing.Point(1, 33);
			this.m_coordinatePlaneColorControlWithDots.Name = "m_coordinatePlaneColorControlWithDots";
			this.m_coordinatePlaneColorControlWithDots.ReadOnly = true;
			this.m_coordinatePlaneColorControlWithDots.Size = new System.Drawing.Size(24, 12);
			this.m_coordinatePlaneColorControlWithDots.TabIndex = 25;
			this.m_coordinatePlaneColorControlWithDots.TabStop = false;
			// 
			// m_coodinateLabelFontColorControlWithDots
			// 
			this.m_coodinateLabelFontColorControlWithDots.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coodinateLabelFontColorControlWithDots.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coodinateLabelFontColorControlWithDots.Location = new System.Drawing.Point(1, 1);
			this.m_coodinateLabelFontColorControlWithDots.Name = "m_coodinateLabelFontColorControlWithDots";
			this.m_coodinateLabelFontColorControlWithDots.ReadOnly = true;
			this.m_coodinateLabelFontColorControlWithDots.Size = new System.Drawing.Size(24, 12);
			this.m_coodinateLabelFontColorControlWithDots.TabIndex = 27;
			this.m_coodinateLabelFontColorControlWithDots.TabStop = false;
			// 
			// m_dataLabelFontColorControlWithDots
			// 
			this.m_dataLabelFontColorControlWithDots.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_dataLabelFontColorControlWithDots.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_dataLabelFontColorControlWithDots.Location = new System.Drawing.Point(1, 65);
			this.m_dataLabelFontColorControlWithDots.Name = "m_dataLabelFontColorControlWithDots";
			this.m_dataLabelFontColorControlWithDots.ReadOnly = true;
			this.m_dataLabelFontColorControlWithDots.Size = new System.Drawing.Size(24, 12);
			this.m_dataLabelFontColorControlWithDots.TabIndex = 30;
			this.m_dataLabelFontColorControlWithDots.TabStop = false;
			// 
			// m_coordinatePlaneSecondaryColorControlWithDots
			// 
			this.m_coordinatePlaneSecondaryColorControlWithDots.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coordinatePlaneSecondaryColorControlWithDots.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_coordinatePlaneSecondaryColorControlWithDots.Location = new System.Drawing.Point(1, 49);
			this.m_coordinatePlaneSecondaryColorControlWithDots.Name = "m_coordinatePlaneSecondaryColorControlWithDots";
			this.m_coordinatePlaneSecondaryColorControlWithDots.ReadOnly = true;
			this.m_coordinatePlaneSecondaryColorControlWithDots.Size = new System.Drawing.Size(24, 12);
			this.m_coordinatePlaneSecondaryColorControlWithDots.TabIndex = 29;
			this.m_coordinatePlaneSecondaryColorControlWithDots.TabStop = false;
			// 
			// m_pallettesGroupBox
			// 
			this.m_pallettesGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.m_paletteListBox});
			this.m_pallettesGroupBox.DrawBorderAroundControl = true;
			this.m_pallettesGroupBox.Name = "m_pallettesGroupBox";
			this.m_pallettesGroupBox.ResizeChildren = true;
			this.m_pallettesGroupBox.Size = new System.Drawing.Size(136, 194);
			this.m_pallettesGroupBox.TabIndex = 34;
			this.m_pallettesGroupBox.TabStop = false;
			this.m_pallettesGroupBox.Text = "Palettes";
			// 
			// m_paletteListBox
			// 
			this.m_paletteListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_paletteListBox.DisplayMember = "Name";
			this.m_paletteListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_paletteListBox.ItemHeight = 18;
			this.m_paletteListBox.Location = new System.Drawing.Point(2, 26);
			this.m_paletteListBox.Name = "m_paletteListBox";
			this.m_paletteListBox.Size = new System.Drawing.Size(132, 162);
			this.m_paletteListBox.TabIndex = 0;
			this.m_paletteListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.m_paletteListBox_DrawItem);
			this.m_paletteListBox.SelectedIndexChanged += new System.EventHandler(this.m_paletteListBox_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_primaryColors});
			this.groupBox1.Location = new System.Drawing.Point(144, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.ResizeChildren = false;
			this.groupBox1.Size = new System.Drawing.Size(160, 48);
			this.groupBox1.TabIndex = 35;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Primary Colors";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_secondaryColors});
			this.groupBox2.Location = new System.Drawing.Point(144, 52);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.ResizeChildren = false;
			this.groupBox2.Size = new System.Drawing.Size(160, 48);
			this.groupBox2.TabIndex = 36;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Secondary Colors";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_specialColorsPanel});
			this.groupBox3.DrawBorderAroundControl = true;
			this.groupBox3.Location = new System.Drawing.Point(144, 104);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.ResizeChildren = false;
			this.groupBox3.Size = new System.Drawing.Size(160, 152);
			this.groupBox3.TabIndex = 38;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Special Colors";
			// 
			// m_specialColorsPanel
			// 
			this.m_specialColorsPanel.AutoScroll = true;
			this.m_specialColorsPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_frameSecondaryColorControl,
																							   this.label16,
																							   this.m_frameColorControl,
																							   this.label15,
																							   this.label14,
																							   this.m_twoDObjectBorderColorControl,
																							   this.m_legendBorderColorControl,
																							   this.label6,
																							   this.m_legendFontColorControl,
																							   this.m_legendBackgroundFontColorControl,
																							   this.m_frameFontColorControl,
																							   this.label10,
																							   this.label11,
																							   this.m_titleFontColorControl,
																							   this.label12,
																							   this.label13,
																							   this.m_coordinatePlaneColorControlWithDots,
																							   this.m_backgroundColorControl,
																							   this.m_coordinateLineColorControlWithDots,
																							   this.label3,
																							   this.label4,
																							   this.m_axisLineColorControl,
																							   this.m_backgroundEndingColorControl,
																							   this.label5,
																							   this.m_coodinateLabelFontColorControlWithDots,
																							   this.m_dataLabelFontColorControlWithDots,
																							   this.label7,
																							   this.label8,
																							   this.label2,
																							   this.m_coordinatePlaneSecondaryColorControlWithDots,
																							   this.label9,
																							   this.label1});
			this.m_specialColorsPanel.Location = new System.Drawing.Point(3, 23);
			this.m_specialColorsPanel.Name = "m_specialColorsPanel";
			this.m_specialColorsPanel.Size = new System.Drawing.Size(157, 126);
			this.m_specialColorsPanel.TabIndex = 37;
			// 
			// m_frameSecondaryColorControl
			// 
			this.m_frameSecondaryColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_frameSecondaryColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_frameSecondaryColorControl.Location = new System.Drawing.Point(0, 176);
			this.m_frameSecondaryColorControl.Name = "m_frameSecondaryColorControl";
			this.m_frameSecondaryColorControl.ReadOnly = true;
			this.m_frameSecondaryColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_frameSecondaryColorControl.TabIndex = 52;
			this.m_frameSecondaryColorControl.TabStop = false;
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.Color.White;
			this.label16.Location = new System.Drawing.Point(25, 176);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(114, 13);
			this.label16.TabIndex = 51;
			this.label16.Text = "Frame Sec";
			// 
			// m_frameColorControl
			// 
			this.m_frameColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_frameColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_frameColorControl.Location = new System.Drawing.Point(0, 160);
			this.m_frameColorControl.Name = "m_frameColorControl";
			this.m_frameColorControl.ReadOnly = true;
			this.m_frameColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_frameColorControl.TabIndex = 50;
			this.m_frameColorControl.TabStop = false;
			// 
			// label15
			// 
			this.label15.BackColor = System.Drawing.Color.White;
			this.label15.Location = new System.Drawing.Point(25, 160);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(114, 13);
			this.label15.TabIndex = 49;
			this.label15.Text = "Frame";
			// 
			// label14
			// 
			this.label14.BackColor = System.Drawing.Color.White;
			this.label14.Location = new System.Drawing.Point(25, 240);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(114, 13);
			this.label14.TabIndex = 47;
			this.label14.Text = "2D Object Border";
			// 
			// m_twoDObjectBorderColorControl
			// 
			this.m_twoDObjectBorderColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_twoDObjectBorderColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_twoDObjectBorderColorControl.Location = new System.Drawing.Point(0, 240);
			this.m_twoDObjectBorderColorControl.Name = "m_twoDObjectBorderColorControl";
			this.m_twoDObjectBorderColorControl.ReadOnly = true;
			this.m_twoDObjectBorderColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_twoDObjectBorderColorControl.TabIndex = 48;
			this.m_twoDObjectBorderColorControl.TabStop = false;
			// 
			// m_legendBorderColorControl
			// 
			this.m_legendBorderColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_legendBorderColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_legendBorderColorControl.Location = new System.Drawing.Point(0, 192);
			this.m_legendBorderColorControl.Name = "m_legendBorderColorControl";
			this.m_legendBorderColorControl.ReadOnly = true;
			this.m_legendBorderColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_legendBorderColorControl.TabIndex = 42;
			this.m_legendBorderColorControl.TabStop = false;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(25, 224);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(114, 13);
			this.label6.TabIndex = 45;
			this.label6.Text = "Legend Font";
			// 
			// m_legendFontColorControl
			// 
			this.m_legendFontColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_legendFontColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_legendFontColorControl.Location = new System.Drawing.Point(0, 224);
			this.m_legendFontColorControl.Name = "m_legendFontColorControl";
			this.m_legendFontColorControl.ReadOnly = true;
			this.m_legendFontColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_legendFontColorControl.TabIndex = 46;
			this.m_legendFontColorControl.TabStop = false;
			// 
			// m_legendBackgroundFontColorControl
			// 
			this.m_legendBackgroundFontColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_legendBackgroundFontColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_legendBackgroundFontColorControl.Location = new System.Drawing.Point(0, 208);
			this.m_legendBackgroundFontColorControl.Name = "m_legendBackgroundFontColorControl";
			this.m_legendBackgroundFontColorControl.ReadOnly = true;
			this.m_legendBackgroundFontColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_legendBackgroundFontColorControl.TabIndex = 44;
			this.m_legendBackgroundFontColorControl.TabStop = false;
			// 
			// m_frameFontColorControl
			// 
			this.m_frameFontColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_frameFontColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_frameFontColorControl.Location = new System.Drawing.Point(0, 144);
			this.m_frameFontColorControl.Name = "m_frameFontColorControl";
			this.m_frameFontColorControl.ReadOnly = true;
			this.m_frameFontColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_frameFontColorControl.TabIndex = 40;
			this.m_frameFontColorControl.TabStop = false;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.White;
			this.label10.Location = new System.Drawing.Point(25, 144);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(114, 13);
			this.label10.TabIndex = 37;
			this.label10.Text = "Frame Font";
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.White;
			this.label11.Location = new System.Drawing.Point(25, 208);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(114, 13);
			this.label11.TabIndex = 43;
			this.label11.Text = "Legend Background";
			// 
			// m_titleFontColorControl
			// 
			this.m_titleFontColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_titleFontColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_titleFontColorControl.Location = new System.Drawing.Point(0, 128);
			this.m_titleFontColorControl.Name = "m_titleFontColorControl";
			this.m_titleFontColorControl.ReadOnly = true;
			this.m_titleFontColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_titleFontColorControl.TabIndex = 39;
			this.m_titleFontColorControl.TabStop = false;
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.White;
			this.label12.Location = new System.Drawing.Point(25, 128);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(114, 13);
			this.label12.TabIndex = 38;
			this.label12.Text = "Title Font";
			// 
			// label13
			// 
			this.label13.BackColor = System.Drawing.Color.White;
			this.label13.Location = new System.Drawing.Point(25, 192);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(114, 13);
			this.label13.TabIndex = 41;
			this.label13.Text = "Legend Border";
			// 
			// m_backgroundColorControl
			// 
			this.m_backgroundColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_backgroundColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_backgroundColorControl.Location = new System.Drawing.Point(1, 81);
			this.m_backgroundColorControl.Name = "m_backgroundColorControl";
			this.m_backgroundColorControl.ReadOnly = true;
			this.m_backgroundColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_backgroundColorControl.TabIndex = 32;
			this.m_backgroundColorControl.TabStop = false;
			this.m_backgroundColorControl.ColorChanged += new System.EventHandler(this.m_backgroundColorControl_ColorChanged);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(26, 113);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(114, 13);
			this.label3.TabIndex = 35;
			this.label3.Text = "Axis Line";
			// 
			// m_axisLineColorControl
			// 
			this.m_axisLineColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_axisLineColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_axisLineColorControl.Location = new System.Drawing.Point(1, 113);
			this.m_axisLineColorControl.Name = "m_axisLineColorControl";
			this.m_axisLineColorControl.ReadOnly = true;
			this.m_axisLineColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_axisLineColorControl.TabIndex = 36;
			this.m_axisLineColorControl.TabStop = false;
			// 
			// m_backgroundEndingColorControl
			// 
			this.m_backgroundEndingColorControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_backgroundEndingColorControl.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
			this.m_backgroundEndingColorControl.Location = new System.Drawing.Point(1, 97);
			this.m_backgroundEndingColorControl.Name = "m_backgroundEndingColorControl";
			this.m_backgroundEndingColorControl.ReadOnly = true;
			this.m_backgroundEndingColorControl.Size = new System.Drawing.Size(24, 12);
			this.m_backgroundEndingColorControl.TabIndex = 34;
			this.m_backgroundEndingColorControl.TabStop = false;
			this.m_backgroundEndingColorControl.ColorChanged += new System.EventHandler(this.m_backgroundEndingColorControl_ColorChanged);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(26, 97);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(114, 13);
			this.label2.TabIndex = 33;
			this.label2.Text = "Background Ending";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(26, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 13);
			this.label1.TabIndex = 31;
			this.label1.Text = "Background";
			// 
			// m_paletteCollectionEditButton
			// 
			this.m_paletteCollectionEditButton.BackColor = System.Drawing.Color.Transparent;
			this.m_paletteCollectionEditButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.m_paletteCollectionEditButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_paletteCollectionEditButton.Location = new System.Drawing.Point(0, 200);
			this.m_paletteCollectionEditButton.Name = "m_paletteCollectionEditButton";
			this.m_paletteCollectionEditButton.Size = new System.Drawing.Size(136, 23);
			this.m_paletteCollectionEditButton.TabIndex = 39;
			this.m_paletteCollectionEditButton.Text = "Edit Palettes...";
			this.m_paletteCollectionEditButton.Click += new System.EventHandler(this.m_paletteCollectionEditButton_Click);
			// 
			// m_loadPalettesButton
			// 
			this.m_loadPalettesButton.BackColor = System.Drawing.Color.Transparent;
			this.m_loadPalettesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.m_loadPalettesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_loadPalettesButton.Location = new System.Drawing.Point(0, 232);
			this.m_loadPalettesButton.Name = "m_loadPalettesButton";
			this.m_loadPalettesButton.Size = new System.Drawing.Size(64, 23);
			this.m_loadPalettesButton.TabIndex = 40;
			this.m_loadPalettesButton.Text = "Load...";
			this.m_loadPalettesButton.Click += new System.EventHandler(this.m_loadPalettesButton_Click);
			// 
			// m_savePalettesButton
			// 
			this.m_savePalettesButton.BackColor = System.Drawing.Color.Transparent;
			this.m_savePalettesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.m_savePalettesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_savePalettesButton.Location = new System.Drawing.Point(72, 232);
			this.m_savePalettesButton.Name = "m_savePalettesButton";
			this.m_savePalettesButton.Size = new System.Drawing.Size(64, 23);
			this.m_savePalettesButton.TabIndex = 41;
			this.m_savePalettesButton.Text = "Save...";
			this.m_savePalettesButton.Click += new System.EventHandler(this.m_savePalettesButton_Click);
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(136, 106);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(168, 3);
			this.separator1.TabIndex = 47;
			// 
			// WizardPaletteDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.separator1,
																		  this.m_savePalettesButton,
																		  this.m_loadPalettesButton,
																		  this.groupBox3,
																		  this.groupBox2,
																		  this.groupBox1,
																		  this.m_pallettesGroupBox,
																		  this.m_paletteCollectionEditButton});
			this.Name = "WizardPaletteDialog";
			this.Size = new System.Drawing.Size(320, 256);
			this.m_pallettesGroupBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.m_specialColorsPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	
		public WizardPaletteDialog()
		{
			InitializeComponent();

			m_primaryColors.PaletteChanged += new EventHandler(PaletteChanged);
			m_secondaryColors.PaletteChanged += new EventHandler(PaletteChanged);

			m_primaryColors.MouseEnter += new EventHandler(MouseEntered);
			m_secondaryColors.MouseEnter += new EventHandler(MouseEntered);

			m_primaryColors.PaletteColorIndexChanged += new EventHandler(PaletteColorIndexChanged);
			m_secondaryColors.PaletteColorIndexChanged += new EventHandler(PaletteColorIndexChanged);

			m_specialColorsPanel.AutoScroll = true;
		}


		private ColorArrayControl m_activeColorArrayControl = null;


		public void PaletteChanged(object sender, EventArgs e) 
		{
			PaletteColorIndexChanged(this, EventArgs.Empty);
			WinChart.Invalidate();
		}

		public void PaletteColorIndexChanged(object sender, EventArgs e) 
		{
		}

		/// <summary>
		/// Handles entry to ColorArrayControl
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MouseEntered(object sender, EventArgs e) 
		{
			if (sender == m_primaryColors) 
			{
				m_secondaryColors.ColorIndex = -1;
				m_secondaryColors.Invalidate();
				m_activeColorArrayControl = m_primaryColors;
				//m_samplePanel.BackColor
			}

			if (sender == m_secondaryColors) 
			{
				m_primaryColors.ColorIndex = -1;
				m_primaryColors.Invalidate();
				m_activeColorArrayControl = m_secondaryColors;
			}
		}

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (WinChart == null)
				return;
			
			int index = WinChart.Palettes.IndexOf(WinChart.SelectedPaletteName);
			//Collection = WinChart.Palettes;
			m_paletteListBox.Items.Clear();
			m_paletteListBox.Items.AddRange(WinChart.Palettes.GetNames());
			m_paletteListBox.SelectedIndex = index;
		}


		public Palette Palette 
		{
			get 
			{
				/*return (Palette)m_paletteListBox.SelectedItem;*/
				return WinChart.Palettes[m_paletteListBox.Text];
			}
		}

		private void m_paletteListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_paletteListBox.SelectedIndex == -1)
				return;

			m_primaryColors.Colors = Palette.PrimaryColors;
			m_secondaryColors.Colors = Palette.SecondaryColors;
			m_primaryColors.Invalidate();
			m_secondaryColors.Invalidate();
		
			m_coodinateLabelFontColorControlWithDots.Color = Palette.CoodinateLabelFontColor;
			m_coordinatePlaneColorControlWithDots.Color = Palette.CoordinatePlaneColor;
			m_coordinatePlaneSecondaryColorControlWithDots.Color = Palette.CoordinatePlaneSecondaryColor;
			m_coordinateLineColorControlWithDots.Color = Palette.CoordinateLineColor;
			m_dataLabelFontColorControlWithDots.Color = Palette.DataLabelFontColor;
			m_axisLineColorControl.Color = Palette.AxisLineColor;

			m_backgroundColorControl.Color = Palette.BackgroundColor;
			m_backgroundEndingColorControl.Color = Palette.BackgroundEndingColor;

			m_titleFontColorControl.Color = Palette.TitleFontColor;
			m_frameFontColorControl.Color = Palette.FrameFontColor;
			m_frameColorControl.Color = Palette.FrameColor;
			m_frameSecondaryColorControl.Color = Palette.FrameSecondaryColor;

			m_legendBackgroundFontColorControl.Color = Palette.LegendBackgroundColor;
			m_legendBorderColorControl.Color = Palette.LegendBorderColor;
			m_legendFontColorControl.Color = Palette.LegendFontColor;
			m_twoDObjectBorderColorControl.Color = Palette.TwoDObjectBorderColor;

			WinChart.SelectedPaletteName = Palette.Name;
			WinChart.Invalidate();
		}

		private void m_paletteCollectionEditButton_Click(object sender, System.EventArgs e)
		{
			InvokeEditor(WinChart.Palettes);

			m_paletteListBox.Items.Clear();
			m_paletteListBox.Items.AddRange(WinChart.Palettes.GetNames());
				
			WinChart.Invalidate();
		}

		private void m_backgroundColorControl_ColorChanged(object sender, System.EventArgs e)
		{
			Palette.BackgroundColor = m_backgroundColorControl.Color;
		}

		private void m_backgroundEndingColorControl_ColorChanged(object sender, System.EventArgs e)
		{
			Palette.BackgroundEndingColor = m_backgroundEndingColorControl.Color;
		}

		static private string getLoadFileName()
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = "XML Files(*.XML)|*.XML|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true ;

			if(openFileDialog1.ShowDialog() == DialogResult.OK)
				return openFileDialog1.FileName;
			else
				return "";
		}

		static private string getSaveFileName()
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
 
			saveFileDialog1.Filter = "XML Files(*.XML)|*.XML|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 1 ;
			saveFileDialog1.RestoreDirectory = true ;
 
			if(saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				return saveFileDialog1.FileName;
			} 
			else
				return "";
		}

		internal static bool LoadPalettes(PaletteCollection pc) 
		{
			try 
			{
				string palettesFileName = getLoadFileName();
				if (palettesFileName == "")
					return false;

				PaletteCollection newPC = new PaletteCollection();
				newPC.InCollectionEditor = true;
				newPC.Clear();
				newPC.InCollectionEditor = false;
				XmlCustomSerializer.Read(palettesFileName, newPC);

				DialogResult dr = MessageBox.Show("Overwrite existing palettes? (Click 'No' for palettes to be appended)", "Load Palettes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (dr == DialogResult.Cancel)
					return false;

				if (dr == DialogResult.Yes) 
				{
					pc.InCollectionEditor = true;
					pc.Clear();
					pc.InCollectionEditor = false;
				}
				foreach(Palette pal in newPC)
					pc.Add(pal);
				
			} 
			catch (Exception ex)
			{
				if(ex.InnerException != null)
					MessageBox.Show(ex.InnerException.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				else
					MessageBox.Show(ex.Message);
			}
			return true;
		}

		internal static void SavePalettes(PaletteCollection pc) 
		{
			try 
			{
				string palettesFileName = getSaveFileName();
				if (palettesFileName == "")
					return;
				XmlCustomSerializer.Write(palettesFileName, pc, "Palettes");
			} 
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace, ex.Message);
				throw;
			}
		}

		private void m_loadPalettesButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				string zz = m_paletteListBox.DisplayMember;

				string selectedName = WinChart.SelectedPaletteName;
			
				bool proceed = LoadPalettes(WinChart.Palettes);

				if (!proceed)
					return;
				int index = WinChart.Palettes.IndexOf(selectedName);

				m_paletteListBox.Items.Clear();
				m_paletteListBox.Items.AddRange(WinChart.Palettes.GetNames());
			
				int setIndex = (index>=0?index:0);
				m_paletteListBox.SelectedIndex = setIndex;
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.StackTrace, ex.Message);
				throw;
			}
		}

		private void m_savePalettesButton_Click(object sender, System.EventArgs e)
		{
			SavePalettes(WinChart.Palettes);
		}

		private void m_paletteListBox_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{		

			Color color1 = Color.White;
			Color color2 = Color.Black;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				color1 = Color.FromArgb(221, 52, 9);
				color2 = Color.White;
			}

			e.Graphics.FillRectangle(new SolidBrush(color1), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
			
			// Draw the current item text based on the current Font and the custom brush settings.
			e.Graphics.DrawString(m_paletteListBox.Items[e.Index].ToString(), m_paletteListBox.Font, new SolidBrush(color2) ,new Point(e.Bounds.X+22, e.Bounds.Y + 2));
			
			e.Graphics.DrawRectangle(Pens.Black, e.Bounds.X, e.Bounds.Y+1, 21, 15);

			((UITypeEditor)TypeDescriptor.GetEditor(typeof (Palette), typeof(UITypeEditor))).PaintValue(new PaintValueEventArgs(null, WinChart.Palettes[e.Index], e.Graphics, new Rectangle(e.Bounds.X+1, e.Bounds.Y+2, 20, 14)));

			// If the ListBox has focus, draw a focus rectangle around the selected item.
			e.DrawFocusRectangle();

		}
	}
}
