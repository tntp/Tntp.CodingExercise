using System.Data.Entity;

namespace Tntp.WebApplication.Models
{
    /// <summary>
    /// The DatabaseContext class is a subclass of DbContext that's aware of the Comments table. It's used by the DatabaseRepository class.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }

        public DatabaseContext() : base("name=Tntp")
        {
            // Code First with Existing Database - Make sure to not automatically create a new database or update an existing one.
            Database.SetInitializer<DatabaseContext>(null);
        }
    }
}