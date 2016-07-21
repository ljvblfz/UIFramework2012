using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting
{
	// ------------------------------------------------------------------------------------------------------
	//		DataPoint label
	// ------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Represents a single label of a <see cref="DataPoint"/> object.
	/// </summary>
	public class DataPointLabel : ChartObject, IDisposable
	{
		private DataPoint			dataPoint;
		private DataPointLabelStyle	labelStyle;
		private string				text;
		private GeometricObject		texts = new GeometricObject();

		private Vector3D			T0=new Vector3D(0,0,0),T1,T2, Vx,Vy;

		/// <summary>
		/// Initializes a new instance of <see cref="DataPointLabel"/> class.
		/// </summary>
		public DataPointLabel() {}

		internal void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (labelStyle != null) 
				{
					labelStyle.Dispose();
					labelStyle = null;
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		internal DataPointLabel(DataPoint dataPoint, DataPointLabelStyle labelStyle, string text)
		{
			this.dataPoint = dataPoint;
			this.labelStyle = new DataPointLabelStyle();
			this.labelStyle.LoadFrom(labelStyle);
			this.text = text;
		}

		/// <summary>
		/// Gets or sets the label style of this <see cref="DataPointLabel"/> object.
		/// </summary>
		public DataPointLabelStyle	LabelStyle		{ get { return labelStyle; } set { labelStyle = value; } }

		/// <summary>
		/// Gets or sets the label style of this <see cref="DataPointLabel"/> object.
		/// </summary>
		public DataPointLabelStyleKind	LabelStyleKind		
		{ 
			get { return labelStyle.StyleKind; } set { labelStyle = OwningChart.DataPointLabelStyles[value]; } }

		/// <summary>
		/// Gets or sets the caption of this <see cref="DataPointLabel"/> object.
		/// </summary>
		public string	Text			{ get { return text; } set { text = value; } }
		internal Series OwningSeries
		{
			get 
			{
				// Search the owner which is simple presentation
				ChartObject obj = Owner;
				while(obj != null && !(obj is Series))
					obj = obj.Owner;
				if(obj == null)
					return null;
				else
					return (obj as Series);
			}
		}

		internal DataPoint DataPoint { get { return dataPoint; } }

		internal override void Render()
		{
			if(!Visible || !DataPoint.Visible || labelStyle == null || !dataPoint.ToRenderLabels)
				return;
			texts.Clear();
			object ao = GE.GetActiveObject();
			GE.SetActiveObject(DataPoint);

			if(dataPoint.OwningSeries.Style.IsLinear)
				RenderLinearShapeLabel();
			else
				RenderPieLabel();
			GE.SetActiveObject(ao);
		}

		private void RenderLinearShapeLabel()
		{
			texts.Clear();
            DataPointLabelStyle LS = new DataPointLabelStyle();
            LS.LoadFrom(this.labelStyle);
			// Transform from local to world coordinate system and render
			LS.HorizontalDirection = dataPoint.LocalToWorldDirection(LS.LocalRefPoint,LS.HorizontalDirection);
			Vector3D Vv = dataPoint.LocalToWorldDirection(LS.LocalRefPoint,LS.VerticalDirection);
			LS.VerticalDirection = Vv;

			Vector3D P;
			if(dataPoint.OwningSeries.Style.IsRadar)
			{
				CoordinateSystem CS = dataPoint.OwningSeries.CoordSystem;
				P = CS.PlaneXY.LogicalToWorld(CS.XAxis.LCS2ICS(dataPoint.X0LCSE), CS.YAxis.LCS2ICS(dataPoint.Y1LCSE))
					+ new Vector3D(0,0,10); // To lift text above other objects
			}
			else
			{
				Vector3D lrp = LS.LocalRefPoint;
				if(dataPoint.Y0LCSE > dataPoint.Y1LCSE)
					lrp = new Vector3D(lrp.X,1-lrp.Y,lrp.Z);
				P = dataPoint.LocalToWorld(lrp);
			}

			Color oldColor = LS.ForeColor;
			if(oldColor.A == 0)
				LS.ForeColor = OwningChart.Palette.DataLabelFontColor;
            LS.Orientation = TextOrientation.UserDefined;
            GE.CreateText(LS, P, text).Tag = dataPoint;
		}

		internal void PositionLabel()
		{
			if(dataPoint.OwningSeries.Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
				PositionPieLabel();
		}

		internal Mapping Mapping { get { return OwningSeries.Mapping; } }
		private void PositionPieLabel()
		{
			DataPointLabelStyle Style = labelStyle;
			if(Style == null)
				return;

			TextOrientation orientation = Style.Orientation;
			Style.Orientation = TextOrientation.UserDefined;

			int i;
			double maxShift = 0.0, xMin=0.0,xMax=0.0;

			Series doughnut = OwningSeries;
			if(doughnut == null)
				return;

			if(Style.PieLabelPosition == PieLabelPositionKind.OutsideAligned)
			{
				for(i=0; i<doughnut.DataPoints.Count;i++)
					maxShift = Math.Max(maxShift, doughnut.DataPoints[i].RelativeShift);
				double x = Math.Max(1.0, Style.RelativeLine1Start+Style.RelativeLine1Length) + Style.RelativeLine2Length;
				T1 = dataPoint.Center + new Vector3D(x*dataPoint.Radius,0,0);
				T1 = Mapping.Map(T1);
				xMax = T1.X;
				T1 = dataPoint.Center - new Vector3D(x*dataPoint.Radius,0,0);
				T1 = Mapping.Map(T1);
				xMin = T1.X;
			}
			
			switch(Style.PieLabelPosition)
			{
				case PieLabelPositionKind.InsideHorizontal:
					T0 = dataPoint.AtLocal(0.5,0.5,1.01);
					Vx = new Vector3D(1,0,0);
					Vy = new Vector3D(0,0,-1);
					break;
				case PieLabelPositionKind.InsideRadial:
					T0 = dataPoint.AtLocal(0.5,0.5,1.01);
					Vx = dataPoint.VxAtLocal(0.5,0.5,1.0);
					Vy = dataPoint.VyAtLocal(0.5,0.5,1.0);
					break;
				case PieLabelPositionKind.InsideCircular:
					T0 = dataPoint.AtLocal(0.5,0.5,1.01);
					Vx = -dataPoint.VyAtLocal(0.5,0.5,1.0);
					Vy = dataPoint.VxAtLocal(0.5,0.5,1.0);
					break;
				case PieLabelPositionKind.OutsideAligned:
				case PieLabelPositionKind.Outside:
					T0 = dataPoint.AtLocal(Style.RelativeLine1Start,0.5,1.01);
					T1 = dataPoint.AtLocal(Style.RelativeLine1Start+Style.RelativeLine1Length,0.5,1.01);
					T2 = T1;
					if(Style.PieLabelPosition == PieLabelPositionKind.Outside)
					{
						if(T1.X>T0.X)
							T2.X = T1.X + Style.RelativeLine2Length*dataPoint.Radius;
						else
							T2.X = T1.X - Style.RelativeLine2Length*dataPoint.Radius;
					}
					T0 = Mapping.MapNoPostProjection(T0);
					T1 = Mapping.MapNoPostProjection(T1);
					T2 = Mapping.MapNoPostProjection(T2);
					if(Style.PieLabelPosition == PieLabelPositionKind.OutsideAligned)
					{
						if(T1.X>T0.X)
							T2.X = xMax;
						else
							T2.X = xMin;
					}
					T2.Y = T1.Y;
					break;

				default:
					break;
			}
		}
		
		private void RenderPieLabel()
		{
			if(labelStyle == null)
				return;
            if (this.DataPoint.OwningSeries.YDimension.Coordinate(this.DataPoint.Y) == 0)
                return;
			DataPointLabelStyle Style = new DataPointLabelStyle();
			Style.LoadFrom(labelStyle);

			Color oldColor = Style.ForeColor;
			if(oldColor.A == 0)
				Style.ForeColor = OwningChart.Palette.DataLabelFontColor;

			TextOrientation orientation = Style.Orientation;
			Style.Orientation = TextOrientation.UserDefined;

			switch(Style.PieLabelPosition)
			{
				case PieLabelPositionKind.InsideHorizontal:
					Style.HorizontalDirection = Vx;
					Style.VerticalDirection = Vy;
                    GE.CreateText(Style,T0,text);
					break;
				case PieLabelPositionKind.InsideRadial:
					Style.HorizontalDirection = Vx;
					Style.VerticalDirection = Vy;
                    GE.CreateText(Style, T0, text);
                    break;
				case PieLabelPositionKind.InsideCircular:
					Style.HorizontalDirection = Vx;
					Style.VerticalDirection = Vy;
					Style.ReferencePoint = TextReferencePoint.Center;
                    GE.CreateText(Style, T0, text);
                    break;
				case PieLabelPositionKind.OutsideAligned:
				case PieLabelPositionKind.Outside:
				{
					LineStyle2D LS = (LineStyle2D)OwningChart.LineStyles2D[OwningSeries.Style.BorderLineStyleName].Clone();
					if(T1.X > T0.X)
					{
						Style.ReferencePoint = TextReferencePoint.LeftCenter;
						Style.HOffset = Style.PixelsToLabel;
					}
					else
					{
						Style.ReferencePoint = TextReferencePoint.RightCenter;
						Style.HOffset = -Style.PixelsToLabel;
					}
					Chart2DText C2T = GE.Create2DText(Style,new Vector3D(T2.X,T2.Y,0),text);
					if(LS != null)
					{
						if(LS.Color.A == 0)
						{
							LS.Color = OwningChart.Palette.TwoDObjectBorderColor;
							LS.Pen.Color = LS.Color;
						}
						Chart2DLine L2D = GE.Create2DLine(LS.Pen);
						if(T1.X<T0.X && T2.X>T1.X || T1.X>T0.X && T2.X<T1.X)
						{
							T2 = T1;
						}
						else if(Style.PieLabelPosition == PieLabelPositionKind.Outside &&
							(T2.Y<T0.Y && T0.Y<T1.Y || T2.Y>T0.Y && T0.Y>T1.Y))
						{
							T1 = T0;
						}
						else if(Style.PieLabelPosition == PieLabelPositionKind.OutsideAligned && 
							(T0.Y < T2.Y && T2.Y < T1.Y || T0.Y > T2.Y && T2.Y > T1.Y))
						{
							double a = (T2.Y-T0.Y)/(T1.Y-T0.Y);
							T1.X = T0.X + a*(T1.X-T0.X);
							T1.Y = T0.Y + a*(T1.Y-T0.Y);
						}
						L2D.Add(new PointF((float)T0.X,(float)T0.Y));
						L2D.Add(new PointF((float)T1.X,(float)T1.Y));
						L2D.Add(new PointF((float)T2.X,(float)T2.Y));

					}

				}
					break;

				default:
					break;
			}

			// Restore the orientation kind
			Style.Orientation = orientation;
		}

		internal Vector3D Point0 { get { return T0; } set { T0 = value; } }
		internal Vector3D Point1 { get { return T1; } set { T1 = value; } }
		internal Vector3D Point2 { get { return T2; } set { T2 = value; } }

		internal void Add(Chart2DObject obj2d)
		{
			Space.Add(obj2d);
		}
	}
}

