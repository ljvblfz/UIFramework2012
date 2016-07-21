using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="LineStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class LineStyleCollection : NamedCollection
	{
		internal LineStyleCollection(Object owner,bool initialize) 
			: base(typeof(LineStyle), owner, 
			new Type[] {
						   typeof(BlockLineStyle), typeof(DashLineStyle), typeof(DotLineStyle), typeof(FlatLineStyle), 
						   typeof(MultiLineStyle), typeof(StripLineStyle)/*, typeof(NoLineStyle)*/, typeof(PipeLineStyle), 
						   typeof(ThickLineStyle)/*, typeof(TwoDLineStyle)*/}
			)
		{
			if(initialize)
				InitializeContents();
		}

		internal LineStyleCollection() : this(null,false) {}

		internal void InitializeContents()
		{
			LineStyle LS;
			
			BlockLineStyle BLS = new BlockLineStyle("Default",Color.Green,4,20);
			BLS.WidthEdgeRadius = 0.5;
			Add(BLS);
			BLS.HasChanged = false;

			BLS = new BlockLineStyle("StripLine",Color.Green,4,20);
			BLS.WidthEdgeRadius = 0.5;
			Add(BLS);
			BLS.HasChanged = false;

			LS = new BlockLineStyle("BlockLine");
			Add(LS);
			LS.HasChanged = false;

			LS = new PipeLineStyle("PipeLine");
			Add(LS);
			LS.HasChanged = false;

			LS = new FlatLineStyle("FlatLine");
			Add(LS);
			LS.HasChanged = false;

			LS = new DotLineStyle("DotLine");
			Add(LS);
			LS.HasChanged = false;

			LS = new DashLineStyle("DashLine","PipeLine","NoLine",15,5);
			Add(LS);
			LS.HasChanged = false;

			LS = new DashLineStyle("DashDotLine","PipeLine","DotLine",20,15);
			Add(LS);
			LS.HasChanged = false;

			LS = new MultiLineStyle("MultiLine", new String[] { "FlatLine","DotLine" } );
			Add(LS);
			LS.HasChanged = false;

		}

		/// <summary>
		/// Indicates the <see cref="LineStyle"/> at the specified indexed location in the <see cref="LineStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="LineStyle"/> from the <see cref="LineStyleCollection"/> object.</param>
		public new LineStyle this[object index]   
		{ 
			get 
			{
				if(index is LineStyleKind)
					index = LineStyle.NameOf((LineStyleKind)index);
				return ((LineStyle)base[index]); 
			} 
			set
			{
				if(index is LineStyleKind)
					index = LineStyle.NameOf((LineStyleKind)index);
				base[index] = value; 
			} 
		}
		
		/// <summary>
		/// Clones and stores the specified <see cref="LineStyle"/>.
		/// </summary>
		/// <param name="originalStyleName">Name of the original style.</param>
		/// <param name="clonedStyleName">Name of the cloned style.</param>
		/// <returns>Returns the cloned style.</returns>
		/// <remarks>If the original style does not exist, the function returns null. 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		internal LineStyle Clone(string originalStyleName, string clonedStyleName)
		{
			LineStyle original = this[originalStyleName];
			if(original == null)
				return null;
			StyleCloner cloner = new StyleCloner();
			LineStyle clonedStyle = cloner.Clone(original) as LineStyle;
			clonedStyle.Name = clonedStyleName;
			original.OwningCollection.Add(clonedStyle);

			return clonedStyle;
		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Crates a new <see cref="LineStyle"/> object from which is a copy of the Default line style and adds it to this <see cref="LineStyleCollection"/> object.
		/// </summary>
		/// <param name="newMemberName"></param>
		/// <returns></returns>
		public LineStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

		/// <summary>
		/// Crates a copy of the specified <see cref="LineStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned style.</param>
		/// <param name="oldMemberName">Name of the original style.</param>
		/// <returns>Returns the new style.</returns>
		/// <remarks>If the original style does not exist, the function returns a copy of style named "Default". 
		/// If "Default" style is not found, function returns null. 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public LineStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			LineStyle style = Clone(oldMemberName,newMemberName);
			if(style != null)
				return style;

			style = CreateFrom("Default",newMemberName);
			if(style != null)
				Add(style);
			return style;
		}

		/// <summary>
		/// Crates a copy of the specified <see cref="LineStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned style.</param>
		/// <param name="oldMemberKind">The <see cref="LineStyleKind"/> of the original style.</param>
		/// <returns>Returns the new style.</returns>
		/// <remarks>If the original style does not exist, the function returns a copy of style named "Default". 
		/// If "Default" style is not found, function returns null. 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public LineStyle CreateFrom(string newMemberName, LineStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,LineStyle.NameOf(oldMemberKind));
		}
		#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ===========  ");
			S.Comment("    Line Styles  ");
			S.Comment("    ===========  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("LineStyle"))
				{
					LineStyle P = SerializeSingleStyle(S);
					Add(P);
					while(S.GoToNext("LineStyle"))
					{
						P = SerializeSingleStyle(S);
						Add(P);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(LineStyle P in this)
				{
					if(S.BeginTag("LineStyle"))
					{
						P.Serialize(S);
						S.EndTag();
					}
				}
			}
		}

		private LineStyle SerializeSingleStyle(XmlCustomSerializer S)
		{
			Type lineType = (Type)S.GetAttribute("LineType",typeof(Type));
			// Construct the object
			System.Reflection.ConstructorInfo ci = lineType.GetConstructor(new Type[0]);
			LineStyle line = (LineStyle)ci.Invoke(null);
			line.Serialize(S);
			return line;
		}
		#endregion
	}
}
