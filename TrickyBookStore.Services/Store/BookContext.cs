using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyBookStore.Models;
using TrickyBookStore.Services.Store;

namespace TrickyBookStore.Repository.Context
{
    public class BookContext : IBookContext
    {
        public IList<Book> BooksData()
        {
            return Books.Data.ToList();
        }

        public IList<BookCategory> CategoriesData()
        {
            return BookCategories.Data.ToList();
        }

        public IList<Customer> CustomersData()
        {
            return Customers.Data.ToList();
        }

        public IList<PurchaseTransaction> PurchaseTransactionsData()
        {
            return PurchaseTransactions.Data.ToList();
        }

        public IList<Subscription> SubscriptionsData()
        {
            return Subscriptions.Data.ToList();
        }
    }
}
