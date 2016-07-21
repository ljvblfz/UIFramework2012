using System;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;

namespace ComponentArt.Web.Visualization.Gauges
{

    /// <summary>
    /// Represents the method that will handle the <see cref="NamedObject.NameChanging"/> and <see cref="NamedObject.NameChanged"/> events of a <see cref="NamedObject"/>;
    /// and the <see cref="NamedObjectCollection.MemberChangingName"/> and <see cref="NamedObjectCollection.MemberChangedName"/> events of a <see cref="NamedObjectCollection"/>;
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="newName">The new name of the object.</param>
	public delegate void NameChangeHandler(NamedObject sender, string newName);
	
    /// <summary>
    /// Represents the method that will handle the <see cref="NamedObjectCollection.MemberAdding"/>, <see cref="NamedObjectCollection.MemberAdded"/>,
    /// <see cref="NamedObjectCollection.MemberRemoving"/> and <see cref="NamedObjectCollection.MemberRemoved"/> events of a <see cref="NamedObjectCollection"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="member">The member of the collection that changed.</param>
    public delegate void NamedObjectCollectionChangedHandler(NamedObjectCollection sender, NamedObject member);
	
	/// <summary>
	/// Represents an object that is named.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(NamedObjectConverter))]
	public class NamedObject : IObjectModelNode
	{
		private string name;
		private IObjectModelNode parent;
		private bool isRequired = false;

        /// <summary>
        /// Occurs before the current object's name is changed.
        /// </summary>
		public event NameChangeHandler NameChanging;
		
