using PS1CB.Areas.Identity;

namespace PS1CB.Data
{
    public class SubPassword
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PositionString { get; set; } // e.g., "2-4-6-8-10"
        public string Hash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; }
    }

}
