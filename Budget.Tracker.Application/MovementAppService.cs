using Budget.Tracker.Application.Interfaces;
using Budget.Tracker.Core.MovementAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Tracker.Application
{
    public class MovementAppService : IMovementAppService
    {
        private readonly IMovementRepository repository;

        public MovementAppService(IMovementRepository repository)
        {
            this.repository = repository;
        }
        public async Task Add(Movement m)
        {
            repository.Add(m);
            await repository.UnitOfWork.SaveChangesAsync();
        }

        public List<Movement> GetMovements(DateTime start, DateTime end)
        {
            return repository.GetAll().Where(m => start <= m.CreatedAt.Date && m.CreatedAt.Date <= end).ToList();
        }

        public Movement GetStatus(DateTime start, DateTime end)
        {
            var allMovements = GetMovements(start, end);
            var inMovements = allMovements.Where(m => m.DirectionId == 1).Sum(x=> x.PlanAmount);
            var outMovements = allMovements.Where(m => m.DirectionId == -1).Sum(x => x.PlanAmount);
            var total = inMovements - outMovements;
            return new Movement(planAmount: Math.Abs(total), name: "Total", directionId: total >= 0 ? 1 : -1, projectId: 0);
        }
    }
}
