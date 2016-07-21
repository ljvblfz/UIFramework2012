using System;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.ComponentModel.Design;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Diagnostics;
using ComponentArt.Web.Visualization.Charting.Design;



namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WebChartDesigner.
	/// </summary>
	internal class WebChartDesigner : 
#if __COMPILING_FOR_2_0_AND_ABOVE__
        System.Web.UI.Design.WebControls.DataBoundControlDesigner
#else
		System.Web.UI.Design.ControlDesigner, IDataSourceProvider 
#endif

	{
        private static bool firstLoad = true;

        private const string tempfilenametemplate = "TempComponentArtChart";
        private ChartDesigner cd;
        private Control m_webchart;
        private ComponentChangedEventHandler changedEventHandler = null;
        private string imgPath = null;
        private IEnumerable m_realData;
        private string m_toBeRemoved = null;

        public WebChartDesigner()
            : base()
        {
            cd = new ChartDesigner();
        }

		// Verbs
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return cd.Verbs;
			}
		}

#if __COMPILING_FOR_2_0_AND_ABOVE__
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection dalc = new DesignerActionListCollection();
                dalc.AddRange(cd.ActionLists);

                for (int i = 0; i < base.ActionLists.Count; ++i)
                {
                    dalc.Insert(i + 1, base.ActionLists[i]);
                }

                return dalc;
            }
        }


#endif


#if !__COMPILING_FOR_2_0_AND_ABOVE__
		object IDataSourceProvider.GetSelectedDataSource()
		{
			return this.GetDataSource();
		}
 
		IEnumerable IDataSourceProvider.GetResolvedSelectedDataSource()
		{
			return (IEnumerable) ((IDataSourceProvider) this).GetSelectedDataSource();
		}
 

		public object GetDataSource()
		{
            return MyWebChart.DataSource;
		}


		/// <summary>
		/// Adds converter in design time
		/// </summary>
		/// <param name="properties"></param>
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor dataSourceDescriptor = (PropertyDescriptor) properties["DataSource"];
			System.ComponentModel.AttributeCollection atts = dataSourceDescriptor.Attributes;
			Attribute[] newAtts = new Attribute[atts.Count + 1];
			atts.CopyTo(newAtts, 0);
			newAtts[atts.Count] = new TypeConverterAttribute(typeof(WebDataSourceConverter /*ComponentArt.Web.Visualization.Charting.Design.DataSourceConverter*/ /*System.Web.UI.Design.DataSourceConverter*/));
			dataSourceDescriptor = TypeDescriptor.CreateProperty(base.GetType(), "DataSource", typeof(string), newAtts);
			properties["DataSource"] = dataSourceDescriptor;

		} 


		public string DataSource
		{
			get
			{
				DataBinding binding1 = base.DataBindings["DataSource"];
				if (binding1 != null)
				{
					return binding1.Expression;
				}
				return string.Empty;
			}
			set
			{
				if ((value == null) || (value.Length == 0))
				{
					base.DataBindings.Remove("DataSource");
                    this.MyWebChart.DataSource = null;
				}
				else
				{
					DataBinding binding1 = base.DataBindings["DataSource"];
					if (binding1 == null)
					{
						binding1 = new DataBinding("DataSource", typeof(IEnumerable), value);
					}
					else
					{
						binding1.Expression = value;
					}
					base.DataBindings.Add(binding1);
									
					IComponent comp = Component.Site.Container.Components[value];
                    if (MyWebChart.DataSource != comp)
                        MyWebChart.DataSource = comp;
				}
				this.OnBindingsCollectionChanged("DataSource");
			}
		}
