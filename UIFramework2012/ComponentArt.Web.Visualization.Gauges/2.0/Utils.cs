using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading;


namespace ComponentArt.Web.Visualization.Gauges
{
  internal class Utils
  {
    public static string GetResponseUrl(HttpContext oContext)
    {
      return
        oContext.Request.Url.Scheme +
        Uri.SchemeDelimiter +
        oContext.Request.Url.Host +
        (oContext.Request.Url.IsDefaultPort ? "" : (":" + oContext.Request.Url.Port)) +
        oContext.Response.ApplyAppPathModifier(oContext.Request.RawUrl);
    }
    
    /// <summary>
    /// A very fast way to check whether the given string can read off as an integer.
    /// </summary>
    /// <param name="s">string to test</param>
    /// <returns>true if it's a string of only digits.</returns>
    /// <remarks>Returns false for an empty string.</remarks>
    public static bool CanParseAsInt(string s)
    {
      char[] chars = s.ToCharArray();
      if (chars.Length == 0)
      {
        return false;
      }
      for (int i=0; i<chars.Length; i++) 
      {
        if (chars[i] > 57 || chars[i] < 48) 
        {
          return false; 
        }
      }
      return true;
    }

    /// <summary>
    /// Places a single quote around each element in comma delimited string
    /// </summary>
    /// <param name="s">comma delimited string</param>
    /// <returns>string with a single quote around each element.</returns>
    public static string QuoteElements(string s)
    {
        String s1 = "'" + s.Replace(",", "','") + "'";
        String s2 = s1.Replace("'true'", "true");
        String s3 = s2.Replace("'false'", "false");
        return s3;
    }

    /// <summary>
    /// Converts a ClientEvents object to an appropriate JavaScript representation.
    /// </summary>
    /// <param name="ce">ClientEvents to convert.</param>
    /// <returns>A string that parses as a JavaScript object corresponding to the given ClientEvents.</returns>
    public static string ConvertClientEventsToJsObject(ClientEvents ce)
    {
      if (ce == null)
      {
        return "null";
      }
      StringBuilder jsce = new StringBuilder();
      jsce.Append("{");
      bool firstPass = true; // used only for the commas
      foreach (string clientHandler in ce.Handlers.Keys)
      {
        if (firstPass)
        {
          firstPass = false;
        }
        else
        {
          jsce.Append(",");
        }
        jsce.Append("'").Append(clientHandler).Append("':").Append(ce.Handlers[clientHandler]);
      }
      jsce.Append("}");
      return jsce.ToString();
    }

    /// <summary>
    /// Converts a DateTime to a corresponding JavaScript Date object.
    /// </summary>
    /// <param name="d">DateTime to convert.</param>
    /// <returns>A string that represents a JavaScript instantiation of
    /// a corresponding Date object.</returns>
    public static string ConvertDateTimeToJsDate(DateTime d)
    {
      /*
      TimeSpan ms = d - new DateTime(1970, 1, 1);

      return "new Date(" + ms.TotalMilliseconds + ")";
      */
    
      StringBuilder dateConstructor = new StringBuilder();
      dateConstructor.Append("new Date(");
      dateConstructor.Append(d.Year);
      dateConstructor.Append(",");
      dateConstructor.Append(d.Month - 1); // In JavaScript, months are zero-delimited
      dateConstructor.Append(",");
      dateConstructor.Append(d.Day);
      dateConstructor.Append(",");
      dateConstructor.Append(d.Hour);
      dateConstructor.Append(",");
      dateConstructor.Append(d.Minute);
      dateConstructor.Append(")");
      return dateConstructor.ToString();
    }

    /// <summary>
    /// Converts a DayOfWeek value to a corresponding JavaScript value.
    /// </summary>
    /// <param name="d">DayOfWeek to convert.</param>
    /// <returns>An integer corresponding to the given day of the week in JavaScript.
    /// In JavaScript, 0 stands for Sunday, 1 for Monday, and so on to 6 which stands for Saturday.</returns>
    public static int ConvertDayOfWeekToJsDay(DayOfWeek d)
    {
      switch (d)
      {
        case DayOfWeek.Sunday: return 0;
        case DayOfWeek.Monday: return 1;
        case DayOfWeek.Tuesday: return 2;
        case DayOfWeek.Wednesday: return 3;
        case DayOfWeek.Thursday: return 4;
        case DayOfWeek.Friday: return 5;
        case DayOfWeek.Saturday: return 6;
        default: return 0; // Actually this line is unreachable, but the compiler requires it
      }
    }

