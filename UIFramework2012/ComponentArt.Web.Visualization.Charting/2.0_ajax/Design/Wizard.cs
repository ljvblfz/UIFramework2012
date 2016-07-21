using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using ComponentArt.Win.UI.Internal;
using System.Drawing.Drawing2D;
using System.ComponentModel.Design;
using ComponentArt.Web.Visualization.Charting;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	internal class Wizard : System.Windows.Forms.Form
	{
		private ChartBase m_chart;
		private ComponentArt.Win.UI.Internal.Button Finish;
		private ComponentArt.Win.UI.Internal.Button Cancel;
		private ComponentArt.Win.UI.Internal.Button Next;
		private WinChart winChart1;

		const int m_noOfSteps = 6;
		private int m_step = 1;
		private Design.WizardElement m_currentDialog;
		private System.Windows.Forms.Label m_hintLabel;
		private ComponentArt.Win.UI.Internal.Button buttonBack;
		private System.Windows.Forms.Panel m_dialogsPanel;
		private System.Windows.Forms.Panel m_previewAndHintPanel;

		ArrayList m_stepsHistory = new ArrayList();
		int m_stepInHistory = 0;
		private ComponentArt.Win.UI.Internal.Button m_applyButton;
		private System.Windows.Forms.PictureBox m_bannerPictureBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel m_navigationPanel;

		private Bitmap [,] m_navigationBitmaps;
		private Bitmap [] m_navigationSpacers;

		private Rectangle [] m_navigationButtonRectangles 
			= (Rectangle []) Array.CreateInstance(typeof(Rectangle), m_noOfSteps);

		int m_navigationHoverIndex = -1;
		int m_navigationIndex = 0;
		object [] m_links = (object [])Array.CreateInstance(typeof(object), m_noOfSteps);
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip m_toolTip;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label m_debugLabel;
		private ComponentArt.Win.UI.Internal.CheckBox m_autoLaunchCheckBox;
		private System.Windows.Forms.Panel m_currentChartPanel;
		private System.Windows.Forms.Panel m_mainPanel;
		private ComponentArt.Win.UI.Internal.Separator separator1;
		private System.Windows.Forms.Label m_hintTitleLabel;
		private System.Windows.Forms.Panel panel2;
		private ComponentArt.Win.UI.Internal.GroupBox m_currentChartGroupBox;
		private ComponentArt.Win.UI.Internal.Button xbutton;

		static DebugTimer m_debugTimer = new DebugTimer();

		internal static DebugTimer DT 
		{
			get {return m_debugTimer;}
		}

        public bool AutoLaunchCheckBoxVisible
        {
            get { return m_autoLaunchCheckBox.Visible; }
            set { m_autoLaunchCheckBox.Visible = value; }
        }
            
		bool m_coordSysEnabled = true;

		internal bool CoordSysEnabled
		{
			get
			{
				return m_coordSysEnabled;
			}
			set
			{
				m_coordSysEnabled = value;
				m_navigationPanel.Invalidate();
			}
		}

		internal Wizard(WinChart wc) 
		{
			InitializeComponent();
            m_chart = wc.Chart;

			SetupWinChart(wc);
		}

		private void SetupWinChart (WinChart wc) 
		{
			this.Size = this.ClientSize;
			this.Icon = new Icon(CommonFunctions.GetManifestResourceStream(ChartBase.ProductName.ToLower() + "-wizard.ico"));
            this.m_bannerPictureBox.Image = CommonFunctions.GetResourceBitmap("wizard-banner-chart.gif");

			this.winChart1 = wc;

			this.winChart1.Location = new System.Drawing.Point(2, 0);
			this.winChart1.Name = "Winchart";

			int minSize = m_currentChartPanel.Size.Height < m_currentChartPanel.Size.Width ?
				m_currentChartPanel.Size.Height : m_currentChartPanel.Size.Width;

			this.winChart1.Size = 
				new Size(m_currentChartPanel.Size.Width - 4, m_currentChartPanel.Size.Height - 2);

			this.winChart1.TrackballEnabled = true;

			// Set the title
			this.Text = "ComponentArt Web.Visualization.Charting Wizard";

			m_currentChartPanel.Controls.Add(this.winChart1);

			m_links[0] = typeof(WizardSeriesStyleDialog);
			m_links[1] = typeof(WizardPresentationDialog);
			m_links[2] = typeof(WizardMappingDialog);
			m_links[3] = typeof(WizardAppearanceDialog);
			m_links[4] = typeof(WizardCoordSystemDialog);
			m_links[5] = typeof(WizardTitlesDialog);

			m_stepsHistory.Add(m_step);

			// Get Navigation Buttons
			m_navigationBitmaps = new Bitmap [m_noOfSteps,4];
			
			string [] stepNames = new string [] {
													"chartType", 
													"series", 
													"3DOptions", 
													"appearance", 
													"coordSystem", 
													"other"
												};

			for (int i=0; i<m_noOfSteps; ++i) 
			{
				getButton(stepNames[i], i);
			}
            
			// Set button places
			int current_x = 0;
			for (int i=0; i<m_noOfSteps; ++i) 
			{
				m_navigationButtonRectangles[i] = new Rectangle(current_x, 0, 
					m_navigationBitmaps[i,0].Width, m_navigationBitmaps[i,0].Height);

				current_x = current_x + m_navigationBitmaps[i,0].Width + 1;
			}
            
			int spaceLeft = m_navigationPanel.Width - 
				(m_navigationButtonRectangles[m_noOfSteps-1].X + m_navigationButtonRectangles[m_noOfSteps-1].Width);
            
			int startButtons = spaceLeft/2;
			for (int i=0; i<m_noOfSteps; ++i) 
			{
				Rectangle r = m_navigationButtonRectangles[i];
				m_navigationButtonRectangles[i] = new Rectangle(r.X +startButtons, r.Y, r.Width, r.Height);
			}

			m_navigationSpacers = new Bitmap [] {
													new Bitmap(startButtons-1, m_navigationBitmaps[0,0].Height),
													new Bitmap(m_navigationPanel.Width - 
													(m_navigationButtonRectangles[m_noOfSteps-1].X + m_navigationButtonRectangles[m_noOfSteps-1].Width)-1, m_navigationBitmaps[0,0].Height)
												};

			Bitmap baseSpacer =
				(Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream("NavigationSpacer.gif"));

			Graphics g;
			g = Graphics.FromImage(m_navigationSpacers[0]);
			g.DrawImage(baseSpacer, new Rectangle(0, 0, m_navigationSpacers[0].Width, m_navigationSpacers[0].Height), new Rectangle(new Point(0,0), baseSpacer.Size), GraphicsUnit.Pixel);
			for (int i=0; i<m_navigationSpacers[0].Width; ++i)
				g.DrawImageUnscaled(baseSpacer, i, 0);
			g.Dispose();

			g = Graphics.FromImage(m_navigationSpacers[1]);
			for (int i=0; i<m_navigationSpacers[1].Width; ++i)
				g.DrawImageUnscaled(baseSpacer, i, 0);
			g.Dispose();


			if (m_showFocused) 
			{
				for (int i=1; i<=m_noOfSteps; ++i )
				{
					m_step = i;
					SetState();
				}

				m_step = 1;
			}

			BuildFrame();

			SetupCurrentChartHint();
		}

		void SetupCurrentChartHint() 
		{
			m_currentChartGroupBox.MouseEnter += new EventHandler(CurrentChartGotFocusHandler);
			m_currentChartGroupBox.MouseLeave += new EventHandler(CurrentChartLostFocusHandler);
			WinChart.MouseEnter += new EventHandler(CurrentChartGotFocusHandler);
			WinChart.MouseLeave += new EventHandler(CurrentChartLostFocusHandler);
			m_currentChartPanel.MouseEnter += new EventHandler(CurrentChartGotFocusHandler);
			m_currentChartPanel.MouseLeave += new EventHandler(CurrentChartLostFocusHandler);
		}

		private void CurrentChartGotFocusHandler(object sender, EventArgs e)
		{
            System.Resources.ResourceManager rm = CommonFunctions.GetComonResourceManager();
			
			Hint = rm.GetString("WizardCurrentChart", System.Threading.Thread.CurrentThread.CurrentCulture);
			HintTitle = m_currentChartGroupBox.Text;

			rm.ReleaseAllResources();
		}

		private void CurrentChartLostFocusHandler(object sender, EventArgs e)
		{
			WizardElement we = m_links[m_step - 1] as WizardElement;

            if (we != null)
            {
                Hint = we.DefaultHint;
                HintTitle = we.DefaultHintTitle;
            }
		}

		void BuildFrame () 
		{

			ArrayList al = new ArrayList();
			foreach (string s in new string [] {"Top", "Left", "Right", "Bottom"}) 
			{
				PictureBox pb = new PictureBox();

				pb.Image = CommonFunctions.GetResourceBitmap("Frame" + s + ".png"); //((System.Drawing.Bitmap)(resources.GetObject("m_bannerPictureBox.Image")));

				pb.Name = "m_bannerPictureBox";
				pb.Size = pb.Image.Size;
				pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
				pb.TabIndex = 24;
				pb.TabStop = false;
				pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_bannerPictureBox_MouseUp);
				pb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_bannerPictureBox_MouseMove);
				pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_bannerPictureBox_MouseDown);

				al.Add(pb);
			}

			Graphics g = Graphics.FromImage(((PictureBox)al[0]).Image);
            string productName = ComponentArt.Web.Visualization.Charting.ChartBase.ProductName;
            if (productName.EndsWith("Designer"))
                productName = productName.Substring(0, productName.Length - 8); // Remove "Designer"
			g.DrawString("ComponentArt " + productName + " Wizard", new Font("Verdana", 12, FontStyle.Bold, GraphicsUnit.Pixel), new SolidBrush(Color.FromArgb(102,102,102)), 28, 6);
			g.Dispose();

			((PictureBox)al[1]).Location = new Point(0, ((PictureBox)al[0]).Height);
			((PictureBox)al[2]).Location = new Point(((PictureBox)al[0]).Width - ((PictureBox)al[2]).Width, ((PictureBox)al[0]).Height);
			((PictureBox)al[3]).Location = new Point(0, ((PictureBox)al[1]).Top + ((PictureBox)al[1]).Height) ;

			this.Controls.AddRange(new Control [] {((PictureBox)al[0]), ((PictureBox)al[1]), ((PictureBox)al[2]), ((PictureBox)al[3])});
			
			this.Size = new Size(
				((PictureBox)al[3]).Left + ((PictureBox)al[3]).Width,
				((PictureBox)al[3]).Top + ((PictureBox)al[3]).Height
			);

			m_mainPanel.Location = new Point(((PictureBox)al[2]).Width, ((PictureBox)al[0]).Height);

			xbutton.Location = new System.Drawing.Point(587, 6);
        }

		const bool m_showFocused = false;

		internal Wizard(ChartBase chart)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_chart = chart;

			WinChart wc = new WinChart();
			wc.Chart = m_chart;
			
			SetupWinChart(wc);
		}
		
		/// <summary>
		/// Background color of the chart
		/// </summary>
		public Color ChartBackColor { get { return WinChart.BackColor; } set { WinChart.BackColor = value; } }
		
		/// <summary>
		/// Foreground color of the chart
		/// </summary>
		public Color ChartForeColor { get { return WinChart.ForeColor; } set { WinChart.ForeColor = value; } }

		/// <summary>
		/// Chart text property
		/// </summary>
		public string ChartText { get { return (string)(WinChart.Text.Clone()); } set { WinChart.Text = value; } }

		void getButton(string filename, int index) 
		{
			m_navigationBitmaps[index,0] =
				(Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream(filename + "-up.gif"));
			m_navigationBitmaps[index,1] =
				(Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream(filename + "-selected.gif"));
			m_navigationBitmaps[index,2] =
				(Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream(filename + "-hover.gif"));

            try
            {
                m_navigationBitmaps[index, 3] =
                    (Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream(filename + "-disabled.gif"));
            }
            catch 
            { 
                // Leave the bitmap null; it won't be used because this button is never disabled
                m_navigationBitmaps[index, 3] = null;
            }
		}

		internal ToolTip ToolTip 
		{
			get {return m_toolTip;}
		}

		public static Control GetParentControlOfType(Control c, System.Type t) 
		{
			// Find the winchart
			Control ctrl = c;

			while (ctrl.Parent != null && ctrl.Parent != ctrl) 
			{
				ctrl = ctrl.Parent;
				if (ctrl.GetType() == t || ctrl.GetType().IsSubclassOf(t)) 
				{
					return ctrl;
				}
			}

			return null;
		}

		/// <summary>
		/// Rotates through the parents in order to find a parent Wizard control
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		internal static WinChart GetWinChart(Control c) 
		{
			object wizard = GetParentControlOfType(c, typeof(Wizard));
			if (wizard != null)
				return ((Wizard)wizard).WinChart;

			return null;
		}

		/// <summary>
		/// Exposes the finish button
		/// </summary>
		public System.Windows.Forms.Button FinishButton 
		{
			get 
			{
				return this.Finish;
			}
		}
		
		/// <summary>
		/// Exposes the Apply button
		/// </summary>
		public System.Windows.Forms.Button ApplyButton 
		{
			get 
			{
				return this.m_applyButton;
			}
		}
		

		public string Hint 
		{
			get {return m_hintLabel.Text;}
			set {m_hintLabel.Text = value;}
		}

		public string HintTitle
		{
			get {return m_hintTitleLabel.Text;}
			set {m_hintTitleLabel.Text = value;}
		}
		

		/// <summary>
		/// Performs functions as a result of change state
		/// </summary>
		private void SetState() 
		{
			this.buttonBack.Enabled = m_step != 1;
			this.Next.Enabled = m_step != m_links.Length;
			if (m_navigationIndex != m_step-1) 
			{
				m_navigationIndex = m_step-1;
				m_navigationPanel.Invalidate();
			}

			// Set the title
			// extract the link data
			object link = m_links[m_step - 1];

			// if link is a type, construct the object from type and set the link to point to the new object
			if (link is Type) 
			{
				WizardElement newwe = (WizardElement)((Type)link).GetConstructor(new Type [0]).Invoke(new object [0]);
                m_links[m_step - 1] = newwe;
                newwe.Location = new Point(4, 8);
				newwe.TabIndex = 10;
				m_dialogsPanel.Controls.Add(newwe);
			}
			
			bool focusInDialog = false;

			if (m_currentDialog == ActiveControl) 
			{
				focusInDialog = true;
			}

			// set current dialog this here
			m_currentDialog = (WizardElement)(m_links[m_step - 1]);

			// Show and hide the dialogs as appropriate
			foreach (object ll in m_links) 
			{
				WizardElement we = ll as WizardElement;

				if (we != null) 
				{
					if (we == m_currentDialog) 
					{
						// View/Hide winchart on the right
						if (ll is WizardSeriesStyleDialog)
							m_previewAndHintPanel.Hide();
						else
							m_previewAndHintPanel.Show();		

						we.Show();
					}
					else we.Hide();
				}
			}

			if (focusInDialog)
				ActiveControl = m_currentDialog;
		}

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

		protected override void OnResize(EventArgs e) 
		{
			base.OnResize(e);
			if (Width == 0 || Height == 0)
				return;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			this.winChart1.Chart.SetDesignMode(true);

			if (m_showFocused) 
			{
				Form f = new Form();
				f.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
				f.Size = new Size(100, 50);
				this.Controls.Remove(m_debugLabel);
				f.Controls.Add(m_debugLabel);
				m_debugLabel.Location = new Point(0,0);
				RegisterFocus(this);
				f.Show();
			}
			else
				m_debugLabel.Visible = false;

			m_autoLaunchCheckBox.Checked = RegistryValues.AutoLaunchWizard;
		}


		void RegisterFocus(Control c) 
		{
			foreach (Control child in c.Controls) 
			{
				child.GotFocus += new EventHandler(GotFocusHandler);
				child.LostFocus += new EventHandler(LostFocusHandler);
				RegisterFocus(child);
			}
		}

		private void GotFocusHandler(object sender, EventArgs e)
		{
			m_debugLabel.Text = ((Control)sender).Name;
		}


		private void LostFocusHandler(object sender, EventArgs e)
		{
		}

        bool setNow = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (setNow)
            {
                Application.DoEvents();
                SetState();
                setNow = false;
            }
            if (m_links[m_step - 1] is Type)
            {
                setNow = true;
                Invalidate();
            }
        }

		public ChartBase Chart 
		{
			get 
			{
				return m_chart;
			}
		}


		internal WinChart WinChart 
		{
			get 
			{
				return winChart1;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Wizard));
            this.Finish = new ComponentArt.Win.UI.Internal.Button();
            this.Cancel = new ComponentArt.Win.UI.Internal.Button();
            this.Next = new ComponentArt.Win.UI.Internal.Button();
            this.m_hintLabel = new System.Windows.Forms.Label();
            this.buttonBack = new ComponentArt.Win.UI.Internal.Button();
            this.m_dialogsPanel = new System.Windows.Forms.Panel();
            this.m_previewAndHintPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_hintTitleLabel = new System.Windows.Forms.Label();
            this.m_currentChartGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
            this.m_currentChartPanel = new System.Windows.Forms.Panel();
            this.m_applyButton = new ComponentArt.Win.UI.Internal.Button();
            this.m_bannerPictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_navigationPanel = new System.Windows.Forms.Panel();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_debugLabel = new System.Windows.Forms.Label();
            this.m_autoLaunchCheckBox = new ComponentArt.Win.UI.Internal.CheckBox();
            this.m_mainPanel = new System.Windows.Forms.Panel();
            this.separator1 = new ComponentArt.Win.UI.Internal.Separator();
            this.xbutton = new ComponentArt.Win.UI.Internal.Button();
            this.m_dialogsPanel.SuspendLayout();
            this.m_previewAndHintPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.m_currentChartGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Finish
            // 
            this.Finish.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Finish.HoverIcon = ((System.Drawing.Bitmap)(resources.GetObject("Finish.HoverIcon")));
            this.Finish.IconDistance = 2;
            this.Finish.IconLocation = new System.Drawing.Point(5, 7);
            this.Finish.Location = new System.Drawing.Point(288, 0);
            this.Finish.Name = "Finish";
            this.Finish.NormalIcon = ((System.Drawing.Bitmap)(resources.GetObject("Finish.NormalIcon")));
            this.Finish.Size = new System.Drawing.Size(65, 21);
            this.Finish.TabIndex = 50;
            this.Finish.Text = "Finish";
            this.Finish.Click += new System.EventHandler(this.Finish_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Cancel.HoverIcon = ((System.Drawing.Bitmap)(resources.GetObject("Cancel.HoverIcon")));
            this.Cancel.IconLocation = new System.Drawing.Point(5, 7);
            this.Cancel.Location = new System.Drawing.Point(216, 0);
            this.Cancel.Name = "Cancel";
            this.Cancel.NormalIcon = ((System.Drawing.Bitmap)(resources.GetObject("Cancel.NormalIcon")));
            this.Cancel.Size = new System.Drawing.Size(65, 21);
            this.Cancel.TabIndex = 40;
            this.Cancel.Text = "Cancel";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Next
            // 
            this.Next.DisabledIcon = ((System.Drawing.Bitmap)(resources.GetObject("Next.DisabledIcon")));
            this.Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Next.HoverIcon = ((System.Drawing.Bitmap)(resources.GetObject("Next.HoverIcon")));
            this.Next.IconDistance = 2;
            this.Next.IconLocation = new System.Drawing.Point(5, 7);
            this.Next.Location = new System.Drawing.Point(72, 0);
            this.Next.Name = "Next";
            this.Next.NormalIcon = ((System.Drawing.Bitmap)(resources.GetObject("Next.NormalIcon")));
            this.Next.Size = new System.Drawing.Size(65, 21);
            this.Next.TabIndex = 20;
            this.Next.Text = "Next";
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // m_hintLabel
            // 
            this.m_hintLabel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.m_hintLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(119)), ((System.Byte)(119)), ((System.Byte)(119)));
            this.m_hintLabel.Location = new System.Drawing.Point(6, 19);
            this.m_hintLabel.Name = "m_hintLabel";
            this.m_hintLabel.Size = new System.Drawing.Size(235, 61);
            this.m_hintLabel.TabIndex = 11;
            this.m_hintLabel.Text = "Hint itself goes here... yes.....";
            // 
            // buttonBack
            // 
            this.buttonBack.DisabledIcon = ((System.Drawing.Bitmap)(resources.GetObject("buttonBack.DisabledIcon")));
            this.buttonBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonBack.HoverIcon = ((System.Drawing.Bitmap)(resources.GetObject("buttonBack.HoverIcon")));
            this.buttonBack.IconDistance = 2;
            this.buttonBack.IconLocation = new System.Drawing.Point(5, 7);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.NormalIcon = ((System.Drawing.Bitmap)(resources.GetObject("buttonBack.NormalIcon")));
            this.buttonBack.Size = new System.Drawing.Size(65, 21);
            this.buttonBack.TabIndex = 10;
            this.buttonBack.Text = "Back";
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // m_dialogsPanel
            // 
            this.m_dialogsPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
            this.m_dialogsPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.m_previewAndHintPanel});
            this.m_dialogsPanel.Location = new System.Drawing.Point(4, 84);
            this.m_dialogsPanel.Name = "m_dialogsPanel";
            this.m_dialogsPanel.Size = new System.Drawing.Size(592, 326);
            this.m_dialogsPanel.TabIndex = 22;
            // 
            // m_previewAndHintPanel
            // 
            this.m_previewAndHintPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(238)), ((System.Byte)(238)), ((System.Byte)(238)));
            this.m_previewAndHintPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																								this.groupBox1,
																								this.m_currentChartGroupBox});
            this.m_previewAndHintPanel.Location = new System.Drawing.Point(340, 8);
            this.m_previewAndHintPanel.Name = "m_previewAndHintPanel";
            this.m_previewAndHintPanel.Size = new System.Drawing.Size(248, 312);
            this.m_previewAndHintPanel.TabIndex = 22;
            this.m_previewAndHintPanel.Visible = false;
            this.m_previewAndHintPanel.VisibleChanged += new System.EventHandler(this.m_previewAndHintPanel_VisibleChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.panel2});
            this.groupBox1.Location = new System.Drawing.Point(0, 224);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 88);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.m_hintTitleLabel,
																				 this.m_hintLabel});
            this.panel2.Location = new System.Drawing.Point(1, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(245, 80);
            this.panel2.TabIndex = 0;
            // 
            // m_hintTitleLabel
            // 
            this.m_hintTitleLabel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.m_hintTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(119)), ((System.Byte)(119)), ((System.Byte)(119)));
            this.m_hintTitleLabel.Location = new System.Drawing.Point(6, 5);
            this.m_hintTitleLabel.Name = "m_hintTitleLabel";
            this.m_hintTitleLabel.Size = new System.Drawing.Size(230, 14);
            this.m_hintTitleLabel.TabIndex = 12;
            this.m_hintTitleLabel.Text = "Hint Title";
            // 
            // m_currentChartGroupBox
            // 
            this.m_currentChartGroupBox.BackColor = System.Drawing.Color.White;
            this.m_currentChartGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																								 this.m_currentChartPanel});
            this.m_currentChartGroupBox.Name = "m_currentChartGroupBox";
            this.m_currentChartGroupBox.ResizeChildren = true;
            this.m_currentChartGroupBox.SimpleGroupBox = false;
            this.m_currentChartGroupBox.Size = new System.Drawing.Size(248, 224);
            this.m_currentChartGroupBox.TabIndex = 23;
            this.m_currentChartGroupBox.TabStop = false;
            this.m_currentChartGroupBox.Text = "Current Chart";
            // 
            // m_currentChartPanel
            // 
            this.m_currentChartPanel.Location = new System.Drawing.Point(2, 26);
            this.m_currentChartPanel.Name = "m_currentChartPanel";
            this.m_currentChartPanel.Size = new System.Drawing.Size(244, 196);
            this.m_currentChartPanel.TabIndex = 0;
            // 
            // m_applyButton
            // 
            this.m_applyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.m_applyButton.HoverIcon = ((System.Drawing.Bitmap)(resources.GetObject("m_applyButton.HoverIcon")));
            this.m_applyButton.IconDistance = 1;
            this.m_applyButton.IconLocation = new System.Drawing.Point(7, 7);
            this.m_applyButton.Location = new System.Drawing.Point(144, 0);
            this.m_applyButton.Name = "m_applyButton";
            this.m_applyButton.NormalIcon = ((System.Drawing.Bitmap)(resources.GetObject("m_applyButton.NormalIcon")));
            this.m_applyButton.Size = new System.Drawing.Size(65, 21);
            this.m_applyButton.TabIndex = 30;
            this.m_applyButton.Text = "Apply";
            this.m_applyButton.Click += new System.EventHandler(this.m_applyButton_Click);
            // 
            // m_bannerPictureBox
            // 
            this.m_bannerPictureBox.Name = "m_bannerPictureBox";
            this.m_bannerPictureBox.Size = new System.Drawing.Size(600, 63);
            this.m_bannerPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_bannerPictureBox.TabIndex = 24;
            this.m_bannerPictureBox.TabStop = false;
            this.m_bannerPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_bannerPictureBox_MouseUp);
            this.m_bannerPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_bannerPictureBox_MouseMove);
            this.m_bannerPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_bannerPictureBox_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.Cancel,
																				 this.Finish,
																				 this.Next,
																				 this.m_applyButton,
																				 this.buttonBack});
            this.panel1.Location = new System.Drawing.Point(240, 421);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 21);
            this.panel1.TabIndex = 1000;
            // 
            // m_navigationPanel
            // 
            this.m_navigationPanel.BackColor = System.Drawing.Color.White;
            this.m_navigationPanel.Location = new System.Drawing.Point(1, 64);
            this.m_navigationPanel.Name = "m_navigationPanel";
            this.m_navigationPanel.Size = new System.Drawing.Size(598, 20);
            this.m_navigationPanel.TabIndex = 25;
            this.m_navigationPanel.Click += new System.EventHandler(this.m_navigationPanel_Click);
            this.m_navigationPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.m_navigationPanel_Paint);
            this.m_navigationPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_navigationPanel_MouseMove);
            this.m_navigationPanel.MouseLeave += new System.EventHandler(this.m_navigationPanel_MouseLeave);
            // 
            // m_toolTip
            // 
            this.m_toolTip.AutomaticDelay = 0;
            this.m_toolTip.ShowAlways = true;
            // 
            // m_debugLabel
            // 
            this.m_debugLabel.Location = new System.Drawing.Point(0, 410);
            this.m_debugLabel.Name = "m_debugLabel";
            this.m_debugLabel.Size = new System.Drawing.Size(128, 40);
            this.m_debugLabel.TabIndex = 26;
            // 
            // m_autoLaunchCheckBox
            // 
            this.m_autoLaunchCheckBox.BackColor = System.Drawing.Color.White;
            this.m_autoLaunchCheckBox.Location = new System.Drawing.Point(8, 424);
            this.m_autoLaunchCheckBox.Name = "m_autoLaunchCheckBox";
            this.m_autoLaunchCheckBox.Size = new System.Drawing.Size(128, 16);
            this.m_autoLaunchCheckBox.TabIndex = 30;
            this.m_autoLaunchCheckBox.Text = "Auto-Launch Wizard";
            this.m_autoLaunchCheckBox.CheckedChanged += new System.EventHandler(this.m_autoLaunchCheckBox_CheckedChanged);
            // 
            // m_mainPanel
            // 
            this.m_mainPanel.BackColor = System.Drawing.Color.White;
            this.m_mainPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.separator1,
																					  this.m_autoLaunchCheckBox,
																					  this.m_debugLabel,
																					  this.m_navigationPanel,
																					  this.panel1,
																					  this.m_bannerPictureBox,
																					  this.m_dialogsPanel});
            this.m_mainPanel.Name = "m_mainPanel";
            this.m_mainPanel.Size = new System.Drawing.Size(600, 448);
            this.m_mainPanel.TabIndex = 1;
            this.m_mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.m_mainPanel_Paint);
            // 
            // separator1
            // 
            this.separator1.Location = new System.Drawing.Point(5, 413);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(591, 3);
            this.separator1.TabIndex = 49;
            this.separator1.TabStop = false;
            // 
            // xbutton
            // 
            this.xbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.xbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xbutton.HoverIcon = ((System.Drawing.Bitmap)(resources.GetObject("xbutton.HoverIcon")));
            this.xbutton.Location = new System.Drawing.Point(584, 0);
            this.xbutton.Name = "xbutton";
            this.xbutton.NormalIcon = ((System.Drawing.Bitmap)(resources.GetObject("xbutton.NormalIcon")));
            this.xbutton.Size = new System.Drawing.Size(16, 16);
            this.xbutton.TabIndex = 29;
            this.xbutton.TabStop = false;
            this.xbutton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Wizard
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(243)), ((System.Byte)(243)));
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(600, 448);
            this.ControlBox = false;
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.xbutton,
																		  this.m_mainPanel});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Wizard";
            this.Text = "Wizard";
            this.m_dialogsPanel.ResumeLayout(false);
            this.m_previewAndHintPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.m_currentChartGroupBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.m_mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


		private void Finish_Click(object sender, System.EventArgs e)
		{
            FinalizeGrids();
			Close();
		}

        private void m_applyButton_Click(object sender, System.EventArgs e)
		{
            FinalizeGrids();
		}

        void FinalizeGrids()
        {
#if __BUILDING_CRI_DESIGNER__
            if (m_links[m_step - 1] is WizardPresentationDialog)
            {
                ((WizardPresentationDialog)m_links[m_step - 1]).m_wizardFiltersControl.MoveToLine(0, true);
                ((WizardPresentationDialog)m_links[m_step - 1]).m_wizardSortingControl.MoveToLine(0, true);
            }
#endif
        }

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void Next_Click(object sender, System.EventArgs e)
		{
			++m_step;
			if (!CoordSysEnabled && m_step == 5)
				++m_step;

			SetState();
		}
		
		private void buttonBack_Click(object sender, System.EventArgs e)
		{
			--m_step;

			if (!CoordSysEnabled && m_step == 5)
				--m_step;

			SetState();
		}

		private void m_previewAndHintPanel_VisibleChanged(object sender, System.EventArgs e)
		{
			WinChart.Chart.SetDesignMode(true);
		}

		private void m_bannerPictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				StartDragging(e);
		}

		private void m_bannerPictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Drag(e);		
		}

		private void m_bannerPictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			StopDragging();
		}

		private void m_navigationPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			
			if (m_navigationSpacers[0].Width > 15)
				g.DrawImageUnscaled(m_navigationSpacers[0], 0, 0);

			for (int i=0; i<m_noOfSteps; ++i) 
			{
				if (i == 4 && !CoordSysEnabled)
					g.DrawImageUnscaled(m_navigationBitmaps[i,3], m_navigationButtonRectangles[i]);
				else
					g.DrawImageUnscaled(m_navigationBitmaps[i,m_navigationIndex==i?1:(m_navigationHoverIndex==i?2:0)], m_navigationButtonRectangles[i]);
			}

			if (m_navigationSpacers[0].Width > 15)
				g.DrawImageUnscaled(m_navigationSpacers[1], m_navigationPanel.Width - m_navigationSpacers[0].Width, 0);
		}

		private void m_navigationPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			for (int i=0; i<m_noOfSteps; ++i) 
			{
				if (m_navigationButtonRectangles[i].IntersectsWith(new Rectangle(e.X, e.Y, 1, 1)))
				{
					if (m_navigationHoverIndex != i) 
					{
						m_navigationHoverIndex = i;
						m_navigationPanel.Invalidate();
						break;
					}
				}
			}
		}

		private void m_navigationPanel_Click(object sender, System.EventArgs e)
		{
			if (m_navigationIndex != m_navigationHoverIndex && m_navigationHoverIndex != -1) 
			{
				m_navigationIndex = m_navigationHoverIndex;
				

				if (!CoordSysEnabled && m_navigationIndex == 4)
					return;


				m_step = m_navigationIndex + 1;
				if (m_stepsHistory.Count-1 > m_stepInHistory) 
				{
					m_stepsHistory.RemoveRange(m_stepInHistory+1, m_stepsHistory.Count-1-m_stepInHistory);
				}

				m_stepsHistory.Add(m_step);
				++m_stepInHistory;

				SetState();
				
				m_navigationPanel.Invalidate();
			}
		}

		private void m_navigationPanel_MouseLeave(object sender, System.EventArgs e)
		{
			m_navigationHoverIndex = -1;
			m_navigationPanel.Invalidate();
		}

		void DrawFocus(object sender, System.Windows.Forms.PaintEventArgs e) 
		{
			System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;
			if (!b.Focused) 
				return;
            Rectangle r = new Rectangle(3,3, b.Width - 7, b.Height - 7);
			Pen focusPen = new Pen(Color.DarkGray);
			focusPen.DashStyle = DashStyle.Dot;
			focusPen.LineJoin = LineJoin.Round;
			e.Graphics.DrawRectangle(focusPen, r);
			focusPen.Dispose();
		}

		private void m_autoLaunchCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			RegistryValues.AutoLaunchWizard = m_autoLaunchCheckBox.Checked;
		}

		private void m_mainPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Rectangle r = new Rectangle(new Point(0,0), new Size(m_mainPanel.Width-1, m_mainPanel.Height-1));
			Pen pen = new Pen(new SolidBrush(Color.FromArgb(153, 153, 153)));
			g.DrawRectangle(pen, r);
			pen.Dispose();
		}


		private string m_backgroundImageURL = "";

		internal string BackgroundImageURL 
		{
			get {return m_backgroundImageURL;}
			set {m_backgroundImageURL = value;}
        }

