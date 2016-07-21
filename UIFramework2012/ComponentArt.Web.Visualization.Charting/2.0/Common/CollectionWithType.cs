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
	/// Provides an abstract base class for a strongly typed collection.
	/// </summary>

#if __COMPILING_FOR_2_0_AND_ABOVE__
    [DataObjectAttribute(true)]
#endif
    [ 
	ListBindable(false) 
	]
#if !__BUILDING_CRI__
	[Editor(typeof(CollectionWithTypeEditor), typeof(UITypeEditor))]
#endif
	[Serializable()]
	public abstract class CollectionWithType : CollectionBase 
	{
		private Type m_type;
		private Type [] m_types;
		private object m_owner;

		internal CollectionWithType(Type memberType, object owner, Type [] types)
		{
			m_type = memberType;
			m_types = types;
			m_owner = owner;
		}

		internal CollectionWithType(Type memberType, object owner) : this(memberType, owner, null) {}

		internal CollectionWithType(Type memberType) : this(memberType, null) {}

		internal CollectionWithType() {}

		/// <summary>
		/// Gets an array of types that can be added to <see cref="CollectionWithType"/> object.
		/// </summary>
		public Type [] Types 
		{
			get { return m_types; }
		}

		internal virtual object Owner 
		{
			get 
			{
				return m_owner;
			}
			set 
			{
				m_owner = value;
			}
		}			

		/// <summary>
		/// Gets the base type of all the items in <see cref="CollectionWithType"/> object.
		/// </summary>
		public Type Type 
		{
			get 
			{
				return m_type;
			}
		}

		/// <summary>
		/// Indicates the <see cref="object"/> at the specified indexed location in the <see cref="CollectionWithType"/> object. 
		/// </summary>
		/// <param name="obj">Zero-based index to retrieve from the <see cref="CollectionWithType"/> object.</param>
		protected object this[object obj]
		{
			get { return List[IndexOf(obj)]; } 
			set { List[IndexOf(obj)] = value;} 
		}

		internal void SetOwner(object owner)
		{
			m_owner = owner;
		}

		/// <summary>
		/// Adds a specified object to a <see cref="CollectionWithType"/> object.
		/// </summary>
		/// <param name="value">Object to be added to <see cref="CollectionWithType"/>.</param>
		/// <returns>The index of the newly added object or -1 if the the object could not be added.</returns>
		public virtual int Add( object value )  
		{
			if (value == null)
				return -1;
			return( List.Add( value ) );
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="CollectionWithType"/> is read only.
		/// </summary>
		public bool IsReadOnly 
		{
			get 
			{
				return (List.IsReadOnly);
			}
		}

		/// <summary>
		/// Searches for the specified <see cref="System.Object"/> and returns the zero-based index of the first occurrence within the entire <see cref="CollectionWithType"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to locate in the <see cref="CollectionWithType"/>.</param>
		/// <returns>The zero-based index of the first occurrence of value within the entire <see cref="CollectionWithType"/>, if found; otherwise, -1.</returns>
		public virtual int IndexOf(object obj) 
		{ 
			if (obj is int)
				return (int)obj;

			else 
			{
				throw new ArgumentException("Only an integer is permitted for the indexer.");
			}
		} 

		/// <summary>
		/// Inserts an element into the <see cref="CollectionWithType"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
		/// <param name="value">The <see cref="object"/> to insert.</param>
		public void Insert( int index, object value )  
		{
			List.Insert( index, value );
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="CollectionWithType"/>.
		/// </summary>
		/// <param name="value">The <see cref="object"/> to remove from the <see cref="CollectionWithType"/>.</param>
		public void Remove( object value )  
		{
			if(value != null)
				List.Remove( value );
		}

		/// <summary>
		/// Determines whether an element is in the <see cref="CollectionWithType"/>.
		/// </summary>
		/// <param name="value">The <see cref="object"/> to locate in the <see cref="CollectionWithType"/>. The element to locate can be null.</param>
		/// <returns>true if item is found in the <see cref="CollectionWithType"/>; otherwise, false.</returns>
		public bool Contains( object value )  
		{
			// If value is not of type Member, this will return false.
			return( List.Contains( value ) );
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the <see cref="CollectionWithType" /> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert <paramref name="value" />.</param>
		/// <param name="value">The new value of the element at <paramref name="index" />.</param>
		protected override void OnInsert( int index, Object value )  
		{
			if(!m_type.IsInstanceOfType(value))
				throw new ArgumentException( "value must be of type " + m_type.ToString() + ".", "value" );

			if (m_owner != null) 
			{

				System.Reflection.MethodInfo mi = m_type.GetMethod(
					"SetContext", BindingFlags.Instance | BindingFlags.NonPublic, null,
					CallingConventions.Any, new Type[] {typeof(object)}, null);
				
				if (mi != null)
				{
					try 
					{
						mi.Invoke(value, new object [] {m_owner});
					} 
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
		}

		bool m_inCollectionEditor = false;
		internal bool InCollectionEditor 
		{
			get {return m_inCollectionEditor;}
			set {m_inCollectionEditor = value;}
		}

		internal virtual void OnEditStarting() { }
		internal virtual void OnEditCompleted(bool cancelled) { }

		/// <summary>
		/// Performs additional custom processes when clearing the contents of the <see cref="CollectionWithType" /> instance.
		/// </summary>
		protected override void OnClear()  
		{
			if (!m_inCollectionEditor) 
			{
				foreach (object o in List) 
				{
					if (CanRemove(o) == false)
						throw new ArgumentException( "Cannot clear the collection since some items cannot be removed." );
				}
			}
			base.OnClear();
		}

		bool CanRemove(Object value) 
		{
			System.Reflection.PropertyInfo pi = 
				value.GetType().GetProperty("Removable", BindingFlags.Instance | BindingFlags.NonPublic);

			if (pi != null && (bool) pi.GetValue(value, null) == false) 
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the <see cref="CollectionWithType" /> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> can be found.</param>
		/// <param name="value">The value of the element to remove from <paramref name="index" />.</param>
		protected override void OnRemove( int index, Object value )  
		{
			if(value == null)
				return;
			if ( value.GetType() != m_type && !value.GetType().IsSubclassOf(m_type))
			{
				// check interfaces as well
				Type[] infs = value.GetType().GetInterfaces();
				for(int i=0; i<infs.Length; i++)
					if(m_type == infs[i])
						return;
				throw new ArgumentException( "value must be of type " + m_type.ToString() + ".", "value" );
			}

			if (CanRemove(value) == false) 
			{
				throw new ArgumentException( "The item '" + value.ToString() + "' cannot be removed." );
			}
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the <see cref="CollectionWithType" /> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="oldValue" /> can be found.</param>
		/// <param name="oldValue">The zero-based index at which <paramref name="oldValue" /> can be found.</param>
		/// <param name="newValue">The value to replace with <paramref name="newValue" />.</param>
		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( !m_type.IsInstanceOfType(newValue))
					throw new ArgumentException( "newValue must be of type " + m_type.ToString() + ".", "newValue" );
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate( Object value )  
		{
			if ( !m_type.IsInstanceOfType(value))
				throw new ArgumentException( "value must be of type " + m_type.ToString() + "." );
		}
	}
}
