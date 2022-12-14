using EFTransactions.Contracts.Entities;
using EFTransactions.Data.Db.DbContexts;
using EFTransactions.Data.Db.Uow;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EFTransactions.Services.Implementations;

public class BlogService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly TransactionsDbContext _context;

    public BlogService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _context = _unitOfWork.DbContext;
    }

    public async Task CreateWithUoWAsync()
    {
        _unitOfWork.DbContext.Blogs.Add(new Blog
        {
            Title = "Added With UoW",
            Content = "UoW Content",
        });

        _unitOfWork.DbContext.Blogs.Add(new Blog
        {
            Title = "Added With UoW",
            Content = "UoW Content",
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public void CreateWithBeginTransaction()
    {
        using var transaction = _context.Database.BeginTransaction();

        _context.Blogs.Add(new Blog
        {
            Title = "Added With Begin Transaction 1",
            Content = "Begin Transaction 1 Content",
        });
        _context.SaveChanges();

        _context.Blogs.Add(new Blog
        {
            Title = "Added With Begin Transaction 2",
            Content = "Begin Transaction 2 Content",
        });
        _context.SaveChanges();

        transaction.Commit();
    }

    public void CreateWithBeginTransactionAndRollback()
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Blogs.Add(new Blog
            {
                Title = "Added With Begin/Rollback Transaction 1",
                Content = "Begin/Rollback Transaction 1 Content",
            });
            _context.SaveChanges();

            transaction.CreateSavepoint("FirstSavePoint");

            _context.Blogs.Add(new Blog
            {
                Title = "Added With Begin/Rollback Transaction 2",
                Content = "Begin/Rollback Transaction 2 Content",
            });
            _context.SaveChanges();

            transaction.CreateSavepoint("SecondSavePoint");

            throw new();

            _context.Blogs.Add(new Blog
            {
                Title = "Added With Begin/Rollback Transaction 3",
                Content = "Begin/Rollback Transaction 3 Content",
            });
            _context.SaveChanges();

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.RollbackToSavepoint("FirstSavePoint");
            transaction.Commit();
        }
    }

    public void CreateWithExternalTransaction()
    {
        using var connection = new SqlConnection("Server=localhost;Database=transactions;Trusted_Connection=True;");
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "DELETE FROM dbo.Blogs";
        command.ExecuteNonQuery();

        var options = new DbContextOptionsBuilder<TransactionsDbContext>()
            .UseSqlServer(connection)
            .Options;

        using (var context = new TransactionsDbContext(options))
        {
            context.Database.UseTransaction(transaction);
            context.Blogs.Add(new Blog
            {
                Title = "Added With External Transaction 1",
                Content = "ExternalTransaction 1 Content",
            });
            context.SaveChanges();
        }

        transaction.Commit();
    }

    public void CreateWithTransactionScope()
    {
        using (var scope = new TransactionScope(
                   TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
        {
            using var connection = new SqlConnection("Server=localhost;Database=transactions;Trusted_Connection=True;");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM dbo.Blogs";
            command.ExecuteNonQuery();

            var options = new DbContextOptionsBuilder<TransactionsDbContext>()
                .UseSqlServer(connection)
                .Options;

            using (var context = new TransactionsDbContext(options))
            {
                context.Blogs.Add(new Blog
                {
                    Title = "Added With Transaction Scope 1",
                    Content = "Transaction Scope 1 Content",
                });
                context.SaveChanges();
            }

            scope.Complete();
        }
    }
}
