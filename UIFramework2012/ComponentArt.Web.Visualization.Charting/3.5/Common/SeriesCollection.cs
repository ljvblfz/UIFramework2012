using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents a collection of <see cref="SeriesBase"/> objects in the <see cref="CompositeSeries"/> object.
	/// </summary>
	[NotIncludedInTemplate]
	public class SeriesCollection: NamedCollection
	{
		internal SeriesBase[] members;
		internal SeriesCollection(Object owner) 
			: base(typeof(SeriesBase), owner, new Type[] { typeof(Series),typeof(CompositeSeries)})
		{ 
		}
		internal SeriesCollection() : this(null) { }

		public override int Add(object obj)
		{
			SeriesBase SB = (SeriesBase)obj;
			if(SB == null)
				throw new Exception("Cannot add object type '" + obj.GetType().Name + "' to a 'SeriesCollection'");
			SB.SetOwner((ChartObject)Owner);
			return base.Add(SB);
		}

		internal override void OnEditStarting()
		{
			// Save names 
			members = new SeriesBase[Count];
			for(int i=0; i<Count; i++)
				members[i] = this[i];
		}

		internal override void OnEditCompleted(bool cancelled)
		{
			base.OnEditCompleted(cancelled);
			if(cancelled)
			{
				members = null;
				return;
			}
			SetCreationOfInitialContentsMode();
			
			// Check for deleted members
			for(int i=0; i<Count; i++)
			{
				for(int j=0; j<members.Length; j++)
				{
					if(members[j]!=null && members[j].Name == this[i].Name)
					{
						members[j] = null;
						break;
					}
				}
			}

			CompositeSeries series = Owner as CompositeSeries;
			for(int i=0; i<members.Length; i++)
			{
				if(members[i] != null)
				   series.NotifyMemberRemoved(members[i]);
			}
			members = null;
		}


		/// <summary>
		/// Removes the <see cref="Series"/> or <see cref="CompositeSeries"/> with the specified name.
		/// </summary>
		/// <param name="name">The object name.</param>
		public override void Remove(string name)
		{
			base.Remove(name);
			SetCreationOfInitialContentsMode();
			CompositeSeries series = Owner as CompositeSeries;
			EnumeratedDataDimension zDim = series.ZDimension as EnumeratedDataDimension;
			if(zDim != null && zDim[name] != null)
				zDim[name].Remove();
			series.SyncronizeZDimension();
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the <see cref="CollectionWithType" /> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> can be found.</param>
		/// <param name="value">The value of the element to remove from <paramref name="index" />.</param>
		protected override void OnRemove( int index, Object value )  
		{
			base.OnRemove(index,value);
			SetCreationOfInitialContentsMode();
		}
		protected override void OnRemoveComplete( int index, Object value )  
		{
		}

		/// <summary>
		/// Performs additional custom processes when clearing the contents of the <see cref="CollectionWithType" /> instance.
		/// </summary>
		protected override void OnClear()  
		{
			bool needsSetting = List.Count > 0;
			base.OnClear();
			if(needsSetting)
				SetCreationOfInitialContentsMode();
		}

		// We delay setting this mode to the point when the set of simple series is emptied
		private void SetCreationOfInitialContentsMode()
		{
			ChartBase chart = (Owner as CompositeSeries).OwningChart;
			CompositeSeries s = chart.Series;
			if(s.NumberOfSimpleSeries == 0)
				chart.NeedsCreationOfInitialContents = false;
		}

		/// <summary>
		/// Indicates the <see cref="Series"/> or <see cref="CompositeSeries"/> at the specified indexed location in the <see cref="SeriesCollection"/> object. 
		/// </summary>
		/// <param name="index">Integer (by position) or string (by name) index to retrieve from the <see cref="SeriesCollection"/> object.</param>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public new SeriesBase this[object index]   
		{ 
			get { return base[index] as SeriesBase; } 
			set { base[index] = value; value.SetOwner((ChartObject)Owner); } 
		}

		internal override string NextAvailableName(string namePrefix)
		{
			// We search whole series hierarchy
			
			SeriesBase s = (Owner as CompositeSeries).OwningChart.Series;

			if(namePrefix == "Series")
				namePrefix = "S";
			else
				namePrefix = "CS";
		
			for(int i = 1; true; i++)
			{
				if (!NameTaken(s,namePrefix + i))
					return namePrefix + i;
			}
		}

		private bool NameTaken(SeriesBase s, string name)
		{
			if(s.Name == name)
				return true;
			CompositeSeries cs = s as CompositeSeries;
			if(cs != null)
			{
				for(int i=0; i<cs.SubSeries.Count; i++)
				{
					SeriesBase ss = cs.SubSeries[i];
					if(NameTaken(ss,name))
						return true;
				}
			}
			return false;
		}
	}
}
