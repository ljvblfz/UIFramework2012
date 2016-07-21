using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Charting
{
	public interface ILegendItemProvider
	{
		bool			LegendItemVisible { get; }
		LegendItemKind	LegendItemKind { get; }
		string			LegendItemText { get; }
		void			DrawLegendItemRectangle	(Graphics g, Rectangle rect);
		double			LegendItemCharacteristicValue { get; }
	}
}
