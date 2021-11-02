using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyBookStore.Repository.Context;

namespace TrickyBookStore.Services
{
    public class BaseService
    {
        protected readonly IBookContext _context;

        protected BaseService(IBookContext context)
        {
            _context = context;
        }
    }
}
