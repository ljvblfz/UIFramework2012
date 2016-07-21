using System;

namespace ComponentArt.Web.Visualization.Charting
{
	
	// -------------------------------------------------------------------------------------------------------
	//	Auto Numeric Intervals Class
	// -------------------------------------------------------------------------------------------------------
	
	internal class AutoIntervals
	{
		double	_min, _max, _step;
		int		_i0, _i1;
		bool	_inside;

		#region ---- Construction ----

		public AutoIntervals(double min, double max, int nSteps, bool inside)
		{
			if(min >= max)
				throw new Exception("Implementation: Cannot create auto intervals since min >= max");
			double[] dStep = { 2.0, 2.5, 2.0 };
			int xStep = 0;

			bool inverse = false;
			_min = min;
			_max = max;
			if(_max < min)
			{
				_max = min;
				_min = max;
				inverse = true;
			}
			_inside = inside;

			double logDiff = Math.Floor(Math.Log10(_max-_min));
			double step = Math.Pow(10.0,logDiff);
			if(TryStep(step)<nSteps)
			{
				do
				{
					step = step/dStep[xStep];
					xStep = (xStep+1)%3;
				}
				while(TryStep(step)<nSteps);
				_step = step;
			}
			else
			{
				double oldStep;
				int oldi0, oldi1;
				do
				{
					oldStep = step;
					oldi0 = _i0;
					oldi1 = _i1;
					step = step*dStep[xStep];
					xStep = (xStep+1)%3;
				}
				while(TryStep(step)>nSteps);
				_i0 = oldi0;
				_i1 = oldi1;
				_step = oldStep;
			}

			if(inverse)
			{
				double a = _min;
				_min = _max;
				_max = a;
				step = -step;
			}
		}

		int TryStep(double step)
		{
			_i0 = (int)(_min/step);
			_i1 = (int)(_max/step);
			if(_inside)
			{
				if(_i0*step < _min) _i0++;
			}
			else
			{
				if(_i1*step > _max) _i1--;
			}
			return _i1-_i0+1;
		}

		#endregion

		#region ---- Properties ----

		public double MinValue 
		{
			get 
			{ 
				if(_inside && _i0*_step<_min)
					return (_i0+1)*_step;
				if(!_inside && _i0*_step>_min)
					return (_i0-1)*_step;
				return _i0*_step; 
			}
		}

		public double MaxValue 
		{
			get 
			{ 
				if(_inside && _i1*_step>_max)
					return (_i1-1)*_step;
				if(!_inside && _i1*_step<_max)
					return (_i1+1)*_step;
				return _i1*_step; 
			}
		}
		public double Step     { get { return _step; } }
		public double MinIndex { get { return _i0; } }
		public double MaxIndex { get { return _i1; } }
		public int    Count	   { get { return _i1 - _i0 + 1; } }
		public double this[int i] { get { return MinValue + i*Step; } }
		public double[] Values 
		{
			get
			{
				double[] val = new double[Count];
				double x = MinValue;
				for(int i=0; i<Count; i++)
				{
					val[i] = x;
					x += Step;
				}
				return val;
			}
		}

		#endregion
	}
}
