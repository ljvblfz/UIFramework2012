/* 
 * To prepare for royalty-free distribution of ComponentArt.Web.UI with your ASP.NET application, 
 * please follow these simple steps: 
 * 
 * 1. Modify the strAppKey constant below to include the name of your application (instead of "XYZ"); 
 * 2. Compile this ComponentArt.Web.UI project; 
 * 3. Add the new ComponentArt.Web.UI.dll assembly to the project references of your ASP.NET application; 
 * 4. Add the following line of code to your Application_Start event (Global.asax): 
 *    C#: 
 *    Application["ComponentArtWebUI_AppKey"] = "This edition of ComponentArt Web.UI is licensed for XYZ application only."; 
 * 
 *    VB.NET: 
 *    Application("ComponentArtWebUI_AppKey") = "This edition of ComponentArt Web.UI is licensed for XYZ application only."
 * 
 *  Again, you will have to use the actual name of your application instead of "XYZ". 
 *  The result is that only your application will be able to instantiate controls from this version of ComponentArt Web.UI. 
 *  If you have any questions, please contact us at support@ComponentArt.com. 
 */

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ComponentArt.Licensing.Providers
{
  #region RedistributableLicenseProvider
	public class RedistributableLicenseProvider : System.ComponentModel.LicenseProvider
	{
    const string strAppKey = "This edition of ComponentArt Web.UI is licensed for XYZ application only.";    
    
    public override System.ComponentModel.License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions) 
    {
      if (context.UsageMode == LicenseUsageMode.Designtime) 
      {
        // We are not going to worry about design time
        // Issue a license
        return new ComponentArt.Licensing.Providers.RedistributableLicense(this, "The App");
      }
      else
      {
        string strFoundAppKey;

        // During runtime, we only want this control to run in the application 
        // that it was packaged with.

        HttpContext ctx = HttpContext.Current;
        strFoundAppKey = (string)ctx.Application["ComponentArtWebUI_AppKey"];

        if(strAppKey == strFoundAppKey)
          return new ComponentArt.Licensing.Providers.RedistributableLicense(this, "The App");
        else
          return null;
      }
    }
  }
  #endregion

  #region RedistributableLicense Class 

  public class RedistributableLicense : System.ComponentModel.License 
  {
    private ComponentArt.Licensing.Providers.RedistributableLicenseProvider owner;
    private string key;

    public RedistributableLicense(ComponentArt.Licensing.Providers.RedistributableLicenseProvider owner, string key) 
    {
      this.owner = owner;
      this.key = key;
    }
    public override string LicenseKey 
    { 
      get 
      {
        return key;
      }
    }

    public override void Dispose() 
    {
    }
  }

  #endregion 
}
