using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Identity
{
    public class IdentityContext(DbContextOptions<IdentityContext> options) 
        : IdentityDbContext<AppUser, AppRole, string>(options) { }
}
