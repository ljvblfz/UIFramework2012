using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="BaseInput"/> controls.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class BaseInputClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the input textbox loses focus.
    /// </summary>
    [Description("This event fires when the input textbox loses focus.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Blur
    {
      get
      {
        return this.GetValue("Blur");
      }
      set
      {
        this.SetValue("Blur", value);
      }
    }

    /// <summary>
    /// This event fires when the input textbox is clicked on.
    /// </summary>
    [Description("This event fires when the input textbox is clicked on.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Click
    {
      get
      {
        return this.GetValue("Click");
      }
      set
      {
        this.SetValue("Click", value);
      }
    }

    /// <summary>
    /// This event fires when the content is cut from the input textbox.
    /// </summary>
    [Description("This event fires when the content is cut from the input textbox.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Cut
    {
      get
      {
        return this.GetValue("Cut");
      }
      set
      {
        this.SetValue("Cut", value);
      }
    }

    /// <summary>
    /// This event fires when the input textbox gains focus.
    /// </summary>
    [Description("This event fires when the input textbox gains focus.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Focus
    {
      get
      {
        return this.GetValue("Focus");
      }
      set
      {
        this.SetValue("Focus", value);
      }
    }

    /// <summary>
    /// This event fires when the input textbox onkeydown fires.
    /// </summary>
    [Description("This event fires when the input textbox onkeydown fires.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent KeyDown
    {
      get
      {
        return this.GetValue("KeyDown");
      }
      set
      {
        this.SetValue("KeyDown", value);
      }
    }

    /// <summary>
    /// This event fires when the input textbox onkeypress fires.
    /// </summary>
    [Description("This event fires when the input textbox onkeypress fires.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent KeyPress
    {
      get
      {
        return this.GetValue("KeyPress");
      }
      set
      {
        this.SetValue("KeyPress", value);
      }
    }

    /// <summary>
    /// This event fires when the input textbox onkeyup fires.
    /// </summary>
    [Description("This event fires when the input textbox onkeyup fires.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent KeyUp
    {
      get
      {
        return this.GetValue("KeyUp");
      }
      set
      {
        this.SetValue("KeyUp", value);
      }
    }

    /// <summary>
    /// This event fires when the control is done loading on the client.
    /// </summary>
    [Description("This event fires when the control is done loading on the client.")]
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
    /// This event fires when the content is pasted into the input textbox.
    /// </summary>
    [Description("This event fires when the content is pasted into the input textbox.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Paste
    {
      get
      {
        return this.GetValue("Paste");
      }
      set
      {
        this.SetValue("Paste", value);
      }
    }
  }
}
