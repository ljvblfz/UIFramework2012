using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
    /// <summary>
    /// Client-side events of <see cref="ColorPicker"/> control.
    /// </summary>
    [TypeConverter(typeof(ClientEventsConverter))]
    public class ColorPickerClientEvents : ClientEvents
    {

        /// <summary>
        /// This event fires when the ColorPicker is loaded.
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
        /// This event fires before the SelectedColor property is changed.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        public ClientEvent BeforeColorChanged
        {
            get
            {
                return this.GetValue("BeforeColorChanged");
            }
            set
            {
                this.SetValue("BeforeColorChanged", value);
            }
        }

        /// <summary>
        /// This event fires when the a color is selected in a ColorPicker.
        /// </summary>
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        public ClientEvent ColorChanged
        {
            get
            {
                return this.GetValue("ColorChanged");
            }
            set
            {
                this.SetValue("ColorChanged", value);
            }
        }

        /// <summary>
        /// This event fires when the ColorPicker is initialized.
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
    }
}
