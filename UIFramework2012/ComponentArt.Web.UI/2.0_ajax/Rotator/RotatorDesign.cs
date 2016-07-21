using System; 
using System.Collections; 
using System.ComponentModel; 
using System.ComponentModel.Design; 
using System.Diagnostics; 
using System.Web.UI; 
using System.Web.UI.Design; 
using System.Web.UI.WebControls; 
using ComponentArt.Web.UI; 

namespace ComponentArt.Web.UI.Design
{
  internal class RotatorDesigner : TemplatedControlDesigner
  {
    private TemplateEditingVerb[] _templateEditingVerbs; 

    #region Design-time HTML

    public override string GetDesignTimeHtml()
    {
      Rotator control = (Rotator)Component; 

      if (control.SlideTemplate == null)
      {
        return GetEmptyDesignTimeHtml(); 
      }

      string designTimeHtml = String.Empty; 
      try
      {
        control.DataSource = GetDummyDataSource(); 
        control.DataBind(); 
        designTimeHtml = base.GetDesignTimeHtml(); 
      }
      catch (Exception e)
      {
        designTimeHtml = GetErrorDesignTimeHtml(e); 
      }

      return designTimeHtml; 
    }

    private IEnumerable GetDummyDataSource()
    {
      string[] dummy = new string[1]; 
      dummy[0] = "dummy"; 
      return dummy; 
    }

    protected override string GetEmptyDesignTimeHtml()
    {
      string msg = "Right-click and select <b>Edit Template / Slide Template</b><br>"; 
      msg += "in order to define your slide template. When finished,<br>"; 
      msg += "select <b>End Template Editing</b>."; 
      return CreatePlaceHolderDesignTimeHtml(msg); 
    }

    protected override string GetErrorDesignTimeHtml(Exception e)
    {
      string msg = "<font color=red><b>Error: </b>"; 
      msg += e.Message + "</font>"; 
      
      return CreatePlaceHolderDesignTimeHtml(msg);  
    }

    #endregion Design-time HTML


    #region Template-editing Functionality 

    [System.Obsolete]
    protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs()
    {
      if (_templateEditingVerbs == null)
      {
        _templateEditingVerbs = new TemplateEditingVerb[1]; 
        _templateEditingVerbs[0] = new TemplateEditingVerb("Slide Template", 0, this); 
      }
      return _templateEditingVerbs; 
    }

    [System.Obsolete]
    protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
    {
      ITemplateEditingFrame frame = null; 

      if ((_templateEditingVerbs != null) && 
        (_templateEditingVerbs[0] == verb))
      {
        ITemplateEditingService teService = 
          (ITemplateEditingService)GetService(typeof(ITemplateEditingService)); 

        if (teService != null)
        {
          Style style = ((Rotator)Component).ControlStyle; 
          frame = teService.CreateFrame(this, verb.Text, new string[] {"SlideTemplate"}, style, null); 
        }
      }
      return frame; 
    }

    private void DisposeTemplateEditingVerbs()
    {
      if (_templateEditingVerbs != null)
      {
        _templateEditingVerbs[0].Dispose(); 
        _templateEditingVerbs = null; 
      }
    }

    [System.Obsolete]
    public override string GetTemplateContent(
      ITemplateEditingFrame editingFrame, 
      string templateName, 
      out bool allowEditing)
    {
      string content = String.Empty; 
      allowEditing = true; 

      if ((_templateEditingVerbs != null) && 
        (_templateEditingVerbs[0] == editingFrame.Verb))
      {
        ITemplate currentTemplate = ((Rotator)Component).SlideTemplate; 
        if (currentTemplate != null)
        {
          content = GetTextFromTemplate(currentTemplate); 
        }
      }
      return content; 
    }

    [System.Obsolete]
    public override void SetTemplateContent(
      ITemplateEditingFrame editingFrame, 
      string templateName, 
      string templateContent)
    {
      if ((_templateEditingVerbs != null) && 
        (_templateEditingVerbs[0] == editingFrame.Verb))
      {
        Rotator control = (Rotator)Component; 
        ITemplate newTemplate = null; 

        if ((templateContent != null) && (templateContent.Length != 0))
        {
          newTemplate = GetTemplateFromText(templateContent); 
        }
        control.SlideTemplate = newTemplate; 
      }
    }

    #endregion Template-editing Functionality 


    #region Other design-time logic 

    public override bool AllowResize
    {
      get
      {
        bool templateExists = ((Rotator)Component).SlideTemplate != null; 
        return templateExists || InTemplateMode; 
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        DisposeTemplateEditingVerbs(); 
      }
      base.Dispose(disposing); 
    }

    public override void Initialize(IComponent component)
    {
      if (!(component is Rotator))
      {
        throw new ArgumentException("Component must be a Rotator control.", "component"); 
      }
      base.Initialize(component); 
    }

    public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
    {
      base.OnComponentChanged(sender, ce); 
      if (ce.Member != null)
      {
        string name = ce.Member.Name; 

        if (name.Equals("Font") || 
          name.Equals("ForeColor") || 
          name.Equals("BackColor"))
        {
          DisposeTemplateEditingVerbs(); 
        }
      }
    }
    #endregion 

  }
}