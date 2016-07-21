using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ComponentArt.Silverlight.Demos.Web
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SilverlightUpload1.Uploaded += new ComponentArt.Silverlight.Server.SilverlightUpload.UploadedEventHandler(Upload1_OnUploaded);
        }
        private void Upload1_OnUploaded(object sender, ComponentArt.Silverlight.Server.SilverlightUpload.UploadUploadedEventArgs args)
        {
            foreach (ComponentArt.Silverlight.Server.UploadedFileInfo oInfo in args.UploadedFiles)
            {
                // this will allow uploads of 11MB
                if (oInfo.Size < 11000 * 1024 && Request.Url.Host.ToString().IndexOf("componentart.com") < 0 )
                {
                    oInfo.SaveAs(Path.Combine(Server.MapPath(SilverlightUpload1.DestinationFolder), oInfo.FileName), SilverlightUpload1.OverwriteExistingFiles);
                }
                else
                {
                    oInfo.RemoveTempFile();
                }
            }
        }
    }
}
