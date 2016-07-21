using System;
using System.Windows.Forms;
using System.Collections;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WinChartControlDesigner.
	/// </summary>
	internal class WinChartControlDesigner  : System.Windows.Forms.Design.ControlDesigner
	{
		internal ChartDesigner cd = new ChartDesigner();

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
                dalc.AddRange(base.ActionLists);
                dalc.AddRange(cd.ActionLists);
                WinChartChooseDataSourceActionList act = new WinChartChooseDataSourceActionList(this);
                act.AutoShow = true;
                dalc.Insert(1, act);
                return dalc;
            }
        }


        public object DataSource
        {
            get
            {
                return ((WinChart)base.Component).DataSource;
            }
            set
            {
                ((WinChart)base.Component).DataSource = value;
            }
        }
#endif


		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			cd.Initialize(component);

			PropertyInfo pi = typeof(WinChart).GetProperty("ChartDesigner", BindingFlags.NonPublic | BindingFlags.Instance);
			pi.SetValue(component, cd, null);
			
			IComponentChangeService service = (IComponentChangeService) GetService(typeof(IComponentChangeService));
			if (service != null) 
			{
				service.ComponentChanged += new ComponentChangedEventHandler(service_ComponentChanged);
			}
		}

		private void service_ComponentChanged(object sender, ComponentChangedEventArgs  e)
		{			
			if (Control != null)
				(Control as WinChart).Invalidate(/*e*/);
		}

    }

#if __COMPILING_FOR_2_0_AND_ABOVE__
        [ComplexBindingProperties("DataSource", "DataMember")]
        internal class WinChartChooseDataSourceActionList : DesignerActionList
        {
            private WinChartControlDesigner owner;

            public WinChartChooseDataSourceActionList(WinChartControlDesigner owner)
                : base(owner.Component)
            {
                this.owner = owner;
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection daic = new DesignerActionItemCollection();
                DesignerActionPropertyItem dapi = new DesignerActionPropertyItem("DataSource", "Choose Data Source");
                dapi.RelatedComponent = this.owner.Component;
                daic.Add(dapi);
                return daic;
            }
 

            [AttributeProvider(typeof(IListSource))]
            public object DataSource
            {
                get
                {
                    return this.owner.DataSource;
                }
                set
                {
                    this.owner.cd.SetProperty("DataSource", value);
                }
            }
        }


#endif

}
