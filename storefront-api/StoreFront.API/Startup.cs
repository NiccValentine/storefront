namespace StoreFront.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using StoreFront.Common.Interfaces.Repositories;
    using StoreFront.Common.Interfaces.Services;
    using StoreFront.Repository;
    using StoreFront.Service;
    using StoreFront.Common;
    using StoreFront.EF.Repository;
    using StoreFront.Common.Interfaces.Logging;
    using StoreFront.Common.Logging;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(cors => cors.AddPolicy("localHostPolicy", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // Repositories
            
            if (Settings.UsingEF)
            {
                services.AddScoped<IProductRepository, ProductRepositoryEF>();
                services.AddScoped<IStoreRepository, StoreRepositoryEF>();
                services.AddScoped<IStoreProductRepository, StoreProductRepositoryEF>();
            }
            else
            {
                services.AddScoped<IProductRepository, ProductRepositoryADO>();
                services.AddScoped<IStoreRepository, StoreRepositoryADO>();
                services.AddScoped<IStoreProductRepository, StoreProductRepositoryADO>();
            }

            // Services
            services.AddSingleton<ILogService, LogService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreProductService, StoreProductService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "StoreFront",
                        Version = "v1",
                        Description = "StoreFront API"
                    });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("localHostPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreFront API v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
