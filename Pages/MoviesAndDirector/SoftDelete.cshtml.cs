using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DREXFINAL.Models; // Make sure this includes Movie model
using DREXFINAL.Services;
using DREXFINAL.Contrast; // Update with your services

namespace DREXFINAL.Web.Pages.MoviesAndDirector
{
    public class SoftDeleteModel : PageModel
    {
        [BindProperty]
        public Movie Item { get; set; }

        private readonly IMovieService _movieService;

        public SoftDeleteModel(IMovieService movieService)
        {
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

            await _movieService.SoftDeleteAsync(Item.Id); 
            return RedirectToPage("/MoviesAndDirector/Index"); 
        }
    }
}
