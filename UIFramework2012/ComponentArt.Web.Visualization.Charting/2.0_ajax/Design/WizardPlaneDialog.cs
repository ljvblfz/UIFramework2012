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
	internal class WizardPlaneDialog : WizardDialog
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		[WizardHint(typeof(CoordinatePlane), "Visible")]
		private ComponentArt.Win.UI.Internal.CheckBox m_planeVisible;
		private ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox m_grid1LineStyleNameComboBox;
		private ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox m_grid2LineStyleNameComboBox;
		private System.Windows.Forms.TrackBar m_depthTrackBar;

		CoordinatePlane m_plane = null;
		private ComponentArt.Win.UI.Internal.CheckBox m_gridLines1Visible;
		private ComponentArt.Win.UI.Internal.CheckBox m_stripes2Visible;
		private ComponentArt.Win.UI.Internal.CheckBox m_gridLines2Visible;
		private ComponentArt.Win.UI.Internal.CheckBox m_stripes1Visible;
		private System.Windows.Forms.TrackBar m_offsetTrackBar;
		private ComponentArt.Win.UI.Internal.Button m_strips2SettingsButton;
		private ComponentArt.Win.UI.Internal.Button m_strips1SettingsButton;
		private System.Windows.Forms.Label m_gridLabel1;
		private System.Windows.Forms.Label m_gridLabel2;
		private System.Windows.Forms.Label m_stripsLabel1;
		private System.Windows.Forms.Label m_stripsLabel2;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		private ComponentArt.Win.UI.Internal.Separator separator3;
		private System.Windows.Forms.Label m_depthLabel;
		private System.Windows.Forms.Label m_offsetLabel;
		[WizardHint(typeof(CoordinatePlane), "Depth")]
		private ComponentArt.Win.UI.Internal.GroupBox m_planeDepthGroupBox;
		[WizardHint(typeof(CoordinatePlane), "ICSOffset")]
		private ComponentArt.Win.UI.Internal.GroupBox m_planeOffsetGroupBox;
		string m_planeLetters = "";

		public WizardPlaneDialog()
		{
			InitializeComponent();
		}

		internal CoordinateSystem CoordinateSystem 
		{
			get 
			{
				return WinChart.CoordinateSystem;
			}
		}

		internal CoordinatePlane CoordinatePlane
		{
			get 
			{
				if (m_plane == null) 
				{
					if (Name == "m_wizardPlaneDialogXY")
						m_plane = CoordinateSystem.PlaneXY;
					else if (Name == "m_wizardPlaneDialogYZ")
						m_plane = CoordinateSystem.PlaneYZ;
					else if (Name == "m_wizardPlaneDialogZX")
						m_plane = CoordinateSystem.PlaneZX;
					else
						m_plane =null;
				}

				return m_plane;
			}
		}

		internal GridCollection Grids
		{
			get 
			{
				return CoordinatePlane.Grids;
			}
		}

		internal StripSetCollection StripSets
		{
			get 
			{
				return CoordinatePlane.StripSets;
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

		StripSet m_s1, m_s2;
		Grid     m_g1, m_g2;

		Grid GetGrid(string name)
		{
			Grid g = Grids[name];
			return g;
		}

		StripSet GetStripSet(string name)
		{
			StripSet s = StripSets[name];
			return s;
		}
		
		void GetGridsAndStripSets() 
		{
			string firstName = m_planeLetters[0]+"inPlane" + m_planeLetters;
			string secondName = m_planeLetters[1]+"inPlane" + m_planeLetters;

			m_s1 = GetStripSet(firstName);
			m_s2 = GetStripSet(secondName);
			m_g1 = GetGrid(firstName);
			m_g2 = GetGrid(secondName);
		}


		void SetStripsAndGridsState()
		{
			GetGridsAndStripSets();

			if (m_g1 != null)
			{
				m_grid1LineStyleNameComboBox.Text = m_g1.LineStyleName;
				m_gridLines1Visible.Checked = m_g1.Visible;
			}
			
			if (m_g2 != null)
			{
				m_grid2LineStyleNameComboBox.Text = m_g2.LineStyleName;
				m_gridLines2Visible.Checked = m_g2.Visible;
			}

			if (m_s1 != null)
				m_stripes1Visible.Checked = m_s1.Visible;

			if (m_s2 != null)
				m_stripes2Visible.Checked = m_s2.Visible;

			m_grid1LineStyleNameComboBox.Enabled = m_gridLines1Visible.Enabled = (m_g1 != null);
			m_grid2LineStyleNameComboBox.Enabled = m_gridLines2Visible.Enabled = (m_g2 != null);
			m_stripes1Visible.Enabled = m_strips1SettingsButton.Enabled = (m_s1 != null);
			m_stripes2Visible.Enabled = m_strips2SettingsButton.Enabled = (m_s2 != null);

		}

		protected override void OnVisibleChanged(EventArgs e) 
		{
			base.OnVisibleChanged(e);
			if (!Visible)
			{
				return;
			}
			SetStripsAndGridsState();
		}

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			m_planeLetters = Name.Substring(Name.Length-2, 2).ToUpper(System.Globalization.CultureInfo.InvariantCulture);

			if (m_planeLetters != "XY" && m_planeLetters != "YZ" && m_planeLetters != "ZX")
				m_planeLetters = "";

			if (WinChart == null)
				return;

			SetStripsAndGridsState();

			m_planeVisible.Text = "Show Plane " + m_planeLetters;

			m_gridLabel1.Text = m_planeLetters[0] + " Grid Line Style:";
			m_gridLabel2.Text = m_planeLetters[1] + " Grid Line Style:";

			m_stripsLabel1.Text = m_planeLetters[0] + " Strips:";
			m_stripsLabel2.Text = m_planeLetters[1] + " Strips:";


			// Connect the comboboxes to collections
			m_grid1LineStyleNameComboBox.SetProperty(typeof(Grid).GetProperty("LineStyleName"), "Edit Line Styles...");
			m_grid2LineStyleNameComboBox.SetProperty(typeof(Grid).GetProperty("LineStyleName"), "Edit Line Styles...");

			m_depthTrackBar.Value = (int)CoordinatePlane.Depth;
			m_depthLabel.Text = m_depthTrackBar.Value.ToString();

			m_planeVisible.Checked = CoordinatePlane.Visible;
			m_offsetTrackBar.Value = (int)-CoordinatePlane.ICSOffset;
			m_offsetLabel.Text = m_offsetTrackBar.Value.ToString();
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_grid1LineStyleNameComboBox = new ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox();
			this.m_gridLines1Visible = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_strips2SettingsButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_stripes2Visible = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_planeVisible = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_depthTrackBar = new System.Windows.Forms.TrackBar();
			this.m_grid2LineStyleNameComboBox = new ComponentArt.Web.Visualization.Charting.Design.SelectedNameComboBox();
			this.m_gridLines2Visible = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_strips1SettingsButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_stripes1Visible = new ComponentArt.Win.UI.Internal.CheckBox();
			this.m_offsetTrackBar = new System.Windows.Forms.TrackBar();
			this.m_gridLabel1 = new System.Windows.Forms.Label();
			this.m_gridLabel2 = new System.Windows.Forms.Label();
			this.m_stripsLabel1 = new System.Windows.Forms.Label();
			this.m_stripsLabel2 = new System.Windows.Forms.Label();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
			this.separator3 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_depthLabel = new System.Windows.Forms.Label();
			this.m_offsetLabel = new System.Windows.Forms.Label();
			this.m_planeDepthGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_planeOffsetGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.m_depthTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_offsetTrackBar)).BeginInit();
			this.m_planeDepthGroupBox.SuspendLayout();
			this.m_planeOffsetGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_grid1LineStyleNameComboBox
			// 
			this.m_grid1LineStyleNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_grid1LineStyleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_grid1LineStyleNameComboBox.Location = new System.Drawing.Point(122, 48);
			this.m_grid1LineStyleNameComboBox.Name = "m_grid1LineStyleNameComboBox";
			this.m_grid1LineStyleNameComboBox.Size = new System.Drawing.Size(108, 21);
			this.m_grid1LineStyleNameComboBox.TabIndex = 10;
			this.m_grid1LineStyleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.m_gridLineStyleNameComboBox_SelectedIndexChanged);
			// 
			// m_gridLines1Visible
			// 
			this.m_gridLines1Visible.BackColor = System.Drawing.Color.White;
			this.m_gridLines1Visible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_gridLines1Visible.Checked = true;
			this.m_gridLines1Visible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_gridLines1Visible.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_gridLines1Visible.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_gridLines1Visible.Location = new System.Drawing.Point(240, 48);
			this.m_gridLines1Visible.Name = "m_gridLines1Visible";
			this.m_gridLines1Visible.Size = new System.Drawing.Size(58, 16);
			this.m_gridLines1Visible.TabIndex = 20;
			this.m_gridLines1Visible.Text = "Show";
			this.m_gridLines1Visible.CheckedChanged += new System.EventHandler(this.m_gridLinesVisible_CheckedChanged);
			// 
			// m_strips2SettingsButton
			// 
			this.m_strips2SettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_strips2SettingsButton.Location = new System.Drawing.Point(122, 157);
			this.m_strips2SettingsButton.Name = "m_strips2SettingsButton";
			this.m_strips2SettingsButton.Size = new System.Drawing.Size(108, 22);
			this.m_strips2SettingsButton.TabIndex = 70;
			this.m_strips2SettingsButton.Text = "More Settings...";
			this.m_strips2SettingsButton.Click += new System.EventHandler(this.m_stripsSettingsButton_Click);
			// 
			// m_stripes2Visible
			// 
			this.m_stripes2Visible.BackColor = System.Drawing.Color.White;
			this.m_stripes2Visible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_stripes2Visible.Checked = true;
			this.m_stripes2Visible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_stripes2Visible.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_stripes2Visible.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_stripes2Visible.Location = new System.Drawing.Point(240, 160);
			this.m_stripes2Visible.Name = "m_stripes2Visible";
			this.m_stripes2Visible.Size = new System.Drawing.Size(58, 16);
			this.m_stripes2Visible.TabIndex = 80;
			this.m_stripes2Visible.Text = "Show";
			this.m_stripes2Visible.CheckedChanged += new System.EventHandler(this.m_stripesVisible_CheckedChanged);
			// 
			// m_planeVisible
			// 
			this.m_planeVisible.BackColor = System.Drawing.Color.White;
			this.m_planeVisible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_planeVisible.Checked = true;
			this.m_planeVisible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_planeVisible.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_planeVisible.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_planeVisible.Location = new System.Drawing.Point(39, 8);
			this.m_planeVisible.Name = "m_planeVisible";
			this.m_planeVisible.Size = new System.Drawing.Size(95, 16);
			this.m_planeVisible.TabIndex = 0;
			this.m_planeVisible.Text = "Show Plane XY";
			this.m_planeVisible.CheckedChanged += new System.EventHandler(this.m_planeVisible_CheckedChanged);
			// 
			// m_depthTrackBar
			// 
			this.m_depthTrackBar.BackColor = System.Drawing.Color.White;
			this.m_depthTrackBar.Location = new System.Drawing.Point(2, 23);
			this.m_depthTrackBar.Maximum = 50;
			this.m_depthTrackBar.Name = "m_depthTrackBar";
			this.m_depthTrackBar.Size = new System.Drawing.Size(144, 45);
			this.m_depthTrackBar.TabIndex = 90;
			this.m_depthTrackBar.TickFrequency = 5;
			this.m_depthTrackBar.Scroll += new System.EventHandler(this.m_depthTrackBar_Scroll);
			// 
			// m_grid2LineStyleNameComboBox
			// 
			this.m_grid2LineStyleNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_grid2LineStyleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_grid2LineStyleNameComboBox.Location = new System.Drawing.Point(122, 128);
			this.m_grid2LineStyleNameComboBox.Name = "m_grid2LineStyleNameComboBox";
			this.m_grid2LineStyleNameComboBox.Size = new System.Drawing.Size(108, 21);
			this.m_grid2LineStyleNameComboBox.TabIndex = 50;
			this.m_grid2LineStyleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.m_gridLineStyleNameComboBox_SelectedIndexChanged);
			// 
			// m_gridLines2Visible
			// 
			this.m_gridLines2Visible.BackColor = System.Drawing.Color.White;
			this.m_gridLines2Visible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_gridLines2Visible.Checked = true;
			this.m_gridLines2Visible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_gridLines2Visible.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_gridLines2Visible.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_gridLines2Visible.Location = new System.Drawing.Point(240, 128);
			this.m_gridLines2Visible.Name = "m_gridLines2Visible";
			this.m_gridLines2Visible.Size = new System.Drawing.Size(58, 16);
			this.m_gridLines2Visible.TabIndex = 60;
			this.m_gridLines2Visible.Text = "Show";
			this.m_gridLines2Visible.CheckedChanged += new System.EventHandler(this.m_gridLinesVisible_CheckedChanged);
			// 
			// m_strips1SettingsButton
			// 
			this.m_strips1SettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_strips1SettingsButton.Location = new System.Drawing.Point(122, 77);
			this.m_strips1SettingsButton.Name = "m_strips1SettingsButton";
			this.m_strips1SettingsButton.Size = new System.Drawing.Size(108, 22);
			this.m_strips1SettingsButton.TabIndex = 30;
			this.m_strips1SettingsButton.Text = "More Settings...";
			this.m_strips1SettingsButton.Click += new System.EventHandler(this.m_stripsSettingsButton_Click);
			// 
			// m_stripes1Visible
			// 
			this.m_stripes1Visible.BackColor = System.Drawing.Color.White;
			this.m_stripes1Visible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_stripes1Visible.Checked = true;
			this.m_stripes1Visible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_stripes1Visible.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.m_stripes1Visible.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_stripes1Visible.Location = new System.Drawing.Point(240, 80);
			this.m_stripes1Visible.Name = "m_stripes1Visible";
			this.m_stripes1Visible.Size = new System.Drawing.Size(58, 16);
			this.m_stripes1Visible.TabIndex = 40;
			this.m_stripes1Visible.Text = "Show";
			this.m_stripes1Visible.CheckedChanged += new System.EventHandler(this.m_stripesVisible_CheckedChanged);
			// 
			// m_offsetTrackBar
			// 
			this.m_offsetTrackBar.BackColor = System.Drawing.Color.White;
			this.m_offsetTrackBar.Location = new System.Drawing.Point(2, 23);
			this.m_offsetTrackBar.Maximum = 50;
			this.m_offsetTrackBar.Name = "m_offsetTrackBar";
			this.m_offsetTrackBar.Size = new System.Drawing.Size(144, 45);
			this.m_offsetTrackBar.TabIndex = 100;
			this.m_offsetTrackBar.TickFrequency = 5;
			this.m_offsetTrackBar.Scroll += new System.EventHandler(this.m_offsetTrackBar_Scroll);
			// 
			// m_gridLabel1
			// 
			this.m_gridLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_gridLabel1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_gridLabel1.Location = new System.Drawing.Point(5, 48);
			this.m_gridLabel1.Name = "m_gridLabel1";
			this.m_gridLabel1.Size = new System.Drawing.Size(108, 16);
			this.m_gridLabel1.TabIndex = 55;
			this.m_gridLabel1.Text = "X Grid Line Style:";
			this.m_gridLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_gridLabel2
			// 
			this.m_gridLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_gridLabel2.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_gridLabel2.Location = new System.Drawing.Point(5, 128);
			this.m_gridLabel2.Name = "m_gridLabel2";
			this.m_gridLabel2.Size = new System.Drawing.Size(108, 16);
			this.m_gridLabel2.TabIndex = 56;
			this.m_gridLabel2.Text = "X Grid Line Style:";
			this.m_gridLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_stripsLabel1
			// 
			this.m_stripsLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_stripsLabel1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_stripsLabel1.Location = new System.Drawing.Point(8, 80);
			this.m_stripsLabel1.Name = "m_stripsLabel1";
			this.m_stripsLabel1.Size = new System.Drawing.Size(105, 16);
			this.m_stripsLabel1.TabIndex = 57;
			this.m_stripsLabel1.Text = "X Strips:";
			this.m_stripsLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// m_stripsLabel2
			// 
			this.m_stripsLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_stripsLabel2.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_stripsLabel2.Location = new System.Drawing.Point(8, 160);
			this.m_stripsLabel2.Name = "m_stripsLabel2";
			this.m_stripsLabel2.Size = new System.Drawing.Size(105, 16);
			this.m_stripsLabel2.TabIndex = 58;
			this.m_stripsLabel2.Text = "X Strips:";
			this.m_stripsLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(8, 32);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(296, 3);
			this.separator1.TabIndex = 59;
			this.separator1.TabStop = false;
			// 
			// separator2
			// 
			this.separator2.Location = new System.Drawing.Point(8, 112);
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(296, 3);
			this.separator2.TabIndex = 60;
			this.separator2.TabStop = false;
			// 
			// separator3
			// 
			this.separator3.Location = new System.Drawing.Point(8, 200);
			this.separator3.Name = "separator3";
			this.separator3.Size = new System.Drawing.Size(296, 3);
			this.separator3.TabIndex = 63;
			this.separator3.TabStop = false;
			// 
			// m_depthLabel
			// 
			this.m_depthLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_depthLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_depthLabel.Location = new System.Drawing.Point(106, 5);
			this.m_depthLabel.Name = "m_depthLabel";
			this.m_depthLabel.Size = new System.Drawing.Size(32, 16);
			this.m_depthLabel.TabIndex = 101;
			this.m_depthLabel.Text = "0";
			this.m_depthLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// m_offsetLabel
			// 
			this.m_offsetLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_offsetLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_offsetLabel.Location = new System.Drawing.Point(106, 5);
			this.m_offsetLabel.Name = "m_offsetLabel";
			this.m_offsetLabel.Size = new System.Drawing.Size(32, 16);
			this.m_offsetLabel.TabIndex = 102;
			this.m_offsetLabel.Text = "0";
			this.m_offsetLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// m_planeDepthGroupBox
			// 
			this.m_planeDepthGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_depthLabel,
																							   this.m_depthTrackBar});
			this.m_planeDepthGroupBox.Location = new System.Drawing.Point(6, 203);
			this.m_planeDepthGroupBox.Name = "m_planeDepthGroupBox";
			this.m_planeDepthGroupBox.ResizeChildren = false;
			this.m_planeDepthGroupBox.Size = new System.Drawing.Size(154, 88);
			this.m_planeDepthGroupBox.TabIndex = 103;
			this.m_planeDepthGroupBox.TabStop = false;
			this.m_planeDepthGroupBox.Text = "Plane Depth:";
			// 
			// m_planeOffsetGroupBox
			// 
			this.m_planeOffsetGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.m_offsetTrackBar,
																								this.m_offsetLabel});
			this.m_planeOffsetGroupBox.Location = new System.Drawing.Point(166, 203);
			this.m_planeOffsetGroupBox.Name = "m_planeOffsetGroupBox";
			this.m_planeOffsetGroupBox.ResizeChildren = false;
			this.m_planeOffsetGroupBox.Size = new System.Drawing.Size(146, 61);
			this.m_planeOffsetGroupBox.TabIndex = 106;
			this.m_planeOffsetGroupBox.TabStop = false;
			this.m_planeOffsetGroupBox.Text = "Plane Offset:";
			// 
			// WizardPlaneDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.separator3,
																		  this.separator2,
																		  this.separator1,
																		  this.m_stripsLabel2,
																		  this.m_stripsLabel1,
																		  this.m_gridLabel2,
																		  this.m_gridLabel1,
																		  this.m_planeVisible,
																		  this.m_strips2SettingsButton,
																		  this.m_stripes2Visible,
																		  this.m_grid2LineStyleNameComboBox,
																		  this.m_gridLines2Visible,
																		  this.m_stripes1Visible,
																		  this.m_strips1SettingsButton,
																		  this.m_grid1LineStyleNameComboBox,
																		  this.m_gridLines1Visible,
																		  this.m_planeDepthGroupBox,
																		  this.m_planeOffsetGroupBox});
			this.Name = "WizardPlaneDialog";
			this.Size = new System.Drawing.Size(320, 272);
			((System.ComponentModel.ISupportInitialize)(this.m_depthTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_offsetTrackBar)).EndInit();
			this.m_planeDepthGroupBox.ResumeLayout(false);
			this.m_planeOffsetGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_gridLineStyleNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			(sender == m_grid1LineStyleNameComboBox?m_g1:m_g2).LineStyleName = ((ComboBox)sender).Text;
			WinChart.Invalidate();
		}

		private void m_gridLinesVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			(sender == m_gridLines1Visible?m_g1:m_g2).Visible = ((CheckBox)sender).Checked;
			WinChart.Invalidate();
		}

		private void m_stripeLayerUpDown_ValueChanged(object sender, System.EventArgs e)
		{
		}

		private void m_stripesVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			(sender == m_stripes1Visible?m_s1:m_s2).Visible = ((CheckBox)sender).Checked;
			WinChart.Invalidate();
		}

		private void m_depthTrackBar_Scroll(object sender, System.EventArgs e)
		{
			CoordinatePlane.Depth = m_depthTrackBar.Value;
			m_depthLabel.Text = m_depthTrackBar.Value.ToString();
			m_depthLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_planeVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			CoordinatePlane.Visible = m_planeVisible.Checked;
			WinChart.Invalidate();
		}

		private void m_offsetTrackBar_Scroll(object sender, System.EventArgs e)
		{
			CoordinatePlane.ICSOffset = -m_offsetTrackBar.Value;
			m_offsetLabel.Text = CoordinatePlane.ICSOffset.ToString();
			m_offsetLabel.Refresh();
			WinChart.Invalidate();
		}

		private void m_stripsSettingsButton_Click(object sender, System.EventArgs e)
		{
			StripSet s = (sender == m_strips1SettingsButton?m_s1:m_s2);

			WizardPropertyGridForm form = new WizardPropertyGridForm();
			form.Closed += new EventHandler(this.HandleDialogClose);

			form.PropertyGrid.SelectedObject = s;

			form.PropertyGrid.PropertyValueChanged +=new PropertyValueChangedEventHandler(onPropertyValueChanged);
			form.ShowDialog();

			WinChart.Invalidate();
		}

		private void onPropertyValueChanged(object sender, PropertyValueChangedEventArgs e) 
		{
			WinChart.Invalidate();
		}

		private void HandleDialogClose(object sender, EventArgs e) 
		{
			m_stripes1Visible.Checked = m_s1.Visible;
			m_stripes2Visible.Checked = m_s2.Visible;
		}
	}
}
