using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web;


namespace ComponentArt.Web.UI
{
  class UploadProgressHandler : IHttpHandler
  {
    // Override the ProcessRequest method.
    public void ProcessRequest(HttpContext context)
    {
      string sUploadId = context.Request.Params["CartUploadId"];

      // suppress caching
      context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      context.Response.ContentType = "text/xml";

      UploadInfo oUploadInfo = (UploadInfo)context.Application["CartUpload_" + sUploadId];

      // was there an error?
      if (oUploadInfo != null && oUploadInfo.Error != null)
      {
        context.Response.Write("<UploadError><![CDATA[");
        context.Response.Write(oUploadInfo.Error);
        context.Response.Write("]]></UploadError>");
      }
      else
      {
        // construct JSON for status
        StringBuilder oSB = new StringBuilder();

        oSB.Append("var UploadProgressData = {");
        if (oUploadInfo == null)
        {
          oSB.Append("'ReceivedBytes': 0,");
          oSB.Append("'TotalBytes': 0,");
          oSB.Append("'CurrentFile': '',");
          oSB.Append("'Progress': 0,");
          oSB.Append("'ElapsedTime': 0,");
          oSB.Append("'RemainingTime': 0,");
          oSB.Append("'Speed': 0,");
        }
        else
        {
          double iElapsedSeconds = DateTime.Now.Subtract(oUploadInfo.StartTime).TotalSeconds;

          oSB.Append("'ReceivedBytes': " + oUploadInfo.ReceivedBytes + ",");
          oSB.Append("'TotalBytes': " + oUploadInfo.TotalBytes + ",");
          oSB.Append("'CurrentFile': " + Utils.ConvertStringToJSString(oUploadInfo.CurrentFile) + ",");
          oSB.AppendFormat(CultureInfo.InvariantCulture, "'Progress': {0},", oUploadInfo.Progress);
          oSB.AppendFormat(CultureInfo.InvariantCulture, "'ElapsedTime': {0},", iElapsedSeconds);
          oSB.AppendFormat(CultureInfo.InvariantCulture, "'RemainingTime': {0},", oUploadInfo.Progress == 0? 0 : (iElapsedSeconds * (1 - oUploadInfo.Progress) / oUploadInfo.Progress));
          oSB.AppendFormat(CultureInfo.InvariantCulture, "'Speed': {0},", ((oUploadInfo.ReceivedBytes / 1024) / iElapsedSeconds));
        }
        oSB.Append("'UploadActive': " + (oUploadInfo == null ? "false" : "true"));
        oSB.Append("};");

        context.Response.Write("<UploadProgress><![CDATA[");
        context.Response.Write(oSB.ToString());
        context.Response.Write("]]></UploadProgress>");
      }      
    }

    // Override the IsReusable property.
    public bool IsReusable
    {
      get { return true; }
    }
  }
}
