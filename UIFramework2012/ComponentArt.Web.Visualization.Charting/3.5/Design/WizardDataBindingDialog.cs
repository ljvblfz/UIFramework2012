using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Data.OleDb;
using System.Data.SqlClient;
#if __BUILDING_CRI_DESIGNER__
using Microsoft.ReportDesigner.Design;
#endif
#if __COMPILING_FOR_2_0_AND_ABOVE__ && __BuildingWebChart__
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
#endif
using ComponentArt.Web.Visualization.Charting;
using System.Web.UI.Design.WebControls;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WizardDataBindingDialog.
	/// </summary>
	internal class WizardDataBindingDialog : WizardDialog
	{
		private ComponentArt.Win.UI.Internal.ComboBox m_dataSources;
		[WizardHint("DataBindingPairs")]
		private System.Windows.Forms.Panel m_dataBindingPairsPanel;
		private ComponentArt.Win.UI.Internal.Separator separator2;
		private ComponentArt.Win.UI.Internal.GroupBox m_dataBindingPairsGroupBox;
		[WizardHint("DataSource")]
        private ComponentArt.Win.UI.Internal.GroupBox m_dataSourcePairs;
        private System.Windows.Forms.Label m_dataSourceLabel;
        private ComponentArt.Win.UI.Internal.Button m_dataSourceButton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardDataBindingDialog()
		{
			InitializeComponent();

#if __BUILDING_CRI_DESIGNER__
            m_dataBindingPairsPanel.Height = m_dataBindingPairsPanel.Height - 40;

            ComponentArt.Win.UI.Internal.Button seriesGroupingsButton = new ComponentArt.Win.UI.Internal.Button();
            seriesGroupingsButton.Text = "Series Groupings...";
            seriesGroupingsButton.Location = new Point(8, m_dataBindingPairsPanel.Top + m_dataBindingPairsPanel.Height + 16);
            seriesGroupingsButton.Size = new Size(120, seriesGroupingsButton.Height);
            seriesGroupingsButton.Click += new EventHandler(seriesGroupingsButton_Click);
            m_dataBindingPairsGroupBox.Controls.Add(seriesGroupingsButton);

#endif
        }

#if __BUILDING_CRI_DESIGNER__
        void seriesGroupingsButton_Click(object sender, EventArgs e)
        {
            WizardGroupingPopupForm wgpf = new WizardGroupingPopupForm();
            wgpf.SqlChartDesigner = (SqlChartDesigner)WinChart.Chart.Owner;

            wgpf.SeriesGroupings = SqlChartDesigner.CloneGroupingsSortingPairs(Wizard.ExtraWizardParameters.m_seriesGroupings);

            DialogResult dg = wgpf.ShowDialog();
            if (dg == DialogResult.OK)
            {
                Wizard.ExtraWizardParameters.m_seriesGroupings = SqlChartDesigner.CloneGroupingsSortingPairs(wgpf.SeriesGroupings);
            }
        }
#endif

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

		TypeConverter m_dataSourcePathConverter = new DataSourcePathConverter();

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (WinChart == null)
                return;

#if __COMPILING_FOR_2_0_AND_ABOVE__ && !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
            this.m_dataSources.Visible = false;
            UpdateDataSourceLabel();
            this.m_dataSourceButton.Click += new System.EventHandler(this.m_dataSourceButton_Click);
            SetupDataBindingPairControls();
#else
            m_dataSourceLabel.Visible = m_dataSourceButton.Visible = false;
            this.m_dataSources.SelectedIndexChanged += new System.EventHandler(this.m_dataSources_SelectedIndexChanged);

            m_dataSourceConverter = (TypeConverter)Activator.CreateInstance(typeof(
#if __COMPILING_FOR_2_0_AND_ABOVE__
#if __BuildingWebChart__
            System.Web.UI.Design.WebControls.DataSourceIDConverter
#else
DataSourceConverter
#endif
#else
            DataSourceConverter
#endif

));

            PopulateDataSourcesCombobox();
#endif
        }

