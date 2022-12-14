using EFTransactions.Data.Db.DbContexts;

namespace EFTransactions.Data.Db.Uow
{
    public class UnitOfWork
    {
        public TransactionsDbContext DbContext { get; set; }

        public UnitOfWork(TransactionsDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}