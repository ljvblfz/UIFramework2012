using System;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Summary description for NoLineStyle.
	/// </summary>
	// ---------------------------------------------------------------------------------------------------

	internal class NoLineStyle : LineStyle
	{
		public NoLineStyle() : base ("",JoinStyle.Bevel,null,0.0f) { }
		public NoLineStyle(string name, ChartBase chart) :this()
		{
			Name = name;
		}

		public NoLineStyle(string name) :base(name)
		{
		}
 
		#region --- Serialization ---

		internal override void ResetJoinStyle() {JoinStyle=JoinStyle.Bevel;}
		private bool ShouldSerializeJoinStyle() {return JoinStyle!=JoinStyle.Bevel;}

		internal override void ResetWidth() {Width=2.0;}
		private bool ShouldSerializeWidth() {return Width!=2.0;}

		private bool ShouldSerializeSurface() {return ChartColor != null;}

		#endregion
	}

}
