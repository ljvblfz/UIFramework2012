using System;
using System.ComponentModel;

using System.Drawing.Design;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Editor for selected palette.
	/// </summary>
	internal class SelectedPaletteEditor : PaletteEditor 
	{
		
		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
		{
            if (e.Context == null)
            {
                e.Graphics.Clear(System.Drawing.Color.Blue);
                return;
            }

			ChartBase chart = ChartBase.GetChartFromObject(e.Context.Instance);

			// Convert to Palette
			PaintValueEventArgs pvea = new PaintValueEventArgs(
				e.Context,
				chart.Palettes[((string)e.Value)],
				e.Graphics, e.Bounds
				);

			base.PaintValue(pvea);
		}
	}
}
