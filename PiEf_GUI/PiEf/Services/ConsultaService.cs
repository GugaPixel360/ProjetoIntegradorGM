using Microsoft.EntityFrameworkCore;
using PiEf.Data;
using PiEf.Models;

namespace PiEf.Services;

/// <summary>Linha de adoção com os nomes já resolvidos (o model guarda apenas IDs).</summary>
public sealed class AdocaoLinha
{
    public int Id { get; init; }
    public DateTime Data { get; init; }
    public int IdCliente { get; init; }
    public string Cliente { get; init; } = string.Empty;
    public int IdAnimal { get; init; }
    public string Animal { get; init; } = string.Empty;
}

public sealed record ResumoPainel(int AnimaisDisponiveis, int Clientes, int Adocoes, List<AdocaoLinha> Recentes);

/// <summary>
/// Consultas de leitura. Sempre que existe um método nos models, ele é usado;
/// o único acesso direto ao banco é a listagem de TODOS os animais (o model só lista os disponíveis).
/// </summary>
public static class ConsultaService
{
    public static async Task<List<Animal>> ListarTodosAnimais()
    {
        await using var db = new AppDbContext();
        return await db.Animais.OrderBy(a => a.Id).ToListAsync();
    }

    public static async Task<List<AdocaoLinha>> ListarAdocoesDetalhadas()
    {
        List<Adocao> adocoes = await Adocao.ListarAdocoes();
        Dictionary<int, Adotante> adotantes = (await Adotante.ListarAdotantes()).ToDictionary(a => a.Id);
        Dictionary<int, Animal> animais = (await ListarTodosAnimais()).ToDictionary(a => a.Id);

        return adocoes
            .OrderByDescending(a => a.DataAdocao)
            .Select(a => new AdocaoLinha
            {
                Id = a.Id,
                Data = a.DataAdocao,
                IdCliente = a.IdAdotante,
                Cliente = adotantes.TryGetValue(a.IdAdotante, out var c) ? c.Nome : "(removido)",
                IdAnimal = a.IdAnimal,
                Animal = animais.TryGetValue(a.IdAnimal, out var n) ? n.Nome : "(removido)"
            })
            .ToList();
    }

    public static async Task<List<AdocaoLinha>> ListarAdocoesDoCliente(int idCliente)
    {
        List<Adocao> adocoes = (await Adocao.ListarAdocoes())
            .Where(a => a.IdAdotante == idCliente)
            .OrderByDescending(a => a.DataAdocao)
            .ToList();
        Dictionary<int, Animal> animais = (await ListarTodosAnimais()).ToDictionary(a => a.Id);

        return adocoes
            .Select(a => new AdocaoLinha
            {
                Id = a.Id,
                Data = a.DataAdocao,
                IdCliente = a.IdAdotante,
                Cliente = string.Empty,
                IdAnimal = a.IdAnimal,
                Animal = animais.TryGetValue(a.IdAnimal, out var animal) ? animal.Nome : "(removido)"
            })
            .ToList();
    }

    public static async Task<ResumoPainel> ObterResumo()
    {
        int disponiveis = (await Animal.ListarAnimaisDisponiveis()).Count;
        int clientes = (await Adotante.ListarAdotantes()).Count(a => a.TipoUsuario != "Admin");
        List<AdocaoLinha> adocoes = await ListarAdocoesDetalhadas();

        return new ResumoPainel(disponiveis, clientes, adocoes.Count, adocoes.Take(6).ToList());
    }
}
