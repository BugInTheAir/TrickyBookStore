using System;
using System.Linq;
using TrickyBookStore.Models;
using TrickyBookStore.Repository.Context;
using TrickyBookStore.Services.Subscriptions;

namespace TrickyBookStore.Services.Customers
{
    public class CustomerService : BaseService, ICustomerService
    {
        private ISubscriptionService _subscriptionService { get; }

        public CustomerService(ISubscriptionService subscriptionService, IBookContext context): base (context)
        {
            _subscriptionService = subscriptionService;
        }

        public Customer GetCustomerById(long id)
        {
            var existedCustomer = this._context.CustomersData().Where(customer => customer.Id.Equals(id)).FirstOrDefault();
            if (existedCustomer is null)
                return null;
            existedCustomer.Subscriptions = _subscriptionService.GetSubscriptions(existedCustomer.SubscriptionIds);
            return existedCustomer;
        }

    }
}
