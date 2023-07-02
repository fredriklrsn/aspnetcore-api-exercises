# Exercise 5: SQL injection attack

## Task 1: Change method for getting all books
Update the method `GetAllBooks` in `SqliteBooks.cs` to not check the order string and just use whatever is sent in:

```csharp
...

public async Task<List<Book>> GetAllBooks(string? filter, string? order)
{
    var sql = "SELECT * FROM Books";
    if (filter is not null)
        sql += " WHERE Name LIKE @Filter";
    if (order is not null)
        sql += $" ORDER BY Name {order.ToUpper()}";
    
    var parameters = filter is null ? null : new { Filter = $"%{filter}%" };

    return (await _connection.QueryAsync<Book>(sql, parameters)).ToList();
}

...
```

## Task 2: Test the api
- Run the application, get all books and see that all books are returned.
- Get all books with order `asc; DROP TABLE Books;`. This will return all books.
- Get all books now and see what happens.


