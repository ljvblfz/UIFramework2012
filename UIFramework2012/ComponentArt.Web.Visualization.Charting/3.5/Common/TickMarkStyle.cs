using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;
using System.Drawing;



namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Specifies the kind of the tick mark
	/// </summary>
	public enum TickMarkKind
	{
		InnerTickmark, 
		OuterTickmark,
		CenteredTickmark
	};

	/// <summary>
	/// Enum type for accessing predefined tick-mark styles.
	/// </summary>
	/// <remarks>
	/// User defined tick-mark styles have StyleKind = <see cref="TickMarkStyleKind.Custom"/>.
	/// </remarks>
	public enum TickMarkStyleKind
	{
		Default,
		Inner,
		Outer,
		Centered,
		Custom
	}

	/// <summary>
	/// Represents the style of the tick marks.
	/// </summary>
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class TickMarkStyle : NamedObjectBase
	{
		private	TickMarkKind	tickMarkKind = TickMarkKind.InnerTickmark;
		private	double			lengthPts = 5;
		private	string			lineStyleName = "Default";
		private bool			hasChanged = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="TickMarkStyle"/> class with specified name, tick mark kind, length and 2D line style kind.
		/// </summary>
		/// <param name="styleName">Name of the TickMarkStyle.</param>
		/// <param name="tickMarkKind">the kind of the tick mark.</param>
		/// <param name="lengthPts">length of a tick mark</param>
		/// <param name="lineStyleName">name of the line style.</param>
		public TickMarkStyle(string styleName, TickMarkKind tickMarkKind, double lengthPts, string lineStyleName)
			: base(styleName)
		{
			this.tickMarkKind	= tickMarkKind;
			this.lengthPts		= lengthPts;
			this.lineStyleName	= lineStyleName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TickMarkStyle"/> class with specified name.
		/// </summary>
		/// <param name="styleName">Name of the TickMarkStyle.</param>
		public TickMarkStyle(string styleName) : base(styleName) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="TickMarkStyle"/> class with default parameters.
		/// </summary>
		public TickMarkStyle() : base ("") { }

		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } } 

		internal void LoadFrom(TickMarkStyle s)
		{
			this.tickMarkKind = s.tickMarkKind;
			this.lengthPts = s.lengthPts;
			this.lineStyleName = s.lineStyleName ;
		}

		#region --- Handling the Tick-mark enums ---

		static TickMarkStyleKind[] kinds = new TickMarkStyleKind[]
		{
			TickMarkStyleKind.Default,
			TickMarkStyleKind.Inner,
			TickMarkStyleKind.Outer,
			TickMarkStyleKind.Centered,
			TickMarkStyleKind.Custom
		};

		static string[] names = new string[]
		{
			"Default",
			"Inner",
			"Outer",
			"Centered",
			"Custom"
		};


		internal static string NameOf(TickMarkStyleKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'Grid' mismatch");
		}

		internal static TickMarkStyleKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return TickMarkStyleKind.Custom;
		}

		/// <summary>
		/// Gets or sets the style kind
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TickMarkStyleKind StyleKind
		{
			get
			{
				return TickMarkStyle.KindOf(Name);
			}
			set
			{
				Name = TickMarkStyle.NameOf(value);
			}
		}

		#endregion

		/// <summary>
		/// Returns an exact copy of this <see cref="TickMarkStyle"/> object.
		/// </summary>
		/// <returns>An exact copy of this <see cref="TickMarkStyle"/> object.</returns>
		public object Clone()
		{
			return new TickMarkStyle(Name,tickMarkKind,lengthPts,lineStyleName);
		}

		#region --- Properties ---
		
		/// <summary>
		/// Gets or sets the kind of this <see cref="TickMarkStyle"/> object.
		/// </summary>
		[SRDescription("TickMarkStyleKindDesrc")]
		public TickMarkKind Kind			{ get { return tickMarkKind; }	set { tickMarkKind = value; } }
		/// <summary>
		/// Gets or sets the length of this <see cref="TickMarkStyle"/> object.
		/// </summary>
		[SRDescription("TickMarkStyleLengthDesrc")]
		public double		Length			{ get { return lengthPts; }		set { lengthPts = value; } }
		/// <summary>
		/// Gets or sets the 2D line style to this object.
		/// </summary>
		/// <remarks>
		/// The value should be a valid <see cref="LineStyle2D.Name"/>.
		/// </remarks>
		[TypeConverter(typeof(SelectedLineStyle2DConverter))]
		[SRDescription("TickMarkStyleLineStyleNameDesrc")]
		public string		LineStyleName	{ get { return lineStyleName; }	set { lineStyleName = value; } }

		#endregion
	}
		
	/// <summary>
	/// A collection of <see cref="TickMarkStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public class TickMarkStyleCollection : NamedCollection
	{
		internal TickMarkStyleCollection(Object owner, bool initialize) 
			: base (typeof(TickMarkStyle),owner) 
		{
			if(initialize)
				InitializeContents();
		}

		internal TickMarkStyleCollection(Object owner) : this(owner,true) { }
		
		internal TickMarkStyleCollection() : this(null) { }
		/// <summary>
		/// Indicates the <see cref="TickMarkStyle"/> at the specified indexed location in the <see cref="TickMarkStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or <see cref="TickMarkStyle"/> or name to retrieve a <see cref="TickMarkStyle"/> from the <see cref="TickMarkStyleCollection"/> object.</param>

		public new TickMarkStyle this[object obj]
		{
			get 
			{
				if(obj is TickMarkStyleKind)
					obj = TickMarkStyle.NameOf((TickMarkStyleKind)obj);
				return (TickMarkStyle)(base[IndexOf(obj)]); 
			} 
			set 
			{
				if(obj is TickMarkStyleKind)
					obj = TickMarkStyle.NameOf((TickMarkStyleKind)obj);
				base[IndexOf(obj)] = value;
			} 
		}

		private void InitializeContents()
		{
			TickMarkStyle style;

			style = new TickMarkStyle("Default",TickMarkKind.InnerTickmark,5,"AxisLine");
			style.HasChanged = false;
			Add(style);

			style = new TickMarkStyle("Inner",TickMarkKind.InnerTickmark,5,"AxisLine");
			style.HasChanged = false;
			Add(style);

			style = new TickMarkStyle("Outer",TickMarkKind.OuterTickmark,5,"AxisLine");
			style.HasChanged = false;
			Add(style);

			style = new TickMarkStyle("Centered",TickMarkKind.CenteredTickmark,5,"AxisLine");
			style.HasChanged = false;
			Add(style);
		}
		
		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates a new <see cref="TickMarkStyle"/> object which is a copy of the default tick mark style and adds it to this <see cref="TickMarkStyleCollection"/>.
		/// </summary>
		/// <param name="newMemberName">The name of the new tick mark style.</param>
		/// <returns>Newly created object.</returns>
		public TickMarkStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

		/// <summary>
		/// Creates a new <see cref="TickMarkStyle"/> object which is a copy of a specified tick mark style and adds it to this <see cref="TickMarkStyleCollection"/>.
		/// </summary>
		/// <param name="newMemberName">The name of the new tick mark style.</param>
		/// <param name="oldMemberName">The name of the tick mark style to be copied.</param>
		/// <returns>Newly created object.</returns>
		public TickMarkStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			TickMarkStyle style;
			TickMarkStyle srcStyle = this[oldMemberName];
			if(srcStyle != null)
			{
				style = (TickMarkStyle)srcStyle.Clone();
				style.Name = newMemberName;
			}
			else
				style = new TickMarkStyle(newMemberName);
			Add(style);
			return style;
		}

		/// <summary>
		/// Creates a new <see cref="TickMarkStyle"/> object which is a copy of a specified tick mark style and adds it to this <see cref="TickMarkStyleCollection"/>.
		/// </summary>
		/// <param name="newMemberName">The name of the new tick mark style.</param>
		/// <param name="oldMemberKind">The <see cref="TickMarkStyleKind"/> of the tick mark style to be copied.</param>
		/// <returns>Newly created object.</returns>
		public TickMarkStyle CreateFrom(string newMemberName, TickMarkStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,TickMarkStyle.NameOf(oldMemberKind));
		}

		#endregion
	}
}
