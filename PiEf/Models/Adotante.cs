
using PiEf.Data;
using Microsoft.EntityFrameworkCore;

namespace PiEf.Models;

public class Adotante
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

    public Adotante(string nome, int idade, string email, string senha = "")
    {
        Nome = nome;
        Idade = idade;
        Email = email;
        Senha = senha;
    }

    public Adotante() { }

    // LISTAR ADOTANTES
    public static async Task<List<Adotante>> ListarAdotantes()
    {
        await using var db = new AppDbContext();

        return await db.Adotantes.ToListAsync();
    }

    // CADASTRAR ADOTANTE
    public static async Task CadastrarAdotante(Adotante adotante)
    {
        await using var db = new AppDbContext();

        db.Adotantes.Add(adotante);

        await db.SaveChangesAsync();
    }

    // EXCLUIR ADOTANTE
    public static async Task ExcluirAdotante(int id)
    {
        await using var db = new AppDbContext();

        var adotante = await db.Adotantes.FindAsync(id);

        if (adotante == null)
        {
            Console.WriteLine("Cliente não encontrado.");
            return;
        }

        db.Adotantes.Remove(adotante);

        await db.SaveChangesAsync();

        Console.WriteLine("Cliente excluído com sucesso!");
    }

    // ATUALIZAR ADOTANTE
    public static async Task AtualizarAdotante(Adotante adotanteAtualizado)
    {
        await using var db = new AppDbContext();

        var adotante = await db.Adotantes.FindAsync(adotanteAtualizado.Id);

        if (adotante == null)
        {
            Console.WriteLine("Cliente não encontrado.");
            return;
        }

        adotante.Nome = adotanteAtualizado.Nome;
        adotante.Idade = adotanteAtualizado.Idade;
        adotante.Email = adotanteAtualizado.Email;

        await db.SaveChangesAsync();

        Console.WriteLine("Cliente atualizado com sucesso!");
    }

    public static async Task<Adotante?> Login(string email, string senha)
    {
        await using var db = new AppDbContext();

        return await db.Adotantes.FirstOrDefaultAsync(
            a => a.Email == email && a.Senha == senha
        );
    }
}
