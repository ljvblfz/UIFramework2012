using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  public class ClientEventBuilder
  {
    private readonly ClientEvent clientEvent;

    public ClientEventBuilder(ClientEvent clientEvent)
    {
      this.clientEvent = clientEvent;
    }
  }
}