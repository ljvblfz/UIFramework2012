using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Runtime.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Determines the kind of values.
	/// </summary>
	internal enum ValueKind
	{
		/// <summary>
		/// The values are generated automatically.
		/// </summary>
		Auto,
		/// <summary>
		/// The values are set by the user.
		/// </summary>
		UserDefined
	};

	// -------------------------------------------------------------------------------------------------------
	//	Coordinate Set Class
	// -------------------------------------------------------------------------------------------------------
/// <summary>
/// Abstract base class for numeric, DateTime, index and enumerated coordinate sets. Coordinate sets are used in axes and coordinate
/// planes annotation.
/// </summary>
	[TypeConverter(typeof(CoordinateSetConverter))]
	public abstract class CoordinateSet/* : ChartObject*/
	{
		private Axis			axis;
		internal ArrayList	valueList = new ArrayList();
		private ValueKind	stepValueKind = ValueKind.Auto;  
		private ValueKind	minValueKind = ValueKind.Auto;  
		private ValueKind	maxValueKind = ValueKind.Auto;  
		private ValueKind	valuesValueKind = ValueKind.Auto;  
		private bool			valueChanged = true;

		/// <summary>
		/// Initialises a new instance of a <see cref="CoordinateSet"/> class with a specified axis.
		/// </summary>
		/// <param name="axis">Axis this coordinate set belongs to.</param>
		protected CoordinateSet(Axis axis)
		{
			this.axis = axis;
		}

		internal CoordinateSet GetCopy()
		{
			CoordinateSet copy = (CoordinateSet)MemberwiseClone();
			copy.ValueList = new ArrayList(ValueList);
			return copy;
		}
		/// <summary>
		/// Initialises a new instance of a <see cref="CoordinateSet"/> class with default parameters.
		/// </summary>
		protected CoordinateSet() : this(null) { }

		#region --- Public Properties ---
	
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(ValueKind.Auto)]
		internal ValueKind MinimumMethod
		{
			get { return minValueKind; }
			set
			{
				if(minValueKind != value)
					valueChanged = true;
				if(minValueKind == ValueKind.Auto && value == ValueKind.UserDefined && axis != null)
				{
					// Set initial value to the same
					SetInitialUserDefinedMinimum();
				}
				minValueKind = value;
			}
		}

		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(ValueKind.Auto)]
		internal ValueKind MaximumMethod
		{
			get { return maxValueKind; }
			set
			{
				if(maxValueKind != value)
					valueChanged = true;
				if(maxValueKind == ValueKind.Auto && value == ValueKind.UserDefined && axis != null)
				{
					// Set initial value to the same
					SetInitialUserDefinedMaximum();
				}
				maxValueKind = value;
			}
		}
	

		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(ValueKind.Auto)]
		internal ValueKind StepMethod
		{
			get { return stepValueKind; }
			set
			{
				if(stepValueKind != value)
					valueChanged = true;
				if(stepValueKind == ValueKind.Auto && value == ValueKind.UserDefined && axis != null)
				{
					// Set initial value to the same
					SetInitialUserDefinedStep();
				}
				stepValueKind = value;
			}
		}


		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(ValueKind.Auto)]
		internal ValueKind ValuesMethod
		{
			get { return valuesValueKind; }
			set
			{
				if(valuesValueKind != value)
					valueChanged = true;
				valuesValueKind = value;
			}
		}

        /// <summary>
        /// Gets the count of the <see cref="Coordinate"/>s in this <see cref="CoordinateSet"/> object.
        /// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count { get { return ValueList.Count; } }

		/// <summary>
		/// Indicates the <see cref="Coordinate"/> at the specified indexed location in this <see cref="CoordinateSet"/> object. 
		/// </summary>
		/// <param name="i">Zero-based index to retrieve a <see cref="Coordinate"/> from this <see cref="CoordinateSet"/> object.</param>
		public Coordinate this[int i] 
		{ 
			get 
			{ 
				if(ValueList[i] is Coordinate)
					return (Coordinate)(ValueList[i]);
				else
				{
					Coordinate coord = new Coordinate(Axis.Dimension,ValueList[i]);
					coord.Offset = Axis.Dimension.Coordinate(ValueList[i]);
					coord.Width = Axis.Dimension.Width(ValueList[i]);
					valueList[i] = coord;
					return coord;
				}
			} 
			set 
			{
				if(valueList == null)
					ComputeValueList();
				SetMember(i,value);
			}
		}

		/// <summary>
		/// Clears all coordinates in this <see cref="CoordinateSet"/> object.
		/// </summary>
		/// <returns>This <see cref="CoordinateSet"/> object.</returns>
		public CoordinateSet Clear()
		{
			valueList = new ArrayList();
			valueChanged = false;
			return this;
		}


		/// <summary>
		/// Finds a <see cref="Coordinate"/> in this <see cref="CoordinateSet"/> object.
		/// </summary>
		/// <param name="obj">value of the coordinate.</param>
		/// <returns><see cref="Coordinate"/> in this <see cref="CoordinateSet"/> object.</returns>
		public Coordinate Find(object obj)
		{
			for(int i=0; i<Count; i++)
			{
				if(this[i].Value == obj || this[i] == obj)
					return this[i];
			}
			return null;
		}

		/// <summary>
		/// Removes a coordinate.
		/// </summary>
		/// <param name="obj">Coordinatre to remove.</param>
 		/// <remarks>
		/// <paramref name="obj" /> is either a <see cref="Coordinate"/> object or a <see cref="Value"/> property of a coordinate removed.
		/// </remarks>
		public void Remove(object obj)
		{
			Coordinate toRemove = null;
			for(int i=0; i<Count; i++)
			{
				if(this[i].Value == obj || this[i] == obj)
				{
					toRemove = this[i];
					break;
				}
			}
			if(toRemove != null)
				valueList.Remove(toRemove);
		}

		/// <summary>
		/// Removes the first <see cref="Coordinate"/> from this <see cref="CoordinateSet"/> object.
		/// </summary>
		/// <returns>This <see cref="CoordinateSet"/> object.</returns>
		public CoordinateSet RemoveFirst()
		{
			if(ValueList != null && valueList.Count>0)
				valueList.Remove(valueList[0]);
			return this;
		}

		/// <summary>
		/// Removes the last <see cref="Coordinate"/> from this <see cref="CoordinateSet"/> object.
		/// </summary>
		/// <returns>This <see cref="CoordinateSet"/> object.</returns>
		public CoordinateSet RemoveLast()
		{
			if(ValueList != null && valueList.Count>0)
				valueList.Remove(valueList[valueList.Count-1]);
			return this;
		}

		/// <summary>
		/// Refreshes the values of this <see cref="CoordinateSet"/> to reflect current <see cref="Axis"/> value range.
		/// </summary>
		/// <remarks>
		/// Should be used after the DataBind() call and when <see cref="Axis.MinValue"/> and/or <see cref="Axis.MaxValue"/> properties have changed.
		/// </remarks>
		public void RefreshValueList()
		{
			valuesValueKind = ValueKind.Auto;
			ComputeValueList();
		}

		/// <summary>
		/// Adds a value or a set of values to the <see cref="CoordinateSet"/> object.
		/// </summary>
		/// <param name="obj">A single <see cref="Coordinate"/> object or a single numeric, DateTime or string value,
		/// depending on the type of coordinate values, or an IEnumerable (like an array) of such values.
		/// </param>
		public void Add(object obj)
		{
			if(obj is Coordinate)
				obj = ((Coordinate)obj).Value;
			else if(obj is string)
				SetMember(Count,obj);
			else if(obj is IEnumerable)
			{
				IEnumerable ie = obj as IEnumerable;
				IEnumerator en = ie.GetEnumerator();
				while(en.MoveNext())
					this.Add(en.Current);
			}
			else
				SetMember(Count,obj);
		}

		/// <summary>
		/// Adds multiple values to this <see cref="CoordinateSet"/> object.
		/// </summary>
		/// <param name="list">List of values to be added to this <see cref="CoordinateSet"/> object.</param>
		public void Add(params object[] list)
		{
			for(int i=0;i<list.Length;i++)
				Add(list[i]);
		}

		internal ArrayList	ValueList 
		{
			get
			{
				if(valueChanged || valueList == null)
				{
					ComputeValueList();
					valueChanged = false;
				}
				else
				{
					if(Axis != null && Axis.Dimension != null)
					{
						// If values in the valueList aren't of Coordinate type,
						// we can convert them now since we know Axis and Dimension
						DataDimension dim = Axis.Dimension;
						for(int i=0; i<valueList.Count; i++)
						{
							if(!(valueList[i] is Coordinate))
							{
								Coordinate c = new Coordinate(dim,valueList[i],dim.Width(valueList[i]),dim.Coordinate(valueList[i]));
								valueList[i] = c;
							}
						}
					}
				}

				return valueList;
			}
			set
			{
				if(value == null)
					valueList.Clear();
				else
					valueList = value;
			}
		}

		#endregion

		#region --- Internal Properties and Methods ---
		
		internal Axis Axis { get { return axis; } set { axis = value; } }
		internal bool ValueChanged 
		{
			get {return valueChanged;}
			set {valueChanged = value;}
		}

		internal abstract void ComputeValueList();

		internal abstract void SetInitialUserDefinedMinimum();
		internal abstract void SetInitialUserDefinedMaximum();
		internal abstract void SetInitialUserDefinedStep();
		internal abstract void SetMember(int i, object obj);
		#endregion
	};

	/// <summary>
	/// Used in <see cref="DataDimension"/> to define a coordinate.
	/// </summary>
	public class Coordinate
	{
		private	DataDimension	dimension;
		private object			val;
		// Hierarchical coordinate sets implementation
		private Coordinate		next;
		private Coordinate		firstChild;
		private Coordinate		parent;
		private double			width = 0;
		private double			offset = 0;
		private bool			hasMergedMembers = false;
		private Hashtable		hashTable = new Hashtable();

		#region --- Constructors ---

		internal Coordinate(DataDimension dimension, object val, double width, double offset)
		{
			this.dimension = dimension;
			this.width = width;
			this.offset = offset;

			next = null;
			firstChild = null;
			parent = null;
			if(width == 20)
				width = 20;

			if(dimension.ItemType == val.GetType())
				this.val = val;
			else if(dimension.ItemType == typeof(double))
			{
				if (val.GetType() == typeof(float))
					this.val = (double)(float)val;
				else if (val.GetType() == typeof(int))
					this.val = (double)(int)val;
				else
					throw new Exception("Object '" + val.ToString() + "' is not member of dimension '" + dimension.GetType().Name + "'");
			}
			else
				this.val = val;
		}

		internal Coordinate(DataDimension dimension, object val, double width) : this (dimension, val, width, 0) 
		{
		}

		internal Coordinate(DataDimension dimension, object val) : this (dimension, val, 0) 
		{
			if(dimension == DataDimension.StandardBooleanDimension ||
				dimension == DataDimension.StandardIndexDimension || 
				dimension is EnumeratedDataDimension)
				width = 1.0;
		}

		// Cloning only
		internal Coordinate(){}

		#endregion
		
		#region --- Properties ---		
		
		internal bool HasMergedMembers	{ get { return hasMergedMembers; } set { hasMergedMembers = value; } }
		internal bool IsMergedMember	
		{ 
			get 
			{ 
				return (Parent != null && Parent.HasMergedMembers);
			}
		}

		/// <summary>
		/// Gets or sets the data value of this <see cref="Coordinate"/> object.
		/// </summary>
        public object Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }


		/// <summary>
		/// Gets the width of this <see cref="Coordinate"/> in the Logical Coordinate Space (LCS).
		/// </summary>
		public double Width		
		{ 
			get 
			{ 
				if(dimension == DataDimension.StandardNumericDimension)
					return 0.0;
				else if(dimension == DataDimension.StandardDateTimeDimension)
					return 0.0;
				else if(dimension == DataDimension.StandardIndexDimension)
					return 1.0;
				else if(dimension == DataDimension.StandardBooleanDimension)
					return 1.0;

				if(firstChild == null)
					return width; 

				if(HasMergedMembers)
				{
					hasMergedMembers = false;
					double w = 0.0;
					Coordinate c = firstChild;
					for(; c != null; c = c.Next)
						w = Math.Max(w,c.Width);
					hasMergedMembers = true;
					return w;
				}
				else
				{
					// If the node has children, the width is summ of children's widths
					double w = 0.0;
					Coordinate c = firstChild;
					for(; c != null; c = c.Next)
						w += c.Width;
					return w;
				}
			} 
			set { width = value; } // this value will be ignored in get method if the node has children
		}

		/// <summary>
		/// Gets the position of this <see cref="Coordinate"/> in the Logical Coordinate System (LCS) defined by containing dimension.
		/// </summary>
		public double Offset
		{
			get
			{
				if(dimension is NumericDataDimension)
					return (double)val;
				else if(dimension is DateTimeDataDimension)
					return dimension.Coordinate(val);
				else if(dimension is IndexDataDimension)
					return (double)(int)val;
				else if(dimension == DataDimension.StandardBooleanDimension)
					return ((bool)val)? 1.0:0.0;

				// For the enumerated dimension

				if(IsMergedMember)
					return Parent.Offset;
				// Otherwise use parent's offset and widths of preceeding siblings
				if(Parent != null)
				{
					Coordinate p = Parent;
					offset = p.Offset;
					Coordinate c = p.FirstChild;
					while(c != this)
					{
						if(c == null)
							return 0;
						offset += c.Width;
						c = c.Next;
					}
					return offset;
				}

				return offset;
			}
			set { offset = value; }
		}
		/// <summary>
		/// Gets the subnode with value specified as index.
		/// </summary>
		/// <remarks>
		/// The search is first done over subnodes and the first subnode with the value specified as index
		/// is returned. If such a subnode is not found, then the same query is 
		/// recursively aplied to subnodes. 
		/// The method guatanties an extended indexing behaviour: if node <c>N</c> has exactly one subnode <c>S</c> with value <c>v</c>,
		/// them <c>N[v]</c> = <c>S</c>.
		/// </remarks>
		internal Coordinate this[object obj]
		{	
			get
			{
				if(IsEqual(Value,obj))
					return this;
				// Try the children 
				Coordinate coo = hashTable[obj] as Coordinate;
				if(coo != null)
					return coo;
				Coordinate c = firstChild;
				for(; c != null; c = c.Next)
				{
					if(IsEqual(c.Value,obj))
						return c;
				}
				// Try the children recursive
				c = firstChild;
				for(; c != null; c = c.Next)
				{
					Coordinate cc = c[obj];
					if(cc != null)
						return cc;
				}
				return null;
			}
		}

		#endregion		
		
        #region --- Reordering ---

        /// <summary>
        /// Moves this Coordinate to the front of every composite series in ancestorial hirearchy
        /// </summary>
        internal void MoveToFront(ChartBase owningChart)
        {
            //get the composite series represented by the parent
            if (Parent == null)
                return;
            
            CompositeSeries parentSeries = owningChart.Series.FindCompositeSeries((String)Parent.Value);

            if (parentSeries.CompositionKind == CompositionKind.Sections && Parent.FirstChild != this)
            {
                //find previous series in the list
                Coordinate previous = Parent.FirstChild;
                while (previous.Next != this)
                    previous = previous.Next;
                
                //move this to the front of the list
                previous.Next = this.Next;
                this.Next = Parent.FirstChild;
                Parent.FirstChild = this;
            }

            //move to the front of every parent composite series with Sections composition
            if (Parent != null)
                Parent.MoveToFront(owningChart);
        }

        /// <summary>
        /// Moves this Coordinate to the back of every composite series in ancestorial hirearchy
        /// </summary>
        internal void MoveToBack(ChartBase owningChart)
        {
            //get the composite series represented by the parent
            if (Parent == null)
                return;

            CompositeSeries parentSeries = owningChart.Series.FindCompositeSeries((String)Parent.Value);

            if (parentSeries.CompositionKind == CompositionKind.Sections && Parent.LastChild != this)
            {
                //find previous series in the list
                Coordinate previous = null;
                if (Parent.FirstChild != this)
                {
                    previous = Parent.FirstChild;
                    while (previous.Next != this)
                        previous = previous.Next;
                }

                //move this to the back of the list
                if (previous != null)
                    previous.Next = this.Next;
                else //this was the first child
                    Parent.FirstChild = this.Next;

                this.Next = null;
                Parent.LastChild.Next = this;
            }

            //move to the back of every parent composite series with Sections composition
            if (Parent != null)
                Parent.MoveToBack(owningChart);
        }

        #endregion

        #region --- Navigation ---
        internal Coordinate Next		{ get { return next; }			set { next = value; } }
		internal Coordinate Parent		{ get { return parent; }		set { parent = value; } }
		internal Coordinate FirstChild	{ get { return firstChild; }	set { firstChild = value; } }
		internal Coordinate LastChild	
		{
			get 
			{ 
				Coordinate item = firstChild;
				if (item==null)
					return item;
				while(item.Next != null)
					item = item.Next;
				return item;
			}
		}

		internal Coordinate NextPostfix
		{
			get
			{
				// If there is next, go to its fist leaf
				if(Next != null)
				{
					Coordinate node = Next;
					while(node.FirstChild != null)
						node = node.FirstChild;
					return node;
				}
				else
					return Parent;
			}
		}
		internal Coordinate NextPrefix
		{
			get
			{
				// If there is child, go to it
				if(FirstChild != null)
					return FirstChild;
				else // starting from this, climbing the branch, find node with sibling
				{
					Coordinate node = this;
					while(node != null && node.Next == null)
						node = node.Parent;
					if(node == null)
						return null;
					else
						return node.Next;
				}
			}
		}

		internal int GenerationOf(Coordinate child)
		{
			Coordinate item = child;
			int gen = 0;
			while(item != null && item != this)
			{
				item = item.Parent;
				gen++;
			}
			if(item == null)
				return -1;
			else
				return gen;
		}

		internal void UpdateOffsetWidth(DataDimension dim)
		{
			offset = dim.Coordinate(Value);
			width = dim.Width(Value);
		}

		#endregion

		#region --- Building ---		
		private void AddCoordinate(Coordinate item)
		{
			item.Parent = this;
			Coordinate c = FirstChild;
			if(c == null)
				firstChild = item;
			else
			{
				while(c.Next != null)
					c = c.Next;
				c.Next = item;
			}
			if(!hashTable.ContainsKey(item.Value))
				hashTable.Add(item.Value,item);
			else
				hashTable[item.Value] = item;

		}

		/// <summary>
		/// Adds multiple child <see cref="Coordinate"/>s.
		/// </summary>
		/// <param name="list">Coordinate values.</param>
		/// Items of the <paramref name="list" /> can be either <see cref="Coordinate"/> objects or <see cref="Value"/> property of a coordinate added.
		/// </remarks>
		public void Add(params object[] list)
		{
			foreach(object obj in list)
				Add(obj);
		}

		/// <summary>
		/// Adds a child <see cref="Coordinate"/>.
		/// </summary>
		/// <param name="value">Coordinate value.</param>
		/// <returns>Newly created coordinate.</returns>
		/// <remarks>
		/// <paramref name="value" /> is either a <see cref="Coordinate"/> object or a <see cref="Value"/> property of a coordinate added.
		/// </remarks>
		public Coordinate Add(object value)
		{
			if(value is Coordinate)
			{
				AddCoordinate(value as Coordinate);
				return value as Coordinate;
			}
			else
			{
				Coordinate item = new Coordinate(dimension,value);
				AddCoordinate(item);
				return item;
			}
		}

		/// <summary>
		/// Removes this <see cref="Coordinate"/> object from its parent.
		/// </summary>
		public void Remove()
		{
			if(parent == null)
				return;
			Coordinate prev = null, cur = parent.FirstChild;
			while(cur != null)
			{
				if(cur == this)
				{
					if(prev == null)
						parent.FirstChild = Next;
					else
						prev.Next = Next;
					break;
				}
				else
				{
					prev = cur;
					cur = cur.Next;
				}
			}
			parent.hashTable.Remove(this.Value);
		}
		#endregion
		
		#region --- Members Enumeration ---

		/// <summary>
		/// Retrieves the <see cref="Coordinate"/>s at a specified generation (level).
		/// </summary>
		/// <param name="gen">Generation or level at which the <see cref="Coordinate"/>s are retrieved.</param>
		/// <returns>List of coordinates at a specified generation.</returns>
		public ArrayList ListGeneration(int gen)
		{
			ArrayList list = new ArrayList();
			AppendGeneration(ref list,gen);
			return list;
		}

		private void AppendGeneration(ref ArrayList list, int gen)
		{
			if(gen<0)
				return;
			if(gen == 0)
				list.Add(this);
			else
			{
				Coordinate item = FirstChild;
				while(item != null)
				{
					item.AppendGeneration(ref list, gen-1);
					item = item.Next;
				}
			}
		}

		/// <summary>
		/// Returns a list of leaves of this <see cref="Coordinate"/> object.
		/// </summary>
		/// <returns>Leaves of this <see cref="Coordinate"/> object.</returns>
		/// <remarks>
		/// A leaf is the object that does not have any children.
		/// </remarks>
		public ArrayList ListLeaves()
		{
			ArrayList list = new ArrayList();
			AppendLeaves(ref list);
			return list;
		}

		private void AppendLeaves(ref ArrayList list)
		{
			if(FirstChild == null)
			{
				list.Add(this);
			}
			else
			{
				Coordinate item = FirstChild;
				while(item != null)
				{
					item.AppendLeaves(ref list);
					item = item.Next;
				}
			}
		}

		#endregion

		#region --- Private helpers ---
		private bool IsEqual(object obj1, object obj2)
		{
			// Unboxes and compares value types int, double, DateTime and string.
			// Other types are compared as reference types
			if(obj1 == obj2)
				return true;
			if(obj1==null && obj2!=null ||obj2==null && obj1!=null)
				return false;

			if(obj1.GetType() == typeof(int) && obj2.GetType() == typeof(int))
				return ((int)obj1 == (int)obj2);
			else if (obj1.GetType() == typeof(double) && obj2.GetType() == typeof(double))
				return ((double)obj1 == (double)obj2);
			else if (obj1.GetType() == typeof(DateTime) && obj2.GetType() == typeof(DateTime))
				return ((DateTime)obj1 == (DateTime)obj2);
			else if (obj1.GetType() == typeof(string) && obj2.GetType() == typeof(string))
				return ((string)obj1 == (string)obj2);

			return false;

		}
		#endregion
	
	}

}
