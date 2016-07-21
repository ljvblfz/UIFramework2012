using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents a multiline style.
	/// </summary>
		
	public class MultiLineStyle : LineStyle
	{
		MultiLineStyleCollection m_lineStyles; 

		internal MultiLineStyle(string name, string[] lineStyles) : this(name) 
		{
			m_lineStyles.Clear();
			foreach (string lineStyleName in lineStyles) 
			{
				MultiLineStyleItem item = new MultiLineStyleItem(lineStyleName);
				item.LineStyle = this;
				m_lineStyles.Add(item);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLineStyle"/> class with specified name.
		/// </summary>
		/// <param name="name">Name of the new <see cref="MultiLineStyle"/> object.</param>
		public MultiLineStyle(string name) :base(name)
		{
			m_lineStyles = new MultiLineStyleCollection(this);
			MultiLineStyleItem item = new MultiLineStyleItem("Default");
			item.LineStyle = this;
			m_lineStyles.Add(item);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLineStyle"/> class with default parameters.
		/// </summary>
		public MultiLineStyle() : this(null) {}
		

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override double Width 
		{
			get {return 0;}
			set {}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override double Height 
		{
			get {return 0;}
			set {}
		}


		/// <summary>
		/// Gets the collection of <see cref="MultiLineStyleItem"/>s that belong to this <see cref="MultiLineStyle"/> object.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[SRDescription("MultiLineStyleLineStylesDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public MultiLineStyleCollection LineStyles 
		{
			get {return m_lineStyles;}
		}

		#region --- XML Serialization ---
		
		internal override void Serialize(XmlCustomSerializer S)
		{
			base.Serialize(S);
			if(S.Reading)
			{
				LineStyles.Clear();
				if(S.GoToFirstChild("SubLineStyle"))
				{
					MultiLineStyleItem E = new MultiLineStyleItem();
					S.AttributeProperty(E,"LineStyleName");
					LineStyles.Add(E);
					while(S.GoToNext("SubLineStyle"))
					{
						E = new MultiLineStyleItem();
						S.AttributeProperty(E,"LineStyleName");
						LineStyles.Add(E);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(MultiLineStyleItem LS in LineStyles)
				{
					if(S.BeginTag("SubLineStyle"))
					{
						S.AttributeProperty(LS,"LineStyleName");
						S.EndTag();
					}
				}
			}
		}
		#endregion
	}

}
