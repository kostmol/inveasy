using System.Reflection.Metadata;

namespace Inveasy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name{ get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public virtual List<Role>? Roles { get; set; }
        public virtual List<Donation>? Donations { get; set; }
        public virtual List<View>? Views { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual DateTime CreatedDate { set;  get; }
        public virtual Image? Image { set; get; }

    }
}
