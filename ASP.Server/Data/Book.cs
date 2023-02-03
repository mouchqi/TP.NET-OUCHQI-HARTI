using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASP.Server.Model
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Price { get; set; }
        public string Content { get; set; }
        public List<Genre> Kinds { get; set; }

        // Mettez ici les propriété de votre livre: Nom, Autheur, Prix, Contenu et Genres associés
        // N'oublier pas qu'un livre peut avoir plusieur genres
    }

    public class BookPublic
    {
        [JsonIgnore]
        public Book Book { init; private get; }
        public int Id { get { return Book.Id; } }
        public string Name { get { return Book.Name; } }
        public string Author { get { return Book.Author; } }
        public string Price { get { return Book.Price; } }
        public List<Genre> Kinds { get { return Book.Kinds; } }
    }

    public class BookGenre
    {
        [JsonIgnore]
        public Book Book { init; private get; }
        public string Name { get { return Book.Name; } }
        public string Author { get { return Book.Author; } }
    }
}
