using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	[Designer(typeof(NonResizableControlDesigner))]
    internal class WizardDialog : WizardElementWithHint
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardDialog(WinChart chart) : base(chart)
		{
			InitializeComponent();
		}

		public WizardDialog() : this(null) { }

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

        internal void InvokeEditor(object obj, PropertyInfo pi)
        {
            InvokeEditor(obj, pi, false);
        }

        internal void InvokeEditor(object obj, PropertyInfo pi, bool useOriginalComponent)
        {
            InvokeEditor(obj, pi, null, useOriginalComponent);
        }

        internal void InvokeEditor(object obj, PropertyInfo pi, object sender, bool useOriginalComponent) 
		{
            InvokeEditor(obj, pi, pi.GetValue(obj, null), sender, useOriginalComponent);
		}

        internal void InvokeEditor(object obj, PropertyInfo pi, object value, object sender, bool useOriginalComponent)
        {
            this.InvokeEditor(obj, pi, value, sender, useOriginalComponent, new IWindowsFormsEditorServiceImpl(this));
        }

		internal void InvokeEditor(object obj, PropertyInfo pi, object value, object sender, bool useOriginalComponent, IServiceProvider isp)
		{
			UITypeEditor editor = null;
			if (obj != null) 
			{
				PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj.GetType());
				PropertyDescriptor pd = pdc[(pi.Name)];
            
				if (isp is IWindowsFormsEditorServiceImpl)
				{
					((IWindowsFormsEditorServiceImpl)isp).PropertyDescriptor = pd;
				}
            

				editor = (UITypeEditor)pd.GetEditor(typeof(UITypeEditor));
			}

            if (editor == null)
            {
                editor = (UITypeEditor)TypeDescriptor.GetEditor(value.GetType(), typeof(UITypeEditor));
            }
			if (editor == null) 
			{
				EditorAttribute ea = (EditorAttribute)Attribute.GetCustomAttribute(pi, typeof(EditorAttribute));
				editor = (UITypeEditor)Activator.CreateInstance(Type.GetType(ea.EditorTypeName));
			}

			object o = null;
            o = editor.EditValue(new ITypeDescriptorContextImpl(this, useOriginalComponent), isp, value);

			if (pi != null && obj != null) 
			{
				if (o != value)
				{
					pi.SetValue(obj, o, null);
				}
			}
		}

		internal void InvokeEditor(object value) 
		{
			InvokeEditor(null, null, value, null, false);
		}

        internal class DataSourceControlDropDown : IWindowsFormsEditorServiceImpl
        {

            Point m_loc;
            Control m_ctrl;

            public DataSourceControlDropDown(WizardDialog wd, Point loc)
                : base(wd)
            {
                m_loc = loc;
            }

            public override void ShowControl(Control control)
            {
                control.Location = new Point(m_loc.X - control.Width, m_loc.Y);
                m_ctrl = control;
                WizardDialog.Controls.Add(control);
            }

            public override void CloseControl()
            {
                WizardDialog.Controls.Remove(m_ctrl);                
            }

        }

        internal class IWindowsFormsEditorServiceImpl : IWindowsFormsEditorService, IServiceProvider
        {
            #region IWindowsFormsEditorService Members

            private DropDownHolder m_dropDownHolder;
            private WizardDialog m_wd;

            internal WizardDialog WizardDialog
            {
                get
                {
                    return m_wd;
                }
            }


            PropertyDescriptor m_pd;

            internal PropertyDescriptor PropertyDescriptor
            {
                get
                {
                    return m_pd;
                }
                set
                {
                    m_pd = value;
                }
            }

            public IWindowsFormsEditorServiceImpl(WizardDialog wd)
            {
                m_wd = wd;
            }

            public virtual void CloseControl()
            {
                this.m_dropDownHolder.SetComponent(null);
                this.m_dropDownHolder.Refresh();
                this.m_dropDownHolder.Visible = false;
            }

            void IWindowsFormsEditorService.CloseDropDown()
            {
                CloseControl();
            }

            public virtual void ShowControl(Control control)
            {
                if (this.m_dropDownHolder == null)
                {
                    this.m_dropDownHolder = new DropDownHolder(this);
                }

                this.m_dropDownHolder.Visible = false;
                this.m_dropDownHolder.SetComponent(control);
                if (m_pd != null)
                    this.m_dropDownHolder.Text = m_pd.DisplayName;
                this.m_dropDownHolder.ShowDialog();
                this.m_dropDownHolder.DoModalLoop();
            }

            void IWindowsFormsEditorService.DropDownControl(Control control)
            {
                ShowControl(control);
            }

            DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
            {
                return dialog.ShowDialog();
            }

            #endregion

            #region IServiceProvider Members

            object IServiceProvider.GetService(Type serviceType)
            {
                if (serviceType == typeof(IWindowsFormsEditorService))
                {
                    return this;
                }
                object h = m_wd.Wizard.Component.Site.GetService(serviceType);
                return h;
            }

            #endregion
}

		internal class ITypeDescriptorContextImpl : ITypeDescriptorContext
		{
			WizardDialog m_wd;
			bool m_useOriginalComponent;

            public ITypeDescriptorContextImpl(WizardDialog wd)
                : this(wd, false)
			{
			}

			public ITypeDescriptorContextImpl(WizardDialog wd, bool useOriginalComponent)
			{
                m_wd = wd;
                m_useOriginalComponent = useOriginalComponent;
            }

			public void OnComponentChanged() 
			{
			}

			public bool OnComponentChanging()
			{
				return true;
			}

			public object GetService ( System.Type serviceType )
			{
                if (m_useOriginalComponent)
                {
                    return m_wd.Wizard.Component.Site.GetService(serviceType);
                }
				return this.m_wd.GetService(serviceType);
			}

			public IContainer Container 
			{
				get 
				{
					if (m_useOriginalComponent) 
					{
						IContainer containter = m_wd.Wizard.Component.Site.Container;
						return containter;
					}

					return null;
				}
			}

			public object Instance 
			{
				get 
				{
					return (m_useOriginalComponent ? m_wd.Wizard.Component : m_wd.WinChart);
				}
			}
		
			public PropertyDescriptor PropertyDescriptor 
			{
				get
				{
					ITypeDescriptorContext service = 
						(ITypeDescriptorContext)m_wd.Site.GetService(typeof(ITypeDescriptorContext));

					if (service != null)
						return service.PropertyDescriptor;
					return null;
				}
			}
		}

		protected override Size DefaultSize 
		{
			get {return new System.Drawing.Size(424, 256);}
		}
		
		protected override void OnCreateControl() 
		{
			base.OnCreateControl();
			if(m_winchart == null)
				m_winchart = Wizard.GetWinChart(this);
		}

		#region Component Designer generated code
		private void InitializeComponent()
		{
			// 
			// WizardDialog
			// 
			this.Name = "WizardDialog";
			this.Size = new System.Drawing.Size(424, 256);

		}
		#endregion
	}

    internal class DropDownHolder : Form
    {
        internal Control m_currentControl;
        internal IWindowsFormsEditorService m_wd;

        public DropDownHolder(IWindowsFormsEditorService wd)
        {
            m_wd = wd;
            this.m_currentControl = null;
            this.Text = "";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            base.Visible = false;
        }


        public void DoModalLoop()
        {
            Application.DoEvents();
        }

        public virtual void SetComponent(Control ctl)
        {
            if (this.m_currentControl != null)
            {
                base.Controls.Remove(this.m_currentControl);
                m_currentControl = null;
            }
            if (ctl != null)
            {
                base.Controls.Add(ctl);
                base.ClientSize = ctl.Size;
                ctl.Location = new Point(0, 0);
                ctl.Visible = true;
                this.m_currentControl = ctl;
                Point p = Cursor.Position;
                this.Location = new Point(p.X - ctl.Width, p.Y);
                FitToScreen(this);
                this.StartPosition = FormStartPosition.Manual;
            }
            base.Enabled = this.m_currentControl != null;
        }

        static void FitToScreen(Control ctl)
        {
            if (ctl.Left < Screen.PrimaryScreen.Bounds.X)
                ctl.Left = Screen.PrimaryScreen.Bounds.X;

            if (ctl.Top < Screen.PrimaryScreen.Bounds.Y)
                ctl.Top = Screen.PrimaryScreen.Bounds.Y;

            if (ctl.Left + ctl.Width >= Screen.PrimaryScreen.Bounds.Width)
                ctl.Left = Screen.PrimaryScreen.Bounds.Width - ctl.Width;

            if (ctl.Top + ctl.Height >= Screen.PrimaryScreen.Bounds.Height)
                ctl.Top = Screen.PrimaryScreen.Bounds.Height - ctl.Height;
        }

        private void OnCurrentControlResize(object o, EventArgs e)
        {
            if (this.m_currentControl != null)
            {
                int num1 = base.Width;
                base.Size = new Size(2 + this.m_currentControl.Width, 2 + this.m_currentControl.Height);
                base.Left -= (base.Width - num1);
            }
        }

        protected override void OnMouseDown(MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                this.m_wd.CloseDropDown();
            }
            base.OnMouseDown(me);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x10)
            {
                if (base.Visible)
                {
                    this.m_wd.CloseDropDown();
                }
                return;
            }
            base.WndProc(ref m);
        }
    }
}
