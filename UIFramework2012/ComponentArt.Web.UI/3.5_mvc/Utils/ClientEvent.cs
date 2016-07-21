using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  [ToolboxItem(false)]
  public class ClientEventConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      if (sourceType == typeof(string))
      {
        return true;
      }

      return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertTo(
        ITypeDescriptorContext context,
        CultureInfo culture,
        object value,
        Type destinationType)
    {
      if (destinationType == typeof(string))
      {
        return ((ClientEvent)value).EventHandler;
      }

      return base.ConvertTo(
          context,
          culture,
          value,
          destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      if (value is string)
      {
        return new ClientEvent((string)value);
      }

      return base.ConvertFrom(context, culture, value);
    }
  }

  /// <summary>
  /// Client event handler definition.
  /// </summary>
  [ToolboxItem(false)]
  [DefaultProperty("EventHandler")]
  [TypeConverter(typeof(ClientEventConverter))]
  public class ClientEvent
  {
    public ClientEvent()
    {
    }

    public ClientEvent(string eventHandler)
    {
      _eventHandler = eventHandler;
    }

    private string _eventHandler;
    [DefaultValue("")]
    public string EventHandler
    {
      get
      {
        return _eventHandler == null ? "" : _eventHandler;
      }
      set
      {
        _eventHandler = value;
      }
    }
  }

  [ToolboxItem(false)]
  public class ClientEventsConverter : ExpandableObjectConverter
  {
    public override object ConvertTo(
        ITypeDescriptorContext context,
        CultureInfo culture,
        object value,
        Type destinationType)
    {
      if (destinationType == typeof(string))
      {
        return "";
      }

      return base.ConvertTo(
          context,
          culture,
          value,
          destinationType);
    }
  }

  /// <summary>
  /// Container for client event definitions.
  /// </summary>
  [ToolboxItem(false)]
  [DefaultProperty("EventHandler")]
  [TypeConverter(typeof(ClientEventsConverter))]
  public class ClientEvents
  {
    private System.Web.UI.AttributeCollection _handlers;
    internal System.Web.UI.AttributeCollection Handlers
    {
      get
      {
        if (_handlers == null)
        {
          StateBag oBag = new StateBag(true);
          _handlers = new System.Web.UI.AttributeCollection(oBag);
        }

        return _handlers;
      }
      set
      {
        _handlers = value;
      }
    }

    protected ClientEvent GetValue(string sKey)
    {
      string sValue = Handlers[sKey];
      return sValue == null ? null : new ClientEvent(sValue);
    }

    protected void SetValue(string sKey, ClientEvent oValue)
    {
      if (oValue != null)
      {
        Handlers[sKey] = oValue.EventHandler;
      }
      else
      {
        Handlers.Remove(sKey);
      }
    }
  }
}
