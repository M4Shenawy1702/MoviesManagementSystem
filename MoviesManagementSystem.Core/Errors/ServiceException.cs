using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Errors
{
    public class ServiceException : Exception
    {
        public int StatusCode { get; }

        public ServiceException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
