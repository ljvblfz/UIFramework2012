using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Xml;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Represents an item in a <see cref="ComboBox"/> control.
  /// </summary>
  public class ComboBoxItem
  {
    internal Hashtable Properties = null;
    internal ComboBox ParentComboBox = null;

    #region Public Properties

    /// <summary>
    /// The client template to use for this item.
    /// </summary>
    [DefaultValue("")]
    public string ClientTemplateId
    {
      get
      {
        string s = (string)Properties["ClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for this item.
    /// </summary>
    [DefaultValue("")]
    public string CssClass
    {
      get
      {
        string s = (string)Properties["CssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["CssClass"] = value;
      }
    }

    /// <summary>
    /// Whether this item is enabled.
    /// </summary>
    [DefaultValue(true)]
    public bool Enabled
    {
      get
      {
        return Utils.ParseBool((string)Properties["Enabled"], true);
      }
      set
      {
        Properties["Enabled"] = value.ToString();
      }
    }

    /// <summary>
    /// The unique identifier of this item.
    /// </summary>
    [DefaultValue("")]
    public string Id
    {
      get
      {
        string s = (string)Properties["Id"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["Id"] = value;
      }
    }

    /// <summary>
    /// Whether this item is currently selected.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool Selected
    {
      get
      {
        if (ParentComboBox != null && ParentComboBox.SelectedItem == this)
        {
          return true;
        }

        return false;
      }
      set
      {
        if (ParentComboBox != null)
        {
          if (value == true)
          {
            ParentComboBox.SelectedItem = this;
          }
          else if(ParentComboBox.SelectedItem == this)
          {
            ParentComboBox.SelectedItem = null;
          }
        }
      }
    }

    /// <summary>
    /// The text for this item.
    /// </summary>
    [DefaultValue("")]
    public string Text
    {
      get
      {
        string s = (string)Properties["Text"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["Text"] = value;
      }
    }

    /// <summary>
    /// The value for this item (if different from text).
    /// </summary>
    [DefaultValue("")]
    public string Value
    {
      get
      {
        string s = (string)Properties["Value"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["Value"] = value;
      }
    }

    #endregion

    #region Public Methods

    public ComboBoxItem()
    {
      Properties = new Hashtable();
    }

    public ComboBoxItem(string sText)
    {
      Properties = new Hashtable();

      this.Text = sText;
    }

    /// <summary>
    /// Returns a value from the item indexed by the field name or index.
    /// </summary>
    /// <param name="obj">Field name or numeric index.</param>
    /// <returns>Cell value</returns>
    public object this[string sKey]
    {
      get
      {
        return Properties[sKey];
      }
      set
      {
        Properties[sKey] = value;
      }
    }

    #endregion

    #region Private Methods

    #endregion
  }
}
