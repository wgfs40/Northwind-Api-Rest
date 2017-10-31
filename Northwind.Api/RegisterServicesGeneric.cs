using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Api.Services;

namespace Northwind.Api
{
    public static class RegisterServicesGeneric
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<INorthwindRepository, NorthwindRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory => {
                var actionContext =
                    implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<ITypeHeperService, TypeHeperService>();
        }
    }
}
