using Microsoft.EntityFrameworkCore;
using PiEf.Data;
using PiEf.Models;
using PiEf.Services;
using PiEf.UI;

namespace PiEf;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Cria o banco e as tabelas automaticamente, se ainda não existirem.
        try
        {
            using var db = new AppDbContext();
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            MessageBox.Show(Mensagens.DeErro(ex), "Banco de dados",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        while (true)
        {
            Adotante? usuario;

            using (var login = new FormLogin())
            {
                Application.Run(login);
                usuario = login.UsuarioLogado;
            }

            if (usuario == null)
                break;

            bool encerrarPrograma;

            using (var principal = new FormPrincipal(usuario))
            {
                Application.Run(principal);
                encerrarPrograma = !principal.EncerrouSessao;
            }

            if (encerrarPrograma)
                break;
        }
    }
}