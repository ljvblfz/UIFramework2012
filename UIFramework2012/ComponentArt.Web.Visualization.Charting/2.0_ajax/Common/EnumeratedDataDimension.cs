using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents an enumerated data dimension.
	/// </summary>
	//===========================================================================================================
	
	public class EnumeratedDataDimension : DataDimension
	{
		private static double standardDepth = 10;
		
		private Coordinate root = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumeratedDataDimension"/> class with specified name, item type and coordinates.
		/// </summary>
		/// <param name="name">Name of the enumerated dimension.</param>
		/// <param name="itemType">Type of the items in this dimension.</param>
		/// <param name="coordinates">Coordinates of this dimension.</param>
		public EnumeratedDataDimension(string name, Type itemType, object[] coordinates) : base(name, itemType) 
		{
			root = new Coordinate(this,name);
			if(coordinates != null)
				Add(coordinates);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumeratedDataDimension"/> class with string coordinates and with specified name and coordinates.
		/// </summary>
		/// <param name="name">Name of the enumerated dimension.</param>
		/// <param name="coordinates">Coordinates of this dimension.</param>
		public EnumeratedDataDimension(string name, string[] coordinates) : this(name, typeof(string), coordinates) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumeratedDataDimension"/> class with specified name and item type.
		/// </summary>
		/// <param name="name">Name of the enumerated dimension.</param>
		/// <param name="itemType">Type of the items in this dimension.</param>
		public EnumeratedDataDimension(string name, Type itemType) : this(name,itemType, null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumeratedDataDimension"/> class with specified name.
		/// </summary>
		/// <param name="name">Name of the enumerated dimension.</param>
		public EnumeratedDataDimension(string name) : this(name, typeof(object)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumeratedDataDimension"/> class with default parameters.
		/// </summary>
		public EnumeratedDataDimension() : this("") { }

		public static double StandardDepth { get { return standardDepth; } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal Coordinate Root 
		{
			get 
			{
				return root; 
			}
		}

		/// <summary>
		/// Gets a coordinate of an object in this data dimension.
		/// </summary>
		/// <param name="obj">Object whose coordinate we want to retrieve.</param>
		public virtual Coordinate this[object obj] 
		{ 
			get 
			{
				Coordinate item = root[obj]; 
				if(item == null && OwningChart !=null && OwningChart.InDesignMode)
				{	// In design mode we add objects to the dimension
					root.Add(obj);
					return root[obj];
				}
				return item;
			} 
		}

		/// <summary>
		/// Adds objects to this <see cref="DataDimension"/>.
		/// </summary>
		/// <param name="list">List of objects to add.</param>
		public void Add(params object[] list)
		{
			foreach(object obj in list)
				Add(obj);
		}
		/// <summary>
		/// Adds objects to this <see cref="DataDimension"/>.
		/// </summary>
		/// <param name="obj">Object to add.</param>
		/// <returns>Coordinate correponding to <paramref name="obj" />.</returns>
		public Coordinate Add(object obj)
		{
			Coordinate item;
			if(obj is Coordinate)
				item = obj as Coordinate;
			else
				item = new Coordinate(this,obj);
			if(root.FirstChild == null && ReferenceValue == null)
				ReferenceValue = obj;
			root.Add(item);
			return item;
		}
		
		/// <summary>
		/// Retrieves the value of an object in the Logical Coordinate System.
		/// </summary>
		/// <param name="obj">object whose coordinate is retrieved.</param>
		/// <returns>The value of an object in the Logical Coordinate System.</returns>
		public override double Coordinate(object obj)
		{
			if(obj == null)
				throw new Exception("Enumerated dimension '" + Name + "': coordinate of a null object is not defined");
			if(this[obj] == null)
				throw new Exception("Enumerated dimension '" + Name + "' does not contain member '" + obj.ToString() + "'");
			return FirstMemberCoordinate + this[obj].Offset;
		}

		public override double Width(object obj) 
		{
			if(obj == null)
				throw new Exception("Enumerated dimension '" + Name + "': width of a null object is not defined");
			if(this[obj] == null)
				throw new Exception("Enumerated dimension '" + Name + "' does not contain member '" + obj.ToString() + "'");
			return this[obj].Width; 
		}

		/// <summary>
		/// Returns the dimension element at a given logical coordinate.
		/// </summary>
		/// <param name="logicalCoordinate">Logical coordinate of the dimension element.</param>
		/// <returns>An Object representing the element at a given logical coordinate. 
		/// To get the String value cast the return value. Return value is null if no element covers the given coordinate.</returns>
		public override object ElementAt(double coordinate)
		{
			Coordinate cc = GetElementAt(root,coordinate);
			if(cc != null)
				return cc.Value;
			else
				return null;
		}

        /// <summary>
        /// Returns the dimension element given a string representation of it.
        /// </summary>
        /// <param name="value">String representation of the dimension element.</param>
        /// <returns>Object representing the dimension element.</returns>
        public override object ValueOf(string value)
        {
            if (root.Value is String)
                return value;
            else if (root.Value is Double)
                return Double.Parse(value);
            else if (root.Value is Int32)
                return Int32.Parse(value);
            else if (root.Value is Int64)
                return Int64.Parse(value);
            else if (root.Value is Int16)
                return Int16.Parse(value);
            else if (root.Value is Boolean)
                return Boolean.Parse(value);
            else
            {
                try
                {
                    return TypeDescriptor.GetConverter(root.Value).ConvertFromString(value);
                }
                catch (Exception)
                {
                    //print out error message that the custom dimension must implement a TypeConverter
                }
            }
            return null;
        }

		private Coordinate GetElementAt(Coordinate node,double x)
		{
			if(x<node.Offset || x>node.Offset+node.Width)
				return null;
			if(node.FirstChild == null)
				return node;
			Coordinate c = node.FirstChild;
			while(c != null)
			{
				Coordinate r = GetElementAt(c,x);
				if(r != null)
					return GetElementAt(r,x);
				else
					c = c.Next;

			}
			return null;
		}

		private class PS // Presedence structure
		{
			public Coordinate item;
			public ArrayList prev = new ArrayList();
			public int count = 0;

			public PS(Coordinate item, int count)
			{
				this.item = item;
				this.count = count;
			}
		};
		
		internal override DataDimension Merge(DataDimension dim)
		{
			int i,j,n;

			EnumeratedDataDimension dim1 = dim as EnumeratedDataDimension;
			if(dim1 == null)
				throw new Exception("Cannot combine 'NumericDataDimension' and '" + dim.GetType().Name +"'");
			if(ItemType != dim1.ItemType)
				throw new Exception("Cannot combine '" + ItemType.Name + "' and '" + dim1.ItemType.Name +"'");
	
			if(ItemType == typeof(bool))
				return this;
			if(dim == this)
				return this;

			if(ItemType != typeof(string))
				throw new Exception("Cannot handle '"+ItemType.Name+"' enumerated dimensions");

			
			// Count members and check the hierarchy depth
			int count = 0, count1 = 0;
			Coordinate c = root.FirstChild;
			while(c != null)
			{
				count++;
				c = c.Next;
				if(c != null && c.FirstChild != null)
					throw new Exception("Cannot combine hierarchical enumerated dimensions");
			}
			c = dim1.Root.FirstChild;
			while(c != null)
			{
				count1++;
				c = c.Next;
				if(c != null && c.FirstChild != null)
					throw new Exception("Cannot combine hierarchical enumerated dimensions");
			}
			if(count == 0)
				return dim;
			if(count1 == 0)
				return this;

			// Populating the presedence list. Checking if input sequences are ordered strings
			bool ascending = true;
			bool descending = true;
			c = root.FirstChild;
			PS p = null;
			PS[] list = new PS[count+count1];
			for(i=0; i< count; i++)
			{
				list[i] = new PS(c,0);
				if(p != null)
					list[i].prev.Add(p);
				p = list[i];
				string thisStr = (string)c.Value;
				c=c.Next;
				if(c != null)
				{
					string nextStr = (string)c.Value;
					if(thisStr.CompareTo(nextStr)>0)
						ascending = false;
					else if(thisStr.CompareTo(nextStr)<0)
						descending = false;
				}
			}
			n = count;
			
			p = null;
			c = dim1.Root.FirstChild;
			for(i=0; i< count1; i++)
			{
				bool found = false;
				for(j=0;j<count;j++)
				{
					if(c.Value == list[j].item.Value)
					{
						found = true;
						if(p != null)
							list[j].prev.Add(p);
						p = list[j];
						break;
					}
				}
				if(!found)
				{
					list[n] = new PS(c,0);
					if(p != null)
						list[n].prev.Add(p);

					p = list[n];
					n++;
				}
				string thisStr = (string)c.Value;
				c=c.Next;
				if(c != null)
				{
					string nextStr = (string)c.Value;
					if(thisStr.CompareTo(nextStr)>0)
						ascending = false;
					else if(thisStr.CompareTo(nextStr)<0)
						descending = false;
				}
			}

			// Try alphabetic ordering
			if(ascending || descending)
			{
				// Aphabetic sort
				for(i=0;i<n-1;i++)
					for(j=i+1;j<n;j++)
					{
						int iComparedToj = ((string)list[i].item.Value).CompareTo(((string)list[j].item.Value));
						if(descending && iComparedToj < 0  || ascending && iComparedToj > 0)
						{
							PS ps = list[i];
							list[i] = list[j];
							list[j] = ps;
						}
					}
			}
			else
			{
				// Iteration
				bool toDoNextStep = true;
				while(toDoNextStep)
				{
					toDoNextStep = false;
					for(i=0;i<n;i++)
					{
						foreach(PS ps in list[i].prev)
						{
							int cnt1 = ps.count + 1;
							if(cnt1>list[i].count)
							{
								list[i].count = cnt1;
								if(cnt1 >= n)
								{
									toDoNextStep = false; // to prevent loops
									break;
								}
								else
									toDoNextStep = true;
							}
						}
					}
				}

				// Sort by counts
				for(i=0;i<n-1;i++)
					for(j=i+1;j<n;j++)
					{
						if(list[i].count > list[j].count)
						{
							PS ps = list[i];
							list[i] = list[j];
							list[j] = ps;
						}
					}
			}

			// Remove duplicates

			int k=1;
			for(i=1;i<n;i++)
			{
				bool found = false;
				for(j=0; j<k; j++)
				{
					if((string)list[j].item.Value == (string)list[i].item.Value)
					{
						found = true;
						break;
					}
				}
				if(!found)
				{
					list[k] = list[i];
					k++;
				}
			}

			n = k;

			EnumeratedDataDimension merged = new EnumeratedDataDimension(Name+"+"+dim.Name,typeof(string));
			for(i=0;i<n;i++)
				merged.Add(list[i].item.Value);

			return merged;
		}

		internal override void Rename(string oldName, string newName) 
		{ 
			Coordinate coo = this[oldName];
			if(coo != null)
				coo.Value = newName;
		}

		internal override int Compare(object coordinate1, object coordinate2)
		{
			if(this[coordinate1] == null || this[coordinate2] == null)
				return -2;
			double c1 = Coordinate(coordinate1);
			double c2 = Coordinate(coordinate2);
			if(c1 < c2)
				return -1;
			if(c1 > c2)
				return 1;
			double w1 = Width(coordinate1);
			double w2 = Width(coordinate2);
			if(w1 < w2)
				return -1;
			if(w1 > w2)
				return 1;
			return 0;
		}

		internal virtual DimensionSpan CreateSpan()
		{
			return new IntDimensionSpan();
		}
	}

	public class IntDimensionSpan : DimensionSpan
	{
		int step;
		public IntDimensionSpan()
		{
			step = 1;
		}
		public int Step { get { return step; } set { step = value; } }

	}
}


