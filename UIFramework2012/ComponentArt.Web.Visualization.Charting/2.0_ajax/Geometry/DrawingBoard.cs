using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for DrawingPanel.
	/// </summary>
	internal class DrawingBoard : GeometricObject
	{
#if DEBUG
		internal static int created = 0;
		internal static int disposed = 0;
#endif
		protected Vector3D	v0,v1,v2,v3,vx=new Vector3D(0,0,0),vy=new Vector3D(0,0,0);
		protected Vector3D  vOffset = new Vector3D(0,0,0);
		protected Graphics	g;
		protected double	reflection = 0.5;
		protected int		logPhong = 6;
		protected double	growth = 0;

		protected int		ix0,ix1,iy0,iy1,yH;
		protected Vector3D	Vx1,Vy1;
		protected double	liftZ = 0.5;

		protected bool		lightsOff = false;

		// Computing Position
		protected bool computingPosition = false;
		protected double x0p,x1p,y0p,y1p;

		protected object activeObject = null;

		private GeometricEngine ge = null;

		#region --- Construction and Initialization ---
		public DrawingBoard(){}
		public DrawingBoard(Vector3D V0, Vector3D Vx, Vector3D Vy)
		{
			v0 = V0;
			vx = Vx;
			vy = Vy;
		}

		public GeometricEngine GE { get { return ge; } set { ge = value; } }

		public virtual void Grow(double deltaWorld)
		{
		}


		internal new virtual void Clear()
		{
		}

		internal virtual void Initialize()
		{
			ix0 = 0;
			iy0 = 0; // TODO: this may deppend on target area?
			Vx1 = vx.Unit();
			Vy1 = vy.Unit();
		}

		internal void Test()
		{
			if(g==null)
				Initialize();
			g.Clear(Color.FromArgb(100,0,0,255));// Test filling bitmap
		}
		#endregion
		
		internal Rectangle GetPositionRectangle()
		{
			return new Rectangle((int)x0p, (int)y0p, (int)(x1p-x0p+1),(int)(y1p-y0p+1));
		}

		internal virtual void SetActiveObject(object obj) 
		{
			Emit(BoardCommand.ActiveObject,obj);
		}

		internal void ClearDefaultActiveObject()
		{
			Tag = null;
		}

		internal virtual void ProcessObjectTrackingArea(GraphicsPath bmpPath) { }

		#region --- Properties ---

		
		public Vector3D V0 { get { return v0; } set { if(v0 != value) Clear(); v0 = value; } }
		public Vector3D Vx { get { return vx; } set { if(vx != value) Clear(); vx = value; } }
		public Vector3D Vy { get { return vy; } set { if(vy != value) Clear(); vy = value; } }
		public int		Yh { get { return yH; } set { if(yH != value) Clear(); yH = value; } }
		public double	Reflection	{ get { return reflection; } set { if(0 < value && value < 1.0) reflection = value; } }
		public int		LogPhong	{ get { return logPhong; } set { logPhong = Math.Max(1,Math.Min(8,value)); } }
		internal bool	LightsOff	{ get { return lightsOff; } set { lightsOff = value; } }

		internal bool IsInitialized { get { return !vx.IsNull && !vy.IsNull; } }

		internal double LiftZ { get { return liftZ; } set { liftZ = value; } }

		internal Graphics G 
		{
			get
			{
				if(g == null)
					Initialize();
				return g;
			}
		}

        // Properties used in the board rendering only
        internal Graphics Graphics { get { return g; } }
        internal Vector3D V1 { get { return v1; } }
        internal Vector3D V2 { get { return v2; } }
        internal Vector3D V3 { get { return v3; } }
        internal int Ix0 { get { return ix0; } }
        internal int Iy0 { get { return iy0; } }

        #endregion
		
        #region --- Coordinate Transformations ---

		
		public PointF WorldToBoard(Vector3D V)
		{
			return new PointF((float)WorldToBoardX(V),(float)WorldToBoardY(V));
		}

		public double WorldToBoardX(Vector3D V)
		{
			if(g==null)
				Initialize();
			return (V-v0-vOffset)*Vx1;
		}

		public double WorldToBoardY(Vector3D V)
		{
			if(g==null)
				Initialize();
			return (V-v0-vOffset)*Vy1;
		}

		public PointF WorldToBmp(Vector3D V)
		{
			return BoardToBmp(WorldToBoardX(V),WorldToBoardY(V));
		}

		public PointF BoardToBmp(PointF boardPoint)
		{
			return BoardToBmp(boardPoint.X,boardPoint.Y);
		}

		public PointF BoardToBmp(double xBoard, double yBoard)
		{
			if(g==null)
				Initialize();
			Vector3D wp1 = v0 + vOffset + Vx1*xBoard + Vy1*yBoard;
			Vector3D P;
			Mapping.Map(wp1, out P);
			return new PointF((float)(P.X-ix0-0.5),(float)(P.Y-iy0-0.5));
		}

		public GraphicsPath BoardToBmp(GraphicsPath path)
		{
			PointF[] points0 = path.PathPoints;
			PointF[] points = new PointF[path.PointCount];
			byte[] types =  path.PathTypes;
			for(int i=0;i<path.PointCount;i++)
				points[i] = BoardToBmp(points0[i]);
			return new GraphicsPath(points,types,path.FillMode);
		}

		#endregion

		#region --- Emitting and Executing Commands ---

		#region --- Emitting Commands
		private ArrayList commands = new ArrayList();
		protected enum BoardCommand
		{
			RenderLineWithStyle,
			RenderLineWithPen,
			RenderLines,
			RenderPath,
			RenderPathWithPen,
            RenderFillPath,
            RenderFillPathWithGradientStyle,
            ActiveObject
		}

		protected void Emit(params object[] list)
		{
			for(int i=0; i<list.Length; i++)
				commands.Add(list[i]);
		}

		public void DrawLine(LineStyle2D style, Vector3D point1, Vector3D point2)
		{
			Emit(BoardCommand.RenderLineWithStyle,style,point1,point2);
		}

		public void DrawPath(LineStyle2D style, GraphicsPath path, bool drawLine, bool drawShade, bool releasePathAfterRendering)
		{
			Emit(BoardCommand.RenderPath,style,path,drawLine,drawShade,releasePathAfterRendering);
		}
		
		public void DrawPath(Pen pen, GraphicsPath path, bool releasePenAfterRendering, bool releasePathAfterRendering)
		{
			Emit(BoardCommand.RenderPathWithPen, pen, path, releasePenAfterRendering, releasePathAfterRendering);
		}

		public void DrawLines(LineStyle2D style, PointF[] pts, bool drawLine, bool drawShade)
		{
			Emit(BoardCommand.RenderLines,style,pts,drawLine,drawShade);
		}

		public void DrawLine(Pen pen, Vector3D point1, Vector3D point2, bool releasePenAfterRendering)
		{
			Emit(BoardCommand.RenderLineWithPen,pen,point1,point2,releasePenAfterRendering);
		}

        public void FillPath(Brush brush, GraphicsPath path, bool releaseBrushAfterRendering, bool releasePathAfterRendering)
        {
            Emit(BoardCommand.RenderFillPath, brush, path, releaseBrushAfterRendering, releasePathAfterRendering);
        }

        public void FillPath(GradientStyle gradientStyle, PointF[] gradientStylePoints, GraphicsPath path, bool releasePathAfterRendering)
        {
            Emit(BoardCommand.RenderFillPathWithGradientStyle, gradientStyle, gradientStylePoints, path, releasePathAfterRendering);
        }

		#endregion

		#region --- RenderingCommands 

		int ixCommand;
		private BoardCommand GetBoardCommand { get { return (BoardCommand)commands[ixCommand++]; } }
		private Vector3D GetVector3D { get { return (Vector3D)commands[ixCommand++]; } }
		private LineStyle2D GetLineStyle2D { get { return commands[ixCommand++] as LineStyle2D; } }
		private GraphicsPath GetGraphicsPath { get { return commands[ixCommand++] as GraphicsPath; } }
		private bool GetBool { get { return (bool)commands[ixCommand++]; } }
		private Pen GetPen { get { return commands[ixCommand++] as Pen; } }
		private Brush GetBrush { get { return commands[ixCommand++] as Brush; } }
        private PointF[] GetPoints { get { return commands[ixCommand++] as PointF[]; } }
        private GradientStyle GetGradientStyle { get { return commands[ixCommand++] as GradientStyle; } }
        private object GetObject { get { return commands[ixCommand++]; } }

		internal virtual void RenderContents(bool computingPosition)
		{
			PrepareToRenderContents();

			this.computingPosition = computingPosition;
			if(computingPosition)
			{
				x0p = double.MaxValue;
				y0p = double.MaxValue;
				x1p = double.MinValue;
				y1p = double.MinValue;
			}
			try
			{
				ixCommand = 0;
				while(ixCommand < commands.Count)
				{
					BoardCommand bcmd = GetBoardCommand;
					switch(bcmd)
					{
                        case BoardCommand.RenderFillPath:
                            RenderFillPath(GetBrush, GetGraphicsPath, GetBool, GetBool);
                            break;
                        case BoardCommand.RenderFillPathWithGradientStyle:
                            RenderFillPath(GetGradientStyle, GetPoints, GetGraphicsPath, GetBool);
                            break;
                        case BoardCommand.RenderLineWithPen:
							RenderLine(GetPen,GetVector3D,GetVector3D,GetBool);
							break;
						case BoardCommand.RenderLines:
							RenderLines(GetLineStyle2D,GetPoints,GetBool, GetBool);
							break;
						case BoardCommand.RenderLineWithStyle:
							RenderLine(GetLineStyle2D,GetVector3D,GetVector3D);
							break;
						case BoardCommand.RenderPath:
							RenderPath(GetLineStyle2D,GetGraphicsPath,GetBool,GetBool,GetBool);
							break;
						case BoardCommand.RenderPathWithPen:
							RenderPath(GetPen,GetGraphicsPath,GetBool,GetBool);
							break;
						case BoardCommand.ActiveObject:
							activeObject = GetObject;
							GE.SetActiveObject(activeObject);
							break;
					}
				}
				commands.Clear();
			}
			catch(Exception ex)
			{
				string t = ex.StackTrace;
			}
		}

		internal virtual void PrepareToRenderContents()
		{
		}

		#endregion

		#endregion

		#region --- High Level Drawing Primitives ---

		public void DrawRectangle(LineStyle2D lineStyle, GradientStyle gradientStyle, Vector3D point1, Vector3D point2)
		{
			// Board coordinates
			float x0 = (float)WorldToBoardX(point1);
			float y0 = (float)WorldToBoardY(point1);
			float x1 = (float)WorldToBoardX(point2);
			float y1 = (float)WorldToBoardY(point2);

			PointF[] points = new PointF[]
				{
					new PointF(x0,y0),
					new PointF(x0,y1),
					new PointF(x1,y1),
					new PointF(x1,y0),
					new PointF(x0,y0)
				};
			DrawArea(lineStyle,gradientStyle,points);
		}

		public void DrawArea(LineStyle2D lineStyle, GradientStyle gradientStyle, PointF[] points)
		{
            GraphicsPath path = new GraphicsPath();
            path.AddLines(points);
            path.CloseFigure();
			FillPath(gradientStyle,points,path,false);
			DrawPath(lineStyle,path,true,false,true);
		}

		public void DrawString(string txt, string familyName, double size, FontStyle style, Brush brush, PointF point, bool releaseBrushAfterRendering)
		{
			DrawString(txt,familyName,size,style,brush,point,TextReferencePoint.LeftBottom,0,0,0, releaseBrushAfterRendering);
		}

		public void DrawString(string txt, string familyName, double size, FontStyle style, 
			Brush brush, PointF point, TextReferencePoint refPt, double xOffsetPts, double yOffsetPts,
			double angleDegrees, bool releaseBrushAfterRendering)
		{
			if(txt == null || txt.Trim() == "")
				return;
			GraphicsPath path = CreateStringPath(txt,familyName,size,style,point,refPt,xOffsetPts,yOffsetPts,angleDegrees);
			FillPath(brush,path,releaseBrushAfterRendering,true);
		}

		public void DrawString(string txt, LabelStyle style, PointF point, double angleDegrees)
		{
			string familyName = style.Font.Name;
			double size = style.Font.SizeInPoints;
			FontStyle fontStyle = style.Font.Style;
			Color fontColor = style.ForeColor;
			TextReferencePoint refPt = style.ReferencePoint;
			double xOffsetPt = style.HOffset;
			double yOffsetPt = style.VOffset;

			float shOffset = (float)style.ShadowDepthPxl;
			if(shOffset != 0.0)
			{
				// draw shadow first
				Color shColor = style.ShadowColor;
				DrawString(txt,familyName,size,fontStyle,new SolidBrush(shColor),new PointF(point.X+shOffset,point.Y+shOffset),refPt,xOffsetPt,yOffsetPt, angleDegrees,true);
			}
			DrawString(txt,familyName,size,fontStyle,new SolidBrush(fontColor),point,refPt,xOffsetPt,yOffsetPt, angleDegrees,true);
		}

		public void DrawString(string txt, TextStyle style, PointF point, double angleDegrees)
		{
			string familyName = style.Font.Name;
			double size = style.Font.SizeInPoints;
			FontStyle fontStyle = style.Font.Style;
			Color fontColor = style.ForeColor;
			TextReferencePoint refPt = TextReferencePoint.LeftBottom;
			double xOffsetPt = 0;
			double yOffsetPt = 0;

			float shOffset = (float)(style.ShadowDepthPxl/Mapping.Enlargement);
			if(shOffset != 0.0)
			{
				// draw shadow first
				Color shColor = style.ShadowColor;
				DrawString(txt,familyName,size,fontStyle,new SolidBrush(shColor),new PointF(point.X+shOffset,point.Y-shOffset),refPt,xOffsetPt,yOffsetPt, angleDegrees,true);
			}
			DrawString(txt,familyName,size,fontStyle,new SolidBrush(fontColor),point,refPt,xOffsetPt,yOffsetPt, angleDegrees,true);
		}
		
		#endregion
		
		#region --- Drawing Primitives ---

		public void DrawPieSegment(LineStyle2D lineStyle, double xC, double yC, double innerRadius, double outerRadius, 
			double shift, double low, double high)
		{

			int nP = (int)(100*(high-low)/Math.PI);
			if(nP<2)
				nP = 2;
			double da = (high-low)/(nP-1);
			
			PointF[] pts = new PointF[nP*2 +1];
			int i,n;
			double a = low;
			n = 2*nP-1;
			for(i=0;i<nP;i++)
			{
				float c = (float)Math.Cos(a);
				float s = (float)Math.Sin(a);
				pts[i] = new PointF((float)(xC + c*outerRadius),(float)(yC + s*outerRadius));
				pts[n] = new PointF((float)(xC + c*innerRadius),(float)(yC + s*innerRadius));
				a = a+da;
				n--;
			}
			pts[nP*2] = pts[0];
			DrawLines(lineStyle,pts,true,true);
		}


		#region --- Primitive 2D Features

		public void DrawRectangle(Pen pen, Vector3D point1, Vector3D point2, bool releasePenAfterRendering)
		{
			// Board coordinates
			float x0 = (float)WorldToBoardX(point1);
			float y0 = (float)WorldToBoardY(point1);
			float x1 = (float)WorldToBoardX(point2);
			float y1 = (float)WorldToBoardY(point2);

			PointF[] points = new PointF[]
				{
					new PointF(x0,y0),
					new PointF(x0,y1),
					new PointF(x1,y1),
					new PointF(x1,y0)
				};

			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(points);
			path.CloseFigure();
			DrawPath(pen,path,releasePenAfterRendering,true);
		}

		public void FillRectangle(Brush brush, Vector3D point1, Vector3D point2, bool releaseBrushAfterRendering)
		{
			// Board coordinates
			float x0 = (float)WorldToBoardX(point1);
			float y0 = (float)WorldToBoardY(point1);
			float x1 = (float)WorldToBoardX(point2);
			float y1 = (float)WorldToBoardY(point2);

			float xx0 = Math.Min(x0,x1);
			float yy0 = Math.Min(y0,y1);
			float xx1 = Math.Max(x0,x1);
			float yy1 = Math.Max(y0,y1);

			if(xx0<xx1 && yy0<yy1)
			{
				PointF[] points = new PointF[]
				{
					new PointF(xx0,yy0),
					new PointF(xx0,yy1),
					new PointF(xx1,yy1),
					new PointF(xx1,yy0)
				};

				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);
				path.CloseFigure();
				FillPath(brush,path,releaseBrushAfterRendering,true);
			}
		}

        public void DrawEllipse(LineStyle2D lineStyle, Vector3D center, double xAxis, double yAxis)
        {
            // Board coordinates
            float x0 = (float)WorldToBoardX(center);
            float y0 = (float)WorldToBoardY(center);

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse((float)(x0 - xAxis), (float)(y0 - yAxis), (float)(xAxis * 2.0f), (float)(yAxis * 2.0f));
            DrawPath(lineStyle, path, true, lineStyle.ShadeWidth > 0, true);
        }

		public void FillEllipse(Brush brush, Vector3D center, double xAxis, double yAxis, bool releaseBrushAfterRendering)
		{
			// Board coordinates
			float x0 = (float)WorldToBoardX(center);
			float y0 = (float)WorldToBoardY(center);

			GraphicsPath path = new GraphicsPath();
			path.AddEllipse((float)(x0-xAxis),(float)(y0-yAxis),(float)(xAxis*2.0f),(float)(yAxis*2.0f));
			FillPath(brush,path,releaseBrushAfterRendering,true);
		}

		#endregion

		#region --- Texts ---

		public GraphicsPath CreateStringPath(string txt, string familyName, double size, FontStyle style, PointF pt, TextReferencePoint referencePoint, double xOffsetPoints, double yOffsetPoints, double angleDeg)
		{
			if(g==null)
				Initialize();
			double fromPointToWorld = Mapping.FromPointToWorld;
			return CreateStringPath(txt,familyName,size,style,pt,referencePoint,xOffsetPoints,yOffsetPoints,angleDeg,fromPointToWorld);
		}

		public static GraphicsPath CreateStringPath(string txt, string familyName, double size, FontStyle style, 
			PointF pt, TextReferencePoint referencePoint, double xOffsetPoints, double yOffsetPoints, 
			double angleDeg, double fromPointToWorld)
		{
			TextReferencePoint refPt = referencePoint;
			double xOffsetPts = xOffsetPoints;
			double yOffsetPts = yOffsetPoints;
			double angleDegrees = angleDeg;

			FixAngleAndReferencePoint(ref refPt, ref xOffsetPts, ref yOffsetPts, ref angleDegrees);
			
			GraphicsPath path = new GraphicsPath(FillMode.Alternate);
			PointF point = pt;

			FontFamily family = new FontFamily(familyName);
			float scaledSize = (float)(size*fromPointToWorld);
			// Create text at (0,0), horizontaly
			path.AddString(txt,family,(int)style,scaledSize,new PointF(0,0),StringFormat.GenericTypographic);
			
			// Transforming coordinates
			
			RectangleF rect = path.GetBounds();
			PointF[] points = path.PathPoints;
			// Offsets
			float offX = (float)(xOffsetPts*fromPointToWorld);
			float offY = (float)(yOffsetPts*fromPointToWorld);
			float h1 = 0, h0 = float.MaxValue;	
			float w1 = 0, w0 = float.MaxValue;
			for(int i=0; i<path.PointCount;i++)
			{
				h0 = Math.Min(h0,points[i].Y);
				h1 = Math.Max(h1,points[i].Y);
				w0 = Math.Min(w0,points[i].X);
				w1 = Math.Max(w1,points[i].X);
			}
			offX -= w0;
			offY += h0;
			float h = h1-h0;
			float w = w1-w0;
			switch(refPt)
			{
				case TextReferencePoint.LeftBottom:
					break;
				case TextReferencePoint.LeftCenter:
					offY -= h/2;
					break;
				case TextReferencePoint.LeftTop:
					offY -= h;
					break;
				case TextReferencePoint.CenterBottom:
					offX -= w/2;
					break;
				case TextReferencePoint.Center:
					offY -= h/2;
					offX -= w/2;
					break;
				case TextReferencePoint.CenterTop:
					offY -= h;
					offX -= w/2;
					break;
				case TextReferencePoint.RightBottom:
					offX -= w;
					break;
				case TextReferencePoint.RightCenter:
					offY -= h/2;
					offX -= w;
					break;
				case TextReferencePoint.RightTop:
					offY -= h;
					offX -= w;
					break;
			}

			float c = (float)Math.Cos(angleDegrees*Math.PI/180.0f);
			float s = (float)Math.Sin(angleDegrees*Math.PI/180.0f);
			for(int i=0; i<path.PointCount;i++)
			{
				float x = points[i].X;
				float y = points[i].Y;
				// Flip it, since GDI reverses y coordinate 
				y = h - y;
				// Shift, based on ref point and offsets
				x += offX;
				y += offY;
				// Rotate around the ref point
				points[i].X = x*c - y*s;
				points[i].Y = x*s + y*c;
				// Move ref point to the origin
				points[i].X += point.X;
				points[i].Y += point.Y;
			}

			family.Dispose();
			
			GraphicsPath newPath = new GraphicsPath(points,path.PathTypes,path.FillMode);
			path.Dispose();
			path = null;
			return newPath;
		}

		public void OutlineString(string txt, string familyName, double size, FontStyle style, Pen pen, PointF point,
			TextReferencePoint refPt, double xOffsetPts, double yOffsetPts, double angleDegrees, bool releasePenAfterRendering)
		{
			GraphicsPath path = CreateStringPath(txt,familyName,size,style,point,refPt,xOffsetPts,yOffsetPts,angleDegrees);
			DrawPath(pen,path,releasePenAfterRendering,true);
		}

		#endregion

		private static void FixAngleAndReferencePoint(ref TextReferencePoint refPt, ref double xOffsetPts, ref double yOffsetPts, ref double angleDegrees)
		{
			while(angleDegrees > 360) angleDegrees -= 360;
			while(angleDegrees < 0.0) angleDegrees += 360;

			if(angleDegrees <= 90.5)
				return;
			if(angleDegrees < 270)
			{
				angleDegrees -=180;
				xOffsetPts = -xOffsetPts;
				yOffsetPts = -yOffsetPts;
				switch(refPt)
				{
					case TextReferencePoint.LeftBottom:
						refPt = TextReferencePoint.RightTop;
						break;
					case TextReferencePoint.LeftCenter:
						refPt = TextReferencePoint.RightCenter;
						break;
					case TextReferencePoint.LeftTop:
						refPt = TextReferencePoint.RightBottom;
						break;
					case TextReferencePoint.CenterBottom:
						refPt = TextReferencePoint.CenterTop;
						break;
					case TextReferencePoint.Center:
						break;
					case TextReferencePoint.CenterTop:
						refPt = TextReferencePoint.CenterBottom;
						break;
					case TextReferencePoint.RightBottom:
						refPt = TextReferencePoint.LeftTop;
						break;
					case TextReferencePoint.RightCenter:
						refPt = TextReferencePoint.LeftCenter;
						break;
					case TextReferencePoint.RightTop:
						refPt = TextReferencePoint.LeftBottom;
						break;
				}
			}
		}

		#endregion

		#region --- Rendering Functions ---

		internal virtual void RenderArea(LineStyle2D lineStyle, Brush brush, PointF[] points, bool releaseBrushAfterRendering)
		{
			// Draw shade 
			DrawLines(lineStyle,points,false,true);
			// Fill area
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(points);
			FillPath(brush,path,releaseBrushAfterRendering,true);
			// Draw line
			DrawLines(lineStyle,points,true,false);
			if (points[0].X != points[points.Length - 1].X || points[0].Y != points[points.Length - 1].Y)
			{
				PointF[] points2 = new PointF[] { points[0], points[points.Length - 1] };
				DrawLines(lineStyle, points2, true, false);
			}
			if(releaseBrushAfterRendering)
				brush.Dispose();
		}

		internal virtual void RenderLine(LineStyle2D style, Vector3D point1, Vector3D point2)
		{
			PointF bmpPoint1 = WorldToBmp(point1);
			PointF bmpPoint2 = WorldToBmp(point2);

			if(computingPosition)
			{
				x0p = Math.Min(x0p,bmpPoint1.X);
				y0p = Math.Min(y0p,bmpPoint1.Y);
				x1p = Math.Max(x1p,bmpPoint1.X);
				y1p = Math.Max(y1p,bmpPoint1.Y);
				x0p = Math.Min(x0p,bmpPoint2.X);
				y0p = Math.Min(y0p,bmpPoint2.Y);
				x1p = Math.Max(x1p,bmpPoint2.X);
				y1p = Math.Max(y1p,bmpPoint2.Y);
				return;
			}

			if(style == null)
				return;
			if(g==null)
				Initialize();
			float scaledWidth = (float)(style.Width*Mapping.FromPointToTarget);
			if(style.ShadeWidth > 0.0)
			{
				float scaledShadeWidth = (float)(style.ShadeWidth*Mapping.FromPointToTarget);
				float scaleShift = (float)(scaledShadeWidth*Math.Sqrt(2.0)/4);
				SolidBrush shBrush = new SolidBrush(Color.FromArgb(128,0,0,0));
				Pen shPen = new Pen(shBrush,scaledWidth);
				shPen.DashStyle = style.DashStyle;
				PointF bmpPoint1s = new PointF(bmpPoint1.X+scaleShift,bmpPoint1.Y-scaleShift);
				PointF bmpPoint2s = new PointF(bmpPoint2.X+scaleShift,bmpPoint2.Y-scaleShift);
				g.DrawLine(shPen,bmpPoint1s,bmpPoint2s);
				shPen.Dispose();
				shBrush.Dispose();
			}

			SolidBrush brush = new SolidBrush(style.Color);
			Pen pen = new Pen(brush,scaledWidth);
			pen.DashStyle = style.DashStyle;

			g.DrawLine(pen,bmpPoint1,bmpPoint2);
			pen.Dispose();
			brush.Dispose();
		}
		internal virtual void RenderPath(LineStyle2D style, GraphicsPath path, bool drawLine, bool drawShade, bool releasePathAfterRendering)
		{
			GraphicsPath pathM = BoardToBmp(path);

			if(computingPosition)
			{
				RectangleF r = pathM.GetBounds();
				x0p = Math.Min(x0p,r.X);
				y0p = Math.Min(y0p,r.Y);
				x1p = Math.Max(x1p,r.X+r.Width);
				y1p = Math.Max(y1p,r.Y+r.Height);
				if(releasePathAfterRendering)
					path.Dispose();
				pathM.Dispose();
				return;
			}
			if(g==null)
				Initialize();

			float scaledWidth = (float)(style.Width*Mapping.FromPointToTarget);
			if(style.ShadeWidth > 0.0 && drawShade)
			{
				float scaledShadeWidth = (float)(style.ShadeWidth*Mapping.FromPointToTarget);
				float scaleShift = (float)(scaledShadeWidth*Math.Sqrt(2.0)/4);
				PathData PD = pathM.PathData;
				for(int i=0;i<PD.Points.Length;i++)
				{
					PD.Points[i].X += scaleShift;
					PD.Points[i].Y -= scaleShift;
				}
				Pen shPen = new Pen(new SolidBrush(Color.FromArgb(128,0,0,0)),scaledShadeWidth);
				shPen.DashStyle = style.DashStyle;
				GraphicsPath shadePath = new GraphicsPath(PD.Points,PD.Types);
				g.DrawPath(shPen,shadePath);
				shPen.Dispose();
				shadePath.Dispose();
				for(int i=0;i<PD.Points.Length;i++)
				{
					PD.Points[i].X -= scaleShift;
					PD.Points[i].Y += scaleShift;
				}
			}

			if(drawLine)
			{
				Pen pen = new Pen(new SolidBrush(style.Color),scaledWidth);
				pen.DashStyle = style.DashStyle;
				g.DrawPath(pen,pathM);
				pen.Dispose();
			}
			pathM.Dispose();
			if(releasePathAfterRendering)
				path.Dispose();
		}

		internal virtual void RenderLines(LineStyle2D style, PointF[] pts, bool drawLine, bool drawShade)
		{
			if(computingPosition)
			{
				for(int i=0; i<pts.Length; i++)
				{
					x0p = Math.Min(x0p,pts[i].X);
					y0p = Math.Min(y0p,pts[i].Y);
					x1p = Math.Max(x1p,pts[i].X);
					y1p = Math.Max(y1p,pts[i].Y);
				}
				return;
			}

			if(style == null)
				return;
			if(g==null)
				Initialize();

			int nPts = pts.Length;
			PointF[] ptsM = new PointF[nPts];
			for(int i=0;i<nPts;i++)
				ptsM[i] = BoardToBmp(pts[i].X,pts[i].Y);

			float scaledWidth = (float)(style.Width*Mapping.FromPointToTarget);
			if(style.ShadeWidth > 0.0 && drawShade)
			{
				float scaledShadeWidth = (float)(style.ShadeWidth*Mapping.FromPointToTarget);
				float scaleShift = (float)(scaledShadeWidth*Math.Sqrt(2.0)/4);
				for(int i=0;i<nPts;i++)
				{
					ptsM[i].X += scaleShift;
					ptsM[i].Y -= scaleShift;
				}
				Pen shPen = new Pen(new SolidBrush(Color.FromArgb(128,0,0,0)),scaledShadeWidth);
				shPen.LineJoin = LineJoin.Round;
				shPen.DashStyle = style.DashStyle;
				g.DrawLines(shPen,ptsM);
				shPen.Dispose();
				for(int i=0;i<nPts;i++)
				{
					ptsM[i].X -= scaleShift;
					ptsM[i].Y += scaleShift;
				}
			}

			if(drawLine)
			{
				Pen pen = new Pen(new SolidBrush(style.Color),scaledWidth);
				pen.DashStyle = style.DashStyle;
				pen.LineJoin = LineJoin.Round;
				g.DrawLines(pen,ptsM);
				pen.Dispose();
			}
		}

		internal virtual void RenderLine(Pen pen, Vector3D point1, Vector3D point2,	bool releasePenAfterRendering)
		{
			PointF bmpPoint1 = WorldToBmp(point1);
			PointF bmpPoint2 = WorldToBmp(point2);

			if(computingPosition)
			{
				x0p = Math.Min(x0p,bmpPoint1.X);
				y0p = Math.Min(y0p,bmpPoint1.Y);
				x1p = Math.Max(x1p,bmpPoint1.X);
				y1p = Math.Max(y1p,bmpPoint1.Y);
				x0p = Math.Min(x0p,bmpPoint1.X);
				y0p = Math.Min(y0p,bmpPoint1.Y);
				x1p = Math.Max(x1p,bmpPoint1.X);
				y1p = Math.Max(y1p,bmpPoint1.Y);
				if(releasePenAfterRendering)
					pen.Dispose();
				return;
			}

			if(g==null)
				Initialize();
			// Modify the pen width
			float width = pen.Width;
			pen.Width = (float)Math.Round(width * Mapping.FromPointToTarget);
			if(Math.Abs(bmpPoint1.X-bmpPoint2.X)<0.5)
			{
				bmpPoint1.X = (float)Math.Round(bmpPoint1.X);
				bmpPoint2.X = bmpPoint1.X;
			}
			else if(Math.Abs(bmpPoint1.Y-bmpPoint2.Y)<0.5)
			{
				bmpPoint1.Y = (float)Math.Round(bmpPoint1.Y);
				bmpPoint2.Y = bmpPoint1.Y;
			}

			G.DrawLine(pen, bmpPoint1,bmpPoint2); 
			if(releasePenAfterRendering)
				pen.Dispose();
			else
				pen.Width = width;
		}

        internal virtual void RenderFillPath(GradientStyle gradientStyle, PointF[] stylePoints, GraphicsPath path, bool releasePathAfterRendering)
        {
            Brush brush = CreateBrush(gradientStyle, stylePoints);
            RenderFillPath(brush, path, true, releasePathAfterRendering);
        }

        internal virtual void RenderFillPath(Brush brush, GraphicsPath path, bool releaseBrushAfterRendering, bool releasePathAfterRendering)
        {
            GraphicsPath bmpPath = BoardToBmp(path);
            ProcessObjectTrackingArea(bmpPath);
            if (computingPosition)
            {
                AdjustComputingRectangle(bmpPath.GetBounds());
            }
            else
                G.FillPath(brush, bmpPath);
            if (releaseBrushAfterRendering)
                brush.Dispose();
            if (releasePathAfterRendering)
                path.Dispose();
            bmpPath.Dispose();
        }

		internal virtual void RenderPath(Pen pen, GraphicsPath path, bool releasePenAfterRendering, bool releasePathAfterRendering)
		{
			if(g==null)
				Initialize();
			float width = pen.Width;
			pen.Width = (float)Math.Round(width*Mapping.FromPointToTarget);
			GraphicsPath bmpPath = BoardToBmp(path);
			if(computingPosition)
			{
				AdjustComputingRectangle(bmpPath.GetBounds());
			}
			else
				G.DrawPath(pen,bmpPath);
			bmpPath.Dispose();
			pen.Width = width;
			if(releasePenAfterRendering)
				pen.Dispose();
			if(releasePathAfterRendering)
				path.Dispose();
		}

		private void AdjustComputingRectangle(RectangleF rect)
		{
			PointF[] points = new PointF[]
				{
					new PointF(rect.X,rect.Y),
					new PointF(rect.X+rect.Width,rect.Y),
					new PointF(rect.X+rect.Width,rect.Y+rect.Height),
					new PointF(rect.X,rect.Y+rect.Height)
				};
			if(G != null && G.Transform != null)
				G.Transform.TransformPoints(points);
			for(int i=0; i<points.Length; i++)
			{
				x0p = Math.Min(x0p,points[i].X);
				y0p = Math.Min(y0p,points[i].Y);
				x1p = Math.Max(x1p,points[i].X);
				y1p = Math.Max(y1p,points[i].Y);
			}
		}
		#endregion

        #region --- Tools Handling ---

		private Brush CreateBrush(GradientStyle gradientStyle, PointF[] points)
		{
			float xMin = points[0].X;
			float yMin = points[0].Y;
			float xMax = xMin;
			float yMax = yMin;
			for(int i=1;i<points.Length;i++)
			{
				xMin = Math.Min(xMin,points[i].X);
				yMin = Math.Min(yMin,points[i].Y);
				xMax = Math.Max(xMax,points[i].X);
				yMax = Math.Max(yMax,points[i].Y);
			}
			if(xMin == xMax) xMax = xMin+1;
			if(yMin == yMax) yMax = yMin+1;

			return CreateBrush(gradientStyle,new RectangleF(xMin,yMin,xMax-xMin+1,yMax-yMin+1));
		}

		private Brush CreateBrush(GradientStyle style, RectangleF rect)
		{
			float xMin = rect.X;
			float yMin = rect.Y;
			float xMax = rect.X + rect.Width;
			float yMax = rect.Y + rect.Height;
			switch(style.GradientKind)
			{
				case GradientKind.DiagonalLeft:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMax),style.StartColor,style.EndColor);
				case GradientKind.DiagonalRight:
					return CreateLinearGradientBrush(new PointF(xMax,yMin),new PointF(xMin,yMax),style.StartColor,style.EndColor);
				case GradientKind.Horizontal:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMin),style.StartColor,style.EndColor);
				case GradientKind.Vertical:
					return CreateLinearGradientBrush(new PointF(xMin,yMax),new PointF(xMin,yMin),style.StartColor,style.EndColor);
				case GradientKind.DiagonalLeftCenter:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMax),style.StartColor,style.EndColor,true);
				case GradientKind.DiagonalRightCenter:
					return CreateLinearGradientBrush(new PointF(xMax,yMin),new PointF(xMin,yMax),style.StartColor,style.EndColor,true);
				case GradientKind.HorizontalCenter:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMin),style.StartColor,style.EndColor,true);
				case GradientKind.VerticalCenter:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMin,yMax),style.StartColor,style.EndColor,true);
				case GradientKind.Center:
					return CreateRadialGradientBrush(rect,style.StartColor,style.EndColor);
			}
			return new SolidBrush(style.StartColor);
		}

		private Brush CreateLinearGradientBrush(PointF boardPoint1, PointF boardPoint2, Color color1, Color color2)
		{
			return CreateLinearGradientBrush(boardPoint1, boardPoint2, color1, color2, false);
		}

		private Brush CreateLinearGradientBrush(PointF boardPoint1, PointF boardPoint2, Color color1, Color color2, bool central)
		{
			if(g == null)
				Initialize();
			// Find the center point
			PointF center = new PointF((boardPoint1.X+boardPoint2.X)*0.5f,(boardPoint1.Y+boardPoint2.Y)*0.5f);
			// Line normal in the board cs
			float dxN = -(boardPoint2.Y-boardPoint1.Y);
			float dyN =  (boardPoint2.X-boardPoint1.X);
			// Point on the normal
			PointF onNorm = new PointF(center.X+dxN, center.Y+dyN);
			// Now we move to the Bitmap C.S.
			PointF centerBMP = BoardToBmp(center);
			PointF onNormBMP = BoardToBmp(onNorm);
			// Unit vector of the gradient in the Bitmap C.S.
			float xvG =  (onNormBMP.Y - centerBMP.Y);
			float yvG = -(onNormBMP.X - centerBMP.X);
			float abs = (float)Math.Sqrt(xvG*xvG + yvG*yvG);
			xvG = xvG/abs;
			yvG = yvG/abs;
			// Gradient endpoints in the bitmap C.S.
			PointF bmpPoint1 = BoardToBmp(boardPoint1);
			float sp = (bmpPoint1.X-centerBMP.X)*xvG + (bmpPoint1.Y-centerBMP.Y)*yvG;
			bmpPoint1.X = centerBMP.X + 2*sp*xvG;
			bmpPoint1.Y = centerBMP.Y + 2*sp*yvG;
			PointF bmpPoint2 = new PointF(centerBMP.X - 2*sp*xvG,centerBMP.Y - 2*sp*yvG);

			int npts;
			float[] pos;
			float[] fac;
			if(central)
			{
				pos = new float[5] { 0.0f, 0.25000f, 0.5f, 0.75000f, 1.0f };
				fac = new float[5] { 0.0f, 0.00001f, 1.0f, 0.00001f, 1.0f };
				npts = 5;
			}
			else
			{
				pos = new float[4] { 0.0f, 0.25000f, 0.75000f, 1.0f };
				fac = new float[4] { 0.0f, 0.00001f, 0.99999f, 1.0f };
				npts = 4;
			}
			LinearGradientBrush brush = new LinearGradientBrush(bmpPoint1,bmpPoint2,color1,color2);
			Blend blend = new Blend(npts);
			blend.Factors = fac;
			blend.Positions = pos;
			brush.Blend = blend;

			return brush;
		}

		private Brush CreateRadialGradientBrush(RectangleF boardRect, Color color1, Color color2)
		{
			if(g == null)
				Initialize();

			int npts = 4;
			float[] pos = new float[4] { 0.0f, 0.25000f, 0.75000f, 1.0f };
			float[] fac = new float[4] { 0.0f, 0.00001f, 0.99999f, 1.0f };

			GraphicsPath path = new GraphicsPath();

			// Create points here
			PointF[] points = new PointF[20];
			Color[] sColors = new Color[20];
			double da = Math.PI/10;
			double f = Math.Sqrt(2.05);
			for(int i=0;i<20;i++)
			{
				points[i] = BoardToBmp(boardRect.Left + boardRect.Width/2.0 + f * boardRect.Width * Math.Cos(i*da)/2,
					boardRect.Top + boardRect.Height/2.0 + f * boardRect.Height * Math.Sin(i*da)/2);
				sColors[i] = color1;
			}
			path.AddClosedCurve(points);
			PathGradientBrush brush = new PathGradientBrush(path);
			Blend blend = new Blend(npts);
			blend.Factors = fac;
			blend.Positions = pos;
			brush.Blend = blend;
			brush.CenterPoint = BoardToBmp(boardRect.Left + boardRect.Width/2f, boardRect.Top + boardRect.Height/2f);
			brush.CenterColor = color2;
			brush.SurroundColors = sColors;

			return brush;
		}

		private PathGradientBrush CreatePathGradientBrush(GraphicsPath path)
		{
			return new PathGradientBrush(BoardToBmp(path));
		}
		#endregion

		internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			TargetCoordinateRange tcr = new TargetCoordinateRange();
			Mapping map = Mapping;
			tcr.Include(map.Map(V0));
			tcr.Include(map.Map(V0+vx));
			tcr.Include(map.Map(V0+vy));
			tcr.Include(map.Map(V0+vx+vy));

			return tcr;
		}
	}
}
