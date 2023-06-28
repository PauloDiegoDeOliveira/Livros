using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Livros.API.Configuration
{
    public static class HealthChecksConfig
    {
        public static void AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                 .AddSqlServer(configuration.GetConnectionString("Connection"),
                   name: "sqlserver", tags: new string[] { "db", "data" });
        }

        public static void UseHealthChecksConfiguration(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/status",
                  new HealthCheckOptions()
                  {
                      ResponseWriter = async (context, report) =>
                      {
                          string result = JsonConvert.SerializeObject(
                              new
                              {
                                  currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  statusApplication = report.Status.ToString(),
                                  healthChecks = report.Entries.Select(e => new
                                  {
                                      check = e.Key,
                                      ErrorMessage = e.Value.Exception?.Message,
                                      status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                  })
                              });
                          context.Response.ContentType = MediaTypeNames.Application.Json;
                          await context.Response.WriteAsync(result);
                      }
                  });
        }
    }
}