#if __COMPILING_FOR_2_0_AND_ABOVE__ && __BuildingWebChart__

        private IDataSourceDesigner _dataSourceDesigner;

        protected virtual void OnDataSourceChanged(bool forceUpdateView)
        {
            bool flag1 = this.ConnectToDataSource();
            
            if (flag1 || forceUpdateView)
            {
                this.UpdateWizard();
            }
        }

        private void OnDataSourceChanged(object sender, EventArgs e)
        {
            this.OnDataSourceChanged(true);
        }
   
        void UpdateWizard()
        {
            this.DataBind();
        }

        private IDataSourceDesigner GetDataSourceDesigner()
        {
            IDataSourceDesigner dataSourceDesigner = null;
            if (m_dataSources.SelectedIndex != 0 && m_dataSources.Text != "")
            {
                System.Web.UI.Control dataSourceControl = (Wizard.ChartDesigner.Component as System.Web.UI.Control).FindControl(m_dataSources.Text);
                if ((dataSourceControl != null) && (dataSourceControl.Site != null))
                {
                    IDesignerHost designerHost = (IDesignerHost)dataSourceControl.Site.GetService(typeof(IDesignerHost));
                    if (designerHost != null)
                    {
                        dataSourceDesigner = designerHost.GetDesigner(dataSourceControl) as IDataSourceDesigner;
                    }
                }
            }
            return dataSourceDesigner;
        }
 

        protected virtual bool ConnectToDataSource()
        {
            IDataSourceDesigner dataSourceDesigner = this.GetDataSourceDesigner();
            if (this._dataSourceDesigner == dataSourceDesigner)
            {
                return false;
            }
            if (this._dataSourceDesigner != null)
            {
                this._dataSourceDesigner.DataSourceChanged -= new EventHandler(this.OnDataSourceChanged);
            }
            this._dataSourceDesigner = dataSourceDesigner;
            if (this._dataSourceDesigner != null)
            {
                this._dataSourceDesigner.DataSourceChanged += new EventHandler(this.OnDataSourceChanged);
            }
            return true;
        }

        public IDataSourceDesigner DataSourceDesigner
        {
            get
            {
                return this._dataSourceDesigner;
            }
        }

        protected virtual int SampleRowCount
        {
            get
            {
                return 5;
            }
        }

        protected virtual void DataBind()
        {
            IEnumerable enumerable1 = this.GetDesignTimeDataSource();
            bool freezeValue = WinChart.Chart.FreezeValuePath;
            WinChart.Chart.FreezeValuePath = false;
            WinChart.DataSource = enumerable1;
            WinChart.Chart.FreezeValuePath = freezeValue;
            WinChart.DataBind();
        }

        IEnumerable m_realData;

        private void OnDataSourceViewSelectCallback(IEnumerable data)
        {
            m_realData = data;
        }

        protected virtual IEnumerable GetDesignTimeDataSource()
        {

            if (m_dataSources.SelectedIndex != 0)
            {
                System.Web.UI.Control control1 = ((System.Web.UI.Control)Wizard.ChartDesigner.Component).FindControl(m_dataSources.Text);
                if (control1 is IDataSource)
                {
                    AccessDataSource ads = null;
                    String AdsFilePath = null;
                    
                    try
                    {   
                        IDataSource ids = (IDataSource)control1;
                        DataSourceView dsv = ids.GetView("");

                        //Access DataSources are usually created with a relative path to the Access file.
                        //However .NET framework does not allow you to execute an SQL query on files with relative paths at design time.
                        //So for design time behaviour to work, we replace such paths at design time with an absolute path to the file in order to get good design time behaviour.
                        if (ids is AccessDataSource)
                        {
                            ads = (AccessDataSource)ids;
                            if (ads.DataFile.StartsWith("~"))
                            {
                                IWebApplication app = (IWebApplication)control1.Page.Site.GetService(typeof(IWebApplication));
                                if (app != null)
                                {
                                    AdsFilePath = String.Copy(ads.DataFile);
                                    String rootPath = app.RootProjectItem.PhysicalPath;
                                    ads.DataFile = rootPath + ads.DataFile.Substring(1);

                                }
                            }
                        }
                        
                        DataSourceViewSelectCallback dsvsc = new DataSourceViewSelectCallback(this.OnDataSourceViewSelectCallback);
                        dsv.Select(DataSourceSelectArguments.Empty, dsvsc);
                                                
                    }
                    catch (Exception e)
                    {
                        Exception be = e.GetBaseException();
                        return null;
                    }

                    //restore original DataPath in order for it to be serialized to the page properly.
                    if (ads != null && AdsFilePath != null)
                        ads.DataFile = AdsFilePath;
                   
                    return m_realData;
                }
            }
            return null;
        }

        protected virtual IEnumerable GetSampleDataSource()
        {
            DataTable table = null;
            if (m_dataSources.SelectedIndex != 0)
            {
                table = DesignTimeData.CreateDummyDataBoundDataTable();
            }
            else
            {
                table = DesignTimeData.CreateDummyDataTable();
            }
            return DesignTimeData.GetDesignTimeDataSource(table, this.SampleRowCount);
        }
