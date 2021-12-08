using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Budget.Tracker.Common;
using Budget.Tracker.Core.MovementAggregate;
using Budget.Tracker.InfraStructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Budget.Tracker.InfraStructure
{
    public class FinanceContext : DbContext, IUnitOfWork
    {
        //public DbSet<Project> Projects { get; set; }

        public DbSet<Direction> Directions { get; set; }

        public DbSet<Movement> Movements { get; set; }

        public DbSet<MovementItem> MovementItems { get; set; }

        //public DbSet<User> Users { get; set; }

        //public DbSet<RefreshToken> RefreshTokens { get; set; }

        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MovementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MovementItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DirectionEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new RefreshTokenEntityTypeConfiguration());

            modelBuilder.Entity<Direction>().HasData(Direction.In, Direction.Out);
            //modelBuilder.Entity<User>().HasData(new
            //{
            //    Id = 1L,
            //    CreatedAt = DateTimeOffset.UtcNow,
            //    UserName = "admin", 
            //    Email = "admin@finance.com",
            //    Role = "admin",
            //    IdentityId = "d48ef9ed-313e-40d5-94f8-6fb1d0b20d3d"
            //});
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetEntitiesUpdatedAt();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetEntitiesUpdatedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        private void SetEntitiesUpdatedAt()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(x => x.Entity is Entity && x.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                (entry.Entity as Entity)?.SetUpdatedAt();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\;Database=InspectionDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }

    public class FinanceContextDesignFactory : IDesignTimeDbContextFactory<FinanceContext>
    {
        public FinanceContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceContext>();
            optionsBuilder.UseSqlServer(@"Server=.\;Database=InspectionDB;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new FinanceContext(optionsBuilder.Options);
        }
    }
}