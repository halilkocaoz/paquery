using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PaQuery.Extensions;

namespace Example.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    public class Movie
    {
        public string Title { get; set; }
        public string Director { get; set; }
    }

    private List<Movie> _movies = new()
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

    private const int MoviePerPage = 3;

    [HttpGet]
    public IActionResult GetMovies(string title, string director, [Range(1, int.MaxValue)] int page = 1)
    {
        var totalPageCount = 0;

        #region basic filter
        var queryableMovies = _movies.AsQueryable();

        if (!string.IsNullOrEmpty(title))
            queryableMovies = queryableMovies.Where(x => x.Title.ToLower().Contains(title));
        if (!string.IsNullOrEmpty(director))
            queryableMovies = queryableMovies.Where(x => x.Director.ToLower().Contains(director));

        _movies = queryableMovies.ToList();
        #endregion

        #region basic pagination
        if (_movies.Count > MoviePerPage)
        {
            totalPageCount = _movies.Count % MoviePerPage != 0 ? _movies.Count / MoviePerPage + 1 : _movies.Count / MoviePerPage;
            _movies = _movies.Skip(MoviePerPage * (page - 1)).Take(MoviePerPage).ToList();
        }
        #endregion

        // * Example
        var pagination = Request.CreatePagination(totalPageCount: totalPageCount, currentPage: page);
        
        return Ok(new { pagination, movies = _movies });
    }
}