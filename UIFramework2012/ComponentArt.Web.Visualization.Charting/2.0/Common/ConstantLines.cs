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
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents a single line in a of a <see cref="Grid"/> object.
	/// </summary>
	public class GridLine
	{
		Coordinate	coordinate;
		string		text = "";
		LabelStyle	labelStyle;
		LineStyle2D lineStyle;

		/// <summary>
		/// Initializes a new instance of <see cref="GridLine"/> class.
		/// </summary>
		/// <param name="coordinate">represents the value and location of the grid line.</param>
		public GridLine(Coordinate coordinate)
		{
			this.coordinate = coordinate;
		}
		internal GridLine() : this(null)
		{
		}

		/// <summary>
		/// Gets or sets the label style of this <see cref="GridLine"/> object.
		/// </summary>
		public LabelStyle	LabelStyle		{ get { return labelStyle; }	set { labelStyle = value; } }
		/// <summary>
		/// Gets or sets the line style of this <see cref="GridLine"/> object.
		/// </summary>
		public LineStyle2D	LineStyle		{ get { return lineStyle; }		set { lineStyle = value; } }
		/// <summary>
		/// Gets or sets the label text of this <see cref="GridLine"/> object.
		/// </summary>
		public string		Text			{ get { return text; }			set { text = value; } }

		/// <summary>
		/// Gets the coordinate of this <see cref="GridLine"/> object.
		/// </summary>
		public Coordinate	Coordinate		{ get { return coordinate; } }
	}

	/// <summary>
	/// A collection of <see cref="GridLine"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class GridLineCollection: CollectionWithType 
	{
		internal GridLineCollection() : base(typeof(GridLine)) { }
		
		/// <summary>
		/// Indicates the <see cref="GridLine"/> at the specified indexed location in the <see cref="GridLineCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve the grid line from the <see cref="GridLineCollection"/> object.</param>
		public new GridLine this[object obj]
		{
			get { return (GridLine)(List[IndexOf(obj)]); } 
			set { List[IndexOf(obj)] = value;} 
		}
	}

	/// <summary>
	/// Enumeration type for coordinate <see cref="Grid"/>s. 
	/// </summary>
	/// <remarks>
	/// User created grids have Kind = Custom. 
    /// A grid created by the control will have the kind set to a value other than Custom.
	/// </remarks>
	public enum GridKind
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
	/// Represents the coordinate lines on the <see cref="CoordinatePlane"/> object.
	/// </summary>

	[Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class Grid : ChartObject, INamedObject, IDisposable
	{
		private AxisOrientation axisOrientation;
		private AxisOrientation axisOrientation2;

		private string				lineStyleName = "CoordinateLine";
		private string				labelStyleName = "DefaultAxisLabels";
		private CoordinateSet		coordSet = null;
		private GridLineCollection	lines = null;
		private LineStyle2D			lineStyle = null, lineStyle2 = null;
		private LabelStyle			labelStyle = null;

		private int					numberOfLabels = 0;

		private bool			hasChanged = true;

		#region --- Access via enum type ---

		static string[] names = new string[]
			{
				"XinPlaneXY",
				"YinPlaneXY",
				"YinPlaneYZ",
				"ZinPlaneYZ",
				"ZinPlaneZX",
				"XinPlaneZX",
				"Custom"
			};

		static GridKind[] kinds = new GridKind[]
			{
				GridKind.XinPlaneXY,
				GridKind.YinPlaneXY,
				GridKind.YinPlaneYZ,
				GridKind.ZinPlaneYZ,
				GridKind.ZinPlaneZX,
				GridKind.XinPlaneZX,
				GridKind.Custom
			};

		internal static string NameOf(GridKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'Grid' mismatch");
		}

		internal static GridKind KindOf(string gridName)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(gridName==names[i])
					return kinds[i];
			}
			return GridKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GridKind Kind
		{
			get
			{
				return Grid.KindOf(Name);
			}
			set
			{
				Name = Grid.NameOf(value);
			}
		}

		#endregion

		#region --- Constructor ---

		// Fixme: What to do with this constructor??? single is not reflected anywhere... name???
		internal Grid(string name, string lineStyleName)
		{
			this.Name = name;
			this.lineStyleName = lineStyleName;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Grid"/> class.
		/// </summary>
		public Grid() : this ("") { }
		/// <summary>
		/// Initializes a new instance of the <see cref="Grid"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="Grid"/> object.</param>
		public Grid(string name) 
		{
			Name = name;
		}
		#endregion

		#region --- NamedStyleInternal Interface Implementation
		/// <summary>
		/// Used in NamedStyleInternal implementation.
		/// </summary>
		private NamedStyleInternal m_nsi = new NamedStyleInternal(); 

		/// <summary>
		/// Owning named collection. Used to control name uniqueness.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public NamedCollection OwningCollection { get {return m_nsi.NamedCollection;} set { m_nsi.NamedCollection = value; }	}
		/// <summary>
		/// Axis name.
		/// </summary>
		[SRCategory("CatGeneral")]
		[SRDescription("GridNameDescr")]
		[NotifyParentProperty(true)]
		public string Name { get { return m_nsi.Name; }	set { m_nsi.Name = value; } }
		#endregion

		#region --- Properties ---

		public override string ToString() { return Name; }

		internal Axis Axis { get { return (Plane.XAxis.Role == axisOrientation)? Plane.XAxis:Plane.YAxis; } }

		internal CoordinatePlane Plane { get { return Owner as CoordinatePlane; } }

		internal CoordinateSystem CoordinateSystem { get { return Axis.CoordSystem; } }

		/// <summary>
		/// Gets or sets the minimum number of lines in this <see cref="Grid"/> object.
		/// </summary>
		[SRDescription("GridMinimumNumberOfLinessDescr")]
		[DefaultValue(0)]
		public int MinimumNumberOfLines { get { return numberOfLabels; } set { numberOfLabels = value; coordSet = null; } }
		
		/// <summary>
		/// Gets or sets the orientation of this <see cref="Grid"/> object.
		/// </summary>
		[SRCategory("CatBehavior")]
		[SRDescription("GridAxisOrientationDescr")]
		public AxisOrientation AxisOrientation { get { return axisOrientation; } set { axisOrientation = value; } }

        
		//[Browsable(false)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// Gets or sets the orthogonal orientation of this <see cref="Grid"/> object.
        /// </summary>
        /// <remarks>
        /// This property is absolete and should not be used.
        /// </remarks>
		[SRCategory("CatBehavior")]
		[SRDescription("GridAxisOrientation2Descr")]
		public AxisOrientation AxisOrientation2 { get { return axisOrientation2; } set { axisOrientation2 = value; } }

        /// <summary>
        /// Gets or sets the primary line style name to this <see cref="Grid"/> object.
		/// </summary>
		[SRCategory("CatBehavior")]
		[Description("The primary line style of the coordinate lines")]
		[TypeConverter(typeof(SelectedLineStyle2DConverter))]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public  string LineStyleName			
		{
			get { return lineStyleName; }
			set 
			{ 
				if(lineStyleName != value)
				{
					lineStyleName = value; 
					lineStyle = null; // To force reloading of the new style
					LineStyle2 = null;
					if(OwningChart == null || !(OwningChart.InDesignMode || OwningChart.InitializeOnDataBind))
						return;
					LineStyle2D ls = LineStyle;
					LineStyle2D ls2 = LineStyle2;
					// Updating existing lines
					if(lines != null)
					{
						for(int i=0;i<lines.Count; i++)
						{
							GridLine line = lines[i];
							if(lineStyle != null)
							{
								if(i%2 == 0)
									line.LineStyle = (LineStyle2D)lineStyle.Clone();
								else
									line.LineStyle = (LineStyle2D)lineStyle2.Clone();
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the coordinate set of this <see cref="Grid"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CoordinateSet Coordinates	
		{ 
			get 
			{ 
				if(coordSet == null || OwningChart.InDesignMode )
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
				return coordSet; 
			}				
			set 
			{
				coordSet = value; 
				this.lines = null;
			}
		}

		/// <summary>
		/// Gets or sets the 2D line style to be used in this <see cref="Grid"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyle2D LineStyle 
		{
			get
			{
				if(lineStyle == null)
				{
					lineStyle = OwningChart.LineStyles2D[lineStyleName];
					if(lineStyle != null)
						lineStyle = (LineStyle2D)LineStyle.Clone();
				}
				return lineStyle;
			}
			set
			{
				lineStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the line style of alternate lines in this <see cref="Grid"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyle2D LineStyle2
		{
			get
			{
				if(lineStyle2 == null)
				{
					lineStyle2 = OwningChart.LineStyles2D[lineStyleName];
					if(lineStyle2 == null)
					{
						lineStyle2 = new LineStyle2D();
						lineStyle2 = (LineStyle2D) lineStyle.Clone();
					}
					else
						lineStyle2 = (LineStyle2D)LineStyle2.Clone();
				}
				return lineStyle2;
			}
			set
			{
				lineStyle2 = value;
			}
		}

		internal void DataBind()
		{
			if(OwningChart.InitializeOnDataBind)
			{
				coordSet = null;
				lines = null;
			}
		}

		/// <summary>
		/// Gets the label style to be used in the labels of this <see cref="Grid"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LabelStyle LabelStyle
		{
			get
			{
				if(labelStyle == null)
				{
					LabelStyle s = OwningChart.LabelStyles[labelStyleName];				
					if(s != null)
					{
						labelStyle = new LabelStyle();
						labelStyle.LoadFrom(s);
					}
				}
				return labelStyle;
			}
		}

		/// <summary>
		/// Gets the collection of <see cref="GridLine"/>s within this <see cref="Grid"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GridLineCollection Lines
		{
			get
			{
				if(lines == null || OwningChart.InDesignMode)
				{
					lines = new GridLineCollection();
					lines.SetOwner(this);
					CoordinateSet cs = Coordinates;
					LineStyle2D lineStyle = LineStyle;
					LineStyle2D lineStyle2 = LineStyle2;
					LabelStyle  labelStyle = LabelStyle;
					
					for(int i=0;i<cs.Count; i++)
					{
						GridLine line = new GridLine(cs[i]);
						line.LabelStyle = new LabelStyle();
						if(labelStyle != null)
							line.LabelStyle.LoadFrom(labelStyle);
						if(lineStyle != null)
						{
							if(i%2 == 0)
								line.LineStyle = (LineStyle2D)lineStyle.Clone();
							else
								line.LineStyle = (LineStyle2D)lineStyle2.Clone();
						}
						lines.Add(line);
					}
				}
				return lines;
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
			if(!Visible)
				return;
			CoordinatePlane plane = Plane;
			if(!Plane.Visible)
				return;

			Axis Ax, Ay;
			if(!GetAxes(out Ax, out Ay))
				return;

			double[] coord = new double[0];

			GridLineCollection lines = Lines;

			coord = new Double[lines.Count];
			for(int k=0; k<lines.Count; k++)
				coord[k] = Ax.LCS2ICS(lines[k].Coordinate.Offset);
			
			Geometry.DrawingBoard B = Plane.CreateDrawingBoard();
			B.LiftZ = 2;
			
			Vector3D P,Sx,Sy;
			P = plane.Origin;
			Sx = Ax.UnitVector;
			Sy = Ay.UnitVector;

			DataDimension xDim = Ax.Dimension;

			for(int i=0; i<coord.Length; i++)
			{
				object coordValue = lines[i].Coordinate.Value;
				if(xDim.Compare(coordValue,Ax.MinValue)<0 ||
					xDim.Compare(Ax.MaxValue,coordValue)<0)
					continue; // Skip lines out of range
				LineStyle2D LS = lines[i].LineStyle;
				LS = (LineStyle2D)LS.Clone();
				Color oldColor = LS.Color;
				if(oldColor.A == 0)
					LS.Color = OwningChart.Palette.CoordinateLineColor;
				Pen pen = LS.Pen;
				double xLog0,xLog1,yLog0,yLog1;
				float shift = (float)(0.5/CoordinatePlane.CoordinateSystem.TargetArea.Mapping.Enlargement);
				if(Plane.IsRadial)
				{
					if(Ax == Plane.XAxis)
					{
						Vector3D P0 = Plane.LogicalToWorld(coord[i],Ay.MinValueICS);
						Vector3D P1 = Plane.LogicalToWorld(coord[i],Ay.MaxValueICS);
						B.DrawLine(pen,P0,P1,false);
					}
					else
					{
						Vector3D P0 = Plane.LogicalToWorld(Ay.MinValueICS,coord[i]);
						Vector3D P1 = Plane.LogicalToWorld((Ay.MinValueICS+Ay.MaxValueICS)*0.5,coord[i]);
						Vector3D C = (P0+P1)*0.5;
						double r = (C - P0).Abs;
						GraphicsPath path = new GraphicsPath();
						path.AddEllipse((float)(C.X - r)+shift,(float)(C.Y - r)+shift,2*(float)r,2*(float)r);
						B.DrawPath(LS,path,true,false,true);
					}
				}
				else
				{
					if(Ax == Plane.XAxis)
					{
						xLog0 = coord[i];
						xLog1 = coord[i];						
						yLog0 = Ay.MinValueICS;
						yLog1 = Ay.MaxValueICS;
					}
					else
					{
						yLog0 = coord[i];
						yLog1 = coord[i];
						xLog0 = Ay.MinValueICS;
						xLog1 = Ay.MaxValueICS;
					}

					Vector3D T0 = Plane.LogicalToWorld(xLog0,yLog0);
					Vector3D T1 = Plane.LogicalToWorld(xLog1,yLog1);
					Vector3D TS = (T0+T1)*0.5;
					B.DrawLine(pen,T0,T1,false);
					if(lines[i].Text != null && lines[i].Text != "")
					{
						Vector3D T = Vector3D.Null;
						LabelStyle labStyle = lines[i].LabelStyle;
						if(labStyle.ForeColor.A == 0)
							labStyle.ForeColor = OwningChart.Palette.CoodinateLabelFontColor;
						switch(labStyle.ReferencePoint)
						{
							case TextReferencePoint.Center:
								T = TS;
								break;
							case TextReferencePoint.CenterBottom:
								T = TS;
								break;
							case TextReferencePoint.CenterTop:
								T = TS;
								break;
							case TextReferencePoint.LeftBottom:
								T = T0;
								break;
							case TextReferencePoint.LeftCenter:
								T = T0;
								break;
							case TextReferencePoint.LeftTop:
								T = T0;
								break;
							case TextReferencePoint.RightBottom:
								T = T1;
								break;
							case TextReferencePoint.RightCenter:
								T = T1;
								break;
							case TextReferencePoint.RightTop:
								T = T1;
								break;
							default:
								T = TS;
								labStyle.ReferencePoint = TextReferencePoint.CenterBottom;
								break;
						}
                        GE.CreateText(labStyle,T,lines[i].Text);
					}
				}
			}
		}
		#endregion

		#region --- Serialization ---
		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }

		#endregion

		internal override void SetContext(Object obj)
		{
			base.SetContext(obj);
			if (LineStyleName == "")
				LineStyleName = OwningChart.LineStyles2D[0].Name;
		}

		internal CoordinatePlane CoordinatePlane
		{
			get 
			{
				if (OwningCollection == null)
					return null;
				return (CoordinatePlane)OwningCollection.Owner;
			}
		}

		#region IDisposable Members

		internal void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (labelStyle != null) 
				{
					labelStyle.Dispose();
					labelStyle = null;
				}
				if (lineStyle2 != null) 
				{
					lineStyle2.Dispose();
					lineStyle2 = null;
				}
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

	}

}