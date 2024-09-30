using FastEndpoints;

namespace CustomCADs.API.Endpoints.Users.DeleteUser
{
    public class DeleteUserRequest
    {
        [BindFrom("username")]
        public required string Username { get; set; }
    }
}
