using System;
using System.Collections.Generic;
using System.Linq;
using TrickyBookStore.Models;
using TrickyBookStore.Repository.Context;

namespace TrickyBookStore.Services.Subscriptions
{
    public class SubscriptionService : BaseService, ISubscriptionService
    {
        public SubscriptionService(IBookContext context) : base(context)
        {

        }

        public IList<Subscription> GetSubscriptions(IEnumerable<int> ids)
        {
            return this._context.SubscriptionsData().Where(sub => ids.Contains(sub.Id)).ToList();
        }
    }
}
