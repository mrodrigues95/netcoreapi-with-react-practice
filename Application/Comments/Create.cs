using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Comments {

    // Create a new comment.
    public class Create {
        public class Command : IRequest<CommentDto> {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentDto> {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken) {
                var activity = await _context.Activities.FindAsync(request.ActivityId);

                // Unable to find activity.
                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not Found" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                // Set comment.
                var comment = new Comment {
                    Author = user,
                    Activity = activity,
                    Body = request.Body,
                    CreatedAt = DateTime.Now
                };

                // Save the comment to the database.
                activity.Comments.Add(comment);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                    return _mapper.Map<CommentDto>(comment);

                throw new Exception("Problem saving changes");
            }
        }
    }
}
