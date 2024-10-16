using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetById;

public record GetUserByIdQuery(string Id) : IRequest<UserModel>;