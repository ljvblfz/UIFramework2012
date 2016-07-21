using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
    /// <summary>
    /// Client-side events of <see cref="Slider"/> control.
    /// </summary>
    [TypeConverter(typeof(ClientEventsConverter))]
    public class SliderClientEvents : ClientEvents
    {

        /// <summary>
        /// This event fires when the Slider is loaded.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        public ClientEvent Load
        {
            get
            {
                return this.GetValue("Load");
            }
            set
            {
                this.SetValue("Load", value);
            }
        }

        /// <summary>
        /// This event fires when the Slider is initialized.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        public ClientEvent Init
        {
            get
            {
                return this.GetValue("Init");
            }
            set
            {
                this.SetValue("Init", value);
            }
        }

        /// <summary>
        /// This event fires before the Slider value changes.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        public ClientEvent BeforeValueChanged
        {
            get
            {
                return this.GetValue("BeforeValueChanged");
            }
            set
            {
                this.SetValue("BeforeValueChanged", value);
            }
        }

        /// <summary>
        /// This event fires when the Slider value changes.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        public ClientEvent ValueChanged
        {
            get
            {
                return this.GetValue("ValueChanged");
            }
            set
            {
                this.SetValue("ValueChanged", value);
            }
        }
    }
}
