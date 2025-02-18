using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moje_filmy_api.Data;
using moje_filmy_api.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace moje_filmy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly HttpClient _client;

        public MovieController(DataContext context)
        {
            _context = context;
            _client = new HttpClient();
        }

        [HttpGet("getMovies")]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            try
            {
                var movies = await _context.Movies.ToListAsync();
                if (movies.Count == 0)
                {
                    return NotFound();
                }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("getMovie/{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var movie = await _context.Movies.FindAsync(id);
                if (movie is null)
                {
                    return NotFound();
                }
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getNewMovies")]
        public async Task<ActionResult<List<Movie>>> GetNewMovies()
        {
            try
            {
                var apiUrl = "https://filmy.programdemo.pl/MyMovies";
                HttpResponseMessage response = await _client.GetAsync(apiUrl);
                if (!response. IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to load the data.");
                }
                var movies = JsonConvert.DeserializeObject<List<Movie>>(await response.Content.ReadAsStringAsync());

                if (movies.Count == 0)
                {
                    return NotFound();
                }
                var existingMovies = await _context.Movies.ToListAsync();
                var addedMovies = new List<Movie>();

                foreach (var movie in movies)
                {
                    if (existingMovies.Any(m => m.ExternalId == movie.Id))
                    {
                        continue;
                    }
                    var newMovie = new Movie
                    {
                        ExternalId = movie.Id,
                        Title = movie.Title,
                        Director = movie.Director,
                        Year = movie.Year,
                        Rate = movie.Rate
                    };
                    _context.Movies.Add(newMovie);
                    addedMovies.Add(newMovie);
                }
                await _context.SaveChangesAsync();
                return Ok(addedMovies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addMovie")]
        public async Task<ActionResult<Movie>> AddMovie(Movie movie)
        {
            try
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("editMovie")]
        public async Task<ActionResult<Movie>> EditMovie(Movie updatedMovie)
        {
            try
            {
                var movieToEdit = await _context.Movies.FindAsync(updatedMovie.Id);
                if (movieToEdit is null)
                {
                    return NotFound("Movie not found.");
                }
                movieToEdit.Title = updatedMovie.Title;
                movieToEdit.Director = updatedMovie.Director;
                movieToEdit.Year = updatedMovie.Year;
                movieToEdit.Rate = updatedMovie.Rate;

                await _context.SaveChangesAsync();
                return Ok(updatedMovie);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("deleteMovie/{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var movieToDelete = _context.Movies.Find(id);
                if (movieToDelete == null)
                {
                    return NotFound();
                }
                _context.Movies.Remove(movieToDelete);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    } 
}
