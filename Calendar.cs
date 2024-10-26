using System.*;

namespace System.Windows.Controls
{
    // Represents a control that enables a user to select a date by using a visual calendar display.
    [TemplatePart(Name = Calendar.ElementRoot, Type = typeof(Panel1))]
    [TemplatePart(Name = Calendar.ElementMonth, Type = typeof(CalendarItem))]

    public class Calendar : Control
    {
        private const string ElementRoot = "PART_Root";
        private const string ElementMonth = "PART_CalendarItem";

        private const int COLS = 7;
        private const int ROWS = 4;
        private const int YEAR_ROWS = 2;
        private const int YEAR_COLS = 4;
        private const int YEARS_PER_DECADE = 10;

        private DateTime? _hoverStart;
        private DateTime? _hoverEnd;
        private bool _isShiftPressed;
        private DateTime? _currentDate;
        private CalendarItem _monthControl;
        private CalendarBlackoutDatesCollection _blackoutDates;
        private SelectedDatesCollection _selectedDates;

        public static readonly RoutedEvent SelectedDatesChangedEvent = EventManager.RegisterRoutedEvent("SelectedDatesChanged", RoutingStrategy.Direct, typeof(EventHandler), typeof(Calendar));

        // Occurs when a date is selected
        public event EventHandler SelectedDatesChanged
        {
            add
            {
                AddHandler(SelectedDatesChangedEvent, value);
            }

            remove
            {
                RemoveHandler(SelectedDatesChangedEvent, value);
            }
        }

        // Occurs when the DisplayDate property is changed.
        public event EventHandler DisplayDateChanged;
        // Occurs when the DisplayMode property is changed.
        public event EventHandler DisplayModeChanged;
        // Occurs when the SelectionMode property is changed.
        public event EventHandler SelectionModeChanged;

        // Static constructor
        static Calendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            LanguageProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnLanguageChanged)));
            EventManager.RegisterClassHandler(typeof(Calendar), UIElement.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
        }
    }
}