using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for IEngine.
	/// </summary>
	internal interface IEngine
	{
        Factory Factory { get; }

		object GetImage(string imageFile);

		void FillArea(PointF[] points,Color color, RenderingContext context);
		void FillRadialArea(PointF[] points,MultiColor mc, Color centerColor, Point2D centerPoint, Point2D[] gradientLine, RenderingContext context);
		void FillLinearArea(PointF[] points,MultiColor mc, Point2D gradientStartPoint, Point2D gradientEndPoint, RenderingContext context);

		LayerVisualPart IndicatorCircleImage();
		LayerVisualPart IndicatorRectImage();
		
		void DrawBackground(object backgroundImage,ImageLayout backgroundImageLayout,RenderingContext context);
		void DrawNiddle(LayerVisualPart part, Point2D centerPoint, Point2D tipPoint, RenderingContext context);
		void DrawNiddleRegion(LayerVisualPart part, Point2D centerPoint, Point2D tipPoint, RenderingContext context, Color color);
        void DrawTickMark(TickMark marker, float dx, float dy, RenderingContext context, TickMarkRenderingContext tmContext);
		MapArea TickMarkMapArea(TickMark marker, float dx, float dy, RenderingContext context);
		void DrawText(TextAnnotation text, RenderingContext context, TextRenderingContext tmContext);
		void DrawText(string text, TextStyle textStyle, RenderingContext context, TextRenderingContext tmContext);
		void DrawTextWithCharacterOverlay(string text, TextStyle textStyle, Layer characterOverlayLayer, RenderingContext context, TextRenderingContext tmContext);
		void DrawLCDText(string text, RenderingContext context, TextRenderingContext tmContext);
		void DrawImage(object imageObject, Size2D relativeSize, Point2D location, RenderingContext context);
		
		void RenderWatermark(RenderingContext context);
		void RenderErrorMessage(string message, RenderingContext context);
	}
}
