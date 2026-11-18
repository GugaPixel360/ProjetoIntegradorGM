using PiEf.Models;
using PiEf.UI;

namespace PiEf;

internal static class Program
{
    /// <summary>
    /// Ponto de entrada da aplicação gráfica.
    /// O menu de console original foi preservado em Legacy/ProgramConsole.cs.txt.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Ciclo: Login -> Sistema -> (Sair da conta) -> Login ... sem reiniciar o programa.
        while (true)
        {
            Adotante? usuario;

            using (var login = new FormLogin())
            {
                Application.Run(login);
                usuario = login.UsuarioLogado;
            }

            if (usuario == null)
                break; // janela de login fechada no "X": encerra o sistema

            bool encerrarPrograma;

            using (var principal = new FormPrincipal(usuario))
            {
                Application.Run(principal);
                encerrarPrograma = !principal.EncerrouSessao;
            }

            if (encerrarPrograma)
                break; // janela principal fechada no "X": encerra o sistema
        }
    }
}
