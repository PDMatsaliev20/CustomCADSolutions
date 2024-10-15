using AutoMapper;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        EntityToModel();
        ModelToEntity();
    }

    private void EntityToModel() => CreateMap<User, UserModel>();

    private void ModelToEntity() => CreateMap<UserModel, User>();
}
