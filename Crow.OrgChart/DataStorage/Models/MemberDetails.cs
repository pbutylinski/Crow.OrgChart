using System;

namespace Crow.OrgChart.DataStorage
{
    public class MemberDetails
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string ContactInfo { get; set; }

        public int Hierarchy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
