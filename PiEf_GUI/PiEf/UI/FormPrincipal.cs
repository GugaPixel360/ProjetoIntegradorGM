using System.Drawing;
using System.Windows.Forms;
using PiEf.Models;
using PiEf.UI.Controles;
using PiEf.UI.Telas;

namespace PiEf.UI;

/// <summary>
/// Janela principal: menu lateral + área de conteúdo. O menu muda conforme o perfil
/// (Administrador ou Cliente), igual aos menus do console.
/// </summary>
public sealed class FormPrincipal : Form
{
    private readonly Adotante _usuario;
    private readonly bool _admin;

    private readonly PainelSuave _sidebar = new() { Dock = DockStyle.Left, Width = 252, BackColor = Tema.Sidebar };
    private readonly FlowLayoutPanel _menu = new();
    private readonly Panel _conteudo = new() { Dock = DockStyle.Fill, BackColor = Tema.Fundo };
    private readonly Panel _cabecalho = new() { Dock = DockStyle.Top, Height = 96, BackColor = Tema.Fundo };
    private readonly Panel _corpo = new() { Dock = DockStyle.Fill, BackColor = Tema.Fundo, Padding = new Padding(32, 0, 32, 28) };
    private readonly Label _titulo = new();
    private readonly Label _subtitulo = new();
    private readonly Label _nomeUsuario = new();
    private readonly List<(BotaoModerno Botao, Func<TelaBase> Criar)> _itens = new();

    private TelaBase? _telaAtual;

    /// <summary>True quando o usuário clicou em "Sair da conta" (volta ao login em vez de fechar o programa).</summary>
    public bool EncerrouSessao { get; private set; }

