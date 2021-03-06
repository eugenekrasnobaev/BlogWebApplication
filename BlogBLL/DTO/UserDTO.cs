namespace BlogBLL.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsExternalAccount { get; set; }
        public RoleDto Role { get; set; }
    }
}
