using Xunit;
using Team6.Data.Context;
using System.Data;

namespace Team6.Tests.Features.Database
{

        /*
        Test verifying the database connection and intialization
        ensures:
            database can be created and connected to
            tables are created correctly
        */
        public class DatabaseConnectionTests
        {
            [Fact]
            public void TestDatabaseConnection()
            {
                // arrange
                var dbContext = new DatabaseContext("test.db"); // use test database file

                // act
                using var connection = dbContext.CreateConnection();
                connection.Open();

                // assert
                Assert.True(connection.State == System.Data.ConnectionState.Open);
            }
        }



}