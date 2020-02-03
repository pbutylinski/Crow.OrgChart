namespace Crow.OrgChart
{
    using AutoMapper;
    using DataStorage;
    using DataStorage.Models;
    using Models;
    using System;
    using System.Linq;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            this.CreateMap<MemberListItemViewModel, MemberDetails>();
            this.CreateMap<MemberDetails, MemberListItemViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id.Value));

            this.CreateMap<OrganizationLevel, OrganizationChartItemViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id.ToString()))
                .ForMember(x => x.Parent, opt => opt.MapFrom(y => (y.ParentId ?? Guid.Empty).ToString()))
                .ForMember(x => x.Members, opt => opt.MapFrom(y => y.Members.Where(m => !m.IsManager).Select(m => m.Name).OrderBy(m => m)))
                .ForMember(x => x.Managers, opt => opt.MapFrom(y => y.Members.Where(m => m.IsManager).Select(m => m.Name).OrderBy(m => m)));

            this.CreateMap<OrganizationLevel, OrganizationLevelViewModel>()
                .ForMember(x => x.LevelName, opt => opt.MapFrom(y => y.Name));
        }
    }
}
