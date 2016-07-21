using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;
using System.Web;

namespace ComponentArt.Web.UI
{

  /// <summary>
  /// Provides common base for SchedulerView controls.
  /// </summary>
  [GuidAttribute("43F4A97A-8A94-45c7-BD13-FFB484A5037B")]
  public abstract class SchedulerViewBase : WebControl
  {
    #region ClientProperties

    internal Hashtable ClientProperties = new Hashtable();

    internal virtual void PopulateClientProperties()
    {
      this.ClientProperties.Clear();
      this.ClientProperties.Add("_EffectiveSchedulerID", Utils.ConvertStringToJSString(this.EffectiveSchedulerID()));
      this.ClientProperties.Add("ClientControlId", Utils.ConvertStringToJSString(this.GetSaneId()));
      this.ClientProperties.Add("Now", this.GetEffectiveNowJSDate());
      this.ClientProperties.Add("UseServerNow", this.UseServerNow.ToString().ToLower(CultureInfo.InvariantCulture));
      this.ClientProperties.Add("SchedulerID", Utils.ConvertStringToJSString(this.SchedulerID));
      this.ClientProperties.Add("TabIndex", this.TabIndex);
    }

    internal string GeneratePropertyStorage()
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

    #endregion ClientProperties

    #region Associated Scheduler

    private Scheduler _scheduler = null;
    /// <summary>
    /// The associated <see cref="Scheduler"/> control.
    /// </summary>
    public Scheduler Scheduler
    {
      get
      {
        this.InitializeScheduler();
        return this._scheduler;
      }
      set
      {
        this._scheduler = value;
        this._schedulerID = value.ID;
      }
    }

    private string _schedulerID = null;
    /// <summary>
    /// <see cref="Control.ID">ID</see> of the associated <see cref="Scheduler"/> control.
    /// </summary>
    public string SchedulerID
    {
      get
      {
        return this._schedulerID;
      }
      set
      {
        this._schedulerID = value;
        this._scheduler = null;
      }
    }

    private string EffectiveSchedulerID()
    {
      return (this.Scheduler != null) ? this.Scheduler.GetSaneId() : null;
    }

    private void InitializeScheduler()
    {
      if (this._scheduler == null && this._schedulerID != null && this._schedulerID != string.Empty)
      {
        this._scheduler = (Scheduler)Utils.FindControl(this, this._schedulerID);
      }
    }

    #endregion Associated Scheduler


    #region DateTime.Now

    private bool _useServerNow = false;
    /// <summary>
    /// Whether to dictate today's date and time from the server.  Default is false.
    /// </summary>
    /// <remarks>
    /// If UseServerNow is false, the SchedulerView uses the client's clock to determine today's date and time.
    /// </remarks>
    public bool UseServerNow
    {
      get
      {
        return this._useServerNow;
      }
      set
      {
        this._useServerNow = value;
      }
    }


    private DateTime? _now;
    /// <summary>
    /// The date and time to treat as current.  This property's value is relevant only when <see cref="UseServerNow"/> is true.
    /// </summary>
    /// <remarks>
    /// <para>This property's value is relevant only when <see cref="UseServerNow"/> is true.</para>
    /// <para>The default value is null, indicating that server's <see cref="DateTime.Now"/> is to be used.</para>
    /// </remarks>
    public DateTime? Now
    {
      get
      {
        return this._now;
      }
      set
      {
        this._now = value;
      }
    }



    private string GetEffectiveNowJSDate()
    {
      return Utils.ConvertNullableDateTimeToJsDate(this.UseServerNow ? (this.Now.HasValue ? this.Now : DateTime.Now) : null);
    }

    #endregion DateTime.Now
  }

  /// <summary>
  /// A scheduler view that displays the appointments in a chronological table.
  /// </summary>
  [PersistChildren(false)]
  [ParseChildren(true)]
  public sealed class SchedulerDaysView : SchedulerViewBase
  {

    private bool _styleSet = false;

    private ClientTemplate _appointmentClientTemplate;
    /// <summary>
    /// Client templates to use for the appointment element.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentClientTemplate
    {
      get
      {
        return this._appointmentClientTemplate;
      }
      set
      {
        this._appointmentClientTemplate = value;
      }
    }

    private string _appointmentCssClass = null;
    /// <summary>
    /// CSS class to apply to appointment elements.
    /// </summary>
    public string AppointmentCssClass
    {
      get
      {
        return this._appointmentCssClass;
      }
      set
      {
        this._appointmentCssClass = value;
      }
    }

    private ClientTemplate _appointmentDescriptionClientTemplate;
    /// <summary>
    /// Client templates to use for descriptions of appointment elements.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentDescriptionClientTemplate
    {
      get
      {
        return this._appointmentDescriptionClientTemplate;
      }
      set
      {
        this._appointmentDescriptionClientTemplate = value;
      }
    }

    private ClientTemplate _appointmentFocusClientTemplate;
    /// <summary>
    /// Client templates to use for the appointment element when it has focus.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentFocusClientTemplate
    {
      get
      {
        return this._appointmentFocusClientTemplate;
      }
      set
      {
        this._appointmentFocusClientTemplate = value;
      }
    }

