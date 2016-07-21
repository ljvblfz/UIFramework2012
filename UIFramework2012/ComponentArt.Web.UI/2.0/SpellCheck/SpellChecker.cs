using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  public class SpellError
  {
    public string Word;
    public int StartIndex;
    public int EndIndex;

    public SpellError(string sWord, int iStartIndex, int iEndIndex)
    {
      Word = sWord;
      StartIndex = iStartIndex;
      EndIndex = iEndIndex;
    }
  }

  internal class CaseInsensitiveEqualityComparer : IEqualityComparer
  {
    public CaseInsensitiveComparer myComparer;

    public CaseInsensitiveEqualityComparer()
    {
      myComparer = CaseInsensitiveComparer.DefaultInvariant;
    }

    public CaseInsensitiveEqualityComparer(System.Globalization.CultureInfo myCulture)
    {
      myComparer = new CaseInsensitiveComparer(myCulture);
    }

    public new bool Equals(object x, object y)
    {
      if (myComparer.Compare(x, y) == 0)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public int GetHashCode(object obj)
    {
      return obj.ToString().ToLower().GetHashCode();
    }
  }

  public class SpellChecker
  {
    char[] _letters = {'a','b','c','d','e','f','g','h','i','j','k','l','m',
        'n','o','p','q','r','s','t','u','v','w','x','y','z' };

    Hashtable _dictionary = null;

    #region Public Methods

    public SpellChecker(string sLanguage)
    {
      if (sLanguage != null && sLanguage != "")
      {
        _dictionary = EnsureDictionary(sLanguage, true);
      }
    }

    public bool AddWordToCustomDictionary(string sWord)
    {
      ISpellCheckCustomDictionaryManager oManager = this.GetCustomDictionaryManager();

      if (oManager != null)
      {
        return oManager.AddToCustomDictionary(HttpContext.Current, sWord);
      }

      return false;
    }

    public SpellError[] Check(string sText)
    {
      return this.Check(sText, true, true, true, true);
    }

    public SpellError[] Check(string sText, bool bIgnoreAcronyms, bool bIgnoreEmailAddresses, bool bIgnoreURLs, bool bIgnoreNonAlpha)
    {
      if (_dictionary == null)
      {
        throw new Exception("No dictionary loaded.");
      }

      ArrayList arErrors = new ArrayList();

      int iStartIndex = -1;

      for (int i = 0; i < sText.Length; i++)
      {
        bool bIsAlNum = this.IsAlphaNumeric(sText[i]);
        if (iStartIndex < 0 && bIsAlNum)
        {
          iStartIndex = i;
        }
        else if(iStartIndex >= 0 && (!bIsAlNum || i == sText.Length - 1))
        {
          int iLength = i - iStartIndex + (i == sText.Length - 1 && bIsAlNum? 1 : 0);
          string sWord = sText.Substring(iStartIndex, iLength);

          // does the word qualify for checking?
          if (this.IsQualified(sWord, bIgnoreAcronyms, bIgnoreEmailAddresses, bIgnoreURLs, bIgnoreNonAlpha))
          {
            // check the word
            if (!_dictionary.ContainsKey(sWord.ToLower()))
            {
              arErrors.Add(new SpellError(sWord, iStartIndex, iStartIndex + iLength));
            }  
          }

          iStartIndex = -1;
        }
      }

      return (SpellError [])arErrors.ToArray(typeof(SpellError));
    }

    public string DetectLanguage(string sText)
    {
      string sDictionaryKeyPrefix = "ComponentArtSpellDictionary/";

      string sBestFitLanguage = "";
      int iBestFit = int.MaxValue;

      ArrayList arKeys = new ArrayList(ConfigurationManager.AppSettings.Keys);

      foreach (string sKey in arKeys)
      {
        // find defined dictionaries
        if (sKey.StartsWith(sDictionaryKeyPrefix))
        {
          // what's the language?
          string sLanguage = sKey.Substring(sDictionaryKeyPrefix.Length);

          // load the dictionary for this language
          this.EnsureDictionary(sLanguage, false);

          // check the text
          SpellError[] arErrors = this.Check(sText);

          // is this the best fit?
          if (arErrors.Length < iBestFit)
          {
            iBestFit = arErrors.Length;
            sBestFitLanguage = sLanguage;
          }
        }
      }

      return sBestFitLanguage;
    }

    public ArrayList GetSuggestions(string sWord, SpellSuggestionDepthType eDepth)
    {
      if (_dictionary == null)
      {
        throw new Exception("No dictionary loaded.");
      }

      ArrayList arSuggestions = new ArrayList();

      // we have no suggestions if the word is correct
      if (_dictionary.ContainsKey(sWord.ToLower()))
      {
        return arSuggestions;
      }

      // Priority one: joined words
      for (int i = 1; i < sWord.Length; i++) // LO
      {
        string s1 = sWord.Substring(0, i).ToLower();
        string s2 = sWord.Substring(i).ToLower();

        if (_dictionary.ContainsKey(s1) && _dictionary.ContainsKey(s2))
        {
          arSuggestions.Add(this.GetCaseAdjustedWord(s1, sWord) + " " + this.GetCaseAdjustedWord(s2, sWord.Substring(s1.Length)));
        }

        // TODO: if only one part matches, check suggestions for the second one
      }

      ArrayList arPossibilities = new ArrayList();

      // Priority one: try built in word mutators
      arPossibilities.AddRange(this.GetTypoPossibilities(sWord)); // LO
      arPossibilities.AddRange(this.GetSwapPossibilities(sWord)); // LO
      arPossibilities.AddRange(this.GetExtraLetterPossibilities(sWord)); // LO
      arPossibilities.AddRange(this.GetMissingLetterPossibilities(sWord)); // LO

      // Priority one: try cheap combinations of built-in mutators
      arPossibilities.AddRange(this.GetMissingLetterPossibilities(this.GetMissingLetterPossibilities(sWord))); // LO
      arPossibilities.AddRange(this.GetSwapPossibilities(this.GetSwapPossibilities(sWord))); // LO
      
      // Priority two: try more expensive combinations
      if ((int)eDepth > 0)
      {
        arPossibilities.AddRange(this.GetExtraLetterPossibilities(this.GetSwapPossibilities(sWord))); // ME 100
        arPossibilities.AddRange(this.GetTypoPossibilities(this.GetMissingLetterPossibilities(sWord))); // ME 100
        arPossibilities.AddRange(this.GetSwapPossibilities(this.GetTypoPossibilities(sWord))); // ME 100
      }

      // word distance:
      if ((int)eDepth > 1)
      {
        int maxDistance = (int)(sWord.Length / 3);
        foreach (string sKey in _dictionary.Keys) // HI 500
        {
          if (sKey[0] == sWord[0] && Math.Abs(sKey.Length - sWord.Length) < 2)
          {
            if (this.GetDistance(sKey, sWord, maxDistance) < maxDistance)
            {
              arPossibilities.Add(sKey);
            }
          }
        }
      }

      //Console.Write (arPossibilities.Count.ToString() + " ");

      foreach (string sPossibility in arPossibilities)
      {
        if (_dictionary.ContainsKey(sPossibility.ToLower()))
        {
          string sSuggestion = this.GetCaseAdjustedWord(sPossibility, sWord);

          if (!arSuggestions.Contains(sSuggestion))
          {
            arSuggestions.Add(sSuggestion);
          }
        }
      }

      return arSuggestions;
    }

    public bool RemoveWordFromCustomDictionary(string sWord)
    {
      ISpellCheckCustomDictionaryManager oManager = this.GetCustomDictionaryManager();

      if (oManager != null)
      {
        return oManager.RemoveFromCustomDictionary(HttpContext.Current, sWord);
      }

      return false;
    }

    #endregion

    private ISpellCheckCustomDictionaryManager GetCustomDictionaryManager()
    {
      string sCustomDictionaryManagerKey = "ComponentArtSpellCheckCustomDictionaryManager";

      string sCustomDictionaryManager = ConfigurationManager.AppSettings[sCustomDictionaryManagerKey];

      if (sCustomDictionaryManager != null && sCustomDictionaryManager != "")
      {
        foreach (System.Reflection.Assembly oAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
          Type managerType = oAssembly.GetType(sCustomDictionaryManager, false);

          if(managerType != null)
          {
            object o = System.Activator.CreateInstance(managerType);

            if (o != null && o is ISpellCheckCustomDictionaryManager)
            {
              return (ISpellCheckCustomDictionaryManager)o;
            }
            else
            {
              return null;
            }
          }
        }
      }

      return null;
    }

    private Hashtable EnsureDictionary(string sLanguage, bool bCache)
    {
      string sDictionaryKey = "ComponentArtSpellCheckDictionary/" + sLanguage;

      // Make sure the dictionary is loaded.
      Hashtable oDictionary = null;

      HttpContext oContext = HttpContext.Current;

      if (oContext != null)
      {
        oDictionary = (Hashtable)oContext.Cache[sDictionaryKey];
      }

      if (oDictionary == null)
      {
        string sPath = ConfigurationManager.AppSettings[sDictionaryKey];

        if (sPath == null)
        {
          throw new Exception("No dictionary specified in web.config for the given language.");
        }

        string sRealPath = oContext.Server.MapPath(Utils.ConvertUrl(oContext, "", sPath));

        StreamReader oReader = new StreamReader(sRealPath);

        oDictionary = new Hashtable(new CaseInsensitiveEqualityComparer());
        string sDictionary = oReader.ReadToEnd();
        string [] sDictionaryLines = sDictionary.Split('\n');
            
        foreach(string sKey in sDictionaryLines)
        {
          string sTrimmedKey = sKey.Trim();

          if (sTrimmedKey.Length > 0 && !oDictionary.ContainsKey(sTrimmedKey))
          {
            oDictionary.Add(sTrimmedKey, null);
          }
        }

        if (oContext != null && bCache)
        {
          oContext.Cache.Add(sDictionaryKey, oDictionary, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
            TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
        }
      }

      // Do we need to add the custom dictionary?
      ISpellCheckCustomDictionaryManager oCustomDictionaryManager = this.GetCustomDictionaryManager();
      if (oCustomDictionaryManager != null)
      {
        string[] arCustomWords = oCustomDictionaryManager.GetCustomDictionary(oContext);

        if (arCustomWords != null && arCustomWords.Length > 0)
        {
          oDictionary = (Hashtable)oDictionary.Clone();

          foreach (string sCustomWord in arCustomWords)
          {
            oDictionary.Add(sCustomWord, null);
          }
        }
      }

      return oDictionary;
    }

    private string GetCaseAdjustedWord(string sWord, string sCasingWord)
    {
      int iUpperCase = 0;
      int iLowerCase = 0;

      char[] arLetters = sWord.ToCharArray();

      for (int i = 0; i < sWord.Length; i++)
      {
        if (i < sCasingWord.Length)
        {
          if (Char.IsLower(sCasingWord[i]))
          {
            arLetters[i] = Char.ToLower(arLetters[i]);
            iLowerCase++;
          }
          else
          {
            arLetters[i] = Char.ToUpper(arLetters[i]);
            iUpperCase++;
          }
        }
        else if(iUpperCase > iLowerCase)
        {
          arLetters[i] = Char.ToUpper(arLetters[i]);
        }
      }

      return new String(arLetters);
    }

    private ArrayList GetExtraLetterPossibilities(ArrayList arWords)
    {
      ArrayList results = new ArrayList();

      foreach (string sWord in arWords)
      {
        results.AddRange(this.GetExtraLetterPossibilities(sWord));
      }

      return results;
    }

    private ArrayList GetExtraLetterPossibilities(string sWord)
    {
      ArrayList results = new ArrayList();
      ArrayList arChars = new ArrayList(sWord.ToCharArray());

      for (int i = 0; i < arChars.Count + 1; i++)
      {
        for (int j = 0; j < _letters.Length; j++)
        {
          ArrayList newChars = (ArrayList)arChars.Clone();
          newChars.Insert(i, _letters[j]);

          results.Add(new String((char[])(newChars.ToArray(typeof(char)))));
        }
      }

      return results;
    }

    private ArrayList GetMissingLetterPossibilities(ArrayList arWords)
    {
      ArrayList results = new ArrayList();

      foreach (string sWord in arWords)
      {
        results.AddRange(this.GetMissingLetterPossibilities(sWord));
      }

      return results;
    }

    private ArrayList GetMissingLetterPossibilities(string sWord)
    {
      ArrayList results = new ArrayList();
      ArrayList arChars = new ArrayList(sWord.ToCharArray());

      for (int i = 0; i < arChars.Count; i++)
      {
        ArrayList newChars = (ArrayList)arChars.Clone();
        newChars.RemoveAt(i);

        results.Add(new String((char[])(newChars.ToArray(typeof(char)))));
      }

      return results;
    }

    private ArrayList GetSwapPossibilities(ArrayList arWords)
    {
      ArrayList results = new ArrayList();

      foreach (string sWord in arWords)
      {
        results.AddRange(this.GetSwapPossibilities(sWord));
      }

      return results;
    }

    private ArrayList GetSwapPossibilities(string sWord)
    {
      ArrayList results = new ArrayList();

      char[] arChars = sWord.ToCharArray();
      
      for (int i = 0; i < arChars.Length - 1; i++)
      {
        char[] arSwapChars = (char [])arChars.Clone();
        char temp = arSwapChars[i];
        arSwapChars[i] = arSwapChars[i + 1];
        arSwapChars[i + 1] = temp;

        results.Add(new String(arSwapChars));
      }

      return results;
    }

    private ArrayList GetTypoPossibilities(ArrayList arWords)
    {
      ArrayList results = new ArrayList();

      foreach (string sWord in arWords)
      {
        results.AddRange(this.GetTypoPossibilities(sWord));
      }

      return results;
    }

    private ArrayList GetTypoPossibilities(string sWord)
    {
      ArrayList results = new ArrayList();
      ArrayList arChars = new ArrayList(sWord.ToCharArray());

      for (int i = 0; i < arChars.Count; i++)
      {
        for (int j = 0; j < _letters.Length; j++)
        {
          ArrayList newChars = (ArrayList)arChars.Clone();
          newChars[i] = _letters[j];

          results.Add(new String((char[])(newChars.ToArray(typeof(char)))));
        }
      }

      return results;
    }

    private int GetDistance(string s1, string s2, int max)
    {
      int[][] d; // matrix
      int n; // length of s
      int m; // length of t
      char s_i; // ith character of s
      char t_j; // jth character of t
      int cost; // cost

      // Step 1

      n = s1.Length;
      m = s2.Length;

      if (n == 0)
      {
        return m;
      }
      if (m == 0)
      {
        return n;
      }

      d = new int[n + 1][];
      for (int i = 0; i < n + 1; i++)
      {
        d[i] = new int[m + 1];
      }

      // Step 2

      for (int i = 0; i <= n; i++)
      {
        d[i][0] = i;
      }

      for (int j = 0; j <= m; j++)
      {
        d[0][j] = j;
      }

      // Step 3

      for (int i = 1; i <= n; i++)
      {

        s_i = s1[i - 1];

        // Step 4

        for (int j = 1; j <= m; j++)
        {

          t_j = s2[j - 1];

          // Step 5

          if (s_i == t_j)
          {
            cost = 0;
          }
          else
          {
            cost = 1;
          }

          // Step 6

          d[i][j] = Math.Min(d[i - 1][j] + 1, Math.Min(d[i][j - 1] + 1, d[i - 1][j - 1] + cost));

          if (i == j && d[i][j] > max)
          {
            return max;
          }
        }
      }

      // Step 7

      return d[n][m];
    }

    private bool IsAlphaNumeric(char c)
    {
      return Char.IsLetterOrDigit(c);
    }

    private bool IsQualified(string sWord, bool bIgnoreAcronyms, bool bIgnoreEmailAddresses, bool bIgnoreURLs, bool bIgnoreNonAlpha)
    {
      // for now, only qualify words with all-letters

      // single letter words are no good
      if(sWord.Length < 2) return false;

      // email addresses no good?
      if (bIgnoreEmailAddresses && sWord.IndexOf("@") >= 0 && sWord.IndexOf(".") >= 0) return false;

      // urls no good?
      if (bIgnoreURLs && sWord.IndexOf("://") >= 0 && sWord.IndexOf(".") >= 0) return false;

      // go through the letters
      bool bIsAcronym = true;
      bool bAllLetters = true;

      for(int i = 0; i < sWord.Length; i++)
      {
        if (!Char.IsUpper(sWord[i]))
        {
          bIsAcronym = false;
        }
        if(!Char.IsLetter(sWord[i]))
        {
          bAllLetters = false;
        }
      }

      // acronyms no good?
      if (bIgnoreAcronyms && bIsAcronym) return false;

      // non alphabetic no good?
      if (bIgnoreNonAlpha && !bAllLetters) return false;

      return true;
    }
  }
}
