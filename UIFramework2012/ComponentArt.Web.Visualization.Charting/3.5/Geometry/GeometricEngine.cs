using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
    /// <summary>
    /// Base class for geometric engines
    /// </summary>
    internal class GeometricEngine : IDisposable
    {
        #region --- Member Variables ---

        #region --- Marker Polygons ---

        static float[] xCross = new float[] { -3, -3, -1, -1, 1, 1, 3, 3, 1, 1, -1, -1, -3 };
        static float[] yCross = new float[] { -1, 1, 1, 3, 3, 1, 1, -1, -1, -3, -3, -1, -1 };
        static float fCross = 1.0f / 6;

        static float[] xBlock = new float[] { -1, -1, 1, 1, -1 };
        static float[] yBlock = new float[] { -1, 1, 1, -1, -1 };
        static float fBlock = 1.0f / 2;

        static float[] xDiam = new float[] { -1, 0, 1, 0, -1 };
        static float[] yDiam = new float[] { 0, 1, 0, -1, 0 };
        static float fDiam = 1.0f / 2;

        static float[] xTria = new float[] { -0.5f, 0, 0.5f, -0.5f };
        static float[] yTria = new float[] { -0.255f, 0.510f, -0.255f, -0.255f };
        static float fTria = 1.0f;

        static float[] xITria = new float[] { -0.5f, 0, 0.5f, -0.5f };
        static float[] yITria = new float[] { 0.255f, -0.510f, 0.255f, 0.255f };
        static float fITria = 1.0f;

        static float[] xLTria = new float[] { 0.255f, -0.510f, 0.255f, 0.255f };
        static float[] yLTria = new float[] { -0.5f, 0, 0.5f, -0.5f };
        static float fLTria = 1.0f;

        static float[] xRTria = new float[] { -0.255f, 0.510f, -0.255f, -0.255f };
        static float[] yRTria = new float[] { -0.5f, 0, 0.5f, -0.5f };
        static float fRTria = 1.0f;

        static float[] xArrow = new float[] { -2.0f, -2.0f, -1.0f, -1.0f, 0.0f, -1.0f, -1.0f, -2.0f };
        static float[] yArrow = new float[] { -0.5f, 0.5f, 0.5f, 1.0f, 0.0f, -1.0f, -0.5f, -0.5f };
        static float fArrow = 0.5f;

        #endregion
        
		#region --- Stack ---

		private GeometricObject top;
		private GeometricObject[] stack = new GeometricObject[100];
		private int nStack = 0, mStack = 100;

		#endregion
        private ChartBase chart = null;
        private double renderingPrecisionPxl = 0.3;
        protected Mapping mapping;//, effectiveMapping;
        protected LightCollection lights;
        private object activeObject;

		protected bool renderInZOrder = false;

        #endregion

		#region --- Features Supported ---
		
		internal virtual bool SupportsFeature(string featureName)
		{
			return false;
		}

		#endregion

		#region --- Computing Position ---

		protected bool computingPosition = false;
		protected double  x0p, y0p, x1p, y1p;

		#endregion

        #region --- Construction and Settings ---

        internal GeometricEngine() : this(null) { }
		internal GeometricEngine(ChartBase chart)
		{
			this.chart = chart;
            Push(new GeometricObject());
		}

		#region --- IDisposable Interface Implementation ---
		public void Dispose()
		{
			Dispose(true);
			// Take yourself off the Finalization queue 
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if(Root != null)
				Root.Dispose();
			for(int i=0; i<nStack; i++)
				stack[i] = null;
		}

		#endregion

		/// <summary>
		/// Seting the mapping object during tree creation
		/// </summary>
		/// <param name="map">Mapping object</param>
		internal virtual void SetMapping(Mapping map)
		{
			this.mapping = map;
		}

        internal virtual void SetLights(LightCollection lights)
        {
            this.lights = lights;
        }

        internal virtual void SetRenderingPrecisionPxl(double renderingPrecision)
        {
        }

		internal virtual void SetBackground(Bitmap backgroundImage)
		{
		}

		internal virtual void SetGraphics(Graphics g)
		{
		}

        internal virtual void SetPositionComputingMode(bool computingPosition)
        {
			this.computingPosition = computingPosition;
        }

        internal virtual Rectangle GetPositionRectangle()
        {
			return new Rectangle((int)x0p,(int)y0p,(int)(x1p-x0p+1),(int)(y1p-y0p+1));
		}

        internal virtual void SetActiveObject(object activeObject)
        {
            this.activeObject = activeObject;
        }

        internal virtual object GetActiveObject()
        {
            return activeObject;
        }

        #endregion

        #region --- Services ---

        internal LineStyle GetLineStyle(string styleName)
        {
            return Chart.LineStyles[styleName];
        }

        internal LabelStyle GetLabelStyle(string styleName)
        {
            return Chart.LabelStyles[styleName];
        }

        internal MarkerStyle GetMarkerStyle(string styleName)
        {
            return Chart.MarkerStyles[styleName];
        }

        internal void SendMessage(string message)
        {
            Chart.Message(message);
        }

		internal virtual ObjectMapper GetObjectMapper()
		{
			return null;
		}

        #endregion

        internal ChartBase Chart { get { return chart; } } 

        internal virtual double RenderingPrecisionInPixelSize
        {
            get { return renderingPrecisionPxl; }
            set { renderingPrecisionPxl = value; }
        }

		internal GeometricObject Root 
		{ 
			get 
			{ 
				return stack[0] as GeometricObject;
			}
		}
				
        #region --- Objects Creation ---

        internal void Add(GeometricObject obj)
        {
            if (obj != null)
            {
                top.Add(obj);
                obj.Tag = activeObject;
				if(activeObject is Series)
					return;
            }
        }

		internal virtual void Clear()
		{
			top = Root;
			top.Clear();
		}

		#region --- Stack handling ---

		internal virtual void Push(GeometricObject obj)
		{
			if(stack.Length > nStack)
			{
				if(obj.Parent == null && top != null)
					top.Add(obj);
				stack[nStack] = obj;
				nStack++;
				top = obj;
			}
			else
				throw new Exception("GE stack overflow");
		}

		internal virtual GeometricObject Pop()
		{
			if(nStack > 0)
			{
				nStack--;
				if(nStack>0)
					top = stack[nStack-1];
				else
					top = null;
			}
			else
				throw new Exception("Geometric Engine stack underflow");
			return stack[nStack]  as GeometricObject;
		}

		internal virtual GeometricObject Pop(Type type)
		{
			if(top != null)
			{
				if(!top.GetType().IsSubclassOf(type) && top.GetType() != type)
				{
					throw new Exception("Invalid type at GE stack top." +
						" Expected '" + type.Name + "', found '" + top.GetType().Name + "'");
				}
				return Pop();
			}
			else
				throw new Exception("Geometric Engine stack underflow");
		}

		internal virtual GeometricObject Top { get { return top; } }

		#endregion

        #region --- Create Pie Segments

        internal virtual PieSegment CreatePieSegment(double alpha0, double alpha1,
            Vector3D C1, Vector3D C2, Vector3D R1, double innerRadius,
            double outerEdgeSmoothingRadius, double innerEdgeSmoothingRadius, ChartColor surface)
        {
            PieSegment PS = new PieSegment(alpha0, alpha1, C1, C2, R1, innerRadius, outerEdgeSmoothingRadius, innerEdgeSmoothingRadius, surface);
            Add(PS);
            return PS;
        }

        #endregion

        #region --- Create Quadrilateral ---

        internal virtual Quadrilateral CreateQuadrilateral(Vector3D P00, Vector3D P01, Vector3D P11, Vector3D P10,
            Vector3D N00, Vector3D N01, Vector3D N11, Vector3D N10, ChartColor surface)
        {
            Quadrilateral q = new Quadrilateral(P00, P01, P11, P10, N00, N01, N11, N10, surface);
            Add(q);
            return q;
        }
        #endregion

		#region --- Create Radial Strip ---

		internal virtual RadialStrip CreateRadialStrip(Vector3D[] innerRing, Vector3D[] outerRing, Vector3D[] normal, ChartColor chartColor)
		{
			RadialStrip rStrip = new RadialStrip(innerRing,outerRing,normal,chartColor);
			Add(rStrip);
			return rStrip;
		}

		#endregion

        #region --- CreateEllipse ---
        internal virtual Ellipse CreateEllipse(Vector3D center, Vector3D radius1, Vector3D radius2, ChartColor color)
        {
            Ellipse e = new Ellipse(center, radius1, radius2, color);
            Add(e);
            return e;
        }

        #endregion

        #region --- Create Text ---

        internal virtual ChartText CreateText(LabelStyle style, Vector3D position, string text)
        {
            ChartText txt = new ChartText(style, position, text);
            Add(txt);
			txt.Tag = activeObject;
            return txt;
        }
        #endregion

        #region --- Create Torus Segments

        internal virtual TorusSegment CreateTorusSegment
            (double alpha0, double alpha1, double beta0, double beta1,
            Vector3D C1, Vector3D C2, Vector3D R1, double r2, double rh, ChartColor surface)
        {
            TorusSegment TS = new TorusSegment(alpha0, alpha1, beta0, beta1, C1, C2, R1, r2, rh, surface);
            Add(TS);
            return TS;
        }

        #endregion

        #region --- Create Prism and Block ---

        internal virtual Prism CreatePrism(Vector3D C1, Vector3D C2, Vector3D R1, double r2, int nSides, double rEdge, ChartColor surface)
        {
            Prism prism = new Prism(C1, C2, R1, r2, nSides, rEdge, surface);
            Add(prism);
            return prism;
        }

        internal Block CreateBlock(Vector3D P, Vector3D Height, Vector3D Side1,
            double side2, double edgeSmoothingRadius, ChartColor surface)
        {
            Block block = new Block(P, Height, Side1, side2, edgeSmoothingRadius, surface);
            Add(block);
            return block;
        }
        #endregion

        #region --- Create Box ---
        internal virtual Box CreateBox(Vector3D P0, Vector3D P1, ChartColor surface)
        {
            Box box = new Box(P0, P1, surface);
            Add(box);
            return box;
        }
        #endregion

        #region --- Create Cylinder ---
        internal Cylinder CreateCylinder(Vector3D C1, Vector3D C2, Vector3D R1, double r2, double rEdge, ChartColor surface)
        {
            Cylinder cylinder = new Cylinder(C1, C2, R1, r2, rEdge, surface);
            Add(cylinder);
            return cylinder;
        }
        internal Cylinder CreateCylinder(Vector3D C1, Vector3D C2, Vector3D R1, double r2, ChartColor surface)
        {
            Cylinder cylinder = new Cylinder(C1, C2, R1, r2, surface);
            Add(cylinder);
            return cylinder;
        }

        #endregion

        #region --- Create Pyramid ---

        internal Pyramid CreatePyramid(Vector3D center, Vector3D vertex,
            Vector3D radius, double normRadius, double relativeH1, double relativeH2, int nSides, ChartColor surface)
        {
 
            Pyramid pyramid = new Pyramid(center, vertex, radius, normRadius, relativeH1, relativeH2, nSides, surface);
            Add(pyramid);
            return pyramid;
        }

        internal Pyramid CreatePyramid(Vector3D center, Vector3D vertex,
            Vector3D radius, double normRadius, int nSides, ChartColor surface)
        {
            Pyramid pyramid = new Pyramid(center, vertex, radius, normRadius, 0, 1, nSides, surface);
            Add(pyramid);
            return pyramid;
        }
        #endregion

        #region --- Create Cone ---

        internal Cone CreateCone(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius, double relativeH1, double relativeH2, ChartColor surface)
        {
            Cone cone = new Cone(center, vertex, radius, normRadius, relativeH1, relativeH2, surface);
            Add(cone);
            return cone;
        }

        internal Cone CreateCone(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius, ChartColor surface)
        {
            Cone cone = new Cone(center, vertex, radius, normRadius, 0, 1, surface);
            Add(cone);
            return cone;
        }

        #endregion

        #region --- Create Ellipsoid and Sphere ---

        internal Ellipsoid CreateEllipsoid(Vector3D center, Vector3D axis, Vector3D height, double axis2Length,
            double alpha0, double alpha1, double theta0, double theta1, ChartColor surface)
        {
            Ellipsoid ellipsoid = new Ellipsoid(center, axis, height, axis2Length, alpha0, alpha1, theta0, theta1, surface);
            Add(ellipsoid);
            return ellipsoid;
        }

        internal Ellipsoid CreateEllipsoid(Vector3D center, Vector3D axis, Vector3D height, double axis2Length, ChartColor surface)
        {
            Ellipsoid ellipsoid = new Ellipsoid(center, axis, height, axis2Length, surface);
            Add(ellipsoid);
            return ellipsoid;
        }

        internal Ellipsoid CreateSphere(Vector3D S, double radius, ChartColor frontSurface)
        {
            return CreateEllipsoid(S, new Vector3D(radius, 0, 0), new Vector3D(0, radius, 0), radius, frontSurface);
        }

        #endregion

        #region --- Create Paraboloid ---

        internal Paraboloid CreateParaboloid(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius,
            double relativeH1, double relativeH2, ChartColor surface)
        {
            Paraboloid paraboloid = new Paraboloid(center, vertex, radius, normRadius, relativeH1, relativeH2, surface);
            Add(paraboloid);
            return paraboloid;
        }

        internal Paraboloid CreateParaboloid(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius, ChartColor surface)
        {
            return CreateParaboloid(center, vertex, radius, normRadius, 0, 1, surface);
        }

        #endregion

        #region --- Create Wall  ---
        internal Wall CreateWall(Vector3D P, Vector3D Vx, Vector3D Vy, SimpleLineCollection lines,
            double height, ChartColor color)
        {
            Wall wall = new Wall(P, Vx, Vy, lines, height, color);
            Add(wall);
            return wall;
        }

         #endregion

        #region --- Create Line ---
        internal ChartLine CreateLine(string styleName, Vector3D P, Vector3D Vx, Vector3D Vy)
        {
            ChartLine line = new ChartLine(styleName, P, Vx, Vy);
            Add(line);
            return line;
        }

        #endregion

        #region --- Create Area ---
        internal ChartArea CreateArea(SimpleLineCollection LC, double h, Vector3D P,
            Vector3D Xe, Vector3D Ye, ChartColor bodyColor, ChartColor wallColor)
        {
            ChartArea area = new ChartArea(LC, h, P, Xe, Ye, bodyColor, wallColor);
            Add(area);
            return area;
        }

        internal Chart2DArea Create2DArea(SimpleLineCollection LC, DrawingBoard drawingBoard,
            GradientStyle primGradient, GradientStyle secGradient, LineStyle2D lineStyle)
        {
            Chart2DArea area = new Chart2DArea(LC,drawingBoard,primGradient,secGradient,lineStyle);
            Add(area);
            return area;
        }

        #endregion

        #region --- Create Marker ---

        internal Marker CreateMarker(string markerStyleName, double size, Vector3D position, ChartColor surface)
        {
            Marker m = new Marker(markerStyleName, position, size, surface);
            Add(m);
            return m;
        }

        internal Marker CreateMarker(string markerStyleName, double size, Vector3D position)
        {
            Marker m = new Marker(markerStyleName, position, size);
            Add(m);
            return m;
        }

        #endregion

		#region --- Creating 2D objects ---

		internal Chart2DText Create2DText(LabelStyle labelStyleRef, Vector3D P, string text)
		{
			Chart2DText cText = new Chart2DText(labelStyleRef,P,text);
			Add(cText);
			return cText;
		}

		internal Chart2DLine Create2DLine(Pen pen)
		{
			Chart2DLine cLine = new Chart2DLine(pen);
			Add(cLine);
			return cLine;
		}
		
		#endregion

		#region --- 2D Drawing in drawing board ---
		internal virtual DrawingBoard CreateDrawingBoard()
		{
			throw new Exception("'CreateDrawingBoard()' not implemented in the GE '" + this.GetType().Name + "'");
		}

		internal virtual DrawingBoard CreateDrawingBoard(Vector3D V0, Vector3D Vx, Vector3D Vy)
		{
			throw new Exception("'CreateDrawingBoard()' not implemented in the GE '" + this.GetType().Name + "'");
		}

        #endregion

		#region --- 2D Drawing in GDI+ Like Graphics "Canvace" ---
		
		internal virtual Canvace CreateCanvace()
		{
			throw new Exception("'CreateCanvace()' not implemented in the GE '" + this.GetType().Name + "'");
		}

		#endregion

        #endregion

        #region --- Objects Rendering ---

		internal bool SetRenderingInZOrderMode(bool inZOrder)
		{
			bool r = renderInZOrder;
			renderInZOrder = inZOrder;
			return r;
		}

        internal virtual object Render()
        {
            RenderObject(top);
            top.Clear();
			return null;
        }

		static int depth = 0;
		internal virtual void RenderObject(GeometricObject obj)
        {
            if (obj == null)
                return;
			
            if (obj is Box) RenderBox(obj as Box);
            else if (obj is Prism) RenderPrism(obj as Prism);
            else if (obj is Cylinder) RenderCylinder(obj as Cylinder);
            else if (obj is Block) RenderBlock(obj as Block);
            else if (obj is Pyramid) RenderPyramid(obj as Pyramid);
            else if (obj is Cone) RenderCone(obj as Cone);
            else if (obj is Ellipsoid) RenderEllipsoid(obj as Ellipsoid);
			else if (obj is Polygon) RenderPolygon(obj as Polygon);
			else if (obj is Paraboloid) RenderParaboloid(obj as Paraboloid);
			else if (obj is PieSegment) RenderPieSegment(obj as PieSegment);
            else if (obj is TorusSegment) RenderTorusSegment(obj as TorusSegment);
            else if (obj is Quadrilateral) RenderQuadrilateral(obj as Quadrilateral);
            else if (obj is Ellipse) RenderEllipse(obj as Ellipse);

            else if (obj is DrawingBoard) RenderDrawingBoard(obj as DrawingBoard);

            else if (obj is Wall) RenderWall(obj as Wall);
            else if (obj is ChartArea) RenderArea(obj as ChartArea);
            else if (obj is Chart2DArea) Render2DArea(obj as Chart2DArea);

            else if (obj is ChartLine) RenderLine(obj as ChartLine);

            else if (obj is ChartText) RenderText(obj as ChartText);
            else if (obj is Marker)     RenderMarker        (obj as Marker);
			// Chart GE hierarchy
			else if (obj is TargetAreaBox) RenderTargetAreaBox(obj as TargetAreaBox);
			else if (obj is CoordinateSystemBox) RenderCoordinateSystemBox(obj as CoordinateSystemBox);
			else if (obj is PieDoughnutBox) RenderPieDoughnutBox(obj as PieDoughnutBox);
			else if (obj is RadarBox) RenderRadarBox(obj as RadarBox);
			else if (obj is SectionBox) RenderSectionBox(obj as SectionBox);
			else if (obj is ColumnBox) RenderColumnBox(obj as ColumnBox);
			else if (obj is SubColumnBox) RenderSubColumnBox(obj as SubColumnBox);
			else if (obj is CoordinatePlaneBox) RenderCoordinatePlaneBox(obj as CoordinatePlaneBox);
			else if (obj is RadialStrip) RenderRadialStrip(obj as RadialStrip);
			else if (obj is Chart2DText) Render2DText(obj as Chart2DText);
			else if (obj is Chart2DLine) Render2DLine(obj as Chart2DLine);

            else
                RenderGeneric(obj);
			depth--;
        }

		/// <summary>
		/// Generic rendering is performed by rendering all sub-objects. All rendering functions
		/// in the base engine class are implemented as generic renderings. Therefore, if a chart
		/// object is implemented in derived engine as a set of subobjects, the derived engine doesn't
		/// have to implement the "RenderXXX" method for that type.
		/// </summary>
		/// <param name="gObject">Object to render</param>
		protected void RenderGeneric(GeometricObject gObject)
		{
			if (gObject.SubObjects == null)
				return;

			if(gObject.Parent == null && gObject != Top)
				gObject.Parent = Top;

			if(gObject.Tag is TargetArea)
			{
				TargetArea ta = gObject.Tag as TargetArea;
				SetMapping(ta.Mapping);
				BeginRenderingTargetArea(gObject.Tag as TargetArea);
			}

			if(renderInZOrder)
				SortObjects(gObject.SubObjects);
			for (int i = 0; i < gObject.SubObjects.Count; i++)
				RenderObject(gObject[i]);

			if(gObject.Tag is TargetArea)
				EndRenderingTargetArea(gObject.Tag as TargetArea);
		}

		#region --- Sorting ---
		internal static void SortObjects(ArrayList list)
		{
			if(list.Count < 5)
			{
				LinearSortObjects(list);
				return;
			}

			int i;
			int n = list.Count;
			int nLists = n/3 + 1;
			ArrayList LList = new ArrayList(nLists);
			for(i=0; i<nLists; i++)
				LList.Add(new ArrayList());
			// find the z-range
			double zMin = double.MaxValue;
			double zMax = double.MinValue;
			for(i=0;i<n;i++)
			{
				GeometricObject go = list[i] as GeometricObject;
				double z = go.OrderingZ();
				zMin = Math.Min(zMin,z);
				zMax = Math.Max(zMax,z);
			}
			// Make sure they aren't the same
			zMax = zMax+0.1;
			zMin = zMin-0.1;
			double d = (zMax - zMin)/nLists;

			for(i=0;i<n;i++)
			{
				GeometricObject go = list[i] as GeometricObject;
				double z = go.OrderingZ();
				int ix = (int)((z-zMin)/d);
				(LList[ix] as ArrayList).Add(go);
			}
			int j=0;
			for(i=0;i<nLists;i++)
			{
				ArrayList AL = LList[i] as ArrayList;
				LinearSortObjects(AL);
				for(int k=0; k<AL.Count; k++)
				{
					list[j] = AL[k];
					j++;
				}
			}
		}

		internal static void LinearSortObjects(ArrayList list)
		{
			if(list.Count < 2)
				return;

			for(int i=0; i<list.Count-1;i++)
			{
				GeometricObject obj1 = list[i] as GeometricObject;
				for(int j=i+1;j<list.Count;j++)
				{
					GeometricObject obj2 = list[j] as GeometricObject;
					if(obj1.OrderingZ() > obj2.OrderingZ())
					{
						list[i] = obj2;
						list[j] = obj1;
						obj1 = obj2;
					}
				}
			}
		}

		#endregion

		#region --- Handling Target Area ---

		internal virtual void BeginRenderingTargetArea(TargetArea ta)
		{
			Rectangle rect = ta.EffectiveOuterTarget;
		}

		internal virtual void EndRenderingTargetArea(TargetArea ta)
		{
		}

		#endregion

		#region --- Rendering Text ---

		internal virtual void RenderText(ChartText text)
		{
			LabelStyle LS = text.LabelStyleRef;
			if (LS == null)
				LS = GetLabelStyle(text.LabelStyleName);
			if (LS == null)
			{
				SendMessage("Label style '" + text.LabelStyleName + "' not found");
				return;
			}
			Push(text);
			double angle = LS.Angle;
			Vector3D Vx = LS.HorizontalDirection;
			Vector3D Vy = LS.VerticalDirection;
			double c = Math.Cos(angle / 180.0 * Math.PI);
			double s = Math.Sin(angle / 180.0 * Math.PI);
			Vector3D Vxa = Vx * c + Vy * s;
			Vector3D Vya = Vy * c - Vx * s;
			DrawingBoard B = LS.Render(this,text.Mapping,text.Text, text.Location, Vxa, Vya);
			if (B != null)
				RenderDrawingBoard(B);
			Pop(typeof(ChartText));
		}
       #endregion

        #region --- Rendering 3D Objects ---

        internal virtual void RenderQuadrilateral (Quadrilateral q)
        {
            RenderGeneric(q);
        }

        internal virtual void RenderEllipse(Ellipse e)
        {
            RenderGeneric(e);
        }

        internal virtual void RenderBox(Box box)
        {
            RenderGeneric(box);
        }

        internal virtual void RenderPrism(Prism obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderCylinder(Cylinder obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderBlock(Block obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderPyramid(Pyramid obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderCone(Cone obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderEllipsoid(Ellipsoid obj)
		{
			RenderGeneric(obj);
		}

        internal virtual void RenderParaboloid(Paraboloid obj)
        {
            RenderGeneric(obj);
        }

        internal virtual void RenderPieSegment(PieSegment obj)
        {
            RenderGeneric(obj);
        }

		internal virtual void RenderTorusSegment(TorusSegment obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderRadialStrip(RadialStrip obj)
		{
			RenderGeneric(obj);
		}

        #endregion

        #region --- Rendering 2D Objects in DrawingBoard ---

        internal virtual void RenderDrawingBoard(DrawingBoard obj)
        {
			if(obj == null)
				return;
			mapping = obj.Mapping;

			obj.RenderContents(computingPosition);
			RenderGeneric(obj);
			if(computingPosition)
			{
				Rectangle r = obj.GetPositionRectangle();
				x0p = Math.Min(x0p,r.X);
				y0p = Math.Min(y0p,r.Y);
				x1p = Math.Max(x1p,r.X+r.Width);
				y1p = Math.Max(y1p,r.Y+r.Height);
			}
			FinishDrawingBoardRendering(obj);
        }

		internal virtual void FinishDrawingBoardRendering(DrawingBoard obj)
		{
			//RenderGeneric(obj);
		}

		internal virtual void RenderArea(ChartArea obj)
		{
			RenderGeneric(obj);
		}

        internal virtual void Render2DArea(Chart2DArea obj)
        {
            RenderGeneric(obj);
        }
        
        #endregion

        #region --- Rendering Wall ---

        internal virtual void RenderWall(Wall wall)
        {
            RenderGeneric(wall);
        }
        #endregion

        #region --- Rendering Line ---

        internal virtual void RenderLine(ChartLine line)
        {
            RenderLine(GetLineStyle(line.LineStyle), line);
        }

        internal virtual void RenderLine(LineStyle style, ChartLine line)
        {
            if (style.GetType() == typeof(NoLineStyle))
                return;

			Push(line);
			if (style.GetType() == typeof(BlockLineStyle))
                RenderBlockLine(style as BlockLineStyle, line);
            else if (style.GetType() == typeof(DashLineStyle))
                RenderDashLine(style as DashLineStyle, line);
            else if (style.GetType() == typeof(DotLineStyle))
                RenderDotLine(style as DotLineStyle, line);
            else if (style.GetType() == typeof(FlatLineStyle))
                RenderFlatLine(style as FlatLineStyle, line);
            else if (style.GetType() == typeof(MultiLineStyle))
                RenderMultiLine(style as MultiLineStyle, line);
            else if (style.GetType() == typeof(PipeLineStyle))
                RenderPipeLine(style as PipeLineStyle, line);
            else if (style.GetType() == typeof(StripLineStyle))
                RenderStripLine(style as StripLineStyle, line);
            else if (style.GetType() == typeof(ThickLineStyle))
                RenderThickLine(style as ThickLineStyle, line);
			Pop(line.GetType());
		}

        internal virtual void RenderBlockLine(BlockLineStyle style, ChartLine line)
        {
        }

        internal virtual void RenderDashLine(DashLineStyle style, ChartLine line)
        {
            int xLine = 0;
            DashLineStyleSegmentCollection m_lineSegments = style.Segments;

			string lineStyleName = m_lineSegments[xLine].LineStyleName;
			//lineStyleName = OverrideDashLineStyleName(lineStyleName);
            ChartLine LS = new ChartLine(lineStyleName, line.GetP0(), line.GetAx(), line.GetAy());
			LS.Parent = line;
            LS.AddPoint(line.Points[0].X, line.Points[0].Y);
            double dToCollect = m_lineSegments[xLine].Length * line.Mapping.FromPointToWorld;
            int i = 1;
            double xLast = line.Points[0].X;
            double yLast = line.Points[0].Y;

			GeometricObject lineSegments = new GeometricObject();
			lineSegments.Parent = line;

            while (i < line.Nt)
            {
                double dx = line.Points[i].X - xLast;
                double dy = line.Points[i].Y - yLast;
                double dd = Math.Sqrt(dx * dx + dy * dy);
                bool tryAgain = true;
                while (tryAgain)
                {
                    if (dd < dToCollect)
                    {
                        LS.AddPoint(line.Points[i].X, line.Points[i].Y);
                        dToCollect -= dd;
                        tryAgain = false;
                        xLast = line.Points[i].X;
                        yLast = line.Points[i].Y;
                    }
                    else
                    {
                        double a = dToCollect / dd;
                        float xx = (float)(a * line.Points[i].X + (1 - a) * xLast);
                        float yy = (float)(a * line.Points[i].Y + (1 - a) * yLast);
                        LS.AddPoint(xx, yy);
                        LS.ChartColor = line.ChartColor;
                        if (m_lineSegments[xLine].LineStyleName != "NoLine")
							lineSegments.Add(LS);
						// Start new line
                        xLine = (xLine + 1) % m_lineSegments.Count;
						lineStyleName = m_lineSegments[xLine].LineStyleName;
                        LS = new ChartLine(lineStyleName, line.GetP0(), line.GetAx(), line.GetAy());
						LS.Parent = line;
                        LS.AddPoint(xx, yy);
                        dToCollect = m_lineSegments[xLine].Length * line.Mapping.FromPointToWorld;
                        dx = xx - xLast;
                        dy = yy - yLast;
                        dd -= Math.Sqrt(dx * dx + dy * dy);
                        xLast = xx;
                        yLast = yy;
                    }
                }
                i++;
            }
            if (m_lineSegments[xLine].LineStyleName != "NoLine")
            {
                LS.ChartColor = line.ChartColor;
				lineSegments.Add(LS);
            }
			RenderGeneric(lineSegments);
        }

		internal virtual string OverrideDashLineStyleName(string lineStyleName)
		{
			// By default - don't override:
			return lineStyleName;
		}

        internal virtual void RenderDotLine(DotLineStyle style, ChartLine line)
        {
            // Adjusting the distance
            double effectiveWidth = style.Width * line.Mapping.FromPointToWorld;
            double effectiveDistance = effectiveWidth * style.RelativeDistance / 2;
            double radius = effectiveWidth / 2;

            Vector3D P = line.Point(0);
            int i = 1;
            double d, dprev = 0;
            Vector3D T, Tprev = P;
			GeometricObject dots = new GeometricObject();
			dots.Parent = line;
			while (i < line.Nt)
			{
				bool tryAgain = true;
				while (tryAgain)
				{
					T = line.Point(i);
					d = (T - P).Abs;
					if (d >= effectiveDistance)
					{
						double a = (effectiveDistance - dprev) / (d - dprev);
						P = T * a + Tprev * (1 - a);
						dots.Add(GetDotObjectForDotLine(P,radius,line.ChartColor));
						dprev = 0;
						Tprev = P;
						effectiveDistance = effectiveWidth * style.RelativeDistance;
					}
					else
					{
						dprev = d;
						Tprev = T;
						tryAgain = false;
					}
				}
				i++;
			}
			RenderGeneric(dots);
        }

        internal virtual void RenderFlatLine(FlatLineStyle style, ChartLine line)
        {
        }

        internal virtual void RenderMultiLine(MultiLineStyle style, ChartLine line)
        {
            MultiLineStyleCollection lineStyles = style.LineStyles;
            for (int i = 0; i < lineStyles.Count; i++)
            {
                string styleName = lineStyles[i].LineStyleName;
                if (styleName != "NoLine")
                {
                    LineStyle sstyle = GetLineStyle(styleName);
                    if (sstyle == null)
                        throw new Exception("Linestyle '" + styleName + "' used un multiline style '" + style.Name + "' does not exist");
                    RenderLine(sstyle, line);
                }
            }
        }

        internal virtual void RenderStripLine(StripLineStyle style, ChartLine line)
        {
        }

        internal virtual void RenderPipeLine(PipeLineStyle style, ChartLine line)
        {
        }

        internal virtual void RenderThickLine(ThickLineStyle style, ChartLine line)
        {
        }

        #endregion

        #region --- Rendering Marker ---

        private ChartColor MarkerGetColor(Marker marker)
        {
            ChartColor surface;
            if (marker.TakeColorFromStyle)
                surface = GetMarkerStyle(marker.StyleName).ChartColor;
            else
                surface = marker.ChartColor;
            return surface;
        }

        internal virtual void RenderMarker(Marker marker)
		{
            Mapping map = marker.Mapping;
            MarkerStyle ms = GetMarkerStyle(marker.StyleName);
            if (ms == null)
                return;

			Push(marker);
            switch (ms.MarkerKind)
            {
                case MarkerKind.Block:
                    MarkerRenderPolygonal(marker, xBlock, yBlock, fBlock);
                    break;
				case MarkerKind.Bubble:
				{
					if(ms.MarkerSize.IsNull)
					{
						ms = ms.Clone() as MarkerStyle;
						ms.MarkerSize = new Vector3D(10,10,10);
					}
					MarkerRenderBubble(marker,ms,MarkerGetColor(marker));
				}
					break;
                case MarkerKind.Circle:
                    MarkerRenderCircle(marker);
                    break;
                case MarkerKind.Diamond:
                    MarkerRenderPolygonal(marker, xDiam, yDiam, fDiam);
                    break;
                case MarkerKind.Triangle:
                    MarkerRenderPolygonal(marker, xTria, yTria, fTria);
                    break;
                case MarkerKind.InvertedTriangle:
                    MarkerRenderPolygonal(marker, xITria, yITria, fITria);
                    break;
                case MarkerKind.LeftTriangle:
                    MarkerRenderPolygonal(marker, xLTria, yLTria, fLTria);
                    break;
                case MarkerKind.RightTriangle:
                    MarkerRenderPolygonal(marker, xRTria, yRTria, fRTria);
                    break;
                case MarkerKind.Cross:
                    MarkerRenderPolygonal(marker, xCross, yCross, fCross);
                    break;
                case MarkerKind.XShape:
                    MarkerRenderXShape(marker);
                    break;
                case MarkerKind.ArrowE:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowW:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowN:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowS:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowNE:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowNW:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowSE:
                    MarkerRenderArrow(marker);
                    break;
                case MarkerKind.ArrowSW:
                    MarkerRenderArrow(marker);
                    break;

                default:
                    break;
            }
			Pop(marker.GetType());
		}

        private void MarkerRenderPolygonal(Marker marker, float[] xx, float[] yy, float f)
        {
            if (!marker.IsTwoDimensional)
                MarkerRenderPolygonal3D(marker, xx, yy, f);
            MarkerRenderPolygonal2D(marker,xx, yy, f);
        }

        private void MarkerRenderPolygonal3D(Marker marker, float[] xx, float[] yy, float f)
        {
            MarkerStyle ms = GetMarkerStyle(marker.StyleName);
            double size = marker.Size;
            float scaleFactor = (float)(f * size * marker.FromPointToWorld);

            float[] x = new float[xx.Length];
            float[] y = new float[xx.Length];
            for (int i = 0; i < xx.Length; i++)
            {
                x[i] = xx[i] * scaleFactor * (float)(ms.MarkerSize.X);
                y[i] = yy[i] * scaleFactor * (float)(ms.MarkerSize.Y);
            }
			
			// Change orientation if negative
			double p = 0;
			for(int i=0; i<xx.Length; i++)
			{
				int j = (i+1)%xx.Length;
				p += (x[j]-x[i])*(y[j]+y[i]);
			}
			CoordinateSystemBox CSB = marker.Owning(typeof(CoordinateSystemBox)) as CoordinateSystemBox;
			CoordinateSystem CS = CSB.CoordinateSystem();
			bool changeOrientation = p < 0 ;//&& CS.IsPositive || p > 0 && !CS.IsPositive;
			
			if(changeOrientation)
			{
				int j = xx.Length;
				for(int i=0; i<xx.Length/2; i++)
				{
					j--;
					float aa = x[i];
					x[i] = x[j];
					x[j] = aa;
					aa = y[i];
					y[i] = y[j];
					y[j] = aa;
				}
			}

            Vector3D vx = marker.Vx;
            Vector3D vy = marker.Vy;
			Vector3D vz = vx.CrossProduct(vy);
			double h = marker.Height;
			if(vz.X + vz.Y + vz.Z < 0)
			{
				h = -h;
			}
            Vector3D loc = marker.Location + vz * h * 0.5;

			Push(marker);
            RenderPath(marker.Mapping,loc, vx, vy, x,y, MarkerGetColor(marker), null);
            Wall wall = new Wall(loc, vx, vy, x, y, h, MarkerGetColor(marker));
            RenderWall(wall);
			Pop(marker.GetType());
        }

        private void MarkerRenderPolygonal2D(Marker marker, float[] xx, float[] yy, float f)
        {
            MarkerStyle ms = GetMarkerStyle(marker.StyleName);
            float scaleFactor = (float)(f * marker.Size * marker.FromPointToWorld);

            PointF[] points = new PointF[xx.Length];
            bool toInvert = false;
            for (int i = 0; i < xx.Length; i++)
            {
                if (toInvert)
                    points[xx.Length - i - 1] = new PointF(xx[i] * scaleFactor * (float)(ms.MarkerSize.X), yy[i] * scaleFactor * (float)(ms.MarkerSize.Y));
                else
                    points[i] = new PointF(xx[i] * scaleFactor * (float)(ms.MarkerSize.X), yy[i] * scaleFactor * (float)(ms.MarkerSize.Y));
            }
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(points);
            LineStyle2D lineStyle = new LineStyle2D("test", 1, ms.BorderColor);
            if (marker.IsTwoDimensional)
                lineStyle.ShadeWidth = ms.ShadeWidth;
            else
                lineStyle.ShadeWidth = 0;
            Vector3D vx = marker.Vx;
            Vector3D vy = marker.Vy;
            Vector3D vz = vx.CrossProduct(vy).Unit();
            Vector3D loc = marker.Location + vz * marker.Height * 0.5;

            RenderPath(marker.Mapping, loc, vx, vy, points, MarkerGetColor(marker), lineStyle);
        }

        private void MarkerRenderCircle(Marker marker)
        {
            int n = 20;
            float[] x = new float[21], y = new float[21];
            double da = 2 * Math.PI / n;
            for (int i = 0; i <= n; i++)
            {
                double a = i * da;
                x[i] = (float)Math.Cos(a);
                y[i] = (float)Math.Sin(a);
            }
            MarkerRenderPolygonal(marker, x, y, 0.5f);
        }

        private void MarkerRenderXShape(Marker marker)
        {
            float[] x = new float[13], y = new float[13];
            for (int i = 0; i < 13; i++)
            {
                x[i] = xCross[i] * 0.71f - yCross[i] * 0.71f;
                y[i] = xCross[i] * 0.71f + yCross[i] * 0.71f;
            }
            MarkerRenderPolygonal(marker, x, y, fCross);
        }

        private void MarkerRenderArrow(Marker marker)
        {
            MarkerStyle ms = GetMarkerStyle(marker.StyleName);
            const int n = 8;
            float[] x = new float[n], y = new float[n];
            double angle = 0;
            switch (ms.MarkerKind)
            {
                case MarkerKind.ArrowE: angle = 0; break;
                case MarkerKind.ArrowN: angle = 90; break;
                case MarkerKind.ArrowW: angle = 180; break;
                case MarkerKind.ArrowS: angle = 270; break;
                case MarkerKind.ArrowNE: angle = 45; break;
                case MarkerKind.ArrowNW: angle = 135; break;
                case MarkerKind.ArrowSE: angle = 315; break;
                case MarkerKind.ArrowSW: angle = 225; break;
                default: angle = 0; break;
            }
            angle = angle * Math.PI / 180;
            float ca = (float)Math.Cos(angle);
            float sa = (float)Math.Sin(angle);
            for (int i = 0; i < n; i++)
            {
                x[i] = xArrow[i] * ca - yArrow[i] * sa;
                y[i] = xArrow[i] * sa + yArrow[i] * ca;
            }
            MarkerRenderPolygonal(marker, x, y, fArrow);
        }

		internal virtual void MarkerRenderBubble(Marker marker, MarkerStyle ms, ChartColor color)
		{ }

        #endregion

		#region --- Rendering Polygons ---
		internal virtual void RenderPolygon(Polygon pol)
		{
			RenderGeneric(pol);
		}
		#endregion

		#region --- Rendering 2D Objects: Text and Line ---
		
		internal virtual void Render2DText(Chart2DText text2D)
		{
			
		}
		
		internal virtual void Render2DLine(Chart2DLine line2D)
		{
			
		}

		#endregion

        #endregion

        #region --- Helper Functions ---

        internal virtual GeometricObject GetDotObjectForDotLine(Vector3D center, double radius, ChartColor color)
        {
            // Override this function to create dot object for dot-line type
            return null;
        }

        internal void RenderPath(Mapping map, Vector3D P, Vector3D Vx, Vector3D Vy, float[] x, float[] y,
            ChartColor surface, LineStyle2D lineStyle2D)
        {
            PointF[] points = new PointF[x.Length];
            for (int i = 0; i < x.Length; i++)
                points[i] = new PointF(x[i], y[i]);
            RenderPath(map,P, Vx, Vy, points, surface, lineStyle2D);
        }

        internal void RenderPath(Mapping map, Vector3D P, Vector3D Vx, Vector3D Vy, GraphicsPath path,
            ChartColor surface, LineStyle2D lineStyle2D)
        {
            RenderPath(map,P, Vx, Vy, path.PathData.Points, surface, lineStyle2D);
        }

        internal void RenderPath(Mapping map, Vector3D P, Vector3D Vx, Vector3D Vy, PointF[] points,
            ChartColor surface, LineStyle2D lineStyle2D)
        {
            Vector3D Vxe = Vx.Unit();
            Vector3D Vye = Vy.Unit();

            int N = points.Length;
            double xMin = double.MaxValue;
            double xMax = double.MinValue;
            double yMin = double.MaxValue;
            double yMax = double.MinValue;
            for (int i = 0; i < N; i++)
            {
                xMin = Math.Min(xMin, points[i].X);
                xMax = Math.Max(xMax, points[i].X);
                yMin = Math.Min(yMin, points[i].Y);
                yMax = Math.Max(yMax, points[i].Y);
            }
            double dx = (xMax - xMin) / 10;
            double dy = (yMax - yMin) / 10;
            xMin -= dx;
            xMax += dx;
            yMin -= dy;
            yMax += dy;

            Vector3D V0 = P + Vxe * xMin + Vye * yMin;
            Vector3D Dx = Vxe * (xMax - xMin);
            Vector3D Dy = Vye * (yMax - yMin);
            DrawingBoard DB = CreateDrawingBoard(V0, Dx, Dy);
            DB.Parent = Top;
			DB.PrepareToRenderContents();

            for (int i = 0; i < N; i++)
            {
                points[i].X -= (float)xMin;
                points[i].Y -= (float)yMin;
            }

            GraphicsPath path2 = new GraphicsPath();
            path2.AddPolygon(points);

            if (lineStyle2D != null)
                DB.RenderPath(lineStyle2D, path2, false, true,false);
            Brush brush = new SolidBrush(surface.Color);
            DB.RenderFillPath(brush, path2,false,false);
            if (lineStyle2D != null)
                DB.RenderPath(lineStyle2D, path2, true, false, false);
            brush.Dispose();
            path2.Dispose();
            FinishDrawingBoardRendering(DB);
            DB.Dispose();
        }

        #endregion
  
		#region --- Chart GE Hierarchy ---

		internal virtual void RenderTargetAreaBox(TargetAreaBox obj)
		{
			TargetArea ta = obj.Tag as TargetArea;
			// This is called only once on the topmost target area box.
			if(ta.OwningTargetArea == null)
				AdjustTargetAreaMappings(obj);
			RenderGeneric(obj);
		}

		private void AdjustTargetAreaMappings(GeometricObject obj)
		{
			// The adjustments are propagated to the children nodes.
			if(obj.SubObjects == null)
				return;
			for(int i=0; i<obj.SubObjects.Count; i++)
				AdjustTargetAreaMappings(obj.SubObjects[i] as GeometricObject);
			// Parent area is adjusted AFTER the children modes
			TargetAreaBox tabx = obj as TargetAreaBox;
			if(tabx != null)
				tabx.AdjustMapping();
		}

		internal virtual void RenderCoordinateSystemBox(CoordinateSystemBox obj)
		{
			RenderObject(obj.CoordinatePlanes);
			RenderObject(obj.SubSystems);
			RenderObject(obj.Interior);
		}

		internal virtual void RenderPieDoughnutBox(PieDoughnutBox obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderRadarBox(RadarBox obj)
		{
			renderInZOrder = true;
			RenderGeneric(obj);
		}

		internal virtual void RenderSectionBox(SectionBox obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderColumnBox(ColumnBox obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderSubColumnBox(SubColumnBox obj)
		{
			RenderGeneric(obj);
		}

		internal virtual void RenderCoordinatePlaneBox(CoordinatePlaneBox obj)
		{
			RenderGeneric(obj);
		}


		#endregion

		#region --- Object Tracking ---
		internal virtual void GetObjectTrackingInfo(out int[,] matrix,  out ArrayList objects)
		{
			matrix = null;
			objects = null;
		}

		#endregion
	}

}
