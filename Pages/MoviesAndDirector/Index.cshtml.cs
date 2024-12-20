using DREXFINAL.Contrast;
using DREXFINAL.Models;
using DREXFINAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DREXFINAL.Web.Pages.MoviesAndDirector
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMovieService _movieService;  // Changed to IMovieService
        private readonly IDirectorService _directorService;  // Changed to IDirectorService

        public List<Movie> Movies { get; set; }

        [BindProperty]
        public SearchParameters? SearchParams { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IMovieService movieService,  // Constructor adjusted to inject services
            IDirectorService directorService)  // Constructor adjusted to inject services
        {
            _logger = logger;
            _movieService = movieService;
            _directorService = directorService;
        }

        public async Task OnGetAsync(
            string? keyword = "",
            string? searchBy = "",
            string? sortBy = null,
            string? sortAsc = "true",
            int pageSize = 5,
            int pageIndex = 1,
            bool showSoftDeleted = false)
        {
            if (SearchParams == null)
            {
                SearchParams = new SearchParameters()
                {
                    SortBy = sortBy,
                    SortAsc = sortAsc == "true",
                    SearchBy = searchBy,
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    IsDeleted = showSoftDeleted
                };
            }

            var movies = await _movieService.GetAllAsync();  // Use service method to get movies

            // Filter soft-deleted movies if applicable
            if (SearchParams.IsDeleted)
            {
                movies = movies.Where(m => m.IsDeleted).ToList();
            }

            // Apply search filters
            if (!string.IsNullOrEmpty(SearchParams.Keyword))
            {
                if (SearchParams.SearchBy?.ToLower() == "title")
                {
                    movies = movies.Where(m => m.Title?.ToLower().Contains(SearchParams.Keyword.ToLower()) == true).ToList();
                }
                else if (SearchParams.SearchBy?.ToLower() == "director")
                {
                    movies = movies.Where(m => m.Director?.Name?.ToLower().Contains(SearchParams.Keyword.ToLower()) == true).ToList();
                }
                else if (SearchParams.SearchBy?.ToLower() == "genre")
                {
                    movies = movies.Where(m => m.Genre?.ToLower().Contains(SearchParams.Keyword.ToLower()) == true).ToList();
                }
            }

            // Apply sorting
            if (SearchParams.SortBy != null)
            {
                if (SearchParams.SortBy.ToLower() == "title" && SearchParams.SortAsc == true)
                {
                    movies = movies.OrderBy(m => m.Title).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "title" && SearchParams.SortAsc == false)
                {
                    movies = movies.OrderByDescending(m => m.Title).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "director" && SearchParams.SortAsc == true)
                {
                    movies = movies.OrderBy(m => m.Director?.Name).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "director" && SearchParams.SortAsc == false)
                {
                    movies = movies.OrderByDescending(m => m.Director?.Name).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "genre" && SearchParams.SortAsc == true)
                {
                    movies = movies.OrderBy(m => m.Genre).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "genre" && SearchParams.SortAsc == false)
                {
                    movies = movies.OrderByDescending(m => m.Genre).ToList();
                }
            }

            // Paging logic
            int totalMovies = movies.Count();
            int totalPages = (int)Math.Ceiling((double)totalMovies / (SearchParams.PageSize ?? 5));

            int skip = ((SearchParams.PageIndex ?? 1) - 1) * (SearchParams.PageSize ?? 5);
            Movies = movies.Skip(skip).Take(SearchParams.PageSize ?? 5).ToList();

            SearchParams.PageCount = totalPages;
        }

        // Search parameter model for filtering, sorting, and pagination
        public class SearchParameters
        {
            public string? SearchBy { get; set; }
            public string? Keyword { get; set; }
            public string? SortBy { get; set; }
            public bool? SortAsc { get; set; }
            public int? PageSize { get; set; }
            public int? PageIndex { get; set; }
            public int? PageCount { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}
