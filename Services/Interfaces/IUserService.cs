public interface IUserService
{
    Task<List<UserReadDto>> GetAllAsync();
    Task<UserReadDto?> GetByIdAsync(int id);
    Task<List<UserReadDto>> GetByRoleAsync(int roleId);
    Task UpdateAsync(int id, UserEditDto dto);
}
