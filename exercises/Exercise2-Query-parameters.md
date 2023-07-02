# Exercise 2: Query parameters

## Task 1: Add a query parameter when getting all books
In the `Controllers/BooksController.cs` file, expand the endpoint for getting all books with a query parameter:
```csharp
[HttpGet]
public IActionResult GetAll(string? filter)
{
    var books = Database.Books;
    if (filter is not null)
        books = books.Where(b => b.Name.Contains(filter)).ToList();

    return Ok(books);
}
```

## Task 2: Add a query parameter when getting all books
In the `Controllers/BooksController.cs` file, expand the endpoint for getting all books with a second query parameter:
```csharp
[HttpGet]
public IActionResult GetAll(string? filter, string? order)
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

    return Ok(books);
}
```