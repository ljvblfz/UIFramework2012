using System;
using System.Web;

using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Implements an HTTP handler to stream a WebGauge image from a caching service.
    /// </summary>
    public class GaugeHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Handles client requests for images stored in the the application cache.
        /// </summary>
        /// <remarks>
        /// To use this class the following to your web.config file within the <sytem.web> tags:
        /// 		<httpHandlers>
        /// 				<add verb="GET" path="cachedimageservice.axd" type="ComponentArt.Charting.CachedImageService,ComponentArt.Charting.WebChart"/>
        /// 		</httpHandlers>
        /// </remarks>

        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string storageKey = "";

            // Retrieve the DATA query string parameter
            if (context.Request["data"] == null)
            {
                WriteError(null);
                return;
            }else
            {
                storageKey = context.Request["data"].ToString();
            }

            // Grab data from the cache 
            object o = HttpContext.Current.Cache[storageKey];
            if (o == null)
            {
                WriteError(storageKey);
                return;
            }

            StoredImage wcii = o as StoredImage;

            WriteChartImageInfo(wcii);

            if (wcii.RemoveOnAccess)
            {
                HttpContext.Current.Cache.Remove(storageKey);
            }

        }

        #endregion

        #region --- Helper Methods ---
        
        private void WriteChartImageInfo(StoredImage img)
        {
            string typeresponse = "";

            typeresponse = img.DynamicImageType == ImageType.Gif ? "gif" : img.DynamicImageType == ImageType.Jpeg ? "jpeg" : "png";

            HttpContext.Current.Response.ContentType = "image/" + typeresponse;
            HttpContext.Current.Response.OutputStream.Write(img.ImageBytes, 0, img.ImageBytes.Length);
        }

        private void WriteError(string storageKey)
        {
            HttpContext.Current.Response.Write("No image specified.");
        }

        #endregion
    }
}
