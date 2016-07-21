using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using ComponentArt.Win.UI.Internal;
using System.Reflection;
using System.Drawing.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardAppearanceDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.TabControl tabControl1;
		private Design.WizardPaletteDialog wizardPaletteDialog;
		private ComponentArt.Win.UI.Internal.TabPage m_backgroundTabPage;
		[WizardHint(ChartBase.MainAssemblyTypeName, 
#if __BuildingWebChart__
			 "BackgroundImageUrl"
#else
#if __BUILDING_CRI_DESIGNER__
            "BackgroundImageForSqlChart"
#else
			"BackgroundImage"
#endif
#endif
			 )]
		private ComponentArt.Win.UI.Internal.Button m_openFileButton;
		private System.Windows.Forms.Panel panel1;
		private ComponentArt.Win.UI.Internal.Button m_removeButton;
		private ComponentArt.Win.UI.Internal.GroupBox m_backGroungImageGroupBox;
		[WizardHint(ChartBase.MainAssemblyTypeName, "BackGradientKind")]
		private ComponentArt.Win.UI.Internal.GroupBox m_backGroungGradientGroupBox;
		private ComponentArt.Win.UI.Internal.ListBox m_gradientListBox;
		private ComponentArt.Win.UI.Internal.TabPage m_frameTabPage;
		private ComponentArt.Web.Visualization.Charting.Design.WizardFrameControl wizardFrameControl1;
		private ComponentArt.Win.UI.Internal.TabPage m_palettesTabPage;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private ComponentArt.Win.UI.Internal.BorderPanel borderPanel1;
		private ComponentArt.Win.UI.Internal.Button m_backgroundImageUrlButton;
		private System.Windows.Forms.TextBox m_backgroundImageUrlTextBox;
		[WizardHint(ChartBase.MainAssemblyTypeName, 
#if __BuildingWebChart__
			"BackgroundImageUrl"
#else
#if __BUILDING_CRI_DESIGNER__
            "BackgroundImageForSqlChart"
#else
			"BackgroundImage"
#endif
#endif
			 )]
		private System.Windows.Forms.Panel m_backgroundImageUrlPanel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardAppearanceDialog()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_gradientListBox.Items.AddRange(Enum.GetNames(typeof(ComponentArt.Web.Visualization.Charting.GradientKind)));
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
			this.tabControl1 = new ComponentArt.Win.UI.Internal.TabControl();
			this.m_palettesTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.wizardPaletteDialog = new ComponentArt.Web.Visualization.Charting.Design.WizardPaletteDialog();
			this.m_backgroundTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.m_backgroundImageUrlPanel = new System.Windows.Forms.Panel();
			this.borderPanel1 = new ComponentArt.Win.UI.Internal.BorderPanel();
			this.m_backgroundImageUrlTextBox = new System.Windows.Forms.TextBox();
			this.m_backgroundImageUrlButton = new ComponentArt.Win.UI.Internal.Button();
			this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
			this.m_backGroungGradientGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_gradientListBox = new ComponentArt.Win.UI.Internal.ListBox();
			this.m_backGroungImageGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.m_removeButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_openFileButton = new ComponentArt.Win.UI.Internal.Button();
			this.m_frameTabPage = new ComponentArt.Win.UI.Internal.TabPage();
			this.wizardFrameControl1 = new ComponentArt.Web.Visualization.Charting.Design.WizardFrameControl();
			this.tabControl1.SuspendLayout();
			this.m_palettesTabPage.SuspendLayout();
			this.m_backgroundTabPage.SuspendLayout();
			this.m_backgroundImageUrlPanel.SuspendLayout();
			this.borderPanel1.SuspendLayout();
			this.m_backGroungGradientGroupBox.SuspendLayout();
			this.m_backGroungImageGroupBox.SuspendLayout();
			this.m_frameTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.m_palettesTabPage,
																					  this.m_backgroundTabPage,
																					  this.m_frameTabPage});
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Size = new System.Drawing.Size(324, 312);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// m_palettesTabPage
			// 
			this.m_palettesTabPage.BackColor = System.Drawing.Color.White;
			this.m_palettesTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.wizardPaletteDialog});
			this.m_palettesTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_palettesTabPage.Name = "m_palettesTabPage";
			this.m_palettesTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_palettesTabPage.TabIndex = 4;
			this.m_palettesTabPage.Text = "Palettes";
			// 
			// wizardPaletteDialog
			// 
			this.wizardPaletteDialog.BackColor = System.Drawing.Color.White;
			this.wizardPaletteDialog.Location = new System.Drawing.Point(8, 8);
			this.wizardPaletteDialog.Name = "wizardPaletteDialog";
			this.wizardPaletteDialog.Size = new System.Drawing.Size(408, 256);
			this.wizardPaletteDialog.TabIndex = 0;
			// 
			// m_backgroundTabPage
			// 
			this.m_backgroundTabPage.BackColor = System.Drawing.Color.White;
			this.m_backgroundTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.m_backgroundImageUrlPanel,
																							  this.separator1,
																							  this.m_backGroungGradientGroupBox,
																							  this.m_backGroungImageGroupBox,
																							  this.m_removeButton,
																							  this.m_openFileButton});
			this.m_backgroundTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_backgroundTabPage.Name = "m_backgroundTabPage";
			this.m_backgroundTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_backgroundTabPage.TabIndex = 5;
			this.m_backgroundTabPage.Text = "Background";
			// 
			// m_backgroundImageUrlPanel
			// 
			this.m_backgroundImageUrlPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.borderPanel1,
																									this.m_backgroundImageUrlButton});
			this.m_backgroundImageUrlPanel.Location = new System.Drawing.Point(152, 120);
			this.m_backgroundImageUrlPanel.Name = "m_backgroundImageUrlPanel";
			this.m_backgroundImageUrlPanel.Size = new System.Drawing.Size(160, 48);
			this.m_backgroundImageUrlPanel.TabIndex = 10;
			// 
			// borderPanel1
			// 
			this.borderPanel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.m_backgroundImageUrlTextBox});
			this.borderPanel1.Location = new System.Drawing.Point(8, 14);
			this.borderPanel1.Name = "borderPanel1";
			this.borderPanel1.Size = new System.Drawing.Size(124, 20);
			this.borderPanel1.TabIndex = 40;
			// 
			// m_backgroundImageUrlTextBox
			// 
			this.m_backgroundImageUrlTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_backgroundImageUrlTextBox.Location = new System.Drawing.Point(3, 3);
			this.m_backgroundImageUrlTextBox.Name = "m_backgroundImageUrlTextBox";
			this.m_backgroundImageUrlTextBox.Size = new System.Drawing.Size(117, 13);
			this.m_backgroundImageUrlTextBox.TabIndex = 37;
			this.m_backgroundImageUrlTextBox.Text = "";
			this.m_backgroundImageUrlTextBox.TextChanged += new System.EventHandler(this.m_backgroundImageUrlTextBox_TextChanged);
			// 
			// m_backgroundImageUrlButton
			// 
			this.m_backgroundImageUrlButton.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_backgroundImageUrlButton.Location = new System.Drawing.Point(136, 17);
			this.m_backgroundImageUrlButton.Name = "m_backgroundImageUrlButton";
			this.m_backgroundImageUrlButton.Size = new System.Drawing.Size(20, 14);
			this.m_backgroundImageUrlButton.TabIndex = 50;
			this.m_backgroundImageUrlButton.Text = "...";
			this.m_backgroundImageUrlButton.TextLocation = new System.Drawing.Point(5, 2);
			this.m_backgroundImageUrlButton.Click += new System.EventHandler(this.m_backgroundImageUrlButton_Click);
			// 
			// separator1
			// 
			this.separator1.Location = new System.Drawing.Point(160, 128);
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(152, 3);
			this.separator1.TabIndex = 48;
			this.separator1.TabStop = false;
			// 
			// m_backGroungGradientGroupBox
			// 
			this.m_backGroungGradientGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									   this.m_gradientListBox});
			this.m_backGroungGradientGroupBox.DrawBorderAroundControl = true;
			this.m_backGroungGradientGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_backGroungGradientGroupBox.Name = "m_backGroungGradientGroupBox";
			this.m_backGroungGradientGroupBox.ResizeChildren = true;
			this.m_backGroungGradientGroupBox.Size = new System.Drawing.Size(136, 248);
			this.m_backGroungGradientGroupBox.TabIndex = 0;
			this.m_backGroungGradientGroupBox.TabStop = false;
			this.m_backGroungGradientGroupBox.Text = "Background Gradient";
			// 
			// m_gradientListBox
			// 
			this.m_gradientListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gradientListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_gradientListBox.ItemHeight = 18;
			this.m_gradientListBox.Location = new System.Drawing.Point(2, 26);
			this.m_gradientListBox.Name = "m_gradientListBox";
			this.m_gradientListBox.Size = new System.Drawing.Size(132, 216);
			this.m_gradientListBox.TabIndex = 0;
			this.m_gradientListBox.SelectedIndexChanged += new System.EventHandler(this.m_gradientListBox_SelectedIndexChanged);
			// 
			// m_backGroungImageGroupBox
			// 
			this.m_backGroungImageGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																									this.panel1});
			this.m_backGroungImageGroupBox.DrawBorderAroundControl = true;
			this.m_backGroungImageGroupBox.Location = new System.Drawing.Point(160, 8);
			this.m_backGroungImageGroupBox.Name = "m_backGroungImageGroupBox";
			this.m_backGroungImageGroupBox.ResizeChildren = true;
			this.m_backGroungImageGroupBox.Size = new System.Drawing.Size(152, 112);
			this.m_backGroungImageGroupBox.TabIndex = 32;
			this.m_backGroungImageGroupBox.TabStop = false;
			this.m_backGroungImageGroupBox.Text = "Background Image";
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(2, 26);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(148, 84);
			this.panel1.TabIndex = 2;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// m_removeButton
			// 
			this.m_removeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_removeButton.Location = new System.Drawing.Point(240, 136);
			this.m_removeButton.Name = "m_removeButton";
			this.m_removeButton.Size = new System.Drawing.Size(72, 23);
			this.m_removeButton.TabIndex = 3;
			this.m_removeButton.Text = "Remove";
			this.m_removeButton.Click += new System.EventHandler(this.m_removeButton_Click);
			// 
			// m_openFileButton
			// 
			this.m_openFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_openFileButton.Location = new System.Drawing.Point(160, 136);
			this.m_openFileButton.Name = "m_openFileButton";
			this.m_openFileButton.Size = new System.Drawing.Size(72, 23);
			this.m_openFileButton.TabIndex = 1;
			this.m_openFileButton.Text = "Browse...";
			this.m_openFileButton.Click += new System.EventHandler(this.m_openFileButton_Click);
			// 
			// m_frameTabPage
			// 
			this.m_frameTabPage.BackColor = System.Drawing.Color.White;
			this.m_frameTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.wizardFrameControl1});
			this.m_frameTabPage.Location = new System.Drawing.Point(2, 25);
			this.m_frameTabPage.Name = "m_frameTabPage";
			this.m_frameTabPage.Size = new System.Drawing.Size(320, 285);
			this.m_frameTabPage.TabIndex = 6;
			this.m_frameTabPage.Text = "Frame";
			// 
			// wizardFrameControl1
			// 
			this.wizardFrameControl1.BackColor = System.Drawing.Color.White;
			this.wizardFrameControl1.Location = new System.Drawing.Point(8, 8);
			this.wizardFrameControl1.Name = "wizardFrameControl1";
			this.wizardFrameControl1.Size = new System.Drawing.Size(304, 280);
			this.wizardFrameControl1.TabIndex = 0;
			// 
			// WizardAppearanceDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.DefaultHint = "Choose from the list of available palettes, edit or define new palettes. Palettes" +
				" can be saved to and loaded from XML files.";
			this.DefaultHintTitle = "Appearance â€“ Palettes";
			this.Name = "WizardAppearanceDialog";
			this.Size = new System.Drawing.Size(324, 312);
			this.Load += new System.EventHandler(this.WizardAppearanceDialog_Load);
			this.tabControl1.ResumeLayout(false);
			this.m_palettesTabPage.ResumeLayout(false);
			this.m_backgroundTabPage.ResumeLayout(false);
			this.m_backgroundImageUrlPanel.ResumeLayout(false);
			this.borderPanel1.ResumeLayout(false);
			this.m_backGroungGradientGroupBox.ResumeLayout(false);
			this.m_backGroungImageGroupBox.ResumeLayout(false);
			this.m_frameTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		protected override void OnCreateControl() 
		{
			base.OnCreateControl();
			if (WinChart == null)
				return;

			m_gradientListBox.SelectedItem = WinChart.BackGradientKind.ToString();

            m_backgroundImageUrlPanel.Visible = 
#if __BuildingWebChart__
                true
#else
			    false
#endif
            ;

			m_backgroundImageUrlTextBox.Text = Wizard.BackgroundImageURL;
		}

		private string getFileName()
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.ICO)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.ICO|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true ;

			if(openFileDialog1.ShowDialog() == DialogResult.OK)
				return openFileDialog1.FileName;
			else
				return "";
		}

		private void m_openFileButton_Click(object sender, System.EventArgs e)
		{
			string backgroundFileName = getFileName();
			if (backgroundFileName=="")
				return;
			WinChart.BackgroundImage = Bitmap.FromFile(backgroundFileName);
			WinChart.Invalidate();
			panel1.Invalidate();
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (WinChart.Chart.Background != null) 
			{
				e.Graphics.DrawImage(WinChart.Chart.Background, new Rectangle(new Point(0, 0), panel1.Size));
			}
		}

		private void m_removeButton_Click(object sender, System.EventArgs e)
		{
			WinChart.BackgroundImage = null;
			WinChart.Invalidate();
			panel1.Invalidate();
		}

		private void WizardAppearanceDialog_Load(object sender, System.EventArgs e)
		{
		}

		private void m_gradientListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WinChart.BackGradientKind = (GradientKind)Enum.Parse(typeof(GradientKind), m_gradientListBox.SelectedItem.ToString());
			WinChart.Invalidate();
			panel1.Invalidate();
		}

		private void m_colorButton_Click(object sender, System.EventArgs e)
		{
			InvokeEditor(WinChart, typeof(Control).GetProperty("BackColor"));
			WinChart.Invalidate();
		}

		private void m_startingColorPropertyControl_ValueChanged(object sender, System.EventArgs e)
		{
			WinChart.Invalidate();
			panel1.Invalidate();
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControl1.SelectedTab == m_palettesTabPage) 
			{
				HintTitle = DefaultHintTitle = "Appearance â€“ Palettes";
				Hint = DefaultHint = "Choose from the list of available palettes, edit or define new palettes. Palettes can be saved to and loaded from XML files.";
			} 
			else if (tabControl1.SelectedTab == m_backgroundTabPage) 
			{
				HintTitle = DefaultHintTitle = "Appearance â€“ Background";
				Hint = DefaultHint = "Set the background color, gradient type, or background image. Setting background colors will override the settings specified within the palette.";
			} 
			else 
			{
				HintTitle = DefaultHintTitle = "Appearance â€“ Frame";
				Hint = DefaultHint = "Specify the frame style, color, text, corner, shade, and spacing settings.";
			}
		}

		private void m_backgroundImageUrlButton_Click(object sender, System.EventArgs e)
		{
#if __BuildingWebChart__
			UITypeEditor editor = (UITypeEditor)Activator.CreateInstance(typeof(System.Web.UI.Design.ImageUrlEditor));
			string url = (string)editor.EditValue(new ITypeDescriptorContextImpl(this, true), new WizardDialog.IWindowsFormsEditorServiceImpl(this), m_backgroundImageUrlTextBox.Text);

			m_backgroundImageUrlTextBox.Text = url;
#endif
		}

		private void m_backgroundImageUrlTextBox_TextChanged(object sender, System.EventArgs e)
		{
			try 
			{
				if (m_backgroundImageUrlTextBox.Text == string.Empty)
					WinChart.BackgroundImage = null;
				else 
				{
					System.Drawing.Image img = System.Drawing.Image.FromFile(m_backgroundImageUrlTextBox.Text);
					WinChart.BackgroundImage = (Bitmap)img;
				}

				Wizard.BackgroundImageURL = m_backgroundImageUrlTextBox.Text;

				WinChart.Invalidate();
				panel1.Invalidate();
			}
			catch (System.IO.FileNotFoundException)
			{
			}
		}
	}
}