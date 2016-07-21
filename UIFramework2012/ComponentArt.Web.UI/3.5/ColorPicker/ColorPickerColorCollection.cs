using System;
using System.Collections;
using System.Text;

namespace ComponentArt.Web.UI
{
    public class ColorPickerColorCollection : System.Collections.CollectionBase
    {

        public int Add(ColorPickerColor obj) { return List.Add(obj); }
        public void Insert(int index, ColorPickerColor obj) { List.Insert(index, obj); }
        public void Remove(ColorPickerColor obj) { List.Remove(obj); }
        public bool Contains(ColorPickerColor obj) { return List.Contains(obj); }
        public void CopyTo(ColorPickerColor[] array, int index) { List.CopyTo(array, index); }

        public ColorPickerColor this[int obj]
        {
            get
            {
                if (obj >= 0)
                    return (ColorPickerColor)List[obj];
                else
                    return null;
            }
            set
            {
                if (obj >= 0)
                    List[obj] = value;
                else
                    this.Add(value);
            }
        }

        public int IndexOf(object obj)
        {
            if (obj is int)
                return (int)obj;

            return -1;
        }
    }
}