    /// <summary>
    /// Converts a FirstDayOfWeek value to a corresponding JavaScript value.
    /// </summary>
    /// <param name="f">FirstDayOfWeek to convert.</param>
    /// <param name="defaultValue">Default first day of week.</param>
    /// <returns>An integer corresponding to the given day of the week in JavaScript.
    /// In JavaScript, 0 stands for Sunday, 1 for Monday, and so on to 6 which stands for Saturday.</returns>
    /// <remarks>FirstDayOfWeek.Default is converted to an integer corresponding to the given defaultValue.</remarks>
    public static int ConvertFirstDayOfWeekToJsDay(FirstDayOfWeek f, DayOfWeek defaultValue)
    {
      if (f == FirstDayOfWeek.Default)
      {
        f = (FirstDayOfWeek) defaultValue;
      }
      switch (f)
      {
        case FirstDayOfWeek.Sunday: return 0;
        case FirstDayOfWeek.Monday: return 1;
        case FirstDayOfWeek.Tuesday: return 2;
        case FirstDayOfWeek.Wednesday: return 3;
        case FirstDayOfWeek.Thursday: return 4;
        case FirstDayOfWeek.Friday: return 5;
        case FirstDayOfWeek.Saturday: return 6;
        default: return 0; // Actually this line is unreachable, but the compiler requires it
      }
    }

/**		/// <summary>
		/// Converts an InheritBool to bool.
		/// </summary>
		/// <param name="ib">InheritBool to convert.</param>
		/// <param name="defaultValue">Default boolean value to return.</param>
		/// <returns>Corresponding boolean value. If ib is InheritBool.Inherit, defaultValue is returned.
		/// Otherwise, true or false is returned, as appropriate.</returns>
		public static bool ConvertInheritBoolToBool(InheritBool ib, bool defaultValue)
		{
			switch (ib)
			{
				case InheritBool.True: return true;
				case InheritBool.False: return false;
				default: return defaultValue;  //InheritBool.Inherit
			}
		}
**/
    /// <summary>
    /// Converts a string to an appropriate JavaScript string constant.
    /// </summary>
    /// <param name="s">string to convert.</param>
    /// <returns>A string that represents the given string in javascript.
    /// A null string converts to string "null".
    /// All other strings convert to a single-quoted javascript string ("example" converts to "'example'").
    /// Care is taken to make sure the characters ' and \ are escaped properly.</returns>
    public static string ConvertStringToJSString(string s)
    {
      if (s == null)
      {
        return "null";
      }
      else
      {
        return "'" + s.Replace("\\","\\\\").Replace("'","\\'") + "'";
      }
    }
    
    /// <summary>
    /// Converts a Unit to an appropriate JavaScript constant.
    /// </summary>
    /// <param name="u">Unit to convert.</param>
    /// <returns>A string that represents the unit in javascript.
    /// Unit.Empty converts to string "null".
    /// Units expressed in pixels convert to a string representing a javascript number (eg: "10").
    /// All other Units convert to a quoted javascript string containing the numeric and the unit part (eg: "'10%'").</returns>
    public static string ConvertUnitToJSConstant(Unit u)
    {
      if (u == Unit.Empty)
      {
        return "null";
      }
      else if (u.Type == UnitType.Pixel)
      {
        return u.Value.ToString();
      }
      else
      {
        return "'" + u.ToString() + "'";
      }
    }

    /// <summary>
    /// Resolve the effective URL given its string, a base URL, and the HttpContext.
    /// </summary>
    /// <param name="oContext">HTTP Context.</param>
    /// <param name="sBaseUrl">Base URL.</param>
    /// <param name="sUrl">URL.</param>
    /// <returns>Effective URL.</returns>
    public static string ConvertUrl(HttpContext oContext, string sBaseUrl, string sUrl)
    {
      if (sUrl == null || sUrl == string.Empty || IsUrlAbsolute(sUrl) || (!sUrl.StartsWith("/") && Path.IsPathRooted(sUrl)))
      {
        return sUrl;
      }

      // Do we have a tilde?
      if(sUrl.StartsWith("~") && oContext != null && oContext.Request != null)
      {
        string sAppPath = oContext.Request.ApplicationPath;
        if(sAppPath.EndsWith("/"))
        {
          sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
        }

        return sUrl.Replace("~", sAppPath);
      }

      if(sBaseUrl != string.Empty)
      {
        // Do we have a tilde in the base url?
        if(sBaseUrl.StartsWith("~") && oContext != null && oContext.Request != null)
        {
          string sAppPath = oContext.Request.ApplicationPath;
          if(sAppPath.EndsWith("/"))
          {
            sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
          }
          sBaseUrl = sBaseUrl.Replace("~", sAppPath);
        }

        if(sBaseUrl.EndsWith("/"))
        {
          sBaseUrl = sBaseUrl.Substring(0, sBaseUrl.Length - 1);
        }

        if(sUrl.StartsWith("/"))
        {
          sUrl = sUrl.Substring(1, sUrl.Length - 1);
        }

        return sBaseUrl + "/" + sUrl;
      }

      return sUrl;
    }

