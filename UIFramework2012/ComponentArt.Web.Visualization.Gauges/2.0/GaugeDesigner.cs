using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using ComponentArt.Web.Visualization.Gauges;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges
{
    internal class GaugeDesigner : ControlDesigner
    {
        private static bool firstLoad = true;

        private Gauge m_webGauge = null;
        private string m_filenameToBeRemoved = null;
        private const string m_filenameTemplate = "TempComponentArtGauge";

        private DesignerVerbCollection m_designerVerbs;
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (m_designerVerbs == null)
                {
                    m_designerVerbs = new DesignerVerbCollection();
                    m_designerVerbs.Add(new DesignerVerb("Wizard...", new EventHandler(this.OnWizard)));
                    m_designerVerbs.Add(new DesignerVerb("Load Template...", new EventHandler(this.OnLoadTemplate)));
					m_designerVerbs.Add(new DesignerVerb("Save Template...", new EventHandler(this.OnSaveTemplate)));
					m_designerVerbs.Add(new DesignerVerb("About...", new EventHandler(OnAbout)));
				}
                return m_designerVerbs;
            }
        }

        public override bool AllowResize
        {
            get { return true; }
        }
        
		private void OnSaveTemplate(object sender, EventArgs e)
		{
			Gauge gauge = Component as Gauge;
			if (gauge == null)
				throw new ArgumentException("Component must be of type WebGauge", "component");

			SaveFileDialog dlg = new SaveFileDialog();
			dlg.DefaultExt = "xml";
			dlg.Title = "Save Template";
			dlg.InitialDirectory = Application.ExecutablePath;
			dlg.FileName = "";

			if(dlg.ShowDialog() == DialogResult.OK)
			{
				gauge.XMLSerialize(dlg.FileName);
			}
		}

		private void OnLoadTemplate(object sender, EventArgs e)
		{
			Gauge gauge = Component as Gauge;
			if (gauge == null)
				throw new ArgumentException("Component must be of type WebGauge", "component");

			RaiseComponentChanging(null);

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.DefaultExt = "xml";
			dlg.Title = "Load Template";
			dlg.InitialDirectory = Application.ExecutablePath;
			dlg.FileName = "";

			if(dlg.ShowDialog() == DialogResult.OK)
			{
				gauge.XMLDeserialize(dlg.FileName);
			}

			RaiseComponentChanged(null, null, null);
			ControlChanged();
		}

        private void OnWizard(object sender, EventArgs e)
        {
			Gauge gauge = Component as Gauge;
			if (gauge == null)
				throw new ArgumentException("Component must be of type WebGauge", "component");

			RaiseComponentChanging(null);

			GaugeWizardForm form = new GaugeWizardForm(gauge);
			form.ShowDialog(null);

			RaiseComponentChanged(null, null, null);

			ControlChanged();
        }

		protected void OnAbout(object sender, EventArgs e) 
		{

			Gauge gauge = this.Component as Gauge;

			Type controlType = gauge.GetType();

			string[] str =  controlType.AssemblyQualifiedName.Split(',');

			string text = controlType.FullName + ": " +
				"\n" + str[2].Trim() + 
				"\n" + str[3].Trim() +
				"\n" + str[4].Trim() ;

			AboutDialog dlg = new AboutDialog(text);
			dlg.ShowDialog();
			dlg.Dispose();
		}  
      
        public override void Initialize(IComponent component)
        {
            if(!(component is Gauge))
            {
                throw new ArgumentException("Component must be a WebGauge control.", "component");
            }

            m_webGauge = (Gauge)component;

            base.Initialize(component);
            m_webGauge.Designer = this;
        }

        internal void ControlChanged()
        {
            UpdateDesignTimeHtml();
#if FW2 || FW3 || FW35
            this.Tag.SetDirty(true);
#else
            this.IsDirty = true;
#endif
        }
        
//		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
//		{
//			  base.InitializeNewComponent(defaultValues);
//		}

        private string m_imagePath = null;
        string ImagePath
        {
            get
            {
                if (m_imagePath == null)
                {
                    Guid guid = Guid.NewGuid();
                    string ext = (firstLoad ? "bmp" : "png");
                    string tempFile = m_filenameTemplate + "-" + guid.ToString() + "." + ext;
                    m_imagePath = Path.Combine(Path.GetTempPath(), tempFile);
                }
                return m_imagePath;
            }
        }

#if FW2 || FW3 || FW35
        public override string GetPersistenceContent()
#else
        public override string GetPersistInnerHtml()
#endif
        {
            if (m_webGauge != null)
                m_webGauge.InSerialization = true;

            string s = null;
            try
            {
#if FW2 || FW3 || FW35
                s = base.GetPersistenceContent();
#else
                s = base.GetPersistInnerHtml();
#endif
            }
            finally
            {
                if (m_webGauge != null)
                    m_webGauge.InSerialization = false;
            }
            return s;
        }

        public override string GetDesignTimeHtml()
        {
            if ((int)m_webGauge.Width.Value == 0 || (int)m_webGauge.Height.Value == 0)
            {
                return "";
            }

#if FW2 || FW3 || FW35
            if (Tag != null)
                Tag.SetDirty(true);
#else
            IsDirty = true;
#endif            

            RemoveOldImage(false);

            string imagePath = ImagePath;            
            m_webGauge.DesignModeFileName = imagePath;            
            firstLoad = false;
            
            Color bgColor = m_webGauge.BackColor;

            if (Path.GetExtension(imagePath) == ".bmp" && bgColor == Color.Transparent)
                m_webGauge.BackColor = Color.White;

            string text = base.GetDesignTimeHtml();
            
            m_webGauge.BackColor = bgColor;
            
            return text;
        }


        void RemoveOldImage(bool dispose)
        {
            if (m_filenameToBeRemoved != null && System.IO.File.Exists(m_filenameToBeRemoved))
            {
                System.IO.File.Delete(m_filenameToBeRemoved);
                m_filenameToBeRemoved = null;
            }
            if (m_imagePath != null && System.IO.File.Exists(m_imagePath))
            {
                if (!dispose)
                    m_filenameToBeRemoved = m_imagePath;
                else
                    System.IO.File.Delete(m_imagePath);
            }
            m_imagePath = null;
        }

        protected override void Dispose(bool disposing)
        {
            RemoveOldImage(disposing);
			m_webGauge.DisposeFactory();

            base.Dispose(disposing);
        }
    }
}
