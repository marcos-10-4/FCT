using CTG_Api.Modelos;
using Microsoft.EntityFrameworkCore;
using System;

namespace CTG_Api.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options)
        : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partido>()
                .HasOne(p => p.Jugador1)
                .WithMany()
                .HasForeignKey(p => p.Jugador1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partido>()
                .HasOne(p => p.Jugador2)
                .WithMany()
                .HasForeignKey(p => p.Jugador2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
