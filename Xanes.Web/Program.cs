using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Utility;
using Xanes.Web.CustomMiddleware;
using Xanes.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureLoggerService();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureHttpClientService();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var userId = builder.Configuration[AC.SecretUserId];
var userPwd = builder.Configuration[AC.SecretUserPwd];

connectionString = new SqlConnectionStringBuilder(connectionString)
{
    UserID = userId,
    Password = userPwd
}.ConnectionString;

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(connectionString
        , x => x.MigrationsAssembly("Xanes.DataAccess"));
    //options.UseLowerCaseNamingConvention();
});

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();
app.UseMiddleware<ContentSecurityPolicyMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.Environment.IsProduction()
    //app.Environment.IsStaging()

    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    //if (context != null)
    //{
    //    await SeedData.SeedDataDb(context);
    //}
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Privacy}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Exchange}/{controller=Quotation}/{action=Index}/{id?}");

app.Run();
