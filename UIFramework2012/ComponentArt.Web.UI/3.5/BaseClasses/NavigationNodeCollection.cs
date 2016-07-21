using System;
using System.ComponentModel;
using System.Collections;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="NavigationNode"/> objects. 
  /// </summary>
	[ToolboxItem(false)]
	public abstract class NavigationNodeCollection : IEnumerable, ICollection, IList
	{
    protected ArrayList nodeList;
    protected NavigationNode parentNode;
    protected BaseNavigator navigator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="oNavigator">The parent <see cref="BaseNavigator"/> control.</param>
    /// <param name="oParent">The parent node of the collection, or null if this is the top-level collection.</param>
    public NavigationNodeCollection(BaseNavigator oNavigator, NavigationNode oParent) 
		{
			parentNode = oParent;
      navigator = oNavigator;
			nodeList = new ArrayList();
		}
		
    internal NavigationNode this[int index] 
		{
			get 
			{
				return (NavigationNode)nodeList[index];
			}
		}

    object IList.this[int index] 
		{
			get 
			{
				return nodeList[index];
			}
			set 
			{
				nodeList[index] = (NavigationNode)value;
			}
		}
    
		internal int Add(NavigationNode item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			
			// Fix some pointers.
			item.parentNode = parentNode;
      item.navigator = navigator;
      
      if(item.parentNode != null && navigator == null)
      {
        item.navigator = item.parentNode.navigator;
      }
      
      if(nodeList.Count > 0)
      {
        item.previousSibling = (NavigationNode)nodeList[nodeList.Count - 1];
        ((NavigationNode)nodeList[nodeList.Count - 1]).nextSibling = item;
      }
      
			nodeList.Add(item);
			
			return nodeList.Count - 1;
		}
        
    /// <summary>
    /// Removes all elements from the collection.
    /// </summary>
    public void Clear() 
		{
			nodeList.Clear();
		}
            
		internal bool Contains(NavigationNode item) 
		{
			if (item == null) 
			{
				return false;
			}
			return nodeList.Contains(item);
		}

		internal int IndexOf(NavigationNode item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			return nodeList.IndexOf(item);
		}

		internal void Insert(int index, NavigationNode item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}

      // Fix some pointers.
      item.parentNode = parentNode;
      item.navigator = navigator;
      
      if(item.parentNode != null && navigator == null)
      {
        item.navigator = item.parentNode.navigator;
      }
      
      if(nodeList.Count > 0)
      {
        item.previousSibling = (NavigationNode)nodeList[nodeList.Count - 1];
        ((NavigationNode)nodeList[nodeList.Count - 1]).nextSibling = item;
      }

			nodeList.Insert(index,item);
		}

    /// <summary>
    /// Removes the element at the specified index of the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
		public void RemoveAt(int index) 
		{
      nodeList.RemoveAt(index);
		}
    
		internal void Remove(NavigationNode item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}

      if(item.previousSibling != null)
      {
        item.previousSibling.nextSibling = item.nextSibling;
      }
      if(item.nextSibling != null)
      {
        item.nextSibling.previousSibling = item.previousSibling;
      }

			int index = IndexOf(item);
			if (index >= 0) 
			{
				RemoveAt(index);
			}
		}

    /// <summary>
    /// Sort the nodes in this collection using the given node comparer.
    /// </summary>
    /// <param name="comparer">The IComparer to use for comparing nodes</param>
    /// <param name="bRecursive">Whether to sort all levels beneath this one</param>
    public void Sort(IComparer comparer, bool bRecursive)
    {
      nodeList.Sort(comparer);

      if(bRecursive)
      {
        foreach(NavigationNode oNode in nodeList)
        {
          oNode.nodes.Sort(comparer, true);
        }
      }
    }

    /// <summary>
    /// Sort the nodes in this collection by the given property.
    /// </summary>
    /// <param name="sPropertyName">The name of the property to sort by</param>
    /// <param name="bDescending">Whether to sort in a descending order</param>
    /// <param name="bNumeric">Whether to sort numerically instead of alphabetically (all values must be numbers)</param>
    /// <param name="bCaseSensitive">Whether to take case into account when sorting</param>
    /// <param name="bRecursive">Whether to recursively sort all levels beneath this one</param>
    public void Sort(string sPropertyName, bool bDescending, bool bNumeric, bool bCaseSensitive, bool bRecursive)
    {
      this.Sort(new CustomComparer(sPropertyName, bDescending, bNumeric, bCaseSensitive), bRecursive);
    }

    /// <summary>
    /// Sort the nodes in this collection by the given property.
    /// </summary>
    /// <param name="sPropertyName">The name of the property to sort by</param>
    /// <param name="bDescending">Whether to sort in a descending order</param>
    /// <param name="bNumeric">Whether to sort numerically instead of alphabetically (all values must be numbers)</param>
    /// <param name="bRecursive">Whether to recursively sort all levels beneath this one</param>
    public void Sort(string sPropertyName, bool bDescending, bool bNumeric, bool bRecursive)
    {
      this.Sort(sPropertyName, bDescending, bNumeric, true, bRecursive);
    }

    /// <summary>
    /// Sort the nodes in this collection by the given property.
    /// </summary>
    /// <param name="sPropertyName">The name of the property to sort by</param>
    /// <param name="bDescending">Whether to sort in a descending order</param>
    /// <param name="bRecursive">Whether to recursively sort all levels beneath this one</param>
    public void Sort(string sPropertyName, bool bDescending, bool bRecursive)
    {
      this.Sort(sPropertyName, bDescending, false, bRecursive);
    }

    /// <summary>
    /// Sort the nodes in this collection by the given property.
    /// </summary>
    /// <param name="sPropertyName">The name of the property to sort by</param>
    /// <param name="bDescending">Whether to sort in a descending order</param>
    public void Sort(string sPropertyName, bool bDescending)
    {
      this.Sort(sPropertyName, bDescending, false);
    }

    /// <summary>
    /// Sort the nodes in this collection by Text, ascending.
    /// </summary>
    public void Sort()
    {
      this.Sort("Text", false);
    }

    #region CustomComparer (sorting)

    private class CustomComparer : IComparer  
    {
      private string PropertyName;
      private bool Reverse;
      private bool Numeric;
      private bool CaseSensitive;


      public CustomComparer(string sPropertyName, bool bReverse, bool bNumeric, bool bCaseSensitive)
      {
        PropertyName = sPropertyName;
        Reverse = bReverse;
        Numeric = bNumeric;
        CaseSensitive = bCaseSensitive;
      }

      int IComparer.Compare( Object a, Object b )  
      {
        string sA = ((NavigationNode)a).Properties[PropertyName];
        string sB = ((NavigationNode)b).Properties[PropertyName];

        // Do we need to reverse?
        if(Reverse)
        {
          string sTemp = sA;
          sA = sB;
          sB = sTemp;
        }

        // Do we need to desensitize?
        if(!CaseSensitive)
        {
          sA = sA.ToLower();
          sB = sB.ToLower();
        }

        if(Numeric)
        {
          return int.Parse(sA) - int.Parse(sB);
        }
        else
        {
          return string.Compare(sA, sB);
        }
      }
    }

    #endregion

    #region IEnumerable Implementation
		public IEnumerator GetEnumerator() 
		{
			return nodeList.GetEnumerator();
		}
        #endregion IEnumerable Implementation

    #region ICollection Implementation
    
    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    /// <value>
    /// The number of elements contained in the collection.
    /// </value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count 
		{
			get 
			{
				return nodeList.Count;
			}
		}

    /// <summary>
    /// Copies the entire collection to a compatible one-dimensional Array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from 
    /// this collection. The <b>Array</b> must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		public void CopyTo(Array array, int index) 
		{
			nodeList.CopyTo(array,index);
		}

    /// <summary>
    /// Gets a value indicating whether access to the collection is synchronized (thread safe).
    /// </summary>
    /// <value>
    /// <b>true</b> if access to the collection is synchronized (thread safe); otherwise, <b>false</b>. The default is <b>false</b>.
    /// </value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSynchronized 
		{
			get 
			{
				return true;
			}
		}

    /// <summary>
    /// Gets an object that can be used to synchronize access to the collection.
    /// </summary>
    /// <value>
    /// An object that can be used to synchronize access to the collection.
    /// </value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SyncRoot 
		{
			get 
			{
				return nodeList.SyncRoot;
			}
		}
        #endregion ICollection Implementation

    #region IList Implementation
   
    /// <summary>
    /// Gets a value indicating whether the collection has a fixed size.
    /// </summary>
    /// <value>
    /// <b>true</b> if the collection has a fixed size; otherwise, <b>false</b>. The default is <b>false</b>.
    /// </value>
		bool IList.IsFixedSize 
		{
			get 
			{
				return false;
			}
		}

    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    /// <value>
    /// <b>true</b> if the collection is read-only; otherwise, <b>false</b>. The default is <b>false</b>.
    /// </value>
		bool IList.IsReadOnly 
		{
			get 
			{
				return false;
			}
		}

		int IList.Add(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is NavigationNode)) 
			{
				throw new ArgumentException("item must be a NavigationNode");
			}

			return Add((NavigationNode)item);
		}

		void IList.Clear() 
		{
			Clear();
		}

		bool IList.Contains(object item) 
		{
			return Contains(item as NavigationNode);
		}

		int IList.IndexOf(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is NavigationNode)) 
			{
				throw new ArgumentException("item must be a NavigationNode");
			}

			return IndexOf((NavigationNode)item);
		}

		void IList.Insert(int index, object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is NavigationNode)) 
			{
				throw new ArgumentException("item must be a NavigationNode");
			}

			Insert(index, (NavigationNode)item);
		}

		void IList.Remove(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is NavigationNode)) 
			{
				throw new ArgumentException("item must be a NavigationNode");
			}

			Remove((NavigationNode)item);
		}

		void IList.RemoveAt(int index) 
		{
			RemoveAt(index);
		}
        #endregion IList Implementation
	}
}