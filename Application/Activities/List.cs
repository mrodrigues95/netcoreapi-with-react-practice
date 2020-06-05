using Domain;
using System.Collections.Generic;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities {
    public class List {
        public class Query : IRequest<List<Activity>> { }

        // GET all of the activites from the database and return them.
        public class Handler : IRequestHandler<Query, List<Activity>> {
            private readonly DataContext _context;

            public Handler(DataContext context) {
                _context = context;
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken) {
                var activities = await _context.Activities.ToListAsync();
                return activities;
            }
        }
    }
}
