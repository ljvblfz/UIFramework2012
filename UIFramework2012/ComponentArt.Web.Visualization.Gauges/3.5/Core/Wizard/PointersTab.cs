using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for PointersTab.
	/// </summary>
	internal class PointersTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.TabControl tabControl1;
		private ComponentArt.WinUI.TabPage tabPage1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox valueTextBox;
		private ComponentArt.WinUI.ComboBox styleDropdown;
		private System.Windows.Forms.CheckBox visibleCheckBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ComponentArt.Web.Visualization.Gauges.SliderControl lengthSlider;

		private Pointer m_pointer = null;
		public Pointer Pointer
		{
			set
			{
				m_pointer = value;
				LoadDataFromPointer();
			}
			get
			{
				return m_pointer;
			}
		}

		public delegate void TabEventHandler (object source);
		public event TabEventHandler Changed;

		public PointersTab()
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
			this.tabControl1 = new ComponentArt.WinUI.TabControl();
			this.tabPage1 = new ComponentArt.WinUI.TabPage();
			this.valueTextBox = new System.Windows.Forms.TextBox();
			this.styleDropdown = new ComponentArt.WinUI.ComboBox();
			this.lengthSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.visibleCheckBox = new System.Windows.Forms.CheckBox();
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
																				   this.valueTextBox,
																				   this.styleDropdown,
																				   this.lengthSlider,
																				   this.label3,
																				   this.label2,
																				   this.label1,
																				   this.visibleCheckBox});
			this.tabPage1.Location = new System.Drawing.Point(2, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 301);
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
			// valueTextBox
			// 
			this.valueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.valueTextBox.Location = new System.Drawing.Point(96, 144);
			this.valueTextBox.Name = "valueTextBox";
			this.valueTextBox.Size = new System.Drawing.Size(32, 20);
			this.valueTextBox.TabIndex = 14;
			this.valueTextBox.Text = "";
			this.valueTextBox.Leave += new System.EventHandler(this.valueTextBox_Leave);
			// 
			// styleDropdown
			// 
			this.styleDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.styleDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.styleDropdown.Location = new System.Drawing.Point(96, 64);
			this.styleDropdown.Name = "styleDropdown";
			this.styleDropdown.Size = new System.Drawing.Size(104, 21);
			this.styleDropdown.TabIndex = 12;
			this.styleDropdown.SelectedIndexChanged += new System.EventHandler(this.styleDropdown_SelectedIndexChanged);
			// 
			// lengthSlider
			// 
			this.lengthSlider.Location = new System.Drawing.Point(96, 92);
			this.lengthSlider.Name = "lengthSlider";
			this.lengthSlider.Size = new System.Drawing.Size(104, 32);
			this.lengthSlider.TabIndex = 13;
			this.lengthSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.lengthSlider_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 148);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 15;
			this.label3.Text = "Value";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 1088);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "Length";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 68);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "Style";
			// 
			// PointersTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "PointersTab";
			this.Size = new System.Drawing.Size(256, 328);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadDataFromPointer()
		{
			this.visibleCheckBox.Checked = m_pointer.Visible;
			this.valueTextBox.Text = m_pointer.Value.ToString();
			this.styleDropdown.Items.Clear();
			foreach(PointerStyle ps in m_pointer.Gauge.PointerStyles)
			{
				this.styleDropdown.Items.Add(ps.Name);
			}
			this.styleDropdown.SelectedItem = m_pointer.Style.Name;
			this.lengthSlider.SliderValue = new SliderValue(0, 120, m_pointer.RelativeLength, 1);
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
			m_pointer.Visible = this.visibleCheckBox.Checked;
			DoChangedEvent();
		}

		private void styleDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_pointer.StyleName = (string)this.styleDropdown.SelectedItem;
			DoChangedEvent();
		}

		private void lengthSlider_ValueChanged(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_pointer.RelativeLength = this.lengthSlider.SliderValue.Value;
			DoChangedEvent();
		}

		private void valueTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_pointer.Value = double.Parse(this.valueTextBox.Text);
			}
			catch(Exception)
			{
				this.valueTextBox.Text = m_pointer.Value.ToString();
			}
			DoChangedEvent();
		}
	}
}
