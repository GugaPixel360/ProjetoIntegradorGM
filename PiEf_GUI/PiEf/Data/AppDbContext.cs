using PiEf.Models;
using Microsoft.EntityFrameworkCore;

namespace PiEf.Data;




public class AppDbContext : DbContext
{
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Adotante> Adotantes { get; set; }
    public DbSet<Adocao> Adocoes { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(@"server=localhost;database=sistema_adocao;user=root;password=Senac2026");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adocao>()
            .HasOne<Adotante>()
            .WithMany()
            .HasForeignKey(a => a.IdAdotante)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Adocao>()
            .HasOne<Animal>()
            .WithMany()
            .HasForeignKey(a => a.IdAnimal)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
