// --------------------------------------------------------------------------
// ComponentArt NavBar for ASP.NET AJAX - Client-side Structure Creation  
// --------------------------------------------------------------------------

function NavBar1_onLoad(sender, eventArgs)
{
  // enable the 'Click to Create' button after NavBar1 has been initialized
  document.getElementById('btnNavBarCreate').disabled = false;

  sender.element.style.visibility = 'hidden';
}

function buildNavBar()
{
  NavBar1.beginUpdate();

  // Build the Mail group 
  var newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Mail');
  newItem.setProperty('IconCssClass', 'icon-outlook-mail');
  newItem.set_expanded(true);     
  NavBar1.get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Mailbox - Scott Guest');
  newItem.setProperty('IconCssClass', 'icon-outlook-home');
  NavBar1.get_items().getItem(0).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Inbox');
  newItem.setProperty('IconCssClass', 'icon-outlook-inbox');
  NavBar1.get_items().getItem(0).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Drafts');
  newItem.setProperty('IconCssClass', 'icon-outlook-drafts');
  NavBar1.get_items().getItem(0).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Deleted Items');
  newItem.setProperty('IconCssClass', 'icon-outlook-trash');
  NavBar1.get_items().getItem(0).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Sent Items');
  newItem.setProperty('IconCssClass', 'icon-outlook-sent');
  NavBar1.get_items().getItem(0).get_items().add(newItem);

  // Build the Notes group 
  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Notes');
  newItem.setProperty('IconCssClass', 'icon-outlook-notes');
  NavBar1.get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Last Seven Days');
  newItem.setProperty('IconCssClass', 'icon-outlook-notes');
  NavBar1.get_items().getItem(1).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('By Category');
  newItem.setProperty('IconCssClass', 'icon-outlook-notes');
  NavBar1.get_items().getItem(1).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('By Color');
  newItem.setProperty('IconCssClass', 'icon-outlook-notes');
  NavBar1.get_items().getItem(1).get_items().add(newItem);

  // Build the Contacts group 
  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Contacts');
  newItem.setProperty('IconCssClass', 'icon-outlook-contacts'); 
  NavBar1.get_items().add(newItem); 

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Address Cards');
  newItem.setProperty('IconCssClass', 'icon-outlook-contacts');
  NavBar1.get_items().getItem(2).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('Detailed Address List');
  newItem.setProperty('IconCssClass', 'icon-outlook-contacts');
  NavBar1.get_items().getItem(2).get_items().add(newItem);

  newItem = new ComponentArt.Web.UI.NavBarItem();
  newItem.set_text('By Category');
  newItem.setProperty('IconCssClass', 'icon-outlook-contacts');
  NavBar1.get_items().getItem(2).get_items().add(newItem);

  NavBar1.endUpdate();

  NavBar1.element.style.visibility = 'visible';

}
