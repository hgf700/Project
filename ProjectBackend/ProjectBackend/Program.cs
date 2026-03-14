using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectBackend.DB;
using ProjectBackend.ExtraTools;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using ProjectBackend.Services.interfaces;
using ProjectBackend.Services.Redis;
using ProjectBackend.Services.Tmdb;
using StackExchange.Redis;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

//builder.Services.AddControllers();
//moze byc problem bo mam enum na liczby w rate
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var db = Environment.GetEnvironmentVariable("POSTGRES_DB");
var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

var connectionString =
    $"Host={host};Port=5432;Database={db};Username={user};Password={pass}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, NullEmailSender>();
builder.Services.AddHttpClient();



//Redis
//var tokenSource = new CancellationTokenSource();
//var token = tokenSource.Token;

//builder.Services.AddSingleton<IConnectionMultiplexer>(
//    ConnectionMultiplexer.Connect("localhost"));

//const string streamName = "telemetry";
//const string groupName = "avg";

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379");
    configuration.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped<INotificationsStore, NotificationsStore>();
builder.Services.AddScoped<TmdbSaveProductionCompaniesService>();
builder.Services.AddScoped<TmdbLoadPeopleRoleService>();
builder.Services.AddScoped<TmdbImportService>();
builder.Services.AddScoped<TmdbService>();
builder.Services.AddScoped<TmdbSeedGenresService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .WithOrigins("http://localhost:5000")
            .AllowAnyMethod() 
            .AllowAnyHeader() 
            .AllowCredentials()
    );
});

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

// Google OAuth z refresh tokenem
string googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
string googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
string JWT_SECRET = Environment.GetEnvironmentVariable("JWT_SECRET");

if (string.IsNullOrWhiteSpace(JWT_SECRET))
    throw new InvalidOperationException("JWT_SECRET is not set");

if (string.IsNullOrWhiteSpace(googleClientId))
    throw new InvalidOperationException("GOOGLE_CLIENT_ID is not set");

if (string.IsNullOrWhiteSpace(googleClientSecret))
    throw new InvalidOperationException("GOOGLE_CLIENT_SECRET is not set");

int zoauth = 0;

if (zoauth == 1)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JWT_SECRET)
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/signin-google";
        options.SaveTokens = true;
    });
}
else
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JWT_SECRET)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAngularDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();