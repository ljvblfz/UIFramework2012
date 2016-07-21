using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for ObjectModelBrowser.
	/// </summary>
	internal class ObjectModelBrowser
	{
		internal static IObjectModelNode GetAncestorByType(IObjectModelNode node, Type ancestorType)
		{
			IObjectModelNode ancestor = node.ParentNode;
			while(ancestor != null)
			{
				if(ancestor.GetType() == ancestorType || ancestor.GetType().IsSubclassOf(ancestorType))
					return ancestor;
				ancestor = ancestor.ParentNode;
			}
			return null;
		}

		internal static SubGauge GetOwningGauge(IObjectModelNode node)
		{
			return GetAncestorByType(node, typeof(SubGauge)) as SubGauge;
		}

		internal static bool InSerialization(IObjectModelNode node)
		{
			SubGauge topGauge = GetOwningTopmostGauge(node);
			return topGauge != null && topGauge.InSerialization;
		}

		internal static SubGauge GetOwningTopmostGauge(IObjectModelNode node)
		{
			if (node == null)
				return null; // Because we are trying from null or non-IObjectModelNode object
			SubGauge gauge = GetOwningGauge(node);
			if(gauge == null)
				return node as SubGauge; // This will return the node itself if it is topmost gauge 
			if(((IObjectModelNode)gauge).ParentNode == null)
				return gauge;
			else
				return GetOwningTopmostGauge(gauge);
		}

		internal static void NotifyChanged(IObjectModelNode node)
		{
			if (node == null)
				return; 
			SubGauge gauge = GetOwningTopmostGauge(node);
			if(gauge != null)
				gauge.Invalidate();
		}

		internal static Factory GetFactory(IObjectModelNode node)
		{
			return GetOwningTopmostGauge(node).Factory;
		}

		// Finding a member in the object tree using an expression
		//
		// The expression is applied to the given node object.
		// An example of expression is
		//    Scales["Main"].Pointers["Main"].Value
		// where the node is a Gauge object.
		//
		// The expression may begin with "Gauge" or "TopGauge" when the node is any IObjectModelNode. In that case 
		// the appropriate gauge object is selected and the rest of the expression applied to it.
		//
		// The collection member names may be specified without quotes, for example
		//    Scales[Main].Pointers[Main].Value

		internal static object GetObjectFromExpression(object node, string expression)
		{
			Debug.WriteLine("Object: " + node.GetType().Name + " Expression: " + expression);

			expression = expression.Trim();
			if(expression == "")
			{
				Debug.WriteLine("Result: " + node.GetType().Name + " = " + node.ToString());
				return node;
			}

			if(node == null)
				throw new Exception("Cannot compute expression '" + expression + "': the object is null.");

			int ix = expression.IndexOf('.');
			string prefix, tail;
			if(ix < 0)
			{
				prefix = expression;
				tail = "";
			}
			else
			{
				prefix = expression.Substring(0,ix);
				tail = expression.Substring(ix+1);
			}

			prefix = prefix.Trim();

			if(prefix.ToLower() == "gauge")
				return GetObjectFromExpression(GetOwningGauge(node as IObjectModelNode),tail);
			if(prefix.ToLower() == "topgauge")
				return GetObjectFromExpression(GetOwningTopmostGauge(node as IObjectModelNode),tail);

			ix = prefix.IndexOf('[');
			PropertyDescriptor pDes;
			if(ix <= 0) // Simple property
			{
				pDes = TypeDescriptor.GetProperties(node)[prefix];
				if(pDes == null)
					throw new Exception("Cannot compute expression '" + prefix + "': object type '" + node.GetType().Name + "' doesn't have property '" + prefix + "'");
				return GetObjectFromExpression(pDes.GetValue(node),tail);
			}
			
			// Collection property: the prefix has to be CollectionName[MemberName] or CollectionName["MemberName"]
			
			if(!prefix.EndsWith("]"))
				throw new Exception("Cannot compute expression '" + prefix + "': it should end with a quote.");
			string collectionName = prefix.Substring(0,ix);
			string memberName = prefix.Substring(ix+1,prefix.Length-ix-2);
			if(memberName.StartsWith("\""))
			{
				if(!memberName.EndsWith("\""))
					throw new Exception("Cannot compute expression '" + prefix + "': them member name begins with a quote doesn't have the qiote at the end.");
				memberName = memberName.Substring(1,memberName.Length-2);
			}
			pDes = TypeDescriptor.GetProperties(node)[collectionName];
			if(pDes == null)
				throw new Exception("Cannot compute expression '" + prefix + "': object type '" + node.GetType().Name + "' doesn't have property '" + collectionName + "'");
			object memberValue = pDes.GetValue(node);
			if(memberValue == null)
				throw new Exception("Cannot compute expression '" + prefix + "': member '" + collectionName + "' is null.");
			NamedObjectCollection coll = memberValue as NamedObjectCollection;
			if(coll == null)
				throw new Exception("Cannot compute expression '" + prefix + "': member '" + collectionName + "' is not a collection.");
			return GetObjectFromExpression(coll[memberName],tail);
		}

		internal static string  CreateExpression(IObjectModelNode node)
		{
			if(node == null)
				return "";
			if(node.ParentNode == null)
				return "Gauge"; // Topmost gauge
			
			string parentExpression = CreateExpression(node.ParentNode);

			if(node is TickMark)
			{
				TickMark mark = node as TickMark;
				return mark.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			}
			
			NamedObject namedObject = node as NamedObject;
			if(namedObject != null) // the node is a member of the parent collection
			{
				string result = parentExpression + "[" + namedObject.Name + "]";
				Pointer ptr = node as Pointer;
				if(ptr != null)
					return result + "=" + ptr.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
				else
					return result;
			}

			NamedObjectCollection collection = node as NamedObjectCollection;
			if(collection == null)
				return "";

			if(collection is PointerCollection)
				return parentExpression + ".Pointers";
			if(collection is RangeCollection)
				return parentExpression + ".Ranges";
			if(collection is ScaleCollection)
				return parentExpression + ".Scales";
			if(collection is SubGaugeCollection)
				return parentExpression + ".SubGauges";
            if(collection is IndicatorCollection) 
            {
                IObjectModelNode parent = (collection as IObjectModelNode).ParentNode;
                if (parent != null && parent is Indicator)
                    return parentExpression + ".IndicatorList";
                    
                return parentExpression + ".Indicators";
            }

			return "";
		}
	}
}
