using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASP.Server.Model
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public List<Book> Books { get; set; }

        // Mettez ici les propriété de votre livre: Nom et Livres associés

        // N'oublier pas qu'un genre peut avoir plusieur livres
    }

}

