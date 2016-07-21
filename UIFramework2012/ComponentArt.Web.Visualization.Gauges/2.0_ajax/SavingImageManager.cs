using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Caching;
using System.Web;

using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges
{
    internal class SavingImageManager : ImageManager
    {
        #region --- Constants ---

        protected const string _baseFileName = "CArtWebGauge";

        #endregion

        #region --- Private Member Variables ---

        private String m_URLGUID;
        private Object m_fileNameLock = new Object();

        #endregion

        #region --- Constructor ---

        public SavingImageManager()
        {
            m_URLGUID = Guid.NewGuid().ToString();
        }

        #endregion

        #region --- ImageManager Override Methods ---

        public override void StoreImage(Bitmap bmp, ImageType imageType, int jpegQuality, int cacheInterval, int deletionDelay)
        {
            string filePath;
            string physicalOutputDirectory;
            
            if (DesignModeFileName != null && DesignModeFileName != String.Empty) 
            {
                filePath = DesignModeFileName;
                physicalOutputDirectory = Path.GetDirectoryName(filePath);
            }
            else 
            {
                //Determine file path
                string ext = (imageType == ImageType.Gif ? "gif" : imageType == ImageType.Jpeg ? "jpg" : "png");
                string nameWithExt = EffectiveImageFilename(_baseFileName) + "." + ext;
                physicalOutputDirectory = PhysicalOutputDirectory;
                filePath = physicalOutputDirectory + "/" + nameWithExt;
            }

            if (!Directory.Exists(physicalOutputDirectory))
            {
                throw new DirectoryNotFoundException("Directory '" + physicalOutputDirectory + "' does not exist.");
            }
            
            lock (m_fileNameLock)
            {
                // TODO: Replace this with removing the cachine entry for the file and deleting the file
                while (System.IO.File.Exists(filePath))
                {
                    System.Threading.Thread.Sleep(50);
                }

                try
                {
                    //Write image to disk
                    FileStream stream = new FileStream(filePath, FileMode.Create);

                    StreamBitmap(bmp, stream, imageType, jpegQuality);
                    stream.Flush();
                    stream.Close();
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    throw new IOException("Could not write image onto the file system at '" + filePath + "'.");
                }
                
                // Save image info to the cache

                if (m_StorageKey == String.Empty)
                    return;

                if (m_Cache[m_StorageKey] == null)
                {
                    DateTime now = DateTime.Now;
                    StoredImageInfo ie = new StoredImageInfo();
                    ie.DateTimeStored = now;
                    ie.Guid = m_URLGUID;
                    ie.ImageUrl = GetImageUrl(imageType);

                    DateTime expiryDateTime = now.AddSeconds(cacheInterval);

                    // Add to the page table
                    if (cacheInterval > 0)
                    {
                        m_Cache.Add(m_StorageKey, ie, null, expiryDateTime, TimeSpan.Zero, _cacheItemPriority, null);
                    }

                    StoredImage storedImage = new StoredImage(null, imageType, false, filePath);
                    CacheItemRemovedCallback removeCallback = new CacheItemRemovedCallback(this.RemovedCallback);

                    expiryDateTime = expiryDateTime.AddSeconds(deletionDelay);
                    m_Cache.Add(m_URLGUID, storedImage, null, expiryDateTime, TimeSpan.Zero, _cacheItemPriority, removeCallback);
                }
            }
        }

        // Calculates and returns the actual URL served to the browser.
        public override string GetImageUrl(ImageType imageType)
        {
            string url = "";

            StoredImageInfo imageInfo = (StoredImageInfo)m_Cache[m_StorageKey];
            if (imageInfo != null)
            {
                url = imageInfo.ImageUrl;
            }
            else
            {
                string ext = (imageType == ImageType.Gif ? "gif" : imageType == ImageType.Jpeg ? "jpg" : "png");
                url = VirtualOutputDirectory + "/" + CustomImageFileName + "." + ext;           
            }

            return url;
        }

        #endregion

        #region --- Helper Methods --- 

        private string EffectiveImageFilename(string name)
        {
            if (CustomImageFileName != null)
                CustomImageFileName = CustomImageFileName.Trim();
            if (CustomImageFileName == null || CustomImageFileName == "")
            {
                Guid g = Guid.NewGuid();
                CustomImageFileName = name + "-" + g.ToString();
            }
            else
            {
                // strip extension
                string[] parts = CustomImageFileName.Split('.');
                CustomImageFileName = parts[0];
            }
            return CustomImageFileName;
        }

        /// <summary>
        /// Determines whether the given string is an absolute URL.
        /// </summary>
        /// <param name="url">The string to examine.</param>
        /// <returns>True if the given string begins with a valid protocol identifier; false otherwise.</returns>
        internal static bool IsUrlAbsolute(string url)
        {
            if (url == null) return false;
            string[] protocols = { "about:", "file:///", "ftp://", "gopher://", "http://", "https://", "javascript:", "mailto:", "news:", "res://", "telnet://", "view-source:" };
            foreach (string protocol in protocols)
                if (url.StartsWith(protocol))
                    return true;
            return false;
        }

        /// <summary>
        /// Resolve the effective URL given its string, a base URL, and the HttpContext.
        /// </summary>
        /// <param name="oContext">HTTP Context.</param>
        /// <param name="sBaseUrl">Base URL.</param>
        /// <param name="sUrl">URL.</param>
        /// <returns>Effective URL.</returns>
        internal static string ConvertUrl(HttpContext oContext, string sBaseUrl, string sUrl)
        {
            if (sUrl == null || sUrl == string.Empty || IsUrlAbsolute(sUrl))
            {
                return sUrl;
            }

            // Do we have a tilde?
            if (sUrl.StartsWith("~") && oContext != null && oContext.Request != null)
            {
                string sAppPath = oContext.Request.ApplicationPath;
                if (sAppPath.EndsWith("/"))
                {
                    sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
                }

                return sUrl.Replace("~", sAppPath);
            }

            if (sBaseUrl != string.Empty)
            {
                // Do we have a tilde in the base url?
                if (sBaseUrl.StartsWith("~") && oContext != null && oContext.Request != null)
                {
                    string sAppPath = oContext.Request.ApplicationPath;
                    if (sAppPath.EndsWith("/"))
                    {
                        sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
                    }
                    sBaseUrl = sBaseUrl.Replace("~", sAppPath);
                }

                if (sBaseUrl.EndsWith("/"))
                {
                    sBaseUrl = sBaseUrl.Substring(0, sBaseUrl.Length - 1);
                }

                if (sUrl.StartsWith("/"))
                {
                    sUrl = sUrl.Substring(1, sUrl.Length - 1);
                }

                return sBaseUrl + "/" + sUrl;
            }

            return sUrl;
        }

        public void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
#if DEBUG
            //Debug.WriteLine(DateTime.Now.ToString() + " Removing " + k + " reason " + r.ToString());
#endif

            if (!(v is StoredImage))
                return;

            StoredImage wcii = (StoredImage)v;

            // remove the image file
            if (wcii.PhysicalFilePath != null && wcii.PhysicalFilePath != string.Empty)
            {
                if (System.IO.File.Exists(wcii.PhysicalFilePath))
                    System.IO.File.Delete(wcii.PhysicalFilePath);
            }
        }
        #endregion
    }
}
