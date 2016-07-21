using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for GaugeWizardForm.
	/// </summary>
	internal class GaugeWizardForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ComponentArt.Web.Visualization.Gauges.GaugeTreeControl gaugeTreeControl;
		private ComponentArt.Web.Visualization.Gauges.PreviewControl gaugePreviewControl;
		private ComponentArt.WinUI.Button startButton;
		private ComponentArt.WinUI.Button scalesButton;
		private ComponentArt.WinUI.Button rangesButton;
		private ComponentArt.WinUI.Button pointersButton;
		private ComponentArt.WinUI.Button subGaugesButton;
		private ComponentArt.WinUI.Button indicatorsButton;
		private ComponentArt.WinUI.Button annotationsButton;
		private ComponentArt.WinUI.Button backButton;
		private ComponentArt.WinUI.Button nextButton;
		private ComponentArt.WinUI.Button applyButton;
		private ComponentArt.WinUI.Button cancelButton;
		private ComponentArt.WinUI.Button finishButton;
		private ComponentArt.WinUI.Button closeButton;
		private ComponentArt.Web.Visualization.Gauges.StartTab startTab;
		private ComponentArt.Web.Visualization.Gauges.ScalesTab scalesTab;
		private ComponentArt.Web.Visualization.Gauges.RangesTab rangesTab;
		private ComponentArt.Web.Visualization.Gauges.PointersTab pointersTab;
		private ComponentArt.Web.Visualization.Gauges.SubGaugesTab subGaugesTab;
		private ComponentArt.Web.Visualization.Gauges.IndicatorsTab indicatorsTab;
		private ComponentArt.Web.Visualization.Gauges.AnnotationsTab annotationsTab;
		private ComponentArt.Web.Visualization.Gauges.CollectionsTab collectionsTab;
		private ComponentArt.WinUI.GroupBox TreeGroupBox;

		private SubGauge m_gaugeClone = null;
		private bool m_activated = false;
		private System.Windows.Forms.PictureBox skinPictureBox;
		private System.Windows.Forms.PictureBox titleBarPictureBox;

#if WEB
        private Gauge m_gauge = null;
		public GaugeWizardForm(Gauge gauge)
#else
        private WinGauge m_gauge = null;
		public GaugeWizardForm(WinGauge gauge)
