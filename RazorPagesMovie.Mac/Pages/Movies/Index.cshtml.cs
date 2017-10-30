using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Mac.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Models.MovieContext _context;

        public IndexModel(RazorPagesMovie.Models.MovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }
        public SelectList Genres;
        public string MovieGenre { get; set; }
        
        /*
        public async Task OnGetAsync()
        {
            Movie = await _context.Movie.ToListAsync();
        }
        */

        // Search for a movie by title
        public async Task OnGetAsync(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                    orderby m.Genre
                                    select m.Genre;

            var movies = from m in _context.Movie
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

            // Return the movie with the specified title, or 
            // all movies in the database if no title was specified
            Movie = await movies.ToListAsync();
        }
    }
}
