using MediatR;
using PlanningGambler.Server.Commands;

namespace PlanningGambler.Server.Handlers;

public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, bool>
{
    public Task<bool> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}