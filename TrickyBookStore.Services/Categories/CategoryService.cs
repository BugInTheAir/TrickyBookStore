using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyBookStore.Models;
using TrickyBookStore.Repository.Context;

namespace TrickyBookStore.Services.Categories
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IBookContext context) : base(context)
        {

        }
        public BookCategory GetCategoryById(int categoryId)
        {
            return this._context.CategoriesData().Where(category => category.Id.Equals(categoryId)).FirstOrDefault();
        }
    }
}
