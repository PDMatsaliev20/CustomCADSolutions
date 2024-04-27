using AutoMapper;
using CustomCADSolutions.App.Models.Cads.Input;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Mappings
{
    public class CadAppProfile : Profile
    {
        public CadAppProfile()
        {
            AddToModel();
            EditToModel();
            ModelToView();
            ModelToEdit();
        }

        public void AddToModel() => CreateMap<CadAddModel, CadModel>();
        
        public void EditToModel() => CreateMap<CadEditModel, CadModel>()
            .ForMember(model => model.Coords, opt => opt.MapFrom(input => new[] { input.X, input.Y, input.Z }));

        public void ModelToView() => CreateMap<CadModel, CadViewModel>()
            .ForMember(view => view.RGB, opt => opt.MapFrom(model => new[] { model.Color.R, model.Color.G, model.Color.B }))
            .ForMember(view => view.Category, opt => opt.MapFrom(model => model.Category.Name))
            .ForMember(view => view.CreatorName, opt => opt.MapFrom(model => model.Creator.UserName))
            .ForMember(view => view.CreationDate, opt => opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")));

        public void ModelToEdit() => CreateMap<CadModel, CadEditModel>()
            .ForMember(input => input.X, opt => opt.MapFrom(model => model.Coords[0]))
            .ForMember(input => input.Y, opt => opt.MapFrom(model => model.Coords[1]))
            .ForMember(input => input.Z, opt => opt.MapFrom(model => model.Coords[2]));
    }
}
