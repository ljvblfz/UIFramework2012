using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using ComponentArt.SOA.UI;

namespace ComponentArt.Silverlight.Demos.Web
{
    // dummy SOA.UI web service
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaTreeViewEdit : SoaTreeViewService
    {
        public override SoaTreeViewAddResponse AddNode(SoaTreeViewAddRequest request)
        {
            sleep();
            return new SoaTreeViewAddResponse() { Cancel = ((bool)request.Tag == true) };
        }

        public override SoaTreeViewDeleteResponse DeleteNode(SoaTreeViewDeleteRequest request)
        {
            sleep();
            return new SoaTreeViewDeleteResponse() { Cancel = ((bool)request.Tag == true) };
        }

        public override SoaTreeViewEditResponse EditNode(SoaTreeViewEditRequest request)
        {
            sleep();
            return new SoaTreeViewEditResponse() { Cancel = ((bool)request.Tag == true) };
        }

        public override SoaTreeViewMoveResponse MoveNode(SoaTreeViewMoveRequest request)
        {
            sleep();
            return new SoaTreeViewMoveResponse() { Cancel = ((bool)request.Tag == true) };
        }

        // slow down service response to showcase busy indicators
        private void sleep()
        {
            System.Threading.Thread.Sleep(500);
        }
    }
}
