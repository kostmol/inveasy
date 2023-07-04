namespace Inveasy.Models
{
    public class View
    {
        public int Id { get; set; }        
        public DateTime Date { get; set; }
        public virtual User? User { get; set; }
        public virtual Project? Project { get; set; }
    }
}
