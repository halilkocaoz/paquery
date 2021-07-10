using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PaQuery;
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
            #region basic filter
            if (!string.IsNullOrEmpty(title))
                movies = movies.Where(x => x.Title.ToLower().Contains(title)).ToList();
            if (!string.IsNullOrEmpty(director))
                movies = movies.Where(x => x.Director.ToLower().Contains(director)).ToList();
            #endregion

            #region basic pagination
            var totalPageCount = 1;
            if (movies.Count > moviePerPage)
            {
                totalPageCount = movies.Count % moviePerPage != 0 ? movies.Count / moviePerPage + 1 : movies.Count / moviePerPage;
                movies = movies.Skip(moviePerPage * (page - 1)).Take(moviePerPage).ToList();
            }
            #endregion

            // *
            var paginationUrl = Request.Query.PaginationUrls(hostPath: $"https://{Request.Host}{Request.Path}", totalPageCount, currentPage: page);

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