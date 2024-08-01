using AutoMapper;
using CustomCADs.App.Models.Cads.Input;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.Core.Models.Cads;

namespace CustomCADs.App.Mappings
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
        
        public void EditToModel() => CreateMap<CadEditModel, CadModel>();

        public void ModelToView() => CreateMap<CadModel, CadViewModel>()
            .ForMember(view => view.Category, opt => opt.MapFrom(model => model.Category.Name))
            .ForMember(view => view.CreatorName, opt => opt.MapFrom(model => model.Creator.UserName))
            .ForMember(view => view.CreationDate, opt => opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")));

        public void ModelToEdit() => CreateMap<CadModel, CadEditModel>();
    }
}
