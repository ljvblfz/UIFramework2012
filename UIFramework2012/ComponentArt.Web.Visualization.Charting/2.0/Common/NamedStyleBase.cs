using System;
using System.ComponentModel;
using System.Reflection;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Base class implementing "Name" property and owning chart
	/// </summary>
	[System.ComponentModel.TypeConverter(typeof(NamedObjectBaseConverter))]
	[Serializable]
	public abstract class NamedObjectBase : INamedObject
	{
		private static int unnamedCount=0;
		internal NamedStyleInternal m_nsi = new NamedStyleInternal();

		internal NamedObjectBase() : this(null)
		{}

		internal NamedObjectBase(string name)
		{
			Name = name;
		}

		internal void SetNextDefaultName()
		{
			Name = NextUnnamed();
		}

		public override string ToString() { return Name; }

		/// <summary>
		/// Gets or sets a name of this <see cref="NamedObjectBase"/> object.
		/// </summary>
		[Category("General")]
		[Description("Indicates the name of the object")]
		public string Name { get { return m_nsi.Name; } set {m_nsi.Name = value;} }

		internal void SetName(string name) { this.Name = name; }

		internal ChartBase  OwningChart	{ get { return m_nsi.OwningChart; } set {m_nsi.OwningChart = value;}}
		//Fixme: can't guarantee uniqueness.
		/// <summary>
		/// Determines an unused name and returns it.
		/// </summary>
		/// <returns>New unused name.</returns>
		public static string NextUnnamed() 
		{
			string r = "_Unnamed_" + unnamedCount.ToString(); 
			unnamedCount++;
			return r;
		}

		private bool ShouldSerializeNamedCollection() { return false; }
		//Fixme: hide it?
		/// <summary>
		/// Gets or sets the owning <see cref="NamedCollection"/> object. This property is not meant to be used by the user.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NamedCollection OwningCollection 
		{
			get {return m_nsi.NamedCollection;}
			set {m_nsi.NamedCollection = value;}
		}
	}

	/// <summary>
	/// Represents an item of a NamedCollection
	/// </summary>
	public interface INamedObject
	{
		/// <summary>
		/// Sets or gets the name of an element
		/// </summary>
		[Category("General")]
		string Name {get; set;}
		/// <summary>
		/// Sets or gets the NamedCollection object the element belongs to
		/// </summary>
		[Browsable(false)]
		NamedCollection OwningCollection {get; set;}
	}

	[Serializable]
	internal class NamedStyleInternal : INamedObjectInternal 
	{
		protected string m_name = "";
		protected NamedCollection m_namedCollection;
		protected ChartBase m_owningChart;
		
		public override string ToString() { return Name; }

		public string Name { 
			get 
			{
				return m_name; 
			}
			set 
			{
				if (Name == value) 
					return;

				
				if (NamedCollection != null) 
				{

					// Are we modofying a non-removable object's name?
					if (m_name != null && m_name != "") 
					{
						int index = NamedCollection.IndexOf(m_name);

						object item = NamedCollection[index];

						if(item != null)
						{
				
							PropertyInfo pi = 
								item.GetType().GetProperty("Removable", BindingFlags.Instance | BindingFlags.NonPublic);

							if (pi != null && (bool)pi.GetValue(item, null) == false)
								throw new ArgumentException( "The item '" + m_name + "' cannot be renamed." );
						}
					}
				

					// Are we setting a name to an already existing name?
					if (NamedCollection.IndexOf(value) != -1) 
					{
						throw new ArgumentException("The name '" + value + "' already exists in the collection.");
					}
				}

				m_name = value;
			}
		}


		private bool ShouldSerializeNamedCollection() { return false; }
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NamedCollection NamedCollection 
		{
			get
			{
				return m_namedCollection;
			}
			set
			{
				m_namedCollection = value;
				if (m_namedCollection != null && m_namedCollection.Owner != null && m_namedCollection.Owner is ChartBase) 
				{
					m_owningChart = (ChartBase)m_namedCollection.Owner;
				}
			}
		}


		public ChartBase OwningChart 
		{
			get
			{
				return m_owningChart;
			}
			set
			{
				m_owningChart = value;
			}
		}

		public void SetContext(object obj) 
		{
		}

	}

	internal interface INamedObjectInternal
	{
		string Name {get; set;}
		ChartBase OwningChart {get; set;}
		void SetContext(object obj);
		[Browsable(false)]
		NamedCollection NamedCollection {get; set;}
	}
}
