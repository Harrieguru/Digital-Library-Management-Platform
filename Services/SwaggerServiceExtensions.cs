using Microsoft.OpenApi.Models;

namespace eBook_manager.Services
{
    public static class SwaggerServiceExtensions
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eBook_manager", Version = "v1" });

            });
        }
    }
}
