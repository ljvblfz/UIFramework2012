using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;

namespace ComponentArt.Web.UI
{
  public static class CalendarHelperExtensions
  {
    /// <summary>
    /// Extracts a Calendar's selected-date from the page's FormCollection.
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="formCollection"></param>
    /// <returns></returns>
    public static DateTime GetSelectedDate(string ID, FormCollection formCollection)
    {
      string sScript = formCollection.Get(ID + "_selecteddates");

      if (sScript != null)
      {
        DateTime visibleDate;

        if (sScript.Split(',')[0].Length > 10)
        {

          visibleDate = Utils.StringToDate(sScript, "yyyy.M.d.H.m.s");
        }
        else
        {
          visibleDate = Utils.StringToDate(sScript, "yyyy.M.d");
        }
        return visibleDate;
      }

      return DateTime.MinValue;
    }
    /// <summary>
    /// Extracts a Calendar's selected-dates from the page's FormCollection.
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="formCollection"></param>
    /// <returns></returns>
    public static DateCollection GetSelectedDates(string ID, FormCollection formCollection)
    {
      string sScript = formCollection.Get(ID + "_selecteddates");
      
      if (sScript != null)
      {
        ArrayList dateArrayList;

        if (sScript.Split(',')[0].Length > 10)
        {
          dateArrayList = Utils.StringToDateArrayList(sScript, "yyyy.M.d.H.m.s");
        }
        else
        {
          dateArrayList = Utils.StringToDateArrayList(sScript, "yyyy.M.d");
        }
        return new DateCollection(dateArrayList);
      }

      return null;
    }
    /// <summary>
    /// Extracts a Calendar's visible/focused-date from the page's FormCollection.
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="formCollection"></param>
    /// <returns></returns>
    public static DateTime GetVisibleDate(string ID, FormCollection formCollection)
    {
      string sScript = formCollection.Get(ID + "_visibledate");

      if (sScript != null)
      {
        DateTime visibleDate;

        if (sScript.Split(',')[0].Length > 10)
        {

          visibleDate = Utils.StringToDate(sScript, "yyyy.M.d.H.m.s");
        }
        else
        {
          visibleDate = Utils.StringToDate(sScript, "yyyy.M.d");
        }
        return visibleDate;
      }

      return DateTime.MinValue;
    }
  }
}
