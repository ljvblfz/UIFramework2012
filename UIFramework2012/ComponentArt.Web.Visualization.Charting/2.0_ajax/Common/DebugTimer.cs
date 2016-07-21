using System;
using System.Collections;
using System.Diagnostics;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class DebugTimer
	{
		string m_comments = "";
		DateTime m_start;

		public DebugTimer()
		{
		}

		public void StartTimer() 
		{
			m_start = DateTime.Now;
			m_comments = "0 - Timer started\r\n";
		}

		public void CheckPoint(string caption) 
		{
			TimeSpan now = DateTime.Now - m_start;
			m_comments = m_comments + now.ToString() + " - " + caption + "\r\n";
		}

		public string Comments 
		{
			get 
			{
				return m_comments;
			}
		}

	}

	internal class DebugLeakingDetector
	{
		long memory = 0;
		string startLine = "";
		bool enableCollection = false;
		internal DebugLeakingDetector()
		{
#if DEBUG
			if(enableCollection)
				memory = GC.GetTotalMemory(true);
			else
				memory = 0;
			StackTrace st1 = new StackTrace(1,true);
			StackFrame sf = st1.GetFrame(0);
			startLine = sf.GetMethod().ToString() + " @ " + ShortFileName(sf.GetFileName()) + "[" + sf.GetFileLineNumber() + "-";
#endif
		}

		internal long LeakToHere
		{
			get
			{
#if DEBUG
				if(enableCollection)
					return GC.GetTotalMemory(true) - memory;
				else
					return 0;
#else
				return 0;
#endif
			}
		}

		internal void ReportLeak()
		{
#if DEBUG
			long leak = enableCollection? GC.GetTotalMemory(true)-memory:0;
			if(leak>0)
			{
				StackTrace st1 = new StackTrace(1,true);
				StackFrame sf = st1.GetFrame(0);
				string message = startLine + sf.GetFileLineNumber() + "] = " + leak.ToString("###,###,##0");
				Debug.WriteLine(message);
			}
#endif
		}
		internal static string ShortFileName(string fileName)
		{
			if(fileName == null || fileName == "")
				return "";
			string[] parts = fileName.Split(new Char[] { '\\' },20);
			if(parts.Length > 0)
				return parts[parts.Length -1];
			else
				return fileName;
		}

	}

	internal struct Instance
	{
		public string where;
		public object what;
	}
	internal class ObjectTracker
	{
		Hashtable table = new Hashtable();
		internal void Created(object obj,int frameOffset)
		{
			Instance inst = new Instance();
			inst.what = obj;
			StackTrace st1 = new StackTrace(1+frameOffset,true);
			StackFrame sf = st1.GetFrame(0);
			inst.where = DebugLeakingDetector.ShortFileName(sf.GetFileName()) + "[" + sf.GetFileLineNumber() + "]";
			table[obj] = inst;
		}

		internal void Deleted(object obj)
		{
			table.Remove(obj);
		}

		internal void ReportLeaks()
		{
			if(table.Count == 0)
			{
				Debug.WriteLine("--- All created objects are deleted ---");
				return;
			}

			ICollection values = table.Values;
			foreach(Instance inst in values)
			{
				Debug.WriteLine("Leaked: " + inst.what.GetType().Name + " created @ " + inst.where);
			}
		}

		internal void Clear()
		{
			table.Clear();
		}
		
	}
}
