using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Mvc.Demos.Models
{
  public class DemoConfig
  {
    public bool Override { get; set; }
    public bool QuirksMode { get; set; }
    public bool Full { get; set; }
    public bool SelectedNode { get; set; }
    public string DemoType { get; set; }
    public string DemoTitle { get; set; }
    public bool ShowCode { get; set; }
  }
}
