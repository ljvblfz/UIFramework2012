using System;
#if false
namespace ComponentArt.Charting
{
	internal class FlatChartLine : ChartLine
	{
		private Vector3D	P;
		private Vector3D	Vx;
		private Vector3D	Vy;

		public FlatChartLine() { }
		internal FlatChartLine(string style, Vector3D P, Vector3D Vx, Vector3D Vy) : base(style)
		{
			this.P = P;
			this.Vx = Vx;
			this.Vy = Vy;
		}

		internal FlatChartLine(Vector3D P, Vector3D Vx, Vector3D Vy) : this(null,P,Vx,Vy) { }

		internal Vector3D GetP0() { return P; }
		internal Vector3D GetAx() { return Vx; }
		internal Vector3D GetAy() { return Vy; }

		internal LineStyle LineStyleRef
		{
			get { return OwningChart.GetLineStyle(LineStyle); }
		}

		internal override void Render()
		{
			if(LineStyleRef==null || Engine == null || Points==null)
				return;
			LineStyleRef.Render(Engine,this);
		}
	}
}
#endif