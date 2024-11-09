using System.Collections.Generic;
using Dapper;
using Team6.Core.Models;



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
        public async Task<CalendarEvent?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get the first calendar event where the Id matches
            return await connection.QueryFirstOrDefaultAsync<CalendarEvent>(
                "SELECT * FROM CalendarEvents WHERE Id = @Id", 
                new {Id = id}
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

        // add a calendar event to the database and return the event Id
        public async Task<int> CreateAsync(CalendarEvent calendarEvent)
        {
            var connection = CreateConnection();
            // define the sql query with the main calendar event params 
            var sql = @"INSERT INTO CalendarEvents (Title, EventDate, Description)
                        VALUES (@Title, @EventDate, @Description);
                        SELECT last_insert_rowid();";
            // execute the query asynchronously and return the new event Id
            return await connection.ExecuteScalarAsync<int>(sql, calendarEvent);
        }

        // update an existing calendar event
        public async Task UpdateAsync(CalendarEvent calendarEvent)
        {
            using var connection = CreateConnection();
            // define the sql query to change the calendar event params
            var sql = @"UPDATE CalendarEvents
                        SET Title = @Title, EventDate = @EventDate, Description = @Description
                        WHERE Id = @Id";
            // execute the query asynchronously
            await connection.ExecuteAsync(sql, calendarEvent);
        }

        // delete an existing calendar event (specified by Id)
        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously
            await connection.ExecuteAsync(
                "DELETE FROM CalendarEVENTS WHERE Id = @Id", new { Id = id}
            );
        }

    }
}