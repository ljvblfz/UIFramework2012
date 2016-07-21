using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
    internal class DialogDesigner : TemplatedControlDesigner 
	{
        private TemplateEditingVerb[] _templateEditingVerbs;

		public override string GetDesignTimeHtml()
		{
			try
			{
				Dialog myDialog = ((Dialog)Component);

                if (myDialog.Content == null && myDialog.Header == null && myDialog.Footer == null)
                {
                    return GetEmptyDesignTimeHtml();
                }

				StringBuilder oSB = new StringBuilder();
				StringWriter oStringWriter = new StringWriter(oSB);
				HtmlTextWriter output = new HtmlTextWriter(oStringWriter);                

                // start design render
                output.Write("<div style='background-color:#EEEEEE;border:1px;border-color:black;border-top-color:gray;border-left-color:gray;border-style:solid;position:absolute;height:" + myDialog.Height.ToString() + ";width:" + myDialog.Width.ToString() + ";'>");
                output.Write("<div style='background-color:#3F3F3F;padding:5px;font-family:verdana;font-size:12px;color:white;cursor:pointer;cursor:hand;'>");

                if (myDialog.Header != null)
                    myDialog.Header.RenderControl(output);

                output.Write("</div>");
                output.Write("<div style='padding:5px;font-family:verdana;font-size:12px;color:black;background-color:#EEEEEE;'>");

                if (myDialog.Content != null)
                    myDialog.Content.RenderControl(output);

                output.Write("</div>");
                output.Write("<div style='padding:5px;font-family:verdana;font-size:12px;color:black;background-color:#EEEEEE;'>");

                if (myDialog.Footer != null)
                    myDialog.Footer.RenderControl(output);

                output.Write("</div></div>");
                // end design render
        
				output.Flush();
				oStringWriter.Flush();
                 
				return oSB.ToString();
			}
			catch(Exception ex)
			{
				return CreatePlaceHolderDesignTimeHtml("Error generating design-time HTML:\n\n" + ex.ToString());
			}
		}

        public override string GetPersistInnerHtml()
        {
            // don't touch inner stuff
            return null;
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return CreatePlaceHolderDesignTimeHtml("Design-time not available with currently set properties.");
        }

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
	}
}
