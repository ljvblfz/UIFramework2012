using System;

using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges
{
    [Serializable]
    internal class StoredImage
    {
        byte[] m_imageBytes;
        ImageType m_ImageType;
        bool m_removeOnAccess = false;
        string m_physicalFilePath = string.Empty;

        public StoredImage(byte[] imageBytes, ImageType imageType, bool removeOnAccess, string physicalFilePath)
        {
            m_imageBytes = imageBytes;
            m_ImageType = imageType;
            m_removeOnAccess = removeOnAccess;
            m_physicalFilePath = physicalFilePath;
        }

        public bool RemoveOnAccess
        {
            get { return m_removeOnAccess; }
        }

        public byte[] ImageBytes
        {
            get { return m_imageBytes; }
            set { m_imageBytes = value; }
        }

        public ImageType DynamicImageType
        {
            get { return m_ImageType; }
            set { m_ImageType = value; }
        }

        public string PhysicalFilePath
        {
            get
            {
                return m_physicalFilePath;
            }
            set
            {
                m_physicalFilePath = value;
            }
        }
    }

    [Serializable]
    internal class StoredImageInfo
    {
        private string m_guid;
        private DateTime m_dateTimeStored;
        private string m_imageUrl;

        internal string Guid
        {
            get { return m_guid; }
            set { m_guid = value; }
        }

        internal DateTime DateTimeStored
        {
            get { return m_dateTimeStored; }
            set { m_dateTimeStored = value; }
        }

        internal string ImageUrl
        {
            get { return m_imageUrl; }
            set { m_imageUrl = value; }
        }
    }
}
