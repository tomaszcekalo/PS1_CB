using Microsoft.AspNetCore.Identity;
using PS1CB.Areas.Identity;
using PS1CB.Data;

namespace PS1CB.Services
{
    public class SubPasswordService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _hasher;

        public SubPasswordService(ApplicationDbContext context, IPasswordHasher<ApplicationUser> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task GenerateSubPasswordsAsync(ApplicationUser user, string fullPassword)
        {
            var oldSubs = _context.SubPasswords.Where(s => s.UserId == user.Id);
            _context.SubPasswords.RemoveRange(oldSubs);
            await _context.SaveChangesAsync();

            var rng = new Random();
            var usedSets = new HashSet<string>();

            for (int i = 0; i < 10; i++)
            {
                int length = rng.Next(5, fullPassword.Length / 2 + 1);
                var positions = new SortedSet<int>();

                while (positions.Count < length)
                    positions.Add(rng.Next(1, fullPassword.Length + 1));

                string posStr = string.Join("-", positions);
                if (!usedSets.Add(posStr)) { i--; continue; }

                string partial = string.Concat(positions.Select(p => fullPassword[p - 1]));
                string hash = _hasher.HashPassword(user, partial);

                _context.SubPasswords.Add(new SubPassword
                {
                    UserId = user.Id,
                    PositionString = posStr,
                    Hash = hash
                });
            }

            await _context.SaveChangesAsync();
        }
    }

}
