// DTO para lectura (listar, detalle)
public class UserReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Available { get; set; }
}

// DTO para edición
public class UserEditDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int? RoleId { get; set; }
    public bool Available { get; set; }
}