    private string _appointmentFocusCssClass = null;
    /// <summary>
    /// CSS class to apply to appointment elements when they have focus.
    /// </summary>
    public string AppointmentFocusCssClass
    {
      get
      {
        return this._appointmentFocusCssClass;
      }
      set
      {
        this._appointmentFocusCssClass = value;
      }
    }

    private ClientTemplate _appointmentFocusDescriptionClientTemplate;
    /// <summary>
    /// Client templates to use for description of appointment element when it has focus.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentFocusDescriptionClientTemplate
    {
      get
      {
        return this._appointmentFocusDescriptionClientTemplate;
      }
      set
      {
        this._appointmentFocusDescriptionClientTemplate = value;
      }
    }

    private ClientTemplate _appointmentFocusFooterClientTemplate;
    /// <summary>
    /// Client templates to use for footers of appointment elements when they have focus.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentFocusFooterClientTemplate
    {
      get
      {
        return this._appointmentFocusFooterClientTemplate;
      }
      set
      {
        this._appointmentFocusFooterClientTemplate = value;
      }
    }

    private string _appointmentFocusFooterCssClass = null;
    /// <summary>
    /// CSS class to apply to footers of appointment elements when they have focus.
    /// </summary>
    public string AppointmentFocusFooterCssClass
    {
      get
      {
        return this._appointmentFocusFooterCssClass;
      }
      set
      {
        this._appointmentFocusFooterCssClass = value;
      }
    }

    private Unit _appointmentFocusFooterHeight = Unit.Empty;
    /// <summary>
    /// Height of the footer of appointment element when it has focus.
    /// </summary>
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit AppointmentFocusFooterHeight
    {
      get
      {
        return this._appointmentFocusFooterHeight;
      }
      set
      {
        if (value.IsEmpty || value.Type == UnitType.Pixel)
        {
          this._appointmentFocusFooterHeight = value;
        }
        else
        {
          throw new Exception("AppointmentFocusFooterHeight may only be specified in pixels.");
        }
      }
    }

    private ClientTemplate _appointmentFocusHeaderClientTemplate;
    /// <summary>
    /// Client templates to use for headers of appointment elements when they have focus.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentFocusHeaderClientTemplate
    {
      get
      {
        return this._appointmentFocusHeaderClientTemplate;
      }
      set
      {
        this._appointmentFocusHeaderClientTemplate = value;
      }
    }

    private string _appointmentFocusHeaderCssClass = null;
    /// <summary>
    /// CSS class to apply to headers of appointment elements when they have focus.
    /// </summary>
    public string AppointmentFocusHeaderCssClass
    {
      get
      {
        return this._appointmentFocusHeaderCssClass;
      }
      set
      {
        this._appointmentFocusHeaderCssClass = value;
      }
    }

    private Unit _appointmentFocusHeaderHeight = Unit.Empty;
    /// <summary>
    /// Height of the header of appointmentFocus element when it has focus.
    /// </summary>
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit AppointmentFocusHeaderHeight
    {
      get
      {
        return this._appointmentFocusHeaderHeight;
      }
      set
      {
        if (value.IsEmpty || value.Type == UnitType.Pixel)
        {
          this._appointmentFocusHeaderHeight = value;
        }
        else
        {
          throw new Exception("AppointmentFocusHeaderHeight may only be specified in pixels.");
        }
      }
    }

    private string _appointmentFocusLookId = null;
    /// <summary>
    /// Look to apply appointment elements when they have focus.
    /// </summary>
    public string AppointmentFocusLookId
    {
      get
      {
        return this._appointmentFocusLookId;
      }
      set
      {
        this._appointmentFocusLookId = value;
      }
    }

    private bool _appointmentFocusShowFooter = false;
    /// <summary>
    /// Whether to show headers for appointment elements when they have focus.
    /// </summary>
    [DefaultValue(false)]
    public bool AppointmentFocusShowFooter
    {
      get
      {
        return this._appointmentFocusShowFooter;
      }
      set
      {
        this._appointmentFocusShowFooter = value;
      }
    }

    private bool _appointmentFocusShowHeader = false;
    /// <summary>
    /// Whether to show headers for appointment elements when they have focus.
    /// </summary>
    [DefaultValue(false)]
    public bool AppointmentFocusShowHeader
    {
      get
      {
        return this._appointmentFocusShowHeader;
      }
      set
      {
        this._appointmentFocusShowHeader = value;
      }
    }

    private ClientTemplate _appointmentFocusTitleClientTemplate;
    /// <summary>
    /// Client templates to use for titles of appointment elements when they have focus.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentFocusTitleClientTemplate
    {
      get
      {
        return this._appointmentFocusTitleClientTemplate;
      }
      set
      {
        this._appointmentFocusTitleClientTemplate = value;
      }
    }

    private string _appointmentFocusTitleCssClass = null;
    /// <summary>
    /// CSS class to apply to titles of appointment elements when they have focus.
    /// </summary>
    public string AppointmentFocusTitleCssClass
    {
      get
      {
        return this._appointmentFocusTitleCssClass;
      }
      set
      {
        this._appointmentFocusTitleCssClass = value;
      }
    }

