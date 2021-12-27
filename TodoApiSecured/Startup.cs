using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using ToreadApi.Models;

namespace ToreadApi
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

            services.AddControllers();
            
            services.AddDbContext<ToreadContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ToreadApiContext"));
                options.EnableSensitiveDataLogging();
            });

            //add swagger generator
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ToreadApi",
                    Version = "v1",
                    Description = "Moje API do obsługi zadań",
                    Contact = new OpenApiContact { Name = "RT", Email = "r@rmail.com" },
                    License = new OpenApiLicense { Name = "Github", Url = new System.Uri("http://github.com/rt/license") }
                });               
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToreadApi v1"));
            }

            app.UseHttpsRedirection();  //demo - outside Swagger ui
            app.UseRouting();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        }
    }
}
