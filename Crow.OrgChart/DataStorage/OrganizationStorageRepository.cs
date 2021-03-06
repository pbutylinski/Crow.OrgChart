﻿using Crow.OrgChart.DataStorage.Models;
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

        private static Organization OrganizationCache;

        public Organization GetOrganization()
        {
            if (OrganizationCache == null)
            {
                this.LoadFile();
            }

            return OrganizationCache ?? new Organization();
        }

        public void SetOrganizationName(string name)
        {
            var organization = this.GetOrganization();
            organization.Name = name;
            this.SaveFile(organization);
        }

        public OrganizationLevel GetLevel(Guid id)
        {
            return this.GetOrganization().OrganizationLevels
                .SingleOrDefault(x => x.Id == id);
        }

        public List<OrganizationLevel> GetChildLevels(Guid? parentId)
        {
            return this.GetOrganization().OrganizationLevels
                .Where(x => x.ParentId == parentId)
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
            this.SaveFile(organization);
        }

        public void DeleteLevel(Guid levelId)
        {
            var organization = this.GetOrganization();
            var level = organization.OrganizationLevels.Single(x => x.Id == levelId);
            level.IsDeleted = true;

            this.SaveFile(organization);

            foreach (var item in organization.OrganizationLevels.Where(x => x.ParentId == levelId))
            {
                this.DeleteLevel(item.Id.Value);
            }
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

            this.SaveFile(organization);
        }

        public void DeleteMember(Guid levelId, Guid memberId)
        {
            var organization = this.GetOrganization();
            var level = organization.OrganizationLevels.Single(x => x.Id == levelId);
            var member = level.Members.SingleOrDefault(x => x.Id == memberId);

            member.IsDeleted = true;

            this.SaveFile(organization);
        }

        public void UpdateLevel(OrganizationLevel level)
        {
            var organization = this.GetOrganization();
            var oldLevel = organization.OrganizationLevels
                .Single(x => x.Id == level.Id);

            oldLevel.Name = level.Name;

            this.SaveFile(organization);
        }

        public void UpdateMember(MemberDetails member)
        {
            var organization = this.GetOrganization();
            var oldLevel = organization.OrganizationLevels
                .Single(x => x.Id == member.LevelId);

            var oldMember = oldLevel.Members
                .Single(x => x.Id == member.Id);

            oldMember.ContactInfo = member.ContactInfo;
            oldMember.Hierarchy = member.Hierarchy;
            oldMember.Name = member.Name;
            oldMember.Notes = member.Notes;
            oldMember.Role = member.Role;
            oldMember.IsManager = member.IsManager;

            this.SaveFile(organization);
        }

        private void LoadFile()
        {
            if (File.Exists(JsonFileName))
            {
                var fileContents = File.ReadAllText(JsonFileName, Encoding.UTF8);
                var organization = JsonConvert.DeserializeObject<Organization>(fileContents);

                this.FilterSoftDeleteFlag(organization);

                OrganizationCache = organization;
            }
        }

        private void SaveFile(Organization organization)
        {
            var fileContents = JsonConvert.SerializeObject(organization);
            File.WriteAllText(JsonFileName, fileContents);
            this.LoadFile();
        }

        private void FilterSoftDeleteFlag(Organization organization)
        {
            organization.OrganizationLevels = organization.OrganizationLevels
                .Where(x => !x.IsDeleted)
                .ToList();

            foreach (var item in organization.OrganizationLevels)
            {
                item.Members = item.Members.Where(x => !x.IsDeleted).ToList();
            }
        }
    }
}
