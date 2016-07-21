using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
    /// <summary>
    /// Client-side events of <see cref="Dialog"/> control.
    /// </summary>
    [TypeConverter(typeof(ClientEventsConverter))]
    public class DialogClientEvents : ClientEvents
    {
        /// <summary>
        /// This event fires after showing a dialog.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue(null)]
        public ClientEvent OnShow
        {
            get
            {
                return this.GetValue("OnShow");
            }
            set
            {
                this.SetValue("OnShow", value);
            }
        }

        /// <summary>
        /// This event fires after closing a dialog.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue(null)]
        public ClientEvent OnClose
        {
            get
            {
                return this.GetValue("OnClose");
            }
            set
            {
                this.SetValue("OnClose", value);
            }
        }

        /// <summary>
        /// This event fires when a dialog is focused.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue(null)]
        public ClientEvent OnFocus
        {
            get
            {
                return this.GetValue("OnFocus");
            }
            set
            {
                this.SetValue("OnFocus", value);
            }
        }

        /// <summary>
        /// This event fires when a drag is started.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue(null)]
        public ClientEvent OnDrag
        {
            get
            {
                return this.GetValue("OnDrag");
            }
            set
            {
                this.SetValue("OnDrag", value);
            }
        }

        /// <summary>
        /// This event fires when a drag is completed.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue(null)]
        public ClientEvent OnDrop
        {
            get
            {
                return this.GetValue("OnDrop");
            }
            set
            {
                this.SetValue("OnDrop", value);
            }
        }


        /// <summary>
        /// This event fires when a resize is completed.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue(null)]
        public ClientEvent OnResizeComplete
        {
            get
            {
                return this.GetValue("OnResizeComplete");
            }
            set
            {
                this.SetValue("OnResizeComplete", value);
            }
        }
    }
}
