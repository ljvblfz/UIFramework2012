using System;
using System.ComponentModel;
using System.Collections; 
using System.Collections.Specialized; 
using System.Reflection; 
using System.Resources;
using System.Text;
using System.IO; 
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{

  #region Custom enumerations

  /// <summary>
  /// Specifies whether the <see cref="Ticker"/> control animates the text on the page or in the status bar.
  /// </summary>
  public enum TickerType
  {
    /// <summary>The ticker works on text within the page.</summary>
    Default,
    
    /// <summary>The ticker works on text within the status bar.</summary>
    StatusBar
  }

  #endregion

  #region Ticker control 

  /// <summary>
  /// Provides typewriter-like animation of text.
  /// </summary>
  [ToolboxItem(false)]
  [DefaultProperty("Lines")]
  [ParseChildren(true, "Lines")]
  [ToolboxData("<{0}:Ticker runat=server></{0}:Ticker>")]
	public class Ticker : ComponentArt.Web.UI.WebControl
	{
    
    #region Ticker Control interface 

    /// <summary>
    /// AutoStart property. 
    /// </summary>
    [
      Category("Ticker"), 
      Description("Whether to automatically start the ticker when the page loads.")
    ] 
    public bool AutoStart 
    {
      get 
      {
        object o = ViewState["AutoStart"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["AutoStart"] = value;
      }
    }

    /// <summary>
    /// CharDelay property. 
    /// </summary>
    [
      Category("Ticker"), 
      Description("Delay between ticker characters in milliseconds.")
    ] 
    public int CharDelay 
    {
      get 
      {
        object o = ViewState["CharDelay"]; 
        return (o == null) ? 50 : (int) o; 
      }
      set 
      {
        ViewState["CharDelay"] = value;
      }
    }

    /// <summary>
    /// LineDelay property. 
    /// </summary>
    [
      Category("Ticker"), 
      Description("Delay between ticker messages in milliseconds.")
    ] 
    public int LineDelay 
    {
      get 
      {
        object o = ViewState["LineDelay"]; 
        return (o == null) ? 1500 : (int) o; 
      }
      set 
      {
        ViewState["LineDelay"] = value;
      }
    }

    /// <summary>
    /// Loop property. 
    /// </summary>
    [
      Category("Ticker"), 
      Description("Whether to loop back to the beginning of the Lines collection when done the last line.")
    ] 
    public bool Loop 
    {
      get 
      {
        object o = ViewState["Loop"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["Loop"] = value;
      }
    }

    /// <summary>
    /// NextTickerDelay property. 
    /// </summary>
    [
    Category("Ticker"), 
    Description("The delay before the next ticker will be triggered, if this instance is used within a ticker sequence.  ")
    ] 
    public int NextTickerDelay 
    {
      get 
      {
        object o = ViewState["NextTickerDelay"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["NextTickerDelay"] = value;
      }
    }

    /// <summary>
    /// Text property. 
    /// </summary>
    [
      Bindable(true), 
      Category("Appearance"), 
      Description("A simple way to specify the ticker text without using the Lines collection, if only one line is needed. ")
    ] 
    public string Text 
    {
      get 
      {
        object o = ViewState["Text"]; 
        return (o == null) ? "ComponentArt Text Ticker" : (string) o; 
      }
      set 
      {
        ViewState["Text"] = value;
      }
    }

    /// <summary>
    /// TickerType property. 
    /// </summary>
    [
      Category("Ticker"), 
      Description("Use to specify default, status bar, or title bar tickers.")
    ] 
    public TickerType TickerType 
    {
      get 
      {
        object o = ViewState["TickerType"]; 
        return (o == null) ? new TickerType() : (TickerType) o; 
      }
      set 
      {
        ViewState["TickerType"] = value;
      }
    }

    private TickerLines _lines = new TickerLines();
    /// <summary>
    /// Lines collection.
    /// </summary>
    [MergableProperty(false)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
    [Category("Ticker")]
    [Description("Ticker content.")]
    public TickerLines Lines 
    {
      get 
      { 
        return _lines; 
      }
    }

    // Client-side events ---------------------------------------------------------------
    /// <summary>
    /// Text property. 
    /// </summary>
    [
    Category("Client-Side Events"), 
    Description("Client-side function to be triggered when the ticker finishes ticking. ")
    ] 
    public string OnEnd 
    {
      get 
      {
        object o = ViewState["OnEnd"]; 
        return (o == null) ? String.Empty : (string) o; 
      }
      set 
      {
        ViewState["OnEnd"] = value;
      }
    }



    #endregion 


    #region Ticker Control Implementation 

    protected override void ComponentArtRender(HtmlTextWriter output)
		{
      if (this.IsRunningInDesignMode() || IsDownLevel())
      {
        RenderBeginTag(output); 
        if (Lines.Count > 0)
          output.Write(Lines[0].Text); 
        else
          output.Write(Text); 
        RenderEndTag(output); 
      }
      else
      {
        if(!Page.IsClientScriptBlockRegistered("A573G788.js"))
        {
          Page.RegisterClientScriptBlock("A573G788.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Rotator.client_scripts", "A573G788.js");
        }

        RenderBeginTag(output); 
        RenderEndTag(output); 
        RenderInstanceClientScript(output); 
      }
		}

    // Renders instance-specific client script, one for each instance of the control 
    private void RenderInstanceClientScript(HtmlTextWriter output)
    {
      StringBuilder instanceScript = new StringBuilder();
      string tickerClientVarName = "tco_" + this.ClientID;
      instanceScript.Append("if(!(window." + tickerClientVarName + "))\n");
      instanceScript.Append("{\n");
      instanceScript.Append("window." + tickerClientVarName + " = new ComponentArt_Ticker();\n");
      instanceScript.Append(tickerClientVarName + ".GlobalID = '" + tickerClientVarName + "';\n");
      instanceScript.Append(tickerClientVarName + ".ElementID = '" + this.ClientID + "';\n");
      instanceScript.Append(tickerClientVarName + ".CharDelay = " + this.CharDelay.ToString() + ";\n");
      instanceScript.Append(tickerClientVarName + ".LineDelay = " + this.LineDelay.ToString() + ";\n");
      instanceScript.Append(tickerClientVarName + ".Loop = " + this.Loop.ToString().ToLower() + ";\n");
      if (this.Lines.Count > 0)
      {
        string[] lines = new string[this.Lines.Count];
        for (int i = 0; i < lines.Length; i++)
        {
          lines[i] = "'" + this.fixTickerLine(this.Lines[i].Text) + "'";
        }
        instanceScript.Append(tickerClientVarName + ".Lines = [" + String.Join(",", lines) + "];\n");
      }
      else
      {
        instanceScript.Append(tickerClientVarName + ".Lines[0] = '" + this.fixTickerLine(this.Text) + "';\n");
      }
      instanceScript.Append(tickerClientVarName + ".NextTickerDelay = " + this.NextTickerDelay.ToString() + ";\n");
      instanceScript.Append(tickerClientVarName + ".TickerType = '" + this.TickerType.ToString().ToLower() + "';\n");
      instanceScript.Append(tickerClientVarName + ".OnEnd = function () {" + (this.OnEnd == String.Empty ? "rcr_doNothing()" : this.OnEnd ) + ";};\n");
      if (this.AutoStart)
      {
        instanceScript.Append("rcr_StartTicker(" + tickerClientVarName + ");\n");
      }
      instanceScript.Append("}\n");
      output.Write(this.DemarcateClientScript(instanceScript.ToString(), "Ticker instance initialization"));
    }

    // Fixes the ticker line string 
    private string fixTickerLine(string line)
    {
      string result = line.Trim(); 
      result = result.Replace("'", "\\'"); 
      result = result.Replace("  ", " "); 
      result = result.Replace("&amp;", "&"); 
      return result; 
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }
      return (!isUpLevelBrowser());
    }

    // Whether the request was made by a supported browser 
    internal bool isUpLevelBrowser()
    {
      if (this.ClientTarget == ClientTargetLevel.Downlevel) return false;
      if (this.ClientTarget == ClientTargetLevel.Uplevel) return true;
      if (Context.Request.UserAgent == null) return false; 
      if (Context.Request.UserAgent.IndexOf("Safari") >= 0) return false; 

      if (Context.Request.Browser.Browser == "IE" & 
        Context.Request.Browser.MajorVersion >= 5 & 
        !(Context.Request.Browser.Platform.ToLower().StartsWith("mac")))
        return true; 
      else if (Context.Request.UserAgent.IndexOf("Gecko") >= 0)
        return true; 
      else if (Context.Request.Browser.Browser == "Opera" & Context.Request.Browser.MajorVersion >= 7)
        return true; 
      else
        return false; 
    }

    #endregion 
	}
  #endregion 

  #region TickerLines collection 

  /// <summary>
  /// A list of <see cref="TickerLine"/> objects.
  /// </summary>
  public class TickerLines : ICollection, IEnumerable, IList
  {
    private ArrayList _tickerLines;

    public TickerLines()
    {
      _tickerLines = new ArrayList();
    }

    object IList.this[int index] 
    {
      get 
      {
        return _tickerLines[index];
      }
      set 
      {
        _tickerLines[index] = (TickerLine)value;
      }
    }


    public int Count
    {
      get
      {
        return _tickerLines.Count;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return true;
      }
    }

    public object SyncRoot
    {
      get
      {
        return _tickerLines.SyncRoot;
      }
    }

    public void CopyTo (Array ar, int index)
    {
    }

    public virtual TickerLine this[int index]
    {
      get
      {
        return (TickerLine) _tickerLines[index];
      }
      set
      {
        _tickerLines[index] = value;
      }
    }

    public void Remove (object item)
    {
      _tickerLines.Remove (item);
    }
		
    public void Insert (int index, object item)
    {
      _tickerLines[index] = item;
    }

    public int Add (object item)
    {
      return _tickerLines.Add(item);
    }

    public void Clear()
    {
      _tickerLines.Clear();
    }

    public bool Contains(object item)
    {
      return _tickerLines.Contains(item);
    }

    public int IndexOf (object item)
    {
      return _tickerLines.IndexOf(item);
    }

    public bool IsFixedSize
    {
      get
      {
        return false;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public void RemoveAt(int index)
    {
      _tickerLines.RemoveAt(index);
    }


    public virtual IEnumerator GetEnumerator()
    {
      return _tickerLines.GetEnumerator();
    }
  }


  /// <summary>
  /// A line of text animated by the <see cref="Ticker"/> control.
  /// </summary>
  [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
  public class TickerLine : IParserAccessor
  {
    public TickerLine() {}

    [DefaultValue ("")]
    private string _text;
		
    /// <summary>
    /// The text to animate.
    /// </summary>
    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        _text = value;
      }
    }

    void IParserAccessor.AddParsedSubObject(object obj) 
    {
      if (obj is LiteralControl) 
      {
        Text = ((LiteralControl)obj).Text;
      }
      else 
      {
				
      }
    }
  
  }

  #endregion 
}
