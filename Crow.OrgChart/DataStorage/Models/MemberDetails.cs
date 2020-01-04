using System;
using System.ComponentModel;

namespace Crow.OrgChart.DataStorage
{
    public class MemberDetails
    {
        public Guid? Id { get; set; }

        public Guid LevelId { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        [DisplayName("Contact info")]
        public string ContactInfo { get; set; }

        public string Notes { get; set; }

        public int Hierarchy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
