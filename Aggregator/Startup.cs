using System.Reflection;
using Aggregator.Services;
using Microsoft.OpenApi.Models;

namespace Aggregator;

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
        // Bypass SSL certificate validation (not recommended for production)
        System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
        {
            if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
            {
                Console.WriteLine($"SSL Certificate Error: {sslPolicyErrors}");

                foreach (var status in chain.ChainStatus)
                {
                    Console.WriteLine($"Chain Status: {status.Status} - {status.StatusInformation}");
                }
            }

            return true; // Bypass SSL certificate validation
        };
        
        services.AddHttpClient<IReadingListService, ReadingListService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:ReadingListUrl"]));

        services.AddHttpClient<IReviewService, ReviewService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:ReviewUrl"]));

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aggregator", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aggregator v1"));
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}