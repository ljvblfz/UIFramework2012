using System;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting
{
	// -------------------------------------------------------------------------------------------------------
	//		2D Objects
	// -------------------------------------------------------------------------------------------------------

	internal abstract class Chart2DObject
	{
		private ChartObject	owner;
		
		internal abstract void Render(Graphics g);

		public ChartBase OwningChart
		{
			get { return owner.OwningChart; }
		}

		internal void SetOwner(ChartObject owner)
		{
			this.owner = owner;
		}
	}
}
