using AutoMapper;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Models.Cads.Input;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Models;
using System.Drawing;

namespace CustomCADSolutions.App.Mappings
{
    /// <summary>
    /// Covers InputModel-ImportDTO, ImportDTO-Model, Model-ExportDTO and ExportDTO-ViewModel
    /// </summary>
    public class CadDTOProfile : Profile
    {
        public CadDTOProfile()
        {
            AddToDTO();
            EditToDTO();
            DTOToModel();
            ModelToDTO();
            DTOToView();
            QueryToDTO();
        }

        public void AddToDTO() => CreateMap<CadAddModel, CadImportDTO>();
        
        public void EditToDTO() => CreateMap<CadEditModel, CadImportDTO>()
            .ForMember(dto => dto.Coords, opt => opt.MapFrom(edit => new int[] { edit.X, edit.Y, edit.Z }));

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
            .ForMember(cad => cad.Bytes, opt => opt.MapFrom(dto => dto.Bytes))
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
            .ForMember(dto => dto.CreationDate, opt => opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(dto => dto.Bytes, opt => opt.MapFrom(model => model.Bytes));

        /// <summary>
        /// Converts JSON Export to User View
        /// </summary>
        /// <returns>IMappingExpression with DTO source and View destinations</returns>
        public IMappingExpression<CadExportDTO, CadViewModel> DTOToView() => CreateMap<CadExportDTO, CadViewModel>()
            .ForMember(view => view.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(view => view.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(view => view.Category, opt => opt.MapFrom(dto => dto.CategoryName))
            .ForMember(view => view.Coords, opt => opt.MapFrom(dto => dto.Coords))
            .ForMember(view => view.SpinAxis, opt => opt.MapFrom(dto => dto.SpinAxis))
            .ForMember(view => view.RGB, opt => opt.MapFrom(dto => dto.RGB))
            .ForMember(view => view.CreatorName, opt => opt.MapFrom(dto => dto.CreatorName))
            .ForMember(view => view.CreationDate, opt => opt.MapFrom(dto => dto.CreationDate));

        public IMappingExpression<CadQueryModel, CadQueryDTO> QueryToDTO() => CreateMap<CadQueryModel, CadQueryDTO>()
            .ForMember(dto => dto.TotalCount, opt => opt.MapFrom(query => query.TotalCount))
            .ForMember(dto => dto.Cads, opt => opt.MapFrom(query => query.Cads))
            ;

        public IMappingExpression<CadQueryDTO, CadQueryModel> DTOToQuery() => CreateMap<CadQueryDTO, CadQueryModel>()
            .ForMember(dto => dto.TotalCount, opt => opt.MapFrom(query => query.TotalCount))
            .ForMember(dto => dto.Cads, opt => opt.MapFrom(query => query.Cads));
    }
}
