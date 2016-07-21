using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace ComponentArt.WinUI
{
#if DEBUG
	public
#else
	internal
#endif
	class CheckBox : System.Windows.Forms.CheckBox
	{


		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
			}
		}


#if false
		protected override void OnPaint(PaintEventArgs pe) 
		{
			
			Bitmap paintHere = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(paintHere);
			PaintEventArgs paintHereEventArgs = new PaintEventArgs(g, pe.ClipRectangle);

			base.OnPaint(paintHereEventArgs);

            
			LayoutData ld = new LayoutData();
			CalcCheckmarkRectangle(ld, this, RtlTranslateContent( this.CheckAlign));

			if (Checked)
			{
				for (int x=2; x<ld.checkBounds.Size.Width-2; ++x) 
				{
					for (int y=2; y<ld.checkBounds.Size.Height-2; ++y) 
					{
						try 
						{
							Color OrigColor = paintHere.GetPixel(ld.checkBounds.X+x, ld.checkBounds.Y+y);

							if (OrigColor.R + OrigColor.B + OrigColor.G < 128 * 3)
								paintHere.SetPixel(ld.checkBounds.X+x, ld.checkBounds.Y+y, Color.FromArgb(255, 0, 0));
						}
						catch (Exception ex) 
						{
							throw ex;
						}
					}
				}
			}


			pe.Graphics.DrawImage(paintHere, 0, 0);
		}
#endif

    }
}
