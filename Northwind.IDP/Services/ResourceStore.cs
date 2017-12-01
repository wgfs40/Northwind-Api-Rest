
namespace Northwind.IDP.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IdentityServer4.Models;
    using IdentityServer4.EntityFramework.DbContexts;
    using System.Linq;
    using IdentityServer4.Stores;

    public class ResourceStore 
    {
        private ConfigurationDbContext _context;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;

        public ResourceStore(ConfigurationDbContext context, IClientStore clientStore, IResourceStore resourceStore)
        {
            this._context = context;
            this._clientStore = clientStore;
            this._resourceStore = resourceStore;
        }

        public async void GetallResource()
        {
            var listResources = await _resourceStore.GetAllResourcesAsync();

        }
    }
}
