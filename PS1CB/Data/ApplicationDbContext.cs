using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PS1CB.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public virtual DbSet<Message> Messages { get; set; } = default!;
    public virtual DbSet<Editor> Editors { get; set; } = default!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
