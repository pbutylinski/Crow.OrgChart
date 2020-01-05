using System;
using System.Collections.Generic;
using Crow.OrgChart.Models;

namespace Crow.OrgChart.Services
{
    public interface IOrganizationViewModelService
    {
        OrganizationLevelViewModel GetLevelViewModel(Guid id);
        OrganizationLevelViewModel GetOrganizationViewModel();
        List<OrganizationChartItemViewModel> GetChartViewModelItems();
    }
}