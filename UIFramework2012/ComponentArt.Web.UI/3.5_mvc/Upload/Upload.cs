using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using ComponentArt.Licensing.Providers;



namespace ComponentArt.Web.UI
{
  #region Auxiliary Classes

  /// <summary>
  /// Collection of <see cref="UploadedFileInfo"/> objects.
  /// </summary>
  public class UploadedFileInfoCollection : System.Collections.CollectionBase
  {
    public int Add(UploadedFileInfo obj) { return List.Add(obj); }
    public void Insert(int index, UploadedFileInfo obj) { List.Insert(index, obj); }
    public void Remove(UploadedFileInfo obj) { List.Remove(obj); }
    public bool Contains(UploadedFileInfo obj) { return List.Contains(obj); }
    public void CopyTo(UploadedFileInfo[] array, int index) { List.CopyTo(array, index); }

    /// <summary>
    /// Returns the index of the given UploadedFileInfo within the collection.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int IndexOf(object obj)
    {
      if (obj is int)
        return (int)obj;

      if (obj == null)
      {
        return -1;
      }

      if (obj is UploadedFileInfo)
      {
        for (int i = 0; i < List.Count; i++)
          if (Object.Equals(List[i], obj))
            return i;

        return -1;
      }
      else
      {
        throw new ArgumentException("Only a UploadedFileInfo or an integer is permitted for the indexer.");
      }
    }

    public UploadedFileInfo this[object obj]
    {
      get
      {
        int iIndex = IndexOf(obj);

        if (iIndex >= 0)
          return (UploadedFileInfo)List[iIndex];
        else
          return null;
      }
      set
      {
        int iIndex = IndexOf(obj);

        if (iIndex >= 0)
          List[iIndex] = value;
        else
          this.Add(value);
      }
    }

    /// <summary>
    /// Remove the given UploadedFileInfo from the collection.
    /// </summary>
    /// <param name="obj">UploadedFileInfo to remove</param>
    public void Remove(object obj)
    {
      if (obj is UploadedFileInfo)
      {
        for (int i = 0; i < List.Count; i++)
        {
          if (Object.Equals(List[i], obj))
          {
            base.RemoveAt(i);
            return;
          }
        }
      }
      else
      {
        throw new ArgumentException("Only an UploadedFileInfo parameter is permitted.");
      }
    }
  }

  /// <summary>
  /// UploadUploadedEventArgs class. Arguments for Upload control's Uploaded event.
  /// </summary>
  public class UploadUploadedEventArgs : EventArgs
  {
    /// <summary>
    /// A collection of UploadedFileInfo objects describing uploaded files.
    /// </summary>
    public UploadedFileInfoCollection UploadedFiles;
  }

  #endregion

