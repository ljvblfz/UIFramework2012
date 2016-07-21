using System;
using System.Windows.Forms;
using System.ComponentModel;

using System.Reflection;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class SelectedNameComboBox : ComponentArt.Win.UI.Internal.ComboBox 
	{

		string m_noneString = "";

		bool m_editable = false;
		string m_editString = null;

		int m_lastNonEditIndex = -1;

		object m_editObject = null;

		System.Reflection.PropertyInfo m_pi;

	
		public SelectedNameComboBox() 
		{
			DropDownStyle = ComboBoxStyle.DropDownList;
		}
	
		internal bool IsEditSelected 
		{
			get 
			{
				return (m_editable && SelectedIndex == Items.Count - 1);
			}
		}
	
		protected override void OnSelectedIndexChanged(System.EventArgs e) 
		{
			if (!m_editable || m_editObject == null)
				return;

			if (IsEditSelected) 
			{
				WizardDialog wd = (WizardDialog)Wizard.GetParentControlOfType(this, typeof(WizardDialog));
				wd.InvokeEditor(m_editObject);

				SelectedIndex = m_lastNonEditIndex;
				SetProperty(m_pi, m_editString);
			}

			if (m_lastNonEditIndex != SelectedIndex) 
			{
				base.OnSelectedIndexChanged(e);
				m_lastNonEditIndex = SelectedIndex;
			}
		}
		

		protected override void OnDropDownStyleChanged ( System.EventArgs e )
		{
			base.OnDropDownStyleChanged(e);

			if (DropDownStyle != ComboBoxStyle.DropDownList)
				DropDownStyle = ComboBoxStyle.DropDownList;
		}



		ChartBase m_chart;

		public void SetProperty (System.Reflection.PropertyInfo pi)
		{
			SetProperty(pi, null);
		}

		public void SetProperty (System.Reflection.PropertyInfo pi, string editString/*, object editObject*/)
		{
			m_pi = pi;

			Control we = this;
			while (!(we is WizardElement)) 
			{
				we = we.Parent;
			}
			WinChart wc = ((WizardElement)we).WinChart;
            
			if (wc == null)
				return;

			m_chart = wc.Chart;
			SetupItems(editString);
		}
        

		void SetupItems(string editString) 
		{
			if (m_chart == null || m_pi == null)
				return;


            m_editObject = SelectedNameConverter.GetSelectedNameConverter(m_pi).GetCollection(m_chart);
			m_editString = editString;
			m_editable = (m_editString != null || m_editString != "");

			string old_text = Text;

			Items.Clear();

			if (NoneString != "")
				Items.Add(NoneString);

            Items.AddRange(SelectedNameConverter.GetNames(m_chart, m_pi).ToArray());

			if (m_editable)
				Items.Add(editString);

			Text = old_text;
		}


		[DefaultValue("")]
		public string NoneString 
		{
			get {return m_noneString;}
			set {m_noneString = value;}
		}


		protected override void OnVisibleChanged ( System.EventArgs e ) 
		{
			base.OnVisibleChanged(e);

			if (Visible) 
			{
				SetupItems(m_editString);
			}
		}

	}
}
