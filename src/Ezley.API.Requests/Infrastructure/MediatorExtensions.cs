using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace ES.API.Requests.Infrastructure
{
   public static class MediatorExtensions
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services, Assembly assembly)
        {
            // services.AddTransient<IRequestHandler<GetPendingOrders, PendingOrdersView>,
            //     GetPendingOrdersHandler>();
            // services.AddTransient<IRequestHandler<GetOrder, OrderView>,
            //     GetOrderHandler>();
            return services;
        }
    }
}