#endif

#if __COMPILING_FOR_2_0_AND_ABOVE__ && !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
        void UpdateDataSourceLabel()
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(typeof(WinChart))["DataSource"];
            TypeConverter tc = pd.Converter;

            m_dataSourceLabel.Text = tc.ConvertToInvariantString(WinChart.DataSource);
        }

		private void m_dataSourceButton_Click(object sender, EventArgs e)
        {
            try
            {
                InvokeEditor(WinChart, typeof(WinChart).GetProperty("DataSource"), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
            WinChart.DataBind();
            UpdateDataSourceLabel();

            SetupDataBindingPairControls();
        }


#else

        TypeConverter m_dataSourceConverter = null;
        bool m_loadingDatasources = false;

        void PopulateDataSourcesCombobox()
        {
#if __BUILDING_CRI_DESIGNER__
            m_loadingDatasources = true;
            SqlChartDesigner rschartDesigner = ((SqlChartDesigner)WinChart.Chart.Owner);
            Microsoft.ReportDesigner.RptDataSets rptDataSets = rschartDesigner.Report.DataSets;
            foreach (Microsoft.ReportDesigner.RptDataSet rptDataSet in rschartDesigner.Report.DataSets)
            {
                m_dataSources.Items.Add(rptDataSet.Name);

                if (rschartDesigner.DataSetName == rptDataSet.Name)
                    m_dataSources.SelectedItem = rptDataSet.Name;
            }

#else
            m_loadingDatasources = true;
            m_dataSources.Items.Clear();

#if !__COMPILING_FOR_2_0_AND_ABOVE__
			this.m_dataSources.Items.Add("New OleDbDataAdapter...");
			this.m_dataSources.Items.Add("New SqlDataAdapter...");
#endif
			
			ICollection svc = m_dataSourceConverter.GetStandardValues(new WizardDialog.ITypeDescriptorContextImpl(this, true));
            foreach (object o in svc)
            {
                string comboboxEntry = (string)m_dataSourceConverter.ConvertTo(o, typeof(string));
                m_dataSources.Items.Add(comboboxEntry);

                bool match;
#if __COMPILING_FOR_2_0_AND_ABOVE__
#if __BuildingWebChart__
                match = string.Compare(Wizard.DataSourceId, comboboxEntry) == 0;
#else
                match = WinChart.DataSource == o;
#endif
#else
                match = WinChart.DataSource == o;
#endif
                if (match)
                    m_dataSources.SelectedItem = comboboxEntry;
            }
#endif
            SetupDataBindingPairControls();
            m_loadingDatasources = false;
        }


        private void m_dataSources_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_loadingDatasources)
                return;

#if __COMPILING_FOR_2_0_AND_ABOVE__
#if __BuildingWebChart__

            string value = (string)m_dataSources.SelectedItem;
            if (m_dataSources.SelectedIndex == m_dataSources.Items.Count-1)
            {
                System.Web.UI.Design.ControlDesigner.InvokeTransactedChange(Wizard.Component, new System.Web.UI.Design.TransactedChangeCallback(this.CreateDataSourceCallback), null, "Create a new data source");
            }
            else
            {
                if (m_dataSources.SelectedIndex == 0)
                {
                    Wizard.DataSourceId = "";
                }
                else
                {

                    Wizard.DataSourceId = m_dataSources.Text;
                }

                this.OnDataSourceChanged(false);
                WinChart.Invalidate();
                SetupDataBindingPairControls();
            }
#endif

#if __BUILDING_CRI_DESIGNER__
            string value = (string)m_dataSources.SelectedItem;
            WinChart.Chart.DataSource = m_dataSources.Text;
            WinChart.Refresh();
            SetupDataBindingPairControls();
#endif

#else
            if (m_dataSources.SelectedIndex == 0 || m_dataSources.SelectedIndex == 1) 
			{
				Type type;
				if (this.m_dataSources.SelectedIndex == 0)
					type = typeof(System.Data.OleDb.OleDbDataAdapter);
				else
					type = typeof(System.Data.SqlClient.SqlDataAdapter);

				IComponent dsComponent = null;

				IDesignerHost host = (IDesignerHost)Wizard.Component.Site.GetService(typeof(IDesignerHost));
				DesignerTransaction transaction = host.CreateTransaction("Create " + type.ToString());
				dsComponent = host.CreateComponent(type);
				transaction.Commit();

				host.Activate();
				IDbDataAdapter newAdapter = dsComponent as IDbDataAdapter;

				if (dsComponent != null && newAdapter != null) 
				{
					IDesigner dsDesigner = host.GetDesigner(dsComponent);
					if (dsDesigner != null)
					{
						dsDesigner.Verbs[0].Invoke();
					}

					if (newAdapter.SelectCommand != null && 
						newAdapter.SelectCommand.CommandText != null && 
						newAdapter.SelectCommand.CommandText.Length > 0) 
					{
						this.m_dataSources.Items.Add(dsComponent.Site.Name);
						this.m_dataSources.SelectedIndex = this.m_dataSources.Items.Count-1;
					}
					else
					{
						if (dsComponent != null)
						{
							host.DestroyComponent(dsComponent);
						}
						this.m_dataSources.SelectedItem = m_dataSourceConverter.ConvertTo(null, typeof(string));
					}
				}
			}

			object selectedDataSource = m_dataSourceConverter.ConvertFrom(new WizardDialog.ITypeDescriptorContextImpl(this, true), System.Globalization.CultureInfo.CurrentCulture, m_dataSources.Text);

			if (WinChart.DataSource == selectedDataSource)
				return;
				
			WinChart.DataSource = selectedDataSource;
			WinChart.Refresh();
			SetupDataBindingPairControls();
#endif
        }

