using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using ComponentArt.Web.Visualization.Charting.Design;
using System.Reflection;


namespace ComponentArt.Web.Visualization.Charting
{
	// ============================================================================================
	/// <summary>
	/// Implements mapping from the "world coordinate system" into the target coordinate system.
	/// The target system is 3D extension of the bitmap coordinate system.
	/// 
	/// </summary>
	[Serializable()]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class Mapping : ICloneable
	{
		internal enum ProjectionPlaneKind
		{
			PlaneXY,
			PlaneZY,
			PlaneXZ
		};

		// Domain and Target Sizes
		private const string	_domainSizeDefault = "(100,80,40)";
		private const string	_targetSizeDefault = "600,400";
		private const string	_nativeSizeDefault = "600,400";
		private const string	_viewDirectionDefault = "(10,7,20)";

		// This is needed for WCS range computation on automatic setup
		private bool isPieDoughnut = false;

		internal static Vector3D		defaultViewDirectionLinear = new Vector3D(_viewDirectionDefault);
		internal static Vector3D		defaultViewDirectionRadial = new Vector3D(0,25,25);

		private const double _xReductionOfZAxisDefault=0.5,_yReductionOfZAxisDefault=0.3;

		private bool			_lightsOffOn2D = true;
		private ProjectionKind	_kind=ProjectionKind.CentralProjection;	// Projection kind
		private double			_perspectiveFactor;					// Perspective factor for central projection
		private double			_xReductionOfZAxis=_xReductionOfZAxisDefault;
		private double			_yReductionOfZAxis=_yReductionOfZAxisDefault;	// Reduced coordinates for isometric projection
		
		private Vector3D	_domainSize = (Vector3D)new Vector3DConverter().ConvertFromInvariantString(_domainSizeDefault);
		private Size		_targetSize = (Size)new System.Drawing.SizeConverter().ConvertFromInvariantString(_targetSizeDefault);
		private Size		_nativeSize = (Size)new System.Drawing.SizeConverter().ConvertFromInvariantString(_nativeSizeDefault);

		// Margines
		private MappingMargins margin = new MappingMargins();

		// Target rectangle relative parameters
		private double relativeX = 0, relativeY = 0, relativeW = 1, relativeH = 1;

		// DPI
		private double		_dpi = 96; 

		// Post-projection adjustment
		private double xOld = 0, yOld = 0, xNew = 0, yNew = 0, ppScale = 1;

		// Working mapping data
		private Matrix3x3	_m = new Matrix3x3();
		private Vector3D	_v;
		private Vector3D	_center;		// Projection center in target coordinate system

		private Vector3D	_projWCenter = new Vector3D(10,10,20);	// Projection center in the world coordinate system (for central projection)

		private ProjectionPlaneKind _projectionPlane = ProjectionPlaneKind.PlaneXY; // Projection plane for 2D projection

		// Caching inverse and enlargement
		private Mapping		_inverse;
		private double		_enlargementX, _enlargementY, _enlargementZ;
		private double		_enlargement;

		// Lazy mapping setup management
		private bool		_isFresh = false;
		private bool		isViewDirectionDefault = true;

		// Bending
		private bool		bendingNeeded = false;

		private ChartBase m_chart=null;

		#region --- Construction and Clonning ---

		internal Shader.Mapping ToCoreMapping()
		{
			return new Shader.Mapping(_m.V1,_m.V2,_m.V3,_v,_center,(int)_kind,_targetSize,_enlargement,_lightsOffOn2D,xNew,yNew,xOld,yOld,bendingNeeded,ppScale);
		}
		/// <summary>
		/// Initialises a new instance of the <see cref="Mapping"/> class with default parameters.
		/// </summary>
		public Mapping()
		{
			Perspective = 50;
			// Identity mapping
			_m = new Matrix3x3();
			_v = new Vector3D();
			InitCache();
		}

		internal Mapping(Matrix3x3 m, Vector3D v, double perspectiveFactor)
		{
			_m = new Matrix3x3(m);
			_v = new Vector3D(v);
			_perspectiveFactor = perspectiveFactor;
			if(perspectiveFactor>1.0)
				_kind = ProjectionKind.CentralProjection;
			else
				_kind = ProjectionKind.ParallelProjection;
			InitCache();
		}

		/// <summary>
		/// Return an exact copy of this <see cref="Mapping"/> object.
		/// </summary>
		/// <returns>An exact copy of this <see cref="Mapping"/> object.</returns>
		public object Clone()
		{
			Mapping clone = new Mapping();
			clone._center = _center;
			clone._domainSize = _domainSize;
			clone._dpi = _dpi;
			clone._enlargement = _enlargement;
			clone._enlargementX = _enlargementX;
			clone._enlargementY = _enlargementY;
			clone._enlargementZ = _enlargementZ;
			clone._inverse = null;
			clone._isFresh = false;
			clone._kind = _kind;
			clone._lightsOffOn2D = _lightsOffOn2D;
			clone._m = _m;
			clone._nativeSize = _nativeSize;
			clone._perspectiveFactor = _perspectiveFactor;
			clone._projectionPlane = _projectionPlane;
			clone._projWCenter = _projWCenter;
            clone.isViewDirectionDefault = isViewDirectionDefault;
			clone._targetSize = _targetSize;
			clone._v = _v;
			clone._xReductionOfZAxis = _xReductionOfZAxis;
			clone._yReductionOfZAxis = _yReductionOfZAxis;
			clone.m_chart = m_chart;
			clone.margin.Left = margin.Left;
			clone.margin.Right = margin.Right;
			clone.margin.Top = margin.Top;
			clone.margin.Bottom = margin.Bottom;

			return clone;
		}
		#endregion

		#region --- Properties ---

		internal bool BendingNeeded { get { return bendingNeeded; } set { bendingNeeded = value;} }

			/// <summary>
			/// Gets or sets the size of the chart space in the World Coordinate System.
			/// </summary>
			/// <remarks>
			/// This property is computed by the control and should not be changed.
			/// </remarks>
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			[DefaultValue(typeof(Vector3D), _domainSizeDefault)]
			public Vector3D	DomainSize
		{
			get { return _domainSize; } 
			set 
			{ 
				if(_domainSize != value)
					_isFresh = false;
				_domainSize = value; 
			}
		}

		/// <summary>
		/// Gets or sets the size of the chart space in the Target Coordinate System.
		/// </summary>
		/// <remarks>
		/// This property is computed by the control and should not be changed.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(typeof(Size), _targetSizeDefault)]
		public Size     TargetSize  
		{
			get { return _targetSize; } 
			set 
			{
				if(value != _targetSize) _isFresh = false; 
				_targetSize = value; 
			} 
		}

		internal bool IsPieDoughnut { get { return isPieDoughnut; } set { isPieDoughnut = value; } }
		
		#region --- Size Transformation ---
		internal double FromWorldToTarget	{ get { return Enlargement; } }
		internal double FromTargetToWorld	{ get { return 1.0/Enlargement; } }
		internal double FromPointToTarget	{ get { return DPI/72.0; } }
		internal double FromPointToWorld	
		{ 
			get 
			{
				return FromPointToTarget*FromTargetToWorld; 
			}
		}

		#endregion

		#region --- Category"Viewing and Mapping Parameters" ---

		/// <summary>
		/// Sets or gets the perspective factor of the image
		/// </summary>
		[
			Description("The perspective factor in percentages. Applicable when the Projection Kind is set to 3Dâ€“Perspective."), 
			Category("Viewing and Mapping Parameters"),
			DefaultValue(2.5),
			NotifyParentProperty(true)
		]
		public int			Perspective	
		{ 
			get 
			{ 
				double a = 0.925/150.0, b = 0.05;
				return (int) ((1.0/_perspectiveFactor-b)/a+0.5); 
			}	
			set 
			{
				double a = 0.925/150.0, b = 0.05;
				int ivalue = Math.Max(0,Math.Min(100,value));
				double pf = 1.0/(a*ivalue+b);
				_isFresh = Math.Abs(_perspectiveFactor-pf) < 0.02;
				_perspectiveFactor = pf; 
			}
		}
		internal double			PerspectiveFactor	{ get { return _perspectiveFactor; }	set { _isFresh = _perspectiveFactor == value;_perspectiveFactor = value; } }
		
		/// <summary>
		/// Sets or gets the view direction of the chart image
		/// </summary>
		[	
			Bindable(true),
			NotifyParentProperty(true),
			RefreshProperties(RefreshProperties.All),
			Description("View point vector"), 
			Category("Viewing and Mapping Parameters")			
			,DefaultValue(typeof(Vector3D),_viewDirectionDefault)
        ]
#if !__BUILDING_CRI__
        [Editor(typeof(ViewPointTrackballEditor), typeof(System.Drawing.Design.UITypeEditor))]
#endif
        public Vector3D			ViewDirection		
		{
			get 
			{ 
				if(isViewDirectionDefault)
					return defaultViewDirectionLinear;
				return _projWCenter; 
			}			
			set 
			{
				if(value.IsNull) return;
				isViewDirectionDefault = false;
				_isFresh = _projWCenter == value;  
				_projWCenter = value;

				if (!_isFresh) 
				{
					OnViewDirectionChanged(EventArgs.Empty);
				}
			}
		}

		internal bool IsViewDirectionDefault 
		{ 
			get { return isViewDirectionDefault; } 
			set { isViewDirectionDefault = value; } 
		}


		/// <summary>
		/// Gets or sets a value that indicates whether lights are turned off in 2D projection
		/// </summary>
		[
			Description("Turn off directed lights when in 2D projection"), 
			Category("Viewing and Mapping Parameters"),
			DefaultValue(true),
			NotifyParentProperty(true)
		]
		public   bool	LightsOffOn2D       { get { return _lightsOffOn2D; } set { _lightsOffOn2D = value; } }
						
		/// <summary>
		/// Gets or sets the reduction factor for X coordinate of Z -axis in isometric projection.
		/// </summary>
		[
			Description("Reduction factor for X coordinate of Z -axis in isometric projection."), 
			Category("Viewing and Mapping Parameters"),
			DefaultValue(_xReductionOfZAxisDefault),
			NotifyParentProperty(true)
		]
		public double			XReductionOfZAxis	{ get { return _xReductionOfZAxis; }	set { _isFresh = _xReductionOfZAxis == value; _xReductionOfZAxis = value; } }
		
		/// <summary>
		/// Gets or sets the reduction factor for Y coordinate of Z -axis in isometric projection.
		/// </summary>
		[
			Description("Reduction factor for Y coordinate of Z -axis in isometric projection"), 
			Category("Viewing and Mapping Parameters"),
			DefaultValue(_yReductionOfZAxisDefault),
			NotifyParentProperty(true)
		]
		public double			YReductionOfZAxis	{ get { return _yReductionOfZAxis; }	set { _isFresh = _yReductionOfZAxis == value; _yReductionOfZAxis = value; } }

		/// <summary>
		/// Gets or sets the projection kind of this chart.
		/// </summary>
		[
		Description("3D-Perspective, 3D-Isometric, 3D-Parallel, or 2D. Lights can be turned on or off in 2D projection."), 
		Category("Viewing and Mapping Parameters"),
		RefreshProperties(RefreshProperties.All),
		DefaultValue(ProjectionKind.CentralProjection),
		NotifyParentProperty(true),
		TypeConverter(typeof(EnumDescConverter))
		]
		public ProjectionKind	Kind				
		{
			get { return _kind; }
			set 
			{
				_isFresh = _kind == value; 
				_kind = value; 
				
				if (!_isFresh && _kind == ProjectionKind.TwoDimensional && Chart != null)
				{
					Chart.Series.CoordSystem.Orientation = CoordinateSystemOrientation.Default;
				}

				if(!_isFresh && MappingKindChanged != null) 
					MappingKindChanged(this,new EventArgs()); 
			}
		}

		internal Vector3D		TargetCoordinatesOfProjectionCenter { get { return _center; } }

		#endregion

		#region --- Margins ---

		/// <summary>
		/// Gets or sets the space margins around the chart.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[NotifyParentProperty(true)]
		[Description("Specifies the margins around the chart")]
		public MappingMargins Margins		{ get { return margin; } set { margin = value; } }
		#endregion

		#region --- Category "Scaling" ---

		/// <summary>
		/// Gets or sets the native size, used in scaling of point measured entities.
		/// </summary>
			[
			Description("Native size, used in scaling of point measured entities"), 
			Category("Scaling"),
			NotifyParentProperty(true),RefreshProperties(RefreshProperties.All),
			DefaultValueAttribute(typeof(Size), _nativeSizeDefault)
			]
			public Size	  NativeSize			{ get { return _nativeSize; }	set { _nativeSize = value; } }
		#endregion

		internal bool	LightsOff           { get { return _lightsOffOn2D && _kind==ProjectionKind.TwoDimensional; } }

		internal ChartBase Chart 
		{
			set {m_chart = value;}
			get {return m_chart;}
		}

		/// <summary>
		/// Gets the approximate scale factor between distances in World Coordinate System (WCS) and Target Coordinate System (TCS) along the X-axis.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double EnlargementX	{ get { if( !_isFresh || _enlargement <= 0)ForceRefresh();	return _enlargementX; } }
		/// <summary>
		/// Gets the approximate scale factor between distances in World Coordinate System (WCS) and Target Coordinate System (TCS) along the Y-axis.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double EnlargementY	{ get { if( !_isFresh || _enlargement <= 0)ForceRefresh();	return _enlargementY; } }
		/// <summary>
		/// Gets the approximate scale factor between distances in World Coordinate System (WCS) and Target Coordinate System (TCS) along the Z-axis.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double EnlargementZ	{ get { if( !_isFresh || _enlargement <= 0)ForceRefresh();	return _enlargementZ; } }
		/// <summary>
		/// Gets the average of the approximate scale factors between distances in World Coordinate System (WCS) and Target Coordinate System (TCS) along all three axes.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double Enlargement 	{ get { if( !_isFresh || _enlargement <= 0)ForceRefresh();	return _enlargement ; } }

		/// <summary>
		/// Gets or sets dot per inch of the target bitmap.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[NotifyParentProperty(true)]
		public double DPI			{ get { return _dpi; } set { if(_dpi != value) _isFresh = false; _dpi = value; } }

		internal double M11 { get { return _m.V1.X; } }
		internal double M12 { get { return _m.V1.Y; } }
		internal double M13 { get { return _m.V1.Z; } }
		internal double M21 { get { return _m.V2.X; } }
		internal double M22 { get { return _m.V2.Y; } }
		internal double M23 { get { return _m.V2.Z; } }
		internal double M31 { get { return _m.V3.X; } }
		internal double M32 { get { return _m.V3.Y; } }
		internal double M33 { get { return _m.V3.Z; } }
		internal double V1  { get { return _v.X; } }
		internal double V2  { get { return _v.Y; } }
		internal double V3  { get { return _v.Z; } }

		internal Vector3D Center { get { return _center; } }
		internal ProjectionPlaneKind ProjectionPlane { get { return _projectionPlane; } set { _projectionPlane = value; _isFresh = false; } }

		#endregion

		#region --- Mapping transformations ---
		// Scaling
		internal Mapping Scale(double p)
		{
			_m = _m*p;
			_v = _v*p;
			_center = _center*p;
			_isFresh = false;
			return this;
		}

		// Translation
		internal Mapping Translate(Vector3D v)
		{
			_v = _v + v;
			_isFresh = false;
			return this;
		}

		// Rotations
		internal Mapping RotateX(double angleRadians)
		{
			Refresh();
			double cs = Math.Cos(angleRadians);
			double sn = Math.Sin(angleRadians);
			Matrix3x3 mat = new Matrix3x3(
				new Vector3D(1.0, 0.0, 0.0),
				new Vector3D(0.0, cs,-sn),
				new Vector3D(0.0, sn, cs));
			Mapping m = new Mapping();
			m._m = mat;
			Apply(m,this);
			_isFresh = false;

			return this;
		}

		internal Mapping RotateY(double angleRadians)
		{
			Refresh();
			double cs = Math.Cos(angleRadians);
			double sn = Math.Sin(angleRadians);
			Matrix3x3 mat = new Matrix3x3(
				new Vector3D( cs, 0.0,-sn),
				new Vector3D( 0.0, 1.0, 0.0),
				new Vector3D( sn, 0.0, cs));
			Mapping m = new Mapping();
			m._m = mat;
			Apply(m,this);
			_isFresh = false;
			return this;
		}

		internal Mapping RotateZ(double angleRadians)
		{
			Refresh();
			double cs = Math.Cos(angleRadians);
			double sn = Math.Sin(angleRadians);
			Matrix3x3 mat = new Matrix3x3(
				new Vector3D( cs,-sn, 0.0),
				new Vector3D( sn, cs, 0.0),
				new Vector3D( 0.0, 0.0, 1.0));
			Mapping m = new Mapping();
			m._m = mat;
			Apply(m,this);
			_isFresh = false;
			return this;
		}

		// Other transformations
		internal Mapping FlipY(double yMax)
		{
			// Modifies Mapping to get
			//		(x,y,z) --> (x,yMax-y,z)
			// Convenient to handle upside-down target coordinates

			Refresh();
			_m.V2 = -_m.V2;
			_v = new Vector3D(_v.X,-_v.Y+yMax,_v.Z);
			_isFresh = false;
			return this;
		}

		internal Mapping Inverse()
		{
			Refresh();
			return _inverse;
		}

		#endregion

		#region --- Post-projection Transformation ---
		internal void SetAfterProjectionParameters(double xOld,double yOld,double xNew,double yNew,double f)
		{
			this.xOld = xOld;
			this.yOld = yOld;
			this.xNew = xNew;
			this.yNew = yNew;
			this.ppScale = f;
		}

		internal void ResetAfterProjectionTransformation()
		{
			xOld = 0;
			yOld = 0;
			xNew = 0;
			yNew = 0;
			ppScale = 1;
		}
		#endregion

		#region --- Mapping ---
		
		internal Vector3D MapOrtho(Vector3D vInput)
		{
			Refresh();
			return _m*vInput + _v;
		}
		
		internal void MapOrtho(Vector3D vInput,out Vector3D vOutput)
		{
			Refresh();
			vOutput = _m*vInput + _v;
		}

		internal void MapHomo(Vector3D v, out double x, out double y)
		{
			Refresh();
			if(Kind == ProjectionKind.CentralProjection)
			{
				double reduction = _center.Z/(_center.Z-v.Z);
				x = _center.X + (v.X - _center.X)*reduction;
				y = _center.Y + (v.Y - _center.Y)*reduction;
			}
			else
			{
				x = v.X;
				y = v.Y;
			}
			x = xNew + (x-xOld)*ppScale;
			y = yNew + (y-yOld)*ppScale;
		}
		
		/// <summary>
		/// Maps a vector in the World Coordinate System to Target Coordinate system.
		/// </summary>
		/// <param name="vInput"></param>
		/// <returns></returns>
		public Vector3D Map(Vector3D vInput)
		{
			if(_v.X==double.NaN)
				throw new Exception("Invalid mapping data");
			Refresh();
			Vector3D vOutput = _m*vInput + _v;
			if(Kind == ProjectionKind.CentralProjection)
			{
				double reduction = _center.Z/(_center.Z-vOutput.Z);
				double dx = (vOutput.X - _center.X)*reduction;
				double dy = (vOutput.Y - _center.Y)*reduction;
				double xp = _center.X + dx;
				double yp = _center.Y + dy;
				xp = xNew + (xp - xOld)*ppScale;
				yp = yNew + (yp - yOld)*ppScale;
				if(bendingNeeded)
				{
					// We introduce a small bending-down amount to get z slightly 
					// smaller as the point is further from the center.
					// This is needed for proper ordering of objects that would otherwise
					// have the same z-coordinate, like boxes in coordinate planes.
					// See defect 5230.
					int tz1 = _targetSize.Width + _targetSize.Height;
					double bendingAmount = (dx*dx+dy*dy)/(tz1*tz1);
					vOutput = new Vector3D(xp, yp, vOutput.Z*ppScale - bendingAmount);
				}
				else
					vOutput = new Vector3D(xp, yp, vOutput.Z*ppScale);
			}
			else
			{
				double xp = xNew + (vOutput.X - xOld)*ppScale;
				double yp = yNew + (vOutput.Y - yOld)*ppScale;
				vOutput = new Vector3D(xp,yp,vOutput.Z*ppScale);
			}
			return vOutput;
		}

		internal Vector3D MapNoPostProjection(Vector3D vInput)
		{
			if(_v.X==double.NaN)
				throw new Exception("Invalid mapping data");
			Refresh();
			Vector3D vOutput = _m*vInput + _v;
			if(Kind == ProjectionKind.CentralProjection)
			{
				double reduction = _center.Z/(_center.Z-vOutput.Z);
				double dx = (vOutput.X - _center.X)*reduction;
				double dy = (vOutput.Y - _center.Y)*reduction;
				double xp = _center.X + dx;
				double yp = _center.Y + dy;
				vOutput = new Vector3D(xp, yp, vOutput.Z);
			}
			return vOutput;
		}

		internal void Map(Vector3D vInput, out Vector3D vOutput)
		{
			vOutput = Map(vInput);
		}
		
		internal Vector3D MapDirectionUnit(Vector3D vInput)
		{
			return (MapOrtho(vInput) - MapOrtho(Vector3D.Null)).Unit();
		}

		internal PointF PostProjectionMap(double x, double y)
		{
			float xp = (float)(xNew + (x - xOld)*ppScale);
			float yp = (float)(yNew + (y - yOld)*ppScale);
			return new PointF(xp,yp);
		}

		#endregion

		#region --- Private methods ---
		/// <summary>
		/// <para>Occurs when the <see cref="ViewDirection" /> property value changes.</para>
		/// </summary>
		public event EventHandler ViewDirectionChanged;	
		/// <summary>
		/// <para>Raises the <see cref="ViewDirectionChanged" /> event.</para>
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		void OnViewDirectionChanged(EventArgs e)
		{
			if (ViewDirectionChanged!=null)
				ViewDirectionChanged(this, e);
		}

		private void SetIdentity()
		{
			_m = new Matrix3x3();
			_v = new Vector3D();
			_isFresh = false;
		}

		internal Rectangle EffectiveTarget()
		{
			return new Rectangle(
				(int)(_targetSize.Width*relativeX),
				(int)(_targetSize.Height*(1.0-relativeY-relativeH)), // this mumbo-jumbo is needed because of y-coordinate flipping
				(int)(_targetSize.Width*relativeW),
				(int)(_targetSize.Height*relativeH)
				);
		}

		internal void Setup(Vector3D domainSize, double relativeX, double relativeY, double relativeW, double relativeH)
		{
			DomainSize = domainSize;
			this.relativeX = relativeX;
			this.relativeY = relativeY;
			this.relativeW = relativeW;
			this.relativeH = relativeH;
			DefaultSetup();
		}

		internal void GetRelativeTargetParameters(out Vector3D domainSize, out double x, out double y, out double width, out double height)
		{
			domainSize = this.DomainSize;
			x = relativeX;
			y = relativeY;
			height = relativeH;
			width  = relativeW;
		}

		internal void DefaultSetup()
		{
			Rectangle eT = EffectiveTarget();
			double xpMin,xpMax,ypMin,ypMax;
			
			double ml = margin.Left*0.01;
			double mr = margin.Right*0.01;
			double mt = margin.Bottom*0.01;	
			double mb = margin.Top*0.01;
			ml = mr = mt = mb = 0;
			xpMin = eT.X + eT.Width*ml;
			ypMin = eT.Y + eT.Height*mb;		
			xpMax = eT.X + eT.Width*(1.0-mr);
			ypMax = eT.Y + eT.Height*(1.0-mt);
			
			xOld = 0;
			yOld = 0;
			xNew = 0;
			xNew = 0;
			ppScale = 1.0;

			DefaultMapping(xpMin,xpMax,ypMin,ypMax,_perspectiveFactor);
		}

		internal Mapping DefaultMapping(double xpMin, double xpMax, double ypMin, double ypMax, double projectionFactor)
		{
			if(_kind == ProjectionKind.CentralProjection || _kind == ProjectionKind.ParallelProjection)
				return DefaultCentralAndParallelMapping(xpMin,xpMax,ypMin,ypMax,projectionFactor);
			else if (_kind == ProjectionKind.TwoDimensional)
				return Default2DMapping(xpMin,xpMax,ypMin,ypMax);
			else
				return DefaultIsometricMapping(xpMin,xpMax,ypMin,ypMax);
		}

		internal Mapping DefaultCentralAndParallelMapping(double xpMin, double xpMax, double ypMin, double ypMax, double projectionFactor)
		{
			if(ViewDirection.IsNull)
			{
				StackTrace st1 = new StackTrace(true);
				Console.WriteLine(" Stack trace for this level: {0}",
					st1.ToString());
			}
			ProjectionKind kind = _kind;
			Mapping m = new Mapping();
			SetIdentity();
	
			// 1. Translate to the original domain center

			Vector3D R = _domainSize*0.5;
			double domainRadius = R.Abs;
			_isFresh = true;
			Translate(-R);
			_isFresh = true; // we have to override standard behaviour while setting up the mapping
	
			// 2. Rotate so that the viewing vector becomes vertical

			// 2.1 Rotate around z-axis first
			Vector3D Vp;
			Vp = _projWCenter;	
			if(Vp.X != 0.0 || Vp.Y != 0.0)
			{
				double alpha = Math.Atan2(Vp.Y,Vp.X);
				m._isFresh = true;
				m.RotateZ(-alpha);
				m._isFresh = true;
				m.Apply(this,this);
				_isFresh = true;
				m._isFresh = true;
				m.MapOrtho(Vp,out Vp);
				// The y-coordinate of Vp is 0 at this point, i.e. Vp is in the x-z plane.

				// 2.2 Rotate around y -axis to make parallel to the z -axis.
				double beta = Math.Atan2(Vp.X,Vp.Z);
				m.SetIdentity();
				m._isFresh = true;
				m.RotateY(beta);
				m._isFresh = true;
				m.MapOrtho(Vp, out Vp); // This is needed for test only
				// Both x and y coordinates of Vp are 0 at this point (or close)
				m.Apply(this,this);
				_isFresh = true;
			}
			// else the vector is already parallel to the z -axis.

			// 3. The projection center is on positive part of z -axis at this point

			_center = new Vector3D(0,0,projectionFactor*domainRadius);

			// 4. Rotate the whole picture around z -axis so that projection of Y is vertical.
			// We may apply another rotation around z -axis as well.
			// This doesn't move the projection center.

			Vector3D Yp0,Yp1;
			Map(R,out Yp0);
			Map(R+new Vector3D(0,1,0),out Yp1);
			Vector3D Yp = Yp1-Yp0;
			if(Yp.X != 0.0 || Yp.Y != 0.0)
			{
				double gamma = Math.Atan2(Yp.X,Yp.Y);
				m.SetIdentity();
				m._isFresh = true;
				m.RotateZ(gamma);
				m._isFresh = true;
				m.MapOrtho(Yp,out Yp); // This is needed for test only
				// x -coordinate of Yp should be 0 at this point (or close)
				m.Apply(this,this);
				_isFresh = true;
			}

			// 5. Scale to fit the size of the inner target rectangle

			_kind = kind;
			Vector3D minCoord, maxCoord;
			GetExtremeCoords(_domainSize, out minCoord, out maxCoord);
			// min coord should be negative max coordinates here
			double scaleX = (xpMax-xpMin)/(maxCoord.X-minCoord.X);
			double scaleY = (ypMax-ypMin)/(maxCoord.Y-minCoord.Y);
			double scale = Math.Min(scaleX,scaleY);
			Scale(scale); // Center is scaled here!
			_isFresh = true;
			

			// 6. Translate
			GetExtremeCoords(_domainSize, out minCoord, out maxCoord);
			Vector3D Vt = new Vector3D((xpMax+xpMin-(maxCoord.X+minCoord.X))*0.5,(ypMax+ypMin-(maxCoord.Y+minCoord.Y))*0.5,0.0);
			Translate(Vt);
			_isFresh = true;
			_center = _center + Vt;

			return this;
		}

		internal Mapping Default2DMapping(double xpMin, double xpMax, double ypMin, double ypMax)
		{
			if(_projectionPlane == ProjectionPlaneKind.PlaneXY)
				return Default2DMappingXY(xpMin, xpMax, ypMin, ypMax);
			else if(_projectionPlane == ProjectionPlaneKind.PlaneXZ)
				return Default2DMappingXZ(xpMin, xpMax, ypMin, ypMax);
			else 
				return Default2DMappingXY(xpMin, xpMax, ypMin, ypMax);
		}

		internal Mapping Default2DMappingXY(double xpMin, double xpMax, double ypMin, double ypMax)
		{// Projection onto XY -plane
			SetIdentity();
			_isFresh = true;
			
			// Target dimension
			double dxP = xpMax-xpMin;
			double dyP = ypMax-ypMin;

			// Scale to the target size

			double scaleX = dxP/_domainSize.X;
			double scaleY = dyP/_domainSize.Y;
			Scale(Math.Min(scaleX,scaleY));
			_isFresh = true;

			// Translate to the target center

			// Map the domain center
			Vector3D cMap;
			Map(_domainSize/2.0,out cMap);
			Translate(new Vector3D((xpMax+xpMin)/2-cMap.X, (ypMax+ypMin)/2-cMap.Y,0));
			_isFresh = true;

			return this;
		}

		internal Mapping Default2DMappingXZ(double xpMin, double xpMax, double ypMin, double ypMax)
		{// Projection onto XY -plane
			SetIdentity();
			_m.V2 = new Vector3D(0,0,-1);
			_m.V3 = new Vector3D(0,1,0);
			_v.Y  = DomainSize.Z;
			_isFresh = true;
			
			// Target dimension
			double dxP = xpMax-xpMin;
			double dyP = ypMax-ypMin;

			// Scale to the target size

			double scaleX = dxP/_domainSize.X;
			double scaleY = dyP/_domainSize.Z;
			Scale(Math.Min(scaleX,scaleY));
			_isFresh = true;

			// Translate to the target center

			// Map the domain center
			Vector3D cMap;
			Map(_domainSize/2.0,out cMap);
			Translate(new Vector3D((xpMax+xpMin)/2-cMap.X, (ypMax+ypMin)/2-cMap.Y, 0));//(ypMax+ypMin)/2-cMap.Z));
			_isFresh = true;

			return this;
		}

		internal Mapping DefaultIsometricMapping(double xpMin, double xpMax, double ypMin, double ypMax)
		{
			SetIdentity();
			_isFresh = true;
			
			// First isometric mapping
			//		x --> x - _xReductionOfZAxis*z
			//		y --> y - _yReductionOfZAxis*z
			_m.V1.Z = -_xReductionOfZAxis;
			_m.V2.Z = -_yReductionOfZAxis;
			
			// Target dimension
			double dxP = xpMax-xpMin;
			double dyP = ypMax-ypMin;

			// First isometric extremes
			Vector3D minCoord, maxCoord;
			GetExtremeCoords(_domainSize, out minCoord, out maxCoord);

			// Scale to the target size

			double scaleX = dxP/(maxCoord.X - minCoord.X);
			double scaleY = dyP/(maxCoord.Y - minCoord.Y);
			Scale(Math.Min(scaleX,scaleY));
			_isFresh = true;

			// Translate to the target center

			// Map the domain center
			Vector3D cMap;
			Map(_domainSize/2.0,out cMap);
			Translate(new Vector3D((xpMax+xpMin)/2-cMap.X, (ypMax+ypMin)/2-cMap.Y,0));
			_isFresh = true;

			return this;
		}

		private void InitCache()
		{
			_inverse = null;
			_enlargementX = -1;
			_enlargementY = -1;
			_enlargementZ = -1;
		}

		private void Apply(Mapping map2, Mapping mapOutput)
		{
			// resulting Mapping
			//		T(x) = T1(T2(x)) = m1*(m2*x+v2)+v1 = (m1*m2)*x + (m1*v2 + v1)
			mapOutput._v = _m*map2._v + _v;
			mapOutput._m = _m*map2._m;	
		}

		private void GetExtremeCoords(Vector3D size, out Vector3D minCoords, out Vector3D maxCoords)
		{
			double x0, x1, y0, y1, z0, z1;
			Vector3D v = new Vector3D(),vr;

			// Initial values
			x0 = double.MaxValue; x1 = double.MinValue;
			y0 = double.MaxValue; y1 = double.MinValue;
			z0 = double.MaxValue; z1 = double.MinValue;

			if(isPieDoughnut)
			{
				Vector3D C = size/2;
				double r = size.X/2;
				int n = 20;
				double da = 2*Math.PI/n;
				for(int i = 0; i<n; i++)
				{
					v.X = C.X + r*Math.Cos(i*da);
					v.Z = C.Z + r*Math.Sin(i*da);
					for (int k=0; k<2; k++)
					{
						v.Y = k*size.Y;
						Map(v,out vr);
						x0 = Math.Min(vr.X, x0); x1 = Math.Max(vr.X, x1);
						y0 = Math.Min(vr.Y, y0); y1 = Math.Max(vr.Y, y1);
						z0 = Math.Min(vr.Z, z0); z1 = Math.Max(vr.Z, z1);
					}
				}
			}
			else
			{
				for (int i=0; i<2; i++)
					for (int j=0; j<2; j++)
						for (int k=0; k<2; k++)
						{
							v.X = i*size.X;
							v.Y = j*size.Y;
							v.Z = k*size.Z;
							Map(v,out vr);
							x0 = Math.Min(vr.X, x0); x1 = Math.Max(vr.X, x1);
							y0 = Math.Min(vr.Y, y0); y1 = Math.Max(vr.Y, y1);
							z0 = Math.Min(vr.Z, z0); z1 = Math.Max(vr.Z, z1);
						}
			}
			minCoords = new Vector3D(x0,y0,z0);
			maxCoords = new Vector3D(x1,y1,z1);
		}

		internal void ForceRefresh()
		{
			_isFresh = false;
			Refresh();
		}

		internal void Refresh()
		{
			if(_isFresh)
				return;
			_isFresh = true;
			// Do mapping setup
			DefaultSetup();
			// Compute inverse
			ComputeInverse();
			// Compute enlargements
			_enlargementX = Math.Sqrt(_m.V1.X * _m.V1.X + _m.V2.X * _m.V2.X + _m.V3.X *_m.V3.X);
			_enlargementY = Math.Sqrt(_m.V1.Y * _m.V1.Y + _m.V2.Y * _m.V2.Y + _m.V3.Y *_m.V3.Y);
			_enlargementZ = Math.Sqrt(_m.V1.Z * _m.V1.Z + _m.V2.Z * _m.V2.Z + _m.V3.Z *_m.V3.Z);
			_enlargement = (_enlargementX+_enlargementY+_enlargementZ)/3.0;

		}
		private void ComputeInverse()
		{
			Matrix3x3 mInverse = _m.Inverse();
			Vector3D tInverse = -(mInverse * _v);
			_inverse = new Mapping(mInverse,tInverse,0.0);
			// vectors that depend on inverse
			_inverse._isFresh = true;
			_inverse.Map(_center, out _inverse._projWCenter);
			Vector3D v1 = new Vector3D(0,0,-1);
			_inverse.Map(v1,out v1);
			Vector3D v0 = new Vector3D();
			_inverse.Map(v0,out v0);
		}
		#endregion

		#region --- Serialization and Browsing Control ---
		private bool ShouldSerializeViewDirection()		{ return _kind == ProjectionKind.ParallelProjection || _kind == ProjectionKind.CentralProjection; }
		private bool ShouldSerializeXReductionOfZAxis() { return _kind == ProjectionKind.Isometric && _xReductionOfZAxis!=_xReductionOfZAxisDefault; }
		private bool ShouldSerializeYReductionOfZAxis() { return _kind == ProjectionKind.Isometric && _yReductionOfZAxis!=_yReductionOfZAxisDefault; }

		private bool ShouldSerializeMarginLeft()	{ return false; }
		private bool ShouldSerializeMarginRight()	{ return false; }
		private bool ShouldSerializeMarginTop()		{ return false; }
		private bool ShouldSerializeMarginBottom()	{ return false; }

		private bool ShouldBrowseViewDirection()		{ return _kind == ProjectionKind.CentralProjection || _kind == ProjectionKind.ParallelProjection; }
		private bool ShouldBrowseXReductionOfZAxis()	{ return _kind == ProjectionKind.Isometric; }
		private bool ShouldBrowseYReductionOfZAxis()	{ return _kind == ProjectionKind.Isometric; }
		private bool ShouldBrowseLightsOffOn2D()		{ return _kind == ProjectionKind.TwoDimensional; }

		private bool ShouldBrowseEnlargementX() { return false; }
		private bool ShouldBrowseEnlargementY() { return false; }
		private bool ShouldBrowseEnlargementZ() { return false; }
		private bool ShouldBrowseEnlargement () { return false; }

		private bool ShouldBrowsePerspective() { return Kind == ProjectionKind.CentralProjection; }
		private bool ShouldSerializePerspective() { return Kind == ProjectionKind.CentralProjection; }

		private static string[] PropertiesOrder = new string[] 
			{
				"Projection",
				"LightsOffOn2D",
				"ViewPoint",
				"Perspective",
				"ViewDirection",
				"XReductionOfZAxis",
				"YReductionOfZAxis",
				"Margins"
			};
	
		#endregion

		#region --- Debuging Helpers ---

		internal void Dump()
		{
			Debug.WriteLine ("V = " + _v.ToString());
			Debug.WriteLine ("M1 = " + _m.V1.ToString());
			Debug.WriteLine ("M2 = " + _m.V2.ToString());
			Debug.WriteLine ("M3 = " + _m.V3.ToString());
		}
		#endregion
		/// <summary>
		/// Occurs when projecting kind changes.
		/// </summary>
		public event EventHandler MappingKindChanged;

	}
}
