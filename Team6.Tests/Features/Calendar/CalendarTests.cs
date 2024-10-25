using Xunit; // required testing framework
using System; // for datetime and other system functions
using System.Collections.Generic; //
using FluentAssertions; // for better/more descript error messages? Not entirely sure but came across it when researching a bit


// Define namespace for calendar testing
namespace Team6.Tests.Features.Calendar 
{
    // create a model to represent a calendar event --> needs to be implemented to actually test 
    public class CalendarEvent 
    {
        public string eventTitle {get; set;} = string.Empty; // store event title
        public DateTime eventDate {get; set;} // store event date and time
    }

    // create a class to manage potential calendar functions 
    public class Calendar
    {
        /*
        function to add events to the calendar
        args:
            title (str): event title
            date (datetime): event date
        returns:
            None
        */
        public void AddEvent (string title, DateTime date) 
        {

        }

        /*
        function to get events from the calendar
        args:
            date (datetime): event date
        returns:
            CalendarEvent object
        */
        public CalendarEvent GetEvent (DateTime date) 
        {
            return null; // allowing null just for now while setting up test structure 
        }

         /*
        function to get all events on a specific date
        args:
            date (datetime): the date to search for
        returns:
            List of CalendarEvent objects
        */
        public List<CalendarEvent> GetEventsForDate(DateTime date)
        {
            return null;
        }

         /*
        function to update an event on the calendar
        args:
            newTitle (str): the updated event title
            date (datetime): event date
        returns:
            None
        */
        public void UpdateEventTitle (string newTitle, DateTime date) 
        {

        }

        /*
        function to update an event date on the calendar
        args:
            oldDate (datetime): the current date of the event
            newDate (datetime): the new date of the event
        */
        public void UpdateEventDate(DateTime oldDate, DateTime newDate)
        {

        }

        /*
        function to delete event from the calendar
        args:
            date (datetime): event date
        returns:
            None
        */
        public void DeleteEvent (DateTime date)
        {

        }

    }

    // create tests for the above basic functions using Arrange-Act-Assert (aka Given-When-Then)
        // arrange: set up test conditions and the required data
        // act: perform the test action
        // assert: verify the result of the act
    public class CalendarTests
    {

         /*
        tests creating a single event on a given date
        ensures:
            can successfully create an event
            can successfully get an event
            the date and title input match the result we retrieve
        */
        [Fact] // mark the function as a unit test
        public void TestCreatingAndRetrievingEvent()
        {
            // arrange
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var inputEventTitle = "Halloween"; // example event title
            var inputEventDate = new DateTime(2024, 10, 31); // example event date
        
            // act
            calendar.AddEvent(inputEventTitle, inputEventDate); // try to add event to the calendar
            var retrievedEvent = calendar.GetEvent(inputEventDate);

            // assert
            retrievedEvent.Should().NotBeNull(); // ensure we retrieved a non-null object
            retrievedEvent.eventTitle.Should().Be(inputEventTitle); // ensure the retrieved title matches the input title
            retrievedEvent.eventDate.Should().Be(inputEventDate); // ensure the retrieved date matches the input date
        }

        /*
        tests creating a duplicate event
        ensures:
            successfully saves only the first event on a date with matching titles
        */
        [Fact]
        public void TestCreateDuplicateEvents()
        {
            // arrange
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var eventTitle = "Halloween Party"; // example event title
            var duplicateTitle = "Halloween Party";
            var eventDate = new DateTime(2024, 10, 31); // example event date

            // act
            calendar.AddEvent(eventTitle, eventDate); // add the original event to the calendar
            calendar.AddEvent(duplicateTitle, eventDate); // attempt to add a duplicate event
            var retrievedEvents = calendar.GetEventsForDate(eventDate); // attempt to retrieve both events
        
            // assert
            retrievedEvents.Count().Should().Be(1); // ensure the list of events only contains one item
            retrievedEvents[0].eventTitle.Should().Be(eventTitle); // make sure the original event is still there
        }

        /*
        tests creating an event with empty title
        ensures:
            only allows valid, non-empty event titles
        */
        [Theory]
        [InlineData("")]  // empty string
        public void TestCreateEventWithInvalidTitle(string invalidTitle)
        {
            // arrange
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var eventDate = new DateTime(2024, 10, 31); // example date

            // act
            calendar.AddEvent(invalidTitle, eventDate); // try to create event with invalid title
            var retrievedEvent = calendar.GetEvent(eventDate);

            // assert
            retrievedEvent.Should().BeNull(); // ensure no event was created
        }

