using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;

// Hwan 2009-12-02
// this class requires System.Web.Mvc, System.Web.Abstractions
using System.Web.Mvc;

namespace ComponentArt.Web.UI
{
  public static class ComponentArtExtensions
  {
    public class ComponentFactory
    {
      private readonly HtmlHelper htmlHelper;
      private List<string> clientScripts = new List<string>();
      private List<string> renderedClientScripts = new List<string>();

      public ComponentFactory(HtmlHelper htmlHelper)
      {
        this.htmlHelper = htmlHelper;
      }

      public CalendarBuilder Calendar()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.Calendar().ToString());

        return new CalendarBuilder(null, htmlHelper.ViewContext);
      }
      public CalendarBuilder Calendar(object boundModel)
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.Calendar().ToString());

        return new CalendarBuilder(boundModel, htmlHelper.ViewContext);
      }

      public ComboBoxBuilder ComboBox()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.ComboBox().ToString());

        return new ComboBoxBuilder(null, htmlHelper.ViewContext);
      }

      public ComboBoxBuilder ComboBox(object boundModel)
      {

        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.ComboBox().ToString());

        return new ComboBoxBuilder(boundModel, htmlHelper.ViewContext);
      }

      public DataGridBuilder DataGrid()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.DataGrid().ToString());

        return new DataGridBuilder(null, htmlHelper.ViewContext);
      }

      public DataGridBuilder DataGrid(object boundModel)
      {
         
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.DataGrid().ToString());

        return new DataGridBuilder(boundModel, htmlHelper.ViewContext);
      }

      public NavBarBuilder NavBar()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.NavBar().ToString());

        return new NavBarBuilder();
      }

      public MenuBuilder Menu()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.Menu().ToString());

        return new MenuBuilder();
      }

      public TabStripBuilder TabStrip()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.TabStrip().ToString());

        return new TabStripBuilder();
      }

      public ToolBarBuilder ToolBar()
      {
          ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
          htmlHelper.ViewContext.HttpContext.Response.Write(csb.ToolBar().ToString());

          return new ToolBarBuilder();
      }

      public TreeViewBuilder TreeView()
      {
        ClientScriptFactory csb = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        htmlHelper.ViewContext.HttpContext.Response.Write(csb.TreeView().ToString());

        return new TreeViewBuilder();
      }

      public ClientScriptBuilder ClientScripts(Action<ClientScriptFactory> addAction)
      {
        var factory = new ClientScriptFactory(ref clientScripts, ref renderedClientScripts);
        addAction(factory);
        return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
      }
    }

    private static readonly string Key = typeof(ComponentFactory).AssemblyQualifiedName;

    public static ComponentFactory ComponentArt(this HtmlHelper htmlHelper)
    {
      ViewContext viewContext = htmlHelper.ViewContext;
      HttpContextBase httpContext = viewContext.HttpContext;
      ComponentFactory factory = httpContext.Items[Key] as ComponentFactory;

      if (factory == null)
      {
        factory = new ComponentFactory(htmlHelper);
        htmlHelper.ViewContext.HttpContext.Items[Key] = factory;
      }

      return factory;
    }
  }

  }