#endif

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.m_dataSources = new ComponentArt.Win.UI.Internal.ComboBox();
            this.m_dataBindingPairsPanel = new System.Windows.Forms.Panel();
            this.separator2 = new ComponentArt.Win.UI.Internal.Separator();
            this.m_dataBindingPairsGroupBox = new ComponentArt.Win.UI.Internal.GroupBox();
            this.m_dataSourcePairs = new ComponentArt.Win.UI.Internal.GroupBox();
            this.m_dataSourceLabel = new System.Windows.Forms.Label();
            this.m_dataSourceButton = new ComponentArt.Win.UI.Internal.Button();
            this.m_dataBindingPairsGroupBox.SuspendLayout();
            this.m_dataSourcePairs.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_dataSources
            // 
            this.m_dataSources.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.m_dataSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_dataSources.Location = new System.Drawing.Point(82, 3);
            this.m_dataSources.Name = "m_dataSources";
            this.m_dataSources.Size = new System.Drawing.Size(204, 21);
            this.m_dataSources.TabIndex = 1;
            // 
            // m_dataBindingPairsPanel
            // 
            this.m_dataBindingPairsPanel.AutoScroll = true;
            this.m_dataBindingPairsPanel.Location = new System.Drawing.Point(2, 26);
            this.m_dataBindingPairsPanel.Name = "m_dataBindingPairsPanel";
            this.m_dataBindingPairsPanel.Size = new System.Drawing.Size(306, 194);
            this.m_dataBindingPairsPanel.TabIndex = 4;
            // 
            // separator2
            // 
            this.separator2.Location = new System.Drawing.Point(8, 40);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(304, 1);
            this.separator2.TabIndex = 48;
            this.separator2.TabStop = false;
            // 
            // m_dataBindingPairsGroupBox
            // 
            this.m_dataBindingPairsGroupBox.Controls.Add(this.m_dataBindingPairsPanel);
            this.m_dataBindingPairsGroupBox.Location = new System.Drawing.Point(6, 46);
            this.m_dataBindingPairsGroupBox.Name = "m_dataBindingPairsGroupBox";
            this.m_dataBindingPairsGroupBox.ResizeChildren = false;
            this.m_dataBindingPairsGroupBox.Size = new System.Drawing.Size(311, 226);
            this.m_dataBindingPairsGroupBox.TabIndex = 50;
            this.m_dataBindingPairsGroupBox.TabStop = false;
            this.m_dataBindingPairsGroupBox.Text = "Data Binding Pairs:";
            // 
            // m_dataSourcePairs
            // 
            this.m_dataSourcePairs.Controls.Add(this.m_dataSourceButton);
            this.m_dataSourcePairs.Controls.Add(this.m_dataSourceLabel);
            this.m_dataSourcePairs.Controls.Add(this.m_dataSources);
            this.m_dataSourcePairs.Location = new System.Drawing.Point(6, 8);
            this.m_dataSourcePairs.Name = "m_dataSourcePairs";
            this.m_dataSourcePairs.ResizeChildren = false;
            this.m_dataSourcePairs.Size = new System.Drawing.Size(311, 25);
            this.m_dataSourcePairs.TabIndex = 51;
            this.m_dataSourcePairs.TabStop = false;
            this.m_dataSourcePairs.Text = "Data Source:";
            // 
            // m_dataSourceLabel
            // 
            this.m_dataSourceLabel.AutoSize = true;
            this.m_dataSourceLabel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.m_dataSourceLabel.Location = new System.Drawing.Point(83, 6);
            this.m_dataSourceLabel.Name = "m_dataSourceLabel";
            this.m_dataSourceLabel.Size = new System.Drawing.Size(122, 13);
            this.m_dataSourceLabel.TabIndex = 5;
            this.m_dataSourceLabel.Text = "Selected Datasource";
            // 
            // m_dataSourceButton
            // 
            this.m_dataSourceButton.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.m_dataSourceButton.Location = new System.Drawing.Point(266, 7);
            this.m_dataSourceButton.Name = "m_dataSourceButton";
            this.m_dataSourceButton.Size = new System.Drawing.Size(20, 14);
            this.m_dataSourceButton.TabIndex = 51;
            this.m_dataSourceButton.Text = "...";
            this.m_dataSourceButton.TextLocation = new System.Drawing.Point(5, 2);
            // 
            // WizardDataBindingDialog
            // 
            this.Controls.Add(this.m_dataBindingPairsGroupBox);
            this.Controls.Add(this.m_dataSourcePairs);
            this.Controls.Add(this.separator2);
            this.Name = "WizardDataBindingDialog";
            this.Size = new System.Drawing.Size(320, 280);
            this.m_dataBindingPairsGroupBox.ResumeLayout(false);
            this.m_dataSourcePairs.ResumeLayout(false);
            this.m_dataSourcePairs.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


