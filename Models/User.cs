namespace Inveasy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name{ get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public virtual IEnumerable<Role> Roles { get; set; } = new List<Role>();
    }
}
