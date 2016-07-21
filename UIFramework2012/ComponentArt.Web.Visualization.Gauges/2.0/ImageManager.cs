using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Caching;

using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies the image format to render.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// Graphics Interchange Format (GIF) image format.
        /// </summary>
        Gif,
        /// <summary>
        /// Joint Photographic Experts Group (JPEG) image format.
        /// </summary>
        Jpeg,
        /// <summary>
        /// W3C Portable Network Graphics (PNG) image format.
        /// </summary>
        Png
    }
    
    internal abstract class ImageManager
    {
        #region --- Constants ---

        protected const CacheItemPriority _cacheItemPriority = CacheItemPriority.NotRemovable;

        #endregion

        #region --- Public Properties ---

        protected String m_StorageKey = String.Empty;
        public String StorageKey
        {
            get
            {
                return m_StorageKey;
            }
            set
            {
                m_StorageKey = value;
            }
        }

        protected Cache m_Cache = null;
        public Cache Cache
        {
            get
            {
                return m_Cache;
            }
            set
            {
                m_Cache = value;
            }
        }

        private string m_customFileName = null;
        public string CustomImageFileName
        {
            get
            {
                return m_customFileName;
            }
            set
            {
                m_customFileName = value;
            }
        }

        private string m_designModeFileName = null;
        internal string DesignModeFileName
        {
            get
            {
                return m_designModeFileName;
            }
            set
            {
                m_designModeFileName = value;
            }
        }

        string m_physicalOutputDirectory = String.Empty;
        public string PhysicalOutputDirectory
        {
            get
            {
                return m_physicalOutputDirectory;
            }
            set
            {
                m_physicalOutputDirectory = value;          
            }
        }

        string m_virtualOutputDirectory = String.Empty;
        public string VirtualOutputDirectory
        {
            get
            {
                return m_virtualOutputDirectory;
            }
            set
            {
                m_virtualOutputDirectory = value;
            }
        }

        #endregion

        #region --- Public Methods ---

        public abstract void StoreImage(Bitmap bmp, ImageType imageType, int jpegQuality, int cacheInterval, int deletionDelay);
        public abstract string GetImageUrl(ImageType imageType);
        
        public bool StoredImageAvailable()
        {
            StoredImageInfo imageInfo = (StoredImageInfo)m_Cache[m_StorageKey];
            return (imageInfo != null);
        }

        #endregion

        #region --- Helper Methods ---
        
        protected void StreamBitmap(Bitmap bmp, Stream stream, ImageType imageType, int jpegQuality)
        {
            ImageFormat imageFormat;
            bool isJpeg = false;

            if (DesignModeFileName != null && DesignModeFileName != String.Empty)
            {
                string fileExt = Path.GetExtension(DesignModeFileName).ToLower();

                if (fileExt == ".jpg")
                    imageFormat = ImageFormat.Jpeg;
                else if (fileExt == ".gif")
                    imageFormat = ImageFormat.Gif;
                else if (fileExt == ".png")
                    imageFormat = ImageFormat.Png;
                else
                    imageFormat = ImageFormat.Bmp;

                jpegQuality = 100;
                isJpeg = (fileExt == ".jpg");
            }
            else
            {
                imageFormat = (imageType == ImageType.Jpeg) ? ImageFormat.Jpeg :
                    (imageType == ImageType.Gif) ? ImageFormat.Gif : ImageFormat.Png;

                isJpeg = (imageType == ImageType.Jpeg);
            }

            if (isJpeg)
            {
                ImageCodecInfo codecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(encoder, (long)jpegQuality);
                encoderParameters.Param[0] = encoderParameter;

                bmp.Save(stream, codecInfo, encoderParameters);
            }
            else
            {
                bmp.Save(stream, imageFormat);
            }
            
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        #endregion
    }
}