		/// <summary>
		/// Occurs after the current object's name has been changed.
		/// </summary>
		public event NameChangeHandler NameChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObject"/> class.
        /// </summary>
		public NamedObject()
		{
			this.name = "New" + this.GetType().Name;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObject"/> class with the specified name.
        /// </summary>
        public NamedObject(string name)
		{
			if(name == string.Empty)
				name = "New" + this.GetType().Name;
			this.name = name;
		}

		internal bool IsRequired { get { return isRequired; } set { isRequired = value; } }

		internal void OverrideName(string name) { this.name = name; }

        /// <summary>
        /// Gets or sets the name of this object.
        /// </summary>
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name == value)
					return;
				if(NameChanging != null)
					NameChanging(this, value);
				name = value;
				if(NameChanged != null)
					NameChanged(this, value);
			}
		}

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return this.GetType().Name + "\"" + this.Name + "\"";
		}

		#region --- IObjectModelNode Implementation ---

		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}

		#endregion
	}
	

    /// <summary>
    /// Contains a collection of <see cref="NamedObject"/> objects.
    /// </summary>
	[Editor(typeof(NamedObjectCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[Serializable]
	[ListBindable(BindableSupport.No)] // Binding to this collection (from another control) doesn't make sense
	public class NamedObjectCollection : CollectionBase, IObjectModelNode
	{
	    /// <summary>
	    /// Occurs before a <see cref="NamedObject"/> object in this collection has it's name changed.
	    /// </summary>
		public event NameChangeHandler MemberChangingName;
		
		/// <summary>
        /// Occurs after a <see cref="NamedObject"/> object in this collection has had it's name changed.
        /// </summary>
		public event NameChangeHandler MemberChangedName;

        /// <summary>
        /// Occurs before a <see cref="NamedObject"/> object is added to this collection.
        /// </summary>
        public event NamedObjectCollectionChangedHandler MemberAdding;

        /// <summary>
        /// Occurs after a <see cref="NamedObject"/> object has been added to this colleciton.
        /// </summary>
        public event NamedObjectCollectionChangedHandler MemberAdded;

        /// <summary>
        /// Occurs before a <see cref="NamedObject"/> object is removed from this collection.
        /// </summary>
        public event NamedObjectCollectionChangedHandler MemberRemoving;

        /// <summary>
        /// Occurs after a <see cref="NamedObject"/> object has been removed from this collection.
        /// </summary>
        public event NamedObjectCollectionChangedHandler MemberRemoved;
		
		private IObjectModelNode parent;

		private Hashtable snapshot = null;

        internal NamedObjectCollection(bool populateInitialContents)
        {
            if (populateInitialContents)
                PopulateInitialContents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObjectCollection"/> class.
        /// </summary>
        public NamedObjectCollection() : this(false) { }

        internal virtual void PopulateInitialContents() { }

		// Used in editor dialog to create a member with generic name
		internal virtual NamedObject CreateNewMember() 
		{ return new NamedObject("NewNamed"); }

		#region --- Member Creation Interface ---

		/// <summary>
		/// Clones and stores the specified <see cref="NamedObject"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		internal NamedObject AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			NamedObject oldMember = this[oldMemberName];
			if(oldMember == null)
				return null;

			NamedObject newMember = GaugeXmlSerializer.GetObject(GaugeXmlSerializer.GetDOM(oldMember)) as NamedObject;
			newMember.Name = newMemberName;
			Add(newMember);

			return newMember;
		}

		#endregion

		internal void PopulateFromList(ArrayList list)
		{
			List.Clear();
			foreach(object obj in list)
				List.Add(obj);
		}

		internal void SelectGenericNewName(NamedObject named)
		{
			string name = "New"+named.GetType().Name;
			if(this[name] != null)
			{
				for(int i=1; ; i++)
				{
					if(this[name+i] == null)
					{
						name = name+i;
						break;
					}
				}
			}
			named.OverrideName(name);
		}

		// Cloning

		internal NamedObjectCollection Clone()
		{
			return GaugeXmlSerializer.GetObject(GaugeXmlSerializer.GetDOM(this)) as NamedObjectCollection;
		}

        /// <summary>
        /// Adds an object the to this collection.
        /// </summary>
        /// <param name="value">The object to be added to this collection.</param>
        /// <returns>The index at which the object was added.</returns>
		public int Add(object value)
		{
			if (value.GetType().IsSubclassOf(typeof(NamedObject)))
				return AddNamed(value as NamedObject);
			throw new ArgumentException("Cannot add a(n) '" + value.GetType().Name + "' to a(n) '" +
				this.GetType().Name + "'");
		}

//		public int Add(NamedObject named)
//		{
//			return AddNamed(named);
//		}
		private int AddNamed(NamedObject named)
		{
			named.NameChanging += new NameChangeHandler(MemberNameChanging);
			named.NameChanged += new NameChangeHandler(MemberNameChanged);
			(named as IObjectModelNode).ParentNode = this;

			if (MemberAdding != null)
				MemberAdding(this, named);

			// Inherit "required" flag
			int ix = IndexOf(named.Name);
			if(ix >= 0)
			{
				named.IsRequired = this[ix].IsRequired;
				List.RemoveAt(ix);
				List.Insert(ix,named);
			}
			else
				ix = List.Add(named);

			if (MemberAdded != null)
				MemberAdded(this, named);
			return ix;
		}

        /// <summary>
        /// Removes an object from this collection.
        /// </summary>
        /// <param name="named">The object to remove from this collection.</param>
		public void Remove(NamedObject named)
		{
			if(named.IsRequired)
				return;
			if (MemberRemoving != null)
				MemberRemoving(this, named);
			this.List.Remove(named);
			if (MemberRemoved != null)
				MemberRemoved(this, named);
		}

		protected override void OnRemoveComplete(int ix, object member)
		{
			int i=ix;
		}

        /// <summary>
        /// Removes an object at the specified index from this collection.
        /// </summary>
        /// <param name="ix">The index of the object to remove from this collection.</param>
		public new void RemoveAt(int ix)
		{
			if (ix < 0 || ix >= Count)
				return;
			NamedObject named = this[ix];
			Remove(named);
		}
    
        /// <summary>
        /// Removes an object with the specified name from this collection.
        /// </summary>
        /// <param name="elementName">The named of the object to remove from this collection.</param>
		public void Remove(string elementName)
		{
			RemoveAt(IndexOf(elementName));
		}

		internal NamedObject this[object x]
		{
			get
			{
				if(x is int)
					return List[(int)x] as NamedObject;
				else if(x is string)
				{
					string name = (string)x;
					int ix = IndexOf(name);
					if (ix >= 0)
						return this[ix];
					else if (name == "Auto")
						return this["Default"];
					else				
						return null;
				}
				else 
					return null;
			}
			set
			{
				if(x is int)
				{
					int index = (int)x;
					if (MemberAdding != null)
						MemberAdding(this, value);
					int ix = IndexOf(value.Name);
					if (ix > 0 && ix != index)
						RemoveAt(ix);
					if (index < Count)
						RemoveAt(index);

					if (ix >=0 && ix < index)
						index--;

					List.Insert(index, value);
             
					if (MemberAdded != null)
						MemberAdded(this, value);
				}
				else if(x is string)
				{
					int ix = IndexOf(value.Name);
					if (ix < 0)
						ix = Count;
					this[ix] = value;
				}
				else
					throw new Exception("'" + x.ToString() + "' is type '" + x.GetType().Name + "' and cannot ve used as index to a collection type '" +
						GetType().Name + ".");

			}
		}
/* -- Reimplemented because of ambiguity detected in ASP parser V 1.1 ---
		public NamedObject this[int index] 
		{ 
			get 
			{
				return List[index] as NamedObject; 
			}
			set
			{
				if (MemberAdding != null)
					MemberAdding(this, value);
				int ix = IndexOf(value.Name);
				if (ix > 0 && ix != index)
					RemoveAt(ix);
				if (index < Count)
					RemoveAt(index);

				if (ix >=0 && ix < index)
					index--;

				List.Insert(index, value);
             
				if (MemberAdded != null)
					MemberAdded(this, value);
			}
		}

		public virtual NamedObject this[string name]
		{
			get
			{
				int ix = IndexOf(name);
				if (ix >= 0)
					return this[ix];
				else if (name == "Auto")
					return this["Default"];
				else				
					return null;
			}
			set
			{
				int ix = IndexOf(value.Name);
				if (ix < 0)
					ix = Count;
				this[ix] = value;
			}
		}
*/
		internal int IndexOf(string objectName)
		{
			for (int i = 0; i < this.Count; i++)
				if (this[i].Name == objectName)
					return i;
			return -1;
		}

		private void MemberNameChanging(NamedObject member, string newName)
		{
			if(MemberChangingName != null)
				MemberChangingName(member, newName);
			NamedObject memberWithSameName = this[newName];
			if (memberWithSameName != null)
				Remove(memberWithSameName);
		}

		private void MemberNameChanged(NamedObject member, string newName)
		{
			if(MemberChangedName != null)
				MemberChangedName(member, newName);
		}
		
		#region --- Snapshot Serialization Implementation ---

		internal void TakeSnapshot()
		{
			try
			{
				snapshot = new Hashtable();

				// We have to set serialization mode
				SubGauge topGauge = ObjectModelBrowser.GetOwningTopmostGauge(this);
				bool inSerialization = false;
				if(topGauge != null)
				{
					inSerialization = topGauge.InSerialization;
					topGauge.InSerialization = true;
				}
				else
					MessageBox.Show("Collection type '" + this.GetType().Name + "' not in object tree");
				foreach (NamedObject no in this)
				{
					GaugeXmlSerializer gxs = new GaugeXmlSerializer(no, no.GetType().ToString());
					snapshot[no.Name] = gxs.ToString(false);
				}

				// Reset serialization mode
				if(topGauge != null)
					topGauge.InSerialization = inSerialization;
			}
			catch(Exception ex)
			{
				string msg = "Cannot serialize collection '" + GetType().Name + "': " + ex.Message;
				Debug.WriteLine(msg);
				Debug.WriteLine(ex.StackTrace);
				throw new Exception(msg,ex);
			}

		}

		internal NamedObjectCollection GetModifiedCollection()
		{
            if (snapshot == null)
                return this;

            NamedObjectCollection noc = (NamedObjectCollection)Activator.CreateInstance(this.GetType());
			(noc as IObjectModelNode).ParentNode = this.parent;

			foreach (NamedObject no in this) 
			{
				string original = (string)snapshot[no.Name];

                if (original != null)
                {
                    GaugeXmlSerializer gxs = new GaugeXmlSerializer(no, no.GetType().ToString());

					string gxsString = gxs.ToString(false);
					if (original.Equals(gxsString))
						continue;
#if DEBUG
					else
					{
						if(this is ThemeCollection)
						{
							Debug.WriteLine("==== Theme " + no.Name );
							Debug.WriteLine("Init: " + original );
							Debug.WriteLine("Curr: " + gxsString );
						}
					}
#endif
                }

				noc.Add(no);
			}

			return noc;
		}

		#endregion
		
  
		#region --- IObjectModelNode Implementation ---

		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set 
			{
				if(this is ThemeCollection && value == null)
					return;
				parent = value; 
			}
		}

		#endregion

        internal void DumpNames(string caption)
        {
            Debug.WriteLine(caption);
            for(int i=0; i<Count; i++)
                Debug.WriteLine("  " + this[i].Name);
        }
	}

	// ==================================================================================

	internal class NamedObjectConverter: ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(InstanceDescriptor) && 
				(value is NamedObject || value.GetType().IsSubclassOf(typeof(NamedObject))))
			{
				System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
					(new Type[] {});
				return new InstanceDescriptor(ci, new Object[] {}, false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	/// <summary>
	/// Base class for name selection converter
	/// </summary>
	internal class SelectedNameConverter : StringConverter 
	{
		protected bool addAuto = false;
		protected bool addNone = false;

		public SelectedNameConverter() { }

		protected virtual NamedObjectCollection GetNamedCollection(SubGauge gauge)
		{
			return null;
		}

		public virtual ArrayList GetNames(SubGauge gauge) 
		{
			// Get the named collection
			NamedObjectCollection coll = GetNamedCollection(gauge);

			// Build a name array from the collection items
			ArrayList values = new ArrayList();
			if(coll != null)
			{
				foreach (NamedObject namedObject in coll)
				{
					values.Add(namedObject.Name);
				}
			}
			if(addNone && !values.Contains("None"))
				values.Add("None");
			if(addNone && !values.Contains("Auto"))
				values.Add("Auto");
			return values;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{ 
			// Getting the gauge
			IObjectModelNode node = null;
			if (context != null)
			{
				node = context.Instance as IObjectModelNode;
				if(node == null)
				{
					IGaugeControl iGauge = context.Instance as IGaugeControl;
					if(iGauge != null)
						node = iGauge.Palettes;
				}
			}

			if(node == null)
				return new StandardValuesCollection(new object[] { });

			SubGauge gauge = ObjectModelBrowser.GetOwningTopmostGauge(node);

			// Return the collection
			return new StandardValuesCollection(GetNames(gauge));
		}
		
		public override bool GetStandardValuesSupported(ITypeDescriptorContext
			context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext
			context)
		{
			return true;
		}
		/*
				public static SelectedNameConverter GetSelectedNameConverter(PropertyInfo pi)
				{
					TypeConverterAttribute tc;

					tc = (TypeConverterAttribute)Attribute.GetCustomAttribute(pi, typeof(TypeConverterAttribute));

					if (tc == null)
						throw new ArgumentException("The property " + pi.Name + " does not have the TypeConverterAttribute");

					string tcs = tc.ConverterTypeName;
					Type t = Type.GetType(tcs.Substring(0, tcs.IndexOf(",")));

					if (!t.IsSubclassOf(typeof(SelectedNameConverter)))
						throw new ArgumentException("The property " + pi.Name + " is not a subclass of SelectedNameConverter");

					SelectedNameConverter snc = (SelectedNameConverter)t.GetConstructor(new Type[0]).Invoke(new object[0]);
					return snc;
				}

				public static ArrayList GetNames(Chart chart, PropertyInfo pi)
				{

					return GetSelectedNameConverter(pi).GetNames(chart);
				}
				*/
	}
}
