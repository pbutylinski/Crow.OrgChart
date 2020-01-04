using Crow.OrgChart.DataStorage;
using Crow.OrgChart.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace Crow.OrgChart.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrganizationStorageRepository repo;

        public HomeController(IOrganizationStorageRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            var organization = this.repo.GetOrganization();
            var model = new IndexViewModel
            {
                OrganizationName = organization.Name
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
            var organization = this.repo.GetOrganization();
            var items = organization.OrganizationLevels.Select(x => new
            {
                id = x.Id.Value,
                parent = (x.ParentId ?? Guid.Empty).ToString(),
                name = x.Name
            }).ToList();

            items.Add(new
            {
                id = Guid.Empty,
                parent = string.Empty,
                name = organization.Name
            });

            return Json(items.ToList());
        }
    }
}
