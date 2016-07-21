using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Caching;

using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges
{
    internal class CachingImageManager: ImageManager
    {

        #region --- Constants ---
        
        private const string IgsBaseUrl = "GaugeHttpHandler.axd?data={0}";

        #endregion

        #region --- Private Member Variables ---
        
        private String m_URLGUID;
   
        #endregion

        #region --- Constructor ---

        public CachingImageManager()
        {
            m_URLGUID = Guid.NewGuid().ToString();
        }

        #endregion

        #region --- ImageManager Override Methods ---

        public override void StoreImage(Bitmap bmp, ImageType imageType, int jpegQuality, int cacheInterval, int deletionDelay)
        {
            MemoryStream ms = new MemoryStream();
            StreamBitmap(bmp, ms, imageType, jpegQuality);
       
            StoredImage storedImage = new StoredImage(ms.ToArray(), imageType, false, null);

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

                expiryDateTime = expiryDateTime.AddSeconds(deletionDelay);
                m_Cache.Add(m_URLGUID, storedImage, null, expiryDateTime, TimeSpan.Zero, _cacheItemPriority, null);
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
                url = String.Format(IgsBaseUrl, m_URLGUID);
            }

            return url;
        }

        #endregion
    }
}
