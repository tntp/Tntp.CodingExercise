using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Tntp.WebApplication.Models;

namespace Tntp.WebApplication.Controllers
{
    /// <summary>
    /// The CommentsController class serves as a web-API controller for posting and viewing comments.
    /// </summary>
    [RoutePrefix("api/comments")]
    public class CommentsController : ApiController
    {
        private const int UsernameMaxLength = 15;
        private const int ContentMaxLength = 140;

        private readonly IRepository _repository;

        public CommentsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Comment> GetComments()
        {
            return _repository.Comments.OrderByDescending(c => c.CreationTimestamp);
        }

        [HttpPost]
        public IHttpActionResult AddComment(Comment comment)
        {
            if((comment.Username?.Length ?? 0) == 0)
            {
                return BadRequest("A username is required.");
            }
            else if(comment.Username.Length > UsernameMaxLength)
            {
                return BadRequest(string.Format("A username must not exceed {0} characters.", UsernameMaxLength));
            }
            else if((comment.Content?.Length ?? 0) == 0)
            {
                return BadRequest("A comment is required.");
            }
            else if(comment.Content.Length > ContentMaxLength)
            {
                return BadRequest(string.Format("A comment must not exceed {0} characters.", ContentMaxLength));
            }

            _repository.Comments.Add(comment);
            _repository.SaveChanges();

            return StatusCode(HttpStatusCode.OK);
        }
    }
}