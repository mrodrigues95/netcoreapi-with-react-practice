using Application.Errors;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User {

    // Handle user registration.
    public class Register {
        
        public class Command : IRequest<User> {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // Validation.
        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator() {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        // Register a user.
        public class Handler : IRequestHandler<Command, User> {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DataContext context, UserManager<AppUser> userManager, 
                IJwtGenerator jwtGenerator) {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken) {
                // If the email is already in use, throw an exception.
                if (await _context.Users.AnyAsync(x => x.Email == request.Email)) {
                    throw new RestException(HttpStatusCode.BadRequest, 
                        new { Email = "Email already exists!" });
                }

                // If the username is already in use, throw an exception.
                if (await _context.Users.AnyAsync(x => x.UserName == request.Username)) {
                    throw new RestException(HttpStatusCode.BadRequest,
                        new { Email = "Username already exists!" });
                }

                // Create user.
                var user = new AppUser {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.Username
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded) {
                    return new User {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}
