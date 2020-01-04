using System;
using System.Collections.Generic;

namespace Crow.OrgChart.DataStorage.Models
{
    public class OrganizationLevel
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public List<MemberDetails> Members { get; set; } = new List<MemberDetails>();

        public bool IsDeleted { get; set; }
    }
}
