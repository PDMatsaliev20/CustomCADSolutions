using CustomCADs.API.Endpoints.Users.Responses;

namespace CustomCADs.API.Endpoints.Users.GetUsers
{
    public class GetUsersResponse
    {
        public int Count { get; set; }
        public UserResponseDto[] Users { get; set; } = [];
    }
}
