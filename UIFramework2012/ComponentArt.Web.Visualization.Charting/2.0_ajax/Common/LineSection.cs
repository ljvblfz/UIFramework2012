using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// ===========================================================================================
	//		Class LineSection - partitions area 
	// ===========================================================================================

	internal class LineSection
	{
		internal double x0, y0;
		internal double x1, y1;
		internal bool   positive;
		internal LineSection next;
		internal LineSection (double x0, double y0, double x1, double y1)
		{
			if(x0 < x1)
			{
				this.x0 = x0;
				this.y0 = y0;
				this.x1 = x1;
				this.y1 = y1;
				positive = true;
			}
			else
			{
				this.x1 = x0;
				this.y1 = y0;
				this.x0 = x1;
				this.y0 = y1;
				positive = false;
			}
		}

		internal LineSection  SplitAt(double x)
		{
			if(x<=x0 || x>=x1)
				return null;

			double a = (x-x0)/(x1-x0);
			double y = a*y1 + (1.0-a)*y0;

			LineSection newSection = new LineSection(x,y,x1,y1);
			newSection.positive = positive;
			x1 = x;
			y1 = y;
			return newSection;
		}
	}
}
