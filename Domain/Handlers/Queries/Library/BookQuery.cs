using Data.Contexts;
using Data.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers.Queries.Library
{
    public class BookQuery : IRequest<BookQueryResponse>, IQuery<BookQueryResponse>
    {
        public BookQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class BookQueryResponse : QueryResponse
    {
        public Book Book { get; set; }
    }

    public interface IBookQueryHandler : IRequestHandler<BookQuery, BookQueryResponse>
    {
    }

    public class BookQueryHandler : IBookQueryHandler
    {
        private readonly ApplicationDbContext _dbContext;

        public BookQueryHandler(ILogger<BookQueryHandler> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookQueryResponse> Handle(BookQuery query, CancellationToken cancellationToken)
        {
            using (_dbContext)
            {
                var result = await _dbContext.Books.FindAsync(query.Id);

                return new BookQueryResponse { Book = result };
            }
        }
    }
}
