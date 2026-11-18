namespace PiEf.Services;

/// <summary>Resultado de uma operação, com a mensagem pronta para mostrar ao usuário.</summary>
public readonly record struct Resultado(bool Sucesso, string Mensagem)
{
    public static Resultado Ok(string mensagem) => new(true, mensagem);
    public static Resultado Falha(string mensagem) => new(false, mensagem);
}
