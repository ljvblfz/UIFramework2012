#if DEBUG
using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.ComponentModel;


#if !__BUILDING_ComponentArt_Win_UI_Internal__
namespace ComponentArt.Win.UI.Internal
#else
namespace ComponentArt.Win.UI.WinChartSamples
#endif
{
	internal class TabControlDesigner : ParentControlDesigner
	{
		protected override bool GetHitTest(Point point) 
		{
			return (Control.PointToClient(point).Y < 
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				.HeaderHeight 
				&& Control.PointToClient(point).X <= ((
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				)Control).LastButtonX);
		}


		public override bool CanParent(ControlDesigner controlDesigner)
		{
			return true;
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				return new DesignerVerbCollection(new DesignerVerb [] 
					{
						new DesignerVerb("Add Tab", new EventHandler(TabAdd)), 
						new DesignerVerb("Remove Tab", new EventHandler(TabRemove))
					});
			}
		}

		void TabAdd(object sender, EventArgs e) 
		{

#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
			ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
			tabControl = (
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
				ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
				) base.Component;
			MemberDescriptor controlsDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
            IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
            if (designerHost != null)
			{
				DesignerTransaction dt = null;
				try
				{
					try
					{
                        dt = designerHost.CreateTransaction("WizardTabControlAddTab");
                        base.RaiseComponentChanging(controlsDescriptor);
					}
					catch (CheckoutException exception)
					{
						if (exception != CheckoutException.Canceled)
						{
							throw exception;
						}
						return;
					}
                    Panel newTabPage = (Panel)designerHost.CreateComponent(typeof(
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabPage
#else
						ComponentArt.Win.UI.WinChartSamples.TabPage
#endif
						));
					string name = null;
                    PropertyDescriptor nameDescriptor = TypeDescriptor.GetProperties(newTabPage)["Name"];
                    if ((nameDescriptor != null) && (nameDescriptor.PropertyType == typeof(string)))
					{
                        name = (string)nameDescriptor.GetValue(newTabPage);
					}
                    if (name != null)
					{
                        newTabPage.Text = name;
					}

                    tabControl.AddTab(newTabPage);


                    base.RaiseComponentChanged(controlsDescriptor, null, null);
				}
				finally
				{
                    if (dt != null)
					{
                        dt.Commit();
					}
				}
			}
		}

		void TabRemove(object sender, EventArgs e) 
		{
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
			tabControl = (
#if !__BUILDING_ComponentArt_Win_UI_Internal__
ComponentArt.Win.UI.Internal.TabControl
#else
	ComponentArt.Win.UI.WinChartSamples.TabControl
#endif
	) base.Component;
            if ((tabControl != null) && (tabControl.Controls.Count != 0))
			{
                MemberDescriptor controlsDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
                Panel tabPage = tabControl.SelectedTab;
				IDesignerHost designerHost = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                if (designerHost != null)
				{
					DesignerTransaction dt = null;
					try
					{
						try
						{
                            dt = designerHost.CreateTransaction("TabControlRemoveTab");
                            base.RaiseComponentChanging(controlsDescriptor);
						}
						catch (CheckoutException exception)
						{
                            if (exception != CheckoutException.Canceled)
							{
                                throw exception;
							}
							return;
						}
                        designerHost.DestroyComponent(tabPage);
                        base.RaiseComponentChanged(controlsDescriptor, null, null);
					}
					finally
					{
                        if (dt != null)
						{
                            dt.Commit();
						}
					}
				}
			}
		}

		protected override bool EnableDragRect
		{
			get
			{
				return false;
			}
		}
 
		public override bool CanParent(Control control)
		{
			return (true);
		}
	}
}
#endif