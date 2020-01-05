using System.Collections.Generic;
using Crow.OrgChart.Models;

namespace Crow.OrgChart.Services
{
    public interface ISearchService
    {
        List<OrganizationLevelViewModel> SearchLevels(string phrase);
        List<MemberListItemViewModel> SearchUsers(string phrase);
    }
}