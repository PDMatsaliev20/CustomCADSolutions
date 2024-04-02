using AutoMapper;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Mappings
{
    /// <summary>
    /// Covers InputModel-ImportDTO, ImportDTO-Model, Model-ExportDTO and ExportDTO-ViewModel
    /// </summary>
    public class CadModelProfile : Profile
    {
        public CadModelProfile()
        {
            InputToDTO();
            DTOToModel();
            ModelToDTO();
            DTOToView();
        }

        /// <summary>
        /// Converts User Cad to JSON Import (set the bytes manually)
        /// </summary>
        /// <returns>IMappingExpression with Input source and DTO destination</returns>
        public IMappingExpression<CadInputModel, CadImportDTO> InputToDTO() => CreateMap<CadInputModel, CadImportDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(input => input.Id))
            .ForMember(dto => dto.Name, opt => opt.MapFrom(input => input.Name))
            .ForMember(dto => dto.Coords, opt => opt.MapFrom(input => new Coords(input.X, input.Y, input.Z)))
            .ForMember(dto => dto.SpinAxis, opt => opt.MapFrom(input => input.SpinAxis))
            .ForMember(dto => dto.IsValidated, opt => opt.MapFrom(input => input.IsValidated));

        /// <summary>
        /// Converts JSON Import to Service Model
        /// </summary>
        /// <returns>IMappingExpression with DTO source and Model destination</returns>
        public IMappingExpression<CadImportDTO, CadModel> DTOToModel() => CreateMap<CadImportDTO, CadModel>()
            .ForMember(cad => cad.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(cad => cad.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(cad => cad.CategoryId, opt => opt.MapFrom(dto => dto.CategoryId))
            .ForMember(cad => cad.Coords, opt => opt.MapFrom(dto => dto.Coords))
            .ForMember(cad => cad.SpinAxis, opt => opt.MapFrom(dto => dto.SpinAxis))
            .ForMember(cad => cad.IsValidated, opt => opt.MapFrom(dto => dto.IsValidated));

        /// <summary>
        /// Converts Service Model to JSON Export
        /// </summary>
        /// <returns>IMappingExpression with Model source and DTO destination</returns>
        public IMappingExpression<CadModel, CadExportDTO> ModelToDTO() => CreateMap<CadModel, CadExportDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
            .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(model => model.Category.Name))
            .ForMember(dto => dto.Coords, opt => opt.MapFrom(model => model.Coords))
            .ForMember(dto => dto.SpinAxis, opt => opt.MapFrom(model => model.SpinAxis))
            .ForMember(dto => dto.IsValidated, opt => opt.MapFrom(model => model.IsValidated))
            .ForMember(dto => dto.RGB, opt => opt.MapFrom(model => model.Color.GetColorBytes()))
            .ForMember(dto => dto.CreatorName, opt =>
            {
                opt.AllowNull();
                opt.MapFrom(model => model.Creator != null ? model.Creator.UserName : null);
            })
            .ForMember(dto => dto.CreationDate, opt =>
            {
                opt.AllowNull();
                opt.MapFrom(model => model.CreationDate != null ? model.CreationDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);
            });

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
            .ForMember(view => view.IsValidated, opt => opt.MapFrom(dto => dto.IsValidated))
            .ForMember(view => view.R, opt => opt.MapFrom(dto => dto.RGB[0]))
            .ForMember(view => view.G, opt => opt.MapFrom(dto => dto.RGB[1]))
            .ForMember(view => view.B, opt => opt.MapFrom(dto => dto.RGB[2]))
            .ForMember(view => view.CreatorName, opt =>
            {
                opt.AllowNull();
                opt.MapFrom(dto => dto.CreatorName);
            })
            .ForMember(view => view.CreationDate, opt =>
            {
                opt.AllowNull();
                opt.MapFrom(dto => dto.CreationDate);
            });
    }
}
