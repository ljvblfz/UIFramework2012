using System;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.IO;


namespace ComponentArt.Web.Visualization.Charting
{
    /// <summary>
    /// Handles client requests for images stored in the the application cache.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// To use this class the following to your web.config file within the <sytem.web> tags:
    /// 		<httpHandlers>
    /// 				<add verb="GET" path="cachedimageservice.axd" type="ComponentArt.Web.Visualization.Charting.CachedImageService,ComponentArt.Web.Visualization.Charting.Shader.Chart"/>
    /// 		</httpHandlers>
    /// ]]>
    /// </remarks>
	public class CachedImageService : IHttpHandler
	{
        
		// Override the IsReusable property
		/// <summary>
        /// Gets a value indicating whether another request can use the <see cref="CachedImageService"/> instance.
		/// </summary>
        public bool IsReusable { get { return true; } }

		// Override the ProcessRequest method
        /// <summary>
        /// Processes cached image requests.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			string storageKey = "";

            // If looking to retrieve the spacer.gif
            if (context.Request["spacer"] != null)
            {
                Bitmap spacer = new Bitmap(this.GetType(), "Resources.spacer.gif");
                
                MemoryStream ms = new MemoryStream();
                // Save to memory using the Jpeg format
                spacer.Save(ms, ImageFormat.Gif);

                // read to end
                byte[] bmpBytes = ms.GetBuffer();
                spacer.Dispose();
                ms.Close();
            
                HttpContext.Current.Response.ContentType = "image/gif";
                HttpContext.Current.Response.OutputStream.Write(bmpBytes, 0, bmpBytes.Length);
                return;
            }

			// Retrieve the DATA query string parameter
			if (context.Request["data"] == null)
			{
				WriteError(null);
				return;
			}
			else storageKey = context.Request["data"].ToString();

			// Grab data from the cache 
			object o = HttpContext.Current.Cache[storageKey];
			if (o == null)
			{
                WriteError(storageKey);
				return;
			}

            ChartImageInfo wcii = o as ChartImageInfo;

            if (wcii == null)
            {
                return;
            }

            WriteChartImageInfo(wcii);

            if (wcii.RemoveOnAccess)
            {
                HttpContext.Current.Cache.Remove(storageKey);
            }

		}

		private void WriteImageBytes(byte[] img)
		{
			HttpContext.Current.Response.ContentType = "image/jpeg"; 
			HttpContext.Current.Response.OutputStream.Write(img, 0, img.Length);
		}

		private void WriteImage(System.Drawing.Image img)
		{
			HttpContext.Current.Response.ContentType = "image/jpeg"; 
			img.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
		}

		private void WriteChartImageInfo(ChartImageInfo img)
		{
			string typeresponse = "";
            
			typeresponse = img.WebChartImageType == ChartImageType.Gif ? "gif" : img.WebChartImageType == ChartImageType.Jpeg ? "jpeg" : "png";

			HttpContext.Current.Response.ContentType = "image/" + typeresponse; 
			HttpContext.Current.Response.OutputStream.Write(img.ImageBytes, 0, img.ImageBytes.Length);
		}

		private void WriteError(string storageKey)
        {
			HttpContext.Current.Response.Write("No image specified");
		}
	}
}