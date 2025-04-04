namespace PS1CB.Data
{
    public class LoginAttempt
    {
        public int Id { get; set; }
        public DateTime When { get; set; } 
        public string Who { get; set; }
        public bool Success { get; set; }
        public string Details { get; set; }
    }
}
