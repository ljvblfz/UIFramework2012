window.TreeView2_Load = function(sender, eventArgs) {
  sender.get_nodes().getNode(0).expand();
}
window.TreeView2_NodeSelect = function(sender, eventArgs) {
  if (!eventArgs.get_node().get_useWebService()) {
    var view = {
      SoaFilters: [{ DataFieldName: 'FeedName', DataFieldValue: eventArgs.get_node().get_text()}]
    };

    LoadView(view);
  }
}
// Takes a view object, with the following properties:
// ---------------------------------------------------
// .SoaFilters = [ { DataFieldName: 'FeedName', DataFieldValue: 'Slashdot' } ]
// .SoaGroupings = [ { ColumnID: 'FeedName', IsAscending: true } ]
// .SortColumnIndex = 4
// .SortColumnDirection = 1
// ---------------------------------------------------
window.LoadView = function(view, bDontAddToHistory) {
  // set filters
  Grid1.SoaFilters = view.SoaFilters;

  // reload
  Grid1.set_recordOffset(0);
  Grid1.webServiceSelect();
}
window.Grid1_WebServiceBeforeInvoke = function(sender, eventArgs) {
  sender.loadingPanelShow();
}
window.Grid1_WebServiceBeforeComplete = function(sender, eventArgs) {
  sender.loadingPanelHide();
}

// BEGIN DATE FORMATTING


window.DateTimeFormatter = function()
{
  this.AbbreviatedDayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  this.AbbreviatedMonthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  this.AMDesignator = 'AM';
  this.DayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  this.MonthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  this.PMDesignator = 'PM';
  this.set_dateTime(new Date());
  this.set_formatString('yyyy-MM-dd h:mm:ss tt');
}

DateTimeFormatter.prototype.get_abbreviatedDayNames = function()
{
  return this.AbbreviatedDayNames;
}
DateTimeFormatter.prototype.set_abbreviatedDayNames = function(value)
{
  this.AbbreviatedDayNames = value;
}
DateTimeFormatter.prototype.get_abbreviatedMonthNames = function()
{
  return this.AbbreviatedMonthNames;
}
DateTimeFormatter.prototype.set_abbreviatedMonthNames = function(value)
{
  this.AbbreviatedMonthNames = value;
}
DateTimeFormatter.prototype.get_dayNames = function()
{
  return this.DayNames;
}
DateTimeFormatter.prototype.set_dayNames = function(value)
{
  this.DayNames = value;
}
DateTimeFormatter.prototype.get_monthNames = function()
{
  return this.MonthNames;
}
DateTimeFormatter.prototype.set_monthNames = function(value)
{
  this.MonthNames = value;
}
DateTimeFormatter.prototype.get_amDesignator = function()
{
  return this.AMDesignator;
}
DateTimeFormatter.prototype.set_amDesignator = function(value)
{
  this.AMDesignator = value;
}
DateTimeFormatter.prototype.get_pmDesignator = function()
{
  return this.PMDesignator;
}
DateTimeFormatter.prototype.set_pmDesignator = function(value)
{
  this.PMDesignator = value;
}

DateTimeFormatter.prototype.get_dateTime = function()
{
  return this.DateTime;
}
DateTimeFormatter.prototype.set_dateTime = function(value)
{
  this.DateTime = value;
  this.DateTimeTokenized = DateTimeFormatter.tokenizeDateTime(this.DateTime);
}

DateTimeFormatter.prototype.get_formatString = function()
{
  return this.FormatString;
}
DateTimeFormatter.prototype.set_formatString = function(value)
{
  this.FormatString = value;
  this.FormatStringTokenized = DateTimeFormatter.tokenizeFormatString(this.FormatString);
}

