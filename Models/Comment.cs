namespace Inveasy.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime dateTime { get; set; }
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
