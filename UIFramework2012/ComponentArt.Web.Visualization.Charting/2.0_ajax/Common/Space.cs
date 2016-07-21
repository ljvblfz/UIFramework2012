using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using System.Drawing.Design;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Projection of the chart.
	/// </summary>
	public enum ProjectionKind
	{
		/// <summary>
		/// Central projection.
		/// </summary>
		[Description("3D - Perspective")]
		CentralProjection,
		/// <summary>
		/// Isometric projection.
		/// </summary>
		[Description("3D - Isometric")]
		Isometric,
		/// <summary>
		/// Parallel projection.
		/// </summary>
		[Description("3D - Parallel")]
		ParallelProjection,
		/// <summary>
		/// Two dimensional projection
		/// </summary>
		[Description("2D")]
		TwoDimensional
	};
}

namespace ComponentArt.Web.Visualization.Charting
{
	[Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	internal class ChartSpace : ChartObject, IDisposable
	{
		// --- Constants ---

		private static Vector3D		defaultDomainSize=new Vector3D(100,80,10);

		// ---------------------------------------------------------
		private double		coordinatePlanesDepth;
		private bool		coordinatePlanesDepthSet;
		private double		renderingPrecision = 0.2;

		// ---------------------------------------------------------
		//
		// Object Tracking Support

		private ObjectMapper	objectMappr;
		private bool			objectTrackingEnabled = false;
		private bool			useMatrixObjectTrackingModel = false;

		// ---------------------------------------------------------

		private CompositeSeries	series;

		// Working data

		private Bitmap			renderedBitmap;
		private bool			valid = false;
		private bool			needsCreationOfInitialContents = false;

		// Owning chart

		private ChartBase			owningChart;

        // Geometric Engine
        private GeometricEngine ge;
		private bool			built=false;

		// Debuging info

		public int				numberOfTElements;
				
		// Object tracking

		private int[,]	indexMatrix = null;
		private ArrayList activeObjects = null;
		private int		reducedSamplingForObjectTrackingStep = 1;

		#region --- Constructors ---

		public ChartSpace() : this(null) { }

		internal ChartSpace(ChartBase owningChart)
		{
			this.owningChart = owningChart;

			coordinatePlanesDepth = 0.0;

			// Series hierarchy initialization

			series = new CompositeSeries("Root");
			series.SetOwner(this);
			series.OwnTargetArea = new TargetArea();

			Mapping mapping = series.OwnTargetArea.Mapping;
			mapping.DomainSize = defaultDomainSize;
			mapping.IsViewDirectionDefault = true;
			mapping.Kind = ProjectionKind.CentralProjection;
			mapping.PerspectiveFactor = 2.5;
			mapping.Chart = owningChart;
			mapping.Refresh();

			CoordinateSystem sys = new CoordinateSystem();
			series.CoordSystem = sys;
            sys.SetOwner(series);
		}

		public void Dispose()
		{
			if(renderedBitmap != null)
			{
				renderedBitmap.Dispose();
				renderedBitmap = null;
			}
		}

		#endregion

		#region --- Properties ---

        internal GeometricEngine GE { get { return ge; } set { ge = value; } }

		internal int[,] ObjectIndexMatrix { get { return indexMatrix; } }

		internal ArrayList ActiveObjects { get { return activeObjects; } }
		
		internal LightCollection Lights { get { return OwningChart.Lights; } }

		public double CoordinatePlanesDepth
		{
			get 
			{ 
				return coordinatePlanesDepthSet? coordinatePlanesDepth:5;
			}
			set
			{
				coordinatePlanesDepth = value;
				coordinatePlanesDepthSet = true;
				series.CoordSystem.PlaneXY.Depth = coordinatePlanesDepth;
				series.CoordSystem.PlaneYZ.Depth = coordinatePlanesDepth;
				series.CoordSystem.PlaneZX.Depth = coordinatePlanesDepth;
			}
		}

		internal CompositeSeries Series
		{
			get { return series; }
			set
			{
				series = value; 
				if(value != null) series.SetOwner(this); 
			} 
		}

		#endregion

		#region --- Lights ---
		public void Add(Light L)
		{
			Lights.Add(L);
		}

		public void SetupDefaultLights()
		{
			Lights.Clear();

			Add(new Light(5));
			Add(new Light(5,new Vector3D(12,0,-20)));
			Add(new Light(5,new Vector3D(15,-15, 15)));
			// This is default lights setting:
			Lights.HasChanged = false; 
		}

		#endregion

		#region --- Navigation ---
		internal override ChartBase OwningChart
		{
			get { return owningChart; }
		}

		internal void SetOwningChart(ChartBase chart) 
		{  
			owningChart = chart; 
		}

		internal override ChartSpace Space
		{
			get { return this; }
		}

		private Vector3D Size { get { return OwningChart.DomainSize; } }

		#endregion

		#region --- Mapping and Coordinates ---


		public Mapping Mapping			
		{ 
			get 
			{ 
				return series.TargetArea.Mapping; 
			}
			set 
			{ 
				series.TargetArea.Mapping = value; 
				series.TargetArea.Mapping.Chart = OwningChart;
			} 
		}
		[Browsable(false)] public double Dpi				{ get { return Mapping.DPI; } set { Mapping.DPI = value; } }
		[Browsable(false)] public double FromWorldToTarget	{ get { UpdateCoordinates();return Mapping.Enlargement; } }
		[Browsable(false)] public double FromTargetToWorld	{ get { UpdateCoordinates();return 1.0/Mapping.Enlargement; } }
		[Browsable(false)] public double FromPointToTarget	{ get { return Mapping.DPI/72.0; } }
		[Browsable(false)] public double FromPointToWorld	{ get { return FromPointToTarget*FromTargetToWorld; } }
		public Size		NativeSize							
		{ 
			get
			{
				return Mapping.NativeSize;
			}
			set
			{ 
				if(Mapping.NativeSize != value)
				{
					Mapping.NativeSize = value; 
					SetCoordinates(); 
					NativeSizeChanged();
					valid = false;
				}
			}
		} 
		
		public Vector3D DomainSize
		{
			get
			{
				return Mapping.DomainSize; 
			}
			set
			{
				if(Mapping.DomainSize != value)
				{
					Mapping.DomainSize = value; 
					SetCoordinates(); 
					DomainSizeChanged();
					valid = false;
				}
			}
		} 

		private void UpdateCoordinates()
		{
			if(!DomainSize.IsNull && !Mapping.TargetSize.IsEmpty)
				Mapping.ForceRefresh();
		}

		public static Vector3D AxisUnitVector(AxisOrientation orientation)
		{
			switch(orientation)
			{
				case AxisOrientation.XAxis: return new Vector3D(1,0,0);
				case AxisOrientation.YAxis: return new Vector3D(0,1,0);
				case AxisOrientation.ZAxis: return new Vector3D(0,0,1);
				case AxisOrientation.XAxisNegative: return new Vector3D(-1,0,0);
				case AxisOrientation.YAxisNegative: return new Vector3D(0,-1,0);
				case AxisOrientation.ZAxisNegative: return new Vector3D(0,0,-1);
			};
			// This never happens:
			return new Vector3D(0,0,0);
		}

		public double AxisLength(AxisOrientation orientation)
		{
			switch(orientation)
			{
				case AxisOrientation.XAxis: return Size.X;
				case AxisOrientation.YAxis: return Size.Y;
				case AxisOrientation.ZAxis: return Size.Z;
				case AxisOrientation.XAxisNegative: return -Size.X;
				case AxisOrientation.YAxisNegative: return -Size.Y;
				case AxisOrientation.ZAxisNegative: return -Size.Z;
			};
			// This never happens:
			return 0;
		}

		public Vector3D AxisEndPoint(AxisOrientation orientation)
		{
			switch(orientation)
			{
				case AxisOrientation.XAxis: return new Vector3D(Size.X,0,0);
				case AxisOrientation.YAxis: return new Vector3D(0,Size.Y,0);
				case AxisOrientation.ZAxis: return new Vector3D(0,0,Size.Z);
				case AxisOrientation.XAxisNegative: return new Vector3D(-Size.X,0,0);
				case AxisOrientation.YAxisNegative: return new Vector3D(0,-Size.Y,0);
				case AxisOrientation.ZAxisNegative: return new Vector3D(0,0,-Size.Z);
			};
			// This never happens:
			return new Vector3D(0,0,0);
		}

		internal void DomainSizeChanged()
		{
			valid = false; 
			built = false;
		}

		internal void TargetSizeChanged()
		{ }
		internal void NativeSizeChanged()
		{ }

		#endregion

		#region --- Object Tracking ---

		internal object GetObjectAt(int x, int y)
		{
			if(!objectTrackingEnabled || ObjectMapper == null)
				return null;
			else
				return ObjectMapper.GetObjectAt(x,y);
		}

		internal ObjectMapper ObjectMapper 
		{
			get 
			{ 
				if(objectMappr == null)
				{
					if(GE == null)
						return null;
					objectMappr = GE.GetObjectMapper();
				}
				return objectMappr; 
			}
		}

		internal bool ObjectTrackingEnabled { get { return objectTrackingEnabled; } set { objectTrackingEnabled = value; } }
		internal MapAreaCollection MapAreas { get { return ObjectMapper.MapAreas; } }
		internal bool UseMatrixObjectTrackingModel { get { return useMatrixObjectTrackingModel; } set { useMatrixObjectTrackingModel = value; } }

		#endregion

		#region --- Initial Default Series ---

		internal bool NeedsCreationOfInitialContents
		{ get { return needsCreationOfInitialContents; } set { needsCreationOfInitialContents = value; } }

		public void CreateInitialSeries()
		{
			if (series == null || !OwningChart.InDesignMode)
				return ;

			if(series.SubSeries.Count == 0 && OwningChart.InDesignMode && needsCreationOfInitialContents)
			{
				series.SubSeries.Add(new Series("S0"));
				series.SubSeries.Add(new Series("S1"));
			}
		}

		#endregion 
		
	    #region --- Building ---

		public void SetCoordinates()
		{
			series.SetTargetArea();
		}

		public void Invalidate()
		{
			valid = false;
			built = false;
		}

		private TimeSpan durationDataBind, duration3DBmp, duration2DBmp, durationMapping;
		internal TimeSpan DurationDataBind	{ get { return durationDataBind; } }
		internal TimeSpan Duration3DBmp		{ get { return duration3DBmp; } }
		internal TimeSpan Duration2DBmp		{ get { return duration2DBmp; } }
		internal TimeSpan DurationMapping	{ get { return durationMapping; } }

		public Bitmap Render(Graphics g, Point position, Bitmap backgroundBitmap)
		{
			if(!valid)
			{
				DateTime then = DateTime.Now;
				PrepareLights();
				series.TargetArea.SetCoordinates();
				GE.SetMapping(this.Mapping);
				GE.SetRenderingPrecisionPxl(renderingPrecision);
				GE.SetBackground(backgroundBitmap);
				GE.SetGraphics(g);
				TwoDObjects.Clear();

				ObjectMapper.Clear();
				ObjectMapper.Initialize(ge,position,OwningChart.TargetSize.Width,OwningChart.TargetSize.Height,
					ObjectTrackingEnabled && !OwningChart.InDesignMode,useMatrixObjectTrackingModel);

				// Rendering series
				
				if(series != null)
					series.Render();
				bool someAreasAdjusted = TargetArea.Adjust2DAreas(GE.Root);
				if(someAreasAdjusted)
				{
					GE.Clear();
					ObjectMapper.Clear();
					series.Render();
					TargetArea.Adjust2DAreas(GE.Root);
					GE.Clear();
					ObjectMapper.Clear();
					series.Render();
				}
				duration3DBmp = DateTime.Now - then;
				then = DateTime.Now;

				if(renderedBitmap != null)
					renderedBitmap.Dispose();
				renderedBitmap = (Bitmap) GE.Render();
				valid = true;

				//Render 2D objects on top
				Render2DObjects(renderedBitmap);
				duration2DBmp = DateTime.Now - then;
				then = DateTime.Now;
			
				ObjectMapper.CreateMapping();
				durationMapping = DateTime.Now - then;
			}
			return renderedBitmap;
		}

		internal void FillLegend(Legend legend)
		{
			if (series == null)
				return;

			series.FillLegend(legend);
		}

		internal void DataBind()
		{
			DateTime then = DateTime.Now;
			if(series != null && series.SubSeries.Count == 0 && OwningChart.InDesignMode)
			{
				CreateInitialSeries();
			}
			series.BindDimensions();
			series.BindParameters();
//			series.OwnTargetArea.ComputeLayot();
			series.ComputeDefaultICSSize();
			series.DataBind();
			durationDataBind = DateTime.Now - then;
		}

		internal override void Build()
		{
			if(built)
				return;

			if(Series != null)
			{
				Series.Build();
			}
			PrepareLights();
			SetCoordinates();

			built = true;
		}

		internal void PrepareLights()
		{
			if(Lights.Count == 0)
				SetupDefaultLights();
			if(GE == null)
				GE = OwningChart.CreateGeometricEngine();
            GE.SetLights(Lights);
		}
		#endregion

		#region --- Rendering Mode ---

		internal double RenderingPrecision
		{
			get 
			{
				return renderingPrecision;
			}
			set
			{
				renderingPrecision = Math.Max(0.1, Math.Min(5,value));
			}
		}

		#endregion

	}
}
