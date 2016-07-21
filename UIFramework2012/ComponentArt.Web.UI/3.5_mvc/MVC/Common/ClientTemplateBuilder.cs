namespace ComponentArt.Web.UI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;

  public class ClientTemplateBuilder
  {
    private readonly ClientTemplate clientTemplate;

    public ClientTemplateBuilder(ClientTemplate clientTemplate)
    {
      this.clientTemplate = clientTemplate;
    }

    public ClientTemplateBuilder Content(string value)
    {
      clientTemplate.Text = value;
      return this;
    }

    public ClientTemplateBuilder ID(string value)
    {
      clientTemplate.ID = value;
      return this;
    }
  }
}