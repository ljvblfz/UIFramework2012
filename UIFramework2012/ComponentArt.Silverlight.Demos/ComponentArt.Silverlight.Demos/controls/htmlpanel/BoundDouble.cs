using System;
using System.ComponentModel;

namespace ComponentArt.Silverlight.Demos
{
    public class BoundDouble : INotifyPropertyChanged
    {
        private double value;

        public double Value
        {
            get { return this.value; }
            set
            {
                if (this.value != value)
                {
                    this.value = (Round ? Math.Round(value) : value);
                    NotifyPropertyChanged("Value");
                }
            }
        }

        public bool Round { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