DateTimeFormatter.prototype.toString = function(dateTime, formatString)
{
  var dateTimeTokens = (dateTime === void 0) ? this.DateTimeTokens : DateTimeFormatter.tokenizeDateTime(dateTime);
  var formatTokens = (formatString === void 0) ? this.FormatTokens : DateTimeFormatter.tokenizeFormatString(formatString);
  var result = new Array();
  for (var i = 0; i < formatTokens.length; i++)
  {
    var formatToken = formatTokens[i];
    if (formatToken.IsLiteral)
    {
      result.push(formatToken.Value);
    }
    else
    {
      switch (formatToken.Value)
      {
        case 'd': result.push(dateTimeTokens.Day); break;
        case 'dd': result.push(dateTimeTokens.Day >= 10 ? '' + dateTimeTokens.Day : '0' + dateTimeTokens.Day); break;
        case 'ddd': result.push(this.AbbreviatedDayNames[dateTimeTokens.DayOfWeek]); break;
        case 'dddd': result.push(this.DayNames[dateTimeTokens.DayOfWeek]); break;
        case 'h': result.push('' + ((dateTimeTokens.Hour + 11) % 12 + 1)); break;
        case 'hh': result.push(((dateTimeTokens.Hour + 11) % 12 + 1) >= 10 ? '' + ((dateTimeTokens.Hour + 11) % 12 + 1) : '0' + ((dateTimeTokens.Hour + 11) % 12 + 1)); break;
        case 'H': result.push('' + dateTimeTokens.Hour); break;
        case 'HH': result.push(dateTimeTokens.Hour >= 10 ? '' + dateTimeTokens.Hour : '0' + dateTimeTokens.Hour); break;
        case 'm': result.push('' + dateTimeTokens.Minute); break;
        case 'mm': result.push(dateTimeTokens.Minute >= 10 ? '' + dateTimeTokens.Minute : '0' + dateTimeTokens.Minute); break;
        case 'M': result.push('' + (dateTimeTokens.Month + 1)); break;
        case 'MM': result.push((dateTimeTokens.Month + 1) >= 10 ? '' + (dateTimeTokens.Month + 1) : '0' + (dateTimeTokens.Month + 1)); break;
        case 'MMM': result.push(this.AbbreviatedMonthNames[dateTimeTokens.Month]); break;
        case 'MMMM': result.push(this.MonthNames[dateTimeTokens.Month]); break;
        case 's': result.push('' + dateTimeTokens.Second); break;
        case 'ss': result.push(dateTimeTokens.Second >= 10 ? '' + dateTimeTokens.Second : '0' + dateTimeTokens.Second); break;

        // I modified this to make 'T' and 'TT' work us upper case AM/PM designators, and 't' and 'tt' work us lower case AM/PM designators.  
        case 't': result.push(dateTimeTokens.Hour >= 12 ? '' + this.PMDesignator.toLowerCase().charAt(0) : '' + this.AMDesignator.toLowerCase().charAt(0)); break;
        case 'tt': result.push(dateTimeTokens.Hour >= 12 ? this.PMDesignator.toLowerCase() : this.AMDesignator.toLowerCase()); break;
        case 'T': result.push(dateTimeTokens.Hour >= 12 ? '' + this.PMDesignator.toUpperCase().charAt(0) : '' + this.AMDesignator.toUpperCase().charAt(0)); break;
        case 'TT': result.push(dateTimeTokens.Hour >= 12 ? this.PMDesignator.toUpperCase() : this.AMDesignator.toUpperCase()); break;

        case 'y': result.push('' + (dateTimeTokens.Year % 100)); break;
        case 'yy': result.push((dateTimeTokens.Year % 100) >= 10 ? '' + (dateTimeTokens.Year % 100) : '0' + (dateTimeTokens.Year % 100)); break;
        case 'yyy': case 'yyyy': result.push('' + dateTimeTokens.Year); break;
      }
    }
  }
  //bujar.push({ 'dateTimeTokens': dateTimeTokens, 'formatTokens': formatTokens, 'toString': result.join('') });
  return result.join('');
}

