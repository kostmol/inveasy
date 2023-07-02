using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inveasy.Models;

namespace Inveasy.Data
{
    public class InveasyContext : DbContext
    {
        public InveasyContext (DbContextOptions<InveasyContext> options)
            : base(options)
        {
        }

        public DbSet<Inveasy.Models.User> User { get; set; } = default!;
        public DbSet<Inveasy.Models.Project> Project { get; set; } = default!;
        public DbSet<Inveasy.Models.Donation> Donation { get; set; } = default!;
        public DbSet<Inveasy.Models.RewardTier> RewardTier { get; set; } = default!;

    }
}
