# Exercise 4: Replace in memory list with database

## Task 1: Add dependencies
In the console run the following commands to add the dependencies we need to work with the database

```
dotnet add package Microsoft.Data.Sqlite 
dotnet add package Dapper
dotnet add package Dapper.Sqlite 
```

## Task 2: Class for database logic
Create a new file named `SqliteBooks.cs` in a folder named `Data`.
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

    public List<Book> GetAllBooks(string? filter, string? order)
    {
        var sql = "SELECT * FROM Books";

        if (filter is not null)
            sql += " WHERE Name LIKE @Filter";

        if (order is not null && (order == "asc" || order == "desc"))
            sql += $" ORDER BY Name {order.ToUpper()}";
        
        var parameters = filter is null ? null : new { Filter = $"%{filter}%" };

        return _connection.Query<Book>(sql, parameters).ToList();
    }

    public Book? Get(int id)
    {
        var sql = "SELECT * FROM Books WHERE Id = @Id";

        return _connection.QueryFirstOrDefault<Book>(sql, new { Id = id });
    }

    public void Create(Book book)
    {
        var sql = "INSERT INTO Books (Id, Name, Year) "
                + "VALUES (@Id, @Name, @Year)";
        
        _connection.Execute(sql, new { Id = book.Id, Name = book.Name, Year = book.Year});
    }

    public bool Update(Book book)
    {
        try
        {
            var sql = "UPDATE Books "
                    + "SET Name = @Name, Year = @Year "
                    + "WHERE Id = @Id";
        
            _connection.Execute(sql, new { Id = book.Id, Name = book.Name, Year = book.Year});

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            var sql = "DELETE FROM Books "
                    + "WHERE Id = @Id";
        
            _connection.Execute(sql, new { Id = id });

            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## Task 3: Connect to and seed database at startup
Update `Program.cs`:
```csharp
using Dapper;
using Microsoft.Data.Sqlite;
using Workshop;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Data Source=:memory:");
using var connection = new SqliteConnection(connectionString);
connection.Open();
SeedDatabase(connection);
builder.Services.AddSingleton<SqliteConnection>(connection);

// Add services to the container.
builder.Services.AddScoped<IBooks, SqliteBooks>();

...

void SeedDatabase(SqliteConnection connection)
{
    connection.Execute("CREATE TABLE Books (Id INTEGER PRIMARY KEY, Name TEXT, Year INTEGER)");
    connection.Execute("INSERT INTO Books (Name, Year) VALUES ('Book 1', 2021)");
    connection.Execute("INSERT INTO Books (Name, Year) VALUES ('Book 2', 2022)");
    connection.Execute("INSERT INTO Books (Name, Year) VALUES ('Book 3', 2023)");
}
```

Note that we have changed the implementation of `Program.cs` from `InMemoryBooks` to `SqliteBooks`. This can be done because both are implementations of the same interface, i.e. they fulfill the same contract.

## Task 4: Use the registered database class
Run the api and test that it works as before.