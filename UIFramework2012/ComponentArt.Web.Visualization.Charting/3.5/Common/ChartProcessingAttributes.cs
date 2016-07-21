using System;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Microsoft.CSharp;
using Microsoft.VisualBasic;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// ========================================================================================
	//	Run Time Compiled Source
	// ========================================================================================

	[Editor(typeof(RunTimeCompiledSourceEditor), typeof(UITypeEditor))]
	[Serializable()]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]


	public class RunTimeCompiledSource
	{
		private string source;
		private ArrayList references;
		private bool isCSharpCode = true;
		private string sourceFileName;
        private string result = "Not compiled";

		private const string initialCSharpSource =
@"using System;


public class CustomChartHandler : RunTimeCompiledChartHandler
{
    public CustomChartHandler(IChart chart) : base (chart) { }
    public override void DoBeforeDataBind()
    {
        // Enter code to be executed immediatelly before data binding.

        // Note: Use the 'Chart' property to access the chart object
    }

    public override void DoAfterDataBind()
    {
        // Enter code to be executed immediatelly after data binding.
        // Use this method to set properties to elements of the chart
        //     object model, such as data points, elements of coordinate
        //     system, minimum and maximum values etc.

        // Note: Use the 'Chart' property to access the chart object
    }
}
" ;

		private const string initialVBSource =
@"Imports System
Imports ComponentArt.Web.Visualization.Charting

Public Class CustomChartHandler 
    Inherits RunTimeCompiledChartHandler

    Public Sub New(ChartObject as IChart) 
        MyBase.New(ChartObject)
    End Sub

    Public Overrides Sub DoBeforeDataBind()

        ' Enter code to be executed immediatelly before data binding.

        ' Note: Use the 'Chart' property to access the chart object
    End Sub

    Public Overrides Sub DoAfterDataBind()
   
        ' Enter code to be executed immediatelly after data binding.
        ' Use this method to set properties to elements of the chart
        '     object model, such as data points, elements of coordinate
        '     system, minimum and maximum values etc.

        ' Note: Use the 'Chart' property to access the chart object
    End Sub

End Class
";

		public RunTimeCompiledSource()
		{
			references = new ArrayList();
			references.Add("System.dll");
			references.Add("System.Xml.dll");
			references.Add("System.Windows.Forms.dll");
			references.Add("System.Drawing.dll");
			references.Add("System.Data.dll");
			references.Add("System.Design.dll");
		}

		internal string InitialSource
		{
			get
			{
				if(isCSharpCode)
					return initialCSharpSource;
				else
					return initialVBSource;
			}
		}

		[DefaultValue(true)]
		public bool IsCSSource { get { return isCSharpCode; } set { isCSharpCode = value; } }

		[DefaultValue(false)]
		public bool IsVBSource { get { return !isCSharpCode; } set { isCSharpCode = !value; } }

		[DefaultValue(null)]
		public string Source 
		{ 
			get { return source; }
			set { source = value; }
		}

		internal static string InitialVBSource { get { return (string)initialVBSource.Clone(); } }
		internal static string InitialCSSource { get { return (string)initialCSharpSource.Clone(); } }

		[DefaultValue(null)]
		internal string SourceFileName 
		{ 
			get { return sourceFileName; }
			set 
			{
				if(sourceFileName == value)
					return;
				sourceFileName = value;
				try
				{
					StreamReader reader = new StreamReader(sourceFileName);
					source = reader.ReadToEnd();
					reader.Close();
				}
				catch
				{
					throw new Exception("Cannot read run-time compiled source from file '" +
						sourceFileName + "'");
				}
			}
		}

		internal string[] References
		{
			get
			{
				return (string[])references.ToArray(typeof(string));
			}
		}

		public void AddReference(string reference)
		{
			references.Add(reference);
		}

		public void RemoveReference(string reference)
		{
			references.Remove(reference);
		}

		public CompilerResults Compile(bool inDesignMode)
		{
			// Obtain an ICodeCompiler from a CodeDomProvider class.  
			ICodeCompiler compiler;

			if(isCSharpCode)
			{
				CSharpCodeProvider provider = new CSharpCodeProvider();
				compiler = provider.CreateCompiler();
			}
			else
			{
				VBCodeProvider provider = new VBCodeProvider();
				compiler = provider.CreateCompiler();
			}

			// Build the parameters for source compilation.
			CompilerParameters cp = new CompilerParameters();

			// Add an assembly reference.
			foreach(string r in references)
				cp.ReferencedAssemblies.Add(r);
            // Add reference to the charting asembly
            string location;
            if (inDesignMode)
                location = Assembly.GetAssembly(typeof(Series)).GetModules()[0].FullyQualifiedName;
            else
                location = Assembly.GetExecutingAssembly().Location;
			cp.ReferencedAssemblies.Add(location);

			// Generate a dll
			cp.GenerateExecutable = false;

			// Keep assembly file in memory
			cp.GenerateInMemory = true;

			// Invoke compilation.
			return compiler.CompileAssemblyFromSource(cp, source);
		}

		internal void ProcessBeforeDataBinding(object ownerControl)
		{
			RunTimeCompiledChartHandler chartHandler = GetRunTimeChartHandler(ownerControl);
			if (chartHandler == null)
				return;

			MethodInfo mi = chartHandler.GetType().GetMethod("DoBeforeDataBind");
			if (mi == null)
				return;
			mi.Invoke(chartHandler, new object[] { });
		}

		internal void ProcessAfterDataBinding(object ownerControl)
		{
			RunTimeCompiledChartHandler chartHandler = GetRunTimeChartHandler(ownerControl);
			if (chartHandler == null)
				return;

			MethodInfo mi = chartHandler.GetType().GetMethod("DoAfterDataBind");
			if (mi == null)
				return;
			mi.Invoke(chartHandler, new object[] { });
		}

		private RunTimeCompiledChartHandler GetRunTimeChartHandler(object ownerControl)
		{
			CompilerResults cr = Compile(false);
			if(cr.Errors.HasErrors)
			{
				string message = "Run-time compilation error:";
				foreach(CompilerError error in cr.Errors)
				{
					if(!error.IsWarning)
						message += "\n  Line " + error.Line + " " + error.ErrorText;
				}
                result = message;
				throw new Exception(message);
			}
			Assembly asm = cr.CompiledAssembly;
            result = "No errors";

			// Search for class implementing RunTimeCompiledChartHandler
			Type handlerType = null;
			foreach (Type t in asm.GetTypes())
			{
				if (t.IsSubclassOf(typeof(RunTimeCompiledChartHandler)))
				{
					if (handlerType == null)
						handlerType = t;

					else
					{
						throw new Exception("More than one class in run-time compiled chart handler" +
							" inherits from 'RunTimeCompiledChartHandler'");
					}
				}
			}

			if (handlerType == null)
				return null;

			ConstructorInfo ci = handlerType.GetConstructor(new Type[] { typeof(IChart) });
			if (ci == null)
			{
				throw new Exception("The run-time compiled chart handler" +
					" does not have appropriate constructor");
			}
            return ci.Invoke(new object[] { ownerControl }) as RunTimeCompiledChartHandler;
		}

        public override string ToString()
        {
            return result;
        }

	}


	public class RunTimeCompiledChartHandler
	{
		private IChart chart;

		public RunTimeCompiledChartHandler(IChart chart)
		{
			this.chart = chart;
		}

		public IChart Chart { get { return chart; } }

		public virtual void DoBeforeDataBind()
		{
		}

		public virtual void DoAfterDataBind()
		{
		}

	};

	// ========================================================================================
	//	Editor
	// ========================================================================================

	internal class RunTimeCompiledSourceEditor : UITypeEditor
	{
		private RunTimeCompiledSourceView view = null; 

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if(provider == null)
				return value;
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc == null) 
				return value;

			RunTimeCompiledSource rtcs = value as RunTimeCompiledSource;
			if(value == null)
				rtcs = new RunTimeCompiledSource();
			if(rtcs == null)
				return value;

			view = new RunTimeCompiledSourceView(rtcs);

			DialogResult dr = edSvc.ShowDialog(view);
			if(dr == DialogResult.OK)
				rtcs = view.GetRunTimeCompiledSource();
			else if(dr == DialogResult.No)
				rtcs = null;

			return rtcs;
		}
 
	}
}

