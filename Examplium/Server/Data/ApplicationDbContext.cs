using Examplium.Shared.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Examplium.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}