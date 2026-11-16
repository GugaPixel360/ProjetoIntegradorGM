
using PiEf.Data;
using Microsoft.EntityFrameworkCore;

namespace PiEf.Models;

public class Adocao
{
    public int Id { get; set; }

    public int IdAdotante { get; set; }

    public int IdAnimal { get; set; }

    public DateTime DataAdocao { get; set; } = DateTime.Now;

    public static async Task<bool> CadastrarAdocao(int idAdotante, int idAnimal)
    {
        await using var db = new AppDbContext();

        var adotante = await db.Adotantes.FindAsync(idAdotante);

        if (adotante == null)
        {
            Console.WriteLine("Cliente não encontrado.");
            return false;
        }

        var animal = await db.Animais.FindAsync(idAnimal);

        if (animal == null)
        {
            Console.WriteLine("Animal não encontrado.");
            return false;
        }

        if (!animal.Disponivel)
        {
            Console.WriteLine("Esse animal não está disponível para adoção.");
            return false;
        }

        Adocao novaAdocao = new Adocao
        {
            IdAdotante = idAdotante,
            IdAnimal = idAnimal,
            DataAdocao = DateTime.Now
        };

        animal.Disponivel = false;

        db.Adocoes.Add(novaAdocao);

        await db.SaveChangesAsync();

        Console.WriteLine("Adoção cadastrada com sucesso!");

        return true;
    }
}
