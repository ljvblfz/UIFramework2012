using System;
using System.Diagnostics;
using System.ComponentModel;
using ComponentArt.Web.Visualization.Charting.Design;



namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents a 3D Vector.
	/// </summary>

	[System.ComponentModel.TypeConverter(typeof(Vector3DConverter))]
	[Serializable]
	public struct Vector3D
	{
		private double _x, _y, _z;

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3D"/> class.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <param name="z">The Z coordinate.</param>
		public Vector3D(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3D"/> class.
		/// </summary>
		/// <param name="v"><see cref="Vector3D"/> object to be copied.</param>
		public Vector3D(Vector3D v)
		{
			_x = v.X;
			_y = v.Y;
			_z = v.Z;
		}

		/// <summary>
		/// This constructor is used to avoid problems in deserialization of negative numbers.
		/// </summary>
		/// <param name="coordinates">The string representation of the Vector</param>
		public Vector3D(string coordinates) 
		{
			object o = new Vector3DConverter().ConvertFrom(coordinates);
			Vector3D V = (Vector3D)o;
			_x = V.X;
			_y = V.Y;
			_z = V.Z;
		}
		
		#region ---- Vector properties ----
		/// <summary>
		/// Gets a zero-vector.
		/// </summary>
		[Browsable(false)]
		public static Vector3D Null { get { return new Vector3D(0,0,0); } }

		/// <summary>
		/// Gets or sets X coordinate of a <see cref="Vector3D"/> object.
		/// </summary>
		[Description("X coordinate of a 3D Vector")]
		public double X			{ get { return _x; } set { _x = value; } }
		/// <summary>
		/// Gets or sets Y coordinate of a <see cref="Vector3D"/> object.
		/// </summary>
		[Description("Y coordinate of a 3D Vector")]
		public double Y			{ get { return _y; } set { _y = value; } }
		/// <summary>
		/// Gets or sets Z coordinate of a <see cref="Vector3D"/> object.
		/// </summary>
		[Description("Z coordinate of a 3D Vector")]
		public double Z			{ get { return _z; } set { _z = value; } }
		/// <summary>
		/// Gets the absolute value of a <see cref="Vector3D"/> object.
		/// </summary>
		[Browsable(false)] 
		public double Abs		{ get { return Math.Sqrt(_x * _x + _y * _y + _z * _z); } }
		/// <summary>
		/// Indicates whether the <see cref="Vector3D"/> object is zero-vector.
		/// </summary>
		[Browsable(false)] 
		public bool   IsNull	{ get{ return _x == 0 && _y == 0 && _z == 0; } }
		#endregion

		#region ---- Vector Serialization ----
		internal void ResetX() {X=0;}
		internal void ResetY() {Y=0;}
		internal void ResetZ() {Z=0;}
		internal bool ShouldSerializeX() {return X!=0;}
		internal bool ShouldSerializeY() {return Y!=0;}
		internal bool ShouldSerializeZ() {return Z!=0;}
		#endregion

		#region ---- Vector operations ----

		// Fixme: do we really need this???
		public void Set(Vector3D v)
		{
			_x = v.X;
			_y = v.Y;
			_z = v.Z;
		}

		public void Set(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		/// <summary>
		/// Unary minus operator
		/// </summary>
		/// <param name="v">The source vector</param>
		/// <returns>Negated vector</returns>
		public static Vector3D operator - (Vector3D v)
		{
			return new Vector3D(-v.X, -v.Y, -v.Z);
		}

		/// <summary>
		/// Adds two <see cref="Vector3D"/>s.
		/// </summary>
		/// <param name="v1">The first <see cref="Vector3D"/>.</param>
		/// <param name="v2">The second <see cref="Vector3D"/>.</param>
		/// <returns>Addition of two vectors.</returns>
		public static Vector3D operator + (Vector3D v1, Vector3D v2)
		{
			return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="v1"><see cref="Vector3D"/> to subtract from.</param>
		/// <param name="v2"><see cref="Vector3D"/> subtracted from v1.</param>
		/// <returns></returns>
		public static Vector3D operator - (Vector3D v1, Vector3D v2)
		{
			return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		/// <summary>
		/// Dot product of 2 vectors.
		/// </summary>
		/// <param name="v1">The first <see cref="Vector3D"/> in the dot product.</param>
		/// <param name="v2">The second <see cref="Vector3D"/> in the dot product.</param>
		/// <returns><see cref="Vector3D"/> resulting from a dot product of two <see cref="Vector3D"/>s.</returns>
		public static double operator * (Vector3D v1, Vector3D v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
		}
		
		/// <summary>
		/// Scalar-<see cref="Vector3D"/> multiptiplication.
		/// </summary>
		/// <param name="v"><see cref="Vector3D"/> to be multiplied.</param>
		/// <param name="p">Scalar to multiply the <see cref="Vector3D"/> v by.</param>
		/// <returns><see cref="Vector3D"/> which is a result of scalar-vector multiplication.</returns>
		public static Vector3D operator * (Vector3D v, double p)
		{
			return new Vector3D(v.X * p, v.Y * p, v.Z * p);
		}
		
		/// <summary>
		/// Scalar-<see cref="Vector3D"/> multiptiplication.
		/// </summary>
		/// <param name="p">Scalar to multiply the <see cref="Vector3D"/> v by.</param>
		/// <param name="v"><see cref="Vector3D"/> to be multiplied.</param>
		/// <returns><see cref="Vector3D"/> which is a result of scalar-vector multiplication.</returns>
		public static Vector3D operator * (double p, Vector3D v)
		{
			return new Vector3D(v.X * p, v.Y * p, v.Z * p);
		}
			
		/// <summary>
		/// Scalar-<see cref="Vector3D"/> division.
		/// </summary>
		/// <param name="v"><see cref="Vector3D"/> to be divided.</param>
		/// <param name="p">Scalar to multiply the <see cref="Vector3D"/> v by.</param>
		/// <returns><see cref="Vector3D"/> which is a result of <see cref="Vector3D"/> division by a scalar.</returns>
		public static Vector3D operator / (Vector3D v, double p)
		{
			return new Vector3D(v.X / p, v.Y / p, v.Z / p);
		}
			
		/// <summary>
		/// Compares two <see cref="Vector3D"/> objects. The result specifies whether the values of the X, Y and Z properties of the two <see langword="Vector3D"/> objects are equal.
		/// </summary>
		/// <param name="v1">A <see cref="Vector3D"/> to compare.</param>
		/// <param name="v2">A <see cref="Vector3D"/> to compare.</param>
		/// <returns>This operator returns true if the X, Y and Z values of v1 and v2 are equal; otherwise, false.</returns>
		public static bool operator == (Vector3D v1, Vector3D v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}

		/// <summary>
		/// Compares two <see cref="Vector3D"/> objects. The result specifies whether the values of the X, Y and Z properties of the two <see langword="Vector3D"/> objects are unequal.
		/// </summary>
		/// <param name="v1">A <see cref="Vector3D"/> to compare.</param>
		/// <param name="v2">A <see cref="Vector3D"/> to compare.</param>
		/// <returns>This operator returns true if the X, Y and Z values of v1 and v2 are unequal; otherwise, false.</returns>
		public static bool operator != (Vector3D v1, Vector3D v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
		}

		public override bool Equals(Object obj)
		{
			if(obj == null || this.GetType() != obj.GetType())
				return false;
			Vector3D v = (Vector3D)obj;
			return v.X == X && v.Y == Y && v.Z == Z;
		}

		public override int GetHashCode()
		{
			return ((int)X) ^ ((int)Y) ^ ((int)Z);
		}

		/// <summary>
		/// Vector product of two vectors.
		/// The resilt is
		///		| i   j   k |
		///		| x1  y1  z1|
		///		| x2  y2  z2|
		///		
		/// </summary>
		public Vector3D CrossProduct (Vector3D v2)
		{
			return new Vector3D
				(
				_y * v2.Z - v2.Y * _z,
				-(_x * v2.Z - v2.X * _z),
				_x * v2.Y - v2.X * _y 
				);
		}

		/// <summary>
		/// Unit vector. Colinear to the original vector, length = 1
		/// </summary>
		public Vector3D Unit()
		{
			if(IsNull)
				throw (new Exception("Unit() of null vector is not defined"));
			else
				return this*(1.0/Abs);
		}

		private double AbsSquared() 
		{
			return _x * _x + _y * _y + _z * _z;
		}
		

		/// <summary>
		/// Builds a projection of a <see cref="Vector3D"/> onto a plane.
		/// </summary>
		/// <param name="normal">The <see cref="Vector3D"/> that represents a normal to the plane.</param>
		/// <returns><see cref="Vector3D"/> which is a projection of this vector onto a plane defined by the normal.</returns>
		public Vector3D ProjectOnPlane (Vector3D normal) 
		{
			double dotprod = this*normal;
            Vector3D projectionOnNormal = dotprod*normal/normal.AbsSquared();
			Vector3D projectionOnPlane = this - projectionOnNormal;

			return projectionOnPlane;
		}

		// Assume vectors are 2D only
		/// <summary>
		/// Rotates a <see cref="Vector3D"/> by 90 degrees right. Assumes that the <see cref="Vector3D"/> is a 2D vector. (Z-coordinate is discarted).
		/// </summary>
		/// <returns><see cref="Vector3D"/> rotated by 90 degrees right.</returns>
		internal Vector3D RotateRight()   {return new Vector3D(Y, -X, 0);}
		/// <summary>
		/// Rotates a <see cref="Vector3D"/> by 90 degrees left. Assumes that the <see cref="Vector3D"/> is a 2D vector. (Z-coordinate is discarted).
		/// </summary>
		/// <returns><see cref="Vector3D"/> rotated by 90 degrees left.</returns>
		internal Vector3D RotateLeft()    {return new Vector3D(-Y, X, 0);}
		/// <summary>
		/// Rotates a <see cref="Vector3D"/> by deg degrees right. Assumes that the <see cref="Vector3D"/> is a 2D vector. (Z-coordinate is discarted).
		/// </summary>
		/// <param name="deg">Amount of degrees to rotate the vector.</param>
		/// <returns></returns>
		internal Vector3D RotateDegrees(float deg)
		{
			double rad = Math.PI*deg/180;
			return new Vector3D(X*Math.Cos(rad) + Y*Math.Sin(rad), Y*Math.Cos(rad) - X*Math.Sin(rad), 0);
		}

		#endregion
		/// <summary>
		/// Translates a <see cref="Vector3D"/> into its string representation.
		/// </summary>
		/// <returns>A string representation of <see cref="Vector3D"/>.</returns>
		public override string ToString()
		{
			return "(" + _x.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," + _y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," + _z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + ")";
		}

		public static implicit operator ComponentArt.Web.Visualization.Charting.Shader.Vector3D (Vector3D v)
		{
			return new ComponentArt.Web.Visualization.Charting.Shader.Vector3D(v.X,v.Y,v.Z);
		}
	}

}
