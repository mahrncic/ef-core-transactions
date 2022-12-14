using EFTransactions.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace EFTransactions.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly BlogService _blogService;

    public BlogsController(BlogService blogService)
    {
        _blogService = blogService;
    }

    [Produces("application/json")]
    [HttpPost("uow")]
    public async Task<IActionResult> CreateWithUow()
    {
        await _blogService.CreateWithUoWAsync();

        return NoContent();
    }

    [Produces("application/json")]
    [HttpPost("begin-transaction")]
    public IActionResult CreateWithBeginTransaction()
    {
        _blogService.CreateWithBeginTransaction();

        return NoContent();
    }

    [Produces("application/json")]
    [HttpPost("begin-rollback-transaction")]
    public IActionResult CreateWithBeginRollbackTransaction()
    {
        _blogService.CreateWithBeginTransactionAndRollback();

        return NoContent();
    }

    [Produces("application/json")]
    [HttpPost("external")]
    public IActionResult CreateWithExternalTransaction()
    {
        _blogService.CreateWithExternalTransaction();

        return NoContent();
    }

    [Produces("application/json")]
    [HttpPost("transaction-scope")]
    public IActionResult CreateWithTransactionScope()
    {
        _blogService.CreateWithTransactionScope();

        return NoContent();
    }
}