DateTimeFormatter.tokenizeDateTime = function(datetime)
{
  return { 'Year': datetime.getFullYear(), 'Month': datetime.getMonth(), 'Day': datetime.getDate(), 'Hour': datetime.getHours(), 'Minute': datetime.getMinutes(), 'Second': datetime.getSeconds(), 'DayOfWeek': datetime.getDay() };
}

// Counts how many consecutive chr characters are found in str starting at startIndex
DateTimeFormatter.countChars = function(str, chr, startIndex, maxResult)
{
  var result = 0;
  var max = Math.min(str.length - startIndex, maxResult);
  while (result < max && str.charAt(startIndex + result) == chr)
  {
    result++;
  }
  return result;
}

// Creates a string of length length consisting entirely of chr characters
DateTimeFormatter.charString = function(chr, length)
{
  var result = new Array();
  for (var i = 0; i < length; i++)
  {
    result[i] = chr;
  }
  return result.join('');
}

DateTimeFormatter.tokenizeFormatString = function(formatString)
{
  var tokens = new Array();
  var index = 0;
  var inQuotation = false; /* Indicates whether we are currently inside of a quotation in our
                            * date format string.  For example, for string "MMM dd'oo' yyyy",
                            * it will be true when index is pointing at one of the two o's. */
  var curStr = new Array(); /* Used to build up the current token */
  while (index < formatString.length)
  {
    var curChar = formatString.charAt(index);
    if (!inQuotation)
    {
      switch (curChar)
      {
        case "'":
          inQuotation = true;
          index++;
          break;

        // I modified this to make 'T' and 'TT' work us upper case AM/PM designators, and 't' and 'tt' work us lower case AM/PM designators.  
        case "d": case "h": case "H": case "m": case "M": case "s": case "t": case "T": case "y": /* We are entering a Symbol */
          if (curStr.length > 0) /* If we already have something built up in our curStr "buffer", 
                                * flush it, it's a Literal. (Note that only literals ever enter the buffer, all 
                                * Symbols are dealt with here immediately, in the same loop iteration in which 
                                * they are encountered.) */
          {
            tokens.push({ 'IsLiteral': true, 'Value': curStr.join('') });
            curStr = new Array();
          }
          /* The for loop may look a bit cryptic at first, but it is quite simple.
          * It tries to match MMMM, then MMM, then MM and finally M.
          * For M, d, and y the longest pattern it tries to find is of length four (MMMM, dddd, or yyyy).
          * For h, H, m, s, and t the longest pattern it tries to find is of length two (hh, HH, mm, ss, or tt).
          * It's always guaranteed to succeed at least on the last (single-letter) try. */
          var pattern = "";
          var maximumPatternLength = 0;
          switch (curChar)
          {
            // I modified this to make 'T' and 'TT' work us upper case AM/PM designators, and 't' and 'tt' work us lower case AM/PM designators.   
            case "h": case "H": case "m": case "s": case "t": case "T":
              maximumPatternLength = 2;
              break;
            case "d": case "M": case "y":
              maximumPatternLength = 4;
              break;
          }
          var patternLength = DateTimeFormatter.countChars(formatString, curChar, index, maximumPatternLength);
          tokens.push({ 'IsLiteral': false, 'Value': DateTimeFormatter.charString(curChar, patternLength) });
          index += patternLength;
          break;

        default: /* Just append to the literal in construction */
          curStr[curStr.length] = curChar;
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
      var quoteCount = DateTimeFormatter.countChars(formatString, '\'', index, 2);
      switch (quoteCount)
      {
        case 2:
          curStr[curStr.length] = '\'';
          index += 2;
          break;

        case 1:
          inQuotation = false;
          index++;
          break;

        case 0:
          curStr[curStr.length] = curChar;
          index++;
          break;
      }
    }
    if ((index >= formatString.length) && (curStr.length > 0))
    /* If we reached the end of formatString and have something "buffered" in curStr, flush it, it's a Literal. */
    {
      tokens.push({ 'IsLiteral': true, 'Value': curStr.join('') });
    }
  }
  return tokens;
}

window.GridDateFormatter = new DateTimeFormatter();








