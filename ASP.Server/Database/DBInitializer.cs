using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Model;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ASP.Server.Database
{
    public class DbInitializer
    {
        public static void Initialize(LibraryDbContext bookDbContext)
        {

            if (bookDbContext.Books.Any())
                return;

            

            bookDbContext.Genre.AddRange(
                new Genre() { Name = "Comedie", Description = "livre de comedie" },
                new Genre() { Name = "Documentaire", Description = "livre documentaire." },
                new Genre() { Name = "Action", Description = "livre d'action." },
                new Genre() { Name = "Drama", Description = "livre Dramatique." },
                new Genre() { Name = "Thriller", Description = "livre Policière." },
                new Genre() { Name = "Adventure", Description = "livre d'aventure." },
                new Genre() { Name = "Crime", Description = "livre Criminelle." },
                new Genre() { Name = "Romance", Description = "livre Romantique." },
                new Genre() { Name = "SF", Description = "livre de SF." }
            );

            

            bookDbContext.SaveChanges();

            // Une fois les moèles complété Vous pouvez faire directement
            // new Book() { Author = "xxx", Name = "yyy", Price = n.nnf, Content = "ccc", Genres = new() { Romance, Thriller } }
            bookDbContext.Books.AddRange(
                new Book()
                {
                    Name = "Lupin",
                    Price = "19",
                    Content = "lupin-policier",
                    Author = "jack",
                    Kinds = new List<Genre> { bookDbContext.Genre.Single(genre => genre.Name == "SF") }



                }, 
                new Book()
                {
                    Name = "alladin",
                    Price = "89",
                    Content = "alladin",
                    Author = "amnay",
                    Kinds = new List<Genre> { bookDbContext.Genre.Single(genre => genre.Name == "SF") }

                },

                new Book()
                {
                    Name = "witcher",
                    Price = "49",
                    Content = "the witcher",
                    Author = "shaun",
                    Kinds = new List<Genre> { bookDbContext.Genre.Single(genre => genre.Name == "SF") }

                },
                new Book()
                {
                    Name = "central park",
                    Price = "19",
                    Content = "park",
                    Author = "park",
                    Kinds = new List<Genre> { bookDbContext.Genre.Single(genre => genre.Name == "Drama") }

                }

            );


            var authors = new List<String>()
            {
                "mouhssine", "safae", "el mernissi", "amnay"
            };

            var length = bookDbContext.Genre.Count() - 1;


           


            // Vous pouvez initialiser la BDD ici

            bookDbContext.SaveChanges();
        }

        
    }
}