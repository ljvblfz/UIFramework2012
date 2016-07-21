using System;
using System.ComponentModel;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{

	/// <summary>
	/// Defines single element in <see cref="MultiLineStyle"/> object.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class MultiLineStyleItem
	{
		LineStyle m_lineStyle = null;
		string m_lineStyleName = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLineStyleItem"/> class.
		/// </summary>
		public MultiLineStyleItem() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLineStyleItem"/> class with the specified name.
		/// </summary>
		/// <param name="lineStyleName">The name of this <see cref="MultiLineStyleItem"/> object.</param>
		public MultiLineStyleItem(string lineStyleName) 
		{
			m_lineStyleName = lineStyleName;
		}

		internal LineStyle LineStyle 
		{
            get { return m_lineStyle; }
            set { m_lineStyle = value; } 
		}

 		/// <summary>
		/// Gets or sets the name of this <see cref="MultiLineStyleItem"/> object.
		/// </summary>
		[SRDescription("MultiLineStyleItemLineStyleNameDescr")]
		[ TypeConverter(typeof(SelectedLineStyleConverter))]
		public string LineStyleName 
		{
			get {return m_lineStyleName;}
			set {m_lineStyleName = value;}
		}

		internal void SetContext(Object obj) 
		{
			if(obj is LineStyle)
				m_lineStyle = (LineStyle)obj;
			if (m_lineStyleName != "" || obj == null)
				return;
            
			m_lineStyleName = SelectedNameConverter.GetNames(LineStyle.OwningChart, GetType().GetProperty("LineStyleName"))[0].ToString();
		}
	}
}
