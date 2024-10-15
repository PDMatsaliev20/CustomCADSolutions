namespace CustomCADs.API.Endpoints.Users.GetUsers;

public class GetUsersRequest
{
    public string? Name { get; set; }
    public string? Sorting { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 50;
}
