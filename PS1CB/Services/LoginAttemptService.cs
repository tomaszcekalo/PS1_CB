using PS1CB.Data;

namespace PS1CB.Services
{
    public interface ILoginAttemptService
    {
        Task<LoginAttempt> AddLoginAttemptAsync(string who, DateTime when, bool success);
    }
    public class LoginAttemptService : ILoginAttemptService
    {
        ApplicationDbContext _applicationDbContext;

        public LoginAttemptService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<LoginAttempt> AddLoginAttemptAsync(string who, DateTime when, bool success)
        {
            var result = await _applicationDbContext.LoginAttempts.AddAsync(new LoginAttempt
            {
                When = when,
                Who = who,
                Success = success,
                Details = ""
            });
            await _applicationDbContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
