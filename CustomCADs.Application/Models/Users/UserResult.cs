namespace CustomCADs.Application.Models.Users;

public class UserResult
{
    public int Count { get; set; }
    public ICollection<UserModel> Users { get; set; } = [];
}
