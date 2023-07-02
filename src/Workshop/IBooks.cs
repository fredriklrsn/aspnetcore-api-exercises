namespace Workshop;

public interface IBooks
{
    Task<List<Book>> GetAllBooks(string? filter, string? order);
    Task<Book?> Get(int id);
    Task Create(Book book);
    Task<bool> Update(Book book);
    Task<bool> Delete(int id);
}