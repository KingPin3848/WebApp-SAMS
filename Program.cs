using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Services;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Service Addition for Progressive Web App
builder.Services.AddMvc();
builder.Services.AddProgressiveWebApp();

//Account Confirmed Service
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

//Google Authentication Service
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
   {
       IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
       googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
       googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
   });

//Attendance Generation Service
//builder.Services.AddHostedService<DefaultAttendanceAdditionService>();

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
