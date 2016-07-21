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
  public class SpellCheckCallbackHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
  {
    // Override the ProcessRequest method.
    public void ProcessRequest(HttpContext context)
    {
      string sLanguage = context.Request.Params["lang"];
      string sText = context.Request.Params["text"];
      string sCommand = context.Request.Params["cmd"];
      
      if (sLanguage == null)
      {
        // TODO: handle issue
        throw new Exception("No language given.");
      }

      sText = HttpUtility.UrlDecode(sText, Encoding.Default);

      //context.Response.Write(new JavaScriptArray(Encoding.Default.GetBytes(sText)));
      //context.Response.Write(sText.Contains("\x00"));
      
      SpellChecker oChecker = new SpellChecker(sLanguage);

      string sResponse = "";

      if (sCommand == "check")
      {
        // get extra params
        bool bIgnoreAcronyms = (context.Request.Params["iga"] != null);
        bool bIgnoreEmailAddresses = (context.Request.Params["ige"] != null);
        bool bIgnoreURLs = (context.Request.Params["igu"] != null);
        bool bIgnoreNonAlpha = (context.Request.Params["igna"] != null);

        JavaScriptArray arArrays = new JavaScriptArray();

        if (sText.IndexOf("\x00") >= 0)
        {
          string[] arStrings = sText.Split('\x00');

          for (int i = 0; i < arStrings.Length; i += 2)
          {
            string sPath = arStrings[i];
            string[] arrPath = sPath.Split(',');

            int [] arPath = new int[arrPath.Length];
            for (int p = 0; p < arrPath.Length; p++)
            {
              arPath[p] = int.Parse(arrPath[p]);
            }

            string sString = arStrings[i + 1];

            SpellError[] arErrors = oChecker.Check(sString, bIgnoreAcronyms, bIgnoreEmailAddresses, bIgnoreURLs, bIgnoreNonAlpha);

            foreach (SpellError oError in arErrors)
            {
              arArrays.Add(new object[] { oError.Word, oError.StartIndex, oError.EndIndex, arPath });
            }
          }
        }
        else
        {
          SpellError[] arErrors = oChecker.Check(sText, bIgnoreAcronyms, bIgnoreEmailAddresses, bIgnoreURLs, bIgnoreNonAlpha);

          foreach (SpellError oError in arErrors)
          {
            arArrays.Add(new object[] { oError.Word, oError.StartIndex, oError.EndIndex });
          }
        }

        sResponse = arArrays.ToString();
      }
      else if (sCommand == "suggest")
      {
        // get depth
        string sDepth = context.Request.Params["sd"];
        SpellSuggestionDepthType searchDepth = (SpellSuggestionDepthType)Enum.Parse(typeof(SpellSuggestionDepthType), sDepth);

        // get max suggestions
        string sMax = context.Request.Params["ms"];
        int iMaxSuggestions = int.Parse(sMax);

        ArrayList arSuggestionList = oChecker.GetSuggestions(sText, searchDepth);
        JavaScriptArray arSuggestions = new JavaScriptArray(arSuggestionList.GetRange(0, Math.Min(iMaxSuggestions, arSuggestionList.Count)));
        
        sResponse = arSuggestions.ToString();
      }
      else if (sCommand == "add")
      {
        sResponse = oChecker.AddWordToCustomDictionary(sText).ToString().ToLower();
      }
      else if (sCommand == "remove")
      {
        sResponse = oChecker.RemoveWordFromCustomDictionary(sText).ToString().ToLower();
      }
      else
      {
        // TODO: handle unknown command
      }

      context.Response.ContentType = "text/xml";
      context.Response.Write("<SpellResponse><![CDATA[");
      context.Response.Write(sResponse);
      context.Response.Write("]]></SpellResponse>");
    }

    // Override the IsReusable property.
    public bool IsReusable
    {
      get { return true; }
    }
  }
}
