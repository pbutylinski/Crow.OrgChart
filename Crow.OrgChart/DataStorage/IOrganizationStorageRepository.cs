using System;
using System.Collections.Generic;
using Crow.OrgChart.DataStorage.Models;

namespace Crow.OrgChart.DataStorage
{
    public interface IOrganizationStorageRepository
    {
        void AddLevel(OrganizationLevel level);
        void AddMember(MemberDetails member);
        List<OrganizationLevel> GetChildLevels(Guid? parentId);
        OrganizationLevel GetLevel(Guid id);
        Organization GetOrganization();
        void SetOrganizationName(string name);
    }
}