using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DREXFINAL.Models;
using DREXFINAL.Services;
using DREXFINAL.Contrast;

namespace DREXFINAL.Web.Pages.MoviesAndDirector
{
    public class UpdateModel : PageModel
    {
        private readonly IMovieService _movieService;

        [BindProperty]
        public Movie Movie { get; set; }

        public UpdateModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _movieService.GetByIdAsync(id.Value);

            if (Movie == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Request.Form["addDirector"] == "on")
            {
                var newDirectorName = Request.Form["NewDirectorName"];

                if (!string.IsNullOrEmpty(newDirectorName))
                {
                    var newDirector = new Director
                    {
                        Name = newDirectorName
                    };

                    Movie.Director = newDirector;
                }
            }

            await _movieService.UpdateAsync(Movie);

            return RedirectToPage("/MoviesAndDirector/Index");
        }
    }
}
