using System.Collections.Generic;
using TrickyBookStore.Models;
using TrickyBookStore.Services;

namespace TrickyBookStore.Repository.Context
{
    public interface IBookContext 
    {
        IList<BookCategory> CategoriesData();
        IList<Book> BooksData();
        IList<Customer> CustomersData();
        IList<PurchaseTransaction> PurchaseTransactionsData();
        IList<Subscription> SubscriptionsData();
    }
}
