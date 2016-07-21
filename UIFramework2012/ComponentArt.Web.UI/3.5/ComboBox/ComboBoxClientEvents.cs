using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="ComboBox"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class ComboBoxClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires just before a callback request is initiated.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent BeforeCallback
    {
      get
      {
        return this.GetValue("BeforeCallback");
      }
      set
      {
        this.SetValue("BeforeCallback", value);
      }
    }

    /// <summary>
    /// This event fires before the selected item is changed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent BeforeChange
    {
      get
      {
        return this.GetValue("BeforeChange");
      }
      set
      {
        this.SetValue("BeforeChange", value);
      }
    }

    /// <summary>
    /// This event fires when a callback request completes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent CallbackComplete
    {
      get
      {
        return this.GetValue("CallbackComplete");
      }
      set
      {
        this.SetValue("CallbackComplete", value);
      }
    }

    /// <summary>
    /// This event fires when a callback request results in an error on the server.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent CallbackError
    {
      get
      {
        return this.GetValue("CallbackError");
      }
      set
      {
        this.SetValue("CallbackError", value);
      }
    }

    /// <summary>
    /// This event fires when the selected item is changed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent Change
    {
      get
      {
        return this.GetValue("Change");
      }
      set
      {
        this.SetValue("Change", value);
      }
    }

    /// <summary>
    /// This event fires when the dropdown is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent Collapse
    {
      get
      {
        return this.GetValue("Collapse");
      }
      set
      {
        this.SetValue("Collapse", value);
      }
    }

    /// <summary>
    /// This event fires when the dropdown is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent Expand
    {
      get
      {
        return this.GetValue("Expand");
      }
      set
      {
        this.SetValue("Expand", value);
      }
    }

    /// <summary>
    /// This event fires when the ComboBox is loaded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
    /// This event fires when the ComboBox is initialized.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent Init
    {
      get
      {
        return this.GetValue("Init");
      }
      set
      {
        this.SetValue("Init", value);
      }
    }
  }
}
