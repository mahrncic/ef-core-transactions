using EFTransactions.API.Extensions;
using EFTransactions.Data.Db.DbContexts;
using EFTransactions.Data.Db.Uow;
using EFTransactions.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TransactionsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Database"]);
});

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<BlogService>();

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
