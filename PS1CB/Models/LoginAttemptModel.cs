namespace PS1CB.Models
{
    public class LoginAttemptModel
    {
        public string Who { get; set; }
        public DateTime? LastSuccessful { get; set; }
        public DateTime? LastFailed { get; internal set; }
        public int FailedCount { get; internal set; }
    }
}
