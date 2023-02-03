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


    public class CreateGenreModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public String Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

    }


    public class GenreController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;

        public GenreController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }


        public ActionResult<IEnumerable<Genre>> List() {
            List<Genre> ListGenre = libraryDbContext.Genre.ToList();

            return View(ListGenre);
        }

        public ActionResult<Boolean> RedirectUpdateGenre(int genreId)
        {
            return RedirectToAction("Update", new { genreId = genreId });
        }

        //mettre à jour un genre de livre 

        public ActionResult<Boolean> Update(int genreId, CreateGenreModel genre)
        {


            // fetch book with id
            var g = libraryDbContext.Genre.Single(g => g.Id == genreId);
            // get new data
            if (ModelState.IsValid)
            {
                g.Name = genre.Name;
                g.Description = genre.Description;

                


                libraryDbContext.Genre.Update(g);
                libraryDbContext.SaveChanges();

                return RedirectToAction("List");


            }
            return View(new CreateGenreModel() { Id = genreId });

        }


        //suprimmer un genre

        public ActionResult<Boolean> RemoveGenre(int genreId)
        {

            var genre = libraryDbContext.Genre.Single(g => g.Id == genreId);
            Console.WriteLine("genre name " + genre.Name);
            libraryDbContext.Genre.Remove(genre);
            libraryDbContext.SaveChanges();

            return RedirectToAction("List");
        }


        // creer un genre 

        public ActionResult<CreateGenreModel> Create(CreateGenreModel genre)
        {
            if (ModelState.IsValid)
            {



                Genre g = new Genre()
                {
                    Name = genre.Name,
                    Description = genre.Description,


                };

                try
                {
                    libraryDbContext.Genre.Add(g);
                    libraryDbContext.SaveChanges();
                    return RedirectToAction("List");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + "error man");
                    throw;
                }
            }

            return View(new CreateGenreModel());
        }


    }
}
