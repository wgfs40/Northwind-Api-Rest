using Northwind.Api.Entities;
using Northwind.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _customerPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                    {"CustomerID", new PropertyMappingValue(new List<string>(){ "CustomerID"})},
                    {"CompanyName", new PropertyMappingValue(new List<string>(){ "CompanyName"})},
                    { "ContactFull", new PropertyMappingValue(new List<string>(){ "ContactName","ContactTitle"})}
                };

        private IList<IPropertyMapping> PropertyMapping = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            PropertyMapping.Add(new PropertyMapping<CustomerDto,Customer>(_customerPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            //get matching mapping
            var matchingMapping = PropertyMapping.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot Find exact property mapping instance for {typeof(TSource)} of {typeof(TDestination)}");
        }
    }
}