#endif

		public override bool AllowResize 
		{
			get 
			{
				return true;
			}
		}

		public override void Initialize ( System.ComponentModel.IComponent component ) 
		{
			base.Initialize(component);
			cd.Initialize(component);
            m_webchart = (Control)component;
            //WebChart webChart = component as WebChart;
            //if (webChart == null)
            //    throw new Exception("Implementation: WebChartDesigner.Initialize() called with non-WebChart component");

            cd.OriginalDesigner = this;

#if __COMPILING_FOR_2_0_AND_ABOVE__
            MyWebChart.ChartBase.FreezeValuePath = true;
#endif
            MyWebChart.ChartDesigner = cd;

			cd.BeforeRaiseComponentChangedCalled += new EventHandler(OnBeforeRaiseComponentChangedCalled);
		
			IComponentChangeService service = (IComponentChangeService) GetService(typeof(IComponentChangeService));
			if (service != null) 
			{
                changedEventHandler = new ComponentChangedEventHandler(service_ComponentChanged);
                service.ComponentChanged += changedEventHandler;
			}

#if !__COMPILING_FOR_2_0_AND_ABOVE__
			IDesignerHost host = (IDesignerHost) GetService(typeof(IDesignerHost));
			if (host != null) 
			{
				host.LoadComplete += new EventHandler(service_LoadComplete);
			}
#endif
			if(cd.Chart.Series.SubSeries.Count == 0)
			{
				MemberDescriptor changingMember = TypeDescriptor.GetProperties(component)["Series"];
				RaiseComponentChanging(changingMember);
				cd.Chart.Series.SubSeries.Add(new Series("S0"));
				cd.Chart.Series.SubSeries.Add(new Series("S1"));
				RaiseComponentChanged(changingMember,null,null);
			}
		}

#if __COMPILING_FOR_2_0_AND_ABOVE__
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
		}
#else
		public override void OnSetComponentDefaults()
		{
			cd.Chart.Series.SubSeries.Add(new Series("S2"));
			cd.Chart.Series.SubSeries.Add(new Series("S3"));
		}
#endif

#if !__COMPILING_FOR_2_0_AND_ABOVE__
		void service_LoadComplete(object sender, EventArgs e) 
		{
			UpdateDesignTimeHtml();
		}
#endif
        private Chart MyWebChart
        {
            get
            {
                return m_webchart as Chart;
            }
        }

		void OnBeforeRaiseComponentChangedCalled(object sender, EventArgs e) 
		{
            if (MyWebChart.DataSource == null) 
			{
				DataSource = String.Empty;
			} 
			else 
			{
                IComponent comp = MyWebChart.DataSource as IComponent;
				if (comp == null)
					return;
				ISite site = comp.Site;
                string ds = (site != null ? site.Name : comp.ToString());
				if (!ds.Equals(DataSource))
					DataSource = ds;
			}
		}

#if __COMPILING_FOR_2_0_AND_ABOVE__
        protected override void OnDataSourceChanged(bool forceUpdateView)
        {
            Control webchart = MyWebChart;
            if (webchart != null)
            {
                ChartBase chart = MyWebChart.ChartBase;
                if (string.IsNullOrEmpty(base.DataSourceID))
                {
                    bool freezeValue = chart.FreezeValuePath;
                    chart.FreezeValuePath = false;
                    chart.DataSource = null;
                    chart.FreezeValuePath = freezeValue;
                }
            }

            base.OnDataSourceChanged(forceUpdateView);
        }

        protected override void OnSchemaRefreshed()
        {
            ChartBase chart = MyWebChart.ChartBase;

            bool freezeValue = chart.FreezeValuePath;
            chart.FreezeValuePath = false;
            chart.DataProvider.ResetValuePaths();
            chart.FreezeValuePath = freezeValue;
        }
