using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class AreaPartition
	{

		private double		xMin = double.MaxValue;
		private double		xMax = double.MinValue;
		private ArrayList	lines = new ArrayList();
		private ArrayList[] groups;
		private int			iGroup, iSeg;

		private double		minX;
		private double		maxX;

		#region --- Internal Interface ---
		internal AreaPartition() : this(double.MinValue,double.MaxValue) {}

		internal AreaPartition(double minX, double maxX) 
		{
			this.minX = minX;
			this.maxX = maxX;
		}

		internal void Add(SimpleLine L)
		{
			int n = L.Length;
			SimpleLine LL = L;
			for(int i=0; i<n; i++,LL = LL.next)
				Add(LL.x,LL.y,LL.next.x,LL.next.y);
		}

		internal void Add(float[] x, float[] y)
		{
			int n = x.Length;
			for(int i=1; i<n; i++)
				Add((double)(x[i-1]),(double)(y[i-1]),(double)(x[i]),(double)(y[i]));
		}

		internal void Add(double x0, double y0, double x1, double y1)
		{
			// Skip vertical lines
			if(x0 == x1)
				return;

			lines.Add(new LineSection(x0,y0,x1,y1));

			xMin = Math.Min(xMin,Math.Min(x0,x1));
			xMax = Math.Max(xMax,Math.Max(x0,x1));
		}

		internal void StartSections()
		{
			Compute();
			iGroup = -1;
		}

		internal bool GetNextSection(out double x0, out double x1, out double s1y0, out double s1y1, out double s2y0, out double s2y1, out bool positive)
		{
			x0 = 0;
			x1 = 0;
			s1y0 = 0;
			s1y1 = 0;
			s2y0 = 0;
			s2y1 = 0;
			positive = false;

			if(!NextLineSection())
				return false;

			LineSection LS1 = (LineSection)(groups[iGroup][iSeg]);
			// Search two line segments with same x-range
			LineSection LS2 = null;
			while(true)
			{
				if(!NextLineSection())
					return false;
				LS2 = (LineSection)(groups[iGroup][iSeg]);
				if(LS2.x0 == LS1.x0)
				{
					LineSection LSa = LS1.SplitAt(minX);
					if(LSa != null)
					{
						LS1 = LSa;
						LS2 = LS2.SplitAt(minX);
					}
					LS1.SplitAt(maxX);
					LS2.SplitAt(maxX);

					if(minX < LS1.x1 && LS1.x0 < maxX)
						break;
				}
				LS1 = LS2;
			}
			x0 = LS1.x0;
			x1 = LS1.x1;
			s1y0 = LS1.y0;
			s1y1 = LS1.y1;
			s2y0 = LS2.y0;
			s2y1 = LS2.y1;
			positive = LS2.positive;

			if(s1y0+s1y1 > s2y0+s2y1)
			{
				// Switch so that section s2 is above section s1
				double aa = s1y0;
				s1y0 = s2y0;
				s2y0 = aa;
				aa = s1y1;
				s1y1 = s2y1;
				s2y1 = aa;
				positive = !positive;
			}

			return true;
		}

		private bool NextLineSection()
		{
			if(iGroup < 0)
			{
				iGroup = 0;
				iSeg = 0;
			}
			else
			{
				iSeg++;
			}

			// This loop is necessary because some groups may be empty
			while(iSeg >= groups[iGroup].Count)
			{
				iGroup++;
				if(iGroup >= groups.Length)
					return false;
				iSeg = 0;
			}
			return true;
		}

		#endregion

		#region --- Private Implementation ---

		private void Compute()
		{
			// Creating groups based on division of x-range
			// Each group contains segments within the group's x-range

			int nGroups = lines.Count/2 + 2;
			int i;
			groups = new ArrayList[nGroups];
			for(i=0;i<nGroups;i++)
				groups[i] = new ArrayList(6);

			double d = (xMax - xMin) + 1;
			xMin -= d/100;
			xMax += d/100;
			double dSect = (xMax - xMin)/nGroups;
			LineSection newLS;
			foreach(LineSection LS in lines)
			{
				int i1 = (int)((LS.x0 - xMin)/dSect);
				int i2 = (int)((LS.x1 - xMin)/dSect);
				for(i=i2; i>=i1; i--)
				{
					double xx = xMin+i*dSect;
					newLS = LS.SplitAt(xx);
					if(newLS != null)
						groups[i].Add(newLS);
				}
				if(LS.x0 < LS.x1)
					groups[i1].Add(LS);
			}

			// Split line sections in the set of x coordinates

			int ig = 0;
			while(ig < groups.Length)
			{
				ArrayList group = (ArrayList) groups[ig];
				ig++;
				int i1 = 0;
				while(i1 < group.Count)
				{
					LineSection LS1 = (LineSection) group[i1];
					i1++;
					int i2 = 0;
					while(i2 < group.Count)
					{
						LineSection LS2 = (LineSection) group[i2];
						i2++;
						newLS = LS1.SplitAt(LS2.x0);
						if(newLS != null)
							group.Add(newLS);
						newLS = LS1.SplitAt(LS2.x1);
						if(newLS != null)
							group.Add(newLS);
					}
				}
			}

			// Find intersections.
			// After this step each group contains segments that don't have coomon
			// points except possibly endpoints

			foreach(ArrayList group in groups)
			{
				i=0;
				while(i < group.Count)
				{
					LineSection LS1 = (LineSection) group[i];
					i++;
					int j = 0;
					while(j < group.Count)
					{
						LineSection LS2 = (LineSection)group[j];
						j++;
						if( j== i)
							continue;

						double x0 = Math.Max(LS1.x0,LS2.x0);
						double x1 = Math.Min(LS1.x1,LS2.x1);
						if(x0 < x1) // there is common x-interval
						{
							// Compute y-coordinates at endpoints of common interval
							double LS1y0 = LS1.y0 + (LS1.y1-LS1.y0)*(x0-LS1.x0)/(LS1.x1-LS1.x0);
							double LS1y1 = LS1.y0 + (LS1.y1-LS1.y0)*(x1-LS1.x0)/(LS1.x1-LS1.x0);
							double LS2y0 = LS2.y0 + (LS2.y1-LS2.y0)*(x0-LS2.x0)/(LS2.x1-LS2.x0);
							double LS2y1 = LS2.y0 + (LS2.y1-LS2.y0)*(x1-LS2.x0)/(LS2.x1-LS2.x0);
							// Is there an intersection
							if((LS1y0-LS2y0)*(LS1y1-LS2y1) < 0)
							{
								double a = (LS1y0-LS2y0)/((LS1y0-LS2y0)-(LS1y1-LS2y1));
								double xs = x0 + a*(x1 - x0);
								newLS = LS1.SplitAt(xs);
								if(newLS != null)
									group.Add(newLS);
								newLS = LS2.SplitAt(xs);
								if(newLS != null)
									group.Add(newLS);
							}
						}
					}
				}
			}

			// Sort linesegments in each group

			foreach(ArrayList group in groups)
			{
				for(i=0; i<group.Count-1;i++)
				{
					for(int j=i+1; j<group.Count; j++)
					{
						LineSection LS1 = (LineSection) group[i];
						LineSection LS2 = (LineSection) group[j];
						if(LS1.x0 > LS2.x0 || LS1.x0 == LS2.x0 && (LS1.y0+LS1.y1 > LS2.y0+LS2.y1))
						{
							group[i] = LS2;
							group[j] = LS1;
						}
					}
				}
			}
		}

		#endregion
	}
}
