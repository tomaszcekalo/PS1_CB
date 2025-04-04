using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PS1CB.Areas.Identity;

namespace PS1CB.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public virtual DbSet<Message> Messages { get; set; } = default!;
    public virtual DbSet<Editor> Editors { get; set; } = default!;
    public virtual DbSet<LoginAttempt> LoginAttempts { get; set; } = default!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
