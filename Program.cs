using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Municipality.Data;
using Municipality.Data.Repository;
using Municipality.Data.Repository.IRepository;
using NuGet.Protocol.Core.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var connectionString = builder.Configuration.GetConnectionString("dbcon");
builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

//BU KISIM USER ÝÇÝN YENÝ EKLENDÝ
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
    option.LoginPath = "/Admin/Account/Login";
    option.AccessDeniedPath = "/Admin/Account/Login";
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(5);
    //içerigi terk edilmeden önce nekadar süredir boþta kalabilecegini gösterir

    option.Cookie.HttpOnly = true;
    //httponly sayesinde javascript kodlarýnýn cookie bilgisini okumasýna izin vermez güvenlik için önemlidir.

    option.Cookie.IsEssential = true;
    //bu özellik çerez uygulamasý çalýþmasýný saglamak için kullanýlýr

});

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",

      pattern: "{area=Admin}/{controller=User}/{action=Index}/{id?}");

//pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
app.Run();
