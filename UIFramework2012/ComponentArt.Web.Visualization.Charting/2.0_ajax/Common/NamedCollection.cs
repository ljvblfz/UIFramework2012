using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Drawing;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Provides an abstract base class for a uniquely named, strongly typed collection.
	/// </summary>
#if !__BUILDING_CRI__
	[Editor(typeof(NamedCollectionEditor), typeof(UITypeEditor))]
#endif
	[Serializable]
	public class NamedCollection : CollectionWithType 
	{

		#region --- Constructors ---
		internal NamedCollection(Type baseType) : 
			this(baseType, null) {}
		
		internal NamedCollection() {}

		
		internal NamedCollection(Type baseType, object owner) : 
			this(baseType, owner, null) {}
		
		internal NamedCollection(Type baseType, object owner, Type [] types) : 
			this(baseType, owner, types, "Name") {}

		internal NamedCollection(Type baseType, object owner, Type [] types, 
			string stringIndexPropertyName) : base(baseType, owner, types)
		{
			CheckNameProperty(baseType,stringIndexPropertyName);
			CheckConstructor();
		}
		#endregion

		#region --- Default naming support ---

		internal string NextAvailableDefaultName
		{
			get
			{
				string namePrefix = this.Type.Name;
				return NextAvailableName(namePrefix);
			}
		}

		internal virtual string NextAvailableName(string namePrefix)
		{
			for(int i = 1; true; i++)
			{
				if (this[namePrefix + i] == null)
					return namePrefix + i;
			}
		}

		internal string GetDefaultName(object obj)
		{
			string namePrefix = obj.GetType().Name;
			return NextAvailableName(namePrefix);
		}
		#endregion

		#region --- Checking type properties ---
		internal void CheckNameProperty(Type baseType, string stringIndexPropertyName)
		{
			PropertyInfo pi = Type.GetProperty(stringIndexPropertyName);

			Type[] interfaces = baseType.GetInterfaces();

			// Find interface
			bool found = false;
			foreach (Type ifc in interfaces) 
			{
				if (ifc == typeof(INamedObject))
				{
					found = true;
					break;
				}
			}
			
			if (!found)
			{
				throw new ArgumentException("Assertion in : " + this.ToString() +
					"  Type '" + baseType.ToString() + 
					"' doesn't implement interface INamedObject");
			}
			
			
			if (pi == null) 
			{
				throw new ArgumentException("Type " + Type.ToString() + 
					" doesn't have property '" + stringIndexPropertyName + "'");
			}

			if (pi.PropertyType != typeof (string))
			{
				throw new ArgumentException("Property " + Type.ToString() + "." + 
					stringIndexPropertyName + " must be of type string.");
			}
		}

		internal void CheckConstructor() 
		{
			if (Types == null)
			{
				if (Type.GetConstructor(new Type[] {typeof(string)}) == null)
					throw new ArgumentException("Constructor(string) on type "+ Type.ToString() +" not found.");
			}
			else 
			{
				foreach (Type t in Types) 
				{
					if (t.GetConstructor(new Type[] {typeof(string)}) == null)
						throw new ArgumentException("Constructor(string) on type "+ t.ToString() +" not found.");
				}
			}
		}

		#endregion

		#region --- Collection interface ---
		
		internal new INamedObject this[ object obj ] 
		{
			get 
			{
				int index = IndexOf(obj);
				if (index == -1) return null;
				return (INamedObject)List[index]; 
			}
			set 
			{
				int ix = IndexOf(obj);
				if (ix < 0)
					Add(value);
				else
					List[ix] = value;
			} 
		}
		

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="NamedCollection"/>.
		/// </summary>
		/// <param name="name">Removes the object with the specified name.</param>
		public virtual void Remove(string name)
		{
			Remove(this[name]);
		}

		// Override these methods to prevent items with same strings

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the <see cref="NamedCollection" /> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert <paramref name="value" />.</param>
		/// <param name="value">The new value of the element at <paramref name="index" />.</param>
		protected override void OnInsert( int index, Object value )  
		{

			((INamedObject)value).OwningCollection = this;

			base.OnInsert(index, value);

			if (((INamedObject)value).Name == null || ((INamedObject)value).Name == "")
				((INamedObject)value).Name = GetDefaultName(value);


			object to_replace = this[((INamedObject)value).Name];
			if (to_replace != null) 
			{

				// Preserve the Removable property if set to true
				System.Reflection.PropertyInfo pi = 
					Type.GetProperty("Removable", BindingFlags.Instance | BindingFlags.NonPublic);

				if (pi != null) 
				{
					if ((bool)pi.GetValue(to_replace, null) == false) 
					{

						pi.SetValue(value, false, null);

						// To be able to remove this object set the removable flag to true
						pi.SetValue(to_replace, true, null);
					}
				}

				Remove(to_replace);
			}
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the <see cref="NamedCollection" /> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="oldValue" /> can be found.</param>
		/// <param name="oldValue">The zero-based index at which <paramref name="oldValue" /> can be found.</param>
		/// <param name="newValue">The value to replace with <paramref name="newValue" />.</param>
		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			base.OnSet(index, oldValue, newValue);

			if (((INamedObject)newValue).Name == ((INamedObject)oldValue).Name)
				return;

			object to_replace = this[((INamedObject)newValue).Name];
			if (to_replace != null) 
			{

				Remove(to_replace);
			}		
		}


		
		/// <summary>
		/// Searches for the specified <see cref="System.Object"/> and returns the zero-based index of the first occurrence within the entire <see cref="NamedCollection"/>.
		/// </summary>
		/// <param name="obj">The item to locate in the <see cref="NamedCollection"/>. Can be specified by Name or zero-based index.</param>
		/// <returns>The zero-based index of the first occurrence of value within the entire <see cref="NamedCollection"/>, if found; otherwise, -1.</returns>
		public override int IndexOf(object obj) 
		{ 
			if (obj is int)
				return (int)obj;

			if (obj is string)
			{
				for (int i = 0; i < List.Count; i++)
					if (((INamedObject)List[i]).Name == obj.ToString()) 
						return i;
				return -1; 
			}
			else 
			{
				// Fixme: maybe should go through the list?
				throw new ArgumentException("Only a string or an integer is permitted for the indexer.");
			}
		} 


		/// <summary>
		/// Retrieves the array of string representing the names of objects in this <see cref="NamedCollection"/>.
		/// </summary>
		/// <returns>Names of objects in this <see cref="NamedCollection"/>.</returns>
		public string [] GetNames() 
		{
			ArrayList al = new ArrayList();
			for (int i = 0; i < List.Count; i++) 
			{
				al.Add(((INamedObject)List[i]).Name);
			}
			return (string []) al.ToArray(typeof(string));		
		}

		#endregion
	}
}
