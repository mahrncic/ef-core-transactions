using EFTransactions.Data.Db.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EFTransactions.API.Extensions;

public static class MigrationBuilder
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var transacationsContext = scope.ServiceProvider.GetRequiredService<TransactionsDbContext>();

        if (transacationsContext.Database.IsRelational())
        {
            transacationsContext.Database.Migrate();
        }

        return host;
    }
}
