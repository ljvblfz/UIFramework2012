using System;
using System.Text;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
    internal class Quadrilateral : GeometricObject
    {
        private Vector3D p00;
        private Vector3D p01;
        private Vector3D p11;
        private Vector3D p10;
        private Vector3D n00;
        private Vector3D n01;
        private Vector3D n11;
        private Vector3D n10;

        private ChartColor color;

        public Quadrilateral() { }

        public Quadrilateral(Vector3D P00, Vector3D P01, Vector3D P11, Vector3D P10,
            Vector3D N00, Vector3D N01, Vector3D N11, Vector3D N10, ChartColor color)
        {
            p00 = P00;
            p01 = P01;
            p11 = P11;
            p10 = P10;
            n00 = N00;
            n01 = N01;
            n11 = N11;
            n10 = N10;
            this.color = color;
        }

        #region --- Properties ---

        public Vector3D P00 { get { return p00; } }
        public Vector3D P01 { get { return p01; } }
        public Vector3D P11 { get { return p11; } }
        public Vector3D P10 { get { return p10; } }

        public Vector3D N00 { get { return n00; } }
        public Vector3D N01 { get { return n01; } }
        public Vector3D N11 { get { return n11; } }
        public Vector3D N10 { get { return n10; } }

        public ChartColor Color { get { return color; } }

        #endregion
    }

	internal class RadialStrip : GeometricObject
	{
		private Vector3D[] innerRing;
		private Vector3D[] outerRing;
		private Vector3D[] normal;
		private ChartColor chartColor;

        public RadialStrip() { }

		public RadialStrip(Vector3D[] innerRing, Vector3D[] outerRing, Vector3D[] normal, ChartColor chartColor)
		{
			this.innerRing = innerRing;
			this.outerRing = outerRing;
			this.normal = normal;
			this.chartColor = chartColor;
		}

		#region --- Properties --

		internal override double OrderingZ() { return -10000; } 
		 
		public Vector3D[] InnerRing { get { return innerRing; } }
		public Vector3D[] OuterRing { get { return outerRing; } }
		public Vector3D[] Normal { get { return normal; } }
		public int Count { get { return innerRing.Length; } }
		public Vector3D Center
		{
			get
			{
				Vector3D C = new Vector3D(0,0,0);
				for(int i=0; i<Count; i++)
					C = C + innerRing[i];
				C = C/Count;
				return C;
			}
		}

		public ChartColor ChartColor { get { return chartColor; } }

		#endregion
	}
}
