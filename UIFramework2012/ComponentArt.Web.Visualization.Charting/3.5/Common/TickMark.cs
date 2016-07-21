using System;
using System.Drawing;

using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents the tick mark on the chart.
	/// </summary>
	public class TickMark : ChartObject
	{
		private AxisAnnotation  axisAnnotation;
		private Coordinate		coordinate;
		private string			styleName = "Default";
		private Vector3D		inDirection;
		private Vector3D		outDirection;
		private float			liftZ = 0.5f;

		/// <summary>
		/// Initializes an instance of <see cref="TickMark"/> class with specified axis annotation, tick mark style name and the coordinate.
		/// </summary>
		/// <param name="axisAnnotation">Axis annotation this tick mark belongs to.</param>
		/// <param name="styleName">Tick mark style name.</param>
		/// <param name="coordinate">Coordinate along the axis of the acis annotation that the tick mark will be at.</param>
		public TickMark(AxisAnnotation axisAnnotation, string styleName, Coordinate coordinate)
		{
			this.axisAnnotation = axisAnnotation;
			this.styleName = styleName;
			this.coordinate	= coordinate;
		}

		internal TickMark() 
		{ }

		#region --- Properties ---

		/// <summary>
		/// Gets or sets tick mark style name.
		/// </summary>
		public string StyleName { get { return styleName; } set { styleName = value; } }
		internal float  LiftZ	{ get { return liftZ; } set { liftZ = value; } }
		internal Vector3D InDirection { set { inDirection = value; } get { return inDirection; } }
		internal Vector3D OutDirection { set { outDirection = value; } get { return outDirection; } }		

		#endregion

		#region --- Rendering ---
		internal override void Render()
		{
			if(coordinate == null)
				return;

			TickMarkStyle style, tms = OwningChart.TickMarkStyles[styleName];
			if(tms == null)
				tms = OwningChart.TickMarkStyles["Default"];
			if(tms == null)
				style = new TickMarkStyle("aa",TickMarkKind.CenteredTickmark,5,"Default");
			else
			{
				style = new TickMarkStyle();
				style.LoadFrom(tms);
			}

			LineStyle2D lineStyle = OwningChart.LineStyles2D[style.LineStyleName];
			if(lineStyle == null)
				return;
			lineStyle = (LineStyle2D) lineStyle.Clone();

			// Adjusting color using pallete if needed
			if(lineStyle.Color.A == 0)
				lineStyle.Color = OwningChart.Palette.AxisLineColor;

			Vector3D P = axisAnnotation.WCSCoordinate(axisAnnotation.Axis.LCS2ICS(coordinate.Offset));
			Vector3D S = P;
			Vector3D axisVector = axisAnnotation.Axis.UnitVector * lineStyle.Width*OwningChart.FromPointToWorld;
			P = P - axisVector;
			axisVector = axisVector*2;

			// Inner part
			if(!inDirection.IsNull && (style.Kind == TickMarkKind.InnerTickmark || style.Kind == TickMarkKind.CenteredTickmark))
			{
				Vector3D inVector = inDirection.Unit()*style.Length*OwningChart.FromPointToWorld;
                DrawingBoard brd = GE.CreateDrawingBoard(P, inVector, axisVector);

				if(axisAnnotation.TargetArea.IsTwoDimensional)
					brd.LiftZ = 1000;
				else
					brd.LiftZ = liftZ;
				brd.DrawLine(lineStyle,S,S+inVector);
			}

			// Outer part
			if(!outDirection.IsNull && (style.Kind == TickMarkKind.OuterTickmark || style.Kind == TickMarkKind.CenteredTickmark))
			{
				Vector3D outVector = outDirection.Unit()*style.Length*OwningChart.FromPointToWorld;
                DrawingBoard brd = GE.CreateDrawingBoard(P, outVector, axisVector);
				if(axisAnnotation.TargetArea.IsTwoDimensional)
					brd.LiftZ = 1000;
				else
					brd.LiftZ = liftZ;
				brd.DrawLine(lineStyle,S,S+outVector);
			}
		}

		#endregion
	};
	
	/// <summary>
	/// A collection of <see cref="TickMark"/> objects. This class cannot be inherited.
	/// </summary>
	public class TickMarkCollection : CollectionWithType
	{
		internal TickMarkCollection() : base(typeof(TickMark)) { }

		/// <summary>
		/// Indicates the <see cref="TickMark"/> at the specified indexed location in the <see cref="TickMarkCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="TickMark"/> from the <see cref="TickMarkCollection"/> object.</param>
		public new TickMark this[object obj]
		{
			get { return (TickMark)(List[IndexOf(obj)]); } 
			set { List[IndexOf(obj)] = value;} 
		}

		public override int Add(object obj)
		{
			TickMark tm = obj as TickMark;
			if(tm != null)
			{
				tm.SetOwner(this.Owner as ChartObject);
				return base.Add(tm);
			}
			else
				return -1;
		}
	};
}
