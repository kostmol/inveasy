using System.Reflection.Metadata;
using Inveasy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inveasy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public virtual DateTime CreatedDate { set; get; }
        public virtual List<Role> Roles { get; set; } = new List<Role>();
        public virtual List<Donation> Donations { get; set; } = new List<Donation>();
        public virtual List<View> Views { get; set; } = new List<View>();
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual Image? Image { set; get; }

    }

}