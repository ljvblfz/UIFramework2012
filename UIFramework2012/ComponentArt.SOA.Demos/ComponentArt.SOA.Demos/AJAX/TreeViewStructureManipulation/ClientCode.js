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
    UpdateToolbar(); 
  }

  function Command_TogglePropertyGrid()
  {
    SnapGridContainer.toggleMinimize(); 
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

    TreeView1.webServiceEdit(eventArgs.get_node(), eventArgs.get_oldText());    
  }

  function TreeView1_onNodeMove(sender, eventArgs) 
  {
    PopulatePropertyGrid(eventArgs.get_node());    
  }

  function TreeView1_onBeforeNodeMove(sender, eventArgs) {

    var node = eventArgs.get_node();
    var oldParent = node.get_parentNode();
    var oldIndex = node.get_index();

    var soaNode = { 'Text': node.get_text(), 'Value': node.get_value(), 'ParentNode': eventArgs.get_newParentNode(), 'Index': eventArgs.get_index() }

    TreeView1.webServiceMove(soaNode, oldParent, oldIndex);
  }
  
  function TreeView1_onNodeExpand(sender, eventArgs)
  {
    PopulatePropertyGrid(eventArgs.get_node());
  }

  function TreeView1_onNodeCollapse(sender, eventArgs)
  {
    PopulatePropertyGrid(eventArgs.get_node());
  }

  function TreeView1_onWebServiceComplete(sender, eventArgs) 
  {
    document.getElementById('soafeedback').innerHTML += "SOA.UI " + eventArgs.get_action() + " request completed.<br/>";
  }

  function Menu1_onItemSelect(sender, eventArgs)
  {
    var clickedItem = eventArgs.get_item();
    var clickedItemValue = clickedItem.get_value();
    var toolBarItemValue = toolBarItem.get_value();
    if (clickedItemValue && clickedItemValue != toolBarItemValue)
    {
      var icon = clickedItem.getProperty('Look-LeftIconUrl');
 
      if(TreeView1.get_selectedNode())
      {
        TreeView1.get_selectedNode().set_imageUrl(icon); 
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

    }
    else
    {
      ToolBar1.get_items().getItemById('AddChildNode').set_enabled(false); 
      ToolBar1.get_items().getItemById('EditNode').set_enabled(false); 
      ToolBar1.get_items().getItemById('RemoveNode').set_enabled(false); 
    }
    
    ToolBar1.endUpdate(); 
  }

  function PopulatePropertyGrid(object)
  {  
  }
