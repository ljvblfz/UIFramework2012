using System;
using System.Text;


namespace ComponentArt.Web.Visualization.Charting
{
    public interface IScrollControl
    {

        /// <summary>
        /// Sets the initial selected range of the zoom control, as percentages of the entire range.
        /// As such both values must be between 0 and 1.
        /// </summary>
        /// <param name="start">start value of the selected range, as percentage of entire range</param>
        /// <param name="end">end value of the selected range, as percentage of entire range</param>
        void SetZoomRangeInPrecent(double start, double end);

        /// <summary>
        /// Sets the initial selected range of the zoom control.
        /// Both parameters must be of the same type as CoordinateSystem type
        /// of the X axis of the TargetChart.
        /// </summary>
        /// <param name="start">start value of the selected range</param>
        /// <param name="end">end value of the selected range</param>
        void SetZoomRange(object start, object end);

    }
}
