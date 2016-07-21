using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for RenderingContext.
	/// </summary>
	internal class RenderingContext
	{
		private object renderingTarget;
		private Size2D size;
		internal Matrix matrix = new Matrix();
		private IEngine engine;
        private Rectangle2D targetArea;

		#region --- Construction and cloning ---

		public RenderingContext() { }

		public virtual RenderingContext CreateCopy()
		{
			throw new Exception("Implementation: '" + GetType().Name + "' should override 'CreateNewInstance()'");
		}

		internal virtual void InitializeNewInstance(RenderingContext context)
		{
			context.renderingTarget = renderingTarget;
			context.Size = size;
			context.engine = engine;
			context.targetArea = targetArea;
			float[] m = matrix.Elements;
			context.matrix = new Matrix(m[0],m[1],m[2],m[3],m[4],m[5]);
		}
		#endregion

		internal Matrix Matrix { get { return matrix; } }
		
		internal PointF TransformPoint(PointF point)
		{
			PointF[] pts = new PointF[] { point };
			matrix.TransformPoints(pts);
			return pts[0];
		}

		#region --- Properties ---
		
		public virtual object RenderingTarget { get { return renderingTarget; } set { renderingTarget = value; } }

        internal virtual Size2D Size { get { return size; } set { size = value; } }
        internal virtual Rectangle2D TargetArea
        {
            get
            {
                if (targetArea == null)
                    targetArea = new Rectangle2D(0, 0, size.Width, size.Height);
                return targetArea;
            }
        }
		
		internal IEngine Engine { get { return engine; } set { engine = value; } }
		#endregion
		#region --- Changing Context ---

        internal RenderingContext SetTargetArea(Rectangle2D targetArea)
        {
            RenderingContext newContext = CreateCopy();
            newContext.targetArea = targetArea;
            return newContext;
        }

        internal RenderingContext SetAreaMapping(Rectangle2D localTargetArea, bool isomorphic)
        {
            return SetAreaMapping(localTargetArea, TargetArea, isomorphic);
        }

        // Caller declares both target and local coordinates it works in, as well as if mapping between
        // local and control coordinates has to be isomophic.
        // Mapping parameters are adjusted so that the local area maps onto the given
        // target area, with possible office adjustment due to isomorphic mapping.
        // The given target area becomes current target area of the context.

        internal RenderingContext SetAreaMapping(Rectangle2D localTargetArea, Rectangle2D targetArea, bool isomorphic)
        {
            RenderingContext newContext = CreateCopy();

            float dx = localTargetArea.Width;
            float dy = localTargetArea.Height;
            float scaleX = targetArea.Width / dx;
            float scaleY = targetArea.Height / dy;
            if (isomorphic)
            {
                float scale = (float)Math.Min(scaleX, scaleY);
                scaleX = scale;
                scaleY = scale;
            }
            // Translate local(x0,y0) -- >(0,0)
            Matrix mat = new Matrix(
                1, 0,
                0, 1,
                -localTargetArea.X, -localTargetArea.Y);
            // Scale
            mat.Scale(scaleX, scaleY, MatrixOrder.Append);
            // Translate (0,0) to the target Area origin
            float tx = (float)(targetArea.X + 0.5 * (targetArea.Width - scaleX * localTargetArea.Width));
            float ty = (float)(targetArea.Y + 0.5 * (targetArea.Height - scaleY * localTargetArea.Height));
            mat.Translate(tx, ty, MatrixOrder.Append);
            // Prepend to the previous trensformation
            newContext.matrix.Multiply(mat, MatrixOrder.Prepend);
            // new target area
            newContext.targetArea = localTargetArea;
            return newContext;
        }

        // Caller declares local target area and two points that define mapped position
        // of (locally) horizontal rectangle base.

        internal RenderingContext SetGaugeArea(Rectangle2D gaugeArea,
            Point2D point00, Point2D point10)
        {
            RenderingContext newContext = CreateCopy();

            float dx = gaugeArea.Width;
            float dy = gaugeArea.Height;
            Size2D xSide = point10 - point00;
            float scale = xSide.Abs() / gaugeArea.Width;

            // Translate local(x0,y0) -- >(0,0)
            Matrix mat = new Matrix(
                1, 0,
                0, 1,
                -gaugeArea.X, -gaugeArea.Y);
            // Scale
            mat.Scale(scale, scale, MatrixOrder.Append);

            // Rotate
            mat.Rotate((float)(180 / Math.PI * Math.Atan2(xSide.Height, xSide.Width)), MatrixOrder.Append);

            // Move to the point00
            mat.Translate(point00.X, point00.Y, MatrixOrder.Append);

            // Prepend to the previous trensformation
            newContext.matrix.Multiply(mat, MatrixOrder.Prepend);

            PointF[] pts = new PointF[] { 
                new PointF(gaugeArea.X, gaugeArea.Y) ,
                new PointF(gaugeArea.X + gaugeArea.Width, gaugeArea.Y) 
            };

            mat.TransformPoints(pts);
            newContext.targetArea = gaugeArea;
            return newContext;
        }

        // Defines mapping so that P0->T0 and P1->T1 using isomorphic transformation

        internal RenderingContext DefineMapping(Point2D P0, Point2D P1, Point2D T0, Point2D T1)
        {
            RenderingContext newContext = CreateCopy();

            Size2D dP = P1 - P0;
            Size2D dT = T1 - T0;
            float scale = dT.Abs()/dP.Abs();

            // Translate P0 --> (0,0)
            Matrix mat = new Matrix(
                1, 0,
                0, 1,
                -P0.X, -P0.Y);
            // Scale
            mat.Scale(scale, scale, MatrixOrder.Append);

            // Rotate
            double angP = Math.Atan2(dP.Height, dP.Width);
            double angT = Math.Atan2(dT.Height, dT.Width);

            mat.Rotate((float)(180 / Math.PI *(angT-angP)), MatrixOrder.Append);

            // Move to the point00
            mat.Translate(T0.X, T0.Y, MatrixOrder.Append);

            // Prepend to the previous trensformation
            newContext.matrix.Multiply(mat, MatrixOrder.Prepend);

            PointF[] pts = new PointF[] { P0, P1 };

            mat.TransformPoints(pts);

            return newContext;
        }

		// Creates context that maps part relative center and relative end point into given center point and end point

		internal RenderingContext GetNeedleContext(LayerVisualPart part, Point2D centerPoint, Point2D endPoint)
		{
			Point2D centerS = new Point2D(
				part.RelativeCenterPoint.X*part.Size.Width*0.01f,
				part.RelativeCenterPoint.Y*part.Size.Height*0.01f);
			Point2D tipS = new Point2D(
				part.RelativeEndPoint.X*part.Size.Width*0.01f,
				part.RelativeEndPoint.Y*part.Size.Height*0.01f);
			return DefineMapping(centerS,tipS,centerPoint,endPoint);
		}

        #endregion

		internal MapAreaCollection Transform(MapAreaCollection mapAreas)
		{
			MapAreaCollection outCollection = new MapAreaCollection();
			for(int i=0; i<mapAreas.Count; i++)
			{
				MapAreaPolygon p = mapAreas[i] as MapAreaPolygon;
				if(p == null)
					continue;
				Point[] points = p.Points;
				matrix.TransformPoints(points);
				outCollection.Add(new MapAreaPolygon(points));				
			}
			return outCollection;
		}
	}
}
