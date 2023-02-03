using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.Server.Database;

namespace ASP.Server.Api
{

    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext libraryDbContext;

        public BookController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        // Methode a ajouter : 
        // - GetBooks
        //   - Entrée: Optionel -> Liste d'Id de genre, limit -> defaut à 10, offset -> défaut à 0
        //     Le but de limit et offset est dé créer un pagination pour ne pas retourner la BDD en entier a chaque appel
        //   - Sortie: Liste d'object contenant uniquement: Auteur, Genres, Titre, Id, Prix
        //     la liste restourner doit être compsé des élément entre <offset> et <offset + limit>-
        //     Dans [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20] si offset=8 et limit=5, les élément retourner seront : 8, 9, 10, 11, 12

        // - GetBook
        //   - Entrée: Id du livre
        //   - Sortie: Object livre entier

        // - GetGenres
        //   - Entrée: Rien
        //   - Sortie: Liste des genres

        // Aide:
        // Pour récupéré un objet d'une table :
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.First()
        // Pour récupéré des objets d'une table :
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.ToList()
        // Pour faire une requète avec filtre:
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Skip().<Selecteurs>
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Take().<Selecteurs>
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Where(x => x == y).<Selecteurs>
        // Pour récupérer une 2nd table depuis la base:
        //   - .Include(x => x.yyyyy)
        //     ou yyyyy est la propriété liant a une autre table a récupéré
        //
        // Exemple:
        //   - Ex: libraryDbContext.MyObjectCollection.Include(x => x.yyyyy).Where(x => x.yyyyyy.Contains(z)).Skip(i).Take(j).ToList()


        // Je vous montre comment faire la 1er, a vous de la compléter et de faire les autres !
        public ActionResult<List<BookPublic>> GetBooks(int? genre, int? limit, int? offset)
        {
            IEnumerable<BookPublic> req = libraryDbContext.Books.Include(x => x.Kinds).Select(book => new BookPublic() { Book = book });
            var count = req.Count();
            if (genre.HasValue)
            {
                try
                {
                    var _genre = libraryDbContext.Genre.Single(_genre => _genre.Id.Equals(genre));
                    req = req.Where(book => {
                        if (book.Kinds != null) { return book.Kinds.Contains(_genre); }
                        return false;
                    });

                }
                catch (InvalidOperationException ex)
                {
                    return NotFound("Genre inexistant");
                }
                catch (Exception ex)
                {
                    return NotFound("Erreur lors de la recherche du genre.");
                }
            }
            if (offset.HasValue)
            {
                if (offset < 0)
                {
                    return NotFound("offset ne peut pas être négatif");
                }
                if (offset > count)
                {
                    offset = count;
                }
                req = req.Skip((int)offset);
            }
            if (limit.HasValue)
            {
                if (limit < 0)
                {
                    return NotFound("limit ne peut pas être négatif");
                }
                if (limit + offset > count)
                {
                    limit = count - offset;
                }
                req = req.Take((int)limit);
            }
            return req.ToList();

        }

        public ActionResult<Book> GetBook(int? id)
        {
            try
            {
                return libraryDbContext.Books.Include(x => x.Kinds).Single(book => book.Id == id);
            }
            catch (Exception e)
            {
                return NotFound("Id book not found");
            }
        }
        [Route("/api/[controller]/[action]")]
        [ApiController]
        public class GenreController : ControllerBase
        {
            private readonly LibraryDbContext libraryDbContext;

            public GenreController(LibraryDbContext libraryDbContext)
            {
                this.libraryDbContext = libraryDbContext;
            }

            public ActionResult<List<Genre>> GetGenres()
            {
                try
                {
                    return libraryDbContext.Genre.ToList();
                }
                catch (Exception e)
                {
                    return NotFound("aucun resultat...");
                }
            }
        }
    }
}


    