#if __COMPILING_FOR_2_0_AND_ABOVE__
#if __BuildingWebChart__
        private bool CreateDataSourceCallback(object context)
        {
            string selectedItem;
            Wizard wizard = this.Wizard;
            ChartDesigner chartDesigner = wizard.ChartDesigner;
            ControlDesigner originalDesigner = chartDesigner.OriginalDesigner as ControlDesigner;

            DialogResult result1 = BaseDataBoundControlDesigner.ShowCreateDataSourceDialog(
                originalDesigner, typeof(System.Web.UI.IDataSource), true, out selectedItem);
            if (selectedItem.Length > 0)
            {
                PopulateDataSourcesCombobox();
                m_dataSources.SelectedItem = selectedItem;
            }
            return (result1 == DialogResult.OK);
        }
#endif
#endif

		private void varCombobox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            ComponentArt.Win.UI.Internal.ComboBox varCombobox = (ComponentArt.Win.UI.Internal.ComboBox)sender;
			string inputVarName =  varCombobox.Name.Split(new char[] {'-'})[1];
			InputVariable iv = WinChart.InputVariables[inputVarName];
#if __BUILDING_CRI_DESIGNER__
            if (varCombobox.SelectedIndex == 0 && m_launchEditor)
            {
                m_launchEditor = false;
                ExpressionEditor editor = new ExpressionEditor();
                string newValue;
                SqlChartDesigner rschartDesigner = ((SqlChartDesigner)WinChart.Chart.Owner);
                newValue = (string)editor.EditValue(null, rschartDesigner.Site, m_oldComboValue);
                varCombobox.Items[0] = newValue;
                iv.RSExpression = newValue;
            }
            else
            {
                iv.RSExpression = varCombobox.Text;
            }


