// DTO para listar y mostrar roles
public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Available { get; set; }
}

// DTO para crear/editar roles
public class RoleCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Available { get; set; }
}
