using backend;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BooksDb"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/books", async (HttpContext context, AppDbContext db) =>
{
    if (!int.TryParse(context.Request.Headers["X-User-Id"], out int userId))
    {
        return Results.BadRequest("Missing or invalid X-User-Id header");
    }
    return Results.Ok(await db.Books.Where(b => b.UserId == userId).ToListAsync());
});

app.MapGet("/books/{id}", async (int id, HttpContext context, AppDbContext db) =>
{
    if (!int.TryParse(context.Request.Headers["X-User-Id"], out int userId))
    {
        return Results.BadRequest("Missing or invalid X-User-Id header");
    }

    var book = await db.Books.FindAsync(id);

    if (book is null || book.UserId != userId)
    {
        return Results.NotFound();
    }

    return Results.Ok(book);
});

app.MapPost("/books", async (Book book, HttpContext context, AppDbContext db) =>
{
    if (!int.TryParse(context.Request.Headers["X-User-Id"], out int userId))
    {
        return Results.BadRequest("Missing or invalid X-User-Id header");
    }

    book.UserId = userId;
    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id}", async (int id, Book inputBook, HttpContext context, AppDbContext db) =>
{
    if (!int.TryParse(context.Request.Headers["X-User-Id"], out int userId))
    {
        return Results.BadRequest("Missing or invalid X-User-Id header");
    }

    var book = await db.Books.FindAsync(id);

    if (book is null || book.UserId != userId) return Results.NotFound();

    book.Title = inputBook.Title;
    book.Author = inputBook.Author;
    book.Genre = inputBook.Genre;
    book.Year = inputBook.Year;
    book.Pages = inputBook.Pages;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/books/{id}", async (int id, HttpContext context, AppDbContext db) =>
{
    if (!int.TryParse(context.Request.Headers["X-User-Id"], out int userId))
    {
        return Results.BadRequest("Missing or invalid X-User-Id header");
    }

    var book = await db.Books.FindAsync(id);

    if (book is null || book.UserId != userId)
    {
        return Results.NotFound();
    }

    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/", () => "Welcome to the Book API! Try /books to see the list of books.");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Books.Any())
    {
        var books = new List<Book>();
        for (int i = 1; i <= 10; i++)
        {
            books.Add(new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Classic", Year = 1925, Pages = 180, UserId = i });
            books.Add(new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Classic", Year = 1960, Pages = 281, UserId = i });
            books.Add(new Book { Title = "1984", Author = "George Orwell", Genre = "Dystopian", Year = 1949, Pages = 328, UserId = i });
            books.Add(new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Genre = "Romance", Year = 1813, Pages = 279, UserId = i });
            books.Add(new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Genre = "Fantasy", Year = 1937, Pages = 310, UserId = i });
        }
        db.Books.AddRange(books);
        db.SaveChanges();
    }
}

app.Run();

