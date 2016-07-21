using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;



namespace ComponentArt.Silverlight.Demos.Web
{
  [DataContract]
  public class Message
  {
    [DataMember]
    public string Subject;

    [DataMember]
    public DateTime LastPostDate;

    [DataMember]
    public int Replies;

    [DataMember]
    public int TotalViews;

    [DataMember]
    public string StartedBy;

    [DataMember]
    public string EmailIcon;

    [DataMember]
    public string LargeEmailIcon;

    [DataMember]
    public string FlagIcon;

    [DataMember]
    public string PriorityIcon;

    [DataMember]
    public string AttachmentIcon;
  }

  public interface IComponentArtMessageService
  {
    List<Message> GetRecords(int pageSize, int pageIndex, string sort);
  }
}
