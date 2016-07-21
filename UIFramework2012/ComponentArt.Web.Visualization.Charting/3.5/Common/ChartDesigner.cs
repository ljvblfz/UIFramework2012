using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using Microsoft.Win32;
using System.Collections;

using ComponentArt.Web.Visualization.Charting;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class ChartDesigner : ComponentDesigner
	{
		private DesignerVerbCollection verbs = new DesignerVerbCollection();
        ComponentArt.Web.Visualization.Charting.Design.Wizard cw;

        DesignerVerb m_wizardVerb;
        DesignerVerb m_aboutVerb;
        DesignerVerb m_loadChartTemplateVerb;
        DesignerVerb m_saveChartTemplateVerb;
        DesignerVerb m_loadPalettesVerb;
        DesignerVerb m_savePalettesVerb;

		public ChartDesigner()
		{
			verbs.Add(m_wizardVerb = new DesignerVerb ("Wizard...", new EventHandler(OnWizard)));
			verbs.Add(m_aboutVerb = new DesignerVerb ("About...", new EventHandler(OnAbout)));
			verbs.Add(m_loadChartTemplateVerb = new DesignerVerb ("Load Chart Template...", new EventHandler(OnLoadChartTemplate)));
			verbs.Add(m_saveChartTemplateVerb = new DesignerVerb ("Save Chart Template...", new EventHandler(OnSaveChartTemplate)));
			verbs.Add(m_loadPalettesVerb = new DesignerVerb ("Load Palettes...", new EventHandler(OnLoadPalettes)));
			verbs.Add(m_savePalettesVerb = new DesignerVerb ("Save Palettes...", new EventHandler(OnSavePalettes)));
        }

        internal DesignerVerb WizardVerb { get { return m_wizardVerb; } }
        internal DesignerVerb AboutVerb { get { return m_aboutVerb; } }
        internal DesignerVerb LoadChartTemplateVerb { get { return m_loadChartTemplateVerb; } }
        internal DesignerVerb SaveChartTemplateVerb { get { return m_saveChartTemplateVerb; } }
        internal DesignerVerb LoadPalettesVerb { get { return m_loadPalettesVerb; } }
        internal DesignerVerb SavePalettesVerb { get { return m_savePalettesVerb; } }

		public override DesignerVerbCollection Verbs
		{
			get
			{
				return verbs;
			}
		}



		Exception m_exception;
		internal Exception Exception 
		{
			get {return m_exception;}
			set 
			{
				m_exception = value;
			}
		}

		internal void OnReportError(object sender, EventArgs e) 
		{
			MessageBox.Show(m_exception.StackTrace, m_exception.Message);
		}

        internal ChartBase Chart
        {
            get
            {
#if __BuildingWebChart__
				Chart chart = OriginalDesigner.Component as Chart;
				return chart.ChartBase;
#else
				WinChart chart = OriginalDesigner.Component as WinChart;
				return chart.Chart;
#endif
            }
        }

		internal void OnWizard(object sender, EventArgs e) 
		{
            WinChart winchart = WinChart.CreateInstanceForWizard(this.Chart);

			cw = new Wizard(winchart);
			cw.ApplyButton.Enabled = !m_autoLaunchedWizard;
            cw.ChartDesigner = this;

			try 
			{
				Point p = RegistryValues.WizardLocation;
				cw.StartPosition = FormStartPosition.Manual;
				cw.Location = p;
			} 
			catch
			{
			}

            ToWizard(Component, cw);

			cw.Component = Component;
           if (VisualStudioVersion >= 9)
                cw.AutoLaunchCheckBoxVisible = false;

			cw.FinishButton.Click += new System.EventHandler(this.FinishApply_Click);
			cw.ApplyButton.Click += new System.EventHandler(this.FinishApply_Click);
			cw.Closed += new System.EventHandler(RememberLocation);
			cw.ShowDialog();
		}

        internal static int VisualStudioVersion
        {
            get
            {
                try
                {
                    return int.Parse(Application.ProductVersion.Split('.')[0]);
                }
                catch
                {
                    return 0;
                }
            }
        }
		public event EventHandler WizardCalled;


		protected virtual void OnWizardCalled(EventArgs e) 
		{
			if (WizardCalled != null)
				WizardCalled(this,e);
		}


		public event EventHandler BeforeRaiseComponentChangedCalled;

		protected virtual void OnBeforeRaiseComponentChangedCalled(EventArgs e) 
		{
			if (BeforeRaiseComponentChangedCalled != null)
				BeforeRaiseComponentChangedCalled(this,e);
		}


		void RememberLocation(object sender, System.EventArgs e)
		{
			RegistryValues.WizardLocation = cw.Location;
		}

		private void FinishApply_Click(object sender, System.EventArgs e)
		{
			ChartBase objToSet = cw.Chart;

			objToSet.InWizard = false;

			RaiseComponentChanging(null);
            FromWizard(cw, Component);

            PropertyValue.Set(Component, "ChartBase", objToSet);

			objToSet.FreezeValuePath = true;

			OnBeforeRaiseComponentChangedCalled(EventArgs.Empty);

			RaiseComponentChanged(null, null, null);

			OnWizardCalled(EventArgs.Empty);
#if __COMPILING_FOR_2_0_AND_ABOVE__ && __BuildingWebChart__
            objToSet.FreezeValuePath = true;
#else
            objToSet.FreezeValuePath = false;
#endif

        }

        private void FromWizard(ComponentArt.Web.Visualization.Charting.Design.Wizard src, object chart)
        {
            PropertyValue.Set(chart,"BackColor", src.WinChart.BackColor);
            PropertyValue.Set(chart,"Text", src.WinChart.Text.Clone());
            PropertyValue.Set(chart,"ForeColor", src.WinChart.ForeColor);

            if (PropertyValue.Exists(chart, "BackgroundImageUrl"))
                PropertyValue.Set(chart, "BackgroundImageUrl", src.BackgroundImageURL); //BackgroundImageUrl = src.BackgroundImageURL;
#if __COMPILING_FOR_2_0_AND_ABOVE__
            PropertyValue.Set(chart, "DataSourceID", src.DataSourceId);  //DataSourceID = src.DataSourceId;
#endif
        }

        internal void ToWizard(object chart, ComponentArt.Web.Visualization.Charting.Design.Wizard dest)
        {
            dest.WinChart.BackColor = (Color)PropertyValue.Get(chart,"BackColor");
            dest.WinChart.Text = (string)((string)PropertyValue.Get(chart,"Text")).Clone();
            dest.WinChart.ForeColor = (Color) PropertyValue.Get(chart,"ForeColor");
            if (PropertyValue.Exists(chart, "BackgroundImageUrl"))
                dest.BackgroundImageURL = (string)PropertyValue.Get(chart, "BackgroundImageUrl");

#if __COMPILING_FOR_2_0_AND_ABOVE__
            dest.DataSourceId = (string)PropertyValue.Get(chart, "DataSourceID");
#endif
        }

		protected void OnAbout(object sender, EventArgs e) 
		{

            Assembly mainAssembly = Assembly.GetCallingAssembly();

			string[] str =  ChartBase.MainAssemblyType.AssemblyQualifiedName.Split(',');

            string text = ChartBase.MainAssemblyType.FullName + ": " + 
				"\n" + str[2].Trim() + 
				"\n" + str[3].Trim() +
				"\n" + str[4].Trim() ;

			AboutDialog dlg = new AboutDialog(text);
			dlg.ShowDialog();
			dlg.Dispose();
		}

		protected void OnLoadChartTemplate(object sender, EventArgs e)
		{
			try
			{
				ChartXmlSerializer.OpenXmlTemplate(Component);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error details:\n" +
					ex.Message + "\n\nCaution: Chart object is not completely loaded",
							"Error in loading from template",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			RaiseComponentChanged(null, null, null);
		}

		protected void OnSaveChartTemplate(object sender, EventArgs e)
		{
			ChartXmlSerializer.SaveXmlTemplate(Component,"Chart");
		}

		protected void OnLoadPalettes(object sender, EventArgs e) 
		{
            PropertyInfo pi = ChartBase.MainAssemblyType.GetProperty("Palettes");
			PaletteCollection pc = (PaletteCollection)pi.GetValue(Component, null);
			WizardPaletteDialog.LoadPalettes(pc);
		}


		protected void OnSavePalettes(object sender, EventArgs e) 
		{
			PropertyInfo pi = ChartBase.MainAssemblyType.GetProperty("Palettes");
			PaletteCollection pc = (PaletteCollection)pi.GetValue(Component, null);
			WizardPaletteDialog.SavePalettes(pc);
		}

        ComponentDesigner m_originalDesigner;
        internal ComponentDesigner OriginalDesigner
        {
            get
            {
                return m_originalDesigner;
            }
            set
            {
                m_originalDesigner = value;
            }
        }

		private IComponentChangeService m_componentChangeService;

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);

			m_componentChangeService = (IComponentChangeService) GetService(typeof(IComponentChangeService));

			if (m_componentChangeService != null) 
			{
				m_componentChangeService.ComponentAdded -= new ComponentEventHandler(service_ComponentAdded);
			}

			if (m_componentChangeService != null) 
			{
				m_componentChangeService.ComponentAdded += new ComponentEventHandler(service_ComponentAdded);
			}
        }


		private void service_ComponentAdded(object sender, ComponentEventArgs e)
		{
			IDesignerHost host = (IDesignerHost) GetService(typeof(IDesignerHost));
			if (host != null && !(host.Loading) && this.Component == e.Component) 
			{
                IChart chartControl = Component as IChart;
                if (chartControl == null)
                    throw new Exception("ChartDesigner cannot handle component type '" + Component.GetType().ToString() + "'");
                else
                {
                    // Making sure the initial contents are created
                    SeriesCollection sc = (SeriesCollection)chartControl.Series;
                    CompositeSeries cSeries = (CompositeSeries)(sc.Owner);
                    ChartBase chart = cSeries.OwningChart;
                    chart.SetDesignMode(true);
                    chart.NeedsCreationOfInitialContents = true;
                    chart.DataBind();
                }
				if (RegistryValues.AutoLaunchWizard && VisualStudioVersion < 9) 
				{
					m_autoLaunchedWizard = true;
					Verbs[0].Invoke();
					m_autoLaunchedWizard = false;
				}
			}
		}

		bool m_autoLaunchedWizard;
		bool m_needToAutoLaunchWizard;

		internal bool NeedToAutoLaunchWizard 
		{
			get 
			{
				return m_needToAutoLaunchWizard;
			}
			set 
			{
				m_needToAutoLaunchWizard = value;
			}
		}


