using Xunit;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Team6.Tests.Features.Calendar
{
    [Trait("Category", "PlatformIndependent")]
    public class PlatformIndependentTests
    {
        private Calendar _calendar;

        public PlatformIndependentTests()
        {
            _calendar = new Calendar();
        }

        [Fact]
        public void Calendar_ShouldInitializeWithTodaysDate()
        {
            // Assert
            _calendar.DisplayDate.Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void Calendar_ShouldHandleBlackoutDates()
        {
            // Arrange
            var blackoutDate = DateTime.Today.AddDays(1);
            
            // Act
            _calendar.BlackoutDates.Add(new CalendarDateRange(blackoutDate));

            // Assert
            _calendar.BlackoutDates.Contains(blackoutDate).Should().BeTrue();
        }

        [Fact]
        public void Calendar_ShouldHandleSelectionMode()
        {
            // Arrange & Act
            _calendar.SelectionMode = CalendarSelectionMode.SingleDate;

            // Assert
            _calendar.SelectionMode.Should().Be(CalendarSelectionMode.SingleDate);
        }

        [Fact]
        public void Calendar_ShouldHandleDisplayDateRange()
        {
            // Arrange
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddMonths(1);

            // Act
            _calendar.DisplayDateStart = startDate;
            _calendar.DisplayDateEnd = endDate;

            // Assert
            _calendar.DisplayDateStart.Should().Be(startDate);
            _calendar.DisplayDateEnd.Should().Be(endDate);
        }

        [Fact]
        public void Calendar_ShouldHandleSelectedDates()
        {
            // Arrange
            var date = DateTime.Today;

            // Act
            _calendar.SelectedDate = date;

            // Assert
            _calendar.SelectedDate.Should().Be(date);
        }

        [Fact]
        public void Calendar_ShouldHandleMultipleSelectedDates()
        {
            // Arrange
            _calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
            var date1 = DateTime.Today;
            var date2 = DateTime.Today.AddDays(1);

            // Act
            _calendar.SelectedDates.Add(date1);
            _calendar.SelectedDates.Add(date2);

            // Assert
            _calendar.SelectedDates.Should().Contain(date1);
            _calendar.SelectedDates.Should().Contain(date2);
            _calendar.SelectedDates.Count.Should().Be(2);
        }

        [Fact]
        public void Calendar_ShouldPreventSelectionOfBlackoutDates()
        {
            // Arrange
            var blackoutDate = DateTime.Today.AddDays(1);
            _calendar.BlackoutDates.Add(new CalendarDateRange(blackoutDate));

            // Act
            _calendar.SelectedDate = blackoutDate;

            // Assert
            _calendar.SelectedDate.Should().NotBe(blackoutDate);
        }

        [Fact]
        public void Calendar_ShouldHandleDisplayModeChanges()
        {
            // Arrange & Act
            _calendar.DisplayMode = CalendarMode.Month;

            // Assert
            _calendar.DisplayMode.Should().Be(CalendarMode.Month);
        }
    }
} 