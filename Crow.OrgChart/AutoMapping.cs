using AutoMapper;
using Crow.OrgChart.DataStorage;
using Crow.OrgChart.DataStorage.Models;
using Crow.OrgChart.Models;
using System;

namespace Crow.OrgChart
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            this.CreateMap<MemberListItemViewModel, MemberDetails>();
            this.CreateMap<MemberDetails, MemberListItemViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id.Value));

            this.CreateMap<OrganizationLevel, OrganizationChartItemViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id.ToString()))
                .ForMember(x => x.Parent, opt => opt.MapFrom(y => (y.ParentId ?? Guid.Empty).ToString()));

            this.CreateMap<OrganizationLevel, OrganizationLevelViewModel>()
                .ForMember(x => x.LevelName, opt => opt.MapFrom(y => y.Name));
        }
    }
}
