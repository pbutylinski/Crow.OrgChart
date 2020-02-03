namespace Crow.OrgChart.Services
{
    using AutoMapper;
    using DataStorage;
    using Helpers;
    using Microsoft.Extensions.Configuration;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OrganizationViewModelService : IOrganizationViewModelService
    {
        private readonly IOrganizationStorageRepository repo;
        private readonly IMapper mapper;
        private readonly string RootUrl;

        public OrganizationViewModelService(
            IOrganizationStorageRepository repo,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.RootUrl = configuration.GetValue<string>("RootUrl");
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
            var level = this.repo.GetLevel(id);
            var organization = this.repo.GetOrganization();
            var childLevels = this.GetChildLevelModels(id);
            var members = level.Members
                .Select(this.mapper.Map<MemberListItemViewModel>)
                .OrderByDescending(x => x.IsManager)
                .ThenBy(x => x.Hierarchy)
                .ThenBy(x => x.Name);

            var model = new OrganizationLevelViewModel
            {
                Id = id,
                LevelName = level.Name,
                ChildLevels = childLevels,
                Members = members,
                ParentLevels = LevelHelper.GetParentLevels(organization, level)
            };

            return model;
        }

        public List<OrganizationChartItemViewModel> GetChartViewModelItems()
        {
            var organization = this.repo.GetOrganization();
            var items = organization.OrganizationLevels
                .Select(this.mapper.Map<OrganizationChartItemViewModel>)
                .ToList();

            foreach (var item in items)
            {
                item.Url = $"{RootUrl}/Organization/Level/{item.Id}";
            }

            items.Add(new OrganizationChartItemViewModel
            {
                Id = Guid.Empty.ToString(),
                Parent = string.Empty,
                Name = organization.Name,
                Url = RootUrl + "/Organization"
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
                                    .Select(this.mapper.Map<OrganizationLevelViewModel>),
                                Members = x.Members
                                    .OrderBy(x => x.IsManager)
                                    .ThenBy(x => x.Hierarchy)
                                    .ThenBy(x => x.Name)
                                    .Select(this.mapper.Map<MemberListItemViewModel>)
                            });
        }
    }
}
