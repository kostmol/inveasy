using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Inveasy.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double FundAmount { get; set; } = 0;
        public double? FundGoal { get; set; }
        public double? TrendingScore { get; set; }
        public virtual User? User { get; set; }
        public virtual List<Category>? Categories { get; set; }
        public virtual List<View>? Views { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<Donation>? Donations { get; set; }
        public virtual List<RewardTier>? RewardsTier { get; set; }
        public virtual DateTime CreatedDate { set; get; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
