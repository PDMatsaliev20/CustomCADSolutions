using AutoMapper;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data.Entities;
using CustomCADs.Infrastructure.Data.Identity;

namespace CustomCADs.Infrastructure.Data
{
    public class Mappings : Profile
    {
        public Mappings() 
        {
            AppUserToUser();
            UserToAppUser();
            PCategoryToCategory();
            CategoryToPCategory();
            POrderToOrder();
            OrderToPOrder();
            PCadToCad();
            CadToPCad();
        }

        private void AppUserToUser() => CreateMap<AppUser, User>();
        private void UserToAppUser() => CreateMap<User, AppUser>();

        private void PCategoryToCategory() => CreateMap<PCategory, Category>();
        private void CategoryToPCategory() => CreateMap<Category, PCategory>();
        
        private void POrderToOrder() => CreateMap<POrder, Order>();
        private void OrderToPOrder() => CreateMap<Order, POrder>();
        
        private void PCadToCad() => CreateMap<PCad, Cad>();
        private void CadToPCad() => CreateMap<Cad, PCad>();
    }
}
