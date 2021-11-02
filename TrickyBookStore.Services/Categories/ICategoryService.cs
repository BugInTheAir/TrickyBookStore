using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyBookStore.Models;

namespace TrickyBookStore.Services.Categories
{
    public interface ICategoryService
    {
        BookCategory GetCategoryById(int categoryId);
    }
}
