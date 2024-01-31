using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Services;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICadService, CadService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddDbContext<CADContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddControllersWithViews();
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
}
).AddEntityFrameworkStores<CADContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
