using Crow.OrgChart.DataStorage;
using Crow.OrgChart.DataStorage.Models;
using Crow.OrgChart.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Crow.OrgChart.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationStorageRepository repo;
        private readonly IOrganizationViewModelService viewModelService;

        public OrganizationController(
            IOrganizationStorageRepository repo,
            IOrganizationViewModelService viewModelService)
        {
            this.repo = repo;
            this.viewModelService = viewModelService;
        }

        public IActionResult Index()
        {
            var model = this.viewModelService.GetOrganizationViewModel();
            return View("Organization", model);
        }

        [HttpGet]
        public IActionResult Level(Guid id)
        {
            var model = this.viewModelService.GetLevelViewModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveLevel(OrganizationLevel model)
        {
            if (model.Id.HasValue)
            {
                this.repo.UpdateLevel(model);
            }
            else
            {
                this.repo.AddLevel(model);
            }

            return RedirectToAction("Level", new { id = model.Id });
        }

        [HttpGet]
        public IActionResult EditOrganization()
        {
            var model = this.repo.GetOrganization();
            return View(model);
        }

        [HttpPost]
        public IActionResult EditOrganization(Organization model)
        {
            this.repo.SetOrganizationName(model.Name);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditLevel(Guid id)
        {
            var level = this.repo.GetLevel(id);
            return View("LevelEdit", level);
        }

        [HttpGet]
        public IActionResult CreateLevel(Guid? parentId)
        {
            var level = new OrganizationLevel { ParentId = parentId };
            return View("LevelEdit", level);
        }

        [HttpGet]
        public IActionResult AddMember(Guid levelId)
        {
            var member = new MemberDetails
            {
                LevelId = levelId
            };

            return View("MemberEdit", member);
        }

        [HttpGet]
        public IActionResult EditMember(Guid levelId, Guid memberId)
        {
            var level = this.repo.GetLevel(levelId);
            var member = level.Members.SingleOrDefault(x => x.Id == memberId);

            return View("MemberEdit", member);
        }

        [HttpPost]
        public IActionResult SaveMember(MemberDetails member)
        {
            if (member.Id.HasValue)
            {
                this.repo.UpdateMember(member);
            }
            else
            {
                this.repo.AddMember(member);
            }

            return RedirectToAction("Level", new { id = member.LevelId });
        }

        [HttpGet]
        public IActionResult Member(Guid levelId, Guid memberId)
        {
            var level = this.repo.GetLevel(levelId);
            var member = level.Members.SingleOrDefault(x => x.Id == memberId);
            return View(member);
        }

        [HttpGet] 
        public IActionResult DeleteLevel(Guid levelId)
        {
            this.repo.DeleteLevel(levelId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteMember(Guid levelId, Guid memberId)
        {
            this.repo.DeleteMember(levelId, memberId);
            return RedirectToAction("Level", new { id = levelId });
        }
    }
}