using System;
using System.Collections;
//using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ComponentArt.Web.Visualization.Gauges
{
  class JsonUtils
  {
    public static string Escape(object o)
    {
      if (o == null) 
        return "null";
        
      if (o is IDictionary)
        return EscapeObject((IDictionary)o);

      if (o is ArrayList)
        return EscapeArray((ArrayList)o);

      if (o is Array)
        return EscapeArray((object[])o);

      if (o is String)
        return EscapeString((string)o);

      if (o is Boolean)
        return EscapeBoolean((bool)o);

      try
      {
        return EscapeNumber(Convert.ToDouble(o));
      }
      catch (Exception) {}

      throw new Exception("Unable to escape object to JSON notation.");        
    }

    private static string EscapeObject(IDictionary d)
    {
      StringBuilder s = new StringBuilder();
      bool first = true;

      s.Append("{");

      foreach (DictionaryEntry entry in d)
      {
        if (!first)
          s.Append(",");

        s.Append(Escape(entry.Key.ToString()));
        s.Append(":");
        s.Append(Escape(entry.Value));

        first = false;
      }

      s.Append("}");

      return s.ToString();
    }

    private static string EscapeArray(ArrayList array)
    {
      return EscapeArray(array.ToArray());
    }

    private static string EscapeArray(object[] array)
    {
      StringBuilder s = new StringBuilder();

      s.Append("[");

      for (int i = 0; i < array.Length; i++)
      {
        if (i > 0)
          s.Append(",");

        s.Append(Escape(array[i]));
      }

      s.Append("]");

      return s.ToString();
    }


    private static string EscapeNumber(double n)
    {
      return n.ToString(CultureInfo.InvariantCulture);
    }

    private static string EscapeString(string s)
    {
      s = s.Replace(@"\", @"\\");
      s = s.Replace("\"", "\\\"");
      s = s.Replace("\n", "\\n");
      s = s.Replace("\r", "\\r");
      
      return '"' + s + '"';
    }

    private static string EscapeBoolean(bool b)
    {
      return (b ? "true" : "false");
    }


    public static object Parse(string json)
    {
      if (json == "" || json == "null")
        return null;
      if (json[0] == '{')
        return ParseObject(json);
      else if (json[0] == '[')
        return ParseArray(json);
      else
        return ParseValue(json);
    }

    private static Hashtable ParseObject(string jsobject)
    {
      Hashtable ht = new Hashtable();

      if (jsobject.Length < 2 || jsobject[0] != '{' || jsobject[jsobject.Length - 1] != '}')
        throw new Exception("Unable to parse object from JSON notation.");        

      jsobject = jsobject.Substring(1, jsobject.Length - 2);

      while (jsobject != "")
      {
        int index = findDelimiterIndex(jsobject, ':');

        if (index == -1)
          break;

        string name = jsobject.Substring(0, index);
        jsobject = jsobject.Substring(index + 1);

        index = findDelimiterIndex(jsobject, ',');

        string value = "";

        if (index == -1)
        {
          value = jsobject;
          jsobject = "";
        }
        else
        {
          value = jsobject.Substring(0, index);
          jsobject = jsobject.Substring(index + 1);
        }
        
        if (IsQuotedString(name))
           name = name.Substring(1, name.Length - 2);

        ht[name] = Parse(value);
      }

      return ht;
    }

    private static ArrayList ParseArray(string jsarray)
    {
      ArrayList arr = new ArrayList();

      if (jsarray.Length < 2 || jsarray[0] != '[' || jsarray[jsarray.Length - 1] != ']')
        throw new Exception("NOT AN ARRAY");

      jsarray = jsarray.Substring(1, jsarray.Length - 2);

      while (jsarray != "")
      {
        int index = findDelimiterIndex(jsarray, ',');
        string value = "";

        if (index == -1)
        {
          value = jsarray;
          jsarray = "";
        }
        else
        {
          value = jsarray.Substring(0, index);
          jsarray = jsarray.Substring(index + 1);
        }

        arr.Add(Parse(value));
      }

      return arr;
    }

    private static object ParseValue(string jsvalue)
    {
      object o = null;

      if (!IsQuotedString(jsvalue))
      {      
        try { return Double.Parse(jsvalue, CultureInfo.InvariantCulture); }
        catch (Exception) {}
      }
      else 
      {
        jsvalue = jsvalue.Substring(1, jsvalue.Length - 2);
      }

      if (jsvalue == "true" || jsvalue == "false")
        o = (jsvalue == "true" ? true : false);
      else
        o = jsvalue;

      return o;
    }
    
    private static bool IsQuotedString(string jsvalue) 
    {
        return ( (jsvalue.Length >= 2) 
            && ( (jsvalue[0] == '"' && jsvalue[jsvalue.Length - 1] == '"')
            || (jsvalue[0] == '\'' && jsvalue[jsvalue.Length - 1] == '\'') ) );
    }
    

    private static int findDelimiterIndex(string s, char delimiter)
    {
      int obj_count = 0;
      int arr_count = 0;
      bool in_stringS = false;
      bool in_stringD = false;
      bool in_escape = false;

      for (int i = 0; i < s.Length; i++)
      {
        char c = s[i];

        switch (c)
        {
          case '\\':
            if (in_stringS || in_stringD)
              in_escape = !in_escape;
            break;

          case '\'':
            if (!in_stringD)
            {
              if (!in_stringS)
                in_stringS = true;
              else if (!in_escape)
                in_stringS = false;
            }

            in_escape = false;
            break;

          case '"':
            if (!in_stringS)
            {
              if (!in_stringD)
                in_stringD = true;
              else if (!in_escape)
                in_stringD = false;
            }

            in_escape = false;
            break;

          default:
            in_escape = false;

            if (!in_stringS && !in_stringD)
            {
              switch (c)
              {
                case '{':
                  ++obj_count;
                  break;

                case '}':
                  --obj_count;
                  break;

                case '[':
                  ++arr_count;
                  break;

                case ']':
                  --arr_count;
                  break;

                default:
                  if (c == delimiter && obj_count == 0 && arr_count == 0)
                    return i;
                  break;
              }
            }
            break;
        }
      }

      return -1;
    }

  }
}
