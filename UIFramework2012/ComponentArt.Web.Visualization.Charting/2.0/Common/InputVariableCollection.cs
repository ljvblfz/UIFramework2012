using System;
using System.Collections;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="InputVariable"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class InputVariableCollection : NamedCollection
	{
		internal InputVariableCollection(object owner) : base(typeof(InputVariable),owner) { }
		internal InputVariableCollection() : this(null) { }

		//internal void SetOwner(object obj) { Owner = obj; }

		/// <summary>
		/// Indicates the <see cref="InputVariable"/> at the specified indexed location in the <see cref="InputVariableCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="InputVariable"/> from the <see cref="InputVariableCollection"/> object.</param>
		public new InputVariable this[object index]   
		{ 
			get { return (InputVariable)base[index]; } 
			set { base[index] = value; } 
		}
		
		/// <summary>
		/// Adds a specified <see cref="InputVariable"/> to a <see cref="CollectionWithType"/> object.
		/// </summary>
		/// <param name="value"> <see cref="InputVariable"/> to be added to <see cref="InputVariableCollection"/>.</param>
		/// <returns>The index of the newly added object or -1 if the the object could not be added.</returns>
		public override int Add( object value )  
		{
			if(value == null || !(value is InputVariable))
				return -1;
			(value as InputVariable).OwningCollection = this;
			//(Owner as DataProvider).OwningChart.Refresh();
			return( List.Add( value ) );
		}

	}
}
