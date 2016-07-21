using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
// Hwan: 2009-12-22 Will including WebControls cause issues?
using System.Web.UI.WebControls;  


namespace ComponentArt.Web.UI
{
  public class CalendarBuilder : ControlBuilder
  {
    Calendar calendar = new Calendar();
    object boundModel;
    ViewContext viewContext;

    /// <summary>
    /// Builder to dynamically generate a Calendar object on the client.  If a Model is passed, it should be one of 
    /// a) ComponentArt.Web.UI.CalendarActionResponse, b) a string parsable to a DateTime, c) a DateTime object,
    /// or d) a ComponentArt.Web.UI.DateCollection.
    /// </summary>
    /// <param name="boundModel"></param>
    /// <param name="viewContext"></param>
    public CalendarBuilder(object boundModel, ViewContext viewContext)
    {
      if (boundModel != null)
      {
        this.boundModel = boundModel;
      }
      this.viewContext = viewContext;
    }
    /// <summary>
    /// Programmatic identifier assigned to the client and server object.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CalendarBuilder ID(string id)
    {
      calendar.ID = id;

      return this;
    }
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public CalendarBuilder ClientEvents(Action<CalendarClientEventFactory> addAction)
    {
      var factory = new CalendarClientEventFactory(calendar.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of CalendarDay objects that represent the customized days in the Calendar.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public CalendarBuilder CustomDays(Action<CalendarDayFactory> addAction)
    {
      var factory = new CalendarDayFactory(calendar.CustomDays);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of DateTime objects that represent the disabled dates in the Calendar. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public CalendarBuilder DisabledDays(Action<CalendarDateFactory> addAction)
    {
      var factory = new CalendarDateFactory(calendar.DisabledDates);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of DateTime objects that represent the selected dates on the Calendar.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public CalendarBuilder SelectedDates(Action<CalendarDateFactory> addAction)
    {
      var factory = new CalendarDateFactory(calendar.SelectedDates);
      addAction(factory);
      return this;
    }

    /// <summary>
    /// One-dimensional array of type String containing the culture-specific abbreviated names of the days of the week. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AbbreviatedDayNames(string[] value)
    {
      calendar.AbbreviatedDayNames = value;
      return this;
    }
    /// <summary>
    /// One-dimensional array of type String containing the culture-specific abbreviated names of the months. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AbbreviatedMonthNames(string[] value)
    {
      calendar.AbbreviatedMonthNames = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar object allows selection of single days.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AllowDaySelection(bool value)
    {
      calendar.AllowDaySelection = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar object shows month selectors. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AllowMonthSelection(bool value)
    {
      calendar.AllowMonthSelection = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar object allows selection of multiple dates. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AllowMultipleSelection(bool value)
    {
      calendar.AllowMultipleSelection = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar object shows week selectors.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AllowWeekSelection(bool value)
    {
      calendar.AllowWeekSelection = value;
      return this;
    }
    /// <summary>
    /// String designator for hours that are "ante meridiem" (before noon).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AMDesignator(string value)
    {
      calendar.AMDesignator = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a post when the Calendar's selection changes. Default is false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AutoPostBackOnSelectionChanged(bool value)
    {
      calendar.AutoPostBackOnSelectionChanged = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a post when the Calendar's visible date changes. Default is false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AutoPostBackOnVisibleDateChanged(bool value)
    {
      calendar.AutoPostBackOnVisibleDateChanged = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AutoTheming(bool value)
    {
      calendar.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder AutoThemingCssClassPrefix(string value)
    {
      calendar.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// CSS class for the calendar portion of the Calendar object. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CalendarCssClass(string value)
    {
      calendar.CalendarCssClass = value;
      return this;
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
    public CalendarBuilder CalendarWeekRule(CalendarWeekRule value)
    {
      calendar.CalendarWeekRule = value;
      return this;
    }
    /// <summary>
    /// Amount of space between the contents of a cell and the cell's border.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CellPadding(int value)
    {
      calendar.CellPadding = value;
      return this;
    }
    /// <summary>
    /// Amount of space between cells.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CellSpacing(int value)
    {
      calendar.CellSpacing = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ClientScriptLocation(string value)
    {
      calendar.ClientScriptLocation = value;
      return this;
    }
    /// <summary>
    /// Specifies the level of client-side content that the control renders.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ClientTarget(ClientTargetLevel value)
    {
      calendar.ClientTarget = value;
      return this;
    }
    /// <summary>
    /// Whether to collapse a pop-up Calendar when a date is selected. Default is true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CollapseOnSelect(bool value)
    {
      calendar.CollapseOnSelect = value;
      return this;
    }
    /// <summary>
    /// CSS class for the inner content portion of the Calendar object. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ContentCssClass(string value)
    {
      calendar.ContentCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether the control renders as a picker or as a calendar. Default is calendar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ControlType(CalendarControlType value)
    {
      calendar.ControlType = value;
      return this;
    }
    /// <summary>
    /// CSS class rendered by the object on the client.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CssClass(string value)
    {
      calendar.CssClass = value;
      return this;
    }
    /// <summary>
    /// Calendar.DateTimeFormat matching the given culture's DateTimeFormat by CultureInfo.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder Culture(CultureInfo value)
    {
      calendar.Culture = value;
      return this;
    }
    /// <summary>
    /// Calendar.DateTimeFormat matching the given culture's DateTimeFormat by ID (int).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CultureId(int value)
    {
      calendar.CultureId = value;
      return this;
    }
    /// <summary>
    /// Calendar.DateTimeFormat matching the given culture's DateTimeFormat by name (string).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder CultureName(string value)
    {
      calendar.CultureName = value;
      return this;
    }
    /// <summary>
    /// DateTimeFormatInfo that defines the appropriate format of displaying dates and times.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DateTimeFormat(DateTimeFormatInfo value)
    {
      calendar.DateTimeFormat = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the days of the displayed calendar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DayActiveCssClass(string value)
    {
      calendar.DayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the days of the displayed calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DayCssClass(string value)
    {
      calendar.DayCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the section that displays the day of the week.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DayHeaderCssClass(string value)
    {
      calendar.DayHeaderCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the days of the displayed calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DayHoverCssClass(string value)
    {
      calendar.DayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Name format of days of the week.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DayNameFormat(DayNameFormat value)
    {
      calendar.DayNameFormat = value;
      return this;
    }
    /// <summary>
    /// One-dimensional array of type String containing the culture-specific full names of the days of the week.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DayNames(string[] value)
    {
      calendar.DayNames = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the disabled days.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DisabledDayActiveCssClass(string value)
    {
      calendar.DisabledDayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the disabled days.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DisabledDayCssClass(string value)
    {
      calendar.DisabledDayCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the disabled days.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder DisabledDayHoverCssClass(string value)
    {
      calendar.DisabledDayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Day to show as the first day of the week in the calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder FirstDayOfWeek(FirstDayOfWeek value)
    {
      calendar.FirstDayOfWeek = value;
      return this;
    }
    /// <summary>
    /// Client-side content to render for the footer of the calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder FooterClientTemplate(string value)
    {
      ClientTemplate ct = new ClientTemplate();
      ct.Text = value;
      calendar.FooterClientTemplate = ct;
      return this;
    }
    /// <summary>
    /// Client-side content to render for the header of the calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder HeaderClientTemplate(string value)
    {
      ClientTemplate ct = new ClientTemplate();
      ct.Text = value;
      calendar.HeaderClientTemplate = ct;
      return this;
    }
    /// <summary>
    /// Assigned height of the Calendar object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder Height(Unit value)
    {
      calendar.Height = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all image URL paths.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ImagesBaseUrl(string value)
    {
      calendar.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// Maximum date and time that can be selected in the control.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MaxDate(DateTime value)
    {
      calendar.MaxDate = value;
      return this;
    }
    /// <summary>
    /// Minimum date and time that can be selected in the control.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MinDate(DateTime value)
    {
      calendar.MinDate = value;
      return this;
    }
    /// <summary>
    /// Number of month columns displayed in the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthColumns(int value)
    {
      calendar.MonthColumns = value;
      return this;
    }
    /// <summary>
    /// CSS class for months in the Calendar control.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthCssClass(string value)
    {
      calendar.MonthCssClass = value;
      return this;
    }
    /// <summary>
    /// One-dimensional array of type String containing the culture-specific full names of the months.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthNames(string[] value)
    {
      calendar.MonthNames = value;
      return this;
    }
    /// <summary>
    /// Amount of space between the contents of a month and the month's border. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthPadding(int value)
    {
      calendar.MonthPadding = value;
      return this;
    }
    /// <summary>
    /// Number of month rows displayed in the Calendar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthRows(int value)
    {
      calendar.MonthRows = value;
      return this;
    }
    /// <summary>
    /// Amount of space between months in the Calendar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthSpacing(int value)
    {
      calendar.MonthSpacing = value;
      return this;
    }
    /// <summary>
    /// CSS class of the title heading for month areas of the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder MonthTitleCssClass(string value)
    {
      calendar.MonthTitleCssClass = value;
      return this;
    }
    /// <summary>
    /// Height of the image displayed in the next-navigation button.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextImageHeight(int value)
    {
      calendar.NextImageHeight = value;
      return this;
    }
    /// <summary>
    /// Path of image displayed in the next-navigation button.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextImageUrl(string value)
    {
      calendar.NextImageUrl = value;
      return this;
    }
    /// <summary>
    /// Width of the image displayed in the next-navigation button.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextImageWidth(int value)
    {
      calendar.NextImageWidth = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the next and previous month navigation elements.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextPrevActiveCssClass(string value)
    {
      calendar.NextPrevActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the next and previous month navigation elements.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextPrevCssClass(string value)
    {
      calendar.NextPrevCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the next and previous month navigation elements.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextPrevHoverCssClass(string value)
    {
      calendar.NextPrevHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Text displayed for the next month navigation element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder NextText(string value)
    {
      calendar.NextText = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the days that are not in the focused month.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder OtherMonthDayActiveCssClass(string value)
    {
      calendar.OtherMonthDayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the days that are not in the focused month. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder OtherMonthDayCssClass(string value)
    {
      calendar.OtherMonthDayCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the days that are not in the focused month.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder OtherMonthDayHoverCssClass(string value)
    {
      calendar.OtherMonthDayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the days that are out of range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder OutOfRangeDayActiveCssClass(string value)
    {
      calendar.OutOfRangeDayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the days that are out of range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder OutOfRangeDayCssClass(string value)
    {
      calendar.OutOfRangeDayCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the days that are out of range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder OutOfRangeDayHoverCssClass(string value)
    {
      calendar.OutOfRangeDayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the picker portion of the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PickerCssClass(string value)
    {
      calendar.PickerCssClass = value;
      return this;
    }
    /// <summary>
    /// Custom date format string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PickerCustomFormat(string value)
    {
      calendar.PickerCustomFormat = value;
      return this;
    }
    /// <summary>
    /// Format of date/time displayed by the picker. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PickerFormat(DateTimeFormatType value)
    {
      calendar.PickerFormat = value;
      return this;
    }
    /// <summary>
    /// String designator for hours that are "post meridiem" (after noon). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PMDesignator(string value)
    {
      calendar.PMDesignator = value;
      return this;
    }
    /// <summary>
    /// Pop-up calendar behavior.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUp(CalendarPopUpType value)
    {
      calendar.PopUp = value;
      return this;
    }
    /// <summary>
    /// Duration of the calendar pop-up collapse animation, in milliseconds.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpCollapseDuration(int value)
    {
      calendar.PopUpCollapseDuration = value;
      return this;
    }
    /// <summary>
    /// Slide type to use for the calendar pop-up collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpCollapseSlide(SlideType value)
    {
      calendar.PopUpCollapseSlide = value;
      return this;
    }
    /// <summary>
    /// Transition effect to use for the calendar pop-up collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpCollapseTransition(TransitionType value)
    {
      calendar.PopUpCollapseTransition = value;
      return this;
    }
    /// <summary>
    /// Custom transition filter to use for the calendar pop-up collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpCollapseTransitionCustomFilter(string value)
    {
      calendar.PopUpCollapseTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// Client-side ID of the element to which this pop-up calendar is aligned. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandControlId(string value)
    {
      calendar.PopUpExpandControlId = value;
      return this;
    }
    /// <summary>
    /// Direction in which the pop-up Calendar expands. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandDirection(CalendarPopUpExpandDirection value)
    {
      calendar.PopUpExpandDirection = value;
      return this;
    }
    /// <summary>
    /// Duration of the calendar pop-up expand animation, in milliseconds. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandDuration(int value)
    {
      calendar.PopUpExpandDuration = value;
      return this;
    }
    /// <summary>
    /// Offset along x-axis from pop-up calendar's normal expand position. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandOffsetX(int value)
    {
      calendar.PopUpExpandOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset along y-axis from pop-up calendar's normal expand position.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandOffsetY(int value)
    {
      calendar.PopUpExpandOffsetY = value;
      return this;
    }
    /// <summary>
    /// Slide type to use for the calendar pop-up expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandSlide(SlideType value)
    {
      calendar.PopUpExpandSlide = value;
      return this;
    }
    /// <summary>
    /// Transition effect to use for the calendar pop-up expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandTransition(TransitionType value)
    {
      calendar.PopUpExpandTransition = value;
      return this;
    }
    /// <summary>
    /// Custom transition filter to use for the calendar pop-up expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpExpandTransitionCustomFilter(string value)
    {
      calendar.PopUpExpandTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// Whether a pop-up calendar has a drop shadow. Default is true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpShadowEnabled(bool value)
    {
      calendar.PopUpShadowEnabled = value;
      return this;
    }
    /// <summary>
    /// CSS zIndex of the Calendar pop-up. Default is 1000. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PopUpZIndex(int value)
    {
      calendar.PopUpZIndex = value;
      return this;
    }
    /// <summary>
    /// This value is the increment by which the time value is able to change. Default value TimeSpan.Zero indicates that all values are allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder Precision(TimeSpan value)
    {
      calendar.Precision = value;
      return this;
    }
    /// <summary>
    /// Height of the image displayed in the previous-navigation button. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PrevImageHeight(int value)
    {
      calendar.PrevImageHeight = value;
      return this;
    }
    /// <summary>
    /// Path of image displayed in the previous-navigation button.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PrevImageUrl(string value)
    {
      calendar.PrevImageUrl = value;
      return this;
    }
    /// <summary>
    /// Width of the image displayed in the previous navigation button. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PrevImageWidth(int value)
    {
      calendar.PrevImageWidth = value;
      return this;
    }
    /// <summary>
    /// Text displayed for the previous navigation element. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder PrevText(string value)
    {
      calendar.PrevText = value;
      return this;
    }
    /// <summary>
    /// Whether the calendar should fire events and/or perform posts when the selected date is clicked. Default is false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ReactOnSameSelection(bool value)
    {
      calendar.ReactOnSameSelection = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder RenderSearchEngineStamp(bool value)
    {
      calendar.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// The selected date.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectedDate(DateTime value)
    {
      calendar.SelectedDate = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the selected dates. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectedDayActiveCssClass(string value)
    {
      calendar.SelectedDayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the selected dates. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectedDayCssClass(string value)
    {
      calendar.SelectedDayCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the selected dates. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectedDayHoverCssClass(string value)
    {
      calendar.SelectedDayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the month selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectMonthActiveCssClass(string value)
    {
      calendar.SelectMonthActiveCssClass = value;
      return this;
    }

    /// <summary>
    /// CSS class for the month selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectMonthCssClass(string value)
    {
      calendar.SelectMonthCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the month selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectMonthHoverCssClass(string value)
    {
      calendar.SelectMonthHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Text displayed for the month selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectMonthText(string value)
    {
      calendar.SelectMonthText = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the week selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectWeekActiveCssClass(string value)
    {
      calendar.SelectWeekActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the week selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectWeekCssClass(string value)
    {
      calendar.SelectWeekCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the week selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectWeekHoverCssClass(string value)
    {
      calendar.SelectWeekHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Text displayed for the week selector.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SelectWeekText(string value)
    {
      calendar.SelectWeekText = value;
      return this;
    }
    /// <summary>
    /// Whether to display the Calendar title.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowCalendarTitle(bool value)
    {
      calendar.ShowCalendarTitle = value;
      return this;
    }
    /// <summary>
    /// Whether the heading for the days of the week is displayed. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowDayHeader(bool value)
    {
      calendar.ShowDayHeader = value;
      return this;
    }
    /// <summary>
    /// Whether the days on the Calendar control are separated with grid lines. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowGridLines(bool value)
    {
      calendar.ShowGridLines = value;
      return this;
    }
    /// <summary>
    /// Whether the month title section is displayed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowMonthTitle(bool value)
    {
      calendar.ShowMonthTitle = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar control displays the next and previous month navigation elements in the title section. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowNextPrev(bool value)
    {
      calendar.ShowNextPrev = value;
      return this;
    }
    /// <summary>
    /// Whether the calendar title section is displayed. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowTitle(bool value)
    {
      calendar.ShowTitle = value;
      return this;
    }
    /// <summary>
    /// Whether the calendar should display week numbers.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ShowWeekNumbers(bool value)
    {
      calendar.ShowWeekNumbers = value;
      return this;
    }
    /// <summary>
    /// Duration of the calendar swap animation, in milliseconds.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SwapDuration(int value)
    {
      calendar.SwapDuration = value;
      return this;
    }
    /// <summary>
    /// Slide type to use for the calendar swap animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SwapSlide(SlideType value)
    {
      calendar.SwapSlide = value;
      return this;
    }
    /// <summary>
    /// Transition effect to use for the calendar swap animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SwapTransition(TransitionType value)
    {
      calendar.SwapTransition = value;
      return this;
    }
    /// <summary>
    /// Custom transition filter to use for the calendar swap animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder SwapTransitionCustomFilter(string value)
    {
      calendar.SwapTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// HTML tab-index to assign to Calendar on the client.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TabIndex(short value)
    {
      calendar.TabIndex = value;
      return this;
    }
    /// <summary>
    /// CSS class of the title heading for the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TitleCssClass(string value)
    {
      calendar.TitleCssClass = value;
      return this;
    }
    /// <summary>
    /// Date format string for date(s) shown in the calendar title.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TitleDateFormat(string value)
    {
      calendar.TitleDateFormat = value;
      return this;
    }
    /// <summary>
    /// String that separates the "to" and "from" dates in the calendar title.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TitleDateRangeSeparatorString(string value)
    {
      calendar.TitleDateRangeSeparatorString = value;
      return this;
    }
    /// <summary>
    /// Type of the contents displayed in the calendar title area. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TitleType(CalendarTitleType value)
    {
      calendar.TitleType = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for today's date on the Calendar object. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TodayDayActiveCssClass(string value)
    {
      calendar.TodayDayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for today's date on the Calendar object. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TodayDayCssClass(string value)
    {
      calendar.TodayDayCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for today's date on the Calendar object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TodayDayHoverCssClass(string value)
    {
      calendar.TodayDayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Value to use for today's date.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder TodaysDate(DateTime value)
    {
      calendar.TodaysDate = value;
      return this;
    }
    /// <summary>
    /// Whether to toggle day selection only when the Ctrl key is held down.  Default is true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ToggleSelectOnCtrlKey(bool value)
    {
      calendar.ToggleSelectOnCtrlKey = value;
      return this;
    }
    /// <summary>
    /// Text displayed when the mouse pointer hovers over the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder ToolTip(string value)
    {
      calendar.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar control should use the server's clock for determining today's date. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder UseServersTodaysDate(bool value)
    {
      calendar.UseServersTodaysDate = value;
      return this;
    }
    /// <summary>
    /// Whether the Calendar's markup is rendered on the page.  To hide the Calendar, use CSS.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder Visible(bool value)
    {
      calendar.Visible = value;
      return this;
    }
    /// <summary>
    /// Date that is focused on and displayed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder VisibleDate(DateTime value)
    {
      calendar.VisibleDate = value;
      return this;
    }
    /// <summary>
    /// Column in the Calendar in which the visible month is displayed. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder VisibleMonthColumn(int value)
    {
      calendar.VisibleMonthColumn = value;
      return this;
    }
    /// <summary>
    /// Row in the Calendar in which the visible month is displayed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder VisibleMonthRow(int value)
    {
      calendar.VisibleMonthRow = value;
      return this;
    }
    /// <summary>
    /// Active CSS class for the weekend dates on the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder WeekendDayActiveCssClass(string value)
    {
      calendar.WeekendDayActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for the weekend dates on the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder WeekendDayCssClass(string value)
    {
      calendar.WeekendDayCssClass = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for the weekend dates on the Calendar.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder WeekendDayHoverCssClass(string value)
    {
      calendar.WeekendDayHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Assigned width of the Calendar element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarBuilder Width(Unit value)
    {
      calendar.Width = value;
      return this;
    }
    /// <summary>
    /// Output the markup to generate a Calendar object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      // Extract params from querystring
      DateTime selectedDate = DateTime.Parse(viewContext.HttpContext.Request.Params[calendar.ClientObjectId + "_SelectedDate"] ?? calendar.SelectedDate.ToString());
      DateTime visibleDate = DateTime.Parse(viewContext.HttpContext.Request.Params[calendar.ClientObjectId + "_VisibleDate"] ?? calendar.VisibleDate.ToString());

      DateCollection selectedDates = null;
      DateCollection disabledDates = null;
      object model = viewContext.ViewData.Model;
      CalendarActionResponse actionResponse = null;

      if (boundModel != null)
      {
        model = boundModel;
      }

      if (model != null)
      {
        switch (model.GetType().FullName)
        {
          case "ComponentArt.Web.UI.CalendarActionResponse":
            actionResponse = (CalendarActionResponse)model;
            break;
          case "System.String":
            actionResponse = (CalendarActionResponse)viewContext.ViewData[model.ToString()];
            break;
          case "System.DateTime":
            selectedDate = DateTime.Parse(model.ToString());
            visibleDate = DateTime.Parse(model.ToString());
            break;
          case "ComponentArt.Web.UI.DateCollection": 
            selectedDates = (DateCollection)model;
            if (selectedDates.Count > 0)
            {
              visibleDate = DateTime.Parse(selectedDates[0].ToString());
            }
            break;
        }
        if (actionResponse != null)
        {
          disabledDates = actionResponse.DisabledDates;
          selectedDate = actionResponse.SelectedDate;
          selectedDates = actionResponse.SelectedDates;
          visibleDate = actionResponse.VisibleDate;
        }
      }

      if (visibleDate != DateTime.MinValue)
      {
        calendar.VisibleDate = visibleDate;
      }
      if (selectedDates != null && selectedDates.Count > 0)
      {
        calendar.SelectedDates.Clear();
        foreach (DateTime date in selectedDates)
        {
          calendar.SelectedDates.Add(date);
        }
      }
      else
      {
        if (selectedDate != DateTime.MinValue)
        {
          calendar.SelectedDate = selectedDate;
        }
      }
      if (disabledDates != null && disabledDates.Count > 0)
      {
        calendar.DisabledDates.Clear();
        foreach (DateTime date in disabledDates)
        {
          calendar.DisabledDates.Add(date);
        }
      }

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);

      calendar.RenderControl(htmlTextWriter1);

      return stringWriter.ToString();
    }
  }
}