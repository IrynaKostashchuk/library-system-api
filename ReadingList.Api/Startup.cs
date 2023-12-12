
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using ReadingList.Api.Repositories;

namespace ReadingList.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            // General Configuration
            services.AddScoped<IReadingListRepository, ReadingListRepository>();
            //services.AddAutoMapper(typeof(Startup));




            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetSection("Auth:Authority").Get<string>();
                    options.Audience = "ReadingList.Api";
                    options.TokenValidationParameters.ValidTypes = new[] {"at+jwt"};
                });
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo {Title = "ReadingListAPI", Version = "v1"});

                // Добавляем Security Definition для OAuth 2.0 с Authorization Code Grant
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl =
                                new Uri(Configuration.GetSection("Auth:Swagger:AuthorizationUrl").Get<string>()),
                            TokenUrl = new Uri(Configuration.GetSection("Auth:Swagger:TokenUrl").Get<string>()),
                            Scopes = new Dictionary<string, string>
                            {
                                {"openid", "OpenID scope"},
                                {"profile", "Profile scope"},
                                {"ReadingListAPI.read", "Read access to ReadingList API"},
                                {"ReadingListAPI.write", "Write access to ReadingList API"}
                            }
                        }
                    }
                });

                
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
                        new[] {"openid", "profile", "ReadingListAPI.read", "ReadingListAPI.write"}
                    }
                });
            });

            // // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            // {
            //     if (env.IsDevelopment())
            //     {
            //         app.UseDeveloperExceptionPage();
            //         app.UseSwagger();
            //         app.UseSwaggerUI(c =>
            //         {
            //             c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadingList.API v1");
            //             c.OAuthUsePkce();
            //         });
            //     }


                // app.UseRouting();
                //
                // app.UseAuthorization();
                //
                // app.UseEndpoints(endpoints =>
                // {
                //     endpoints.MapControllers(); // This line is responsible for mapping controllers
                // });

            }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                 app.UseSwagger();
                 app.UseSwaggerUI(c =>
                 {
                     c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadingList.API v1");
                     c.OAuthUsePkce();
                });
            }


         app.UseRouting();
        
         app.UseAuthorization();
        
         app.UseEndpoints(endpoints =>
         {
             endpoints.MapControllers(); // This line is responsible for mapping controllers
         });

    }
            
        }
    
    }

