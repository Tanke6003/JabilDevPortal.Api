namespace JabilDevPortal.Api.DTOs.Auth
{
    public class UserInfoDto
    {
        public int    Id         { get; set; }
        public string Username   { get; set; } = null!;
        public string FullName   { get; set; } = null!;
        public string Email      { get; set; } = null!;
        public string Department { get; set; } = null!;
       public int    RoleId   { get; set; }
public string RoleName { get; set; } = null!;

    }
}
