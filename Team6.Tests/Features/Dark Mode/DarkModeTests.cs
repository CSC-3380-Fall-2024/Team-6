using Xunit; // required testing framework
using System; // for datetime and other system functions
using System.Collections.Generic; //
using FluentAssertions;
using System.Dynamic; // for better/more descript error messages? Not entirely sure but came across it when researching a bit

namespace Team6.Tests.Features.DarkMode;

public class Theme
{
    public string currTheme {get; set;} = "light"; //light mode as default them
    public bool isDarkMode { get; set;} = false; // bool to track dark mode

    // create a class to manage potential dark mode / color theme functions 
    public class ColorTheme
    {
        /*
        function to swap between light and dark mode
        args:
            None
        returns:
            None
        */
        public void SwapTheme () 
        {

        }

        /*
        function to get current theme
        args:
            None
        returns:
            Theme object
        */
        public Theme GetTheme () 
        {
            return null; // allowing null just for now while setting up test structure 
        }

         /*
        function to set a given theme
        args:
            themeName (string): name of the theme 
        returns:
            None
        */
        public void SetTheme(string themeName)
        {

        }

         /*
        function to reset to light mode
        args:
            None
        returns:
            None
        */
        public void ResetTheme () 
        {

        }

    }

    // create tests for the above basic functions using Arrange-Act-Assert (aka Given-When-Then)
        // arrange: set up test conditions and the required data
        // act: perform the test action
        // assert: verify the result of the act
    public class ThemTests
    {

         /*
        tests swapping between themes
        ensures:
            can successfully change from light to dark mode
            can successfully change the theme state boolean
        */
        [Fact] // mark the function as a unit test
        public void TestSwappingThemes()
        {
            // arrange
           var colorTheme = new ColorTheme(); //create a new instance for testing
        
            // act
           colorTheme.SwapTheme(); // swap from light to dark mode
           var currTheme = colorTheme.GetTheme(); // get the current theme state

            // assert
            currTheme.Should().NotBeNull(); // ensure we retrieved a non-null object
            currTheme.currTheme.Should().Be("dark"); // ensure the theme is now dark
            currTheme.isDarkMode.Should().BeTrue(); // ensure the dark mode boolean is now true
        }

         /*
        tests theme reset
        ensures:
            can successfully change from dark mode back to light mode
            can successfully change the theme state boolean
        */
        [Fact]
        public void TestThemeReset()
        {
            // arrange
            var colorTheme = new ColorTheme(); // create a new instance for testing

            // act
            colorTheme.SwapTheme(); // swap to dark mode 
            colorTheme.ResetTheme(); // swap back to light mode
            var currTheme = colorTheme.GetTheme(); // get current theme state
        
            // assert
            currTheme.Should().NotBeNull(); // ensure we retrieved a non-null object
            currTheme.currTheme.Should().Be("light"); // ensure the theme is now dark
            currTheme.isDarkMode.Should().BeFalse(); // ensure the dark mode boolean is now false
        }
    }
}