  /// <summary>
  /// Provides file-upload functionality.
  /// </summary>
  [GuidAttribute("53f586d7-6911-4cb0-82bd-564a9f882220")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:Upload runat=server></{0}:Upload>")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [Designer(typeof(ComponentArt.Web.UI.UploadDesigner))]
  public sealed class Upload : WebControl
  {
    #region Properties

    /// <summary>
    /// A comma-delimited list of file extensions to allow. If none are specified, all are allowed.
    /// </summary>
    [DefaultValue("")]
    [Category("Data")]
    [Description("A comma-delimited list of file extensions to allow. If none are specified, all are allowed.")]
    public string AllowedFileExtensions
    {
      get
      {
        object o = ViewState["AllowedFileExtensions"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["AllowedFileExtensions"] = value;
      }
    }

    /// <summary>
    /// A comma-delimited list of mime types to allow. If none are specified, all are allowed.
    /// </summary>
    [DefaultValue("")]
    [Category("Data")]
    [Description("A comma-delimited list of mime types to allow. If none are specified, all are allowed.")]
    public string AllowedMimeTypes
    {
      get
      {
        object o = ViewState["AllowedMimeTypes"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["AllowedMimeTypes"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback after an upload is complete, and raise server-side events.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback after an upload is complete, and raise server-side events.")]
    public bool AutoPostBack
    {
      get
      {
        object o = ViewState["AutoPostBack"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBack"] = value;
      }
    }

    /// <summary>
    /// The optional callback parameter passed from the client.
    /// </summary>
    [Description("The optional callback parameter passed from the client.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public string CallbackParameter
    {
      get
      {
        string s = Context.Request.Params[string.Format("Cart_{0}_CallbackParameter", this.GetSaneId())];

        if (s != null)
        {
          ViewState["CallbackParameter"] = s;
        }
        else
        {
          s = (string)ViewState["CallbackParameter"];
        }

        return s == null ? "" : s;
      }
      set
      {
        ViewState["CallbackParameter"] = value;
      }
    }

    /// <summary>
    /// Whether we are currently in a callback request that this control caused. Read-only.
    /// </summary>
    [Description("Whether we are currently in a callback request that this control caused.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool CausedCallback
    {
      get
      {
        if (Context != null && Context.Request != null)
        {
          if (Context.Request.Params[string.Format("Cart_{0}_Callback", this.GetSaneId())] != null)
          {
            return true;
          }
        }

        return false;
      }
    }

    private UploadClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public UploadClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new UploadClientEvents();
        }
        return _clientEvents;
      }
    }

    internal ClientTemplateCollection _clientTemplates = new ClientTemplateCollection();
    /// <summary>
    /// Collection of client-templates that may be used by this control.
    /// </summary>
    [Browsable(false)]
    [Description("Collection of client-templates that may be used by this control.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplateCollection ClientTemplates
    {
      get
      {
        return (ClientTemplateCollection)_clientTemplates;
      }
    }

    /// <summary>
    /// Whether to include verbose debugging information in error messages sent to the client. Default: true.
    /// </summary>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to include verbose debugging information in error messages sent to the client. Default: true.")]
    public bool Debug
    {
      get
      {
        object o = ViewState["Debug"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["Debug"] = value;
      }
    }

    /// <summary>
    /// The folder into which to optionally move files once they are uploaded, under their
    /// original name.
    /// </summary>
    [DefaultValue("")]
    [Category("Data")]
    [Description("The folder into which to optionally move files once they are uploaded, under their original name.")]
    public string DestinationFolder
    {
      get
      {
        object o = ViewState["DestinationFolder"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["DestinationFolder"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the file input area.
    /// </summary>
    [DefaultValue("")]
    [Category("Appearance")]
    [Description("The ID of the client template to use for the file input area.")]
    public string FileInputClientTemplateId
    {
      get
      {
        object o = ViewState["FileInputClientTemplateId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["FileInputClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use for the file selection popup button on hover.
    /// </summary>
    [DefaultValue("")]
    [Category("Appearance")]
    [Description("The URL of the image to use for the file selection popup button on hover.")]
    public string FileInputHoverImageUrl
    {
      get
      {
        object o = ViewState["FileInputHoverImageUrl"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["FileInputHoverImageUrl"] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use for the file selection popup button.
    /// </summary>
    [DefaultValue("")]
    [Category("Appearance")]
    [Description("The URL of the image to use for the file selection popup button.")]
    public string FileInputImageUrl
    {
      get
      {
        object o = ViewState["FileInputImageUrl"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["FileInputImageUrl"] = value;
      }
    }

    /// <summary>
    /// The number of file inputs to render initially. Default: 1.
    /// </summary>
    [DefaultValue(1)]
    [Category("Behavior")]
    [Description("The number of file inputs to render initially. Default: 1.")]
    public int InitialFileCount
    {
      get
      {
        object o = ViewState["InitialFileCount"];
        return o == null ? 1 : (int)o;
      }
      set
      {
        ViewState["InitialFileCount"] = value;
      }
    }

    /// <summary>
    /// The maximum number of file inputs to allow. Default: 1.
    /// </summary>
    [DefaultValue(1)]
    [Category("Data")]
    [Description("The maximum number of file inputs to allow. Default: 1.")]
    public int MaximumFileCount
    {
      get
      {
        object o = ViewState["MaximumFileCount"];
        return o == null ? 1 : (int)o;
      }
      set
      {
        ViewState["MaximumFileCount"] = value;
      }
    }

    /// <summary>
    /// The maximum upload size to allow, in kilobytes. Default: 0 (unlimited).
    /// </summary>
    [DefaultValue(0)]
    [Category("Data")]
    [Description("The maximum upload size to allow, in kilobytes. Default: 0 (unlimited).")]
    public int MaximumUploadSize
    {
      get
      {
        object o = ViewState["MaximumUploadSize"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["MaximumUploadSize"] = value;
      }
    }

    /// <summary>
    /// Whether to overwrite existing files in the destination folder.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to overwrite existing files in the destination folder.")]
    public bool OverwriteExistingFiles
    {
      get
      {
        object o = ViewState["OverwriteExistingFiles"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["OverwriteExistingFiles"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the upload progress status popup.
    /// </summary>
    [DefaultValue("")]
    [Category("Appearance")]
    [Description("The ID of the client template to use for the upload progress status popup.")]
    public string ProgressClientTemplateId
    {
      get
      {
        object o = ViewState["ProgressClientTemplateId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ProgressClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The ID of the DOM element into which to optionally render progress information.
    /// </summary>
    [DefaultValue("")]
    [Category("Behavior")]
    [Description("The ID of the DOM element into which to optionally render progress information.")]
    public string ProgressDomElementId
    {
      get
      {
        object o = ViewState["ProgressDomElementId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ProgressDomElementId"] = value;
      }
    }

    /// <summary>
    /// The offset in pixels, along the X axis, by which to move the progress popup. Default: 0.
    /// </summary>
    [DefaultValue(0)]
    [Category("Appearance")]
    [Description("The offset in pixels, along the X axis, by which to move the progress popup. Default: 0.")]
    public int ProgressPopupOffsetX
    {
      get
      {
        object o = ViewState["ProgressPopupOffsetX"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["ProgressPopupOffsetX"] = value;
      }
    }

    /// <summary>
    /// The offset in pixels, along the Y axis, by which to move the progress popup. Default: 0.
    /// </summary>
    [DefaultValue(0)]
    [Category("Appearance")]
    [Description("The offset in pixels, along the Y axis, by which to move the progress popup. Default: 0.")]
    public int ProgressPopupOffsetY
    {
      get
      {
        object o = ViewState["ProgressPopupOffsetY"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["ProgressPopupOffsetY"] = value;
      }
    }

    /// <summary>
    /// The interval (in milliseconds) between progress checks during upload.
    /// </summary>
    [DefaultValue(200)]
    [Category("Behavior")]
    [Description("The interval (in milliseconds) between progress checks during upload.")]
    public int ProgressUpdateInterval
    {
      get
      {
        object o = ViewState["ProgressUpdateInterval"];
        return o == null ? 200 : (int)o;
      }
      set
      {
        ViewState["ProgressUpdateInterval"] = value;
      }
    }

    /// <summary>
    /// The local folder in which to place temporary upload files.
    /// </summary>
    [DefaultValue("")]
    [Category("Data")]
    [Description("The local folder in which to place temporary upload files.")]
    public string TempFileFolder
    {
      get
      {
        object o = ViewState["TempFileFolder"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["TempFileFolder"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the upload progress status popup, when the upload completes.
    /// </summary>
    [DefaultValue("")]
    [Category("Appearance")]
    [Description("The ID of the client template to use for the upload progress status popup, when the upload completes.")]
    public string UploadCompleteClientTemplateId
    {
      get
      {
        object o = ViewState["UploadCompleteClientTemplateId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["UploadCompleteClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the upload progress status popup, when the upload aborts due to error.
    /// </summary>
    [DefaultValue("")]
    [Category("Appearance")]
    [Description("The ID of the client template to use for the upload progress status popup, when the upload aborts due to error.")]
    public string UploadErrorClientTemplateId
    {
      get
      {
        object o = ViewState["UploadErrorClientTemplateId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["UploadErrorClientTemplateId"] = value;
      }
    }

    private string UploadId
    {
      get
      {
        object o = ViewState["UploadId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["UploadId"] = value;
      }
    }

    
    #endregion

    #region Methods

    private string CreateUploadCredentials()
    {
      string sUploadId = Guid.NewGuid().ToString();

      // create upload info
      UploadInfo oUploadInfo = new UploadInfo();
      oUploadInfo.TempFolder = this.TempFileFolder;
      oUploadInfo.MaximumBytes = this.MaximumUploadSize * 1024;
      oUploadInfo.MaximumFiles = this.MaximumFileCount;
      oUploadInfo.Debug = this.Debug;

      HttpContext.Current.Application["CartUpload_" + sUploadId] = oUploadInfo;

      return sUploadId;
    }

    /// <summary>
    /// Returns a collection of UploadedFileInfo objects describing files uploaded using this control.
    /// </summary>
    /// <returns>A collection of UploadedFileInfo objects.</returns>
    public UploadedFileInfoCollection GetUploadedFiles()
    {
      UploadedFileInfoCollection arFiles = new UploadedFileInfoCollection();

      if (this.IsDownLevel())
      {
        this.EnsureChildControls();

        foreach (FileUpload oFileUpload in this.Controls)
        {
          if (oFileUpload.PostedFile != null)
          {
            UploadedFileInfo oFileInfo = new UploadedFileInfo();
            oFileInfo.FileName = oFileUpload.PostedFile.FileName;
            oFileInfo.ContentType = oFileUpload.PostedFile.ContentType;
            oFileInfo.Size = oFileUpload.PostedFile.ContentLength;

            arFiles.Add(oFileInfo);
          }
        }
      }
      else
      {
        UploadInfo oUploadInfo = (UploadInfo)HttpContext.Current.Application["CartUpload_" + this.UploadId];

        if (oUploadInfo != null)
        {
          foreach (UploadedFileInfo oFileInfo in oUploadInfo.UploadedFiles)
          {
            arFiles.Add(oFileInfo);
          }
        }
      }

      return arFiles;
    }

    protected override void OnInit(EventArgs args)
    {
    }

    protected override void OnLoad(EventArgs args)
    {
      base.OnLoad(args);

      if (this.IsDownLevel())
      {
        this.EnsureChildControls();

        // do we have an event handler? 
        if (this.Uploaded != null)
        {
          // we have files, prepare event args and fire event
          UploadUploadedEventArgs oArgs = new UploadUploadedEventArgs();

          // populate args
          oArgs.UploadedFiles = this.GetUploadedFiles();

          this.OnUploaded(oArgs);
        }
        else if(this.DestinationFolder != "")
        {
          foreach (FileUpload oFileUpload in this.Controls)
          {
            if (oFileUpload.PostedFile != null)
            {
              oFileUpload.SaveAs(Path.Combine(this.DestinationFolder, oFileUpload.FileName));
            }
          }
        }
      }
      else
      {
        // is an uploadId passed in?
        if (HttpContext.Current != null && HttpContext.Current.Request.Params["CartUploadId"] != null)
        {
          // set it
          this.UploadId = HttpContext.Current.Request.Params["CartUploadId"];
        }

        // make sure we have a unique id
        if (this.UploadId == "" && !Page.IsPostBack)
        {
          this.UploadId = CreateUploadCredentials();
        }
        else
        {
          // have we uploaded anything?
          UploadInfo oUploadInfo = (UploadInfo)HttpContext.Current.Application["CartUpload_" + this.UploadId];
          if (oUploadInfo != null && !oUploadInfo.Handled)
          {
            if (oUploadInfo.UploadedFiles.Count > 0 && oUploadInfo.Progress == 1)
            {
              if (this.DestinationFolder != "")
              {
                foreach (UploadedFileInfo oInfo in oUploadInfo.UploadedFiles)
                {
                  // do we do validation on extensions?
                  if (this.AllowedFileExtensions != "")
                  {
                    ArrayList extensions = new ArrayList(this.AllowedFileExtensions.Split(','));

                    if (!extensions.Contains(oInfo.Extension))
                    {
                      // fail validation
                      continue;
                    }
                  }

                  // do we do validation on mime type?
                  if (this.AllowedMimeTypes != "")
                  {
                    ArrayList types = new ArrayList(this.AllowedMimeTypes.Split(','));

                    if (!types.Contains(oInfo.ContentType))
                    {
                      // fail validation
                      continue;
                    }
                  }

                  oInfo.SaveAs(Path.Combine(this.DestinationFolder, oInfo.FileName), this.OverwriteExistingFiles);
                }
              }

              // have an ID, do we have an event handler? 
              if (this.Uploaded != null)
              {
                // we have files, prepare event args and fire event
                UploadUploadedEventArgs oArgs = new UploadUploadedEventArgs();

                // populate args
                oArgs.UploadedFiles = this.GetUploadedFiles();

                this.OnUploaded(oArgs);
              }

              oUploadInfo.Handled = true;

              // give us new credentials
              this.UploadId = CreateUploadCredentials();
            }
            else
            {
              // upload aborted before being complete, do nothing, but give us new credentials
              this.UploadId = CreateUploadCredentials();
            }
          }
          else // uploadinfo not found, or upload already handled
          {
            // give us new credentials
            this.UploadId = CreateUploadCredentials();
          }
        }
      }

      // Is this a callback? Handle it now
      if (this.CausedCallback)
      {
        this.HandleCallback();
        return;
      }

      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Upload.client_scripts.A573P101.js");
      }
    }

    protected override void CreateChildControls()
    {
      if (this.IsDownLevel())
      {
        for (int i = 0; i < this.InitialFileCount; i++)
        {
          FileUpload oFileUpload = new FileUpload();

          this.Controls.Add(oFileUpload);
        }
      }
    }

    protected override void ComponentArtPreRender(EventArgs oArgs)
    {
      if (this.IsDownLevel())
      {
        // for down-level, prepare the form on the server
        this.Page.Form.Enctype = "multipart/form-data";
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      string sControlId = this.GetSaneId();

      // override writer
      output = new HtmlTextWriter(output, string.Empty);

      if (this.IsDownLevel())
      {
        RenderDownLevelContent(output);
      }
      else
      {
        // write script
        if (Page != null && !this.IsDownLevel())
        {
          // do we need to render scripts for non-Atlas?
          ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
          if (oScriptManager == null)
          {
            if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
            {
              Page.RegisterClientScriptBlock("A573G988.js", "");
              WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
            }
            if (!Page.IsClientScriptBlockRegistered("A573P101.js"))
            {
              Page.RegisterClientScriptBlock("A573P101.js", "");
              WriteGlobalClientScript(output, "ComponentArt.Web.UI.Upload.client_scripts", "A573P101.js");
            }
          }
        }

        // write markup
        AddAttributesToRender(output);
        // write content
        output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
        this.RenderContents(output);
        output.RenderEndTag(); // </div>

        // write init
        StringBuilder oSB = new StringBuilder();
        oSB.Append("/*** ComponentArt.Web.UI.Upload ").Append(this.VersionString()).Append(" ").Append(sControlId).Append(" ***/\n");

        oSB.Append("window.ComponentArt_Init_" + sControlId + " = function() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        oSB.Append("if(!window.ComponentArt_Upload_Loaded)\n");
        oSB.Append("\t{setTimeout('ComponentArt_Init_" + sControlId + "()', 50); return; }\n\n");

        oSB.AppendFormat("window.{0} = new ComponentArt_Upload('{0}');\n", sControlId);

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != sControlId)
        {
          oSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sControlId + "; " + sControlId + ".GlobalAlias = '" + ID + "'; }\n");
        }

        oSB.Append(sControlId + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");

        // Write properties
        if (AutoPostBack) oSB.Append(sControlId + ".AutoPostBack = 1;\n");
        if (CallbackParameter != "") oSB.Append(sControlId + ".CallbackParameter = " + Utils.ConvertStringToJSString(this.CallbackParameter) + ";\n");
        oSB.Append(sControlId + ".ClientEvents = " + Utils.ConvertClientEventsToJsObject(this._clientEvents) + ";\n");
        oSB.Append(sControlId + ".ClientTemplates = " + this._clientTemplates.ToString() + ";\n");
        if (this.FileInputClientTemplateId != "") oSB.Append(sControlId + ".FileInputClientTemplateId = '" + this.FileInputClientTemplateId + "';\n");
        if (this.FileInputHoverImageUrl != "") oSB.Append(sControlId + ".FileInputHoverImageUrl = '" + this.FileInputHoverImageUrl + "';\n");
        if (this.FileInputImageUrl != "") oSB.Append(sControlId + ".FileInputImageUrl = '" + this.FileInputImageUrl + "';\n");
        oSB.Append(sControlId + ".InitialFileCount = " + this.InitialFileCount + ";\n");
        oSB.Append(sControlId + ".MaximumFileCount = " + this.MaximumFileCount + ";\n");
        if (this.ProgressClientTemplateId != "") oSB.Append(sControlId + ".ProgressClientTemplateId = '" + this.ProgressClientTemplateId + "';\n");
        if (this.ProgressDomElementId != "") oSB.Append(sControlId + ".ProgressDomElementId = '" + this.ProgressDomElementId + "';\n");
        oSB.Append(sControlId + ".ProgressPopupOffsetX = " + this.ProgressPopupOffsetX + ";\n");
        oSB.Append(sControlId + ".ProgressPopupOffsetY = " + this.ProgressPopupOffsetY + ";\n");
        oSB.Append(sControlId + ".ProgressUpdateInterval = " + this.ProgressUpdateInterval + ";\n");
        if (this.UploadCompleteClientTemplateId != "") oSB.Append(sControlId + ".UploadCompleteClientTemplateId = '" + this.UploadCompleteClientTemplateId + "';\n");
        if (this.UploadErrorClientTemplateId != "") oSB.Append(sControlId + ".UploadErrorClientTemplateId = '" + this.UploadErrorClientTemplateId + "';\n");
        oSB.Append(sControlId + ".UploadId = '" + this.UploadId + "';\n");

        oSB.Append(sControlId + ".Initialize();\n");
        oSB.Append("\n}\n");

        // Initiate client control creation
        oSB.Append("ComponentArt_Init_" + sControlId + "();\n");

        output.Write(this.DemarcateClientScript(oSB.ToString()));
      }
    }

    private void HandleCallback()
    {
      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<UploadCallbackSuccess>");
      Context.Response.Write(this.UploadId); // send new id
      Context.Response.Write("</UploadCallbackSuccess>");
      Context.Response.End();
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }

      if (Context == null || Page == null)
      {
        return true;
      }

      string sUserAgent = Context.Request.UserAgent;

      if (sUserAgent == null || sUserAgent == "") return true;

      int iMajorVersion = 0;

      try
      {
        iMajorVersion = Context.Request.Browser.MajorVersion;
      }
      catch { }

      if (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion < 8)
      {
        return true;
      }
      else if ( // We are good if:

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5 && !Context.Request.Browser.Platform.ToUpper().StartsWith("MAC")) ||

        // 2. We have Gecko-based browser (Mozilla, Firefox, Netscape 6+)
        (sUserAgent.IndexOf("Gecko") >= 0) ||

        // 3. We have Opera 8 or later
        (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 8)
        )
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    internal void RenderDownLevelContent(HtmlTextWriter output)
    {
      // write markup
      AddAttributesToRender(output);
      // write content
      output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
      this.RenderContents(output);
      output.RenderEndTag(); // </div>
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="Uploaded"/> event of <see cref="Upload"/> class.
    /// </summary>
    public delegate void UploadedEventHandler(object sender, UploadUploadedEventArgs e);

    /// <summary>
    /// Fires after a file or files have been uploaded using the Upload control.
    /// </summary>
    [Description("Fires after a file or files have been uploaded using the Upload control."),
   Category("Upload Events")]
    public event UploadedEventHandler Uploaded;

    private void OnUploaded(UploadUploadedEventArgs e)
    {
      if (Uploaded != null)
      {
        Uploaded(this, e);
      }
    }

    #endregion
  }
}
