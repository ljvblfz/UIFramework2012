using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Web.UI;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: AssemblyCompany("ComponentArt")]
[assembly: AssemblyCopyright("Copyright 2011, ComponentArt")]
[assembly: AssemblyVersion("2012.1.1016.35")]
[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers()]
[assembly: TagPrefix("ComponentArt.Web.UI", "ComponentArt")]


[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573G988.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573Z388.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573P290.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573P291.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573S188.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573T069.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.client_scripts.A573L991.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Callback.client_scripts.A573P191.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Calendar.client_scripts.A573Q148.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Calendar.client_scripts.A573W128.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.client_scripts.A573C779.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ComboBox.client_scripts.A573P123.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ComboBox.client_scripts.A573P124.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ComboBox.client_scripts.A573P456.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Dialog.client_scripts.A573G999.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Dialog.client_scripts.A573G130.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Editor.client_scripts.A572GI43.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Editor.client_scripts.A572GI44.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Editor.client_scripts.A572GI45.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Editor.client_scripts.A572GI46.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Grid.client_scripts.A573R378.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Grid.client_scripts.A573R178.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Grid.client_scripts.A573G188.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Grid.client_scripts.A573J198.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Grid.client_scripts.A573L238.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Input.client_scripts.A570I433.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Input.client_scripts.A570I432.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Input.client_scripts.A570I431.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Input.client_scripts.A579I433.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Input.client_scripts.A579I432.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.MultiPage.client_scripts.A573A488.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Menu.client_scripts.A573W888.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Menu.client_scripts.A573Q288.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Menu.client_scripts.A573R388.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.NavBar.client_scripts.A573E888.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.NavBar.client_scripts.A573D588.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.NavBar.client_scripts.A573M488.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Rotator.client_scripts.A573Z288.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Rotator.client_scripts.A573G788.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Slider.client_scripts.A573HR343.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Scheduler.client_scripts.A577AB33.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Scheduler.client_scripts.A577AB34.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Scheduler.client_scripts.A577AB36.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573U699.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573P288.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573J988.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573V588.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573X288.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573K688.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573W988.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Snap.client_scripts.A573T388.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Splitter.client_scripts.A573J482.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.SpellCheck.client_scripts.A573O912.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.TreeView.client_scripts.A573S388.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.TreeView.client_scripts.A573O788.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.TreeView.client_scripts.A573R288.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.TabStrip.client_scripts.A573C488.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.TabStrip.client_scripts.A573I688.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.TabStrip.client_scripts.A573B188.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ToolBar.client_scripts.A573B288.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ToolBar.client_scripts.A573I788.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ToolBar.client_scripts.A573H988.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Upload.client_scripts.A573P101.js", "text/javascript")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Input.defaultStyle.css", "text/css")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.Menu.accessibleStyle.css", "text/css")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.blank.gif", "image/gif")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.grip.gif", "image/gif")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.crosshair.gif", "image/gif")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.hsvbloom.png", "image/png")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.hsvwheel.png", "image/png")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.hue.png", "image/png")]
[assembly: WebResourceAttribute("ComponentArt.Web.UI.ColorPicker.images.saturation.png", "image/png")]