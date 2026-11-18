using PiEf.Services;
using PiEf.UI.Controles;

namespace PiEf.UI;

/// <summary>Leitura e validação de campos compartilhadas por várias telas.</summary>
public static class Formularios
{
    /// <summary>
    /// Valida nome, idade e e-mail de um cliente, marcando o erro em cada campo.
    /// Retorna null se algum campo estiver inválido.
    /// </summary>
    public static async Task<(string Nome, int Idade, string Email)?> LerCliente(
        Campo nome, Campo idade, Campo email, int idadeMinima)
    {
        bool valido = true;

        string? erro = Validacao.Obrigatorio(nome.Texto, "o nome");
        if (erro != null) { nome.MostrarErro(erro); valido = false; }

        erro = Validacao.Idade(idade.Texto, idadeMinima, out int valorIdade);
        if (erro != null) { idade.MostrarErro(erro); valido = false; }

        string emailLimpo = email.Texto.Replace(" ", "");
        erro = await Validacao.EmailAsync(emailLimpo);
        if (erro != null) { email.MostrarErro(erro); valido = false; }

        if (!valido) return null;
        return (nome.Texto.Trim(), valorIdade, emailLimpo);
    }
}
