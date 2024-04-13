using AutoMapper;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using System.Drawing;

namespace CustomCADSolutions.Core.Mappings
{
    public class CadProfile : Profile
    {
        public CadProfile()
        {
            DTOToModel();
            ModelToDTO();
            
            QueryToDTO();
            DTOToQuery();

            ModelToEntity();
            EntityToModel();
        }

        /// <summary>
        /// Converts JSON Import to Service Model
        /// </summary>
        /// <returns>IMappingExpression with DTO source and Model destination</returns>
        public IMappingExpression<CadImportDTO, CadModel> DTOToModel() => CreateMap<CadImportDTO, CadModel>()
            .ForMember(cad => cad.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(cad => cad.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(cad => cad.CategoryId, opt => opt.MapFrom(dto => dto.CategoryId))
            .ForMember(cad => cad.Coords, opt => opt.MapFrom(dto => dto.Coords))
            .ForMember(cad => cad.Color, opt => opt.MapFrom(dto => Color.FromArgb(1, dto.RGB[0], dto.RGB[1], dto.RGB[2])))
            .ForMember(cad => cad.IsValidated, opt => opt.MapFrom(dto => dto.IsValidated))
            .ForMember(dto => dto.Price, opt => opt.MapFrom(input => input.Price))
            .ForMember(cad => cad.CreatorId, opt => opt.MapFrom(dto => dto.CreatorId))
            .ForMember(cad => cad.SpinAxis, opt =>
            {
                opt.AllowNull();
                opt.MapFrom(dto => dto.SpinAxis);
            });

        /// <summary>
        /// Converts Service Model to JSON Export
        /// </summary>
        /// <returns>IMappingExpression with Model source and DTO destination</returns>
        public IMappingExpression<CadModel, CadExportDTO> ModelToDTO() => CreateMap<CadModel, CadExportDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
            .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(model => model.Category.Name))
            .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(model => model.CategoryId))
            .ForMember(dto => dto.Coords, opt => opt.MapFrom(model => model.Coords))
            .ForMember(dto => dto.SpinAxis, opt => opt.MapFrom(model => model.SpinAxis))
            .ForMember(dto => dto.IsValidated, opt => opt.MapFrom(model => model.IsValidated))
            .ForMember(dto => dto.Price, opt => opt.MapFrom(model => model.Price))
            .ForMember(dto => dto.RGB, opt => opt.MapFrom(model => new byte[] { model.Color.R, model.Color.G, model.Color.B }))
            .ForMember(dto => dto.CreatorName, opt => opt.MapFrom(model => model.Creator != null ? model.Creator.UserName : null))
            .ForMember(dto => dto.CreatorId, opt => opt.MapFrom(model => model.CreatorId))
            .ForMember(dto => dto.CreationDate, opt => opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")));

        public IMappingExpression<CadQueryModel, CadQueryDTO> QueryToDTO() => CreateMap<CadQueryModel, CadQueryDTO>()
            .ForMember(dto => dto.TotalCount, opt => opt.MapFrom(query => query.TotalCount))
            .ForMember(dto => dto.Cads, opt => opt.MapFrom(query => query.Cads))
            ;

        public IMappingExpression<CadQueryDTO, CadQueryModel> DTOToQuery() => CreateMap<CadQueryDTO, CadQueryModel>()
            .ForMember(dto => dto.TotalCount, opt => opt.MapFrom(query => query.TotalCount))
            .ForMember(dto => dto.Cads, opt => opt.MapFrom(query => query.Cads));

        public IMappingExpression<Cad, CadModel> ModelToEntity() => CreateMap<Cad, CadModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(m => m.Bytes, opt => opt.MapFrom(c => c.Bytes))
                .ForMember(m => m.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(m => m.IsValidated, opt => opt.MapFrom(c => c.IsValidated))
                .ForMember(m => m.Price, opt => opt.MapFrom(c => c.Price))
                .ForMember(m => m.CreationDate, opt => opt.MapFrom(c => c.CreationDate))
                .ForMember(m => m.Coords, opt => opt.MapFrom(c => new int[] { c.X, c.Y, c.Z }))
                .ForMember(m => m.Color, opt => opt.MapFrom(c => Color.FromArgb(1, c.R, c.G, c.B)))
                .ForMember(m => m.SpinAxis, opt => opt.MapFrom(c => c.SpinAxis))
                .ForMember(m => m.CategoryId, opt => opt.MapFrom(c => c.CategoryId))
                .ForMember(m => m.CreatorId, opt => opt.MapFrom(c => c.CreatorId))
                .ForMember(m => m.Category, opt => opt.MapFrom(c => c.Category))
                .ForMember(m => m.Creator, opt => opt.MapFrom(c => c.Creator))
                .ForMember(m => m.Orders, opt => opt.MapFrom(c => c.Orders.ToArray()));

        public IMappingExpression<CadModel, Cad> EntityToModel() => CreateMap<CadModel, Cad>()
                .ForMember(c => c.Id, opt => opt.MapFrom(m => m.Id))
                .ForMember(c => c.Bytes, opt =>
                {
                    opt.AllowNull();
                    opt.MapFrom(m => m.Bytes);
                })
                .ForMember(c => c.Name, opt => opt.MapFrom(m => m.Name))
                .ForMember(c => c.IsValidated, opt => opt.MapFrom(m => m.IsValidated))
                .ForMember(c => c.Price, opt => opt.MapFrom(m => m.Price))
                .ForMember(c => c.CreationDate, opt => opt.MapFrom(m => m.CreationDate))
                .ForMember(c => c.X, opt => opt.MapFrom(m => m.Coords[0]))
                .ForMember(c => c.Y, opt => opt.MapFrom(m => m.Coords[1]))
                .ForMember(c => c.Z, opt => opt.MapFrom(m => m.Coords[2]))
                .ForMember(c => c.R, opt => opt.MapFrom(m => m.Color.R))
                .ForMember(c => c.G, opt => opt.MapFrom(m => m.Color.G))
                .ForMember(c => c.B, opt => opt.MapFrom(m => m.Color.B))
                .ForMember(c => c.SpinAxis, opt => opt.MapFrom(m => m.SpinAxis))
                .ForMember(c => c.CategoryId, opt => opt.MapFrom(m => m.CategoryId))
                .ForMember(c => c.CreatorId, opt => opt.MapFrom(m => m.CreatorId))
                .ForMember(c => c.Category, opt => opt.MapFrom(m => m.Category))
                .ForMember(c => c.Creator, opt => opt.MapFrom(m => m.Creator))
                .ForMember(c => c.Orders, opt => opt.MapFrom(m => m.Orders.ToArray()));
    }
}
