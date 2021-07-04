using Data.Models;
using Domain.Handlers.Queries.Library;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly IMediator _mediator;

        public LibraryController(ILogger<LibraryController> logger,
                                 IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/Author/{authorId}")]
        public async Task<Author> Author(int authorId)
        {
            var query = new AuthorQuery(authorId);
            var queryResult = await _mediator.Send(query);

            return queryResult.Author;
        }

        [HttpGet]
        [Route("/Authors")]
        public async Task<IEnumerable<Author>> Authors()
        {
            var query = new AuthorsQuery();
            var queryResult = await _mediator.Send(query);

            return queryResult.Authors;
        }

        [HttpGet]
        [Route("/Book/{bookId}")]
        public async Task<Book> Book(int bookId)
        {
            var query = new BookQuery(bookId);
            var queryResult = await _mediator.Send(query);

            return queryResult.Book;
        }
    }
}
