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
using System.Reflection;


namespace ComponentArt.Web.UI
{
  internal static class Utils
  {
    public static string GetResourceContent(string sFileName)
    {
      try
      {
        Stream oStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sFileName);
        StreamReader oReader = new StreamReader(oStream);

        return oReader.ReadToEnd();
      }
      catch (Exception ex)
      {
        throw new Exception("Could not read resource \"" + sFileName + "\": " + ex);
      }
    }

    public static string GetResponseUrl(HttpContext oContext)
    {
      string sUrl = 
        oContext.Request.Url.Scheme +
        Uri.SchemeDelimiter +
        oContext.Request.Url.Host +
        (oContext.Request.Url.IsDefaultPort ? "" : (":" + oContext.Request.Url.Port)) +
        oContext.Response.ApplyAppPathModifier(oContext.Request.RawUrl);

      // encode tags to prevent script blocks being broken
      sUrl = sUrl.Replace("<", "%3C");

      return sUrl;
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
      dateConstructor.Append(",");
      dateConstructor.Append(d.Second);
      if (d.Millisecond > 0)
      {
        dateConstructor.Append(",");
        dateConstructor.Append(d.Millisecond);
      }
      dateConstructor.Append(")");
      return dateConstructor.ToString();
    }

    /// <summary>
    /// Converts a DateTime? to an appropriate JavaScript string constant.
    /// </summary>
    /// <param name="d">DateTime? to convert.</param>
    /// <returns>A string that represents the given value in javascript.
    /// An undefined DateTime? converts to string "null".
    /// A defined DateTime? converts to its string representation.</returns>
    public static string ConvertNullableDateTimeToJsDate(DateTime? datetime)
    {
      if (datetime.HasValue)
      {
        return Utils.ConvertDateTimeToJsDate(datetime.Value);
      }
      else
      {
        return "null";
      }
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
    /// Converts a FirstDayOfWeek value to a corresponding DayOfWeek value.
    /// </summary>
    /// <param name="f">FirstDayOfWeek to convert.</param>
    /// <param name="defaultValue">Default first day of week.</param>
    /// <returns>The corresponding DayOfWeek. FirstDayOfWeek.Default is converted to the given defaultValue.</returns>
    public static DayOfWeek ConvertFirstDayOfWeekToDayOfWeek(FirstDayOfWeek f, DayOfWeek defaultValue)
    {
      if (f == FirstDayOfWeek.Default)
      {
        return defaultValue;
      }
      return (DayOfWeek)f;
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

		/// <summary>
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
				default: /*InheritBool.Inherit*/ return defaultValue;
			}
		}

    /// <summary>
    /// Converts a double? to an appropriate JavaScript string constant.
    /// </summary>
    /// <param name="d">double? to convert.</param>
    /// <returns>A string that represents the given value in javascript.
    /// An undefined double? converts to string "null".
    /// A defined double? converts to its string representation.</returns>
    public static string ConvertNullableDoubleToJSString(double? d)
    {
      if (d.HasValue)
      {
        return d.Value.ToString(CultureInfo.InvariantCulture);
      }
      else
      {
        return "null";
      }
    }

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
        return "'" + s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n","\\n") + "'";
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

