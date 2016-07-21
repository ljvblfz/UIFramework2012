using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class Chart2DText : GeometricObject
	{
		LabelStyle  labelStyleRef=null;
		string		labelStyle=null;
		Vector3D	P;
		string		text;
		TextReferencePoint	referencePoint;

		#region --- Construction ---
		public Chart2DText()
		{
			text = "";
			P = new Vector3D(0,0,0);
			labelStyle = "Default";
		}

		public Chart2DText(LabelStyle labelStyleRef, Vector3D P, string text)
		{
			this.labelStyleRef = labelStyleRef;
			this.P = P;
			this.text = text;
			referencePoint = labelStyleRef.ReferencePoint;
		}

		#endregion

		#region --- Target coordinate range ---
		/// <summary>
		/// Default implementation of coordinate range, based on subobjects.
		/// </summary>
		/// <returns>Target coordinate range of this object</returns>
		internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			Mapping map = TargetArea.Mapping;
			TargetCoordinateRange tcr = new TargetCoordinateRange();
			if(usingTexts)
			{
				PointF sP, eP;
				GetTextCorners(out sP, out eP);
				// NOTE: This is not quit correct: all 4 texxt rectangle corners should be considered
				tcr.Include(new Vector3D(sP.X,sP.Y,0));
				tcr.Include(new Vector3D(eP.X,eP.Y,0));
			}
			return tcr;
		}
		#endregion

		#region --- Rendering ---

		internal void GetTextCorners(out PointF startingPoint, out PointF oppositePoint)
		{
			bool useStoredReferencePoint = true;
			if(labelStyleRef == null)
			{
				labelStyleRef = OwningChart.GetLabelStyle(labelStyle);
				useStoredReferencePoint = false;
			}
			 
			TextReferencePoint rp = labelStyleRef.ReferencePoint;
			if(useStoredReferencePoint)
				labelStyleRef.ReferencePoint = referencePoint;

			Font font = labelStyleRef.Font;
			bool toDisposeFont = false;
			if(OwningChart.Dpi != 96)
			{
				font = new Font(font.Name,(float)(font.Size*OwningChart.Dpi/96.0f),font.Style);
				toDisposeFont = true;
			}

			int yMax = OwningChart.TargetSize.Height;
			PointF PM = TargetArea.Mapping.PostProjectionMap(P.X,P.Y);
			float xT = (float)PM.X;
			float yT = (float)PM.Y;

			
			SizeF size = OwningChart.WorkingGraphics.MeasureString(text,font);
			// Enlarge to comply with post projection scaling
			size = new SizeF(size.Width*1.3f,size.Height*1.3f);

			float dxT = 0, dyT = 0;

			switch(labelStyleRef.HorizontalAlignment)
			{
				case HorizontalAlignment.Left:		dxT = 0;					break;
				case HorizontalAlignment.Center:	dxT = -size.Width*0.5f;	break;
				case HorizontalAlignment.Right:		dxT = -size.Width;		break;
			}

			switch(labelStyleRef.VerticalAlignment)
			{
				case VerticalAlignment.Bottom:		dyT = 0;					break;
				case VerticalAlignment.Center:		dyT = +size.Height*0.5f;	break;
				case VerticalAlignment.Top:			dyT = +size.Height;		break;
			}

			dxT += (float)labelStyleRef.HOffset;
			dyT += (float)labelStyleRef.VOffset;

			float angle = (float)labelStyleRef.Angle;

			float c = (float)Math.Cos(angle/180.0*Math.PI);
			float s = (float)Math.Sin(angle/180.0*Math.PI);
			xT += c*dxT - s*dyT;
			yT += s*dxT + c*dyT;

			startingPoint = new PointF(xT,yT);
			oppositePoint = new PointF(xT + c*size.Width - s*size.Height, yT + s*size.Width + c*size.Height);

			if(toDisposeFont)
				font.Dispose();
		}

		internal void Render(Graphics g)
		{
			bool useStoredReferencePoint = true;
			if(labelStyleRef == null)
			{
				labelStyleRef = OwningChart.GetLabelStyle(labelStyle);
				useStoredReferencePoint = false;
			}
			 
			TextReferencePoint rp = labelStyleRef.ReferencePoint;
			if(useStoredReferencePoint)
				labelStyleRef.ReferencePoint = referencePoint;

			Font font = labelStyleRef.Font;
			bool toDisposeFont = false;
			if(OwningChart.Dpi != 96)
			{
				font = new Font(font.Name,(float)(font.Size*OwningChart.Dpi/96.0f),font.Style);
				toDisposeFont = true;
			}

			int yMax = OwningChart.TargetSize.Height;
			PointF PM = TargetArea.Mapping.PostProjectionMap(P.X,P.Y);
			float xT = (float)PM.X;
			float yT = (float)PM.Y;

			
			SizeF size = g.MeasureString(text,font);

			float dxT = 0, dyT = 0;

			switch(labelStyleRef.HorizontalAlignment)
			{
				case HorizontalAlignment.Left:		dxT = 0;					break;
				case HorizontalAlignment.Center:	dxT = -size.Width*0.5f;	break;
				case HorizontalAlignment.Right:		dxT = -size.Width;		break;
			}

			switch(labelStyleRef.VerticalAlignment)
			{
				case VerticalAlignment.Bottom:		dyT = 0;					break;
				case VerticalAlignment.Center:		dyT = +size.Height*0.5f;	break;
				case VerticalAlignment.Top:			dyT = +size.Height;		break;
			}

			dxT += (float)labelStyleRef.HOffset;
			dyT += (float)labelStyleRef.VOffset;

			float angle = (float)labelStyleRef.Angle;
			float c = (float)Math.Cos(angle/180.0*Math.PI);
			float s = (float)Math.Sin(angle/180.0*Math.PI);
			xT += c*dxT - s*dyT;
			yT += s*dxT + c*dyT;

			GraphicsContainer container = g.BeginContainer();
			g.TextRenderingHint = TextRenderingHint.AntiAlias;	

			g.TranslateTransform(xT,yMax -yT);
			g.RotateTransform(-angle);

			xT = 0; yT = 0;
			if(labelStyleRef.ShadowDepthPxl>0)
			{
				Brush sBrush = new SolidBrush(labelStyleRef.ShadowColor);
				g.DrawString(text,font,sBrush,
					new PointF((float)(xT+labelStyleRef.ShadowDepthPxl),(float)(yT+labelStyleRef.ShadowDepthPxl)));
				sBrush.Dispose();
				sBrush = null;
			}
			Brush brush = new SolidBrush(labelStyleRef.ForeColor);

			g.DrawString(text,font,brush,xT,yT);
			brush.Dispose();
			brush = null;
			if(toDisposeFont)
				font.Dispose();

			g.EndContainer(container);
			
			labelStyleRef.ReferencePoint = rp;
		}
		#endregion
	}		
}
