using System.Data.Entity;
using Tntp.WebApplication.Models;

namespace Tntp.WebApplication.Tests
{
    public class MockRepository : IRepository
    {
        private readonly IDbSet<Comment> _comments = new MockDbSet<Comment>();

        public IDbSet<Comment> Comments
        {
            get { return _comments; }
        }

        public void SaveChanges() { }

        public void Dispose() { }
    }
}