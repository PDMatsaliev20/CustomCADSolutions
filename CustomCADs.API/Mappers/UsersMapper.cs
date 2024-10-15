using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Identity.Register;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.Models.Users;
using Mapster;

namespace CustomCADs.API.Mappers;

public class UsersMapper
{
    public static void Map()
    {
        TypeAdapterConfig<UserModel, UserResponseDto>.NewConfig()
            .Map(r => r.Username, u => u.UserName)
            .Map(r => r.Role, u => u.RoleName);

        TypeAdapterConfig<RegisterRequest, UserModel>.NewConfig()
            .Map(u => u.UserName, r => r.Username)
            .Map(u => u.RoleName, r => r.Role)
            .Map(u => u.Role, r => new RoleModel());
    }
}
