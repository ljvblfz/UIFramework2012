using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Attribute indicating that a server-side method can be asynchronously called from the client by a <see cref="CallBack"/>  control.
  /// </summary>
  /// <seealso cref="CallbackHandler"/>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class ComponentArtCallbackMethod : Attribute
  {
  }

  /// <summary>
  /// HttpHandler that enables server-side methods to be asynchronously called from the client by a <see cref="CallBack"/>  control.
  /// </summary>
  /// <remarks>
  /// By assigning this handler to a page in Web.config's httpHandlers section, the page gets the ability to
  /// have its public methods marked with the <see cref="ComponentArtCallbackMethod" /> attribute be accessible
  /// on the client via AJAX calls. The client-side method of the same name will return the value returned by its
  /// server-side counterpart.
  /// </remarks>
  public class CallbackHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
  {
    #region Private Methods

    private string GenerateClientScriptBlock(HttpContext context, Page page, string sDefaultPath, string sScriptFile)
    {
      string sScript = string.Empty;
      string sScriptLocation = string.Empty;
      string sVersionString =
        Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "_" +
        Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "_" +
        Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
		
      
      // First, try application config variable
      string sLocation = ConfigurationSettings.AppSettings["ComponentArt.Web.UI.ClientScriptLocation"];
      if(sLocation != null)
      {
        sScriptLocation = Path.Combine(Path.Combine(sLocation, sVersionString), sScriptFile).Replace("\\", "/");
      }
      
      // Next, try server root
      if(sScriptLocation == string.Empty)
      {
        try
        {
          string sStandardRootClientScriptPath = Path.Combine(Path.Combine("/componentart_webui_client", sVersionString), sScriptFile).Replace("\\", "/");

          if(File.Exists(context.Server.MapPath(sStandardRootClientScriptPath)))
          {
            sScriptLocation = sStandardRootClientScriptPath;
          }
        } 
        catch {}
      }

      // If failed, try application root
      if(sScriptLocation == string.Empty)
      {
        try
        {
          string sAppRootClientScriptPath = Path.Combine(Path.Combine(Path.Combine(context.Request.ApplicationPath, "componentart_webui_client"), sVersionString), sScriptFile).Replace("\\", "/");

          if(File.Exists(context.Server.MapPath(sAppRootClientScriptPath)))
          {
            sScriptLocation = sAppRootClientScriptPath;
          }
        } 
        catch {}
      }
      
      if(sScriptLocation != string.Empty)
      {
        // Do we have a tilde?
        if(sScriptLocation.StartsWith("~") && context != null && context.Request != null)
        {
          string sAppPath = context.Request.ApplicationPath;
          if(sAppPath.EndsWith("/"))
          {
            sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
          }

          sScriptLocation = sScriptLocation.Replace("~", sAppPath);
        }

        if(File.Exists(context.Server.MapPath(sScriptLocation)))
        {
          sScript = "<script src=\"" + sScriptLocation + "\" type=\"text/javascript\"></script>";
        }
        else
        {
          throw new Exception(sScriptLocation + " not found");
        }
      }
      else 
      {
        // If everything failed, emit our internal script
        string sResourceUrl = page.ClientScript.GetWebResourceUrl(this.GetType(), sDefaultPath + "." + sScriptFile);
        sScript = "<script src=\"" + sResourceUrl + "\" type=\"text/javascript\"></script>";

      }

      return sScript;
    }

    // get all methods starting from the given control, looking for the ComponentArtCallbackMethod attribute
    // on public methods on the page, or static methods in controls
    private ArrayList GetAllMethods(Control oControl, ArrayList arList)
    {
      //HttpContext.Current.Response.Write(oControl.UniqueID + " " + oControl.Controls.Count + " ");

      if (arList == null) arList = new ArrayList();

      MethodInfo[] methodInfos;

      if (oControl is Page)
      {
        methodInfos = oControl.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
      }
      else
      {
        methodInfos = oControl.GetType().GetMethods(BindingFlags.Static);
      }

      // check each method for our attribute
      foreach (MethodInfo methodInfo in methodInfos)
      {
        object[] arAttributes = methodInfo.GetCustomAttributes(typeof(ComponentArtCallbackMethod), false);

        // we have one with the right attribute set, and we haven't added it yet?
        if (arAttributes != null && arAttributes.Length > 0 && !arList.Contains(methodInfo))
        {
          arList.Add(methodInfo);
        }
      }

      // go into child controls
      if (oControl.HasControls())
      {
        foreach (Control oChildControl in oControl.Controls)
        {
          if (oChildControl is UserControl)
          {
            GetAllMethods(oChildControl, arList);
          }
        }
      }

      //HttpContext.Current.Response.Write(arList.Count + "<br>\n");

      return arList;
    }

    private string GetResourceContent(string sFileName)
    {
      try
      {
        Stream oStream = Assembly.GetAssembly(this.GetType()).GetManifestResourceStream(sFileName);
        StreamReader oReader = new StreamReader(oStream);
  		
        return oReader.ReadToEnd();
      }
      catch(Exception ex)
      {
        throw new Exception("Could not read resource \"" + sFileName + "\": " + ex);
      }
    }

    private void RegisterScript(HttpContext context, Page page, string sDefaultPath, string sScriptFile)
    {
      string sScript = GenerateClientScriptBlock(context, page, sDefaultPath, sScriptFile);
		  
      page.RegisterStartupScript(sDefaultPath + "." + sScriptFile, sScript);
    }

    #endregion

    // Override the ProcessRequest method.
    public void ProcessRequest(HttpContext context)
    {
      Page page = (Page)PageParser.GetCompiledPageInstance(context.Request.Path, null, context);

      string sMethodName = context.Request.Params["Cart_Callback_Method"];
      
      if(sMethodName != null)
      {
        MethodInfo foundMethod = null;
        
        ArrayList arMethods = GetAllMethods(page, null);
        
        string[] arParamStrings = context.Request.Params.GetValues("Cart_Callback_Method_Param");
        int iNumParams = arParamStrings == null ? 0 : arParamStrings.Length;

        // go through the methods
        foreach (MethodInfo methodInfo in arMethods)
        {
          // we have one with the right name?
          if (methodInfo.Name == sMethodName)
          {
            foundMethod = methodInfo;
            break;
          }
        }

        // not found in page? try the user controls
        if (foundMethod == null)
        {
          
        }

        // found matching method?
        if (foundMethod != null)
        {
          ParameterInfo[] arMethodParams = foundMethod.GetParameters();

          // we have one with the right name and number of parameters?
          if (iNumParams == arMethodParams.Length)
          {
            object[] arParams = null;

            if (arParamStrings != null)
            {
              arParams = new Object[arParamStrings.Length];

              for (int i = 0; i < arParamStrings.Length; i++)
              {
                arParams[i] = Convert.ChangeType(arParamStrings[i], arMethodParams[i].ParameterType);
              }
            }

            object oReturnValue = foundMethod.Invoke(page, arParams);

            string sResponse = oReturnValue == null ? "" : oReturnValue.ToString();

            context.Response.ContentType = "text/xml";

            // write the response in a safe way
            context.Response.Write("<returns><![CDATA[");
            context.Response.Write(sResponse.Replace("]]>", "$$$CART_CDATA_CLOSE$$$"));
            context.Response.Write("]]></returns>");

            return; // done
          }
        }

        throw new Exception("No suitable method (\"" + sMethodName + "\" with " + iNumParams + " arguments) found in \"" + page.GetType() + "\".");
      }
      else
      {
        if(!page.IsClientScriptBlockRegistered("A573L991.js"))
        {
          page.RegisterClientScriptBlock("A573L991.js", "");
          RegisterScript(context, page, "ComponentArt.Web.UI.client_scripts", "A573L991.js");
        }

        StringBuilder oSB = new StringBuilder();
        oSB.Append("<script language=\"javascript\">\n//<![CDATA[\n\n");

        ArrayList arMethods = GetAllMethods(page, null);
        
        foreach (MethodInfo methodInfo in arMethods)
        {
          oSB.AppendFormat("window.{0} = function() {{ return ComponentArt_AjaxCall('{0}', arguments, true); }}\n", methodInfo.Name);
        }

        oSB.Append("//]]>\n</script>\n");

        page.RegisterStartupScript("ComponentArt_AjaxMethods", oSB.ToString());

        page.ProcessRequest(context);
      }
    }

    // Override the IsReusable property.
    public bool IsReusable
    {
      get { return true; }
    }
  }
}
