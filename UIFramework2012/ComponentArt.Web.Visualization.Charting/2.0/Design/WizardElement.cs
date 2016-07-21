using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Resources;
using ComponentArt.Win.UI.Internal;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	[Designer(typeof(WizardElementDesigner))]
	internal class WizardElement : ComponentArt.Win.UI.Internal.UserControl
	{
		private System.ComponentModel.Container components = null;

		string m_defaultHintTitle = "";
		string m_defaultHint = "";

		protected WinChart m_winchart = null;

		Wizard m_wizard = null;

		public WizardElement(WinChart chart)
		{
			m_winchart = chart;
			InitializeComponent();
		}

		public WizardElement() : this(null) { }

		protected override void OnResize(EventArgs e) 
		{
			base.OnResize(e);
			if (Width == 0 || Height == 0)
				return;
		}

		internal WinChart WinChart 
		{
			get 
			{
				return m_winchart;
			}
		}
		
		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (Wizard != null)
				m_winchart = Wizard.WinChart;
		}

		internal Wizard Wizard
		{
			get 
			{
				if (m_wizard != null) 
					return m_wizard;

				// Find the winchart
				Control ctrl = this;

				while (ctrl.Parent != null && ctrl.Parent != ctrl) 
				{
					ctrl = ctrl.Parent;
					if (ctrl is Wizard) 
					{
						return (Wizard)ctrl;
					}
				}
				return null;
			}
		}

		protected virtual string HintMessage() 
		{
			return null;
		}

		[DefaultValue("")]
		public string DefaultHintTitle 
		{
			get 
			{
				if (m_defaultHintTitle != "")
					return m_defaultHintTitle;
				WizardElement we = Wizard.GetParentControlOfType(this, typeof(WizardElement)) as WizardElement;
				if (we == null)
					return "";
				else
					return m_defaultHintTitle = we.DefaultHintTitle;
			}
			set
			{
				m_defaultHintTitle = value;
			}
		}

		[DefaultValue("")]
		public string DefaultHint
		{
			get 
			{
				if (m_defaultHint != "")
					return m_defaultHint;
				WizardElement we = Wizard.GetParentControlOfType(this, typeof(WizardElement)) as WizardElement;
				if (we == null)
					return "";
				else
					return m_defaultHint = we.DefaultHint;
			}
			set
			{
				m_defaultHint = value;
			}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string HintTitle
		{
			get 
			{
				if (Wizard == null)
					return null;
				else
					return Wizard.HintTitle;
			}

			set 
			{
				if (Wizard != null && value != "")
					Wizard.HintTitle = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string Hint 
		{
			get 
			{
				if (Wizard == null)
					return null;
				else
					return Wizard.Hint;
			}

			set 
			{
				if (Wizard != null && value != "")
					Wizard.Hint = value;
			}
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
			// 
			// WizardElement
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Name = "WizardElement";

		}
		#endregion
	}

	internal class WizardElementDesigner : System.Windows.Forms.Design.ControlDesigner 
	{
		public override System.ComponentModel.Design.DesignerVerbCollection Verbs 
		{
			get 
			{
				return new System.ComponentModel.Design.DesignerVerbCollection(new System.ComponentModel.Design.DesignerVerb [] 
					{
						new System.ComponentModel.Design.DesignerVerb("Test", new EventHandler(ShowThisDialog)), 
				});
			}
		}

		void ShowThisDialog(object sender, EventArgs e) 
		{
			WizardElement we = (WizardElement)Activator.CreateInstance(Component.GetType());

			Form form = new Form();
			form.ClientSize = we.Size;
			form.Controls.AddRange(new System.Windows.Forms.Control[] {we});
			form.Name = "zzzz";

			form.ShowDialog();
		}
	}
}
