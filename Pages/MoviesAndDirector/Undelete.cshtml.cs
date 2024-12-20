using DREXFINAL.Contrast;
using DREXFINAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DREXFINAL.Web.Pages.MoviesAndDirector
{
    public class UndeleteModel : PageModel
    {
        [BindProperty]
        public Movie? Item { get; set; }

        private readonly IMovieService _movieService;
        private readonly ILogger<UndeleteModel> _logger;

        public UndeleteModel(ILogger<UndeleteModel> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        public async Task OnGet(Guid? id = null)
        {
            if (id == null)
            {
                Item = null;
                return;
            }

            Item = await _movieService.GetByIdAsync(id.Value);
        }

        public async Task<IActionResult> OnPost()
        {
            if (Item == null)
            {
                return NotFound();
            }

            await _movieService.UndeleteAsync(Item.Id);
            return RedirectToPage("/MoviesAndDirector/Index");
        }
    }
}
