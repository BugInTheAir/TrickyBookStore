﻿ using System;
using System.Collections.Generic;
using System.Linq;
using TrickyBookStore.Models;
using TrickyBookStore.Repository.Context;
using TrickyBookStore.Services.Books;
using TrickyBookStore.Services.Customers;
using TrickyBookStore.Services.PurchaseTransactions;
using TrickyBookStore.Services.Utils;
namespace TrickyBookStore.Services.Payment
{
    public class PaymentService : BaseService, IPaymentService
    {
        private ICustomerService _customerService { get; }
        private IPurchaseTransactionService _purchaseTransactionService { get; }
        
        public PaymentService(ICustomerService customerService,
            IPurchaseTransactionService purchaseTransactionService, IBookContext context) : base(context)
        {
            _customerService = customerService;
            _purchaseTransactionService = purchaseTransactionService;
        }

        public double GetPaymentAmount(long customerId, int atMonth, int atYear)
        {
            var existedCustomer = _customerService.GetCustomerById(customerId);
            var customerTransactions = _purchaseTransactionService.GetPurchaseTransactions(customerId, atMonth, atYear);
            if (customerTransactions is null || existedCustomer is null || customerTransactions.Count() == 0)
                return 0;
            customerTransactions = customerTransactions.OrderByDescending(transaction => transaction.Book.Price).ToList();
            var subscriptions = existedCustomer.GetCustomerSubscriptionsPerTypeOrderByPriority();
            if (subscriptions.Count == 0)
                return UseFreePaymentCalculatorForOneMonth(customerTransactions);
            switch (subscriptions.First().Key)
            {
                case (int)SubscriptionTypes.Premium:
                    return UsePremiumPaymentCalculatorForOneMonth(customerTransactions);
                case (int)SubscriptionTypes.CategoryAddicted:
                    return UseCategoryAddictedCalculatorForOneMonth(customerTransactions,subscriptions.First().Value);
                case (int)SubscriptionTypes.Paid:
                    return UsePaidPaymentCalculatorForOneMonth(customerTransactions);
                case (int)SubscriptionTypes.Free:
                    return UseFreePaymentCalculatorForOneMonth(customerTransactions);
            }
            return 0;
        }
        private double UseCategoryAddictedCalculatorForOneMonth(IEnumerable<PurchaseTransaction> transactions, IEnumerable<Subscription> categoryAddictedSubscriptions)
        {
            double payment = 0;
            IList<PurchaseTransaction> fullPriceTransactions = transactions.Where(transaction => categoryAddictedSubscriptions.Where(subValue => subValue.BookCategoryId.Equals(transaction.Book.CategoryId)).FirstOrDefault() is null).ToList();
            IList<PurchaseTransaction> discountableTransactions = transactions.Where(transaction => categoryAddictedSubscriptions.Where(subValue => subValue.BookCategoryId.Equals(transaction.Book.CategoryId)).FirstOrDefault() != null).OrderByDescending(transaction => transaction.Book.Price).ToList();
            foreach(var transaction in fullPriceTransactions)
            {
                payment += transaction.Book.Price;
            }
            IDictionary<int, IList<PurchaseTransaction>> transactionPerCategory = new Dictionary<int, IList<PurchaseTransaction>>();
            foreach(var transaction in discountableTransactions)
            {
                if (transaction.Book.IsOld)
                    continue;
                if (!transactionPerCategory.ContainsKey(transaction.Book.CategoryId))
                {
                    IList<PurchaseTransaction> newTransactions = new List<PurchaseTransaction>();
                    newTransactions.Add(transaction);
                    transactionPerCategory.Add(transaction.Book.CategoryId, newTransactions);
                }
                else
                {
                    transactionPerCategory[transaction.Book.CategoryId].Add(transaction);
                }
            }
            foreach(var category in transactionPerCategory)
            {
                int discountForNewBooksRemain = 3;
                foreach(var transaction in category.Value)
                {
                    var basePrice = transaction.Book.Price;
                    if(discountForNewBooksRemain == 0)
                    {
                        payment += basePrice;
                    }
                    else
                    {
                        payment += basePrice - GetDiscountValue(basePrice, 15);
                        discountForNewBooksRemain--;
                    }
                }
            }
            return payment.Round(2);
        }
        private double UsePremiumPaymentCalculatorForOneMonth(IEnumerable<PurchaseTransaction> transactions)
        {
            double payment = 0;
            int freeBooksRemain = 3;
            foreach(var transaction in transactions)
            {
                var basePrice = transaction.Book.Price;
                if (!transaction.Book.IsOld)
                {
                    if(freeBooksRemain == 0)
                    {
                        payment += basePrice;
                    }
                    else
                    {
                        freeBooksRemain--;
                        payment += basePrice - GetDiscountValue(basePrice, 15);
                    }
                }
            }
            return payment.Round(2);
        }
        private double UsePaidPaymentCalculatorForOneMonth(IEnumerable<PurchaseTransaction> transactions)
        {
            double payment = 0;
            int discountForNewBooksRemain = 3;
            foreach(var transaction in transactions)
            {
                var basePrice = transaction.Book.Price;
                if (transaction.Book.IsOld)
                {
                    payment += GetDiscountValue(basePrice, 5);
                }
                else
                {
                    if(discountForNewBooksRemain != 0)
                    {
                        discountForNewBooksRemain--;
                        payment += basePrice - GetDiscountValue(basePrice, 5);
                    }
                    else
                    {
                        payment += basePrice;
                    }
                }
            }
            return payment.Round(2);
        }
        private double UseFreePaymentCalculatorForOneMonth(IEnumerable<PurchaseTransaction> transactions)
        {
            double payment = 0;
            foreach(var transaction in transactions)
            {
                var basePrice = transaction.Book.Price;
                if (transaction.Book.IsOld)
                {
                    payment += basePrice - GetDiscountValue(basePrice, 10);
                }
                else
                {
                    payment += basePrice;
                }
            }
            return payment.Round(2);
        }
        private double GetDiscountValue(double basePrice, double percentDiscount)
        {
            return (basePrice * percentDiscount) / 100;
        }
    }
}