using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Services
{
    public interface ITypeHeperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
