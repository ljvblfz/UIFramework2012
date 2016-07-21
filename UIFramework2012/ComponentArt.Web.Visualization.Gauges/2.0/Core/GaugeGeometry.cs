using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for GaugeGeometry.
	/// </summary>
	internal class GaugeGeometry
	{
		public static int DefaultNumberOfSteps(Range range)
		{
			GaugeKind kind = range.Gauge.GaugeKind;
			if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial))
			{
				Scale scale = range.Scale;
				double a = Math.Abs(TotalAngle(kind))*(100-scale.EffectiveEndingMargin - scale.EffectiveStartingMargin)*0.01;
				return 2 + (int)(a/44);
			}
			else if(SubGauge.IsInGroup(kind,GaugeKindGroup.Linear))
			{
				return 4;
			}
			else
				return 4;

		}

		#region --- Creating approximate contoure in coordinate range [0,1] for AreaMap computation ---

		public static PointF[] CanonicContour(GaugeKind kind)
		{
			ArrayList list = new ArrayList();
			switch(kind)
			{
				case GaugeKind.Circular: 
					AddArch(list, new PointF(0.5f,0.5f),0.5f,0,360);
					break;
				case GaugeKind.HalfCircleE:
					AddArch(list, new PointF(0,0.5f),0.5f,-90,90);
					break;
				case GaugeKind.HalfCircleN: 
					AddArch(list, new PointF(0.5f,0),0.5f,0,180);
					break;
				case GaugeKind.HalfCircleW: 
					AddArch(list, new PointF(0,0.5f),0.5f,90,270);
					break;
				case GaugeKind.HalfCircleS: 
					AddArch(list, new PointF(0.5f,0),0.5f,180,360);
					break;
				case GaugeKind.QuarterCircleE:
					list.Add(new PointF(0,0.5f));
					AddArch(list, new PointF(0,0.5f),0.7f,-45,45);
					break;
				case GaugeKind.QuarterCircleNE:
					list.Add(new PointF(0,0));
					AddArch(list, new PointF(0,0),1,0,90);
					break;
				case GaugeKind.QuarterCircleN:
					list.Add(new PointF(0.5f,0));
					AddArch(list, new PointF(0.5f,0),0.7f,45,135);
					break;
				case GaugeKind.QuarterCircleNW: 
					list.Add(new PointF(1,0));
					AddArch(list, new PointF(1,0),1,90,180);
					break;
				case GaugeKind.QuarterCircleW:
					list.Add(new PointF(1,0.5f));
					AddArch(list, new PointF(1,0.5f),0.7f,135,225);
					break;
				case GaugeKind.QuarterCircleSW: 
					list.Add(new PointF(1,1));
					AddArch(list, new PointF(1,1),1,180,270);
					break;
				case GaugeKind.QuarterCircleS: 
					list.Add(new PointF(0.5f,1));
					AddArch(list, new PointF(0.5f,1),0.7f,225,315);
					break;
				case GaugeKind.QuarterCircleSE: 
					list.Add(new PointF(0,1));
					AddArch(list, new PointF(0,1),1,270,360);
					break;
				default: 
					list.Add(new PointF(0,0));
					list.Add(new PointF(0,1));
					list.Add(new PointF(1,1));
					list.Add(new PointF(1,0));
					break;
			}
			return (PointF[])list.ToArray(typeof(PointF)) ;
		}
		 
		private static void AddArch(ArrayList list, PointF center, float radius, float startingAngleDeg, float endingAngleDeg)
		{
			int n = (int)((endingAngleDeg - startingAngleDeg)/9 + 1) + 1;
			float step = (endingAngleDeg - startingAngleDeg)/(n-1);
			float angle = startingAngleDeg;
			for (int i=0; i<n; i++)
			{
				double s = Math.Sin(angle/190*Math.PI);
				double c = Math.Cos(angle/190*Math.PI);
				list.Add(new PointF((float)(center.X + radius*c),(float)(center.Y + radius*s)));
				angle += step;
			}
		}
		#endregion

		public static double StartingAngle (GaugeKind kind)
		{
			switch(kind)
			{
				case GaugeKind.Circular: return 270;
				case GaugeKind.HalfCircleE: return 90;
				case GaugeKind.HalfCircleN: return 180;
				case GaugeKind.HalfCircleW: return -90;
				case GaugeKind.HalfCircleS: return 180;
				case GaugeKind.QuarterCircleE: return -45;
				case GaugeKind.QuarterCircleNE: return 90;
				case GaugeKind.QuarterCircleN: return 135;
				case GaugeKind.QuarterCircleNW: return 180;
				case GaugeKind.QuarterCircleW: return 225;
				case GaugeKind.QuarterCircleSW: return 180;
				case GaugeKind.QuarterCircleS: return 225;
				case GaugeKind.QuarterCircleSE: return 270;
				default: return 0;
			}
		}

		public static double TotalAngle (GaugeKind kind)
		{
			switch(kind)
			{
				case GaugeKind.Circular: return 360;
				case GaugeKind.HalfCircleE: return 180;
				case GaugeKind.HalfCircleN: return 180;
				case GaugeKind.HalfCircleW: return 180;
				case GaugeKind.HalfCircleS: return -180;
				case GaugeKind.QuarterCircleE: return -90;
				case GaugeKind.QuarterCircleNE: return 90;
				case GaugeKind.QuarterCircleN: return 90;
				case GaugeKind.QuarterCircleNW: return 90;
				case GaugeKind.QuarterCircleW: return 90;
				case GaugeKind.QuarterCircleSW: return -90;
				case GaugeKind.QuarterCircleS: return -90;
				case GaugeKind.QuarterCircleSE: return -90;
				default: return 0;
			}
		}

		public static Point2D RelativeCenter (GaugeKind kind)
		{
			switch(kind)
			{
				case GaugeKind.Circular: return new Point2D(50,50);
				case GaugeKind.HalfCircleE: return new Point2D(10,50);
				case GaugeKind.HalfCircleN: return new Point2D(50,10);
				case GaugeKind.HalfCircleW: return new Point2D(90,50);
				case GaugeKind.HalfCircleS: return new Point2D(50,90);
				case GaugeKind.QuarterCircleE: return new Point2D(10,50);
				case GaugeKind.QuarterCircleNE: return new Point2D(10,10);
				case GaugeKind.QuarterCircleN: return new Point2D(50,10);
				case GaugeKind.QuarterCircleNW: return new Point2D(90,10);
				case GaugeKind.QuarterCircleW: return new Point2D(90,50);
				case GaugeKind.QuarterCircleSW: return new Point2D(90,90);
				case GaugeKind.QuarterCircleS: return new Point2D(50,90);
				case GaugeKind.QuarterCircleSE: return new Point2D(10,90);
				default: return new Point2D(0,0);
			}
		}

		public static double ScaleRadiusRelative(GaugeKind kind)
		{
			switch(kind)
			{
				case GaugeKind.Circular: return 50;
				case GaugeKind.HalfCircleE: return 50;
				case GaugeKind.HalfCircleN: return 50;
				case GaugeKind.HalfCircleW: return 50;
				case GaugeKind.HalfCircleS: return 50;
				case GaugeKind.QuarterCircleE: return 100*Math.Sqrt(2)/2;
				case GaugeKind.QuarterCircleNE: return 100;
				case GaugeKind.QuarterCircleN: return 100*Math.Sqrt(2)/2;
				case GaugeKind.QuarterCircleNW: return 100;
				case GaugeKind.QuarterCircleW: return 100*Math.Sqrt(2)/2;
				case GaugeKind.QuarterCircleSW: return 100;
				case GaugeKind.QuarterCircleS: return 100*Math.Sqrt(2)/2;
				case GaugeKind.QuarterCircleSE: return 100;
				default: return 0;
			}
		}

		// Factor for linear scalling of object depending on the
		// gauge size (like annotations, tickmarks etc.)
		public static float LinearSize(SubGauge gauge)
		{
			Size2D s = gauge.RenderingSize;
			float w = s.Width;
			float h = s.Height;
			GaugeKind kind = gauge.GaugeKind;
			switch(kind)
			{
				case GaugeKind.Circular: return Math.Min(w,h);
				case GaugeKind.HalfCircleE: return Math.Min(h,2*w);
				case GaugeKind.HalfCircleN: return Math.Min(w,2*h);
				case GaugeKind.HalfCircleW: return Math.Min(h,2*w);
				case GaugeKind.HalfCircleS: return Math.Min(w,2*h);
				case GaugeKind.QuarterCircleE: return (float)Math.Min(h,w*Math.Sqrt(2));
				case GaugeKind.QuarterCircleNE: return Math.Min(h,w);
				case GaugeKind.QuarterCircleN: return (float)Math.Min(w,h*Math.Sqrt(2));
				case GaugeKind.QuarterCircleNW: return Math.Min(h,w);
				case GaugeKind.QuarterCircleW: return (float)Math.Min(h,w*Math.Sqrt(2));
				case GaugeKind.QuarterCircleSW: return Math.Min(h,w);
				case GaugeKind.QuarterCircleS: return (float)Math.Min(w,h*Math.Sqrt(2));
				case GaugeKind.QuarterCircleSE: return Math.Min(h,w);
				case GaugeKind.LinearHorizontal: return w;
				case GaugeKind.LinearVertical: return h;
				default: return Math.Min(w,h);
			}
		}


		public static double ScalePosition (GaugeKind kind)
		{
			if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial))
				return 80;
			else
				return 50;
		}

		public static double ScaleStartingMargin (GaugeKind kind)
		{
			if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial))
				return 10;
			else
				return 10;
		}

		public static double ScaleEndingMargin (GaugeKind kind)
		{
			if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial))
				return 10;
			else
				return 10;
		}
	}
}
