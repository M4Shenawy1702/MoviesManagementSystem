using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Errors
{
    public class NotFoundException(string Message) : ServiceException(StatusCodes.Status404NotFound, Message)
    {
    }
}
