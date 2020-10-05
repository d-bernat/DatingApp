using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace API
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
            SetupHealthCheck(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            SetupHealthCheckOption(app);
        }

        private void SetupHealthCheck(IServiceCollection services)
        {
            services.AddHealthChecks().AddSqlServer(Configuration["ConnectionStrings:AppDatabase"], name: "Dating App Database");
        }

        private static void SetupHealthCheckOption(IApplicationBuilder app)
        {
            var options = new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(
                        new { status = report.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() }) });
                    await context.Response.WriteAsync(result);
                }
            };

            app.UseHealthChecks("/monitoring/health", options);
        }
    }
}
