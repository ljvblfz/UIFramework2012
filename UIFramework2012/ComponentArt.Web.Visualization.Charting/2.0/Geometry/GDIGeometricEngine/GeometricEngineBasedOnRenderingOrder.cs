using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal enum BorderDrawingMode
	{
		NoBorder,
		SameColorBorder,
		DarkerColorBorder
	}

	internal enum LightDirectionKind
	{
		FrontLightDirection,
		BackLightDirection
	}

    /// <summary>
    /// Base class for geometric engines
    /// </summary>
    internal class GeometricEngineBasedOnRenderingOrder : GeometricEngine
    {
		protected double renderingPrecision;
		protected Bitmap backgroundImage;
		private ArrayList flatObjects = new ArrayList();

		protected object activeObject;
		protected Graphics g;
		protected int darkerColorPercentage = 80;

		protected Vector3D frontLightDirection = new Vector3D(0.1,1,2).Unit();
		protected Vector3D lightDirection;
		protected LightDirectionKind lightDirectionKind = LightDirectionKind.FrontLightDirection;
		protected double ambientFraction = 0.5;
		protected CoordinateSystem coordinateSystem;

		private ObjectMapper objectMapper;

		private ObjectTrackingData objectTrackingData;

		// bubble resources

		private Bitmap	bubbleBitmap = null;

		internal Bitmap Bitmap { get { return backgroundImage; } }

		internal ObjectTrackingData ObjectTrackingData { get { return objectTrackingData; } }

        #region --- Construction and Settings ---

        internal GeometricEngineBasedOnRenderingOrder() : this(null) 
		{
			objectTrackingData = new ObjectTrackingData(this);
		}
		internal GeometricEngineBasedOnRenderingOrder(ChartBase chart) : base(chart) 
		{
			objectTrackingData = new ObjectTrackingData(this);
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(bubbleBitmap != null)
					bubbleBitmap.Dispose();
				bubbleBitmap = null;
			}
			base.Dispose(disposing);
		}


        internal override void SetRenderingPrecisionPxl(double renderingPrecision)
        {
			this.renderingPrecision = renderingPrecision;
        }

		internal LightDirectionKind LightDirectionKind { get { return lightDirectionKind; } set { lightDirectionKind = value; } }
		
		internal LightDirectionKind GetSetLightDirectionKind (LightDirectionKind newLightDirectionKind)
		{
			LightDirectionKind oldLightDirectionKind = lightDirectionKind;
			lightDirectionKind = newLightDirectionKind;
			return oldLightDirectionKind;
		}

        internal override void SetBackground(Bitmap backgroundImage)
        {
			this.backgroundImage = backgroundImage;
			g = Graphics.FromImage(backgroundImage);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			g.Transform = new Matrix(1,0,0,-1,0,backgroundImage.Height);
        }

		protected Bitmap BubbleBitmap
		{
			get
			{
				if(bubbleBitmap == null)
				{
					bubbleBitmap =
                        (Bitmap)Bitmap.FromStream(CommonFunctions.GetManifestResourceStream("bubble.PNG"));
                    if (bubbleBitmap == null)
						throw new Exception("Implementation: bubble bitmap not found in the assembly resources");
				}
				return bubbleBitmap;
			}
		}

		internal override void SetGraphics(Graphics g)
		{
			
		}

		#endregion

		internal override ObjectMapper GetObjectMapper()
		{
			if(objectMapper == null)
				objectMapper = new ObjectMapper(this);

			return objectMapper;
		}

		internal override void GetObjectTrackingInfo(out int[,] matrix, out ArrayList objects)
		{
			objectTrackingData.GetObjectTrackingInfo(out matrix, out objects);
		}

		#region --- Computing position rextangle ---

		internal override void SetPositionComputingMode(bool computingPosition)
		{
			x0p = double.MaxValue;
			y0p = double.MaxValue;
			x1p = double.MinValue;
			y1p = double.MinValue;
			base.computingPosition = computingPosition;
		}

		internal override Rectangle GetPositionRectangle()
		{
			double y0 = mapping.TargetSize.Height - y1p;
			double y1 = mapping.TargetSize.Height - y0p;
			return new Rectangle((int)x0p,(int)y0,(int)(x1p-x0p+1),(int)(y1-y0+1));
		}
		
		#endregion

		#region --- 2D Drawing in drawing board ---
        internal override DrawingBoard CreateDrawingBoard()
        {
            DrawingBoardOB db = new DrawingBoardOB();
			db.GE = this;
			db.SetGraphics(g);
            Add(db);
            return db;
        }

        internal override DrawingBoard CreateDrawingBoard(Vector3D V0, Vector3D Vx, Vector3D Vy)
        {
            DrawingBoardOB db = new DrawingBoardOB(V0,Vx,Vy);
			db.GE = this;
			db.SetGraphics(g);
			Add(db);
            return db;
        }

		#endregion
		
		#region --- 2D Drawing in GDI+ Like Graphics "Canvace" ---
		
		internal override Canvace CreateCanvace()
		{
			return new GDICanvace(Chart.WorkingGraphics,Chart.TargetSize.Height);
		}

        #endregion

		internal override object Render()
		{
			flatObjects.Clear();

			objectTrackingData.StartTracking(this.backgroundImage.Width,this.backgroundImage.Height);
			base.Render();

			if(flatObjects.Count > 0)
			{
				Graphics g = Graphics.FromImage(backgroundImage);
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				RenderFlatObjects(g);
				g.Dispose();
			}

			return backgroundImage;
		}

        # region --- Rendering 3D Objects ---

        internal override void RenderQuadrilateral (Quadrilateral q)
        {
        }

        internal override void RenderEllipse(Ellipse e)
        {
        }

        internal override void RenderBox(Box box)
        {
			objectTrackingData.SetActiveObject(box.Tag);
			Vector3D P0 = box.P0;
			Vector3D P1 = box.P1;
			ChartColor surface = box.Color;

			double x0 = Math.Min(P0.X, P1.X);
			double x1 = Math.Max(P0.X, P1.X);
			double y0 = Math.Min(P0.Y, P1.Y);
			double y1 = Math.Max(P0.Y, P1.Y);
			double z0 = Math.Min(P0.Z, P1.Z);
			double z1 = Math.Max(P0.Z, P1.Z);

			Vector3D P000 = new Vector3D(x0, y0, z0);
			Vector3D P001 = new Vector3D(x0, y0, z1);
			Vector3D P010 = new Vector3D(x0, y1, z0);
			Vector3D P011 = new Vector3D(x0, y1, z1);
			Vector3D P100 = new Vector3D(x1, y0, z0);
			Vector3D P101 = new Vector3D(x1, y0, z1);
			Vector3D P110 = new Vector3D(x1, y1, z0);
			Vector3D P111 = new Vector3D(x1, y1, z1);

			// face
			RenderParallelogram(box,surface, P001, P101, P011, false);
			// right side
			RenderParallelogram(box,surface, P101, P100, P111, false);
			// top side
			RenderParallelogram(box,surface, P011, P111, P010, false);
			// back
			RenderParallelogram(box,surface, P100, P000, P110, false);
			// left side
			RenderParallelogram(box,surface, P000, P001, P010, false);
			// bottom side
			RenderParallelogram(box,surface, P000, P100, P001, false);
		}

        internal override void RenderPrism(Prism obj)
		{
			objectTrackingData.SetActiveObject(obj.Tag);
			Push(obj);

			int np = obj.NumberOfSides+1;
			Vector3D[] pts = new Vector3D[np];
			Vector3D[] side = new Vector3D[5];
			// Base
			for(int i=0; i<np; i++)
			{
				pts[i] = obj.BasePoint(i%(np-1));
			}
			RenderPolygon(obj,obj.ChartColor.Color,pts,false,BorderDrawingMode.SameColorBorder);
			// Sides
			Vector3D H = obj.TopCenter-obj.BaseCenter;
			for(int i=0; i<np-1; i++)
			{
				side[0] = pts[i];
				side[1] = pts[i] + H;
				side[2] = pts[i+1] + H;
				side[3] = pts[i+1];
				side[4] = pts[i];
				RenderPolygon(obj,obj.ChartColor.Color,side,false,BorderDrawingMode.SameColorBorder);
			}
			// Top
			for(int i=0; i<np/2; i++)
			{
				Vector3D P = pts[i];
				pts[i] = pts[np-1-i];
				pts[np-1-i] = P;
			}
			for(int i=0; i<np; i++)
				pts[i] = pts[i] + H;
			RenderPolygon(obj,obj.ChartColor.Color,pts,false,BorderDrawingMode.SameColorBorder);

			Pop(obj.GetType());
		}

		internal override void RenderCylinder(Cylinder cylinder)
		{
			objectTrackingData.SetActiveObject(cylinder.Tag);
			Vector3D C2 = cylinder.TopCenter;
			Vector3D C1 = cylinder.BaseCenter;
			Vector3D R1 = cylinder.BaseRadius;
			double r2 = cylinder.OrthogonalRadius;
			ChartColor surface = cylinder.ChartColor;

			Vector3D H = C2-C1;
			Vector3D R2 = H.CrossProduct(R1);
			if (!R2.IsNull)
				R2 = R2.Unit() * r2;

			if (R2.IsNull)
				return;

			double h = H.Abs;
			double rEdge = cylinder.EdgeRadius * mapping.FromPointToWorld;
			if (h > 0)
				rEdge = Math.Min(rEdge, h / 3);
			else
				return;

			Push(cylinder);

			// Number of approximation points
			double xAxisW = (mapping.Map(C1 + R1) - mapping.Map(C1)).Abs;
			double yAxisW = (mapping.Map(C1 + R2) - mapping.Map(C1)).Abs;
			int np = cylinder.NumberOfApproximationPointsForEllipse(xAxisW,yAxisW,RenderingPrecisionInPixelSize);

			// Ellipses
			
			double fde = 1.0 - rEdge/R1.Abs;
			double fh = rEdge/(C2-C1).Abs;
			Vector3D C1R = C1 + H*fh;
			Vector3D C2R = C2 - H*fh;
			Vector3D[] topx = new Vector3D[np+1];
			Vector3D[] top = new Vector3D[np+1];
			Vector3D[] bot = new Vector3D[np+1];
			Vector3D[] top1 = new Vector3D[np+1];
			Vector3D[] bot1 = new Vector3D[np+1];
			for(int i = 0; i<=np;i++)
			{	double angle = (i%np)*2*Math.PI/np;
				double c = Math.Cos(angle);
				double s = Math.Sin(angle);
				topx[np-i] = C2 + R1*(c*fde) + R2*(s*fde);
				top[i] = C2 + R1*(c*fde) + R2*(s*fde);
				top1[i] = C2R + R1*c + R2*s;
				bot1[i] = C1R + R1*c + R2*s;
				bot[i] = C1 + R1*(c*fde) + R2*(s*fde);
			}
			RenderPolygon(cylinder,cylinder.ChartColor.Color,topx,false,BorderDrawingMode.SameColorBorder);
			RenderPolygon(cylinder,cylinder.ChartColor.Color,bot,false,BorderDrawingMode.SameColorBorder);
			RenderStrip(top1,top,cylinder.ChartColor,false,BorderDrawingMode.SameColorBorder,true,true);
			RenderStrip(bot,bot1,cylinder.ChartColor,false,BorderDrawingMode.SameColorBorder,true,true);
			RenderStrip(bot1,top1,cylinder.ChartColor,false,BorderDrawingMode.SameColorBorder,true,true);
		
			Pop(cylinder.GetType());
		}

		internal override void RenderBlock(Block obj)
		{
			RenderPrism(obj);
		}

		internal override void RenderPyramid(Pyramid pyramid)
		{
			objectTrackingData.SetActiveObject(pyramid.Tag);
			Push(pyramid);

			int np = pyramid.NumberOfSides+1;
			Vector3D[] ptsBase = new Vector3D[np];
			Vector3D[] ptsTop = new Vector3D[np];
			Vector3D[] side = new Vector3D[5];
			// Base
			for(int i=0; i<np; i++)
			{
				ptsBase[i] = pyramid.BasePoint(i%(np-1));
				ptsTop[i] = pyramid.TopPoint(i%(np-1));
			}
			RenderPolygon(pyramid,pyramid.ChartColor.Color,ptsBase,false,BorderDrawingMode.SameColorBorder);
			// Sides
			for(int i=0; i<np-1; i++)
			{
				side[0] = ptsBase[i];
				side[1] = ptsTop[i];
				side[2] = ptsTop[i+1];
				side[3] = ptsBase[i+1];
				side[4] = ptsBase[i];
				RenderPolygon(pyramid,pyramid.ChartColor.Color,side,false,BorderDrawingMode.SameColorBorder);
			}
			// Top
			for(int i=0; i<np/2; i++)
			{
				Vector3D P = ptsTop[i];
				ptsTop[i] = ptsTop[np-1-i];
				ptsTop[np-1-i] = P;
			}
			RenderPolygon(pyramid,pyramid.ChartColor.Color,ptsTop,false,BorderDrawingMode.SameColorBorder);

			Pop(pyramid.GetType());
		}

		internal override void RenderCone(Cone cone)
		{
			objectTrackingData.SetActiveObject(cone.Tag);
			Push(cone);
			Mapping map = cone.Mapping;
			Vector3D C = cone.BaseCenter;
			Vector3D Rx = cone.BaseRadius;
			Vector3D V = cone.Vertex;
			Vector3D Ry = ((V-C).CrossProduct(Rx)).Unit()*cone.OrthogonalRadius;
			double a1 = (map.Map(C+Rx)-map.Map(C)).Abs;
			double a2 = (map.Map(C+Ry)-map.Map(C)).Abs;
			int np = cone.NumberOfApproximationPointsForEllipse(a1,a2,RenderingPrecisionInPixelSize);

			Vector3D[] topx = new Vector3D[np];
			Vector3D[] top = new Vector3D[np];
			Vector3D[] bot = new Vector3D[np];

			Vector3D C1 = cone.RelativeH1*V + (1.0-cone.RelativeH1)*C;
			Vector3D C2 = cone.RelativeH2*V + (1.0-cone.RelativeH2)*C;
			Vector3D Rx1 = Rx*(1.0-cone.RelativeH1);
			Vector3D Ry1 = Ry*(1.0-cone.RelativeH1);
			Vector3D Rx2 = Rx*(1.0-cone.RelativeH2);
			Vector3D Ry2 = Ry*(1.0-cone.RelativeH2);

			for(int i=0; i<np; i++)
			{
				double angle = 2.0*i*Math.PI/np;
				double c = Math.Cos(angle);
				double s = Math.Sin(angle);
				top[i] = C2 + Rx2*c + Ry2*s;
				bot[i] = C1 + Rx1*c + Ry1*s;
				topx[np-i-1] = top[i];
			}

			RenderPolygon(cone,cone.ChartColor,topx,false,BorderDrawingMode.NoBorder);
			RenderStrip(bot,top,cone.ChartColor,false,BorderDrawingMode.NoBorder,true,true);
			RenderPolygon(cone,cone.ChartColor,bot,false,BorderDrawingMode.NoBorder);

			Pop(cone.GetType());
		}

		internal override void RenderEllipsoid(Ellipsoid elp)
		{
			objectTrackingData.SetActiveObject(elp.Tag);

			Vector3D CP = elp.Mapping.Map(elp.Center);
			int nu = elp.GetNu(this);
			int nv = elp.GetNv(this);
			
			double r = 0;
			for(int i=0; i<nu; i++)
			{
				double u = elp.U0 + i*elp.DU;
				for(int j = 0; j<nv; j++)
				{
					double v = elp.V0 + j*elp.DV;
					Vector3D PP = elp.Mapping.Map(elp.Point(u,v));
					r = Math.Max(r,(PP-CP).Abs);
				}
			}

			RenderBubble(CP.X,CP.Y,r,elp.ChartColor.Color);
		}
				
		private void RenderBubble(double x, double y, double r, Color color)
		{			
			if(computingPosition)
			{
				x0p = Math.Min(x0p,x);
				y0p = Math.Min(y0p,y);
				x1p = Math.Max(x1p,x);
				y1p = Math.Max(y1p,y);
				return;
			}
			Bitmap bmp = BubbleBitmap;
			int dx = bmp.Width, dy = bmp.Height;
			
			float R = (float)color.R/255f;
			float G = (float)color.G/255f;
			float B = (float)color.B/255f;
			ImageAttributes imageAttrs = new ImageAttributes();
			ColorMatrix colorMatrix = new ColorMatrix(
				new float[][] {
								  new float[] { R, G, B, 0, 0 },
								  new float[] { (1-R)/2, (1-G)/2, (1-B)/2, 0, 0 },
								  new float[] { (1-R)/2, (1-G)/2, (1-B)/2, 0, 0 },
								  new float[] { 0, 0, 0, 1, 0 },
								  new float[] { 0, 0, 0, 0, 1 }
							  });
			imageAttrs.SetColorMatrix(colorMatrix);

			GraphicsPath path = new GraphicsPath();
			Rectangle rect = new Rectangle((int)(x-r+1),(int)(y-r+1),(int)(2*r-2),(int)(2*r-2));
			path.AddEllipse(rect);
			GraphicsContainer cont = g.BeginContainer();
			g.SetClip(path);
			g.DrawImage(bmp,rect,0,0,dx,dy,GraphicsUnit.Pixel,imageAttrs);
			g.EndContainer(cont);
			Color c = Color.FromArgb(color.A,color.R/2,color.G/2, color.B/2);
			Pen pen = new Pen(c,1);
			g.DrawPath(pen,path);
			pen.Dispose();
		}
				
		internal override void MarkerRenderBubble(Marker marker, MarkerStyle ms, ChartColor color)
		{
			Mapping map = marker.Mapping;
			Vector3D P = map.Map(marker.Location);
			RenderBubble(P.X,P.Y,ms.MarkerSize.X * marker.FromPointToWorld*map.Enlargement,color.Color);
		}

		internal override void RenderRadialStrip(RadialStrip rStrip)
		{
			Mapping map = rStrip.Mapping;
			Vector3D[] V00S = rStrip.InnerRing;
			Vector3D[] V01S = rStrip.OuterRing;
			Vector3D[] N0S = rStrip.Normal;
			ChartColor eSurface = rStrip.ChartColor;
			int nSeg = rStrip.Count;
			Vector3D C = map.Map(rStrip.Center);
			double d0 = (map.Map(rStrip.InnerRing[0])-C).Abs;
			double d1 = (map.Map(rStrip.OuterRing[0])-C).Abs;
			double xc = C.X;
			double yc = C.Y;
			Color color = eSurface.Color;

			GraphicsPath path = new GraphicsPath();
			
			path.AddEllipse((float)(xc-d1),(float)(yc-d1),2*(float)d1,2*(float)d1);
			path.AddEllipse((float)(xc-d0),(float)(yc-d0),2*(float)d0,2*(float)d0);
			
			int nPts = nSeg;
			PointF[] points = new PointF[nPts];
			Color[] colors = new Color[nPts];
			double a0 = Math.PI/4;
			double da = Math.PI*2/nPts;
			double a = a0;
			Vector3D N = N0S[7*nPts/8].Unit();
			for(int i=0; i<nPts;i++)
			{
				Vector3D P = map.Map(V01S[i]);
				// we have to extend the radius for a bit
				double dx = P.X - C.X;
				double dy = P.Y - C.Y;
				float xf = (float)(C.X + dx*2);
				float yf = (float)(C.Y + dy*2);
				points[i] = new PointF(xf,yf);
				double sp = N0S[i].Unit()*N;
				double ambient = 0.60;
				sp = ambient + (1-ambient)*sp;
				sp = Math.Max(0,Math.Min(sp,1.0));
				colors[i] = Color.FromArgb((int)(color.R*sp),(int)(color.G*sp),(int)(color.B*sp));
				a += da;
			}

			PathGradientBrush brush = new PathGradientBrush(points);
			brush.CenterColor = color;
			brush.SurroundColors = colors;
			brush.WrapMode = WrapMode.Tile;
			
			g.FillPath(brush,path);
			objectTrackingData.ProcessRegion(path);
			brush.Dispose();
			path.Dispose();
		}

        internal override void RenderParaboloid(Paraboloid para)
        {
			Cone cone = new Cone(para.BaseCenter,para.Vertex,para.BaseRadius,para.OrthogonalRadius,para.RelativeH1,
				para.RelativeH2,para.ChartColor);
			cone.Parent = para.Parent;
			cone.Tag = para.Tag;
			RenderCone(cone);
        }

		internal override void RenderTorusSegment(TorusSegment obj)
        {
        }

		internal override void Render2DArea(Chart2DArea area)
		{
			DrawingBoard dBoard = area.DrawingBoard;
			foreach (SimpleLine L in area.Lines)
			{
				PointF[] points = L.GetPoints();
				for (int i = 0; i < points.Length; i++)
				{
					points[i].X = (float)(points[i].X - dBoard.V0.X);
					points[i].Y = (float)(points[i].Y - dBoard.V0.Y);
				}
				dBoard.DrawArea(area.LineStyle, area.Gradient, points);
			}
		}
	
		internal override void RenderArea(ChartArea area)
		{
			objectTrackingData.SetActiveObject(area.Tag);
			SimpleLineCollection lines = area.Lines;
			for(int i=0; i<lines.Count; i++)
			{
				RenderSimpleLine(lines[i],area);
			}
		}

		private void RenderSimpleLine(SimpleLine line, ChartArea owningArea)
		{
			Vector3D P0 = owningArea.Origin;
			Vector3D Vx = owningArea.XDirection;
			Vector3D Vy = owningArea.YDirection;
			Vector3D wallVector = (Vy.CrossProduct(Vx)).Unit() * owningArea.Height;

			ChartColor color = owningArea.BodyColor;

			PointF[] points2D = line.GetPoints();
			int np = points2D.Length;
			Vector3D[] points3D = new Vector3D[np];
			Vector3D[] points3Dw = new Vector3D[np];
			Vector3D[] points3Dn = new Vector3D[np];
			for(int j=0; j<np; j++)
			{
				points3D[j] = P0 + Vx*points2D[j].X + Vy*points2D[j].Y;
				points3Dw[j] = points3D[j] + wallVector;
				points3Dn[np-j-1] = points3D[j] + wallVector;
			}
			SimpleLineCollection SLC = new SimpleLineCollection();
			SLC.Add(line);
			Wall wall = new Wall(owningArea.Origin, owningArea.XDirection, owningArea.YDirection, SLC, owningArea.Height, owningArea.WallColor);
			wall.Parent = owningArea;
			LightDirectionKind oldLDK = GetSetLightDirectionKind(LightDirectionKind.BackLightDirection);
			RenderWall(wall);
			GetSetLightDirectionKind(oldLDK);
			RenderPolygon(owningArea,color,points3D,false,BorderDrawingMode.NoBorder,false);
			RenderPolygon(owningArea,color,points3Dn,false,BorderDrawingMode.NoBorder,false);
		}
	
		
		# endregion


		internal override void RenderTargetAreaBox(TargetAreaBox obj)
		{
			TargetArea ta = obj.Tag as TargetArea;
			if(ta != null)
			{
				ta.Mapping.BendingNeeded = true;
			}
			base.RenderTargetAreaBox(obj);
		}

		internal override void RenderCoordinateSystemBox(CoordinateSystemBox csBox)
		{

			coordinateSystem = csBox.Tag as CoordinateSystem;

			bool rzo = renderInZOrder;
			renderInZOrder = true;

			// Coordinate planes in background
			if(csBox.CoordinatePlanes.SubObjects != null)
			{
				for(int i=0; i<csBox.CoordinatePlanes.SubObjects.Count;i++)
				{
					CoordinatePlaneBox cpb = csBox.CoordinatePlanes.SubObjects[i] as CoordinatePlaneBox;
					if(cpb.FrontSideVisible())
						RenderObject(cpb);
				}
			}

			// Interior
			RenderGeneric(csBox.Interior);

			// Coordinate planes in foreground
			if(csBox.CoordinatePlanes.SubObjects != null)
			{
				for(int i=0; i<csBox.CoordinatePlanes.SubObjects.Count;i++)
				{
					CoordinatePlaneBox cpb = csBox.CoordinatePlanes.SubObjects[i] as CoordinatePlaneBox;
					if(!cpb.FrontSideVisible())
						RenderObject(cpb);
				}
			}

			renderInZOrder = rzo;
		}

		#region --- Rendering pie/doughnut chart ---

		#region --- Old implementation
		internal void CollectDataPoints(SectionBox sb, ArrayList list)
		{
			// This collects data points in a hierarchy with SectionBox-es at internal nodes
			// and DataPoint related objects at leaves
			if(sb.SubObjects == null)
				return;
			for(int i=0; i<sb.SubObjects.Count; i++)
			{
				GeometricObject obj = sb.SubObjects[i] as GeometricObject;
				if(obj.Tag is DataPoint)
					list.Add(obj);
				else if (obj is SectionBox)
					CollectDataPoints(obj as SectionBox,list);
			}
		}

		internal void PopulatePieSegmentElements(PieSegment pie, GeometricObject bases, GeometricObject sides, GeometricObject tops)
		{
			if(pie == null)
				return;

			// Base
			
			Vector3D[] innerLine = pie.GetInnerLine(RenderingPrecisionInPixelSize,false);
			Vector3D[] outerLine = pie.GetOuterLine(RenderingPrecisionInPixelSize,false);
			Vector3D[] innerLine1 = pie.GetInnerLine(RenderingPrecisionInPixelSize,true);
			Vector3D[] outerLine1 = pie.GetOuterLine(RenderingPrecisionInPixelSize,true);
			int n = innerLine.Length;
			Vector3D[] line = new Vector3D[n*2+1];

			for(int i=0; i<n; i++)
				line[i] = innerLine[n-i-1];
			for(int i=0; i<n; i++)
				line[n+i] = outerLine[i];
			line[2*n] = line[0];

			bases.Add(new Polygon(line,pie.ChartColor,true,false));

			// Sides
			
			//    Render inner wall
			for(int i=0; i<n/2; i++)
			{
				Vector3D A = innerLine[i];
				innerLine[i] = innerLine[innerLine.Length-i-1];
				innerLine[innerLine.Length-i-1] = A;
				A = innerLine1[i];
				innerLine1[i] = innerLine1[innerLine.Length-i-1];
				innerLine1[innerLine.Length-i-1] = A;
			}
			sides.Add(new Polygon(new Vector3D[] 
				{ 
					innerLine[0],
					outerLine[n-1], 
					outerLine[n-1]+pie.TopCenter - pie.BaseCenter,
					innerLine[0]+pie.TopCenter - pie.BaseCenter,
					innerLine[0] 
				},pie.ChartColor,false,true));
			sides.Add(new Polygon(new Vector3D[] 
				{ 
					innerLine[n-1],									// 5
					innerLine[n-1]+pie.TopCenter - pie.BaseCenter,	// 4
					outerLine[0]+pie.TopCenter - pie.BaseCenter,	// 3
					outerLine[0],									// 2
					innerLine[n-1]									// 1
				},pie.ChartColor,false,true));


			Band band = new Band(innerLine,innerLine1,pie.ChartColor,false,true);
			band.Parent = pie;
			ArrayList inVisible = band.GetVisibleSections();
			foreach(Band b in inVisible)
				sides.Add(b);

			band = new Band(outerLine,outerLine1,pie.ChartColor,false,true);
			band.Parent = pie;
			ArrayList outVisible = band.GetVisibleSections();
			foreach(Band b in outVisible)
				sides.Add(b);

			// Top

			line = new Vector3D[n*2+1];
			for(int i=0; i<n; i++)
				line[i] = outerLine1[n-i-1];
			for(int i=0; i<n; i++)
				line[n+i] = innerLine1[n-i-1];
			line[2*n] = line[0];
			tops.Add(new Polygon(line,pie.ChartColor,true,false));
		}

		#endregion

		private void CollectLinearListOfSections(SectionBox sBox,ArrayList list)
		{
			if(sBox.SubObjects == null)
				return;
			if(sBox.SubObjects[0] is SectionBox) // inner node
			{
				for(int i=0; i<sBox.SubObjects.Count; i++)
					CollectLinearListOfSections(sBox.SubObjects[i] as SectionBox, list);
			}
			else // leaf
				list.Add(sBox);
		}

		private void RenderPieSegmentBaseAndTop(PieSegment pie, bool top)
		{
			if(pie == null)
				return;

			// Base
			
			Vector3D[] innerLine = pie.GetInnerLine(RenderingPrecisionInPixelSize,top);
			Vector3D[] outerLine = pie.GetOuterLine(RenderingPrecisionInPixelSize,top);
			int n = innerLine.Length;
			Vector3D[] line = new Vector3D[n*2+1];

			if(top)
			{
				for(int i=0; i<n; i++)
					line[i] = outerLine[n-i-1];
				for(int i=0; i<n; i++)
					line[n+i] = innerLine[i];
			}
			else
			{
				for(int i=0; i<n; i++)
					line[i] = innerLine[n-i-1];
				for(int i=0; i<n; i++)
					line[n+i] = outerLine[i];
			}
			line[2*n] = line[0];

			RenderPolygon(pie,pie.ChartColor,line,false,BorderDrawingMode.DarkerColorBorder,false);
		}

		
		private void PieSegmentAddInnerWall(PieSegment pie, GeometricObject sides, ArrayList bands)
		{					
			Vector3D[] innerLine = pie.GetInnerLine(RenderingPrecisionInPixelSize,false);
			Vector3D[] outerLine = pie.GetOuterLine(RenderingPrecisionInPixelSize,false);
			Vector3D[] innerLine1 = pie.GetInnerLine(RenderingPrecisionInPixelSize,true);
			Vector3D[] outerLine1 = pie.GetOuterLine(RenderingPrecisionInPixelSize,true);
			int n = innerLine.Length;

			Mapping map = pie.Mapping;
			
			//    Render inner wall
			for(int i=0; i<n/2; i++)
			{
				Vector3D A = innerLine[i];
				innerLine[i] = innerLine[innerLine.Length-i-1];
				innerLine[innerLine.Length-i-1] = A;
				A = innerLine1[i];
				innerLine1[i] = innerLine1[innerLine.Length-i-1];
				innerLine1[innerLine.Length-i-1] = A;
			}
			
			// This part connects inner wall to the outer wall
			Vector3D H = pie.TopCenter - pie.BaseCenter;
			Vector3D TB0 = outerLine[n-1];
			Vector3D TB1 = innerLine[0];
			if(map.Map(TB0).X < map.Map(TB1).X)
			{
				Polygon pol = new Polygon(new Vector3D[] 
				{ 
					TB1,
					TB0, 
					TB0+H,
					TB1+H,
					TB1 
				},pie.ChartColor,false,true);
				sides.Add(pol);
			}
			// This part connects outer wall to inner wall
			Vector3D TE0 = innerLine[n-1];
			Vector3D TE1 = outerLine[0];
			if(map.Map(TE0).X < map.Map(TE1).X)
			{
				Polygon pol = new Polygon(new Vector3D[] 
				{ 
					TE1,	
					TE0,	
					TE0+H,	
					TE1+H,	
					TE1		
				},pie.ChartColor,false,true);
				sides.Add(pol);
			}
			Band band = new Band(innerLine,innerLine1,pie.ChartColor,false,true);
			band.Parent = pie ;
			ArrayList inVisible = band.GetVisibleSections();
			foreach(Band b in inVisible)
			{
				b.Parent = pie;
				bands.Add(b);
			}
		}
		
		private void PieSegmentAddOuterWall(PieSegment pie, GeometricObject sides, ArrayList bands)
		{
			Vector3D[] innerLine = pie.GetInnerLine(RenderingPrecisionInPixelSize,false);
			Vector3D[] outerLine = pie.GetOuterLine(RenderingPrecisionInPixelSize,false);
			Vector3D[] innerLine1 = pie.GetInnerLine(RenderingPrecisionInPixelSize,true);
			Vector3D[] outerLine1 = pie.GetOuterLine(RenderingPrecisionInPixelSize,true);
			int n = innerLine.Length;
			
			//    Render outer wall
			for(int i=0; i<n/2; i++)
			{
				Vector3D A = innerLine[i];
				innerLine[i] = innerLine[innerLine.Length-i-1];
				innerLine[innerLine.Length-i-1] = A;
				A = innerLine1[i];
				innerLine1[i] = innerLine1[innerLine.Length-i-1];
				innerLine1[innerLine.Length-i-1] = A;
			}

			Vector3D H = pie.TopCenter - pie.BaseCenter;
			Vector3D TB1 = innerLine[n-1];
			Vector3D TB0 = outerLine[0];
			if(WeakColinearProjection(TB1,TB0,outerLine[1],pie.Mapping))
			{
				sides.Add(new Polygon(new Vector3D[] 
				{ 
					TB0,
					TB1,
					TB1+H,
					TB0+H,
					TB0
				},pie.ChartColor,false,true));
			}
			Vector3D TE1 = outerLine[n-1];
			Vector3D TE0 = innerLine[0];
			if(WeakColinearProjection(outerLine[n-2],TE1,TE0,pie.Mapping))
			{
				sides.Add(new Polygon(new Vector3D[] 
				{ 
					TE0,
					TE1, 
					TE1+H,
					TE0+H,
					TE0
				},pie.ChartColor,false,true));
			}
			Band band = new Band(outerLine,outerLine1,pie.ChartColor,false,true);
			band.Parent = pie;
			ArrayList outVisible = band.GetVisibleSections();
			foreach(Band b in outVisible)
			{
				b.Parent = pie;
				bands.Add(b);
			}
		}

		private bool WeakColinearProjection(Vector3D V0, Vector3D V1, Vector3D V2, Mapping map)
		{
			Vector3D P0 = map.Map(V0);
			Vector3D P1 = map.Map(V1);
			Vector3D P2 = map.Map(V2);
			double sp = (P0.X-P1.X)*(P1.X-P2.X) + (P0.Y-P1.Y)*(P1.Y-P2.Y);
			return (sp > 0);
		}

		internal override void RenderPieSegment(PieSegment pie)
		{
			// We render entire PieDoughnutBox as a unit because of complex ordering
			// of elements, so - noting to do here.
		}

		internal override void RenderPieDoughnutBox(PieDoughnutBox obj)
		{
			Push(obj);
			GeometricObject bases = new GeometricObject();
			//GeometricObject sides = new GeometricObject();
			GeometricObject tops = new GeometricObject();
			bases.Parent = obj;
			tops.Parent = obj;

			int nSeries = obj.SubObjects.Count;

			// --- 1. Series might be in a hierarchy. Collect a linear list of simple section boxes
			//		Note: Inner series are listed first

			ArrayList simpleList = new ArrayList();
			for(int i=0; i<nSeries; i++)
			{
				if(obj.SubObjects[i] is SectionBox)
					CollectLinearListOfSections(obj.SubObjects[i] as SectionBox,simpleList);
			}
			
			// --- 2. Render all bases
			ArrayList toRenderLater = new ArrayList();
			int nSeg = simpleList.Count;
			for(int i=0; i<nSeg; i++)
			{
				SectionBox sb = simpleList[i] as SectionBox;
				if(sb != null && sb.SubObjects != null)
				{
					for(int j=0; j<sb.SubObjects.Count; j++)
					{
						PieSegment ps = sb.SubObjects[j] as PieSegment;
						if(ps != null)
						{
							SetActiveObject(ps.Tag);
							RenderPieSegmentBaseAndTop(ps,false);
						}
						else
							toRenderLater.Add(sb.SubObjects[j]);
					}
				}
			}

			// --- 3. Render inner side - outer segments first

			GeometricObject sides = new GeometricObject();
			sides.Parent = obj;
			ArrayList bands = new ArrayList();
			for(int i=0; i<nSeg; i++)
			{
				bands.Clear();
				if(sides.SubObjects != null)
					sides.SubObjects.Clear();
				SectionBox sb = simpleList[i] as SectionBox;
				if(sb != null && sb.SubObjects != null)
				{
					for(int j=0; j<sb.SubObjects.Count; j++)
					{
						PieSegment ps = sb.SubObjects[j] as PieSegment;
						if(ps != null)
							PieSegmentAddInnerWall(ps, sides,bands);

					}
				}
				renderInZOrder = true;
				RenderObject(sides);
				foreach(Band band in bands)
					RenderStrip(band.Line0,band.Line1,band.ChartColor,true,BorderDrawingMode.NoBorder,true,false);
			}

			// --- 4. Render outer side - inner segments first

			for(int i=nSeg-1; i>=0; i--)
			{
				bands.Clear();
				if(sides.SubObjects != null)
					sides.SubObjects.Clear();
				SectionBox sb = simpleList[i] as SectionBox;
				if(sb != null && sb.SubObjects != null)
				{
					for(int j=0; j<sb.SubObjects.Count; j++)
					{
						PieSegment ps = sb.SubObjects[j] as PieSegment;
						if(ps != null)
							PieSegmentAddOuterWall(ps, sides,bands);
					}
				}
				renderInZOrder = true;
				RenderObject(sides);
				foreach(Band band in bands)
					RenderStrip(band.Line0,band.Line1,band.ChartColor,true,BorderDrawingMode.NoBorder,true,false);
			}

			// --- 5. Render all tops

			for(int i=0; i<nSeg; i++)
			{
				SectionBox sb = simpleList[i] as SectionBox;
				if(sb != null && sb.SubObjects != null)
				{
					for(int j=0; j<sb.SubObjects.Count; j++)
					{
						PieSegment ps = sb.SubObjects[j] as PieSegment;
						if(ps != null)
							RenderPieSegmentBaseAndTop(ps,true);
					}
				}
			}

			// --- 6. Render the rest
			for(int i=0; i<toRenderLater.Count; i++)
			{
				GeometricObject go = toRenderLater[i] as GeometricObject;
				if(go != null)
					RenderObject(go);
			}
			Pop(obj.GetType());
			return;
		}

		private void CollectLinearListOfSeries(PieDoughnutBox obj,ArrayList list)
		{
			if(obj.SubObjects == null || obj.Tag == null)
				return;
			if(obj.Tag is Series)
				list.Add(obj.Tag);
			else
			{
				foreach(PieDoughnutBox pdb in obj.SubObjects)
					CollectLinearListOfSeries(pdb,list);
			}
		}

		#endregion

		#region --- Rendering lines ---

		internal override void RenderBlockLine(BlockLineStyle style, ChartLine line)
		{
			RenderLine(style.Height,style.Width,style.ChartColor,line);
		}

		internal override void RenderFlatLine(FlatLineStyle style, ChartLine line)
		{
			RenderLine(style.Height,style.Width,style.ChartColor,line);
		}

		internal override void RenderStripLine(StripLineStyle style, ChartLine line)
		{
			RenderLine(style.Height,style.Width,style.ChartColor,line);
		}

		internal override void RenderPipeLine(PipeLineStyle style, ChartLine line)
		{
			RenderLine(style.Height,style.Width,style.ChartColor,line);
		}

		internal override void RenderThickLine(ThickLineStyle style, ChartLine line)
		{
			RenderLine(style.Height,style.Width,style.ChartColor,line);
		}

		private void RenderLine(double height, double width, ChartColor color, ChartLine line)
		{
			Mapping map = line.Mapping;

			GraphicsPath path = new GraphicsPath();
			double h = height*map.FromPointToWorld;
			double w = width*map.FromPointToWorld;

			int nt = line.Nt;
			PointF[] pointsF = new PointF[nt];
			for(int i = 0; i< nt; i++)
			{
				pointsF[i] = new PointF((float)(line.Point(i).X),(float)(line.Point(i).Y));
			}
			path.AddLines(pointsF);
			Pen pen = new Pen(Color.Black,(float)w/2);
			pen.LineJoin = LineJoin.MiterClipped;
			pen.MiterLimit = 3;
			pen.StartCap = LineCap.NoAnchor;
			pen.EndCap = LineCap.NoAnchor;
			path.Widen(pen);
			pen.Dispose();
			double z = line.Point(0).Z;

			PointF[] border = path.PathPoints;
			Vector3D[] points = new Vector3D[border.Length+1];
			Vector3D[] pointsTop = new Vector3D[border.Length+1];
			Vector3D side = -line.GetAx().CrossProduct(line.GetAy())*h;
			if(side.Z > 0) 
				side = -side;
			for(int i=1; i<=border.Length; i++)
			{
				int ix = border.Length-i;
				points[i] = new Vector3D(border[ix].X,border[ix].Y,z+h/2);
				pointsTop[i] = points[i] + side;
			}
			points[0] = points[border.Length];
			pointsTop[0] = pointsTop[border.Length];
			path.Dispose();

			// check the orientation
			double p = 0;
			for(int i=0; i<border.Length;i++)
				p += (points[i+1].X-points[i].X)*(points[i].Y+points[i+1].Y);
			if(p<0)
			{
				int np = border.Length+1;
				int n2 = np/2;
				for(int i=0; i<n2; i++)
				{
					Vector3D pf = points[i];
					points[i] = points[np-1-i];
					points[np-1-i] = pf;
					pf = pointsTop[i];
					pointsTop[i] = pointsTop[np-1-i];
					pointsTop[np-1-i] = pf;
				}
			}
			Polygon polygon = new Polygon(points,line.ChartColor,true,false);

			Band band = new Band(points,pointsTop,line.ChartColor,false,false);
			RenderStrip(points,pointsTop,line.ChartColor,false,BorderDrawingMode.NoBorder,false,true);

			band.Parent = line;
			
			RenderPolygon(line,line.ChartColor,points,true,BorderDrawingMode.NoBorder);
		}

		internal override string OverrideDashLineStyleName(string lineStyleName)
		{
			return "FlatLine";
		}

		internal override GeometricObject GetDotObjectForDotLine(Vector3D center, double radius, ChartColor color)
		{
			Ellipsoid elp = new Ellipsoid(center,new Vector3D(radius,0,0), new Vector3D(0,radius,0),radius, color);
			elp.Parent = Top;
			return elp;
		}

		#endregion
		internal override void RenderRadarBox(RadarBox obj)
		{
			base.RenderRadarBox(obj);
		}

		internal override void RenderSectionBox(SectionBox obj)
		{
			if(obj.SubObjects == null)
				return;

			ArrayList objects = new ArrayList();
			for(int i = 0; i<obj.SubObjects.Count; i++)
			{
				if(!(obj.SubObjects[i] is ChartText))
					objects.Add(obj.SubObjects[i]);
			}
			SortObjects(objects);
			for(int i = 0; i<objects.Count; i++)
			{
				RenderObject(objects[i] as GeometricObject);
			}
			for(int i = 0; i<obj.SubObjects.Count; i++)
			{
				if((obj.SubObjects[i] is ChartText))
				{
					ChartText ct = obj.SubObjects[i] as ChartText;
					ct.Location = obj.ProjectToFrontFace(ct.Location);
					RenderObject(ct);
				}
			}
		}

		internal override void RenderColumnBox(ColumnBox obj)
		{
			base.RenderColumnBox(obj);
		}

		internal override void RenderSubColumnBox(SubColumnBox obj)
		{
			base.RenderSubColumnBox(obj);
		}

		internal override void RenderCoordinatePlaneBox(CoordinatePlaneBox obj)
		{
			SortObjects(obj.SubObjects);
			for(int i=0; i<obj.SubObjects.Count;i++)
			{
				if(obj.SubObjects[i] is DrawingBoard)
					continue;
				RenderObject(obj.SubObjects[i] as GeometricObject);
			}
			if(obj.FrontSideVisible())
			{
				for(int i=0; i<obj.SubObjects.Count;i++)
				{
					if(obj.SubObjects[i] is DrawingBoard)
						RenderObject(obj.SubObjects[i] as GeometricObject);
				}
			}
		}

		internal override void RenderPolygon(Polygon pol)
		{
			objectTrackingData.SetActiveObject(pol.Tag);
			if(pol.Parent == null)
				pol.Parent = Top;
			RenderPolygon(pol,pol.ChartColor,pol.Points,false,BorderDrawingMode.NoBorder);
		}

		#region --- Rendering 2D Objects: Text and Line ---
		
		internal override void Render2DText(Chart2DText text2D)
		{
			// We don't render at this time, instead we delay to render on top of the bitmap
			flatObjects.Add(text2D);
		}
		
		internal override void Render2DLine(Chart2DLine line2D)
		{
			// We don't render at this time, instead we delay to render on top of the bitmap
			flatObjects.Add(line2D);
		}

		private void RenderFlatObjects(Graphics g)
		{
			for(int i=0; i<flatObjects.Count; i++)
			{
				Chart2DText txt = flatObjects[i] as Chart2DText;
				if(txt != null)
				{
					txt.Render(g);
				}
				else
				{
					Chart2DLine line = flatObjects[i] as Chart2DLine;
					if(line != null)
					{
						line.Render(g);
					}
				}
			}
		}
		#endregion

		#region --- Helpers ---
		internal override void SetActiveObject(object activeObject)
		{
			base.SetActiveObject(activeObject);
			objectTrackingData.SetActiveObject(activeObject);
		}

		private Color GetColor(ChartColor c, Vector3D NE)
		{
			double spDirect,spReflected;
			GetLightComponents(NE,out spDirect, out spReflected);
			return c.GetColor(spDirect,spReflected,ambientFraction);
		}

		private void GetLightComponents(Vector3D NE, out double spDirect, out double spReflected)
		{
			double f = 10;
			Vector3D viewDirection = mapping.ViewDirection.Unit();
			if(lightDirectionKind == LightDirectionKind.BackLightDirection)
			{
				lightDirection = -viewDirection;
				lightDirection.Y = - lightDirection.Y;
				f = 1;
			}
			else
			{
				lightDirection = frontLightDirection;
				f = 10;
			}

			Vector3D VD = new Vector3D(viewDirection.X,viewDirection.Y/f,viewDirection.Z);
			Vector3D NE1 = new Vector3D(NE.X,NE.Y/f,NE.Z);
			Vector3D LD = new Vector3D(lightDirection.X,lightDirection.Y/f,lightDirection.Z);
			VD = VD.Unit();
			NE1 = NE1.Unit();
			LD = LD.Unit();
			Vector3D reflectedLight = NE1*(2*NE1*LD) - LD;
			spDirect = NE*lightDirection;
			spReflected = reflectedLight*VD;
		}

		private void RenderParallelogram(GeometricObject obj, ChartColor surface, Vector3D P0, Vector3D P1, Vector3D P2, bool showBack)
		{
			//  P2     P3
			//  |
			//  |
			//  |
			//  P0 --- P1

			Vector3D P3 = P1 - P0 + P2;
			RenderPolygon(obj,surface, new Vector3D[] { P0, P2, P3, P1, P0 }, showBack, BorderDrawingMode.SameColorBorder);
		}
				
		private bool RenderPolygon(GeometricObject obj, ChartColor surface, Vector3D[] pointsICS, bool showBack, BorderDrawingMode drawBorder)
		{
			return RenderPolygon(obj,surface,pointsICS,showBack, drawBorder, true);
		}

		private bool RenderPolygon(GeometricObject obj, ChartColor surface, Vector3D[] pointsICS, bool showBack, BorderDrawingMode drawBorder, bool adjustColor)
		{

			// GDI+ Dependent
			int np = pointsICS.Length;
			// Closing the polygon
			PointF[] points = new PointF[np];
			for(int i=0;i<pointsICS.Length;i++)
			{
				Vector3D pWCS = obj.Mapping.Map(pointsICS[i]);
				points[i] = new PointF((float)(pWCS.X),(float)(pWCS.Y));
			}

			if(computingPosition)
			{
				for(int i=0;i<pointsICS.Length;i++)
				{
					x0p = Math.Min(x0p,points[i].X);
					x1p = Math.Max(x1p,points[i].X);
					y0p = Math.Min(y0p,points[i].Y);
					y1p = Math.Max(y1p,points[i].Y);
				}
				return true;
			}

			// Compute side
			if(!showBack)
			{
				double p = 0;
				for(int i=0;i<np;i++)
				{
					int j = (i+1)%np;
					p += (points[i].Y+points[j].Y)*(points[j].X-points[i].X);
				}
				if(p < 0)
					return false;
			}

			Color c;

			if(adjustColor)
			{
				// Compute normal
				Vector3D N = new Vector3D(0,0,0);
				Vector3D P0 = pointsICS[0];
				for(int i=1;i<np-1;i++)
					N = N - (pointsICS[i]-P0).CrossProduct(pointsICS[(i+1)%np]-P0);

				if(N.IsNull)
					return false;

				c = GetColor(surface,N.Unit());
			}
			else
				c = surface.Color;

			Brush brush = new SolidBrush(c);
			SetActiveObject(obj.Tag);
			g.FillPolygon(brush,points,FillMode.Winding);
			objectTrackingData.ProcessRegion(points);

			if(drawBorder != BorderDrawingMode.NoBorder)
			{
				Pen pen = null;
				if(drawBorder == BorderDrawingMode.SameColorBorder)
					pen = new Pen(brush,1);
				else
				{
					int p = darkerColorPercentage;
					Color b1 = Color.FromArgb(c.A,(c.R*p)/100,(c.G*p)/100,(c.B*p)/100);
					pen = new Pen(b1,1);
				}
				g.DrawPolygon(pen,points);
				pen.Dispose();
			}
			brush.Dispose();
			return true;
		}

		private void FillPolygon(ChartGradient grad, Vector3D[] pointsICS, bool showBack, BorderDrawingMode drawBorder)
		{
			// GDI+ Dependent
			if(grad is LinearChartGradient)
			{
				LinearChartGradient lg = grad as LinearChartGradient;
				LinearGradientBrush brush = new LinearGradientBrush(lg.Point1,lg.Point2,Color.White,Color.White);
				ColorBlend blend = new ColorBlend(lg.Colors.Length);
				blend.Colors = lg.Colors;
				blend.Positions = lg.Positions;
				brush.InterpolationColors = blend;
				FillPolygon(brush,grad.BorderColor,pointsICS,showBack,drawBorder);
				brush.Dispose();
			}
		}

		private void FillPolygon(Color c, Vector3D[] pointsICS, bool showBack,BorderDrawingMode drawBorder)
		{
			// GDI+ Dependent
			Brush brush = new SolidBrush(c);
			FillPolygon(brush,c, pointsICS,showBack,drawBorder);
			brush.Dispose();
		}

		private void FillPolygon(Brush brush, Color color, Vector3D[] pointsICS, bool showBack, BorderDrawingMode drawBorder)
		{
			// GDI+ Dependent
			int np = pointsICS.Length;
			// Closing the polygon
			PointF[] points = new PointF[np];
			for(int i=0;i<pointsICS.Length;i++)
			{
				Vector3D pWCS = mapping.Map(pointsICS[i]);
				points[i] = new PointF((float)(pWCS.X),(float)(pWCS.Y));
			}

			// Compute side
			if(!showBack)
			{
				double p = 0;
				for(int i=1;i<=np;i++)
					p += (points[(i-1)%np].Y+points[i%np].Y)*(points[i%np].X-points[(i-1)%np].X);
				if(p < 0)
					return;
			}
			g.FillPolygon(brush,points);
			objectTrackingData.ProcessRegion(points);

			// Drawing border

			if(drawBorder == BorderDrawingMode.NoBorder)
				return;
			
			Pen pen = null;
			if(drawBorder == BorderDrawingMode.SameColorBorder)
				pen = new Pen(brush,1);
			else if(drawBorder == BorderDrawingMode.DarkerColorBorder)
			{
				int p = darkerColorPercentage; // % of light
				pen = new Pen(Color.FromArgb(color.A, (p*color.R)/100, (p*color.G)/100, (p*color.B)/100),1);
			}
			else
				throw new Exception("Implementation: not supported BorderDrawingMode." + drawBorder.ToString());

			g.DrawPolygon(pen,points);
			pen.Dispose();		
		}

		internal override void RenderWall(Wall wall)
		{
			Vector3D P = wall.Origin;
			Vector3D Vx = wall.XDirection;
			Vector3D Vy = wall.YDirection;
			double height = wall.Height;
			ChartColor frontSurface = wall.Color;
			if(wall.Parent == null)
				wall.Parent = Top;

			Vector3D Vxe = Vx.Unit();
			Vector3D Vye = Vy.Unit();
			Vector3D Vh = Vye.CrossProduct(Vxe) * height;

			GeometricObject bandSections = wall.GetBandSections();
			bandSections.Parent = wall;
			Push(wall);

			foreach(Band b in bandSections.SubObjects)
			{
				ArrayList vs = b.GetVisibleSections();
				foreach(Band vb in vs)
				{
					RenderStrip(vb.Line0,vb.Line1,vb.ChartColor,false,BorderDrawingMode.NoBorder,vb.Smooth,vb.Closed);
				}
			}
			Pop(wall.GetType());
		}

		internal void RenderWall(Vector3D[] pointsICS, Vector3D sideICS, ChartColor color, bool showBack, BorderDrawingMode drawBorder, bool smooth, bool closed)
		{
			int np = pointsICS.Length;
			Vector3D[] points1ICS = new Vector3D[np];
			for(int i = 0; i<np; i++)
				points1ICS[i] = pointsICS[i] + sideICS;
			RenderStrip(pointsICS, points1ICS,color,showBack,drawBorder,smooth,closed);
		}

		internal LinearChartGradient GetLinearGradient(Band band)
		{
			Vector3D[] line0 = band.Line0;
			Vector3D[] line1 = band.Line1;
			ChartColor color = band.ChartColor;
			Mapping map = mapping;

			// Finding the point getting the most light. To be used for gradient orientation
			double spDirect,spReflected;
			double maxSP = double.MinValue;
			int index = 0;
			int np = line0.Length;
			int ns = np-1;
			if(band.Closed)
				ns++;

			int i;
			ArrayList nNulPts = new ArrayList();
			for(i=0; i<ns+1; i++)
			{
				int iNext = (i+1)%np;
				int inp = i%np;
				Vector3D NE = (line0[iNext]-line0[inp]).CrossProduct(line1[inp]-line0[inp]);
				if(!NE.IsNull)
				{
					nNulPts.Add(inp);
					NE = NE.Unit();
					GetLightComponents(NE,out spDirect,out spReflected);
					if(spDirect + spReflected > maxSP)
					{
						index = inp;
						maxSP = spDirect + spReflected;
					}
				}
			}
			int[] ptix = (int[])nNulPts.ToArray(typeof(int));

			// Side direction and gradient endpoints
			Vector3D CP = map.Map(line0[index]);
			Vector3D sideP = map.Map(line1[index]) - CP;
			//   endpoints direction; 90 dgrs clockvise
			double xge = sideP.Y;
			double yge = -sideP.X;
			double dg = Math.Sqrt(xge*xge + yge*yge);
			xge /= dg;
			yge /= dg;
			//   calculating endpoints
			double sp0 = double.MaxValue;
			double sp1 = double.MinValue;
			for(i=0; i<np; i++)
			{
				Vector3D P0 = map.Map(line0[i]);
				Vector3D P1 = map.Map(line1[i]);
				double xp0 = P0.X;
				double yp0 = P0.Y;
				double xp1 = P1.X;
				double yp1 = P1.Y;
				double sp = (xp0 - CP.X)*xge + (yp0 - CP.Y)*yge;
				sp0 = Math.Min(sp0,sp);
				sp1 = Math.Max(sp1,sp);
				sp = (xp1 - CP.X)*xge + (yp1 - CP.Y)*yge;
				sp0 = Math.Min(sp0,sp);
				sp1 = Math.Max(sp1,sp);
			}
			double dsp = (sp1-sp0);
			sp0 -= dsp;
			sp1 += dsp;
			PointF gradPt0 = new PointF((float)(CP.X+sp0*xge),(float)(CP.Y+sp0*yge));
			PointF gradPt1 = new PointF((float)(CP.X+sp1*xge),(float)(CP.Y+sp1*yge));
			int nPos = ptix.Length;
			Color[] colors = new Color[nPos];
			float[] positions = new float[nPos];
			double d = (gradPt1.X-gradPt0.X)*(gradPt1.X-gradPt0.X) + (gradPt1.Y-gradPt0.Y)*(gradPt1.Y-gradPt0.Y);
			d = Math.Sqrt(d);
			for(int ii=0; ii<nPos ; ii++)
			{
				i = ptix[ii];
				int iNext = ptix[(ii+1)%nPos];
				Vector3D P0 = map.Map(line0[i]);
				double xp0 = P0.X;
				double yp0 = P0.Y;
				Vector3D N = new Vector3D(0,0,0);;
				try
				{
					N = ((line0[iNext]-line0[i]).CrossProduct(line1[i]-line0[i]));
				}
				catch
				{
				}
				if(!N.IsNull)
				{
					N = N.Unit();
					colors[i] = GetColor(color,N);
				}
				else
				{
					if(i>0)
						colors[i] = colors[i-1];
					else
						colors[i] = color.Color;
				}
				positions[i] = (float)(((xp0-gradPt0.X)*xge + (yp0-gradPt0.Y)*yge)/d);
			}
			positions[0] = 0;
			colors[0] = colors[1];
			positions[nPos-1] = 1;
			colors[nPos-1] = colors[nPos-2];

			return new LinearChartGradient(colors,color.Color,positions,gradPt0,gradPt1);
		}


		internal void RenderStrip(Vector3D[] points0ICS, Vector3D[] points1ICS, ChartColor color, bool showBack, BorderDrawingMode drawBorder, bool smooth, bool closed)
		{
			Band band = new Band(points0ICS, points1ICS,color,closed,smooth);
			band.Parent = Top;

			ArrayList sections = band.GetVisibleSections();
			if(showBack)
			{
				ArrayList invisibleSections = band.GetInvisibleSections();
				foreach(Band b in invisibleSections)
					sections.Add(b);
			}

			foreach(Band b in sections)
			{
				if(smooth)
				{
					ChartGradient grad = GetLinearGradient(b);
					Polygon polygon = b.GetPolygon();
					FillPolygon(grad,polygon.Points,showBack,drawBorder);
				}
				else
				{
					int nRect = b.Line0.Length-1;
					for(int i=0; i<nRect; i++)
					{
						Vector3D N = (b.Line0[i+1]-b.Line0[i]).CrossProduct(b.Line1[i]-b.Line0[i]);
						if(N.IsNull)
							continue;
						N = N.Unit();
						FillPolygon(GetColor(b.ChartColor.Color,N),
							new Vector3D[] { b.Line0[i],b.Line1[i],b.Line1[i+1],b.Line0[i+1] },
							showBack,BorderDrawingMode.SameColorBorder);
					}
				}
			}
		}

		#endregion
	}

	internal class ChartGradient
	{
		private Color   borderColor;
		public Color   BorderColor { get { return borderColor; } set { borderColor = value; } }
		public ChartGradient(Color borderColor)
		{
			this.borderColor = borderColor;
		}
	}

	internal class LinearChartGradient : ChartGradient
	{
		private float[] positions;
		private Color[] colors;
		private PointF	point1;
		private PointF	point2;

		public LinearChartGradient(Color[] colors, Color borderColor, float[] positions, PointF point1, PointF point2) 
			: base(borderColor)
		{
			this.colors = colors;
			this.positions = positions;
			this.point1 = point1;
			this.point2 = point2;
		}

		public Color[] Colors { get { return colors; } set { colors = value; } }
		public float[] Positions { get { return positions; } set { positions = value; } }
		public PointF Point1 { get { return point1; } set { point1 = value; } }
		public PointF Point2 { get { return point2; } set { point2 = value; } }

	}

	internal class RadialChartGradient : ChartGradient
	{
		private PointF[] points;
		private Color[] colors;
		private float[] positions;
		private PointF	centerPoint;

		public RadialChartGradient(Color[] colors, float[] positions, PointF[] points, PointF centerPoint) 
			: base(Color.Black)
		{
			this.colors = colors;
			this.positions = positions;
			this.points = points;
			this.centerPoint = centerPoint;
		}

		public Color[] Colors { get { return colors; } set { colors = value; } }
		public float[] Positions { get { return positions; } set { positions = value; } }
		public PointF[] Points { get { return points; } set { points = value; } }
		public PointF CenterPoint { get { return centerPoint; } set { centerPoint = value; } }
	}

	internal class ObjectTrackingData 
	{
		private GeometricEngineBasedOnRenderingOrder engine;
		private int nx, ny;
		private bool isTracking;
		private int [,] matrix;

		private ArrayList objects = new ArrayList();
		private int activeObjectIndex = 0;

		internal ObjectTrackingData(GeometricEngine engine)
		{
			this.engine = engine as GeometricEngineBasedOnRenderingOrder;
		}

		internal void StartTracking(int nx, int ny)
		{
			isTracking = engine.Chart.ObjectTrackingEnabled;
			if(!isTracking)
				return;
			this.nx = nx;
			this.ny = ny;
			matrix = new int[nx,ny];
		}

		internal int ActiveObjectIndex { get { return activeObjectIndex; } }

		internal void SetActiveObject(object obj)
		{
			if(obj == null || !isTracking)
			{
				activeObjectIndex = 0;
				return;
			}

			// NB: objectIndex is its index in the array + 1 !!!
			// This is because index = 0 is used fo "no object" case

			int x = objects.IndexOf(obj) + 1;
			if(x>0 && x == activeObjectIndex) // this is current active object, we don't change anything
				return;

			activeObjectIndex = x;
			if(activeObjectIndex <= 0)
				activeObjectIndex = objects.Add(obj)+1;
		}

		internal void ProcessRegion(Region region, int x0, int x1, int y0, int y1)
		{
			if(activeObjectIndex <= 0)
				return;
			x0 = Math.Max(x0,0);
			y0 = Math.Max(y0,0);
			x1 = Math.Min(x1,nx-1);
			y1 = Math.Min(y1,ny-1);
			for(int i=x0; i<=x1; i++)
				for(int j=y0; j<=y1; j++)
				{
					if(region.IsVisible(new Point(i,j)))
						matrix[i,j] = activeObjectIndex;
				}				
		}

		internal void ProcessRegion(GraphicsPath path)
		{
			if(activeObjectIndex == 0)
				return;
			PointF[] points = path.PathData.Points;
			int x0 = int.MaxValue;
			int y0 = int.MaxValue;
			int x1 = int.MinValue;
			int y1 = int.MinValue;
			for(int i=0;i<points.Length;i++)
			{
				x0 = Math.Min(x0,(int)(points[i].X));
				y0 = Math.Min(y0,(int)(points[i].Y));
				x1 = Math.Max(x1,(int)(points[i].X));
				y1 = Math.Max(y1,(int)(points[i].Y));
			}

			Region region = new Region(path);
			ProcessRegion(region,x0,x1,y0,y1);
			region.Dispose();
		}

		internal void ProcessRegion(PointF[] points)
		{
			if(activeObjectIndex == 0)
				return;
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(points);
			ProcessRegion(path);
			path.Dispose();
		}
		
		internal void GetObjectTrackingInfo(out int[,] matrix, out ArrayList objects)
		{
			matrix = this.matrix;
			objects = this.objects;
		}
	}
}
