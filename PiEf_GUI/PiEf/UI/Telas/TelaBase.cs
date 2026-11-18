using System.Drawing;
using System.Windows.Forms;
using PiEf.Services;
using PiEf.UI.Dialogos;

namespace PiEf.UI.Telas;

/// <summary>Base de todas as telas exibidas na área de conteúdo da janela principal.</summary>
public abstract class TelaBase : UserControl
{
    protected TelaBase()
    {
        Dock = DockStyle.Fill;
        BackColor = Tema.Fundo;
    }

    public abstract string Titulo { get; }
    public virtual string Subtitulo => string.Empty;

    /// <summary>Recarrega os dados reais do banco.</summary>
    public virtual Task AtualizarAsync() => Task.CompletedTask;

    /// <summary>Executa uma ação e mostra erro amigável se o banco falhar.</summary>
    protected async Task ExecutarAsync(Func<Task> acao)
    {
        try
        {
            UseWaitCursor = true;
            await acao();
        }
        catch (Exception ex)
        {
            // Se o usuário já trocou de tela, a tela antiga foi descartada: ignora o erro dela.
            if (!IsDisposed)
                FormMensagem.Erro(FindForm(), Mensagens.DeErro(ex));
        }
        finally
        {
            if (!IsDisposed)
                UseWaitCursor = false;
        }
    }

    /// <summary>Barra superior com busca à esquerda e botões à direita.</summary>
    protected static Panel CriarBarra(Control busca, out FlowLayoutPanel acoes)
    {
        var barra = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = Tema.Fundo };

        busca.Dock = DockStyle.Left;
        busca.Width = 320;

        acoes = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Tema.Fundo,
            Padding = new Padding(0, 0, 0, 0)
        };

        barra.Controls.Add(acoes);
        barra.Controls.Add(busca);
        return barra;
    }

    protected static BotaoModernoAtalho Botao(string texto, string icone, Controles.EstiloBotao estilo, int largura = 150)
        => new(texto, icone, estilo, largura);

    /// <summary>Mensagem exibida no lugar da tabela quando não há registros.</summary>
    protected static Label CriarMensagemVazia() => new()
    {
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = Tema.Subtitulo,
        ForeColor = Tema.TextoSuave,
        BackColor = Color.White,
        Visible = false
    };
}

/// <summary>Cria botões de barra de ferramentas já com tamanho e margem padronizados.</summary>
public sealed class BotaoModernoAtalho : Controles.BotaoModerno
{
    public BotaoModernoAtalho(string texto, string icone, Controles.EstiloBotao estilo, int largura)
    {
        Text = texto;
        Icone = icone;
        Estilo = estilo;
        Size = new Size(largura, 42);
        Margin = new Padding(10, 10, 0, 10);
    }
}
