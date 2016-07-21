using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardSeriesAdvancedDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.GroupBox m_sortGroupBox;
		private ComponentArt.Win.UI.Internal.GroupBox groupBox1;
		private System.Windows.Forms.TextBox m_sortTextBox;
		private System.Windows.Forms.TextBox m_selectTextBox;
		private System.Windows.Forms.NumericUpDown m_topNumberUpDown;
		private System.Windows.Forms.RadioButton m_allRowsRadioButton;
		private System.Windows.Forms.RadioButton m_topRowsRadioButton;
		private System.Windows.Forms.Label m_errorLabel;
		private ComponentArt.Win.UI.Internal.GroupBox m_selectGroupBox;
	
		public WizardSeriesAdvancedDialog() 
		{
			InitializeComponent();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (WinChart == null)
				return;

			m_selectTextBox.Text = WinChart.SelectClause;
			m_sortTextBox.Text = WinChart.OrderClause;
			m_topNumberUpDown.Value = (Decimal)WinChart.TopNumber;
			m_allRowsRadioButton.Checked = WinChart.TopNumber == 0;
			m_topRowsRadioButton.Checked = WinChart.TopNumber != 0;
		}

		#region Component Designer generated code
		private void InitializeComponent()
		{
			this.m_selectGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_selectTextBox = new System.Windows.Forms.TextBox();
			this.m_sortGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_sortTextBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new ComponentArt.Win.UI.Internal.GroupBox();
			this.m_topNumberUpDown = new System.Windows.Forms.NumericUpDown();
			this.m_topRowsRadioButton = new System.Windows.Forms.RadioButton();
			this.m_allRowsRadioButton = new System.Windows.Forms.RadioButton();
			this.m_errorLabel = new System.Windows.Forms.Label();
			this.m_selectGroupBox.SuspendLayout();
			this.m_sortGroupBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_topNumberUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// m_selectGroupBox
			// 
			this.m_selectGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.m_selectTextBox});
			this.m_selectGroupBox.DrawBorderAroundControl = true;
			this.m_selectGroupBox.Location = new System.Drawing.Point(8, 8);
			this.m_selectGroupBox.Name = "m_selectGroupBox";
			this.m_selectGroupBox.ResizeChildren = false;
			this.m_selectGroupBox.Size = new System.Drawing.Size(304, 44);
			this.m_selectGroupBox.TabIndex = 39;
			this.m_selectGroupBox.TabStop = false;
			this.m_selectGroupBox.Text = "Select criteria";
			// 
			// m_selectTextBox
			// 
			this.m_selectTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_selectTextBox.Location = new System.Drawing.Point(8, 24);
			this.m_selectTextBox.Name = "m_selectTextBox";
			this.m_selectTextBox.Size = new System.Drawing.Size(288, 13);
			this.m_selectTextBox.TabIndex = 0;
			this.m_selectTextBox.Text = "";
			this.m_selectTextBox.TextChanged += new System.EventHandler(this.m_selectTextBox_TextChanged);
			// 
			// m_sortGroupBox
			// 
			this.m_sortGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.m_sortTextBox});
			this.m_sortGroupBox.DrawBorderAroundControl = true;
			this.m_sortGroupBox.Location = new System.Drawing.Point(8, 64);
			this.m_sortGroupBox.Name = "m_sortGroupBox";
			this.m_sortGroupBox.ResizeChildren = false;
			this.m_sortGroupBox.Size = new System.Drawing.Size(304, 44);
			this.m_sortGroupBox.TabIndex = 40;
			this.m_sortGroupBox.TabStop = false;
			this.m_sortGroupBox.Text = "Sort clause";
			// 
			// m_sortTextBox
			// 
			this.m_sortTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_sortTextBox.Location = new System.Drawing.Point(8, 24);
			this.m_sortTextBox.Name = "m_sortTextBox";
			this.m_sortTextBox.Size = new System.Drawing.Size(288, 13);
			this.m_sortTextBox.TabIndex = 0;
			this.m_sortTextBox.Text = "";
			this.m_sortTextBox.TextChanged += new System.EventHandler(this.m_sortTextBox_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_topNumberUpDown,
																					this.m_topRowsRadioButton,
																					this.m_allRowsRadioButton});
			this.groupBox1.Location = new System.Drawing.Point(8, 120);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.ResizeChildren = false;
			this.groupBox1.Size = new System.Drawing.Size(160, 80);
			this.groupBox1.TabIndex = 41;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Row selection";
			// 
			// m_topNumberUpDown
			// 
			this.m_topNumberUpDown.Location = new System.Drawing.Point(49, 55);
			this.m_topNumberUpDown.Name = "m_topNumberUpDown";
			this.m_topNumberUpDown.Size = new System.Drawing.Size(40, 20);
			this.m_topNumberUpDown.TabIndex = 42;
			this.m_topNumberUpDown.ValueChanged += new System.EventHandler(this.m_topNumberUpDown_ValueChanged);
			// 
			// m_topRowsRadioButton
			// 
			this.m_topRowsRadioButton.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_topRowsRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_topRowsRadioButton.Location = new System.Drawing.Point(8, 56);
			this.m_topRowsRadioButton.Name = "m_topRowsRadioButton";
			this.m_topRowsRadioButton.Size = new System.Drawing.Size(144, 16);
			this.m_topRowsRadioButton.TabIndex = 44;
			this.m_topRowsRadioButton.Text = "Top           rows";
			this.m_topRowsRadioButton.CheckedChanged += new System.EventHandler(this.m_topRowsRadioButton_CheckedChanged);
			// 
			// m_allRowsRadioButton
			// 
			this.m_allRowsRadioButton.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_allRowsRadioButton.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
			this.m_allRowsRadioButton.Location = new System.Drawing.Point(8, 32);
			this.m_allRowsRadioButton.Name = "m_allRowsRadioButton";
			this.m_allRowsRadioButton.Size = new System.Drawing.Size(72, 16);
			this.m_allRowsRadioButton.TabIndex = 43;
			this.m_allRowsRadioButton.Text = "All rows";
			this.m_allRowsRadioButton.CheckedChanged += new System.EventHandler(this.m_allRowsRadioButton_CheckedChanged);
			// 
			// m_errorLabel
			// 
			this.m_errorLabel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.m_errorLabel.ForeColor = System.Drawing.Color.Red;
			this.m_errorLabel.Location = new System.Drawing.Point(8, 216);
			this.m_errorLabel.Name = "m_errorLabel";
			this.m_errorLabel.Size = new System.Drawing.Size(304, 56);
			this.m_errorLabel.TabIndex = 42;
			// 
			// WizardSeriesAdvancedDialog
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_errorLabel,
																		  this.groupBox1,
																		  this.m_sortGroupBox,
																		  this.m_selectGroupBox});
			this.Name = "WizardSeriesAdvancedDialog";
			this.Size = new System.Drawing.Size(320, 272);
			this.m_selectGroupBox.ResumeLayout(false);
			this.m_sortGroupBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_topNumberUpDown)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		void ProcessOrderFilter_TextChanged()
		{
			DataTable dataTable = WinChart.Chart.DataProvider.m_lastDataTable;
			if (dataTable != null) 
			{
				try 
				{
					DataRow [] dr = dataTable.Select(m_selectTextBox.Text, m_sortTextBox.Text);
				}			
				catch (System.Data.DataException ex) 
				{
					m_errorLabel.Text = "Cannot perform SELECT operation on input table.\n" +ex.Message;
					return;
				} 
				catch (	System.IndexOutOfRangeException ex) 
				{
					m_errorLabel.Text = "Cannot perform SELECT operation on input table.\n" +ex.Message;
					return;
				}
				catch (	Exception ex) 
				{
					m_errorLabel.Text = "Cannot perform SELECT operation on input table.\n" +ex.Message;
					return;
				}
			}

			m_errorLabel.Text = "";
			
			WinChart.SelectClause = m_selectTextBox.Text;
			WinChart.OrderClause = m_sortTextBox.Text;
			WinChart.Invalidate();
		}

		private void m_selectTextBox_TextChanged(object sender, System.EventArgs e)
		{
			ProcessOrderFilter_TextChanged();
		}

		private void m_sortTextBox_TextChanged(object sender, System.EventArgs e)
		{
			ProcessOrderFilter_TextChanged();
		}

		private void m_topNumberUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			WinChart.TopNumber = (int)m_topNumberUpDown.Value;
			WinChart.Invalidate();
		}

		private void m_allRowsRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			WinChart.TopNumber = 0;
			m_topNumberUpDown.Enabled = false;
			WinChart.Invalidate();
		}

		private void m_topRowsRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_topNumberUpDown.Enabled = true;
			WinChart.TopNumber = (int)m_topNumberUpDown.Value;
			WinChart.Invalidate();
		}
	}
}
