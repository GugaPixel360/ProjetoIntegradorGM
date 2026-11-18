using Microsoft.EntityFrameworkCore;
using PiEf.Data;
using PiEf.Models;

namespace PiEf.Services;

/// <summary>
/// Operações de escrita. Cada uma confere antes o que o console apenas "imprimia"
/// (registro inexistente, animal indisponível, vínculos com adoções) para devolver
/// uma mensagem clara, e então chama o método original do model.
/// </summary>
public static class Operacoes
{
    // ---------- Animais ----------

    public static async Task<Resultado> AtualizarAnimalAsync(Animal dados)
    {
        await using (var db = new AppDbContext())
        {
            if (!await db.Animais.AnyAsync(a => a.Id == dados.Id))
                return Resultado.Falha("Animal não encontrado.");
        }

        await Animal.AtualizarAnimal(dados);
        return Resultado.Ok("Animal atualizado com sucesso!");
    }

    public static async Task<Resultado> ExcluirAnimalAsync(int id)
    {
        await using (var db = new AppDbContext())
        {
            if (!await db.Animais.AnyAsync(a => a.Id == id))
                return Resultado.Falha("Animal não encontrado.");

            if (await db.Adocoes.AnyAsync(a => a.IdAnimal == id))
                return Resultado.Falha("Este animal já foi adotado e possui adoção registrada, por isso não pode ser excluído.");
        }

        try
        {
            await Animal.ExcluirAnimal(id);
            return Resultado.Ok("Animal excluído com sucesso!");
        }
        catch (DbUpdateException)
        {
            return Resultado.Falha("Não foi possível excluir: o animal possui registros vinculados.");
        }
    }

    // ---------- Clientes ----------

    public static async Task<Resultado> AtualizarClienteAsync(Adotante dados)
    {
        await using (var db = new AppDbContext())
        {
            if (!await db.Adotantes.AnyAsync(a => a.Id == dados.Id))
                return Resultado.Falha("Cliente não encontrado.");
        }

        await Adotante.AtualizarAdotante(dados);
        return Resultado.Ok("Cliente atualizado com sucesso!");
    }

    public static async Task<Resultado> ExcluirClienteAsync(int id)
    {
        await using (var db = new AppDbContext())
        {
            if (!await db.Adotantes.AnyAsync(a => a.Id == id))
                return Resultado.Falha("Cliente não encontrado.");

            if (await db.Adocoes.AnyAsync(a => a.IdAdotante == id))
                return Resultado.Falha("Este cliente possui adoções registradas, por isso a conta não pode ser excluída.");
        }

        try
        {
            await Adotante.ExcluirAdotante(id);
            return Resultado.Ok("Cliente excluído com sucesso!");
        }
        catch (DbUpdateException)
        {
            return Resultado.Falha("Não foi possível excluir: o cliente possui registros vinculados.");
        }
    }

    // ---------- Adoções ----------

    public static async Task<Resultado> CadastrarAdocaoAsync(int idAdotante, int idAnimal)
    {
        await using (var db = new AppDbContext())
        {
            if (!await db.Adotantes.AnyAsync(a => a.Id == idAdotante))
                return Resultado.Falha("Cliente não encontrado.");

            Animal? animal = await db.Animais.FindAsync(idAnimal);

            if (animal == null)
                return Resultado.Falha("Animal não encontrado.");

            if (!animal.Disponivel)
                return Resultado.Falha("Esse animal não está disponível para adoção.");
        }

        bool ok = await Adocao.CadastrarAdocao(idAdotante, idAnimal);

        return ok
            ? Resultado.Ok("Adoção cadastrada com sucesso!")
            : Resultado.Falha("Não foi possível cadastrar a adoção.");
    }
}
