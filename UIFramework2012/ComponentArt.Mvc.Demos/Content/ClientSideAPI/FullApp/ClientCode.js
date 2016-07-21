// --------------------------------------------------------------------------
// ComponentArt TreeView for ASP.NET AJAX - Client-side Designer 
// --------------------------------------------------------------------------

  // Toolbar commands ---------------------------------------------------------
  function Command_AddTopNode()
  {
    TreeView1.beginUpdate(); 
    var newNode = new ComponentArt.Web.UI.TreeViewNode();
    newNode.set_text('Root Node ' + TreeView1.get_nodes().get_length());
    newNode.set_imageUrl("folders.gif");
    newNode.set_expanded(true);
    TreeView1.get_nodes().add(newNode);
    newNode.select();
    TreeView1.endUpdate(); 
  }

  function Command_AddChildNode()
  {
    TreeView1.beginUpdate(); 
    var newNode = new ComponentArt.Web.UI.TreeViewNode(); 
    newNode.set_text('Child Node ' + TreeView1.get_selectedNode().get_nodes().get_length());
    newNode.set_imageUrl("folder.gif");
    newNode.set_expanded(true);
    TreeView1.get_selectedNode().get_nodes().add(newNode); 
    TreeView1.endUpdate(); 
  }

  function Command_EditNode()
  {
    TreeView1.get_selectedNode().edit(); 
  }

  function Command_RemoveNode()
  {
    TreeView1.get_selectedNode().remove();
    Grid1.get_table().clearData();
    UpdateToolbar(); 
  }

  function Command_TogglePropertyGrid()
  {
    document.getElementById('SnapGridContainer').style.display = document.getElementById('SnapGridContainer').style.display == 'none' ? 'block' : 'none';
    
    if (TreeView1.get_selectedNode()) PopulatePropertyGrid(TreeView1.get_selectedNode()); 
  }

  // Client-side event handlers -----------------------------------------------
  function TreeView1_onNodeSelect(sender, eventArgs)
  {
    UpdateToolbar();
    PopulatePropertyGrid(eventArgs.get_node());
  }

  function TreeView1_onNodeRename(sender, eventArgs)
  {
    PopulatePropertyGrid(eventArgs.get_node());
  }

  function TreeView1_onNodeExpand(sender, eventArgs)
  {
    PopulatePropertyGrid(eventArgs.get_node());
  }

  function TreeView1_onNodeCollapse(sender, eventArgs)
  {
    PopulatePropertyGrid(eventArgs.get_node());
  }

  function Menu1_onItemSelect(sender, eventArgs)
  {
    var clickedItem = eventArgs.get_item();
    var toolBarItem = ToolBar1.get_items().getItemById('SetNodeIcon');
    var clickedItemValue = clickedItem.get_value();
    var toolBarItemValue = toolBarItem.get_value();
    if (clickedItemValue && clickedItemValue != toolBarItemValue)
    {
      var icon = clickedItem.getProperty('IconUrl');
 
      ToolBar1.beginUpdate();
      toolBarItem.set_value(clickedItemValue);
      toolBarItem.set_imageUrl(icon);
      toolBarItem.set_disabledImageUrl('disabled_'+icon);
      ToolBar1.endUpdate();

      if(TreeView1.get_selectedNode())
      {
        TreeView1.get_selectedNode().set_imageUrl(icon);

        PopulatePropertyGrid(TreeView1.get_selectedNode());
      }
    }
  }

  function TreeView1_onLoad(sender, eventArgs)
  {
    UpdateToolbar();
  }

  // Helper functions ---------------------------------------------------------

  function UpdateToolbar()
  {
    ToolBar1.beginUpdate(); 
    // Set enabled state for toolbar buttons 
    if (window.TreeView1 && TreeView1.get_selectedNode()) 
    {
      ToolBar1.get_items().getItemById('AddChildNode').set_enabled(true);
      ToolBar1.get_items().getItemById('EditNode').set_enabled(true);
      ToolBar1.get_items().getItemById('RemoveNode').set_enabled(true); 

      var nodeImage = TreeView1.get_selectedNode().get_imageUrl();
      ToolBar1.get_items().getItemById('SetNodeIcon').set_enabled(true); 
      ToolBar1.get_items().getItemById('SetNodeIcon').set_imageUrl(nodeImage);
      ToolBar1.get_items().getItemById('SetNodeIcon').set_disabledImageUrl('disabled_'+nodeImage);
    }
    else
    {
      ToolBar1.get_items().getItemById('AddChildNode').set_enabled(false); 
      ToolBar1.get_items().getItemById('EditNode').set_enabled(false); 
      ToolBar1.get_items().getItemById('RemoveNode').set_enabled(false); 

      ToolBar1.get_items().getItemById('SetNodeIcon').set_enabled(false); 
    }
    
    // Set the current node icon for the toolbar button 
    if (window.Treeview1 && TreeView1.get_selectedNode())
    {
      ToolBar1.get_items().getItemById('SetNodeIcon').setProperty('ImageUrl',
        TreeView1.get_selectedNode().get_imageUrl());
    }

    ToolBar1.endUpdate(); 
  }

  function PopulatePropertyGrid(object)
  {  
    if (!object || document.getElementById('SnapGridContainer').style.display == 'none') return null; 
    
    Grid1.beginUpdate(); 
    Grid1.get_table().clearData(); 

    var properties = object.getPropertyNames();
    for (var i = 0; i < properties.length; i++) 
    {
      var propertyName = properties[i];
      var value = object.getProperty(propertyName);

      if (value || value == 0 || value == '')
      {
        Grid1.get_table().addEmptyRow();
        var newRow = Grid1.get_table().getRow(Grid1.get_recordCount() - 1); 
        newRow.setValue(0, propertyName, true, true); 
        newRow.setValue(1, value, true, true); 
      }
    } 
    Grid1.endUpdate(); 
  }
