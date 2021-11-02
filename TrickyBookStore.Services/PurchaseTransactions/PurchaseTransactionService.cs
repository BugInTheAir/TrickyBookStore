using System;
using System.Collections.Generic;
using System.Linq;
using TrickyBookStore.Models;
using TrickyBookStore.Repository.Context;
using TrickyBookStore.Services.Books;

namespace TrickyBookStore.Services.PurchaseTransactions
{
    public class PurchaseTransactionService :BaseService, IPurchaseTransactionService
    {
        private IBookService _bookService { get; }

        public PurchaseTransactionService(IBookService bookService, IBookContext context):base(context)
        {
            _bookService = bookService;
        }

        public IList<PurchaseTransaction> GetPurchaseTransactions(long customerId, int atMonth, int atYear)
        {
            var customerTransactions = this._context.PurchaseTransactionsData().Where(transaction => transaction.CustomerId.Equals(customerId) && transaction.CreatedDate.Month.Equals(atMonth) && transaction.CreatedDate.Year.Equals(atYear)).ToList();
            if (customerTransactions is null)
                return null;
            foreach(var transaction in customerTransactions)
            {
                transaction.Book = _bookService.GetBooks(transaction.BookId).FirstOrDefault();
            }
            return customerTransactions;
        }
    }
}
