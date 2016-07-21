using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="ToolBar"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class ToolBarClientEvents : ClientEvents
  {

    /// <summary>
    /// This event fires when an item's dropdown hides.
    /// </summary>
    [Description("This event fires when an item's dropdown hides.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DropDownHide
    {
      get
      {
        return this.GetValue("DropDownHide");
      }
      set
      {
        this.SetValue("DropDownHide", value);
      }
    }

    /// <summary>
    /// This event fires when an item's dropdown shows.
    /// </summary>
    [Description("This event fires when an item's dropdown shows.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DropDownShow
    {
      get
      {
        return this.GetValue("DropDownShow");
      }
      set
      {
        this.SetValue("DropDownShow", value);
      }
    }

    /// <summary>
    /// This event fires before a toggle item is checked or unchecked and can cancel the action.
    /// </summary>
    [Description("This event fires before a toggle item is checked or unchecked and can cancel the action.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemBeforeCheckChange
    {
      get
      {
        return this.GetValue("ItemBeforeCheckChange");
      }
      set
      {
        this.SetValue("ItemBeforeCheckChange", value);
      }
    }

    /// <summary>
    /// This event fires before an item command has been executed and can cancel the action.
    /// </summary>
    [Description("This event fires before an item command has been executed and can cancel the action.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemBeforeSelect
    {
      get
      {
        return this.GetValue("ItemBeforeSelect");
      }
      set
      {
        this.SetValue("ItemBeforeSelect", value);
      }
    }

    /// <summary>
    /// This event fires after a toggle item has been checked or unchecked.
    /// </summary>
    [Description("This event fires after a toggle item has been checked or unchecked.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemCheckChange
    {
      get
      {
        return this.GetValue("ItemCheckChange");
      }
      set
      {
        this.SetValue("ItemCheckChange", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse button is pressed over an item.
    /// </summary>
    [Description("This event fires when the mouse button is pressed over an item.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemMouseDown
    {
      get
      {
        return this.GetValue("ItemMouseDown");
      }
      set
      {
        this.SetValue("ItemMouseDown", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer moves away from an item.
    /// </summary>
    [Description("This event fires when the mouse pointer moves away from an item.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemMouseOut
    {
      get
      {
        return this.GetValue("ItemMouseOut");
      }
      set
      {
        this.SetValue("ItemMouseOut", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer moves over an item.
    /// </summary>
    [Description("This event fires when the mouse pointer moves over an item.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemMouseOver
    {
      get
      {
        return this.GetValue("ItemMouseOver");
      }
      set
      {
        this.SetValue("ItemMouseOver", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse button is released over an item.
    /// </summary>
    [Description("This event fires when the mouse button is released over an item.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemMouseUp
    {
      get
      {
        return this.GetValue("ItemMouseUp");
      }
      set
      {
        this.SetValue("ItemMouseUp", value);
      }
    }

    /// <summary>
    /// This event fires after an item command has been executed.
    /// </summary>
    [Description("This event fires after an item command has been executed.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemSelect
    {
      get
      {
        return this.GetValue("ItemSelect");
      }
      set
      {
        this.SetValue("ItemSelect", value);
      }
    }

    /// <summary>
    /// This event fires when the ToolBar is done loading on the client.
    /// </summary>
    [Description("This event fires when the ToolBar is done loading on the client.")]
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
    /// This event fires when a web service call completes successfully.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent WebServiceComplete
    {
      get
      {
        return this.GetValue("WebServiceComplete");
      }
      set
      {
        this.SetValue("WebServiceComplete", value);
      }
    }

    /// <summary>
    /// This event fires when a web service call results in an error.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent WebServiceError
    {
      get
      {
        return this.GetValue("WebServiceError");
      }
      set
      {
        this.SetValue("WebServiceError", value);
      }
    }
  }
}
