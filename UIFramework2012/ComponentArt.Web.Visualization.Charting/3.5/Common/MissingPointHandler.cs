using System;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines the missing point handling algorithm.
	/// </summary>
	public enum MissingPointHandlerKind 
	{
		/// <summary>
		/// Use the handler kind defined in the parent composite series. 
        /// If the parent series doesn't exist, handle as in "Ignore".
		/// </summary>
		Auto,
		/// <summary>
		/// Connect non-missing points in the line and area chart types, 
		/// skip missing points in other chart types.
		/// </summary>
		Ignore,
		/// <summary>
        /// Leave empty space in the line and area chart types, 
		/// skip missing points in other chart types.
		/// </summary>
		Skip,
		/// <summary>
		/// Use linear interpolation and extrapolation to compute logical values at the missing points.
		/// </summary>
		Linear,
		/// <summary>
		/// Set zero or other null value at missing points.
		/// </summary>
		Zero,
		/// <summary>
		/// Use cubic interpolation and extrapolation to compute logical values at the missing points.
		/// </summary>
		Cubic,
		/// <summary>
		/// Use a custom missing point handler.
		/// </summary>
		Custom
	}

	/// <summary>
	/// Base class for missing points handlers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class provides basic services to inherited classes: access to data points, x-coordinates of the
	/// data points and values along y-axis. The data points exposed to the derived class are always in 
	/// the increasing order by the x-coordinate, even if points don't have this order in the associated 
	/// <see cref="Series"/> object. 
	/// </para>
	/// <para>
	/// Note that missing points handling is possible even if values along the y-axis are not numerics.
	/// The derived classes get logical coordinates of the non-missing points (which are always numeric) and
	/// set logical computed values of the missing points. The complexity of mapping real to logical coordinates
	/// and v.v. is handled by this base class.
	/// </para>
	/// <para>
	/// There are 5 predefined missing point handler strategies immpemented in classes that inherit from 
	/// the base class <see cref="MissingPointHandler"/>. Custom handlers can also be implemented as classes 
	/// that inherit from the same base class.
	/// </para>
	/// <para>
	/// A predefined missing point handler can be associated with a <see cref="SeriesBase"/> object 
	/// using the <see cref="Seriesbase.MissingPointHandlerKind"/> property. The custom handler can be set
	/// using the method <see cref="SeriesBase.SetCustomMissingPointHandler"/>. If a handler is set on a
	/// <see cref="CompositeSeries"/> node of the series hierarchy, it is used by all subnodes, unless overriden.
	/// </para>
	/// <para>
	/// A missing point handler can be set on the chart control level using "MissingPointHandlerKind" property, or
	/// "SetCustomMissingPointHandler" method. When set that way, the handler is used in all the series, unless overriden
	/// in some nodes of the series hierarchy.
	/// </para>
	/// </remarks>
	public abstract class MissingPointHandler
	{
		private class DPValue
		{
			internal double x;
			internal double[] y = new double[4]; // y, From-To or OHLC
			internal bool missing;
			internal DataPoint dp;
		};

		private DataDimension xDim;
		private DataDimension yDim;

		private DPValue[] points;
		private bool isRange;
		private bool isFinancial;

		static internal MissingPointHandler GetHandlerByKind(MissingPointHandlerKind kind)
		{
			switch(kind)
			{
				case MissingPointHandlerKind.Ignore: return null;
				case MissingPointHandlerKind.Skip: return new SkippingMissingPointHandler();
				case MissingPointHandlerKind.Linear: return new LinearMissingPointHandler();
				case MissingPointHandlerKind.Cubic: return new CubicMissingPointHandler();
				case MissingPointHandlerKind.Zero: return new ConstantMissingPointHandler(0);
				case MissingPointHandlerKind.Custom: return null;
				default: return null;
			}
		}

		private double MiddleCoord(DataDimension dim, object coord)
		{
			return dim.Coordinate(coord) + 0.5*dim.Width(coord);
		}

		internal void SetData(DataPointCollection dpc)
		{
			int nPoints = dpc.Count;
			if(nPoints == 0)
			{
				points = null;
				return;
			}
			else
				points = new DPValue[nPoints];

			this.xDim = dpc[0].OwningSeries.XDimension;
			this.yDim = dpc[0].OwningSeries.YDimension;

			isRange = false;
			isFinancial = false;

			for(int i=0; i<nPoints; i++)
			{
				// Get the new point
				DataPoint dp = dpc[i];
				DPValue dpv = new DPValue();
				dpv.dp = dp;
				dpv.missing = dp.IsMissing;
				dpv.x = MiddleCoord(xDim,dp.Parameters["x"]);
				
				object yp = dp.Parameters["y"];
				if(yp != null)
					dpv.y[0] = MiddleCoord(yDim,yp);
				else
				{
					object fromp = dp.Parameters["from"];
					object top = dp.Parameters["to"];
					if(fromp != null)
					{
						dpv.y[0] = MiddleCoord(yDim,fromp);
						dpv.y[1] = MiddleCoord(yDim,top);
						isRange = true;
					}
					else
					{
						object open = dp.Parameters["open"];
						object high = dp.Parameters["high"];
						object low = dp.Parameters["low"];
						object close = dp.Parameters["close"];
						if(open != null)
						{
							dpv.y[0] = MiddleCoord(yDim,open);
							dpv.y[1] = MiddleCoord(yDim,high);
							dpv.y[2] = MiddleCoord(yDim,low);
							dpv.y[3] = MiddleCoord(yDim,close);
							isFinancial = true;
						}
						else
							dpv.missing = true;
					}
				}

				// Insert the point to ensure increasing x coordinate

				int j;
				for(j=i-1; j>=0; j--)
				{
					if(dpv.x < points[j].x) // if point[j] is after dpv, move it to the right
						points[j+1] = points[j];
					else
						break; 
				}
				points[j+1] = dpv;
			}	
		}


		#region --- Protected interface ---
		/// <summary>
		/// Data point at specified position. Note that data points are arranged in increasing x -coordinates,
		/// to make computation of missing values easier in derived classes. Consequently, the order is not 
		/// necessarily the same as in the data point collection of the <see cref="Series"/>object.
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns><see cref="DataPoint"/> object at position i.</returns>
		protected DataPoint Point(int i) { return points[i].dp; }
		/// <summary>
		/// Logical x -coordinate of the point. If the x-coordinate has a non-zero width, this is coordinate of
		/// the mid-point. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>Logical x -coordinate of the <see cref="DataPoint"/> at position i.</returns>
		/// <remarks>
		/// <para>
		///		If the coordinate type is double, this value is equal to the coordinate.
		/// </para>
		/// <para>
		///		If the coordinate type is string, then the <see cref="DataDimension"/> object created by the control
		///		assignes width equal 1 to all points and the first point has initial coordinate 0. Therefore, the
		///		mid-point of the first coordinate is 0.5, the next is 1.5 etc.
		/// </para>
		/// <para>
		///		If coordinate type is <see cref="DateTime"/>, the X coordinate is distance in days (and fractions
		///		of days) from a fixed date.
		/// </para>
		/// </remarks>
		protected double X(int i) { return points[i].x; }
		/// <summary>
		/// Logical y -coordinate of the point. If the y-coordinate has a non-zero width, this is coordinate of
		/// the mid-point. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>Logical y -coordinate of the <see cref="DataPoint"/> at position i.</returns>
		/// <remarks>
		/// <para>
		///		If the coordinate type is double, this value is equal to the coordinate.
		/// </para>
		/// <para>
		///		If the coordinate type is string, then the <see cref="DataDimension"/> object created by the control
		///		assignes width equal 1 to all points and the first point has initial coordinate 0. Therefore, the
		///		mid-point of the first coordinate is 0.5, the next is 1.5 etc.
		/// </para>
		/// <para>
		///		If coordinate type is <see cref="DateTime"/>, the X coordinate is distance in days (and fractions
		///		of days) from a fixed date.
		/// </para>
		/// </remarks>
		protected double Y(int i) { return points[i].y[0]; }
		/// <summary>
		/// The "from" value of the point, for a range type of series. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>The "from" value of the <see cref="DataPoint"/> at position i.</returns>
		protected double From(int i) { return points[i].y[0]; }
		/// <summary>
		/// The "to" value of the point, for a range type of series. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>The "to" value of the <see cref="DataPoint"/> at position i.</returns>
		protected double To(int i) { return points[i].y[1]; }
		/// <summary>
		/// The "open" value of the point, for a financial type of series. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>The "open" value of the <see cref="DataPoint"/> at position i.</returns>
		protected double Open(int i) { return points[i].y[0]; }
		/// <summary>
		/// The "high" value of the point, for a financial type of series. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>The "high" value of the <see cref="DataPoint"/> at position i.</returns>
		protected double High(int i) { return points[i].y[1]; }
		/// <summary>
		/// The "low" value of the point, for a financial type of series. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>The "low" value of the <see cref="DataPoint"/> at position i.</returns>
		protected double Low(int i) { return points[i].y[2]; }
		/// <summary>
		/// The "close" value of the point, for a financial type of series. 
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns>The "close" value of the <see cref="DataPoint"/> at position i.</returns>
		protected double Close(int i) { return points[i].y[3]; }
		/// <summary>
		/// Generic access to values in a data point.
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <param name="j">Index of data item. </param>
		/// <returns>The value of the data item.</returns>
		/// <remarks>
		/// <para>
		/// This function provides unified access to data associated to a data point, that doesn't depend on
		/// the series type. It could make implementation of custom event handlers easier.
		/// </para>
		/// <para>
		///		Value(i,0) is Y(i) or From(i) or Open(i), deppending on the type of series.
		/// </para>
		/// <para>
		///		Value(i,1) is To(i) or High(i), deppending on the type of series.
		/// </para>
		/// <para>
		///		Value(i,2) is Low(i), if the series is financial.
		/// </para>
		/// <para>
		///		Value(i,3) is Close(i), if the series is financial.
		/// </para>
		/// </remarks>
		protected double Value(int i, int j) { return points[i].y[j]; }
		/// <summary>
		/// Indicates whether the data related to  a data point are missing.
		/// </summary>
		/// <param name="i">Index of the point.</param>
		/// <returns></returns>
		protected bool IsMissing(int i) { return points[i].missing; }
		/// <summary>
		/// Gets the data dimension of x -coordinates.
		/// </summary>
		protected DataDimension XDimension { get { return xDim; } }
		/// <summary>
		/// Gets the data dimension of y -coordinates.
		/// </summary>
		protected DataDimension YDimension { get { return yDim; } }
		/// <summary>
		/// Gets the number of data points.
		/// </summary>
		protected int NPoints { get { return points.Length; } }
		/// <summary>
		/// Indicates whether this series is range type. Range type series has parameters "from" and "to".
		/// </summary>
		protected bool IsRange { get { return isRange; } }
		/// <summary>
		/// Indicates whether this series is financial type. Financial type series has parameters "open", "high", "low" and "close".
		/// </summary>
		protected bool IsFinancial { get { return isFinancial; } }

		/// <summary>
		/// Sets computed "y" parameter of the data point.
		/// </summary>
		/// <param name="dp">Data point.</param>
		/// <param name="logicalValue">Logical value of the "y" parameter.</param>
		/// <remarks>
		/// The y -dimension object computes the value of the "y" parameter using its logical value. 
		/// This way we can handle any type of dimension.
		/// </remarks>
		protected void SetLogicalValue(DataPoint dp, double logicalValue)
		{
			if(dp.IsMissing)
			{
				object val = yDim.ElementAt(logicalValue);
				if(val != null)
				{
					dp.Parameters["y"] = val;
					dp.BindParameters();
				}
				else
					throw new Exception("Computing missing points: invalid logical value " + logicalValue
						+ " in point x = " + dp.Parameters["x"].ToString());
				dp.IsMissing = false;

			}
		}

		/// <summary>
		/// Sets computed "from" and "to" parameters of the data point.
		/// </summary>
		/// <param name="dp">Data point.</param>
		/// <param name="fromValue">Logical value of the "from" parameter.</param>
		/// <param name="toValue">Logical value of the "to" parameter.</param>
		/// <remarks>
		/// The y -dimension object computes values of "from" and "to" parameters using logical values. 
		/// This way we can handle any type of dimension.
		/// </remarks>
		protected void SetLogicalRange(DataPoint dp, double fromValue, double toValue)
		{
			if(dp.IsMissing)
			{
				object val = yDim.ElementAt(fromValue);
				if(val != null)
					dp.Parameters["from"] = val;
				else
					throw new Exception("Computing missing points: invalid logical value " + fromValue
						+ " in point x = " + dp.Parameters["x"].ToString());
				val = yDim.ElementAt(toValue);
				if(val != null)
					dp.Parameters["to"] = val;
				else
					throw new Exception("Computing missing points: invalid logical value " + toValue
						+ " in point x = " + dp.Parameters["x"].ToString());
				dp.BindParameters();
				dp.IsMissing = false;
			}
		}


		/// <summary>
		/// Sets computed "open", "high",  "low" and "close" parameters of the data point.
		/// </summary>
		/// <param name="dp">Data point.</param>
		/// <param name="fromValue">Logical value of the "from" parameter.</param>
		/// <param name="toValue">Logical value of the "to" parameter.</param>
		/// <remarks>
		/// The y -dimension object computes values of "from" and "to" parameters using logical values. 
		/// This way we can handle any type of dimension.
		/// </remarks>
		
		/// <summary>
		/// Sets computed "open", "high",  "low" and "close" parameters of the data point.
		/// </summary>
		/// <param name="dp">Data point.</param>
		/// <param name="open">The value of the "open" parameter.</param>
		/// <param name="high">The value of the "high" parameter.</param>
		/// <param name="low">The value of the "low" parameter.</param>
		/// <param name="close">The value of the "close" parameter.</param>
		protected void SetFinancial(DataPoint dp, double open, double high, double low, double close)
		{
			if(dp.IsMissing)
			{
				dp.Parameters["open"] = open;
				dp.Parameters["high"] = high;
				dp.Parameters["low"] = low;
				dp.Parameters["close"] = close;
				dp.BindParameters();
				dp.IsMissing = false;
			}
		}

		/// <summary>
		/// Abstract method for missing points computation. Inherited classes must implement this method.
		/// </summary>
		public abstract void ComputeMissingPoints();
		#endregion
	}

	// ==================================================================================================
	
	/// <summary>
	/// This missing point handler makes missing points invisible. In the case of area and line styles, the
	/// area or line is interrupted at the missing points.
	/// </summary>
	internal class SkippingMissingPointHandler : MissingPointHandler
	{
		public override void ComputeMissingPoints()
		{
			for(int i=0; i< NPoints; i++)
				if(IsMissing(i))
					Point(i).Visible = false;
		}
	}

	// ==================================================================================================
	
	/// <summary>
    /// This missing point handler uses linear interpolation and extrapolation to compute values at the missing points.
	/// </summary>
	internal class LinearMissingPointHandler : MissingPointHandler
	{
		public override void ComputeMissingPoints()
		{
			// Get indices of existing points
			int[] indexE = new int[NPoints];
			int ne = 0;
			int i;

			for(i=0;i<NPoints; i++)
			{
				if(!IsMissing(i))
				{
					indexE[ne] = i;
					ne++;
				}
			}
			if(ne == 0)
				return;

			// Interpolate
			for(i=0; i< NPoints; i++)
			{
				if(IsMissing(i))
				{
					if(ne == 1)
					{
						if(IsRange)
							SetLogicalRange(Point(i),From(0),To(0));
						else if(IsFinancial)
							SetFinancial(Point(i),Open(0),High(0),Low(0),Close(0));
						else
							SetLogicalValue(Point(i),Y(0));
					}
					else
					{
						double xx = X(i);
						// Find two existing points
						int j;
						for(j=1; j<ne-1; j++)
						{
							if(X(indexE[j])>=xx)
								break;
						}
						int i0 = indexE[j-1];
						int i1 = indexE[j];
						double a = (xx-X(i0))/(X(i1)-X(i0));
						if(IsRange)
							SetLogicalRange(Point(i),a*From(i1) + (1-a)*From(i0),a*To(i1) + (1-a)*To(i0));
						else if(IsFinancial)
							SetFinancial(Point(i),
								a*Open(i1) + (1-a)*Open(i0),
								a*High(i1) + (1-a)*High(i0),
								a*Low(i1) + (1-a)*Low(i0),
								a*Close(i1) + (1-a)*Close(i0));
						else
							SetLogicalValue(Point(i),a*Y(i1) + (1-a)*Y(i0));

					}
				}
			}
		}
	}

	// ==================================================================================================
	
	/// <summary>
    /// This missing point handler uses qubic interpolation and extrapolation to compute values at the missing points.
	/// </summary>
	/// <remarks>
	/// <para>
	///		Two existing points to the left of the interpolated point and two to the right
	///		are used to fit a qubic function and interpolate the value. If there is not
	///		enough points to the left or to the right (esp. at the beginning and end of the series)
	///		the rule "2+2" can be violated so that more than two points are used on the left
	///		or the right of the point of interpolation.
	/// </para>
	/// <para>
	///		If there is not enough existing points, the qubic function can degenerate to quadratic or linear 
	///		function or constant function.
	/// </para>
	/// </remarks>
	internal class CubicMissingPointHandler : MissingPointHandler
	{
		public override void ComputeMissingPoints()
		{
			// Get indices of existing points
			int[] indexE = new int[NPoints];
			int ne = 0;
			int i;

			for(i=0;i<NPoints; i++)
			{
				if(!IsMissing(i))
				{
					indexE[ne] = i;
					ne++;
				}
			}
			if(ne == 0)
				return;

			// Interpolate
			for(i=0; i< NPoints; i++)
			{
				if(IsMissing(i))
				{
					if(ne == 1)
					{
						if(IsRange)
							SetLogicalRange(Point(i),From(0),To(0));
						else if(IsFinancial)
							SetFinancial(Point(i),Open(0),High(0),Low(0),Close(0));
						else
							SetLogicalValue(Point(i),Y(0));
					}
					else
					{
						double xx = X(i);
						// Find the first point > xx
						int j;
						for(j=2; j<ne-2; j++)
						{
							if(X(indexE[j])>=xx)
								break;
						}
						int j0 = Math.Max(0,j-2);
						int j1 = Math.Min(ne-1,j0+3);
						double[] values = new double[4];
						for(j=j0; j<=j1; j++)
						{
							double p = 1;
							for(int k=j0; k<=j1; k++)
							{
								if(k!=j)
									p = p*(xx - X(indexE[k]))/(X(indexE[j]) - X(indexE[k]));
							}
							for(int k=0; k<4; k++)
								values[k] = values[k] + p*Value(indexE[j],k);
						}

						if(IsRange)
							SetLogicalRange(Point(i),values[0],values[1]);
						else if(IsFinancial)
							SetFinancial(Point(i),values[0],values[1],values[2],values[3]);
						else
							SetLogicalValue(Point(i),values[0]);

					}
				}
			}
		}
	}


	// ==================================================================================================
	
	/// <summary>
    /// This missing point handler sets a constant value to the missing points.
	/// </summary>	
	internal class ConstantMissingPointHandler : MissingPointHandler
	{
		private object val;
		public ConstantMissingPointHandler(object val)
		{
			this.val = val;
		}

		public override void ComputeMissingPoints()
		{
			for(int i=0; i< NPoints; i++)
			{
				if(IsMissing(i))
				{
					if(IsRange)
					{
						Point(i).Parameters["from"] = val;
						Point(i).Parameters["to"] = val;
					}
					else if(IsFinancial)
					{
						Point(i).Parameters["open"] = val;
						Point(i).Parameters["high"] = val;
						Point(i).Parameters["low"] = val;
						Point(i).Parameters["close"] = val;
					}
					else
						Point(i).Parameters["y"] = val;
				}
			}
		}
	}
}
