using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ASP.Server.Controllers
{
    public class CreateBookModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public String Name { get; set; }
        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Price")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        // Ajouter ici tous les champ que l'utilisateur devra remplir pour ajouter un livre

        // Liste des genres séléctionné par l'utilisateur
        public List<int> Genres { get; set; }

        // Liste des genres a afficher à l'utilisateur
        public IEnumerable<Genre> AllGenres { get; init; }
    }

    public class BookController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;


        public BookController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        public ActionResult<IEnumerable<Book>> List()
        {

            // récupérer les livres dans la base de donées pour qu'elle puisse être affiché

            List<Book> ListBooks = libraryDbContext.Books.Include(g => g.Kinds).ToList();

            return View(ListBooks);
        }

        public ActionResult<CreateBookModel> Create(CreateBookModel book)
        {
            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis
            if (ModelState.IsValid)
            {
                try
                {
                    // Il faut intéroger la base pour récupérer l'ensemble des objets genre qui correspond aux id dans CreateBookModel.Genres
                    List<Genre> genres = new List<Genre>();
                    foreach (var item in book.Genres)
                    {
                        var genre = libraryDbContext.Genre.Single(_genre => _genre.Id.Equals(item));
                        genres.Add(genre);

                    }
                    Console.WriteLine("test Genres");
                    //Console.WriteLine(book.Genres.ToList());

                    // Completer la création du livre avec toute les information nécéssaire que vous aurez ajoutez, et metter la liste des gener récupéré de la base aussi
                    Book b = new Book()
                    {
                        Name = book.Name,
                        Author = book.Author,
                        Price = book.Price,
                        Content = book.Content,
                        Kinds = genres
                    };

                    try
                    {
                        libraryDbContext.Add(b);
                        libraryDbContext.SaveChanges();
                        return RedirectToAction("List");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e + "error man");
                        throw;
                    }
                }
                catch (Exception e)
                {
                    return NotFound("Le prix indiqué n'est pas valide");
                }
            }

            // Il faut interoger la base pour récupérer tous les genres, pour que l'utilisateur puisse les slécétionné
            return View(new CreateBookModel() { AllGenres = libraryDbContext.Genre.ToList() });
        }
        //  deletebook page
        public ActionResult<IEnumerable<Book>> Action()
        {
            List<Book> ListBooks = libraryDbContext.Books.Include(g => g.Kinds).ToList();
            Console.WriteLine("ok Delete page !");


            return View(ListBooks);
        }


        //remove book

        public ActionResult<Boolean> RemoveBook(int bookId)
        {

            var book = libraryDbContext.Books.Single(book => book.Id == bookId);
            Console.WriteLine("book name " + book.Name);
            libraryDbContext.Books.Remove(book);
            libraryDbContext.SaveChanges();

            return RedirectToAction("List");
        }


        // simple redirect to methode update book

        public ActionResult<Boolean> RedirectUpdateBook(int bookId)
        {
            return RedirectToAction("Update", new { bookId = bookId });
        }


        // methode permet de mettre a jour un book
        public ActionResult<Boolean> Update(int bookId, CreateBookModel book)
        {
            // fetch book with id
            var b = libraryDbContext.Books.Single(book => book.Id == bookId);
            // get new data
            if (ModelState.IsValid)
            {
                List<Genre> genres = new List<Genre>();
                foreach (var item in book.Genres)
                {
                    var genre = libraryDbContext.Genre.Single(_genre => _genre.Id.Equals(item));
                    genres.Add(genre);
                }
                b.Name = book.Name;
                b.Author = book.Author;
                b.Price = book.Price;
                b.Content = book.Content;
                b.Kinds = genres;

                libraryDbContext.Update(b);
                libraryDbContext.SaveChanges();

                return RedirectToAction("List");
            }
            return View(new CreateBookModel() { Id = bookId, AllGenres = libraryDbContext.Genre.ToList() });
        }
    }
}
