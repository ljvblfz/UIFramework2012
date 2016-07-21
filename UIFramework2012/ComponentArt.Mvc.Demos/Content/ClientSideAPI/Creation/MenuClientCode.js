// --------------------------------------------------------------------------
// ComponentArt Menu for ASP.NET - Client-side Structure Creation  
// --------------------------------------------------------------------------

function Menu1_onLoad(sender, eventArgs)
{
  // enable the 'Click to Create' button after Menu1 has been initialized
  document.getElementById('btnMenuCreate').disabled = false;

  // hide Menu area initially
  document.getElementById('Menu1').style.display = 'none';
}

function buildMenu()
{
  Menu1.beginUpdate(); 
  populateMenu();
  Menu1.endUpdate();
  document.getElementById('Menu1').style.display = 'block';
}

function populateMenu()
{
  var newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('File');
   newItem.set_id('file');
  Menu1.get_items().add(newItem);
  
  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Edit');
   newItem.set_id('edit');
  Menu1.get_items().add(newItem);
  
  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('New...');
  newItem.setProperty('IconCssClass', 'icon-file-new'); 
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Open...');
  newItem.setProperty('IconCssClass', 'icon-file-open'); 
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_lookId('BreakItem');
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Save');
  newItem.setProperty('IconCssClass', 'icon-file-save'); 
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Save As...');
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Save as Web Page');
  newItem.setProperty('IconCssClass', 'icon-file-saveas'); 
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('File Search...');
  newItem.setProperty('IconCssClass', 'icon-view-maginify'); 
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_lookId('BreakItem');
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Exit');
  Menu1.findItemById('file').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Cut');
  newItem.setProperty('IconCssClass', 'icon-edit-cut'); 
  Menu1.findItemById('edit').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Copy');
  newItem.setProperty('IconCssClass', 'icon-edit-copy'); 
  Menu1.findItemById('edit').get_items().add(newItem);
  
  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Office Clipboard...');
  newItem.setProperty('IconCssClass', 'icon-outlook-tasks'); 
  Menu1.findItemById('edit').get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.MenuItem(); 
  newItem.set_text('Paste');
  newItem.setProperty('IconCssClass', 'icon-edit-paste'); 
  Menu1.findItemById('edit').get_items().add(newItem);
}

