using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Mvc.Demos.Models
{
  public class FeedItem
  {
    public string FeedName { get; set; }
    public string Title { get; set; }
    public DateTime DatePosted {get;set;}
    public int Visits { get; set; }
    public int Rating { get; set; }
    public string ItemURL { get; set; }
  }
}