#else
			if (iv == null)
				return;
			
			iv.ValuePath = (string)m_dataSourcePathConverter.ConvertFrom(varCombobox.Text);

#endif
            WinChart.Invalidate();
		}

		protected override void OnVisibleChanged(EventArgs e) 
		{
			if (!Visible)
				return;

#if __COMPILING_FOR_2_0_AND_ABOVE__ && __BuildingWebChart__
            ConnectToDataSource();
#endif
            if (Wizard == null)
                return;

			SetupDataBindingPairControls();
            Invalidate();
		}

        const int SpaceBetweenEntries = 44;

		internal void SetupDataBindingPairControls()
		{
			m_dataBindingPairsPanel.Controls.Clear();
			if (WinChart == null)
				return;

			InputVariableCollection ivc = WinChart.InputVariables;
            int i = 0;

#if __BUILDING_CRI_DESIGNER__
            Series[] seriesLeaves = this.WinChart.Chart.AllLeaves();

            foreach (Series series in seriesLeaves)
            {
                if (ivc[series.Name + ":action"] == null)
                    ivc.Add(new InputVariable(series.Name + ":action"));
            }
#endif

            foreach (InputVariable iv in ivc ) 
			{

#if __BUILDING_CRI_DESIGNER__
                if (iv.Name.EndsWith(":x"))
                    continue;
#endif
                System.Windows.Forms.Label varLabel = new System.Windows.Forms.Label();
				varLabel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(102)), ((System.Byte)(102)), ((System.Byte)(102)));
                varLabel.Location = new System.Drawing.Point(0, 8 + i * SpaceBetweenEntries);
				varLabel.Name = "varLabel-" + iv.Name;
                varLabel.Size = new System.Drawing.Size(276, 16);
				varLabel.Font = new System.Drawing.Font("Verdana", 8.25F);
				varLabel.Text = iv.Name;

                if (iv.Name.EndsWith(":action"))
                {
                    ComponentArt.Win.UI.Internal.Button actionButton = new ComponentArt.Win.UI.Internal.Button();
                    actionButton.Name = "actionButton-" + iv.Name;
                    actionButton.Text = "...";
                    actionButton.Size = new Size(24, 21);
                    actionButton.Location = new System.Drawing.Point(80, 5 + i * SpaceBetweenEntries + 20);
#if __BUILDING_CRI_DESIGNER__
                    actionButton.Click += new EventHandler(actionButton_Click);
#endif
                    m_dataBindingPairsPanel.Controls.AddRange(new System.Windows.Forms.Control[] { varLabel, actionButton });

                }
                else
                {
                    ComponentArt.Win.UI.Internal.ComboBox varCombobox = new ComponentArt.Win.UI.Internal.ComboBox();
                    varCombobox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                    WizardObjectDescriptorContext wodc = new WizardObjectDescriptorContext(iv);
                    bool exclusive = m_dataSourcePathConverter.GetStandardValuesExclusive(wodc);
                    varCombobox.DropDownStyle = (exclusive ? System.Windows.Forms.ComboBoxStyle.DropDownList : System.Windows.Forms.ComboBoxStyle.DropDown);
                    varCombobox.Location = new System.Drawing.Point(8, 5 + i * SpaceBetweenEntries + 20);
                    varCombobox.Name = "varCombobox-" + iv.Name;
                    varCombobox.Size = new System.Drawing.Size(276, 21);
                    varCombobox.TabIndex = 10 * i + 10;
#if !__BUILDING_CRI_DESIGNER__
				varCombobox.Items.Add(DataBindingPair.m_noneString);
#endif

#if __BUILDING_CRI_DESIGNER__
                    varCombobox.Items.Clear();
                    varCombobox.Items.Add("<Expression...>");
                    varCombobox.SelectionChangeCommitted += new EventHandler(varCombobox_SelectionChangeCommitted);
                    SqlChartDesigner rschartDesigner = ((SqlChartDesigner)WinChart.Chart.Owner);
                    Microsoft.ReportDesigner.RptDataSet dataset = rschartDesigner.GetDataSet(m_dataSources.Text);
                    if (dataset != null)
                    {
                        for (int j = 0; j < dataset.Fields.Count; j++)
                        {
                            if (string.Compare(iv.Name, "x") != 0)
                            {
                                varCombobox.Items.Add(SqlChartDesigner.GetAggregate(dataset.Fields[j]));
                            }
                            else
                            {
                                varCombobox.Items.Add("=Fields!" + dataset.Fields[j].Name + ".Value");
                            }
                        }
                    }
#else
                varCombobox.Items.AddRange(WinChart.Chart.DataSourcePaths);
#endif


#if __BUILDING_CRI_DESIGNER__
                    varCombobox.Text = iv.RSExpression;
#else
				varCombobox.Text = (string)m_dataSourcePathConverter.ConvertTo(iv.ValuePath, typeof(string));
#endif

                    varCombobox.SelectedIndexChanged += new System.EventHandler(this.varCombobox_SelectedIndexChanged);
#if __BUILDING_CRI_DESIGNER__
                    varCombobox.TextChanged += new EventHandler(varCombobox_TextChanged);
#endif
                    m_dataBindingPairsPanel.Controls.AddRange(new System.Windows.Forms.Control[] { varLabel, varCombobox });
                }
                ++i;
			}
		}


