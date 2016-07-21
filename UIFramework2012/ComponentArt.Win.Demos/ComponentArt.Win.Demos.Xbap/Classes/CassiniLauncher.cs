using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;


namespace ComponentArt.Win.Demos
{
  public class CassiniLauncher
  {
    private static string registryRoot = "SOFTWARE\\ComponentArt\\UIFramework";
    private static string registryCassiniNode = "CassiniLoc";

    string _server = null;
    string _root = null;

    private Process _proc;


    public CassiniLauncher(string root)
    {
        // server locations
        string server2005 = Path.Combine(
            Environment.GetEnvironmentVariable("WINDIR"),
            @"Microsoft.NET\Framework\v2.0.50727\WebDev.WebServer.EXE");
        string server9 = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
            @"Microsoft Shared\DevServer\9.0\WebDev.WebServer.EXE");
        string server9x64 = Path.Combine(
            @"C:\Program Files (x86)\",
            @"Common Files\Microsoft Shared\DevServer\9.0\WebDev.WebServer.EXE");
        string server10 = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
            @"Microsoft Shared\DevServer\10.0\WebDev.WebServer20.EXE");
        string server10x64 = Path.Combine(
            @"C:\Program Files (x86)\",
            @"Common Files\Microsoft Shared\DevServer\10.0\WebDev.WebServer20.EXE");
        string server104 = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
            @"Microsoft Shared\DevServer\10.0\WebDev.WebServer40.EXE");
        string server104x64 = Path.Combine(
            @"C:\Program Files (x86)\",
            @"Common Files\Microsoft Shared\DevServer\10.0\WebDev.WebServer40.EXE");
        string serverUserCustom = GetCassiniLocationFromRegistry();

        _server = 
            File.Exists(server104) ? server104 :
            File.Exists(server104x64) ? server104x64 :
            File.Exists(server10) ? server10 :
            File.Exists(server10x64) ? server10x64 :
            File.Exists(server9) ? server9 :
            File.Exists(server9x64) ? server9x64 :
            File.Exists(server2005) ? server2005 :
            File.Exists(serverUserCustom) ? serverUserCustom : null;
                
        _root = root;
    }

    public Process GetCassiniProcess()
    {
      return _proc;
    }

    public string LaunchCassini()
    {
      // look for .web application
      string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
      while(!Directory.Exists(Path.Combine(dir, "..\\ComponentArt.Win.Demos.Web")))
      {
        if(dir != Directory.GetDirectoryRoot(dir))
        {
          dir = Directory.GetParent(dir).FullName; 
        }
        else
        {
          dir = "";
          break;
        }
      }

      if(dir != "")
      {
        string appPath = Path.Combine(Directory.GetParent(dir).FullName, "ComponentArt.Win.Demos.Web");
        return LaunchCassini(appPath, "");
      }
      else
      {
        return "";
      }
    }

    public string LaunchCassini(string path, string vpath)
    {
      int port = 0;
      if (_proc == null)
      {
        port = GetAvailablePort();
        _proc = StartWebServer(port, Path.Combine(_root, path), vpath);
        if (_proc == null) return null;
      }
      else if (_proc.HasExited)
      {
        _proc.Start();
      }

      port = int.Parse(Regex.Match(_proc.StartInfo.Arguments, @"/port:(\d+)").Groups[1].Value);
      System.Threading.Thread.Sleep(2000);

      return string.Format("http://localhost:{0}/{1}", port, vpath);
    }

    private int GetAvailablePort()
    {
      TcpListener listener = new TcpListener(IPAddress.Any, 0);
      listener.Start();
      int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
      listener.Stop();
      listener = null;
      return port;
    }

    private Process StartWebServer(int port, string path, string vpath)
    {
      //if we couldn't find Casini in its regular places, give the user a chance to locate it
      if (_server == null)
      {
        if (MessageBox.Show("The application could not locate your Cassini web server that is required to launch the Live Demos web site. \n"
                  + "Do you want to specify the location of your Cassini web server in order to launch the demos? ",
                  "ComponentArt UI Framework Live Demos", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          _server = UserLocateCassini();
          if (_server != null)
            StoreCassiniLocationInRegistry(_server);
        }
      }

      if (_server == null) return null;

      Process proc = new Process();
      proc.StartInfo.FileName = _server;
      proc.StartInfo.Arguments = string.Format("/port:{0} /path:\"{1}\" /vpath:\"/{2}\"",
          port, path, vpath);
      proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      proc.Start();
      System.Threading.Thread.Sleep(3000);
      return proc;
    }

    private String UserLocateCassini()
    {
      // Display an OpenFileDialog so the user can Locate his Cassini instance.
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "Cassini Web Server|WebDev.WebServer.EXE|All Files|*.*";
      ofd.Title = "Locate your Cassini Web Server";

      if (ofd.ShowDialog() ?? false)
        return ofd.FileName;
      else
        return null;
    }

    private void StoreCassiniLocationInRegistry(String loc)
    {
      try
      {
        RegistryKey regKey = Registry.LocalMachine.OpenSubKey(registryRoot, true);
        regKey.SetValue(registryCassiniNode, loc, RegistryValueKind.String);
      }
      catch (Exception e)
      {
        //ignore if it fails for whatever reason
      }
    }

    private String GetCassiniLocationFromRegistry()
    {
      try
      {
        RegistryKey regKey = Registry.LocalMachine.OpenSubKey(registryRoot);
        Object regValue = regKey.GetValue(registryCassiniNode);
        return (String)regValue;
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
