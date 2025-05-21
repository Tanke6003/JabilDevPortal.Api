public interface IRoleService
{
    List<RoleDto> GetAll();
    RoleDto? GetById(int id);
    void Create(RoleCreateUpdateDto dto);
    void Update(int id, RoleCreateUpdateDto dto);
    void Delete(int id);
}