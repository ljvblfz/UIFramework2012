
function TabStrip1_onLoad(sender, e)
{
  document.getElementById('TabStrip1').style.display = 'none';
}
function BuildClientTabStrip()
{
  TabStrip1.beginUpdate();

  var topTabs = TabStrip1.get_tabs();
  var newTab;

  newTab = new ComponentArt.Web.UI.TabStripTab();
  newTab.set_text('General');
  newTab.set_id('General');
  topTabs.add(newTab);

  newTab = new ComponentArt.Web.UI.TabStripTab();
  newTab.set_text('Security');
  newTab.set_id('Security');
  topTabs.add(newTab);

  newTab = new ComponentArt.Web.UI.TabStripTab();
  newTab.set_text('Privacy');
  newTab.set_id('Privacy');
  topTabs.add(newTab);

  newTab = new ComponentArt.Web.UI.TabStripTab();
  newTab.set_text('Content');
  newTab.set_id('Content');
  topTabs.add(newTab);
  
  TabStrip1.selectTabById('General');

  TabStrip1.endUpdate();

  document.getElementById('TabStrip1').style.display = 'block';
}            
      