using AutoMapper;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Mappings
{
    public class OrderDTOProfile : Profile
    {
        public OrderDTOProfile()
        {
            InputToDTO();
            DTOToModel();
            ModelToDTO();
            DTOToView();
        }

        public IMappingExpression<OrderInputModel, OrderImportDTO> InputToDTO() => CreateMap<OrderInputModel, OrderImportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.MapFrom(input => input.CadId))
            .ForMember(dto => dto.BuyerId, opt => opt.MapFrom(input => input.BuyerId))
            .ForMember(dto => dto.Description, opt => opt.MapFrom(input => input.Description))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(input => input.Status.ToString()))
            .ForMember(dto => dto.Cad, opt => opt.MapFrom(input => new CadImportDTO
            {
                Id = input.CadId,
                Name = input.Name,
                CategoryId = input.CategoryId,
            }));

        public IMappingExpression<OrderImportDTO, OrderModel> DTOToModel() => CreateMap<OrderImportDTO, OrderModel>()
            .ForMember(model => model.CadId, opt => opt.MapFrom(dto => dto.CadId))
            .ForMember(model => model.BuyerId, opt => opt.MapFrom(dto => dto.BuyerId))
            .ForMember(model => model.Description, opt => opt.MapFrom(dto => dto.Description))
            .ForMember(model => model.Status, opt => opt.MapFrom(dto => dto.Status))
            .ForMember(model => model.Cad, opt => opt.MapFrom(dto => new CadModel()
            {
                Id = dto.Cad.Id,
                Name = dto.Cad.Name,
                CategoryId = dto.Cad.CategoryId, 
            }));

        public IMappingExpression<OrderModel, OrderExportDTO> ModelToDTO() => CreateMap<OrderModel, OrderExportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.MapFrom(model => model.CadId))
            .ForMember(dto => dto.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(dto => dto.BuyerId, opt => opt.MapFrom(model => model.BuyerId))
            .ForMember(dto => dto.Description, opt => opt.MapFrom(model => model.Description))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(dto => dto.Cad, opt => opt.MapFrom(model => new CadExportDTO
            {
                Id = model.CadId,
                Name = model.Cad.Name,
                CategoryName = model.Cad.Category.Name,
                CategoryId = model.Cad.Category.Id,
            }))
            ;


        public IMappingExpression<OrderExportDTO, OrderViewModel> DTOToView() => CreateMap<OrderExportDTO, OrderViewModel>()
            .ForMember(view => view.CadId, opt => opt.MapFrom(dto => dto.CadId))
            .ForMember(view => view.Name, opt => opt.MapFrom(dto => dto.Cad.Name))
            .ForMember(view => view.Category, opt => opt.MapFrom(dto => dto.Cad.CategoryName))
            .ForMember(view => view.Status, opt => opt.MapFrom(dto => dto.Status))
            .ForMember(view => view.Description, opt => opt.MapFrom(dto => dto.Description))
            .ForMember(view => view.OrderDate, opt => opt.MapFrom(dto => dto.OrderDate))
            .ForMember(view => view.BuyerName, opt => opt.MapFrom(dto => dto.BuyerName))
            .ForMember(view => view.BuyerId, opt => opt.MapFrom(dto => dto.BuyerId))
            ;
    }
}
