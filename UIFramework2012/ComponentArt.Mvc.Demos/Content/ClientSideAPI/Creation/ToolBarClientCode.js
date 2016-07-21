// --------------------------------------------------------------------------
// ComponentArt ToolBar for ASP.NET - Client-side Structure Creation  
// --------------------------------------------------------------------------

function ToolBar1_onLoad(sender, eventArgs)
{
  // enable the 'Click to Create' button after ToolBar1 has been initialized
  document.getElementById('btnToolBarCreate').disabled = false;

  document.getElementById('ToolBar1').style.display = 'none';
}

function buildToolBar()
{
  ToolBar1.beginUpdate(); 
  populateToolBar();
  ToolBar1.endUpdate();

  document.getElementById('ToolBar1').style.display = 'block';
}

function populateToolBar()
{
  var item;
  var toolBarItems = ToolBar1.get_items();

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_toolTip("Cut");
  item.setProperty('IconCssClass', 'icon-edit-cut');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_toolTip("Copy");
  item.setProperty('IconCssClass', 'icon-edit-copy');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_toolTip("Paste");
  item.setProperty('IconCssClass', 'icon-edit-paste');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(2);
  item.set_toolTip("Replace");
  item.setProperty('IconCssClass', 'icon-edit-replace');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(1);
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(2);
  item.set_toolTip("Bold");
  item.setProperty('IconCssClass', 'icon-format-bold');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(2);
  item.set_toolTip("Italic");
  item.setProperty('IconCssClass', 'icon-format-italic');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(2);
  item.set_toolTip("Underline");
  item.setProperty('IconCssClass', 'icon-format-underline');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(1);
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(3);
  item.set_toggleGroupId("align");
  item.set_checked(true);
  item.set_toolTip("Align Left");
  item.setProperty('IconCssClass', 'icon-format-alignleft');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(3);
  item.set_toggleGroupId("align");
  item.set_toolTip("Center");
  item.setProperty('IconCssClass', 'icon-format-center');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(3);
  item.set_toggleGroupId("align");
  item.set_toolTip("Align Right");
  item.setProperty('IconCssClass', 'icon-format-alignright');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(3);
  item.set_toggleGroupId("align");
  item.set_toolTip("Justify");
  item.setProperty('IconCssClass', 'icon-format-justify');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(1);
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(4);
  item.set_toggleGroupId("bulleting");
  item.set_toolTip("Numbering");
  item.setProperty('IconCssClass', 'icon-insert-numbering');
  toolBarItems.add(item);

  item = new ComponentArt.Web.UI.ToolBarItem();
  item.set_itemType(4);
  item.set_toggleGroupId("bulleting");
  item.set_toolTip("Bullets");
  item.setProperty('IconCssClass', 'icon-insert-bullets');
  toolBarItems.add(item);
}

