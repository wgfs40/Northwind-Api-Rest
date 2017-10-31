using Northwind.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Api.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            //the orderby string is separated by "," so we split it.
            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                //trim the orderByClause, as it might contain leading,
                //or trailing space, Can't trim the var in foreach
                //so use another var
                var trimmedOrderByClause = orderByClause.Trim();

                //if sort options ends with with " desc" from OrderByClause, so we
                //get the property name to look for in the mapping dictionary
                var orderByDesending = trimmedOrderByClause.EndsWith(" desc");

                //remove " asc" or " desc" from orderByClause, so we 
                // get the property name to look for the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);


                //find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                // get the PropertyMappingvalue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                // run through the property names is reverse 
                // so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    
                    // revert sort order if necesary
                    if (propertyMappingValue.Revert)
                    {
                        orderByDesending = !orderByDesending;
                    }
                    
                    //source = source.OrderBy(destinationProperty + (orderByDesending? " descending":" ascending"));

                }
            }
            return source;
        }
    }
}
