namespace Inveasy.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public virtual User? User { get; set; }
        public virtual Project? Project{ get; set; }
    }
}
