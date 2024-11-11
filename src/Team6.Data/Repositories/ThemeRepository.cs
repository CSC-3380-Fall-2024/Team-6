using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{
    
    // create a repository to manage operations for user theme setttings
    public class ThemeSettingRepository : BaseRepository
    {

        // initialize an instance of ThemeSettingRepository while calling the base constructor to establish the database connection
        public ThemeSettingRepository(DatabaseContext context) : base(context)
        {

        }

        // get the theme setting for a specific user
        public async Task<ThemeSetting?> GetByUserIdAsync(string userId)
        {
            using var connection = CreateConnection();

        }
    }

}