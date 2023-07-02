# Exercise 3: Dependency injection

## Task 1: Seperate class responsible for database logic
Move all the logic for talking to our database to a separate class named `InMemoryBooks.cs` in the folder named `Data`:
```csharp
namespace Workshop;

public class InMemoryBooks
{
    public List<Book> GetAllBooks(string? filter, string? order)
    {
        var books = Database.Books;
        if (filter is not null)
            books = books.Where(b => b.Name.Contains(filter)).ToList();

        if (order is not null)
        {
            if (order == "asc")
                books = books.OrderBy(b => b.Name).ToList();
            else if (order == "desc")
                books = books.OrderByDescending(b => b.Name).ToList();
        }

        return books;
    }

    public Book? Get(int id)
    {
        return Database.Books.FirstOrDefault(b => b.Id == id);
    }

    public void Create(Book book)
    {
        Database.Books.Add(book);
    }

    public bool Update(Book book)
    {
        var bookToUpdate = Database.Books.FirstOrDefault(b => b.Id == book.Id);
        if (bookToUpdate is null)
            return false;

        bookToUpdate.Name = book.Name;
        bookToUpdate.Year = book.Year;

        return true;
    }

    public bool Delete(int id)
    {
        var bookToDelete = Database.Books.FirstOrDefault(b => b.Id == id);
        if (bookToDelete is null)
            return false;

        Database.Books.Remove(bookToDelete);

        return true;
    }
}
```

## Task 2: Interface for defining functionality
Create a new file name `IBooks.cs` in the folder named `Data`:
```csharp
namespace Workshop;

public interface IBooks
{
    List<Book> GetAllBooks(string? filter, string? order);
    Book? Get(int id);
    void Create(Book book);
    bool Update(Book book);
    bool Delete(int id);
}
```

Update the class `InMemoryBooks` so the class declaration now looks like this:

```csharp
...
public class InMemoryBooks : IBooks
...
```

## Task 3: Register our database logic class
In the `Program.cs` file, register the new class we just created as the implementation of our interface:

```csharp
builder.Services.AddScoped<IBooks, InMemoryBooks>();
```

The `Program.cs` file should now look like this.

```csharp
using Workshop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBooks, InMemoryBooks>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

...
```

## Task 4: Use the registered database class
In the `Program.cs` file, register the new class we just created as the implementation of our interface:

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
    public IActionResult GetAll(string? filter, string? order)
    {
        var books = _bookService.GetAllBooks(filter, order);

        return Ok(books);
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        _bookService.Create(book);

        return StatusCode(201);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var book = _bookService.Get(id);
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPut]
    public IActionResult Update(Book book)
    {
        var success = _bookService.Update(book);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _bookService.Delete(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
```

## Task 5: Use the registered database class
Run the api and test that it works as before.