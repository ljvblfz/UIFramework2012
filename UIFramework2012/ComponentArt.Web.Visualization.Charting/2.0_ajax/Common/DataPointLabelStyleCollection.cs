using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="DataPointLabelStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class DataPointLabelStyleCollection : NamedCollection
	{
		internal DataPointLabelStyleCollection(Object owner,bool initialize) : base(typeof(DataPointLabelStyle),owner)
		{ 
			if(initialize)
				InitializeContent();
		}

		internal DataPointLabelStyleCollection() { }

		/// <summary>
		/// Indicates the <see cref="DataPointLabelStyle"/> at the specified indexed location in the <see cref="DataPointLabelStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="DataPointLabelStyle"/> from the <see cref="DataPointLabelStyleCollection"/> object.</param>
		public new DataPointLabelStyle this[object index]
		{ 
			get 
			{ 
				if(index is DataPointLabelStyleKind)
				{
					index = ((DataPointLabelStyleKind)index).ToString();
				}
				return ((DataPointLabelStyle)base[index]); 
			} 
			set { base[index] = value; } 
		}

		private void InitializeContent()
		{
			LabelStyle style = new DataPointLabelStyle("Default",new Vector3D(0.5,0.5,1.0));
			style.ForeColor = Color.FromArgb(0,0,0,0);
			style.HasChanged = false;
			Add(style);

			for(LabelPositionKind lpk = LabelPositionKind.TopVertical; lpk <= LabelPositionKind.BelowHorizontal; lpk++)
				CreateStyle(lpk);
			for(PieLabelPositionKind lpk = PieLabelPositionKind.InsideRadial; lpk <= PieLabelPositionKind.OutsideAligned; lpk++)
				CreateStyle(lpk);
		}

		private void CreateStyle(LabelPositionKind lpk)
		{
			DataPointLabelStyle style = new DataPointLabelStyle(lpk.ToString(),new Vector3D(0.5,0.5,1.0));
			// Take the color from palette
			style.ForeColor = Color.FromArgb(0,0,0,0);
			style.LabelPosition = lpk;
			style.HasChanged = false;
			Add(style);
		}

		private void CreateStyle(PieLabelPositionKind lpk)
		{
			DataPointLabelStyle style = new DataPointLabelStyle(lpk.ToString(),new Vector3D(0.5,0.5,1.0));
			// Take the color from palette
			style.ForeColor = Color.FromArgb(0,0,0,0);
			style.PieLabelPosition = lpk;
			style.DataPointLabelKind = DataPointLabelKind.PieDoughnutShape;
			style.HasChanged = false;
			Add(style);
		}

		#region --- Member Creation Interface ---

        /// <summary>
        /// Creates a new default <see cref="DataPointLabelStyle"/> object and adds it to this <see cref="DataPointLabelStyleCollection"/> object.
        /// </summary>
        /// <param name="newMemberName">The name of the style.</param>
        /// <returns>Newly created <see cref="DataPointLabelStyle"/>.</returns>
		public DataPointLabelStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

		/// <summary>
		/// Copies a specified <see cref="DataPointLabelStyle"/> and adds it to this <see cref="DataPointLabelStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberName">The name of the style to copy.</param>
		/// <returns>Newly created <see cref="DataPointLabelStyle"/>.</returns>
		public DataPointLabelStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			DataPointLabelStyle style = new DataPointLabelStyle(newMemberName);
			if(this[oldMemberName] != null)
				style.LoadFrom(this[oldMemberName]);
			Add(style);
			return style;
		}

		/// <summary>
		/// Copies a specified <see cref="DataPointLabelStyle"/> and adds it to this <see cref="DataPointLabelStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberKind"><see cref="DataPointLabelStyleKind"/> of the style to copy.</param>
		/// <returns>Newly created <see cref="DataPointLabelStyle"/>.</returns>
		public DataPointLabelStyle CreateFrom(string newMemberName, DataPointLabelStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,oldMemberKind.ToString());
		}
		#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ==================  ");
			S.Comment("    Point Label Styles  ");
			S.Comment("    ==================  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("DataPointLabelStyle"))
				{
					DataPointLabelStyle P = new DataPointLabelStyle();
					P.Serialize(S);
					Add(P);
					while(S.GoToNext("DataPointLabelStyle"))
					{
						P = new DataPointLabelStyle();
						P.Serialize(S);
						Add(P);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(DataPointLabelStyle P in this)
				{
					if(S.BeginTag("DataPointLabelStyle"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}

		internal void CreateDOM(XmlElement parent)
		{
			XmlDocument doc = parent.OwnerDocument;
			XmlElement root = doc.CreateElement("LabelStyles");

			foreach(DataPointLabelStyle S in this)
				root.AppendChild(S.CreateDOM(doc));
			parent.AppendChild(root);
		}

		internal static DataPointLabelStyleCollection CreateFromDOM(XmlElement root)
		{
			if(root.Name.ToLower() != "labelstyles")
				return null;
			DataPointLabelStyleCollection C = new DataPointLabelStyleCollection(null,true);

			foreach (XmlElement e in root.ChildNodes)
				C.Add(DataPointLabelStyle.CreateFromDOM(e));

			return C;
		}
		#endregion
	}
}
