using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;


namespace ComponentArt.Web.UI
{
	internal class SnapDesigner : TemplatedControlDesigner 
	{
		private TemplateEditingVerb[] _templateEditingVerbs;
	
		#region Design-time HTML
		public override string GetDesignTimeHtml() 
		{
			Snap control = (Snap)Component;

			if (control.HeaderTemplate == null && 
				control.ContentTemplate == null &&
				control.FooterTemplate == null &&
        control.Header == null &&
        control.Content == null &&
        control.Footer == null
        )
			{
				return GetEmptyDesignTimeHtml();
			}

			string designTimeHtml = String.Empty;
			try 
			{
				ControlCollection controls = control.Controls;
				return base.GetDesignTimeHtml();
			}
			catch (Exception e) 
			{
				designTimeHtml = GetErrorDesignTimeHtml(e);
			}

			return designTimeHtml;
		}

		protected override string GetEmptyDesignTimeHtml() 
		{
			return CreatePlaceHolderDesignTimeHtml("Right-click to edit the ContentTemplate property.");
		}

    public override string GetPersistInnerHtml()
    {
      // don't touch inner stuff
      return null;
    }

		#endregion Design-time HTML

		#region Template-editing Functionality

		protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs() 
		{
			if (_templateEditingVerbs == null) 
			{
				_templateEditingVerbs = new TemplateEditingVerb[1];
				_templateEditingVerbs[0] = new TemplateEditingVerb("Content Template", 0, this); 
			}
			return _templateEditingVerbs;
		}

		protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb) 
		{
			ITemplateEditingFrame frame = null;

			if ((_templateEditingVerbs != null) && (_templateEditingVerbs[0] == verb)) 
			{
				ITemplateEditingService teService = (ITemplateEditingService)GetService(typeof(ITemplateEditingService));

				if (teService != null) 
				{
					Style style = ((Snap)Component).ControlStyle;
					frame = teService.CreateFrame(this, verb.Text, new string[] { "ContentTemplate" }, style, null);
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

		public override string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing) 
		{
			string content = String.Empty;

			allowEditing = true;

			if ((_templateEditingVerbs != null) && (_templateEditingVerbs[0] == editingFrame.Verb)) 
			{
				ITemplate currentTemplate = ((Snap)Component).ContentTemplate;

				if (currentTemplate != null) 
				{
					content = GetTextFromTemplate(currentTemplate);
				}
			}

			return content;
		}

		public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent) 
		{
			if ((_templateEditingVerbs != null) && (_templateEditingVerbs[0] == editingFrame.Verb)) 
			{
				Snap control = (Snap)Component;
				ITemplate newTemplate = null;

				if ((templateContent != null) && (templateContent.Length != 0)) 
				{
					newTemplate = GetTemplateFromText(templateContent);
				}

				control.ContentTemplate = newTemplate;
			}
		}
		#endregion Template-editing Functionality

		public override bool AllowResize 
		{
			get 
			{
				Snap control = (Snap)Component;
				bool templateExists =
					(control.ContentTemplate != null || control.HeaderTemplate != null || control.FooterTemplate != null);
				
				// When templates are not defined, render a read-only fixed-
				// size block. Once templates are defined or are being edited, the control should allow
				// resizing.
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
			if (!(component is Snap)) 
			{
				throw new ArgumentException("Component must be a Snap control.", "component");
			}
			base.Initialize(component);
		}
	}
}
