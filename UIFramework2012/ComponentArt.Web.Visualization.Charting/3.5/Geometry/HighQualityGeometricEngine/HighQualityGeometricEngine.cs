using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

using ComponentArt.Web.Visualization.Charting.Shader;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class ConvertToCore
	{
		public static Shader.ChartColor Color(ChartColor cColor)
		{
			return new Shader.ChartColor(cColor.Alpha,cColor.Red,cColor.Green,cColor.Blue,cColor.Reflection,cColor.LogPhong);
		}
	}
}

namespace ComponentArt.Web.Visualization.Charting.Geometry.HighQualityRendering
{
    /// <summary>
    /// Geometric engine implemetation based on concept of 3D bitmap with sub-pixel division.
    /// </summary>
    internal class HighQualityGeometricEngine : GeometricEngine
    {
        private RenderingEngine engine;
        private Bitmap backgroundImage;
		private ArrayList flatObjects = new ArrayList();

        internal HighQualityGeometricEngine() { }
        internal HighQualityGeometricEngine(ChartBase chart)
            : base(chart)
        {
            engine = new RenderingEngine();
        }

		#region --- Features Supported ---
				
		internal override bool SupportsFeature(string featureName)
		{
			if(featureName == "VariablePieHeight")
				return true;
			if(featureName == "PieLift")
				return true;
			return base.SupportsFeature(featureName);
		}

		#endregion

		internal override void SetMapping(Mapping map)
		{
			base.SetMapping(map);
			Engine.Mapping = map.ToCoreMapping();
		}

        internal override void SetLights(LightCollection lights)
        {
            base.SetLights(lights);
        }

        internal override void SetBackground(Bitmap backgroundImage)
        {
            base.SetBackground(backgroundImage);
            this.backgroundImage = backgroundImage;
        }

        internal override void SetRenderingPrecisionPxl(double renderingPrecision)
        {
            Engine.RenderingPrecisionPxl = renderingPrecision;
        }

        internal override void SetPositionComputingMode(bool computingPosition)
        {
            engine.PositionTestRun = computingPosition;
        }

        internal override Rectangle GetPositionRectangle()
        {
            return engine.PositionRectangle;
        }

        private RenderingEngine Engine { get { return engine; } }

		internal override void GetObjectTrackingInfo(out int[,] indexMatrix, out ArrayList objects)
		{
			engine.GetObjectTrackingInfo(out indexMatrix,out objects);
		}
		
		internal override ObjectMapper GetObjectMapper()
		{
			return new ObjectMapper(this);
		}
		
		#region --- 2D Drawing in drawing board ---
		internal override DrawingBoard CreateDrawingBoard()
		{
			DrawingBoard db = new DrawingBoardHQ();
			db.GE = this;
			Add(db);
			return db;
		}

