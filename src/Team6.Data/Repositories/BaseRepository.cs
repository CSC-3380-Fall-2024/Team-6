using System.Data;
using System.Collections.Generic;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly DatabaseContext _context;
        protected BaseRepository (DatabaseContext context) 
        {
            _context = context;
        }
        protected IDbConnection CreateConnection() 
        {
            return _context.CreateConnection;
        }
    }
}