using System;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines an object that specified one segment of the <see cref="DashLineStyle"/> object.
	/// </summary>
	public sealed class DashLineStyleSegment : MultiLineStyleItem
	{

		double m_segmentLength = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyleSegment"/> class.
		/// </summary>
		public DashLineStyleSegment() {}
		// Fixme: units???
		/// <summary>
		/// Initializes a new instance of the <see cref="DashLineStyleSegment"/> class with the specified name and segment length.
		/// </summary>
		/// <param name="lineStyleName">Name of the line style to be used for this segment. This value is stored in the <see cref="DashLineStyleSegment.LineStyleName"/> property.</param>
		/// <param name="length">The length of this segment. This value is stored in the <see cref="DashLineStyleSegment.Length"/> property.</param>
		public DashLineStyleSegment(string lineStyleName, double length) 
			: base(lineStyleName) 
		{
			m_segmentLength = length;
		}

		/// <summary>
		/// The length of the segment.
		/// </summary>
		[SRDescription("DashLineStyleSegmentLengthDescr")]
		public double Length 
		{
			get {return m_segmentLength;}
			set 
			{
				if (value <=0 )
					throw new ArgumentException("Cannot assign a non-positive segment length");

				m_segmentLength = value;
			}
		}

		/// <summary>
		/// The name of the line style to be used in this segment.
		/// </summary>
		[ System.ComponentModel.TypeConverter(typeof(Design.SelectedLineStyleWithNoLineConverter))]
		public new string LineStyleName  
		{
			get {return base.LineStyleName;}
			set {base.LineStyleName = value;}
		}

	}
}
