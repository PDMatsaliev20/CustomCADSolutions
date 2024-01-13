using CustomCADSolutions.App.Import;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Services;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

CADContext context = new();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
string json = File.ReadAllText("C:\\Users\\35988\\source\\repos\\WebApps\\CustomCADSolutions\\CustomCADSolutions.App\\Import\\categories.json");
Deserializer.ImportCategories(context, json);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICADService, CADService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDeepAIService, DeepAIService>();

builder.Services.AddDbContext<CADContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddControllersWithViews();
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<CustomCADSolutionsContext>();

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
