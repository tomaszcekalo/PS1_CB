namespace PS1CB.Models
{
    public class SubPasswordLoginViewModel
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string PositionString { get; set; } // e.g. "2-4-6"
        public List<int> Positions => PositionString.Split('-').Select(int.Parse).ToList();
        public List<string> Inputs { get; set; } = new List<string>();
    }

}
