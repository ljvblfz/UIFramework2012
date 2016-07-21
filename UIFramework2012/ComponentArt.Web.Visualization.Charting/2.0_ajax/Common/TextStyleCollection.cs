using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Xml;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="TextStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class TextStyleCollection : NamedCollection
	{
		internal TextStyleCollection(Object owner, bool initialize) : base(typeof(TextStyle),owner)
		{ 
			if(initialize)
				InitializeContents();
		}

		internal TextStyleCollection(Object owner) : this(owner,false)
		{ }

		internal TextStyleCollection() { }

		/// <summary>
		/// Indicates the <see cref="TextStyle"/> at the specified indexed location in the <see cref="TextStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or <see cref="TextStyleKind"/> or name to retrieve a <see cref="TextStyle"/> from the <see cref="TextStyleCollection"/> object.</param>
		public new TextStyle this[object index]   
		{ 
			get 
			{
				if(index is TextStyleKind)
					index = TextStyle.NameOf((TextStyleKind)index);
				return ((TextStyle)base[index]); 
			} 
			set 
			{
				if(index is TextStyleKind)
					index = TextStyle.NameOf((TextStyleKind)index);
				base[index] = value; 
			} 
		}

		private void InitializeContents()
		{
			// Default
			TextStyle TS = new TextStyle("Default", new Font("Arial", 10), Color.Black, Color.Black, 0.0,
				ReverseSideStyle.FlipReverseSide, ReverseDirectionStyle.FlipReverseDirection, false);
			Add(TS);
			TS.HasChanged = false;

			// Data value
			TS = new TextStyle("DataValue", new Font("Arial", 10), Color.Black, Color.Black, 0.0,
				ReverseSideStyle.FlipReverseSide, ReverseDirectionStyle.FlipReverseDirection, false);
			Add(TS);
			TS.HasChanged = false;

			// Special value
			TS = new TextStyle("HighlightedDataValue", new Font("Arial", 10), Color.Red, Color.Black, 0.0,
				ReverseSideStyle.FlipReverseSide, ReverseDirectionStyle.FlipReverseDirection, false);
			Add(TS);
			TS.HasChanged = false;
			
			// Golden
			TS = new TextStyle("Golden", new Font("Arial", 10), Color.Yellow, Color.Black, 1.0,
				ReverseSideStyle.FlipReverseSide, ReverseDirectionStyle.FlipReverseDirection, false);
			Add(TS);
			TS.HasChanged = false;
		}

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ===========  ");
			S.Comment("    Text Styles  ");
			S.Comment("    ===========  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("TextStyle"))
				{
					TextStyle style = new TextStyle();
					style.Serialize(S);
					Add(style);
					while(S.GoToNext("TextStyle"))
					{
						style = new TextStyle();
						style.Serialize(S);
						Add(style);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(TextStyle style in this)
				{
					if(S.BeginTag("TextStyle"))
					{
						style.Serialize(S);
						S.EndTag();
					}
				}
			}
		}
	}

}
