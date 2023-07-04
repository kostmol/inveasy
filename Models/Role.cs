using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Inveasy.Models
{
    public class Role
    {
        public int Id { get; set; }
        public virtual List<View>? Views { get; set; }
        public virtual List<Donation>? Donations { get; set; }
        public virtual List<RewardTier>? RewardsTier { get; set; }
    }
}
