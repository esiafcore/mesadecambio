using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Utility;
using Xanes.Web.CustomMiddleware;
using Xanes.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add srvs to the container.
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

//Configurar la Autenticación
builder.Services.AddDistributedMemoryCache();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "AspNetCore.Session";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

//Configurar la Autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Auth/Auth/Login";
        options.AccessDeniedPath = "/Auth/Auth/AccessDenied";
        options.SlidingExpiration = true;
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

//// forward headers configuration for reverse proxy
//builder.Services.Configure<ForwardedHeadersOptions>(options => {
//    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//    options.KnownNetworks.Clear();
//    options.KnownProxies.Clear();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ContentSecurityPolicyMiddleware>();

    //app.Environment.IsProduction()
    //app.Environment.IsStaging()

    app.UseExceptionHandler("/Home/Error");
    //app.UseForwardedHeaders();
    app.UseHsts();
}
else
{
    app.UseMiddleware<ContentSecurityPolicyMiddlewareDevelopment>();
    using var scope = app.Services.CreateScope();
    await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    //if (context != null)
    //{
    //    await SeedData.SeedDataDb(context);
    //}
    //app.UseForwardedHeaders();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();


//app.MapControllerRoute(1
//    name: "default",
//    pattern: "{controller=Home}/{action=Privacy}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Auth}/{controller=Auth}/{action=Login}/{id?}");

app.Run();
