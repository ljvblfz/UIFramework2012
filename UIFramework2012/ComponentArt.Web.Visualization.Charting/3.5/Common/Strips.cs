using ComponentArt.Web.Visualization.Charting.Design;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.IO;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Enumeration type for <see cref="StripSet"/>s. 
	/// </summary>
	/// <remarks>
	/// User created strip sets have StripsKind = Custom. 
	/// Other enumeration values are assigned to strip sets created by the control.
	/// </remarks>
	public enum StripsKind
	{
		/// <summary>
		/// Related to x-axis, in y-axis direction
		/// </summary>
		XinPlaneXY,
		/// <summary>
		/// Related to y-axis, in x-axis direction
		/// </summary>
		YinPlaneXY,
		/// <summary>
		/// Related to y-axis, in z-axis direction
		/// </summary>
		YinPlaneYZ,
		/// <summary>
		/// Related to z-axis, in y-axis direction
		/// </summary>
		ZinPlaneYZ,
		/// <summary>
		/// Related to z-axis, in x-axis direction
		/// </summary>
		ZinPlaneZX,
		/// <summary>
		/// Related to x-axis, in z-axis direction
		/// </summary>
		XinPlaneZX,
		/// <summary>
		/// Custom <see cref="StripSet"/>
		/// </summary>
		Custom
	};
	
	/// <summary>
	/// Represents a single strip in a <see cref="StripSet"/> object.
	/// </summary>
	public class Strip : ChartObject
	{
		private	ChartColor		surface;
		private bool			surfaceSetAtConstruction;
		private Axis			axis;
		private CoordinatePlane coordinatePlane;
		private double			minValueLCS = 0, maxValueLCS = 0;
		private double			liftZ = 0.1;
		private int				layer = 1;
		private object			minValue = null, maxValue = null;
		private SpecialColor	paletteIndex = SpecialColor.CoordinatePlane;

		#region --- Constructors ---
		/// <summary>
		/// Initialises a new instance of a <see cref="Strip"/> class with specified axis, coordinate plane, surface, and minimum and maximum vaules.
		/// </summary>
		/// <param name="axis">The <see cref="Axis"/> this strip is on.</param>
		/// <param name="coordinatePlane">Coordinate plane that this strip belongs to.</param>
		/// <param name="chartColor">The color of the strip.</param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		public Strip(Axis axis, CoordinatePlane coordinatePlane, ChartColor chartColor, object minValue, object maxValue)
		{
			this.axis = axis;
			this.surface = chartColor;
			this.coordinatePlane = coordinatePlane;
			this.minValue = minValue;
			this.maxValue = maxValue;
			surfaceSetAtConstruction = true;
			this.SetOwner(axis);
		}
		
		internal Strip(Axis axis, CoordinatePlane coordinatePlane, SpecialColor paletteIndex, object minValue, object maxValue)
		{
			this.axis = axis;
			this.paletteIndex = paletteIndex;
			this.coordinatePlane = coordinatePlane;
			this.minValue = minValue;
			this.maxValue = maxValue;
			surface = new ChartColor(0.5f,6,Color.Transparent);
			this.SetOwner(axis);
		}
		
		/// <summary>
		/// Initialises a new instance of a <see cref="Strip"/> class with specified axis, coordinate plane, and surface.
		/// </summary>
		/// <param name="axis">The <see cref="Axis"/> this strip is on.</param>
		/// <param name="coordinatePlane">Coordinate plane that this strip belongs to.</param>
		/// <param name="chartColor">The color of the strip.</param>
		public Strip(Axis axis, CoordinatePlane coordinatePlane, ChartColor chartColor)
			: this(axis,coordinatePlane,chartColor,null,null) { }

		internal Strip(Axis axis, CoordinatePlane coordinatePlane, SpecialColor paletteIndex)
			: this(axis,coordinatePlane,paletteIndex,null,null) { }

		internal Strip() : this (null,null,null) { }
		#endregion

		#region --- Properties ---

		/// <summary>
		/// Gets the axis this <see cref="Strip"/> belongs to.
		/// </summary>
		public Axis Axis { get { return axis; } }
		/// <summary>
		/// Gets the plane this <see cref="Strip"/> belongs to.
		/// </summary>
		public CoordinatePlane CoordinatePlane { get { return coordinatePlane; } }
		/// <summary>
		/// Gets and sets the color of this <see cref="Strip"/>.
		/// </summary>
		public ChartColor ChartColor 
		{ 
			get { return surface; } 
			set 
			{
				surface = value; 
			}
		}

		// FIXME: add description and also in the constructor.
		public object MinValue { get { return minValue; } set { minValue = value; if(Axis != null && Axis.Dimension != null) minValueLCS = Axis.Dimension.Coordinate(minValue); } }
		public object MaxValue { get { return maxValue; } set { maxValue = value; if(Axis != null && Axis.Dimension != null) minValueLCS = Axis.Dimension.Coordinate(maxValue) + Axis.Dimension.Width(maxValue); } }
		internal double LiftZ    { get { return 0.2*layer/OwningChart.FromWorldToTarget; } }
		/// <summary>
		/// Gets the layer of this strip.
		/// </summary>
		/// <remarks>
		/// Higher layer are on top of the lower layers, thus lower levels might be partially of fully covered.
		/// </remarks>
		[DefaultValue(1)]
		public int Layer { get { return layer; } set { layer = Math.Max(1,value); } }
		
		private ChartColor EffectiveChartColor
		{
			get
			{
				if(surface.Color.A == 0)
				{
					if(paletteIndex == SpecialColor.CoordinatePlane)
						return new ChartColor(0f,1,OwningChart.Palette.CoordinatePlaneColor);
					else
						return new ChartColor(0f,1,OwningChart.Palette.CoordinatePlaneSecondaryColor);
				}
				else
					return surface;
			}
		}

        [Obsolete()]
		public void SetLCSRange (double minLCS, double maxLCS)
		{
			minValueLCS = minLCS;
			maxValueLCS = maxLCS;
			minValue = null;
			maxValue = null;
		}

		internal bool SurfaceSetAtConstruction { get { return surfaceSetAtConstruction; } }

		#endregion
 
		#region --- Rendering ---

		internal override void Render()
		{
			if(!Visible)
				return;
			if(CoordinatePlane.IsRadial)
				RenderRadial();
			else
				RenderParallel();
		}

		private void RenderRadial()
		{
			Axis Ax, Ay;
			Ax = axis;
			if(coordinatePlane.XAxis == axis)
				Ay = coordinatePlane.YAxis;
			else
				Ay = coordinatePlane.XAxis;

			// Getting coordinates

			double minValueICS = axis.LCS2ICS(minValueLCS);
			double maxValueICS = axis.LCS2ICS(maxValueLCS);
			if(minValue != null)
				minValueICS = axis.Dimension.Coordinate(minValue);
			if(maxValue != null)
				maxValueICS = axis.ICoordinate(maxValue) + axis.IWidth(maxValue);

			// Coordinates reduced to the ICS range of the axis

			double c1 = Math.Max(axis.MinValueICS,minValueICS);
			double c2 = Math.Min(axis.MaxValueICS,maxValueICS);
			if(c1 >= c2) // ranges of the strip and axis don't overlap
				return;

			double xLog0,xLog1, yLog0,yLog1;
			if(Ax == coordinatePlane.XAxis)
			{
				xLog0 = c1;
				xLog1 = c2;
				yLog0 = Ay.MinValueICS;
				yLog1 = Ay.MaxValueICS;
			}
			else
			{
				yLog0 = c1;
				yLog1 = c2;
				xLog0 = Ay.MinValueICS;
				xLog1 = Ay.MaxValueICS;
			}

						
			// Rendering rectangle in the logical system:
			//	Ax: xLog0..xLog1
			//  Ay: yLog0..yLog1
			Vector3D Az = Ax.UnitVector.CrossProduct(Ay.UnitVector);
			int nSeg = 100;
			double dSeg = (xLog1-xLog0)/nSeg;
			double x0 = xLog0;
			double x1;
			Vector3D N0,N1=new Vector3D(0,0,0);

			ChartColor eSurface = EffectiveChartColor;

			Vector3D[] V00S = new Vector3D[nSeg];
			Vector3D[] V01S = new Vector3D[nSeg];
			Vector3D[] N0S = new Vector3D[nSeg];

			for (int ix=0; ix<nSeg; ix++)
			{
				x1 = x0+dSeg;
				// Points: (Note that (V00,V10) and (V01,V11) are arcs)
				//         V11
				//         +
				//        /|
				//       / |
				//  V10 +  |
				//      |  |
				//      |  |
				//      +--+
				/// V00    V01
				Vector3D V00 = coordinatePlane.LogicalToWorld(x0,yLog0);
				Vector3D V10 = coordinatePlane.LogicalToWorld(x1,yLog0);
				Vector3D V11 = coordinatePlane.LogicalToWorld(x1,yLog1);
				Vector3D V01 = coordinatePlane.LogicalToWorld(x0,yLog1);
				if(ix==0)
				{
					N0 = (V01-V00);
					if(!N0.IsNull)
					{
						N0 = N0.Unit();
						N0.Z += 0.5;
						N0 = N0.Unit();
					}
				}
				else
					N0 = N1;

				N1 = (V11-V10);
				if(!N1.IsNull)
				{
					N1 = N1.Unit();
					N1.Z += 0.5;
					N1 = N1.Unit();
				}
				V00S[ix] = V00;
				V01S[ix] = V01;
				N0S[ix] = N0;
				x0 = x1;
			}

			GE.CreateRadialStrip(V00S,V01S,N0S,eSurface);
		}

		private void RenderParallel()
		{
			Axis Ax, Ay;
			Ax = axis;
			double fLift;
			if(coordinatePlane.XAxis == axis)
			{
				fLift = 1.0;
				Ay = coordinatePlane.YAxis;
			}
			else
			{
				fLift = -1.0;
				Ay = coordinatePlane.XAxis;
			}

			// Getting coordinates
			double minValueICS = axis.LCS2ICS(minValueLCS);
			double maxValueICS = axis.LCS2ICS(maxValueLCS);
			if(minValue != null)
				minValueICS = axis.ICoordinate(minValue);
			if(maxValue != null)
				maxValueICS = axis.ICoordinate(maxValue) + axis.IWidth(maxValue);
			// Reverse if necessary
			if(minValueICS > maxValueICS)
			{
				double a = minValueICS;
				minValueICS = maxValueICS;
				maxValueICS = a;
			}

			// Coordinates reduced to the ICS range of the axis

			double c1 = Math.Max(axis.MinValueICS,minValueICS);
			double c2 = Math.Min(axis.MaxValueICS,maxValueICS);
			if(c1 >= c2) // ranges of the strip and axis don't overlap
			{
				return;
			}

			// Render
			Vector3D P0, P1;
			double fz = -coordinatePlane.Depth*coordinatePlane.CoordinateSystem.TargetArea.Mapping.FromPointToWorld;
				
			Vector3D vLift = Ax.UnitVector.CrossProduct(Ay.UnitVector)*(LiftZ*fLift);
			if(Ax == coordinatePlane.XAxis)
			{
				P0 = coordinatePlane.ICS2WCS(c1,Ay.MinValueICS);
				P1 = coordinatePlane.ICS2WCS(c2,Ay.MaxValueICS,fz);
			}
			else
			{
				P0 = coordinatePlane.ICS2WCS(Ay.MinValueICS,c1);
				P1 = coordinatePlane.ICS2WCS(Ay.MaxValueICS,c2,fz);
			}
			P0 = P0 + vLift;
			P1 = P1 - vLift;
            GE.CreateBox(P0,P1,EffectiveChartColor);
		}
		#endregion

	}

	/// <summary>
	/// A collection of <see cref="Strip"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class StripCollection: CollectionWithType 
	{
		internal StripCollection() : base(typeof(Strip)) { }
		/// <summary>
		/// Indicates the <see cref="Strip"/> at the specified indexed location in the <see cref="StripCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="Strip"/> from the <see cref="StripCollection"/> object.</param>
		public new Strip this[object obj]
		{
			get { return (Strip)(List[IndexOf(obj)]); } 
			set { List[IndexOf(obj)] = value;} 
		}

		public new void Add(object obj)
		{
			base.Add(obj);
		}
	}
	
	/// <summary>
	/// Describes the type of strips.
	/// </summary>
	public enum StripSetKind
	{
		Simple,
		/// <summary>
		/// StripSet are of alternating color along the plane.
		/// </summary>
		Alternating,
		/// <summary>
		/// There is only one strip on the plane.
		/// </summary>
		SingleStrip
	}

	internal enum StripLineKind
	{
		Parallel,
		Circular,
		Radial
	};

	/// <summary>
	/// Describes the strips on a <see cref="CoordinatePlane"/> of the chart.
	/// </summary>

	// =============================================================================================
	//		Stripes
	// =============================================================================================

	[Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(StripSetConverter))]
	[Serializable]
	public class StripSet : ChartObject, INamedObject
	{
		private AxisOrientation axisOrientation;

		const string			_StripColorDefault = "0,0,0,0";
		const string			_AlternateStripColorDefault = "0,0,0,0";
		const StripSetKind		_KindDefault = StripSetKind.Alternating;
		const double			_WidthDefault = 0;
		const StripLineKind		_StripKindDefault = StripLineKind.Parallel;
		const int				_LayerDefault = 0;

		private Color			stripColor;
		private Color			alternateStripColor;
		private CoordinateSet	coordSet;
		private StripSetKind	kind = _KindDefault;
		private double			width;
		private StripLineKind	stripKind = _StripKindDefault;
		private StripCollection strips = null;
		private int				layer = _LayerDefault;
		private int				numberOfLabels = 0;

		private bool			hasChanged = true;

		// INamedObject I/F data
		private string				name = "";
		private StripSetCollection	owningCollection;

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of the <see cref="StripSet"/> class with default parameters.
		/// </summary>
		public StripSet() : this ("",_WidthDefault,(Color)new ColorConverter().ConvertFromInvariantString(_StripColorDefault),(Color)new ColorConverter().ConvertFromInvariantString(_AlternateStripColorDefault)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="StripSet"/> class with specified axis name, width, strip and alternate strip colors.
		/// </summary>
		/// <param name="width">The width of each strip in this <see cref="StripSet"/> object.</param>
		/// <param name="stripColor">The color of the strips.</param>
		/// <param name="alternateStripColor">The color between the strips.</param>
		public StripSet(string name, double width, Color stripColor, Color alternateStripColor)
		{
			this.name = name;
			this.stripColor = stripColor;
			this.alternateStripColor = alternateStripColor;
			this.width = width;
			if(width > 0)
				kind = StripSetKind.Simple;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StripSet"/> class with specified axis name and width.
		/// </summary>
		/// <param name="width">The width of each strip in this <see cref="StripSet"/> object.</param>
		public StripSet(string name, double width)
		{
			this.name = name;
			this.width = width;
			stripColor=(Color)new ColorConverter().ConvertFromInvariantString(_StripColorDefault);
			alternateStripColor=(Color)new ColorConverter().ConvertFromInvariantString(_AlternateStripColorDefault);
			if(width > 0)
				kind = StripSetKind.Simple;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StripSet"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="StripSet"/> object. This value is stored in the <see cref="StripSet.Name"/> property.</param>

		public StripSet(string name)
			: this(name,0) {}

		#endregion

		#region --- INamedObject Interface Implementation ---

		/// <summary>
		/// Gets or sets the name of this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripSetNameDescr")]
		[DefaultValue("")]
		public string Name 
		{
			get { return name; } 
			set 
			{
				name = value; 
				if(owningCollection != null)
				{
					owningCollection.Remove(this);
					owningCollection.Add(this);
				}
			} 
		}

		/// <summary>
		/// Gets or sets the owning <see cref="NamedCollection"/> of this <see cref="StripSet"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public NamedCollection OwningCollection 
		{
			get { return owningCollection; } 
			set 
			{
				if(!(value is StripSetCollection))
					throw new Exception("Invalid type of value");
				owningCollection = value as StripSetCollection; 
			}
		}

		#endregion

		#region --- Handling StripSets ---
		static string[] stripNames = new string[]
			{
				"XinPlaneXY",
				"YinPlaneXY",
				"YinPlaneYZ",
				"ZinPlaneYZ",
				"ZinPlaneZX",
				"XinPlaneZX",
				"Custom"
			};

		static StripsKind[] stripKinds = new StripsKind[]
			{
				StripsKind.XinPlaneXY,
				StripsKind.YinPlaneXY,
				StripsKind.YinPlaneYZ,
				StripsKind.ZinPlaneYZ,
				StripsKind.ZinPlaneZX,
				StripsKind.XinPlaneZX,
				StripsKind.Custom
			};

		internal static string NameOf(StripsKind kind)
		{
			for(int i=0; i<stripKinds.Length;i++)
			{
				if(kind==stripKinds[i])
					return stripNames[i];
			}
			throw new Exception("Implementation: arrays stripNames/stripKinds mismatch");
		}

		internal static StripsKind KindOf(string stripsName)
		{
			for(int i=0; i<stripKinds.Length;i++)
			{
				if(stripsName==stripNames[i])
					return stripKinds[i];
			}
			return StripsKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StripsKind Kind
		{
			get
			{
				return StripSet.KindOf(Name);
			}
			set
			{
				Name = StripSet.NameOf(value);
			}
		}

		#endregion

		#region --- Properties ---

		public override string ToString() { return Name; }

		/// <summary>
		/// Gets or sets the layer of this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripSetLayerDescr")]
		[DefaultValue(_LayerDefault)]
		public int Layer    
		{ 
			get { return layer; } 
			set 
			{ 
				if(layer != value)
				{
					hasChanged = true;
					layer = value; 
				}
			}
		}

		/// <summary>
		/// Collection of <see cref="Strip"/> objects in this <see cref="StripSet"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public StripCollection Strips
		{
			get
			{
				if(strips == null || OwningChart.InDesignMode)
				{
					double liftZ = layer*0.1/OwningChart.FromWorldToTarget;
					Axis Ax,Ay;
					if(GetAxes(out Ax,out Ay))
					{
						strips = new StripCollection();
						SpecialColor[] spColors = new SpecialColor[2];
						spColors[0] = SpecialColor.CoordinatePlane;
						spColors[1] = SpecialColor.CoordinatePlaneSecondary;

						ChartColor[] surfaces = new ChartColor[2];
						surfaces[0] = new ChartColor(0f,1,stripColor);
						surfaces[1] = new ChartColor(0f,1,(kind == StripSetKind.Alternating)? alternateStripColor:stripColor);

						CoordinateSet coords = CoordinateSet;
						if(kind == StripSetKind.Alternating)
						{
							int i = 0, k = 0;
							double coord0, coord1;
							coord0 = coords[0].Offset;
							while(i<coords.Count)
							{
								if(i<coords.Count-1)
								{
									coord1 = coords[i+1].Offset;
								}
								else
									coord1 = coord0 + coords[i].Width;
								Strip strip = new Strip(Ax,Plane,spColors[k%2]);
								strips.Add(strip);
								strip.SetOwner(Ax);
								strip.ChartColor = surfaces[k%2];
								strip.SetLCSRange(coord0,coord1);
								strip.Layer = Layer;
								coord0 = coord1;
								i++;
								k++;
							}
						}
						else
						{
							for(int i=0;i<coords.Count; i++)
							{
								Strip strip = new Strip(Ax,Plane,spColors[0]);
								strips.Add(strip);
								strip.ChartColor = surfaces[0];
								strip.SetOwner(Ax);
								strip.SetLCSRange(coords[i].Offset,coords[i].Offset+width);
								strip.Layer = Layer;
							}
						}
					}
				}
				return strips;
			}
		}
		
		/// <summary>
		/// Gets or sets the color of the strips in this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripSetColorDescr")]
		[DefaultValue(typeof(Color), _StripColorDefault)]
		public Color	Color				
		{
			get { return stripColor; }
			set 
			{
				if(stripColor != value)
				{
					hasChanged = true;
					stripColor = value; 
				}
			} 
		}
		/// <summary>
		/// Gets or sets the color between the strips in this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripSetAlternateColorDescr")]
		[DefaultValue(typeof(Color), _AlternateStripColorDefault)]
		public Color	AlternateColor		
		{ 
			get { return alternateStripColor; }	
			set 
			{ 
				if(alternateStripColor != value)
				{
					hasChanged = true;
					alternateStripColor = value;  
				}
			}
		}
		/// <summary>
		/// Gets or sets the width of each strip in this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripSetWidthDescr")]
		[DefaultValue(_WidthDefault)]
		public double	Width
		{ 
			get { return width; }		
			set 
			{ 
				if(width != value)
				{
					hasChanged = true;
					width = value;  
				}
			}
		}
		/// <summary>
		/// Gets or sets the kind of strips in this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripSetStripSetKindDescr")]
		[DefaultValue(_KindDefault)]
		public StripSetKind StripSetKind	
		{ 
			get { return kind; }
			set 
			{ 
				if(kind != value)
				{
					hasChanged = true;
					kind = value;  
				}
			}
		}

		
		/// <summary>
		/// Gets or sets the minimum number of strips in this <see cref="StripSet"/> object.
		/// </summary>
		[SRDescription("StripsMinimumNumberOfStripsDescr")]
		[DefaultValue(0)]
		public int MinimumNumberOfStrips 
		{ 
			get { return numberOfLabels; }
			set 
			{
				if(numberOfLabels != value)
				{
					hasChanged = true;
					numberOfLabels = value; 
					coordSet = null; 
				}
			}
		}

		/// <summary>
		/// Gets or sets the coordinate set of this <see cref="StripSet"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CoordinateSet CoordinateSet	
		{ 
			get 
			{ 
				if(coordSet == null || OwningChart.InDesignMode)
				{
					Axis Ax,Ay;
					if(GetAxes(out Ax,out Ay))
					{
						if(numberOfLabels > 0)
							coordSet = Ax.CreateCoordinateSet(numberOfLabels);
						else
							coordSet = Ax.DefaultCoordinateSet.GetCopy();
					}
				}
				ArrayList vals = coordSet.ValueList;
				Axis axis = this.Axis;
				if(vals.Count > 0)
				{
					object val = vals[vals.Count-1];
					if(val is Coordinate)
						val = (val as Coordinate).Value;
					if(axis.Dimension.Compare(val,axis.MaxValue) < 0)
					{
						coordSet.Add(axis.MaxValue);
					}
					val = vals[0];
					if(val is Coordinate)
						val = (val as Coordinate).Value;
					if(axis.Dimension.Compare(val,axis.MinValue) > 0)
					{
						coordSet.ValueList.Insert(0,axis.MinValue);
					}
				}
				return coordSet; 
			}				
			set 
			{
				hasChanged = true;
				coordSet = value; 
			}
		}

		internal Axis				Axis
		{
			get { return (Plane.XAxis.Role == AxisOrientation)? Plane.XAxis:Plane.YAxis; }
		}

		internal CoordinatePlane	Plane	
		{ 
			get 
			{
				return Owner as CoordinatePlane;
			}
		}

		internal CoordinateSystem CoordinateSystem { get { return Axis.CoordSystem; } }
			
		/// <summary>
		/// Gets or sets the orientation of this <see cref="StripSet"/> object.
		/// </summary>
		[Browsable(false)]
		public AxisOrientation AxisOrientation 
		{ 
			get { return axisOrientation; } 
			set 
			{
				if(axisOrientation != value)
				{
					hasChanged = true;
					axisOrientation = value; 
				}
			}
		}
		
		#endregion

		#region --- Rendering ---

		private bool GetAxes(out Axis Ax, out Axis Ay)
		{
			Ax = Axis;
			Ay = null;
			if(Plane.XAxis.Role == axisOrientation)
				Ay = Plane.YAxis;
			else if(Plane.YAxis.Role == axisOrientation)
				Ay = Plane.XAxis;
			else
				return false; // because the axis has not been found

			return true;			
		}

		internal override void Render()
		{
			if(!this.Visible || Plane == null || !Plane.Visible)
				return;
			StripCollection stripes = Strips;
			if(stripes != null)
			{
				foreach(Strip s in stripes)
					s.Render();
			}
		}

		#endregion

		#region --- Serialization ---
		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }

		#endregion


	}

}