		internal override DrawingBoard CreateDrawingBoard(Vector3D V0, Vector3D Vx, Vector3D Vy)
		{
			DrawingBoard db = new DrawingBoardHQ(V0,Vx,Vy);
			db.GE = this;
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

        #region --- Render the Root Object ---

		internal override object Render()
		{
			// Prepare lights
		
			double ambientLightIntensity = 0;

			LightCollection effectiveLights;
			
			if(mapping.LightsOff)
			{
				effectiveLights = new LightCollection();
				effectiveLights.Add(new Light(1));
			}
			else
			{
				effectiveLights = this.lights;
			}

			double sIntensity = 0;
			int i;

			// Compute total light

			int nrl = 0;
			for(i=0; i<effectiveLights.Count; i++)
			{
				sIntensity += effectiveLights[i].Intensity;
				if(effectiveLights[i].IsAmbient)
					ambientLightIntensity += effectiveLights[i].Intensity;
				else
					nrl++;
			}

			ambientLightIntensity = ambientLightIntensity/sIntensity;

			// Initialize rendering lights
			RenderingLight[]renderingLights = new RenderingLight[nrl+1]; // we create an extra light to avoid empty array
			int n = 0;
			for(i = 0; i<effectiveLights.Count; i++)
			{
				if(!effectiveLights[i].IsAmbient)
				{
					renderingLights[n].Direction = base.mapping.MapDirectionUnit(effectiveLights[i].Direction);
					renderingLights[n].Intensity = effectiveLights[i].Intensity/sIntensity;
					renderingLights[n].CreateCoordinateSystem();
					n++;
				}
			}

			// Improve contribution and set light coordinates

			Vector3D s = Vector3D.Null;
			for(i=0;i<renderingLights.Length-1;i++)
			{
				Shader.Vector3D coreLD = renderingLights[i].Direction;
				Vector3D LD = new Vector3D(coreLD.X,coreLD.Y,coreLD.Z);
				s = s + LD *renderingLights[i].Intensity;
			}
			double sa = s.Abs;
			if(sa>0.0)
			{
				double F = (1.0 - ambientLightIntensity)/sa;
				for(i=0; i<renderingLights.Length-1; i++)
					renderingLights[i].Intensity *= F;
			}
	

			Engine.Mapping = mapping.ToCoreMapping();
			flatObjects.Clear();
			
			Engine.StartRendering(renderingLights,ambientLightIntensity);
			base.Render();
			object resultBitmap = Engine.EndRendering(backgroundImage);

			if(flatObjects.Count > 0)
			{
				Graphics g = Graphics.FromImage(resultBitmap as Image);
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				RenderFlatObjects(g);
				g.Dispose();
			}
			
			return resultBitmap;
		}

        internal override void RenderObject(GeometricObject obj)
        {
            if (obj != null && obj.Mapping != mapping && obj.Mapping != null)
                Engine.Mapping = obj.Mapping.ToCoreMapping();
			if(obj != null)
				engine.SetActiveObject(obj.Tag);
            base.RenderObject(obj);
        }

        #endregion

        #region --- Rendering 3D Objects ---
        internal override void RenderEllipse(Ellipse e)
        {
            EllipseRender(e.Center,e.Radius1,e.Radius2, e.Color, e.Nt);
        }

        internal void EllipseRender(Vector3D C, Vector3D R1, Vector3D R2, ChartColor surface, int n)
        {
            RenderingEngine e = engine;

            Vector3D P0 = Vector3D.Null, P1;
            if (n == 0)
                n = e.NumberOfApproximationPointsForEllipse(R1.Abs, R2.Abs);
            Vector3D N = R1.CrossProduct(R2);
            double a = 0;
            double da = 2 * Math.PI / n;
            for (int i = 0; i <= n; i++)
            {
                P1 = C + R1 * Math.Cos(a) + R2 * Math.Sin(a);
                if (i > 0)
                    RenderElement(C, P1, P0, N, N, N, surface, true);
                P0 = P1;
                a = a + da;
            }
        }

        internal override void RenderBox(Box box)
        {
            Vector3D P0 = box.P0;
            Vector3D P1 = box.P1;
            ChartColor surface = box.Color;

            RenderingEngine e = engine;
            double x0 = Math.Min(P0.X, P1.X);
            double x1 = Math.Max(P0.X, P1.X);
            double y0 = Math.Min(P0.Y, P1.Y);
            double y1 = Math.Max(P0.Y, P1.Y);
            double z0 = Math.Min(P0.Z, P1.Z);
            double z1 = Math.Max(P0.Z, P1.Z);

            // face
            Vector3D N = new Vector3D(0, 0, 1);
            RenderElement(new Vector3D(x0, y0, z1), new Vector3D(x0, y1, z1), new Vector3D(x1, y1, z1), N, N, N, surface, false);
            RenderElement(new Vector3D(x0, y0, z1), new Vector3D(x1, y1, z1), new Vector3D(x1, y0, z1), N, N, N, surface, false);
            // right side
            N = new Vector3D(1, 0, 0);
            RenderElement(new Vector3D(x1, y0, z1), new Vector3D(x1, y1, z1), new Vector3D(x1, y1, z0), N, N, N, surface, false);
            RenderElement(new Vector3D(x1, y0, z1), new Vector3D(x1, y1, z0), new Vector3D(x1, y0, z0), N, N, N, surface, false);
            // top side
            N = new Vector3D(0, 1, 0);
            RenderElement(new Vector3D(x0, y1, z0), new Vector3D(x1, y1, z0), new Vector3D(x1, y1, z1), N, N, N, surface, false);
            RenderElement(new Vector3D(x0, y1, z0), new Vector3D(x1, y1, z1), new Vector3D(x0, y1, z1), N, N, N, surface, false);
            // back
            N = new Vector3D(0, 0, -1);
            RenderElement(new Vector3D(x0, y1, z0), new Vector3D(x0, y0, z0), new Vector3D(x1, y1, z0), N, N, N, surface, false);
            RenderElement(new Vector3D(x1, y1, z0), new Vector3D(x0, y0, z0), new Vector3D(x1, y0, z0), N, N, N, surface, false);
            // left side
            N = new Vector3D(-1, 0, 0);
            RenderElement(new Vector3D(x0, y1, z1), new Vector3D(x0, y0, z1), new Vector3D(x0, y1, z0), N, N, N, surface, false);
            RenderElement(new Vector3D(x0, y1, z0), new Vector3D(x0, y0, z1), new Vector3D(x0, y0, z0), N, N, N, surface, false);
            // bottom side
            N = new Vector3D(0, -1, 0);
            RenderElement(new Vector3D(x1, y0, z0), new Vector3D(x0, y0, z0), new Vector3D(x1, y0, z1), N, N, N, surface, false);
            RenderElement(new Vector3D(x1, y0, z1), new Vector3D(x0, y0, z0), new Vector3D(x0, y0, z1), N, N, N, surface, false);
        }

		private void RenderElement(Vector3D P0, Vector3D P1, Vector3D P2, Vector3D N0, Vector3D N1, Vector3D N2, ChartColor cColor, bool removeBackTriangles)
		{
			Engine.RenderElement(P0,P1,P2, N0,N1,N2, ConvertToCore.Color(cColor), removeBackTriangles);
		}

		private void RenderBitmapTriangle(Vector3D P0W, Vector3D P1W, Vector3D P2W, 
			Bitmap bmp, int x0bmp, int y0bmp, double reflection, int phong, bool lightsOff)
		{
			Engine.RenderBitmapTriangle(P0W,P1W,P2W, bmp, x0bmp,y0bmp,reflection,phong,lightsOff);
		}

        internal override void RenderEllipsoid(Ellipsoid obj)
        {
			Push(obj);
            RenderParametricFacet(obj);
			Pop(obj.GetType());
		}
		
		internal override void MarkerRenderBubble(Marker marker, MarkerStyle ms, ChartColor color)
		{
			Ellipsoid ellipsoid = new Ellipsoid(//center, axis, height, axis2Length, alpha0, alpha1, theta0, theta1, surface);
				marker.Location,
				new Vector3D(ms.MarkerSize.X * marker.FromPointToWorld, 0, 0),
				new Vector3D(0, ms.MarkerSize.Y * marker.FromPointToWorld, 0),
				ms.MarkerSize.Z * marker.FromPointToWorld,
				color);
			Add(ellipsoid);
			RenderEllipsoid(ellipsoid);
		}


        internal override void RenderParaboloid(Paraboloid obj)
        {
			Push(obj);
			RenderParametricFacet(obj);
			Pop(obj.GetType());
		}

		internal override void RenderRadialStrip(RadialStrip rStrip)
		{
			Vector3D[] V00S = rStrip.InnerRing;
			Vector3D[] V01S = rStrip.OuterRing;
			Vector3D[] N0S = rStrip.Normal;
			ChartColor eSurface = rStrip.ChartColor;
			int nSeg = rStrip.Count;

			for (int ix=0; ix<nSeg; ix++)
			{
				int jx = (ix+1)%nSeg;
				Quadrilateral Q =  new Quadrilateral(V00S[ix],V01S[ix],V01S[jx],V00S[jx],N0S[ix],N0S[ix],N0S[jx],N0S[jx],eSurface);
				Q.Parent = rStrip;
				RenderQuadrilateral(Q);
			}

		}
 
        #region --- Cylinder ---

        internal override void RenderCylinder(Cylinder cylinder)
        {
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
            RenderingEngine e = this.Engine;
            double h = H.Abs;
            double rEdge = cylinder.EdgeRadius * this.mapping.FromPointToWorld;
            if (h > 0)
                rEdge = Math.Min(rEdge, h / 3);
            else
                return;
            Vector3D He = H / h;
            Vector3D P1 = C1 + He * rEdge;
            Vector3D P2 = C2 - He * rEdge;

            // Top torus segment
            TorusSegment TSTop = new TorusSegment(0.0, 2 * Math.PI, 0.0, 0.5 * Math.PI, P2, C2,
                R1, r2, rEdge, surface);
            TSTop.Parent = cylinder;
            if (rEdge > 0)
            {
                RenderParametricFacet(TSTop);
            }

            // Top ellipse
            double f = rEdge / R1.Abs;
            double r1e = R1.Abs * (1.0 - f);
            double r2e = R2.Abs * (1.0 - f);
            Vector3D R1e = R1.Unit() * r1e;
            Vector3D R2e = R2.Unit() * r2e;
            EllipseRender(C2, R1e, R2e, surface,TSTop.Nu);

            // Cylindric envelope
            CylindricSegment CS = new CylindricSegment(0.0, 2 * Math.PI, P1, P2, R1, r2, surface);
			TSTop.Parent = cylinder;
			CS.Nu = TSTop.Nu;
            RenderParametricFacet(CS);

            // Bottom torus segment
            if (rEdge > 0)
            {
                TorusSegment TSBott = new TorusSegment(0.0, 2 * Math.PI, -0.5 * Math.PI, 0.0, P1, P1 + He * rEdge,
                    R1, r2, rEdge, surface);
                TSBott.Parent = cylinder;
                RenderParametricFacet(TSBott);
            }

            // Bottom ellipse
            EllipseRender(C1, R2e, R1e, surface,TSTop.Nu);
        }

        #endregion

        #region --- Prism
        internal override void RenderPrism(Prism prism)
        {
            Vector3D H = prism.TopCenter - prism.BaseCenter;
            double h = H.Abs;
            Vector3D He = H / h;
            double startingAngle = -Math.PI * (0.5 - 1.0 / prism.NumberOfSides);
            double angleStep = 2 * Math.PI / prism.NumberOfSides;

            Vector3D R2 = H.CrossProduct(prism.BaseRadius);
            if (R2.IsNull)
                return;
            R2 = R2.Unit() * prism.OrthogonalRadius;
            double rEdge = prism.EdgeRadius * mapping.FromPointToWorld;
            rEdge = Math.Min(rEdge, h / 3);
            double f = rEdge / prism.BaseRadius.Abs / Math.Cos(angleStep / 2);
            double angle = startingAngle;
            for (int i = 0; i < prism.NumberOfSides; i++)
            {
                RenderPrismSide(prism,angle, angleStep, Engine, f, prism.BaseCenter, prism.TopCenter, prism.BaseRadius, R2, H, He, prism.ChartColor,
                    rEdge, h);
                angle += angleStep;
            }
        }

        private void RenderPrismSide(Prism prism, double angle, double angleStep, RenderingEngine e, double f,
            Vector3D C1, Vector3D C2, Vector3D R1, Vector3D R2, Vector3D H, Vector3D He, ChartColor surface,
            double rEdge, double h)
        {
            Vector3D D0 = R1 * Math.Cos(angle) + R2 * Math.Sin(angle);
            Vector3D D1 = R1 * Math.Cos(angle + angleStep) + R2 * Math.Sin(angle + angleStep);

            // 1. Render top triangle
            Vector3D A0 = C2 + D0 * (1.0 - f);
            Vector3D A1 = C2 + D1 * (1.0 - f);
            RenderElement(C2, A1, A0, H, H, H, surface, true);

            // 2. Upper edge cylindric segment
            Vector3D A0A1e = (A1 - A0).Unit();
            // Vector in base plane, normal to A0A1
            Vector3D Ne = (D0 - A0A1e * (A0A1e * D0)).Unit();
            Vector3D B0 = A0 - He * rEdge;
            Vector3D E0 = B0 + Ne * rEdge;
            Vector3D B1 = A1 - He * rEdge;
            Vector3D E1 = E0 + (A1 - A0);
            if (rEdge > 0)
            {
                CylindricSegment CS1 = new CylindricSegment(0, Math.PI / 2, B0, B1, A0 - B0, (E0 - B0).Abs, surface);
                CS1.Parent = prism;
                RenderParametricFacet(CS1);
            }
            // 3. The side
            Vector3D F0 = E0 - He * (h - 2 * rEdge);
            Vector3D F1 = E1 - He * (h - 2 * rEdge);
            RenderElement(E0, F1, F0, Ne, Ne, Ne, surface, true);
            RenderElement(E0, E1, F1, Ne, Ne, Ne, surface, true);

            // 4. Lower edge cylindric segment
            Vector3D G0 = F0 + (B0 - E0);
            Vector3D H0 = C1 + (A0 - C2);
            Vector3D G1 = G0 + (B1 - B0);
            if (rEdge > 0)
            {
                CylindricSegment CS2 = new CylindricSegment(0, Math.PI / 2, G0, G1, F0 - G0, (H0 - G0).Abs, surface);
                CS2.Parent = prism;
                CS2.Closed = false;
                RenderParametricFacet(CS2);
            }

            // 5. Verical edge cylindric segment
            Vector3D H1 = H0 + (B1 - B0);
            double f2 = f * Math.Cos(angleStep / 2);
            if (rEdge > 0)
            {
                CylindricSegment CS3 = new CylindricSegment(0, angleStep, G1, B1, F1 - G1, rEdge, surface);
                CS3.Parent = prism;
                RenderParametricFacet(CS3);
            }

            if (rEdge > 0)
            {
                // 6. Upper corner
                Ellipsoid El1 = new Ellipsoid(B1, E1 - B1, A1 - B1, rEdge, 0, angleStep, 0, Math.PI / 2, surface);
                El1.Parent = prism;
                RenderParametricFacet(El1);

                // 7. Lower corner
                Ellipsoid El2 = new Ellipsoid(G1, F1 - G1, H1 - G1, rEdge, -angleStep, 0, 0, Math.PI / 2, surface);
                El2.Parent = prism;
                RenderParametricFacet(El2);
            }

            // 8. Bottom triangle
            RenderElement(C1, H0, H1, -H, -H, -H, surface, true);
        }

        #endregion

        #region --- Pyramid ---
        internal override void RenderPyramid(Pyramid pyramid)
        {
            Vector3D vertex = pyramid.Vertex;
            Vector3D center = pyramid.BaseCenter;
            double normRadius = pyramid.OrthogonalRadius;
            Vector3D radius = pyramid.BaseRadius;
            double H1 = pyramid.RelativeH1;
            double H2 = pyramid.RelativeH2;
            int nSides = pyramid.NumberOfSides;
            ChartColor surface = pyramid.ChartColor;

            double startingAngle = -Math.PI * (0.5 - 1.0 / nSides);
            double angleStep = 2 * Math.PI / nSides;

            Vector3D radiusN = (vertex - center).CrossProduct(radius).Unit() * normRadius;
            Vector3D C1 = vertex * H1 + (1.0 - H1) * center;
            Vector3D C2 = vertex * H2 + (1.0 - H2) * center;
            Vector3D radius1 = radius * (1.0 - H1);
            Vector3D radius2 = radius * (1.0 - H2);
            Vector3D radiusN1 = radiusN * (1.0 - H1);
            Vector3D radiusN2 = radiusN * (1.0 - H2);
            Vector3D H = C2 - C1;

            // Rendering sides and bases

            int i;
            Vector3D P1 = Vector3D.Null, P2 = Vector3D.Null, T1, T2;
            double angle = startingAngle;

            for (i = 0; i <= nSides; i++)
            {
                T1 = C1 + radius1 * Math.Cos(angle) + radiusN1 * Math.Sin(angle);
                T2 = C2 + radius2 * Math.Cos(angle) + radiusN2 * Math.Sin(angle);
                if (i > 0)
                {
                    Vector3D N = (T2 - T1).CrossProduct(P1 - T1);
                    RenderElement(P1, P2, T2, N, N, N, surface, true);
                    RenderElement(P1, T2, T1, N, N, N, surface, true);
                    RenderElement(T2, P2, C2, H, H, H, surface, true);
                    RenderElement(P1, T1, C1, -H, -H, -H, surface, true);
                }
                P1 = T1;
                P2 = T2;
                angle += angleStep;
            }
        }
        #endregion

        #region --- Cone ---

        internal override void RenderCone(Cone cone)
        {
            Vector3D vertex = cone.Vertex;
            Vector3D center = cone.BaseCenter;
            Vector3D radius = cone.BaseRadius;
            double normRadius = cone.OrthogonalRadius;
            double H1 = cone.RelativeH1;
            double H2 = cone.RelativeH2;
            ChartColor surface = cone.ChartColor;

            RenderingEngine e = this.Engine;
            Vector3D radiusN = (vertex - center).CrossProduct(radius).Unit() * normRadius;
            Vector3D C1 = vertex * H1 + (1.0 - H1) * center;
            Vector3D C2 = vertex * H2 + (1.0 - H2) * center;
            Vector3D radius1 = radius * (1.0 - H1);
            Vector3D radius2 = radius * (1.0 - H2);
            Vector3D radiusN1 = radiusN * (1.0 - H1);
            Vector3D radiusN2 = radiusN * (1.0 - H2);

            e.ChartColor = ConvertToCore.Color(cone.ChartColor);

            int nPts = cone.NumberOfApproximationPointsForEllipse(radius.Abs, normRadius,RenderingPrecisionInPixelSize);

            Vector3D LB0 = Vector3D.Null, LB1, UB0 = Vector3D.Null, UB1, N0 = Vector3D.Null, N1;
            double r2Abs = radius2.Abs;
            double a = 0;
            double da = 2.0 * Math.PI / nPts;
            double c0, s0, c1, s1;
            for (int i = 0; i <= nPts; i++)
            {
                c1 = Math.Cos(a);
                s1 = Math.Sin(a);
                LB1 = C1 + radius1 * c1 + radiusN1 * s1;
                UB1 = C2 + radius2 * c1 + radiusN2 * s1;
                // Tangent vector at LB1
                Vector3D T = -radius1 * s1 + radiusN1 * c1;
                // Normal
                N1 = T.CrossProduct(UB1 - LB1);
                if (i > 0)
                {
                    if (r2Abs > 0)
                        RenderElement(LB0, UB0, UB1, N0, N0, N1, surface, true);
                    RenderElement(LB0, UB1, LB1, N0, N1, N1, surface, true);
                }
                UB0 = UB1;
                LB0 = LB1;
                N0 = N1;
                c0 = c1;
                s0 = s1;
                a += da;
            }
        }
        #endregion

        #region --- PieSegment ---

        internal override void RenderPieSegment(PieSegment seg)
        {
            RenderingEngine e = this.Engine;
            Vector3D C1 = seg.BaseCenter;
            Vector3D C2 = seg.TopCenter;
            double alpha0 = seg.Alpha0;
            double alpha1 = seg.Alpha1;
            Vector3D R1 = seg.BaseRadius;
            double innerRadius = seg.InnerRadius;
            double innerEdgeSmoothingRadius = seg.InnerEdgeRadius;
            double outerEdgeSmoothingRadius = seg.OuterEdgeRadius;
            ChartColor surface = seg.ChartColor;

            int i;

            Vector3D H = C2 - C1;
            double h = H.Abs;
            Vector3D He = H / h;

            // Correct values if needed

            if (outerEdgeSmoothingRadius > h)
                outerEdgeSmoothingRadius = h;
            if (innerEdgeSmoothingRadius > h)
                innerEdgeSmoothingRadius = h;
            double s = outerEdgeSmoothingRadius + innerEdgeSmoothingRadius;
            double f = (R1.Abs - innerRadius) / s;
            if (s < 0)
            {
                outerEdgeSmoothingRadius *= s;
                innerEdgeSmoothingRadius *= s;
            }

            // Outer torus center
            Vector3D TCOut = C2 - He * outerEdgeSmoothingRadius;
            // Inner torus center
            Vector3D TCIn = C2 - He * innerEdgeSmoothingRadius;
            // Inner radius vector
            Vector3D R1In = R1.Unit() * (innerRadius + 2 * innerEdgeSmoothingRadius);

            // Outer edge torus segment
            Geometry.TorusSegment TSOut = new Geometry.TorusSegment(alpha0, alpha1, 0, Math.PI / 2, TCOut, C2, R1, R1.Abs, outerEdgeSmoothingRadius, surface);
            TSOut.Parent = seg;
            TSOut.Nu = TSOut.GetNu(this);
            TSOut.Nv = TSOut.GetNv(this);
            RenderTorusSegment(TSOut);

            // Outer cylinder
            Vector3D P0 = Vector3D.Null, P1, P2, Q0 = Vector3D.Null, Q1;
            Vector3D N0 = Vector3D.Null, N1;
            Vector3D HCout = He * (H.Abs - outerEdgeSmoothingRadius);
            Vector3D HCin = He * (H.Abs - innerEdgeSmoothingRadius);
            for (i = 0; i <= TSOut.Nu; i++)
            {
                double u = i * TSOut.DU + TSOut.U0;
                P1 = TSOut.Point(u, TSOut.V0);
                Q1 = P1 - HCout;
                N1 = TSOut.Normal(u, TSOut.V0);
                if (i > 0)
                {
                    RenderElement(Q1, P0, P1, N1, N0, N1, surface, true);
                    RenderElement(Q1, Q0, P0, N1, N0, N0, surface, true);
                }
                P0 = P1;
                Q0 = Q1;
                N0 = N1;
            }

            Geometry.TorusSegment TSin = null;
            if (innerRadius > 0)
            {
                // Inner edge torus segment
                TSin = new Geometry.TorusSegment(alpha0, alpha1, Math.PI / 2, Math.PI, TCIn, C2, R1In, innerRadius + 2 * innerEdgeSmoothingRadius, innerEdgeSmoothingRadius, surface);
                TSin.Parent = seg;
                TSin.Nv = TSin.GetNv(this);
                TSin.Nu = TSOut.Nu;
                RenderTorusSegment(TSin);

                // Inner cylinder
                for (i = 0; i <= TSin.Nu; i++)
                {
                    double u = i * TSin.DU + TSin.U0;
                    P1 = TSin.Point(u, TSin.V1);
                    Q1 = P1 - HCin;
                    N1 = TSin.Normal(u, TSin.V1);
                    if (i > 0)
                    {
                        RenderElement(Q1, P1, P0, N1, N1, N0, surface, true);
                        RenderElement(Q1, P0, Q0, N1, N0, N0, surface, true);
                    }
                    P0 = P1;
                    Q0 = Q1;
                    N0 = N1;
                }

                // Top flat ring
                for (i = 0; i <= TSOut.Nu; i++)
                {
                    double u = i * TSOut.DU + TSOut.U0;
                    P1 = TSOut.Point(u, TSOut.V1);
                    Q1 = TSin.Point(u, TSin.V0);
                    if (i > 0)
                    {
                        RenderElement(Q1, P1, P0, H, H, H, surface, true);
                        RenderElement(Q1, P0, Q0, H, H, H, surface, true);
                    }
                    P0 = P1;
                    Q0 = Q1;
                }
            }
            else
            {
                // Top flat ring
                for (i = 0; i <= TSOut.Nu; i++)
                {
                    double u = i * TSOut.DU + TSOut.U0;
                    P1 = TSOut.Point(u, TSOut.V1);
                    if (i > 0)
                    {
                        RenderElement(C2, P1, P0, H, H, H, surface, true);
                    }
                    P0 = P1;
                }
            }

            // Side at U0

            double uu = TSOut.U0;
            P1 = TSOut.Point(uu, TSOut.V0);
            P0 = P1 - HCout;
            Vector3D N = Vector3D.Null;
            for (i = 1; i <= TSOut.Nv; i++)
            {
                double v = i * TSOut.DV + TSOut.V0;
                P2 = TSOut.Point(uu, v);
                if (i == 1)
                    N = H.CrossProduct(P2 - P1);
                RenderElement(P0, P2, P1, N, N, N, surface, true);
                P1 = P2;
            }
            if (innerRadius > 0)
            {
                for (i = 0; i <= TSin.Nv; i++)
                {
                    double v = i * TSin.DV + TSin.V0;
                    P2 = TSin.Point(uu, v);
                    RenderElement(P0, P2, P1, N, N, N, surface, true);
                    P1 = P2;
                }
                P2 = P1 - HCin;
                RenderElement(P0, P2, P1, N, N, N, surface, true);
            }
            else
            {
                P2 = C2;
                RenderElement(P0, P2, P1, N, N, N, surface, true);
                P1 = P2;
                P2 = C1;
                RenderElement(P0, P2, P1, N, N, N, surface, true);
            }

            // Side at U1

            uu = TSOut.U1;
            P1 = TSOut.Point(uu, TSOut.V0);
            P0 = P1 - HCout;
            for (i = 1; i <= TSOut.Nv; i++)
            {
                double v = i * TSOut.DV + TSOut.V0;
                P2 = TSOut.Point(uu, v);
                if (i == 1)
                    N = -H.CrossProduct(P2 - P1);
                RenderElement(P0, P1, P2, N, N, N, surface, true);
                P1 = P2;
            }
            if (innerRadius > 0)
            {
                for (i = 0; i <= TSin.Nv; i++)
                {
                    double v = i * TSin.DV + TSin.V0;
                    P2 = TSin.Point(uu, v);
                    RenderElement(P0, P1, P2, N, N, N, surface, true);
                    P1 = P2;
                }
                P2 = P1 - HCin;
                RenderElement(P0, P1, P2, N, N, N, surface, true);
            }
            else
            {
                P2 = C2;
                RenderElement(P0, P1, P2, N, N, N, surface, true);
                P1 = P2;
                P2 = C1;
                RenderElement(P0, P1, P2, N, N, N, surface, true);
            }

            // Bottom
            //HCout = He*H.Abs;
            Vector3D NBott = -He;
            for (i = 0; i <= TSOut.Nu; i++)
            {
                double u = i * TSOut.DU + TSOut.U0;
                P1 = TSOut.Point(u, TSOut.V0) - HCout;
                if (TSin != null)
                {
                    Q1 = TSin.Point(u, TSin.V1) - HCin;
                    if (i > 0)
                    {
                        RenderElement(Q1, P0, P1, NBott, NBott, NBott, surface, true);
                        RenderElement(Q1, Q0, P0, NBott, NBott, NBott, surface, true);
                    }
                    P0 = P1;
                    Q0 = Q1;
                }
                else
                {
                    Q1 = C2 - HCin;
                    if (i > 0)
                    {
                        RenderElement(Q1, P0, P1, NBott, NBott, NBott, surface, true);
                    }
                    P0 = P1;
                }
            }
        }

        internal override void RenderTorusSegment(TorusSegment tSeg)
        {
            RenderParametricFacet(tSeg);
        }


        #endregion

        #region ---2D and 3D Area ---

        internal override void RenderArea(ChartArea area)
        {
			Engine.SetActiveObject(area.Tag);
			Wall wall = new Wall(area.Origin, area.XDirection, area.YDirection, area.Lines, area.Height, area.WallColor);
			wall.Tag = area.Tag;
            area.Add(wall);
            base.RenderArea(area);

            Vector3D Xe = area.XDirection;
            Vector3D Ye = area.YDirection;
            Vector3D P = area.Origin;

            Vector3D Vz = Xe.CrossProduct(Ye);
            Vector3D VzInTarget = area.Mapping.Map(P + Vz) - area.Mapping.Map(P);
            Vz = VzInTarget.Z < 0 ? -Vz : Vz;

            AreaPartition AP = new AreaPartition();
            foreach(SimpleLine L in area.Lines)
                AP.Add(L);

            AP.StartSections();
            double x0, x1, s1y0, s1y1, s2y0, s2y1;
            bool positive;
            while (AP.GetNextSection(out x0, out x1, out s1y0, out s1y1, out s2y0, out s2y1, out positive))
            {
                Vector3D P0 = P + Xe * x0 + Ye * s1y0;
                Vector3D P1 = P + Xe * x1 + Ye * s2y1;
                Vector3D P2 = P + Xe * x0 + Ye * s2y0;

                ChartColor color = area.BodyColor;
                RenderElement(P1, P0, P2, Vz, Vz, Vz, color,false);

                P0 = P + Xe * x0 + Ye * s1y0;
                P1 = P + Xe * x1 + Ye * s1y1;
                P2 = P + Xe * x1 + Ye * s2y1;
                RenderElement(P1, P0, P2, Vz, Vz, Vz, color, false);
            }
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

        #endregion

        #region --- Parametric facet ---
        //    1 +----+ 2
        //      |   /|
        //      |  / |
        //      | /  |
        //      |/   |
        //    0 +----+ 3

        protected void RenderQuadrilateralWithNormals(
            Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3,
            Vector3D n0, Vector3D n1, Vector3D n2, Vector3D n3,
            ChartColor surface)
        {
            RenderElement(p0, p1, p2, n0, n1, n2, surface, false);
            RenderElement(p2,p3,p0, n2,n3,n0, surface, false);
        }

		
		protected void RenderParametricFacet(ParametricFacet facet)
		{
			if (facet.Nu == 0)
				facet.Nu = facet.GetNu(this);
			if (facet.Nv == 0)
				facet.Nv = facet.GetNv(this);

			int Nu = facet.Nu;
			int Nv = facet.Nv;
			if(Nu < 1 || Nv < 1)
				return;

			Vector3D[] p0 = new Vector3D[Nv+1];
			Vector3D[] p1 = new Vector3D[Nv+1];
			Vector3D[] n0 = new Vector3D[Nv+1];
			Vector3D[] n1 = new Vector3D[Nv+1];

			double du = facet.DU;
			double dv = facet.DV;

			double u, v;
			u = facet.U0;

			for(int i=0;i<=Nu;i++)
			{
				int j;
				v = facet.V0;
				for(j=0;j<=Nv; j++)
				{
					p1[j] = facet.Point(u,v);
					n1[j] = facet.Normal(u, v);
					v += dv;
				}
				u += du;

				if(i > 0)
				{
					for(j=1;j<=Nv;j++)
					{
						//  j,0 +----+ j,1
						//      |   /|
						//      |  / |
						//      | /  |
						//      |/   |
						//j-1,0 +----+ j-1,1
						this.RenderQuadrilateralWithNormals(
							p0[j - 1], p0[j], p1[j], p1[j - 1],
							n0[j - 1], n0[j], n1[j], n1[j - 1],
							facet.ChartColor);
					}
				}

				for(j=0;j<=Nv; j++)
				{
					p0[j] = p1[j];
					n0[j] = n1[j];
					v += dv;
				}
			}
		}

        #endregion

        #region --- Rendering Quadrilateral ---

        internal override void RenderQuadrilateral(Quadrilateral q)
        {
            RenderQuadrilateralWithNormals(q.P00, q.P01, q.P11, q.P10, q.N00, q.N01, q.N11, q.N10, q.Color);
        }

        #endregion

        #region --- Rendering Wall ---

        internal override void RenderWall(Wall wall)
        {
            Vector3D P = wall.Origin;
            Vector3D Vx = wall.XDirection;
            Vector3D Vy = wall.YDirection;
            double height = wall.Height;
            ChartColor frontSurface = wall.Color;

            Vector3D Vxe = Vx.Unit();
            Vector3D Vye = Vy.Unit();
            Vector3D Vh = Vye.CrossProduct(Vxe) * height;


            foreach (SimpleLine line in wall.Lines)
            {
                double[] x, y;
                bool[] smooth;
                line.GetPoints(out x, out y, out smooth);
                // Make sure that points are different
                SimpleLine last = line.LastPoint();
                bool closed = last.next == null && last.x == line.x && last.y == line.y || last.next == line;
                int i, n = 0;
                for (i = 1; i < x.Length; i++)
                {
                    if (x[i] != x[n] || y[i] != y[n])
                    {
                        n++;
                        x[n] = x[i];
                        y[n] = y[i];
                        smooth[n] = smooth[n] && smooth[i];
                    }
                }
                while (n > 0 && x[n] == x[0] && y[n] == y[0])
                {
                    smooth[0] = smooth[0] && smooth[n];
                    n--;
                }
                n++;
                int numberOfRectangles = (closed ? n : n - 1);
                for (i = 0; i < numberOfRectangles; i++)
                {
                    // Rendering segment between points i and i+1

                    int ip = (i + n - 1) % n; // +n is needed when i=0
                    int inxt = (i + 1) % n;
                    int inxt1 = (i + 2) % n;
                    Vector3D P0 = P + Vxe * x[i] + Vye * y[i];
                    Vector3D P1 = P + Vxe * x[inxt] + Vye * y[inxt];
                    Vector3D Q0 = P0 + Vh;
                    Vector3D Q1 = P1 + Vh;
                    // Normals without smoothing
                    Vector3D N0 = N0 = (P1 - P0).CrossProduct(Vh), N1 = N0;

                    if (smooth[i] && (i > 0 || closed)) // we dont smooth @ point[0] unless closed
                    {
                        Vector3D Pp = P + Vxe * x[ip] + Vye * y[ip];
                        N0 = (P0 - Pp).CrossProduct(Vh).Unit() + (P1 - P0).CrossProduct(Vh).Unit();
                    }
                    if (smooth[inxt] && (i < n - 1 || closed))
                    {
                        Vector3D Pnxt1 = P + Vxe * x[inxt1] + Vye * y[inxt1];
                        N1 = (Pnxt1 - P1).CrossProduct(Vh).Unit() + (P1 - P0).CrossProduct(Vh).Unit();
                    }

                    RenderElement(P0, Q0, Q1, N0, N0, N1, frontSurface, false);
                    RenderElement(P0, Q1, P1, N0, N1, N1, frontSurface, false);
                    // Opposite direction
                    RenderElement(P0, Q1, Q0, -N0, -N1, -N0, frontSurface, false);
                    RenderElement(P0, P1, Q1, -N0, -N1, -N1, frontSurface, false);
                }
            }
        }
        #endregion

        #region --- Rendering Lines ---

        internal override void RenderBlockLine(BlockLineStyle style, ChartLine line)
        {
            RenderThickLine(style, line);
        }

        internal override void RenderFlatLine(FlatLineStyle style, ChartLine line)
        {
            RenderThickLine(style, line);
        }

        internal override void RenderPipeLine(PipeLineStyle style, ChartLine line)
        {
            RenderThickLine(style, line);
        }

        internal override void RenderStripLine(StripLineStyle style, ChartLine line)
        {
            RenderThickLine(style, line);
        }

        internal override void RenderThickLine(ThickLineStyle style, ChartLine line)
        {
            Generatrice gen = null;
            double rw = Math.Min(0.5, style.WidthEdgeRadius);
            double rh = Math.Min(0.5, style.HeightEdgeRadius);
            // Effective width and height
            double eWidth = style.Width * line.Mapping.FromPointToWorld;
            double eHeight = style.Height * line.Mapping.FromPointToWorld;

            double aRW = rw * eWidth;
            double aRH = rh * eHeight;
            if (style.EqualRadii)
            {
                double min = Math.Min(aRW, aRH);
                aRW = min;
                aRH = min;
            }
            if (rh <= 0 || rw <= 0)
                gen = Generatrice.CreateBlock(eHeight, eWidth, style.ChartColor);
            else
                gen = Generatrice.CreateRoundBlock(eWidth, eHeight, aRW, aRH, style.ChartColor);

            line.JoinStyle = style.JoinStyle;
            line.Render(gen, Engine);
        }

        #endregion

        #region --- Rendering Text ---

        #endregion

        #region --- Rendering Marker ---

        #endregion

        #endregion

        #region --- Rendering 2D Objects in DrawingBoard ---

        internal override void FinishDrawingBoardRendering(DrawingBoard dbb)
        {
			DrawingBoardHQ db = dbb as DrawingBoardHQ;
			if(db == null)
				return;

            RenderingEngine e = this.Engine;
            Vector3D v0 = db.V0;
            Vector3D v1 = db.V1;
            Vector3D v2 = db.V2;
            Vector3D v3 = db.V3;
            Bitmap bmp = db.BMP;
            int ix0 = db.Ix0;
            int iy0 = db.Iy0;
            double reflection = db.Reflection;
            int logPhong = db.LogPhong;
            bool lightsOff = db.LightsOff;

            Graphics g = db.Graphics;

            if (g == null)
                return;
            float saveLiftZ = e.LiftZ;
            e.LiftZ = (float)db.LiftZ;
            RenderBitmapTriangle(v0, v3, v2, bmp, ix0, iy0, reflection, logPhong, lightsOff);
            RenderBitmapTriangle(v0, v2, v1, bmp, ix0, iy0, reflection, logPhong, lightsOff);
            RenderBitmapTriangle(v0, v2, v3, bmp, ix0, iy0, reflection, logPhong, lightsOff);
            RenderBitmapTriangle(v0, v1, v2, bmp, ix0, iy0, reflection, logPhong, lightsOff);
            db.Clear();
            e.LiftZ = saveLiftZ;
        }

        #endregion

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

        #region --- Helper Functions ---
        internal override GeometricObject GetDotObjectForDotLine(Vector3D center, double radius, ChartColor color)
        {
            GeometricObject go = new Ellipsoid(center, new Vector3D(radius, 0, 0), new Vector3D(0, radius, 0), radius, color);
			go.Parent = Top;
			return go;
        }

        #endregion
    }
}
