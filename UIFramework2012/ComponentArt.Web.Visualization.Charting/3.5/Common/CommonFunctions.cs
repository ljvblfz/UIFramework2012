using System;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Resources;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Common utilities.
	/// </summary>
	public class CommonFunctions
	{
        static string[] resourceNames = null;
		static ResourceManager resourceManager = null;
		private CommonFunctions() 
		{
		}

		public static string GetNamespace()
		{
			return typeof(ChartBase).Namespace;
		}

        internal static ResourceManager GetComonResourceManager()
        {
			object lockObject = new Object();
			lock (lockObject)
			{
				if(resourceManager == null)
					resourceManager = new ResourceManager("ComponentArt.Web.Visualization.Charting.Common.Resource", Assembly.GetExecutingAssembly());
			}
			return resourceManager;
        }

        private static string[] ResourceNames()
        {
            object lockObject = new Object();
            lock (lockObject)
            {
                if (resourceNames == null)
                {
                    resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                }
            }
            return resourceNames;
        }

        private static string MapName(string shortName)
        {
            string[] names = ResourceNames();
            foreach (string name in names)
                if (name.ToLower().EndsWith(shortName.ToLower()))
                    return name;

            // name not mapped
            string nameList = "";
            foreach (string n in names)
            {
                nameList = nameList + n + "\n";
            }
            string message = "CHARTING IMPLEMENTATION ERROR: Resource short name = '" + shortName + "' not found. Available full names are:\n" + nameList;
            Debug.WriteLine(message);
            throw new ArgumentException(message);
        }

		public static System.Drawing.Bitmap GetResourceBitmap(string name, bool throwExceptionIfMissing) 
		{
			System.Drawing.Bitmap bmp = null;
			try 
			{
				bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(CommonFunctions.GetManifestResourceStream(MapName(name)));
			} 
			catch (Exception ex) 
			{
#if !__BUILDING_CRI__

				System.Windows.Forms.MessageBox.Show(ex.StackTrace, ex.Message);
#endif
				if(throwExceptionIfMissing)
					throw;
				else
					bmp = new System.Drawing.Bitmap(1, 1);
			}

			return bmp;
		}

		public static System.Drawing.Bitmap GetResourceBitmap(string name) 
		{
			return GetResourceBitmap(name,true);
		}

		public static string GetResourceString(string name) 
		{
			return name;
		}

		public static Stream GetManifestResourceStream(System.String name)
		{
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(MapName(name));
		}

        public static String GetCultureFreeString(object item)
        {
            if (item is Decimal || item is Double)
            {
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", item);
            }
            else
                return item.ToString();
        }

        const string BeginningVariableTag = "#[";
        const string EndningVariableTag = "]";
        const string BeginningParamsTag = "Param(";
        const string EndningParamsTag = ")";
        const string SeriesName = "SeriesName";

        public static string ParseFormatString(string s, object o)
        {
            string res = (string)s.Clone();
            int char_ind = 0;
            while (char_ind < res.Length)
            {
                // find #[
                int beginningTagIndex = res.IndexOf(BeginningVariableTag, char_ind);
                if (beginningTagIndex == -1)
                    break;

                // find ]
                int endingTagIndex = res.IndexOf(EndningVariableTag, beginningTagIndex);
                if (endingTagIndex == -1)
                    break;

                char_ind = endingTagIndex + EndningVariableTag.Length;

                // get what's between [ and ]
                string insideTheBrackets = res.Substring(beginningTagIndex + BeginningVariableTag.Length, endingTagIndex - beginningTagIndex - BeginningVariableTag.Length);

                if (insideTheBrackets.Length > BeginningParamsTag.Length + EndningParamsTag.Length && string.Compare(insideTheBrackets.Substring(0, BeginningParamsTag.Length), BeginningParamsTag) == 0 && string.Compare(insideTheBrackets.Substring(insideTheBrackets.Length - EndningParamsTag.Length, EndningParamsTag.Length), EndningParamsTag) == 0)
                {
                    // go here if the string is [#param(...)]
                    string paramName = insideTheBrackets.Substring(BeginningParamsTag.Length, insideTheBrackets.Length - BeginningParamsTag.Length - EndningParamsTag.Length);

                    // substitute with appropriate parameter
                    if (paramName.Length > 0 && (o is DataPoint || o is Series))
                    {
                        object replacementObject = (o is DataPoint) ? ((DataPoint)o).Parameter(paramName) : ((Series)o).Parameter(paramName);
                        if (replacementObject != null)
                        {
                            string replacementString = replacementObject.ToString();
                            char_ind = beginningTagIndex + replacementString.Length;
                            res = res.Substring(0, beginningTagIndex) + replacementString + res.Substring(endingTagIndex + EndningVariableTag.Length);
                        }
                    }
                }
                else if (string.Compare(insideTheBrackets, SeriesName) == 0)
                {
                    // Substitute with owning series name for data point or it's own name for series
                    if (o is DataPoint)
                    {
                        char_ind = beginningTagIndex + ((DataPoint)o).OwningSeries.Name.Length;
                        res = res.Substring(0, beginningTagIndex) + ((DataPoint)o).OwningSeries.Name + res.Substring(endingTagIndex + EndningVariableTag.Length);

                    }
                    else if (o is Series)
                    {
                        char_ind = beginningTagIndex + ((Series)o).Name.Length;
                        res = res.Substring(0, beginningTagIndex) + ((Series)o).Name + res.Substring(endingTagIndex + EndningVariableTag.Length);
                    }
                }
                else if (string.Compare(insideTheBrackets, "Index") == 0)
                {
                    // Substitute with the index
                    if (o is DataPoint)
                    {
                        DataPoint dp = (DataPoint)o;
                        int dataPointIndex = dp.Index;
                        char_ind = beginningTagIndex + dataPointIndex.ToString(System.Globalization.CultureInfo.InvariantCulture).Length;
                        res = res.Substring(0, beginningTagIndex) + dataPointIndex.ToString(System.Globalization.CultureInfo.InvariantCulture) + res.Substring(endingTagIndex + EndningVariableTag.Length);

                    }
                    else if (o is Series)
                    {
                        Series ser = (Series)o;


                        CompositeSeries parentSer = ser.OwningSeries;
                        if (parentSer != null)
                        {
                            int ind = ((IList)parentSer.SubSeries).IndexOf(ser);

                            char_ind = beginningTagIndex + ind.ToString(System.Globalization.CultureInfo.InvariantCulture).Length;
                            res = res.Substring(0, beginningTagIndex) + ind.ToString(System.Globalization.CultureInfo.InvariantCulture) + res.Substring(endingTagIndex + EndningVariableTag.Length);
                        }
                    }
                }

            }
            return res;
        }
    }
}