    public static string NormalizePath(string path)
    {
        // replace any backslashes 
        path = path.Replace('\\', '/');

        // remove any double forward slashes                 
        int pathLength = 0;
        while (path.Length > 0 && path.Length != pathLength)
        {
            pathLength = path.Length;
            path = path.Replace("//", "/");
        }

        // remove trailing any forward slashes
        while (path.Length > 0 && path.EndsWith("/"))
            path = path.Substring(0, path.Length - 1);

        // split parts of path up                    
        string[] pathParts = path.Split(new char[] { '/' });

        // collapse any . and .. parts
        int index = 0;
        for (int i = 0; i < pathParts.Length; i++)
        {
            string pathPart = pathParts[i];

            // ignore current directory references
            if (pathPart == ".")
                continue;

            // handle parent directory references, if we have a parent
            if (pathPart == ".." && index > 0)
            {
                --index;
                continue;
            }

            // else add path part
            pathParts[index++] = pathPart;
        }

        return String.Join("/", pathParts, 0, index);
    }

    public static string MapPhysicalPath(string path)
    {
        if (path == null || path == String.Empty)
            return String.Empty;

        path = path.Trim();

        if (path == String.Empty)
            return String.Empty;

        if (IsUrlAbsolute(path))
        {
            const string fileProtocol = "file:///";

            if (!path.StartsWith(fileProtocol))
                return String.Empty;

            path = path.Substring(fileProtocol.Length);
        }

        if (!path.StartsWith("/") && Path.IsPathRooted(path))
            return Path.GetFullPath(path);

        HttpContext context = HttpContext.Current;

        if (context == null)
            return path;

        return context.Server.MapPath(path);
    }

    public static string ConvertAbsoluteToVirtualPath(string path)
    {
        HttpContext context = HttpContext.Current;
        string appPath = NormalizePath(MapPhysicalPath(context.Request.ApplicationPath));
        string absPath = NormalizePath(MapPhysicalPath(path));

        if (String.Compare(appPath, 0, absPath, 0, appPath.Length, true) != 0)
            throw new Exception("Path '" + absPath + "' is not descended from Application root.");
            
        string virtualPath = NormalizePath(context.Request.ApplicationPath + absPath.Substring(appPath.Length));

        return virtualPath;
    }

    public static string ConvertVirtualToRelativePath(string path, Page page, Gauge webGauge)
    {
        string dummyRoot = "http://www.componentart.com";
        Uri pageUri = new Uri(dummyRoot + NormalizePath(page.ResolveUrl(".")) + "/");
        Uri pathUri = new Uri(dummyRoot + NormalizePath(webGauge.ResolveUrl(path)) + "/");

#if FW2 || FW3 || FW35
        string relativePath = NormalizePath(pageUri.MakeRelativeUri(pathUri).ToString());        
#else
        string relativePath = NormalizePath(pageUri.MakeRelative(pathUri).ToString());
#endif
        return (relativePath == "" ? "." : relativePath);
    }
    
    public static string DemarcateClientScript(string script)
    {
      return DemarcateClientScript(script, null);
    }
    
    public static string DemarcateClientScript(string script, string title)
    {
      StringBuilder result = new StringBuilder();
      result.Append("<script type=\"text/javascript\">\n");
      result.Append("//<![CDATA[\n");
      if (title != null)
      {
        result.Append("/*** "); result.Append(title); result.Append(" ***/\n");
      }
      result.Append(script);
      result.Append("\n");
      result.Append("//]]>\n");
      result.Append("</script>\n");
      return result.ToString();
    }

    /// <summary>
    /// Private recursive method called by the public FindControl method.
    /// </summary>
    /// <param name="oTopControl">The container within which to search.</param>
    /// <param name="oSourceControl">The subcontainer which has been searched previously, and may therefore be avoided now.</param>
    /// <param name="sId">The ID of the control to find.</param>
    /// <returns>The control with the given ID, or null if none exists in oTopControl.</returns>
    private static Control FindControl(Control oTopControl, Control oSourceControl, string sId)
    {
      if (oTopControl.ID == sId || oTopControl.UniqueID == sId)
      {
        return oTopControl;
      }

      // if we didn't find it here recurse into child containers - except oSourceControl
      if (oTopControl.HasControls())
      {
        foreach (Control oControl in oTopControl.Controls)
        {
          if (oControl != oSourceControl)
          {
            if (oControl.ID == sId || oControl.UniqueID == sId)
            {
              return oControl;
            }

            if (oControl.HasControls()) // We have a container. Let's recurse into it.
            {
              Control oSubControl = FindControl(oControl, null, sId);

              if (oSubControl != null)
              {
                return oSubControl; // We found the control! (in a child container)
              }
            }
          }
        }
      }

      return null; // No control with the given ID found in the hierarchy
    }

