using Microsoft.AspNetCore.Mvc;

namespace Workshop.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBooks _bookService;

    public BooksController(IBooks bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? filter, string? order)
    {
        var books = await _bookService.GetAllBooks(filter, order);
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        await _bookService.Create(book);
        return StatusCode(201);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var book = await _bookService.Get(id);
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Book book)
    {
        var success = await _bookService.Update(book);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _bookService.Delete(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}