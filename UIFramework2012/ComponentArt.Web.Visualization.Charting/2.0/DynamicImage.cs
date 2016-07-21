using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace ComponentArt.Web.Visualization.Charting
{
	internal class DynamicImage
	{
        const CacheItemPriority _cacheItemPriority = CacheItemPriority.NotRemovable;

		private const string IgsBaseUrl = "cachedimageservice.axd?data={0}";
        private const string ClientsideHtmlDelimeter = "`~$~`";

        String m_clientsideHtmlUrl = String.Empty;
        String m_ImageFile = String.Empty;
        String m_StorageKey = String.Empty;
        String m_URLGUID = String.Empty;

		public DynamicImage()
		{ }
		
		internal string StorageKey
		{
            get { return m_StorageKey; }
            set { m_StorageKey = value; }
		}

        internal string ClientsideHtmlUrl
		{
            get { return m_clientsideHtmlUrl; }
            set { m_clientsideHtmlUrl = value; }
		}        

        internal void StoreClientsideHtml(Chart wc)
        {
            if (wc.SaveImageOnDisk)
            {
                try //write the two HTML chunks to disk
                {
                    TextWriter tw = new StreamWriter(ClientsideHtmlUrl, false);
                    tw.WriteLine(wc.WebChartImageInfo.ClientsidePostbackHtml);
                    tw.WriteLine(ClientsideHtmlDelimeter);
                    tw.WriteLine(wc.WebChartImageInfo.ClientsideCallbackHtml);
                    tw.Flush();
                    tw.Close();

                    wc.WebChartImageInfo.ClientsidePostbackHtml = null;
                    wc.WebChartImageInfo.ClientsideCallbackHtml = null;

                    wc.WebChartImageInfo.ClientsideHtmlFileName = ClientsideHtmlUrl;
                    //this.DynamicImage_PreRender(this, EventArgs.Empty);
                    StoreData(wc.WebChartImageInfo, wc);
                }
                catch (Exception e)
                {
                     throw new IOException("Could not cache client-side settings onto the file system at '" + ClientsideHtmlUrl + "'.", e);
                }
            }
            else
            {
                StoreData(wc.WebChartImageInfo, wc);
            }
        }

        internal String ReadClientsideHtml(Chart wc, bool isCallback)
        {
            ImageEntry ie = (ImageEntry)wc.Page.Cache[wc.Key];
            String chartGuid = ((ImageEntry)wc.Page.Cache[wc.Key]).Guid;
            ChartImageInfo wcii = (ChartImageInfo)wc.Page.Cache[chartGuid];
               
            if (wc.SaveImageOnDisk)
            {
                try //read the two HTML chunks from disk
                {
                    TextReader tr = new StreamReader(wcii.ClientsideHtmlFileName);

                    String allHtml = tr.ReadToEnd();
                    int splitAt = allHtml.IndexOf(ClientsideHtmlDelimeter);
                    String[] htmlChunks = new String[] { allHtml.Substring(0, splitAt), allHtml.Substring(splitAt + ClientsideHtmlDelimeter.Length) };

                    tr.Close();

                    if (isCallback)
                        return htmlChunks[1];
                    else
                        return htmlChunks[0];
                }
                catch (Exception e)
                {
                    throw new IOException("Could not read client-side settings from the file system at '" + ClientsideHtmlUrl + "'.", e);
                }                
            }
            else
            {
                if (isCallback)
                    return wcii.ClientsideCallbackHtml;
                else
                    return wcii.ClientsidePostbackHtml;
            }
        }

		// PROPERTY: GetImageUrl 
		// DESCRIPT: Calculates and returns the actual URL served to the browser 
		internal string GetImageUrl(Chart wc)
		{
			string url = "";

            if (StorageKey.Length == 0)
            {
                StorageKey = wc.Key;
                
                if (URLGUID.Length == 0)
                {
                    URLGUID = Guid.NewGuid().ToString();
                }
            }

            if (wc.UseCached)
            {
                return ((ImageEntry)wc.Page.Cache[StorageKey]).ImageUrl;
            } 

			// Check ImageFile 
			if (ImageFile.Length >0)
			{
				url = ImageFile;
			}
			else // Check ImageBytes and Image
			{
				if (wc.WebChartImageInfo != null)
				{
					url = GetCachedImageUrl();
				}
			}

			return url;
		}

        // PROPERTY: ImageFile
		// DESCRIPT: URL to the file to display (when set defaults to Image)
        public string ImageFile
		{
            get { return m_ImageFile; }
            set { m_ImageFile = value; }
        }

        internal string URLGUID
        {
            //get {return Convert.ToString(ViewState["urlGuid"]);}
            //set {ViewState["urlGuid"] = value;}
            get { return m_URLGUID; }
            set { m_URLGUID = value; }
        }

		// PROPERTY: GetCachedImageUrl 
		// DESCRIPT: Return the URL of the cached image
		private string GetCachedImageUrl()
        {
            return String.Format(IgsBaseUrl, URLGUID);
		}

        // METHOD:   StoreData
		// DESCRIPT: Helper method to store data to the cache
        internal void StoreData(ChartImageInfo data, Chart wc)
		{
            if (wc.Page != null && wc.Page.Cache[StorageKey] == null)
            {
                DateTime now = DateTime.Now;

                int cacheInterval = (wc.Customized && !wc.ClientsideCustomizedImageCachingEnabled) ? 0 : wc.CacheInterval;

                ImageEntry ie = new ImageEntry();
                ie.DateTimeStored = now;
                ie.ImageUrl = GetImageUrl(wc);
                ie.ClientsideHtmlUrl = ClientsideHtmlUrl;
				ie.Guid = URLGUID;

                DateTime expiryDateTime = now.AddSeconds(cacheInterval);

                // Add to the page table
                if (wc.CacheInterval > 0)
                {
                    wc.Page.Cache.Add(StorageKey,
                        ie,
                        null,
                        expiryDateTime,
                        TimeSpan.Zero,
                        _cacheItemPriority,
                        null);
                }

                CacheItemRemovedCallback removeCallback = null;

                if (data.FileName != null && data.FileName.Length > 0)
                    removeCallback = new CacheItemRemovedCallback(this.RemovedCallback);

#if DEBUG
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " Adding " + URLGUID);
#endif

                wc.Page.Cache.Add(URLGUID,
                    data,
                    null,
                    expiryDateTime.AddSeconds(wc.DeletionDelay),
                    TimeSpan.Zero,
                    _cacheItemPriority,
                    removeCallback);
            }
		}

        [Serializable]
        internal class ImageEntry
        {
            private string m_guid;
            private DateTime m_dateTimeStored;
            private string m_imageUrl;
            private string m_maps;
            private string m_clientsideHtmlUrl;

            internal string Maps
            {
                get
                {
                    return m_maps;
                }
                set
                {
                    m_maps = value;
                }
            }

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

            internal string ClientsideHtmlUrl
            {
                get { return m_clientsideHtmlUrl; }
                set { m_clientsideHtmlUrl = value; }
            }
        }

        public void RemovedLogCallback(String k, Object v, CacheItemRemovedReason r)
        {
        }



        public void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " Removing " + k + " reason " +
                r.ToString());
#endif

            if (!(v is ChartImageInfo))
                return;
            
            ChartImageInfo wcii = (ChartImageInfo)v;

            // remove the image file
            if (wcii.FileName != null && wcii.FileName != string.Empty)
            {
                if (System.IO.File.Exists(wcii.FileName))
                    System.IO.File.Delete(wcii.FileName);
            }

            // remove the cached client-side HTML file
            if (wcii.ClientsideHtmlFileName != null && wcii.ClientsideHtmlFileName != string.Empty)
            {
                if (System.IO.File.Exists(wcii.ClientsideHtmlFileName))
                    System.IO.File.Delete(wcii.ClientsideHtmlFileName);
            }
        }

        internal DateTime GetStoredDate(string storageKey, Chart wc)
        {
            if (wc.Page.Cache[storageKey] == null)
                return DateTime.MinValue;
            return ((ImageEntry)wc.Page.Cache[storageKey]).DateTimeStored;
        }
	}
}