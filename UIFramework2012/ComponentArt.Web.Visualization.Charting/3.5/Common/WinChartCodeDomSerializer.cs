
using System;
using System.CodeDom;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Summary description for WinChartCodeDomSerializer.
	/// </summary>
	internal class WinChartCodeDomSerializer:CodeDomSerializer
	{
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject) 
		{
			// This is how we associate the component with the serializer.
			CodeDomSerializer baseClassSerializer = (CodeDomSerializer)manager.
				GetSerializer(typeof(WinChart).BaseType, typeof(CodeDomSerializer));

			/* This is the simplest case, in which the class just calls the base class
				to do the work. */
			return baseClassSerializer.Deserialize(manager, codeObject);
		}
 
		public override object Serialize(IDesignerSerializationManager manager, object value) 
		{
			/* Associate the component with the serializer in the same manner as with
				Deserialize */
			try
			{
				CodeDomSerializer baseClassSerializer = (CodeDomSerializer)manager.
					GetSerializer(typeof(WinChart).BaseType, typeof(CodeDomSerializer));

				if (!((WinChart)value).Chart.InCollectionEditing)
					(value as WinChart).InSerialization = true;
				object codeObject = baseClassSerializer.Serialize(manager, value);
				(value as WinChart).InSerialization = false;
				return codeObject;
			}
			catch(Exception e)
			{
				Debug.WriteLine("Serialization exception\n" + e.StackTrace);
				throw;
			}
		}
	}
}
