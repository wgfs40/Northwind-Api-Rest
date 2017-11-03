﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Northwind.Api.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject>ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            //create a list to hold our ExpandoObject
            var expandoObjectList = new List<ExpandoObject>();

            //create a list with propertyInfo objects  on TSource. Reflection is 
            // expensive, so rather than doing it for each object in the list, we do 
            // it once and reuse the results. After all, part of the Reflection is on the
            // type of object (TSource), not on the instance
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                //only the public properties that match the fields should be
                //in the ExpandoObject 

                // the field are separed by ",", so we split it.
                var fieldAfterSplit = fields.Split(',');

                foreach (var field in fieldAfterSplit)
                {
                    //thim each field, as it might contain leading
                    //or trailing spaces, Can't trim the var in foreach
                    //so use another var.
                    var propertyName = field.Trim();

                    // use reflection to get the property on the source object
                    // we need to include public instance, b/c specifying a binding flag overwrites the 
                    // already-existing biding flags.
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    }

                    // add propertyInfo to list
                    propertyInfoList.Add(propertyInfo);
                }
            }

            //run throught the source objects
            foreach (TSource sourceObject in source)
            {
                // create the ExpandoObject tha will hold the
                // selected properties & values
                var dataShapedObject = new ExpandoObject();

                //Get the value of each property we have to return. For that
                // we run through the list
                foreach (var propertyInfo in propertyInfoList)
                {
                    // get value return the value of property on the source object
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    // add the field to the expandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name,propertyValue);
                }

                // add the ExpandoObject to be list
                expandoObjectList.Add(dataShapedObject);
            }

            // return the list
            return expandoObjectList;
        }
    }
}
