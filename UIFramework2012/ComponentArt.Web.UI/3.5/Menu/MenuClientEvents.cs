using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Menu"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class MenuClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the context menu is hidden.
    /// </summary>
    [Description("This event fires when the context menu is hidden.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ContextMenuHide
    {
      get
      {
        return this.GetValue("ContextMenuHide");
      }
      set
      {
        this.SetValue("ContextMenuHide", value);
      }
    }

    /// <summary>
    /// This event fires when the context menu is shown.
    /// </summary>
    [Description("This event fires when the context menu is shown.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ContextMenuShow
    {
      get
      {
        return this.GetValue("ContextMenuShow");
      }
      set
      {
        this.SetValue("ContextMenuShow", value);
      }
    }

    /// <summary>
    /// This event fires when a group of items starts collapsing.
    /// </summary>
    [Description("This event fires when a group of items starts collapsing.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GroupCollapseBegin
    {
      get
      {
        return this.GetValue("GroupCollapseBegin");
      }
      set
      {
        this.SetValue("GroupCollapseBegin", value);
      }
    }

    /// <summary>
    /// This event fires when a group of items finishes collapsing.
    /// </summary>
    [Description("This event fires when a group of items finishes collapsing.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GroupCollapseEnd
    {
      get
      {
        return this.GetValue("GroupCollapseEnd");
      }
      set
      {
        this.SetValue("GroupCollapseEnd", value);
      }
    }

    /// <summary>
    /// This event fires when a group of items starts expanding.
    /// </summary>
    [Description("This event fires when a group of items starts expanding.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GroupExpandBegin
    {
      get
      {
        return this.GetValue("GroupExpandBegin");
      }
      set
      {
        this.SetValue("GroupExpandBegin", value);
      }
    }

    /// <summary>
    /// This event fires when a group of items finishes expanding.
    /// </summary>
    [Description("This event fires when a group of items finishes expanding.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GroupExpandEnd
    {
      get
      {
        return this.GetValue("GroupExpandEnd");
      }
      set
      {
        this.SetValue("GroupExpandEnd", value);
      }
    }

    /// <summary>
    /// This event fires before a toggle item is checked or unchecked and can cancel the action.
    /// </summary>
    [Description("This event fires before a toggle item is checked or unchecked and can cancel the action.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
    /// This event fires before a mouse over and can cancel the action.
    /// </summary>
    [Description("This event fires before a mouse over and can cancel the action.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemBeforeMouseOver
    {
      get
      {
        return this.GetValue("ItemBeforeMouseOver");
      }
      set
      {
        this.SetValue("ItemBeforeMouseOver", value);
      }
    }

    /// <summary>
    /// This event fires before an item is selected and can cancel the action.
    /// </summary>
    [Description("This event fires before an item is selected and can cancel the action.")]
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

    // ItemBlur event is unimplemented for now.  This event will be added in a future release.
    ///// <summary>
    ///// This event fires when an item loses focus.
    ///// </summary>
    //[Description("This event fires when an item loses focus.")]
    //[NotifyParentProperty(true)]
    //[PersistenceMode(PersistenceMode.InnerProperty)]
    //[DefaultValue(null)]
    //public ClientEvent ItemBlur
    //{
    //  get
    //  {
    //    return this.GetValue("ItemBlur");
    //  }
    //  set
    //  {
    //    this.SetValue("ItemBlur", value);
    //  }
    //}

    /// <summary>
    /// This event fires after a toggle item has been checked or unchecked.
    /// </summary>
    [Description("This event fires after a toggle item has been checked or unchecked.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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

    // ItemFocus event is unimplemented for now.  This event will be added in a future release.
    ///// <summary>
    ///// This event fires when an item gains focus.
    ///// </summary>
    //[Description("This event fires when an item gains focus.")]
    //[NotifyParentProperty(true)]
    //[PersistenceMode(PersistenceMode.InnerProperty)]
    //[DefaultValue(null)]
    //public ClientEvent ItemFocus
    //{
    //  get
    //  {
    //    return this.GetValue("ItemFocus");
    //  }
    //  set
    //  {
    //    this.SetValue("ItemFocus", value);
    //  }
    //}
    
    /// <summary>
    /// This event fires when the mouse pointer moves away from a menu item.
    /// </summary>
    [Description("This event fires when the mouse pointer moves away from a menu item.")]
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
    /// This event fires when the mouse pointer moves over a menu item.
    /// </summary>
    [Description("This event fires when the mouse pointer moves over a menu item.")]
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
    /// This event fires when an item is selected.
    /// </summary>
    [Description("This event fires when an item is selected.")]
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
    /// This event fires when the Menu is done loading on the client.
    /// </summary>
    [Description("This event fires when the Menu is done loading on the client.")]
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
