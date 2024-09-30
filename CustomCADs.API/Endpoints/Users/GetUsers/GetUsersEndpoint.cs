using AutoMapper;
using CustomCADs.API.Models.Users;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using System.Collections.Generic;

namespace CustomCADs.API.Endpoints.Users.GetUsers
{
    using static StatusCodes;

    public class GetUsersEndpoint(IUserService service) : Endpoint<GetUsersRequest, GetUsersResponse>
    {
        public override void Configure()
        {
            Get("");
            Group<UsersGroup>();
            Description(d => d.WithSummary("Gets All Users."));
            Options(opt =>
            {
                opt.Produces<GetUsersResponse>(Status200OK, "application/json");
                opt.ProducesProblem(Status400BadRequest);
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(GetUsersRequest req, CancellationToken ct)
        {
            UserResult result = service.GetAll(
                username: req.Name,
                sorting: req.Sorting ?? "",
                page: req.Page,
                limit: req.Limit
            );
            
            GetUsersResponse response = new()
            {
                Count = result.Count,
                Users = result.Users
                    .Select(u => new UserGetDTO()
                    {
                        Email = u.Email,
                        Username = u.UserName,
                        Role = u.RoleName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                    }).ToArray()
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
