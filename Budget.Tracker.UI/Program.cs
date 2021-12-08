using Budget.Tracker.Application;
using Budget.Tracker.Application.Interfaces;
using Budget.Tracker.Core.MovementAggregate;
using Budget.Tracker.InfraStructure;
using Budget.Tracker.InfraStructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Budget.Tracker.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
             .AddDbContext<FinanceContext>(ServiceLifetime.Scoped)

            .AddScoped<IMovementRepository, MovementRepository> ()
            .AddScoped<IMovementAppService, MovementAppService> ()

            .BuildServiceProvider();

            var movementAppService = serviceProvider.GetService<IMovementAppService>();
            var result = movementAppService.GetMovements(DateTime.Now.AddDays(-10), DateTime.MaxValue);
            Console.WriteLine(result.Count);

            var total = movementAppService.GetStatus(DateTime.Now.AddDays(-10), DateTime.MaxValue);
            
            Console.WriteLine($"Total : {total.PlanAmount * total.DirectionId}");
            Console.ReadLine();
        }
    }
}
