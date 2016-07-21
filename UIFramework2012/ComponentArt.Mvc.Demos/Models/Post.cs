using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Mvc.Demos.Models
{
  public class Post
  {
    public int PostId
    {
      get;
      set;
    }

    public string Subject
    {
      get;
      set;
    }

    public string StartedBy
    {
      get;
      set;
    }
    public string LastPostBy
    {
      get;
      set;
    }
    public DateTime LastPostDate
    {
      get;
      set;
    }
    public int Replies
    {
      get;
      set;
    }
    public int TotalViews
    {
      get;
      set;
    }
    public string Icon
    {
      get;
      set;
    }
    public string PriorityIcon
    {
      get;
      set;
    }
    public string EmailIcon
    {
      get;
      set;
    }
    public string AttachmentIcon
    {
      get;
      set;
    }
    public string FlagIcon
    {
      get;
      set;
    }
  }
}
