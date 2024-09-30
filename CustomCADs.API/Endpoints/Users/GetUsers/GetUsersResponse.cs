using CustomCADs.API.Models.Users;

namespace CustomCADs.API.Endpoints.Users.GetUsers
{
    public class GetUsersResponse
    {
        public int Count { get; set; }
        public UserGetDTO[] Users { get; set; } = [];
    }
}