    public FormPrincipal(Adotante usuario)
    {
        _usuario = usuario;
        _admin = usuario.TipoUsuario == "Admin";

        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = "Sistema de Adoção";
        ClientSize = new Size(1280, 780);
        MinimumSize = new Size(1060, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Tema.Fundo;
        Font = Tema.Normal;

        ConstruirConteudo();
        ConstruirSidebar();

        Controls.Add(_conteudo);
        Controls.Add(_sidebar);

        Shown += async (_, _) =>
        {
            if (_itens.Count > 0)
                await NavegarAsync(_itens[0].Botao, _itens[0].Criar);
        };
    }

    private void ConstruirConteudo()
    {
        _titulo.Font = Tema.Titulo;
        _titulo.ForeColor = Tema.Texto;
        _titulo.AutoSize = true;
        _titulo.Location = new Point(32, 18);

        _subtitulo.Font = Tema.Subtitulo;
        _subtitulo.ForeColor = Tema.TextoSuave;
        _subtitulo.AutoSize = true;
        _subtitulo.Location = new Point(34, 60);

        _cabecalho.Controls.Add(_titulo);
        _cabecalho.Controls.Add(_subtitulo);

        _conteudo.Controls.Add(_corpo);      // Fill primeiro
        _conteudo.Controls.Add(_cabecalho);  // depois o Top
    }

    private void ConstruirSidebar()
    {
        // Marca
        var marca = new Panel { Dock = DockStyle.Top, Height = 96, BackColor = Tema.Sidebar };
        marca.Controls.Add(new Label
        {
            Text = Icones.Animais,
            Font = new Font("Segoe UI Emoji", 20f),
            ForeColor = Tema.Destaque,
            AutoSize = false,
            Location = new Point(22, 26),
            Size = new Size(48, 44)
        });
        marca.Controls.Add(new Label
        {
            Text = "Adoção",
            Font = Tema.SubtituloNegrito,
            ForeColor = Color.White,
            AutoSize = false,
            Location = new Point(76, 26),
            Size = new Size(160, 24)
        });
        marca.Controls.Add(new Label
        {
            Text = "Gestão de animais",
            Font = Tema.Pequena,
            ForeColor = Tema.Hex("#8FB3B8"),
            AutoSize = false,
            Location = new Point(77, 50),
            Size = new Size(160, 20)
        });

        // Menu
        _menu.Dock = DockStyle.Fill;
        _menu.FlowDirection = FlowDirection.TopDown;
        _menu.WrapContents = false;
        _menu.BackColor = Tema.Sidebar;
        _menu.Padding = new Padding(12, 6, 12, 6);

        if (_admin)
        {
            AdicionarItem("Painel", Icones.Painel, () => new TelaPainel());
            AdicionarItem("Animais", Icones.Animais, () => new TelaAnimais(_usuario));
            AdicionarItem("Adoções", Icones.Adocoes, () => new TelaAdocoes());
            AdicionarItem("Clientes", Icones.Clientes, () => new TelaClientes(_usuario));
        }
        else
        {
            AdicionarItem("Animais disponíveis", Icones.Animais, () => new TelaAnimais(_usuario));
            AdicionarItem("Minha conta", Icones.Usuario, CriarTelaMinhaConta);
        }

        // Rodapé com usuário e saída
        var rodape = new PainelSuave { Dock = DockStyle.Bottom, Height = 136, BackColor = Tema.Sidebar };
        rodape.Paint += (_, e) =>
        {
            using var pen = new Pen(Tema.SidebarHover);
            e.Graphics.DrawLine(pen, 16, 0, rodape.Width - 16, 0);
        };

        _nomeUsuario.Text = _usuario.Nome;
        _nomeUsuario.Font = Tema.Negrito;
        _nomeUsuario.ForeColor = Color.White;
        _nomeUsuario.AutoSize = false;
        _nomeUsuario.AutoEllipsis = true;
        _nomeUsuario.Location = new Point(24, 16);
        _nomeUsuario.Size = new Size(204, 22);

        var perfil = new Label
        {
            Text = _admin ? "Administrador" : "Cliente",
            Font = Tema.Pequena,
            ForeColor = Tema.Hex("#8FB3B8"),
            AutoSize = false,
            Location = new Point(24, 40),
            Size = new Size(204, 20)
        };

        var btnSair = new BotaoModerno
        {
            Text = "Sair da conta",
            Icone = Icones.Sair,
            Estilo = EstiloBotao.Menu,
            Size = new Size(228, 46),
            Location = new Point(12, 72)
        };
        btnSair.Click += (_, _) =>
        {
            EncerrouSessao = true;
            Close();
        };

        rodape.Controls.Add(_nomeUsuario);
        rodape.Controls.Add(perfil);
        rodape.Controls.Add(btnSair);

        _sidebar.Controls.Add(_menu);   // Fill primeiro
        _sidebar.Controls.Add(marca);
        _sidebar.Controls.Add(rodape);
    }

    private TelaBase CriarTelaMinhaConta()
    {
        var tela = new TelaMinhaConta(_usuario);
        tela.DadosAtualizados += (_, _) => _nomeUsuario.Text = _usuario.Nome;
        tela.ContaExcluida += (_, _) =>
        {
            EncerrouSessao = true;
            Close();
        };
        return tela;
    }

    private void AdicionarItem(string texto, string icone, Func<TelaBase> criar)
    {
        var botao = new BotaoModerno
        {
            Text = texto,
            Icone = icone,
            Estilo = EstiloBotao.Menu,
            Font = Tema.Normal,
            Size = new Size(228, 46),
            Margin = new Padding(0, 0, 0, 4)
        };

        botao.Click += async (_, _) => await NavegarAsync(botao, criar);

        _menu.Controls.Add(botao);
        _itens.Add((botao, criar));
    }

    private async Task NavegarAsync(BotaoModerno botao, Func<TelaBase> criar)
    {
        foreach (var item in _itens)
            item.Botao.Ativo = ReferenceEquals(item.Botao, botao);

        TelaBase nova = criar();
        TelaBase? antiga = _telaAtual;
        _telaAtual = nova;

        _titulo.Text = nova.Titulo;
        _subtitulo.Text = nova.Subtitulo;

        _corpo.Controls.Add(nova);

        if (antiga != null)
        {
            _corpo.Controls.Remove(antiga);
            antiga.Dispose();
        }

        await nova.AtualizarAsync();
    }
}
