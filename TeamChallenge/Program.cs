using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamChallenge.DbContext;
using TeamChallenge.Filters;
using TeamChallenge.Logic;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.SendEmailModels;
using TeamChallenge.Models.Static_data;
using TeamChallenge.Repositories;
using TeamChallenge.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SenderModel>(builder.Configuration.GetSection("Sender"));
builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter just your valid JWT token.\n\nExample: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

SetupFilters(builder.Services);
SetupCustomServices(builder.Services);
SetupDatabase(builder.Services, builder.Configuration);
SetupAuthentication(builder.Services, builder.Configuration);
SetupInMemoryStorage(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000 ",
                "https://TO-BE-DONE.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { GlobalConstants.Roles.Admin, GlobalConstants.Roles.Member, GlobalConstants.Roles.Unauthorized };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

//Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamChallenge API v1");
    options.RoutePrefix = "swagger";
});

GlobalConstants.ClientUrl = app.Configuration["ClientUrl:Debug"]!;

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.Run();


#region Inner methods

void SetupFilters(IServiceCollection services)
{
    services.AddControllers(options =>
    {
        options.Filters.Add<ValidationFilter>();
    });
}

void SetupCustomServices(IServiceCollection services)
{
    services.AddSingleton<IGenerateToken, GenerateTokenService>();
    services.AddScoped<RepositoryFactory>();
    services.AddScoped<ICategoryLogic, CategoryLogic>();
    services.AddScoped<ISubCategoryLogic, SubCategoryLogic>();
    services.AddScoped<IProductLogic, ProductLogic>();
    services.AddScoped<IProductBundleLogic, ProductBundleLogic>();
    services.AddScoped<IReviewLogic, ReviewLogic>();
    services.AddScoped<IGoogleOAuth, GoogleOAuthService>();
    services.AddSingleton<IEmailSend, EmailSenderService>();
    services.AddScoped<ILoginService, LoginService>();
    services.AddScoped<ICartLogic, CartLogic>();
    services.AddScoped<ICartItemLogic, CartItemLogic>();
    services.AddScoped<IUserLogic, UserLogic>();
    services.AddScoped<ITokenReaderService, TokenReaderService>();
    services.AddScoped<ValidationFilter>();
}

void SetupAuthentication(IServiceCollection services, IConfiguration config)
{
    services.AddAuthentication(x =>
    {
        x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
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
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role,

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

    builder.Services.AddAuthorization();
}

void SetupDatabase(IServiceCollection services, IConfiguration config)
{
    builder.Services.AddDbContext<CosmeticStoreDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
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
}

void SetupInMemoryStorage(IServiceCollection services, IConfiguration config)
{
    builder.Services.AddDistributedMemoryCache();

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });
}

#endregion Inner methods
