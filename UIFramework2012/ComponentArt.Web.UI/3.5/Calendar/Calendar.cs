using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices; 
using ComponentArt.Licensing.Providers;
using System.Web.UI.HtmlControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Provides date manipulation interface with a browsable month-view table or a customized text box.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Depending on the value of <see cref="ControlType"/> property, creates a <b>Calendar</b> or a <b>Picker</b> on the page.
  ///   </para>
  ///   <para>
  ///     Both calendar and picker can have their date range limited using <see cref="MinDate"/> and <see cref="MaxDate"/> properties.
  ///   </para>
  ///   <para>
  ///     <b>Calendar</b>
  ///   </para>
  ///   <para>
  ///     A Calendar is a table with a cell for each day.  Depending on the values of <see cref="MonthRows"/> and <see cref="MonthColumns"/> properties,
  ///     the calendar can show one or more months.
  ///   </para>
  ///   <para>
  ///     Zero, one or more of the days can be <b>selected</b> by the user or programmatically - on the server or on the client.  
  ///     <see cref="SelectedDates"/> property contains the collection of selected dates.  It is possible to customize selection options, using
  ///     properties like <see cref="AllowDaySelection"/>, <see cref="AllowWeekSelection"/>, <see cref="AllowMonthSelection"/>, and 
  ///     <see cref="AllowMultipleSelection"/>.  Days can be designated as unselectable, by adding them to <see cref="DisabledDates"/> collection.
  ///   </para>
  ///   <para>
  ///     Appearance of the day cells can be customized based on a number of factors.  Today's day, weekend days, selected days, disabled days, and
  ///     days that are out of range can all have specific appearance.  More than one appearance can be in effect, as described in 
  ///     <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Calendar_Composing_Day_Styles.htm">Composing Calendar Day Styles</a> tutorial.  Appearance of days 
  ///     can be further customized using the <see cref="CustomDays"/> collection.
  ///   </para>
  ///   <para>
  ///     <b>Picker</b>
  ///   </para>
  ///   <para>
  ///     A Picker is a customized textbox, which reacts to various mouse and keyboard commands to show a date value.  Unlike a calendar, a picker 
  ///     always shows exactly one date, reflected by <see cref="SelectedDate"/> property.
  ///   </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.CalendarDesigner))]
  public sealed class Calendar : ComponentArt.Web.UI.WebControl, IPostBackDataHandler
  {
    #region Public Properties

    private string[] _abbreviatedDayNames = new string[7];
    /// <summary>
    /// Gets or sets a one-dimensional array of type <see cref="String"/> containing the culture-specific abbreviated names of the days of the week.
    /// </summary>
    /// <value>
    /// A one-dimensional array of type <see cref="String"/> containing the culture-specific abbreviated names of the days of the week. 
    /// This property corresponds to <see cref="DateTimeFormatInfo.AbbreviatedDayNames">DateTimeFormatInfo.AbbreviatedDayNames</see>
    /// </value>
    /// <remarks>
    /// This property gets modified when <see cref="DateTimeFormat"/> is set.
    /// </remarks>
    /// <seealso cref="AbbreviatedMonthNames" />
    /// <seealso cref="AMDesignator" />
    /// <seealso cref="DayNames" />
    /// <seealso cref="MonthNames" />
    /// <seealso cref="PMDesignator" />
    [TypeConverter(typeof(System.Web.UI.WebControls.StringArrayConverter))]
    public string[] AbbreviatedDayNames
    {
      get
      {
        return _abbreviatedDayNames;
      }
      set
      {
        if (value == null)
          throw new System.ArgumentNullException("AbbreviatedDayNames must not be null");
        if (!(value.Rank==1 && value.Length==7))
          throw new System.ArgumentException("AbbreviatedDayNames must be a one-dimensional string array of length 7");
        value.CopyTo(_abbreviatedDayNames, 0);
      }
    }

    private string[] _abbreviatedMonthNames = new string[13]; //Mimicking Microsoft's month arrays, the 13th is ""
    /// <summary>
    /// Gets or sets a one-dimensional array of type <see cref="String"/> containing the culture-specific abbreviated names of the months.
    /// </summary>
    /// <value>
    /// A one-dimensional array of type <see cref="String"/> containing the culture-specific abbreviated names of the months.
    /// In a 12-month calendar, the 13th element of the array is an empty string.
    /// This property corresponds to <see cref="DateTimeFormatInfo.AbbreviatedMonthNames">DateTimeFormatInfo.AbbreviatedMonthNames</see>.
    /// <note>Unlike <see cref="DateTimeFormatInfo.AbbreviatedMonthNames">DateTimeFormatInfo.AbbreviatedMonthNames</see>, 
    /// this property allows you to set it to an array of length 12. However, when getting this value, it always returns an array of length 13.</note>
    /// </value>
    /// <remarks>
    /// This property gets modified when <see cref="DateTimeFormat"/> is set.
    /// </remarks>
    /// <seealso cref="AbbreviatedDayNames" />
    /// <seealso cref="AMDesignator" />
    /// <seealso cref="DayNames" />
    /// <seealso cref="MonthNames" />
    /// <seealso cref="PMDesignator" />
    [TypeConverter(typeof(System.Web.UI.WebControls.StringArrayConverter))]
    public string[] AbbreviatedMonthNames
    {
      get
      {
        return _abbreviatedMonthNames;
      }
      set
      {
        if (value == null)
          throw new System.ArgumentNullException("AbbreviatedMonthNames must not be null");
        if (!((value.Rank==1) && (value.Length==12 || value.Length==13)))
          throw new System.ArgumentException("AbbreviatedMonthNames must be a one-dimensional string array of length 12 or 13");
        value.CopyTo(_abbreviatedMonthNames, 0);
        _abbreviatedMonthNames[12] = "";
      }
    }
    
    /// <summary>
    /// Whether the <see cref="Calendar"/> control allows selection of single days.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control allows selection of single days; otherwise, <b>false</b>. 
    /// The default value is <b>true</b>.
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    public bool AllowDaySelection
    {
      get
      {
        return Utils.ParseBool(ViewState["AllowDaySelection"], true);
      }
      set
      {
        ViewState["AllowDaySelection"] = value;
      }
    }

    /// <summary>
    /// Whether the <see cref="Calendar"/> control shows month selectors.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control shows the selectors that select all days in a month; 
    /// otherwise, <b>false</b>.  The default value is <b>false</b>.
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    public bool AllowMonthSelection
    {
      get
      {
        return Utils.ParseBool(ViewState["AllowMonthSelection"], false);
      }
      set
      {
        ViewState["AllowMonthSelection"] = value;
      }
    }

    /// <summary>
    /// Whether the <see cref="Calendar"/> control allows selection of multiple dates.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control allows the selection of more than one date entry; 
    /// otherwise, <b>false</b>.  The default value is <b>false</b>.
    /// </value>
    /// <remarks>
    /// When <see cref="AllowMonthSelection"/> or <see cref="AllowWeekSelection"/> are set to <b>true</b>, 
    /// the week and month selectors still select multiple dates even when <b>AllowMultipleSelection</b> 
    /// is set to <b>false.</b>  In these situations selection is limited to only one week or one month.
    /// <note>To show a <see cref="Calendar"/> control with no selectable dates, set all of 
    /// <see cref="AllowDaySelection"/>, <see cref="AllowMonthSelection"/>, and <see cref="AllowWeekSelection"/> 
    /// to <b>false</b>.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public bool AllowMultipleSelection
    {
      get
      {
        return Utils.ParseBool(ViewState["AllowMultipleSelection"], false);
      }
      set
      {
        ViewState["AllowMultipleSelection"] = value;
      }
    }

    /// <summary>
    /// Whether the <see cref="Calendar"/> control shows week selectors.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control shows the selectors that select all days in a week; 
    /// otherwise, <b>false</b>.  The default value is <b>false</b>.
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    public bool AllowWeekSelection
    {
      get
      {
        return Utils.ParseBool(ViewState["AllowWeekSelection"], false);
      }
      set
      {
        ViewState["AllowWeekSelection"] = value;
      }
    }

    private string _amDesignator = null;
    /// <summary>
    /// Gets or sets the string designator for hours that are "ante meridiem" (before noon).
    /// </summary>
    /// <value>
    /// The string designator for hours that are "ante meridiem" (before noon).
    /// This property corresponds to <see cref="DateTimeFormatInfo.AMDesignator">DateTimeFormatInfo.AMDesignator</see>
    /// </value>
    /// <remarks>
    /// This property gets modified when <see cref="DateTimeFormat"/> is set.
    /// </remarks>
    /// <seealso cref="AbbreviatedDayNames" />
    /// <seealso cref="AbbreviatedMonthNames" />
    /// <seealso cref="DayNames" />
    /// <seealso cref="MonthNames" />
    /// <seealso cref="PMDesignator" />
    public string AMDesignator
    {
      get
      {
        return _amDesignator;
      }
      set
      {
        _amDesignator = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when the calendar selection changes. Default is false.
    /// </summary>
    [DefaultValue(false)]
    public bool AutoPostBackOnSelectionChanged
    {
      get 
      {
        return Utils.ParseBool(ViewState["AutoPostBackOnSelectionChanged"], false);
      }
      set 
      {
        ViewState["AutoPostBackOnSelectionChanged"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when the calendar visible date changes. Default is false.
    /// </summary>
    [DefaultValue(false)]
    public bool AutoPostBackOnVisibleDateChanged
    {
      get 
      {
        return Utils.ParseBool(ViewState["AutoPostBackOnVisibleDateChanged"], false);
      }
      set 
      {
        ViewState["AutoPostBackOnVisibleDateChanged"] = value;
      }
    }

    /// <summary>
    /// Whether to use predefined CSS classes for theming.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to use predefined CSS classes for theming.")]
    public bool AutoTheming
    {
      get
      {
        object o = ViewState["AutoTheming"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["AutoTheming"] = value;
      }
    }

    /// <summary>
    /// String to be prepended to CSS classes used in theming.  Default is 'cart-'.
    /// </summary>
    [DefaultValue("cart-")]
    [Category("Behavior")]
    [Description("String to be prepended to CSS classes used in theming.  Default is 'cart-'.")]
    public string AutoThemingCssClassPrefix
    {
      get
      {
        object o = ViewState["AutoThemingCssClassPrefix"];
        return o == null ? "cart-" : (string)o;
      }
      set
      {
        ViewState["AutoThemingCssClassPrefix"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the calendar portion of the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    public string CalendarCssClass
    {
      get
      {
        return (string)ViewState["CalendarCssClass"];
      }
      set
      {
        ViewState["CalendarCssClass"] = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="TitleCssClass"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use TitleCssClass instead.")]
    [Obsolete("Deprecated.  Use TitleCssClass instead.", false)]
    public string CalendarTitleCssClass
    {
      get
      {
        return TitleCssClass;
      }
      set
      {
        TitleCssClass = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="TitleDateFormat"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use TitleDateFormat instead.")]
    [Obsolete("Deprecated.  Use TitleDateFormat instead.", false)]
    public string CalendarTitleDateFormat
    {
      get
      {
        return TitleDateFormat;
      }
      set
      {
        TitleDateFormat = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="TitleDateRangeSeparatorString"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use TitleDateRangeSeparatorString instead.")]
    [Obsolete("Deprecated.  Use TitleDateRangeSeparatorString instead.", false)]
    public string CalendarTitleDateRangeSeparatorString
    {
      get
      {
        return TitleDateRangeSeparatorString;
      }
      set
      {
        TitleDateRangeSeparatorString = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="TitleType"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use TitleType instead.")]
    [Obsolete("Deprecated.  Use TitleType instead.", false)]
    public CalendarTitleType CalendarTitleType
    {
      get
      {
        return TitleType;
      }
      set
      {
        TitleType = value;
      }
    }

    /// <summary>
    /// Specifies which rule is used to determine the first calendar week of the year.
    /// </summary>
    /// <remarks>
    /// Default value is the server culture's default FirstDayOfWeek.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>,
    /// the <see cref="AllowWeekSelection"/> property is set to true, 
    /// and the <see cref="ShowWeekNumbers"/> property is set to true.</para>
    /// </remarks>
    public CalendarWeekRule CalendarWeekRule
    {
      get
      {
        return Utils.ParseCalendarWeekRule(ViewState["CalendarWeekRule"], this.DateTimeFormat.CalendarWeekRule);
      }
      set
      {
        ViewState["CalendarWeekRule"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the amount of space between the contents of a cell and the cell's border.
    /// </summary>
    /// <value>
    /// The amount of space (in pixels) between the contents of a cell and the cell's border. The default value is <b>2</b>.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified cell padding is less than -1.</exception>
    /// <remarks>
    /// Use this property to control the spacing between the contents of a cell and the cell's border.
    /// The padding amount specified is added to all four sides of a cell. Individual cell sizes cannot be specified.
    /// <note>Setting this property to <b>-1</b> indicates that this property is not set in the rendered table.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public int CellPadding
    {
      get
      {
        return Utils.ParseInt(ViewState["CellPadding"], 2);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.CellPadding may not be less than -1.", this.ID));
        }
        ViewState["CellPadding"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the amount of space between cells.
    /// </summary>
    /// <value>
    /// The amount of space (in pixels) between cells. The default value is <b>0</b>.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified cell spacing is less than -1.</exception>
    /// <remarks>
    /// Use this property to control the spacing between individual cells in the calendar. 
    /// This spacing is applied both vertically and horizontally.
    /// <note>Setting this property to <b>-1</b> indicates that this property is not set in the rendered table.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public int CellSpacing
    {
      get
      {
        return Utils.ParseInt(ViewState["CellSpacing"], 0);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.CellSpacing may not be less than -1.", this.ID));
        }
        ViewState["CellSpacing"] = value;
      }
    }

    private CalendarClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public CalendarClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new CalendarClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Client-side function to call after the Calendar control finishes a <see cref="VisibleDate"/> month swap.
    /// </summary>
    /// <remarks>
    /// The function is passed the client-side Calendar object.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// <note>
    /// <b>ClientSideOnAfterVisibleDateChanged</b> is an event that fires after a month swap animation finishes.
    /// Not every change of <see cref="VisibleDate"/> is accompanied by a month swap animation, so it is possible to have 
    /// <see cref="ClientSideOnVisibleDateChanged"/> fire without <b>ClientSideOnAfterVisibleDateChanged</b> firing.
    /// However, when there is a month swap animation, <b>ClientSideOnAfterVisibleDateChanged</b> and
    /// <see cref="ClientSideOnBeforeVisibleDateChanged"/> will both fire, and in this case the order of firing is always: 
    /// <b>ClientSideOnBeforeVisibleDateChanged</b>, <b>ClientSideOnVisibleDateChanged</b>, <b>ClientSideOnAfterVisibleDateChanged</b>.
    /// </note>
    /// </remarks>
    /// <seealso cref="ClientSideOnBeforeVisibleDateChanged" />
    /// <seealso cref="ClientSideOnVisibleDateChanged" />
    [Category("Behavior")]
    [Description("Client-side function to call after the Calendar control finishes a VisibleDate month swap.")]
    public string ClientSideOnAfterVisibleDateChanged
    {
      get
      {
        return (string)ViewState["ClientSideOnAfterVisibleDateChanged"];
      }
      set
      {
        ViewState["ClientSideOnAfterVisibleDateChanged"] = value;
      }
    }

    /// <summary>
    /// Client-side function to call before the Calendar control starts a <see cref="VisibleDate"/> month swap.
    /// </summary>
    /// <remarks>
    /// The function is passed the client-side Calendar object.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// <note>
    /// <b>ClientSideBeforeVisibleDateChanged</b> is an event that fires before a month swap animation starts.
    /// Not every change of <see cref="VisibleDate"/> is accompanied by a month swap animation, so it is possible to have 
    /// <see cref="ClientSideOnVisibleDateChanged"/> fire without <b>ClientSideBeforeVisibleDateChanged</b> firing.
    /// However, when there is a month swap animation, <b>ClientSideBeforeVisibleDateChanged</b> and
    /// <see cref="ClientSideOnAfterVisibleDateChanged"/> will both fire, and in this case the order of firing is always: 
    /// <b>ClientSideOnBeforeVisibleDateChanged</b>, <b>ClientSideOnVisibleDateChanged</b>, <b>ClientSideOnAfterVisibleDateChanged</b>.
    /// </note>
    /// </remarks>
    /// <seealso cref="ClientSideOnAfterVisibleDateChanged" />
    /// <seealso cref="ClientSideOnVisibleDateChanged" />
    [Category("Behavior")]
    [Description("Client-side function to call before the Calendar control starts a VisibleDate month swap.")]
    public string ClientSideOnBeforeVisibleDateChanged
    {
      get
      {
        return (string)ViewState["ClientSideOnBeforeVisibleDateChanged"];
      }
      set
      {
        ViewState["ClientSideOnBeforeVisibleDateChanged"] = value;
      }
    }

    /// <summary>
    /// Client-side function to call when the <see cref="SelectedDates"/> collection of the calendar changes.
    /// </summary>
    /// <remarks>The function is passed the client-side Calendar object.</remarks>
    /// <seealso cref="ClientSideOnVisibleDateChanged" />
    [Category("Behavior")]
    [Description("Client-side function to call when the SelectedDates collection of the calendar changes.")]
    public string ClientSideOnSelectionChanged
    {
      get
      {
        return (string)ViewState["ClientSideOnSelectionChanged"];
      }
      set
      {
        ViewState["ClientSideOnSelectionChanged"] = value;
      }
    }

    /// <summary>
    /// Client-side function to call when the <see cref="VisibleDate"/> of the calendar changes.
    /// </summary>
    /// <remarks>
    /// The function is passed the client-side Calendar object.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    /// <seealso cref="ClientSideOnAfterVisibleDateChanged" />
    /// <seealso cref="ClientSideOnBeforeVisibleDateChanged" />
    /// <seealso cref="ClientSideOnSelectionChanged" />
    [Category("Behavior")]
    [Description("Client-side function to call when the VisibleDate of the calendar changes.")]
    public string ClientSideOnVisibleDateChanged
    {
      get
      {
        return (string)ViewState["ClientSideOnVisibleDateChanged"];
      }
      set
      {
        ViewState["ClientSideOnVisibleDateChanged"] = value;
      }
    }
    
    /// <summary>
    /// Whether to collapse a pop-up Calendar when a date is selected. Default is true.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [DefaultValue(true)]
    public bool CollapseOnSelect
    {
      get
      {
        return Utils.ParseBool(ViewState["CollapseOnSelect"], true);
      }
      set
      {
        ViewState["CollapseOnSelect"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the inner content portion of the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// <para>
    /// The DOM element to which this CSS class is applied is located just inside the DOM element to which <see cref="CalendarCssClass"/>
    /// is applied.
    /// </para>
    /// </remarks>
    public string ContentCssClass
    {
      get
      {
        return (string)ViewState["ContentCssClass"];
      }
      set
      {
        ViewState["ContentCssClass"] = value;
      }
    }

    /// <summary>
    /// Determines whether the control renders as a picker or as a calendar. Default is calendar.
    /// </summary>
    public CalendarControlType ControlType
    {
      get
      {
        return Utils.ParseCalendarControlType(ViewState["ControlType"], CalendarControlType.Calendar);
      }
      set
      {
        ViewState["ControlType"] = value;
      }
    }

    /// <summary>
    /// This is a set-only property enabling you to set the calendar's <see cref="DateTimeFormat"/> to the given culture's <b>DateTimeFormat</b>.
    /// </summary>
    /// <seealso cref="CultureId" />
    /// <seealso cref="CultureName" />
    /// <seealso cref="DateTimeFormat" />
    public CultureInfo Culture
    {
      set
      {
        this.DateTimeFormat = value.DateTimeFormat;
      }
    }

    /// <summary>
    /// This is a set-only property enabling you to set the calendar's <see cref="DateTimeFormat"/> to the given culture's <b>DateTimeFormat</b>.
    /// </summary>
    /// <seealso cref="Culture" />
    /// <seealso cref="CultureName" />
    /// <seealso cref="DateTimeFormat" />
    public int CultureId
    {
      set
      {
        this.DateTimeFormat = new CultureInfo(value,false).DateTimeFormat;
      }
    }

    /// <summary>
    /// This is a set-only property enabling you to set the calendar's <see cref="DateTimeFormat"/> to the given culture's <b>DateTimeFormat</b>.
    /// </summary>
    /// <seealso cref="Culture" />
    /// <seealso cref="CultureId" />
    /// <seealso cref="DateTimeFormat" />
    public string CultureName
    {
      set
      {
        this.DateTimeFormat = new CultureInfo(value,false).DateTimeFormat;
      }
    }

    /// <summary>
    /// Gets a collection of <see cref="CalendarDay"/> objects that represent the customized days in the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    [Browsable(false)]
    [Description("Collection of template controls.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public CalendarDayCollection CustomDays
    {
      get
      {
        if (this._customDaysCollection == null)
        {
          if (this._customDaysList == null)
          {
            this._customDaysList = new SortedList();
          }
          this._customDaysCollection = new CalendarDayCollection(this._customDaysList);
        }
        return this._customDaysCollection;
      }
    }

    private DateTimeFormatInfo _dateTimeFormat;
    /// <summary>
    /// Gets or sets a <see cref="DateTimeFormatInfo"/> that defines the appropriate format of displaying dates and times.
    /// </summary>
    /// <value>A <see cref="DateTimeFormatInfo"/> that defines the culturally appropriate format of displaying dates and times.</value>
    /// <remarks>When setting this property, a number of calendar's other properties will be set to the corresponding value from the
    /// given <see cref="DateTimeFormatInfo"/>.  These include: <see cref="AbbreviatedDayNames"/>, <see cref="AbbreviatedMonthNames"/>, 
    /// <see cref="AMDesignator"/>, <see cref="DayNames"/>, <see cref="MonthNames"/>, and <see cref="PMDesignator"/>.</remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTimeFormatInfo DateTimeFormat
    {
      get
      {
        return _dateTimeFormat;
      }
      set
      {
        this._dateTimeFormat = value;
        this.AbbreviatedDayNames = value.AbbreviatedDayNames;
        this.AbbreviatedMonthNames = value.AbbreviatedMonthNames;
        this.AMDesignator = value.AMDesignator;
        this.DayNames = value.DayNames;
        this.MonthNames = value.MonthNames;
        this.PMDesignator = value.PMDesignator;
        this._defaultFirstDayOfWeek = value.FirstDayOfWeek;
        this._defaultCalendarWeekRule = value.CalendarWeekRule;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the days of the displayed calendar.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for days displayed in the <see cref="Calendar"/> control. If the 
    /// <b>DayActiveCssClass</b> property is not set, the CSS class specified in the <see cref="DayHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DayActiveCssClass
    {
      get
      {
        return (string)ViewState["DayActiveCssClass"];
      }
      set
      {
        ViewState["DayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the days of the displayed calendar.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for days displayed in the <see cref="Calendar"/> control.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DayCssClass
    {
      get
      {
        return (string)ViewState["DayCssClass"];
      }
      set
      {
        ViewState["DayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the section that displays the day of the week.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ShowDayHeader"/> property is set to <b>true</b>.
    /// <note>The name format for the days of the week is controlled by the <see cref="DayNameFormat"/> property.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DayHeaderCssClass
    {
      get
      {
        return (string)ViewState["DayHeaderCssClass"];
      }
      set
      {
        ViewState["DayHeaderCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the days of the displayed calendar.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for days displayed in the <see cref="Calendar"/> control. If the 
    /// <b>DayHoverCssClass</b> property is not set, the CSS class specified in the <see cref="DayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DayHoverCssClass
    {
      get
      {
        return (string)ViewState["DayHoverCssClass"];
      }
      set
      {
        ViewState["DayHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the name format of days of the week.
    /// </summary>
    /// <value>
    /// One of the <see cref="DayNameFormat"/> values. The default value is <b>Short</b>.
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ShowDayHeader"/> property is set to <b>true</b>.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public DayNameFormat DayNameFormat
    {
      get
      {
        return Utils.ParseDayNameFormat(ViewState["DayNameFormat"], DayNameFormat.Short);
      }
      set
      {
        ViewState["DayNameFormat"] = value;
      }
    }

    private string[] _dayNames = new string[7];
    /// <summary>
    /// Gets or sets a one-dimensional array of type <see cref="String"/> containing the culture-specific full names of the days of the week.
    /// </summary>
    /// <value>
    /// A one-dimensional array of type <see cref="String"/> containing the culture-specific full names of the days of the week. 
    /// This property corresponds to <see cref="DateTimeFormatInfo.DayNames">DateTimeFormatInfo.DayNames</see>
    /// </value>
    /// <remarks>
    /// This property gets modified when <see cref="DateTimeFormat"/> is set.
    /// </remarks>
    /// <seealso cref="AbbreviatedDayNames" />
    /// <seealso cref="AbbreviatedMonthNames" />
    /// <seealso cref="AMDesignator" />
    /// <seealso cref="MonthNames" />
    /// <seealso cref="PMDesignator" />
    [TypeConverter(typeof(System.Web.UI.WebControls.StringArrayConverter))]
    public string[] DayNames
    {
      get
      {
        return _dayNames;
      }
      set
      {
        if (value == null)
          throw new System.ArgumentNullException("DayNames must not be null");
        if (!(value.Rank==1 && value.Length==7))
          throw new System.ArgumentException("DayNames must be a one-dimensional string array of length 7");
        value.CopyTo(_dayNames, 0);
      }
    }

    /// <summary>
    /// Gets a collection of <see cref="System.DateTime"/> objects that represent the disabled dates in the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="SelectedDates"/> collection, <b>DisabledDates</b> collection does not require all the dates
    /// to be chronologically between <see cref="MinDate"/> and <see cref="MaxDate"/> (inclusively).
    /// However, since all dates before <b>MinDate</b> and all dates after <b>MaxDate</b> are automatically treated
    /// as disabled, there is no point in ever adding dates outside that range to the <b>DisabledDates</b> collection.
    /// </remarks>
    public DateCollection DisabledDates
    {
      get
      {
        if (this._disabledDatesCollection == null)
        {
          if (this._disabledDatesList == null)
          {
            this._disabledDatesList = new ArrayList();
          }
          this._disabledDatesCollection = new DateCollection(this._disabledDatesList);
        }
        return this._disabledDatesCollection;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the disabled days.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for the disabled days of the <see cref="Calendar"/> control.
    /// If the <b>DisabledDayActiveCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="DisabledDayHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DisabledDayActiveCssClass
    {
      get
      {
        return (string)ViewState["DisabledDayActiveCssClass"];
      }
      set
      {
        ViewState["DisabledDayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the disabled days.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for the disabled days of the <see cref="Calendar"/> control.
    /// If the <b>DisabledDayCssClass</b> property is not set, the CSS class specified in the <see cref="DayCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DisabledDayCssClass
    {
      get
      {
        return (string)ViewState["DisabledDayCssClass"];
      }
      set
      {
        ViewState["DisabledDayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the disabled days.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for the disabled days of the <see cref="Calendar"/> control.
    /// If the <b>DisabledDayHoverCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="DisabledDayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string DisabledDayHoverCssClass
    {
      get
      {
        return (string)ViewState["DisabledDayHoverCssClass"];
      }
      set
      {
        ViewState["DisabledDayHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Determines which day to show as the first day of the week in the calendar.
    /// </summary>
    /// <remarks>
    /// Default value is the server culture's default FirstDayOfWeek.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public FirstDayOfWeek FirstDayOfWeek
    {
      get
      {
        return Utils.ParseFirstDayOfWeek(ViewState["FirstDayOfWeek"]);
      }
      set
      {
        ViewState["FirstDayOfWeek"] = value;
      }
    }

    private ClientTemplate _footerClientTemplate;
    /// <summary>
    /// Client-side template for the footer of the calendar.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate FooterClientTemplate
    {
      get
      {
        return this._footerClientTemplate;
      }
      set
      {
        this._footerClientTemplate = value;
      }
    }
    
    private ClientTemplate _headerClientTemplate;
    /// <summary>
    /// Client-side template for the header of the calendar.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate HeaderClientTemplate
    {
      get
      {
        return this._headerClientTemplate;
      }
      set
      {
        this._headerClientTemplate = value;
      }
    }

    /// <summary>
    /// Prefix to use for all image URL paths.
    /// </summary>
    [Category("Support")]
    [Description("Used as a prefix for all image URLs.")]
    [DefaultValue("")]
    public string ImagesBaseUrl
    {
      get
      {
        object o = ViewState["ImagesBaseUrl"]; 
        return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o); 
      }

      set
      {
        ViewState["ImagesBaseUrl"] = value; 
      }
    }

    /// <summary>
    /// Gets or sets the maximum date and time that can be selected in the control.
    /// </summary>
    /// <value>
    /// The minimum date and time that can be selected in the control.
    /// The default is December 31st 00:00:00 of the year twenty years after <see cref="TodaysDate"/>.
    /// </value>
    /// <exception cref="ArgumentException">The value assigned is less than the <see cref="MinDate"/> value.</exception>
    /// <exception cref="SystemException">The value assigned is more than the <see cref="MaxDateTime"/> value.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The value assigned is less than a member of 
    /// <see cref="SelectedDates"/> collection.</exception>
    public DateTime MaxDate
    {
      get
      {
        return Utils.ParseDateTime(ViewState["MaxDate"], new DateTime(this.TodaysDate.Year+20, 12, 31));
      }
      set
      {
        if (Calendar.MaxDateTime < value)
        {
          throw new Exception(String.Format("{0}.MaxDate may not be greater than Calendar.MaxDateTime.", this.ID));
        }
        ViewState["MaxDate"] = value;
      }
    }

    /// <summary>
    /// Specifies the maximum date value of the Calendar control. This field is read-only.
    /// <b>Note: Use <see cref="MaxDate" /> to adjust the settable range of dates.</b>
    /// </summary>
    /// <remarks>
    /// The maximum date is set to 12/31/9998 23:59:59.
    /// <para>
    /// <b>Use <see cref="MaxDate" /> to adjust the settable range of dates.</b>
    /// </para>
    /// </remarks>
    /// <seealso cref="MaxDate"/>
    public static readonly DateTime MaxDateTime = new DateTime(9998, 12, 31, 23, 59, 59);

    /// <summary>
    /// Gets or sets the minimum date and time that can be selected in the control.
    /// </summary>
    /// <value>
    /// The minimum date and time that can be selected in the control.
    /// The default is January 1st 00:00:00 of the year one hundred years before <see cref="TodaysDate"/>.
    /// </value>
    /// <exception cref="ArgumentException">The value assigned is more than the <see cref="MaxDate"/> value.</exception>
    /// <exception cref="SystemException">The value assigned is less than the <see cref="MinDateTime"/> value.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The value assigned is more than a member of 
    /// <see cref="SelectedDates"/> collection.</exception>
    public DateTime MinDate
    {
      get
      {
        return Utils.ParseDateTime(ViewState["MinDate"], new DateTime(this.TodaysDate.Year-100, 1, 1));
      }
      set
      {
        if (value < Calendar.MinDateTime)
        {
          throw new Exception(String.Format("{0}.MinDate may not be less than Calendar.MinDateTime.", this.ID));
        }
        ViewState["MinDate"] = value;
      }
    }

    /// <summary>
    /// Specifies the minimum date value of the Calendar control. This field is read-only.
    /// <b>Note: Use <see cref="MinDate" /> to adjust the settable range of dates.</b>
    /// </summary>
    /// <remarks>
    /// The minimum date is set to 1/1/1753 00:00:00.
    /// <para>
    /// <b>Use <see cref="MinDate" /> to adjust the settable range of dates.</b>
    /// </para>
    /// </remarks>
    /// <seealso cref="MinDate"/>
    public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1, 0, 0, 0);

    /// <summary>
    /// Gets or sets the number of month columns displayed in the Calendar.
    /// </summary>
    /// <value>
    /// The number of columns of months displayed in the <see cref="Calendar"/> control. The default value is <b>1</b>.
    /// </value>
    /// <remarks>
    /// The value of <b>MonthColumns</b> must be at least 1 and no less than the value of <see cref="VisibleMonthColumn"/> property.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified number of columns is less than 1 or less than
    /// the value of the <b>VisibleMonthColumn</b> property.</exception>
    public int MonthColumns
    {
      get
      {
        return Utils.ParseInt(ViewState["MonthColumns"], 1);
      }
      set
      {
        if (value < 1)
        {
          throw new Exception(String.Format("{0}.MonthColumns may not be less than 1.", this.ID));
        }
        ViewState["MonthColumns"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for months in the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    public string MonthCssClass
    {
      get
      {
        return (string)ViewState["MonthCssClass"];
      }
      set
      {
        ViewState["MonthCssClass"] = value;
      }
    }

    private string[] _monthNames = new string[13]; //Mimicking Microsoft's month arrays, the 13th is ""
    /// <summary>
    /// Gets or sets a one-dimensional array of type <see cref="String"/> containing the culture-specific full names of the months.
    /// </summary>
    /// <value>
    /// A one-dimensional array of type <see cref="String"/> containing the culture-specific full names of the months.
    /// In a 12-month calendar, the 13th element of the array is an empty string.
    /// This property corresponds to <see cref="DateTimeFormatInfo.MonthNames">DateTimeFormatInfo.MonthNames</see>.
    /// <note>Unlike <see cref="DateTimeFormatInfo.MonthNames">DateTimeFormatInfo.MonthNames</see>, 
    /// this property allows you to set it to an array of length 12. 
    /// However, when getting this value, it always returns an array of length 13.</note>
    /// </value>
    /// <remarks>
    /// This property gets modified when <see cref="DateTimeFormat"/> is set.
    /// </remarks>
    /// <seealso cref="AbbreviatedDayNames" />
    /// <seealso cref="AbbreviatedMonthNames" />
    /// <seealso cref="AMDesignator" />
    /// <seealso cref="DayNames" />
    /// <seealso cref="PMDesignator" />
    [TypeConverter(typeof(System.Web.UI.WebControls.StringArrayConverter))]
    public string[] MonthNames
    {
      get
      {
        return _monthNames;
      }
      set
      {
        if (value == null)
          throw new System.ArgumentNullException("MonthNames must not be null");
        if (!((value.Rank==1) && (value.Length==12 || value.Length==13)))
          throw new System.ArgumentException("MonthNames must be a one-dimensional string array of length 12 or 13");
        value.CopyTo(_monthNames, 0);
        _monthNames[12] = "";
      }
    }

    /// <summary>
    /// Gets or sets the amount of space between the contents of a month and the month's border.
    /// </summary>
    /// <value>
    /// The amount of space (in pixels) between the contents of a month and the month's border. The default value is <b>2</b>.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified month padding is less than -1.</exception>
    /// <remarks>
    /// Use this property to control the spacing between the contents of a month and the month's border.
    /// The padding amount specified is added to all four sides of a month.
    /// <note>Setting this property to <b>-1</b> indicates that this property is not set in the rendered Calendar.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public int MonthPadding
    {
      get
      {
        return Utils.ParseInt(ViewState["MonthPadding"], 2);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.MonthPadding may only be set to -1 or greater.", this.ID));
        }
        ViewState["MonthPadding"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the number of month rows displayed in the Calendar.
    /// </summary>
    /// <value>
    /// The number of rows of months displayed in the <see cref="Calendar"/> control. The default value is <b>1</b>.
    /// </value>
    /// <remarks>
    /// The value of <b>MonthRows</b> must be at least 1 and no less than the value of <see cref="VisibleMonthRow"/> property.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified number of rows is less than 1 or less than
    /// the value of the <b>VisibleMonthRow</b> property.</exception>
    public int MonthRows
    {
      get
      {
        return Utils.ParseInt(ViewState["MonthRows"], 1);
      }
      set
      {
        if (value < 1)
        {
          throw new Exception(String.Format("{0}.MonthRows must be set to 1 or greater.", this.ID));
        }
        ViewState["MonthRows"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the amount of space between months in the Calendar.
    /// </summary>
    /// <value>
    /// The amount of space (in pixels) between months in the Calendar control. The default value is <b>0</b>.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified month spacing is less than -1.</exception>
    /// <remarks>
    /// Use this property to control the spacing between individual months in the calendar. 
    /// This spacing is applied both vertically and horizontally.
    /// <note>Setting this property to <b>-1</b> indicates that this property is not set in the rendered Calendar.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public int MonthSpacing
    {
      get
      {
        return Utils.ParseInt(ViewState["MonthSpacing"], 0);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.MonthSpacing may only be set to -1 or greater.", this.ID));
        }
        ViewState["MonthSpacing"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class of the title heading for month areas of the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// The <b>MonthTitleCssClass</b> property only applies when the <see cref="ShowMonthTitle"/> property is set to <b>true</b>.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string MonthTitleCssClass
    {
      get
      {
        return (string)ViewState["MonthTitleCssClass"];
      }
      set
      {
        ViewState["MonthTitleCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the height of the image displayed in the next navigation control.
    /// </summary>
    /// <value>
    /// The height of the image displayed in the next navigation control, measured in pixels.
    /// The default value is <b>-1</b>, indicating that the height is not set.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified image height is less than -1.</exception>
    /// <remarks>
    /// This property only applies if the <see cref="ShowNextPrev"/> property is set to true 
    /// and <see cref="NextImageUrl"/> property is not set to null.
    /// <note>Setting this property to <b>-1</b> indicates that next image height is not set in the rendered Calendar.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public int NextImageHeight
    {
      get 
      {
        return Utils.ParseInt(ViewState["NextImageHeight"], -1);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.NextImageHeight may only be set to -1 or greater.", this.ID));
        }
        ViewState["NextImageHeight"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the image displayed in the next navigation control.
    /// </summary>
    /// <value>
    /// The URL of the image displayed in the next navigation control.  The default value is <b>null</b>, 
    /// which indicates to show the text specified in <see cref="NextText"/> property instead of an image.
    /// </value>
    /// <remarks>
    /// This property only applies if the <see cref="ShowNextPrev"/> property is set to true.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string NextImageUrl
    {
      get
      {
        return (string)ViewState["NextImageUrl"];
      }
      set
      {
        ViewState["NextImageUrl"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the width of the image displayed in the next navigation control.
    /// </summary>
    /// <value>
    /// The width of the image displayed in the next navigation control, measured in pixels.
    /// The default value is <b>-1</b>, indicating that the width is not set.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified image width is less than -1.</exception>
    /// <remarks>
    /// This property only applies if the <see cref="ShowNextPrev"/> property is set to true 
    /// and <see cref="NextImageUrl"/> property is not set to null.
    /// <note>Setting this property to <b>-1</b> indicates that next image width is not set in the rendered Calendar.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public int NextImageWidth
    {
      get 
      {
        return Utils.ParseInt(ViewState["NextImageWidth"], -1);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(string.Format("{0}.NextImageWidth may only be set to -1 or greater.", this.ID));
        }
        ViewState["NextImageWidth"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the text displayed for the next month navigation control.
    /// </summary>
    /// <value>
    /// The caption text for the next month navigation control.
    /// The default value is "&amp;&amp;raquo;", which renders as "&amp;raquo;".
    /// </value>
    /// <remarks>
    /// This property only applies if the <see cref="ShowNextPrev"/> property is set to true.
    /// <note>If <see cref="NextImageUrl"/> property is set to null, this property is output as text.
    /// Otherwise, this property is output as text alternative to the graphic (<i>alt</i> attribute of <i>img</i> tag).</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string NextText
    {
      get
      {
        return ViewState["NextText"]==null ? "&raquo;" : (string)ViewState["NextText"];
      }
      set
      {
        ViewState["NextText"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the next and previous month navigation elements.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ShowNextPrev"/> property is set to <b>true</b>.
    /// If the <b>NextPrevActiveCssClass</b> property is not set, the CSS class specified in the <see cref="NextPrevHoverCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string NextPrevActiveCssClass
    {
      get
      {
        return (string)ViewState["NextPrevActiveCssClass"];
      }
      set
      {
        ViewState["NextPrevActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the next and previous month navigation elements.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ShowNextPrev"/> property is set to <b>true</b>.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string NextPrevCssClass
    {
      get
      {
        return (string)ViewState["NextPrevCssClass"];
      }
      set
      {
        ViewState["NextPrevCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the next and previous month navigation elements.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ShowNextPrev"/> property is set to <b>true</b>.
    /// If the <b>NextPrevHoverCssClass</b> property is not set, the CSS class specified in the <see cref="NextPrevCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string NextPrevHoverCssClass
    {
      get
      {
        return (string)ViewState["NextPrevHoverCssClass"];
      }
      set
      {
        ViewState["NextPrevHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the days that are not in the displayed month.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for the days that are not in the displayed month of the 
    /// <see cref="Calendar"/> control.
    /// If the <b>OtherMonthDayActiveCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="OtherMonthDayHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string OtherMonthDayActiveCssClass
    {
      get
      {
        return (string)ViewState["OtherMonthDayActiveCssClass"];
      }
      set
      {
        ViewState["OtherMonthDayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the days that are not in the displayed month.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for the days that are not in the displayed month of the 
    /// <see cref="Calendar"/> control.
    /// If the <b>OtherMonthDayCssClass</b> property is not set, the CSS class specified in the <see cref="DayCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string OtherMonthDayCssClass
    {
      get
      {
        return (string)ViewState["OtherMonthDayCssClass"];
      }
      set
      {
        ViewState["OtherMonthDayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the days that are not in the displayed month.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for the days that are not in the displayed month of the 
    /// <see cref="Calendar"/> control.
    /// If the <b>OtherMonthDayHoverCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="OtherMonthDayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string OtherMonthDayHoverCssClass
    {
      get
      {
        return (string)ViewState["OtherMonthDayHoverCssClass"];
      }
      set
      {
        ViewState["OtherMonthDayHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the days that are out of range.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for the days that are less than <see cref="MinDate"/> or 
    /// greater than <see cref="MaxDate"/> in the <see cref="Calendar"/> control.
    /// If the <b>OutOfRangeDayActiveCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="OutOfRangeDayHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string OutOfRangeDayActiveCssClass
    {
      get
      {
        return (string)ViewState["OutOfRangeDayActiveCssClass"];
      }
      set
      {
        ViewState["OutOfRangeDayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the days that are out of range.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for the days that are less than <see cref="MinDate"/> or 
    /// greater than <see cref="MaxDate"/> in the <see cref="Calendar"/> control.
    /// If the <b>OutOfRangeDayCssClass</b> property is not set, the CSS class specified in the <see cref="DayCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string OutOfRangeDayCssClass
    {
      get
      {
        return (string)ViewState["OutOfRangeDayCssClass"];
      }
      set
      {
        ViewState["OutOfRangeDayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the days that are out of range.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for the days that are less than <see cref="MinDate"/> or 
    /// greater than <see cref="MaxDate"/> in the <see cref="Calendar"/> control.
    /// If the <b>OutOfRangeDayHoverCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="OutOfRangeDayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string OutOfRangeDayHoverCssClass
    {
      get
      {
        return (string)ViewState["OutOfRangeDayHoverCssClass"];
      }
      set
      {
        ViewState["OutOfRangeDayHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the picker portion of the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Picker</b>.
    /// </remarks>
    public string PickerCssClass
    {
      get
      {
        return (string)ViewState["PickerCssClass"];
      }
      set
      {
        ViewState["PickerCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the custom date format string.
    /// </summary>
    /// <value>A string that represents the custom date format.</value>
    /// <remarks>
    /// To display string literals that contain date format strings you must escape the substring. 
    /// For example, to display the date as "05 de Mayo" (with <b>MonthNames</b> set to Spanish words like "Mayo"), 
    /// set the <b>PickerCustomFormat</b> property to "dd 'de' MMMM". If the "de" substring is not escaped, the result 
    /// is "05 5e Mayo" because the "d" character is read as the one-letter day format string (see the format 
    /// string table below).
    /// <para>To show a single quote (') character within an escaped substring, enter it twice (''). 
    /// You can not show a single quote character outside of an escaped substring.</para>
    /// <note>The <see cref="PickerFormat" /> property must be set to <b>Custom</b> for 
    /// <b>PickerCustomFormat</b> to affect the formatting of the displayed date.</note>
    /// The following table lists all the valid format strings and their descriptions:
    /// <list type="table">
    /// <listheader><term>Format String</term><description>Description</description></listheader>
    /// <item><term>d</term><description>The one or two-digit day.</description></item>
    /// <item><term>dd</term><description>The two-digit day. Single digit day values are preceded by a zero.</description></item>
    /// <item><term>ddd</term><description>The day-of-week abbreviation.</description></item>
    /// <item><term>dddd</term><description>The full day-of-week name.</description></item>
    /// <item><term>h</term><description>The one or two-digit hour in 12-hour format.</description></item>
    /// <item><term>hh</term>
    ///   <description>The two-digit hour in 12-hour format. Single digit values are preceded by a zero.</description></item>
    /// <item><term>H</term><description>The one or two-digit hour in 24-hour format.</description></item>
    /// <item><term>HH</term>
    ///   <description>The two-digit hour in 24-hour format. Single digit values are preceded by a zero.</description></item>
    /// <item><term>M</term><description>The one or two-digit month number.</description></item>
    /// <item><term>MM</term>
    ///   <description>The two-digit month number. Single digit values are preceded by a zero.</description></item>
    /// <item><term>MMM</term><description>The month abbreviation.</description></item>
    /// <item><term>MMMM</term><description>The full month name.</description></item>
    /// <item><term>s</term><description>The one or two-digit seconds.</description></item>
    /// <item><term>ss</term><description>The two-digit seconds. Single digit values are preceded by a zero.</description></item>
    /// <item><term>t</term><description>The one-letter AM/PM abbreviation in lower case ("AM" is displayed as "a").</description></item>
    /// <item><term>tt</term><description>The two-letter AM/PM abbreviation in lower case ("AM" is displayed as "am").</description></item>
    /// <item><term>T</term><description>The one-letter AM/PM abbreviation in upper case ("AM" is displayed as "A").</description></item>
    /// <item><term>TT</term><description>The two-letter AM/PM abbreviation in upper case ("AM" is displayed as "AM").</description></item>
    /// <item><term>y</term><description>The one-digit year (2001 is displayed as "1").</description></item>
    /// <item><term>yy</term><description>The last two digits of the year (2001 is displayed as "01").</description></item>
    /// <item><term>yyy</term><description>Same as yyyy</description></item>
    /// <item><term>yyyy</term><description>The full year (2001 is displayed as "2001").</description></item>
    /// </list>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Picker</b>.</para>
    /// </remarks>
    /// <example>
    /// The following example sets the <b>PickerCustomFormat</b> property so that the <see cref="Calendar"/> 
    /// will display the date as "June 01, 2001 - Friday". This code assumes that an instance of a 
    /// <b>Calendar</b> control named <c>MyCalendar</c> has been created on the page.
    /// <code>
    /// <![CDATA[
    /// void Page_Load()
    /// {
    ///	  MyCalendar.PickerFormat = PickerFormat.Custom;
    ///	  MyCalendar.PickerCustomFormat = "MMMM dd, yyyy - dddd";
    ///	}
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="PickerFormat" />
    public string PickerCustomFormat
    {
      get
      {
        return (string)ViewState["PickerCustomFormat"];
      }
      set
      {
        ViewState["PickerCustomFormat"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the format of date/time displayed by the picker.
    /// </summary>
    /// <value>One of the <see cref="DateTimeFormatType" /> values. The default is 
    /// <b>Long</b>.</value>
    /// <remarks>
    /// This property determines the date/time format of the picker's display. 
    /// The resulting format is based on <see cref="Calendar"/>'s <see cref="DateTimeFormat"/> property.
    /// <note><b>PickerFormat</b> property must be set to <b>Custom</b> for the 
    /// <see cref="PickerCustomFormat" /> property to affect the formatting of the displayed date.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Picker</b>.</para>
    /// </remarks>
    public DateTimeFormatType PickerFormat
    {
      get
      {
        return Utils.ParseDateTimeFormatType(ViewState["PickerFormat"], DateTimeFormatType.Long);
      }
      set
      {
        ViewState["PickerFormat"] = value;
      }
    }

    private string _pmDesignator = null;
    /// <summary>
    /// Gets or sets the string designator for hours that are "post meridiem" (after noon).
    /// </summary>
    /// <value>
    /// The string designator for hours that are "post meridiem" (after noon).
    /// This property corresponds to <see cref="DateTimeFormatInfo.PMDesignator">DateTimeFormatInfo.PMDesignator</see>
    /// </value>
    /// <remarks>
    /// This property gets modified when <see cref="DateTimeFormat"/> is set.
    /// </remarks>
    /// <seealso cref="AbbreviatedDayNames" />
    /// <seealso cref="AbbreviatedMonthNames" />
    /// <seealso cref="AMDesignator" />
    /// <seealso cref="DayNames" />
    /// <seealso cref="MonthNames" />
    public string PMDesignator
    {
      get
      {
        return _pmDesignator;
      }
      set
      {
        _pmDesignator = value;
      }
    }

    /// <summary>
    /// Determines how the pop-up calendar is triggered.
    /// </summary>
    /// <value>Gets or sets a <see cref="CalendarPopUpType"/> value indicating the type of pop-up calendar to render.
    /// The default value is <b>CalendarPopUpType.None</b>, indicating that this will not be a pop-up calendar.</value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    public CalendarPopUpType PopUp
    {
      get
      {
        return Utils.ParseCalendarPopUpType(ViewState["PopUp"], CalendarPopUpType.None);
      }
      set
      {
        ViewState["PopUp"] = value;
      }
    }

    /// <summary>
    /// The duration of the calendar pop-up collapse animation, in milliseconds.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The duration of the calendar pop-up animation, in milliseconds.")]
    [DefaultValue(200)]
    public int PopUpCollapseDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["PopUpCollapseDuration"], 200);
      }
      set 
      {
        ViewState["PopUpCollapseDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the calendar pop-up collapse animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The slide type to use for the calendar pop-up animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    public SlideType PopUpCollapseSlide
    {
      get 
      {
        return Utils.ParseSlideType(ViewState["PopUpCollapseSlide"], SlideType.ExponentialDecelerate);
      }
      set 
      {
        ViewState["PopUpCollapseSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the calendar pop-up collapse animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The transition effect to use for the calendar pop-up animation.")]
    [DefaultValue(TransitionType.None)]
    public TransitionType PopUpCollapseTransition
    {
      get 
      {
        return Utils.ParseTransitionType(ViewState["PopUpCollapseTransition"]);
      }
      set 
      {
        ViewState["PopUpCollapseTransition"] = value;
      }
    }

    /// <summary>
    /// The custom transition filter to use for the calendar pop-up collapse animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The custom transition filter to use for the calendar pop-up animation.")]
    [DefaultValue(null)]
    public string PopUpCollapseTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["PopUpCollapseTransitionCustomFilter"];
      }
      set
      {
        ViewState["PopUpCollapseTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// Client-side ID of the element to which this pop-up calendar is aligned.
    /// </summary>
    /// <value>
    /// Gets or sets the client-side ID of the HTML DOM element to which this pop-up calendar is aligned.
    /// Default value is <b>null</b>, indicating that the calendar will not be automatically aligned to any page elements.
    /// In this case, the coordinates where the pop-up expands must be supplied on the client-side.
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// <note>
    /// The specified HTML element does not automatically trigger pop-up of the Calendar in any way.
    /// Instead, it is just used by the calendar to determine the position where to position itself.
    /// </note>
    /// </remarks>
    [Category("Behavior")]
    [Description("Client-side ID of the element to which this pop-up calendar is aligned.")]
    public string PopUpExpandControlId
    {
      get
      {
        return (string)ViewState["PopUpExpandControlId"];
      }
      set
      {
        ViewState["PopUpExpandControlId"] = value;
      }
    }

    /// <summary>
    /// Direction in which the pop-up Calendar expands.
    /// </summary>
    /// <value>
    /// Gets or sets the a  direction in which the pop-up Calendar expands.  Default is <b>CalendarPopUpExpandDirection.BelowRight</b>
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Behavior")]
    [Description("Client-side ID of the element to which this pop-up calendar is aligned.")]
    public CalendarPopUpExpandDirection PopUpExpandDirection
    {
      get
      {
        return Utils.ParseCalendarPopUpExpandDirection(ViewState["PopUpExpandDirection"], CalendarPopUpExpandDirection.BelowRight);
      }
      set
      {
        ViewState["PopUpExpandDirection"] = value;
      }
    }

    /// <summary>
    /// The duration of the calendar pop-up expand animation, in milliseconds.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The duration of the calendar pop-up animation, in milliseconds.")]
    [DefaultValue(200)]
    public int PopUpExpandDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["PopUpExpandDuration"], 200);
      }
      set 
      {
        ViewState["PopUpExpandDuration"] = value;
      }
    }

    /// <summary>
    /// Offset along x-axis from pop-up calendar's normal expand position.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along x-axis from pop-up calendar's normal expand position.")]
    public int PopUpExpandOffsetX
    {
      get 
      {
        return Utils.ParseInt(ViewState["PopUpExpandOffsetX"], 0);
      }
      set 
      {
        ViewState["PopUpExpandOffsetX"] = value;
      }
    }

    /// <summary>
    /// Offset along y-axis from pop-up calendar's normal expand position.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along y-axis from pop-up calendar's normal expand position.")]
    public int PopUpExpandOffsetY
    {
      get 
      {
        return Utils.ParseInt(ViewState["PopUpExpandOffsetY"], 0);
      }
      set 
      {
        ViewState["PopUpExpandOffsetY"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the calendar pop-up expand animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The slide type to use for the calendar pop-up animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    public SlideType PopUpExpandSlide
    {
      get 
      {
        return Utils.ParseSlideType(ViewState["PopUpExpandSlide"], SlideType.ExponentialDecelerate);
      }
      set 
      {
        ViewState["PopUpExpandSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the calendar pop-up expand animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The transition effect to use for the calendar pop-up animation.")]
    [DefaultValue(TransitionType.None)]
    public TransitionType PopUpExpandTransition
    {
      get 
      {
        return Utils.ParseTransitionType(ViewState["PopUpExpandTransition"]);
      }
      set 
      {
        ViewState["PopUpExpandTransition"] = value;
      }
    }

    /// <summary>
    /// The custom transition filter to use for the calendar pop-up expand animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The custom transition filter to use for the calendar pop-up animation.")]
    [DefaultValue(null)]
    public string PopUpExpandTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["PopUpExpandTransitionCustomFilter"];
      }
      set
      {
        ViewState["PopUpExpandTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// Whether a pop-up calendar has a drop shadow.  Default is true.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// <para>Shadows always point in the direction of the bottom right.</para>
    /// <para>Shadows are only available in Internet Explorer 6+ on Windows.</para>
    /// </remarks>
    [DefaultValue(true)]
    [Description("Whether a pop-up calendar has a drop shadow.")]
    public bool PopUpShadowEnabled
    {
      get
      {
        return Utils.ParseBool(ViewState["PopUpShadowEnabled"], true);
      }
      set
      {
        ViewState["PopUpShadowEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// zIndex of the Calendar pop-up.  Default is 1000.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="PopUp"/> property is not set to <b>CalendarPopUpType.None</b>.
    /// </remarks>
    [Category("Layout")]
    [DefaultValue(1000)]
    [Description("zIndex of the Calendar pop-up.  Default is 1000.")]
    public int PopUpZIndex
    {
      get
      {
        return Utils.ParseInt(ViewState["PopUpZIndex"], 1000);
      }
      set
      {
        ViewState["PopUpZIndex"] = value.ToString();
      }
    }

    /// <summary>
    /// This value is the increment by which the time value is able to change.  Default value <b>TimeSpan.Zero</b> indicates that all values are allowed.
    /// </summary>
    /// <remarks>
    /// <para>If the value of this field is a non-zero TimeSpan, it indicates the precision at which the Picker operates.</para>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Picker</b>.</para>
    /// </remarks>
    /// <example>If the value of Precision is 15 minutes, the minute field will only accept 00, 15, 30, and 45.</example>
    [DefaultValue(typeof(TimeSpan), "0")]
    public TimeSpan Precision
    {
      get
      {
        return Utils.ParseTimeSpan(ViewState["Precision"]);
      }
      set
      {
        ViewState["Precision"] = value.ToString();
        this.AdjustSelectedDate();
      }
    }

    /// <summary>
    /// Gets or sets the height of the image displayed in the previous navigation control.
    /// </summary>
    /// <value>
    /// The height of the image displayed in the previous navigation control, measured in pixels.
    /// The default value is <b>-1</b>, indicating that the height is not set.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified image height is less than -1.</exception>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="ShowNextPrev"/> property is set to true 
    /// and <see cref="PrevImageUrl"/> property is not set to null.
    /// <note>Setting this property to <b>-1</b> indicates that previous image height is not set in the rendered Calendar.</note>
    /// </remarks>
    public int PrevImageHeight
    {
      get 
      {
        return Utils.ParseInt(ViewState["PrevImageHeight"], -1);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.PrevImageHeight may only be set to -1 or greater.", this.ID));
        }
        ViewState["PrevImageHeight"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the image displayed in the previous navigation control.
    /// </summary>
    /// <value>
    /// The URL of the image displayed in the previous navigation control.  The default value is <b>null</b>, 
    /// which indicates to show the text specified in <see cref="PrevText"/> property instead of an image.
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="ShowNextPrev"/> property is set to true.
    /// This property only applies if the <see cref="ShowNextPrev"/> property is set to true.
    /// </remarks>
    public string PrevImageUrl
    {
      get
      {
        return (string)ViewState["PrevImageUrl"];
      }
      set
      {
        ViewState["PrevImageUrl"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the width of the image displayed in the previous navigation control.
    /// </summary>
    /// <value>
    /// The width of the image displayed in the previous navigation control, measured in pixels.
    /// The default value is <b>-1</b>, indicating that the width is not set.
    /// </value>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified image width is less than -1.</exception>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="ShowNextPrev"/> property is set to true 
    /// and <see cref="PrevImageUrl"/> property is not set to null.
    /// <note>Setting this property to <b>-1</b> indicates that previous image width is not set in the rendered Calendar.</note>
    /// </remarks>
    public int PrevImageWidth
    {
      get 
      {
        return Utils.ParseInt(ViewState["PrevImageWidth"], -1);
      }
      set
      {
        if (value < -1)
        {
          throw new Exception(String.Format("{0}.PrevImageWidth may only be set to -1 or greater.", this.ID));
        }
        ViewState["PrevImageWidth"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the text displayed for the previous navigation control.
    /// </summary>
    /// <value>
    /// The caption text for the previous month navigation control.
    /// The default value is "&amp;&amp;laquo;", which renders as "&amp;laquo;".
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="ShowNextPrev"/> property is set to true.
    /// <note>If <see cref="NextImageUrl"/> property is set to null, this property is output as text.
    /// Otherwise, this property is output as text alternative to the graphic (<i>alt</i> attribute of <i>img</i> tag).</note>
    /// </remarks>
    public string PrevText
    {
      get
      {
        return ViewState["PrevText"]==null ? "&laquo;" : (string)ViewState["PrevText"];
      }
      set
      {
        ViewState["PrevText"] = value;
      }
    }

    /// <summary>
    /// Whether the calendar should fire events and perform postbacks when the selected date is clicked. Default is false.
    /// </summary>
    /// <remarks>
    /// Determines the behaviour when the user clicks on the Calendar selecting a date (or dates), but when this action doesn't alter the selection.
    /// For example, when the selection is December 25th, and the user clicks December 25th.  If ReactOnSameSelection is false, the click is ignored.
    /// If ReactOnSameSelection is true, the click is acted on: the client-side SelectionChanged event is raised, and, if applicable, 
    /// a postback is performed.
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether the calendar should fire events and perform postbacks when the selected date is clicked. Default is false.")]
    public bool ReactOnSameSelection
    {
      get
      {
        return Utils.ParseBool(ViewState["ReactOnSameSelection"], false);
      }
      set
      {
        ViewState["ReactOnSameSelection"] = value.ToString();
      }
    }

    /// <summary>
    /// Gets or sets the selected date.
    /// </summary>
    /// <value>
    /// A <see cref="System.DateTime"/> object that represents the selected date.
    /// The default value is <see cref="DateTime.MinValue">DateTime.MinValue</see>, which indicates that no date is selected.
    /// </value>
    /// <remarks>
    /// The <b>SelectedDate</b> property and the <see cref="SelectedDates"/> collection are closely related.
    /// When <see cref="AllowMultipleSelection"/>, <see cref="AllowWeekSelection"/>, and <see cref="AllowMonthSelection"/> 
    /// properties are all set to <b>false</b>, no more than one date can be selected, <b>SelectedDate</b> and 
    /// <b>SelectedDates[0]</b> have the same value and <b>SelectedDates.Count</b> equals 1.  When multiple dates can be 
    /// selected, <b>SelectedDate</b> and <b>SelectedDates[0]</b> have the same value.
    /// <note>
    /// Setting the <b>SelectedDate</b> property has the side effect of setting the <b>SelectedDates</b> collection to a 
    /// one-element collection containing the <b>SelectedDate</b>.
    /// </note>
    /// <note>
    /// There is one special case.  A <b>SelectedDate</b> of <see cref="DateTime.MinValue">DateTime.MinValue</see> indicates 
    /// that <b>SelectedDates</b> collection is empty.  Setting the <b>SelectedDate</b> property to 
    /// <see cref="DateTime.MinValue">DateTime.MinValue</see> has a side effect of clearing the <b>SelectedDates</b> collection.
    /// </note>
    /// </remarks>
    public DateTime SelectedDate
    {
      get
      {
        if (this.SelectedDates.Count == 0)
        {
          return DateTime.MinValue;
        }
        else
        {
          return this.SelectedDates[0];
        }
      }
      set
      {
        this.SelectedDates.Clear();
        if (value != DateTime.MinValue)
        {
          this.SelectedDates.Add(value);
        }
        this.AdjustSelectedDate();
      }
    }

    /// <summary>
    /// Gets a collection of <see cref="System.DateTime"/> objects that represent the selected dates on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// The <see cref="SelectedDate"/> property and the <b>SelectedDates</b> collection are closely related.
    /// When <see cref="AllowMultipleSelection"/>, <see cref="AllowWeekSelection"/>, and <see cref="AllowMonthSelection"/> 
    /// properties are all set to <b>false</b>, no more than one date can be selected, <b>SelectedDate</b> and 
    /// <b>SelectedDates[0]</b> have the same value and <b>SelectedDates.Count</b> equals 1.  When multiple dates can be 
    /// selected, <b>SelectedDate</b> and <b>SelectedDates[0]</b> have the same value.
    /// <note>
    /// Setting the <b>SelectedDates</b> collection has the side effect of setting the <b>SelectedDate</b> property to the
    /// first element of the collection.
    /// </note>
    /// <note>
    /// There is one special case.  A <b>SelectedDate</b> of <see cref="DateTime.MinValue">DateTime.MinValue</see> indicates that 
    /// <b>SelectedDates</b> collection is empty.  Clearing the <b>SelectedDates</b> collection has the side effect of setting the 
    /// <b>SelectedDate</b> property to <see cref="DateTime.MinValue">DateTime.MinValue</see>.
    /// </note>
    /// <p>You can also use the <b>SelectedDates</b> collection to programmatically select dates on the <b>Calendar</b> control. 
    /// Use the <see cref="DateCollection.Add">Add</see>, <see cref="DateCollection.Remove">Remove</see>, 
    /// <see cref="DateCollection.Clear">Clear</see>, and <see cref="DateCollection.SelectRange">SelectRange</see> 
    /// methods to programmatically manipulate the selected dates in the <b>SelectedDates</b> collection.</p>
    /// <note>
    /// This property is most relevant when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// If the <see cref="ControlType"/> property is set to <b>CalendarControlType.Picker</b>, <see cref="VisibleDate"/> property
    /// is of more interest.
    /// </note>
    /// </remarks>
    public DateCollection SelectedDates
    {
      get
      {
        if (this._selectedDatesCollection == null)
        {
          this._selectedDatesCollection = new DateCollection(this._selectedDatesList, this);
        }
        return this._selectedDatesCollection;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the selected dates.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for the selected dates on the <see cref="Calendar"/> control.  
    /// If the <b>SelectedDayActiveCssClass</b> property is not set, the CSS class specified in the 
    /// <see cref="SelectedDayHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b></para>
    /// </remarks>
    public string SelectedDayActiveCssClass
    {
      get
      {
        return (string)ViewState["SelectedDayActiveCssClass"];
      }
      set
      {
        ViewState["SelectedDayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the selected dates.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for the selected dates on the <see cref="Calendar"/> control.  
    /// If the <b>SelectedDayCssClass</b> property is not set, the CSS class specified in the <see cref="DayCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b></para>
    /// </remarks>
    public string SelectedDayCssClass
    {
      get
      {
        return (string)ViewState["SelectedDayCssClass"];
      }
      set
      {
        ViewState["SelectedDayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the selected dates.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for the selected dates on the <see cref="Calendar"/> control.  
    /// If the <b>SelectedDayHoverCssClass</b> property is not set, the CSS class specified in the <see cref="SelectedDayCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b></para>
    /// </remarks>
    public string SelectedDayHoverCssClass
    {
      get
      {
        return (string)ViewState["SelectedDayHoverCssClass"];
      }
      set
      {
        ViewState["SelectedDayHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the month selector.
    /// </summary>
    /// <remarks>
    /// If the <b>SelectMonthActiveCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="SelectMonthHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowMonthSelection"/> property is set to true.</para>
    /// </remarks>
    public string SelectMonthActiveCssClass
    {
      get
      {
        return (string)ViewState["SelectMonthActiveCssClass"];
      }
      set
      {
        ViewState["SelectMonthActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the month selector.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowMonthSelection"/> property is set to true.
    /// </remarks>
    public string SelectMonthCssClass
    {
      get
      {
        return (string)ViewState["SelectMonthCssClass"];
      }
      set
      {
        ViewState["SelectMonthCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the month selector.
    /// </summary>
    /// <remarks>
    /// If the <b>SelectMonthHoverCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="SelectMonthCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowMonthSelection"/> property is set to true.</para>
    /// </remarks>
    public string SelectMonthHoverCssClass
    {
      get
      {
        return (string)ViewState["SelectMonthHoverCssClass"];
      }
      set
      {
        ViewState["SelectMonthHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the text displayed for the month selector.
    /// </summary>
    /// <value>
    /// The text displayed for the month selection element in the selector column.
    /// The default value is <b>"&gt;&gt;"</b>, which renders as two greater than signs (>>).
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowMonthSelection"/> property is set to true.
    /// </remarks>
    public string SelectMonthText
    {
      get
      {
        return ViewState["SelectMonthText"]==null ? "&gt;&gt;" : (string)ViewState["SelectMonthText"];
      }
      set
      {
        ViewState["SelectMonthText"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the week selector.
    /// </summary>
    /// <remarks>
    /// If the <b>SelectWeekActiveCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="SelectWeekHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowWeekSelection"/> or <see cref="ShowWeekNumbers" /> property is set to true.</para>
    /// </remarks>
    public string SelectWeekActiveCssClass
    {
      get
      {
        return (string)ViewState["SelectWeekActiveCssClass"];
      }
      set
      {
        ViewState["SelectWeekActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the week selector.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowWeekSelection"/> or <see cref="ShowWeekNumbers" /> property is set to true.
    /// </remarks>
    public string SelectWeekCssClass
    {
      get
      {
        return (string)ViewState["SelectWeekCssClass"];
      }
      set
      {
        ViewState["SelectWeekCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the week selector.
    /// </summary>
    /// <remarks>
    /// If the <b>SelectWeekHoverCssClass</b> property is not set, the CSS class specified in the
    /// <see cref="SelectWeekCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and the <see cref="AllowWeekSelection"/> or <see cref="ShowWeekNumbers" /> property is set to true.
    /// </remarks>
    public string SelectWeekHoverCssClass
    {
      get
      {
        return (string)ViewState["SelectWeekHoverCssClass"];
      }
      set
      {
        ViewState["SelectWeekHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the text displayed for the week selector.
    /// </summary>
    /// <value>
    /// The text displayed for the week selection element in the selector column.
    /// The default value is <b>"&gt;&gt;"</b>, which renders as two greater than signs (>>).
    /// </value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>,
    /// the <see cref="AllowWeekSelection"/> property is set to true, 
    /// and the <see cref="ShowWeekNumbers"/> property is set to false.
    /// </remarks>
    public string SelectWeekText
    {
      get
      {
        return ViewState["SelectWeekText"]==null ? "&gt;&gt;" : (string)ViewState["SelectWeekText"];
      }
      set
      {
        ViewState["SelectWeekText"] = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="ShowTitle"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ShowTitle instead.")]
    [Obsolete("Deprecated.  Use ShowTitle instead.", false)]
    public bool ShowCalendarTitle 
    {
      get
      {
        return ShowTitle;
      }
      set
      {
        ShowTitle = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the heading for the days of the week is displayed.
    /// </summary>
    /// <value>
    /// <b>true</b> if the heading for the days of the week is displayed; otherwise, <b>false</b>. The default is <b>true</b>.
    /// </value>
    /// <remarks>
    /// The appearance of the heading can be customized by using the <see cref="DayHeaderCssClass"/> property.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public bool ShowDayHeader
    {
      get
      {
        return Utils.ParseBool(ViewState["ShowDayHeader"], true);
      }
      set
      {
        ViewState["ShowDayHeader"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the days on the <see cref="Calendar"/> control are separated with grid lines.
    /// </summary>
    /// <value>
    /// <b>true</b> if the days on the <see cref="Calendar"/> control are separated with grid lines; otherwise, <b>false</b>. 
    /// The default value is <b>false</b>.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </value>
    public bool ShowGridLines 
    {
      get
      {
        return Utils.ParseBool(ViewState["ShowGridLines"], false);
      }
      set
      {
        ViewState["ShowGridLines"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the month title section is displayed.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control displays the title section for months; otherwise, <b>false</b>. 
    /// The default value is <b>false</b>.
    /// </value>
    /// <remarks>
    /// The appearance of the month title section can be customized by using the <see cref="MonthTitleCssClass"/> property.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public bool ShowMonthTitle 
    {
      get
      {
        return Utils.ParseBool(ViewState["ShowMonthTitle"], false);
      }
      set
      {
        ViewState["ShowMonthTitle"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Calendar"/> control displays the next and previous month 
    /// navigation elements in the title section.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control displays the next and previous month navigation elements in the title 
    /// section; otherwise, <b>false</b>. The default value is <b>true</b>.
    /// </value>
    /// <remarks>
    /// The appearance of the next and previous month navigation controls can be customized 
    /// by using the <see cref="NextPrevCssClass"/> property.
    /// <note>Hiding the calendar title section by setting <see cref="ShowTitle"/> property to <b>false</b> also hides 
    /// the next and previous month navigation controls.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public bool ShowNextPrev 
    {
      get
      {
        return Utils.ParseBool(ViewState["ShowNextPrev"], true);
      }
      set
      {
        ViewState["ShowNextPrev"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the calendar title section is displayed.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control displays the title section; otherwise, <b>false</b>. 
    /// The default value is <b>true</b>.
    /// </value>
    /// <remarks>
    /// The appearance of the calendar title section can be customized by using the <see cref="TitleCssClass"/> property.
    /// <note>Hiding the calendar title section also hides the next and previous month navigation controls.</note>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public bool ShowTitle
    {
      get
      {
        return Utils.ParseBool(ViewState["ShowTitle"], true);
      }
      set
      {
        ViewState["ShowTitle"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the calendar should display week numbers.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control displays the week numbers; otherwise, <b>false</b>. 
    /// The default value is <b>false</b>.
    /// </value>
    /// <remarks>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    [Category("Appearance")]
    [Description("Gets or sets a value indicating whether the calendar should display week numbers. The default value is false")]
    [DefaultValue(false)]
    public bool ShowWeekNumbers
    {
      get
      {
        return Utils.ParseBool(ViewState["ShowWeekNumbers"], false);
      }
      set
      {
        ViewState["ShowWeekNumbers"] = value;
      }
    }

    /// <summary>
    /// The duration of the calendar swap animation, in milliseconds.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The duration of the calendar swap animation, in milliseconds.")]
    [DefaultValue(200)]
    public int SwapDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["SwapDuration"], 200);
      }
      set 
      {
        ViewState["SwapDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the calendar swap animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The slide type to use for the calendar swap animation.")]
    [DefaultValue(SlideType.Linear)]
    public SlideType SwapSlide
    {
      get 
      {
        return Utils.ParseSlideType(ViewState["SwapSlide"], SlideType.Linear);
      }
      set 
      {
        ViewState["SwapSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the calendar swap animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The transition effect to use for the calendar swap animation.")]
    [DefaultValue(TransitionType.None)]
    public TransitionType SwapTransition
    {
      get 
      {
        return Utils.ParseTransitionType(ViewState["SwapTransition"]);
      }
      set 
      {
        ViewState["SwapTransition"] = value;
      }
    }

    /// <summary>
    /// The custom transition filter to use for the calendar swap animation.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    [Category("Animation")]
    [Description("The custom transition filter to use for the calendar swap animation.")]
    [DefaultValue(null)]
    public string SwapTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["SwapTransitionCustomFilter"];
      }
      set
      {
        ViewState["SwapTransitionCustomFilter"] = value;
      }
    }

    private CalendarDayCustomTemplateCollection _templates;
    /// <summary>
    /// Custom templates which are referenced by <see cref="CustomDays"/> to implement customized renderings.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// </remarks>
    [Browsable(false)]
    [Description("Collection of template controls. ")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public CalendarDayCustomTemplateCollection Templates
    {
      get
      {
        if (_templates == null)
        {
          _templates = new CalendarDayCustomTemplateCollection();
        }
        return _templates;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class of the title heading for the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b> 
    /// and the <see cref="ShowTitle"/> property is set to <b>true</b>.
    /// </remarks>
    public string TitleCssClass
    {
      get
      {
        return (string)ViewState["TitleCssClass"];
      }
      set
      {
        ViewState["TitleCssClass"] = value;
      }
    }

    /// <summary>Gets or sets the date format string for date(s) shown in the calendar title.</summary>
    /// <value>A string that represents the custom date format for date(s) shown in the calendar title.
    /// Default is "MMMM yyyy", rendering as "February 2006" for example.</value>
    /// <remarks>
    /// To display string literals that contain date format strings you must escape the substring. 
    /// For example, to display the date as "05 de Mayo" (with <b>MonthNames</b> set to Spanish words like "Mayo"), 
    /// set the <b>TitleDateFormat</b> property to "dd 'de' MMMM". If the "de" substring is not escaped, the result 
    /// is "05 5e Mayo" because the "d" character is read as the one-letter day format string (see the format 
    /// string table below).
    /// <para>To show a single quote (') character within an escaped substring, enter it twice (''). 
    /// You can not show a single quote character outside of an escaped substring.</para>
    /// The following table lists all the valid format strings and their descriptions:
    /// <list type="table">
    /// <listheader><term>Format String</term><description>Description</description></listheader>
    /// <item><term>d</term><description>The one or two-digit day.</description></item>
    /// <item><term>dd</term><description>The two-digit day. Single digit day values are preceded by a zero.</description></item>
    /// <item><term>ddd</term><description>The day-of-week abbreviation.</description></item>
    /// <item><term>dddd</term><description>The full day-of-week name.</description></item>
    /// <item><term>h</term><description>The one or two-digit hour in 12-hour format.</description></item>
    /// <item><term>hh</term>
    ///   <description>The two-digit hour in 12-hour format. Single digit values are preceded by a zero.</description></item>
    /// <item><term>H</term><description>The one or two-digit hour in 24-hour format.</description></item>
    /// <item><term>HH</term>
    ///   <description>The two-digit hour in 24-hour format. Single digit values are preceded by a zero.</description></item>
    /// <item><term>M</term><description>The one or two-digit month number.</description></item>
    /// <item><term>MM</term>
    ///   <description>The two-digit month number. Single digit values are preceded by a zero.</description></item>
    /// <item><term>MMM</term><description>The month abbreviation.</description></item>
    /// <item><term>MMMM</term><description>The full month name.</description></item>
    /// <item><term>s</term><description>The one or two-digit seconds.</description></item>
    /// <item><term>ss</term><description>The two-digit seconds. Single digit values are preceded by a zero.</description></item>
    /// <item><term>t</term><description>The one-letter AM/PM abbreviation in lower case ("AM" is displayed as "a").</description></item>
    /// <item><term>tt</term><description>The two-letter AM/PM abbreviation in lower case ("AM" is displayed as "am").</description></item>
    /// <item><term>T</term><description>The one-letter AM/PM abbreviation in upper case ("AM" is displayed as "A").</description></item>
    /// <item><term>TT</term><description>The two-letter AM/PM abbreviation in upper case ("AM" is displayed as "AM").</description></item>
    /// <item><term>y</term><description>The one-digit year (2001 is displayed as "1").</description></item>
    /// <item><term>yy</term><description>The last two digits of the year (2001 is displayed as "01").</description></item>
    /// <item><term>yyy</term><description>Same as yyyy</description></item>
    /// <item><term>yyyy</term><description>The full year (2001 is displayed as "2001").</description></item>
    /// </list>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b> 
    /// and the <see cref="ShowTitle"/> property is set to <b>true</b>.</para>
    /// </remarks>
    public string TitleDateFormat
    {
      get
      {
        return ViewState["TitleDateFormat"] == null ? "MMMM yyyy" : (string)ViewState["TitleDateFormat"];
      }
      set
      {
        ViewState["TitleDateFormat"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the string that separates the to and from dates in the calendar title.
    /// </summary>
    /// <value>A string that separates the text of to and from dates when the calendar title shows a date range.
    /// Default value is "<b> - </b>".</value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b> 
    /// and the <see cref="ShowTitle"/> property is set to <b>true</b>
    /// and the <see cref="TitleType"/> is set to <b>CalendarTitleType.VisibleRangeText</b>.
    /// </remarks>
    public string TitleDateRangeSeparatorString
    {
      get
      {
        return ViewState["TitleDateRangeSeparatorString"] == null ? " - " : (string)ViewState["TitleDateRangeSeparatorString"];
      }
      set
      {
        ViewState["TitleDateRangeSeparatorString"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the type of the contents displayed in the calendar title area.
    /// </summary>
    /// <value>A value indicating which kind of <b>CalendarTitleType</b> to display.
    /// Default is <b>CalendarTitleType.VisibleDateText</b></value>
    /// <remarks>
    /// This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b> 
    /// and the <see cref="ShowTitle"/> property is set to <b>true</b>.
    /// </remarks>
    public CalendarTitleType TitleType
    {
      get
      {
        return Utils.ParseCalendarTitleType(ViewState["TitleType"], ComponentArt.Web.UI.CalendarTitleType.VisibleDateText);
      }
      set
      {
        ViewState["TitleType"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for today's date on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for today's date on the <see cref="Calendar"/> control. If the 
    /// <b>TodayDayActiveCssClass</b> property is not set, the CSS class specified in the <see cref="TodayDayHoverCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string TodayDayActiveCssClass
    {
      get
      {
        return (string)ViewState["TodayDayActiveCssClass"];
      }
      set
      {
        ViewState["TodayDayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for today's date on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for today's date on the <see cref="Calendar"/> control. If the 
    /// <b>TodayDayCssClass</b> property is not set, the CSS class specified in the <see cref="DayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string TodayDayCssClass
    {
      get
      {
        return (string)ViewState["TodayDayCssClass"];
      }
      set
      {
        ViewState["TodayDayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for today's date on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for today's date on the <see cref="Calendar"/> control. If the 
    /// <b>TodayDayHoverCssClass</b> property is not set, the CSS class specified in the <see cref="TodayDayCssClass"/> 
    /// property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string TodayDayHoverCssClass
    {
      get
      {
        return (string)ViewState["TodayDayHoverCssClass"];
      }
      set
      {
        ViewState["TodayDayHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the value for today's date.
    /// </summary>
    /// <value>
    /// A <see cref="System.DateTime"/> object that contains the value that the <see cref="Calendar"/> control considers to be 
    /// today's date.  If this property is not explicitly set, this date will be the date on the server.
    /// </value>
    /// <remarks>
    /// The appearance of date specified by the <b>TodaysDate</b> property can be customized by using the 
    /// <see cref="TodayDayCssClass"/> property.
    /// <note>If the <see cref="VisibleDate"/> property is not set, the date specified by the <b>TodaysDate</b> property 
    /// determines which date is displayed in the <see cref="Calendar"/> control.</note>
    /// <note>This property is only significant if <see cref="UseServersTodaysDate"/> property is set to <b>true</b>.
    /// If it is set to <b>false</b>, client's clock will be used to determine today's date, and the value of <b>TodaysDate</b>
    /// property will have no bearing on <b>VisibleDate</b> property or on appearance of the date it specifies.</note>
    /// </remarks>
    public DateTime TodaysDate
    {
      get
      {
        return Utils.ParseDateTime(ViewState["TodaysDate"], this._todaysDate);
      }
      set
      {
        ViewState["TodaysDate"] = value;
      }
    }

    /// <summary>
    /// Whether to toggle day selection only when it is Ctrl + clicked.  Default is true.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control toggles selected date only when a date is clicked with Ctrl key held down. 
    /// <b>false</b> if every click toggles whether the date is selected.  The default value is <b>true</b>.
    /// </value>
    /// <remarks>
    /// If this property is set to true, when a date is clicked without the Ctrl key, it becomes the lone selected date.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>
    /// and <see cref="AllowMultipleSelection"/> property is set to <b>true</b>.</para>
    /// </remarks>
    [DefaultValue(true)]
    public bool ToggleSelectOnCtrlKey
    {
      get 
      {
        return Utils.ParseBool(ViewState["ToggleSelectOnCtrlKey"], true);
      }
      set 
      {
        ViewState["ToggleSelectOnCtrlKey"] = value;
      }
    }

    /// <summary>
    /// Whether the <see cref="Calendar"/> control should use server's clock for determining today's date.
    /// </summary>
    /// <value>
    /// <b>true</b> if the <see cref="Calendar"/> control uses the server's clock to determine today's date; 
    /// <b>false</b> if it uses the client's clock. The default value is <b>true</b>.
    /// </value>
    /// <remarks>
    /// If <b>UseServersTodaysDate</b> property is set to <b>true</b>, today's date specified in <see cref="TodaysDate"/>
    /// property is used.  Otherwise, client's clock determines today's date.
    /// </remarks>
    public bool UseServersTodaysDate
    {
      get
      {
        return Utils.ParseBool(ViewState["UseServersTodaysDate"], true);
      }
      set
      {
        ViewState["UseServersTodaysDate"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the date that is displayed in the <see cref="Calendar"/> control.
    /// </summary>
    /// <value>
    /// A <see cref="System.DateTime"/> object that specifies the date that <see cref="Calendar"/> control displays. 
    /// The default value is <see cref="DateTime.MinValue">DateTime.MinValue</see>, indicating to default to <see cref="TodaysDate"/>.
    /// </value>
    /// <remarks>
    /// If <b>VisibleDate</b> property is set to <b>DateTime.MinValue</b>, but <see cref="UseServersTodaysDate"/> property
    /// is set to <b>false</b>, <b>VisibleDate</b> will attempt to default to client's today's date, and <b>TodaysDate</b>
    /// property will be ignored.
    /// <note>
    /// This property is most relevant when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.
    /// If the <see cref="ControlType"/> property is set to <b>CalendarControlType.Picker</b>, <see cref="SelectedDate"/> property
    /// is of more interest.
    /// </note>
    /// </remarks>
    public DateTime VisibleDate 
    {
      get
      {
        return Utils.ParseDateTime(ViewState["VisibleDate"], DateTime.MinValue);
      }
      set
      {
        ViewState["VisibleDate"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the column in the Calendar in which the visible month is displayed.
    /// </summary>
    /// <value>
    /// The number of the column of the <see cref="Calendar"/> control in which the visible month is displayed.  Default is <b>1</b>.
    /// </value>
    /// <remarks>
    /// This property is one-delimited.  The leftmost column is numbered 1.
    /// <p>The value of <b>VisibleMonthColumn</b> must be at least 1 and no more than the value of <see cref="MonthColumns"/> property.</p>
    /// <p>Which month is visible is determined by <see cref="VisibleDate"/> property.</p>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified column number is less than 1 or more than the value of 
    /// <b>MonthColumns</b> property.</exception>
    public int VisibleMonthColumn
    {
      get
      {
        return Utils.ParseInt(ViewState["VisibleMonthColumn"], 1);
      }
      set
      {
        if (value < 1)
        {
          throw new Exception(String.Format("{0}.VisibleMonthColumn may not be less than 1.", this.ID));
        }
        ViewState["VisibleMonthColumn"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the row in the Calendar in which the visible month is displayed.
    /// </summary>
    /// <value>
    /// The number of the row of the <see cref="Calendar"/> control in which the visible month is displayed.  Default is <b>1</b>.
    /// </value>
    /// <remarks>
    /// This property is one-delimited.  The topmost row is numbered 1.
    /// <p>The value of <b>VisibleMonthRow</b> must be at least 1 and no more than the value of <see cref="MonthRows"/> property.</p>
    /// <p>Which month is visible is determined by <see cref="VisibleDate"/> property.</p>
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    /// <exception cref="System.ArgumentOutOfRangeException">The specified row number is less than 1 or more than the value of 
    /// <b>MonthRows</b> property.</exception>
    public int VisibleMonthRow
    {
      get
      {
        return Utils.ParseInt(ViewState["VisibleMonthRow"], 1);
      }
      set
      {
        if (value < 1)
        {
          throw new Exception(String.Format("{0}.VisibleMonthRow may not be less than 1.", this.ID));
        }
        ViewState["VisibleMonthRow"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the active CSS class for the weekend dates on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the active CSS class for the weekend dates on the <see cref="Calendar"/> control.
    /// If the <b>WeekendDayActiveCssClass</b> property is not set, the CSS class specified in the 
    /// <see cref="WeekendDayHoverCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string WeekendDayActiveCssClass
    {
      get
      {
        return (string)ViewState["WeekendDayActiveCssClass"];
      }
      set
      {
        ViewState["WeekendDayActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the CSS class for the weekend dates on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the CSS class for the weekend dates on the <see cref="Calendar"/> control.
    /// If the <b>WeekendDayCssClass</b> property is not set, the CSS class specified in the 
    /// <see cref="DayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string WeekendDayCssClass
    {
      get
      {
        return (string)ViewState["WeekendDayCssClass"];
      }
      set
      {
        ViewState["WeekendDayCssClass"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the hover CSS class for the weekend dates on the <see cref="Calendar"/> control.
    /// </summary>
    /// <remarks>
    /// Use this property to specify the hover CSS class for the weekend dates on the <see cref="Calendar"/> control.
    /// If the <b>WeekendDayHoverCssClass</b> property is not set, the CSS class specified in the 
    /// <see cref="WeekendDayCssClass"/> property is used.
    /// <para>This property only applies when the <see cref="ControlType"/> property is set to <b>CalendarControlType.Calendar</b>.</para>
    /// </remarks>
    public string WeekendDayHoverCssClass
    {
      get
      {
        return (string)ViewState["WeekendDayHoverCssClass"];
      }
      set
      {
        ViewState["WeekendDayHoverCssClass"] = value;
      }
    }

    #endregion


    #region Protected Properties
    #endregion

    #region Internal Properties
    #endregion

    #region Private Properties

    private DayOfWeek _defaultFirstDayOfWeek;
    private CalendarWeekRule _defaultCalendarWeekRule;
    private DateTimeFormatToken[] _pickerTokens = null;
    private DateTimeFormatToken[] _calendarTitleTokens = null;
    private DateTime _todaysDate = DateTime.Today;
    private DateCollection _selectedDatesCollection = null;
    private ArrayList _selectedDatesList;
    private DateCollection _disabledDatesCollection = null;
    private ArrayList _disabledDatesList = null;
    private CalendarDayCollection _customDaysCollection = null;
    private SortedList _customDaysList = null;
    private bool _selectedDatesChanged = false;
    private bool _visibleDateChanged = false;
    private Hashtable _calendarCellData = new Hashtable();
    private string _clientVarName = null; // Calculated at OnInit and updated a couple of times
    private bool _isPickerUplevel;
    private bool _isCalendarUplevel;
    private bool _isPopupUplevel;
    private string _footerClientTemplateString = null;
    private string _headerClientTemplateString = null;


    #endregion

    #region Public Methods

    /// <summary>
    /// Constructor
    /// </summary>
    public Calendar()
    {
      this.DateTimeFormat = DateTimeFormatInfo.CurrentInfo;
      this._selectedDatesList = new ArrayList();
    }

    /// <summary>
    /// Sets predefined CSS classes and client templates for theming purposes.
    /// </summary>
    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      // Base
      if ((this.CalendarCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.CalendarCssClass = prefix + "calendar";
      }
      if ((this.ContentCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.ContentCssClass = prefix + "calendar-content";
      }
      if ((this.DayCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DayCssClass = prefix + "calendar-day";
      }
      if ((this.DayHeaderCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DayHeaderCssClass = prefix + "calendar-day-header";
      }
      if ((this.MonthCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.MonthCssClass = prefix + "calendar-month";
      }
      if ((this.MonthTitleCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.MonthTitleCssClass = prefix + "calendar-month-title";
      }
      if ((this.NextPrevCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.NextPrevCssClass = prefix + "calendar-next-previous";
      }
      if ((this.OtherMonthDayCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.OtherMonthDayCssClass = prefix + "calendar-other-month-day";
      }
      if ((this.SelectedDayCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.SelectedDayCssClass = prefix + "calendar-day-selected";
      } 
      if (ViewState["CellPadding"] == null || overwrite)
      {
        ViewState["CellPadding"] = 0;
      }
      if (ViewState["CellSpacing"] == null || overwrite)
      {
        ViewState["CellSpacing"] = 0;
      }
      if (ViewState["DayNameFormat"] == null || overwrite)
      {
        ViewState["DayNameFormat"] = DayNameFormat.FirstTwoLetters;
      }
    }

    /// <summary>
    /// Implements <see cref="IPostBackDataHandler.LoadPostData"/> method of <see cref="IPostBackDataHandler"/> interface.
    /// </summary>
    public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
    {
      postDataKey = this._clientVarName  = this.ClientID.Replace("$", "_"); // really, we are ignoring postDataKey and calculating it ourself

      string mySelectedDates = Utils.DateArrayListToString(this._selectedDatesList, (this.ControlType == CalendarControlType.Calendar) ? "yyyy.M.d" : "yyyy.M.d.H.m.s");
      string myVisibleDate = Utils.DateToString(this.VisibleDate, "yyyy.M.d");
      
      if (this.ControlType==CalendarControlType.Picker && !this._isPickerUplevel)
      {
        // Downlevel picker is a special case
        string pickerTextPostBack = postCollection[this._clientVarName + "_picker"];
        if (pickerTextPostBack != null)
        {
          if (pickerTextPostBack == String.Empty)
          {
            this.SelectedDates.Clear();
          }
          else
          {
            try
            {
              this.SelectedDate = DateTime.Parse(pickerTextPostBack);
            }
            catch (Exception e)
            {
              string message = this.ID + " input text cannot be parsed as a valid SelectedDate.";
              throw new FormatException(message, e);
            }
          }
        }
      }
      else
      {
        // Everything other than downlevel picker executes this code.
        string selectedDatesPostBack = postCollection[this._clientVarName + "_selecteddates"];
        if (selectedDatesPostBack != null)
        {
          this.SelectedDates.Clear();
          foreach (DateTime selectedDate in Utils.StringToDateArrayList(selectedDatesPostBack, (this.ControlType == CalendarControlType.Calendar) ? "yyyy.M.d" : "yyyy.M.d.H.m.s"))
          {
            this.SelectedDates.Add(selectedDate);
          }
        }
        string visibleDatePostBack = postCollection[this._clientVarName + "_visibledate"];
        if (visibleDatePostBack != null)
        {
          this.VisibleDate = Utils.StringToDate(visibleDatePostBack, "yyyy.M.d");
        }
      }
      
      string apparentVisibleDateText = postCollection[this._clientVarName + "_apparentvisibledate"];
      this._visibleDateChanged = myVisibleDate != Utils.DateToString(this.VisibleDate, "yyyy.M.d");
      if (this._visibleDateChanged && apparentVisibleDateText != null && apparentVisibleDateText != "")
      {
        DateTime apparentVisibleDate = DateTime.ParseExact(apparentVisibleDateText+".1", "yyyy.M.d", DateTimeFormatInfo.InvariantInfo);
        this._visibleDateChanged = !(this.VisibleDate.Year == apparentVisibleDate.Year && this.VisibleDate.Month == apparentVisibleDate.Month);
      }

      this._selectedDatesChanged = mySelectedDates != Utils.DateArrayListToString(this._selectedDatesList, (this.ControlType == CalendarControlType.Calendar) ? "yyyy.M.d" : "yyyy.M.d.H.m.s");

      return this._selectedDatesChanged || this._visibleDateChanged;
    }

    /// <summary>
    /// Implements <see cref="IPostBackDataHandler.RaisePostDataChangedEvent"/> method of <see cref="IPostBackDataHandler"/> interface.
    /// </summary>
    public void RaisePostDataChangedEvent()
    {
      if (this._selectedDatesChanged)
        this.OnSelectionChanged(EventArgs.Empty);
      if (this._visibleDateChanged)
        this.OnVisibleDateChanged(EventArgs.Empty);    
    }
  
    /// <summary>
    /// Force the re-loading and re-binding of custom day templates.
    /// </summary>
    /// <seealso cref="Templates" />
    public void ReloadTemplates()
    {
      this.Controls.Clear();
      this.InstantiateTemplatedDays();
    }

    #endregion

    #region Protected Methods

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      this._clientVarName = this.ClientID.Replace("$", "_");


      // do we need to render scripts for non-Atlas?
      if (Page != null)
      {
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
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
          if (this._isPopupUplevel)
          {
            if (!Page.IsClientScriptBlockRegistered("A573T069.js"))
            {
              Page.RegisterClientScriptBlock("A573T069.js", "");
              WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573T069.js");
            }
          }
          if (!Page.IsClientScriptBlockRegistered("A573Q148.js"))
          {
            Page.RegisterClientScriptBlock("A573Q148.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Calendar.client_scripts", "A573Q148.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573W128.js"))
          {
            Page.RegisterClientScriptBlock("A573W128.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Calendar.client_scripts", "A573W128.js");
          }
        }
      }

      if (this.AutoTheming)
      {
        this.ApplyTheming(false);
      }

      // Output the SelectedDates data
      output.Write("<input type=\"hidden\"");
      output.WriteAttribute("id", this._clientVarName + "_selecteddates");
      output.WriteAttribute("name", this._clientVarName + "_selecteddates");
      output.WriteAttribute("value", Utils.DateArrayListToString(this._selectedDatesList, (this.ControlType == CalendarControlType.Calendar) ? "yyyy.M.d" : "yyyy.M.d.H.m.s"));

      output.Write(" />");

      // Output the VisibleDate data
      output.Write("<input type=\"hidden\"");
      output.WriteAttribute("id", this._clientVarName + "_visibledate");
      output.WriteAttribute("name", this._clientVarName + "_visibledate");
      output.WriteAttribute("value", Utils.DateToString(this.VisibleDate, "yyyy.M.d"));
      output.Write(" />");

      if (this.ControlType == CalendarControlType.Calendar && this.VisibleDate == DateTime.MinValue)
      {
        // Output the apparent VisibleDate hidden data field
        // This oddball is needed sometimes when the visible date is not explicitly specified.
        // If the end user then changes the calendar's visible date on the client we need a 
        // way to know whether we should fire a VisibleDate changed event or not.
        output.Write("<input type=\"hidden\"");
        output.WriteAttribute("id", this._clientVarName + "_apparentvisibledate");
        output.WriteAttribute("name", this._clientVarName + "_apparentvisibledate");
        output.Write(" />");
      }

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessibleContent(output);
      }
      else
      {

        if (this.ControlType == CalendarControlType.Picker)
        {
          // Output the picker textbox
          output.Write("<input type=\"text\"");
          output.WriteAttribute("id", this._clientVarName + "_picker");
          output.WriteAttribute("name", this._clientVarName + "_picker");
          output.WriteAttribute("class", this.PickerCssClass);
          this._pickerTokens = DateTimeFormatTokenizer.Tokenize(this.ResolvePickerFormatString());
          output.WriteAttribute("size", TokenLengthSum(this._pickerTokens).ToString());
          if (!this.Enabled)
          {
            output.WriteAttribute("disabled", null);
          }

          if (this._isPickerUplevel)
          {
            output.WriteAttribute("onfocus", "return ComponentArt_Calendar_PickerOnFocus(this)");
            output.WriteAttribute("onblur", "return ComponentArt_Calendar_PickerOnBlur(this)");
            output.WriteAttribute("ondblclick", "return ComponentArt_Calendar_DblClick(window." + this._clientVarName + ",event);");
            output.WriteAttribute("onmousedown", "return ComponentArt_Calendar_PickerOnMouseDown(this)");
            output.WriteAttribute("onmouseup", "return ComponentArt_Calendar_PickerOnMouseUp(this)");
            output.WriteAttribute("onkeydown", "return ComponentArt_Calendar_PickerOnKeyDown(event,this)");
            output.WriteAttribute("onkeyup", "return ComponentArt_Calendar_PickerOnKeyUp(this)");
            output.WriteAttribute("onkeypress", "return ComponentArt_Calendar_PickerOnKeyPress(event,this)");
            output.WriteAttribute("onselect", "return ComponentArt_Calendar_PickerOnSelect(this)");
            output.WriteAttribute("onselectstart", "return ComponentArt_Calendar_PickerOnSelectStart(this);");
            output.WriteAttribute("ondragstart", "return ComponentArt_Calendar_PickerOnDragStart(this)");
          }

          output.Write(" />");
        }

        this._calendarTitleTokens = DateTimeFormatTokenizer.Tokenize(this.TitleDateFormat);

        // Calculate client-side templates if any
        if (this.FooterClientTemplate != null && this.FooterClientTemplate.Text != null && this.FooterClientTemplate.Text != String.Empty)
        {
          this._footerClientTemplateString = this.FooterClientTemplate.Text.Replace("\n", "").Replace("\r", "");
        }
        if (this.HeaderClientTemplate != null && this.HeaderClientTemplate.Text != null && this.HeaderClientTemplate.Text != String.Empty)
        {
          this._headerClientTemplateString = this.HeaderClientTemplate.Text.Replace("\n", "").Replace("\r", "");
        }

        if (this.ControlType == CalendarControlType.Calendar && this.PopUp == CalendarPopUpType.None)
        {
          output.Write("<table ");
          output.WriteAttribute("border", "0");
          output.WriteAttribute("cellpadding", "0");
          output.WriteAttribute("cellspacing", "0");
          output.WriteAttribute("id", this._clientVarName); // for static calendar, this table is the associated element
          output.WriteAttribute("class", this.CalendarCssClass);
          output.Write("><tr><td ");
          output.WriteAttribute("id", this._clientVarName + "_calendarcontents");
          output.Write(">");
          if (this.IsDownLevel()) // uplevel output is generated on the client, in the client-side Render method
          {
            this.RenderDownLevelContent(output);
          }
          output.Write("</td></tr></table>");
        }
        else
        {
          output.Write("<div style=\"display:none\" ");
          output.WriteAttribute("id", this._clientVarName); // pickers and pop-up calendars have a dummy display:none div as the associated element
          output.Write("></div>");
        }

        // Process templates
        if (!this.IsDownLevel())
        {
          // Render server-side templates if any
          foreach (Control oTemplate in this.Controls)
          {
            output.Write("<div id=\"" + oTemplate.ID + "\" style=\"visibility:hidden;position:absolute;left:-100px;top:-100px;\">");
            oTemplate.RenderControl(output);
            output.Write("</div>");
          }
        }

        // Output the Calendar initialization function.
        StringBuilder startupScript = new StringBuilder();
        startupScript.Append("function ComponentArt_Init_" + this._clientVarName + "() {\n");

        // Include check for whether everything we need is loaded, and a retry after a delay in case it isn't.
        string readyToInitializeCalendar = "window.cart_calendar_kernel_loaded && window.cart_calendar_support_loaded";
        startupScript.Append("if (!(" + readyToInitializeCalendar + "))\n");
        startupScript.Append("{\n\tsetTimeout('ComponentArt_Init_" + this._clientVarName + "()', 100);\n\treturn;\n}\n");

        // Instantiate calendar object
        startupScript.Append("window." + this._clientVarName + " = new ComponentArt_Calendar('" + this._clientVarName + "');\n");

        // Write postback function reference
        if (Page != null)
        {
          startupScript.Append(_clientVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != _clientVarName)
        {
          startupScript.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + _clientVarName + "; " + _clientVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        // Define properties
        startupScript.Append("var properties = [\n");
        startupScript.Append("['AbbreviatedDayNames'," + new JavaScriptArray(this.AbbreviatedDayNames).ToString() + "],");
        startupScript.Append("['AbbreviatedMonthNames'," + new JavaScriptArray(this.AbbreviatedMonthNames).ToString() + "],");
        startupScript.Append("['AllowDaySelection'," + this.AllowDaySelection.ToString().ToLower() + "],");
        startupScript.Append("['AllowMonthSelection'," + this.AllowMonthSelection.ToString().ToLower() + "],");
        startupScript.Append("['AllowMultipleSelection'," + this.AllowMultipleSelection.ToString().ToLower() + "],");
        startupScript.Append("['AllowWeekSelection'," + this.AllowWeekSelection.ToString().ToLower() + "],");
        startupScript.Append("['AMDesignator'," + Utils.ConvertStringToJSString(this.AMDesignator) + "],");
        startupScript.Append("['ApplicationPath'," + ((this.Context != null) ? Utils.ConvertStringToJSString(this.Context.Request.ApplicationPath) : String.Empty) + "],");
        startupScript.Append("['AutoPostBackOnSelectionChanged'," + this.AutoPostBackOnSelectionChanged.ToString().ToLower() + "],");
        startupScript.Append("['AutoPostBackOnVisibleDateChanged'," + this.AutoPostBackOnVisibleDateChanged.ToString().ToLower() + "],");
        if (this.AutoTheming)
        {
          startupScript.Append("['AutoTheming',1],");
          startupScript.Append("['AutoThemingCssClassPrefix'," + Utils.ConvertStringToJSString(this.AutoThemingCssClassPrefix) + "],");
        }
        startupScript.Append("['CalendarCssClass'," + Utils.ConvertStringToJSString(this.CalendarCssClass) + "],");
        startupScript.Append("['CalendarWeekRule'," + ((int)this.CalendarWeekRule).ToString() + "],");
        startupScript.Append("['CellPadding'," + this.CellPadding.ToString() + "],");
        startupScript.Append("['CellSpacing'," + this.CellSpacing.ToString() + "],");
        startupScript.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
        startupScript.Append("['ClientSideOnAfterVisibleDateChanged'," + (this.ClientSideOnAfterVisibleDateChanged == null ? "null" : this.ClientSideOnAfterVisibleDateChanged) + "],");
        startupScript.Append("['ClientSideOnBeforeVisibleDateChanged'," + (this.ClientSideOnBeforeVisibleDateChanged == null ? "null" : this.ClientSideOnBeforeVisibleDateChanged) + "],");
        startupScript.Append("['ClientSideOnSelectionChanged'," + (this.ClientSideOnSelectionChanged == null ? "null" : this.ClientSideOnSelectionChanged) + "],");
        startupScript.Append("['ClientSideOnVisibleDateChanged'," + (this.ClientSideOnVisibleDateChanged == null ? "null" : this.ClientSideOnVisibleDateChanged) + "],");
        startupScript.Append("['CollapseOnSelect'," + this.CollapseOnSelect.ToString().ToLower() + "],");
        startupScript.Append("['ContentCssClass'," + Utils.ConvertStringToJSString(this.ContentCssClass) + "],");
        startupScript.Append("['ControlType'," + ((int)this.ControlType).ToString() + "],");
        startupScript.Append("['CustomDays',new ComponentArt_CalendarDayCollection(" + this.CustomDays.ToString(this.ClientID) + ",window." + this._clientVarName + ")],");
        startupScript.Append("['DayActiveCssClass'," + Utils.ConvertStringToJSString(this.DayActiveCssClass) + "],");
        startupScript.Append("['DayCssClass'," + Utils.ConvertStringToJSString(this.DayCssClass) + "],");
        startupScript.Append("['DayHoverCssClass'," + Utils.ConvertStringToJSString(this.DayHoverCssClass) + "],");
        startupScript.Append("['DayHeaderCssClass'," + Utils.ConvertStringToJSString(this.DayHeaderCssClass) + "],");
        startupScript.Append("['DayNameFormat'," + ((int)this.DayNameFormat).ToString() + "],");
        startupScript.Append("['DayNames'," + new JavaScriptArray(this.DayNames).ToString() + "],");
        startupScript.Append("['DisabledDates',new ComponentArt_Calendar_DateCollection(" + new JavaScriptArray(this.DisabledDates).ToString() + ",window." + this._clientVarName + ")],");
        startupScript.Append("['DisabledDayActiveCssClass'," + Utils.ConvertStringToJSString(this.DisabledDayActiveCssClass) + "],");
        startupScript.Append("['DisabledDayCssClass'," + Utils.ConvertStringToJSString(this.DisabledDayCssClass) + "],");
        startupScript.Append("['DisabledDayHoverCssClass'," + Utils.ConvertStringToJSString(this.DisabledDayHoverCssClass) + "],");
        startupScript.Append("['Enabled'," + this.Enabled.ToString().ToLower() + "],");
        startupScript.Append("['FirstDayOfWeek'," + Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + "],");
        startupScript.Append("['FooterClientTemplate'," + Utils.ConvertStringToJSString(this._footerClientTemplateString) + "],");
        startupScript.Append("['HeaderClientTemplate'," + Utils.ConvertStringToJSString(this._headerClientTemplateString) + "],");

        if (!Height.IsEmpty && Height.Type == UnitType.Pixel) startupScript.Append("['Height'," + Height.Value + "],");

        startupScript.Append("['ImagesBaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl)) + "],");
        startupScript.Append("['IsCalendarUplevel'," + this._isCalendarUplevel.ToString().ToLower() + "],");
        startupScript.Append("['IsPickerUplevel'," + this._isPickerUplevel.ToString().ToLower() + "],");
        startupScript.Append("['IsPopupUplevel'," + this._isPopupUplevel.ToString().ToLower() + "],");
        startupScript.Append("['MaxDate'," + Utils.ConvertDateTimeToJsDate(this.MaxDate) + "],");
        startupScript.Append("['MinDate'," + Utils.ConvertDateTimeToJsDate(this.MinDate) + "],");
        startupScript.Append("['MonthColumns'," + this.MonthColumns.ToString() + "],");
        startupScript.Append("['MonthCssClass'," + Utils.ConvertStringToJSString(this.MonthCssClass) + "],");
        startupScript.Append("['MonthNames'," + new JavaScriptArray(this.MonthNames).ToString() + "],");
        startupScript.Append("['MonthPadding'," + this.MonthPadding.ToString() + "],");
        startupScript.Append("['MonthSpacing'," + this.MonthSpacing.ToString() + "],");
        startupScript.Append("['MonthRows'," + this.MonthRows.ToString() + "],");
        startupScript.Append("['MonthTitleCssClass'," + Utils.ConvertStringToJSString(this.MonthTitleCssClass) + "],");
        startupScript.Append("['NextImageHeight'," + this.NextImageHeight.ToString() + "],");
        startupScript.Append("['NextImageUrl'," + Utils.ConvertStringToJSString(this.NextImageUrl) + "],");
        startupScript.Append("['NextImageWidth'," + this.NextImageWidth.ToString() + "],");
        startupScript.Append("['NextText'," + Utils.ConvertStringToJSString(this.NextText) + "],");
        startupScript.Append("['NextPrevActiveCssClass'," + Utils.ConvertStringToJSString(this.NextPrevActiveCssClass) + "],");
        startupScript.Append("['NextPrevCssClass'," + Utils.ConvertStringToJSString(this.NextPrevCssClass) + "],");
        startupScript.Append("['NextPrevHoverCssClass'," + Utils.ConvertStringToJSString(this.NextPrevHoverCssClass) + "],");
        startupScript.Append("['OtherMonthDayActiveCssClass'," + Utils.ConvertStringToJSString(this.OtherMonthDayActiveCssClass) + "],");
        startupScript.Append("['OtherMonthDayCssClass'," + Utils.ConvertStringToJSString(this.OtherMonthDayCssClass) + "],");
        startupScript.Append("['OtherMonthDayHoverCssClass'," + Utils.ConvertStringToJSString(this.OtherMonthDayHoverCssClass) + "],");
        startupScript.Append("['OutOfRangeDayActiveCssClass'," + Utils.ConvertStringToJSString(this.OutOfRangeDayActiveCssClass) + "],");
        startupScript.Append("['OutOfRangeDayCssClass'," + Utils.ConvertStringToJSString(this.OutOfRangeDayCssClass) + "],");
        startupScript.Append("['OutOfRangeDayHoverCssClass'," + Utils.ConvertStringToJSString(this.OutOfRangeDayHoverCssClass) + "],");
        startupScript.Append("['PickerCssClass'," + Utils.ConvertStringToJSString(this.PickerCssClass) + "],");
        startupScript.Append("['PickerTokensArray'," + new JavaScriptArray(this._pickerTokens).ToString() + "],");
        startupScript.Append("['PMDesignator'," + Utils.ConvertStringToJSString(this.PMDesignator) + "],");
        if (this.Page != null)
        {
          startupScript.Append("['PostBackCommand'," + Utils.ConvertStringToJSString(this.Page.GetPostBackEventReference(this)) + "],");
        }
        startupScript.Append("['PopUp'," + ((int)this.PopUp).ToString() + "],");
        startupScript.Append("['PopUpCollapseDuration'," + this.PopUpCollapseDuration.ToString() + "],");
        startupScript.Append("['PopUpCollapseSlide'," + ((int)this.PopUpCollapseSlide).ToString() + "],");
        startupScript.Append("['PopUpCollapseTransition'," + ((int)this.PopUpCollapseTransition).ToString() + "],");
        startupScript.Append("['PopUpCollapseTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.PopUpCollapseTransitionCustomFilter) + "],");
        startupScript.Append("['PopUpExpandControlId'," + Utils.ConvertStringToJSString(this.PopUpExpandControlId) + "],");
        startupScript.Append("['PopUpExpandDirection'," + ((int)this.PopUpExpandDirection).ToString() + "],");
        startupScript.Append("['PopUpExpandDuration'," + this.PopUpExpandDuration.ToString() + "],");
        startupScript.Append("['PopUpExpandOffsetX'," + this.PopUpExpandOffsetX.ToString() + "],");
        startupScript.Append("['PopUpExpandOffsetY'," + this.PopUpExpandOffsetY.ToString() + "],");
        startupScript.Append("['PopUpExpandSlide'," + ((int)this.PopUpExpandSlide).ToString() + "],");
        startupScript.Append("['PopUpExpandTransition'," + ((int)this.PopUpExpandTransition).ToString() + "],");
        startupScript.Append("['PopUpExpandTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.PopUpExpandTransitionCustomFilter) + "],");
        startupScript.Append("['PopUpShadowEnabled'," + this.PopUpShadowEnabled.ToString().ToLower() + "],");
        startupScript.Append("['PopUpZIndex'," + this.PopUpZIndex.ToString() + "],");
        startupScript.Append("['Precision'," + this.Precision.TotalMilliseconds.ToString() + "],");
        startupScript.Append("['PrevImageHeight'," + this.PrevImageHeight.ToString() + "],");
        startupScript.Append("['PrevImageUrl'," + Utils.ConvertStringToJSString(this.PrevImageUrl) + "],");
        startupScript.Append("['PrevImageWidth'," + this.PrevImageWidth.ToString() + "],");
        startupScript.Append("['PrevText'," + Utils.ConvertStringToJSString(this.PrevText) + "],");
        startupScript.Append("['ReactOnSameSelection'," + this.ReactOnSameSelection.ToString().ToLower() + "],");
        startupScript.Append("['SelectedDates',new ComponentArt_Calendar_DateCollection(" + new JavaScriptArray(this.SelectedDates).ToString() + ",window." + this._clientVarName + ")],");
        startupScript.Append("['SelectedDayActiveCssClass'," + Utils.ConvertStringToJSString(this.SelectedDayActiveCssClass) + "],");
        startupScript.Append("['SelectedDayCssClass'," + Utils.ConvertStringToJSString(this.SelectedDayCssClass) + "],");
        startupScript.Append("['SelectedDayHoverCssClass'," + Utils.ConvertStringToJSString(this.SelectedDayHoverCssClass) + "],");
        startupScript.Append("['SelectMonthActiveCssClass'," + Utils.ConvertStringToJSString(this.SelectMonthActiveCssClass) + "],");
        startupScript.Append("['SelectMonthCssClass'," + Utils.ConvertStringToJSString(this.SelectMonthCssClass) + "],");
        startupScript.Append("['SelectMonthHoverCssClass'," + Utils.ConvertStringToJSString(this.SelectMonthHoverCssClass) + "],");
        startupScript.Append("['SelectMonthText'," + Utils.ConvertStringToJSString(this.SelectMonthText) + "],");
        startupScript.Append("['SelectWeekActiveCssClass'," + Utils.ConvertStringToJSString(this.SelectWeekActiveCssClass) + "],");
        startupScript.Append("['SelectWeekCssClass'," + Utils.ConvertStringToJSString(this.SelectWeekCssClass) + "],");
        startupScript.Append("['SelectWeekHoverCssClass'," + Utils.ConvertStringToJSString(this.SelectWeekHoverCssClass) + "],");
        startupScript.Append("['SelectWeekText'," + Utils.ConvertStringToJSString(this.SelectWeekText) + "],");
        startupScript.Append("['ShowDayHeader'," + this.ShowDayHeader.ToString().ToLower() + "],");
        startupScript.Append("['ShowGridLines'," + this.ShowGridLines.ToString().ToLower() + "],");
        startupScript.Append("['ShowMonthTitle'," + this.ShowMonthTitle.ToString().ToLower() + "],");
        startupScript.Append("['ShowNextPrev'," + this.ShowNextPrev.ToString().ToLower() + "],");
        startupScript.Append("['ShowWeekNumbers'," + this.ShowWeekNumbers.ToString().ToLower() + "],");
        startupScript.Append("['ShowTitle'," + this.ShowTitle.ToString().ToLower() + "],");
        startupScript.Append("['SwapDuration'," + this.SwapDuration.ToString() + "],");
        startupScript.Append("['SwapSlide'," + ((int)this.SwapSlide).ToString() + "],");
        startupScript.Append("['SwapTransition'," + ((int)this.SwapTransition).ToString() + "],");
        startupScript.Append("['SwapTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.SwapTransitionCustomFilter) + "],");
        startupScript.Append("['TitleCssClass'," + Utils.ConvertStringToJSString(this.TitleCssClass) + "],");
        startupScript.Append("['TitleDateRangeSeparatorString'," + Utils.ConvertStringToJSString(this.TitleDateRangeSeparatorString) + "],");
        startupScript.Append("['TitleTokensArray'," + new JavaScriptArray(this._calendarTitleTokens).ToString() + "],");
        startupScript.Append("['TitleType'," + ((int)this.TitleType).ToString() + "],");
        startupScript.Append("['ToggleSelectOnCtrlKey'," + this.ToggleSelectOnCtrlKey.ToString().ToLower() + "],");
        startupScript.Append("['TodayDayActiveCssClass'," + Utils.ConvertStringToJSString(this.TodayDayActiveCssClass) + "],");
        startupScript.Append("['TodayDayCssClass'," + Utils.ConvertStringToJSString(this.TodayDayCssClass) + "],");
        startupScript.Append("['TodayDayHoverCssClass'," + Utils.ConvertStringToJSString(this.TodayDayHoverCssClass) + "],");
        startupScript.Append("['TodaysDate'," + (this.UseServersTodaysDate || this.IsDownLevel() ? Utils.ConvertDateTimeToJsDate(this.TodaysDate) : "null") + "],");
        startupScript.Append("['VisibleDate'," + (this.VisibleDate == DateTime.MinValue ? "null" : Utils.ConvertDateTimeToJsDate(this.VisibleDate)) + "],");
        startupScript.Append("['VisibleMonthColumn'," + this.VisibleMonthColumn.ToString() + "],");
        startupScript.Append("['VisibleMonthRow'," + this.VisibleMonthRow.ToString() + "],");
        startupScript.Append("['WeekendDayActiveCssClass'," + Utils.ConvertStringToJSString(this.WeekendDayActiveCssClass) + "],");
        startupScript.Append("['WeekendDayCssClass'," + Utils.ConvertStringToJSString(this.WeekendDayCssClass) + "],");
        startupScript.Append("['WeekendDayHoverCssClass'," + Utils.ConvertStringToJSString(this.WeekendDayHoverCssClass) + "],");

        if (!Width.IsEmpty && Width.Type == UnitType.Pixel) startupScript.Append("['Width'," + Width.Value + "],");

        // End properties
        startupScript.Append("];\n");

        // Set properties
        startupScript.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", this._clientVarName);

        // Initialize the Calendar
        startupScript.Append(this._clientVarName + ".Initialize();\n");

        // Render the Calendar
        startupScript.Append(this._clientVarName + ".Render();\n");

        if (!this._isCalendarUplevel)
        {
          startupScript.Append(this._clientVarName + ".PopulateCellData(" + this.CalendarCellDataJavaScriptArray() + ");\n");
        }

        // Set the flag that the calendar has been initialized.  This is the last action in the calendar initialization function.
        startupScript.Append("window." + this._clientVarName + "_loaded = true;\n}\n");

        // Call this initialization function.  Remember that it will be repeated after a delay if it's not all ready.
        startupScript.Append("ComponentArt_Init_" + this._clientVarName + "();");

        // Output the initialization script
        WriteStartupScript(output, this.DemarcateClientScript(startupScript.ToString(), "ComponentArt.Web.UI.Calendar " + this.VersionString() + " " + this._clientVarName));
      }

    }

    protected override void CreateChildControls()
    {
      if (this.Templates.Count > 0 && this.CustomDays.Count > 0)
      {
        this.Controls.Clear();
        this.InstantiateTemplatedDays();
      }
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget == ClientTargetLevel.Downlevel) return true;
      if (this.ClientTarget == ClientTargetLevel.Uplevel) return false;
      
      // this.ClientTarget == ClientTargetLevel.Auto
      
      // is validator?
      if(Context != null && Context.Request != null && Page != null &&
        Context.Request.UserAgent != null && Context.Request.UserAgent.IndexOf("Validator") >= 0)
      {
        return false;
      }
      
      if (this.ControlType == CalendarControlType.Picker)
      {
        return !this._isPickerUplevel;      
      }
      else // this.ControlType == CalendarControlType.Calendar
      {
        if (this.PopUp == CalendarPopUpType.None)
        {
          return !this._isCalendarUplevel;
        }
        else
        {
          return !this._isPopupUplevel;
        }
      }
    }

    protected override void LoadViewState(object savedState)
    {
      if (savedState != null)
      {
        // Load State from the array of objects that was saved in SaveViewState
        object[] myState = (object[])savedState;
        if (myState[0] != null)
        {
          base.LoadViewState(myState[0]);
        }
        if (myState[1] != null)
        {
          this._selectedDatesList.Clear();
          ArrayList selectedDatesList = Utils.StringToDateArrayList(myState[1].ToString(), (this.ControlType == CalendarControlType.Calendar) ? "yyyy.M.d" : "yyyy.M.d.H.m.s");
          foreach (object o in selectedDatesList)
          {
            this._selectedDatesList.Add(o);
          }          
        }
        if (myState[2] != null)
        {
          this.VisibleDate = Utils.StringToDate(myState[2].ToString(), "yyyy.M.d");
        }
      }
    }
    
    protected override void OnInit(System.EventArgs e)
    {
      base.OnInit(e);
      
      if (Context == null || Context.Request == null || Context.Request.Form == null)
      {
        return;
      }
      this._clientVarName = this.ClientID.Replace("$", "_");
      this.DetermineBrowserCapabilities();
    }

    protected override void OnLoad(System.EventArgs e)
    {
      base.OnLoad(e);
      if (Page != null)
      {
        if (ScriptManager.GetCurrent(Page) != null)
        {
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573P290.js");
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573T069.js");
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.Calendar.client_scripts.A573Q148.js");
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.Calendar.client_scripts.A573W128.js");
        }
      }
    }

    protected override void OnPreRender(System.EventArgs e)
    {
      this.VerifyDependentProperties();
      base.OnPreRender(e);
      if (Page != null)
      {
        this.Page.RegisterRequiresPostBack(this);
      }
    }

    protected override object SaveViewState()
    {
      // Save State as a cumulative array of objects.
      object baseState = base.SaveViewState();
      object[] allStates = new object[3];
      allStates[0] = baseState;
      allStates[1] = Utils.DateArrayListToString(this._selectedDatesList, (this.ControlType == CalendarControlType.Calendar) ? "yyyy.M.d" : "yyyy.M.d.H.m.s");
      allStates[2] = Utils.DateToString(this.VisibleDate, "yyyy.M.d");
      return allStates;    
    }

    #endregion

    #region Internal Methods
    #endregion

    #region Private Methods

    /// <summary>
    /// This method makes sure the SelectedDate agrees with Precision.
    /// This method only gets called when the control is of type Picker.
    /// </summary>
    private void AdjustSelectedDate()
    {
      if (this.ControlType != CalendarControlType.Picker)
      {
        return;
      }
      if (this.Precision <= TimeSpan.Zero)
      {
        return;
      }
      if (this.SelectedDates.Count == 0)
      {
        return;
      }
      long precisionInJsTicks = (long)(this.Precision.TotalMilliseconds);
      long selectedDateInJsTicks = (long)(this.SelectedDate.Ticks / TimeSpan.TicksPerMillisecond);
      this.SelectedDates.Clear();
      this.SelectedDates.Add(new DateTime((long)(selectedDateInJsTicks / precisionInJsTicks) * precisionInJsTicks * TimeSpan.TicksPerMillisecond));
    }

    private string CalendarCellDataJavaScriptArray()
    {
      string[] cellDataEntries = new string[this._calendarCellData.Count];
      int i = 0;
      foreach (DictionaryEntry cellDataEntry in this._calendarCellData)
      {
        string cellId = cellDataEntry.Key.ToString();
        CalendarCellData cellData = (CalendarCellData) cellDataEntry.Value;
        StringBuilder cellDataString = new StringBuilder();
        cellDataString.Append("[");
        cellDataString.Append(Utils.ConvertStringToJSString(cellId));
        cellDataString.Append(",[");
        string[] cellDataArray = new String[5];
        cellDataArray[0] = Utils.ConvertStringToJSString(cellData.Dormant);
        cellDataArray[1] = Utils.ConvertStringToJSString(cellData.Hover);
        cellDataArray[2] = Utils.ConvertStringToJSString(cellData.Active);
        cellDataArray[3] = Utils.ConvertDateTimeToJsDate(cellData.Date);
        cellDataArray[4] = Utils.ConvertStringToJSString(cellData.TemplateInstanceId);
        cellDataString.Append(String.Join(",", cellDataArray));
        cellDataString.Append("]]");
        cellDataEntries[i] = cellDataString.ToString();
        i++;
      }
      return "[" + String.Join(",",cellDataEntries) + "]";
    }

    private string CalendarCellDateId(int year, int month, int day, string suffix)
    {
      string[] tokens = {year.ToString(), month.ToString(), day.ToString(), suffix};
      return String.Join("_", tokens);
    }

    private CalendarDayCustomTemplate FindTemplateById(string sTemplateId)
    {
      foreach(CalendarDayCustomTemplate oTemplate in this.Templates)
      {
        if(oTemplate.ID == sTemplateId)
        {
          return oTemplate;
        }
      }
      return null;
    }

    /// <summary>
    /// Returns a new DateTime object. It's smarter than "new DateTime", because it accepts zero and negative months, days, etc.
    /// </summary>
    private DateTime CreateDate(int year, int month, int day)
    {
      return new DateTime(year,1,1).AddMonths(month-1).AddDays(day-1);    
    }
    
    /// <summary>
    /// Returns the number of days in the specified month.
    /// </summary>
    private int DaysInMonth(int year, int month)
    {
      return new DateTime(year,1,1).AddMonths(month).AddDays(-1).Day;
    }

    private void InstantiateTemplatedDays()
    {
      foreach (Object o in this.CustomDays)
      {
        CalendarDay calendarDay = (CalendarDay) o;
        if (calendarDay.TemplateId != null && calendarDay.TemplateId != string.Empty)
        {
          CalendarDayCustomTemplate template = this.FindTemplateById(calendarDay.TemplateId);
          if (template == null)
          {
            throw new Exception("Template not found: " + calendarDay.TemplateId);
          }
          CalendarDayTemplateContainer container = new CalendarDayTemplateContainer(calendarDay);
          template.Template.InstantiateIn(container);
          container.ID = this.ClientID + "_" + calendarDay.Date.ToString("yyyyMMdd");
          this.Controls.Add(container);
          container.DataBind();
        }
      }
    }

    private bool IsInCallBack()
    {
      Control ancestor = this.Parent;
      while (ancestor != null)
      {
        if (ancestor is CallBack)
        {
          return true;
        }
        ancestor = ancestor.Parent;
      }
      return false;
    }

    private DateTime ConfinedVisibleDate(DateTime proposedVisibleDate)
    {
      if (proposedVisibleDate < this.MinDate)
      {
        proposedVisibleDate = this.MinDate;
      }
      if (this.MaxDate < proposedVisibleDate)
      {
        proposedVisibleDate = this.MaxDate;
      }
      return proposedVisibleDate;
    }
    
    private void DetermineBrowserCapabilities()
    {
      if (Context == null || Page == null)
      {
        this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = false;
        return;
      }
      if (this.ClientTarget == ClientTargetLevel.Downlevel)
      {
        this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = false;
        return;
      }
      if (this.ClientTarget == ClientTargetLevel.Uplevel)
      {
        this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = true;
        return;
      }
      Utils._BrowserCapabilities bc = Utils.BrowserCapabilities(Context.Request);
      if (bc.IsBrowserWinIE)
      {
        this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = bc.Version >= 5.5;
        return;
      }
      if (bc.IsBrowserOpera)
      {
        this._isPickerUplevel = bc.Version >= 8;
        this._isCalendarUplevel = true;
        this._isPopupUplevel = true;
        return;
      }
      if (bc.IsBrowserNetscape6)
      {
        this._isPickerUplevel = true;
        this._isCalendarUplevel = true;
        this._isPopupUplevel = false;
        return;
      }
      if (bc.IsBrowserSafari)
      {
        this._isPickerUplevel = bc.Version >= 3;
        this._isCalendarUplevel = true;
        this._isPopupUplevel = true;
        return;
      }
      if (bc.IsBrowserMozilla) // Other than Netscape6 and Safari obviously
      {
        this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = true;
        return;
      }
      if (bc.IsBrowserMacIE)
      {
        this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = false;
        return;
      }
      this._isPickerUplevel = this._isCalendarUplevel = this._isPopupUplevel = false;
    }

    #region Accessible Rendering

    private void RenderAccessibleContent(HtmlTextWriter output)
    {
      if (this.VisibleDate == DateTime.MinValue)
      {
        this.VisibleDate = ConfinedVisibleDate(this.TodaysDate);
      }
      if (this.ControlType == CalendarControlType.Calendar)
      {
        RenderAccessibleCalendar(output);
      }
      else // if (this.ControlType == CalendarControlType.Picker)
      {
        RenderDownLevelPicker(output);
      }
    }

    private void RenderAccessibleCalendar(HtmlTextWriter output)
    {
      output.Write("<div");
      output.Write(" id=\"");
      output.Write(this.GetSaneId());
      output.Write("\"");
      output.Write(">");

      RenderAccessibleMonths(output);

      output.Write("</div>");
    }

    private void RenderAccessibleMonths(HtmlTextWriter output)
    {
      int year = this.VisibleDate.Year;
      int month = this.VisibleDate.Month;
      
      output.Write("<table id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsTable\"");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(">");

      if (this.ShowTitle)
      {
        output.Write("<caption>");
        output.Write(this.CalendarTitle());
        output.Write("</caption>");
      }

      for (int row=0; row<this.MonthRows; row++)
      {
        output.Write("<tr");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        for (int col=0; col<this.MonthColumns; col++)
        {
          output.Write("<td");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(">");
          
          RenderAccessibleMonth(output, year, month);
          
          output.Write("</td>");
          month++;
          if (month > 12)
          {
            month = 1;
            year++;
          }
        }
        output.Write("</tr>");
      }
      output.Write("</table>");
    }

    private void RenderAccessibleMonth(HtmlTextWriter output, int year, int month)
    {
      DateTime firstDayOfMonth = new DateTime(year, month, 1);
      int daysPrevMonthShowing = (Utils.ConvertDayOfWeekToJsDay(firstDayOfMonth.DayOfWeek) - Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + 7) % 7;
      CalendarCellInfo[] datesArray = new CalendarCellInfo[6 * 7]; /* 6-week must for now */
      int datesArrayLength = 0; // Keeps track of the index of datesArray at which to place the next CalendarCellInfo
      if (daysPrevMonthShowing > 0)
      {
        int daysPrevMonth = DaysInMonth(year, month - 1); // Note: this will work even if the month ends up negative
        for (int i = daysPrevMonth - daysPrevMonthShowing + 1; i <= daysPrevMonth; i++)
        {
          DateTime cellDate = CreateDate(year, month - 1, i);
          datesArray[datesArrayLength] = new CalendarCellInfo(cellDate, CalendarCellDateId(year, month - 1, i, "1"), this.SelectedDates.Contains(cellDate), this.DisabledDates.Contains(cellDate), i, -1, this.CustomDays[cellDate]);
          datesArrayLength++;
        }
      }
      int daysThisMonth = DaysInMonth(year, month);
      for (int i = 1; i <= daysThisMonth; i++)
      {
        DateTime cellDate = CreateDate(year, month, i);
        datesArray[datesArrayLength] = new CalendarCellInfo(cellDate, CalendarCellDateId(year, month, i, "0"), this.SelectedDates.Contains(cellDate), this.DisabledDates.Contains(cellDate), i, 0, this.CustomDays[cellDate]);
        datesArrayLength++;
      }
      int daysNextMonthShowing = 6 * 7 /* 6-week must, top-aligned is the only option for now */ - daysPrevMonthShowing - daysThisMonth;
      for (int i = 1; i <= daysNextMonthShowing; i++)
      {
        DateTime cellDate = CreateDate(year, month + 1, i);
        datesArray[datesArrayLength] = new CalendarCellInfo(cellDate, CalendarCellDateId(year, month + 1, i, "1"), this.SelectedDates.Contains(cellDate), this.DisabledDates.Contains(cellDate), i, 1, this.CustomDays[cellDate]);
        datesArrayLength++;
      }
      // At this point, datesArray has been fully populated
      output.Write("<table");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(" class=\"");
      output.Write(this.MonthCssClass);
      output.Write("\">");
      bool showSelectorColumn = this.AllowMonthSelection || this.AllowWeekSelection || this.ShowWeekNumbers;
      if (this.ShowMonthTitle)
      {
        output.Write("<caption>");
        output.Write(this.MonthNames[month - 1]); // -1 is there because we're switching between one- and zero-based.
        output.Write("</caption>");
      }
      if (this.ShowDayHeader)
      {
        output.Write("<thead");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        if (showSelectorColumn)
        {
          output.Write("<th id=\"");
          output.Write(this._clientVarName);
          output.Write("_MS_");
          output.Write(year);
          output.Write("_");
          output.Write(month);
          output.Write("\"");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(" class=\"");
          output.Write(this.SelectMonthCssClass);
          output.Write("\"");
          output.Write(">");
          output.Write(this.SelectMonthText);
          output.Write("</th>");
        }
        for (int i = 0; i < 7; i++)  // day names
        {
          output.Write("<th");
          if (this.DayHeaderCssClass != null && this.DayHeaderCssClass.Length > 0)
          {
            output.Write(" scope=\"col\"");
            output.Write(" class=\"");
            output.Write(this.DayHeaderCssClass);
            output.Write("\"");
            if (!this.Enabled)
            {
              output.Write(" disabled");
            }
          }
          output.Write(">");
          switch (this.DayNameFormat)
          {
            case DayNameFormat.FirstLetter:
              output.Write(this.DayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7].Substring(0, 1));
              break;
            case DayNameFormat.FirstTwoLetters:
              output.Write(this.DayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7].Substring(0, 2));
              break;
            case DayNameFormat.Full:
              output.Write(this.DayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7]);
              break;
            case DayNameFormat.Short:
              output.Write(this.AbbreviatedDayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7]);
              break;
          }
          output.Write("</th>");
        }
        output.Write("</thead>");
      }

      PopulateCalendarCellData(datesArray);

      string columnWidth = showSelectorColumn ? "12%" : "14%";
      int k = 0; // k will always equal i*7+j in the loops below. Introduced to cut down on multiplications.
      for (int i = 0; i < 6; i++) //weeks
      {
        output.Write("<tr");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        if (showSelectorColumn)
        {
          DateTime weekStartDate = (DateTime)datesArray[k].Date;
          output.Write("<th id=\"");
          output.Write(this._clientVarName);
          output.Write("_WS_");
          output.Write(weekStartDate.Year);
          output.Write("_");
          output.Write(weekStartDate.Month - 1); // Adjustment because JavaScript months are zero-offset
          output.Write("_");
          output.Write(weekStartDate.Day);
          output.Write("\"");
          output.Write(" scope=\"row\"");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(" class=\"");
          output.Write(this.SelectWeekCssClass);
          output.Write("\" width=\"");
          output.Write(columnWidth);
          output.Write("\"");
          output.Write(">");
          output.Write(this.ShowWeekNumbers ? this.DateTimeFormat.Calendar.GetWeekOfYear(weekStartDate, this.CalendarWeekRule, Utils.ConvertFirstDayOfWeekToDayOfWeek(this.FirstDayOfWeek, this.DateTimeFormat.FirstDayOfWeek)).ToString() : this.SelectWeekText);
          output.Write("</th>");
        }
        for (int j = 0; j < 7; j++, k++) //this week's days
        {
          CalendarCellInfo cellInfo = datesArray[k];
          string cellId = cellInfo.Id;
          output.Write("<td");
          output.Write(" id=\"");
          output.Write(this._clientVarName);
          output.Write("_");
          output.Write(cellId);
          output.Write("\"");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(" class=\"");
          output.Write(((CalendarCellData)_calendarCellData[cellId]).Dormant);
          output.Write("\" width=\"");
          output.Write(columnWidth);
          output.Write("\">");

          DateTime cellDate = cellInfo.Date;
          string dayText = cellInfo.Day.ToString();
          bool isOtherMonth = cellInfo.Month != 0;
          bool isSelectable = !this.DisabledDates.Contains(cellDate);
          bool isSelected = this.SelectedDates.Contains(cellDate);
          bool isToday = cellDate.Date == this.TodaysDate.Date;
          bool isWeekend = cellDate.DayOfWeek == DayOfWeek.Saturday || cellDate.DayOfWeek == DayOfWeek.Sunday;

          if (this.DayRender != null)
          {
            CalendarDayRender dayRenderInfo = new CalendarDayRender(cellDate, dayText, isOtherMonth, isSelectable, isSelected, isToday, isWeekend);
            CalendarDayRenderEventArgs e = new CalendarDayRenderEventArgs(output, dayRenderInfo);
            this.DayRender(this, e);
          }
          else
          {
            string customTemplateInstanceId = ((CalendarCellData)_calendarCellData[cellId]).TemplateInstanceId;
            if (customTemplateInstanceId != null && customTemplateInstanceId.Length > 0)
            {
              foreach (Control oTemplate in this.Controls)
              {
                if (customTemplateInstanceId == oTemplate.ID)
                {
                  oTemplate.RenderControl(output);
                }
              }
            }
            else
            {
              if (!isOtherMonth)
              {
                output.Write("<a href=\"#\">");
              }
              output.Write(cellInfo.Day);
              if (!isOtherMonth)
              {
                output.Write("</a>");
              }
            }
          }
          output.Write("</td>");
        }
        output.Write("</tr>");
      }

      output.Write("</table>");
    }

    #endregion Accessible Rendering

    #region Down-level Rendering

    internal void RenderDownLevelContent(HtmlTextWriter output)
    {
      if (this.VisibleDate == DateTime.MinValue)
      {
        this.VisibleDate = ConfinedVisibleDate(this.TodaysDate);
      }
      if (this.ControlType == CalendarControlType.Calendar)
      {
        RenderDownLevelCalendar(output);
      }
      else // if (this.ControlType == CalendarControlType.Picker)
      {
        RenderDownLevelPicker(output);
      }
    }

    private void RenderDownLevelPicker(HtmlTextWriter output)
    {
      // Output the picker textbox
      output.Write("<input type=\"text\"");
      output.WriteAttribute("class", this.PickerCssClass);
      string pickerFormatString = this.ResolvePickerFormatString();
      DateTimeFormatToken [] pickerTokens = DateTimeFormatTokenizer.Tokenize(pickerFormatString);
      output.WriteAttribute("size", TokenLengthSum(pickerTokens).ToString());
      output.WriteAttribute("value", this.TodaysDate.ToString(pickerFormatString));
      if (!this.Enabled)
      {
        output.WriteAttribute("disabled",null);
      }
      output.Write(" />");
    }

    private void RenderDownLevelCalendar(HtmlTextWriter output)
    {
      output.Write("<table id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarTable\" ");
      if (!this.Enabled)
      {
        output.Write("disabled ");
      }
      if (this.ContentCssClass != null && this.ContentCssClass != String.Empty)
      {
        output.Write("class=\"");
        output.Write(this.ContentCssClass);
        output.Write("\" ");
      }
      output.Write("onselectstart=\"return false;\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
      if (this._headerClientTemplateString != null)
      {
        output.Write("<tr id=\"");
        output.Write(this._clientVarName);
        output.Write("_HeaderTr\"");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write("><td id=\"");
        output.Write(this._clientVarName);
        output.Write("_HeaderTd\"");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        output.Write("</td></tr>");
      }
      if (this.ShowTitle)
      {
        output.Write("<tr id=\"");
        output.Write(this._clientVarName);
        output.Write("_CalendarTitleTr\"");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write("><td id=\"");
        output.Write(this._clientVarName);
        output.Write("_CalendarTitleTd\" class=\"");
        output.Write(this.TitleCssClass);
        output.Write("\"");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        this.RenderDownLevelCalendarTitle(output);
        output.Write("</td></tr>");
      }
      output.Write("<tr id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsArea\"");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write("><td");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write("><div");
      output.Write(" id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsSwapContainer\" ");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write("style=\"width:100%;height:100%;margin:0px;padding:0px;border:none;\"><table id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsSwapTable\" ");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write("cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%;\"><tr id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsSwapTr\"");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write("><td id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsSwapTd\"");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(" width=\"100%\" height=\"100%\">");
      RenderDownLevelCalendarMonths(output);
      output.Write("</td></tr></table></div></td></tr>");
      if (this._footerClientTemplateString != null)
      {
        output.Write("<tr id=\"");
        output.Write(this._clientVarName);
        output.Write("_FooterTr\"");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write("><td id=\"");
        output.Write(this._clientVarName);
        output.Write("_FooterTd\"");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        output.Write("</td></tr>");
      }
      output.Write("</table>");
    }

    private void RenderDownLevelCalendarTitle(HtmlTextWriter output)
    {
      DateTime visibleDateStart = new DateTime(this.VisibleDate.Year, this.VisibleDate.Month, 1);
      DateTime visibleDateEnd = visibleDateStart.AddMonths(this.MonthColumns * this.MonthRows).AddDays(-1);

      output.Write("<table ");
      if (!this.Enabled)
      {
        output.Write("disabled ");
      }
      output.Write("cellspacing=\"0\" border=\"0\" style=\"width:100%;\"><tr");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write("><td");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(" width=\"0%\" align=\"left\" id=\"");
      output.Write(this._clientVarName);
      output.Write("_NextPrev_Prev\"");
      bool showPrevMonth = this.ShowNextPrev && visibleDateStart > this.MinDate;
      if (showPrevMonth)
      {
        output.Write(" class=\"");
        output.Write(this.NextPrevCssClass);
        output.Write("\" onclick=\"ComponentArt_Calendar_NextPrevOnClick(this)\" onselectstart=\"return false\" onmouseover=\"ComponentArt_Calendar_NextPrevOnMouseOver(this)\" onmouseout=\"ComponentArt_Calendar_NextPrevOnMouseOut(this)\" onmousedown=\"ComponentArt_Calendar_NextPrevOnMouseDown(this)\" onmouseup=\"ComponentArt_Calendar_NextPrevOnMouseUp(this)\">");
        if (this.PrevImageUrl != null && this.PrevImageUrl.Length > 0)
        {
          output.Write("<img border=\"0\" alt=\"\"");
          if (this.PrevImageHeight > -1)
          {
            output.Write(" height=\"");
            output.Write(this.PrevImageHeight);
            output.Write("\"");
          }
          if (this.PrevImageWidth > -1)
          {
            output.Write(" width=\"");
            output.Write(this.PrevImageWidth);
            output.Write("\"");
          }
          /*
          This bit of the original JavaScript can't be easily replicated on the server:
          if (cart_browser_mozilla && document.compatMode!='BackCompat')
          {
            // Mozillas in standards-compatible mode need this adjustment or
            // they can't show an image with height less than the font height.
            titleHtml[titleHtml.length] = ' style="display:block;"';
          }
          */
          output.Write(" src=\"");
          output.Write(Utils.ConvertUrl(this.Context, this.ImagesBaseUrl, this.PrevImageUrl));
          output.Write("\" />");
        }
        else
        {
          output.Write(this.PrevText);
        }
        output.Write("</td>");
      }
      else
      {
        output.Write("></td>");
      }
      output.Write("<td");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(" align=\"Center\" style=\"width:100%;\">");
      output.Write(this.CalendarTitle());
      output.Write("</td><td");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(" width=\"0%\" align=\"right\" id=\"");
      output.Write(this._clientVarName);
      output.Write("_NextPrev_Next\"");
      bool showNextMonth = this.ShowNextPrev && visibleDateEnd <= this.MaxDate;
      if (showNextMonth)
      {
        output.Write(" class=\"");
        output.Write(this.NextPrevCssClass);
        output.Write("\" onclick=\"ComponentArt_Calendar_NextPrevOnClick(this)\" onselectstart=\"return false\" onmouseover=\"ComponentArt_Calendar_NextPrevOnMouseOver(this)\" onmouseout=\"ComponentArt_Calendar_NextPrevOnMouseOut(this)\" onmousedown=\"ComponentArt_Calendar_NextPrevOnMouseDown(this)\" onmouseup=\"ComponentArt_Calendar_NextPrevOnMouseUp(this)\">");
        if (this.NextImageUrl != null && this.NextImageUrl.Length > 0)
        {
          output.Write("<img border=\"0\" alt=\"\"");
          if (this.NextImageHeight > -1)
          {
            output.Write(" height=\"");
            output.Write(this.NextImageHeight);
            output.Write("\"");
          }
          if (this.NextImageWidth > -1)
          {
            output.Write(" width=\"");
            output.Write(this.NextImageWidth);
            output.Write("\"");
          }
          /*
          This bit of the original JavaScript can't be easily replicated on the server:
          if (cart_browser_mozilla && document.compatMode!='BackCompat')
          {
            // Mozillas in standards-compatible mode need this adjustment or
            // they can't show an image with height less than the font height.
            titleHtml[titleHtml.length] = ' style="display:block;"';
          }
          */
          output.Write(" src=\"");
          output.Write(Utils.ConvertUrl(this.Context, this.ImagesBaseUrl, this.NextImageUrl));
          output.Write("\" />");
        }
        else
        {
          output.Write(this.NextText);
        }
        output.Write("</td>");
      }
      else
      {
        output.Write("></td>");
      }
      output.Write("</tr></table>");
    }

    private string CalendarTitle()
    {
      switch (this.TitleType)
      {
        case ComponentArt.Web.UI.CalendarTitleType.TodayDateText:
          return this.TodaysDate.ToString(this.TitleDateFormat, this.DateTimeFormat);
        case ComponentArt.Web.UI.CalendarTitleType.VisibleDateText:
          return this.VisibleDate.ToString(this.TitleDateFormat, this.DateTimeFormat);
        case ComponentArt.Web.UI.CalendarTitleType.VisibleRangeText:
          DateTime visibleDateStart = new DateTime(this.VisibleDate.Year, this.VisibleDate.Month, 1);
          DateTime visibleDateEnd = visibleDateStart.AddMonths(this.MonthColumns * this.MonthRows).AddDays(-1);
          string visibleDateStartString = visibleDateStart.ToString(this.TitleDateFormat, this.DateTimeFormat);
          string visibleDateEndString = visibleDateEnd.ToString(this.TitleDateFormat, this.DateTimeFormat);
          return visibleDateStartString + this.TitleDateRangeSeparatorString + visibleDateEndString;
        //case ComponentArt.Web.UI.CalendarTitleType.VisibleDatePicker:
        //  return "unimplemented"; //UNDONE: Picker in title
        //case ComponentArt.Web.UI.CalendarTitleType.SelectedDatePicker:
        //  return "unimplemented"; //UNDONE: Picker in title
        default:
          return "unimplemented"; //TODO: Remove this lame line if possible to get it by the overzealous compiler
      }
    }

    private void RenderDownLevelCalendarMonths(HtmlTextWriter output)
    {
      int year = this.VisibleDate.Year;
      int month = this.VisibleDate.Month;
      
      output.Write("<table id=\"");
      output.Write(this._clientVarName);
      output.Write("_CalendarMonthsTable\"");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      output.Write(" border=\"0\" style=\"width:100%;\"");
      if (this.MonthSpacing >= 0)
      {
        output.Write(" cellspacing=\"");
        output.Write(this.MonthSpacing);
        output.Write("\"");
      }
      if (this.MonthPadding >= 0)
      {
        output.Write(" cellpadding=\"");
        output.Write(this.MonthPadding);
        output.Write("\"");
      }
      output.Write(">");
      for (int row=0; row<this.MonthRows; row++)
      {
        output.Write("<tr");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        for (int col=0; col<this.MonthColumns; col++)
        {
          output.Write("<td");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(">");
          
          RenderDownLevelCalendarMonth(output, year, month);
          
          output.Write("</td>");
          month++;
          if (month > 12)
          {
            month = 1;
            year++;
          }
        }
        output.Write("</tr>");
      }
      output.Write("</table>");
    }

    #endregion Down-level Rendering

    /// <summary>
    /// This struct is used in down-level rendering algorithm.
    /// </summary>
    private struct CalendarCellInfo
    {
      public DateTime Date;
      public string Id;
      public bool IsSelected;
      public bool IsDisabled;
      public int Day;
      public int Month;
      public CalendarDay Custom;
      
      public CalendarCellInfo(DateTime date, string id, bool isSelected, bool isDisabled, int day, int month, CalendarDay custom)
      {
        this.Date = date;
        this.Id = id;
        this.IsSelected = isSelected;
        this.IsDisabled = isDisabled;
        this.Day = day;
        this.Month = month;
        this.Custom = custom;
      }
    }
    
    /// <summary>
    /// This struct is used in down-level rendering algorithm.
    /// </summary>
    private struct CalendarCellData
    {
      public string Dormant;
      public string Hover;
      public string Active;
      public DateTime Date;
      public string TemplateInstanceId;
      
      public CalendarCellData(string dormant, string hover, string active, DateTime date, string templateInstanceId)
      {
        this.Dormant = dormant;
        this.Hover = hover;
        this.Active = active;
        this.Date = date;
        this.TemplateInstanceId = templateInstanceId;
      }
    }
    
    private void PopulateCalendarCellData(CalendarCellInfo[] cellInfoArray)
    {
      foreach (CalendarCellInfo cellInfo in cellInfoArray)
      {
        this._calendarCellData[cellInfo.Id] = CalculateCalendarCellData(cellInfo);
      }  
    }
    
    private CalendarCellData CalculateCalendarCellData(CalendarCellInfo cellInfo)
    {
      string[] dormantClasses = new string[8];
      string[] hoverClasses = new string[8];
      string[] activeClasses = new string[8];
  
      int i = 0;
      if (this.DayCssClass != null && this.DayCssClass.Length > 0)
      {
        activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.DayCssClass;
      }
      if (this.DayHoverCssClass != null && this.DayHoverCssClass.Length > 0)
      {
        activeClasses[i] = hoverClasses[i] = this.DayHoverCssClass;
      }
      if (this.DayActiveCssClass != null && this.DayActiveCssClass.Length > 0)
      {
        activeClasses[i] = this.DayActiveCssClass;
      }
  
      i++;
      DayOfWeek dayOfWeek = cellInfo.Date.DayOfWeek;
      if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
      {
        if (this.WeekendDayCssClass != null && this.WeekendDayCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.WeekendDayCssClass;
        }
        if (this.WeekendDayHoverCssClass != null && this.WeekendDayHoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = this.WeekendDayHoverCssClass;
        }
        if (this.WeekendDayActiveCssClass != null && this.WeekendDayActiveCssClass.Length > 0)
        {
          activeClasses[i] = this.WeekendDayActiveCssClass;
        }
      }
  
      i++;
      if (cellInfo.Month != 0)
      {
        if (this.OtherMonthDayCssClass != null && this.OtherMonthDayCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.OtherMonthDayCssClass;
        }
        if (this.OtherMonthDayHoverCssClass != null && this.OtherMonthDayHoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = this.OtherMonthDayHoverCssClass;
        }
        if (this.OtherMonthDayActiveCssClass != null && this.OtherMonthDayActiveCssClass.Length > 0)
        {
          activeClasses[i] = this.OtherMonthDayActiveCssClass;
        }
      }
  
      i++;
      if (cellInfo.IsDisabled)
      {
        if (this.DisabledDayCssClass != null && this.DisabledDayCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.DisabledDayCssClass;
        }
        if (this.DisabledDayHoverCssClass != null && this.DisabledDayHoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = this.DisabledDayHoverCssClass;
        }
        if (this.DisabledDayActiveCssClass != null && this.DisabledDayActiveCssClass.Length > 0)
        {
          activeClasses[i] = this.DisabledDayActiveCssClass;
        }
      }

      i++;
      if (cellInfo.Date < this.MinDate || this.MaxDate < cellInfo.Date)
      {
        if (this.OutOfRangeDayCssClass != null && this.OutOfRangeDayCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.OutOfRangeDayCssClass;
        }
        if (this.OutOfRangeDayHoverCssClass != null && this.OutOfRangeDayHoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = this.OutOfRangeDayHoverCssClass;
        }
        if (this.OutOfRangeDayActiveCssClass != null && this.OutOfRangeDayActiveCssClass.Length > 0)
        {
          activeClasses[i] = this.OutOfRangeDayActiveCssClass;
        }
      }

      i++;
      if (cellInfo.Date == this.TodaysDate)
      {
        if (this.TodayDayCssClass != null && this.TodayDayCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.TodayDayCssClass;
        }
        if (this.TodayDayHoverCssClass != null && this.TodayDayHoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = this.TodayDayHoverCssClass;
        }
        if (this.TodayDayActiveCssClass != null && this.TodayDayActiveCssClass.Length > 0)
        {
          activeClasses[i] = this.TodayDayActiveCssClass;
        }
      }

      i++;
      if (cellInfo.IsSelected)
      {
        if (this.SelectedDayCssClass != null && this.SelectedDayCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = this.SelectedDayCssClass;
        }
        if (this.SelectedDayHoverCssClass != null && this.SelectedDayHoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = this.SelectedDayHoverCssClass;
        }
        if (this.SelectedDayActiveCssClass != null && this.SelectedDayActiveCssClass.Length > 0)
        {
          activeClasses[i] = this.SelectedDayActiveCssClass;
        }
      }
  
      i++;
      string templateInstanceId = null;
      if (cellInfo.Custom != null)
      {
        if (cellInfo.Custom.CssClass != null && cellInfo.Custom.CssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = dormantClasses[i] = cellInfo.Custom.CssClass;
        }
        if (cellInfo.Custom.HoverCssClass != null && cellInfo.Custom.HoverCssClass.Length > 0)
        {
          activeClasses[i] = hoverClasses[i] = cellInfo.Custom.HoverCssClass;
        }
        if (cellInfo.Custom.ActiveCssClass != null && cellInfo.Custom.ActiveCssClass.Length > 0)
        {
          activeClasses[i] = cellInfo.Custom.ActiveCssClass;
        }
        if (cellInfo.Custom.TemplateId != null && cellInfo.Custom.TemplateId.Length > 0)
        {
          templateInstanceId = this.ClientID + "_" + cellInfo.Date.ToString("yyyyMMdd");
        }
      }

      return new CalendarCellData(ComposeCssClasses(dormantClasses), ComposeCssClasses(hoverClasses), ComposeCssClasses(activeClasses), cellInfo.Date, templateInstanceId);
    }
    
    private string ComposeCssClasses(string[] cssClassArray)
    {
      StringBuilder result = new StringBuilder();
      bool firstClass = true;
      foreach (string cssClass in cssClassArray)
      {
        if (cssClass != null && cssClass != String.Empty)
        {
          if (firstClass) firstClass = false; else result.Append(" "); // insert a space between classes
          result.Append(cssClass);
        }
      }
      return result.ToString();
    }

    private void RenderDownLevelCalendarMonth(HtmlTextWriter output, int year, int month)
    {
      DateTime firstDayOfMonth = new DateTime(year, month, 1);
      int daysPrevMonthShowing = (Utils.ConvertDayOfWeekToJsDay(firstDayOfMonth.DayOfWeek) - Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + 7) % 7;
      CalendarCellInfo[] datesArray = new CalendarCellInfo[6*7]; /* 6-week must for now */
      int datesArrayLength = 0; // Keeps track of the index of datesArray at which to place the next CalendarCellInfo
      if (daysPrevMonthShowing > 0)
      {
        int daysPrevMonth = DaysInMonth(year, month-1); // Note: this will work even if the month ends up negative
        for (int i = daysPrevMonth - daysPrevMonthShowing + 1; i <= daysPrevMonth; i++)
        {
          DateTime cellDate = CreateDate(year, month-1, i);
          datesArray[datesArrayLength] = new CalendarCellInfo(cellDate, CalendarCellDateId(year, month-1, i, "1"), this.SelectedDates.Contains(cellDate), this.DisabledDates.Contains(cellDate), i, -1, this.CustomDays[cellDate]);
          datesArrayLength++;
        }
      }
      int daysThisMonth = DaysInMonth(year, month);
      for (int i = 1; i <= daysThisMonth; i++)
      {
        DateTime cellDate = CreateDate(year, month, i);
        datesArray[datesArrayLength] = new CalendarCellInfo(cellDate, CalendarCellDateId(year, month, i, "0"), this.SelectedDates.Contains(cellDate), this.DisabledDates.Contains(cellDate), i, 0, this.CustomDays[cellDate]);
        datesArrayLength++;
      }
      int daysNextMonthShowing = 6*7 /* 6-week must, top-aligned is the only option for now */ - daysPrevMonthShowing - daysThisMonth;  
      for (int i = 1; i <= daysNextMonthShowing; i++)
      {
        DateTime cellDate = CreateDate(year, month+1, i);
        datesArray[datesArrayLength] = new CalendarCellInfo(cellDate, CalendarCellDateId(year, month+1, i, "1"), this.SelectedDates.Contains(cellDate), this.DisabledDates.Contains(cellDate), i, 1, this.CustomDays[cellDate]);
        datesArrayLength++;
      }
      // At this point, datesArray has been fully populated
      output.Write("<table");
      if (!this.Enabled)
      {
        output.Write(" disabled");
      }
      if (this.CellSpacing >= 0)
      {
        output.Write(" cellspacing=\"");
        output.Write(this.CellSpacing);
        output.Write("\"");
      }
      if (this.CellPadding >= 0)
      {
        output.Write(" cellpadding=\"");
        output.Write(this.CellPadding);
        output.Write("\"");
      }
      if (this.ShowGridLines)
      {
        output.Write(" rules=\"all\" border=\"1\"");
      }
      else
      {
        output.Write(" border=\"0\"");
      }
      output.Write(" class=\"");
      output.Write(this.MonthCssClass);
      output.Write("\">");
      bool showSelectorColumn = this.AllowMonthSelection || this.AllowWeekSelection || this.ShowWeekNumbers;
      if (this.ShowMonthTitle)
      {
        output.Write("<tr");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write("><td");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(" align=\"center\" class=\"");
        output.Write(this.MonthTitleCssClass);
        output.Write("\" colspan=\"");
        output.Write(showSelectorColumn ? 8 : 7);
        output.Write("\">");
        output.Write(this.MonthNames[month-1]); // -1 is there because we're switching between one- and zero-based.
        output.Write(" ");
        output.Write(year);
        output.Write("</td></tr>");
      }
      if (this.ShowDayHeader)
      {
        output.Write("<tr");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        if (showSelectorColumn)
        {
          output.Write("<td id=\"");
          output.Write(this._clientVarName);
          output.Write("_MS_");
          output.Write(year);
          output.Write("_");
          output.Write(month); 
          output.Write("\"");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(" class=\"");
          output.Write(this.SelectMonthCssClass);
          output.Write("\"");
          if (this.AllowMonthSelection)
          {
            output.Write(" onclick=\"ComponentArt_Calendar_MonthSelectorOnClick(this,event)\" onmouseover=\"ComponentArt_Calendar_MonthSelectorOnMouseOver(this)\" onmouseout=\"ComponentArt_Calendar_MonthSelectorOnMouseOut(this)\" onmousedown=\"ComponentArt_Calendar_MonthSelectorOnMouseDown(this)\" onmouseup=\"ComponentArt_Calendar_MonthSelectorOnMouseUp(this)\"");
          }
          output.Write(">");
          output.Write(this.SelectMonthText);
          output.Write("</td>");
        }
        for (int i=0; i<7; i++)  // day names
        {
          output.Write("<td");
          if (this.DayHeaderCssClass != null && this.DayHeaderCssClass.Length > 0)
          {
            output.Write(" class=\"");
            output.Write(this.DayHeaderCssClass);
            output.Write("\"");
            if (!this.Enabled)
            {
              output.Write(" disabled");
            }
          }
          output.Write(">");
          switch (this.DayNameFormat)
          {
            case DayNameFormat.FirstLetter:
              output.Write(this.DayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7].Substring(0,1));
              break;
            case DayNameFormat.FirstTwoLetters:
              output.Write(this.DayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7].Substring(0,2));
              break;
            case DayNameFormat.Full:
              output.Write(this.DayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7]);
              break;
            case DayNameFormat.Short:
              output.Write(this.AbbreviatedDayNames[(Utils.ConvertFirstDayOfWeekToJsDay(this.FirstDayOfWeek, this._defaultFirstDayOfWeek) + i) % 7]);
              break;
          }
          output.Write("</td>");
        }
        output.Write("</tr>");
      }

      PopulateCalendarCellData(datesArray);

      string columnWidth = showSelectorColumn ? "12%" : "14%";
      int k = 0; // k will always equal i*7+j in the loops below. Introduced to cut down on multiplications.
      for (int i=0; i<6; i++) //weeks
      {
        output.Write("<tr");
        if (!this.Enabled)
        {
          output.Write(" disabled");
        }
        output.Write(">");
        if (showSelectorColumn)
        {
          DateTime weekStartDate = (DateTime) datesArray[k].Date;
          output.Write("<td id=\"");
          output.Write(this._clientVarName);
          output.Write("_WS_");
          output.Write(weekStartDate.Year);
          output.Write("_");
          output.Write(weekStartDate.Month-1); // Adjustment because JavaScript months are zero-offset
          output.Write("_");
          output.Write(weekStartDate.Day);
          output.Write("\"");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(" class=\"");
          output.Write(this.SelectWeekCssClass);
          output.Write("\" width=\"");
          output.Write(columnWidth);
          output.Write("\"");
          if (this.AllowWeekSelection)
          {
            output.Write(" onclick=\"ComponentArt_Calendar_WeekSelectorOnClick(this,event)\" onmouseover=\"ComponentArt_Calendar_WeekSelectorOnMouseOver(this)\" onmouseout=\"ComponentArt_Calendar_WeekSelectorOnMouseOut(this)\" onmousedown=\"ComponentArt_Calendar_WeekSelectorOnMouseDown(this)\" onmouseup=\"ComponentArt_Calendar_WeekSelectorOnMouseUp(this)\"");
          }
          output.Write(">");
          output.Write(this.ShowWeekNumbers ?  this.DateTimeFormat.Calendar.GetWeekOfYear(weekStartDate, this.CalendarWeekRule, Utils.ConvertFirstDayOfWeekToDayOfWeek(this.FirstDayOfWeek, this.DateTimeFormat.FirstDayOfWeek)).ToString() : this.SelectWeekText);
          output.Write("</td>");
        }
        for (int j=0; j<7; j++,k++) //this week's days
        {
          CalendarCellInfo cellInfo = datesArray[k];
          string cellId = cellInfo.Id;
          output.Write("<td");
          if (this.AllowDaySelection)
          {
            output.Write(" onmousedown=\"ComponentArt_Calendar_CalendarDayOnMouseDown(this)\" onmouseup=\"ComponentArt_Calendar_CalendarDayOnMouseUp(this)\" onmouseover=\"ComponentArt_Calendar_CalendarDayOnMouseOver(this)\" onmouseout=\"ComponentArt_Calendar_CalendarDayOnMouseOut(this)\" onclick=\"ComponentArt_Calendar_CalendarDayOnClick(this,event)\"");
          }
          output.Write(" id=\"");
          output.Write(this._clientVarName);
          output.Write("_");
          output.Write(cellId);
          output.Write("\"");
          if (!this.Enabled)
          {
            output.Write(" disabled");
          }
          output.Write(" class=\"");
          output.Write(((CalendarCellData)_calendarCellData[cellId]).Dormant);
          output.Write("\" width=\"");
          output.Write(columnWidth);
          output.Write("\">");
          if (this.DayRender != null)
          {
            DateTime cellDate = cellInfo.Date;
            string dayText = cellInfo.Day.ToString();
            bool isOtherMonth = cellInfo.Month != 0;
            bool isSelectable = !this.DisabledDates.Contains(cellDate);
            bool isSelected = this.SelectedDates.Contains(cellDate);
            bool isToday = cellDate.Date == this.TodaysDate.Date;
            bool isWeekend = cellDate.DayOfWeek == DayOfWeek.Saturday || cellDate.DayOfWeek == DayOfWeek.Sunday;
            CalendarDayRender dayRenderInfo = new CalendarDayRender(cellDate, dayText, isOtherMonth, isSelectable, isSelected, isToday, isWeekend);
            CalendarDayRenderEventArgs e = new CalendarDayRenderEventArgs(output, dayRenderInfo);
            this.DayRender(this, e);
          }          
          else
          {
            string customTemplateInstanceId = ((CalendarCellData)_calendarCellData[cellId]).TemplateInstanceId;
            if (customTemplateInstanceId != null && customTemplateInstanceId.Length > 0)
            {
              foreach (Control oTemplate in this.Controls)
              {
                if (customTemplateInstanceId == oTemplate.ID)
                {
                  oTemplate.RenderControl(output);
                }
              }
            }
            else
            {
              output.Write(cellInfo.Day);
            }
          }
          output.Write("</td>");
        }
        output.Write("</tr>");
      }
      
      output.Write("</table>");
    }
    
    /// <summary>Returns the date string that is in effect for the Picker.</summary>
    /// <remarks>Really only applicable when <see cref="ControlType"/> is <b>Picker</b>.</remarks>
    private string ResolvePickerFormatString()
    {
      switch (this.PickerFormat)
      {
        case DateTimeFormatType.Long: return this.DateTimeFormat.LongDatePattern;
        case DateTimeFormatType.Short: return this.DateTimeFormat.ShortDatePattern;
        case DateTimeFormatType.LongTime: return this.DateTimeFormat.LongTimePattern;
        case DateTimeFormatType.ShortTime: return this.DateTimeFormat.ShortTimePattern;
        case DateTimeFormatType.Custom: return this.PickerCustomFormat;
        default: return null; //unreachable actually, but tell that to the compiler
      }
    }

    /// <summary>Given an array of tokens, it adds up the lengths for all tokens.</summary>
    private int TokenLengthSum(DateTimeFormatToken[] tokens)
    {
      int result = 0;
      foreach (DateTimeFormatToken token in tokens)
      {
        if (token.Type == DateTimeFormatTokenType.Literal)
        {
          result += token.Value.Length;
        }
        else
        {
          result += SymbolTokenLength(token);
        }
      }
      return result;
    }

    /// <summary>Given a DateTimeFormatToken of Type Symbol, returns its maximum length.</summary>
    private int SymbolTokenLength(DateTimeFormatToken symbolToken)
    {
      switch (symbolToken.Value)
      {
        case "d": return 2;
        case "dd": return 2;
        case "ddd": return MaxStringLength(this.AbbreviatedDayNames);
        case "dddd": return MaxStringLength(this.DayNames);
        case "h": return 2;
        case "hh": return 2;
        case "H": return 2;
        case "HH": return 2;
        case "m": return 2;
        case "mm": return 2;
        case "M": return 2;
        case "MM": return 2;
        case "MMM": return MaxStringLength(this.AbbreviatedMonthNames);
        case "MMMM": return MaxStringLength(this.MonthNames);
        case "s": return 2;
        case "ss": return 2;
        case "t": case "T": return 1;
        case "tt": case "TT": return Math.Max(this.AMDesignator.Length, this.PMDesignator.Length);
        case "y": return 2;
        case "yy": return 2;
        case "yyy": case "yyyy": return 4;
        default: return 0; // this line is actually unreachable
      }
    }

    /// <summary>Simply returns the length of the longest string in the given string array.
    /// If the array contains no non-null strings, -1 is returned.</summary>
    private int MaxStringLength(string[] stringArray)
    {
      int maxLength = -1;
      foreach (string s in stringArray)
      {
        if (s != null && s.Length > maxLength)
        {
          maxLength = s.Length;
        }
      }
      return maxLength;
    }

    /// <summary>
    /// Make sure that values of various properties make sense with respect to each other. Raise exceptions if they do not.
    /// </returns>
    private void VerifyDependentProperties()
    {
      if (this.MinDate >= this.MaxDate)
      {
        throw new Exception(String.Format("{0}.MinDate may not be greater than {0}.MaxDate.", this.ID));
      }
      if (this.SelectedDates.Count > 0)
      {
        if (this.SelectedDates[0] < this.MinDate)
        {
          throw new Exception(String.Format("No member of {0}'s SelectedDates collection may be less than {0}.MinDate.", this.ID));
        }
        if (this.MaxDate < this.SelectedDates[this.SelectedDates.Count - 1])
        {
          throw new Exception(String.Format("No member of {0}'s SelectedDates collection may be greater than {0}.MaxDate.", this.ID));
        }
      }
      if (this.VisibleDate != DateTime.MinValue) // if VisibleDate is defined
      {
        if (this.VisibleDate < this.MinDate)
        {
          throw new Exception(String.Format("{0}.VisibleDate may not be less than {0}.MinDate.", this.ID));
        }
        if (this.MaxDate < this.VisibleDate)
        {
          throw new Exception(String.Format("{0}.VisibleDate may not be greater than {0}.MaxDate.", this.ID));
        }
      }
      if (this.MonthColumns < this.VisibleMonthColumn)
      {
        throw new Exception(String.Format("{0}.MonthColumns may not be less than {0}.VisibleMonthColumn.", this.ID));
      }
      if (this.MonthRows < this.VisibleMonthRow)
      {
        throw new Exception(String.Format("{0}.MonthRows may not be less than {0}.VisibleMonthRow.", this.ID));
      }
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="SelectionChanged"/> event of <see cref="Calendar"/> class.
    /// </summary>
    public delegate void SelectionChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// SelectionChanged event.
    /// </summary>
    public event SelectionChangedEventHandler SelectionChanged;

    private void OnSelectionChanged(EventArgs e) 
    {
      if (this.SelectionChanged != null) 
      {
        this.SelectionChanged(this, e);
      }
    }
    
    /// <summary>
    /// Delegate for <see cref="VisibleDateChanged"/> event of <see cref="Calendar"/> class.
    /// </summary>
    public delegate void VisibleDateChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// VisibleDateChanged event.
    /// </summary>
    public event VisibleDateChangedEventHandler VisibleDateChanged;

    private void OnVisibleDateChanged(EventArgs e) 
    {
      if (this.VisibleDateChanged != null) 
      {
        this.VisibleDateChanged(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="DayRender"/> event of <see cref="Calendar"/> class.
    /// </summary>
    public delegate void DayRenderEventHandler(object sender, CalendarDayRenderEventArgs e);

    /// <summary>
    /// DayRender event.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This event will not fire unless the <see cref="WebControl.ClientTarget" /> property is set to <code>ClientTargetLevel.Downlevel</code>.
    /// </para>
    /// </remarks>
    public event DayRenderEventHandler DayRender;

    #endregion

  }

  #region Supporting code

  /// <summary>
  /// A collection of <see cref="CalendarDayCustomTemplate"/> objects.
  /// </summary>
  public class CalendarDayCustomTemplateCollection : CollectionBase
  {
    public new CalendarDayCustomTemplate this[int index]
    {
      get
      {
        return (CalendarDayCustomTemplate)base.List[index];
      }
      set
      {
        base.List[index] = value;
      }
    }
    public new int Add(CalendarDayCustomTemplate template)
    {
      return this.List.Add(template);
    }
  }

  /// <summary>
  /// A collection of <see cref="System.DateTime"/> objects.
  /// </summary>
  /// <remarks>
  /// This collection behaves exactly like <see cref="System.Web.UI.WebControls.SelectedDatesCollection"/>.
  /// Unfortunately the name of <b>SelectedDatesCollection</b> is too dismissive, considering that 
  /// <see cref="Calendar"/> control uses it for other purposes as well (such as disabled dates).
  /// And since <b>SelectedDatesCollection</b> is sealed, we could not rename it by simply inheriting from it, 
  /// and we had to recreate it.
  /// </remarks>
  public class DateCollection : ICollection, IEnumerable
  {
    internal Calendar _parentControl = null;

    internal bool _isInPicker
    {
      get
      {
        return (this._parentControl != null) &&  (this._parentControl.ControlType == CalendarControlType.Picker);
      }
    }

    private ArrayList _dateList;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateCollection"/> class with the specified Date list.
    /// </summary>
    /// <param name="dateList">A <see cref="System.Collections.ArrayList"/> that represents a collection of Dates.</param>
    public DateCollection(ArrayList dateList)
    {
      this._dateList = dateList;
    }
    internal DateCollection(ArrayList dateList, Calendar parentControl)
    {
      this._dateList = dateList;
      this._parentControl = parentControl;
    }

    #region IEnumerable implementation

    /// <summary>
    /// Implements <see cref="IEnumerable.GetEnumerator"/> method of <see cref="IEnumerable"/> interface.
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      return this._dateList.GetEnumerator();
    }

    #endregion

    #region ICollection implementation

    /// <summary>
    /// Implements <see cref="ICollection.Count"/> property of <see cref="ICollection"/> interface.
    /// </summary>
    public int Count
    {
      get
      {
        return this._dateList.Count;
      }
    }
    
    /// <summary>
    /// Implements <see cref="ICollection.IsSynchronized"/> property of <see cref="ICollection"/> interface.
    /// </summary>
    public bool IsSynchronized
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Implements <see cref="ICollection.SyncRoot"/> property of <see cref="ICollection"/> interface.
    /// </summary>
    public object SyncRoot
    {
      get
      {
        return this;
      }
    }
    
    /// <summary>
    /// Implements <see cref="ICollection.CopyTo"/> method of <see cref="ICollection"/> interface.
    /// </summary>
    public void CopyTo(Array array, int index)
    {
      IEnumerator enumerator = this.GetEnumerator();
      while (enumerator.MoveNext())
      {
        array.SetValue(enumerator.Current, index++);
      }
    }

    #endregion

    /// <summary>
    /// Performs a binary search for the given date.
    /// </summary>
    /// <param name="date">Date to search for.</param>
    /// <param name="index">Returns the index where date is (if it exists) or should be (if it doesn't).</param>
    /// <returns>True if the given date is found, false if it is not.</returns>
    private bool FindIndex(DateTime date, out int index)
    {
      int lowerBound = 0;
      int upperBound = this.Count;
      while (lowerBound < upperBound)
      {
        index = (lowerBound + upperBound) / 2;
        if (date == this[index])
        {
          return true; // found
        }
        if (date < this[index])
        {
          upperBound = index; // iterate into the lower half
        }
        else // (this[index] < date)
        {
          lowerBound = index + 1; // iterate into the upper half
        }
      }
      index = lowerBound;
      return false; // not found
    }

    /// <summary>
    /// Add the given DateTime value to the collection.
    /// </summary>
    public void Add(DateTime datetime)
    {
      int index;
      if (!this.FindIndex(this._isInPicker ? datetime : datetime.Date, out index))
      {
        this._dateList.Insert(index, this._isInPicker ? datetime : datetime.Date);
      }
    }

    /// <summary>
    /// Clear the collection.
    /// </summary>
    public void Clear()
    {
      this._dateList.Clear();
    }
    
    /// <summary>
    /// Whether the collection contains the given DateTime value.
    /// </summary>
    public bool Contains(DateTime datetime)
    {
      int dummy;
      return this.FindIndex(this._isInPicker ? datetime : datetime.Date, out dummy);
    }

    /// <summary>
    /// Remove the given DateTime value from the collection.
    /// </summary>
    public void Remove(DateTime datetime)
    {
      int index;
      if (this.FindIndex(this._isInPicker ? datetime : datetime.Date, out index))
      {
        this._dateList.RemoveAt(index);
      }
    }

    /// <summary>
    /// Clear the collection, then add all the dates between the given dates (inclusive) to the collection.
    /// </summary>
    public void SelectRange(DateTime fromDate, DateTime toDate)
    {
      this._dateList.Clear();
      if (fromDate <= toDate)
      {
        for (DateTime dateIterator = (this._isInPicker ? fromDate : fromDate.Date); dateIterator <= (this._isInPicker ? toDate : toDate.Date); dateIterator = dateIterator.AddDays(1))
        {
          this._dateList.Add(dateIterator);
        }
      }
    }

    /// <summary>
    /// Whether the collection is read-only.  Always false.
    /// </summary>
    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Access the DateTime element of the collection by its index number.
    /// </summary>
    public DateTime this[int index]
    {
      get
      {
        return (DateTime) this._dateList[index];
      }
    }
  }

  /// <summary>
  /// Specifies custom look for a day cell of a <see cref="Calendar"/> control.
  /// </summary>
  public class CalendarDay
  {
    private string _activeCssClass;
    /// <summary>
    /// Gets or sets the active CSS class for the day cell.
    /// </summary>
    public string ActiveCssClass
    {
      get
      {
        return this._activeCssClass;
      }
      set
      {
        this._activeCssClass = value;
      }
    }

    private string _cssClass;
    /// <summary>
    /// Gets or sets the CSS class for the day cell.
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

    private DateTime _date;
    /// <summary>
    /// Gets or sets the date this day cell corresponds to.
    /// </summary>
    public DateTime Date
    {
      get
      {
        return this._date;
      }
      set
      {
        this._date = value.Date;
      }
    }

    private string _hoverCssClass;
    /// <summary>
    /// Gets or sets the hover CSS class for the day cell.
    /// </summary>
    public string HoverCssClass
    {
      get
      {
        return this._hoverCssClass;
      }
      set
      {
        this._hoverCssClass = value;
      }
    }

    private string _templateId;
    /// <summary>
    /// Gets or sets the ID of the template this day cell uses.
    /// </summary>
    public string TemplateId
    {
      get
      {
        return this._templateId;
      }
      set
      {
        this._templateId = value;
      }
    }

    public CalendarDay()
    {
    }

    public CalendarDay(DateTime date, string cssClass, string hoverCssClass, string activeCssClass, string templateId)
    {
      this._date = date;
      this._cssClass = cssClass;
      this._hoverCssClass = hoverCssClass;
      this._activeCssClass = activeCssClass;
      this._templateId = templateId;
    }

    public string ToString(string controlClientId)
    {
      StringBuilder result = new StringBuilder();
      result.Append("[");
      result.Append(Utils.ConvertDateTimeToJsDate(this.Date));
      result.Append(",");
      result.Append(Utils.ConvertStringToJSString(this.CssClass));
      result.Append(",");
      result.Append(Utils.ConvertStringToJSString(this.HoverCssClass));
      result.Append(",");
      result.Append(Utils.ConvertStringToJSString(this.ActiveCssClass));
      if (this.TemplateId != null && this.TemplateId.Length > 0)
      {
        result.Append(",");
        result.Append(Utils.ConvertStringToJSString(this.TemplateId));
        result.Append(",");
        result.Append(Utils.ConvertStringToJSString(controlClientId+"_"+this.Date.ToString("yyyyMMdd")));
      }
      result.Append("]");
      return result.ToString();
    }
  }

  internal class CalendarDayCollectionEnumerator : IEnumerator
  {
    private IEnumerator _calendarDayListEnumerator;
    public CalendarDayCollectionEnumerator(IEnumerator calendarDayListEnumerator)
    {
      this._calendarDayListEnumerator = calendarDayListEnumerator;
    }
    public bool MoveNext()
    {
      return this._calendarDayListEnumerator.MoveNext();
    }
    public void Reset()
    {
      this._calendarDayListEnumerator.Reset();
    }
    public Object Current
    {
      get
      {
        return ((DictionaryEntry) this._calendarDayListEnumerator.Current).Value;
      }
    }
  }

  /// <summary>
  /// A collection of <see cref="CalendarDay"/> objects.
  /// </summary>
  public class CalendarDayCollection : ICollection, IEnumerable
  {
    private SortedList _calendarDayList;

    public CalendarDayCollection(SortedList calendarDayList)
    {
      this._calendarDayList = calendarDayList;
    }

    #region IEnumerable implementation

    /// <summary>
    /// Implements <see cref="IEnumerable.GetEnumerator"/> method of <see cref="IEnumerable"/> interface.
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      return new CalendarDayCollectionEnumerator(this._calendarDayList.GetEnumerator());
    }

    #endregion

    #region ICollection implementation

    /// <summary>
    /// Implements <see cref="ICollection.Count"/> property of <see cref="ICollection"/> interface.
    /// </summary>
    public int Count
    {
      get
      {
        return this._calendarDayList.Count;
      }
    }

    /// <summary>
    /// Implements <see cref="ICollection.IsSynchronized"/> property of <see cref="ICollection"/> interface.
    /// </summary>
    public bool IsSynchronized
    {
      get
      {
        return this._calendarDayList.IsSynchronized;
      }
    }

    /// <summary>
    /// Implements <see cref="ICollection.SyncRoot"/> property of <see cref="ICollection"/> interface.
    /// </summary>
    public object SyncRoot
    {
      get
      {
        return this._calendarDayList.SyncRoot;
      }
    }

    /// <summary>
    /// Implements <see cref="ICollection.CopyTo"/> method of <see cref="ICollection"/> interface.
    /// </summary>
    public void CopyTo(Array array, int index)
    {
      IEnumerator enumerator = this.GetEnumerator();
      while (enumerator.MoveNext())
      {
        array.SetValue(enumerator.Current, index++);
      }
    }

    #endregion

    /// <summary>
    /// Add the given CalendarDay object to the collection.
    /// </summary>
    public void Add(CalendarDay calendarDay)
    {
      calendarDay.Date = calendarDay.Date.Date;
      this._calendarDayList.Add(calendarDay.Date, calendarDay);
    }
    
    /// <summary>
    /// Clear the collection.
    /// </summary>
    public void Clear()
    {
      this._calendarDayList.Clear();
    }

    /// <summary>
    /// Check whether the collection contains the given CalendarDay object.
    /// </summary>
    public bool Contains(CalendarDay calendarDay)
    {
      return this._calendarDayList.Contains(calendarDay.Date.Date);
    }

    /// <summary>
    /// Remove the given CalendarDay object from the collection.
    /// </summary>
    public void Remove(CalendarDay calendarDay)
    {
      this._calendarDayList.Remove(calendarDay.Date.Date);
    }

    /// <summary>
    /// Whether the collection is read-only.  Always false.
    /// </summary>
    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Access a CalendarDay in the collection by index.
    /// </summary>
    public CalendarDay this[int index]
    {
      get
      {
        return (CalendarDay) this._calendarDayList.GetByIndex(index);
      }
    }

    /// <summary>
    /// Access a CalendarDay in the collection by its date.
    /// </summary>
    public CalendarDay this[DateTime date]
    {
      get
      {
        return (CalendarDay) this._calendarDayList[date.Date];
      }
    }

    public string ToString(string controlClientId)
    {
      StringBuilder result = new StringBuilder();
      result.Append("[");
      foreach (CalendarDay cd in this)
      {
        result.Append(cd.ToString(controlClientId));
        result.Append(",");
      }
      if (result.Length > 1)
      {
        result.Remove(result.Length-1, 1); // remove the last comma
      }
      result.Append("]");
      return result.ToString();
    }
  }
  
  /// <summary>
  /// Represents a day cell of the <see cref="Calendar"/> control as it is being rendered on the server.
  /// </summary>
  public struct CalendarDayRender
  {
    private DateTime date;
    /// <summary>
    /// The date this cell corresponds to.
    /// </summary>
    public DateTime Date
    {
      get
      {
        return this.date;
      }
    }
    
    private string dayNumberText;
    /// <summary>
    /// The number of the day of this day cell.
    /// </summary>
    /// <remarks>By default this string would be rendered as the contents of the day cell.</remarks>
    public string DayNumberText
    {
      get
      {
        return this.dayNumberText;
      }
    }
    
    private bool isOtherMonth;
    /// <summary>
    /// Whether this cell's date is in the other month.
    /// </summary>
    /// <remarks>For example, when rendering the month of August, the trailing cells also include those
    /// corresponding to a few dates in September.  This property would be true for those cells.</remarks>
    public bool IsOtherMonth
    {
      get
      {
        return this.isOtherMonth;
      }
    }
    
    private bool isSelectable;
    /// <summary>
    /// Whether this date cell is selectable.
    /// </summary>
    /// <remarks>This would be false if the cell corresponds to a disabled or an out-of-range date.</remarks>
    public bool IsSelectable
    {
      get
      {
        return this.isSelectable;
      }
    }
    
    private bool isSelected;
    /// <summary>
    /// Whether this cell's date is selected.
    /// </summary>
    public bool IsSelected
    {
      get
      {
        return this.isSelected;
      }
    }
    
    private bool isToday;
    /// <summary>
    /// Whether this cell's date corresponds to <see cref="Calendar.TodaysDate"/>.
    /// </summary>
    public bool IsToday
    {
      get
      {
        return this.isToday;
      }
    }
    
    private bool isWeekend;
    /// <summary>
    /// Whether this cell's date is a weekend day.
    /// </summary>
    public bool IsWeekend
    {
      get
      {
        return this.isWeekend;
      }
    }
    
    public CalendarDayRender(DateTime date, string dayNumberText, bool isOtherMonth, bool isSelectable, bool isSelected, bool isToday, bool isWeekend)
    {
      this.date = date;
      this.dayNumberText = dayNumberText;
      this.isOtherMonth = isOtherMonth;
      this.isSelectable = isSelectable;
      this.isSelected = isSelected;
      this.isToday = isToday;
      this.isWeekend = isWeekend;
    }  
  }
  
  /// <summary>
  /// Arguments for <see cref="Calendar.DayRender"/> server-side event of <see cref="Calendar"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class CalendarDayRenderEventArgs : EventArgs
  {
    private HtmlTextWriter output;
    /// <summary>
    /// The stream to which HTML is written.
    /// </summary>
    public HtmlTextWriter Output
    {
      get
      {
        return this.output;
      }
    }
    
    private CalendarDayRender dayRenderInfo;
    /// <summary>
    /// Rendering information for the day cell in question.
    /// </summary>
    public CalendarDayRender DayRenderInfo
    {
      get
      {
        return this.dayRenderInfo;
      }
    }
      
    public CalendarDayRenderEventArgs(HtmlTextWriter output, CalendarDayRender dayRenderInfo)
    {
      this.output = output;
      this.dayRenderInfo = dayRenderInfo;
    }
  }


  #region Template Classes

  /// <summary>
  /// Template class used for specifying customized rendering for day cells of <see cref="Calendar"/> control.
  /// </summary>
  [DefaultProperty("Template")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [ToolboxItem(false)]
  public class CalendarDayCustomTemplate : System.Web.UI.WebControls.WebControl
  {
    private ITemplate m_oTemplate;

    /// <summary>
    /// The template.
    /// </summary>
    [
    Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
    PersistenceMode(PersistenceMode.InnerProperty),
    TemplateContainer(typeof(ComponentArt.Web.UI.CalendarDayTemplateContainer)),
    NotifyParentProperty(true)
    ]
    public virtual ITemplate Template
    {
      get
      {
        return m_oTemplate;
      }
      set
      {
        m_oTemplate = value;
      }
    }
  }

  /// <summary>
  /// Naming container for a customized <see cref="CalendarDay"/> instance.
  /// </summary>
  [ToolboxItem(false)]
  public class CalendarDayTemplateContainer : Control, INamingContainer
  {
    private CalendarDay _dataItem;

    /// <summary>
    /// CalendarDayTemplateContainer constructor.
    /// </summary>
    public CalendarDayTemplateContainer(CalendarDay oDay)
    {
      _dataItem = oDay;
    }

    /// <summary>
    /// Item containing data to bind to (a CalendarDay).
    /// </summary>
    public virtual CalendarDay DataItem
    {
      get
      {
        return _dataItem;
      }
    }
  }

  #endregion

  #region Tokenizing DateTimeFormat strings

  /* enum DateTimeFormatTokenType, struct DateTimeFormatToken and class DateTimeFormatTokenizer are used to 
  * break down a date format string like:   "yyyy-MMM-d, DDD" 
  * into an array of tokens like:  (yyyy) "-" (MMM) "-" (d) ", " (DDDD) */

  internal enum DateTimeFormatTokenType
  {
    Symbol, //these include: d,dd,ddd,dddd,h,hh,H,HH,m,mm,M,MM,MMM,MMMM,s,ss,t,tt,T,TT,y,yy,yyy,yyyy;  yyy and yyyy are equivalent
    Literal //just a string constant, like ", " or "-" or "whatever"
  }

  internal struct DateTimeFormatToken
  {
    public DateTimeFormatToken(DateTimeFormatTokenType _type, string _value)
    {
      /* Note that only token's Type and Value (and Length in case of tokens of Literal type) are 
        * calculated correctly on initialization. All other properties will be recalculated later. */
      this._type = _type;
      this._value = _value;
    }
    private DateTimeFormatTokenType _type;
    public DateTimeFormatTokenType Type // Symbol or Literal
    {
      get
      {
        return _type;
      } 
      set
      {
        _type = value;
      }
    }
    private string _value;
    public string Value // Date format string like "yy" or "M" for Symbols, or the string itself for Literals
    {
      get
      {
        return _value;
      } 
      set
      {
        _value = value;
      }
    }
    public override string ToString()
    {
      return "[" + (this.Type==DateTimeFormatTokenType.Symbol ? "1" : "") + "," + Utils.ConvertStringToJSString(this.Value) + "]";
    }
  } // struct DateTimeFormatToken

  /* Provides one method - Tokenize. It breaks a date/time format string down to an array of DateTimeFormatTokens. */
  internal class DateTimeFormatTokenizer
  {

    /* Simply checks whether smallStr is a substring of bigStr at location bigIndex.
      * The work is done on the second line, the first line is to avoid exceptions. */
    private static bool IsSubStrAt(string smallStr, string bigStr, int bigIndex)
    {
      if (smallStr.Length+bigIndex > bigStr.Length) return false;
      return bigStr.Substring(bigIndex,smallStr.Length).Equals(smallStr);
    }

    public static DateTimeFormatToken[] Tokenize(string CustomDateTimeFormatString)
    {
      string input = CustomDateTimeFormatString; /* Redundant.  It's just nice to work with a short name like "input" 
                                                  * internally, but still expose a descriptive parameter name.*/
      ArrayList tokens = new ArrayList();
      int index = 0;
      bool inQuotation = false; /* Indicates whether we are currently inside of a quotation in our
									                * CustomDateTimeFormatString.  For example, for string "MMM dd'oo' yyyy",
									                * it will be true when index is pointing at one of the two o's. */
      StringBuilder curStr = new StringBuilder(); /* Used to build up the current token */
      while (index<input.Length) 
      {
        char curChar = input[index];
        if (!inQuotation)
        {
          switch (curChar.ToString())
          {
            case "'":
              inQuotation = true;
              index++;
              break;

            case "d": case "h": case "H": case "m": case "M": case "s": case "t": case "T": case "y": /* We are entering a Symbol */
              if (curStr.Length>0) /* If we already have something built up in our curStr "buffer", 
						                        * flush it, it's a Literal. (Note that only literals ever enter the buffer, all 
						                        * Symbols are dealt with here immediately, in the same loop iteration in which 
						                        * they are encountered.) */
              {
                tokens.Add(new DateTimeFormatToken(DateTimeFormatTokenType.Literal, curStr.ToString()));
                curStr = new StringBuilder();
              }
              /* The for loop may look a bit cryptic at first, but it is quite simple.
                * It tries to match MMMM, then MMM, then MM and finally M.
                * For M, d, and y the longest pattern it tries to find is of length four (MMMM, dddd, or yyyy).
                * For h, H, m, s, t, and T the longest pattern it tries to find is of length two (hh, HH, mm, ss, tt, or TT).
                * It's always guaranteed to succeed at least on the last (single-letter) try. */
              string pattern = "";
              int maximumPatternLength = 0;
              switch (curChar.ToString())
              {
                case "h": case "H": case "m": case "s": case "t": case "T":
                  maximumPatternLength = 2;
                  break;
                case "d": case "M": case "y":
                  maximumPatternLength = 4;
                  break;
              }
              for (int i = maximumPatternLength; i>=1; i--)
              {
                pattern = new string(curChar, i);
                if (IsSubStrAt(pattern, input, index)) break;
              }
              tokens.Add(new DateTimeFormatToken(DateTimeFormatTokenType.Symbol, pattern));
              index += pattern.Length;
              break;

            default: /* Just append to the literal in construction */
              curStr.Append(curChar);
              index++;
              break;
          }
        }
        else //inQuotation==true
        {
          /* While in quotation, we pay no attention to the pattern characters (MMM or d and such).
            * Instead simply treat everything as a literal character, except:
            * ' - which closes the quotation
            * '' - which inserts a ' character without closing the quotation */
          if (IsSubStrAt("''",input,index))
          {
            curStr.Append("'");
            index += 2;
          } 
          else 
          {
            if (input[index].ToString()=="'")
            {
              inQuotation = false;
              index++;
            }
            else 
            {
              curStr.Append(input[index]);
              index++;
            }
          }
        }
        if ((index>=input.Length)&&(curStr.Length>0))
          /* If we reached the end of input and have something "buffered" in curStr, flush it, it's a Literal. */
        {
          tokens.Add(new DateTimeFormatToken(DateTimeFormatTokenType.Literal, curStr.ToString()));
        }
      }
      DateTimeFormatToken[] result = new DateTimeFormatToken[tokens.Count];
      tokens.CopyTo(result);
      return result;
    } // method Tokenize

  } // class DateTimeFormatTokenizer

  #endregion // Tokenizing DateTimeFormat strings

  #region Enums

  /// <summary>Specifies the date/time format the <see cref="Calendar"/> control's picker displays.</summary>
  /// <remarks>
  /// This enumeration is used by <see cref="Calendar"/> members such as <see cref="Calendar.PickerFormat"/>.
  /// <note>The actual date/time format is affected by <b>Calendar</b>'s other settings.</note>
  /// </remarks>
  public enum DateTimeFormatType
  {
    /// <summary><see cref="Calendar"/> control displays the date value in a custom format
    /// of its <see cref="Calendar.PickerCustomFormat"/> property.</summary>
    Custom,
    /// <summary><see cref="Calendar"/> control displays the date value in the long date format 
    /// of its <see cref="Calendar.DateTimeFormat"/> property.</summary>
    Long,
    /// <summary><see cref="Calendar"/> control displays the date value in the short date format 
    /// of its <see cref="Calendar.DateTimeFormat"/> property.</summary>
    Short,
    /// <summary>The <see cref="Calendar"/> control displays the date value in the long time format 
    /// of its <see cref="Calendar.DateTimeFormat"/> property.</summary>
    LongTime,
    /// <summary>The <see cref="Calendar"/> control displays the date value in the short time format 
    /// of its <see cref="Calendar.DateTimeFormat"/> property.</summary>
    ShortTime
  }
  
  /// <summary>
  /// Specifies what a <see cref="Calendar"/> control renders on the client.
  /// </summary>
  public enum CalendarControlType
  {
    /// <summary>A browsable month-view table of dates.</summary>
    Calendar,
    /// <summary>A customized date text box.</summary>
    Picker
  }

  /// <summary>
  /// Specifies the direction of expansion for the pop-up <see cref="Calendar"/>.
  /// </summary>
  public enum CalendarPopUpExpandDirection
  {
    /// <summary>Pop-up is just below the alignment element, and their right edges are aligned.</summary>
    BelowRight,

    /// <summary>Pop-up is just below the alignment element, and their left edges are aligned.</summary>
    BelowLeft,

    /// <summary>Pop-up is just above the alignment element, and their right edges are aligned.</summary>
    AboveRight,

    /// <summary>Pop-up is just above the alignment element, and their left edges are aligned.</summary>
    AboveLeft,

    /// <summary>Pop-up is just to the right of the alignment element, and their top edges are aligned.</summary>
    RightDown,

    /// <summary>Pop-up is just to the right of the alignment element, and their bottom edges are aligned.</summary>
    RightUp,

    /// <summary>Pop-up is just to the left of the alignment element, and their top edges are aligned.</summary>
    LeftDown,

    /// <summary>Pop-up is just to the left of the alignment element, and their bottom edges are aligned.</summary>
    LeftUp
  }

  /// <summary>
  /// Specifies whether this is a pop-up <see cref="Calendar"/>.
  /// </summary>
  public enum CalendarPopUpType
  {
    /// <summary>This is a static Calendar rendered in page.</summary>
    None,
    /// <summary>This is a pop-up Calendar triggered by a client-side command.</summary>
    Custom
  }

  /// <summary>
  /// Specifies what a <see cref="Calendar"/> control renders in its title section.
  /// </summary>
  public enum CalendarTitleType
  {
    /// <summary>Static text label of today's date.</summary>
    TodayDateText,

    /// <summary>Static text label of selected date.</summary>
    SelectedDateText,

    /// <summary>Static text label of visible date.</summary>
    VisibleDateText,

    /// <summary>Static text label of the range of visible dates.</summary>
    VisibleRangeText/*,

    Picker in Calendar title options are not yet implemented.
    /// <summary>Date picker containing selected date.</summary>
    SelectedDatePicker,

    /// <summary>Date picker containing visible date.</summary>
    VisibleDatePicker*/
  }

  #endregion // Enums

  #endregion // Supporting code

  }
