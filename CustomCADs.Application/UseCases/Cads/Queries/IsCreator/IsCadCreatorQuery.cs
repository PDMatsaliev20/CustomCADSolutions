using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.IsCreator;

public record IsCadCreatorQuery(int Id, string Username) : IRequest<bool>;
