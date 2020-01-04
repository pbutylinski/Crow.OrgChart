using System.Collections.Generic;

namespace Crow.OrgChart.Models
{
    public class SearchResultViewModel
    {
        public string Phrase { get; set; }
        public IEnumerable<MemberListItemViewModel> Members { get; set; }
        public IEnumerable<OrganizationLevelViewModel> Levels { get; set; }
    }
}
