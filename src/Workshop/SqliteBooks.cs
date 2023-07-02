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