using Data.Contexts;
using Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers.Queries.Library
{
    public class AuthorsQuery : IRequest<AuthorsQueryResponse>, IQuery
    {
    }

    public class AuthorsQueryResponse : QueryResponse
    {
        public IEnumerable<Author> Authors { get; set; }
    }

    public interface IAuthorsQueryHandler : IRequestHandler<AuthorsQuery, AuthorsQueryResponse>
    {
    }

    public class AuthorsQueryHandler : IAuthorsQueryHandler
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorsQueryHandler(ILogger<AuthorsQueryHandler> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthorsQueryResponse> Handle(AuthorsQuery query, CancellationToken cancellationToken)
        {
            using (_dbContext)
            {
                var result = await _dbContext.Authors.ToListAsync();

                return new AuthorsQueryResponse { Authors = result };
            }
        }
    }
}
