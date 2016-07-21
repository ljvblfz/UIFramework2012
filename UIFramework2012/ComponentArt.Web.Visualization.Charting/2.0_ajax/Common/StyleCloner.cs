using System;
using System.Reflection;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class StyleCloner : ChartCloner 
	{
		public override object CloneReferenceObject(object srcObj) 
		{
			if (srcObj is NamedStyleInternal)
			{
				NamedStyleInternal nsi = new NamedStyleInternal();
				nsi.Name = ((NamedStyleInternal)srcObj).Name;
				return nsi;
			}
			return base.CloneReferenceObject(srcObj);
		}
	}
	
}
