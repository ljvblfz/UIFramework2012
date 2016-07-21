using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// UploadedFileInfo class. Contains information about a file uploaded using the Upload control.
  /// </summary>
  public class UploadedFileInfo
  {
    /// <summary>
    /// The name of the file.
    /// </summary>
    public string FileName;

    /// <summary>
    /// The full path to the temporary file containing the file data.
    /// </summary>
    public string TempFileName;

    /// <summary>
    /// The type of content contained in the file.
    /// </summary>
    public string ContentType;

    /// <summary>
    /// The file extension, if any.
    /// </summary>
    public string Extension;

    /// <summary>
    /// The size of the file in bytes.
    /// </summary>
    public long Size;

    // Internal members
    internal long StartOffset;
    internal long EndOffset;
    internal long LastWriteOffset;
    internal FileStream FileStream;

    #region Methods

    /// <summary>
    /// Remove this uploaded file.
    /// </summary>
    public void Delete()
    {
      File.Delete(TempFileName);
    }

    public FileStream GetStream()
    {
      return File.Open(TempFileName, FileMode.Open, FileAccess.Read);
    }

    public void SaveAs(string sPath)
    {
      this.SaveAs(sPath, false);
    }

    public void SaveAs(string sPath, bool bOverWrite)
    {
      File.Copy(TempFileName, sPath, bOverWrite);

      File.Delete(TempFileName);
    }

    #endregion
  }

  /// <summary>
  /// UploadInfo class - this contains information relating to an upload control instance
  /// </summary>
  class UploadInfo : IDisposable
  {
    public string TempFolder;
    public long TotalBytes;
    public long ReceivedBytes;
    public double Progress;
    public string CurrentFile;
    public long MaximumBytes;
    public int MaximumFiles;
    public bool Aborted;
    public string Error;
    public bool Handled;
    public bool Debug;
    public ArrayList UploadedFiles = new ArrayList();

    internal string Boundary;
    internal DateTime StartTime;

    public void Dispose()
    {
      // remove all files remaining
      foreach (UploadedFileInfo oFileInfo in UploadedFiles)
      {
        oFileInfo.Delete();
      }
    }
  }

  /// <summary>
  /// UploadModule class - this HTTP module handles the actual file uploading process.
  /// </summary>
  class UploadModule : IHttpModule
  {
    public void Dispose()
    {
    }
    
    private string GetContentBoundary(HttpRequest oRequest)
    {
      string sMarker = "boundary=";

      int iMarkerIndex = oRequest.ContentType.IndexOf(sMarker);

      if (iMarkerIndex < 0) return null;

      return "--" + oRequest.ContentType.Substring(iMarkerIndex + sMarker.Length);
    }

    private HttpWorkerRequest GetHttpWorkerRequest(HttpContext oContext)
    {
      return (HttpWorkerRequest)(oContext.GetType().GetProperty("WorkerRequest", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(oContext, null));
    }

    private void HandleBeginRequest(object sender, EventArgs args)
    {
      HttpApplication oApplication = (HttpApplication)sender;

      if (this.IsFileUpload(oApplication.Request))
      {
        HttpWorkerRequest oHWR = this.GetHttpWorkerRequest(oApplication.Context);

        if (oHWR != null)
        {
          UploadInfo oUploadInfo = this.InitializeUploadInfo(oApplication);

          try
          {
            if (!oUploadInfo.Aborted)
            {
              // read in the upload
              this.HandleUpload(oApplication, oHWR, oUploadInfo);
            }
          }
          catch (Exception ex)
          {
            if (oUploadInfo.Debug)
            {
              oUploadInfo.Error = "An error has occured on the server:\n" + ex + "\n\n" + "To disable verbose error messages, set Debug to false on the Upload control instance.";
            }
            else
            {
              oUploadInfo.Error = "An error has occured on the server, resulting in this upload being aborted.";
            }
            oUploadInfo.Aborted = true;
          }
          finally
          {
            // TODO: make this vary somehow?
            // we need the process to go on at least long enough for the client
            // to read the status and halt things on that end

            // wait for a max of 10 seconds for the upload to be handled
            for (int i = 0; i < 10; i++)
            {
              if (oUploadInfo.Handled)
              {
                break;
              }

              System.Threading.Thread.Sleep(1000);
            }

            // stop the request from completing (posting)
            oApplication.Response.End();
          }
        }
      }
    }

    private void HandleEndRequest(object sender, EventArgs args)
    {
      HttpApplication oApplication = (HttpApplication)sender;
      if (this.IsFileUpload(oApplication.Request))
      {
        this.RemoveUploadInfo((HttpApplication)sender);
      }
    }

    private void HandleError(object sender, EventArgs args)
    {
      HttpApplication oApplication = (HttpApplication)sender;
      if (this.IsFileUpload(oApplication.Request))
      {
        this.RemoveUploadInfo((HttpApplication)sender);
      }
    }

    public void HandleUpload(HttpApplication oApplication, HttpWorkerRequest oHWR, UploadInfo oInfo)
    {
      const int BUFFER_SIZE = 32768;

      if (oHWR.HasEntityBody())
      {
        // Start parser
        UploadParser oParser = new UploadParser();

        int iChunkReceivedBytes = 0;

        byte[] arChunk = new byte[BUFFER_SIZE];

        oInfo.ReceivedBytes = 0;
        // Get upload file size
        oInfo.TotalBytes = int.Parse(oHWR.GetKnownRequestHeader(HttpWorkerRequest.HeaderContentLength));

        // is the upload too big
        if (oInfo.MaximumBytes > 0 && oInfo.TotalBytes > oInfo.MaximumBytes)
        {
          throw new Exception("Upload too big.");
        }

        oParser.StartProcess(oInfo.TotalBytes);

        // Load pre-loaded data
        byte[] arFirstChunk = oHWR.GetPreloadedEntityBody();

        if (arFirstChunk != null)
        {
          oInfo.ReceivedBytes = iChunkReceivedBytes = arFirstChunk.Length;

          int iOffset = 0;

          // is there enough stuff preloaded for processing?
          while(iChunkReceivedBytes >= BUFFER_SIZE)
          {
            Array.Copy(arFirstChunk, iOffset, arChunk, 0, BUFFER_SIZE);

            oParser.ProcessChunk(oInfo, arChunk, BUFFER_SIZE);

            iOffset += BUFFER_SIZE;
            iChunkReceivedBytes -= BUFFER_SIZE;
          }
          
          // is there leftover data?
          if(iChunkReceivedBytes > 0)
          {
            // it will be processed later, store it and move on
            Array.Copy(arFirstChunk, iOffset, arChunk, 0, iChunkReceivedBytes);
          }
        }

        // Is there more?
        if (!oHWR.IsEntireEntityBodyIsPreloaded())
        {
          int iReceivedBytes = 0;

          while (oInfo.TotalBytes > oInfo.ReceivedBytes && oHWR.IsClientConnected())
          {
            // Read the next chunk

            iReceivedBytes = oHWR.ReadEntityBody(arChunk, iChunkReceivedBytes, Math.Min(BUFFER_SIZE - iChunkReceivedBytes, (int)(oInfo.TotalBytes - oInfo.ReceivedBytes)));
            if (iReceivedBytes == 0)
            {
              break;
            }

            oInfo.ReceivedBytes += iReceivedBytes;
            iChunkReceivedBytes += iReceivedBytes;

            // have we filled the buffer?
            if (iChunkReceivedBytes == BUFFER_SIZE)
            {
              // Process the chunk
              oParser.ProcessChunk(oInfo, arChunk, BUFFER_SIZE);

              // Restart buffer
              iChunkReceivedBytes = 0;
            }
          }
        }

        // Process the last chunk
        oParser.ProcessChunk(oInfo, arChunk, iChunkReceivedBytes);

        // Complete process
        oParser.FinishProcess(oInfo);
      }
    }

    public void Init(HttpApplication oApplication)
    {
      oApplication.BeginRequest += new EventHandler(this.HandleBeginRequest);
      oApplication.EndRequest += new EventHandler(this.HandleEndRequest);
      oApplication.Error += new EventHandler(this.HandleError);
    }

    private UploadInfo InitializeUploadInfo(HttpApplication oApplication)
    {
      string sId = oApplication.Request.QueryString["CartUploadId"];

      UploadInfo oInfo = (UploadInfo)HttpContext.Current.Application["CartUpload_" + sId];

      // Upload info not found?
      if (oInfo != null)
      {
        oInfo.Boundary = this.GetContentBoundary(oApplication.Request);
        oInfo.StartTime = DateTime.Now;
      }
      else
      {
        oInfo = new UploadInfo();
        oInfo.Aborted = true;
        oInfo.Error = "Upload credentials expired.";

        HttpContext.Current.Application["CartUpload_" + sId] = oInfo;
      }

      return oInfo;
    }

    private bool IsFileUpload(HttpRequest oRequest)
    {
      const string BeginString = "multipart/form-data";

      return (oRequest.ContentType.Substring(0, Math.Min(BeginString.Length, oRequest.ContentType.Length)).ToLower().StartsWith(BeginString) &&
        oRequest.QueryString["CartUploadId"] != null);
    }

    private void RemoveUploadInfo(HttpApplication oApplication)
    {
      string sId = oApplication.Request.QueryString["CartUploadId"];

      UploadInfo oInfo = (UploadInfo)HttpContext.Current.Application["CartUpload_" + sId];
      if (oInfo != null)
      {
        foreach (UploadedFileInfo oFileInfo in oInfo.UploadedFiles)
        {
          oFileInfo.Delete();
        }

        HttpContext.Current.Application.Remove("CartUpload_" + sId);
      }
    }
  }
}
