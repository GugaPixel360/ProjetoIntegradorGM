using PiEf.Models;
using Microsoft.EntityFrameworkCore;

namespace PiEf.Data;




public class AppDbContext : DbContext
{
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Adotante> Adotantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(@"server=localhost;database=sistema_adocao;user=root;password=Senac2026");
    }
}
