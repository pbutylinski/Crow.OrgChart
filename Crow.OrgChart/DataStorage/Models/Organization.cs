using System.Collections.Generic;

namespace Crow.OrgChart.DataStorage.Models
{
    public class Organization
    {
        public string Name { get; set; } = "New organization";

        public List<OrganizationLevel> OrganizationLevels { get; set; } = new List<OrganizationLevel>();
    }
}
