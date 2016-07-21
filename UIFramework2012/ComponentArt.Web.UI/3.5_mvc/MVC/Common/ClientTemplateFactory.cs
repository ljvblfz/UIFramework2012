using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  public class ClientTemplateFactory
  {
    private ComponentArt.Web.UI.ClientTemplateCollection clientTemplates;

    public ClientTemplateFactory(ComponentArt.Web.UI.ClientTemplateCollection clientTemplates)
    {
      this.clientTemplates = clientTemplates;
    }

    public virtual ClientTemplateBuilder Add()
    {
      ClientTemplate clientTemplate = new ClientTemplate();

      clientTemplates.Add(clientTemplate);

      return new ClientTemplateBuilder(clientTemplate);
    }
  }
}