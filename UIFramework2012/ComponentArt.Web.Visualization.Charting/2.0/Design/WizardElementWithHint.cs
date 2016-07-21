using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Threading;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class WizardElementWithHint : WizardElement
	{
		private System.ComponentModel.Container components = null;
		ResourceManager m_rm;
		CultureInfo m_ci;

		public WizardElementWithHint(WinChart chart) : base(chart)
		{
			InitializeComponent();
		}

		public WizardElementWithHint() : this(null) { }

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
			components = new System.ComponentModel.Container();
		}
		#endregion


		protected override string HintMessage() 
		{
			return "";
		}


		Hashtable m_table = new Hashtable();

		internal Hashtable HintTable 
		{
			get {return m_table;}
		}


		public void RegisterHint(Control c, string hint) 
		{
			RegisterHint(c, hint, null);
		}

		public void RegisterHint(Control c, string hint, Control parentHintControl) 
		{
			if (m_table[c] == null) 
			{
				if (parentHintControl == null)
					m_table.Add(c, hint);
				else 
					m_table.Add(c, parentHintControl);
				c.MouseEnter += new EventHandler(GotFocusHandler);
				c.MouseLeave += new EventHandler(LostFocusHandler);

				Control newParentHintControl = (parentHintControl == null ? c : parentHintControl);

				foreach (Control child in c.Controls) 
				{
					if (!c.GetType().IsSubclassOf(typeof(WizardElementWithHint)))
						RegisterHint(child, hint, newParentHintControl);
					else
						((WizardElementWithHint)c).RegisterHint(child, hint);
				}
			}
		}

		private void GotFocusHandler(object sender, EventArgs e)
		{
			Control HintControl = (Control)(m_table[sender] is string ? sender : m_table[sender]);

			Hint = (string)m_table[HintControl];

			Control parentTitleControl = null;

			if (HintControl is GroupBox || HintControl is CheckBox)
				parentTitleControl = HintControl;

            
			if (parentTitleControl == null)
				parentTitleControl = ComponentArt.Web.Visualization.Charting.Design.Wizard.GetParentControlOfType(HintControl, typeof(GroupBox)) as GroupBox;

			if (parentTitleControl != null)
				HintTitle = parentTitleControl.Text.TrimEnd(new char[] {':'});
			else
                HintTitle = DefaultHintTitle;
		}


		private void LostFocusHandler(object sender, EventArgs e)
		{
			Hint = DefaultHint;
			HintTitle = DefaultHintTitle;
		}

		protected virtual bool IsTopLevelNaming() 
		{
			return true;
		}

		protected override void OnVisibleChanged(EventArgs e) 
		{
			base.OnVisibleChanged(e);

			if (Visible) 
			{
				Hint = DefaultHint;
				HintTitle = DefaultHintTitle;
			}
		}

		protected override void OnCreateControl() 
		{
			base.OnCreateControl();

			if (WinChart == null)
				return;

			// Create a resource manager to retrieve resources.
            m_rm = CommonFunctions.GetComonResourceManager();

			// Get the culture of the currently executing thread.
			// The value of ci will determine the culture of
			// the resources that the resource manager retrieves.
			m_ci = Thread.CurrentThread.CurrentCulture;

			//RegisterHintsThroughControls(this);
			RegisterHintsThroughFields();

			m_rm.ReleaseAllResources();
		
			Hint = DefaultHint;
			HintTitle = DefaultHintTitle;
		}

		void RegisterHintsThroughFields()
		{
			Type parentType = GetType();

			FieldInfo [] fields;

			ArrayList fal = new ArrayList();

			do 
			{
				fields = parentType
					.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
				parentType = parentType.BaseType;
				foreach (FieldInfo field in fields) 
				{
					if (field.FieldType.IsSubclassOf(typeof(Control)))
					{
						fal.Add(field);
					}
				}
			} while (parentType != null && parentType.GetType().IsSubclassOf(typeof(WizardElementWithHint)));
            
			foreach (FieldInfo field in fal) 
			{
				Attribute att = 
					Attribute.GetCustomAttribute(field, typeof(WizardHintAttribute));
				
				if (att == null) 
					continue;
				
				WizardHintAttribute wha = (WizardHintAttribute)att;

				string hint = null;
					
				if (wha.Key != "" && wha.Key != null)
				{
					hint = m_rm.GetString(wha.Key, m_ci);
				}
				else if (wha.MemberInfo != null) 
				{
					
					Attribute datt = null;
					try 
					{
						datt = Attribute.GetCustomAttribute(wha.MemberInfo, typeof(DescriptionAttribute), false);
					} 
					catch 
					{
                        Attribute [] atts = Attribute.GetCustomAttributes(wha.MemberInfo);
					}

					if (datt != null) 
					{
						hint = ((DescriptionAttribute)datt).Description;
					}
				}

				if (hint != null) 
				{
					string hintWithReturns = hint.Replace("\\r\\n", "\r\n");
					RegisterHint((Control)field.GetValue(this), hintWithReturns);
				}
			}
		}
	}
}