#if __COMPILING_FOR_2_0_AND_ABOVE__

        ChartActionList m_upperActionList;
        ChartActionList m_lowerActionList;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection dalc = new DesignerActionListCollection();
                dalc.AddRange(base.ActionLists);

                this.m_upperActionList = new UpperChartActionList(this);
                this.m_upperActionList.AutoShow = true;
                this.m_lowerActionList = new LowerChartActionList(this);
                this.m_lowerActionList.AutoShow = true;

                dalc.Add(this.m_upperActionList);
                dalc.Add(this.m_lowerActionList);

                return dalc;
            }
        }

        internal object GetProperty(string propertyName)
        {
            PropertyInfo pi = Component.GetType().GetProperty(propertyName);
            object o = pi.GetValue(Component, null);
            return o;
        }

        internal void SetProperty(string propertyName, object value)
        {
            SetProperty(propertyName, value, Component.GetType().GetProperty(propertyName), Component);
        }

        internal void SetProperty(string propertyName, object value, PropertyInfo piToSet, object obj)
        {
           
            IDesignerHost designerHost = this.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
            PropertyDescriptor pdToChange = TypeDescriptor.GetProperties(Component.GetType())[propertyName];
            IComponentChangeService changeService = this.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            string transactionStr = /*((WinChart)Component).Name + " " +*/ propertyName + " changed";
            DesignerTransaction transaction = designerHost.CreateTransaction(transactionStr);
            try
            {
                changeService.OnComponentChanging(this.Component, pdToChange);
                piToSet.SetValue(obj, value, null);
                changeService.OnComponentChanged(this.Component, pdToChange, null, null);
                transaction.Commit();
                transaction = null;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Cancel();
                }
            }
      }



