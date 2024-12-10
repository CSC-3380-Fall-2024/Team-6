using System;
using System.Windows.Input;
using Xamarin.Forms;
using XCalendar.Core.Extensions;
using XCalendar.Core.Models;

namespace CalendarApp
{
    public class MainPageViewModel
    {
        public CalendarApp<CalendarDay> calendar = new Calendar<CalendarDay>();
        public ICommand NavigateCalendarCommand;

        public MainPageViewModel()
        {
            NavigateCalendarCommand = new Command<int>(NavigateCalendar);
        }

        public void NavigateCalendar(int amount)
        {
            DateTime targetDateTime = calendar.NavigatedDate.AddMonths(amount);
            calendar.Navigate(targetDateTime - calendar.NavigatedDate);
        }
    }
}