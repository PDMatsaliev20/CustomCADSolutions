using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Cads.GetCad;
using CustomCADs.API.Endpoints.Cads.GetCads;
using CustomCADs.API.Endpoints.Cads.PostCad;
using CustomCADs.API.Endpoints.Cads.RecentCads;
using CustomCADs.API.Endpoints.Designer.UncheckedCad;
using CustomCADs.API.Endpoints.Designer.UncheckedCads;
using CustomCADs.API.Endpoints.Home.Gallery;
using CustomCADs.API.Endpoints.Home.GalleryDetails;
using CustomCADs.Application.Models.Cads;
using Mapster;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Mappers
{
    public class CadsMapper
    {
        private static readonly Func<CadModel, string> mapCreationDate = m => m.CreationDate.ToString(DateFormatString);
        private static readonly Func<CadModel, string> mapCreatorName = m => m.Creator.UserName;
        private static readonly Func<CadModel, string> mapStatus = m => m.Status.ToString();
        private static readonly Func<CadModel, int> mapOrdersCount = m => m.Orders.Count;
        private static readonly Func<CadModel, string> mapCadPath = m => m.Paths.FilePath;
        private static readonly Func<CadModel, string> mapImagePath = m => m.Paths.ImagePath;
        private static readonly Func<CadModel, CoordinatesDto> mapCamCoordinates = m => new(m.CamCoordinates.X, m.CamCoordinates.Y, m.CamCoordinates.Z);
        private static readonly Func<CadModel, CoordinatesDto> mapPanCoordinates = m => new(m.PanCoordinates.X, m.PanCoordinates.Y, m.PanCoordinates.Z);
        private static readonly Func<CadModel, CategoryDto> mapCategory = m => new(m.CategoryId, m.Category.Name);
        public static void Map()
        {
            TypeAdapterConfig<CadModel, GalleryDetailsResponse>.NewConfig()
                .Map(r => r.CadPath, m => mapCadPath(m))
                .Map(r => r.CreatorName, m => mapCreatorName(m))
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.CamCoordinates, m => mapCamCoordinates(m))
                .Map(r => r.PanCoordinates, m => mapPanCoordinates(m))
                .Map(r => r.Category, m => mapCategory(m));

            TypeAdapterConfig<CadModel, GalleryResponse>.NewConfig()
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.CreatorName, m => mapCreatorName(m))
                .Map(r => r.ImagePath, m => mapImagePath(m));

            TypeAdapterConfig<CadModel, RecentCadsResponse>.NewConfig()
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.Status, m => mapStatus(m))
                .Map(r => r.Category, m => mapCategory(m));

            TypeAdapterConfig<CadModel, PostCadResponse>.NewConfig()
                .Map(r => r.CadPath, m => mapCadPath(m))
                .Map(r => r.ImagePath, m => mapImagePath(m))
                .Map(r => r.CamCoordinates, m => mapCamCoordinates(m))
                .Map(r => r.PanCoordinates, m => mapPanCoordinates(m))
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.CreatorName, m => mapCreatorName(m))
                .Map(r => r.Status, m => mapStatus(m))
                .Map(r => r.Category, m => mapCategory(m));

            TypeAdapterConfig<CadModel, GetCadResponse>.NewConfig()
                .Map(r => r.CadPath, m => mapCadPath(m))
                .Map(r => r.CamCoordinates, m => mapCamCoordinates(m))
                .Map(r => r.PanCoordinates, m => mapPanCoordinates(m))
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.Category, m => mapCategory(m));

            TypeAdapterConfig<CadModel, GetCadsResponse>.NewConfig()
                .Map(r => r.ImagePath, m => mapImagePath(m))
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.CreatorName, m => mapCreatorName(m))
                .Map(r => r.Category, m => mapCategory(m));
            
            TypeAdapterConfig<CadModel, UncheckedCadsResponse>.NewConfig()
                .Map(r => r.ImagePath, m => mapImagePath(m))
                .Map(r => r.CreationDate, m => mapCreationDate(m))
                .Map(r => r.CreatorName, m => mapCreatorName(m));

            TypeAdapterConfig<CadModel, UncheckedCadResponse>.NewConfig()
                .Map(r => r.CadPath, m => mapCadPath(m))
                .Map(r => r.CamCoordinates, m => mapCamCoordinates(m))
                .Map(r => r.PanCoordinates, m => mapPanCoordinates(m));
        }
    }
}
