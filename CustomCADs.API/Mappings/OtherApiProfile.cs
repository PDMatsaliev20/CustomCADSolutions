using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.API.Mappings
{
    public class OtherApiProfile : Profile
    {
        public OtherApiProfile()
        {
            CategoryToDTO();
            CadSortingToDTO();
            CadStatusToDTO();
            OrderStatusToDTO();
        }

        private void CategoryToDTO() => CreateMap<CategoryModel, CategoryDTO>();

        private void CadSortingToDTO() => CreateMap<CadSorting, CadSortingDTO>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(sort => sort.ToString()))
            .ForMember(dto => dto.Value, opt => opt.MapFrom(sort => sort));
        
        private void CadStatusToDTO() => CreateMap<CadStatus, CadStatusDTO>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(status => status.ToString()))
            .ForMember(dto => dto.Value, opt => opt.MapFrom(status => status));

        private void OrderStatusToDTO() => CreateMap<OrderStatus, OrderStatusDTO>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(status => status.ToString()))
            .ForMember(dto => dto.Value, opt => opt.MapFrom(status => status));
    }
}
