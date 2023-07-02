// namespace Workshop;

// public class InMemoryBooks : IBooks
// {
//     public List<Book> GetAllBooks(string? filter, string? order)
//     {
//         var books = Database.Books;
//         if (filter is not null)
//             books = books.Where(b => b.Name.Contains(filter)).ToList();

//         if (order is not null)
//         {
//             if (order == "asc")
//                 books = books.OrderBy(b => b.Name).ToList();
//             else if (order == "desc")
//                 books = books.OrderByDescending(b => b.Name).ToList();
//         }

//         return books;
//     }

//     public Book? Get(int id)
//     {
//         return Database.Books.FirstOrDefault(b => b.Id == id);
//     }

//     public void Create(Book book)
//     {
//         Database.Books.Add(book);
//     }

//     public bool Update(Book book)
//     {
//         var bookToUpdate = Database.Books.FirstOrDefault(b => b.Id == book.Id);
//         if (bookToUpdate is null)
//             return false;

//         bookToUpdate.Name = book.Name;
//         bookToUpdate.Year = book.Year;

//         return true;
//     }

//     public bool Delete(int id)
//     {
//         var bookToDelete = Database.Books.FirstOrDefault(b => b.Id == id);
//         if (bookToDelete is null)
//             return false;

//         Database.Books.Remove(bookToDelete);

//         return true;
//     }
// }