    /// <summary>
    /// Finds the control with the given ID in the proximity of the given control.
    /// </summary>
    /// <remarks>
    /// This method is much more intelligent than ASP.NET's FindControl method.
    /// For starters, it recurses into containers.
    /// Secondly, it starts the search in the vicinity of the given control, and then moves further away.
    /// This is necessary for FindControl to properly work for situations like TabStrips+MultiPages across various user controls, master pages, etc.
    /// </remarks>
    /// <param name="oControl">The control near which to start the search.</param>
    /// <param name="sId">The ID of the control to search for.</param>
    /// <returns>The control with the given ID, or null if none exists in the page.</returns>
    public static Control FindControl(Control oControl, string sId)
    {
      Control oFoundControl = null;
      
      oFoundControl = FindControl(oControl, null, sId); // Search within oControl first
      if (oFoundControl != null)
      {
        return oFoundControl; // We found a control with ID sId in this control's subtree (could be this control itself)
      }
      
      // No such control found within oControl.
      // Let's do a "proximity search" for a control with sId.
      // We start in oControl's container.  If we don't find it there, we keep moving up the container tree.
      // All the while we keep track of the subtree we already searched, so we do not duplicate our work.

      Control searchTree = oControl.NamingContainer;
      Control alreadySearchedTree = oControl;

      while (true)
      {
        oFoundControl = FindControl(searchTree, alreadySearchedTree, sId);

        if (oFoundControl != null)
        {
          return oFoundControl;
        }

        if (searchTree.NamingContainer == null || searchTree.NamingContainer == searchTree)
        {
          return null;
        }

        alreadySearchedTree = searchTree;
        searchTree = searchTree.NamingContainer;
      }
    }

    /// <summary>
    /// Determines whether the given string is an absolute URL.
    /// </summary>
    /// <param name="url">The string to examine.</param>
    /// <returns>True if the given string begins with a valid protocol identifier; false otherwise.</returns>
    public static bool IsUrlAbsolute(string url)
    {
      if (url == null) return false;
      string [] protocols = {"about:", "file:///", "ftp://", "gopher://", "http://", "https://", "javascript:", "mailto:", "news:", "res://", "telnet://", "view-source:"};
      foreach (string protocol in protocols)
        if (url.StartsWith(protocol))
          return true;
      return false;
    }

    public static string MakeStringXmlSafe(string sString)
    {
      return sString.Replace("<","&lt;").Replace("+","&#43;");
    }
    
    /// <summary>
    /// Converts the specified object to bool.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding boolean value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot be parsed as a 
    /// bool, a FormatException is raised.</returns>
    public static bool ParseBool(object o, bool defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }

      string sLower = o.ToString().ToLower();

      if (sLower == "true" || sLower == "1")
      {
        return true;
      }

      if (sLower == "false" || sLower == "0")
      {
        return false;
      }

      throw new FormatException("'" + o.ToString() + "' can not be parsed as a bool.");
    }

/**    /// <summary>
    /// Converts the specified object to ClientTargetLevel.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding ClientTargetLevel value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to ClientTargetLevel, a FormatException is raised.</returns>
    public static ClientTargetLevel ParseClientTargetLevel(object o, ClientTargetLevel defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (ClientTargetLevel) Enum.Parse(typeof(ComponentArt.Web.UI.ClientTargetLevel), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a ClientTargetLevel.");
      }
    }
**/
    /// <summary>
    /// Converts the specified object to Color.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding Color value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot be parsed as a
    /// valid HTML Color, a FormatException is raised.</returns>
    public static Color ParseColor(object o, Color defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return ColorTranslator.FromHtml(o.ToString());
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a color.");
      }
    }

/**    /// <summary>
    /// Converts the specified object to ContextMenuType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding ContextMenuType value. If the object is null or converts to
    /// an empty string, default ContextMenuType value is returned. If the object cannot
    /// be converted to ContextMenuType, a FormatException is raised.</returns>
    public static ContextMenuType ParseContextMenuType(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new ContextMenuType();
      }
      try
      {
        return (ContextMenuType) Enum.Parse(typeof(ComponentArt.Web.UI.ContextMenuType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a ContextMenuType.");
      }
    }
**/
    /// <summary>
    /// Converts the specified object to DateTime.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding DateTime value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to DateTime, a FormatException is raised.</returns>
    public static DateTime ParseDateTime(object o, DateTime defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (DateTime) o;
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a DateTime.");
      }
    }

/**    /// <summary>
    /// Converts the specified object to DateTimeFormatType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding DateTimeFormatType value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to DateTimeFormatType, a FormatException is raised.</returns>
    public static DateTimeFormatType ParseDateTimeFormatType(object o, DateTimeFormatType defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (DateTimeFormatType) Enum.Parse(typeof(ComponentArt.Web.UI.DateTimeFormatType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a DateTimeFormatType.");
      }
    }
**/
    /// <summary>
    /// Converts the specified object to DayNameFormat.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding DayNameFormat value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to DayNameFormat, a FormatException is raised.</returns>
    public static DayNameFormat ParseDayNameFormat(object o, DayNameFormat defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (DayNameFormat) Enum.Parse(typeof(System.Web.UI.WebControls.DayNameFormat), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a DayNameFormat.");
      }
    }

    /// <summary>
    /// Converts the specified object to FirstDayOfWeek.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding FirstDayOfWeek value. If the object is null or converts to
    /// an empty string, FirstDayOfWeek.Default is returned. If the object cannot
    /// be converted to FirstDayOfWeek, a FormatException is raised.</returns>
    public static FirstDayOfWeek ParseFirstDayOfWeek(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return FirstDayOfWeek.Default;
      }
      try
      {
        return (FirstDayOfWeek) Enum.Parse(typeof(System.Web.UI.WebControls.FirstDayOfWeek), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a FirstDayOfWeek.");
      }
    }


