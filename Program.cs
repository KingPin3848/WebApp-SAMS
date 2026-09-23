using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using SAMS;
using SAMS.Data;
using SAMS.Middleware;
using SAMS.Services;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();

// Service addition for Progressive Web App
services.AddMvc();
services.AddProgressiveWebApp();

// Account confirmed service
services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
services.AddDataProtection();
services.AddSession();
services.AddControllersWithViews();
services.AddProblemDetails();
services.AddExceptionHandler<GlobalExceptionHandler>();

// Google authentication service
services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"]!;
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
    googleOptions.Scope.Add("email");
});

// Authentication and Authorization Requirement Policy for all pages
//builder.Services.AddAuthorizationBuilder().AddFallbackPolicy("Login Required", options => options.RequireAuthenticatedUser().Build());

//Attendance Generation Service
//builder.Services.AddHostedService<DailyAttendanceAdditionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "myArea",
    pattern: "{area:exists}/{controller}/{action}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.MapRazorPages();

app.Run();
