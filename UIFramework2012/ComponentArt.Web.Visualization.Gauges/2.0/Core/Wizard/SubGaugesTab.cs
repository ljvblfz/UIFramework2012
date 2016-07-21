using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for SubGaugesTab.
	/// </summary>
	internal class SubGaugesTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.TabControl tabControl1;
		private ComponentArt.WinUI.TabPage tabPage1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private ComponentArt.WinUI.ComboBox typeDropdown;
		private ComponentArt.WinUI.ComboBox subTypeDropdown;
		private ComponentArt.WinUI.ComboBox themeDropdown;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl relativePositionControl;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl relativeSizeControl;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SubGauge m_gauge = null;
		public SubGauge Gauge
		{
			set
			{
				m_gauge = value;
				LoadDataFromObject();
				EnableSubGaugeControls();
			}
			get
			{
				return m_gauge;
			}
		}

		public delegate void TabEventHandler (object source);
		public event TabEventHandler Changed;

		public SubGaugesTab()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			EnableSubGaugeControls();
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
			this.tabControl1 = new ComponentArt.WinUI.TabControl();
			this.tabPage1 = new ComponentArt.WinUI.TabPage();
			this.relativeSizeControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.relativePositionControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.themeDropdown = new ComponentArt.WinUI.ComboBox();
			this.subTypeDropdown = new ComponentArt.WinUI.ComboBox();
			this.typeDropdown = new ComponentArt.WinUI.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.ColorBehind = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1});
			this.tabControl1.Multiline = false;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Size = new System.Drawing.Size(256, 328);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl1.TabIndex = 10;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.relativeSizeControl,
																				   this.relativePositionControl,
																				   this.themeDropdown,
																				   this.subTypeDropdown,
																				   this.typeDropdown,
																				   this.label5,
																				   this.label4,
																				   this.label3,
																				   this.label2,
																				   this.label1});
			this.tabPage1.Location = new System.Drawing.Point(2, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 301);
			this.tabPage1.TabIndex = 11;
			this.tabPage1.Text = "Properties";
			// 
			// relativeSizeControl
			// 
			this.relativeSizeControl.Location = new System.Drawing.Point(128, 172);
			this.relativeSizeControl.Name = "relativeSizeControl";
			this.relativeSizeControl.Size = new System.Drawing.Size(104, 104);
			this.relativeSizeControl.TabIndex = 16;
			this.relativeSizeControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.relativeSizeControl_ValueChanged);
			// 
			// relativePositionControl
			// 
			this.relativePositionControl.Location = new System.Drawing.Point(16, 172);
			this.relativePositionControl.Name = "relativePositionControl";
			this.relativePositionControl.Size = new System.Drawing.Size(104, 104);
			this.relativePositionControl.TabIndex = 15;
			this.relativePositionControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.relativePositionControl_ValueChanged);
			// 
			// themeDropdown
			// 
			this.themeDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.themeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.themeDropdown.Location = new System.Drawing.Point(96, 104);
			this.themeDropdown.Name = "themeDropdown";
			this.themeDropdown.Size = new System.Drawing.Size(104, 21);
			this.themeDropdown.TabIndex = 14;
			this.themeDropdown.SelectedIndexChanged += new System.EventHandler(this.themeDropdown_SelectedIndexChanged);
			// 
			// subTypeDropdown
			// 
			this.subTypeDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.subTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.subTypeDropdown.Location = new System.Drawing.Point(96, 64);
			this.subTypeDropdown.Name = "subTypeDropdown";
			this.subTypeDropdown.Size = new System.Drawing.Size(104, 21);
			this.subTypeDropdown.TabIndex = 13;
			this.subTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.subTypeDropdown_SelectedIndexChanged);
			// 
			// typeDropdown
			// 
			this.typeDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.typeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeDropdown.Location = new System.Drawing.Point(96, 24);
			this.typeDropdown.Name = "typeDropdown";
			this.typeDropdown.Size = new System.Drawing.Size(104, 21);
			this.typeDropdown.TabIndex = 12;
			this.typeDropdown.SelectedIndexChanged += new System.EventHandler(this.typeDropdown_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(128, 148);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 17;
			this.label5.Text = "Relative Size";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 148);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 18;
			this.label4.Text = "Relative Location";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 108);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 19;
			this.label3.Text = "Theme";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 20;
			this.label2.Text = "Sub-Type";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 21;
			this.label1.Text = "Type";
			// 
			// SubGaugesTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "SubGaugesTab";
			this.Size = new System.Drawing.Size(256, 328);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadDataFromObject()
		{
			this.themeDropdown.Items.Clear();
			foreach (Theme no in m_gauge.Themes)
			{
				this.themeDropdown.Items.Add(no.Name);
			}
			this.themeDropdown.SelectedItem = m_gauge.ThemeName;

			this.typeDropdown.Items.Clear();
			foreach (GaugeKindGroup gkg in GaugeThumbnailListBox.SelectableGaugeKindGroups)
			{
				this.typeDropdown.Items.Add(gkg.ToString());
				if(ComponentArt.Web.Visualization.Gauges.SubGauge.IsInGroup(m_gauge.GaugeKind, gkg))
					this.typeDropdown.SelectedItem = gkg.ToString();
			}

			PopulateSubTypeDropdown();
			
			this.relativePositionControl.SetValues(0, 100, 0, 100, 1, 1);
			this.relativePositionControl.X = m_gauge.RelativeLocation.X;
            this.relativePositionControl.Y = m_gauge.RelativeLocation.Y;

			this.relativeSizeControl.SetValues(1, 100, 1, 100, 1, 1);
			this.relativeSizeControl.X = m_gauge.RelativeSize.Width;
			this.relativeSizeControl.Y =  m_gauge.RelativeSize.Height;
		}
		
		void PopulateSubTypeDropdown()
		{
			GaugeKindGroup kindGroup = (GaugeKindGroup)Enum.Parse(typeof(GaugeKindGroup), (string)this.typeDropdown.SelectedItem, false);
			this.subTypeDropdown.Items.Clear();
			foreach(string kindString in Enum.GetNames(typeof(GaugeKind)))
			{
				GaugeKind kind = (GaugeKind)Enum.Parse(typeof(GaugeKind), kindString, false);
				if (ComponentArt.Web.Visualization.Gauges.SubGauge.IsInGroup(kind, kindGroup))
				{
					this.subTypeDropdown.Items.Add(kindString);
				}
			}
			if (ComponentArt.Web.Visualization.Gauges.SubGauge.IsInGroup(m_gauge.GaugeKind, kindGroup))
			{
				this.subTypeDropdown.SelectedItem = m_gauge.GaugeKind.ToString();
			}
			else
			{
				this.subTypeDropdown.SelectedIndex = 0;
			}
		}
		
		private void DoChangedEvent()
		{
			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void typeDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopulateSubTypeDropdown();
			m_gauge.GaugeKind = (GaugeKind) Enum.Parse(typeof(GaugeKind), (string)this.subTypeDropdown.SelectedItem, false);
			DoChangedEvent();
		}

		private void subTypeDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_gauge.GaugeKind = (GaugeKind) Enum.Parse(typeof(GaugeKind), (string)this.subTypeDropdown.SelectedItem, false);
			DoChangedEvent();
		}

		private void themeDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_gauge.ThemeName = (string)this.themeDropdown.SelectedItem;
			DoChangedEvent();
		}

		private void relativePositionControl_ValueChanged(object sender, float x, float y)
		{
            m_gauge.RelativeLocation.X = this.relativePositionControl.X;
            m_gauge.RelativeLocation.Y = this.relativePositionControl.Y;
			DoChangedEvent();
		}

		private void relativeSizeControl_ValueChanged(object sender, float x, float y)
		{
			m_gauge.RelativeSize.Width = this.relativeSizeControl.X;
			m_gauge.RelativeSize.Height = this.relativeSizeControl.Y;
			DoChangedEvent();
		}

		private void EnableSubGaugeControls()
		{
			this.relativeSizeControl.Enabled = (this.m_gauge != null);
			this.relativePositionControl.Enabled = (this.m_gauge != null);
			this.themeDropdown.Enabled = (this.m_gauge != null);
			this.subTypeDropdown.Enabled = (this.m_gauge != null);
			this.typeDropdown.Enabled = (this.m_gauge != null);
		}
	}
}
