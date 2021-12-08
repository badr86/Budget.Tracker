using Budget.Tracker.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Budget.Tracker.Core.MovementAggregate
{
    public interface IMovementRepository : IRepository<Movement>
    {
        void Add(Movement movement);
        
        void Update(Movement movement);

        void Remove(Movement movement);
        
        void Add(MovementItem movementItem);
        
        void Update(MovementItem movementItem);
        
        void Remove(MovementItem movementItem);

        Task<Movement> FindById(long movementId);
        
        Task<MovementItem> FindMovementItemById(long movementItemId);

        Task<IEnumerable<Movement>> FindByProjectId(long projectId);
        IQueryable<Movement> GetAll();
    }
}