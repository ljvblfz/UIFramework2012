using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for ScalesTab.
	/// </summary>
	internal class ScalesTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.TabControl tabControl;
		private ComponentArt.WinUI.TabPage tabPage1;
		private ComponentArt.WinUI.TabPage tabPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox logCheckBox;
		private System.Windows.Forms.TextBox logBaseEditBox;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox maxValueEditBox;
		private System.Windows.Forms.TextBox minValueEditBox;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl relativeCenterControl;
		private ComponentArt.Web.Visualization.Gauges.SliderControl endingMarginSlider;
		private ComponentArt.Web.Visualization.Gauges.SliderControl startingMarginSlider;
		private ComponentArt.Web.Visualization.Gauges.SliderControl startingAngleSlider;
		private ComponentArt.Web.Visualization.Gauges.SliderControl positionSlider;
		private System.Windows.Forms.CheckBox visibleCheckBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Scale m_scale = null;
		PropertyDescriptorCollection m_scaleLayoutDesc = null;
		public new Scale Scale
		{
			set
			{
				m_scale = value;
				if(m_scale != null)
				{
					m_scaleLayoutDesc = TypeDescriptor.GetProperties(m_scale.ScaleLayout);
					LoadDataFromScale();
				}
			}
			get
			{
				return m_scale;
			}
		}

		public delegate void TabEventHandler (object source);
		public event TabEventHandler Changed;

		public ScalesTab()
		{
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
			this.tabControl = new ComponentArt.WinUI.TabControl();
			this.tabPage1 = new ComponentArt.WinUI.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.maxValueEditBox = new System.Windows.Forms.TextBox();
			this.minValueEditBox = new System.Windows.Forms.TextBox();
			this.relativeCenterControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.endingMarginSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.startingMarginSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.startingAngleSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.positionSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.tabPage2 = new ComponentArt.WinUI.TabPage();
			this.label8 = new System.Windows.Forms.Label();
			this.logBaseEditBox = new System.Windows.Forms.TextBox();
			this.logCheckBox = new System.Windows.Forms.CheckBox();
			this.visibleCheckBox = new System.Windows.Forms.CheckBox();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.ColorBehind = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.tabPage1,
																					 this.tabPage2});
			this.tabControl.Multiline = false;
			this.tabControl.Name = "tabControl";
			this.tabControl.Size = new System.Drawing.Size(256, 328);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl.TabIndex = 10;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label7,
																				   this.label6,
																				   this.label5,
																				   this.label4,
																				   this.label3,
																				   this.label2,
																				   this.label1,
																				   this.maxValueEditBox,
																				   this.minValueEditBox,
																				   this.relativeCenterControl,
																				   this.endingMarginSlider,
																				   this.startingMarginSlider,
																				   this.startingAngleSlider,
																				   this.positionSlider});
			this.tabPage1.Location = new System.Drawing.Point(2, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 301);
			this.tabPage1.TabIndex = 11;
			this.tabPage1.Text = "Properties";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 278);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "Ending Margin";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 246);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "Starting Margin";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 214);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Starting Angle";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Relative Center";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(128, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Max Value";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 62);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Position";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Min Value";
			// 
			// maxValueEditBox
			// 
			this.maxValueEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.maxValueEditBox.Location = new System.Drawing.Point(192, 18);
			this.maxValueEditBox.Name = "maxValueEditBox";
			this.maxValueEditBox.Size = new System.Drawing.Size(32, 20);
			this.maxValueEditBox.TabIndex = 13;
			this.maxValueEditBox.Text = "100";
			this.maxValueEditBox.Leave += new System.EventHandler(this.maxValueEditBox_Leave);
			// 
			// minValueEditBox
			// 
			this.minValueEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.minValueEditBox.Location = new System.Drawing.Point(80, 18);
			this.minValueEditBox.Name = "minValueEditBox";
			this.minValueEditBox.Size = new System.Drawing.Size(32, 20);
			this.minValueEditBox.TabIndex = 12;
			this.minValueEditBox.Text = "0";
			this.minValueEditBox.Leave += new System.EventHandler(this.minValueEditBox_Leave);
			// 
			// relativeCenterControl
			// 
			this.relativeCenterControl.Location = new System.Drawing.Point(106, 88);
			this.relativeCenterControl.Name = "relativeCenterControl";
			this.relativeCenterControl.Size = new System.Drawing.Size(104, 104);
			this.relativeCenterControl.TabIndex = 15;
			this.relativeCenterControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.RelativeCenterControl_Changed);
			// 
			// endingMarginSlider
			// 
			this.endingMarginSlider.Location = new System.Drawing.Point(106, 262);
			this.endingMarginSlider.Name = "endingMarginSlider";
			this.endingMarginSlider.Size = new System.Drawing.Size(104, 32);
			this.endingMarginSlider.TabIndex = 18;
			this.endingMarginSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.EndingMarginSlider_Changed);
			// 
			// startingMarginSlider
			// 
			this.startingMarginSlider.Location = new System.Drawing.Point(106, 230);
			this.startingMarginSlider.Name = "startingMarginSlider";
			this.startingMarginSlider.Size = new System.Drawing.Size(104, 32);
			this.startingMarginSlider.TabIndex = 17;
			this.startingMarginSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.StartingMarginSlider_Changed);
			// 
			// startingAngleSlider
			// 
			this.startingAngleSlider.Location = new System.Drawing.Point(106, 198);
			this.startingAngleSlider.Name = "startingAngleSlider";
			this.startingAngleSlider.Size = new System.Drawing.Size(104, 32);
			this.startingAngleSlider.TabIndex = 16;
			this.startingAngleSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.StartingAngleSlider_Changed);
			// 
			// positionSlider
			// 
			this.positionSlider.Location = new System.Drawing.Point(106, 48);
			this.positionSlider.Name = "positionSlider";
			this.positionSlider.Size = new System.Drawing.Size(104, 32);
			this.positionSlider.TabIndex = 14;
			this.positionSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.PositionSlider_Changed);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.White;
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label8,
																				   this.logBaseEditBox,
																				   this.logCheckBox,
																				   this.visibleCheckBox});
			this.tabPage2.Location = new System.Drawing.Point(2, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(252, 301);
			this.tabPage2.TabIndex = 21;
			this.tabPage2.Text = "Advanced";
			this.tabPage2.Visible = false;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(136, 62);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "LogBase";
			// 
			// logBaseEditBox
			// 
			this.logBaseEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.logBaseEditBox.Location = new System.Drawing.Point(192, 58);
			this.logBaseEditBox.Name = "logBaseEditBox";
			this.logBaseEditBox.Size = new System.Drawing.Size(32, 20);
			this.logBaseEditBox.TabIndex = 23;
			this.logBaseEditBox.Text = "10";
			this.logBaseEditBox.Leave += new System.EventHandler(this.logBaseEditBox_Leave);
			// 
			// logCheckBox
			// 
			this.logCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.logCheckBox.Location = new System.Drawing.Point(24, 58);
			this.logCheckBox.Name = "logCheckBox";
			this.logCheckBox.Size = new System.Drawing.Size(88, 24);
			this.logCheckBox.TabIndex = 22;
			this.logCheckBox.Text = "Logarithmic";
			this.logCheckBox.CheckedChanged += new System.EventHandler(this.logCheckBox_Changed);
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
			// ScalesTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl});
			this.Name = "ScalesTab";
			this.Size = new System.Drawing.Size(256, 328);
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadDataFromScale()
		{
			if (double.IsNaN(m_scale.ScaleLayout.Position))
				positionSlider.SliderValue = new SliderValue(0, 100, m_scale.EffectivePosition, 1);
			else
				positionSlider.SliderValue = new SliderValue(0, 100, m_scale.ScaleLayout.Position, 1);
			
			relativeCenterControl.SetValues(0, 100, 0, 100, 1, 1);
			if (m_scale.ScaleLayout.RelativeCenter == null)
			{
				relativeCenterControl.X = m_scale.RelativeCenter.X; 
				relativeCenterControl.Y = m_scale.RelativeCenter.Y;
			}
			else
			{
				relativeCenterControl.X = m_scale.ScaleLayout.RelativeCenter.X; 
				relativeCenterControl.Y = m_scale.ScaleLayout.RelativeCenter.Y;
			}

//			startingAngleSlider.SliderValue = new SliderValue(0, 360, (double)m_scaleLayoutDesc["StartingAngle"].Converter.ConvertTo(m_scale.ScaleLayout.StartingAngle, typeof(double)), 5);
//			startingMarginSlider.SliderValue = new SliderValue(0, 50, (double)m_scaleLayoutDesc["StartingMargin"].Converter.ConvertTo(m_scale.ScaleLayout.StartingMargin, typeof(double)), 1);
//			endingMarginSlider.SliderValue = new SliderValue(0, 50, (double)m_scaleLayoutDesc["EndingMargin"].Converter.ConvertTo(m_scale.ScaleLayout.EndingMargin, typeof(double)), 1);

			if (double.IsNaN(m_scale.ScaleLayout.StartingAngle))
				startingAngleSlider.SliderValue = new SliderValue(0, 360, m_scale.StartingAngle, 5);
			else
				startingAngleSlider.SliderValue = new SliderValue(0, 360, m_scale.ScaleLayout.StartingAngle, 5);
			if (double.IsNaN(m_scale.ScaleLayout.StartingMargin))
				startingMarginSlider.SliderValue = new SliderValue(0, 50, m_scale.EffectiveStartingMargin, 1);
			else
				startingMarginSlider.SliderValue = new SliderValue(0, 50, m_scale.ScaleLayout.StartingMargin, 1);
			if (double.IsNaN(m_scale.ScaleLayout.EndingMargin))
				endingMarginSlider.SliderValue = new SliderValue(0, 50, m_scale.EffectiveEndingMargin, 1);
			else
				endingMarginSlider.SliderValue = new SliderValue(0, 50, m_scale.ScaleLayout.EndingMargin, 1);


			minValueEditBox.Text = m_scale.MinValue.ToString();
			maxValueEditBox.Text = m_scale.MaxValue.ToString();
			
			//Logarithmic Tab
			this.visibleCheckBox.Checked = m_scale.Visible;
			logCheckBox.Checked = m_scale.IsLogarithmic;
			logBaseEditBox.Text = m_scale.LogBase.ToString();
		}

		private void DoChangedEvent()
		{
			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void PositionSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_scale.ScaleLayout.Position = positionSlider.SliderValue.Value;
			DoChangedEvent();
		}

		private void RelativeCenterControl_Changed(object sender, float x, float y)
		{
			if (m_scale.ScaleLayout == null || m_scale.ScaleLayout.RelativeCenter == null)
				return;
			m_scale.ScaleLayout.RelativeCenter.X = relativeCenterControl.X; 
			m_scale.ScaleLayout.RelativeCenter.Y = relativeCenterControl.Y;
			DoChangedEvent();
		}

		private void StartingAngleSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_scale.ScaleLayout.StartingAngle = startingAngleSlider.SliderValue.Value;
			DoChangedEvent();
		}

		private void StartingMarginSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_scale.ScaleLayout.StartingMargin = startingMarginSlider.SliderValue.Value;
			DoChangedEvent();
		}

		private void EndingMarginSlider_Changed(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_scale.ScaleLayout.EndingMargin = endingMarginSlider.SliderValue.Value;
			DoChangedEvent();
		}

		private void minValueEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_scale.MinValue = double.Parse(minValueEditBox.Text);
				DoChangedEvent();
			}
			catch(Exception)
			{
				minValueEditBox.Text = m_scale.MinValue.ToString();
			}
		}

		private void maxValueEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_scale.MaxValue = double.Parse(maxValueEditBox.Text);
				DoChangedEvent();
			}
			catch(Exception)
			{
				maxValueEditBox.Text = m_scale.MaxValue.ToString();
			}
		}

		private void visibleCheckBox_Changed(object sender, System.EventArgs e)
		{
			m_scale.Visible = this.visibleCheckBox.Checked;
			DoChangedEvent();
		}

		private void logCheckBox_Changed(object sender, System.EventArgs e)
		{
			m_scale.IsLogarithmic = logCheckBox.Checked;
			DoChangedEvent();
		}

		private void logBaseEditBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				m_scale.LogBase = double.Parse(logBaseEditBox.Text);
				DoChangedEvent();
			}
			catch(Exception)
			{
				logBaseEditBox.Text = m_scale.LogBase.ToString();
			}
		}
	}
}
