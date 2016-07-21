using System;
using System.Drawing;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class RunTimeCompiledSourceView : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.ComboBox languageCombo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.TextBox sourceTextBox;
		private System.Windows.Forms.ListView resultsListView;
		private System.Windows.Forms.ColumnHeader columnLine;
		private System.Windows.Forms.ColumnHeader columnDescription;

        private Timer tmr;
		private string csSource;
		private string vbSource;
		private bool isCSSource;
		private bool toHandleLanguageSelection = true;
		private System.Windows.Forms.Button checkSyntaxButton;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel lineNumberPanel;

		private CompilerResults compilerResults = null;
		public RunTimeCompiledSourceView(RunTimeCompiledSource rtcs)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			AddControls();

#if __COMPILING_FOR_2_0_AND_ABOVE__
            this.resultsListView.UseCompatibleStateImageBehavior = false;
#endif
			SetRunTimeCompiledSource(rtcs);
			SetControlsPositionAndSize();

			this.Resize += new EventHandler(RunTimeCompiledSourceView_Resize);
            tmr.Start();
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
                    tmr.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region --- Properties ---

		public RunTimeCompiledSource GetRunTimeCompiledSource()
		{
			RunTimeCompiledSource runTimeCompiledSource = new RunTimeCompiledSource();
			runTimeCompiledSource.Source = Source;
			runTimeCompiledSource.IsCSSource = IsCSSource;
			return runTimeCompiledSource; 
		}

		private void SetRunTimeCompiledSource(RunTimeCompiledSource rtcs)
		{
			Source = rtcs.Source;

			toHandleLanguageSelection = false;
			isCSSource = rtcs.IsCSSource;
			languageCombo.SelectedIndex = (isCSSource? 0:1);
			toHandleLanguageSelection = true;

            csSource = RunTimeCompiledSource.InitialCSSource;
            vbSource = RunTimeCompiledSource.InitialVBSource;
			string source = rtcs.Source;

			if(isCSSource)
			{
				if(source != null && source.Trim() != "")
					csSource = source;
				Source = csSource;
			}
			else
			{
				if(source != null && source.Trim() != "")
					vbSource = source;
				Source = vbSource;
			}
		}

		public string Source
		{
			get { return sourceTextBox.Text; }
			set { sourceTextBox.Text = value; }
		}

		public bool IsCSSource
		{
			get { return isCSSource; }
			set { isCSSource = value; }
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkSyntaxButton = new System.Windows.Forms.Button();
			this.languageCombo = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonClear = new System.Windows.Forms.Button();
			this.sourceTextBox = new System.Windows.Forms.TextBox();
			this.resultsListView = new System.Windows.Forms.ListView();
			this.columnLine = new System.Windows.Forms.ColumnHeader();
			this.columnDescription = new System.Windows.Forms.ColumnHeader();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.lineNumberPanel = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)(this.lineNumberPanel)).BeginInit();
			this.SuspendLayout();
			// 
			// checkSyntaxButton
			// 
			this.checkSyntaxButton.Location = new System.Drawing.Point(8, 8);
            this.checkSyntaxButton.Name = "checkSyntaxButton";
            this.checkSyntaxButton.Text = "Check Syntax";
            this.checkSyntaxButton.TabIndex = 0;
            this.checkSyntaxButton.Click += new EventHandler(checkSyntaxButton_Click);
			// 
			// languageCombo
			// 
			this.languageCombo.Items.AddRange(new object[] {
															   "C#",
															   "VB"});
			this.languageCombo.Location = new System.Drawing.Point(192, 11);
			this.languageCombo.Name = "languageCombo";
			this.languageCombo.Size = new System.Drawing.Size(64, 21);
			this.languageCombo.TabIndex = 1;
			this.languageCombo.Text = "C#";
			this.languageCombo.SelectedIndexChanged += new System.EventHandler(this.languageCombo_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(120, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Language:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(696, 8);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 24);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(592, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 24);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(488, 8);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(96, 24);
			this.buttonClear.TabIndex = 5;
			this.buttonClear.Text = "Clear Code";
            this.buttonClear.Click += new EventHandler(buttonClear_Click);
			// 
			// sourceTextBox
			// 
			this.sourceTextBox.AcceptsReturn = true;
			this.sourceTextBox.AcceptsTab = true;
			this.sourceTextBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.sourceTextBox.Location = new System.Drawing.Point(8, 40);
			this.sourceTextBox.Multiline = true;
			this.sourceTextBox.Name = "sourceTextBox";
			this.sourceTextBox.Size = new System.Drawing.Size(784, 432);
			this.sourceTextBox.TabIndex = 0;
			this.sourceTextBox.Text = "textBox1";
            this.sourceTextBox.TextChanged += new System.EventHandler(this.sourceTextBox_TextChanged);
            this.sourceTextBox.ScrollBars = ScrollBars.Vertical;

			// 
			// resultsListView
			// 
			this.resultsListView.BackColor = System.Drawing.SystemColors.Info;
			this.resultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this.columnLine,
																							  this.columnDescription});
			this.resultsListView.GridLines = true;
			this.resultsListView.Location = new System.Drawing.Point(8, 480);
			this.resultsListView.Name = "resultsListView";
			this.resultsListView.Size = new System.Drawing.Size(784, 96);
			this.resultsListView.TabIndex = 6;
			this.resultsListView.View = System.Windows.Forms.View.Details;
			// 
			// columnLine
			// 
			this.columnLine.Text = "Line";
			// 
			// columnDescription
			// 
			this.columnDescription.Text = "Description";
			this.columnDescription.Width = 715;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 244);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.lineNumberPanel});
			this.statusBar1.Size = new System.Drawing.Size(292, 22);
			this.statusBar1.TabIndex = 0;
			this.statusBar1.Text = "";
			// 
			// lineNumberPanel
			// 
			this.lineNumberPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.lineNumberPanel.Text = "Line";
			this.lineNumberPanel.ToolTipText = "Current line number";
			this.lineNumberPanel.Width = 36;
			((System.ComponentModel.ISupportInitialize)(this.lineNumberPanel)).EndInit();
			this.ResumeLayout(false);

		}

        void buttonClear_Click(object sender, EventArgs e)
        {
            csSource = RunTimeCompiledSource.InitialCSSource;
            vbSource = RunTimeCompiledSource.InitialVBSource;
            if (isCSSource)
                Source = csSource;
            else
                Source = vbSource;
            sourceTextBox.Refresh();
        }

		private void AddControls()
		{
			this.Controls.Add(this.checkSyntaxButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.languageCombo);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.sourceTextBox);
			this.Controls.Add(this.resultsListView);
			this.Controls.Add(this.statusBar1);
            tmr = new Timer();
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Interval = 100;

			this.Size = new Size(800,616);
			this.MinimumSize = new Size(600,300);
			this.ResumeLayout(false);

		}

        void tmr_Tick(object sender, EventArgs e)
        {
            int p = sourceTextBox.SelectionStart;
            int nl = 1;
            for (int i = 0; i < p; i++)
            {
                if (sourceTextBox.Text[i] == '\n')
                    nl++;
            }

            statusBar1.Text = " Line " + nl;
        }
		#endregion

		private void checkSyntaxButton_Click(object sender, System.EventArgs e)
		{
			RunTimeCompiledSource rtcs = this.GetRunTimeCompiledSource();
			if(rtcs == null)
				return;
			compilerResults = rtcs.Compile(true);

			ListView view = resultsListView;
			view.Items.Clear();
			if(compilerResults.Errors.HasErrors)
			{
				view.View = System.Windows.Forms.View.Details;
				for(int i=0; i<compilerResults.Errors.Count;i++)
				{
					ListViewItem item = new ListViewItem
						(new String[] { compilerResults.Errors[i].Line.ToString(), compilerResults.Errors[i].ErrorText } );
					view.Items.Add(item);
				}
			}
			else
			{
				view.Items.Add(new ListViewItem(new string[] {"","There is no syntax error."} ));
			}
			view.Refresh();
		}

		private void resultsListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ListView view = resultsListView;
			int ierr = -1;
			for(int i=0; i<view.Items.Count;i++)
			{
				if(view.SelectedItems.Count > 0 && view.SelectedItems[0] == view.Items[i])
				{
					ierr = i;
					break;
				}
			}
		}

        private void sourceTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (resultsListView.Items.Count > 0)
            {
                resultsListView.Items.Clear();
                resultsListView.Refresh();
            }
        }

		private void RunTimeCompiledSourceView_Resize(object sender, System.EventArgs e)
		{
			if(this.Size.IsEmpty || this.ClientSize.IsEmpty)
				return;
			SetControlsPositionAndSize();
		}

		private void SetControlsPositionAndSize()
		{
			this.SuspendLayout();

			// Buttons
			int y0 = 8;
			buttonOK.Location = new Point(this.ClientSize.Width - buttonOK.Width - 8,y0);
			buttonCancel.Location = new Point(buttonOK.Location.X - buttonCancel.Width - 8,y0);
			buttonClear.Location = new Point(buttonCancel.Location.X - buttonClear.Width - 8,y0);

			// Results view
			resultsListView.Location = new Point(8,this.ClientSize.Height - resultsListView.Height - 24);
			resultsListView.Size = new Size(this.ClientSize.Width - 16,resultsListView.Height);

			// Edit control
			sourceTextBox.Location = new Point(8,sourceTextBox.Top);
			sourceTextBox.Size = new Size(this.ClientSize.Width - 16,resultsListView.Location.Y - sourceTextBox.Location.Y - 8);

			this.ResumeLayout(true);
		}

		private void languageCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(!toHandleLanguageSelection)
				return;

			if(isCSSource) 
			{
				csSource = Source;
				Source = vbSource;
			}
			else
			{
				vbSource = Source;
				Source = csSource;
			}

			isCSSource = languageCombo.SelectedIndex == 0;
		}
	}
}