        /*
        tests creating a single event on an invalid date (a past date or a date that doesn't exist) 
        ensures:
            can successfully handle input dates from the past
            can successfully report the error through an exception
        */
        [Fact]
        public void TestCreateWithInvalidDate() 
        {
                // arrange
                var calendar = new Calendar(); // create a new instance of the calendar for testing 
                var pastEventTitle = "Past Event"; // create an event that occurred on a date in the past
                var pastDate = DateTime.Today.AddDays(-1);

                var invalidEventTitle = "Invalid Title";
                var invalidDate = new DateTime(2024, 2, 30); // use February 30th as an invalid date

                // act
                calendar.AddEvent(pastEventTitle, pastDate); // try to add a past event
                calendar.AddEvent(invalidEventTitle, invalidDate); // try to add an event on invalid date
                var retrievedPastEvent = calendar.GetEvent(pastDate); // try to retrieve the newly created past event 
                var retrievedInvalidEvent = calendar.GetEvent(invalidDate); // try to retrieve the newly created invalid event
                
                // assert
                retrievedPastEvent.Should().BeNull(); //ensure the retrieved past event returns a null object
                retrievedInvalidEvent.Should().BeNull(); // ensure the retrieved invalid event returns a null object
        }

        /*
        tests updating a single event title on a given date
        ensures:
            can successfully change and store an event title
            ensure that the update function only modifies the target variables
            can successfully retrieve the updated event
        */
        [Fact]
        public void TestUpdatingEvent()
        {
            // arrange
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var prevTitle = "OG Title"; // example data
            var newTitle = "Updated Title"; // example data
            var inputEventDate = new DateTime(2024, 10, 31); // example data

            // act
            calendar.AddEvent(prevTitle, inputEventDate); // create original event
            calendar.UpdateEventTitle(newTitle, inputEventDate); // update the event title
            var updatedEvent = calendar.GetEvent(inputEventDate); // get the event date to see if it changed
            
            // assert
            updatedEvent.Should().NotBeNull(); // ensure we retrieved a non-null object
            updatedEvent.eventTitle.Should().Be(newTitle); // ensure we successfully updated the title
        }

        /*
        tests updating event dates
        ensures:
            can successfully change an event's date
            event maintains its other properties after date change
        */
        [Fact]
        public void TestUpdatingEventDate()
        {
            // arrange
            var calendar = new Calendar();
            var eventTitle = "Moveable Event";
            var originalDate = new DateTime(2024, 10, 31);
            var newDate = new DateTime(2024, 11, 1);

            // act
            calendar.AddEvent(eventTitle, originalDate); // create event
            calendar.UpdateEventDate(originalDate, newDate); // change event date
            var oldDateEvent = calendar.GetEvent(originalDate); // check old date
            var newDateEvent = calendar.GetEvent(newDate); // check new date

            // assert
            oldDateEvent.Should().BeNull(); // ensure event no longer exists at old date
            newDateEvent.Should().NotBeNull(); // ensure event exists at new date
            newDateEvent.eventTitle.Should().Be(eventTitle); // ensure event maintained its title
        }

        /*
        tests updating an event that doesn't exist 
        ensures:
            can successfully report the error through an exception
        */
        [Fact]
        public void TestUpdatingNullEventTitle()
        {
            // arrange
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var newTitle = "Updated Title"; // example data
            var nullEventDate = new DateTime(2024, 10, 31); // example data

            // act
            calendar.UpdateEventTitle(newTitle, nullEventDate); // update the event title
            var updatedEvent = calendar.GetEvent(nullEventDate); // attempt to retrieve the non-existent event
            
            // assert
            updatedEvent.Should().BeNull(); // ensure we retrieved a null object
        }


        /*
        tests deleting a single event title on a given date
        ensures:
            can successfully remove events from the calendar
        */
        [Fact]
        public void TestDeletingEvent()
        {
            // arrage
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var inputEventTitle ="Delete This Event"; // example data
            var inputEventDate = new DateTime(2024, 10, 31); // example data

            // act
            calendar.AddEvent(inputEventTitle, inputEventDate); // create a new instance of the calendar for test
            calendar.DeleteEvent(inputEventDate); // delete the event
            var retrievedEvent = calendar.GetEvent(inputEventDate); // attempt to get the deleted event

            // assert
            retrievedEvent.Should().BeNull(); // ensure that the event is not found and returns a null object
        }

        /*
        tests deleting an event that doesn't exist
        ensures:
            can successfully report the error 
        */
        [Fact]
        public void TestDeletingNullEvent()
        {
            // arrage
            var calendar = new Calendar(); // create a new instance of the calendar for the test
            var nullEventDate = new DateTime(2024, 10, 31); // example data

            // act
            calendar.DeleteEvent(nullEventDate); // delete the event
            var retrievedEvent = calendar.GetEvent(nullEventDate); // attempt to get the deleted nonexistent event

            // assert
            retrievedEvent.Should().BeNull(); // ensure that the event is not found and returns a null object
        }

    }
}