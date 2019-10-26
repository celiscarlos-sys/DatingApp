using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DatingAppContext : DbContext
    {
        public DatingAppContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) /* Change PKs */
        {
            modelBuilder.Entity<Usuario>().HasKey(x => x.Cedula);
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Values> Values { get; set; }
    }
}