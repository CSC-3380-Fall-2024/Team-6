using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;


namespace Team6.Data.Repositories
{
    // create a CalendarRepository class to specifically manage CalendarEvent data in the database, inheriting from the base repository so we can reuse the connection
    public class CalendarRepository : BaseRepository
    {   

        // initializes an instance of a CalendarRepository while calling the base constructor to establish the database connection
        public CalendarRepository(DatabaseContext context) : base(context) 
        {

        }

        // get a calendar event specified by Id
        public async Task<CalendarEvent?> GetByIdAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get the first calendar event where the Ids match
            return await connection.QueryFirstOrDefaultAsync<CalendarEvent>(
                "SELECT * FROM CalendarEvents WHERE Id = @Id AND UserId = @UserId", 
                new {Id = id, UserId = userId }
            );
        }

        // iterate through and get all of the calendar events 
        public async Task<IEnumerable<CalendarEvent>> GetAllAsync()
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get a list of all calendar events
            return await connection.QueryAsync<CalendarEvent>(
                "SELECT * FROM CalendarEvents"
            );
        }

        // iterate through and get all of the calendar events for a user 
        public async Task<IEnumerable<CalendarEvent>> GetAllForUserAsync(int userId)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get a list of all calendar events
            return await connection.QueryAsync<CalendarEvent>(
                "SELECT * FROM CalendarEvents WHERE UserId = @UserId ORDER BY EventDate",
                new { UserId = userId }
            );
        }

        // add a calendar event to the database and return the event Id
        public async Task<int> CreateAsync(CalendarEvent calendarEvent, int userId)
        {
            var connection = CreateConnection();
            // define the sql query with the main calendar event params 
            var sql = @"INSERT INTO CalendarEvents (Title, EventDate, Description, UserId)
                        VALUES (@Title, @EventDate, @Description, @UserId);
                        SELECT last_insert_rowid();";
            // execute the query asynchronously and return the new event Id
            return await connection.ExecuteScalarAsync<int>(
                sql, 
                new {
                    calendarEvent.Title,
                    calendarEvent.EventDate,
                    calendarEvent.Description,
                    UserId = userId
                }
            );
        }

        // update an existing calendar event
        public async Task UpdateAsync(CalendarEvent calendarEvent, int userId)
        {
            using var connection = CreateConnection();
            // define the sql query to change the calendar event params
            var sql = @"UPDATE CalendarEvents
                        SET Title = @Title, EventDate = @EventDate, Description = @Description
                        WHERE Id = @Id AND UserId = @UserId";
            // execute the query asynchronously
            await connection.ExecuteAsync(
                sql, 
                new {
                    calendarEvent.Title,
                    calendarEvent.EventDate,
                    calendarEvent.Description, 
                    calendarEvent.Id,
                    UserId = userId
                }
            );
        }

        // delete an existing calendar event (specified by Id)
        public async Task DeleteAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously
            await connection.ExecuteAsync(
                "DELETE FROM CalendarEVENTS WHERE Id = @Id AND UserId = @UserId",
                new { Id = id, UserId = userId }
            );
        }

    }
}