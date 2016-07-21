using ComponentArt.Web.Visualization.Charting.Design;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.IO;

using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{


	internal enum CoordinatePlaneOrientation
	{
		XYPlane,
		YZPlane,
		ZXPlane
	}
	
	// =============================================================================================
	//  Coordinate Plane
	// =============================================================================================
 
	/// <summary>
	///  Represents a coordinate plane on the chart.
	/// </summary> 
	/// <remarks>
	///   <para>
	///  Coordinate plane hosts two collections of objects. Property <see cref="StripSets"/> is a 
	///  <see cref="StripSetCollection"/>, property <see cref="Grids"/> is a grid collection.
	///  Members of these collections are named and names serve as indices to the collection objects.
	///   </para>
	///   <para>
	///     At the data binding time two default <see cref="StripSet"/>s are created for each coordinate plane:
	///     <list type="bullet">
	///       <item>"XinPlaneXY" (initially invisible) and "YinPlaneXY" in <see cref="CoordinateSystem.PlaneXY"/>,</item>
	///       <item>"YinPlaneYZ" and "ZinPlaneYZ" (initially invisible) in <see cref="CoordinateSystem.PlaneYZ"/>, and</item>
	///       <item>"ZinPlaneZX" (initially invisible) and "XinPlaneZX" in <see cref="CoordinateSystem.PlaneZX"/>.</item>
	///     </list>
	///     The name contains the axis whose set of values is used for strips creation (the first letter)
	///     and the name of the plane. After-data binding user code can change visibility of strip sets 
	///     and get vertical instead of horizontal strips, or vv.
	///   </para>
	///   <para>
	///  User may modify  the  existing strip sets or add new ones  by using <see cref="CreateStrips"/> methods
	///  in  the post-DataBind code. When adding new
	///  strip sets, care should be taken that these sets have different <see cref="StripSet.Layer"/> 
	///  properties. Layers take values 0, 1 etc. The concept of  a  layer is needed because strip sets cover 
	///  the same coordinate plane surface. If two or more visible strip sets share the same layer, the spatial order
	///  of pixels coming from different strip sets is quasi-random, causing unwanted rendering artefacts.
	///   </para>
	///   <para>
	///     At the data binding time two default <see cref="Grid"/>s are created for each coordinate plane.
	///     The names are the same as  the  names of the default strip sets. All default grids are initially
	///     visible.
	///   </para>
	/// </remarks>

	[System.ComponentModel.TypeConverter(typeof(CoordinatePlaneConverter))]
	[Serializable()]

	public class CoordinatePlane : ChartObject
	{
		private const double default3DDepth = 5; 
		private CoordinateSystem	system;

		private bool		hasChanged = true;

		private Axis		xAxis = null, yAxis = null;
		private Vector3D	Vz;
		private bool		autoICSOffset = true;
		private double		iCSOffset = 0;
		private double		depth = default3DDepth;

		private DrawingBoard		drawingBoard = null;
		private bool				isRadial = false;

		private GridCollection		grids;
		private StripSetCollection	stripSets;

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of <see cref="CoordinatePlane"/> class. 
		/// </summary>
		public CoordinatePlane()
		{
			grids = new GridCollection(this);
			stripSets = new StripSetCollection(this);
		}

		private void SetupAxes()
		{
			if(CoordinateSystem.PlaneXY == this)
			{
				xAxis = CoordinateSystem.XAxis;
				yAxis = CoordinateSystem.YAxis;
			}
			else if(CoordinateSystem.PlaneYZ == this)
			{
				xAxis = CoordinateSystem.YAxis;
				yAxis = CoordinateSystem.ZAxis;
			}
			else	
			{
				xAxis = CoordinateSystem.ZAxis;
				yAxis = CoordinateSystem.XAxis;
			}
			Vz = xAxis.UnitVector.CrossProduct(yAxis.UnitVector);
			if(!system.IsPositive)
				Vz = -Vz;
			return;
		}
		#endregion
		
		#region --- Properties ---

		internal CoordinatePlaneOrientation ICSOrientation
		{
			get
			{
				Vector3D xRole = ChartSpace.AxisUnitVector(XAxis.Role);
				Vector3D yRole = ChartSpace.AxisUnitVector(YAxis.Role);
				Vector3D zAxis = xRole.CrossProduct(yRole); 
				if(zAxis.Z != 0)
					return CoordinatePlaneOrientation.XYPlane;
				else if(zAxis.X != 0)
					return CoordinatePlaneOrientation.YZPlane;
				else
					return CoordinatePlaneOrientation.ZXPlane;
			}
		}

		internal CoordinatePlaneOrientation WCSOrientation
		{
			get
			{
				Vector3D zAxis = XAxis.UnitVector.CrossProduct(YAxis.UnitVector); 
				if(zAxis.Z != 0)
					return CoordinatePlaneOrientation.XYPlane;
				else if(zAxis.X != 0)
					return CoordinatePlaneOrientation.YZPlane;
				else
					return CoordinatePlaneOrientation.ZXPlane;
			}
		}

		internal bool HasChanged 
		{
			get { return hasChanged || grids.HasChanged || stripSets.HasChanged;  } 
			set { hasChanged = value; } 
		}
		
		/// <summary>
		/// Collection of grid lines in this coordinate plane.
		/// </summary>
		/// <remarks>
		/// For each coordinate plane two <ref>Grids</ref> are automatically created by the control
		/// and default visibility of those objects is set:
		/// <list type=">">
		/// "YinPlaneXY" in XY plane along the YAxis,
		/// "XinPlaneXY" in XY plane along the XAxis,
		/// "YinPlaneYZ" in YZ plane along the XAxis,
		/// "ZinPlaneYZ" in YZ plane along the YAxis,
		/// "XinPlaneZX" in ZX plane along the YAxis and
		/// "ZinPlaneZX" in ZX plane along the XAxis.
		/// </list>
		/// User can override visibility and/or coordinates of predefined grids. 
		/// If needed, additional grids may be created using function "CreateGrid()".
		/// (KED)
		/// </remarks>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[SRDescription("CoordinatePlaneGridsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public GridCollection     Grids		
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization)
				{
					GridCollection outGrids = new GridCollection(this);
					foreach(Grid grid in grids)
					{
						if(grid.HasChanged)
							outGrids.Add(grid);
					}
					return outGrids;
				}
				else
					return grids; 
			}
		}
		/// <summary>
		/// Collection of strips in this coordinate plane.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[SRDescription("CoordinatePlaneStripSetsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public StripSetCollection StripSets 
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization)
				{
					StripSetCollection ssc = new StripSetCollection(this);
					ssc.Clear();
					foreach(StripSet ss in stripSets)
					{
						if(ss.HasChanged)
							ssc.Add(ss);
					}
					return ssc;
				}
				else
					return stripSets; 
			}
		}

		/// <summary>
		/// Gets or sets the thickness of this <see cref="CoordinatePlane"/> object.
		/// </summary>
		[DefaultValue(default3DDepth)]
		[Description("Thickness of the plane.")]
		public double Depth							{ get { return depth; }	set { if(depth != value) hasChanged = true; depth = value; } }
		/// <summary>
		/// Gets the parent <see cref="CoordinateSystem"/> of this <see cref="CoordinatePlane"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CoordinateSystem	CoordinateSystem	{ get { return system; } }
		/// <summary>
		/// Gets the first <see cref="Axis"/> belonging to this <see cref="CoordinatePlane"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Axis XAxis							{ get { if(xAxis==null) SetupAxes(); return xAxis; } }
		/// <summary>
		/// Gets the second <see cref="Axis"/> belonging to this <see cref="CoordinatePlane"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Axis YAxis							{ get { if(yAxis==null) SetupAxes(); return yAxis; } }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal bool IsRadial						{ get { return isRadial; } set { if(isRadial != value) hasChanged = true; isRadial = value; } }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal Vector3D Origin 
		{
			get
			{
				SetupAxes();
				return CoordinateSystem.OriginWCS + Vz*ICSOffset;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DefaultValue(true)]
		internal bool AutoICSOffset { get { return autoICSOffset; } set { if(autoICSOffset != value) hasChanged = true; autoICSOffset = value; } }

		/// <summary>
		/// Gets or sets the offset ot this <see cref="CoordinatePlane"/> object from its original location.
		/// </summary>
		[DefaultValue(0.0)]
		[Description("Indicates the offset ot the coordinate plane from its original location.")]
		public double ICSOffset	
		{ 
			get { return iCSOffset; } 
			set 
			{ 
				if(value != iCSOffset) hasChanged = true; 
				AutoICSOffset = false;
				iCSOffset = value;
			}
		}

        internal DrawingBoard DrawingBoard	
		{ 
			get
			{
				if(drawingBoard == null)
					drawingBoard = CreateDrawingBoard();
				
				return drawingBoard; 
			} 
		}

		#endregion

		#region --- Creating Lines ---

		/// <summary>
		/// Creates a new <see cref="Grid"/> and adds it the the <see cref="Grids"/> collection.
		/// </summary>
		/// <param name="name">The name of the <see cref="Grid"/>.</param>
		/// <param name="axisOrientation">The orientation of the grid.</param>
		/// <returns>Newly created <see cref="Grid"/> object.</returns>
		public Grid CreateGrid(string name, AxisOrientation axisOrientation)
		{
			Grid grid = new Grid(name);
			grid.AxisOrientation = axisOrientation;
			grid.AxisOrientation2 = (XAxis.Role == axisOrientation)? YAxis.Role:XAxis.Role;
			grid.Name = name;
			grids.Add(grid);			

			return grid;
		}

		private Grid CreateDefaultGrid(string name, AxisOrientation axisOrientation)
		{
			Grid grid = CreateGrid(name,axisOrientation);
			grid.HasChanged = false;
			return grid;
		}
		
		private void CreateDefaultLines()
		{
			CoordinateSystem CS = Owner as CoordinateSystem;
			bool embeded = CS.IsEmbeded;

			if(OwningChart.InitializeOnDataBind)
			{
				Grids.Clear();
			}

			if(CS.PlaneXY == this)
			{
				if(Grids["YinPlaneXY"] == null)
					CreateDefaultGrid("YinPlaneXY",YAxis.Role);
				if(Grids["XinPlaneXY"] == null)
					CreateDefaultGrid("XinPlaneXY",XAxis.Role).Visible = !embeded;
			}
			else if(CS.PlaneYZ == this)
			{
				if(Grids["YinPlaneYZ"] == null)
					CreateDefaultGrid("YinPlaneYZ",XAxis.Role);
				if(Grids["ZinPlaneYZ"] == null)
					CreateDefaultGrid("ZinPlaneYZ",YAxis.Role).Visible = !embeded;
			}
			else // CS.PlaneZX == this
			{
				if(Grids["XinPlaneZX"] == null)
					CreateDefaultGrid("XinPlaneZX",YAxis.Role).Visible = !embeded;
				if(Grids["ZinPlaneZX"] == null)
					CreateDefaultGrid("ZinPlaneZX",XAxis.Role).Visible = !embeded;
			}
		}

		#endregion

		#region --- Creating StripSet ---
		
		/// <summary>
		/// Creates a new <see cref="StripSet"/> and adds it the the <see cref="StripSets"/> collection.
		/// </summary>
		/// <param name="name">The name of the <see cref="StripSet"/>.</param>
		/// <param name="axisOrientation">The orientation of the strips.</param>
		/// <param name="stripColor">The main color of the strips.</param>
		/// <param name="secondaryStripColor">The color between the strips.</param>
		/// <returns>Newly created <see cref="StripSet"/> object.</returns>
		public StripSet CreateStrips(string name, AxisOrientation axisOrientation, Color stripColor, Color secondaryStripColor)
		{
			StripSet sSet = new StripSet(name);
			sSet.AxisOrientation = axisOrientation;
			Axis axis = (XAxis.Role == axisOrientation)? XAxis:YAxis;
			sSet.CoordinateSet = null; //axis.CreateCoordinateSet(5);
			sSet.StripSetKind = StripSetKind.Alternating;
			sSet.Color = stripColor;
			sSet.AlternateColor = secondaryStripColor;
			stripSets.Add(sSet);
			return sSet;
		}
		
		/// <summary>
		/// Creates a new <see cref="StripSet"/> and adds it the the <see cref="StripSets"/> collection.
		/// </summary>
		/// <param name="name">The name of the <see cref="StripSet"/>.</param>
		/// <param name="axisOrientation">The orientation of the strips.</param>
		/// <param name="stripColor">The main color of the strips.</param>
		/// <param name="width">The width of the strips.</param>
		/// <returns>Newly created <see cref="StripSet"/> object.</returns>
		public StripSet CreateStrips(string name, AxisOrientation axisOrientation, Color stripColor, double width)
		{
			StripSet sSet = new StripSet(name);
			sSet.AxisOrientation = axisOrientation;
			Axis axis = (XAxis.Role == axisOrientation)? XAxis:YAxis;
			sSet.CoordinateSet = null;//axis.CreateCoordinateSet(5);
			sSet.StripSetKind = StripSetKind.Simple;
			sSet.Color = stripColor;
			sSet.AlternateColor = stripColor;
			sSet.Width = width;
			stripSets.Add(sSet);
			return sSet;
		}

		private void CreateDefaultStripSet()
		{
			CoordinateSystem CS = Owner as CoordinateSystem;

			string name;
			ClearStripSet();
			StripSet ss;
			if(CS.PlaneXY == this)
			{
				name = "XinPlaneXY";
				if(StripSets[name] == null)
				{
					ss = CreateStrips(name,XAxis.Role,Color.FromArgb(0,0,0,0),Color.FromArgb(0,0,0,0));
					ss.Visible = false;
					ss.Layer = 1;
				}

				name = "YinPlaneXY";
				if(StripSets[name] == null)
					CreateStrips(name,YAxis.Role,Color.FromArgb(0,0,0,0),Color.FromArgb(0,0,0,0));
			}
			else if (CS.PlaneYZ == this)
			{
				name = "YinPlaneYZ";
				if(StripSets[name] == null)
					CreateStrips(name,XAxis.Role,Color.FromArgb(0,0,0,0),Color.FromArgb(0,0,0,0));
				name = "ZinPlaneYZ";
				if(StripSets[name] == null)
				{
					ss = CreateStrips(name,YAxis.Role,Color.FromArgb(0,0,0,0),Color.FromArgb(0,0,0,0));
					ss.Visible = false;
					ss.Layer = 1;
				}
			}
			else	// Plane ZX
			{
				name = "ZinPlaneZX";
				if(StripSets[name] == null)
				{
					ss = CreateStrips(name,XAxis.Role,Color.FromArgb(0,0,0,0),Color.FromArgb(0,0,0,0));
					ss.Visible = false;
					ss.Layer = 1;
				}
				name = "XinPlaneZX";
				if(StripSets[name] == null)
					CreateStrips(name,YAxis.Role,Color.FromArgb(0,0,0,0),Color.FromArgb(0,0,0,0));
			}

			foreach (ComponentArt.Web.Visualization.Charting.StripSet S in stripSets)
			{
				S.HasChanged = false;
				if(CS.IsEmbeded)
					S.Visible = false;
			}
		}

		internal void ClearStripSet()
		{
			if (stripSets != null)
				stripSets.Clear();
		}

		#endregion

		#region --- Internal Methods ---

		/// <summary>
		/// Maps from ICS coordinates of the plane to WCS coordinates
		/// </summary>
		/// <param name="coordinate1ICS">ICS coordinate along the first axis of the plane</param>
		/// <param name="coordinate2ICS">ICS coordinate along the second axis of the plane</param>
		/// <returns>Vector in the world coordinate system of the point in the plane</returns>
		internal Vector3D ICS2WCS(double coordinate1ICS, double coordinate2ICS)
		{
			if(XAxis == CoordinateSystem.XAxis)
				return CoordinateSystem.ICS2WCS(new Vector3D(coordinate1ICS, coordinate2ICS, iCSOffset));
			else if(XAxis == CoordinateSystem.YAxis)
				return CoordinateSystem.ICS2WCS(new Vector3D(iCSOffset, coordinate1ICS, coordinate2ICS));
			else if(XAxis == CoordinateSystem.ZAxis)
				return CoordinateSystem.ICS2WCS(new Vector3D(coordinate2ICS, iCSOffset, coordinate1ICS));
			else
				throw new Exception("Implementation: Axis doesn't belong to the parent coordinate system");
		}

		internal Vector3D ICS2WCS(double coordinate1ICS, double coordinate2ICS, double coordinate3ICS)
		{
			if(XAxis == CoordinateSystem.XAxis)
				return CoordinateSystem.ICS2WCS(new Vector3D(coordinate1ICS, coordinate2ICS, iCSOffset + coordinate3ICS));
			else if(XAxis == CoordinateSystem.YAxis)
				return CoordinateSystem.ICS2WCS(new Vector3D(iCSOffset + coordinate3ICS, coordinate1ICS, coordinate2ICS));
			else if(XAxis == CoordinateSystem.ZAxis)
				return CoordinateSystem.ICS2WCS(new Vector3D(coordinate2ICS, iCSOffset + coordinate3ICS, coordinate1ICS));
			else
				throw new Exception("Implementation: Axis doesn't belong to the parent coordinate system");
		}

		/// <summary>
		/// Maps from logical coordinates of the plane to world coordinates. Logical 
		/// coordinates may be radial (x = radius, y = angle), or orthogonal
		/// </summary>
		/// <param name="xLogical">logical x coordinate</param>
		/// <param name="yLogical">logical y coordinate</param>
		/// <returns>Vector in the world coordinate system</returns>
		internal Vector3D LogicalToWorld(double xLogical, double yLogical)
		{
			if(!isRadial)
				return ICS2WCS(xLogical,yLogical);

			// ICS coordinates of the centre
			double xcICS = (XAxis.MinValueICS + XAxis.MaxValueICS)*0.5;
			double ycICS = (YAxis.MinValueICS + YAxis.MaxValueICS)*0.5;
			// ICS radius
			double  rICS = Math.Min(
				Math.Abs(XAxis.MaxValueICS - XAxis.MinValueICS),
				Math.Abs(YAxis.MaxValueICS - YAxis.MinValueICS)
				)*0.5;

			rICS *= (yLogical-YAxis.MinValueICS)/(YAxis.MaxValueICS - YAxis.MinValueICS);
			//rICS *= 0.90;

			double angle = YAngleAtLogical(xLogical);
			return ICS2WCS(xcICS + rICS*Math.Cos(angle),ycICS + rICS*Math.Sin(angle));
		}

		internal double YAngleAtLogical(double xLogical)
		{
			if(!isRadial)
				return 0.0;
			double startingAngleDegree = 90;
			double startingAngle = Math.PI*2.0/360*startingAngleDegree;
			return startingAngle - Math.PI*2.0*(xLogical-XAxis.MinValueICS)/(XAxis.MaxValueICS-XAxis.MinValueICS);
		}

		internal void SetDefaultDepth()
		{
			if(CoordinateSystem.TargetArea.IsTwoDimensional)
				depth = 0;
			else
				depth = default3DDepth;
		}

        internal DrawingBoard CreateDrawingBoard()
		{
            DrawingBoard drawingBoard = GE.CreateDrawingBoard();// new Geometry.DrawingBoard();
			SetupAxes();
			Vector3D Vx = ICS2WCS(XAxis.MaxValueICS,YAxis.MinValueICS) - ICS2WCS(XAxis.MinValueICS,YAxis.MinValueICS);
			Vector3D Vy = ICS2WCS(XAxis.MinValueICS,YAxis.MaxValueICS) - ICS2WCS(XAxis.MinValueICS,YAxis.MinValueICS);
			drawingBoard.V0 = Origin;
			drawingBoard.Vx = Vx;
			drawingBoard.Vy = Vy;
			drawingBoard.Grow(5/*OwningChart.FromPointToWorld*5*/);
			drawingBoard.Initialize();
			return drawingBoard;
		}

		internal void SetCoordinateSystem (CoordinateSystem system)
		{
			SetOwner(system);
			this.system = system;
		}

		internal void CreateDefaultContents()
		{
			// Default lines and stripes

			CreateDefaultLines();
			CreateDefaultStripSet();
			stripSets.HasChanged = false;
			grids.HasChanged = false;
		}

		internal override void Build()
		{			
		}

		internal override void Render()
		{
			if(!Visible)
				return;

			if(GE.Top is RadarBox)
			{
				GE.Top.Tag = this;
				CoordinatePlaneBox gpBox = new CoordinatePlaneBox();
				GE.Push(gpBox);
				gpBox.Tag = this;
			}
			else
			{
				if(!(GE.Top is CoordinateSystemBox))
					throw new Exception("Implementation: CoordinatePlaneBox as child of " + GE.Top.GetType().Name);
				GeometricObject planes = (GE.Top.Owning(typeof(CoordinateSystemBox)) as CoordinateSystemBox).CoordinatePlanes;
				CoordinatePlaneBox gpBox = new CoordinatePlaneBox();
				GE.Push(gpBox);
				gpBox.Tag = this;
			}

			// Lines and stripes

			foreach(StripSet sSet in stripSets)
				sSet.Render();

			foreach(Grid grid in grids)
				grid.Render();
			
			Vector3D Vx = ICS2WCS(XAxis.MaxValueICS,YAxis.MinValueICS) - ICS2WCS(XAxis.MinValueICS,YAxis.MinValueICS);
			Vector3D Vy = ICS2WCS(XAxis.MinValueICS,YAxis.MaxValueICS) - ICS2WCS(XAxis.MinValueICS,YAxis.MinValueICS);

			// Stripes

			// Check if some axis has strips in this plane

			bool stripsExist = false;
			foreach(StripSet sSet in StripSets)
				if(sSet.Plane == this && sSet.Visible)
				{
					stripsExist = true;
					break;
				}

			if(!stripsExist) // whole plane is one strip
			{
				StripSet s = new StripSet("---",0,
					OwningChart.Palette.CoordinateLineColor,
					OwningChart.Palette.CoordinatePlaneColor);
				s.AxisOrientation = CoordinateSystem.Role(XAxis);
				s.SetOwner(XAxis);
				CoordinateSet cSet = XAxis.CreateCoordinateSet(0);
				cSet.Add(XAxis.MinValue,XAxis.MaxValue);
				s.CoordinateSet = cSet;
				s.Render();
			}

			// Plane edges
			LineStyle2D LS = OwningChart.LineStyles2D["AxisLine"];
			if(LS == null || IsRadial)
			{
				drawingBoard = null;
			}
			else
			{

				// Setting color from palette if the style color is fully transparent
				DrawingBoard db = CreateDrawingBoard();
				if(LS.Color.A == 0)
				{
					LS = LS.Clone() as LineStyle2D;
					LS.Color = OwningChart.Palette.AxisLineColor;
				}

				db.DrawLine(LS,Origin,Origin+Vx);
				db.DrawLine(LS,Origin,Origin+Vy);
				db.DrawLine(LS,Origin+Vx,Origin+Vx+Vy);
				db.DrawLine(LS,Origin+Vy,Origin+Vx+Vy);
			}

//			if(!isRadar)
				GE.Pop(typeof(CoordinatePlaneBox));
		}
		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CoordinatePlane"/> is displayed.
		/// </summary>
		[Description("Determintes whether to show the plane.")]
		[DefaultValue(true)]
		public override bool Visible
		{
			get 
			{ 
				if(!OwningChart.InSerialization)
				{
					if(CoordinateSystem.TargetArea.IsTwoDimensional)
						return (CoordinateSystem.PlaneXY == this) && base.Visible;
					SeriesStyle style = CoordinateSystem.OwningSeries.Style;
					if(style.IsRadar)
						return (CoordinateSystem.PlaneXY == this);
					if(style.ChartKind == ChartKind.Pie || style.ChartKind == ChartKind.Doughnut)
						return false;
				}
				return base.Visible;
			}
			set 
			{ 
				if(base.Visible != value) hasChanged = true; base.Visible = value;
			}
		}

		#region --- Serialization ---


		private bool ShouldSerializeXAxisName()		{ return false; }
		private bool ShouldSerializeYAxisName()		{ return false; }
		private bool ShouldSerializeXAxis()			{ return false; }
		private bool ShouldSerializeYAxis()			{ return false; }

		private static string[] PropertiesOrder = new string[] 
			{
				"PlaneKind",
				"Depth",
				"Z0",
				"Visible"

			};
		#endregion
	};

}
