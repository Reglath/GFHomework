using Microsoft.EntityFrameworkCore;
using System;

namespace Homework.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        //public DbSet<Voted> Voted { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
