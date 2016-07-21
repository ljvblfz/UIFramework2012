using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
    internal class ChartArea : GeometricObject
    {
        private SimpleLineCollection LC;
        private Vector3D P, Xe, Ye;
        private double h;
        private ChartColor bodyColor, wallColor;

        public ChartArea() { }

        public ChartArea(SimpleLineCollection LC, double h, Vector3D P,
            Vector3D Xe, Vector3D Ye, ChartColor bodyColor, ChartColor wallColor)
        {
            this.LC = LC;
            this.h = h;
            this.P = P;
            this.Xe = Xe;
            this.Ye = Ye;
            this.bodyColor = bodyColor;
            this.wallColor = wallColor;
        }

        internal SimpleLineCollection Lines { get { return LC; } }
        internal double Height { get { return h; } }
        internal Vector3D Origin { get { return P; } }
        internal Vector3D XDirection { get { return Xe; } }
        internal Vector3D YDirection { get { return Ye; } }
        internal ChartColor BodyColor { get { return bodyColor; } }
        internal ChartColor WallColor { get { return wallColor; } }

		internal override double OrderingZ()
		{
			return Mapping.Map(Origin).Z;
		}
    }

    internal class Chart2DArea : GeometricObject
    {
        private SimpleLineCollection LC;
        private DrawingBoard drawingBoard;
        private GradientStyle primGradient, secGradient;
        private LineStyle2D lineStyle;

        public Chart2DArea() { }

        public Chart2DArea(SimpleLineCollection LC, DrawingBoard drawingBoard,
            GradientStyle primGradient, GradientStyle secGradient, LineStyle2D lineStyle)
        {
            this.LC = LC;
            this.drawingBoard = drawingBoard;
            this.primGradient = primGradient;
            this.secGradient = secGradient;
            this.lineStyle = lineStyle;
        }

        internal SimpleLineCollection Lines { get { return LC; } }
        internal DrawingBoard DrawingBoard { get { return drawingBoard; } }
        internal GradientStyle Gradient { get { return primGradient; } }
        internal GradientStyle SecondaryGradient { get { return secGradient; } }
        internal LineStyle2D LineStyle { get { return lineStyle; } }
		internal override double OrderingZ()
		{
			return Mapping.Map(drawingBoard.V0).Z;
		}
	}

}