#endif	
		{
			m_gauge = gauge;

			TakeSnapshot();
			// Required for Windows Form Designer support
			InitializeComponent();

			MyInitialization();

			SelectStartTab();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.startButton = new ComponentArt.WinUI.Button();
			this.scalesButton = new ComponentArt.WinUI.Button();
			this.rangesButton = new ComponentArt.WinUI.Button();
			this.pointersButton = new ComponentArt.WinUI.Button();
			this.subGaugesButton = new ComponentArt.WinUI.Button();
			this.indicatorsButton = new ComponentArt.WinUI.Button();
			this.annotationsButton = new ComponentArt.WinUI.Button();
			this.backButton = new ComponentArt.WinUI.Button();
			this.nextButton = new ComponentArt.WinUI.Button();
			this.applyButton = new ComponentArt.WinUI.Button();
			this.cancelButton = new ComponentArt.WinUI.Button();
			this.finishButton = new ComponentArt.WinUI.Button();
			this.closeButton = new ComponentArt.WinUI.Button();
			this.startTab = new ComponentArt.Web.Visualization.Gauges.StartTab();
			this.scalesTab = new ComponentArt.Web.Visualization.Gauges.ScalesTab();
			this.rangesTab = new ComponentArt.Web.Visualization.Gauges.RangesTab();
			this.pointersTab = new ComponentArt.Web.Visualization.Gauges.PointersTab();
			this.subGaugesTab = new ComponentArt.Web.Visualization.Gauges.SubGaugesTab();
			this.indicatorsTab = new ComponentArt.Web.Visualization.Gauges.IndicatorsTab();
			this.annotationsTab = new ComponentArt.Web.Visualization.Gauges.AnnotationsTab();
			this.collectionsTab = new ComponentArt.Web.Visualization.Gauges.CollectionsTab();
			this.gaugeTreeControl = new ComponentArt.Web.Visualization.Gauges.GaugeTreeControl();
			this.gaugePreviewControl = new ComponentArt.Web.Visualization.Gauges.PreviewControl();
			this.skinPictureBox = new System.Windows.Forms.PictureBox();
			this.titleBarPictureBox = new System.Windows.Forms.PictureBox();
			this.TreeGroupBox = new ComponentArt.WinUI.GroupBox();
			this.SuspendLayout();
			// 
			// skinPictureBox
			// 
			this.skinPictureBox.Location = new System.Drawing.Point(0, 25);
			this.skinPictureBox.Name = "skinPictureBox";
			this.skinPictureBox.Size = new System.Drawing.Size(700, 475);
			this.skinPictureBox.TabStop = false;
			// 
			// titleBarPictureBox
			// 
			this.titleBarPictureBox.Location = new System.Drawing.Point(0, 0);
			this.titleBarPictureBox.Name = "titleBarPictureBox";
			this.titleBarPictureBox.Size = new System.Drawing.Size(700, 25);
			this.titleBarPictureBox.TabStop = false;
			this.titleBarPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleBarPictureBox_MouseUp);
			this.titleBarPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBarPictureBox_MouseMove);
			this.titleBarPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBarPictureBox_MouseDown);
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(6, 90);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(77, 25);
			this.startButton.TabStop = false;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// scalesButton
			// 
			this.scalesButton.Location = new System.Drawing.Point(84, 90);
			this.scalesButton.Name = "scalesButton";
			this.scalesButton.Size = new System.Drawing.Size(86, 25);
			this.scalesButton.TabStop = false;
			this.scalesButton.Click += new System.EventHandler(this.scalesButton_Click);
			// 
			// rangesButton
			// 
			this.rangesButton.Location = new System.Drawing.Point(171, 90);
			this.rangesButton.Name = "rangesButton";
			this.rangesButton.Size = new System.Drawing.Size(91, 25);
			this.rangesButton.TabStop = false;
			this.rangesButton.Click += new System.EventHandler(this.rangesButton_Click);
			// 
			// pointersButton
			// 
			this.pointersButton.Location = new System.Drawing.Point(263, 90);
			this.pointersButton.Name = "pointersButton";
			this.pointersButton.Size = new System.Drawing.Size(94, 25);
			this.pointersButton.TabStop = false;
			this.pointersButton.Click += new System.EventHandler(this.pointersButton_Click);
			// 
			// subGaugesButton
			// 
			this.subGaugesButton.Location = new System.Drawing.Point(358, 90);
			this.subGaugesButton.Name = "subGaugesButton";
			this.subGaugesButton.Size = new System.Drawing.Size(118, 25);
			this.subGaugesButton.TabStop = false;
			this.subGaugesButton.Click += new System.EventHandler(this.subGaugesButton_Click);
			// 
			// indicatorsButton
			// 
			this.indicatorsButton.Location = new System.Drawing.Point(477, 90);
			this.indicatorsButton.Name = "indicatorsButton";
			this.indicatorsButton.Size = new System.Drawing.Size(103, 25);
			this.indicatorsButton.TabStop = false;
			this.indicatorsButton.Click += new System.EventHandler(this.indicatorsButton_Click);
			// 
			// annotationsButton
			// 
			this.annotationsButton.Location = new System.Drawing.Point(581, 90);
			this.annotationsButton.Name = "annotationsButton";
			this.annotationsButton.Size = new System.Drawing.Size(113, 25);
			this.annotationsButton.TabStop = false;
			this.annotationsButton.Click += new System.EventHandler(this.annotationsButton_Click);
			// 
			// backButton
			// 
			this.backButton.Location = new System.Drawing.Point(331, 466);
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(65, 21);
			this.backButton.TabIndex = 50;
			this.backButton.Click += new System.EventHandler(this.backButton_Click);
			// 
			// nextButton
			// 
			this.nextButton.Location = new System.Drawing.Point(404, 466);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(65, 21);
			this.nextButton.TabIndex = 51;
			this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(477, 466);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(65, 21);
			this.applyButton.TabIndex = 52;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(550, 466);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(65, 21);
			this.cancelButton.TabIndex = 53;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// finishButton
			// 
			this.finishButton.Location = new System.Drawing.Point(623, 466);
			this.finishButton.Name = "finishButton";
			this.finishButton.Size = new System.Drawing.Size(65, 21);
			this.finishButton.TabIndex = 54;
			this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(679, 5);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(16, 16);
			this.closeButton.TabStop = false;
			this.closeButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// startTab
			// 
			this.startTab.Location = new System.Drawing.Point(26, 122);
			this.startTab.Name = "startTab";
			this.startTab.Size = new System.Drawing.Size(646, 328);
			this.startTab.TabIndex = 1;
			this.startTab.Visible = false;
			this.startTab.Changed += new ComponentArt.Web.Visualization.Gauges.StartTab.TabEventHandler(this.TabData_Changed);
			// 
			// scalesTab
			// 
			this.scalesTab.Location = new System.Drawing.Point(189, 122);
			this.scalesTab.Name = "scalesTab";
			this.scalesTab.Size = new System.Drawing.Size(256, 328);
			this.scalesTab.TabIndex = 2;
			this.scalesTab.Visible = false;
			this.scalesTab.Changed += new ComponentArt.Web.Visualization.Gauges.ScalesTab.TabEventHandler(this.TabData_Changed);
			// 
			// rangesTab
			// 
			this.rangesTab.Location = new System.Drawing.Point(189, 122);
			this.rangesTab.Name = "rangesTab";
			this.rangesTab.TabIndex = 3;
			this.rangesTab.Visible = false;
			this.rangesTab.Changed += new ComponentArt.Web.Visualization.Gauges.RangesTab.TabEventHandler(this.TabData_Changed);
			this.rangesTab.TabSelected += new ComponentArt.Web.Visualization.Gauges.RangesTab.TabEventHandlerWithArgs(this.Tabs_TabSelected);
			// 
			// pointersTab
			// 
			this.pointersTab.Location = new System.Drawing.Point(189, 122);
			this.pointersTab.Name = "pointersTab";
			this.pointersTab.Size = new System.Drawing.Size(256, 328);
			this.pointersTab.TabIndex = 4;
			this.pointersTab.Visible = false;
			this.pointersTab.Changed += new ComponentArt.Web.Visualization.Gauges.PointersTab.TabEventHandler(this.TabData_Changed);
			// 
			// subGaugesTab
			// 
			this.subGaugesTab.Location = new System.Drawing.Point(189, 122);
			this.subGaugesTab.Name = "subGaugesTab";
			this.subGaugesTab.Size = new System.Drawing.Size(256, 328);
			this.subGaugesTab.TabIndex = 5;
			this.subGaugesTab.Visible = false;
			this.subGaugesTab.Changed += new ComponentArt.Web.Visualization.Gauges.SubGaugesTab.TabEventHandler(this.TabData_Changed);
			// 
			// indicatorsTab
			// 
			this.indicatorsTab.Location = new System.Drawing.Point(189, 122);
			this.indicatorsTab.Name = "indicatorsTab";
			this.indicatorsTab.Size = new System.Drawing.Size(256, 328);
			this.indicatorsTab.TabIndex = 6;
			this.indicatorsTab.Visible = false;
			this.indicatorsTab.Changed += new ComponentArt.Web.Visualization.Gauges.IndicatorsTab.TabEventHandler(this.TabData_Changed);
			// 
			// annotationsTab
			// 
			this.annotationsTab.Location = new System.Drawing.Point(189, 122);
			this.annotationsTab.Name = "annotationsTab";
			this.annotationsTab.Size = new System.Drawing.Size(256, 328);
			this.annotationsTab.TabIndex = 7;
			this.annotationsTab.Visible = false;
			this.annotationsTab.Changed += new ComponentArt.Web.Visualization.Gauges.AnnotationsTab.TabEventHandler(this.TabData_Changed);
			this.annotationsTab.TabSelected += new ComponentArt.Web.Visualization.Gauges.AnnotationsTab.TabEventHandlerWithArgs(this.Tabs_TabSelected);
			// 
			// collectionsTab
			// 
			this.collectionsTab.Location = new System.Drawing.Point(189, 122);
			this.collectionsTab.Name = "collectionsTab";
			this.collectionsTab.Size = new System.Drawing.Size(256, 328);
			this.collectionsTab.TabStop = false;
			this.collectionsTab.Visible = false;
			// 
			// gaugeTreeControl
			// 
			this.gaugeTreeControl.Location = new System.Drawing.Point(3, 23);
			this.gaugeTreeControl.Name = "gaugeTreeControl";
			this.gaugeTreeControl.Size = new System.Drawing.Size(164, 303);
			this.gaugeTreeControl.TabIndex = 1;
			this.gaugeTreeControl.AfterSelect += new ComponentArt.Web.Visualization.Gauges.GaugeTreeControl.GaugeTreeViewEventHandlerWithArgs(this.OnTreeObjectSelect);
			this.gaugeTreeControl.Changed += new ComponentArt.Web.Visualization.Gauges.GaugeTreeControl.GaugeTreeViewEventHandler(this.TabData_Changed);
			// 
			// TreeGroupBox
			// 
			this.TreeGroupBox.Location = new System.Drawing.Point(10, 122);
			this.TreeGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.gaugeTreeControl});
			this.TreeGroupBox.Name = "TreeGroupBox";
			this.TreeGroupBox.ResizeChildren = false;
			this.TreeGroupBox.Size = new System.Drawing.Size(170, 329);
			this.TreeGroupBox.TabIndex = 0;
			this.TreeGroupBox.Text = "Object Hierarchy";
			// 
			// gaugePreviewControl
			// 
			this.gaugePreviewControl.Location = new System.Drawing.Point(451, 172);
			this.gaugePreviewControl.Name = "gaugePreviewControl";
			this.gaugePreviewControl.Size = new System.Drawing.Size(238, 228);
			this.gaugePreviewControl.TabStop = false;
			this.gaugePreviewControl.Visible = false;
			// 
			// GaugeWizardForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.ClientSize = new System.Drawing.Size(700, 500);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.collectionsTab,
																		  this.annotationsTab,
																		  this.indicatorsTab,
																		  this.subGaugesTab,
																		  this.pointersTab,
																		  this.rangesTab,
																		  this.scalesTab,
																		  this.startTab,
																		  this.closeButton,
																		  this.finishButton,
																		  this.cancelButton,
																		  this.applyButton,
																		  this.nextButton,
																		  this.backButton,
																		  this.annotationsButton,
																		  this.indicatorsButton,
																		  this.subGaugesButton,
																		  this.pointersButton,
																		  this.rangesButton,
																		  this.scalesButton,
																		  this.startButton,
																		  this.gaugePreviewControl,
																		  this.TreeGroupBox,
																		  this.titleBarPictureBox,
																		  this.skinPictureBox});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "GaugeWizardForm";
			this.Text = "GaugeWizardForm";
			this.Activated += new System.EventHandler(this.GaugeWizardForm_Activated);
			this.ResumeLayout(false);

		}
		#endregion

		void MyInitialization()
		{
			this.SuspendLayout();

			SetupButtonImages();
			gaugeTreeControl.Gauge = m_gauge;
			gaugeTreeControl.PopulateTree();

			gaugePreviewControl.Control = (IDrawableControl) m_gauge;
			startTab.Gauge = m_gauge;
			this.ResumeLayout(false);
		}

		private void SetupButtonImages()
		{
			System.IO.Stream stream;
			Bitmap bitmap;

			// ********** Tab Buttons **********
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.start.png");
			this.startButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);		
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.start-hover.png");
			this.startButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);	
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.start-selected.png");
			this.startButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.scales.png");
			this.scalesButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.scales-hover.png");
			this.scalesButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);	
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.scales-selected.png");
			this.scalesButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.ranges.png");
			this.rangesButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);	
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.ranges-hover.png");
			this.rangesButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);	
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.ranges-selected.png");
			this.rangesButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.pointers.png");
			this.pointersButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.pointers-hover.png");
			this.pointersButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.pointers-selected.png");
			this.pointersButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.subGauges.png");
			this.subGaugesButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.subGauges-hover.png");
			this.subGaugesButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.subGauges-selected.png");
			this.subGaugesButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.indicators.png");
			this.indicatorsButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.indicators-hover.png");
			this.indicatorsButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.indicators-selected.png");
			this.indicatorsButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.annotations.png");
			this.annotationsButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);
			
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.annotations-hover.png");
			this.annotationsButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);	
		
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.navButtons.annotations-selected.png");
			this.annotationsButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);	


			// ********** Bottm Right Chrome Buttons *********

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.back.png");
			this.backButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.back-hover.png");
			this.backButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.back-down.png");
			this.backButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.next.png");
			this.nextButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.next-hover.png");
			this.nextButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.next-down.png");
			this.nextButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.apply.png");
			this.applyButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.apply-hover.png");
			this.applyButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.apply-down.png");
			this.applyButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.cancel.png");
			this.cancelButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.cancel-hover.png");
			this.cancelButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.cancel-down.png");
			this.cancelButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.finish.png");
			this.finishButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.finish-hover.png");
			this.finishButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.finish-down.png");
			this.finishButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.close.png");
			this.closeButton.NormalIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.close-hover.png");
			this.closeButton.HoverIcon = (Bitmap)Bitmap.FromStream(stream);

			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.chromeButtons.close-down.png");
			this.closeButton.DownIcon = (Bitmap)Bitmap.FromStream(stream);

			this.backButton.ImageIndex = 0;
			this.nextButton.ImageIndex = 0;
			this.applyButton.ImageIndex = 0;
			this.cancelButton.ImageIndex = 0;
			this.finishButton.ImageIndex = 0;

			// ********** Skin Image **********
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.wizardSkin.png");
			bitmap = (Bitmap)Bitmap.FromStream(stream);
			this.skinPictureBox.Size = bitmap.Size;
			this.skinPictureBox.Image = bitmap;

			// ********** TitleBar Image **********
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.wizardTitleBar.png");
			bitmap = (Bitmap)Bitmap.FromStream(stream);
			this.titleBarPictureBox.Size = bitmap.Size;
			this.titleBarPictureBox.Image = bitmap;
		}

		private void GaugeWizardForm_Activated(object sender, System.EventArgs e)
		{
			if (m_activated)
				return;
			m_activated = true;
			Refresh();
			startTab.GeneratePreviewImages();
		}

		private void OnTreeObjectSelect(object source, TreeNodeSelectEventArgs e)
		{
			if (e.SelectedObject is NamedObjectCollection)
			{
				SelectCollectionsTab((NamedObjectCollection)e.SelectedObject);
			}
			else if (e.SelectedObject is Scale)
			{
				SelectScalesTab((Scale)e.SelectedObject);
			}
			else if (e.SelectedObject is Range)
			{
				SelectRangesTab((Range)e.SelectedObject);
			}			
			else if (e.SelectedObject is Annotation)
			{
				SelectAnnotationsSubTab((Annotation)e.SelectedObject);
			}
			else if (e.SelectedObject is Pointer)
			{
				SelectPointersTab((Pointer)e.SelectedObject);
			}
			else if (e.SelectedObject is SubGauge)
			{
				SelectSubGaugesTab((SubGauge)e.SelectedObject);
			}
			else if (e.SelectedObject is Indicator)
			{
				SelectIndicatorsTab((Indicator)e.SelectedObject);
			}
			else if (e.SelectedObject is ImageAnnotation)
			{
				SelectImageAnnotationsSubTab((ImageAnnotation)e.SelectedObject);
			}
			else if (e.SelectedObject is TextAnnotation)
			{
				SelectTextAnnotationsSubTab((TextAnnotation)e.SelectedObject);
			}
		}

		private void TabData_Changed(object source)
		{
			gaugePreviewControl.Invalidate();
		}

		private void startButton_Click(object sender, System.EventArgs e)
		{
			SelectStartTab();
		}

		private void scalesButton_Click(object sender, System.EventArgs e)
		{
			if (m_gauge.MainScale == null)
			{
				gaugeTreeControl.SelectNode(m_gauge.Scales);
				SelectCollectionsTab(m_gauge.Scales);
			}
			else
			{
				gaugeTreeControl.SelectNode(m_gauge.MainScale);
				SelectScalesTab(m_gauge.MainScale);
			}
		}

		private void rangesButton_Click(object sender, System.EventArgs e)
		{
			if (m_gauge.MainScale == null)
			{
				gaugeTreeControl.SelectNode(m_gauge.Scales);
				SelectCollectionsTab(m_gauge.Scales);
			}
			else if (m_gauge.MainScale.MainRange == null)
			{
				gaugeTreeControl.SelectNode(m_gauge.MainScale.Ranges);
				SelectCollectionsTab(m_gauge.MainScale.Ranges);
			}
			else
			{
				gaugeTreeControl.SelectNode(m_gauge.MainScale.MainRange);
				SelectRangesTab(m_gauge.MainScale.MainRange);
			}
		}

		private void pointersButton_Click(object sender, System.EventArgs e)
		{
			if (m_gauge.MainScale == null)
			{
				gaugeTreeControl.SelectNode(m_gauge.Scales);
				SelectCollectionsTab(m_gauge.Scales);
			}
			else if (m_gauge.MainScale.MainRange == null)
			{
				gaugeTreeControl.SelectNode(m_gauge.MainScale.Pointers);
				SelectCollectionsTab(m_gauge.MainScale.Pointers);
			}
			else
			{
				gaugeTreeControl.SelectNode(m_gauge.MainScale.MainPointer);
				SelectPointersTab(m_gauge.MainScale.MainPointer);
			}

		}

		private void subGaugesButton_Click(object sender, System.EventArgs e)
		{
			if(m_gauge.SubGauges.Count > 0)
			{
				gaugeTreeControl.SelectNode(m_gauge.SubGauges[0]);
				SelectSubGaugesTab(m_gauge.SubGauges[0]);
			}
			else
			{
				gaugeTreeControl.SelectNode(m_gauge.SubGauges);
				SelectCollectionsTab(m_gauge.SubGauges);
			}
		}

		private void indicatorsButton_Click(object sender, System.EventArgs e)
		{
			if(m_gauge.Indicators.Count > 0)
			{
				gaugeTreeControl.SelectNode(m_gauge.Indicators[0]);
				SelectIndicatorsTab(m_gauge.Indicators[0]);
			}
			else
			{
				gaugeTreeControl.SelectNode(m_gauge.Indicators);
				SelectCollectionsTab(m_gauge.Indicators);
			}

		}

		private void annotationsButton_Click(object sender, System.EventArgs e)
		{
			if(m_gauge.ImageAnnotations.Count > 0)
			{
				gaugeTreeControl.SelectNode(m_gauge.ImageAnnotations[0]);
				SelectImageAnnotationsSubTab(m_gauge.ImageAnnotations[0]);
			}
			else if (m_gauge.TextAnnotations.Count > 0)
			{
				annotationsTab.TextAnnotation = m_gauge.TextAnnotations[0];
				SelectTextAnnotationsSubTab(m_gauge.TextAnnotations[0]);
			}
			else
			{
				gaugeTreeControl.SelectNode(m_gauge.TextAnnotations);
				SelectCollectionsTab(m_gauge.TextAnnotations);
			}
		}

		private void SelectStartTab()
		{
			HideTabs();
			this.startButton.Depressed = true;
			startTab.Visible = true;
		}

		private void SelectScalesTab(Scale s)
		{
			HideTabs();
			if(scalesTab.Scale != s)
				scalesTab.Scale = s;
			this.scalesButton.Depressed = true;
			scalesTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
		}

		private void SelectRangesTab(Range r)
		{
			HideTabs();
			//if(rangesTab.Range != r)
				rangesTab.Range = r;
			this.rangesButton.Depressed = true;
			rangesTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
		}
		
		private void SelectAnnotationsSubTab(Annotation a)
		{
			HideTabs();
			//if (rangesTab.Annotation != a)
				rangesTab.Annotation = a;
			this.rangesButton.Depressed = true;
			rangesTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
		}

		private void SelectPointersTab(Pointer p)
		{
			HideTabs();
			if (pointersTab.Pointer != p)
				pointersTab.Pointer = p;
			this.pointersButton.Depressed = true;
			pointersTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
		}

		private void SelectSubGaugesTab(SubGauge g)
		{
			HideTabs();
			if(g != null && subGaugesTab.Gauge != g)
				subGaugesTab.Gauge = g;
			this.subGaugesButton.Depressed = true;
			subGaugesTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
		}

		private void SelectIndicatorsTab(Indicator i)
		{
			HideTabs();
			if(i != null && indicatorsTab.Indicator != i)
				indicatorsTab.Indicator = i;
			this.indicatorsButton.Depressed = true;
			indicatorsTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
		}

		private void SelectImageAnnotationsSubTab(ImageAnnotation ia)
		{
			HideTabs();
			this.annotationsButton.Depressed = true;
			annotationsTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
			if(ia != null/* && annotationsTab.ImageAnnotation != ia*/)
			{
				annotationsTab.ImageAnnotation = ia;
			}
		}

		private void SelectTextAnnotationsSubTab(TextAnnotation ta)
		{
			HideTabs();
			this.annotationsButton.Depressed = true;
			annotationsTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
			if(ta != null/* && annotationsTab.TextAnnotation != ta*/)
			{
				annotationsTab.TextAnnotation = ta;
			}
		}

		private void SelectImageAnnotationsSubTab(ImageAnnotationCollection iac)
		{
			HideTabs();
			this.annotationsButton.Depressed = true;
			annotationsTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
			if(iac != null/* && annotationsTab.ImageAnnotationCollection != iac*/)
			{
				annotationsTab.ImageAnnotationCollection = iac;
			}
		}

		private void SelectTextAnnotationsSubTab(TextAnnotationCollection tac)
		{
			HideTabs();
			this.annotationsButton.Depressed = true;
			annotationsTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
			if(tac != null/* && annotationsTab.TextAnnotationCollection != tac*/)
			{
				annotationsTab.TextAnnotationCollection = tac;
			}
		}

		private void SelectCollectionsTab(NamedObjectCollection collection)
		{
			HideTabs();

			collectionsTab.Visible = true;
			gaugePreviewControl.Visible = true;
			//gaugeTreeControl.Visible = true;
			this.TreeGroupBox.Visible = true;
			if (collection is ScaleCollection)
			{
				this.scalesButton.Depressed = true;
				collectionsTab.ObjectName = "Scale";
			}
			else if (collection is RangeCollection)
			{
				this.rangesButton.Depressed = true;
				collectionsTab.ObjectName = "Range";
			}
			else if (collection is AnnotationCollection)
			{
				this.rangesButton.Depressed = true;
				collectionsTab.ObjectName = "Annotation";
			}
			else if (collection is PointerCollection)
			{
				this.pointersButton.Depressed = true;
				collectionsTab.ObjectName = "Pointer";
			}
			else if (collection is SubGaugeCollection)
			{
				this.subGaugesButton.Depressed = true;
				collectionsTab.ObjectName = "Sub-Gauge";
			}
			else if (collection is IndicatorCollection)
			{
				this.indicatorsButton.Depressed = true;
				collectionsTab.ObjectName = "Indicator";
			}
			else if (collection is TextAnnotationCollection)
			{
				this.annotationsButton.Depressed = true;
				//collectionsTab.ObjectName = "Text Annotation";
				SelectTextAnnotationsSubTab((TextAnnotationCollection)collection);
				
			}
			else if (collection is ImageAnnotationCollection)
			{
				this.annotationsButton.Depressed = true;
				//collectionsTab.ObjectName = "Image Annotation";
				SelectImageAnnotationsSubTab((ImageAnnotationCollection)collection);
			}
		}

		private void HideTabs()
		{
			this.startTab.Visible = false;
			this.scalesTab.Visible = false;
			this.rangesTab.Visible = false;
			this.pointersTab.Visible = false;
			this.subGaugesTab.Visible = false;
			this.indicatorsTab.Visible = false;
			this.annotationsTab.Visible = false;
			this.collectionsTab.Visible = false;
			this.gaugePreviewControl.Visible = false;
			this.TreeGroupBox.Visible = false;

			this.startButton.Depressed = false;
			this.scalesButton.Depressed = false;
			this.rangesButton.Depressed = false;
			this.pointersButton.Depressed = false;
			this.subGaugesButton.Depressed = false;
			this.indicatorsButton.Depressed = false;
			this.annotationsButton.Depressed = false;
		}

		private void TakeSnapshot()
		{
			m_gauge.InSerialization = true;
			m_gaugeClone = GaugeXmlSerializer.GetObject(GaugeXmlSerializer.GetDOM(m_gauge.SubGauge)) as SubGauge;
			m_gauge.InSerialization = false;
		}

		private void RestoreOriginal()
		{
			m_gauge.SubGauge = m_gaugeClone;
			m_gauge.OverwriteInternalDesignMode(true);
		}

		private void Tabs_TabSelected(object sender, TabSelectEventArgs e)
		{
			gaugeTreeControl.SelectNode(e.RelatedObject);
		}

		private void backButton_Click(object sender, System.EventArgs e)
		{
			if (scalesButton.Depressed)
				startButton_Click(this, null);
			else if(rangesButton.Depressed)
				scalesButton_Click(this, null);
			else if(pointersButton.Depressed)
				rangesButton_Click(this, null);
			else if(subGaugesButton.Depressed)
				pointersButton_Click(this, null);
			else if(indicatorsButton.Depressed)
				subGaugesButton_Click(this, null);
			else if(annotationsButton.Depressed)
				indicatorsButton_Click(this, null);
		}

		private void nextButton_Click(object sender, System.EventArgs e)
		{
			if (startButton.Depressed)
				scalesButton_Click(this, null);
			else if (scalesButton.Depressed)
				rangesButton_Click(this, null);
			else if(rangesButton.Depressed)
				pointersButton_Click(this, null);
			else if(pointersButton.Depressed)
				subGaugesButton_Click(this, null);
			else if(subGaugesButton.Depressed)
				indicatorsButton_Click(this, null);
			else if(indicatorsButton.Depressed)
				annotationsButton_Click(this, null);				
		}

		private void applyButton_Click(object sender, System.EventArgs e)
		{
			TakeSnapshot();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			RestoreOriginal();
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void finishButton_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}


		//********************** Dragging Tile Bar *************************
		bool m_dragging = false;
		int m_lastX, m_lastY;

		internal void StartDragging(System.Windows.Forms.MouseEventArgs e) 
		{
			m_dragging = true;
			m_lastX = e.X;
			m_lastY = e.Y;
		}

		internal void Drag(System.Windows.Forms.MouseEventArgs e) 
		{
			if (m_dragging)
				Location = new Point(Location.X + e.X - m_lastX, Location.Y + e.Y - m_lastY);
		}

		internal void StopDragging() 
		{
			m_dragging = false;
		}

		/// <summary>
		/// Allows dragging of the form at any point
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left) 
			{
				StartDragging(e);
			}
		}

		/// <summary>
		/// Allows dragging of the form at any point
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseMove(e);
			Drag(e);		
		}

		/// <summary>
		/// Allows dragging of the form at any point
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseUp(e);
			StopDragging();
		}

		private void titleBarPictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				StartDragging(e);
		}

		private void titleBarPictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Drag(e);		
		}

		private void titleBarPictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				StopDragging();
		}
	}
}
