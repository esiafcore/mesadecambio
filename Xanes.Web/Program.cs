using Microsoft.EntityFrameworkCore;
using System;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository;
using Xanes.DataAccess.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ,x => x.MigrationsAssembly("Xanes.DataAccess"));
    //options.UseLowerCaseNamingConvention();
});

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.Environment.IsProduction()
    //app.Environment.IsStaging()

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    if (context != null)
    {
        await SeedData.SeedDataDb(context);
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Privacy}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Exchange}/{controller=Home}/{action=Index}/{id?}");

app.Run();
