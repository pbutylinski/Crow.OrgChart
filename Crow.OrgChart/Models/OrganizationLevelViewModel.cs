using System;
using System.Collections.Generic;

namespace Crow.OrgChart.Models
{
    public class OrganizationLevelViewModel
    {
        public Guid? Id { get; set; }

        public string LevelName { get; set; }

        public IEnumerable<MemberListItemViewModel> Members { get; set; } = new List<MemberListItemViewModel>();

        public IEnumerable<OrganizationLevelViewModel> ChildLevels { get; set; } = new List<OrganizationLevelViewModel>();

        public IEnumerable<OrganizationLevelViewModel> ParentLevels { get; set; } = new List<OrganizationLevelViewModel>();
    }
}
