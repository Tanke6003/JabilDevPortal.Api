using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<UserReadDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<UserReadDto>> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("by-role/{roleId}")]
    public async Task<ActionResult<List<UserReadDto>>> GetByRole(int roleId)
        => Ok(await _service.GetByRoleAsync(roleId));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserEditDto dto)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null) return NotFound();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
}
