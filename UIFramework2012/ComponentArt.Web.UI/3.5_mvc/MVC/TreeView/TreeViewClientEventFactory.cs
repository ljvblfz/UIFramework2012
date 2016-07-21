using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
    public class TreeViewClientEventFactory
    {
        private ComponentArt.Web.UI.TreeViewClientEvents treeViewClientEvents;
      /// <summary>
        /// Factory to define client event handlers for the TreeView.
      /// </summary>
      /// <param name="treeViewClientEvents"></param>
        public TreeViewClientEventFactory(ComponentArt.Web.UI.TreeViewClientEvents treeViewClientEvents)
        {
            this.treeViewClientEvents = treeViewClientEvents;
        }
      /// <summary>
        /// Fires when a callback completes.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder CallbackComplete(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.CallbackComplete = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is right-clicked.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder ContextMenu(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.ContextMenu = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when the TreeView is done loading.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder Load(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.Load = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires before a node is checked or unchecked.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeBeforeCheckChange(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeBeforeCheckChange = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires before a node is collapsed.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeBeforeCollapse(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeBeforeCollapse = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires before a node is expanded.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeBeforeExpand(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeBeforeExpand = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires before a node is moved to a different place in the tree.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeBeforeMove(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeBeforeMove = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
      /// Fires before a node is renamed.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeBeforeRename(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeBeforeRename = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires before a node is selected.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeBeforeSelect(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeBeforeSelect = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires before a node is checked or unchecked.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeCheckChange(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeCheckChange = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is collapsed.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeCollapse(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeCollapse = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is copied.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeCopy(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeCopy = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is expanded.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeExpand(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeExpand = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is dropped onto an external element.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeExternalDrop(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeExternalDrop = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is highlighted using keyboard navigation.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeKeyboardNavigate(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeKeyboardNavigate = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is double-clicked.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeMouseDoubleClick(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeMouseDoubleClick = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is moved to a different place.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeMove(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeMove = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when the mouse moves away from a node.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeMouseOut(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeMouseOut = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when the mouse moves over a node.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeMouseOver(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeMouseOver = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is renamed.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeRename(string eventHandler)
        {
          ClientEvent clientEvent = new ClientEvent();
          clientEvent.EventHandler = eventHandler;

          treeViewClientEvents.NodeRename = clientEvent;

          return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Fires when a node is selected.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder NodeSelect(string eventHandler)
        {
            ClientEvent clientEvent = new ClientEvent();
            clientEvent.EventHandler = eventHandler;

            treeViewClientEvents.NodeSelect = clientEvent;

            return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Called when a web service request completes.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder WebServiceComplete(string eventHandler)
        {
            ClientEvent clientEvent = new ClientEvent();
            clientEvent.EventHandler = eventHandler;

            treeViewClientEvents.WebServiceComplete = clientEvent;

            return new ClientEventBuilder(clientEvent);
        }
      /// <summary>
        /// Called when a web service request results in an error.
      /// </summary>
      /// <param name="eventHandler"></param>
      /// <returns></returns>
        public virtual ClientEventBuilder WebServiceError(string eventHandler)
        {
            ClientEvent clientEvent = new ClientEvent();
            clientEvent.EventHandler = eventHandler;

            treeViewClientEvents.WebServiceError = clientEvent;

            return new ClientEventBuilder(clientEvent);
        }
    }
}