using CustomCADs.Application.Models.Roles;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAll;

public record GetAllRolesQuery(
    string? Name = null, 
    string? Description = null, 
    string Sorting = "") : IRequest<IEnumerable<RoleModel>>;
