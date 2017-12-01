namespace Northwind.IDP.Services
{
    using IdentityServer4.EntityFramework.Entities;
    using IdentityServer4.Stores;

    public interface IResourceStoreNorthwind : IResourceStore
    {
        IdentityResource GetIdentity();
    }
}
