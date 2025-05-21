// Services/Interfaces/IDashboardService.cs
using JabilDevPortal.Api.DTOs.Dashboard;

namespace JabilDevPortal.Api.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }
}
