using System;


namespace ComponentArt.Web.Visualization.Charting
{

	/// <summary>
	/// A collection of <see cref="SeriesLabels"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class SeriesLabelsCollection : CollectionWithType
	{
		internal SeriesLabelsCollection(ChartObject owner) : base(typeof(SeriesLabels), owner) 
		{ }

		internal SeriesLabelsCollection() : base(null) 
		{ }

		/// <summary>
		/// Indicates the <see cref="SeriesLabels"/> at the specified indexed location in the <see cref="SeriesLabelsCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="SeriesLabels"/> from the <see cref="SeriesLabelsCollection"/> object.</param>
		public new SeriesLabels this[object index]   
		{ 
			get { return ((SeriesLabels)base[index]); } 
			set { base[index] = value; } 
		}

		public override int Add (object obj)
		{
			SeriesLabels labels = obj as SeriesLabels;
			if(labels == null)
				throw new Exception("Cannot add to the 'SeriesLabelsCollection' an object of type '" + obj.GetType().Name + "'");
			labels.SetOwner(this.Owner as ChartObject);
			int x = base.Add(labels);

			Series s = (Owner as Series);

            if (s != null && s.DataPoints != null && s.DataPoints.Count > 0)
            {
                labels.DataBind();
            }
			return x;
		}


		internal void Build()
		{
			foreach (SeriesLabels SL in this)
				SL.Build();
		}

		internal void Render()
		{
			foreach (SeriesLabels SL in this)
			{
				SL.LabelStyle = null; // To invoke labelstyle refresh
				SL.Render();
			}
		}
		
	}
}
