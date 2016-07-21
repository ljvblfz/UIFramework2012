using System;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Defines a method for rendering a control.
	/// </summary>
	public interface IDrawableControl
	{		
		Bitmap RenderBitmap(int width, int height);
	}
}
