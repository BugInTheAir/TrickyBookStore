using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyBookStore.Models;

namespace TrickyBookStore.Services.Utils
{
    public static class CustomerExtensions
    {
        public static IDictionary<int, IList<Subscription>>GetCustomerSubscriptionsPerType(this Customer customer)
        {
            Dictionary<int, IList<Subscription>> subscriptionPerType = new Dictionary<int, IList<Subscription>>();
            foreach(var sub in customer.Subscriptions)
            {
                switch (sub.SubscriptionType)
                {
                    case SubscriptionTypes.Free:
                        EnrichSubscriptionsDictionary(subscriptionPerType, sub, (int)SubscriptionTypes.Free);
                        break;
                    case SubscriptionTypes.Paid:
                        EnrichSubscriptionsDictionary(subscriptionPerType, sub, (int)SubscriptionTypes.Paid);
                        break;
                    case SubscriptionTypes.Premium:
                        EnrichSubscriptionsDictionary(subscriptionPerType, sub, (int)SubscriptionTypes.Premium);
                        break;
                    case SubscriptionTypes.CategoryAddicted:
                        EnrichSubscriptionsDictionary(subscriptionPerType, sub, (int)SubscriptionTypes.CategoryAddicted);
                        break;
                }
            }
            return subscriptionPerType;
        }

        private static void EnrichSubscriptionsDictionary(Dictionary<int, IList<Subscription>> subscriptionPerType, Subscription sub, int type)
        {
            if (subscriptionPerType.ContainsKey(type))
            {
                subscriptionPerType[type].Add(sub);
            }
            else
            {
                List<Subscription> subs = new List<Subscription>();
                subs.Add(sub);
                subscriptionPerType.Add(type, subs);
            }
        }
    }
}
