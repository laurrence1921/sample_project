using CleanArchitecture.Application.Account.Models;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Application.Account.Commands;

public class LoginCommand : IRequest<UserViewModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = true;

    public class Handler : IRequestHandler<LoginCommand, UserViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ILogger _logger;

        public Handler(IApplicationDbContext context, 
            IMediator mediator, 
            IIdentityService identityService,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _mediator = mediator;
            _identityService = identityService;
            _logger = loggerFactory.CreateLogger<LoginCommand>();
        }

        public async Task<UserViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.SignIn(request.Email, request.Password, request.RememberMe, true);
        }
    }
}
