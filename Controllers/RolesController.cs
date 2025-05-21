using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JabilDevPortal.Api.Controllers
{
   using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;
    public RolesController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<RoleDto>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<RoleDto> GetById(int id)
    {
        var role = _service.GetById(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public IActionResult Create([FromBody] RoleCreateUpdateDto dto)
    {
        _service.Create(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] RoleCreateUpdateDto dto)
    {
        var role = _service.GetById(id);
        if (role == null) return NotFound();
        _service.Update(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var role = _service.GetById(id);
        if (role == null) return NotFound();
        _service.Delete(id);
        return NoContent();
    }
}

}