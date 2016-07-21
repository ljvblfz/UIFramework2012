using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for StartTab.
	/// </summary>
	internal class StartTab : System.Windows.Forms.UserControl
	{
		private ComponentArt.WinUI.GroupBox CriteriaGroupBox;
		private ComponentArt.WinUI.GroupBox ThumbnailGroupBox;
		private ComponentArt.WinUI.ComboBox GaugeThemeControl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private ComponentArt.WinUI.ComboBox GaugeKindGroupControl;
		private ComponentArt.Web.Visualization.Gauges.GaugeThumbnailListBox gaugeThumbnailListBox1;
		private ComponentArt.Web.Visualization.Gauges.ProgressBar progressBar1;
		private ComponentArt.WinUI.Button loadFromTemplateButton;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public delegate void TabEventHandler (object source);
		public event TabEventHandler Changed;

		private string m_templateFileName = "";

#if WEB
		private Gauge m_gauge = null;
		public Gauge Gauge
#else
		private WinGauge m_gauge = null;
		public WinGauge Gauge
#endif
		{
			set
			{
				m_gauge = value;
				gaugeThumbnailListBox1.ImageGenerationGauge = value;
				PopulateControls();

			}
		}

		public StartTab()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			gaugeThumbnailListBox1.ProgressBar = progressBar1;
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
			this.CriteriaGroupBox = new ComponentArt.WinUI.GroupBox();
			this.ThumbnailGroupBox = new ComponentArt.WinUI.GroupBox();
			this.GaugeThemeControl = new ComponentArt.WinUI.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.GaugeKindGroupControl = new ComponentArt.WinUI.ComboBox();
			this.gaugeThumbnailListBox1 = new ComponentArt.Web.Visualization.Gauges.GaugeThumbnailListBox();
			this.progressBar1 = new ComponentArt.Web.Visualization.Gauges.ProgressBar();
			this.loadFromTemplateButton = new ComponentArt.WinUI.Button();
			this.SuspendLayout();
			// 
			// GaugeThemeControl
			// 
			this.GaugeThemeControl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.GaugeThemeControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GaugeThemeControl.Location = new System.Drawing.Point(276, 36);
			this.GaugeThemeControl.Name = "GaugeThemeControl";
			this.GaugeThemeControl.Size = new System.Drawing.Size(121, 21);
			this.GaugeThemeControl.TabIndex = 12;
			this.GaugeThemeControl.SelectedIndexChanged += new System.EventHandler(this.GaugeThemeControl_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(231, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.Text = "Theme";
			this.label1.BackColor = Color.FromArgb(255, 255, 255);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.Text = "Gauge Type";
			this.label2.BackColor = Color.FromArgb(255, 255, 255);
			// 
			// GaugeKindGroupControl
			// 
			this.GaugeKindGroupControl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.GaugeKindGroupControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GaugeKindGroupControl.Location = new System.Drawing.Point(88, 36);
			this.GaugeKindGroupControl.Name = "GaugeKindGroupControl";
			this.GaugeKindGroupControl.Size = new System.Drawing.Size(121, 21);
			this.GaugeKindGroupControl.TabIndex = 11;
			this.GaugeKindGroupControl.SelectedIndexChanged += new System.EventHandler(this.GaugeKindGroupControl_SelectedIndexChanged);
			// 
			// gaugeThumbnailListBox1
			// 
			this.gaugeThumbnailListBox1.BackColor = System.Drawing.Color.White;
			this.gaugeThumbnailListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.gaugeThumbnailListBox1.IntegralHeight = true;
			this.gaugeThumbnailListBox1.ItemHeight = 90;
			this.gaugeThumbnailListBox1.Location = new System.Drawing.Point(3, 3);
			this.gaugeThumbnailListBox1.MultiColumn = true;
			this.gaugeThumbnailListBox1.Name = "gaugeThumbnailListBox1";
			this.gaugeThumbnailListBox1.SelectedIndex = -1;
			this.gaugeThumbnailListBox1.SelectedItem = null;
			this.gaugeThumbnailListBox1.Size = new System.Drawing.Size(640, 232);
			this.gaugeThumbnailListBox1.TabIndex = 14;
			this.gaugeThumbnailListBox1.Visible = false;
			this.gaugeThumbnailListBox1.SelectedIndexChanged += new System.EventHandler(this.gaugeThumbnailListBox1_SelectedIndexChanged);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(114, 180);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(421, 30);
			this.progressBar1.Text = "progressBar1";
			this.progressBar1.TabStop = false;
			// 
			// loadFromTemplateButton
			// 
			this.loadFromTemplateButton.Location = new System.Drawing.Point(464, 36);
			this.loadFromTemplateButton.Name = "loadFromTemplateButton";
			this.loadFromTemplateButton.Size = new System.Drawing.Size(152, 23);
			this.loadFromTemplateButton.TabIndex = 13;
			this.loadFromTemplateButton.Text = "Load From Template";
			this.loadFromTemplateButton.Click += new System.EventHandler(this.loadFromTemplateButton_Click);
			// 
			// CriteriaGroupBox
			// 
			this.CriteriaGroupBox.Location = new System.Drawing.Point(0, 4);
			this.CriteriaGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.loadFromTemplateButton,
																									this.GaugeThemeControl,
																									this.GaugeKindGroupControl,
																					   				this.label2,
																									this.label1});
			this.CriteriaGroupBox.Name = "CriteriaGroupBox";
			this.CriteriaGroupBox.ResizeChildren = false;
			this.CriteriaGroupBox.Size = new System.Drawing.Size(646, 68);
			this.CriteriaGroupBox.TabIndex = 10;
			this.CriteriaGroupBox.Text = "";
			// 
			// ThumbnailGroupBox
			// 
			this.ThumbnailGroupBox.Location = new System.Drawing.Point(0, 84);
			this.ThumbnailGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.gaugeThumbnailListBox1});
			this.ThumbnailGroupBox.Name = "ThumbnailGroupBox";
			this.ThumbnailGroupBox.ResizeChildren = false;
			this.ThumbnailGroupBox.Size = new System.Drawing.Size(646, 238);
			this.ThumbnailGroupBox.TabIndex = 10;
			this.ThumbnailGroupBox.Text = "";
			// 
			// StartTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.progressBar1,
																		  this.ThumbnailGroupBox,
																		  this.CriteriaGroupBox});
			this.Name = "StartTab";
			this.Size = new System.Drawing.Size(640, 288);
			this.ResumeLayout(false);

		}
		#endregion


		private void PopulateControls()
		{
			GaugeThemeControl.Items.Clear();
			foreach (NamedObject no in m_gauge.Themes)
			{
				GaugeThemeControl.Items.Add(no.Name);
			}
			this.GaugeThemeControl.SelectedItem = m_gauge.ThemeName;

			GaugeKindGroupControl.Items.Clear();
			foreach (GaugeKindGroup gkg in GaugeThumbnailListBox.SelectableGaugeKindGroups)
			{
				GaugeKindGroupControl.Items.Add(gkg.ToString());
			}
			GaugeKindGroupControl.SelectedItem = GaugeKindGroup.All.ToString();
		}

		public void GeneratePreviewImages()
		{
		    gaugeThumbnailListBox1.GeneratePlaceholders();

		    GaugeKindGroup gaugeKindGroup = (GaugeKindGroup)Enum.Parse(typeof(GaugeKindGroup), GaugeKindGroupControl.Text, false);
		    if (m_gauge.GaugeKind == GaugeKind.Numeric)
				gaugeThumbnailListBox1.SetNewCriteria(gaugeKindGroup, GaugeThemeControl.Text, m_gauge.Skin.NumericDisplayKind);
			else
				gaugeThumbnailListBox1.SetNewCriteria(gaugeKindGroup, GaugeThemeControl.Text, m_gauge.GaugeKind);

		    gaugeThumbnailListBox1.Invalidate();
			Refresh();
		}

		private void GaugeThemeControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_gauge.ThemeName == GaugeThemeControl.Text)
				return;
			m_gauge.ThemeName = GaugeThemeControl.Text;
			GaugeKindGroup gaugeKindGroup;
			try
			{
				gaugeKindGroup = (GaugeKindGroup)Enum.Parse(typeof(GaugeKindGroup), GaugeKindGroupControl.Text, false);
				if (m_gauge.GaugeKind == GaugeKind.Numeric)
					gaugeThumbnailListBox1.SetNewCriteria(gaugeKindGroup, GaugeThemeControl.Text, m_gauge.Skin.NumericDisplayKind);
				else
					gaugeThumbnailListBox1.SetNewCriteria(gaugeKindGroup, GaugeThemeControl.Text, m_gauge.GaugeKind);
			}
			catch (Exception) { }
			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void GaugeKindGroupControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			GaugeKindGroup gaugeKindGroup;
			try
			{
				gaugeKindGroup = (GaugeKindGroup)Enum.Parse(typeof(GaugeKindGroup), GaugeKindGroupControl.Text, false);
				if (m_gauge.GaugeKind == GaugeKind.Numeric)
					gaugeThumbnailListBox1.SetNewCriteria(gaugeKindGroup, GaugeThemeControl.Text, m_gauge.Skin.NumericDisplayKind);
				else
					gaugeThumbnailListBox1.SetNewCriteria(gaugeKindGroup, GaugeThemeControl.Text, m_gauge.GaugeKind);
			}
			catch (Exception) { }

			if (gaugeThumbnailListBox1.SelectedIndex == -1)
				return;

			GaugeThumbnailPlaceholder gt = ((GaugeThumbnailPlaceholder)((ListBoxThumbnail)gaugeThumbnailListBox1.SelectedItem).Item);

			if (m_gauge.GaugeKind == gt.GaugeKind && m_gauge.ThemeName == gt.ThemeName)
				return;
			m_gauge.GaugeKind = gt.GaugeKind;
			m_gauge.ThemeName = gt.ThemeName;

			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void gaugeThumbnailListBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (gaugeThumbnailListBox1.SelectedIndex == -1)
				return;

			GaugeThumbnailPlaceholder gt = ((GaugeThumbnailPlaceholder)((ListBoxThumbnail)gaugeThumbnailListBox1.SelectedItem).Item);

			if (m_gauge.GaugeKind == gt.GaugeKind && m_gauge.ThemeName == gt.ThemeName)
				return;
			m_gauge.GaugeKind = gt.GaugeKind;
			m_gauge.ThemeName = gt.ThemeName;
			if (gt.GaugeKind == GaugeKind.Numeric)
				m_gauge.Skin.NumericDisplayKind = gt.NumericDisplayKind;

			if (Changed != null)
			{
				Changed(this);
			}
		}

		private void loadFromTemplateButton_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.DefaultExt = "xml";
			dlg.Title = "Loading Template";
			if(m_templateFileName == "")
			{
				dlg.InitialDirectory = Application.ExecutablePath;
				dlg.FileName = "";
			}
			else
			{
				int ix = m_templateFileName.LastIndexOf(@"\");

				dlg.InitialDirectory = m_templateFileName.Substring(0,ix);
				dlg.FileName = m_templateFileName.Substring(ix+1);
			}
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				m_templateFileName = dlg.FileName;
				m_gauge.XMLDeserialize(m_templateFileName);

				PopulateControls();
				GeneratePreviewImages();

				if (Changed != null)
				{
					Changed(this);
				}
			}
		}		
	}
}
