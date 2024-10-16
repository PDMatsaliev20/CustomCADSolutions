using CustomCADs.Application.Models.Roles;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetByName;

public record GetRoleByNameQuery(string Name) : IRequest<RoleModel>;