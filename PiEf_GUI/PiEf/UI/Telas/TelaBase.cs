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

        
    protected static Panel CriarBarra(Control busca, out FlowLayoutPanel acoes)
    {
        var barra = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = Tema.Fundo };

        var botoes = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Tema.Fundo,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };

        busca.Height = 42; 
        void Posicionar()
        {
            int altura = barra.ClientSize.Height;
            int larguraBarra = barra.ClientSize.Width;

            
            botoes.Location = new Point(larguraBarra - botoes.Width, Math.Max(0, (altura - botoes.Height) / 2));

            
            busca.SetBounds(0, Math.Max(0, (altura - busca.Height) / 2),
                            Math.Max(120, botoes.Left - 16), busca.Height);
        }

        barra.Controls.Add(busca);
        barra.Controls.Add(botoes);

        barra.SizeChanged += (_, _) => Posicionar();
        botoes.SizeChanged += (_, _) => Posicionar();
        Posicionar();

        acoes = botoes;
        return barra;
    }

    protected static BotaoModernoAtalho Botao(string texto, string icone, Controles.EstiloBotao estilo, int largura = 150)
        => new(texto, icone, estilo, largura);

    
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
