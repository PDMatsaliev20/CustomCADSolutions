using AutoMapper;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        EntityToModel();
        ModelToEntity();
    }

    private void EntityToModel() => CreateMap<Role, RoleModel>();

    private void ModelToEntity() => CreateMap<RoleModel, Role>();
}
