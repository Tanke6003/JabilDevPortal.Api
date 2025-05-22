

namespace JabilDevPortal.Api.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<List<ApplicationReadDto>> GetAllAsync();
        Task<ApplicationReadDto> GetByIdAsync(int id);
        Task<int> CreateAsync(ApplicationCreateDto dto);
        Task UpdateAsync(int id, ApplicationCreateDto dto);
        Task DeleteAsync(int id);
        Task<List<ApplicationReadDto>> SearchAsync(string? query, string? department, int? ownerId);
    }
}