#if __BUILDING_CRI_DESIGNER__
        void actionButton_Click(object sender, EventArgs e)
        {
            ComponentArt.Win.UI.Internal.Button actionButton = (ComponentArt.Win.UI.Internal.Button)sender;
            string inputVarName = actionButton.Name.Split(new char[] { '-' })[1];
            InputVariable iv = WinChart.InputVariables[inputVarName];
            ActionForm af = new ActionForm();
            af.InputVariable = iv;
            DialogResult dr = af.ShowDialog();
            if (dr == DialogResult.OK)
            {
                iv.RSExpression = af.GetRSExpression();
            }
        }
#endif


#if __BUILDING_CRI_DESIGNER__
        void varCombobox_TextChanged(object sender, EventArgs e)
        {
            ComponentArt.Win.UI.Internal.ComboBox varCombobox = (ComponentArt.Win.UI.Internal.ComboBox)sender;
            string inputVarName = varCombobox.Name.Split(new char[] { '-' })[1];
            InputVariable iv = WinChart.InputVariables[inputVarName];

            if (iv == null)
                return;

            iv.RSExpression = varCombobox.Text;
        }
#endif

        private String m_oldComboValue;
#if __BUILDING_CRI_DESIGNER__
        private bool m_launchEditor = false;
#endif

        void varCombobox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            m_oldComboValue = combo.Text;

#if __BUILDING_CRI_DESIGNER__
			m_launchEditor = true;
#endif
		}

	}

    internal class WizardObjectDescriptorContext : ITypeDescriptorContext
    {
        object m_wizardObject;

        internal WizardObjectDescriptorContext(object wizardObject)
        {
            m_wizardObject = wizardObject;
        }

        // Methods
        void ITypeDescriptorContext.OnComponentChanged() { }
        bool ITypeDescriptorContext.OnComponentChanging() { return true; }
        object IServiceProvider.GetService(System.Type serviceType)
        {
            return null;
        }

        // Properties
        IContainer ITypeDescriptorContext.Container { get { return null; } }
        object ITypeDescriptorContext.Instance { get { return m_wizardObject; } }
        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor { get { return null; } }
    }

}
