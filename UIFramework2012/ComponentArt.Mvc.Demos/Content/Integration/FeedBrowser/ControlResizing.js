window.Grid1_Load = function(sender, eventArgs)
{
  window.Grid1_Loaded = true;
  //window.ResizeTheGrid();
}

window.TabStrip1_Load = function(sender, eventArgs)
{
  sender.beginUpdate();
  for (var i = 0; i < window.FeaturedFeeds.length; i++)
  {
    var featuredFeed = window.FeaturedFeeds[i];
    var featuredFeedTab = new ComponentArt.Web.UI.TabStripTab();
    featuredFeedTab.setProperty('FeedIndex', i);
    featuredFeedTab.set_id(featuredFeed.Name);
    featuredFeedTab.setProperty('FeedImage', featuredFeed.Image);
    featuredFeedTab.setProperty('FeedName', featuredFeed.Name);
    featuredFeedTab.setProperty('FeedDescription', featuredFeed.Description);
    featuredFeedTab.setProperty('ClientTemplateId', 'DefaultTabClientTemplate');
    sender.get_tabs().add(featuredFeedTab);
  }
  sender.endUpdate();
  window.TabStrip1_Loaded = true;
}

window.Grid1_Loaded = false;
window.TabStrip1_Loaded = false;
window.MinimumResizeInterval = 100;
window.resizeRequested = false;
window.resizeAllowed = true;
window.priorWidth = null;
window.priorHeight = null;


window.findMultiPagePages = function()
{
  window.multiPagePages = [];
  var alldivs = document.getElementsByTagName('div');
  for (var i = 0; i < alldivs.length; i++)
  {
    if (alldivs[i].className == 'MultiPage1_page')
    {
      window.multiPagePages.push(alldivs[i]);
    }
  }
}

window.resizeMultiPage = function()
{
  if (!window.multiPagePages)
  {
    window.findMultiPagePages();
  }
  var newMultiPageHeight = document.body.offsetHeight - 322 + 'px';
  for (var i = 0; i < window.multiPagePages.length; i++)
  {
    window.multiPagePages[i].style.height = newMultiPageHeight;
  }
}

window.AllowResizeAgain = function()
{
  window.resizeAllowed = true;
  if (window.resizeRequested)
  {
    window.ResizeTheControls();
  }
}

window.ResizeTheGrid = function()
{
  if (window.Grid1_Loaded)
  {
    window.Grid1.beginUpdate();
    
    // Constants that may need to be tweaked:
    var widthOfOtherStuffOnPage = 451;
    var heightOfOtherStuffOnPage = 254;
    var rowHeight = 25;
    var heightOfGridHeaderFooterEtc = 61;
    
    var gridWidth = document.body.offsetWidth - widthOfOtherStuffOnPage;
    var gridHeight = document.body.offsetHeight - heightOfOtherStuffOnPage;
    var pageSize = Math.floor((gridHeight - heightOfGridHeaderFooterEtc) / rowHeight);
    window.Grid1.element.parentNode.style.width = window.Grid1.element.style.width = gridWidth + 'px';
    window.Grid1.element.parentNode.style.height = window.Grid1.element.style.height = gridHeight + 'px';
    window.Grid1.set_pageSize(pageSize);
    window.Grid1.endUpdate();
  }
}

window.ResizeTheControls = function()
{
  if (window.resizeAllowed)
  {
    window.resizeAllowed = false;
    window.resizeRequested = false;
    window.resizeMultiPage();
    window.ResizeTheGrid();
    if (window.TabStrip1_Loaded)
    {
      window.TabStrip1.render();
    }
    window.setTimeout(window.AllowResizeAgain, window.MinimumResizeInterval);
  }
  else
  {
    window.resizeRequested = true;
  }
}

window.Window_Resize = function()
{
  // This is a check to do away with IE's erroneous double firing of the onresize event
  var newWidth = document.body.offsetWidth;
  var newHeight = document.body.offsetHeight;
  if (newWidth == window.priorWidth && newHeight == window.priorHeight)
  {
    return; // Ignore this resize!
  }
  else
  {
    window.priorWidth = newWidth;
    window.priorHeight = newHeight;
  }
  window.ResizeTheControls();
}

// window.onload = Window_Resize;
// window.onresize = Window_Resize;
