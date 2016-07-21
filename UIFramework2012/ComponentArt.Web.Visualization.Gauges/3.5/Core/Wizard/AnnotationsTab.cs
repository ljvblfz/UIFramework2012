using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for AnnotationsTab.
	/// </summary>
	internal class AnnotationsTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.TabControl tabControl1;
		private ComponentArt.WinUI.TabPage tabPage1;
		private ComponentArt.WinUI.TabPage tabPage2;
		private System.Windows.Forms.Label labelTextHelp;
		private System.Windows.Forms.Label labelImageHelp;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox imagePathTextBox;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl imageSizeControl;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl imagePositionControl;
		private ComponentArt.Web.Visualization.Gauges.SizePositionControl textPositionControl;
		private ComponentArt.WinUI.ComboBox textStyleDropdown;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label7;
		private ComponentArt.Web.Visualization.Gauges.SliderControl textAngleSlider;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ImageAnnotation m_imageAnnotation = null;
		public ImageAnnotation ImageAnnotation
		{
			set
			{
				m_imageAnnotation = value;
				if (m_imageAnnotation == null)
					return;
				m_imageAnnotationCollection = null;
				LoadDataFromImageAnnotation();
				this.tabControl1.SelectedIndex = 1;

				if (m_imageAnnotation.Gauge.TextAnnotations.Count > 0)
				{
					m_textAnnotation = m_imageAnnotation.Gauge.TextAnnotations[0];
					m_textAnnotationCollection = null;
				}
				else
				{
					m_textAnnotation = null;
					m_textAnnotationCollection = m_imageAnnotation.Gauge.TextAnnotations;
				}
				if (m_textAnnotation != null)
					LoadDataFromTextAnnotation();
				EnableAnnotationControls();
			}
			get
			{
				return m_imageAnnotation;
			}
		}

		private TextAnnotation m_textAnnotation = null;
		public TextAnnotation TextAnnotation
		{
			set
			{
				m_textAnnotation = value;
				if (m_textAnnotation == null)
					return;
				m_textAnnotationCollection = null;
				LoadDataFromTextAnnotation();
				this.tabControl1.SelectedIndex = 0;

				if (m_textAnnotation.Gauge.ImageAnnotations.Count > 0)
				{
					m_imageAnnotation = m_textAnnotation.Gauge.ImageAnnotations[0];
					m_imageAnnotationCollection = null;
				}
				else
				{
					m_imageAnnotation = null;
					m_imageAnnotationCollection =  m_textAnnotation.Gauge.ImageAnnotations;
				}
				if (m_imageAnnotation != null)
					LoadDataFromImageAnnotation();
				EnableAnnotationControls();
			}
			get
			{
				return m_textAnnotation;
			}
		}

		private ImageAnnotationCollection m_imageAnnotationCollection = null;
		public ImageAnnotationCollection ImageAnnotationCollection
		{
			set
			{
				m_imageAnnotationCollection = value;
				if (m_imageAnnotationCollection == null)
					return;

				m_imageAnnotation = null;
				this.tabControl1.SelectedIndex = 1;
				
				if (m_imageAnnotationCollection.Gauge.TextAnnotations.Count > 0)
				{
					m_textAnnotation = m_imageAnnotationCollection.Gauge.TextAnnotations[0];
					m_textAnnotationCollection = null;
				}
				else
				{
					m_textAnnotation = null;
					m_textAnnotationCollection = m_imageAnnotationCollection.Gauge.TextAnnotations;
				}
				if (m_textAnnotation != null)
					LoadDataFromTextAnnotation();
				EnableAnnotationControls();
			}
			get
			{
				return m_imageAnnotationCollection;
			}
		}

		private TextAnnotationCollection m_textAnnotationCollection = null;
		public TextAnnotationCollection TextAnnotationCollection
		{
			set
			{
				m_textAnnotationCollection = value;
				if (m_textAnnotationCollection == null)
					return;
				m_textAnnotation = null;
				this.tabControl1.SelectedIndex = 0;

				if (m_textAnnotationCollection.Gauge.ImageAnnotations.Count > 0)
				{
					m_imageAnnotation = m_textAnnotationCollection.Gauge.ImageAnnotations[0];
					m_imageAnnotationCollection = null;
				}
				else
				{
					m_imageAnnotation = null;
					m_imageAnnotationCollection = m_textAnnotationCollection.Gauge.ImageAnnotations;
				}
				if (m_imageAnnotation != null)
					LoadDataFromImageAnnotation();
				EnableAnnotationControls();
			}
			get
			{
				return m_textAnnotationCollection;
			}
		}

		public delegate void TabEventHandler (object source);
		public delegate void TabEventHandlerWithArgs (object source, TabSelectEventArgs e);
		public event TabEventHandler Changed;
		public event TabEventHandlerWithArgs TabSelected;


		public AnnotationsTab()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			EnableAnnotationControls();
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
			this.tabPage2 = new ComponentArt.WinUI.TabPage();
			this.labelTextHelp = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textAngleSlider = new ComponentArt.Web.Visualization.Gauges.SliderControl();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textStyleDropdown = new ComponentArt.WinUI.ComboBox();
			this.textPositionControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.tabPage1 = new ComponentArt.WinUI.TabPage();
			this.labelImageHelp = new System.Windows.Forms.Label();
			this.imageSizeControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.imagePositionControl = new ComponentArt.Web.Visualization.Gauges.SizePositionControl();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.imagePathTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.ColorBehind = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage2,
																					  this.tabPage1});
			this.tabControl1.Multiline = false;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Size = new System.Drawing.Size(256, 328);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl1.TabIndex = 31;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_TabChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.White;
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.labelTextHelp,
																				   this.label7,
																				   this.textAngleSlider,
																				   this.label3,
																				   this.label6,
																				   this.textTextBox,
																				   this.label2,
																				   this.textStyleDropdown,
																				   this.textPositionControl});
			this.tabPage2.Location = new System.Drawing.Point(2, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(252, 301);
			this.tabPage2.TabIndex = 11;
			this.tabPage2.Text = "Text Annotations";
			// 
			// labelTextHelp
			// 
			this.labelTextHelp.Location = new System.Drawing.Point(16, 36);
			this.labelTextHelp.Name = "labelTextHelp";
			this.labelTextHelp.Size = new System.Drawing.Size(224, 44);
			this.labelTextHelp.TabIndex = 0;
			this.labelTextHelp.Text = "Create a new text annotation or select an existing one.";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 238);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Angle";
			// 
			// textAngleSlider
			// 
			this.textAngleSlider.Location = new System.Drawing.Point(96, 222);
			this.textAngleSlider.Name = "textAngleSlider";
			this.textAngleSlider.Size = new System.Drawing.Size(104, 32);
			this.textAngleSlider.TabIndex = 15;
			this.textAngleSlider.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SliderValueChangedHandler(this.textAngleSlider_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 152);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Position";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 28);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "Text";
			// 
			// textTextBox
			// 
			this.textTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textTextBox.Location = new System.Drawing.Point(96, 24);
			this.textTextBox.Name = "textTextBox";
			this.textTextBox.TabIndex = 12;
			this.textTextBox.Text = "";
			this.textTextBox.TextChanged += new System.EventHandler(this.textTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 18;
			this.label2.Text = "Style";
			// 
			// textStyleDropdown
			// 
			this.textStyleDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.textStyleDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.textStyleDropdown.Location = new System.Drawing.Point(96, 64);
			this.textStyleDropdown.Name = "textStyleDropdown";
			this.textStyleDropdown.Size = new System.Drawing.Size(104, 21);
			this.textStyleDropdown.TabIndex = 13;
			this.textStyleDropdown.SelectedIndexChanged += new System.EventHandler(this.textStyleDropdown_SelectedIndexChanged);
			// 
			// textPositionControl
			// 
			this.textPositionControl.Location = new System.Drawing.Point(96, 104);
			this.textPositionControl.Name = "textPositionControl";
			this.textPositionControl.Size = new System.Drawing.Size(104, 104);
			this.textPositionControl.TabIndex = 14;
			this.textPositionControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.textPositionControl_ValueChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.labelImageHelp,
																				   this.imageSizeControl,
																				   this.imagePositionControl,
																				   this.label5,
																				   this.label4,
																				   this.imagePathTextBox,
																				   this.label1});
			this.tabPage1.Location = new System.Drawing.Point(2, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 301);
			this.tabPage1.TabIndex = 21;
			this.tabPage1.Text = "Image Annotations";
			this.tabPage1.Visible = false;
			// 
			// labelImageHelp
			// 
			this.labelImageHelp.Location = new System.Drawing.Point(16, 36);
			this.labelImageHelp.Name = "labelImageHelp";
			this.labelImageHelp.Size = new System.Drawing.Size(224, 44);
			this.labelImageHelp.TabIndex = 0;
			this.labelImageHelp.Text = "Create a new image annotation or select an existing one.";
			// 
			// imageSizeControl
			// 
			this.imageSizeControl.Location = new System.Drawing.Point(128, 100);
			this.imageSizeControl.Name = "imageSizeControl";
			this.imageSizeControl.Size = new System.Drawing.Size(104, 104);
			this.imageSizeControl.TabIndex = 24;
			this.imageSizeControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.imageSizeControl_ValueChanged);
			// 
			// imagePositionControl
			// 
			this.imagePositionControl.Location = new System.Drawing.Point(16, 100);
			this.imagePositionControl.Name = "imagePositionControl";
			this.imagePositionControl.Size = new System.Drawing.Size(104, 104);
			this.imagePositionControl.TabIndex = 23;
			this.imagePositionControl.ValueChanged += new ComponentArt.Web.Visualization.Gauges.SizePositionEditor.ValueChangedHandler(this.imagePositionControl_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(128, 80);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 16);
			this.label5.TabIndex = 25;
			this.label5.Text = "Size";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 26;
			this.label4.Text = "Position";
			// 
			// imagePathTextBox
			// 
			this.imagePathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imagePathTextBox.Location = new System.Drawing.Point(16, 40);
			this.imagePathTextBox.Name = "imagePathTextBox";
			this.imagePathTextBox.Size = new System.Drawing.Size(216, 20);
			this.imagePathTextBox.TabIndex = 22;
			this.imagePathTextBox.Text = "";
			this.imagePathTextBox.TextChanged += new System.EventHandler(this.imagePathTextBox_Leave);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 27;
			this.label1.Text = "Path";
			// 
			// AnnotationsTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "AnnotationsTab";
			this.Size = new System.Drawing.Size(256, 328);
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadDataFromImageAnnotation()
		{
#if WEB
			this.imagePathTextBox.Text = m_imageAnnotation.ImageURL;
#endif
			this.imageSizeControl.SizeMode = true;
			this.imageSizeControl.SetValues(1, 100, 1, 100, 1, 1);
			this.imageSizeControl.X = m_imageAnnotation.Size.Width;
			this.imageSizeControl.Y = m_imageAnnotation.Size.Height;
			this.imagePositionControl.SizeMode = false;
			this.imagePositionControl.SetValues(0, 100, 0, 100, 1, 1);
			this.imagePositionControl.X = m_imageAnnotation.RelativeLocation.X;
			this.imagePositionControl.Y = m_imageAnnotation.RelativeLocation.Y;
		}

		private void LoadDataFromTextAnnotation()
		{
			this.textPositionControl.SizeMode = false;
			this.textPositionControl.SetValues(0, 100, 0, 100, 1, 1);
			this.textPositionControl.X = m_textAnnotation.RelativeLocation.X;
			this.textPositionControl.Y = m_textAnnotation.RelativeLocation.Y;
			this.textTextBox.Text = m_textAnnotation.Text;
			textStyleDropdown.Items.Clear();
			foreach(TextStyle ts in m_textAnnotation.Gauge.TextStyles)
			{
				textStyleDropdown.Items.Add(ts.Name);
			}
			this.textStyleDropdown.SelectedItem = m_textAnnotation.TextStyle.Name;
			this.textAngleSlider.SliderValue = new SliderValue(-180, 180, m_textAnnotation.AngleDegrees, 5);
		}

		private void EnableAnnotationControls()
		{
			if (this.tabControl1.SelectedIndex == 0)
			{
				this.tabPage1.Visible = false;
				this.tabPage2.Visible = true;
			}
			else if (this.tabControl1.SelectedIndex == 1)
			{
				this.tabPage1.Visible = true;
				this.tabPage2.Visible = false;
			}

			this.imageSizeControl.Visible = (m_imageAnnotation != null);
			this.imagePositionControl.Visible = (m_imageAnnotation != null);
			this.imagePathTextBox.Visible = (m_imageAnnotation != null);
			this.label5.Visible = (m_imageAnnotation != null);
			this.label4.Visible = (m_imageAnnotation != null);
			this.imagePathTextBox.Visible = (m_imageAnnotation != null);
			this.label1.Visible = (m_imageAnnotation != null);
			this.labelImageHelp.Visible = (m_imageAnnotation == null);

			this.textAngleSlider.Visible = (m_textAnnotation != null);
			this.textTextBox.Visible = (m_textAnnotation != null);
			this.textStyleDropdown.Visible = (m_textAnnotation != null);
			this.textPositionControl.Visible = (m_textAnnotation != null);
			this.label7.Visible = (m_textAnnotation != null);
			this.label3.Visible = (m_textAnnotation != null);
			this.label6.Visible = (m_textAnnotation != null);
			this.label2.Visible = (m_textAnnotation != null);
			this.labelTextHelp.Visible = (m_textAnnotation == null);
		}

		private void DoChangedEvent()
		{
			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void imagePathTextBox_Leave(object sender, System.EventArgs e)
		{
#if WEB
			m_imageAnnotation.ImageURL = this.imagePathTextBox.Text;
#endif
			DoChangedEvent();
		}

		private void imagePositionControl_ValueChanged(object sender, float x, float y)
		{
			m_imageAnnotation.RelativeLocation.X = this.imagePositionControl.X;
			m_imageAnnotation.RelativeLocation.Y = this.imagePositionControl.Y;
			DoChangedEvent();
		}

		private void imageSizeControl_ValueChanged(object sender, float x, float y)
		{
			m_imageAnnotation.Size.Width = this.imageSizeControl.X;
			m_imageAnnotation.Size.Height = this.imageSizeControl.Y;
			DoChangedEvent();		
		}

		private void textTextBox_TextChanged(object sender, System.EventArgs e)
		{
			m_textAnnotation.Text = this.textTextBox.Text;
			DoChangedEvent();
		}

		private void textStyleDropdown_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_textAnnotation.TextStyleName = (string)this.textStyleDropdown.SelectedItem;
			DoChangedEvent();
		}

		private void textPositionControl_ValueChanged(object sender, float x, float y)
		{
			m_textAnnotation.RelativeLocation.X = this.textPositionControl.X;
			m_textAnnotation.RelativeLocation.Y = this.textPositionControl.Y;
			DoChangedEvent();
		}

		private void textAngleSlider_ValueChanged(object sender, ComponentArt.Web.Visualization.Gauges.SliderValue sliderValue)
		{
			m_textAnnotation.AngleDegrees = this.textAngleSlider.SliderValue.Value;
			DoChangedEvent();
		}

		private void tabControl1_TabChanged(object sender, System.EventArgs e)
		{
			TabSelectEventArgs args = null;
			if (TabSelected != null)
			{
				if (this.tabControl1.SelectedIndex == 1)
				{
					if(m_imageAnnotation != null)
						args = new TabSelectEventArgs(m_imageAnnotation);
					else if (m_imageAnnotationCollection != null)
						args = new TabSelectEventArgs(m_imageAnnotationCollection);
				}
				else
				{
					if(m_textAnnotation != null)
						args = new TabSelectEventArgs(m_textAnnotation);
					else if (m_textAnnotationCollection != null)
						args = new TabSelectEventArgs(m_textAnnotationCollection);
				}
				TabSelected(this, args);
			}
		}
	}
}
