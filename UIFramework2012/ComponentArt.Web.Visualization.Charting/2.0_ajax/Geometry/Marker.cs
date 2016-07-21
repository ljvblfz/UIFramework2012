using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Marker.
	/// </summary>

	internal class Marker : GeometricObject
	{
		protected Vector3D vx = new Vector3D(1,0,0);
		protected Vector3D vy = new Vector3D(0,1,0);
		protected Vector3D vz = new Vector3D(0,0,1);
		protected ChartColor surface;
		protected bool		takeColorFromStyle = true;
		protected string	style;
		protected Vector3D	location;
		protected double	size;
		protected bool		customSetHeight = false;
		protected double    height;
		protected MarkerStyle ms;
		protected bool		isTwoDimensional = false;

		public Marker() { }

		public Marker(string style, Vector3D location, double size, ChartColor surface)
		{
			this.style = style;
			this.location = location;
			this.size = size;
			this.surface = surface;
			takeColorFromStyle = false;
		}

		public Marker(string style, Vector3D location, double size)
		{
			this.style = style;
			this.location = location;
			this.size = size;
			takeColorFromStyle = true;
		}

		public Vector3D Vx		{ get { return vx; }	set { vx = value.Unit(); } }
		public Vector3D Vy		{ get { return vy; }	set { vy = value.Unit(); } }
		internal bool IsTwoDimensional { get { return isTwoDimensional; } set { isTwoDimensional=value; } }
        public string StyleName { get { return style; } }
        public Vector3D Location { get { return location; } }
        internal bool TakeColorFromStyle { get { return takeColorFromStyle; } }
        internal bool HeightIsCustom { get { return customSetHeight; } }
        public double Size { get { return size; } }
        public ChartColor ChartColor	
		{
            get { return surface; }
			set { surface = value; takeColorFromStyle = false; } 
		}

 		internal double Height // in ICS
		{
            get { return height; }
			set
			{
				customSetHeight = true;
				height = value;
			}
		}

		internal Vector3D Vz { get{ return vz; } set { vz = value.Unit(); } }
	}
}
