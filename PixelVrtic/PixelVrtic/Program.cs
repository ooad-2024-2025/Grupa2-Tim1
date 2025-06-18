using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Controllers;
using PixelVrtic.Data;
using PixelVrtic.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = false;
    options.LoginPath = "/Identity/Account/Login";

    // Make sure cookie is NOT persistent when RememberMe is false
    options.Events.OnSigningIn = context =>
    {
        if (!context.Properties.IsPersistent)
        {
            // Session cookie: no expiration date
            context.Properties.ExpiresUtc = null;
        }
        return Task.CompletedTask;
    };
});


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Korisnik>(options =>
options.SignIn.RequireConfirmedAccount = true)
 .AddRoles<IdentityRole>()
 .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IzvjestajGeneratorService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IzvjestajGeneratorService>());


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
