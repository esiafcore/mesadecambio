using Microsoft.EntityFrameworkCore;
using System;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Web.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery(x => x.SuppressXFrameOptionsHeader = true);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ,x => x.MigrationsAssembly("Xanes.DataAccess"));
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

//Content Security Policy
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Permissions-Policy", "camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), usb=()");
    //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    await next();
});

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
    if (context != null)
    {
        await SeedData.SeedDataDb(context);
    }
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
