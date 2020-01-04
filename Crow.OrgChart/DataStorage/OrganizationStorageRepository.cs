using Crow.OrgChart.DataStorage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Crow.OrgChart.DataStorage
{
    public class OrganizationStorageRepository : IOrganizationStorageRepository
    {
        private const string JsonFileName = "data.json";

        public Organization GetOrganization()
        {
            if (File.Exists(JsonFileName))
            {
                var fileContents = File.ReadAllText(JsonFileName, Encoding.UTF8);
                return JsonConvert.DeserializeObject<Organization>(fileContents);
            }

            return new Organization();
        }

        public void SetOrganizationName(string name)
        {
            var organization = this.GetOrganization();
            organization.Name = name;
            this.Save(organization);
        }

        public OrganizationLevel GetLevel(Guid id)
        {
            return this.GetOrganization().OrganizationLevels
                .SingleOrDefault(x => x.Id == id && !x.IsDeleted);
        }

        public List<OrganizationLevel> GetChildLevels(Guid? parentId)
        {
            return this.GetOrganization().OrganizationLevels
                .Where(x => x.ParentId == parentId && !x.IsDeleted)
                .ToList();
        }

        public void AddLevel(OrganizationLevel level)
        {
            if (level.Id.HasValue)
            {
                throw new Exception("Id must be null for new entries");
            }

            if (level.IsDeleted)
            {
                throw new Exception("Cannot add a level already marked as deleted");
            }

            var organization = this.GetOrganization();
            level.Id = Guid.NewGuid();
            organization.OrganizationLevels.Add(level);
            this.Save(organization);
        }

        private void DeleteLevel(Guid levelId)
        {
            var organization = this.GetOrganization();
            var level = organization.OrganizationLevels.Single(x => x.Id == levelId && !x.IsDeleted);
            level.IsDeleted = true;
            this.Save(organization);
        }

        public void AddMember(MemberDetails member)
        {
            if (member.Id.HasValue)
            {
                throw new Exception("Id must be null for new entries");
            }

            if (member.IsDeleted)
            {
                throw new Exception("Cannot add a member already marked as deleted");
            }

            if (string.IsNullOrWhiteSpace(member.Name))
            {
                throw new Exception("Name cannot be empty");
            }

            var organization = this.GetOrganization();
            var level = organization.OrganizationLevels
                .Single(x => x.Id == member.LevelId && !x.IsDeleted);

            member.Id = Guid.NewGuid();
            level.Members.Add(member);

            this.Save(organization);
        }

        private void DeleteMember(Guid levelId, Guid memberId)
        {
            var organization = this.GetOrganization();
            var level = organization.OrganizationLevels.Single(x => x.Id == levelId && !x.IsDeleted);
            var member = level.Members.SingleOrDefault(x => x.Id == memberId && !x.IsDeleted);

            member.IsDeleted = true;

            this.Save(organization);
        }

        private void Save(Organization organization)
        {
            var fileContents = JsonConvert.SerializeObject(organization);
            File.WriteAllText(JsonFileName, fileContents);
        }
    }
}
