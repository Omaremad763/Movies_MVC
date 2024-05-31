using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Movies_CRUD_MVC.Entites;
using Movies_CRUD_MVC.Models;
using Movies_CRUD_MVC.VewModels;
using NToastNotify;

namespace Movies_CRUD_MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Movies_Context _Context;
        private readonly IToastNotification _toast;
        private List<string> _AllowedExtentions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedPosterSize = 10048567;
        public MoviesController(Movies_Context context, IToastNotification toast)

        {
            _Context = context;
            _toast = toast;

        }
        public async Task<IActionResult> Index()
        {
            var movies = await _Context.Movies.OrderByDescending(m => m.rate).ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieFormViewModel
            {
                Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync()
            };
            return View("MovieForm", ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync();
                return View("MovieForm", model);
            }

            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync();
                ModelState.AddModelError("poster", "Please select movie poster");
                return View("MovieForm", model);
            }
            var poster = files.FirstOrDefault();
            if (!_AllowedExtentions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync();
                ModelState.AddModelError("poster", "add only jpg and png");
                return View("MovieForm", model);
            }
            if (poster.Length > _MaxAllowedPosterSize)
            {
                model.Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync();
                ModelState.AddModelError("poster", "poster can be more than 1 MB");
                return View("MovieForm",model);
            }
           

            using var  datastream = new MemoryStream();

            await poster.CopyToAsync(datastream);

            var movie = new Movie
            {
           
                Title = model.Title,
                GenreId = model.GenreId,
                year = model.year,
                rate = model.rate,
                Storyline = model.Storyline,
                Poster = datastream.ToArray()
            };

            _Context.Movies.Add(movie);
            _Context.SaveChanges();

            _toast.AddSuccessToastMessage("Movie created successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var movie = await _Context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var ViewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                year = movie.year,
                rate = movie.rate,
                Storyline = movie.Storyline,
                Poster = movie.Poster,
                Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync()
            };

            return View("MovieForm", ViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync();
                return View(model);
            }

            var movie = await _Context.Movies.FindAsync(model.Id);

            if (movie == null)
            {
                return NotFound();
            }

            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                using var datastream = new MemoryStream();
                await poster.CopyToAsync(datastream);
                model.Poster = datastream.ToArray();

                if (!_AllowedExtentions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Genres = await _Context.Genres.OrderBy(g => g.name).ToListAsync();
                    ModelState.AddModelError("poster", "add only jpg and png");
                    return View(model);
                }
                if (poster.Length > _MaxAllowedPosterSize)
                {
                    model.Genres = await _Context.Genres.OrderBy(n => n.name).ToListAsync();
                    ModelState.AddModelError("poster", "poster can be more than 1 MB");
                    return View(model);
                }
                movie.Poster = model.Poster;
            }

            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.year = model.year;
            movie.rate = model.rate;
            movie.Storyline = model.Storyline;
            //movie.Poster = datastream.ToArray()
            _Context.SaveChanges();
            _toast.AddSuccessToastMessage("Movie updated successfully");
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int ? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var movie = await _Context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m=>m.Id==id);

            if (movie == null)
            {
                return NotFound();


            }
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var movie = await _Context.Movies.FindAsync( id);

            if (movie == null)
            {
                return NotFound(); 
            }

            _Context.Movies.Remove(movie);
            _Context.SaveChanges();
            
            
            return Ok();
        }
    }
}
