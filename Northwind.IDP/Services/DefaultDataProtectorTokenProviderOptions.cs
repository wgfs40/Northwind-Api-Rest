namespace Northwind.IDP.Services
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    public class DefaultDataProtectorTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }

    public class DefaultDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public DefaultDataProtectorTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DefaultDataProtectorTokenProviderOptions> options)
            :base(dataProtectionProvider, options)
        {

        }
    }
}
