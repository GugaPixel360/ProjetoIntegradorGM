using PiEf.Models;

namespace PiEf.Services;

/// <summary>
/// Regras de validação de campos. Reaproveita Adotante.ValidarEmail / ValidarSenha
/// e mantém as mesmas regras de idade do menu de console.
/// </summary>
public static class Validacao
{
    /// <summary>Cadastro de cliente: o console aceitava idade >= 0.</summary>
    public const int IdadeMinimaCadastro = 0;

    /// <summary>Atualização de cliente: o console exigia idade >= 18.</summary>
    public const int IdadeMinimaAtualizacao = 18;

    public static string? Obrigatorio(string? valor, string nomeCampo)
        => string.IsNullOrWhiteSpace(valor) ? $"Informe {nomeCampo}." : null;

    public static string? Idade(string? valor, int minimo, out int idade)
    {
        idade = 0;
        string limpo = (valor ?? string.Empty).Replace(" ", "");

        if (limpo.Length == 0)
            return "Informe a idade.";

        if (!int.TryParse(limpo, out idade) || idade < minimo)
        {
            return minimo > 0
                ? $"Idade inválida. É necessário ter {minimo} anos ou mais."
                : "Idade inválida. Use um número inteiro igual ou maior que zero.";
        }

        return null;
    }

    public static async Task<string?> EmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return "Informe o e-mail.";

        return await Adotante.ValidarEmail(email)
            ? null
            : "E-mail inválido. Use @gmail.com, @hotmail.com, @outlook.com ou @yahoo.com.";
    }

    public static async Task<string?> SenhaAsync(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            return "Informe a senha.";

        return await Adotante.ValidarSenha(senha)
            ? null
            : "Senha inválida. Use de 6 a 10 caracteres.";
    }
}
