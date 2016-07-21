
    var TreeView1Generated = false;

    function BuildTreeView()
    {
      if(!TreeView1Generated)
      {
        TreeView1.beginUpdate(); 
        TreeView1.set_showLines(true);
        populateTreeView();
        TreeView1.endUpdate();
        treeViewGenerated = true;

        TreeView1.element.style.display = 'block';
      }
    }

    function populateTreeView()
    {
      var newNode;
      var curParentNode; 
      
      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Mailbox');
      newNode.set_imageUrl('root.gif');
      newNode.set_expanded(true);
      TreeView1.get_nodes().add(newNode);
      
      curParentNode = newNode; 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Calendar');
      newNode.set_imageUrl('calendar.gif');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Deleted Items');
      newNode.set_imageUrl('deleted.gif');
      newNode.set_expanded(true);
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Drafts');
      newNode.set_imageUrl('drafts.gif');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Inbox');
      newNode.set_imageUrl('inbox.gif');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Junk E-mail');
      newNode.set_imageUrl('junk.gif');
      curParentNode.get_nodes().add(newNode); 

      curParentNode = TreeView1.Nodes(0).Nodes(1); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Folder 1');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Folder 2');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Folder 3');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Public Folders');
      newNode.set_expanded(true);
      TreeView1.get_nodes().add(newNode); 
      
      curParentNode = newNode; 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Folder 1');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Folder 2');
      curParentNode.get_nodes().add(newNode); 

      newNode = new ComponentArt.Web.UI.TreeViewNode(); 
      newNode.set_text('Folder 3');
      curParentNode.get_nodes().add(newNode);  
    }

    function TreeView1_onLoad(sender, e)
    {
      document.getElementById('TreeView1').style.display = 'none';
    }
