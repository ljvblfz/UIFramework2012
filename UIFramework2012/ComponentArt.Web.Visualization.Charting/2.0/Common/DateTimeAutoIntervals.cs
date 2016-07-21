using System;

namespace ComponentArt.Web.Visualization.Charting
{
	// -------------------------------------------------------------------------------------------------------
	//	Auto DateTime Intervals Class
	// -------------------------------------------------------------------------------------------------------
	
	internal class DateTimeAutoIntervals
	{
		DateTime		min, max;
		int				step;
		DateTimeUnit	unit;
		DateTime[]		R;
		
		#region Steps Array
		
		private struct DTStep
		{
			public int			n;
			public DateTimeUnit	unit;
			internal DTStep(int n,DateTimeUnit unit)
			{
				this.n = n;
				this.unit = unit;
			}
		};

		static DTStep[] steps = new  DTStep[]
			{
				new DTStep(  1, DateTimeUnit.Second ),
				new DTStep(  2, DateTimeUnit.Second ),
				new DTStep(  5, DateTimeUnit.Second ),
				new DTStep( 10, DateTimeUnit.Second ),
				new DTStep( 15, DateTimeUnit.Second ),
				new DTStep( 20, DateTimeUnit.Second ),
				new DTStep( 30, DateTimeUnit.Second ),
				new DTStep(  1, DateTimeUnit.Minute ),
				new DTStep(  2, DateTimeUnit.Minute ),
				new DTStep(  5, DateTimeUnit.Minute ),
				new DTStep( 10, DateTimeUnit.Minute ),
				new DTStep( 15, DateTimeUnit.Minute ),
				new DTStep( 20, DateTimeUnit.Minute ),
				new DTStep( 30, DateTimeUnit.Minute ),
				new DTStep(  1, DateTimeUnit.Hour ),
				new DTStep(  2, DateTimeUnit.Hour ),
				new DTStep(  4, DateTimeUnit.Hour ),
				new DTStep(  6, DateTimeUnit.Hour ),
				new DTStep( 12, DateTimeUnit.Hour ),
				new DTStep(  1, DateTimeUnit.Day ),
				new DTStep(  2, DateTimeUnit.Day ),
				new DTStep(  5, DateTimeUnit.Day ),
				new DTStep( 10, DateTimeUnit.Day ),
				new DTStep( 15, DateTimeUnit.Day ),
				new DTStep(  1, DateTimeUnit.Month ),
				new DTStep(  2, DateTimeUnit.Month ),
				new DTStep(  3, DateTimeUnit.Month ),
				new DTStep(  4, DateTimeUnit.Month ),
				new DTStep(  6, DateTimeUnit.Month ),
				new DTStep(  1, DateTimeUnit.Year ),
				new DTStep(  2, DateTimeUnit.Year ),
				new DTStep(  5, DateTimeUnit.Year ),
				new DTStep( 10, DateTimeUnit.Year ),
				new DTStep( 20, DateTimeUnit.Year ),
				new DTStep( 50, DateTimeUnit.Year ),
				new DTStep(100, DateTimeUnit.Year ),
				new DTStep(200, DateTimeUnit.Year ),
				new DTStep(500, DateTimeUnit.Year ),
				new DTStep(1000, DateTimeUnit.Year )
			};
		#endregion

		#region ---- Construction ----

