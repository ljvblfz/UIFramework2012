using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Editor"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class EditorClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the enter key is pressed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Enter
    {
      get
      {
        return this.GetValue("Enter");
      }
      set
      {
        this.SetValue("Enter", value);
      }
    }

    /// <summary>
    /// This event fires when the current element changes or it's style changes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ContextChange
    {
      get
      {
        return this.GetValue("ContextChange");
      }
      set
      {
        this.SetValue("ContextChange", value);
      }
    }

    /// <summary>
    /// This event fires when the edited element is updated in Inline EditMode.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ContentUpdate
    {
      get
      {
        return this.GetValue("ContentUpdate");
      }
      set
      {
        this.SetValue("ContentUpdate", value);
      }
    }

    /// <summary>
    /// This event fires when the save callback returns with a failure.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SaveError
    {
      get
      {
        return this.GetValue("SaveError");
      }
      set
      {
        this.SetValue("SaveError", value);
      }
    }

    /// <summary>
    /// This event fires when the Editor finishes loading.
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
    /// This event fires when the save callback returns successfully.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SaveSuccess
    {
      get
      {
        return this.GetValue("SaveSuccess");
      }
      set
      {
        this.SetValue("SaveSuccess", value);
      }
    }
  }
}
