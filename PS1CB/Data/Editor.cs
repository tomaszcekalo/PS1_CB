using System.ComponentModel.DataAnnotations.Schema;

namespace PS1CB.Data
{
    public class Editor
    {
        public int Id { get; set; }
        [ForeignKey("Message")]
        public int MessageId { get; set; }
        public string UserId { get; set; } = default!;

    }
}