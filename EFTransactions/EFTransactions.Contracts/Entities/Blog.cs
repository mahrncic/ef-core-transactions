namespace EFTransactions.Contracts.Entities;

public class Blog
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;
}
