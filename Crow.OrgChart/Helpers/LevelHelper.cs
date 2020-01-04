using Crow.OrgChart.DataStorage.Models;
using Crow.OrgChart.Models;
using System.Collections.Generic;
using System.Linq;

namespace Crow.OrgChart.Helpers
{
    public static class LevelHelper
    {
        public static List<OrganizationLevelViewModel> GetParentLevels(Organization organization, OrganizationLevel level)
        {
            var parentLevels = new List<OrganizationLevelViewModel>();
            var currentParentId = level.ParentId;

            while (currentParentId.HasValue)
            {
                var parent = organization.OrganizationLevels.Single(x => x.Id == currentParentId);
                currentParentId = parent.ParentId;
                parentLevels.Add(new OrganizationLevelViewModel
                {
                    Id = parent.Id,
                    LevelName = parent.Name
                });
            }

            parentLevels.Reverse();

            return parentLevels;
        }
    }
}
