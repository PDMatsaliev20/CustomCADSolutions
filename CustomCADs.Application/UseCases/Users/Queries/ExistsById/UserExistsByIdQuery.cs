using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsById;

public record UserExistsByIdQuery(string Id) : IRequest<bool>;
