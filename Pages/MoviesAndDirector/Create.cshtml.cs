using DREXFINAL.Contrast;
using DREXFINAL.Models;
using DREXFINAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DREXFINAL.Web.Pages.MoviesAndDirector
{
    public class CreateModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IDirectorService _directorService;
        [BindProperty]
        public CreateMovieViewModel Input { get; set; }

        public CreateModel(IMovieService movieService, IDirectorService directorService)
        {
            _movieService = movieService;
            _directorService = directorService;
        }

        public async Task<IActionResult> OnGet()
        {
            var directors = await _directorService.GetAllAsync();

            // Ensure we handle any missing director names
            var directorsWithNames = directors.Select(d => new Director
            {
                Id = d.Id,
                Name = string.IsNullOrEmpty(d.Name) ? "Unnamed Director" : d.Name,
            }).ToList();

            ViewData["Directors"] = directorsWithNames;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var directors = await _directorService.GetAllAsync();
                ViewData["Directors"] = directors;
                return Page();
            }

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = Input.Title,
                Genre = Input.Genre,
                ReleaseDate = Input.ReleaseDate,
                Director = new Director { Id = Input.DirectorId }
            };

            await _movieService.AddAsync(movie);
            return RedirectToPage("/MoviesAndDirector/Index");
        }
    }

    public class CreateMovieViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid DirectorId { get; set; }
    }
}
