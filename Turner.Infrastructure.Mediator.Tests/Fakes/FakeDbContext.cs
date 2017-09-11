using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Turner.Infrastructure.Mediator.Tests.Fakes
{
    public class FakeDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            optionsBuilder.UseInMemoryDatabase();
            base.OnConfiguring(optionsBuilder);
        }

        public override DatabaseFacade Database => new FakeDatabaseFacade(this);
    }

    public class FakeDatabaseFacade : DatabaseFacade
    {
        private IDbContextTransaction _currentTransaction;

        public static int TransactionCount { get; set; }
        public static int CommitCount { get; set; }
        public static int RollbackCount { get; set; }

        public FakeDatabaseFacade(DbContext context) : base(context)
        {
            TransactionCount = 0;
            CommitCount = 0;
            RollbackCount = 0;
        }

        public override IDbContextTransaction CurrentTransaction => _currentTransaction;

        public override IDbContextTransaction BeginTransaction()
        {
            _currentTransaction = new FakeTransaction(this);
            TransactionCount++;
            return _currentTransaction;
        }

        public override void CommitTransaction()
        {
            CommitCount++;
            _currentTransaction = null;
        }

        public override void RollbackTransaction()
        {
            RollbackCount++;
            _currentTransaction = null;
        }
    }

    public class FakeTransaction : InMemoryTransaction
    {
        private readonly FakeDatabaseFacade _database;

        public FakeTransaction(FakeDatabaseFacade database)
        {
            _database = database;
        }

        public override void Commit()
        {
            _database.CommitTransaction();
        }

        public override void Rollback()
        {
            _database.RollbackTransaction();
        }
    }
}
