using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Helpers
{
    public class UnProcessableEntityObjectResult : ObjectResult
    {
        public UnProcessableEntityObjectResult(ModelStateDictionary modelState):
            base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new Exception(nameof(modelState));
            }

            StatusCode = 422;
        }
    }
}
