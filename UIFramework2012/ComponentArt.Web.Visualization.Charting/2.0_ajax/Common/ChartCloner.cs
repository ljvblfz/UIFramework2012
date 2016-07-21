using System;
using System.Reflection;
using System.Collections;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class ChartCloner : GenericCloner 
	{

		protected override Type [] DoNotCloneTypes()
		{
            return new Type[] { typeof(Shader.RenderingEngine) };
		}

		ChartBase m_chart;
		
		public override object Clone(object srcObj) 
		{
			m_chart = ChartBase.GetChartFromObject(srcObj) as ChartBase;
			return base.Clone(srcObj);
		}

		public override object CloneReferenceObject(object srcObj) 
		{
			if(srcObj == null)
				return null;

			object dstObj;

			if (srcObj.GetType().IsSubclassOf(typeof(GeometricEngine)))
			{
				dstObj = null;
			}
			else if (srcObj is System.ComponentModel.IListSource || srcObj is System.Data.DataColumn) 
			{
				dstObj = srcObj;
			}
			else if (m_currentField != null && m_currentField.FieldType == typeof(System.Object) && m_chart != null && m_chart.DataSource == srcObj 
				)
			{
				dstObj = srcObj;
			}
			else if (srcObj is NamedStyleInternal)
			{
				NamedStyleInternal nsi = new NamedStyleInternal();
				nsi.Name = ((NamedStyleInternal)srcObj).Name;
				dstObj = nsi;
			}
			else if (srcObj.GetType() == typeof(System.Collections.ArrayList))
			{
				dstObj = new ArrayList();
				CloneItems((IList)srcObj, (IList)dstObj);
			}
			else if (srcObj.GetType() == typeof(Hashtable))
			{
				dstObj = new Hashtable();
				foreach (DictionaryEntry de in ((Hashtable)srcObj))
				{
					((Hashtable)dstObj).Add(CloneRecursive(de.Key), CloneRecursive(de.Value));
				}
			}
			else if (srcObj is Array)
			{
				dstObj = ((Array)srcObj).Clone();
				if (((Array)srcObj).Length > 0)
					for (int i=0; i<((Array)srcObj).Length; ++i)
						((IList)dstObj)[i] = this.CloneRecursive(((IList)srcObj)[i]);
			}
			else if (srcObj is Exception)
			{
				dstObj = null;
			}
			else if (srcObj is System.Drawing.Graphics)
			{
				dstObj = null;
			}
			else if (srcObj is ObjectMapper)
			{
				ObjectMapper om = srcObj as ObjectMapper;
				dstObj = om.GeometricEngine.GetObjectMapper();
			}
			else 
			{
				dstObj = base.CloneReferenceObject(srcObj);
			}

			return dstObj;
		}
	}

}
