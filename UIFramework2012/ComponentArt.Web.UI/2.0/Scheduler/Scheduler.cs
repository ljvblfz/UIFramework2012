using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml;
using ComponentArt.Licensing.Providers;
using System.Data;
using System.IO;
using System.Globalization;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Scheduler control that allows for editing and managing appointments.
  /// It is usually used in conjunction with a scheduler view control.
  /// </summary>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [PersistChildren(false)]
  [ParseChildren(true)]
  public sealed class Scheduler : WebControl
  {

    private bool _dataBound = false;

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      // Is this a callback? Handle it now
      if (this.CausedCallback)
      {
        this.HandleCallback();
      }

      if (!this.IsCallback)
      {
      }

      this.RaiseEvents();
    }

    private void RaiseEvents()
    {
      foreach (SchedulerCommand command in this.Commands)
      {
        switch (command.Type)
        {
          case "add":
            this.OnAppointmentAdded(new SchedulerAppointmentAddedEventArgs((SchedulerAppointment)command.Properties["Appointment"], command));
            break;
          case "remove":
            this.OnAppointmentRemoved(new SchedulerAppointmentRemovedEventArgs((SchedulerAppointment)command.Properties["Appointment"], command));
            break;
          case "modify":
            this.OnAppointmentModified(new SchedulerAppointmentModifiedEventArgs((SchedulerAppointment)command.Properties["AppointmentBefore"], (SchedulerAppointment)command.Properties["AppointmentAfter"], command));
            break;
          case "addRecurring":
            this.OnRecurringAppointmentAdded(new SchedulerRecurringAppointmentAddedEventArgs((SchedulerRecurringAppointment)command.Properties["RecurringAppointment"], command));
            break;
          case "removeRecurring":
            this.OnRecurringAppointmentRemoved(new SchedulerRecurringAppointmentRemovedEventArgs((SchedulerRecurringAppointment)command.Properties["RecurringAppointment"], command));
            break;
          case "modifyRecurring":
            this.OnRecurringAppointmentModified(new SchedulerRecurringAppointmentModifiedEventArgs((SchedulerRecurringAppointment)command.Properties["RecurringAppointmentBefore"], (SchedulerRecurringAppointment)command.Properties["RecurringAppointmentAfter"], command));

            break;
        }
      }
    }

    protected override void OnInit(EventArgs oArgs)
    {
      base.OnInit(oArgs);
      if (this.Context != null && this.Context.Request != null && this.Context.Request.Form != null)
      {
        this.LoadAppointments(HttpUtility.UrlDecode(this.Context.Request.Form[this.AppointmentsInputId]));

        if (!this.CausedCallback)
        {
          this.LoadCommands(HttpUtility.UrlDecode(this.Context.Request.Form[this.CommandsInputId]));
        }
      }
      if (this.Context != null && this.Page != null)
      {
        string dummy = this.Page.GetPostBackEventReference(this); // Ensure that __doPostBack is output to client side
      }
    }

    private void HandleCallback()
    {
      try
      {
        StreamReader reader = new StreamReader(this.Context.Request.InputStream, this.Context.Request.ContentEncoding);
        string postData = reader.ReadToEnd();
        reader.Close();
        this.LoadCommands(postData);
      }
      catch (Exception ex)
      {
        //if (this.CatchServerErrors)
        //{
          this.HandleCallbackError(ex);
        //}
        //else
        //{
        //  throw ex;
        //}
      }
    }

    private void HandleCallbackError(Exception ex)
    {
      // don't output child tables
      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<CallbackError><![CDATA[");
      Context.Response.Write(ex.Message);
      Context.Response.Write("]]></CallbackError>");
      Context.Response.End();
    }

    private void LoadCommands(string commandString)
    {
      if (commandString == null)
      {
        return;
      }
      this.Commands.Clear();
      XmlDocument commandDoc = new XmlDocument();
      commandDoc.LoadXml(commandString);
      XmlNodeList commandNodes = commandDoc.DocumentElement.ChildNodes;
      foreach (XmlNode commandNode in commandNodes)
      {
        SchedulerCommand newCommand = new SchedulerCommand();
        newCommand.Type = commandNode.Attributes["Type"].Value;
        foreach (XmlNode childNode in commandNode.ChildNodes)
        {
          switch (childNode.Name)
          {
            case "Appointment":
            case "AppointmentBefore":
            case "AppointmentAfter":
              newCommand.Properties[childNode.Name] = this.LoadAppointment(childNode.FirstChild);
              break;
            case "Arguments":
              foreach (XmlNode argumentNode in childNode.ChildNodes)
              {
                string propertyName = argumentNode.FirstChild.InnerText;
                string propertyValue = argumentNode.LastChild.InnerText;
                switch (propertyName)
                {
                  case "Start":
                    newCommand.Arguments.Add(propertyName, Scheduler.ParseDateTime(propertyValue));
                    break;
                  case "Duration":
                    newCommand.Arguments.Add(propertyName, TimeSpan.FromMilliseconds(Utils.ParseDouble(propertyValue)));
                    break;
                  default:
                    newCommand.Arguments.Add(propertyName, propertyValue);
                    break;
                }
              }
              break;
            case "RecurringAppointment":
            case "RecurringAppointmentBefore":
            case "RecurringAppointmentAfter":
              newCommand.Properties[childNode.Name] = this.LoadRecurringAppointment(childNode.FirstChild);
              break;
          }
        }
        this.Commands.Add(newCommand);
      }
    }

    // Try including both appointment and recurringappointments into single XML
    private void LoadAppointments(string appointmentsString)
    {
      if (appointmentsString == null)
      {
        return;
      }
      this.Appointments.Clear();
      XmlDocument appointmentsDoc = new XmlDocument();
      appointmentsDoc.LoadXml(appointmentsString);
      XmlNodeList appointmentNodes = appointmentsDoc.DocumentElement.FirstChild.ChildNodes;
      foreach (XmlNode appointmentNode in appointmentNodes)
      {
        this.Appointments.Add(this.LoadAppointment(appointmentNode));
      }

      this.RecurringAppointments.Clear();
      if (appointmentsDoc.DocumentElement.ChildNodes.Count > 1)
      {
        XmlNodeList recurringAppointmentNodes = appointmentsDoc.DocumentElement.ChildNodes[1].ChildNodes;
        foreach (XmlNode recurringAppointmentNode in recurringAppointmentNodes)
        {
          this.RecurringAppointments.Add(this.LoadRecurringAppointment(recurringAppointmentNode));
        }
      }
    }

    private SchedulerAppointment LoadAppointment(XmlNode appointmentNode)
    {
      SchedulerAppointment appointment = new SchedulerAppointment();
      foreach (XmlNode propertyNode in appointmentNode.FirstChild.ChildNodes)
      {
        string propertyName = propertyNode.FirstChild.InnerText;
        string propertyValue = propertyNode.LastChild.InnerText;
        switch (propertyName)
        {
          case "Start":
            appointment.Start = Scheduler.ParseDateTime(propertyValue);
            break;
          case "Duration":
            appointment.Duration = TimeSpan.FromMilliseconds(Utils.ParseDouble(propertyValue));
            break;
          default:
            appointment.Properties[propertyName] = propertyValue;
            break;
        }
      }
      return appointment;
    }

    private SchedulerRecurringAppointment LoadRecurringAppointment(XmlNode recurringAppointmentNode)
    {
      SchedulerRecurringAppointment recurringAppointment = new SchedulerRecurringAppointment();
      foreach (XmlNode propertyNode in recurringAppointmentNode.FirstChild.ChildNodes)
      {
        string propertyName = propertyNode.FirstChild.InnerText;
        string propertyValue = propertyNode.LastChild.InnerText;
        switch (propertyName)
        {
          case "Range":
            Period period = new Period();
            foreach (XmlNode periodPropertyNode in propertyNode.LastChild.FirstChild.FirstChild.ChildNodes)
            {
              string periodPropertyName = periodPropertyNode.FirstChild.InnerText;
              string periodPropertyValue = periodPropertyNode.LastChild.InnerText;
              switch (periodPropertyName)
              {
                case "StartDateTime":
                  period.StartDateTime = Scheduler.ParseDateTime(periodPropertyValue);
                  break;
                case "Duration":
                  period.Duration = TimeSpan.FromMilliseconds(Utils.ParseDouble(periodPropertyValue));
                  break;
              }
            }
            recurringAppointment.Range = period;
            break;
          case "Appointment":
            recurringAppointment.Appointment = LoadAppointment(propertyNode.LastChild.FirstChild);
            break;
          case "Pattern":
            SchedulerRecurrencePattern pattern = new SchedulerRecurrencePattern();

            foreach (XmlNode patternPropertyNode in propertyNode.LastChild.FirstChild.FirstChild.ChildNodes)
            {
              string patternPropertyName = patternPropertyNode.FirstChild.InnerText;
              string patternPropertyValue = patternPropertyNode.LastChild.InnerText;
              switch (patternPropertyName)
              {
                case "DayOfMonth":
                  switch (patternPropertyValue)
                  {
                    case "0":
                      pattern.DayOfMonth = SchedulerDayOfMonth.Sunday;
                      break;
                    case "1":
                      pattern.DayOfMonth = SchedulerDayOfMonth.Monday;
                      break;
                    case "2": 
                      pattern.DayOfMonth = SchedulerDayOfMonth.Tuesday;
                      break;
                    case "3":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.Wednesday;
                      break;
                    case "4":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.Thursday;
                      break;
                    case "5":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.Friday;
                      break;
                    case "6":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.Saturday;
                      break;
                    case "7":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.Day;
                      break;
                    case "8":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.Weekday;
                      break;
                    case "9":
                      pattern.DayOfMonth =  SchedulerDayOfMonth.WeekendDay;
                      break;
                    default:  // Unset
                       pattern.DayOfMonth =  SchedulerDayOfMonth.Unset;
                      break;
                  }
                  break;
                case "WeekOfMonth":
                  switch (patternPropertyValue)
                  {
                    case "0":
                      pattern.WeekOfMonth = SchedulerWeekOfMonth.First;
                      break;
                    case "1":
                      pattern.WeekOfMonth = SchedulerWeekOfMonth.Second;
                      break;
                    case "2":
                      pattern.WeekOfMonth = SchedulerWeekOfMonth.Third;
                      break;
                    case "3":
                      pattern.WeekOfMonth = SchedulerWeekOfMonth.Fourth;
                      break;
                    default:
                      pattern.WeekOfMonth = SchedulerWeekOfMonth.Last;
                      break;
                  }
                  break;
                case "DaysOfWeek":
                  SchedulerDaysOfWeekCollection days = new SchedulerDaysOfWeekCollection();
                  foreach (XmlNode dayNode in patternPropertyNode.LastChild.ChildNodes)
                  {
                    switch (dayNode.InnerText)
                    {
                      case "0":
                        days.Add(DayOfWeek.Sunday);
                        break;
                      case "1":
                        days.Add(DayOfWeek.Monday);
                        break;
                      case "2":
                        days.Add(DayOfWeek.Tuesday);
                        break;
                      case "3":
                        days.Add(DayOfWeek.Wednesday);
                        break;
                      case "4":
                        days.Add(DayOfWeek.Thursday);
                        break;
                      case "5":
                        days.Add(DayOfWeek.Friday);
                        break;
                      default:
                        days.Add(DayOfWeek.Saturday);
                        break;
                    }
                  }
                  pattern.DaysOfWeek = days;
                  break;
                case "OccurrenceType":
                  switch (patternPropertyValue)
                  {
                    case "0":
                      pattern.OccurrenceType = SchedulerOccurrenceType.Minutely;
                      break;
                    case "1":
                      pattern.OccurrenceType = SchedulerOccurrenceType.Daily;
                      break;
                    case "2":
                      pattern.OccurrenceType = SchedulerOccurrenceType.Weekly;
                      break;
                    case "3":
                      pattern.OccurrenceType = SchedulerOccurrenceType.Monthly;
                      break;
                    default:
                      pattern.OccurrenceType = SchedulerOccurrenceType.Yearly;
                      break;
                  }
                  break;
                default:
                  pattern.Properties[patternPropertyName] = Int32.Parse(patternPropertyValue);
                  break;
              }
            }
            recurringAppointment.Pattern = pattern;
            break;
          default:
            recurringAppointment.Properties[propertyName] = propertyValue;
            break;
        }
      }
      return recurringAppointment;
    }
    /*
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
        return (Context != null && Context.Request != null && Context.Request.Params[this.CallbackParamName] != null);
      }
    }
    */

    private SchedulerAppointmentCollection _appointments;
    /// <summary>
    /// The collection of Appointments for this Scheduler.
    /// </summary>
    [Description("The collection of appointments in this storage.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SchedulerAppointmentCollection Appointments
    {
      get
      {
        if (this._appointments == null)
        {
          this._appointments = new SchedulerAppointmentCollection();
        }
        return this._appointments;
      }
    }

    private SchedulerRecurringAppointmentCollection _recurringAppointments;
    /// <summary>
    /// The collection of RecurringAppointments for this Scheduler.
    /// </summary>
    [Description("The collection of recurring appointments in this storage.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SchedulerRecurringAppointmentCollection RecurringAppointments
    {
      get
      {
        if (this._recurringAppointments == null)
        {
          this._recurringAppointments = new SchedulerRecurringAppointmentCollection();
        }
        return this._recurringAppointments;
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if (!this.IsDownLevel() && this.Page != null)
      {
          // Add core code
          if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
          {
            Page.RegisterClientScriptBlock("A573G988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573P290.js"))
          {
            Page.RegisterClientScriptBlock("A573P290.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573P290.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A577AB33.js"))
          {
            Page.RegisterClientScriptBlock("A577AB33.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Scheduler.client_scripts", "A577AB33.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A577AB34.js"))
          {
            Page.RegisterClientScriptBlock("A577AB34.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Scheduler.client_scripts", "A577AB34.js");
          }

        output.Write("<input type=\"hidden\"");
        output.WriteAttribute("id", this.CommandsInputId);
        output.WriteAttribute("name", this.CommandsInputId);
        // Not setting the value, because we use this only in client-to-server direction
        output.Write(" />");

        output.Write("<input type=\"hidden\"");
        output.WriteAttribute("id", this.AppointmentsInputId);
        output.WriteAttribute("name", this.AppointmentsInputId);
        // Not setting the value, because we use this only in client-to-server direction
        output.Write(" />");

        this.WriteStartupScript(output, this.GenerateClientSideIntializationScript(this.GetSaneId()));
      }
    }

    private string AppointmentsInputId
    {
      get
      {
        return this.GetSaneId() + "_appointments";
      }
    }

    private string CommandsInputId
    {
      get
      {
        return this.GetSaneId() + "_commands";
      }
    }

    private string GenerateClientSideIntializationScript(string clientControlId)
    {
      StringBuilder scriptSB = new StringBuilder();
      scriptSB.Append("window.ComponentArt_Instantiate_" + clientControlId + " = function() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      int retryDelay = 100; // 100 ms retry time sounds about right
      string areScriptsLoaded = "(window.cart_scheduler_utils_loaded && window.cart_scheduler_loaded)";
      scriptSB.Append("if (!" + areScriptsLoaded + ")\n");
      scriptSB.Append("{\n\tsetTimeout('ComponentArt_Instantiate_" + clientControlId + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

      // Instantiate the client-side object
      scriptSB.Append("window." + clientControlId + " = new ComponentArt_Scheduler(" + Utils.ConvertStringToJSString(clientControlId) + ");\n");

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != clientControlId)
      {
        scriptSB.Append("if(!window['" + this.ID + "']) { window['" + this.ID + "'] = window." + clientControlId + "; " + clientControlId + ".GlobalAlias = '" + this.ID + "'; }\n");
      }

      // Output client property settings
      this.PopulateClientProperties();
      scriptSB.Append(clientControlId + ".LoadProperties(" + this.GeneratePropertyStorage() + ");\n");

      // Initialize the client-side object
      scriptSB.Append(clientControlId + ".Initialize();\n");

      scriptSB.Append(clientControlId + ".render();\n");

      // Set the flag that the control has been initialized.  This is the last action in the initialization.
      scriptSB.Append("window." + clientControlId + "_loaded = true;\n}\n");

      // Call this instantiation function.  Remember that it will be repeated after a delay if it's not all ready.
      scriptSB.Append("ComponentArt_Instantiate_" + clientControlId + "();");

      return this.DemarcateClientScript(scriptSB.ToString(), "ComponentArt_SchedulerStorage_Startup_" + clientControlId + " " + this.VersionString());
    }

    protected override bool IsDownLevel()
    {
      return false;
    }

    /*
    private bool _isCallbackProcessed = false;
    private bool _isCallback = false;
    /// <summary>
    /// Whether the current request is a callback.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool IsCallback
    {
      get
      {
        if (!_isCallbackProcessed)
        {
          if (Context != null && Context.Request != null)
          {
            foreach (string key in Context.Request.Params.AllKeys)
            {
              if (key != null && key.StartsWith("Cart_") && key.IndexOf("_Callback") > 0)
              {
                _isCallback = true;
                _isCallbackProcessed = true;
                break;
              }
            }
          }
          _isCallbackProcessed = true;
        }
        return _isCallback;
      }
    }
    */

    private bool _autoUpdate = true;
    /// <summary>
    /// Whether to propagate changes in appointments to the server automatically.
    /// </summary>
    public bool AutoUpdate
    {
      get
      {
        return this._autoUpdate;
      }
      set
      {
        this._autoUpdate = value;
      }
    }

    private int _autoUpdateInterval = 0;
    /// <summary>
    /// How often appointment changes are propagated to the server.  Default is 0, indicating immediate propagation.
    /// </summary>
    public int AutoUpdateInterval
    {
      get
      {
        return this._autoUpdateInterval;
      }
      set
      {
        this._autoUpdateInterval = value;
      }
    }

    private SchedulerUpdateMode _autoUpdateMode = new SchedulerUpdateMode();
    /// <summary>
    /// The method of propagation to the server of appointment updates.
    /// </summary>
    public SchedulerUpdateMode AutoUpdateMode
    {
      get
      {
        return this._autoUpdateMode;
      }
      set
      {
        this._autoUpdateMode = value;
      }
    }

    private SchedulerClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public SchedulerClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SchedulerClientEvents();
        }
        return _clientEvents;
      }
    }

    private object _dataSource = null;
    /// <summary>
    /// The DataSource to bind to.
    /// </summary>
    [DefaultValue(null)]
    [Description("The DataSource to bind to.")]
    public object DataSource
    {
      get
      {
        return this._dataSource;
      }
      set
      {
        this._dataSource = value;
      }
    }

    private string _dataSourceID = null;
    /// <summary>
    /// ID of the DataSource to bind to.
    /// </summary>
    public string DataSourceID
    {
      get
      {
        return this._dataSourceID;
      }
      set
      {
        this._dataSourceID = value;
      }
    }

    private string CallbackParamName
    {
      get
      {
        return "Cart_" + this.GetSaneId() + "_Callback";
      }
    }

    private string _appointmentDataMember = null;
    /// <summary>
    /// The member in the DataSource from which to load data.  DEPRECATED; Use AppointmentDataMember instead.
    /// </summary>
    [Category("Data")]
    [Description("The member in the DataSource from which to load data.")]
    [DefaultValue("")]
    public string DataMember
    {
      get
      {
        return this._appointmentDataMember;
      }
      set
      {
        this._appointmentDataMember = value;
      }
    }
    /// <summary>
    /// The member in the DataSource from which to load appointment data.
    /// </summary>
    [Category("Data")]
    [Description("The member in the DataSource from which to load appointment data.")]
    [DefaultValue("")]
    public string AppointmentDataMember
    {
      get
      {
        return this._appointmentDataMember;
      }
      set
      {
        this._appointmentDataMember = value;
      }
    }
    private string _recurringAppointmentDataMember = null;
    /// <summary>
    /// The member in the DataSource from which to load recurring appointment data.
    /// </summary>
    [Category("Data")]
    [Description("The member in the DataSource from which to load recurring appointment data.")]
    [DefaultValue("")]
    public string RecurringAppointmentDataMember
    {
      get
      {
        return this._recurringAppointmentDataMember;
      }
      set
      {
        this._recurringAppointmentDataMember = value;
      }
    }

    private string _soaService = null;
    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode.
    /// </summary>
    /// <seealso cref="AutoUpdateMode" />
    [Category("Data")]
    [DefaultValue("")]
    [Description("The name of the standard SOA.UI service to use in web service mode.")]
    public string SoaService
    {
      get
      {
        return this._soaService;
      }
      set
      {
        this._soaService = value;
      }
    }

    private void LoadDataFromDataView(DataView dataView)
    {
      this.Appointments.Clear();
      for (int i = 0; i < dataView.Count; i++)
      {
        SchedulerAppointment appointment = new SchedulerAppointment();
        foreach (DataColumn column in dataView.Table.Columns)
        {
          appointment.Properties[column.ColumnName] = dataView[i][column.ColumnName];
        }
        this.Appointments.Add(appointment);
      }
    }

    private void LoadRecurringDataFromDataView(DataView dataView)
    {
      this.RecurringAppointments.Clear();
      for (int i = 0; i < dataView.Count; i++)
      {
        SchedulerRecurringAppointment recurringAppointment = new SchedulerRecurringAppointment();
        foreach (DataColumn column in dataView.Table.Columns)
        {
          recurringAppointment.Properties[column.ColumnName] = dataView[i][column.ColumnName];
        }
        this.RecurringAppointments.Add(recurringAppointment);
      }
    }

    private void LoadDataFromIEnumerable(IEnumerable enumerable)
    {
      IEnumerator enumerator = enumerable.GetEnumerator();

      this.Appointments.Clear();

      // go to the beginning
      enumerator.Reset();
      enumerator.MoveNext();

      while (enumerator.Current != null)
      {
        object current = enumerator.Current;

        SchedulerAppointment appointment = new SchedulerAppointment();
      }

      //TODO: Complete loading from IEnumerable
      throw new Exception("Loading from IEnumerable is not yet implemented");
    }

    private void LoadData()
    {
      if (this.DataSource == null)
      {
        return; // Exit here
      }

      if (this.DataSource is DataView)
      {
        this.LoadDataFromDataView((DataView)this.DataSource);
        return; // EXIT HERE
      }

      if (this.DataSource is DataSet)
      {
        if (this.AppointmentDataMember != null && this.AppointmentDataMember != String.Empty)
        {
          this.LoadDataFromDataView(((DataSet)this.DataSource).Tables[this.AppointmentDataMember].DefaultView);
          if (this.RecurringAppointmentDataMember != null && this.RecurringAppointmentDataMember != String.Empty)
          {
            this.LoadRecurringDataFromDataView(((DataSet)this.DataSource).Tables[this.RecurringAppointmentDataMember].DefaultView);
          }
        }
        else
        {
          this.LoadDataFromDataView(((DataSet)this.DataSource).Tables[0].DefaultView);
        }
        return; // EXIT HERE
      }

      if (this.DataSource is DataTable)
      {
        this.LoadDataFromDataView(((DataTable)this.DataSource).DefaultView);
        return; // EXIT HERE
      }

      if (this.DataSource is IEnumerable)
      {
        this.LoadDataFromIEnumerable((IEnumerable)this.DataSource);
        return; // EXIT HERE
      }

      throw new Exception("Cannot bind to data source of type " + _dataSource.GetType().ToString());
    }

    /// <summary>
    /// Bind to the set DataSource.
    /// </summary>
    public override void DataBind()
    {

      // Convert the data source control into disconnected data we can bind with
      if (this.DataSource == null && !String.IsNullOrEmpty(this.DataSourceID))
      {
        Control datasource = Utils.FindControl(this, this.DataSourceID);

        if (datasource == null)
        {
          throw new Exception("Data source control '" + this.DataSourceID + "' not found.");
        }

        else if (datasource is SqlDataSource)
        {
          SqlDataSource sqldatasource = (SqlDataSource)datasource;
          if (sqldatasource.DataSourceMode != SqlDataSourceMode.DataSet)
          {
            throw new Exception("DataSourceMode must be set to DataSet on the SqlDataSource control.");
          }
          this.DataSource = sqldatasource.Select(DataSourceSelectArguments.Empty);
        }

        else if (datasource is ObjectDataSource)
        {
          this.DataSource = ((ObjectDataSource)datasource).Select();
        }

        else if (datasource is IListSource)
        {
          this.DataSource = ((IListSource)datasource).GetList();
        }

        else
        {
          throw new Exception("Data source control must be a SqlDataSource or ObjectDataSource or must implement IListSource.");
        }
      }


      this.LoadData();

      base.DataBind();

      this._dataBound = true;
    }

    protected override void ComponentArtPreRender(EventArgs oArgs)
    {
      // make sure we're data-bound
      if (!this._dataBound && this.Appointments.Count == 0)
      {
        this.DataBind();
      }
    }

    protected Hashtable ClientProperties = new Hashtable();

    protected void PopulateClientProperties()
    {
      this.ClientProperties.Clear();
      this.ClientProperties.Add("AppointmentsPropertyStorage", this.Appointments.GeneratePropertyStorage());
      this.ClientProperties.Add("AutoUpdate", this.AutoUpdate.ToString().ToLower());
      this.ClientProperties.Add("AutoUpdateInterval", this.AutoUpdateInterval);
      this.ClientProperties.Add("AutoUpdateMode", (int)this.AutoUpdateMode);
      this.ClientProperties.Add("CallbackUrl", Utils.ConvertStringToJSString(Utils.GetResponseUrl(this.Context).Replace("'", "\\'") + (this.Context.Request.QueryString.Count > 0 ? "&" : "?") + this.CallbackParamName + "=yes"));
      this.ClientProperties.Add("ClientEvents", Utils.ConvertClientEventsToJsObject(this._clientEvents));
      this.ClientProperties.Add("SoaService", Utils.ConvertStringToJSString(this.SoaService));
      this.ClientProperties.Add("ViewIDs", this.GetViewIDs());

      this.ClientProperties.Add("RecurringAppointmentsPropertyStorage", this.RecurringAppointments.GeneratePropertyStorage());
    }

    protected string GeneratePropertyStorage()
    {
      String[] propertyArray = new String[this.ClientProperties.Count];
      int i = 0;
      foreach (string propertyName in this.ClientProperties.Keys)
      {
        propertyArray[i] = "['" + propertyName + "', " + this.ClientProperties[propertyName] + "]";
        i++;
      }
      return "[\n" + String.Join(",\n", propertyArray) + "\n]";
    }

    private ArrayList Commands = new ArrayList();

    internal ArrayList Views = new ArrayList();

    private string GetViewIDs()
    {
      string[] viewIDs = new string[this.Views.Count];
      for (int i = 0; i < this.Views.Count; i++)
      {
        viewIDs[i] = Utils.ConvertStringToJSString(((SchedulerViewBase)this.Views[i]).GetSaneId());
      }
      return "[" + String.Join(",", viewIDs) + "]";
    }

    #region Events

    //TODO: Adding more events, like appointments loaded

    /// <summary>
    /// Delegate for <see cref="AppointmentAdded"/> event of <see cref="Scheduler"/> class.
    /// </summary>
    public delegate void AppointmentAddedEventHandler(object sender, SchedulerAppointmentAddedEventArgs e);

    /// <summary>
    /// Fires after an appointment is added.
    /// </summary>
    [Description("Fires after an appointment is added.")]
    [Category("Events")]
    public event AppointmentAddedEventHandler AppointmentAdded;

    private void OnAppointmentAdded(SchedulerAppointmentAddedEventArgs e)
    {
      if (this.AppointmentAdded != null)
      {
        this.AppointmentAdded(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="AppointmentRemoved"/> event of <see cref="Scheduler"/> class.
    /// </summary>
    public delegate void AppointmentRemovedEventHandler(object sender, SchedulerAppointmentRemovedEventArgs e);

    /// <summary>
    /// Fires after an appointment is removed.
    /// </summary>
    [Description("Fires after an appointment is removed.")]
    [Category("Events")]
    public event AppointmentRemovedEventHandler AppointmentRemoved;

    private void OnAppointmentRemoved(SchedulerAppointmentRemovedEventArgs e)
    {
      if (this.AppointmentRemoved != null)
      {
        this.AppointmentRemoved(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="AppointmentModified"/> event of <see cref="Scheduler"/> class.
    /// </summary>
    public delegate void AppointmentModifiedEventHandler(object sender, SchedulerAppointmentModifiedEventArgs e);

    /// <summary>
    /// Fires after an appointment is modified.
    /// </summary>
    [Description("Fires after an appointment is modified.")]
    [Category("Events")]
    public event AppointmentModifiedEventHandler AppointmentModified;

    private void OnAppointmentModified(SchedulerAppointmentModifiedEventArgs e)
    {
      if (this.AppointmentModified != null)
      {
        this.AppointmentModified(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="RecurringAppointmentAdded"/> event of <see cref="Scheduler"/> class.
    /// </summary>
    public delegate void RecurringAppointmentAddedEventHandler(object sender, SchedulerRecurringAppointmentAddedEventArgs e);

    /// <summary>
    /// Fires after a recurring appointment is added.
    /// </summary>
    [Description("Fires after a recurring appointment is added.")]
    [Category("Events")]
    public event RecurringAppointmentAddedEventHandler RecurringAppointmentAdded;

    private void OnRecurringAppointmentAdded(SchedulerRecurringAppointmentAddedEventArgs e)
    {
      if (this.RecurringAppointmentAdded != null)
      {
        this.RecurringAppointmentAdded(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="RecurringAppointmentRemoved"/> event of <see cref="Scheduler"/> class.
    /// </summary>
    public delegate void RecurringAppointmentRemovedEventHandler(object sender, SchedulerRecurringAppointmentRemovedEventArgs e);

    /// <summary>
    /// Fires after a recurring appointment is removed.
    /// </summary>
    [Description("Fires after a recurring appointment is removed.")]
    [Category("Events")]
    public event RecurringAppointmentRemovedEventHandler RecurringAppointmentRemoved;

    private void OnRecurringAppointmentRemoved(SchedulerRecurringAppointmentRemovedEventArgs e)
    {
      if (this.RecurringAppointmentRemoved != null)
      {
        this.RecurringAppointmentRemoved(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="RecurringAppointmentModified"/> event of <see cref="Scheduler"/> class.
    /// </summary>
    public delegate void RecurringAppointmentModifiedEventHandler(object sender, SchedulerRecurringAppointmentModifiedEventArgs e);

    /// <summary>
    /// Fires after a recurring appointment is modified.
    /// </summary>
    [Description("Fires after a recurring appointment is modified.")]
    [Category("Events")]
    public event RecurringAppointmentModifiedEventHandler RecurringAppointmentModified;

    private void OnRecurringAppointmentModified(SchedulerRecurringAppointmentModifiedEventArgs e)
    {
      if (this.RecurringAppointmentModified != null)
      {
        this.RecurringAppointmentModified(this, e);
      }
    }


    #endregion

    /// <summary>
    /// Parses the given object as DateTime, attempting to avoid all the pitfalls of DateTime parsing typically encountered
    /// by the scheduler.
    /// </summary>
    internal static DateTime ParseDateTime(object o)
    {
      if (o is DateTime)
      {
        return (DateTime)o;
      }
      DateTime result;
      if (DateTime.TryParseExact(o.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
      {
        return result;
      }
      return DateTime.Parse(o.ToString(), CultureInfo.InvariantCulture);
    }

  }

  #region Events

  /// <summary>
  /// Arguments for <see cref="Scheduler.AppointmentAdded">AppointmentAdded</see> server-side event of <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerAppointmentAddedEventArgs : EventArgs
  {
    /// <summary>
    /// Appointment that has been added.
    /// </summary>
    public SchedulerAppointment Appointment;

    /// <summary>
    /// Information on the add appointment command that has been executed.
    /// </summary>
    public SchedulerCommand Command;

    /// <summary>
    /// Constructor of SchedulerAppointmentAddedEventArgs.
    /// </summary>
    /// <param name="appointment"></param>
    /// <param name="command"></param>
    public SchedulerAppointmentAddedEventArgs(SchedulerAppointment appointment, SchedulerCommand command)
    {
      this.Appointment = appointment;
      this.Command = command;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Scheduler.AppointmentRemoved">AppointmentRemoved</see> server-side event of <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerAppointmentRemovedEventArgs : EventArgs
  {
    /// <summary>
    /// Appointment that has been removed.
    /// </summary>
    public SchedulerAppointment Appointment;

    /// <summary>
    /// Information on the remove appointment command that has been executed.
    /// </summary>
    public SchedulerCommand Command;

    /// <summary>
    /// Constructor of SchedulerAppointmentRemovedEventArgs.
    /// </summary>
    /// <param name="appointment"></param>
    /// <param name="command"></param>
    public SchedulerAppointmentRemovedEventArgs(SchedulerAppointment appointment, SchedulerCommand command)
    {
      this.Appointment = appointment;
      this.Command = command;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Scheduler.AppointmentModified">AppointmentModified</see> server-side event of <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerAppointmentModifiedEventArgs : EventArgs
  {
    /// <summary>
    /// Appointment that has been modified, as it was before the modification.
    /// </summary>
    public SchedulerAppointment AppointmentBefore;

    /// <summary>
    /// Appointment that has been modified, as it is after the modification.
    /// </summary>
    public SchedulerAppointment AppointmentAfter;

    /// <summary>
    /// Information on the modify appointment command that has been executed.
    /// </summary>
    public SchedulerCommand Command;

    /// <summary>
    /// Constructor of SchedulerAppointmentModifiedEventArgs.
    /// </summary>
    /// <param name="appointmentBefore"></param>
    /// <param name="appointmentAfter"></param>
    /// <param name="command"></param>
    public SchedulerAppointmentModifiedEventArgs(SchedulerAppointment appointmentBefore, SchedulerAppointment appointmentAfter, SchedulerCommand command)
    {
      this.AppointmentBefore = appointmentBefore;
      this.AppointmentAfter = appointmentAfter;
      this.Command = command;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Scheduler.RecurringAppointmentAdded">RecurringAppointmentAdded</see> server-side event of <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerRecurringAppointmentAddedEventArgs : EventArgs
  {
    /// <summary>
    /// Appointment that has been added.
    /// </summary>
    public SchedulerRecurringAppointment RecurringAppointment;

    /// <summary>
    /// Information on the add recurring appointment command that has been executed.
    /// </summary>
    public SchedulerCommand Command;

    /// <summary>
    /// Constructor of SchedulerRecurringAppointmentAddedEventArgs.
    /// </summary>
    /// <param name="recurringAppointment"></param>
    /// <param name="command"></param>
    public SchedulerRecurringAppointmentAddedEventArgs(SchedulerRecurringAppointment recurringAppointment, SchedulerCommand command)
    {
      this.RecurringAppointment = recurringAppointment;
      this.Command = command;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Scheduler.RecurringAppointmentRemoved">RecurringAppointmentRemoved</see> server-side event of <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerRecurringAppointmentRemovedEventArgs : EventArgs
  {
    /// <summary>
    /// Recurring appointment that has been removed.
    /// </summary>
    public SchedulerRecurringAppointment RecurringAppointment;

    /// <summary>
    /// Information on the remove recurring appointment command that has been executed.
    /// </summary>
    public SchedulerCommand Command;

    /// <summary>
    /// Constructor of SchedulerRecurringAppointmentRemovedEventArgs.
    /// </summary>
    /// <param name="recurringAppointment"></param>
    /// <param name="command"></param>
    public SchedulerRecurringAppointmentRemovedEventArgs(SchedulerRecurringAppointment recurringAppointment, SchedulerCommand command)
    {
      this.RecurringAppointment = recurringAppointment;
      this.Command = command;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Scheduler.RecurringAppointmentModified">RecurringAppointmentModified</see> server-side event of <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerRecurringAppointmentModifiedEventArgs : EventArgs
  {
    /// <summary>
    /// Recurring appointment that has been modified, as it was before the modification.
    /// </summary>
    public SchedulerRecurringAppointment RecurringAppointmentBefore;

    /// <summary>
    /// Recurring appointment that has been modified, as it is after the modification.
    /// </summary>
    public SchedulerRecurringAppointment RecurringAppointmentAfter;

    /// <summary>
    /// Information on the modify recurring appointment command that has been executed.
    /// </summary>
    public SchedulerCommand Command;

    /// <summary>
    /// Constructor of SchedulerRecurringAppointmentModifiedEventArgs.
    /// </summary>
    /// <param name="recurringAppointmentBefore"></param>
    /// <param name="recurringAppointmentAfter"></param>
    /// <param name="command"></param>
    public SchedulerRecurringAppointmentModifiedEventArgs(SchedulerRecurringAppointment recurringAppointmentBefore, SchedulerRecurringAppointment recurringAppointmentAfter, SchedulerCommand command)
    {
      this.RecurringAppointmentBefore = recurringAppointmentBefore;
      this.RecurringAppointmentAfter = recurringAppointmentAfter;
      this.Command = command;
    }
  }

  #endregion

  /// <summary>
  /// The information on the appointment command that has been executed on the client.
  /// </summary>
  public class SchedulerCommand
  {
    /// <summary>
    /// Command type.
    /// </summary>
    public String Type = null;

    /// <summary>
    /// Arguments of the command.
    /// </summary>
    public Hashtable Arguments = new Hashtable();

    internal Hashtable Properties = new Hashtable();
  }

  /// <summary>
  /// Type of method to use to propagate the data between the client and the server.
  /// </summary>
  public enum SchedulerUpdateMode
  {
    /// <summary>
    /// Perform callbacks to propagate updates between the client and the server.
    /// </summary>
    CallBack,

    /// <summary>
    /// Perform postbacks to propagate updates between the client and the server.
    /// </summary>
    PostBack,

    /// <summary>
    /// Perform webservice calls to propagate updates between the client and the server.
    /// </summary>
    WebService

  }

}
