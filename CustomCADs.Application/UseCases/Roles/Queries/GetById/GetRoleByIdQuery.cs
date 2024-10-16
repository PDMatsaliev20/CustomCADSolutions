using CustomCADs.Application.Models.Roles;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetById;

public record GetRoleByIdQuery(string Id) : IRequest<RoleModel>;