using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PaQuery.Extensions;

namespace Example.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        public class Movie
        {
            public string Title { get; set; }
            public string Director { get; set; }
        }

        private List<Movie> movies = new List<Movie>
        {
            new Movie { Title = "Vizontele", Director = "Yılmaz Erdoğan" },
            new Movie { Title = "The Wild Pear Tree", Director = "Nuri Bilge Ceylan" },
            new Movie { Title = "Once Upon a Time in Anatolia", Director = "Nuri Bilge Ceylan" },
            new Movie { Title = "Bulantı", Director = "Zeki Demirkubuz" },
            new Movie { Title = "Three Monkeys", Director = "Nuri Bilge Ceylan" },
            new Movie { Title = "Money Trap", Director = "Yılmaz Erdoğan" },
            new Movie { Title = "The Confession", Director = "Zeki Demirkubuz" },
            new Movie { Title = "Vizontele Tuuba", Director = "Yılmaz Erdoğan" },
            new Movie { Title = "Distant", Director = "Nuri Bilge Ceylan" },
            new Movie { Title = "The Destiny", Director = "Zeki Demirkubuz" },
            new Movie { Title = "The Butterfly's Dream", Director = "Yılmaz Erdoğan" },
            new Movie { Title = "Ember", Director = "Zeki Demirkubuz" },
        };
        private static readonly int moviePerPage = 3;

        [HttpGet]
        public IActionResult GetMovies(string title, string director, [Range(1, int.MaxValue)] int page = 1)
        {
            int totalPageCount = 0;

            #region basic filter
            IQueryable<Movie> queryableMovies = movies.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                queryableMovies = queryableMovies.Where(x => x.Title.ToLower().Contains(title));
            if (!string.IsNullOrEmpty(director))
                queryableMovies = queryableMovies.Where(x => x.Director.ToLower().Contains(director));

            movies = queryableMovies.ToList();
            #endregion

            #region basic pagination
            if (movies.Count > moviePerPage)
            {
                totalPageCount = movies.Count % moviePerPage != 0 ? movies.Count / moviePerPage + 1 : movies.Count / moviePerPage;
                movies = movies.Skip(moviePerPage * (page - 1)).Take(moviePerPage).ToList();
            }
            #endregion

            // * Example
            var paginationUrl = Request.CreatePaginationUrl(totalPageCount: totalPageCount, currentPage: page);

            var pagination = new
            {
                paginationUrl.Next,
                paginationUrl.Previous,
                current = page,
                total = totalPageCount
            };
            
            return Ok(new { pagination, movies });
        }
    }
}