using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	internal class ChartShadeFrameInner : ChartFrameInner
	{
		const int _raisedShadeWidthDefault = 5;
		const bool _softShadeDefault = true;

		private int		raisedShadeWidth = _raisedShadeWidthDefault;		
		private bool		softShade = _softShadeDefault;

		private ChartFrameShadePrimitive raisedShade = null;
		private ChartFrameShadePosition internalShadePosition = ChartFrameShadePosition.NoShade;

		public ChartShadeFrameInner() { }

		#region --- Properties ---

		[NotifyParentProperty(true)]
		[DefaultValue(ChartFrameShadePosition.NoShade)]
		public ChartFrameShadePosition ShadePosition	{ get { return internalShadePosition; }		set { internalShadePosition = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_raisedShadeWidthDefault)]
		public int		ShadeWidth						{ get { return raisedShadeWidth; }			set { raisedShadeWidth = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_softShadeDefault)]
		public bool		SoftShade						{ get { return softShade; }					set { softShade = value; } }

		private bool ShouldSerializeShadePosition()	{ return ShadePosition != ChartFrameShadePosition.NoShade; }
		private bool ShouldSerializeShadeWidth()	{ return raisedShadeWidth != _raisedShadeWidthDefault; }
		private bool ShouldSerializeSoftShade()		{ return SoftShade != _softShadeDefault; }
		#endregion

		public void AddRaisedShade()
		{
			raisedShade = new ChartFrameShadePrimitive(Rectangle);
			raisedShade.ShadePosition = ChartFrameShadePosition.RightBottom;
			raisedShade.CornerRadius = CornerRadius*scaleFactor;
			raisedShade.RoundBottom = RoundBottomCorners;
			raisedShade.RoundTop = RoundTopCorners;
			raisedShade.Width = (int)(raisedShadeWidth*scaleFactor);	
			raisedShade.Outside = false;
			return;
		}

		private Rectangle ExternalRectangle()
		{
			return Rectangle;
		}

		public override void Render(Graphics g)
		{
			AddRaisedShade();

			Rectangle R = ExternalRectangle();

			// Raised Shade

			if(raisedShade != null)
			{
				raisedShade.ShadePosition = ShadePosition;
				raisedShade.Width = ShadeWidth;
				raisedShade.CornerRadius = CornerRadius;
				raisedShade.RoundBottom = RoundBottomCorners;
				raisedShade.RoundTop = RoundTopCorners;
				raisedShade.Rectangle = R;
				raisedShade.Render(g);
			}
		}
	}
}
