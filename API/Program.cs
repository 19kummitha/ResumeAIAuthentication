using System.Text;
using API.Data;
using API.TokenManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    var Connectionstring = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(Connectionstring);
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTAuth:Secret"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully.");
            return Task.CompletedTask;
        }
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseAuthorization();




app.MapControllers();

app.Run();