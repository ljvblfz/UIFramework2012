using System;
using System.Drawing;
using System.Windows.Forms;

#if !__BUILDING_ComponentArt_Win_UI_Internal__
namespace ComponentArt.Win.UI.Internal
#else
namespace ComponentArt.Win.UI.WinChartSamples
#endif
{
	/// <summary>
	/// Summary description for WaterMark.
	/// </summary>
	internal class WaterMark
	{
		static Bitmap drawSimpleWatermark(string mainTitle, string message) 
		{
			Font mainTitleFont = new Font("Arial", 24, FontStyle.Bold, GraphicsUnit.Pixel);
			Font messageFont = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);

			Bitmap b = new Bitmap(1,1);
			Graphics g = Graphics.FromImage(b);

			SizeF mainTitleSize = g.MeasureString(mainTitle, mainTitleFont);
			SizeF messageTitleSize = g.MeasureString(message, messageFont);

			g.Dispose();
			b.Dispose();

			Bitmap bmp = new Bitmap((int) (mainTitleSize.Width > messageTitleSize.Width ? mainTitleSize.Width :messageTitleSize.Width), (int)(mainTitleSize.Height + messageTitleSize.Height + 10));

			g = Graphics.FromImage(bmp);
			g.DrawString(mainTitle, mainTitleFont, new SolidBrush(Color.FromArgb(128, 0, 0, 0)), bmp.Width / 2 - mainTitleSize.Width / 2, 0);

			g.DrawString(message, messageFont, new SolidBrush(Color.FromArgb(128, 0, 0, 0)), bmp.Width / 2 - messageTitleSize.Width / 2, bmp.Height - messageTitleSize.Height);

			return bmp;
		}

		static Bitmap m_waterMark = drawSimpleWatermark("ComponentArt", "This control is for use within the charting product only");

		static private void Draw(Graphics g, Bitmap bmp, System.Windows.Forms.Control c) 
		{
			Type WizardElementType = Type.GetType("ComponentArt.Web.Visualization.Charting.Design.WizardElement");
			if (WizardElementType != null) 
			{
				Control ic = c.Parent;
				while (ic != null) 
				{
					if (ic.GetType() == WizardElementType || ic.GetType().IsSubclassOf(WizardElementType))
						return;
					ic = ic.Parent;
				}
			}
			g.DrawImage(bmp, new Rectangle(0, 0, c.Width, c.Height));
		}

		static public void Draw(Graphics g, System.Windows.Forms.Control c) 
		{
			Draw(g, m_waterMark, c);
		}

		static public void Draw(Graphics g, System.Windows.Forms.Control c, string mainTitle, string message) 
		{
			Bitmap bmp = drawSimpleWatermark(mainTitle, message);
			Draw(g, bmp, c);
			bmp.Dispose();
		}
	}
}