#if __BuildingWebChart__
        string manipulatedPropertyName = "";
        PropertyInfo m_piToSet;
        object m_objToSet;


        private bool SetPropertyCallback(object context)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(m_objToSet.GetType())[m_piToSet.Name];
            descriptor.SetValue(m_objToSet, context);
            return true;
        }
#endif


#endif

    }

    #region ActionListClasses
    #if __COMPILING_FOR_2_0_AND_ABOVE__
    internal class UpperChartActionList : ChartActionList
    {
        public UpperChartActionList(ChartDesigner chartDesigner)
            : base(chartDesigner)
        {
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection daic = new DesignerActionItemCollection();
            daic.Add(new DesignerActionVerbItem(this, ChartDesigner.WizardVerb, "Wizard"));
            return daic;
        }

    }

    internal class LowerChartActionList : ChartActionList
    {
        public LowerChartActionList(ChartDesigner chartDesigner)
            : base(chartDesigner)
        {
        }


        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection daic = new DesignerActionItemCollection();
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(ChartDesigner.Component);

            PropertyDescriptor pd;

            pd = pdc["MainStyle"];
            daic.Add(new DesignerActionPropertyItem(pd.Name, "Main Style:", pd.Category, pd.Description));

            pd = pdc["SelectedPaletteName"];
            daic.Add(new DesignerActionPropertyItem(pd.Name, "Selected Palette Name:", pd.Category, pd.Description));

            pd = TypeDescriptor.GetProperties(typeof(Mapping))["ViewDirection"];
            daic.Add(new DesignerActionPropertyItem(pd.Name, "View Direction:", pd.Category, pd.Description));

            daic.Add(new DesignerActionVerbItem(this, ChartDesigner.LoadChartTemplateVerb, "ChartTemplate"));
            daic.Add(new DesignerActionVerbItem(this, ChartDesigner.SaveChartTemplateVerb, "ChartTemplate"));

            daic.Add(new DesignerActionVerbItem(this, ChartDesigner.LoadPalettesVerb, "Palettes"));
            daic.Add(new DesignerActionVerbItem(this, ChartDesigner.SavePalettesVerb, "Palettes"));

            daic.Add(new DesignerActionVerbItem(this, ChartDesigner.AboutVerb, "About"));

            return daic;

        }


    }

    internal class ChartActionList : DesignerActionList/*, ICustomTypeDescriptor*/
    {

        private ChartDesigner m_chartDesigner;

        internal ChartDesigner ChartDesigner
        {
            get
            {
                return m_chartDesigner;
            }
        }


        public ChartActionList(ChartDesigner chartDesigner)
            : base(chartDesigner.Component)
        {
            this.m_chartDesigner = chartDesigner;
            this.AutoShow = true;
        }

        internal ChartBase Chart
        {
            get
            {
                return m_chartDesigner.Chart;
            }
        }

        //[EditorAttribute(typeof(SelectedPaletteEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SelectedPaletteConverter))]
        public string SelectedPaletteName
        {
            get
            {
                return (string)this.m_chartDesigner.GetProperty("SelectedPaletteName");
            }
            set
            {
                this.m_chartDesigner.SetProperty("SelectedPaletteName", value);
            }
        }


        [
            Bindable(true),
            NotifyParentProperty(true),
            RefreshProperties(RefreshProperties.All),
            Description("View point vector"),
            Category("Viewing and Mapping Parameters"),
            Editor(typeof(ViewPointTrackballEditor), typeof(System.Drawing.Design.UITypeEditor))
        ]
        public Vector3D ViewDirection
        {

            get
            {
                return this.m_chartDesigner.Chart.Mapping.ViewDirection;
            }
            set
            {
                this.m_chartDesigner.SetProperty("View", value, typeof(Mapping).GetProperty("ViewDirection"), this.m_chartDesigner.Chart.Mapping);
            }

        }

        [Description("Chart style")]
        [Category("Chart Contents")]
        //[TypeConverter(typeof(Design.SelectedSeriesStyleConverter))]
        [Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        public string MainStyle
        {
            get
            {
                return (string)this.m_chartDesigner.GetProperty("MainStyle");
            }
            set
            {
                this.m_chartDesigner.SetProperty("MainStyle", value);
            }
        }
    }

    internal class DesignerActionVerbItem : DesignerActionMethodItem
    {
        // Methods
        internal DesignerActionVerbItem(DesignerActionList list, DesignerVerb verb, string category)
            : base(list, string.Empty, verb.Text, category)
        {
            this.verb = verb;
        }

        public override void Invoke()
        {
            verb.Invoke();
        }

        // Fields
        private DesignerVerb verb;
    }

#endif
    #endregion

}