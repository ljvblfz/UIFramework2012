using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Describes a light source of the chart.
	/// </summary>
	[TypeConverter(typeof(LightConverter))]
	[Serializable()]
	public sealed class Light : ICloneable
	{
		private bool		isAmbient;
		private float		intensity;
		private Vector3D	srcDirection = new Vector3D(1,1,1);
		private bool		hasChanged = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Light"/> class.
		/// </summary>
		public Light()
		{
			isAmbient = false;
			intensity = 1;
			srcDirection = new Vector3D(1,1,1);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Light"/> class with a specified intensity.
		/// </summary>
		/// <param name="intens">The intensity of the light.</param>
		public Light(float intens)
		{
			intensity = intens;
			isAmbient = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Light"/> class with a specified intensity and direction.
		/// </summary>
		/// <param name="intens">The intensity of the light.</param>
		/// <param name="sourceDirection">The direction of the light.</param>
		public Light(float intens, Vector3D sourceDirection)
		{
			intensity = intens;
			srcDirection = sourceDirection;
			isAmbient = false;
		}

		/// <summary>
		/// Gets or sets a value that indicates whether this <see cref="Light"/> is ambient.
		/// </summary>
		[SRCategory("CatBehavior")]
		[DefaultValue(false),RefreshProperties(RefreshProperties.All)]
		[Description("Determines whether this light is ambient or not")]
		public bool	 IsAmbient	{ get { return isAmbient; } set { if(isAmbient != value) hasChanged = true; isAmbient = value; } }
		/// <summary>
		/// Gets or sets the intensity of this <see cref="Light"/> object.
		/// </summary>
		[SRCategory("CatBehavior")]
		[DefaultValue(1.0)]
		[Description("The intensity of this light")]
		public float Intensity	{ get { return intensity; } set { if(intensity != value) hasChanged = true; intensity = Math.Abs(value); } }
		/// <summary>
		/// Gets or sets the direction of this <see cref="Light"/> object. Only applicable if the light is not ambient.
		/// </summary>
		[SRCategory("CatBehavior")]
		[DefaultValue("(1,1,1)")]
		[Description("The direction of this light")]
		public Vector3D Direction { get { return srcDirection; } set { if(srcDirection != value) hasChanged = true; srcDirection = value; } }

		internal float X			{ get { return (float)srcDirection.X; } set { srcDirection.X = value; } }
		internal float Y			{ get { return (float)srcDirection.Y; } set { srcDirection.Y = value; } }
		internal float Z			{ get { return (float)srcDirection.Z; } set { srcDirection.Z = value; } }

		#region -- Serialization and Browsing Control--
		internal bool HasChanged	{ get { return hasChanged; } }

		private void ResetIntensity() {Intensity=1;}
		private bool ShouldSerializeIntensity() {return Intensity!=1;}

		private void ResetDirection() {Direction=new Vector3D(1,1,1);}
		private bool ShouldSerializeDirection() {return Direction!=new Vector3D(1,1,1);}

		private void ResetIsAmbient() {IsAmbient=false;}
		private bool ShouldSerializeIsAmbient() {return IsAmbient;}

		private bool ShouldBrowseDirection() { return !isAmbient; }
		private static string[] PropertiesOrder = new string[]
			{
				"IsAmbient",
			    "Intensity",
			    "Direction"
			};

		#endregion

		/// <summary>
		/// Creates an exact copy of this <see cref="Light"/> object.
		/// </summary>
		/// <returns>An object that can be cast to a <see cref="Light"/> object.</returns>
		public object Clone()
		{
			Light light = new Light(intensity, srcDirection);
			light.isAmbient = isAmbient;
			light.hasChanged = hasChanged;
			return light;
		}
	}
}
