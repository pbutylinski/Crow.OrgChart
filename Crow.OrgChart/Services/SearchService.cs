using AutoMapper;
using Crow.OrgChart.DataStorage;
using Crow.OrgChart.Helpers;
using Crow.OrgChart.Models;
using System.Collections.Generic;
using System.Linq;

namespace Crow.OrgChart.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrganizationStorageRepository repo;
        private readonly IMapper mapper;

        public SearchService(
            IOrganizationStorageRepository repo,
            IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public List<OrganizationLevelViewModel> SearchLevels(string phrase)
        {
            var organization = this.repo.GetOrganization();
            var phraseClean = this.CleanPhrase(phrase);
            var levels = organization.OrganizationLevels
                .Where(x => x.Name.ToLower().Contains(phraseClean))
                .Select(x => new OrganizationLevelViewModel
                {
                    Id = x.Id,
                    LevelName = x.Name,
                    ParentLevels = LevelHelper.GetParentLevels(organization, x)
                });

            return levels.ToList();
        }

        public List<MemberListItemViewModel> SearchUsers(string phrase)
        {
            var organization = this.repo.GetOrganization();
            var phraseClean = this.CleanPhrase(phrase);
            var users = organization.OrganizationLevels
                .SelectMany(x => x.Members)
                .Where(x => x.Name.ToLower().Contains(phraseClean))
                .Select(this.mapper.Map<MemberListItemViewModel>)
                .OrderBy(x => x.Name)
                .ToList();

            return users;
        }

        private string CleanPhrase(string phrase) => (phrase ?? "").Trim().ToLower();
    }
}
