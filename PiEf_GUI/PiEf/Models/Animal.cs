using PiEf.Data;
using Microsoft.EntityFrameworkCore;


namespace PiEf.Models;



public class Animal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string Raca { get; set; } = string.Empty;

    public int Idade { get; set; }

    public string Sexo { get; set; } = string.Empty;

    public bool Disponivel { get; set; } = true;


    public Animal (string nome, string especie, string raca, int idade, string sexo)
    {
        Nome = nome;
        Especie = especie;
        Raca = raca;
        Idade = idade;
        Sexo = sexo;
    }

   public static async Task<List<Animal>> ListarAnimaisDisponiveis()
    {
        await using var db = new AppDbContext();

        return await db.Animais
            .Where(a => a.Disponivel)
            .ToListAsync();

    }

    public static async Task CadastrarAnimal(Animal animal)
    {
        await using var db = new AppDbContext();

        db.Animais.Add(animal);

        await db.SaveChangesAsync();
    }

    public static async Task ExcluirAnimal(int id)
    {
        await using var db = new AppDbContext();

        var animal = await db.Animais.FindAsync(id);

        if (animal == null)
        {
            Console.WriteLine("Animal não encontrado.");
            return;
        }

        db.Animais.Remove(animal);

        await db.SaveChangesAsync();

        Console.WriteLine("Animal excluído com sucesso!");
    }

    public static async Task AtualizarAnimal(Animal animalAtualizado)
    {
        await using var db = new AppDbContext();

        var animalExistente = await db.Animais.FindAsync(animalAtualizado.Id);

        if (animalExistente == null)
        {
            Console.WriteLine("Animal não encontrado.");
            return;
        }

        animalExistente.Nome = animalAtualizado.Nome;
        animalExistente.Especie = animalAtualizado.Especie;
        animalExistente.Raca = animalAtualizado.Raca;
        animalExistente.Idade = animalAtualizado.Idade;
        animalExistente.Sexo = animalAtualizado.Sexo;

        await db.SaveChangesAsync();

        Console.WriteLine("Animal atualizado com sucesso!");
    }


}













