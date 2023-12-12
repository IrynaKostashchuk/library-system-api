using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot();

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetSection("Auth:Authority").Get<string>();
        options.Audience = "Gateway";
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });*/



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "DriverAPI", Version = "v1" });

    // Добавляем Security Definition для OAuth 2.0 с Authorization Code Grant
    /*opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration.GetSection("Auth:Swagger:AuthorizationUrl").Get<string>()),
                TokenUrl = new Uri(builder.Configuration.GetSection("Auth:Swagger:TokenUrl").Get<string>()),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID scope" },
                    { "profile", "Profile scope" },
                    { "DriverApi.read", "Read access to Driver API" },
                    { "DriverApi.write", "Write access to Driver API" }
                }
            }
        }
    });

    // Добавляем Security Requirement для указания использования OAuth 2.0
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "openid", "profile", "DriverApi.read", "DriverApi.write" }
        }
    });*/
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    //options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway");
    options.OAuthUsePkce();
});

app.UseOcelot().Wait();

app.Run();