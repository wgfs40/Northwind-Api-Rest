using Microsoft.Extensions.DependencyInjection;
using Northwind.IDP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.IDP
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddNorthwindUserStore(this IIdentityServerBuilder builder)
        {
            //builder.Services.AddSingleton<INorthwindUserRepository, NorthwindUserRepository>();
            builder.AddProfileService<NorthwindUserProfileService>();
            return builder;
        }
    }
}
