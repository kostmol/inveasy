namespace Inveasy.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string FundAmount { get; set; }
        public string? FundGoal { get; set; }
        public string TrendingScore { get; set; }
        public virtual User? User { get; set; }
        public virtual Category? Category { get; set; }
        public virtual List<View>? Views { get; set; }
        public virtual List<Donation>? Donations { get; set; }   
        public virtual List<RewardTier>? RewardsTier { get; set; }

    }
}
