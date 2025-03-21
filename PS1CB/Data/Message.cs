using System.ComponentModel.DataAnnotations.Schema;

namespace PS1CB.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public string AuthorName { get; set; }
        [NotMapped]
        public List<Editor> Editors { get; set; } = new List<Editor>();
    }
}