using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Summary description for SimpleLineCollection.
	/// </summary>
	internal class SimpleLineCollection : CollectionBase
	{
		public SimpleLineCollection() : base() { }

		public SimpleLineCollection(SimpleLineCollection orig) : base()
		{
			foreach(SimpleLine line in orig)
				Add(new SimpleLine(line));
		}

        public int Add(SimpleLine line)
        {
            return List.Add(line);
        }

        public int Add(SimpleLineCollection lines)
        {
            int r = 0;
            for (int i = 0; i < lines.Count; i++)
                r = this.Add(lines[i]);
            return r;
        }

		public SimpleLine this[int x]
		{
			get
			{
				return List[x] as SimpleLine;
			}
		}

        public SimpleLineCollection Reverse()
        {
            SimpleLineCollection r = new SimpleLineCollection();
            foreach (SimpleLine line in this)
                r.Add(line.Reverse());
            return r;
        }

		public static SimpleLineCollection CreateFromLowerAndUpperBounds(SimpleLine lowerBound, SimpleLine upperBound)
		{
			// This metod basically appends upper bound to the reversed lower bound, 
			// but it also takes in account possible intersections between the two lines.
			// If the lines intersect, multiple simple lines are created.

			// The algorithm assumes that both lowerBound and upperBound have monotonic x-coordinate,
			// i.e. the x coordinate increases or decreases betwen points

			SimpleLineCollection SLC = new SimpleLineCollection();

			// Fast check: if y-ranges don't intersect, just use the two lines

			double yLmin = double.MaxValue;
			double yLmax = double.MinValue;
			double yUmin = double.MaxValue;
			double yUmax = double.MinValue;

			double xLmin = double.MaxValue;
			double xLmax = double.MinValue;
			double xUmin = double.MaxValue;
			double xUmax = double.MinValue;

			SimpleLine sl = lowerBound;
			while(sl != null)
			{
				yLmin = Math.Min(yLmin,sl.y);
				yLmax = Math.Max(yLmax,sl.y);
				xLmin = Math.Min(xLmin,sl.x);
				xLmax = Math.Max(xLmax,sl.x);
				sl = sl.next;
				if(sl == lowerBound)
					break;
			}
			sl = upperBound;
			// Push the upper bound up for a small amount, just to make sure the values don't coincide.
			// This is needed to avoid ugly rendering artefacts 
			double smallAmount = (yLmax - yLmin + xLmax - xLmin)/500;
			while(sl != null)
			{
				sl.y += smallAmount;
				yUmin = Math.Min(yUmin,sl.y);
				yUmax = Math.Max(yUmax,sl.y);
				xUmin = Math.Min(xUmin,sl.x);
				xUmax = Math.Max(xUmax,sl.x);
				sl = sl.next;
				if(sl == upperBound)
					break;
			}

			// Create initial line

			lowerBound = lowerBound.Reverse();
			lowerBound.Append(upperBound);
			lowerBound.Close();
			
			if(Math.Min(yLmax,yUmax) < Math.Max(yLmin,yUmin) || Math.Min(xLmax,xUmax) < Math.Max(xLmin,xUmin))
			{
				// There is no intersection of y-ranges, so lines don't intersect.
				// The initial line is the solution
				SLC.Add(lowerBound);
			}
			else
			{
				// There is a possible self-intersection.
				// Push the initial line on a stack.
				// Taking one line at a time from the stack...
				//  ... if there is a slef-intersection, split it and push both parts back, or
				//  ... if there is no self-intersection, add the line to the resulting collection.

				ArrayList stack = new ArrayList();
				stack.Add(lowerBound);
				int nStack = 1;

				while (nStack > 0)
				{
					// Popping the stack
					nStack = nStack-1;
					SimpleLine line = stack[nStack] as SimpleLine;
					// Process "line" : if it has self-intersection, split it in two and push them back, or
					// add it to the solution
					double xMin = double.MaxValue;
					double xMax = double.MinValue;
					SimpleLine s = line;
					while(s != null)
					{
						xMin = Math.Min(xMin,s.x);
						xMax = Math.Max(xMax,s.x);
						s = s.next;
						if(s == line)
							break;
					}
					// Get line partitions for faster searching
					int nSeg = 20;
					double dx = (xMax-xMin)/nSeg;
					ArrayList parts = line.GetPartitions(xMin,xMax,nSeg);
					s = line;
					bool splitCreated = false;
					while(s != null)
					{
						// we look for intersections between a left-to-right edge and right-to-left one
						if(s.x < s.next.x)
						{
							// Orthogonal vector
							double dxn = s.y - s.next.y;
							double dyn = s.next.x - s.x;
							double d2 = dxn*dxn + dyn*dyn;
							if(d2 == 0)
								continue; // ... not interested in null edges
							int i0 = (int)((s.x - xMin)/dx);
							int i1 = (int)((s.next.x - xMin)/dx);
							i0 = Math.Max(i0,0);
							i1 = Math.Min(i1,parts.Count-1);
							for(int i=i0; i<=i1; i++)
							{
								ArrayList vertices = parts[i] as ArrayList;
								foreach(SimpleLine v in vertices)
								{
									if(v.x <= v.next.x)
										continue; // because we need right-to-left edge
									if(v.x < s.x || v.next.x > s.next.x)
										continue; // because there is no chance for intersection
									if(v.next == s || s.next == v)
										continue;
									// are the vertices from differenst sides of "sl" ?
									double sp0 = (v.x-s.x)*dxn + (v.y-s.y)*dyn;
									double sp1 = (v.next.x-s.x)*dxn + (v.next.y-s.y)*dyn;
									if(sp0 * sp1 > 0 || (sp0 == 0 && sp1 == 0)) // on the same side
										continue;
									// v has vertices from different sides of sl.
									// Check now if v.v. is true
									double dxnv = v.y - v.next.y;
									double dynv = v.next.x - v.x;
									double sp0v = (s.x-v.x)*dxnv + (s.y-v.y)*dynv;
									double sp1v = (s.next.x-v.x)*dxnv + (s.next.y-v.y)*dynv;
									if(sp0v * sp1v > 0) // on the same side
										continue;
									// there is an intersection. Compute the point, split the contour
									// and insert it.
									double a = Math.Abs(sp0/(sp0-sp1));
									double x = a*v.next.x + (1-a)*v.x;
									double y = a*v.next.y + (1-a)*v.y;
									SimpleLine newPoint1 = new SimpleLine(x,y);
									SimpleLine newPoint2 = new SimpleLine(x,y);
									newPoint1.next = v.next;
									newPoint2.next = s.next;
									s.next = newPoint1;
									v.next = newPoint2;

									stack[nStack] = newPoint1;
									nStack++;
									if(stack.Count > nStack)
										stack[nStack] = newPoint2;
									else
										stack.Add(newPoint2);
									nStack++;
									splitCreated = true;
									break;									
								} // end of loop over segment vertices
								if(splitCreated)
									break;
							}// end of loop over segments
							if(splitCreated)
								break;
						}// end of if (s.x < s.next.x)
						// loop control:
						s = s.next;
						if(s == line)
							break;
					}// end of loop over edges of the line from stack
					if(!splitCreated)
						SLC.Add(line);
				}
			}
			return SLC;
		}

		/// <summary>
		/// Area is positive if it is oriented clocl-wise in the x-y coordinate plane
		/// </summary>
		internal bool IsPositiveArea()
		{
			double area = 0;
			foreach(SimpleLine line in this)
			{
				SimpleLine L = line;
				SimpleLine N = null;
				while((N = L.next)!=null)
				{
					area += (N.x-L.x)*(N.y+L.y);
					L = N;
					if(L == line)
						break;
				}
			}
			return area > 0;
		}

		#region --- Line Cropping ---
		public SimpleLineCollection SplitLineAtX(double x)
		{
			SimpleLineCollection result = new SimpleLineCollection();
			foreach(SimpleLine line in this)
			{
				if(line.InsertAtX(x))
				{
					SimpleLine L = line;
					while(L != null)
					{
						SimpleLine next = L.next;
						if(L.x == x && next != null)
						{
							L.next = null;
							SimpleLine newLine = new SimpleLine(L.x,L.y);
							newLine.next = next;
							result.Add(newLine);
						}
						L = next;
						if(L == line)
							break;
					}
				}
				result.Add(line);
			}
			return result;
		}

		public SimpleLineCollection SplitLineAtY(double y)
		{
			SimpleLineCollection result = new SimpleLineCollection();
			foreach(SimpleLine line in this)
			{
				if(line.InsertAtY(y))
				{
					SimpleLine L = line;
					while(L != null)
					{
						SimpleLine next = L.next;
						if(L.y == y && next != null)
						{
							L.next = null;
							SimpleLine newLine = new SimpleLine(L.x,L.y);
							newLine.next = next;
							result.Add(newLine);
						}
						L = next;
						if(L == line)
							break;
					}
				}
				result.Add(line);
			}
			return result;
		}

		/// <summary>
		/// Crops all lines in the collection so that the resulting collection contains
		/// only the line segments left of the specified x-coordinate if left = true, or right of 
        /// the specified x-coordinate if left = false.
		/// </summary>
		/// <param name="x">The cropping x-coordinate.</param>
		/// <param name="left">A flag thatindicates if left part is taken (if left = true) 
        /// or right part (if left = false) </param>
		/// <returns>Collection of lines on one side of the x-coordinate.</returns>
		public SimpleLineCollection CropLineAtX(double x, bool left)
		{
			// This modifies this object by adding some points

			SimpleLineCollection lines = SplitLineAtX(x);
			SimpleLineCollection result = new SimpleLineCollection();

			foreach(SimpleLine line in lines)
			{
				bool toBeRemoved = false;
				SimpleLine L = line;
				while(L != null)
				{
					if(L.x > x && left || L.x < x && !left)
					{
						toBeRemoved = true;
						break;
					}
					L = L.next;
					if(L == line)
						break;
				}
				if(!toBeRemoved)
					result.Add(line);
			}

			//Debug.WriteLine("Cropped " + (left?"left":"right") + " of x = " + x.ToString());

			return result;
		}

		/// <summary>
		/// Crops all lines in the collection so that the resulting collection contains
        /// only the line segments below the specified y-coordinate if below = true, or above 
        /// the y-coordinate if below = false.
		/// </summary>
		/// <param name="y">The cropping y-coordinate.</param>
		/// <param name="below">A flag that indicates if the lower part is taken (if below = true) 
        /// or the upper part (if below = false). </param>
		/// <returns>Collection of lines on one side of the y-coordinate.</returns>
		public SimpleLineCollection CropLineAtY(double y, bool below)
		{
			// This modifies this object by adding some points

			SimpleLineCollection lines = SplitLineAtY(y);
			SimpleLineCollection result = new SimpleLineCollection();

			foreach(SimpleLine line in lines)
			{
				bool toBeRemoved = false;
				SimpleLine L = line;
				while(L != null)
				{
					if(L.y > y && below || L.y < y && !below)
					{
						toBeRemoved = true;
						break;
					}
					L = L.next;
				}
				if(!toBeRemoved)
					result.Add(line);
			}
			//Debug.WriteLine("Cropped " + (below?"below":"above") + " of y = " + y.ToString());

			return result;
		}

		#endregion

		#region --- Area Cropping ---
		/// <summary>
		/// Crops the area covered by this collection so that the resulting collection 
        /// covers only the part of the original area which is to the left of the specified 
        /// x-coordinate if left = true, or right of the x-coordinate if left = false.
		/// </summary>
		/// <param name="x">The cropping x-coordinate.</param>
		/// <param name="left">A flag that indicates if the left part is taken (if left = true) or 
        /// the right part (if left = false). </param>
		/// <returns>Collection covering the area on one side of the x-coordinate.</returns>
		public SimpleLineCollection CropAreaAtX(double x, bool left)
		{
			bool areaPositive = this.IsPositiveArea();

			// This modifies this object by adding some points

			// If the line crosses the cropping coordinate, let the 
			// starting point be outside the range

			SimpleLineCollection result = new SimpleLineCollection();
			SimpleLineCollection lines2 = new SimpleLineCollection();


			// In this loop
			// 1. We drop complete simple lines that don't have any point on the 
			//    resulting contour (i.e. completelly on the wrong side of the cropping
			//    coordinate),
			// 2. Include completely contours that have ALL points in the resulting contour, 
			// 3. Collect all other contours in collection "lines2" for further processing, and
			// 4. Change the starting point in all contours in "lines2" to have the starting point on the 
			//    wrong side of the cropping coordinate
			foreach(SimpleLine line in this)
			{
				// Lines are supposed to be closed
				line.Close();

				areaPositive = line.IsPositiveArea();

				bool thereIsPointsOnWrongSide = false;
				bool thereIsPointsOnRightSide = false;
				SimpleLine L = line;
				do
				{
					if(L.x != x)
					{
						if(L.x < x && left || L.x > x && !left)
							thereIsPointsOnRightSide = true;
						else if(L.x != x)
							thereIsPointsOnWrongSide = true;
					}
					L = L.next;
				} while(L != line);


				if(thereIsPointsOnRightSide) // this contour will somehow be included, at least partially
				{
					if(!thereIsPointsOnWrongSide) // line fully included
					{
						result.Add(line);
					}
					else 
					{
						// At this point the line is partialy included in the area. 
						// Make sure that the first point is outside the range
						L = line;
						do
						{
							if(L.x > x && left || L.x < x && !left)
								break;
							L = L.next;
						} while(L != line);
						lines2.Add(L);
					}
				}
			}
			
			// Now all lines in "lines2" are open and they have to start and end at the cropping coordinate

			SimpleLineCollection lines = lines2.CropLineAtX(x,left);

			while(lines.Count > 0)
			{
				// Find a line with the same orientation as the original area
				SimpleLine line1 = null;
				for(int i=0; i<lines.Count; i++)
				{
					SimpleLine line = lines[i];
					SimpleLine last = line.LastPoint();
					if(line.x != x || last.x != x)
						throw new Exception("Implementation error: line doesn't start at cropping coordinate");
					bool linePositive = (line.y < last.y) && left || (line.y > last.y) && !left;
					if(linePositive == areaPositive)
					{
						line1 = line;
						break;
					}
				}
				if(line1 == null)
					break;

				// Drop two-point lines
				if(line1.next.next == null)
				{
					lines.List.Remove(line1);
					continue;
				}

				//line1.Dump("Found as line1");

				// Find the closest line with opposite orientation

				SimpleLine line2 = null;
				do
				{
					SimpleLine line1Last = line1.LastPoint();
					double dist = double.MaxValue;
					for(int i=0; i<lines.Count; i++)
					{
						SimpleLine line = lines[i];
						if(line == line1)
							continue;
						SimpleLine last = line.LastPoint();
						if(line.x != x || last.x != x)
							throw new Exception("Implementation error: line doesn't start at cropping coordinate");
						bool linePositive = (line.y < last.y) && left || (line.y > last.y) && !left;
						if(linePositive != areaPositive)
						{
							if((last.y > line1.y && line.y < line1Last.y) && left ||
								(last.y < line1.y && line.y > line1Last.y) && !left)
							{
								double d = left? line1Last.y - line.y : line.y - line1Last.y;
								if(d < dist)
								{
									dist = d;
									line2 = line;
								}
							}
						}
					}
					if(line2 == null)
					{
						//Debug.WriteLine("line2 not found - we close line 1 and emit");
						line1.Close();
						if(lines.List.Contains(line1))
							lines.List.Remove(line1);
						result.Add(line1);
					}
					else
					{
						//line2.Dump("Found as line2");
						line1Last.next = line2;
						if(lines.List.Contains(line2))
							lines.List.Remove(line2);
						line2 = null;
					}
				}while(line2 != null);
			}

			return result;
		}

        /// <summary>
        /// Crops the area covered by this collection so that the resulting collection 
        /// covers only the part of the original area which is below of the specified 
        /// y-coordinate if below = true, or above the y-coordinate if below = false.
        /// </summary>
        /// <param name="y">The cropping y-coordinate.</param>
        /// <param name="below">A flag that indicates if the lower part is taken (if below = true) 
        /// or the upper part (if below = false). </param>
        /// <returns>Collection covering the area on one side of the y-coordinate.</returns>
        public SimpleLineCollection CropAreaAtY(double y, bool below)
		{
			bool areaPositive = this.IsPositiveArea();

			// This modifies this object by adding some points

			// If the line crosses the cropping coordinate, let the 
			// starting point be outside the range

			SimpleLineCollection result = new SimpleLineCollection();
			SimpleLineCollection lines2 = new SimpleLineCollection();

			// In this loop
			// 1. We drop complete simple lines that don't have any point on the 
			//    resulting contour (i.e. completelly on the wrong side of the cropping
			//    coordinate),
			// 2. Include completely contours that have ALL points in the resulting contour, 
			// 3. Collect all other contours in collection "lines2" for further processing, and
			// 4. Change the starting point in all contours in "lines2" to have the starting point on the 
			//    wrong side of the cropping coordinate
			foreach(SimpleLine line in this)
			{
				// Lines are supposed to be closed
				line.Close();

				bool thereIsPointsOnWrongSide = false;
				bool thereIsPointsOnRightSide = false;
				SimpleLine L = line;
				do
				{
					if(L.y != y)
					{
						if(L.y < y && below || L.y > y && !below)
							thereIsPointsOnRightSide = true;
						else 
							thereIsPointsOnWrongSide = true;
					}
					L = L.next;
				} while(L != line);


				if(thereIsPointsOnRightSide) // this contour will somehow be included, at least partially
				{
					if(!thereIsPointsOnWrongSide) // line fully included
					{
						result.Add(line);
					}
					else 
					{
						// At this point the line is partialy included in the area. 
						// Make sure that the first point is outside the range
						L = line;
						do
						{
							if(L.y > y && below || L.y < y && !below)
								break;
							L = L.next;
						} while(L != line);
						lines2.Add(L);
					}
				}
			}
			// Now all lines in "lines2" are open and they have to start and end at the cropping coordinate

			// Connecting partial contours in "lines2" to make closed contours

			if(lines2.Count == 0)
				return result;

			lines2.Dump("Initial lines");
			SimpleLineCollection lines = lines2.CropLineAtY(y,below); 
			lines.Dump("After cropping at y = " + y);

			while(lines.Count > 0)
			{
				// Find a line with the same orientation as the original area
				SimpleLine line1 = null;
				for(int i=0; i<lines.Count; i++)
				{
					SimpleLine line = lines[i];
					SimpleLine last = line.LastPoint();
					if(line.y != y || last.y != y)
						throw new Exception("Implementation error: line doesn't start at cropping coordinate");
					bool linePositive = (line.x > last.x) && below || (line.x < last.x) && !below;
					if(linePositive == areaPositive)
					{
						line1 = line;
						break;
					}
				}
				if(line1 == null)
					break;

				// Drop two-point lines because both points are on the cropping coordinate
				if(line1.next.next == null)
				{
					lines.List.Remove(line1);
					continue;
				}

				// Find the closest line with opposite orientation

				SimpleLine line2 = null;
				do
				{
					SimpleLine line1Last = line1.LastPoint();
					double dist = double.MaxValue;
					for(int i=0; i<lines.Count; i++)
					{
						SimpleLine line = lines[i];
						if(line == line1)
							continue;
						SimpleLine last = line.LastPoint();
						if(line.y != y || last.y != y)
							throw new Exception("Implementation error: line doesn't start at cropping coordinate");
						bool linePositive = (line.x > last.x) && below || (line.x < last.x) && !below;
						if(linePositive != areaPositive)
						{
							if((last.x > line1.x && line.x < line1Last.x) && !below || // is line "inside" line1?
								(last.x < line1.x && line.x > line1Last.x) && below)
							{
								double d = below? line.x-line1Last.x: line1Last.x-line.x;
								if(d < dist)
								{
									dist = d;
									line2 = line;
								}
							}
						}
					}
					if(line2 == null)
					{
						//Debug.WriteLine("line2 not found - we close line 1 and emit");
						line1.Close();
						if(lines.List.Contains(line1))
							lines.List.Remove(line1);
						result.Add(line1);
					}
					else
					{
						line1Last.next = line2;
						if(lines.List.Contains(line2))
							lines.List.Remove(line2);
						line2 = null;
					}
				}while(line2 != null);
			}

			return result;
		}

		internal GraphicsPath GetPath()
		{
			GraphicsPath path = new GraphicsPath();
			foreach(SimpleLine line in this)
			{
				PointF[] pts = line.GetPoints();
				path.AddClosedCurve(pts,0);
			}
			return path;
		}
		internal Region GetRegion()
		{
			return new Region(GetPath());
		}

		#endregion

		#region --- Testing Helpers ---

		internal void DrawArea(Graphics g, Color color)
		{
			Brush brush = new SolidBrush(color);
			g.FillPath(brush,GetPath());
			brush.Dispose();
		}
		internal void Dump(string header)
		{
#if DEBUG_
			Debug.WriteLine(header);
			int i = 0;
			foreach(SimpleLine line in this)
			{
				line.Dump("[" + i + "]");
				i++;
			}


#endif
		}
		#endregion

	}

}
