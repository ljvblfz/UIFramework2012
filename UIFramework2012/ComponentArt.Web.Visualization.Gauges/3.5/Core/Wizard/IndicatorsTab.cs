using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for IndicatorsTab.
	/// </summary>
	internal class IndicatorsTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.TabControl tabControl1;
		private ComponentArt.WinUI.TabPage tabPage1;
		private ComponentArt.WinUI.TabPage tabPage2;
		private ComponentArt.WinUI.TabPage tabPage3;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl sizeControl;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl positionControl;
		private ComponentArt.WinUI.ComboBox typeDropdown;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label1;
		private ComponentArt.WinUI.ComboBox positionDropdown;
		private ComponentArt.WinUI.ComboBox styleDropdown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private ComponentArt.WinUI.ComboBox valueTypeDropdown;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox maxValueEditBox;
		private System.Windows.Forms.TextBox minValueEditBox;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox labelTextBox;
		private System.Windows.Forms.TextBox valueTextBox;
		private ComponentArt.WinUI.ComboBox stateDropdown;
		private ComponentArt.Web.Visualization.Gauges.Core.WinUI.MultiColorDialog multiColorDialog1;
		private ComponentArt.Web.Visualization.Gauges.Core.WinUI.StateColorDialog stateColorDialog1;
		private ComponentArt.WinUI.Button buttonColorDialog;
		private System.Windows.Forms.TextBox textBoxColor;


		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;



		private Indicator m_indicator = null;
		public Indicator Indicator
		{
			set
			{
				m_indicator = value;
				LoadDataFromObject();
				EnableIndicatorControls();
			}
			get
			{
				return m_indicator;
			}
		}

		public delegate void TabEventHandler (object source);
		public event TabEventHandler Changed;

		public IndicatorsTab()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			EnableIndicatorControls();
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
			this.tabControl1 = new ComponentArt.WinUI.TabControl();
			this.tabPage1 = new ComponentArt.WinUI.TabPage();
			this.sizeControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.positionControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.typeDropdown = new ComponentArt.WinUI.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new ComponentArt.WinUI.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.labelTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.positionDropdown = new ComponentArt.WinUI.ComboBox();
			this.styleDropdown = new ComponentArt.WinUI.ComboBox();
			this.tabPage3 = new ComponentArt.WinUI.TabPage();
			this.label12 = new System.Windows.Forms.Label();
			this.stateColorDialog1 = new ComponentArt.Web.Visualization.Gauges.Core.WinUI.StateColorDialog();
			this.multiColorDialog1 = new ComponentArt.Web.Visualization.Gauges.Core.WinUI.MultiColorDialog();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.stateDropdown = new ComponentArt.WinUI.ComboBox();
			this.valueTextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.maxValueEditBox = new System.Windows.Forms.TextBox();
			this.minValueEditBox = new System.Windows.Forms.TextBox();
			this.valueTypeDropdown = new ComponentArt.WinUI.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBoxColor = new System.Windows.Forms.TextBox();
			this.buttonColorDialog = new ComponentArt.WinUI.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.ColorBehind = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2,
																					  this.tabPage3});
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
																				   this.sizeControl,
																				   this.positionControl,
																				   this.typeDropdown,
																				   this.label5,
																				   this.label4,
																				   this.label1});
			this.tabPage1.Location = new System.Drawing.Point(2, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 301);
			this.tabPage1.TabIndex = 11;
			this.tabPage1.Text = "Properties";
			// 
			// sizeControl
			// 
			this.sizeControl.Location = new System.Drawing.Point(128, 92);
			this.sizeControl.Name = "sizeControl";
			this.sizeControl.Size = new System.Drawing.Size(104, 104);
			this.sizeControl.TabIndex = 14;
			this.sizeControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.sizeControl_ValueChanged);
			// 
			// positionControl
			// 
			this.positionControl.Location = new System.Drawing.Point(16, 92);
			this.positionControl.Name = "positionControl";
			this.positionControl.Size = new System.Drawing.Size(104, 104);
			this.positionControl.TabIndex = 13;
			this.positionControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.positionControl_ValueChanged);
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
			this.label5.Location = new System.Drawing.Point(128, 68);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 16);
			this.label5.TabIndex = 15;
			this.label5.Text = "Size";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 68);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 16;
			this.label4.Text = "Position";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "Type";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.White;
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label6,
																				   this.labelTextBox,
																				   this.label3,
																				   this.label2,
																				   this.positionDropdown,
																				   this.styleDropdown});
			this.tabPage2.Location = new System.Drawing.Point(2, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(252, 301);
			this.tabPage2.TabIndex = 21;
			this.tabPage2.Text = "Label";
			this.tabPage2.Visible = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 28);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 16);
			this.label6.TabIndex = 0;
			this.label6.Text = "Text";
			// 
			// labelTextBox
			// 
			this.labelTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelTextBox.Location = new System.Drawing.Point(96, 24);
			this.labelTextBox.Name = "labelTextBox";
			this.labelTextBox.TabIndex = 22;
			this.labelTextBox.Text = "";
			this.labelTextBox.TextChanged += new System.EventHandler(this.labelTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 108);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 23;
			this.label3.Text = "Position";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 24;
			this.label2.Text = "Style";
			// 
			// positionDropdown
			// 
			this.positionDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.positionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.positionDropdown.Location = new System.Drawing.Point(96, 104);
			this.positionDropdown.Name = "positionDropdown";
			this.positionDropdown.Size = new System.Drawing.Size(104, 21);
			this.positionDropdown.TabIndex = 24;
			this.positionDropdown.SelectedIndexChanged += new System.EventHandler(this.positionDropdown_SelectedIndexChanged);
			// 
			// styleDropdown
			// 
			this.styleDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.styleDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.styleDropdown.Location = new System.Drawing.Point(96, 64);
			this.styleDropdown.Name = "styleDropdown";
			this.styleDropdown.Size = new System.Drawing.Size(104, 21);
			this.styleDropdown.TabIndex = 23;
			this.styleDropdown.SelectedIndexChanged += new System.EventHandler(this.styleDropdown_SelectedIndexChanged);
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.Color.White;
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label12,
																				   this.buttonColorDialog,
																				   this.textBoxColor,
																				   this.stateColorDialog1,
																				   this.multiColorDialog1,
																				   this.label11,
																				   this.label10,
																				   this.stateDropdown,
																				   this.valueTextBox,
																				   this.label8,
																				   this.label9,
																				   this.maxValueEditBox,
																				   this.minValueEditBox,
																				   this.valueTypeDropdown,
																				   this.label7});
			this.tabPage3.Location = new System.Drawing.Point(2, 24);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(252, 301);
			this.tabPage3.TabIndex = 31;
			this.tabPage3.Text = "Value";
			this.tabPage3.Visible = false;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(16, 90);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(40, 16);
			this.label12.TabIndex = 0;
			this.label12.Text = "State";
			// 
			// textBoxColor
			// 
			this.textBoxColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxColor.Location = new System.Drawing.Point(96, 104);
			this.textBoxColor.Name = "textBoxColor";
			this.textBoxColor.Size = new System.Drawing.Size(72, 13);
			this.textBoxColor.TabIndex = 39;
			this.textBoxColor.Text = "";
			this.textBoxColor.Visible = false;
			this.textBoxColor.Leave += new System.EventHandler(this.textBoxColor_Leave);
			// 
			// buttonColorDialog
			// 
			this.buttonColorDialog.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.buttonColorDialog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonColorDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.buttonColorDialog.Location = new System.Drawing.Point(180, 106);
			this.buttonColorDialog.Name = "buttonColorDialog";
			this.buttonColorDialog.Size = new System.Drawing.Size(20, 17);
			this.buttonColorDialog.TabIndex = 40;
			this.buttonColorDialog.Text = "...";
			this.buttonColorDialog.TextLocation = new System.Drawing.Point(4, -2);
			this.buttonColorDialog.Visible = false;
			this.buttonColorDialog.Click += new System.EventHandler(this.buttonColorDialog_Click);
			// 
			// stateColorDialog1
			// 
			this.stateColorDialog1.Location = new System.Drawing.Point(64, 144);
			this.stateColorDialog1.Name = "stateColorDialog1";
			this.stateColorDialog1.Size = new System.Drawing.Size(176, 136);
			this.stateColorDialog1.TabIndex = 38;
			this.stateColorDialog1.OnChanged += new System.EventHandler(this.stateColorDialog1_OnChanged);
			// 
			// multiColorDialog1
			// 
			this.multiColorDialog1.BackColor = System.Drawing.Color.White;
			this.multiColorDialog1.Location = new System.Drawing.Point(64, 144);
			multiColor1.ColorStops = new ComponentArt.Web.Visualization.Gauges.ColorStopCollection("Empty");
			this.multiColorDialog1.MultiColor = multiColor1;
			this.multiColorDialog1.Name = "multiColorDialog1";
			this.multiColorDialog1.Size = new System.Drawing.Size(176, 136);
			this.multiColorDialog1.TabIndex = 37;
			this.multiColorDialog1.OnChanged += new System.EventHandler(this.multiColorDialog1_OnChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(16, 148);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(40, 16);
			this.label11.TabIndex = 39;
			this.label11.Text = "Colors";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 108);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 16);
			this.label10.TabIndex = 40;
			this.label10.Text = "Value";
			// 
			// stateDropdown
			// 
			this.stateDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.stateDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.stateDropdown.Location = new System.Drawing.Point(96, 86);
			this.stateDropdown.Name = "stateDropdown";
			this.stateDropdown.Size = new System.Drawing.Size(104, 21);
			this.stateDropdown.TabIndex = 36;
			this.stateDropdown.SelectedIndexChanged += new System.EventHandler(this.stateDropdown_SelectedIndexChanged);
			// 
			// valueTextBox
			// 
			this.valueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.valueTextBox.Location = new System.Drawing.Point(96, 104);
			this.valueTextBox.Name = "valueTextBox";
			this.valueTextBox.Size = new System.Drawing.Size(32, 20);
			this.valueTextBox.TabIndex = 35;
			this.valueTextBox.Text = "0";
			this.valueTextBox.Leave += new System.EventHandler(this.valueTextBox_Leave);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(136, 68);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(60, 16);
			this.label8.TabIndex = 41;
			this.label8.Text = "Max Value";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 68);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(56, 16);
			this.label9.TabIndex = 42;
			this.label9.Text = "Min Value";
			// 
			// maxValueEditBox
			// 
			this.maxValueEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.maxValueEditBox.Location = new System.Drawing.Point(198, 64);
			this.maxValueEditBox.Name = "maxValueEditBox";
			this.maxValueEditBox.Size = new System.Drawing.Size(32, 20);
			this.maxValueEditBox.TabIndex = 34;
			this.maxValueEditBox.Text = "100";
			this.maxValueEditBox.Leave += new System.EventHandler(this.maxValueEditBox_Leave);
			// 
			// minValueEditBox
			// 
			this.minValueEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minValueEditBox.Location = new System.Drawing.Point(96, 64);
			this.minValueEditBox.Name = "minValueEditBox";
			this.minValueEditBox.Size = new System.Drawing.Size(32, 20);
			this.minValueEditBox.TabIndex = 33;
			this.minValueEditBox.Text = "0";
			this.minValueEditBox.Leave += new System.EventHandler(this.minValueEditBox_Leave);
			// 
			// valueTypeDropdown
			// 
			this.valueTypeDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.valueTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.valueTypeDropdown.Location = new System.Drawing.Point(96, 24);
			this.valueTypeDropdown.Name = "valueTypeDropdown";
			this.valueTypeDropdown.Size = new System.Drawing.Size(104, 21);
			this.valueTypeDropdown.TabIndex = 32;
			this.valueTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.valueTypeDropdown_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 28);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(32, 16);
			this.label7.TabIndex = 43;
			this.label7.Text = "Type";
			// 
			// IndicatorsTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "IndicatorsTab";
			this.Size = new System.Drawing.Size(256, 328);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadDataFromObject()
		{
			//Properties Tab
			this.sizeControl.SetValues(1, 100, 1, 100, 1, 1);
			this.sizeControl.X = m_indicator.ImageSize.Width;
			this.sizeControl.Y = m_indicator.ImageSize.Height;
			
			this.positionControl.SetValues(0, 100, 0, 100, 1, 1);
			this.positionControl.X = m_indicator.Location.X;
			this.positionControl.Y = m_indicator.Location.Y;

			this.typeDropdown.Items.Clear();
			foreach(string ik in Enum.GetNames(typeof(IndicatorKind)))
			{
				this.typeDropdown.Items.Add(ik);
			}
			this.typeDropdown.SelectedItem = m_indicator.IndicatorKind.ToString();

			//Label Tab
			this.positionDropdown.Items.Clear();
			foreach(string iplk in Enum.GetNames(typeof(IndicatorLabelPositionKind)))
			{
				this.positionDropdown.Items.Add(iplk);
			}
			this.positionDropdown.SelectedItem = m_indicator.LabelPositionKind.ToString();
			this.styleDropdown.Items.Clear();
			foreach(TextStyle ts in m_indicator.Gauge.TextStyles)
			{
				this.styleDropdown.Items.Add(ts.Name);
			}
			this.styleDropdown.SelectedItem = m_indicator.LabelStyle.Name;
			this.labelTextBox.Text = m_indicator.Label;

			//Value Tab
			this.valueTypeDropdown.Items.Clear();
			foreach(string ivk in Enum.GetNames(typeof(IndicatorValueKind)))
			{
				this.valueTypeDropdown.Items.Add(ivk);
			}

			this.valueTypeDropdown.SelectedItem = m_indicator.IndicatorValueKind.ToString();

			this.maxValueEditBox.Text = m_indicator.MaxValue.ToString();
			this.minValueEditBox.Text = m_indicator.MinValue.ToString();

			this.valueTextBox.Text = m_indicator.Value.ToString();
			this.multiColorDialog1.MultiColor = m_indicator.ValueColors;


			LoadStateDropdown();
			this.stateDropdown.SelectedItem = m_indicator.State;
			this.stateColorDialog1.StateColor = m_indicator.StateColors;

			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color));
			this.textBoxColor.Text = tc.ConvertToString(m_indicator.CustomColor);
			this.buttonColorDialog.BackColor = m_indicator.CustomColor;

		}

		private void LoadStateDropdown()
		{
			bool oldSelectedInTheList = false;
			string oldSelected = (string)stateDropdown.SelectedItem;

			stateDropdown.Items.Clear();
			foreach(StateColor sc in m_indicator.StateColors)
			{
				stateDropdown.Items.Add(sc.Name);
				if (sc.Name == oldSelected)
					oldSelectedInTheList = true;
			}
			if (oldSelectedInTheList)
				stateDropdown.SelectedItem = oldSelected;
			else if (stateDropdown.Items.Count != 0)
				stateDropdown.SelectedIndex = 0;
			else
				stateDropdown.SelectedItem = "";
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
			m_indicator.IndicatorKind = (IndicatorKind)Enum.Parse(typeof(IndicatorKind), (string)this.typeDropdown.SelectedItem, false);
			DoChangedEvent();
		}

		private void positionControl_ValueChanged(object sender, float x, float y)
		{

			m_indicator.Location.X = this.positionControl.X;
			m_indicator.Location.Y = this.positionControl.Y;
			DoChangedEvent();
		}

		private void sizeControl_ValueChanged(object sender, float x, float y)
		{
			m_indicator.ImageSize.Width = this.sizeControl.X;
			m_indicator.ImageSize.Height = this.sizeControl.Y;
			DoChangedEvent();
		}

		private void labelTextBox_TextChanged(object sender, System.EventArgs e)
		{
			m_indicator.Label = this.labelTextBox.Text;
			DoChangedEvent();
		}

		private void styleDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_indicator.LabelStyleName = (string)this.styleDropdown.SelectedItem;
			DoChangedEvent();
		}

		private void positionDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_indicator.LabelPositionKind = (IndicatorLabelPositionKind)Enum.Parse(typeof(IndicatorLabelPositionKind), (string)this.positionDropdown.SelectedItem, false);
			DoChangedEvent();
		}

		private void valueTypeDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_indicator.IndicatorValueKind = (IndicatorValueKind)Enum.Parse(typeof(IndicatorValueKind), (string)this.valueTypeDropdown.SelectedItem, false);
			EnableIndicatorControls();
			DoChangedEvent();
		}

		private void minValueEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_indicator.MinValue = double.Parse(this.minValueEditBox.Text);
			}
			catch(Exception)
			{
				this.minValueEditBox.Text = m_indicator.MinValue.ToString();
			}
			DoChangedEvent();
		}

		private void maxValueEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_indicator.MaxValue = double.Parse(this.maxValueEditBox.Text);
			}
			catch(Exception)
			{
				this.maxValueEditBox.Text = m_indicator.MaxValue.ToString();
			}
			DoChangedEvent();
		}

		private void valueTextBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_indicator.Value = double.Parse(this.valueTextBox.Text);
			}
			catch(Exception)
			{
				this.valueTextBox.Text = m_indicator.Value.ToString();
			}
			DoChangedEvent();
		}

		private void stateDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_indicator.State = (string)this.stateDropdown.SelectedItem;
			DoChangedEvent();
		}

		private void multiColorDialog1_OnChanged(object sender, System.EventArgs e)
		{
			DoChangedEvent();
		}

		private void stateColorDialog1_OnChanged(object sender, System.EventArgs e)
		{
			LoadStateDropdown();
			DoChangedEvent();
		}

		private void buttonColorDialog_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.AllowFullOpen = true;
			dlg.AnyColor = true;
			dlg.FullOpen = true;
			dlg.Color = m_indicator.CustomColor;
			if(dlg.ShowDialog(this) == DialogResult.OK)
			{
				m_indicator.CustomColor = dlg.Color;
				DoChangedEvent();

				TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color));
				this.textBoxColor.Text = tc.ConvertToString(m_indicator.CustomColor);
				this.buttonColorDialog.BackColor = m_indicator.CustomColor;
				Invalidate();
			}
		}

		private void textBoxColor_Leave(object sender, System.EventArgs e)
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color));
			Color color = (Color)tc.ConvertFrom(textBoxColor.Text);
			if(color != m_indicator.CustomColor)
			{
				m_indicator.CustomColor = color;
				DoChangedEvent();

				this.textBoxColor.Text = tc.ConvertToString(m_indicator.CustomColor);
				this.buttonColorDialog.BackColor = m_indicator.CustomColor;
				this.buttonColorDialog.Invalidate();
				Invalidate();
			}
		}

		private void EnableIndicatorControls()
		{
			this.sizeControl.Enabled = (this.m_indicator != null);
			this.positionControl.Enabled = (this.m_indicator != null);
			this.typeDropdown.Enabled = (this.m_indicator != null);
			this.labelTextBox.Enabled = (this.m_indicator != null);
			this.positionDropdown.Enabled = (this.m_indicator != null);
			this.styleDropdown.Enabled = (this.m_indicator != null);
			this.valueTextBox.Enabled = (this.m_indicator != null);
			this.maxValueEditBox.Enabled = (this.m_indicator != null);
			this.minValueEditBox.Enabled = (this.m_indicator != null);
			this.valueTypeDropdown.Enabled = (this.m_indicator != null);

			if (m_indicator != null && m_indicator.IndicatorValueKind == IndicatorValueKind.Numeric)
			{
				this.label11.Text = "Colors";
				this.label10.Text = "Value";
				this.valueTextBox.Visible = true;
				this.multiColorDialog1.Visible = true;
				this.stateDropdown.Visible = false;
				this.stateColorDialog1.Visible = false;
				this.label12.Visible = false;
				this.label11.Visible = true;
				this.label10.Visible = true;
				this.label8.Visible = true;
				this.label9.Visible = true;
				this.maxValueEditBox.Visible = true;
				this.minValueEditBox.Visible = true;
				this.buttonColorDialog.Visible = false;
				textBoxColor.Visible = false;
			}
			else if  (m_indicator != null && m_indicator.IndicatorValueKind == IndicatorValueKind.State)
			{
				this.label11.Text = "States";
				this.stateDropdown.Visible = true;
				this.stateColorDialog1.Visible = true;
				this.valueTextBox.Visible = false;
				this.multiColorDialog1.Visible = false;
				this.label12.Visible = true;
				this.label11.Visible = true;
				this.label10.Visible = false;
				this.label8.Visible = false;
				this.label9.Visible = false;
				this.maxValueEditBox.Visible = false;
				this.minValueEditBox.Visible = false;
				this.buttonColorDialog.Visible = false;
				textBoxColor.Visible = false;
			}
			else if (m_indicator != null)
			{
				this.label10.Text = "Color";
				this.stateDropdown.Visible = false;
				this.stateColorDialog1.Visible = false;
				this.valueTextBox.Visible = false;
				this.multiColorDialog1.Visible = false;
				this.label12.Visible = false;
				this.label11.Visible = false;
				this.label10.Visible = true;
				this.label8.Visible = false;
				this.label9.Visible = false;
				this.maxValueEditBox.Visible = false;
				this.minValueEditBox.Visible = false;
				this.buttonColorDialog.Visible = true;
				textBoxColor.Visible = true;
			}
			else
			{
				this.stateDropdown.Visible = false;
				this.stateColorDialog1.Visible = false;
				this.valueTextBox.Visible = false;
				this.multiColorDialog1.Visible = false;
				this.label12.Visible = false;
				this.label11.Visible = false;
				this.label10.Visible = false;
				this.label8.Visible = false;
				this.label9.Visible = false;
				this.maxValueEditBox.Visible = false;
				this.minValueEditBox.Visible = false;
				this.buttonColorDialog.Visible = false;
				textBoxColor.Visible = false;
			}
		}
	}
}
