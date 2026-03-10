using CTG_API.Modelos;
using Microsoft.EntityFrameworkCore;
using System;

namespace CTG_API.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options)
        : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Partidos> Partidos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partidos>()
                .HasOne(p => p.Jugador1)
                .WithMany()
                .HasForeignKey(p => p.Jugador1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partidos>()
                .HasOne(p => p.Jugador2)
                .WithMany()
                .HasForeignKey(p => p.Jugador2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
