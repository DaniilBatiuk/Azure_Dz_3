using Azure_Dz_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Azure_Dz_2.Data
{
    public class PhotoContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; } = default!;

        public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