#if __COMPILING_FOR_2_0_AND_ABOVE__ && __BuildingWebChart__
        private string m_dataSourceId = "";
        internal string DataSourceId
        {
            get { return m_dataSourceId; }
            set { m_dataSourceId = value; }
        }
#endif


#if __BUILDING_CRI_DESIGNER__
        ExtraWizardParametersWrapper m_extraWizardParametersWrapper;
        internal ExtraWizardParametersWrapper ExtraWizardParameters
        {
            get
            {
                return m_extraWizardParametersWrapper;
            }
            set
            {
                m_extraWizardParametersWrapper = value;
            }
        }
#endif

        private IComponent m_component;

		public IComponent Component 
		{
			get {return m_component;}
			set {m_component = value;}
		}

        private ChartDesigner m_chartDesigner;

        internal ChartDesigner ChartDesigner
        {
            get
            {
                return m_chartDesigner;
            }
            set
            {
                m_chartDesigner = value;
            }
        }

	}

    internal class RegistryValues
    {
        const string defualtRegistryKey = @"Software\ComponentArt\Web.Visualization.Charting\Settings";

        private RegistryValues() { }

        static Microsoft.Win32.RegistryKey Key
        {
            get
            {
                Microsoft.Win32.RegistryKey key =
                    Microsoft.Win32.Registry.CurrentUser.CreateSubKey(defualtRegistryKey);

                return key;
            }
        }

        const string AutoLaunchWizardString =
#if __COMPILING_FOR_2_0_AND_ABOVE__
 "AutoLaunchWizardWhenSmartTasksAvailable"
#else
 "AutoLaunchWizard"
#endif
;

        static internal bool AutoLaunchWizard
        {
            get
            {
                return ((int)Key.GetValue(AutoLaunchWizardString,
#if __COMPILING_FOR_2_0_AND_ABOVE__
 0
#else
 1
#endif
) == 1 ? true : false);
            }
            set
            {
                Key.SetValue(AutoLaunchWizardString, value ? 1 : 0);
            }
        }

        static internal Point WizardLocation
        {
            get
            {
                string val = (string)Key.GetValue(@"Location");
                Point p = (Point)new PointConverter().ConvertFromInvariantString(val);
                return p;
            }
            set
            {
                Key.SetValue(@"Location", new PointConverter().ConvertToInvariantString(value));
            }
        }

        static internal Point WizardPropertyGridFormLocation
        {
            get
            {
                string val = (string)Key.GetValue(@"WizardPropertyGridFormLocation");
                Point p = (Point)new PointConverter().ConvertFromInvariantString(val);
                return p;
            }
            set
            {
                Key.SetValue(@"WizardPropertyGridFormLocation", new PointConverter().ConvertToInvariantString(value));
            }
        }

    }

	internal sealed class WizardHintAttribute : Attribute
	{

		// Keep a variable internally ...
		private string m_key = "";
        private MemberInfo m_mi = null;
		
		// The constructor is called when the attribute is set.
		public WizardHintAttribute(string key) 
		{
			m_key = key;
		}

		public WizardHintAttribute(string parentTypeName, string member) : this(Type.GetType(parentTypeName), member)
		{}

		public WizardHintAttribute(Type parentType, string member) 
		{
			MemberInfo [] mis = parentType.GetMember(member);
			if (mis == null || mis.Length<1) 
			{
				MessageBox.Show("Couldn't find the member " + member + " in the type " + parentType.Name);
				m_mi = null;
			} 
			else 
			{
				m_mi = mis[0];
			}
		}

		public string Key 
		{
			get {return m_key;}
		}

		public MemberInfo MemberInfo 
		{
			get {return m_mi;}
		}
	}
}