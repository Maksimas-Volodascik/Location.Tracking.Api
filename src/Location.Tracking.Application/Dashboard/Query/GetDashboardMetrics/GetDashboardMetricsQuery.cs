using Location.Tracking.Application.DTOs.Devices;
using Location.Tracking.Application.DTOs.Records;
using Location.Tracking.Application.DTOs.Users;
using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics
{
    public record GetDashboardMetricsQuery: IRequest<SystemMetrics> { }
}
