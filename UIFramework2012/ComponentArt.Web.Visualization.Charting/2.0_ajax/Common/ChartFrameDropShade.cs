using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// =============================================================================================================
	//	ChartFrameDropShade Class
	// =============================================================================================================

	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	internal class ChartFrameDropShade
	{
		const int _dropShadeWidthDefault = 10;
		const bool _softShadeDefault = true;

		private Rectangle	rect;
		private bool		roundTop = false;
		private bool		roundBottom = false;
		private int			cornerRadius = 10;
		private int			dropShadeWidth = _dropShadeWidthDefault;
		private Color		outsideColor = SystemColors.Control;
		private Color		borderColor = Color.FromArgb(100,0,0,0);
		
		private ChartFrameShadePrimitive dropShade = null;
		private bool		softShade = _softShadeDefault;
		private float		scaleFactor = 1.0f;
		private Color		formColor = Color.White;

		private ChartFrameShadePosition externalShadePosition = ChartFrameShadePosition.NoShade;

		public ChartFrameDropShade() { }

		#region --- Properties ---

		[NotifyParentProperty(true)]
		[DefaultValue(ChartFrameShadePosition.NoShade)]
		public ChartFrameShadePosition ShadePosition{ get { return externalShadePosition; }		set { externalShadePosition = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_dropShadeWidthDefault)]
		public int		ShadeWidth					{ get { return dropShadeWidth; }			set { dropShadeWidth = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_softShadeDefault)]
		public bool		SoftShade					{ get { return softShade; }					set { softShade = value; } }
		[Browsable(false)]
		internal bool	RoundTopCorners				{ get { return roundTop; }					set { roundTop = value; } }
		[Browsable(false)]
		internal bool	RoundBottomCorners			{ get { return roundBottom; }				set { roundBottom = value; } }

		internal Rectangle Rectangle { get { return rect; } set { rect = value; } }
		#endregion

		internal void SetScaleFactor(float scale)
		{
			scaleFactor = scale;
		}
		internal Color FormColor { get { return formColor; } set { formColor = value; } }

		internal int CornerRadius { get { return cornerRadius; } set { cornerRadius = value; } }

		public void AddDropShade()
		{
			if(ShadePosition == ChartFrameShadePosition.NoShade)
			{
				dropShade = null;
				return;
			}
			dropShade = new ChartFrameShadePrimitive(rect);
			dropShade.ShadePosition = ShadePosition;
			dropShade.CornerRadius = cornerRadius*scaleFactor;
			dropShade.RoundBottom = roundBottom;
			dropShade.RoundTop = roundTop;
			dropShade.Width = (int)(dropShadeWidth*scaleFactor);	
			dropShade.Outside = true;
			dropShade.Soft = softShade;
		}

		public void Render(Graphics g)
		{
			AddDropShade();
			if(dropShade == null)
				return;				

			dropShade.Rectangle = rect;
			dropShade.FormColor = formColor;
			dropShade.Render(g);
		}
	}
}
