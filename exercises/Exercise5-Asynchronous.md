# Exercise 5: Asynchronous database interaction

## Task 1: Make database class asynchronous
Update the class `SqliteBooks.cs` so all the methods are async methods:

```csharp
using Dapper;
using Microsoft.Data.Sqlite;

namespace Workshop;

public class SqliteBooks : IBooks
{
    private readonly SqliteConnection _connection;

    public SqliteBooks(SqliteConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<Book>> GetAllBooks(string? filter, string? order)
    {
        var sql = "SELECT * FROM Books";

        if (filter is not null)
            sql += " WHERE Name LIKE @Filter";

        if (order is not null && (order == "asc" || order == "desc"))
            sql += $" ORDER BY Name {order.ToUpper()}";
        
        var parameters = filter is null ? null : new { Filter = $"%{filter}%" };

        return (await _connection.QueryAsync<Book>(sql, parameters)).ToList();
    }

    public async Task<Book?> Get(int id)
    {
        var sql = "SELECT * FROM Books WHERE Id = @Id";

        return await _connection.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
    }

    public async Task Create(Book book)
    {
        var sql = "INSERT INTO Books (Id, Name, Year) "
                + "VALUES (@Id, @Name, @Year)";
        
        await _connection.ExecuteAsync(sql, new { Id = book.Id, Name = book.Name, Year = book.Year});
    }

    public async Task<bool> Update(Book book)
    {
        try
        {
            var sql = "UPDATE Books "
                    + "SET Name = @Name, Year = @Year "
                    + "WHERE Id = @Id";
        
            await _connection.ExecuteAsync(sql, new { Id = book.Id, Name = book.Name, Year = book.Year});

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var sql = "DELETE FROM Books "
                    + "WHERE Id = @Id";
        
            await _connection.ExecuteAsync(sql, new { Id = id });

            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## Task 2: Make interface asynchronous
Update the class `SqliteBooks.cs` so all the methods are async methods:

```csharp
namespace Workshop;

public interface IBooks
{
    Task<List<Book>> GetAllBooks(string? filter, string? order);
    Task<Book?> Get(int id);
    Task Create(Book book);
    Task<bool> Update(Book book);
    Task<bool> Delete(int id);
}
```

## Task 3: Update controller to use asynchronous methods
Update the class `SqliteBooks.cs` so all the methods are async methods:

```csharp
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
```

## Task 5: Use the asynchronous api
Run the api and test that it works as before.