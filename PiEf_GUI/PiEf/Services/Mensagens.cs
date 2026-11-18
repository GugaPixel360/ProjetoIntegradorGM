namespace PiEf.Services;

public static class Mensagens
{
    /// <summary>Converte uma exceção (normalmente de acesso ao banco) em texto compreensível.</summary>
    public static string DeErro(Exception ex)
    {
        Exception raiz = ex;
        while (raiz.InnerException != null)
            raiz = raiz.InnerException;

        return "Não foi possível acessar o banco de dados. Verifique se o MySQL está em execução " +
               "e se a conexão em Data/AppDbContext.cs está correta.\n\nDetalhe: " + raiz.Message;
    }
}
