using System;

namespace ComponentArt.Web.Visualization.Charting
{

	// ============================================================================================
	/// <summary>
	/// Implements 3x3 matrix.
	/// 
	/// </summary>
	internal class Matrix3x3
	{
		internal Vector3D V1, V2, V3;
			
		#region --- Constructors ---
		public Matrix3x3()
		{
			V1 = new Vector3D(1,0,0);
			V2 = new Vector3D(0,1,0);
			V3 = new Vector3D(0,0,1);
		}

		public Matrix3x3(Vector3D v1, Vector3D v2, Vector3D v3)
		{
			V1 = new Vector3D(v1);
			V2 = new Vector3D(v2);
			V3 = new Vector3D(v3);
		}

		public Matrix3x3(Matrix3x3 m) : this (m.V1,m.V2,m.V3) { }
		#endregion

		#region --- Operators ---
		public static Matrix3x3	operator +(Matrix3x3 m1, Matrix3x3 m2)
		{
			return new Matrix3x3(m1.V1+m2.V1,m1.V2+m2.V2,m1.V3+m2.V3);
		}

		public static Matrix3x3	operator -(Matrix3x3 m1, Matrix3x3 m2)
		{
			return new Matrix3x3(m1.V1-m2.V1,m1.V2-m2.V2,m1.V3-m2.V3);
		}

		public static Matrix3x3	operator *(Matrix3x3 m, double v)
		{
			return new Matrix3x3(m.V1*v,m.V2*v,m.V3*v);
		}

		public static Matrix3x3	operator *(double v, Matrix3x3 m)
		{
			return new Matrix3x3(m.V1*v,m.V2*v,m.V3*v);
		}

		public static Matrix3x3	operator /(Matrix3x3 m, double v)
		{
			return new Matrix3x3(m.V1/v,m.V2/v,m.V3/v);
		}

		public static Vector3D	operator *(Matrix3x3 m, Vector3D v)
		{
			return new Vector3D(m.V1*v, m.V2*v, m.V3*v);
		}		

		public static Matrix3x3	operator *(Matrix3x3 m1, Matrix3x3 m2)
		{
			// Multiply
			//  | m1.v1.x  m1.v1.y m1.v1.z | | m2.v1.x  m2.v1.y m2.v1.z | 
			//  | m1.v2.x  m1.v2.y m1.v2.z |*| m2.v2.x  m2.v2.y m2.v2.z | 
			//  | m1.v3.x  m1.v3.y m1.v3.z | | m2.v3.x  m2.v3.y m2.v3.z | 
			Vector3D v1 = new Vector3D
				(
				m1.V1.X*m2.V1.X + 
				m1.V1.Y*m2.V2.X + 
				m1.V1.Z*m2.V3.X
				,
				m1.V1.X*m2.V1.Y + 
				m1.V1.Y*m2.V2.Y + 
				m1.V1.Z*m2.V3.Y
				,
				m1.V1.X*m2.V1.Z + 
				m1.V1.Y*m2.V2.Z + 
				m1.V1.Z*m2.V3.Z
				);
			Vector3D v2 = new Vector3D
				(
				m1.V2.X*m2.V1.X + 
				m1.V2.Y*m2.V2.X + 
				m1.V2.Z*m2.V3.X
				,
				m1.V2.X*m2.V1.Y + 
				m1.V2.Y*m2.V2.Y + 
				m1.V2.Z*m2.V3.Y
				,
				m1.V2.X*m2.V1.Z + 
				m1.V2.Y*m2.V2.Z + 
				m1.V2.Z*m2.V3.Z
				);
			Vector3D v3 = new Vector3D
				(
				m1.V3.X*m2.V1.X + 
				m1.V3.Y*m2.V2.X + 
				m1.V3.Z*m2.V3.X
				,
				m1.V3.X*m2.V1.Y + 
				m1.V3.Y*m2.V2.Y + 
				m1.V3.Z*m2.V3.Y
				,
				m1.V3.X*m2.V1.Z + 
				m1.V3.Y*m2.V2.Z + 
				m1.V3.Z*m2.V3.Z
				);
			return new Matrix3x3(v1,v2,v3);
		}

		#endregion

		#region --- Inverse and Determinante ---
		public Matrix3x3 Inverse()
		{
			double det = Determinante();

			if(det == 0.0)
				throw new InvalidOperationException("Inverse undefined for singular Matrix3x3");
			double rdet = 1.0/det;

			double m11 = V2.Y*V3.Z - V2.Z*V3.Y;
			double m12 = V2.X*V3.Z - V2.Z*V3.X;
			double m13 = V2.X*V3.Y - V2.Y*V3.X;

			double m21 = V1.Y*V3.Z - V1.Z*V3.Y;
			double m22 = V1.X*V3.Z - V1.Z*V3.X;
			double m23 = V1.X*V3.Y - V1.Y*V3.X;

			double m31 = V1.Y*V2.Z - V1.Z*V2.Y;
			double m32 = V1.X*V2.Z - V1.Z*V2.X;
			double m33 = V1.X*V2.Y - V1.Y*V2.X;

			return new Matrix3x3(
				new Vector3D ( m11*rdet, -m21*rdet,  m31*rdet),
				new Vector3D (-m12*rdet,  m22*rdet, -m32*rdet),
				new Vector3D ( m13*rdet, -m23*rdet,  m33*rdet));
		}

		public double	Determinante()
		{
			// Matrix members
			//  | v1.x  v1.y v1.z |
			//  | v2.x  v2.y v2.z |
			//  | v3.x  v3.y v3.z |
			return
				V1.X * V2.Y * V3.Z
				+ V1.Y * V2.Z * V3.X
				+ V1.Z * V2.X * V3.Y
				- V1.X * V2.Z * V3.Y
				- V1.Y * V2.X * V3.Z
				- V1.Z * V2.Y * V3.X;
		}
		#endregion
	}
}
