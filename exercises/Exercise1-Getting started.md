# Exercise 1: Getting Started

## Task 1: Create a new web API project

Got to a folder of your choice on your computer, open a command prompt and run the following command to create a new web API project:

```
dotnet new webapi --name Workshop
```

Open the application in Visual Studio Code. If asked to add debug configuration, say yes. Inspect the code to see what is part of the api template.

Run the application by navigating to the project folder and rcalling the run command using the following commands:
```
cd Workshop
dotnet run
```

Open your browser and navigate to the url in the console, e.g. `http://localhost:5001/swagger`, to access the Swagger documentation for the API. Here you can explore and test the available endpoints.

You can also start the application in debug mode using F5.

Delete the default endpoint by removing the `WeatherForecastController.cs` and the `WeatherForecast.cs` file.

## Task 2: Create a data store

Create a new file named `Book.cs` in a folder named `Data`.
```csharp
namespace Workshop;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
}
```

Create a new file named `Database.cs` in the folder named `Data`. This is going to serve as our database for now.

Add the following code to define the `Database` class with a static list of books:
```csharp
namespace Workshop;

public class Database
{
    public static List<Book> Books = new List<Book>
    {
        new Book { Id = 1, Name = "Book 1", Year = 2021 },
        new Book { Id = 2, Name = "Book 2", Year = 2022 },
        new Book { Id = 3, Name = "Book 3", Year = 2023 }
    };
}
```
## Task 3: Create a GET endpoint to retrieve all books
Create a new file named `BooksController.cs` in the folder named `Controllers`.

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Workshop.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
}
```

In the `Controllers/BooksController.cs` file, add the following code inside the `BooksController` class to define the GET endpoint:
```csharp
[HttpGet]
public IActionResult GetAllBooks()
{
    return Ok(Database.Books);
}
```

## Task 4: Create a GET endpoint to get a specific book
In the `Controllers/BooksController.cs` file, add the following code inside the `BooksController` class to define the GET endpoint:
```csharp
[HttpGet("{id}")]
public IActionResult Get(int id)
{
    var book = Database.Books.FirstOrDefault(b => b.Id == id);
    if (book is null)
        return NotFound();

    return Ok(book);
}
```

## Task 5: Create a POST endpoint to create a book
In the `Controllers/BooksController.cs` file, add the following code inside the `BooksController` class to define the POST endpoint:
```csharp
[HttpPost]
public IActionResult Create(Book book)
{
    Database.Books.Add(book);

    return StatusCode(201);
}
```

## Task 6: Create a PUT endpoint to update a book
In the `Controllers/BooksController.cs` file, add the following code inside the `BooksController` class to define the PUT endpoint:
```csharp
[HttpPut]
public IActionResult Update(Book book)
{
    var bookToUpdate = Database.Books.FirstOrDefault(b => b.Id == book.Id);
    if (bookToUpdate is null)
        return NotFound();

    bookToUpdate.Name = book.Name;
    bookToUpdate.Year = book.Year;

    return NoContent();
}
```

## Task 7: Create a DELETE endpoint to delete a book
In the `Controllers/BooksController.cs` file, add the following code inside the `BooksController` class to define the PUT endpoint:
```csharp
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    var bookToDelete = Database.Books.FirstOrDefault(b => b.Id == id);
    if (bookToDelete is null)
        return NotFound();

    Database.Books.Remove(bookToDelete);
    
    return NoContent();
}
```