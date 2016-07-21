using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class ChartObjects : ChartObject
	{
		ArrayList	list = new ArrayList();
		public ChartObjects() { }

		public int Add(ChartObject obj)
		{
			if(obj == null)
				return -1;
			obj.SetOwner(this);
			return list.Add(obj);
		}

		internal override void Render()
		{
			foreach(ChartObject o in list)
				o.Render();
		}

		public int Count { get { return list.Count; } }

		public void Clear() { list.Clear(); }

		public ChartObject this[int ix] { get { return list[ix] as ChartObject; } }
	}
}
