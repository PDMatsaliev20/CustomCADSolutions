using CustomCADs.Application.Models.Users;
using FastEndpoints;
using Microsoft.AspNetCore.JsonPatch;

namespace CustomCADs.API.Endpoints.Users.PatchUser
{
    public class PatchUserRequest
    {
        [BindFrom("username")]
        public required string Username { get; set; }

        [FromBody]
        public required JsonPatchDocument<UserModel> PatchDocument { get; set; }
    }
}
