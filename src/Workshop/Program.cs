using Dapper;
using Microsoft.Data.Sqlite;
using Workshop;

var builder = WebApplication.CreateBuilder(args);

// Register and seed database.
var connectionString = builder.Configuration.GetConnectionString("Data Source=:memory:");
using var connection = new SqliteConnection(connectionString);
connection.Open();
SeedDatabase(connection);
builder.Services.AddSingleton<SqliteConnection>(connection);

// Add services to the container.
builder.Services.AddScoped<IBooks, SqliteBooks>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void SeedDatabase(SqliteConnection connection)
{
    connection.Execute("CREATE TABLE Books (Id INTEGER PRIMARY KEY, Name TEXT, Year INTEGER)");
    connection.Execute("INSERT INTO Books (Name, Year) VALUES ('Book 1', 2021)");
    connection.Execute("INSERT INTO Books (Name, Year) VALUES ('Book 2', 2022)");
    connection.Execute("INSERT INTO Books (Name, Year) VALUES ('Book 3', 2023)");
}