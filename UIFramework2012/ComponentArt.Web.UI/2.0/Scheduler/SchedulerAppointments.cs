using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace ComponentArt.Web.UI
{

  [PersistChildren(false)]
  [ParseChildren(true)]
  public class SchedulerAppointmentLook : IAttributeAccessor
  {
    /// <summary>
    /// Constructor for SchedulerAppointmentLook.
    /// </summary>
    public SchedulerAppointmentLook()
    {
    }

    internal Hashtable Properties = new Hashtable();

    #region IAttributeAccessor implementation
    // (IAttributeAccessor is used to allow expando properties)

    public String GetAttribute(string key)
    {
      return this.Properties[key].ToString();
    }

    public void SetAttribute(string key, string value)
    {
      this.Properties[key] = value;
    }

    #endregion IAttributeAccessor implementation

    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate ClientTemplate
    {
      get
      {
        return (ClientTemplate)this.Properties["ClientTemplate"];
      }
      set
      {
        this.Properties["ClientTemplate"] = value;
      }
    }

    [DefaultValue(null)]
    public string CssClass
    {
      get
      {
        return this.Properties["CssClass"].ToString();
      }
      set
      {
        this.Properties["CssClass"] = value;
      }
    }
  
    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate DescriptionClientTemplate
    {
      get
      {
        return (ClientTemplate)this.Properties["DescriptionClientTemplate"];
      }
      set
      {
        this.Properties["DescriptionClientTemplate"] = value;
      }
    }
  
    [DefaultValue(null)]
    public string DescriptionCssClass
    {
      get
      {
        return this.Properties["DescriptionCssClass"].ToString();
      }
      set
      {
        this.Properties["DescriptionCssClass"] = value;
      }
    }
  
    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate FooterClientTemplate
    {
      get
      {
        return (ClientTemplate)this.Properties["FooterClientTemplate"];
      }
      set
      {
        this.Properties["FooterClientTemplate"] = value;
      }
    }

    [DefaultValue(null)]
    public string FooterCssClass
    {
      get
      {
        return this.Properties["FooterCssClass"].ToString();
      }
      set
      {
        this.Properties["FooterCssClass"] = value;
      }
    }
    
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit FooterHeight
    {
      get
      {
        return Utils.ParseUnit(this.Properties["FooterHeight"]);
			}
			set 
			{
        if (value.IsEmpty || value.Type == UnitType.Pixel)
        {
          this.Properties["FooterHeight"] = value;
        }
        else
        {
          throw new Exception("FooterHeight may only be specified in pixels.");
        }
      }
    }

    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate HeaderClientTemplate
    {
      get
      {
        return (ClientTemplate)this.Properties["HeaderClientTemplate"];
      }
      set
      {
        this.Properties["HeaderClientTemplate"] = value;
      }
    }

    [DefaultValue(null)]
    public string HeaderCssClass
    {
      get
      {
        return this.Properties["HeaderCssClass"].ToString();
      }
      set
      {
        this.Properties["HeaderCssClass"] = value;
      }
    }

    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit HeaderHeight
    {
      get
      {
        return Utils.ParseUnit(this.Properties["HeaderHeight"]);
      }
      set
      {
        if (value.IsEmpty || value.Type == UnitType.Pixel)
        {
          this.Properties["HeaderHeight"] = value;
        }
        else
        {
          throw new Exception("HeaderHeight may only be specified in pixels.");
        }
      }
    }

    [DefaultValue(null)]
    public string LookId
    {
      get
      {
        return this.Properties["LookId"].ToString();
      }
      set
      {
        this.Properties["LookId"] = value;
      }
    }

    [DefaultValue(false)]
    public bool ShowFooter
    {
      get
      {
        return Utils.ParseBool(this.Properties["ShowFooter"], false);
      }
      set
      {
        this.Properties["ShowFooter"] = value;
      }
    }

    [DefaultValue(false)]
    public bool ShowHeader
    {
      get
      {
        return Utils.ParseBool(this.Properties["ShowHeader"], false);
      }
      set
      {
        this.Properties["ShowHeader"] = value;
      }
    }

    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate TitleClientTemplate
    {
      get
      {
        return (ClientTemplate)this.Properties["TitleClientTemplate"];
      }
      set
      {
        this.Properties["TitleClientTemplate"] = value;
      }
    }

    [DefaultValue(null)]
    public string TitleCssClass
    {
      get
      {
        return this.Properties["TitleCssClass"].ToString();
      }
      set
      {
        this.Properties["TitleCssClass"] = value;
      }
    }

    private static Hashtable _propertyTypes = new Hashtable();
    private static bool _propertyTypesInitialized = false;
    private static void InitializePropertyTypes()
    {
      _propertyTypesInitialized = true;
      _propertyTypes["FooterHeight"] = "Unit";
      _propertyTypes["HeaderHeight"] = "Unit";
      _propertyTypes["ShowFooter"] = "bool";
      _propertyTypes["ShowHeader"] = "bool";
      _propertyTypes["ClientTemplate"] = "ClientTemplate";
      _propertyTypes["DescriptionClientTemplate"] = "ClientTemplate";
      _propertyTypes["FooterClientTemplate"] = "ClientTemplate";
      _propertyTypes["HeaderClientTemplate"] = "ClientTemplate";
      _propertyTypes["TitleClientTemplate"] = "ClientTemplate";
    }
    internal static Hashtable PropertyTypes
    {
      get
      {
        if (!_propertyTypesInitialized)
        {
          InitializePropertyTypes();
        }
        return _propertyTypes;
      }
    }

    internal string GeneratePropertyStorage()
    {
      String[] propertyArray = new String[this.Properties.Count];
      int i = 0;
      foreach (string propertyName in this.Properties.Keys)
      {
        string propertyType = SchedulerAppointmentLook.PropertyTypes.Contains(propertyName) ? SchedulerAppointmentLook.PropertyTypes[propertyName].ToString() : null;
        object propertyValue = this.Properties[propertyName];
        string propertyValueJS;
        switch (propertyType)
        {
          case "ClientTemplate":
            propertyValueJS = ClientTemplate.TextToJsString((ClientTemplate)propertyValue);
            break;
          case "bool":
            propertyValueJS = propertyValue.ToString().ToLower();
            break;
          case "Unit":
            propertyValueJS = Utils.ConvertUnitToJSComponentArtUnit((Unit)propertyValue);
            break;
          default:
            propertyValueJS = Utils.ConvertStringToJSString(propertyValue.ToString());
            break;
        }
        propertyArray[i] = "'" + propertyName + "':" + propertyValueJS;
        i++;
      }
      return "{" + String.Join(",", propertyArray) + "}";
    }

  }

  public class SchedulerAppointmentLookCollection : CollectionBase
  {

    public new SchedulerAppointmentLook this[int index]
    {
      get
      {
        return (SchedulerAppointmentLook)this.List[index];
      }
      set
      {
        this.List[index] = value;
      }
    }

    public new int Add(SchedulerAppointmentLook look)
    {
      return this.List.Add(look);
    }

    internal string GeneratePropertyStorage()
    {
      string[] lookPropertyStorages = new string[this.Count];
      for (int i = 0; i < this.Count; i++)
      {
        lookPropertyStorages[i] = this[i].GeneratePropertyStorage();
      }
      return "[" + String.Join(",", lookPropertyStorages) + "]";
    }

  }

  /// <summary>
  /// Represents an appointment of the <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerAppointment : IAttributeAccessor
  {
    /// <summary>
    /// Constructor for SchedulerAppointment.
    /// </summary>
    public SchedulerAppointment()
    {
    }

    internal Hashtable Properties = new Hashtable();

    #region IAttributeAccessor implementation
    // (IAttributeAccessor is used to allow expando properties)

    public String GetAttribute(string key)
    {
      return this.Properties[key].ToString();
    }

    public void SetAttribute(string key, string value)
    {
      this.Properties[key] = value;
    }

    #endregion IAttributeAccessor implementation

    /// <summary>
    /// ID of the Appointment.
    /// </summary>
    public string AppointmentID
    {
      get
      {
        return this.Properties["AppointmentID"].ToString();
      }
      set
      {
        this.Properties["AppointmentID"] = value;
      }
    }

    /// <summary>
    /// Start DateTime of the Appointment.
    /// </summary>
    public DateTime Start
    {
      get
      {
        return Scheduler.ParseDateTime(this.Properties["Start"]);
      }
      set
      {
        this.Properties["Start"] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    /// <summary>
    /// Duration of the appointment.
    /// </summary>
    public TimeSpan Duration
    {
      get
      {
        return TimeSpan.FromMilliseconds(Double.Parse(this.Properties["Duration"].ToString()));
      }
      set
      {
        this.Properties["Duration"] = value.TotalMilliseconds.ToString();
      }
    }

    /// <summary>
    /// The time period of this appointment.
    /// </summary>
    public Period Period
    {
      get
      {
        return new Period(this.Start, this.Duration);
      }
      set
      {
        this.Start = value.StartDateTime;
        this.Duration = value.Duration;
      }
    }

    /// <summary>
    /// End DateTime of the Appointment.
    /// </summary>
    public DateTime End
    {
      get
      {
        return this.Start.Add(this.Duration);
      }
    }

    /// <summary>
    /// The Appointment Title.
    /// </summary>
    public string Title
    {
      get
      {
        return this.Properties["Title"].ToString();
      }
      set
      {
        this.Properties["Title"] = value;
      }
    }

    /// <summary>
    /// The Appointment Description.
    /// </summary>
    public string Description
    {
      get
      {
        return this.Properties["Description"].ToString();
      }
      set
      {
        this.Properties["Description"] = value;
      }
    }

    /// <summary>
    /// ID of the associated RecurringAppointment, if any.  Non-null if this Appointment is an instance or Exception of a RecurringAppointment.
    /// </summary>
    public string RecurringAppointmentID
    {
      get
      {
        return this.Properties["RecurringAppointmentID"].ToString();
      }
      set
      {
        this.Properties["RecurringAppointmentID"] = value;
      }
    }

    /// <summary>
    /// Whether to render this appointment in the associated view(s).  Appointment will be available on client.
    /// </summary>
    public Boolean Visible
    {
      get
      {
        if (this.Properties["Visible"] != null)
        {
          return Boolean.Parse(this.Properties["Visible"].ToString());
        }
        else
        {
          return true;
        }
      }
      set
      {
        this.Properties["Visible"] = value;
      }
    }

    /// <summary>
    /// Whether this appointment is an exception (false) or an instance (true) for a recurring appointment.
    /// Ignored if not associated with an RecurringAppointment ie. RecurringAppointmentID is null/blank.
    /// </summary>
    public Boolean Instance
    {
      get
      {
        if (this.Properties["Instance"] != null)
        {
          return Boolean.Parse(this.Properties["Instance"].ToString());
        }
        else
        {
          return false;
        }
      }
      set
      {
        this.Properties["Instance"] = value;
      }
    }

    /// <summary>
    /// Optional custom data.
    /// </summary>
    public string Tag
    {
        get
        {
          if (this.Properties["Tag"] != null)
          {
            return this.Properties["Tag"].ToString();
          }
          else
          {
            return null;
          }
        }
        set
        {
            this.Properties["Tag"] = value;
        }
    }

    internal Hashtable ClientProperties = new Hashtable();

    internal Hashtable _clientPropertyTypes = null;
    internal Hashtable ClientPropertyTypes
    {
      get
      {
        if (this._clientPropertyTypes == null)
        {
          this._clientPropertyTypes = new Hashtable();
          this._clientPropertyTypes.Add("Start", "DateTime");
          this._clientPropertyTypes.Add("Duration", "Number");
          this._clientPropertyTypes.Add("Visible", "Boolean");
          this._clientPropertyTypes.Add("Instance", "Boolean");
        }
        return this._clientPropertyTypes;
      }
    }

    private void PopulateClientProperties()
    {
      this.ClientProperties.Clear();
      foreach (string propertyName in this.Properties.Keys)
      {
        object propertyValue = this.Properties[propertyName];
        string clientPropertyType = this.ClientPropertyTypes.Contains(propertyName) ? this.ClientPropertyTypes[propertyName].ToString() : null;
        switch (clientPropertyType)
        {
          case "DateTime":
            this.ClientProperties.Add(propertyName, Utils.ConvertDateTimeToJsDate(Scheduler.ParseDateTime(propertyValue)));
            break;
          case "Number":
            this.ClientProperties.Add(propertyName, Utils.ParseDouble(propertyValue));
            break;
          case "Boolean":
            this.ClientProperties.Add(propertyName, propertyValue.ToString().ToLower());
            break;
          default:
            this.ClientProperties.Add(propertyName, Utils.ConvertStringToJSString(propertyValue.ToString()));
            break;
        }
      }
    }

    internal string GeneratePropertyStorage()
    {
      this.PopulateClientProperties();
      String[] propertyArray = new String[this.ClientProperties.Count];
      int i = 0;
      foreach (string propertyName in this.ClientProperties.Keys)
      {
        propertyArray[i] = "'" + propertyName + "':" + this.ClientProperties[propertyName];
        i++;
      }
      return "{" + String.Join(",", propertyArray) + "}";
    }
  }

  /// <summary>
  /// Collection of <see cref="SchedulerAppointment"/> objects.
  /// </summary>
  public class SchedulerAppointmentCollection : CollectionBase
  {
    /// <summary>
    /// SchedulerAppointmentCollection constructor.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public SchedulerAppointment this[int index]
    {
      get
      {
        return (SchedulerAppointment)this.List[index];
      }
      set
      {
        this.List[index] = value;
      }
    }

    /// <summary>
    /// Add an Appointment to the collection.
    /// </summary>
    /// <param name="appointment"></param>
    /// <returns></returns>
    public int Add(SchedulerAppointment appointment)
    {
      return this.List.Add(appointment);
    }

    /// <summary>
    /// Returns the Collection of the Appointments for the given period.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public SchedulerAppointmentCollection GetAppointmentsForPeriod(Period period)
    {
      SchedulerAppointmentCollection result = new SchedulerAppointmentCollection();
      foreach (SchedulerAppointment appointment in this.List)
      {
        if (appointment.Period.Overlaps(period))
        {
          result.Add(appointment);
        }
      }
      return result;
    }

    internal string GeneratePropertyStorage()
    {
      string[] appointmentPropertyStorages = new string[this.Count];
      for (int i = 0; i < this.Count; i++)
      {
        appointmentPropertyStorages[i] = this[i].GeneratePropertyStorage();
      }
      return "[" + String.Join(",", appointmentPropertyStorages) + "]";
    }
  
  }

  /// <summary>
  /// Represents a period of time.
  /// </summary>
  public class Period
  {
    /// <summary>
    /// Period constructor.
    /// </summary>
    public Period()
    {
      this._startDateTime = DateTime.Now;
      this._duration = TimeSpan.Zero;
    }
    /// <summary>
    /// Period constructor.
    /// </summary>
    public Period(DateTime startDateTime, DateTime endDateTime)
    {
      this._startDateTime = startDateTime;
      this._duration = endDateTime - startDateTime;
    }
    /// <summary>
    /// Period constructor.
    /// </summary>
    public Period(DateTime startDateTime, TimeSpan duration)
    {
      this._startDateTime = startDateTime;
      this._duration = duration;
    }

    private DateTime _startDateTime;
    /// <summary>
    /// Start of the Period.
    /// </summary>
    public DateTime StartDateTime
    {
      get
      {
        return this._startDateTime;
      }
      set
      {
        this._startDateTime = value;
      }
    }

    /// <summary>
    /// End of the Period.
    /// </summary>
    public DateTime EndDateTime
    {
      get
      {
        return this._startDateTime + this._duration;
      }
      set
      {
        this._duration = value - this._startDateTime;
      }
    }

    private TimeSpan _duration;
    /// <summary>
    /// Duration of the period.
    /// </summary>
    public TimeSpan Duration
    {
      get
      {
        return this._duration;
      }
      set
      {
        this._duration = value;
      }
    }

    /// <summary>
    /// Whether two given periods overlap.
    /// </summary>
    /// <param name="period1"></param>
    /// <param name="period2"></param>
    /// <returns></returns>
    public static bool PeriodsOverlap(Period period1, Period period2)
    {
      return period1.EndDateTime >  period2.StartDateTime && period2.EndDateTime > period1.StartDateTime;
    }

    /// <summary>
    /// Whether this Period Overlaps with the given Period.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public bool Overlaps(Period period)
    {
      return Period.PeriodsOverlap(this, period);
    }

    /// <summary>
    /// Returns a string which evaluates to this Period in JavaScript.
    /// </summary>
    /// <returns></returns>
    public string ToJavaScriptString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("new ComponentArt_Period(");
      sb.Append(Utils.ConvertDateTimeToJsDate(this.StartDateTime));
      sb.Append(",");
      sb.Append(this.Duration.TotalMilliseconds);
      sb.Append(")");
      return sb.ToString();
    }

  }
  /*
    ['Occurrence', Number], 0-Minutely, 1-Daily, 2-Weekly, 3-Monthly, 4-Yearly 
    ['Interval', Number],  Every Interval Occurrence OR Every Interval-daynumber of Month OR  
    ['WeekOfMonth', Number],  0,1,2,3,4 : First, Second, Third, Fourth, Last
    ['DayOfMonth', Number],  0-Sunday, 1-Monday, 6-Saturday, 7-day, 8-weekday, 9-weekend day
    ['DaysOfWeek', Array],  0-Sunday, 1-Monday, 6-Saturday
    ['Month', Number]  0-January, etc 
  */

    /// <summary>
    ///     Lists the week in a month in which the event will occur.
    /// </summary>
    public enum SchedulerWeekOfMonth
    {
      /// <summary>
      ///     The recurring event will occur on the specified day or days
      ///     of the first week in the month.
      /// </summary>    
      First,

      /// <summary>
      ///  The recurring event will occur on the specified day or days of the second week in the month.
      /// </summary> 
      Second,

      /// <summary>
      /// The recurring event will occur on the specified day or days of the third week in the month.
      /// </summary> 
      Third,

      /// <summary>
      /// The recurring event will occur on the specified day or days of the fourth week in the month.
      /// </summary> 
      Fourth,

      /// <summary>
      /// The recurring event will occur on the specified day or days of the last week in the month.
      /// </summary> 
      Last
    }

    /// <summary>
    /// A relative day of the month.
    /// </summary>
    public enum SchedulerDayOfMonth
    {
      Sunday,
      Monday,
      Tuesday,
      Wednesday,
      Thursday,
      Friday,
      Saturday,

      /// <summary>
      /// The recurring appointment will occur on the nth day specified in WeekOfMonth.
      /// </summary>    
      Day,

      /// <summary>
      /// The recurring appointment will occur on the nth weekday (default Monday through Friday) specified in WeekOfMonth.
      /// </summary> 
      Weekday,

      /// <summary>
      /// The recurring appointment will occur on the nth weekend day (default Saturday and Sunday) specified in WeekOfMonth.
      /// </summary> 
      WeekendDay,

      /// <summary>
      /// The recurring appointment will not use this property for determining the pattern of instances.  Used for "Day m of every n months" repetition, where m is the day of the month inherited from the Appointment and n is Interval.
      /// </summary> 
      Unset = -1
    }

    /// <summary>
    /// Specifies the base unit used to determine when to repeat a set of recurring appointments.
    /// </summary>
    public enum SchedulerOccurrenceType
    {
      /// <summary>
      /// Repeat every n minutes, where n is set in Interval.
      /// </summary>
      Minutely,

      /// <summary>
      /// Repeat every n days, where n is set in Interval.
      /// </summary>
      Daily,

      /// <summary>
      /// Repeat every n weeks on the set of weekdays m, where n is Interval and m is WeekDays.
      /// </summary>
      Weekly,

      /// <summary>
      /// 1. Repeat on day m of every n month(s), where m is inherited from the Appointment and n is Interval.
      /// 2. Repeat on week m, day of week p of every n month(s), where m is DayOfMonth, p is WeekOfMonth and n is Interval.
      /// </summary>
      Monthly,

      /// <summary>
      /// 1. Repeat every nth of month m, where n is Interval and m is Month.
      /// 2. Repeat on day m of week p of every month n, where m is DayOfMonth, p is WeekOfMonth and n is Month.
      /// </summary>
      Yearly
    }

    public class SchedulerDaysOfWeekCollection : CollectionBase
    {

      public new SchedulerDaysOfWeekCollection this[int index]
      {
        get
        {
          return (SchedulerDaysOfWeekCollection)this.List[index];
        }
        set
        {
          this.List[index] = value;
        }
      }

      public new int Add(DayOfWeek day)
      {
        return this.List.Add(day);
      }

      public bool Includes(DayOfWeek day)
      {
        foreach (DayOfWeek someDay in this.List)
        {
          if (someDay == day)
          {
            return true;
          }
        }
        return false;
      }

      internal string GeneratePropertyStorage()
      {
        string[] lookPropertyStorages = new string[this.Count];
        for (int i = 0; i < this.Count; i++)
        {
          lookPropertyStorages[i] = this[i].GeneratePropertyStorage();
        }
        return "[" + String.Join(",", lookPropertyStorages) + "]";
      }

    }

  
  /// <summary>
  /// Defines how a recurring appointment will be repeated.
  /// </summary>
  public class SchedulerRecurrencePattern : IAttributeAccessor
  {
    /// <summary>
    /// Constructor for SchedulerRecurrencePattern.
    /// </summary>
    public SchedulerRecurrencePattern()
    {
    }

    internal Hashtable Properties = new Hashtable();

    #region IAttributeAccessor implementation
    // (IAttributeAccessor is used to allow expando properties)

    public String GetAttribute(string key)
    {
      return this.Properties[key].ToString();
    }

    public void SetAttribute(string key, string value)
    {
      this.Properties[key] = value;
    }

    #endregion IAttributeAccessor implementation

    /// <summary>
    /// Base unit used in determining how the appointment will be repeated.
    /// </summary>
    public SchedulerOccurrenceType OccurrenceType
    {
      get
      {
        if (this.Properties["OccurenceType"] != null)
        {
          return (SchedulerOccurrenceType)this.Properties["OccurenceType"];
        }
        else
        {
          throw new Exception("OccurrenceType of a RecurrencePattern may not be null.");
        }
      }
      set
      {
        this.Properties["OccurenceType"] = (SchedulerOccurrenceType)value;
      }
    }

    /// <summary>
    /// Number of base units (set in OccurrenceType) between recurring appointment instances.  Some OccurrenceTypes do not use Interval and expect it to be zero (0).
    /// </summary>
    public int Interval
    {
      get
      {
        if (this.Properties["Interval"] != null)
        {
          return (int)this.Properties["Interval"];
        }
        else
        {
          if (this.OccurrenceType == SchedulerOccurrenceType.Minutely)
          {
            return 60;
          }
          else
          {
            return 0;
          }
        }
      }
      set
      {
        this.Properties["Interval"] = (int)value;
      }
    }

    /// <summary>
    /// The week of a month that a recurring appointment occurs on.  Used with OccurrenceType.Monthly and OccurrenceType.Yearly, ignored by other OccurrenceTypes.
    /// </summary>
    public SchedulerWeekOfMonth WeekOfMonth
    {
      get
      {
        if (this.Properties["WeekOfMonth"] != null)
        {
          return (SchedulerWeekOfMonth)this.Properties["WeekOfMonth"];
        }
        else
        {
          return SchedulerWeekOfMonth.First;
        }
      }
      set
      {
        this.Properties["WeekOfMonth"] = (SchedulerWeekOfMonth)value;
      }
    }

    /// <summary>
    /// The day of a month that a recurring appointment occurs on.  Used with OccurrenceType.Monthly and OccurrenceType.Yearly, ignored by other OccurrenceTypes.
    /// </summary>
    public SchedulerDayOfMonth DayOfMonth
    {
      get
      {
        if (this.Properties["DayOfMonth"] != null)
        {
          return (SchedulerDayOfMonth)this.Properties["DayOfMonth"];
        }
        else
        {
          return SchedulerDayOfMonth.Unset;  // -1
        }
      }
      set
      {
        this.Properties["DayOfMonth"] = (SchedulerDayOfMonth)value;
      }
    }

    /// <summary>
    /// The day(s) of a week that a recurring appointment occurs on.  Used with <see cref="OccurrenceType.Weekly"/>, ignored by other OccurrenceTypes.
    /// </summary>
    public SchedulerDaysOfWeekCollection DaysOfWeek
    {
      get
      {
        if (this.Properties["DaysOfWeek"] != null)
        {
          return (SchedulerDaysOfWeekCollection)this.Properties["DaysOfWeek"];
        }
        else
        {
          return new SchedulerDaysOfWeekCollection();
        }
      }
      set
      {
        this.Properties["DaysOfWeek"] = (SchedulerDaysOfWeekCollection)value;
      }
    }
    /// <summary>
    /// The month that a recurring appointment occurs on.  Used with OccurrenceType.Yearly, ignored by other OccurrenceTypes.
    /// </summary>
    public int Month
    {
      get
      {
        if (this.Properties["Month"] != null)
        {
          return (int)this.Properties["Month"];
        }
        else
        {
          return 1;  // default January
        }
      }
      set
      {
        this.Properties["Month"] = (int)value;
      }
    }
    /*
    internal Hashtable ClientProperties = new Hashtable();

    internal Hashtable _clientPropertyTypes = null;
    internal Hashtable ClientPropertyTypes
    {
      get
      {
        if (this._clientPropertyTypes == null)
        {
          this._clientPropertyTypes = new Hashtable();
          this._clientPropertyTypes.Add("Start", "DateTime");
          this._clientPropertyTypes.Add("Duration", "Number");
        }
        return this._clientPropertyTypes;
      }
    }

    private void PopulateClientProperties()
    {
      this.ClientProperties.Clear();
      foreach (string propertyName in this.Properties.Keys)
      {
        object propertyValue = this.Properties[propertyName];
        string clientPropertyType = this.ClientPropertyTypes.Contains(propertyName) ? this.ClientPropertyTypes[propertyName].ToString() : null;
        switch (clientPropertyType)
        {
          case "DateTime":
            this.ClientProperties.Add(propertyName, Utils.ConvertDateTimeToJsDate(Scheduler.ParseDateTime(propertyValue)));
            break;
          case "Number":
            this.ClientProperties.Add(propertyName, Utils.ParseDouble(propertyValue));
            break;
          default:
            this.ClientProperties.Add(propertyName, Utils.ConvertStringToJSString(propertyValue.ToString()));
            break;
        }
      }
    }

    internal string GeneratePropertyStorage()
    {
      this.PopulateClientProperties();
      String[] propertyArray = new String[this.ClientProperties.Count];
      int i = 0;
      foreach (string propertyName in this.ClientProperties.Keys)
      {
        propertyArray[i] = "'" + propertyName + "':" + this.ClientProperties[propertyName];
        i++;
      }
      return "{" + String.Join(",", propertyArray) + "}";
    }
     */
  }
  
  /// <summary>
  /// Represents a recurring appointment set of the <see cref="Scheduler"/> control.
  /// </summary>
  public class SchedulerRecurringAppointment : IAttributeAccessor
  {
    /// <summary>
    /// Constructor for SchedulerAppointment.
    /// </summary>
    public SchedulerRecurringAppointment()
    {
    }

    internal Hashtable Properties = new Hashtable();

    #region IAttributeAccessor implementation
    // (IAttributeAccessor is used to allow expando properties)

    public String GetAttribute(string key)
    {
      return this.Properties[key].ToString();
    }

    public void SetAttribute(string key, string value)
    {
      this.Properties[key] = value;
    }

    #endregion IAttributeAccessor implementation

    /// <summary>
    /// ID of the RecurringAppointment.
    /// </summary>
    public string ID
    {
      get
      {
        return this.Properties["ID"].ToString();
      }
      set
      {
        this.Properties["ID"] = value;
      }
    }

    /// <summary>
    /// Appointment that will be repeated over the range of the RecurringAppointment.  Acts like a template definition for repeated instances.
    /// </summary>
    public SchedulerAppointment Appointment
    {
      get
      {
        return (SchedulerAppointment)this.Properties["Appointment"];
      }
      set
      {
        this.Properties["Appointment"] = (SchedulerAppointment)value;
      }
    }

    /// <summary>
    /// The time period that this set of recurring appointments spans.
    /// </summary>
    public Period Range
    {
      get
      {
        return (Period)this.Properties["Range"];
      }
      set
      {
        this.Properties["Range"] = (Period)value;
      }
    }

    /// <summary>
    /// Description of how the recurring appointments are repeated.
    /// </summary>
    public SchedulerRecurrencePattern Pattern
    {
      get
      {
        return (SchedulerRecurrencePattern)this.Properties["Pattern"];
      }
      set
      {
        this.Properties["Pattern"] = (SchedulerRecurrencePattern)value;
      }
    }

    /// <summary>
    /// Number of appointments that this recurring appointment will generate.  For values less than 1 Scheduler
    /// will use the Range to determine the number of appointments to generate.
    /// </summary>
    public int Occurrences
    {
      get
      {
        return (int)this.Properties["Occurrences"];
      }
      set
      {
        this.Properties["Occurrences"] = (int)value;
      }
    }

    /// <summary>
    /// Optional custom data.
    /// </summary>
    public string Tag
    {
      get
      {
        return this.Properties["Tag"].ToString();
      }
      set
      {
        this.Properties["Tag"] = value;
      }
    }

    internal Hashtable ClientProperties = new Hashtable();

    internal Hashtable _clientPropertyTypes = null;
    internal Hashtable ClientPropertyTypes
    {
      get
      {
        if (this._clientPropertyTypes == null)
        {
          this._clientPropertyTypes = new Hashtable();
          this._clientPropertyTypes.Add("Appointment", "ComponentArt_SchedulerAppointment");
          this._clientPropertyTypes.Add("Range", "ComponentArt_Period");
          this._clientPropertyTypes.Add("Pattern", "ComponentArt_SchedulerRecurrencePattern");
        }
        return this._clientPropertyTypes;
      }
    }

    private void PopulateClientProperties()
    {
      this.ClientProperties.Clear();
      foreach (string propertyName in this.Properties.Keys)
      {
        object propertyValue = this.Properties[propertyName];
        string clientPropertyType = this.ClientPropertyTypes.Contains(propertyName) ? this.ClientPropertyTypes[propertyName].ToString() : null;
        switch (clientPropertyType)
        {
          case "ComponentArt_SchedulerAppointment":
            this.ClientProperties.Add(propertyName, this.ConvertAppointmentIntoJS((SchedulerAppointment)propertyValue));
            break;
          case "ComponentArt_Period":
            this.ClientProperties.Add(propertyName, this.ConvertPeriodIntoJS((Period)propertyValue));
            break;
          case "ComponentArt_SchedulerRecurrencePattern":
            this.ClientProperties.Add(propertyName, this.ConvertRecurrencePatternIntoJS((SchedulerRecurrencePattern)propertyValue));
            break;
          default:
            this.ClientProperties.Add(propertyName, Utils.ConvertStringToJSString(propertyValue.ToString()));
            break;
        }
      }
    }

    private string ConvertPeriodIntoJS(Period period)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("new ComponentArt_Period(");
      sb.Append(Utils.ConvertDateTimeToJsDate(period.StartDateTime));
      sb.Append(", " + Utils.ParseDouble(period.Duration.TotalMilliseconds));
      sb.Append(")");

      return sb.ToString();
    }
    /*
     *   window.ComponentArt_SchedulerRecurrencePattern = function(occurrenceType, interval, weekOfMonth, dayOfMonth, daysOfWeek, month)
     */
    private string ConvertRecurrencePatternIntoJS(SchedulerRecurrencePattern pattern)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("new ComponentArt_SchedulerRecurrencePattern(");
      switch (pattern.OccurrenceType)
      {
        case SchedulerOccurrenceType.Minutely:
          sb.Append("0");
          break;
        case SchedulerOccurrenceType.Daily:
          sb.Append("1");
          break;
        case SchedulerOccurrenceType.Weekly:
          sb.Append("2");
          break;
        case SchedulerOccurrenceType.Monthly:
          sb.Append("3");
          break;
        case SchedulerOccurrenceType.Yearly:
          sb.Append("4");
          break;
        default:
          sb.Append("1");
          break;
      }
      sb.Append(",");
      sb.Append(pattern.Interval.ToString());
      sb.Append(",");
      switch (pattern.WeekOfMonth)
      {
        case SchedulerWeekOfMonth.First:
          sb.Append("0");
          break;
        case SchedulerWeekOfMonth.Second:
          sb.Append("1");
          break;
        case SchedulerWeekOfMonth.Third:
          sb.Append("2");
          break;
        case SchedulerWeekOfMonth.Fourth:
          sb.Append("3");
          break;
        default:
          sb.Append("4"); // Last
          break;
      }
      sb.Append(",");
      switch (pattern.DayOfMonth)
      {
        case SchedulerDayOfMonth.Sunday:
          sb.Append("0");
          break;
        case SchedulerDayOfMonth.Monday:
          sb.Append("1");
          break;
        case SchedulerDayOfMonth.Tuesday:
          sb.Append("2");
          break;
        case SchedulerDayOfMonth.Wednesday:
          sb.Append("3");
          break;
        case SchedulerDayOfMonth.Thursday:
          sb.Append("4");
          break;
        case SchedulerDayOfMonth.Friday:
          sb.Append("5");
          break;
        case SchedulerDayOfMonth.Saturday:
          sb.Append("6");
          break;
        case SchedulerDayOfMonth.Day:
          sb.Append("7");
          break;
        case SchedulerDayOfMonth.Weekday:
          sb.Append("8");
          break;
        case SchedulerDayOfMonth.WeekendDay:
          sb.Append("9");
          break;
        default:  // Unset
          sb.Append("-1");
          break;
      }
      sb.Append(",");
      sb.Append("[");
      if (pattern.DaysOfWeek != null)
      {
        foreach (DayOfWeek dw in pattern.DaysOfWeek)
        {
          switch (dw)
          {
            case DayOfWeek.Sunday:
              sb.Append("0,");
              break;
            case DayOfWeek.Monday:
              sb.Append("1,");
              break;
            case DayOfWeek.Tuesday:
              sb.Append("2,");
              break;
            case DayOfWeek.Wednesday:
              sb.Append("3,");
              break;
            case DayOfWeek.Thursday:
              sb.Append("4,");
              break;
            case DayOfWeek.Friday:
              sb.Append("5,");
              break;
            default: // DayOfWeek.Saturday:
              sb.Append("6,");
              break;
          }
        }
        if (pattern.DaysOfWeek.Count > 0)
        {
          sb.Remove(sb.Length - 1, 1);
        }
      }
      sb.Append("]");
      sb.Append(",");
      sb.Append((pattern.Month - 1).ToString());
      sb.Append(")");

      return sb.ToString();
    }
    /* window.ComponentArt_SchedulerAppointment = function(appointmentId, start, duration, title, description, tag, recurringAppointmentId, instance)
     */
    private string ConvertAppointmentIntoJS(SchedulerAppointment appointment)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("new ComponentArt_SchedulerAppointment(");
      sb.Append(Utils.ConvertStringToJSString(appointment.AppointmentID));
      sb.Append(",");
      sb.Append(Utils.ConvertDateTimeToJsDate(Scheduler.ParseDateTime(appointment.Start)));
      sb.Append(",");
      sb.Append(Utils.ParseDouble(appointment.Duration.TotalMilliseconds));
      sb.Append(",");
      sb.Append(Utils.ConvertStringToJSString(appointment.Title));
      sb.Append(",");
      sb.Append(Utils.ConvertStringToJSString(appointment.Description));
      sb.Append(",");
      sb.Append(appointment.Visible.ToString().ToLower());
      sb.Append(",");
      if (appointment.Tag != null)
      {
        sb.Append(Utils.ConvertStringToJSString(appointment.Tag));
      }
      else
      {
        sb.Append("null");
      }
      sb.Append(",");
      sb.Append(Utils.ConvertStringToJSString(this.ID));
      sb.Append(")");

      return sb.ToString();
    }
    internal string GeneratePropertyStorage()
    {
      this.PopulateClientProperties();
      String[] propertyArray = new String[this.ClientProperties.Count];
      int i = 0;
      foreach (string propertyName in this.ClientProperties.Keys)
      {
        propertyArray[i] = "'" + propertyName + "':" + this.ClientProperties[propertyName];
        i++;
      }
      return "{" + String.Join(",", propertyArray) + "}";
    }
  }

  /// <summary>
  /// Collection of <see cref="SchedulerRecurringAppointment"/> objects.
  /// </summary>
  public class SchedulerRecurringAppointmentCollection : CollectionBase
  {
    /// <summary>
    /// SchedulerRecurringAppointmentCollection constructor.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public SchedulerRecurringAppointment this[int index]
    {
      get
      {
        return (SchedulerRecurringAppointment)this.List[index];
      }
      set
      {
        this.List[index] = value;
      }
    }

    /// <summary>
    /// Add a RecurringAppointment to the collection.
    /// </summary>
    /// <param name="appointment"></param>
    /// <returns></returns>
    public int Add(SchedulerRecurringAppointment recurringAppointment)
    {
      return this.List.Add(recurringAppointment);
    }

    /// <summary>
    /// Returns the Collection of the Appointments for the given period.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public SchedulerRecurringAppointmentCollection GetRecurringAppointmentsForPeriod(Period period)
    {
      SchedulerRecurringAppointmentCollection result = new SchedulerRecurringAppointmentCollection();
      foreach (SchedulerRecurringAppointment recurringAppointment in this.List)
      {
        if (recurringAppointment.Range.Overlaps(period))
        {
          result.Add(recurringAppointment);
        }
      }
      return result;
    }

    internal string GeneratePropertyStorage()
    {
      string[] appointmentPropertyStorages = new string[this.Count];
      for (int i = 0; i < this.Count; i++)
      {
        appointmentPropertyStorages[i] = this[i].GeneratePropertyStorage();
      }
      return "[" + String.Join(",", appointmentPropertyStorages) + "]";
    }

  }
}
