using Microsoft.Extensions.Hosting;
using StructureMap;
using System;
using System.Threading.Tasks;
using TrickyBookStore.Repository.Context;
using TrickyBookStore.Services;
using TrickyBookStore.Services.Books;
using TrickyBookStore.Services.Customers;
using TrickyBookStore.Services.Payment;
using TrickyBookStore.Services.PurchaseTransactions;
using TrickyBookStore.Services.Subscriptions;

namespace TrickyBookStore.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(IScannableServiceAssembly));
                    _.WithDefaultConventions();
                });
                config.Populate(services);
            });
            var myPaymentService = container.GetInstance<IPaymentService>();
            int customerID;
            int atMonth;
            int atYear;
            while (true)
            {
                Console.Write("Customer ID: ");
                customerID = int.Parse(Console.ReadLine());
                Console.Write("Month: ");
                atMonth = int.Parse(Console.ReadLine());
                Console.Write("Year: ");
                atYear = int.Parse(Console.ReadLine());
                Console.WriteLine($"Payment amount: {myPaymentService.GetPaymentAmount(customerID, atMonth, atYear)} USD");
            }
          
        }
    }
}
