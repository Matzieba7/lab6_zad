using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToreadApi.Models;


namespace ReadMvc.Models
{
    public class FakeContext : DbContext
    {
        public FakeContext(DbContextOptions<FakeContext> options)
            : base(options)
        {
        }

        public DbSet<ToreadItem> ToreadItems { get; set; }

        public DbSet<ToreadApi.Models.ToreadItem> ToreadItem { get; set; }
    }

}