    private ClientTemplate _appointmentFooterClientTemplate;
    /// <summary>
    /// Client templates to use for footers of appointment elements.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentFooterClientTemplate
    {
      get
      {
        return this._appointmentFooterClientTemplate;
      }
      set
      {
        this._appointmentFooterClientTemplate = value;
      }
    }

    private string _appointmentFooterCssClass = null;
    /// <summary>
    /// CSS class to apply to footers of appointment elements.
    /// </summary>
    public string AppointmentFooterCssClass
    {
      get
      {
        return this._appointmentFooterCssClass;
      }
      set
      {
        this._appointmentFooterCssClass = value;
      }
    }

    private Unit _appointmentFooterHeight = Unit.Empty;
    /// <summary>
    /// Height of the footer of appointment element.
    /// </summary>
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit AppointmentFooterHeight
    {
      get
      {
        return this._appointmentFooterHeight;
      }
      set
      {
        if (value.IsEmpty || value.Type == UnitType.Pixel)
        {
          this._appointmentFooterHeight = value;
        }
        else
        {
          throw new Exception("AppointmentFooterHeight may only be specified in pixels.");
        }
      }
    }

    private ClientTemplate _appointmentHeaderClientTemplate;
    /// <summary>
    /// Client templates to use for headers of appointment elements.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentHeaderClientTemplate
    {
      get
      {
        return this._appointmentHeaderClientTemplate;
      }
      set
      {
        this._appointmentHeaderClientTemplate = value;
      }
    }

    private string _appointmentHeaderCssClass = null;
    /// <summary>
    /// CSS class to apply to headers of appointment elements.
    /// </summary>
    public string AppointmentHeaderCssClass
    {
      get
      {
        return this._appointmentHeaderCssClass;
      }
      set
      {
        this._appointmentHeaderCssClass = value;
      }
    }

    private Unit _appointmentHeaderHeight = Unit.Empty;
    /// <summary>
    /// Height of the header of appointment element.
    /// </summary>
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit AppointmentHeaderHeight
    {
      get
      {
        return this._appointmentHeaderHeight;
      }
      set
      {
        if (value.IsEmpty || value.Type == UnitType.Pixel)
        {
          this._appointmentHeaderHeight = value;
        }
        else
        {
          throw new Exception("AppointmentHeaderHeight may only be specified in pixels.");
        }
      }
    }

    private int _appointmentHorizontalSpacing = 0;
    /// <summary>
    /// Horizontal spacing in pixels between two appointment elements whose periods overlap.  Default value is 0. 
    /// </summary>
    /// <remarks>
    /// A positive value indicates a space between the DOM elements.
    /// A negative value indicates an overlap of the DOM elements.
    /// Care should be taken to keep the value of this property in a reasonable range, or rendering problems may result.
    /// </remarks>
    public int AppointmentHorizontalSpacing
    {
      get
      {
        return this._appointmentHorizontalSpacing;
      }
      set
      {
        this._appointmentHorizontalSpacing = value;
      }
    }

    private string _appointmentLookId = null;
    /// <summary>
    /// Look to apply to appointment elements.
    /// </summary>
    public string AppointmentLookId
    {
      get
      {
        return this._appointmentLookId;
      }
      set
      {
        this._appointmentLookId = value;
      }
    }

    private bool _appointmentShowFooter = false;
    /// <summary>
    /// Whether to show headers for appointment elements.
    /// </summary>
    [DefaultValue(false)]
    public bool AppointmentShowFooter
    {
      get
      {
        return this._appointmentShowFooter;
      }
      set
      {
        this._appointmentShowFooter = value;
      }
    }

    private bool _appointmentShowHeader = false;
    /// <summary>
    /// Whether to show headers for appointment elements.
    /// </summary>
    [DefaultValue(false)]
    public bool AppointmentShowHeader
    {
      get
      {
        return this._appointmentShowHeader;
      }
      set
      {
        this._appointmentShowHeader = value;
      }
    }

    private ClientTemplate _appointmentTitleClientTemplate;
    /// <summary>
    /// Client templates to use for titles of appointment elements.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate AppointmentTitleClientTemplate
    {
      get
      {
        return this._appointmentTitleClientTemplate;
      }
      set
      {
        this._appointmentTitleClientTemplate = value;
      }
    }

    private string _appointmentTitleCssClass = null;
    /// <summary>
    /// CSS class to apply to titles of appointment elements.
    /// </summary>
    public string AppointmentTitleCssClass
    {
      get
      {
        return this._appointmentTitleCssClass;
      }
      set
      {
        this._appointmentTitleCssClass = value;
      }
    }

    private TimeSpan _startTime = TimeSpan.FromHours(0);
    /// <summary>
    /// Start time of the grid view.
    /// </summary>
    public TimeSpan StartTime
    {
      get
      {
        return this._startTime;
      }
      set
      {
        this._startTime = value;
      }
    }

    private TimeSpan _endTime = TimeSpan.FromHours(24);
    /// <summary>
    /// End time of the grid view.
    /// </summary>
    public TimeSpan EndTime
    {
      get
      {
        return this._endTime;
      }
      set
      {
        this._endTime = value;
      }
    }

