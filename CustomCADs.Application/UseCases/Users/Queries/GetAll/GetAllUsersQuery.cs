using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetAll;

public record GetAllUsersQuery(
    bool? HasRT = null,
    string? Username = null,
    string? Email = null,
    string? FirstName = null,
    string? LastName = null,
    DateTime? RtEndDateBefore = null,
    DateTime? RtEndDateAfter = null,
    string Sorting = "",
    int Page = 1,
    int Limit = 20) : IRequest<UserResult>;