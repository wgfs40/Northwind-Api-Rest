using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Northwind.Api.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //our binder works only on enumerable types
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            //Get the inputted value throught the value provider 
            var value = bindingContext.ValueProvider
                        .GetValue(bindingContext.ModelName).ToString();

            //if that value is null or whitespace, we return null
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            //the value isn't null whitespace
            //and the type of the model is enumerable 
            //get enumerable's type, and converter
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            //Convert each time in the value list to the enumerable type
            var values = value.Split(new[] { ","},StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();

            //create an array of tha type, and set it as the Model value
            var typeValues = Array.CreateInstance(elementType,values.Length);
            values.CopyTo(typeValues,0);
            bindingContext.Model = typeValues;

            // return a successul result, passing in the Model
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
