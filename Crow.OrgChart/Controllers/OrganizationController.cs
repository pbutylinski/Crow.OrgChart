using Crow.OrgChart.DataStorage;
using Crow.OrgChart.DataStorage.Models;
using Crow.OrgChart.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crow.OrgChart.Controllers
{
    [Route("organization")]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationStorageRepository repo;

        public OrganizationController(IOrganizationStorageRepository repo)
        {
            this.repo = repo;
        }

        [Route("")]
        public IActionResult Index()
        {
            var organization = this.repo.GetOrganization();
            var childLevels = this.GetChildLevelModels(null);

            var model = new OrganizationLevelViewModel
            {
                LevelName = organization.Name,
                ChildLevels = childLevels
            };

            return View("Level", model);
        }

        [Route("level/{id}")]
        public IActionResult Level(Guid id)
        {
            // TODO: Use AutoMapper
            OrganizationLevelViewModel parentLevel = null;

            var level = this.repo.GetLevel(id);
            var childLevels = this.GetChildLevelModels(id);
            var members = level.Members.Select(x => new MemberListItemViewModel
            {
                Id = x.Id.Value,
                Hierarchy = x.Hierarchy,
                Name = x.Name,
                Role = x.Role
            });

            if (level.ParentId.HasValue)
            {
                var parent = this.repo.GetLevel(level.ParentId.Value);
                parentLevel = new OrganizationLevelViewModel
                {
                    Id = parent.Id,
                    LevelName = parent.Name
                };
            }

            var model = new OrganizationLevelViewModel
            {
                Id = id,
                LevelName = level.Name,
                ChildLevels = childLevels,
                Members = members.OrderBy(x => x.Hierarchy).ThenBy(x => x.Name),
                ParentLevel = parentLevel
            };

            return View(model);
        }

        [Route("level/{parentId?}/add/{name}")]
        public IActionResult AddLevel(Guid? parentId, string name)
        {
            var level = new OrganizationLevel
            {
                ParentId = parentId,
                Name = name
            };

            this.repo.AddLevel(level);

            return Json(level.Id);
        }

        [Route("level/{levelId}/members/add/{name}")]
        public IActionResult AddMember(Guid levelId, string name)
        {
            var member = new MemberDetails
            {
                Name = name
            };

            this.repo.AddMember(member, levelId);

            return Json(member.Id);
        }

        private IEnumerable<OrganizationLevelViewModel> GetChildLevelModels(Guid? parentLevelId)
        {
            return this.repo.GetChildLevels(parentLevelId)
                            .OrderBy(x => x.Name)
                            .Select(x => new OrganizationLevelViewModel
                            {
                                Id = x.Id.Value,
                                LevelName = x.Name,
                                ChildLevels = this.repo
                                    .GetChildLevels(x.Id)
                                    .OrderBy(x => x.Name)
                                    .Select(c => new OrganizationLevelViewModel
                                    {
                                        Id = c.Id.Value,
                                        LevelName = c.Name
                                    }),
                                Members = x.Members
                                    .OrderBy(x => x.Hierarchy)
                                    .Select(m => new MemberListItemViewModel
                                    {
                                        Id = m.Id.Value,
                                        Hierarchy = m.Hierarchy,
                                        Name = m.Name,
                                        Role = m.Role
                                    })
                            });
        }
    }
}