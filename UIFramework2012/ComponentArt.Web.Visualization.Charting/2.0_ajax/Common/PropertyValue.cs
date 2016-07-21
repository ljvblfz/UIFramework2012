using System;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting
{
    public class PropertyValue
    {
        public static void Set(object target, string propertyPath, object value)
        {
			if (target == null)
				throw new Exception("Cannot set property '" + propertyPath + "'. Target objest is null.");

			PropertyInfo pDes;
            string[] path = propertyPath.Split('.');
            int i = 0;
            while (i < path.Length - 1)
            {
                pDes = PropertyInfo(target, path[i]);
                if(pDes == null)
                    throw new Exception("Cannot get property '" + target.GetType().Name + "." + path[i] + "'.");
                target = pDes.GetValue(target,null);
                if (target == null)
                    throw new Exception("Null object reference, cannot set value to '" + target.GetType().Name + "." + propertyPath + "'.");
                i++;
            }
            pDes = PropertyInfo(target, path[i]);
            if (pDes == null)
                throw new Exception("Cannot get property '" + target.GetType().Name + "." + path[i] + "'.");
            pDes.SetValue(target, value,null);
            target = pDes.GetValue(target,null);
        }

        private static PropertyInfo PropertyInfo(object obj, string propName)
        {
            return obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        private static PropertyInfo PropertyInfo(Type type, string propName)
        {
            return type.GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        public static bool Exists(object target, string propertyPath)
        {
            PropertyInfo pDes;
            string[] path = propertyPath.Split('.');
            int i = 0;
            Type type = target.GetType();
            while (i < path.Length - 1)
            {
                pDes = PropertyInfo(type, path[i]);
                if (pDes == null)
                    return false;
                type = pDes.PropertyType;
                i++;
            }
            return true;
        }

        public static object Get(object target, string propertyPath)
        {
            PropertyInfo pDes;
            string[] path = propertyPath.Split('.');
            int i = 0;
            while (i < path.Length - 1)
            {
                pDes = PropertyInfo(target, path[i]);
                if (pDes == null)
                    throw new Exception("Cannot get property '" + target.GetType().Name + "." + path[i] + "'.");
                target = pDes.GetValue(target,null);
                if (target == null)
                    throw new Exception("Null object reference, cannot set value to '" + target.GetType().Name + "." + propertyPath + "'.");
                i++;
            }
            pDes = PropertyInfo(target, path[i]);
            if (pDes == null)
                throw new Exception("Cannot get property '" + target.GetType().Name + "." + path[i] + "'.");
            return pDes.GetValue(target,null);
        }
    }
}
