using System;

namespace backend
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int? Year { get; set; }
        public int? Pages { get; set; }
        public int UserId { get; set; }
    }
}