    public static string ConvertUnitToJSComponentArtUnit(Unit u)
    {
      if (u == Unit.Empty)
      {
        return "new ComponentArt_Unit()";
      }
      else
      {
        return "ComponentArt_Unit.parse('" + u.ToString() + "')";
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
      if(sUrl == null || sUrl == string.Empty || IsUrlAbsolute(sUrl))
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

    /// <summary>
    /// Converts the specified object to CalendarControlType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding CalendarControlType value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to CalendarControlType, a FormatException is raised.</returns>
    public static CalendarControlType ParseCalendarControlType(object o, CalendarControlType defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (CalendarControlType) Enum.Parse(typeof(ComponentArt.Web.UI.CalendarControlType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a CalendarControlType.");
      }
    }

    /// <summary>
    /// Converts the specified object to CalendarPopUpType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding CalendarPopUpType value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to CalendarPopUpType, a FormatException is raised.</returns>
    public static CalendarPopUpType ParseCalendarPopUpType(object o, CalendarPopUpType defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (CalendarPopUpType) Enum.Parse(typeof(ComponentArt.Web.UI.CalendarPopUpType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a CalendarPopUpType.");
      }
    }

    /// <summary>
    /// Converts the specified object to CalendarTitleType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding CalendarTitleType value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to CalendarTitleType, a FormatException is raised.</returns>
    public static CalendarTitleType ParseCalendarTitleType(object o, CalendarTitleType defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (CalendarTitleType) Enum.Parse(typeof(ComponentArt.Web.UI.CalendarTitleType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a CalendarTitleType.");
      }
    }

    /// <summary>
    /// Converts the specified object to CalendarWeekRule.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding CalendarWeekRule value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to CalendarWeekRule, a FormatException is raised.</returns>
    public static CalendarWeekRule ParseCalendarWeekRule(object o, CalendarWeekRule defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (CalendarWeekRule)Enum.Parse(typeof(System.Globalization.CalendarWeekRule), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a CalendarWeekRule.");
      }
    }

    /// <summary>
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

    /// <summary>
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

    /// <summary>
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
    /// Converts the specified object to double.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding double value. If the object is null or converts to
    /// an empty string, 0 is returned. If the object cannot be parsed as a double, 
    /// a FormatException is raised.</returns>
    public static double ParseDouble(object o)
    {
      return ParseDouble(o, 0);
    }

    /// <summary>
    /// Converts the specified object to double.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return in case of empty object.</param>
    /// <returns>Corresponding double value. If the object is null or converts to
    /// an empty string, the specified default value is returned. If the object cannot be parsed as a double, 
    /// a FormatException is raised.</returns>
    public static double ParseDouble(object o, double defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return double.Parse(o.ToString(), CultureInfo.InvariantCulture);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a double.");
      }
    }

    /// <summary>
    /// Converts the specified object to EditorCommandType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding EditorCommandType value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to EditorCommandType, a FormatException is raised.</returns>
    public static EditorCommandType ParseEditorCommandType(object o, EditorCommandType defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (EditorCommandType)Enum.Parse(typeof(ComponentArt.Web.UI.EditorCommandType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a EditorCommandType.");
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

    /// <summary>
    /// Converts the specified object to GroupExpandDirection.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding GroupExpandDirection value. If the object is null or converts to
    /// an empty string, default GroupExpandDirection value is returned. If the object cannot
    /// be converted to GroupExpandDirection, a FormatException is raised.</returns>
    public static GroupExpandDirection ParseGroupExpandDirection(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new GroupExpandDirection();
      }
      try
      {
        return (GroupExpandDirection) Enum.Parse(typeof(ComponentArt.Web.UI.GroupExpandDirection), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a GroupExpandDirection.");
      }
    }

    /// <summary>
    /// Converts the specified object to GroupOrientation.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding GroupOrientation value. If the object is null or converts to
    /// an empty string, default GroupOrientation value is returned. If the object cannot
    /// be converted to GroupOrientation, a FormatException is raised.</returns>
    public static GroupOrientation ParseGroupOrientation(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new GroupOrientation();
      }
      try
      {
        return (GroupOrientation) Enum.Parse(typeof(ComponentArt.Web.UI.GroupOrientation), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a GroupOrientation.");
      }
    }

		/// <summary>
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
    /// Converts the specified object to ItemIconVisibility.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding ItemIconVisibility value. If the object is null or converts to
    /// an empty string, default ItemIconVisibility value is returned. If the object cannot
    /// be converted to ItemIconVisibility, a FormatException is raised.</returns>
    public static ItemIconVisibility ParseItemIconVisibility(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new ItemIconVisibility();
      }
      try
      {
        return (ItemIconVisibility)Enum.Parse(typeof(ComponentArt.Web.UI.ItemIconVisibility), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as ItemIconVisibility.");
      }
    }

    /// <summary>
    /// Converts the specified object to ItemToggleType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding ItemToggleType value. If the object is null or converts to
    /// an empty string, default ItemToggleType value is returned. If the object cannot
    /// be converted to ItemToggleType, a FormatException is raised.</returns>
    public static ItemToggleType ParseItemToggleType(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new ItemToggleType();
      }
      try
      {
        return (ItemToggleType)Enum.Parse(typeof(ComponentArt.Web.UI.ItemToggleType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as ItemToggleType.");
      }
    }

    public static string ParseJSString(object o)
    {
      if (o == null) return null;
      if (o.ToString() == "null") return null;
      else return o.ToString();
    }

    /// <summary>
    /// Converts the specified object to MaskedInputTextMode.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding MaskedInputTextMode value. If the object is null or converts to
    /// an empty string, default MaskedInputTextMode value is returned. If the object cannot
    /// be converted to MaskedInputTextMode, a FormatException is raised.</returns>
    public static MaskedInputTextMode ParseMaskedInputTextMode(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new MaskedInputTextMode();
      }
      try
      {
        return (MaskedInputTextMode)Enum.Parse(typeof(ComponentArt.Web.UI.MaskedInputTextMode), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a MaskedInputTextMode.");
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
    /// Converts the specified object to double?.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding double? value. If the object cannot be parsed as a double, null is returned.</returns>
    public static double? ParseNullableDouble(object o)
    {
      if (o == null || o.ToString() == "" || o.ToString() == "null")
      {
        return new Double?();
      }
      try
      {
        return new Double?(double.Parse(o.ToString(), CultureInfo.InvariantCulture));
      }
      catch
      {
        return new Double?();
      }
    }


    /// <summary>
    /// Converts the specified object to CalendarPopUpExpandDirection.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding CalendarPopUpExpandDirection value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to CalendarPopUpExpandDirection, a FormatException is raised.</returns>
    public static CalendarPopUpExpandDirection ParseCalendarPopUpExpandDirection(object o, CalendarPopUpExpandDirection defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (CalendarPopUpExpandDirection) Enum.Parse(typeof(ComponentArt.Web.UI.CalendarPopUpExpandDirection), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a CalendarPopUpExpandDirection.");
      }
    }

    /// <summary>
    /// Converts the specified object to SlideType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding SlideType value. If the object is null or converts to
    /// an empty string, default SlideType value is returned. If the object cannot
    /// be converted to SlideType, a FormatException is raised.</returns>
    public static SlideType ParseSlideType(object o)
    {
      return ParseSlideType(o, new SlideType());
    }

    /// <summary>
    /// Converts the specified object to SlideType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding SlideType value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to SlideType, a FormatException is raised.</returns>
    public static SlideType ParseSlideType(object o, SlideType defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (SlideType) Enum.Parse(typeof(ComponentArt.Web.UI.SlideType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a SlideType.");
      }
    }

    /// <summary>
    /// Converts the specified object to TabOrientation.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding TabOrientation value. If the object is null or converts to
    /// an empty string, default TabOrientation value is returned. If the object cannot
    /// be converted to TabOrientation, a FormatException is raised.</returns>
    public static TabOrientation ParseTabOrientation(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new TabOrientation();
      }
      try
      {
        return (TabOrientation) Enum.Parse(typeof(ComponentArt.Web.UI.TabOrientation), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a TabOrientation.");
      }
    }

    /// <summary>
    /// Converts the specified object to TabStripAlign.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding TabStripAlign value. If the object is null or converts to
    /// an empty string, default TabStripAlign value is returned. If the object cannot
    /// be converted to TabStripAlign, a FormatException is raised.</returns>
    public static TabStripAlign ParseTabStripAlign(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new TabStripAlign();
      }
      try
      {
        return (TabStripAlign) Enum.Parse(typeof(ComponentArt.Web.UI.TabStripAlign), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a TabStripAlign.");
      }
    }

    /// <summary>
    /// Converts the specified object to TabStripOrientation.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding TabStripOrientation value. If the object is null or converts to
    /// an empty string, default TabStripOrientation value is returned. If the object cannot
    /// be converted to TabStripOrientation, a FormatException is raised.</returns>
    public static TabStripOrientation ParseTabStripOrientation(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new TabStripOrientation();
      }
      try
      {
        return (TabStripOrientation) Enum.Parse(typeof(ComponentArt.Web.UI.TabStripOrientation), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a TabStripOrientation.");
      }
    }

    /// <summary>
    /// Converts the specified object to GroupExpandDirection.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding GroupExpandDirection value. If the object is null or converts to
    /// an empty string, default GroupExpandDirection value is returned. If the object cannot
    /// be converted to GroupExpandDirection, a FormatException is raised.</returns>
    public static TextAlign ParseTextAlign(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new TextAlign();
      }
      try
      {
        return (TextAlign) Enum.Parse(typeof(ComponentArt.Web.UI.TextAlign), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a TextAlign.");
      }
    }

    /// <summary>
    /// Converts the specified object to TimeSpan.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding TimeSpan value. If the object is null or converts to
    /// an empty string, TimeSpan.Zero is returned. If the object cannot
    /// be converted to TimeSpan, a FormatException is raised.</returns>
    public static TimeSpan ParseTimeSpan(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return TimeSpan.Zero;
      }
      TimeSpan result;
      return TimeSpan.TryParse(o.ToString(), out result) ? result : TimeSpan.Zero;
    }

    /// <summary>
    /// Converts the specified object to ToolBarDropDownImagePosition.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding ToolBarDropDownImagePosition value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to ToolBarDropDownImagePosition, a FormatException is raised.</returns>
    public static ToolBarDropDownImagePosition ParseToolBarDropDownImagePosition(object o, ToolBarDropDownImagePosition defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (ToolBarDropDownImagePosition)Enum.Parse(typeof(ComponentArt.Web.UI.ToolBarDropDownImagePosition), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a ToolBarDropDownImagePosition.");
      }
    }

    /// <summary>
    /// Converts the specified object to ToolBarItemType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding ToolBarItemType value. If the object is null or converts to
    /// an empty string, default ToolBarItemType value is returned. If the object cannot
    /// be converted to ToolBarItemType, a FormatException is raised.</returns>
    public static ToolBarItemType ParseToolBarItemType(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new ToolBarItemType();
      }
      try
      {
        return (ToolBarItemType)Enum.Parse(typeof(ComponentArt.Web.UI.ToolBarItemType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a ToolBarItemType.");
      }
    }

    /// <summary>
    /// Converts the specified object to ToolBarTextImageRelation.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <param name="defaultValue">Default value to return.</param>
    /// <returns>Corresponding ToolBarTextImageRelation value. If the object is null or converts to
    /// an empty string, defaultValue is returned. If the object cannot
    /// be converted to ToolBarTextImageRelation, a FormatException is raised.</returns>
    public static ToolBarTextImageRelation ParseToolBarTextImageRelation(object o, ToolBarTextImageRelation defaultValue)
    {
      if (o == null || o.ToString() == "")
      {
        return defaultValue;
      }
      try
      {
        return (ToolBarTextImageRelation)Enum.Parse(typeof(ComponentArt.Web.UI.ToolBarTextImageRelation), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a ToolBarTextImageRelation.");
      }
    }

    /// <summary>
    /// Converts the specified object to TransitionType.
    /// </summary>
    /// <param name="o">Object to convert.</param>
    /// <returns>Corresponding TransitionType value. If the object is null or converts to
    /// an empty string, default TransitionType value is returned. If the object cannot
    /// be converted to TransitionType, a FormatException is raised.</returns>
    public static TransitionType ParseTransitionType(object o)
    {
      if (o == null || o.ToString() == "")
      {
        return new TransitionType();
      }
      try
      {
        return (TransitionType) Enum.Parse(typeof(ComponentArt.Web.UI.TransitionType), o.ToString(), true);
      }
      catch
      {
        throw new FormatException("'" + o.ToString() + "' can not be parsed as a TransitionType.");
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

    public static string DateToString(DateTime dateTime, string dateTimeFormatString)
    {
      return dateTime.ToString(dateTimeFormatString);
    }

    public static DateTime StringToDate(string dateTimeString, string dateTimeFormatString)
    {
      return DateTime.ParseExact(dateTimeString, dateTimeFormatString, CultureInfo.InvariantCulture);
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

      public bool IsBrowserIE
      {
        get
        {
          if (_request.UserAgent == null) return false;
          if (IsBrowserOpera) return false;
          return _request.UserAgent.IndexOf("MSIE") >= 0;
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

    public override string ToString()
    {
      return this.Type==JavaScriptArrayType.Sparse ? this.ToSparseString() : this.ToDenseString();
    }

    private string ToSparseString()
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
          else
          {
            result.Add(item.ToString());
          }
        }
        index++;
      }
      return "[" + string.Join(",", (string[])result.ToArray(typeof(string)) ) + "]";
    }

    private string ToDenseString()
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

  }
}
