using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Upload"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class UploadClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when a file selection is changed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent FileChange
    {
      get
      {
        return this.GetValue("FileChange");
      }
      set
      {
        this.SetValue("FileChange", value);
      }
    }

    /// <summary>
    /// This event fires when the Upload control is loaded on the client.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Load
    {
      get
      {
        return this.GetValue("Load");
      }
      set
      {
        this.SetValue("Load", value);
      }
    }

    /// <summary>
    /// This event fires when the Upload control gets a progress update during an upload.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ProgressUpdate
    {
      get
      {
        return this.GetValue("ProgressUpdate");
      }
      set
      {
        this.SetValue("ProgressUpdate", value);
      }
    }

    /// <summary>
    /// This event fires when the Upload control starts an upload process.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent UploadBegin
    {
      get
      {
        return this.GetValue("UploadBegin");
      }
      set
      {
        this.SetValue("UploadBegin", value);
      }
    }

    /// <summary>
    /// This event fires when the Upload control encounters an error while uploading.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent UploadError
    {
      get
      {
        return this.GetValue("UploadError");
      }
      set
      {
        this.SetValue("UploadError", value);
      }
    }

    /// <summary>
    /// This event fires when the Upload control ends an upload process.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent UploadEnd
    {
      get
      {
        return this.GetValue("UploadEnd");
      }
      set
      {
        this.SetValue("UploadEnd", value);
      }
    }
  }
}
