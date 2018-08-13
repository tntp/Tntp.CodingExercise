using System;
using System.Data.Entity;

namespace Tntp.WebApplication.Models
{
    /// <summary>
    /// The IRepository interface is a light abstraction for data access.
    /// </summary>
    public interface IRepository : IDisposable
    {
        IDbSet<Comment> Comments { get; }
    }
}