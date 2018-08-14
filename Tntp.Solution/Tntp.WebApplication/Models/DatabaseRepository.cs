using System;
using System.Data.Entity;

namespace Tntp.WebApplication.Models
{
    /// <summary>
    /// The DatabaseRepository class is an implementation of IRepository that connects to a database.
    /// </summary>
    public class DatabaseRepository : IRepository
    {
        private readonly DatabaseContext _databaseContext = new DatabaseContext();

        private bool _isDisposed;

        public IDbSet<Comment> Comments
        {
            get { return _databaseContext.Comments; }
        }

        public void SaveChanges()
        {
            _databaseContext.SaveChanges();
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _databaseContext.Dispose();
                _isDisposed = true;
            }
        }
    }
}