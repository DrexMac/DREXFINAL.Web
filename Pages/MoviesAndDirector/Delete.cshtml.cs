using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DREXFINAL.Models;
using DREXFINAL.Contrast;

namespace DREXFINAL.Web.Pages.MoviesAndDirector
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Movie? Item { get; set; }

        private readonly IMovieService _movieService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ILogger<DeleteModel> logger, IMovieService movieService)
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

            await _movieService.DeleteAsync(Item.Id);
            return RedirectToPage("/MoviesAndDirector/Index");
        }
    }
}
