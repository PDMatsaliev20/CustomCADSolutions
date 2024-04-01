using AutoMapper;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using System.Drawing;

namespace CustomCADSolutions.Core.Mappings
{
    public class CadProfile : Profile
    {
        public CadProfile()
        {
            ModelToEntity();
            EntityToModel();
        }

        public IMappingExpression<Cad, CadModel> ModelToEntity() => CreateMap<Cad, CadModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(m => m.Bytes, opt => opt.MapFrom(c => c.Bytes))
                .ForMember(m => m.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(m => m.IsValidated, opt => opt.MapFrom(c => c.IsValidated))
                .ForMember(m => m.CreationDate, opt => opt.MapFrom(c => c.CreationDate))
                .ForMember(m => m.Coords, opt => opt.MapFrom(c => new Coords(c.X, c.Y, c.Z)))
                .ForMember(m => m.Color, opt => opt.MapFrom(c => Color.FromArgb(1, c.R, c.G, c.B)))
                .ForMember(m => m.SpinAxis, opt => opt.MapFrom(c => c.SpinAxis))
                .ForMember(m => m.CategoryId, opt => opt.MapFrom(c => c.CategoryId))
                .ForMember(m => m.CreatorId, opt => opt.MapFrom(c => c.CreatorId))
                .ForMember(m => m.Category, opt => opt.MapFrom(c => c.Category))
                .ForMember(m => m.Creator, opt => opt.MapFrom(c => c.Creator))
                .ForMember(m => m.Orders, opt => opt.MapFrom(c => c.Orders.ToArray()));

        public IMappingExpression<CadModel, Cad> EntityToModel() => CreateMap<CadModel, Cad>()
                .ForMember(c => c.Id, opt => opt.MapFrom(m => m.Id))
                .ForMember(c => c.Bytes, opt => opt.MapFrom(m => m.Bytes))
                .ForMember(c => c.Name, opt => opt.MapFrom(m => m.Name))
                .ForMember(c => c.IsValidated, opt => opt.MapFrom(m => m.IsValidated))
                .ForMember(c => c.CreationDate, opt => opt.MapFrom(m => m.CreationDate))
                .ForMember(c => c.X, opt => opt.MapFrom(m => m.Coords.Item1))
                .ForMember(c => c.Y, opt => opt.MapFrom(m => m.Coords.Item2))
                .ForMember(c => c.Z, opt => opt.MapFrom(m => m.Coords.Item3))
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
