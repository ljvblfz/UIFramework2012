using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// -------------------------------------------------------------------------------------------------------
	//	Static Intervals Computation
	// -------------------------------------------------------------------------------------------------------

	internal class Intervals
	{
		#region --- Logarithmic Intervals ---
		public static double[] LogValues(double start, double end, int logBase, int nItems)		
		{
			if(start == 0) 
				throw new Exception("start == 0");
			if(start <= 0 || end < start || logBase <= 0)
				return null;

			// Finding the range in form of logBase^n
			double v0 = 1.0, v1, v = 1.0;
			if(v0 < start)
			{
				while(v < start)
				{
					v0 = v;
					v = v*logBase;
				}
			}
			else
			{
				while(v0 > start)
					v0 = v0/logBase;
			}
			v1 = v0;
			int nd = 1;
			while(v1<end)
			{
				nd++;
				v1 = v1*logBase;
			}

			// At this point v1 = v0*logBase^(nd -1)

			// Points v0*logBase^n (for n=0,1,...,nd) belong to the array of values. 
			// Here we check if some intermediate values will also go

			if(nd >= nItems) // Only powers are used
			{
				double [] val = new double[nd];
				for(int i=0;i<nd;i++)
					val[i] = v0*Math.Pow(logBase,(double)i);
				return val;
			}
			else
			{
				// Finding the intermediate step. Note that intermediate members form an arithmetic progression.
				int step = 1, i, j;
				for(i=1; i<logBase; i++)
				{
					if(logBase%i == 0)
					{
						if(logBase/i*(nd-1) < nItems)
							break;
						else
							step = i;
					}
				}
				int ni = logBase/step;
				int np = (nd-1)*ni+1;
				double[] val = new double[np];
				val[0] = v0;
				i = 0;

				v = v0;
				while(i<np-1)
				{
					for(j=step; j<=logBase && i<(np-1); j+= step)
					{
						i++;
						val[i] = v*j;
					}
					v = val[i];
				}
				return val;
			}

		}
		#endregion

		#region --- Numeric Intervals ---
		public static double[] FromStartEndStep(double start, double end, double step)
		{
			// Handle inverse case
			if(start > end)
			{
				double[] rr = FromStartEndStep(-Math.Abs(step),end,start);
				int j = rr.Length - 1;
				for (int i=0; i<rr.Length/2;i++,j--)
				{
					double a = rr[i];
					rr[i] = rr[j];
					rr[j] = a;
				}
				return rr;
			}

			int nStep = (int)((end-start)/step);
			double e = start + nStep*step;
			if(e < end - 0.001*step)
				nStep++;
			nStep++;
			double[] r = new double[nStep];
			for(int i=0;i<nStep;i++)
				r[i] = start + i*step;
			return r;
		}

		public static double[] MultiplesOfStepBetween(double step, double start, double end)
		{
			// Handle inverse case
			if(start > end)
			{
				double[] rr = MultiplesOfStepBetween(-Math.Abs(step),end,start);
				int j = rr.Length - 1;
				for (int i=0; i<rr.Length/2;i++,j--)
				{
					double a = rr[i];
					rr[i] = rr[j];
					rr[j] = a;
				}
				return rr;
			}

			// if start and/or end are multiple of step, they are not included
			int q1 = (int)(start/step-0.0001)+1;
			int q2 = (int)(end  /step-0.0001);
			if(q1 > q2) 
				return new double[] {};
			double[] r = new double[q2-q1+1];
			int ix = 0;
			for(int i=q1;i<=q2;i++,ix++)
				r[ix] = i*step;	

			return r;
		}

		public static double[] MultiplesOfStepIncludingEnds(double step, double start, double end)
		{
			// Handle inverse case
			if(start > end)
			{
				double[] rr = MultiplesOfStepIncludingEnds(-Math.Abs(step),end,start);
				int j = rr.Length - 1;
				for (int i=0; i<rr.Length/2;i++,j--)
				{
					double a = rr[i];
					rr[i] = rr[j];
					rr[j] = a;
				}
				return rr;
			}

			// Internal points start with q1*step and end with q2*step
			int q1 = (int)(start/step-0.0001)+1;
			int q2 = (int)(end  /step-0.0001);
			if(q1 > q2) 
				return new double[] {start,end};

			double[] r = new double[q2-q1+3];
			r[0] = start;
			int ix = 1;
			for(int i=q1;i<=q2;i++,ix++)
				r[ix] = i*step;	// in this case coordinate is the step
			r[ix] = end;
			return r;
		}


		/// <summary>
		/// Creates array of values between <c>min</c> and <c>max</c>.
		/// </summary>
		/// <param name="min">Lower bound of the interval.</param>
		/// <param name="max">Upper bound of the interval.</param>
		/// <param name="nSteps">Minimum number of points in the array. The number of points in the array is greater or 
		///			equal to <c>nSteps</c></param>
		/// <param name="inside">If true, no value less than <c>min</c> or greater then <c>max</c>is generated. If false, 
		///			the first value may be less then min (if it is multiple of the selected step) and/or the last value 
		///			may be greater than <c>max</c>.</param>
		/// <returns>Double array of values between <c>min</c> and <c> max</c>.</returns>
		/// <remarks>
		/// 
		/// The values are coosen so that they are multiples of round values, like 1, 2, 5, 10, 20, 50 etc. 
		///		The interval is the bigest possible interval that doesn't produce les points than <c>nSteps</c>. 
		/// </remarks>
		public static double[] AutoInRange(double min, double max, int nSteps, bool inside)
		{
			AutoIntervals AI = new AutoIntervals(min,max,nSteps,inside);
			return AI.Values;
		}
		#endregion

		#region --- DateTime Intervals ---

		public static DateTime[] MultiplesOfStepBetween(int step, DateTimeUnit unit, DateTime start, DateTime end)
		{
			// Range of values
			DateTime t1 = Intervals.ClosestDateTime(start,step,unit,false);
			DateTime t2 = Intervals.ClosestDateTime(start,step,unit,true);
			// Number of values
			DateTime t = t1;
			int n = 0;
			while(t<=t2)
			{
				if(t>=t1)
					n++;
				t = Intervals.Add(t,step,unit);
			}
			// Compute 
			DateTime[] r = new DateTime[n];
			t = t1;
			n = 0;
			while(t<=t2)
			{
				if(t>=t1)
				{
					r[n] = t;
					n++;
				}
				t = Intervals.Add(t,step,unit);
			}
			return r;
		}

		public static DateTime[] MultiplesOfStepIncludingEnds(int step, DateTimeUnit unit, DateTime start, DateTime end)
		{
			// Range of values
			DateTime t1 = Intervals.ClosestDateTime(start,step,unit,true);
			DateTime t2 = Intervals.ClosestDateTime(start,step,unit,false);
			// Number of values
			DateTime t = t1;
			int n = 2;
			while(t<t2)
			{
				if(t>t1)
					n++;
				t = Intervals.Add(t,step,unit);
			}
			// Compute 
			DateTime[] r = new DateTime[n];
			r[0] = start;
			t = t1;
			n = 1;
			while(t<t2)
			{
				if(t>t1)
				{
					r[n] = t;
					n++;
				}
				t = Intervals.Add(t,step,unit);
			}
			r[n-1] = end;
			return r;
		}

		public static DateTime Add(DateTime t, int span, DateTimeUnit unit)
		{
			switch(unit)
			{
				case DateTimeUnit.Year:
					return t.AddYears(span);
				case DateTimeUnit.Month:
					return t.AddMonths(span);
				case DateTimeUnit.Day:
					return t.AddDays(span);
				case DateTimeUnit.Hour:
					return t.AddHours(span);
				case DateTimeUnit.Minute:
					return t.AddMinutes(span);
				case DateTimeUnit.Second:
					return t.AddSeconds(span);
			}
			return t;
		}

		public static DateTime ClosestDateTime(DateTime t, int iStep, DateTimeUnit unit, bool greaterOrEqual)
		{
			if(iStep<1) 
				iStep = 1;
			int n;
			DateTime t1 = t;
			switch(unit)
			{
				case DateTimeUnit.Year:
					n = (t.Year/iStep)*iStep;
					t1 = new DateTime(n,1,1,12,0,0,0);
					break;
				case DateTimeUnit.Month:
					n = (t.Month/iStep)*iStep;
					t1 = new DateTime(t.Year,n,1,12,0,0,0);
					break;
				case DateTimeUnit.Day:
					n = (t.Day/iStep)*iStep;
					t1 = new DateTime(t.Year,t.Month,n,12,0,0,0);
					break;
				case DateTimeUnit.Hour:
					n = (t.Hour/iStep)*iStep;
					t1 = new DateTime(t.Year,t.Month,t.Day,n,0,0,0);
					break;
				case DateTimeUnit.Minute:
					n = (t.Minute/iStep)*iStep;
					t1 = new DateTime(t.Year,t.Month,t.Day,t.Hour,n,0,0);
					break;
				case DateTimeUnit.Second:
					n = (t.Second/iStep)*iStep;
					t1 = new DateTime(t.Year,t.Month,t.Day,t.Hour,t.Minute,n,0);
					break;
			}

			if(greaterOrEqual && t1<t)
			{
				switch(unit)
				{
					case DateTimeUnit.Year:
						t1 = t1.AddYears(iStep);
						break;
					case DateTimeUnit.Month:
						t1 = t1.AddMonths(iStep);
						break;
					case DateTimeUnit.Day:
						t1 = t1.AddDays(iStep);
						break;
					case DateTimeUnit.Hour:
						t1 = t1.AddHours(iStep);
						break;
					case DateTimeUnit.Minute:
						t1 = t1.AddMinutes(iStep);
						break;
					case DateTimeUnit.Second:
						t1 = t1.AddSeconds(iStep);
						break;
				}
			}

			if(!greaterOrEqual && t1>t)
			{
				switch(unit)
				{
					case DateTimeUnit.Year:
						t1 = t1.AddYears(-iStep);
						break;
					case DateTimeUnit.Month:
						t1 = t1.AddMonths(-iStep);
						break;
					case DateTimeUnit.Day:
						t1 = t1.AddDays(-iStep);
						break;
					case DateTimeUnit.Hour:
						t1 = t1.AddHours(-iStep);
						break;
					case DateTimeUnit.Minute:
						t1 = t1.AddMinutes(-iStep);
						break;
					case DateTimeUnit.Second:
						t1 = t1.AddSeconds(-iStep);
						break;
				}
			}
			return t1;
		}

		public static DateTime[] AutoInRange(DateTime min, DateTime max, int nSteps, bool inside)
		{
			DateTimeAutoIntervals ai = new DateTimeAutoIntervals(min, max, nSteps, inside);
			return ai.Values;
		}


		#endregion
	}
}
