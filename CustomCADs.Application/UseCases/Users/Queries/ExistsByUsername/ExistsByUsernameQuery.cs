using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsByUsername;

public record ExistsByUsernameQuery(string Username) : IRequest<bool>;