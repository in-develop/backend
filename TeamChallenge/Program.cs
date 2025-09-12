using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamChallenge.StaticData;
using TeamChallenge.DbContext;
using TeamChallenge.Filters;
using TeamChallenge.Logic;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.SendEmailModels;
using TeamChallenge.Repositories;
using TeamChallenge.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Host.UseSerilog((context, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGenerateToken, GenerateTokenService>();
builder.Services.AddScoped<RepositoryFactory>();
builder.Services.AddScoped<ICategoryLogic, CategoryLogic>();
builder.Services.AddScoped<IProductLogic, ProductLogic>();
builder.Services.AddScoped<IProductBundleLogic, ProductBundleLogic>();
builder.Services.AddScoped<IReviewLogic, ReviewLogic>();
builder.Services.AddScoped<IGoogleOAuth, GoogleOAuthService>();
builder.Services.AddSingleton<IEmailSend, EmailSenderService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICartLogic, CartLogic>();
builder.Services.AddScoped<ICartItemLogic, CartItemLogic>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<ValidationFilter>();
var sender = builder.Services.Configure<SenderModel>(builder.Configuration.GetSection("Sender"));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<CosmeticStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddIdentity<UserEntity, IdentityRole>(
    opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.SignIn.RequireConfirmedEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequiredLength = 1;
        opt.Password.RequireDigit = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CosmeticStoreDbContext>()
    .AddApiEndpoints();


builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(cfg => cfg.SlidingExpiration = true)
    .AddGoogle(googleOptions =>
    {
        var clientId = googleOptions.ClientId = config["Authentication:Google:ClientId"]!;
        if (clientId == null)
        {
            throw new ArgumentNullException(nameof(clientId));
        }

        var clientSecret = googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"]!;
        if (clientSecret == null)
        {
            throw new ArgumentNullException(nameof(clientSecret));
        }
        googleOptions.ClientId = clientId;
        googleOptions.ClientSecret = clientSecret;
        googleOptions.CallbackPath = "/signin-google";
        googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        googleOptions.Events.OnTicketReceived = async ctx =>
        {
            var identity = (ClaimsIdentity)ctx.Principal!.Identity!;
            var email = identity.FindFirst(ClaimTypes.Email);
            var name = identity.FindFirst(ClaimTypes.Name)?.Value;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, name ?? string.Empty),
                //new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId ?? string.Empty),
                // new Claim("CartId", cartId.ToString()) // Uncomment if you have cartId
            };
            identity.AddClaims(claims);
        };

    })
    .AddJwtBearer(jwtOptions =>
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RoleClaimType = ClaimTypes.Role
            };

            jwtOptions.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Authentication failed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully!");
                    return Task.CompletedTask;
                }
            };
        });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Member", "Unauthorized" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

//Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    BaseClass.ClientUrl = config["ClientUrl:Debug"]!;
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();

app.UseSession();
app.UseHttpsRedirection();
app.MapIdentityApi<IdentityUser>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
