using Data.Contexts;
using Data.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers.Queries.Library
{
    public class AuthorQuery : IRequest<AuthorQueryResponse>, IQuery
    {
        public AuthorQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class AuthorQueryResponse : QueryResponse
    {
        public Author Author { get; set; }
    }

    public interface IAuthorQueryHandler : IRequestHandler<AuthorQuery, AuthorQueryResponse>
    {
    }

    public class AuthorQueryHandler : IAuthorQueryHandler
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorQueryHandler(ILogger<AuthorQueryHandler> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthorQueryResponse> Handle(AuthorQuery query, CancellationToken cancellationToken)
        {
            using (_dbContext)
            {
                var result = await _dbContext.Authors.FindAsync(query.Id);

                return new AuthorQueryResponse { Author = result }; ;
            }
        }
    }
}
