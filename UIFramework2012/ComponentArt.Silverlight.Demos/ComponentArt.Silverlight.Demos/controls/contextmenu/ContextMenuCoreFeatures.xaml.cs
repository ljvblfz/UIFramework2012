using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Silverlight.UI.Navigation;
using ComponentArt.Silverlight.UI.Utils;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Browser;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace ComponentArt.Silverlight.Demos {

    public partial class ContextMenuCoreFeatures : UserControl, IDisposable
    {
        double imgw=0;
        double imgh=0;

        public ContextMenuCoreFeatures()
        {
			InitializeComponent();
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            menu1.Dispose();
            menu2.Dispose();
            
        }

        #endregion

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
		}

        private void menu1_MenuClick(object sender, MenuCommandEventArgs mce)
        {
            DateTime date1;
            if (mce.ItemSource.Id == "goto")
            {
                date1 = DateTime.Parse(mce.ItemSource.Text, CultureInfo.InvariantCulture);
                myCalendar.SelectedDate = date1;
                myCalendar.DisplayDate = date1;
            }
            else if (mce.ItemSource.Id == "select")
            {
                String sel = mce.ItemSource.Text;
                if (sel == "Select Today")
                {
                    date1 = DateTime.Today;
                    myCalendar.DisplayDate = date1;
                    myCalendar.SelectedDate = date1;
                }
                else if (sel == "Select Current Week")
                {
                    date1 = DateTime.Today;
                    myCalendar.DisplayDate = date1;
                    SelectThisWeek();
                }
                else if (sel == "Select Current Month")
                {
                    date1 = DateTime.Today;
                    myCalendar.DisplayDate = date1;
                    SelectThisMonth();
                }
                else if (sel == "Select Week Days")
                {
                    date1 = DateTime.Today;
                    myCalendar.DisplayDate = date1;
                    SelectWeekdays();
                }
                else if (sel == "Select Weekend Days")
                {
                    date1 = DateTime.Today;
                    myCalendar.DisplayDate = date1;
                    SelectWeekends();
                }
            }
        }
        // Selects the current week 
        private void SelectThisWeek()
        {
          DateTime curDate = DateTime.Today; 
          TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

          myCalendar.SelectionMode = CalendarSelectionMode.MultipleRange;
          for (int i = 1; curDate.DayOfWeek != DayOfWeek.Sunday; i++)
          {
              curDate = curDate.Subtract(oneDay);
          }

          myCalendar.SelectedDates.Clear(); 
          for (int i = 1; i <= 7; i++) 
          {
            myCalendar.SelectedDates.Add(curDate); 
            curDate = curDate.Add(oneDay); 
          }
        }

        // Selects the current month 
        private void SelectThisMonth()
        {
          DateTime curDate = DateTime.Today; 
          TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

          for (int i = 1; i < DateTime.Today.Day; i++)
          {
              curDate = curDate.Subtract(oneDay);
          }
          myCalendar.SelectionMode = CalendarSelectionMode.MultipleRange;
          myCalendar.SelectedDates.Clear(); 
          for (int i = 1; i <= DateTime.DaysInMonth(curDate.Year, curDate.Month); i++) 
          {
            myCalendar.SelectedDates.Add(curDate); 
            curDate = curDate.Add(oneDay); 
          }
        }

        // Selects all weekdays in the current month 
        private void SelectWeekdays()
        {
          DateTime curDate = DateTime.Today; 
          TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

          for (int i = 1; i < DateTime.Today.Day; i++)
          {
              curDate = curDate.Subtract(oneDay);
          }
          myCalendar.SelectionMode = CalendarSelectionMode.MultipleRange;
          myCalendar.SelectedDates.Clear(); 
          for (int i = 1; i <= DateTime.DaysInMonth(curDate.Year, curDate.Month); i++) 
          {
              if (curDate.DayOfWeek != DayOfWeek.Saturday && curDate.DayOfWeek != DayOfWeek.Sunday)
              {
                  myCalendar.SelectedDates.Add(curDate);
              }
            curDate = curDate.Add(oneDay); 
          }
        }

        // Selects all the weekend days in the current month 
        private void SelectWeekends()
        {
          DateTime curDate = DateTime.Today; 
          TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

          for (int i = 1; i < DateTime.Today.Day; i++)
          {
              curDate = curDate.Subtract(oneDay);
          }
          myCalendar.SelectionMode = CalendarSelectionMode.MultipleRange;
          myCalendar.SelectedDates.Clear(); 
          for (int i = 1; i <= DateTime.DaysInMonth(curDate.Year, curDate.Month); i++) 
          {
              if (curDate.DayOfWeek == DayOfWeek.Saturday || curDate.DayOfWeek == DayOfWeek.Sunday)
              {
                  myCalendar.SelectedDates.Add(curDate);
              }
              curDate = curDate.Add(oneDay); 
          }
        }

        private void menu2_MenuClick(object sender, MenuCommandEventArgs mce)
        {
            string file = mce.ItemSource.Text;
            if (file == null) { return; }
            if (file == "Reset Image")
            {
                file = "asdf dolphin.jpg";
                myImage.Width = imgw;
                myImage.Height = imgh;
            }

            BitmapImage bi = new BitmapImage(new Uri("assets/"+file.Split(new char[]{' '})[1],UriKind.RelativeOrAbsolute));
            myImage.Source = (ImageSource)bi;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (myImage.ActualHeight > 0)
            {
                if (imgw == 0)
                {
                    imgw = myImage.ActualWidth;
                    imgh = myImage.ActualHeight;
                }
                else
                {
                    myImage.Width = imgw * e.NewValue;
                    myImage.Height = imgh * e.NewValue;
                }
            }
        }
	}
}
