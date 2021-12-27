using Microsoft.EntityFrameworkCore;

namespace ToreadApi.Models
{
    public class ToreadContext : DbContext
    {
        public ToreadContext(DbContextOptions<ToreadContext> options)
            : base(options)
        {
        }

        public DbSet<ToreadItem> ToreadItems { get; set; }
    }
}