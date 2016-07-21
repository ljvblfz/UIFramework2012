using System;
using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="MarkerStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class MarkerStyleCollection : NamedCollection
	{
		internal MarkerStyleCollection(object owner, bool initialize) : base(typeof(MarkerStyle),owner) 
		{
			if(initialize)
				Initialize();
		}
		internal MarkerStyleCollection(object owner) : this(owner,false) { }

		internal MarkerStyleCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="MarkerStyle"/> at the specified indexed location in the <see cref="MarkerStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="MarkerStyle"/> from the <see cref="MarkerStyleCollection"/> object.</param>
		public new MarkerStyle this[object index]   
		{ 
			get 
			{
				if(index is MarkerStyleKind)
					index = MarkerStyle.NameOf((MarkerStyleKind)index);
				return (MarkerStyle)base[index]; 
			} 
			set 
			{
				if(index is MarkerStyleKind)
					index = MarkerStyle.NameOf((MarkerStyleKind)index);
				base[index] = value; 
			} 
		}
 		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    =============  ");
			S.Comment("    Marker Styles  ");
			S.Comment("    =============  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("MarkerStyle"))
				{
					MarkerStyle P = new MarkerStyle();
					P.Serialize(S);
					Add(P);
					while(S.GoToNext("MarkerStyle"))
					{
						P = new MarkerStyle();
						P.Serialize(S);
						Add(P);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(MarkerStyle P in this)
				{
					if(S.BeginTag("MarkerStyle"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}
		#endregion

		private void AddMarker(MarkerKind kind)
		{
			Vector3D size = new Vector3D(10,10,10);
			MarkerStyle MS;
			Color color = Color.Red;

			if(kind == MarkerKind.Bubble)
				size = new Vector3D(0,0,0); // this is to keep backwards compatibility
			MS = new MarkerStyle(kind.ToString(),kind,size,color);
			MS.IsDefault = true;
			MS.Removable = false;
			Add(MS);
		}

		private void Initialize()
		{
			AddMarker(MarkerKind.Bubble);
			AddMarker(MarkerKind.Block);
			AddMarker(MarkerKind.Circle);
			AddMarker(MarkerKind.Diamond);
			AddMarker(MarkerKind.Triangle);
			AddMarker(MarkerKind.InvertedTriangle);
			AddMarker(MarkerKind.LeftTriangle);
			AddMarker(MarkerKind.RightTriangle);
			AddMarker(MarkerKind.Cross);
			AddMarker(MarkerKind.XShape);
			AddMarker(MarkerKind.ArrowE);
			AddMarker(MarkerKind.ArrowW);
			AddMarker(MarkerKind.ArrowN);
			AddMarker(MarkerKind.ArrowS);
			AddMarker(MarkerKind.ArrowNE);
			AddMarker(MarkerKind.ArrowNW);
			AddMarker(MarkerKind.ArrowSE);
			AddMarker(MarkerKind.ArrowSW);
		}
		
		#region --- Member Creation Interface ---

        /// <summary>
        /// Creates a new default <see cref="MarkerStyle"/> object and adds it to this <see cref="MarkerStyleCollection"/> object.
        /// </summary>
        /// <param name="newMemberName">The name of the style.</param>
        /// <returns>Newly created <see cref="MarkerStyle"/>.</returns>
        public MarkerStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"");
		}

		/// <summary>
		/// Copies a specified <see cref="MarkerStyle"/> and adds it to this <see cref="MarkerStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberName">The name of the style to copy.</param>
		/// <returns>Newly created <see cref="MarkerStyle"/>.</returns>
		public MarkerStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			MarkerStyle style = new MarkerStyle(newMemberName);
			MarkerStyle srcStyle = this[oldMemberName];
			if( srcStyle!= null)
			{
				style = (MarkerStyle)srcStyle.Clone();
				style.Name = newMemberName;
			}
			else
				style = new MarkerStyle(newMemberName);

			Add(style);
			return style;
		}

		/// <summary>
		/// Copies a specified <see cref="MarkerStyle"/> and adds it to this <see cref="MarkerStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName">The name of the new style.</param>
		/// <param name="oldMemberKind">The <see cref="MarkerStyleKind"/> of the style to copy.</param>
		/// <returns>Newly created <see cref="MarkerStyle"/>.</returns>
		public MarkerStyle CreateFrom(string newMemberName, MarkerStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,MarkerStyle.NameOf(oldMemberKind));
		}
		#endregion

	}
}
