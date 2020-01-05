using Crow.OrgChart.DataStorage;
using Crow.OrgChart.Helpers;
using Crow.OrgChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crow.OrgChart.Services
{
    public class OrganizationViewModelService : IOrganizationViewModelService
    {
        private readonly IOrganizationStorageRepository repo;

        public OrganizationViewModelService(IOrganizationStorageRepository repo)
        {
            this.repo = repo;
        }

        public OrganizationLevelViewModel GetOrganizationViewModel()
        {
            var organization = this.repo.GetOrganization();
            var childLevels = this.GetChildLevelModels(null);

            var model = new OrganizationLevelViewModel
            {
                LevelName = organization.Name,
                ChildLevels = childLevels
            };

            return model;
        }

        public OrganizationLevelViewModel GetLevelViewModel(Guid id)
        {
            // TODO: Use AutoMapper
            var level = this.repo.GetLevel(id);
            var organization = this.repo.GetOrganization();
            var childLevels = this.GetChildLevelModels(id);
            var members = level.Members.Select(x => new MemberListItemViewModel
            {
                Id = x.Id.Value,
                Hierarchy = x.Hierarchy,
                Name = x.Name,
                Role = x.Role,
                LevelId = x.LevelId,
                IsManager = x.IsManager
            });

            var model = new OrganizationLevelViewModel
            {
                Id = id,
                LevelName = level.Name,
                ChildLevels = childLevels,
                Members = members.OrderBy(x => x.IsManager).ThenBy(x => x.Hierarchy).ThenBy(x => x.Name),
                ParentLevels = LevelHelper.GetParentLevels(organization, level)
            };

            return model;
        }

        public List<OrganizationChartItemViewModel> GetChartViewModelItems()
        {
            var organization = this.repo.GetOrganization();
            var items = organization.OrganizationLevels.Select(x => new OrganizationChartItemViewModel
            {
                Id = x.Id.ToString(),
                Parent = (x.ParentId ?? Guid.Empty).ToString(),
                Name = x.Name
            }).ToList();

            items.Add(new OrganizationChartItemViewModel
            {
                Id = Guid.Empty.ToString(),
                Parent = string.Empty,
                Name = organization.Name
            });

            return items;
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
                                        Role = m.Role,
                                        LevelId = m.LevelId,
                                        IsManager = m.IsManager
                                    })
                            });
        }
    }
}