    private DateTime _startDate = DateTime.Today;
    /// <summary>
    /// Start date of the grid view.
    /// </summary>
    public DateTime StartDate
    {
      get
      {
        return this._startDate;
      }
      set
      {
        this._startDate = value;
      }
    }

    private DateTime _endDate = DateTime.Today + TimeSpan.FromDays(7 - 1);
    /// <summary>
    /// End date of the grid view.
    /// </summary>
    public DateTime EndDate
    {
      get
      {
        return this._endDate;
      }
      set
      {
        this._endDate = value;
      }
    }

    /// <summary>
    /// The number of days showing in the grid view.
    /// </summary>
    public int DateCount
    {
      get
      {
        return (this.EndDate - this.StartDate).Days + 1;
      }
      set
      {
        this.EndDate = this.StartDate + TimeSpan.FromDays(DateCount - 1);
      }
    }

    private string _rowCssClass = null;
    /// <summary>
    /// CSS class to apply to rows of the grid view.
    /// </summary>
    public string RowCssClass
    {
      get
      {
        return this._rowCssClass;
      }
      set
      {
        this._rowCssClass = value;
      }
    }

    private SchedulerDaysViewClientEvents _clientEvents = null;
    /// <summary>
    /// ClientEvents of the grid view.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SchedulerDaysViewClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SchedulerDaysViewClientEvents();
        }
        return _clientEvents;
      }
    }

    private string _columnCssClass = null;
    /// <summary>
    /// CSS class to apply to columns of the grid view.
    /// </summary>
    public string ColumnCssClass
    {
      get
      {
        return this._columnCssClass;
      }
      set
      {
        this._columnCssClass = value;
      }
    }

    private string _todayColumnCssClass = null;
    /// <summary>
    /// CSS class to apply to today's column of the grid view.
    /// </summary>
    public string TodayColumnCssClass
    {
      get
      {
        return this._todayColumnCssClass;
      }
      set
      {
        this._todayColumnCssClass = value;
      }
    }

    private TimeSpan _rowTime = TimeSpan.FromHours(1);
    /// <summary>
    /// The time that one row of the grid covers.  Default is one hour.
    /// </summary>
    public TimeSpan RowTime
    {
      get
      {
        return this._rowTime;
      }
      set
      {
        this._rowTime = value;
      }
    }

    private string _columnHeaderCssClass = null;
    /// <summary>
    /// CSS class for column header of the grid view.
    /// </summary>
    public string ColumnHeaderCssClass
    {
      get
      {
        return this._columnHeaderCssClass;
      }
      set
      {
        this._columnHeaderCssClass = value;
      }
    }

    private ClientTemplate _columnHeaderCellClientTemplate;
    /// <summary>
    /// Client template for column header cells.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate ColumnHeaderCellClientTemplate
    {
      get
      {
        return this._columnHeaderCellClientTemplate;
      }
      set
      {
        this._columnHeaderCellClientTemplate = value;
      }
    }

    private string _columnHeaderCellCssClass = null;
    /// <summary>
    /// CSS class for column header cells.
    /// </summary>
    public string ColumnHeaderCellCssClass
    {
      get
      {
        return this._columnHeaderCellCssClass;
      }
      set
      {
        this._columnHeaderCellCssClass = value;
      }
    }

    private string _todayColumnHeaderCellCssClass = null;
    /// <summary>
    /// CSS class for today's column header cell. 
    /// </summary>
    public string TodayColumnHeaderCellCssClass
    {
      get
      {
        return this._todayColumnHeaderCellCssClass;
      }
      set
      {
        this._todayColumnHeaderCellCssClass = value;
      }
    }

    private Unit _columnHeaderHeight = Unit.Empty;
    /// <summary>
    /// Height of the column header.
    /// </summary>
    public Unit ColumnHeaderHeight
    {
      get
      {
        return this._columnHeaderHeight;
      }
      set
      {
        this._columnHeaderHeight = value;
      }
    }

    private int _columnPaddingLeft = 0;
    /// <summary>
    /// Spacing in pixels from the left side of each column to the appointment element.
    /// </summary>
    public int ColumnPaddingLeft
    {
      get
      {
        return this._columnPaddingLeft;
      }
      set
      {
        this._columnPaddingLeft = value;
      }
    }

    private int _columnPaddingRight = 0;
    /// <summary>
    /// Spacing in pixels from the right side of each column to the appointment element.
    /// </summary>
    public int ColumnPaddingRight
    {
      get
      {
        return this._columnPaddingRight;
      }
      set
      {
        this._columnPaddingRight = value;
      }
    }

    private string _rowHeaderCssClass = null;
    /// <summary>
    /// CSS class for the row header of the grid view.
    /// </summary>
    public string RowHeaderCssClass
    {
      get
      {
        return this._rowHeaderCssClass;
      }
      set
      {
        this._rowHeaderCssClass = value;
      }
    }

    private string _rowHeaderCellCssClass = null;
    /// <summary>
    /// CSS class for row header cells.
    /// </summary>
    public string RowHeaderCellCssClass
    {
      get
      {
        return this._rowHeaderCellCssClass;
      }
      set
      {
        this._rowHeaderCellCssClass = value;
      }
    }

    private Unit _rowHeaderWidth = Unit.Empty;
    /// <summary>
    /// Width of the row header of the grid view.
    /// </summary>
    public Unit RowHeaderWidth
    {
      get
      {
        return this._rowHeaderWidth;
      }
      set
      {
        this._rowHeaderWidth = value;
      }
    }

    private string _rowHeaderTimeFormat = "h:mm";
    /// <summary>
    /// Format of the time displayed in row headers.
    /// </summary>
    public string RowHeaderTimeFormat
    {
      get
      {
        return this._rowHeaderTimeFormat;
      }
      set
      {
        this._rowHeaderTimeFormat = value;
      }
    }

    private string _columnHeaderDateFormat = "ddd M/d";
    /// <summary>
    /// Format of the dates displayed in column headers.
    /// </summary>
    public string ColumnHeaderDateFormat
    {
      get
      {
        return this._columnHeaderDateFormat;
      }
      set
      {
        this._columnHeaderDateFormat = value;
      }
    }

    private Unit _width = Unit.Empty;
    /// <summary>
    /// Width of the grid view.
    /// </summary>
    public override Unit Width
    {
      get
      {
        return this._width;
      }
      set
      {
        this._width = value;
      }
    }

    private Unit _height = Unit.Empty;
    /// <summary>
    /// Height of the grid view.
    /// </summary>
    public override Unit Height
    {
      get
      {
        return this._height;
      }
      set
      {
        this._height = value;
      }
    }

    private Unit _gridWidth = Unit.Empty;
    /// <summary>
    /// Width of the grid surface where the appointments are shown.
    /// </summary>
    public Unit GridWidth
    {
      get
      {
        return this._gridWidth;
      }
      set
      {
        this._gridWidth = value;
      }
    }

    private Unit _gridHeight = Unit.Empty;
    /// <summary>
    /// Height of the grid surface where the appointments are shown.
    /// </summary>
    public Unit GridHeight
    {
      get
      {
        return this._gridHeight;
      }
      set
      {
        this._gridHeight = value;
      }
    }

    private Unit _columnWidth = Unit.Empty;
    /// <summary>
    /// Width of the columns of the grid view.
    /// </summary>
    public Unit ColumnWidth
    {
      get
      {
        return this._columnWidth;
      }
      set
      {
        this._columnWidth = value;
      }
    }

    private Unit _rowHeight = Unit.Empty;
    /// <summary>
    /// Height of the rows of the grid view.
    /// </summary>
    public Unit RowHeight
    {
      get
      {
        return this._rowHeight;
      }
      set
      {
        this._rowHeight = value;
      }
    }

    private ClientTemplate _loadingPanelClientTemplate;
    /// <summary>
    /// Client template to use for feedback while waiting for a callback/webservice to complete.
    /// </summary>
    [Description("ID of client-template to use for feedback while waiting for a callback/webservice to complete.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate LoadingPanelClientTemplate
    {
      get
      {
        return _loadingPanelClientTemplate;
      }
      set
      {
        _loadingPanelClientTemplate = value;
      }
    }

    private int _loadingPanelFadeDuration = 100;
    /// <summary>
    /// The duration of the fade effect when transitioning the loading template, in milliseconds. A value of 0
    /// turns off the fade effect.
    /// </summary>
    [DefaultValue(100)]
    [Description("The duration of the fade effect when transitioning the loading template, in milliseconds.")]
    public int LoadingPanelFadeDuration
    {
      get
      {
        return this._loadingPanelFadeDuration;
      }
      set
      {
        this._loadingPanelFadeDuration = value;
      }
    }

    private int _loadingPanelFadeMaximumOpacity = 60;
    /// <summary>
    /// The maximum opacity percentage to fade to. Between 0 and 100. Default: 100.
    /// </summary>
    [DefaultValue(60)]
    [Description("The maximum opacity percentage to fade to.")]
    public int LoadingPanelFadeMaximumOpacity
    {
      get
      {
        return this._loadingPanelFadeMaximumOpacity;
      }
      set
      {
        this._loadingPanelFadeMaximumOpacity = value;
      }
    }

    private TimeSpan _precision = TimeSpan.Zero;
    /// <summary>
    /// Precision to which the starts and ends of appointments are positioned on the grid.
    /// Default is TimeSpan.Zero indicating that the appointments are positioned exactly.
    /// </summary>
    public TimeSpan Precision
    {
      get
      {
        return this._precision;
      }
      set
      {
        this._precision = value;
      }
    }

    private string _gridCssClass = null;
    /// <summary>
    /// CSS class applied to the grid surface element.
    /// </summary>
    public string GridCssClass
    {
      get
      {
        return this._gridCssClass;
      }
      set
      {
        this._gridCssClass = value;
      }
    }

    private string _cssClass = null;
    /// <summary>
    /// CSS class applied to the root element of the grid.
    /// </summary>
    public string CssClass
    {
      get
      {
        return this._cssClass;
      }
      set
      {
        this._cssClass = value;
      }
    }

    private ClientTemplate _rowHeaderCellClientTemplate;
    /// <summary>
    /// Client template for row header cells.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate RowHeaderCellClientTemplate
    {
      get
      {
        return this._rowHeaderCellClientTemplate;
      }
      set
      {
        this._rowHeaderCellClientTemplate = value;
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
          if (!Page.IsClientScriptBlockRegistered("A577AB36.js"))
          {
            Page.RegisterClientScriptBlock("A577AB36.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Scheduler.client_scripts", "A577AB36.js");
          }

        if (!this._styleSet)
        {
          output.Write(this.StyleTag());
        }

        string clientControlId = this.GetSaneId();

        output.Write("<span");
        output.Write(" id=\"" + clientControlId + "\"");
        output.Write(">");
        output.Write("</span>");

        output.Write("<input type=\"hidden\"");
        output.WriteAttribute("id", this.PropertiesInputId);
        output.WriteAttribute("name", this.PropertiesInputId);
        output.Write(" />");

        this.WriteStartupScript(output, this.GenerateClientSideIntializationScript(clientControlId));
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
      if (this.Scheduler != null)
      {
        this.Scheduler.Views.Add(this);
      }
    }

    protected override void OnInit(EventArgs oArgs)
    {
      base.OnInit(oArgs);
      if (this.Context != null && this.Context.Request != null && this.Context.Request.Form != null)
      {
        this.LoadProperties(HttpUtility.UrlDecode(this.Context.Request.Form[this.PropertiesInputId]));
      }
      if (this.Context != null && this.Page != null)
      {
        string dummy = this.Page.GetPostBackEventReference(this); // Ensure that __doPostBack is output to client side
      }
    }

    private string PropertiesInputId
    {
      get
      {
        return this.GetSaneId() + "_properties";
      }
    }

    private string StyleTag()
    {
      StringBuilder styleTag = new StringBuilder();
      styleTag.Append("<style type=\"text/css\">");
      styleTag.Append(this.GetResourceContent("ComponentArt.Web.UI.Scheduler.schedulerView.css"));
      styleTag.Append("</style>");
      return styleTag.ToString();
    }

    internal override void PopulateClientProperties()
    {
      base.PopulateClientProperties();
      this.ClientProperties.Add("AppointmentClientTemplate", ClientTemplate.TextToJsString(this.AppointmentClientTemplate));
      this.ClientProperties.Add("AppointmentCssClass", Utils.ConvertStringToJSString(this.AppointmentCssClass));
      this.ClientProperties.Add("AppointmentDescriptionClientTemplate", ClientTemplate.TextToJsString(this.AppointmentDescriptionClientTemplate));
      this.ClientProperties.Add("AppointmentFocusClientTemplate", ClientTemplate.TextToJsString(this.AppointmentFocusClientTemplate));
      this.ClientProperties.Add("AppointmentFocusCssClass", Utils.ConvertStringToJSString(this.AppointmentFocusCssClass));
      this.ClientProperties.Add("AppointmentFocusDescriptionClientTemplate", ClientTemplate.TextToJsString(this.AppointmentFocusDescriptionClientTemplate));
      this.ClientProperties.Add("AppointmentFocusFooterClientTemplate", ClientTemplate.TextToJsString(this.AppointmentFocusFooterClientTemplate));
      this.ClientProperties.Add("AppointmentFocusFooterCssClass", Utils.ConvertStringToJSString(this.AppointmentFocusFooterCssClass));
      this.ClientProperties.Add("AppointmentFocusFooterHeight", Utils.ConvertUnitToJSComponentArtUnit(this.AppointmentFocusFooterHeight));
      this.ClientProperties.Add("AppointmentFocusHeaderClientTemplate", ClientTemplate.TextToJsString(this.AppointmentFocusHeaderClientTemplate));
      this.ClientProperties.Add("AppointmentFocusHeaderCssClass", Utils.ConvertStringToJSString(this.AppointmentFocusHeaderCssClass));
      this.ClientProperties.Add("AppointmentFocusHeaderHeight", Utils.ConvertUnitToJSComponentArtUnit(this.AppointmentFocusHeaderHeight));
      this.ClientProperties.Add("AppointmentFocusLookId", Utils.ConvertStringToJSString(this.AppointmentFocusLookId));
      this.ClientProperties.Add("AppointmentFocusShowFooter", this.AppointmentFocusShowFooter.ToString().ToLower());
      this.ClientProperties.Add("AppointmentFocusShowHeader", this.AppointmentFocusShowHeader.ToString().ToLower());
      this.ClientProperties.Add("AppointmentFocusTitleClientTemplate", ClientTemplate.TextToJsString(this.AppointmentFocusTitleClientTemplate));
      this.ClientProperties.Add("AppointmentFocusTitleCssClass", Utils.ConvertStringToJSString(this.AppointmentFocusTitleCssClass));
      this.ClientProperties.Add("AppointmentFooterClientTemplate", ClientTemplate.TextToJsString(this.AppointmentFooterClientTemplate));
      this.ClientProperties.Add("AppointmentFooterCssClass", Utils.ConvertStringToJSString(this.AppointmentFooterCssClass));
      this.ClientProperties.Add("AppointmentFooterHeight", Utils.ConvertUnitToJSComponentArtUnit(this.AppointmentFooterHeight));
      this.ClientProperties.Add("AppointmentHeaderClientTemplate", ClientTemplate.TextToJsString(this.AppointmentHeaderClientTemplate));
      this.ClientProperties.Add("AppointmentHeaderCssClass", Utils.ConvertStringToJSString(this.AppointmentHeaderCssClass));
      this.ClientProperties.Add("AppointmentHeaderHeight", Utils.ConvertUnitToJSComponentArtUnit(this.AppointmentHeaderHeight));
      this.ClientProperties.Add("AppointmentHorizontalSpacing", this.AppointmentHorizontalSpacing);
      this.ClientProperties.Add("AppointmentLookId", Utils.ConvertStringToJSString(this.AppointmentLookId));
      this.ClientProperties.Add("AppointmentShowFooter", this.AppointmentShowFooter.ToString().ToLower());
      this.ClientProperties.Add("AppointmentShowHeader", this.AppointmentShowHeader.ToString().ToLower());
      this.ClientProperties.Add("AppointmentTitleClientTemplate", ClientTemplate.TextToJsString(this.AppointmentTitleClientTemplate));
      this.ClientProperties.Add("AppointmentTitleCssClass", Utils.ConvertStringToJSString(this.AppointmentTitleCssClass));
      this.ClientProperties.Add("ClientEvents", Utils.ConvertClientEventsToJsObject(this._clientEvents));
      this.ClientProperties.Add("ColumnCssClass", Utils.ConvertStringToJSString(this.ColumnCssClass));
      this.ClientProperties.Add("ColumnHeaderCellClientTemplate", ClientTemplate.TextToJsString(this.ColumnHeaderCellClientTemplate));
      this.ClientProperties.Add("ColumnHeaderCellCssClass", Utils.ConvertStringToJSString(this.ColumnHeaderCellCssClass));
      this.ClientProperties.Add("ColumnHeaderCssClass", Utils.ConvertStringToJSString(this.ColumnHeaderCssClass));
      this.ClientProperties.Add("ColumnHeaderDateFormat", Utils.ConvertStringToJSString(this.ColumnHeaderDateFormat));
      this.ClientProperties.Add("ColumnHeaderHeight", Utils.ConvertUnitToJSComponentArtUnit(this.ColumnHeaderHeight));
      this.ClientProperties.Add("ColumnPaddingLeft", Utils.ConvertUnitToJSComponentArtUnit(Unit.Pixel(this.ColumnPaddingLeft)));
      this.ClientProperties.Add("ColumnPaddingRight", Utils.ConvertUnitToJSComponentArtUnit(Unit.Pixel(this.ColumnPaddingRight)));
      this.ClientProperties.Add("ColumnWidth", Utils.ConvertUnitToJSComponentArtUnit(this.ColumnWidth));
      this.ClientProperties.Add("CssClass", Utils.ConvertStringToJSString(this.CssClass));
      this.ClientProperties.Add("EndDate", Utils.ConvertDateTimeToJsDate(this.EndDate));
      this.ClientProperties.Add("EndTime", this.EndTime.TotalMilliseconds.ToString());
      this.ClientProperties.Add("GridCssClass", Utils.ConvertStringToJSString(this.GridCssClass));
      this.ClientProperties.Add("GridHeight", Utils.ConvertUnitToJSComponentArtUnit(this.GridHeight));
      this.ClientProperties.Add("GridWidth", Utils.ConvertUnitToJSComponentArtUnit(this.GridWidth));
      this.ClientProperties.Add("Height", Utils.ConvertUnitToJSComponentArtUnit(this.Height));
      this.ClientProperties.Add("LoadingPanelClientTemplate", ClientTemplate.TextToJsString(this._loadingPanelClientTemplate));
      this.ClientProperties.Add("LoadingPanelFadeDuration", this._loadingPanelFadeDuration.ToString());
      this.ClientProperties.Add("LoadingPanelFadeMaximumOpacity", this._loadingPanelFadeMaximumOpacity.ToString());
      this.ClientProperties.Add("Precision", this.Precision.TotalMilliseconds.ToString());
      this.ClientProperties.Add("RowCssClass", Utils.ConvertStringToJSString(this.RowCssClass));
      this.ClientProperties.Add("RowHeaderCellCssClass", Utils.ConvertStringToJSString(this.RowHeaderCellCssClass));
      this.ClientProperties.Add("RowHeaderCellClientTemplate", ClientTemplate.TextToJsString(this.RowHeaderCellClientTemplate));
      this.ClientProperties.Add("RowHeaderCssClass", Utils.ConvertStringToJSString(this.RowHeaderCssClass));
      this.ClientProperties.Add("RowHeaderTimeFormat", Utils.ConvertStringToJSString(this.RowHeaderTimeFormat));
      this.ClientProperties.Add("RowHeaderWidth", Utils.ConvertUnitToJSComponentArtUnit(this.RowHeaderWidth));
      this.ClientProperties.Add("RowHeight", Utils.ConvertUnitToJSComponentArtUnit(this.RowHeight));
      this.ClientProperties.Add("RowTime", this.RowTime.TotalMilliseconds.ToString());
      this.ClientProperties.Add("StartDate", Utils.ConvertDateTimeToJsDate(this.StartDate));
      this.ClientProperties.Add("StartTime", this.StartTime.TotalMilliseconds.ToString());
      this.ClientProperties.Add("TodayColumnCssClass", Utils.ConvertStringToJSString(this.TodayColumnCssClass));
      this.ClientProperties.Add("TodayColumnHeaderCellCssClass", Utils.ConvertStringToJSString(this.TodayColumnHeaderCellCssClass));
      this.ClientProperties.Add("Width", Utils.ConvertUnitToJSComponentArtUnit(this.Width));

      this.ClientProperties.Add("AppointmentLooksPropertyStorage", this.AppointmentLooks.GeneratePropertyStorage());

      this.ClientProperties.Add("ElementID", Utils.ConvertStringToJSString(this.GetSaneId()));
    }

    private string GenerateClientSideIntializationScript(string clientControlId)
    {
      StringBuilder scriptSB = new StringBuilder();
      scriptSB.Append("window.ComponentArt_Instantiate_" + clientControlId + " = function() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      int retryDelay = 100; // 100 ms retry time sounds about right
      string areScriptsLoaded = "window.cart_schedulerdaysview_loaded";
      scriptSB.Append("if (!" + areScriptsLoaded + ")\n");
      scriptSB.Append("{\n\tsetTimeout('ComponentArt_Instantiate_" + clientControlId + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

      // Instantiate the client-side object
      scriptSB.Append("window." + clientControlId + " = new ComponentArt_SchedulerDaysView('" + clientControlId + "');\n");

      // Write postback function reference
      if (this.Page != null)
      {
        scriptSB.Append(clientControlId + ".Postback = function() { " + this.Page.GetPostBackEventReference(this) + " };\n");
      }

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

      // Set the flag that the control has been initialized.  This is the last action in the initialization.
      scriptSB.Append("window." + clientControlId + "_loaded = true;\n}\n");

      // Call this instantiation function.  Remember that it will be repeated after a delay if it's not all ready.
      scriptSB.Append("ComponentArt_Instantiate_" + clientControlId + "();");

      return this.DemarcateClientScript(scriptSB.ToString(), "ComponentArt_SchedulerDaysView_Startup_" + clientControlId + " " + this.VersionString());
    }

    private void LoadProperties(string propertiesXml)
    {
      if (propertiesXml == null)
      {
        return;
      }
//      this.Properties.Clear(); 
      XmlDocument propertiesDoc = new XmlDocument();
      propertiesDoc.LoadXml(propertiesXml);
      XmlNodeList propertyNodes = propertiesDoc.DocumentElement.ChildNodes;
      foreach (XmlNode propertyNode in propertyNodes)
      {
        string propertyName = propertyNode.FirstChild.InnerText;
        string propertyValue = propertyNode.LastChild.InnerText;

        switch (propertyName)
        {
          case "StartDate":
            this.StartDate = Scheduler.ParseDateTime(propertyValue);
            break;
          case "EndDate":
            this.EndDate = Scheduler.ParseDateTime(propertyValue);
            break;
          case "StartTime":
            this.StartTime = TimeSpan.FromMilliseconds(Utils.ParseDouble(propertyValue));
            break;
          case "EndTime":
            this.EndTime = TimeSpan.FromMilliseconds(Utils.ParseDouble(propertyValue));
            break;
          default:
            //
            break;
        }

      }
    }

    private SchedulerAppointmentLookCollection _looks;
    [Description("The collection of appointment looks defined by this view.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SchedulerAppointmentLookCollection AppointmentLooks
    {
      get
      {
        if (_looks == null)
        {
          _looks = new SchedulerAppointmentLookCollection();
        }
        return _looks;
      }
    }

    protected override bool IsDownLevel()
    {
      return false;
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      if (this.Page.Header != null)
      {
        this.Page.Header.Controls.Add(new LiteralControl(this.StyleTag()));
        this._styleSet = true;
      }
    }

  }

  /// <summary>
  /// Client-side events of <see cref="SchedulerDaysView"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class SchedulerDaysViewClientEvents : ClientEvents
  {
    /// <summary>
    /// Fires when the add appointment client-side logic should be executed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AppointmentAddOpen
    {
      get
      {
        return this.GetValue("AppointmentAddOpen");
      }
      set
      {
        this.SetValue("AppointmentAddOpen", value);
      }
    }

    /// <summary>
    /// Fires when the modify appointment client-side logic should be executed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AppointmentModifyOpen
    {
      get
      {
        return this.GetValue("AppointmentModifyOpen");
      }
      set
      {
        this.SetValue("AppointmentModifyOpen", value);
      }
    }

    /// <summary>
    /// Fires when the client-side object loads.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Load
    {
      get
      {
        return this.GetValue("Load");
      }
      set
      {
        this.SetValue("Load", value);
      }
    }  
  }

}
