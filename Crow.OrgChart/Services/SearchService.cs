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

        public SearchService(IOrganizationStorageRepository repo)
        {
            this.repo = repo;
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
                .Select(x => new MemberListItemViewModel
                {
                    Id = x.Id.Value,
                    LevelId = x.LevelId,
                    Role = x.Role,
                    Name = x.Name
                });

            return users.ToList();
        }

        private string CleanPhrase(string phrase) => (phrase ?? "").Trim().ToLower();
    }
}
