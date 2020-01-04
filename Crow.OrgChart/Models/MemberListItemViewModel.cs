using System;

namespace Crow.OrgChart.Models
{
    public class MemberListItemViewModel
    {
        public Guid Id { get; set; }
        public Guid? LevelId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Hierarchy { get; set; }
    }
}