/**		/// <summary>
		/// Converts the specified object to InheritBool.
		/// </summary>
		/// <param name="o">Object to convert.</param>
		/// <returns>Corresponding InheritBool value. If the object is null or converts to
		/// an empty string, default InheritBool value is returned. If the object cannot
		/// be converted to InheritBool, a FormatException is raised.</returns>
		public static InheritBool ParseInheritBool(object o)
		{
			if (o == null || o.ToString() == "")
			{
				return new InheritBool();
			}
			try
			{
				return (InheritBool) Enum.Parse(typeof(ComponentArt.Web.UI.InheritBool), o.ToString(), true);
			}
			catch
			{
				throw new FormatException("'" + o.ToString() + "' can not be parsed as an InheritBool (inherit, false, or true).");
			}
		}
**/
    /// <summary>
    /// Converts the specified object to int.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding integer value. If the object is null or converts to
    /// an empty string, 0 is returned. If the object cannot be parsed as an integer, 
    /// a FormatException is raised.</returns>
    public static int ParseInt(object o)
    {
      return ParseInt(o, 0);
    }
    
    /// <summary>
    /// Converts the specified object to int.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="iDefault">Default value to return in case of empty object.</param>
    /// <returns>Corresponding integer value. If the object is null or converts to
    /// an empty string, the specified default value is returned. If the object cannot be parsed as an integer, 
    /// a FormatException is raised.</returns>
    public static int ParseInt(object o, int iDefault)
    {
      if (o == null || o.ToString() == "")
      {
        return iDefault;
      }
      try
      {
        return Int32.Parse(o.ToString());
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as an int.");
      }
    }

 
    /// <summary>
    /// Converts the specified object to NextPrevFormat.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding NextPrevFormat value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to NextPrevFormat, a FormatException is raised.</returns>
    public static NextPrevFormat ParseNextPrevFormat(object o, NextPrevFormat defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (NextPrevFormat) Enum.Parse(typeof(System.Web.UI.WebControls.NextPrevFormat), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a NextPrevFormat.");
      }
    }


    /// <summary>
    /// Converts the specified object to Unit.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding Unit value. If the object is null Unit.Empty is returned.
    /// If the object cannot be converted to Unit, a FormatException is raised.</returns>
    public static Unit ParseUnit(object o)
    {
      if (o == null)
      {
        return Unit.Empty;
      }
      else
      {
        return Unit.Parse(o.ToString());
      }
    }

    public static string ResolveBaseUrl(HttpContext context, string baseUrl)
    {
      string resolvedBaseUrl = Utils.ConvertUrl(context, "", baseUrl);
      if (resolvedBaseUrl == null || resolvedBaseUrl == "")
      {
        return resolvedBaseUrl;
      }
      else
      {
        return resolvedBaseUrl + (resolvedBaseUrl.EndsWith("/") ? "" : "/");
      }
    }

    public static void WriteOpenDiv(System.Web.UI.WebControls.WebControl oControl, HtmlTextWriter output)
    {
      // Render the content
      output.Write("<div");

      output.WriteAttribute("id", oControl.ClientID);

      if(oControl.CssClass != string.Empty)
      {
        output.WriteAttribute("class", oControl.CssClass); 
      }
      if(!oControl.Enabled)
      {
        output.WriteAttribute("disabled", "disabled");
      }

      // output style
      output.Write(" style=\"");
      if(!oControl.Width.IsEmpty)
      {
        output.WriteStyleAttribute("width", oControl.Width.ToString());
      }
      if(!oControl.Height.IsEmpty)
      {
        output.WriteStyleAttribute("height", oControl.Height.ToString());
      }
      if(!oControl.BackColor.IsEmpty)
      {
        output.WriteStyleAttribute("background-color", ColorTranslator.ToHtml(oControl.BackColor));
      }
      if(!oControl.BorderWidth.IsEmpty)
      {
        output.WriteStyleAttribute("border-width", oControl.BorderWidth.ToString());
      }
      if(oControl.BorderStyle != BorderStyle.NotSet)
      {
        output.WriteStyleAttribute("border-style", oControl.BorderStyle.ToString());
      }
      if(!oControl.BorderColor.IsEmpty)
      {
        output.WriteStyleAttribute("border-color", ColorTranslator.ToHtml(oControl.BorderColor));
      }

      // write other attributes
      foreach(string sKey in oControl.Style.Keys)
      {
        output.WriteStyleAttribute(sKey, oControl.Style[sKey]);
      }
      output.Write("\">");
    }

    #region DateCollection translation

    public static string DateArrayListToString(ArrayList dateArrayList)
    {
      return Utils.DateArrayListToString(dateArrayList, Utils.DefaultDateTimeFormatString);
    }

    public static string DateArrayListToString(ArrayList dateArrayList, string dateTimeFormatString)
    {
      if (dateArrayList == null || dateArrayList.Count == 0) {return String.Empty;}
      String[] datetexts = new String[dateArrayList.Count];
      for (int i=0; i<dateArrayList.Count; i++)
      {
        datetexts[i] = Utils.DateToString((DateTime)dateArrayList[i], dateTimeFormatString);
      }
      return String.Join(",", datetexts);
    }

    public static string DateToString(DateTime dateTime)
    {
      return Utils.DateToString(dateTime, Utils.DefaultDateTimeFormatString);
    }

    public static string DateToString(DateTime dateTime, string dateTimeFormatString)
    {
      return dateTime.ToString(dateTimeFormatString);
    }

    public static DateTime StringToDate(string dateTimeString)
    {
      return Utils.StringToDate(dateTimeString, Utils.DefaultDateTimeFormatString);
    }

    public static DateTime StringToDate(string dateTimeString, string dateTimeFormatString)
    {
      return DateTime.ParseExact(dateTimeString, dateTimeFormatString, CultureInfo.InvariantCulture);
    }

    public static ArrayList StringToDateArrayList(string dateCollectionString)
    {
      return Utils.StringToDateArrayList(dateCollectionString, Utils.DefaultDateTimeFormatString);
    }

    public static ArrayList StringToDateArrayList(string dateCollectionString, string dateTimeFormatString)
    {
      if (dateCollectionString == null || dateCollectionString.Length == 0) {return new ArrayList();}
      string[] dateStrings = dateCollectionString.Split(',');
      ArrayList dateArrayList = new ArrayList();
      foreach (string dateString in dateStrings)
      {
        dateArrayList.Add(Utils.StringToDate(dateString, dateTimeFormatString));
      }
      return dateArrayList;
    }

    public static readonly string DefaultDateTimeFormatString = "yyyy.M.d";

    #endregion

    public static _BrowserCapabilities BrowserCapabilities(HttpRequest request)
    {
      return new _BrowserCapabilities(request);
    }

    internal class _BrowserCapabilities
    {
      private HttpRequest _request;

      public _BrowserCapabilities(HttpRequest request)
      {
        _request = request;
      }

      public string GetScript()
      {
        StringBuilder result = new StringBuilder();

        #region Needed for Positioning
        result.Append("cart_browser_ie = " + IsBrowserIE.ToString().ToLower() + ";\n");
        result.Append("cart_browser_iemac = " + IsBrowserMacIE.ToString().ToLower() + ";\n");
        result.Append("cart_browser_ie4 = " + IsBrowserIE4.ToString().ToLower() + ";\n");
        result.Append("cart_browser_safari = " + IsBrowserSafari.ToString().ToLower() + ";\n");
        result.Append("cart_browser_konqueror = " + IsBrowserKonqueror.ToString().ToLower() + ";\n");
        #endregion

        #region Needed for Transitions
        result.Append("cart_browser_transitions = " + BrowserSupportsTransitions.ToString().ToLower() + ";\n");
        #endregion

        #region Needed by the Menu
        result.Append("cart_browser_opera = " + IsBrowserOpera.ToString().ToLower() + ";\n"); // for subgroup positioning
        result.Append("cart_browser_mozilla = " + IsBrowserMozilla.ToString().ToLower() + ";\n"); // for subgroup positioning
        result.Append("cart_browser_shadows = " + BrowserSupportsShadows.ToString().ToLower() + ";\n"); // for group shadows
        result.Append("cart_browser_slides = " + BrowserSupportsSlides.ToString().ToLower() + ";\n"); // for group sliding
        result.Append("cart_browser_overlays = " + BrowserSupportsOverlays.ToString().ToLower() + ";\n"); // for windowed element overlays
        result.Append("cart_browser_hideselects = " + BrowserNeedsSelectElementHiding.ToString().ToLower() + ";\n"); // for select element hiding
        result.Append("cart_browser_addeventhandlers = " + BrowserSupportsAddingEventHandlers.ToString().ToLower() + ";\n"); // ExpandOnClick and context menus
        result.Append("cart_browser_contextmenus = " + BrowserSupportsContextMenus.ToString().ToLower() + ";\n"); // context menus
        result.Append("cart_browser_noncustomcontextmenus = " + BrowserSupportsNonCustomContextMenus.ToString().ToLower() + ";\n"); // context menus
        result.Append("cart_browser_expandonclick = " + BrowserSupportsExpandOnClick.ToString().ToLower() + ";\n"); // ExpandOnClick
        result.Append("cart_browser_recyclegroups = " + BrowserSupportsRecyclingGroups.ToString().ToLower() + ";\n"); // group recycling
        #endregion

        return result.ToString();
      }

      public bool BrowserSupportsAddingEventHandlers
      {
        get
        {
          return !IsBrowserMacIE; // Mac IE currently cannot attach a new handler without destroying the present one.
        }
      }

      public bool BrowserSupportsContextMenus
      {
        get
        {
          return BrowserSupportsAddingEventHandlers; // Must be able to attach a handler to document.onmousedown.
        }
      }

      public bool BrowserSupportsExpandOnClick
      {
        get
        {
          return BrowserSupportsAddingEventHandlers; // Must be able to attach a handler to document.onmousedown.
        }
      }

      public bool BrowserSupportsNonCustomContextMenus
      {
        get
        {
          return BrowserSupportsContextMenus && !IsBrowserOpera && !IsBrowserSafari;
          // Opera and Safari disallow overriding of their context menus.
        }
      }

      public bool BrowserSupportsOverlays
      {
        get
        {
          return IsBrowserWinIE5point5plus; // IE 5.5+ on Windows
        }
      }

      public bool BrowserSupportsRecyclingGroups
      {
        get
        {
          return !IsBrowserMacIE; // Mac IE's scrollbars flash when groups are recycled.
        }
      }

      public bool BrowserSupportsShadows
      {
        get
        {
          return IsBrowserWinIE && _request.Browser.MajorVersion >= 6; // IE 6+ on Windows
          /* Note: technically IE 5.5 on Windows also supports shadows, but they can cause some
          * problems such as premature subgroup collapses, and so are not worth it.  */
        }
      }

      public bool BrowserSupportsSlides
      {
        get
        {
          return !IsBrowserNetscape6 && !IsBrowserKonqueror; // All except Netscape 6 and Konqueror
        }
      }

      public bool BrowserSupportsTransitions
      {
        get
        {
          return IsBrowserWinIE5point5plus && !IsPlatfromWinNT; // IE 5.5+ on Windows other than Windows NT4
        }
      }

      public bool BrowserNeedsSelectElementHiding
      {
        get
        {
          return IsBrowserWinIE; // IE on Windows
        }
      }

      public bool IsBrowserIE
      {
        get
        {
          if (_request.UserAgent == null) return false;
          if (IsBrowserOpera) return false;
          return _request.UserAgent.IndexOf("MSIE") >= 0;
        }
      }

      public bool IsBrowserIE4
      {
        get
        {
          return IsBrowserIE && _request.Browser.MajorVersion == 4;
        }
      }

      public bool IsBrowserWinIE
      {
        get
        {
          return IsBrowserIE && IsPlatfromWin;
        }
      }

      public bool IsBrowserWinIE5point5plus
      {
        get
        {
          return IsBrowserWinIE && Version >= 5.5;
        }
      }

      public bool IsBrowserMacIE
      {
        get
        {
          return IsBrowserIE && IsPlatformMac;
        }
      }

      public bool IsBrowserNetscape6
      {
        get
        {
          if (_request.UserAgent == null) return false;
          return _request.UserAgent.IndexOf("Netscape6") >= 0;
        }
      }

      public bool IsBrowserNetscape60
      {
        get
        {
          if (_request.UserAgent == null) return false;
          return _request.UserAgent.IndexOf("Netscape6/6.0") >= 0;
        }
      }

      public bool IsBrowserMozilla
      {
        get
        {
          if (_request.UserAgent == null) return false;
          return _request.UserAgent.IndexOf("Gecko") >= 0;
        }
      }

      public bool IsBrowserOpera
      {
        get
        {
          if (_request.UserAgent == null) return false;
          return _request.UserAgent.IndexOf("Opera") >= 0;
        }
      }

      public bool IsBrowserSafari
      {
        get
        {
          if (_request.UserAgent == null) return false;
          return _request.UserAgent.IndexOf("Safari") >= 0;
        }
      }

      public bool IsBrowserKonqueror
      {
        get
        {
          if (_request.UserAgent == null) return false; 
          return _request.UserAgent.IndexOf("Konqueror") >= 0;
        }
      }

      public bool IsPlatformMac
      {
        get
        {
          return _request.Browser.Platform.ToUpper().StartsWith("MAC");
        }
      }

      public bool IsPlatfromWin
      {
        get
        {
          return _request.Browser.Win32;
        }
      }

      public bool IsPlatfromWinNT
      {
        get
        {
          return IsPlatfromWin && _request.Headers["User-Agent"].IndexOf("NT 4") >= 0; // the only way to check for NT4
        }
      }

      public double Version
      {
        get
        {
          return _request.Browser.MajorVersion + _request.Browser.MinorVersion;
        }
      }

    }

  }

  internal enum JavaScriptArrayType
  {
    Dense,
    Sparse
  }

  internal class JavaScriptArray : ArrayList
  {
    public JavaScriptArray() : base()
    {
      // by not setting type, we're opting for its default value
    }

    public JavaScriptArray(JavaScriptArrayType type) : base()
    {
      this.type = type;
    }

    public JavaScriptArray(ICollection c) : base()
    {
      if (c != null)
      {
        this.AddRange(c);
      }
    }

    private JavaScriptArrayType type;
    public JavaScriptArrayType Type
    {
      get
      {
        return this.type;
      }
      set
      {
        this.type = value;
      }
    }

/**    public override string ToString()
    {
      return this.Type==JavaScriptArrayType.Sparse ? this.ToSparseString() : this.ToDenseString();
    }
**/
/**    private string ToSparseString()
    {
      ArrayList result = new ArrayList();
      int index = 0;
      foreach (object item in this)
      {
        // if it's not one of the scenarios in which we use blanks for storage
        if (!(
          item == null || 
          item.ToString() == string.Empty || 
          (item is Unit && (Unit)item == Unit.Empty) 
          ))
        {
          result.Add(index.ToString());
          if (item is string)
          {
            result.Add("'" + ((string)item).Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "'");
          }
          else if (item is bool)
          {
            result.Add((bool)item ? "1" : "0");
          }
          else if (item is JavaScriptArray)
          {
            result.Add(item.ToString());
          }
          else if (item is ArrayList)
          {
            result.Add((new JavaScriptArray((ArrayList)item)).ToString());
          }
          else if (item is Array)
          {
            result.Add((new JavaScriptArray((Array)item)).ToString());
          }
          else if (item is Unit)
          {
            result.Add(Utils.ConvertUnitToJSConstant((Unit)item));
          }
          else if (item is DateTime)
          {
            result.Add(Utils.ConvertDateTimeToJsDate((DateTime)item));
          }
          else if (item is Enum)
          {
            result.Add(((int)item).ToString());
          }
          else if (item is Color)
          {
            result.Add("'" + ColorTranslator.ToHtml((Color)item) + "'");
          }
          else if (item is ClientTemplate)
          {
            result.Add("['" + ((ClientTemplate)item).ID + "','" + ((ClientTemplate)item).Text.Replace("\n", "").Replace("\r", "").Replace("'", "\\'") + "']");
          }
          else
          {
            result.Add(item.ToString());
          }
        }
        index++;
      }
      return "[" + string.Join(",", (string[])result.ToArray(typeof(string)) ) + "]";
    }
**/
    /**    private string ToDenseString()
        {
          ArrayList oStringList = new ArrayList();

          int iNullsSinceLastItem = 0;
          foreach(object oItem in this)
          {
            // the scenarios in which we use blanks for storage
            if(oItem == null || oItem.ToString() == string.Empty || (oItem is Unit && ((Unit)oItem == Unit.Empty)))
            {
              iNullsSinceLastItem++;
            }
            else
            {
              for(int i = 0; i < iNullsSinceLastItem; i++)
              {
                oStringList.Add(null);
              }

              if(oItem is string)
              {
                oStringList.Add(string.Format("'{0}'", ((string)oItem).Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t")));
              }
              else if (oItem is JavaScriptArray)
              {
                oStringList.Add(oItem.ToString());
              }
              else if (oItem is ArrayList)
              {
                oStringList.Add((new JavaScriptArray((ArrayList)oItem)).ToString());
              }
              else if (oItem is Array)
              {
                oStringList.Add((new JavaScriptArray((Array)oItem)).ToString());
              }
              else if(oItem is bool)
              {
                oStringList.Add((bool)oItem ? "1" : "0");
              }
              else if (oItem is Unit)
              {
                oStringList.Add(Utils.ConvertUnitToJSConstant((Unit)oItem));
              }
              else if (oItem is DateTime)
              {
                oStringList.Add(Utils.ConvertDateTimeToJsDate((DateTime)oItem));
              }
              else if (oItem is Enum)
              {
                oStringList.Add(((int)oItem).ToString());
              }
              else if (oItem is Color)
              {
                oStringList.Add("'" + ColorTranslator.ToHtml((Color)oItem) + "'");
              }
              else if (oItem is ClientTemplate)
              {
                oStringList.Add("['" + ((ClientTemplate)oItem).ID + "','" + ((ClientTemplate)oItem).Text.Replace("\n", "").Replace("\r", "").Replace("'", "\\'") + "']");
              }
              else
              {
                oStringList.Add(oItem.ToString());
              }

              iNullsSinceLastItem = 0;
            }
          }
	
          string [] arArray = (string [])(oStringList.ToArray(typeof(string)));

          return string.Format("[{0}]", string.Join(",", arArray));
        }
    **/
    }
}
