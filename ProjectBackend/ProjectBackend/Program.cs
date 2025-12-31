using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.ExtraTools;
using ProjectBackend.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var db = Environment.GetEnvironmentVariable("POSTGRES_DB");
var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

var connectionString =
    $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, NullEmailSender>();
builder.Services.AddHttpClient();

builder.Services.AddAuthorization();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = false;

    options.User.RequireUniqueEmail = false;
});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.Cookie.SameSite = SameSiteMode.Lax;
//    options.ExpireTimeSpan = TimeSpan.FromHours(1); // D³u¿szy czas ¿ycia sesji
//    options.SlidingExpiration = true;
//    options.LoginPath = "/Identity/Account/Login";
//    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
//    options.LogoutPath = "/Identity/Account/Logout";
//});

// Google OAuth z refresh tokenem
//string googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
//string googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(options =>
//{
//    options.ClientId = googleClientId;
//    options.ClientSecret = googleClientSecret;
//    options.CallbackPath = "/signin-google";
//    options.AccessType = "offline";
//    options.SaveTokens = true;
//});

var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
//    context.Response.Headers["Pragma"] = "no-cache";
//    context.Response.Headers["Expires"] = "-1";
//    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
//    context.Response.Headers["X-Frame-Options"] = "DENY";
//    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

//    await next.Invoke();
//});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
