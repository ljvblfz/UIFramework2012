using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Palette editor.
	/// </summary>
	internal class PaletteEditor : System.Drawing.Design.UITypeEditor
	{        
		public PaletteEditor()
		{
		}

		public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return true;
		}
		
		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
		{

			ChartColorCollection colArr;
            if (e.Value == null)
                return;

			if (e.Value.GetType() == typeof(Palette)) 
			{
				Palette p = (Palette)e.Value;
				colArr = p.PrimaryColors;
			} 
			else if (e.Value.GetType() == typeof(ChartColorCollection))
			{
				colArr = (ChartColorCollection)(e.Value);
			} 
			else 
			{				
				throw new InvalidCastException("Could not convert " +e.Value.GetType().ToString()+" to Palette");
			}

			int strips = colArr.Count;

			double step = (double)(e.Bounds.Width - 1) / (double)strips;

			e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);

			for (int i=0; i<strips; ++i) 
			{
				Rectangle rect = new Rectangle(
					e.Bounds.X+1 + (int)(i*step+0.5),
					e.Bounds.Y,
					(int)(step+0.5),
					e.Bounds.Height
				);

				e.Graphics.FillRectangle(new SolidBrush(colArr[i].Color), rect);
			}
		}
	}


}