		public DateTimeAutoIntervals(DateTime min, DateTime max, int nSteps, bool inside)
		{
			DateTime[] S1 = new DateTime[5*nSteps];		// Working array 1
			DateTime[] S2 = new DateTime[5*nSteps];		// Working array 2

			// Work with intervals that include the given interval (as if inside = false),
			// then fix the result at the end

			// Start with the bigest step and refine it until the required 
			//	number of data in the interval is obtained
			int ix = steps.Length - 1;
			S1[0] = Intervals.ClosestDateTime(min, steps[ix].n,steps[ix].unit,false);
			S1[1] = Intervals.ClosestDateTime(max, steps[ix].n,steps[ix].unit,true);
			int nS1 = 2, nS2, ne;
			do
			{
				// choose next smaler interval and merge with values from S1
				ix --;
				int n2 = 0;
				S2[0] = S1[0];
				for(int i=1;i<nS1;i++)
				{
					DateTime t = min;
					while(true)
					{
						t = Intervals.Add(S2[n2],steps[ix].n,steps[ix].unit);
						if(t>=S1[i])
						{
							n2++;
							S2[n2] = S1[i];
							break;
						}
						// here is t<S1[i]
						if(t<=min)
							S2[0] = t;
						else
						{
							n2++;
							S2[n2] = t;
						}
						if(t>=max)
							break;
					}
					if(t>=max)
						break;
				}
				n2++;
				// remove extra items in S2.
				// Exactly one value <= min and exactly one value >= max is left after this step
				nS2 = 0;
				for(int i=0;i<n2;i++)
				{
					if(S2[i] <= min)
						S2[0] = S2[i];
					else // S2[i] > min
					{
						nS2++;
						S2[nS2] = S2[i];
					}
					// we stop as soon as we reach or exceed max
					if(S2[i] >= max)
						break;
				}
				nS2++;
				// compute the number of elements
				ne = nS2;
				if(inside)
				{
					if(S2[0] < min)
						ne --;
					if(S2[nS2-1] > max)
						ne --;
				}

				// If units are days and step multiple of 5 then
				//		Change 6-->5, 11-->10, 16-->15, 21-->20, 26-->25, and
				//		Remove 31
				if(steps[ix].unit == DateTimeUnit.Day && steps[ix].n >= 5)
				{
					int nn = 0;
					for(int i=0;i<ne;i++)
					{
						if(S2[i].Day != 31)
						{
							int nDay = S2[i].Day;
							if(nDay == 6 || nDay == 11 || nDay == 16 || nDay == 21 || nDay == 26)
								S2[nn] = new DateTime(S2[i].Year,S2[i].Month,nDay-1,12,0,0);
							else
								S2[nn] = S2[i];
							nn++;
						}
					}
					ne = nn;
					nS2 = ne;
				}

				if(ne>=nSteps) // we exit with elements in S2 and number of elements = nS2
					break; 

				// not enough members, prepare for the next smaller interval
				// Strategy: 
				// - if the next interval is in the same unit class as previous,
				//   we continue with the same S1. For example, if S1 was in 1 month steps
				//   and we tried with 10 days step in S2, then next time we'll keep 1 month
				//   steps in S1 and try with 5 days step in S2.
				// - if the next interval is in the lower unit class, then the current S2
				//   will be used as S1 in the next step.
				// This strategy ensures that all subdivisions are made in different unit classes

				if(ix==0)
					break;
				if(steps[ix-1].unit != steps[ix].unit) // S2 becomes S1 for the next iteration
				{
					for(int i=0; i<nS2; i++)
						S1[i] = S2[i];
					nS1 = nS2;
				}
				
			} while(ix>0);

			// S2 [0..nS2-1] contains elements. Remove first and last if necessary

			int n;
			if(ne == 0) // Guard against too small interval
			{
				R = new DateTime[1] { S2[0] };
				n = 1;
			}
			else
			{
				R = new DateTime[ne];
				n = 0;
				for(int i=0;i<nS2;i++)
					if(!inside || (S2[i]>=min && S2[i]<=max))
					{
						R[n] = S2[i];
						n++;
					}
			}
			
			// Other state variables
			this.min = R[0];
			if(n == 0)
				this.max = this.min;
			else
				this.max = R[n-1];
			this.unit = steps[ix].unit;
			this.step = steps[ix].n;

			// ...To secure the case of very small min/max range
			if(this.min == this.max)
				this.max = NextValue(this.min);
		}

		#endregion

		#region ---- Properties ----

		public DateTime		MinValue	{ get { return min; } }
		public DateTime		MaxValue	{ get { return max; } }
		public int			Step		{ get { return step; } }
		public DateTimeUnit	StepUnit	{ get { return unit; } }
		public int			Count		{ get { return R.Length; } }
		public DateTime[]	Values		{ get { return R; } }
		
		public static DateTime	Shift(DateTime time, int n, DateTimeUnit unit)
		{
			switch(unit)
			{
				case DateTimeUnit.Year:		return time.AddYears(n);
				case DateTimeUnit.Month:	return time.AddMonths(n);
				case DateTimeUnit.Day:		return time.AddDays(n);
				case DateTimeUnit.Hour:		return time.AddHours(n);
				case DateTimeUnit.Minute:	return time.AddMinutes(n);
				case DateTimeUnit.Second:	return time.AddSeconds(n);	
			}
			return time;
		}
		
		public DateTime		PreviousValue(DateTime time)
		{
			return Shift(time,-Step,StepUnit);
		}
		public DateTime		NextValue(DateTime time)
		{
			return Shift(time,Step,StepUnit);
		}

		#endregion
	}
}
