using Xunit;
using System;
using System.Windows.Controls;
using FluentAssertions;

namespace Team6.Tests.Features.Calendar
{
    public class WPFTests
    {
        private Calendar _calendar;

        public WPFTests()
        {
            _calendar = new Calendar();
        }

        [Fact]
        public void WPFCalendar_ShouldInitializeWithTodaysDate()
        {
            // Assert
            _calendar.DisplayDate.Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void WPFCalendar_ShouldAllowDateSelection()
        {
            // Arrange
            var testDate = DateTime.Today.AddDays(1);

            // Act
            _calendar.SelectedDate = testDate;

            // Assert
            _calendar.SelectedDate.Should().Be(testDate);
        }

        [Fact]
        public void WPFCalendar_ShouldRespectSelectionMode()
        {
            // Arrange
            _calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
            var date1 = DateTime.Today.AddDays(1);
            var date2 = DateTime.Today.AddDays(2);

            // Act
            _calendar.SelectedDates.Add(date1);
            _calendar.SelectedDates.Add(date2);

            // Assert
            _calendar.SelectedDates.Should().Contain(date1);
            _calendar.SelectedDates.Should().Contain(date2);
            _calendar.SelectedDates.Count.Should().Be(2);
        }

        [Fact]
        public void WPFCalendar_ShouldPreventSelectionWhenDisabled()
        {
            // Arrange
            _calendar.IsEnabled = false;
            var testDate = DateTime.Today.AddDays(1);

            // Act
            _calendar.SelectedDate = testDate;

            // Assert
            _calendar.SelectedDate.Should().NotBe(testDate);
        }

        [Fact]
        public void WPFCalendar_ShouldRespectDisplayDateRange()
        {
            // Arrange
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddMonths(1);

            // Act
            _calendar.DisplayDateStart = startDate;
            _calendar.DisplayDateEnd = endDate;
            var dateOutsideRange = endDate.AddDays(1);
            _calendar.SelectedDate = dateOutsideRange;

            // Assert
            _calendar.DisplayDateStart.Should().Be(startDate);
            _calendar.DisplayDateEnd.Should().Be(endDate);
            _calendar.SelectedDate.Should().NotBe(dateOutsideRange);
        }

        [Fact]
        public void WPFCalendar_ShouldHandleBlackoutDates()
        {
            // Arrange
            var blackoutDate = DateTime.Today.AddDays(1);
            
            // Act
            _calendar.BlackoutDates.Add(new CalendarDateRange(blackoutDate));
            _calendar.SelectedDate = blackoutDate;

            // Assert
            _calendar.BlackoutDates.Contains(blackoutDate).Should().BeTrue();
            _calendar.SelectedDate.Should().NotBe(blackoutDate);
        }
    }
}