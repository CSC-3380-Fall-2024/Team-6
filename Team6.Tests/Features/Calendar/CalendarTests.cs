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
        public string eventTitle {get; set;} // store event title
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
        public void UpdateEvent (string newTitle, DateTime date) 
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
            var inputEventTitle = 'Halloween'; // example event title
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
        tests creating a single event on an invalid date (a past date or a date that doesn't exist) --> needs implementation
        ensures:
            can successfully handle input dates from the past
            can successfully report the error through an exception
        */
        [Fact]
        public void TestCreateWithInvalidDateAndThrowException() 
        {
            // arrange
        
            // act
        
            // assert
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
            var prevTitle = 'OG Title'; // example data
            var newTitle = 'Updated Title'; // example data
            var inputEventDate = new DateTime(2024, 10, 31); // example data

            // act
            calendar.AddEvent(prevTitle, inputEventDate); // create original event
            calendar.UpdateEvent(newTitle, inputEventDate); // update the event title
            var updatedEvent = calendar.GetEvent(inputEventDate); // get the event date to see if it changed
            
            // assert
            updatedEvent.Should().NotBeNull(); // ensure we retrieved a non-null object
            updatedEvent.eventTitle.Should().Be(newTitle); // ensure we successfully updated the title
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
            var inputEventTitle = 'Delete This Event'; // example data
            var inputEventDate = new DateTime(2024, 10, 31); // example data

            // act
            calendar.AddEvent(inputEventTitle, inputEventDate); // create a new instance of the calendar for test
            calendar.DeleteEvent(inputEventDate); // delete the event
            var retrievedEvent = calendar.GetEvent(inputEventDate); // attempt to get the deleted event

            // assert
            retrievedEvent.Should().BeNull(); // ensure that the event is not found and returns a null object
        }

    }
}