#endif

        private void service_ComponentChanged(object sender, ComponentChangedEventArgs  e)
		{
#if __COMPILING_FOR_2_0_AND_ABOVE__
            try
            {
                Tag.SetDirty(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WebChartDesigner.service_ComponentChanged: SetDirty() failed; stack:");
                Debug.WriteLine(ex.StackTrace);
            }
#else
			this.IsDirty = true;
#endif
			if (e.Component /*!= null*/ == this.Component)
				UpdateDesignTimeHtml();
		}

        public override void OnComponentChanged(Object sender, ComponentChangedEventArgs ce)
        {
            base.OnComponentChanged(sender, ce);
        }


		public override string 
#if __COMPILING_FOR_2_0_AND_ABOVE__
            GetPersistenceContent()
#else
			GetPersistInnerHtml()
#endif
		{
            if (MyWebChart != null)
                MyWebChart.InSerialization = true;
			
			string s = null;
#if __COMPILING_FOR_2_0_AND_ABOVE__
            s = base.GetPersistenceContent();
#else
			s = base.GetPersistInnerHtml();
#endif

            if (MyWebChart != null)
                MyWebChart.InSerialization = false;
            return s;
		}


		protected override void Dispose(bool disposing)
		{
            IComponentChangeService service = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (service != null)
            {
                service.ComponentChanged -= changedEventHandler;
            }
            
            base.Dispose(disposing);

			RemoveOldImage(disposing);

            if (MyWebChart != null)
			{
                ChartBase chart = MyWebChart.ChartBase;
                if(chart != null)
                {
                chart.Dispose();
                MyWebChart.ChartBase = null;
                }
			}
			
			cd.Dispose();
		}

#if __COMPILING_FOR_2_0_AND_ABOVE__

        private IDataSource GetDataSource()
        {
            Control webchart = MyWebChart;

            string dataSourceID = base.DataSourceID;
            if (!string.IsNullOrEmpty(dataSourceID))
            {
                //Control dataSourceControl = FindControl(base.Component.Site, (Control)base.Component, dataSourceID);
                Control dataSourceControl = webchart.FindControl(dataSourceID);
                if (dataSourceControl != null)
                {
                    return (dataSourceControl as IDataSource);
                }
            }
            return null;
        }

        private void OnDataSourceViewSelectCallback(IEnumerable data)
        {
            m_realData = data;
        }



       protected override IEnumerable GetDesignTimeDataSource()
        {
            IDataSource ids = GetDataSource();
            if (ids == null)
                return null;

            // fixme: what-what???
            DataSourceView dsv = ids.GetView("");

            //Access DataSources are usually created with a relative path to the Access file.
            //However .NET framework does not allow you to execute an SQL query on files with relative paths at design time.
            //So for design time behaviour to work, we replace such paths at design time with an absolute path to the file in order to get good design time behaviour.
            AccessDataSource ads = null;
            String AdsFilePath = null;
            Control webchart = MyWebChart;

            if (ids is AccessDataSource)
            {
                ads = (AccessDataSource)ids;
                if (ads.DataFile.StartsWith("~"))
                {
                    IWebApplication app = (IWebApplication)webchart.Page.Site.GetService(typeof(IWebApplication));
                    if (app != null)
                    {
                        AdsFilePath = String.Copy(ads.DataFile);
                        String rootPath = app.RootProjectItem.PhysicalPath;
                        ads.DataFile = rootPath + ads.DataFile.Substring(1);

                    }
                }
            }

            if (ids is System.Web.UI.WebControls.ObjectDataSource)
            {
                string selectMethod = ((System.Web.UI.WebControls.ObjectDataSource)ids).SelectMethod;
                if (selectMethod == null || selectMethod == "")
                {
                    return null;
                }
            }

           // not everything is supported in design time.
           // selection is not always supported in AccessDataSource
            try
            {
                dsv.Select(DataSourceSelectArguments.Empty, new DataSourceViewSelectCallback(this.OnDataSourceViewSelectCallback));
            }
            catch (System.NotSupportedException)
            {
                MyWebChart.ChartBase.DataSource = null;
                return null;
            }
            catch (System.Reflection.TargetInvocationException)
            {
                MyWebChart.ChartBase.DataSource = null;
                return null;
            }

            //restore original DataPath in order for it to be serialized to the page properly.
            if (ads != null && AdsFilePath != null)
                ads.DataFile = AdsFilePath;
            
            return m_realData;

        }

        protected override IEnumerable GetSampleDataSource()
        {
            object obj = MyWebChart.DataSourceID;
            if (obj == null || !(obj is string) || ((string)obj).Length == 0)
            {
                return null;
            }
            IEnumerable zz = base.GetSampleDataSource();

            return zz;
        }

        protected override void DataBind(System.Web.UI.WebControls.BaseDataBoundControl dataBoundControl)
        {
            base.DataBind(dataBoundControl);
        }
#endif


        public override string GetDesignTimeHtml() 
		{
            //IChart wc = MyWebChart;
            ChartBase chart = MyWebChart.ChartBase;

			if (chart == null) 
				return "";

			chart.SetDesignMode(true);

            object wObj = MyWebChart.Width.Value;
            object hObj = MyWebChart.Height.Value;
            int w = 0;
            int h = 0;
            try
            {
                if (wObj != null)
                    w = (int) (double)wObj;
                if (hObj != null)
                    h = (int) (double)hObj;
            }
            catch { }

			if (w==0 ||	h==0) 
				return "";

#if __COMPILING_FOR_2_0_AND_ABOVE__
            if (Tag != null)
                Tag.SetDirty(true);
#else
				IsDirty = true;
#endif

#if !__COMPILING_FOR_2_0_AND_ABOVE__
			if (DataSource != string.Empty) 
			{
                bool freezeValue = chart.FreezeValuePath;//wc.Chart.FreezeValuePath;
				chart.FreezeValuePath = true;
                if (MyWebChart.DataSource == null)
				{
					IComponent comp = Component.Site.Container.Components[DataSource];
					
					if (comp == null) 
					{
						IDesignerHost host = (IDesignerHost) GetService(typeof(IDesignerHost));
						if (host != null) 
						{
							if (host.Loading) 
							{
								chart.FreezeValuePath = freezeValue;
								return "";
							}
						}
					}

                    MyWebChart.DataSource = comp;
				}
                (Component as IChart).DataBind();
				//wc.DataBind();
                MyWebChart.ChartBase.FreezeValuePath =  freezeValue;
			}

#endif

            RemoveOldImage(false);

            string imagePath = Imgpath;
            MyWebChart.DesignSavePath = imagePath;
            chart.Invalidate();
            firstLoad = false;

            Color bgColor = MyWebChart.BackColor;

            if (Path.GetExtension(imagePath) == ".bmp" && bgColor == Color.Transparent)
                MyWebChart.BackColor = Color.White;
                
            string text = base.GetDesignTimeHtml();

            MyWebChart.BackColor = bgColor;
            
            return text;
		}


#if __COMPILING_FOR_2_0_AND_ABOVE__
		private IDataSourceViewSchema GetDataSourceSchema()
        {
            DesignerDataSourceView view1 = base.DesignerView;
            
            if (view1 != null)
            {
                return view1.Schema;
            }
            return null;
        }
#endif 

		void RemoveOldImage(bool dispose)
		{
			if (m_toBeRemoved != null && File.Exists(m_toBeRemoved))
			{
				File.Delete(m_toBeRemoved);
				m_toBeRemoved = null;
			}
			if (imgPath != null && File.Exists(imgPath))
			{
				if (!dispose)
					m_toBeRemoved = imgPath;
				else
					File.Delete(imgPath);
			}
			imgPath = null;
		}

        string Imgpath 
		{
			get 
			{
				if (imgPath == null)
				{
					Guid guid = Guid.NewGuid();
                    string ext = (firstLoad ? "bmp" : "png");
                    string tempFile = tempfilenametemplate + "-" + guid.ToString() + "." + ext;
					imgPath = Path.Combine(Path.GetTempPath(), tempFile);
				}
				return imgPath;
			}
		}

		string Html 
		{
			get 
			{
				return "<img src=\"" + Imgpath + "\">";
			}
		}

		protected override string GetErrorDesignTimeHtml(Exception e) 
		{
			return "Exception: " + e.Message + "<br>" + e.StackTrace;
		}
	}
}
