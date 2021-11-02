using System;
using System.Collections.Generic;
using System.Linq;
using TrickyBookStore.Models;
using TrickyBookStore.Repository.Context;
using TrickyBookStore.Services.Categories;

namespace TrickyBookStore.Services.Books
{
    public class BookService : BaseService, IBookService
    {
        private ICategoryService _categoryService;

        public BookService(IBookContext context, ICategoryService categoryService):base(context)
        {
            _categoryService = categoryService;
        }
        public IList<Book> GetBooks(params long[] ids)
        {
            var books = this._context.BooksData().Where(book => ids.Contains(book.Id)).ToList();
            foreach(var book in books)
            {
                book.Category = _categoryService.GetCategoryById(book.CategoryId);
            }
            return books;
        }
    }
}
