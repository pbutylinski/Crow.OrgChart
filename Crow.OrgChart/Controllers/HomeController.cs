using Crow.OrgChart.DataStorage;
using Crow.OrgChart.Models;
using Crow.OrgChart.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace Crow.OrgChart.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrganizationStorageRepository repo;
        private readonly IOrganizationViewModelService viewModelService;
        private readonly ISearchService searchService;

        public HomeController(
            IOrganizationStorageRepository repo,
            IOrganizationViewModelService viewModelService,
            ISearchService searchService)
        {
            this.repo = repo;
            this.viewModelService = viewModelService;
            this.searchService = searchService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var organization = this.repo.GetOrganization();
            var model = new IndexViewModel { OrganizationName = organization.Name };
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string phrase)
        {
            var levels = this.searchService.SearchLevels(phrase);
            var users = this.searchService.SearchUsers(phrase);

            var model = new SearchResultViewModel
            {
                Phrase = phrase,
                Levels = levels.OrderBy(x => x.LevelName),
                Members = users.OrderBy(x => x.Name)
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChartData()
        {
            var items = this.viewModelService.GetChartViewModelItems();
            return Json(items.ToList());
        }
    }
}
