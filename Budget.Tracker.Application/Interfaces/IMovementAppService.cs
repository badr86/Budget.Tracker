using Budget.Tracker.Core.MovementAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Tracker.Application.Interfaces
{
    public interface IMovementAppService
    {
        Task Add(Movement m);
        List<Movement> GetMovements(DateTime start, DateTime End);
        Movement GetStatus(DateTime start, DateTime end);
    }
}
