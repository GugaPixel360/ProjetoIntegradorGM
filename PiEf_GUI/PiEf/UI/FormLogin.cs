using System.Drawing;
using System.Windows.Forms;
using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;
using PiEf.UI.Dialogos;
using System.Drawing.Drawing2D;

namespace PiEf.UI;

/// <summary>Tela inicial: entrar ou criar conta de cliente (equivale ao menu inicial do console).</summary>
public sealed class FormLogin : Form
{
        // Identidade visual desta tela
    private static readonly Color FundoDireita = Tema.Hex("#F4F6F7");
    private static readonly Color TextoPrincipal = Tema.Hex("#263238");
    private static readonly Color TextoSecundario = Tema.Hex("#7B898D");
    private static readonly Font FonteTitulo = new("Segoe UI", 22f, FontStyle.Bold);
    private static readonly Font FonteTituloEsquerda = new("Segoe UI", 26f, FontStyle.Bold);

    private readonly PainelSuave _direita = new() { Dock = DockStyle.Fill, BackColor = FundoDireita, AutoScroll = true };
    private readonly Panel _painelLogin = new() { Size = new Size(400, 430), BackColor = FundoDireita };
    private readonly Panel _painelCadastro = new() { Size = new Size(400, 580), BackColor = FundoDireita };

    private readonly Campo _loginEmail = new() { Rotulo = "E-mail", Icone = Icones.Email };
    private readonly Campo _loginSenha = new() { Rotulo = "Senha", Senha = true, Icone = Icones.Cadeado };
    private readonly Campo _cadNome = new() { Rotulo = "Nome completo" };
    private readonly Campo _cadIdade = new() { Rotulo = "Idade" };
    private readonly Campo _cadEmail = new() { Rotulo = "E-mail", Icone = Icones.Email };
    private readonly Campo _cadSenha = new() { Rotulo = "Senha", Senha = true, Icone = Icones.Cadeado, Ajuda = "Use de 6 a 10 caracteres." };

    private readonly BotaoModerno _btnEntrar = new() { Text = "Entrar  →", Size = new Size(400, 46) };
    private readonly BotaoModerno _btnCriar = new() { Text = "Criar conta", Size = new Size(400, 46) };

    /// <summary>Preenchido quando o login é bem-sucedido.</summary>
    public Adotante? UsuarioLogado { get; private set; }

    public FormLogin()
    {
        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = "Sistema de Adoção";
        ClientSize = new Size(1000, 680);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;
        Font = Tema.Normal;

        ConstruirLadoEsquerdo();
        ConstruirPainelLogin();
        ConstruirPainelCadastro();

        _direita.Controls.Add(_painelLogin);
        _direita.Controls.Add(_painelCadastro);
        _direita.Resize += (_, _) => Centralizar();
        _direita.Paint += (_, e) => DesenharDireita(e.Graphics, _direita.ClientSize);

        foreach (Campo c in new[] { _loginEmail, _loginSenha, _cadNome, _cadIdade, _cadEmail, _cadSenha })
            c.BackColor = FundoDireita;

        Controls.Add(_direita);
        // O lado esquerdo é adicionado depois por ser Dock Left (Fill vem primeiro).
        Controls.Add(_esquerda!);

        Shown += (_, _) => { Centralizar(); MostrarLogin(); };
    }

    private PainelSuave? _esquerda;

    private void ConstruirLadoEsquerdo()
    {
        _esquerda = new PainelSuave { Dock = DockStyle.Left, Width = 420, BackColor = Tema.Sidebar };
        _esquerda.Paint += (_, e) => DesenharEsquerda(e.Graphics, _esquerda.ClientSize);

        _esquerda.Controls.Add(new Label
        {
            Text = Icones.Animais,
            Font = Tema.IconeGrande,
            ForeColor = Tema.Destaque,
            BackColor = Color.Transparent,
            AutoSize = false,
            Location = new Point(44, 190),
            Size = new Size(90, 80)
        });

        foreach ((string linha, int y) in new[] { ("Sistema de", 276), ("Adoção", 334) })
        {
            _esquerda.Controls.Add(new Label
            {
                Text = linha,
                Font = FonteTituloEsquerda,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Location = new Point(44, y),
                Size = new Size(360, 56)
            });
        }

        _esquerda.Controls.Add(new Label
        {
            Text = "Cadastre animais, acompanhe adoções e ajude cada um a encontrar um novo lar.",
            Font = Tema.Subtitulo,
            ForeColor = Tema.Hex("#D5E6E8"),
            BackColor = Color.Transparent,
            AutoSize = false,
            Location = new Point(46, 412),
            Size = new Size(330, 90)
        });
    }

    private static Label CriarTexto(string texto, Font fonte, Color cor, int y, int altura) => new()
    {
        Text = texto,
        Font = fonte,
        ForeColor = cor,
        AutoSize = false,
        Location = new Point(0, y),
        Size = new Size(400, altura)
    };

    private static void Posicionar(Panel painel, Campo campo, int y)
    {
        campo.Location = new Point(0, y);
        campo.Width = 400;
        painel.Controls.Add(campo);
    }

    private LinkLabel CriarLink(string texto, int y, EventHandler aoClicar, string? destaque = null)
    {
        var link = new LinkLabel
        {
            Text = texto,
            Font = Tema.Normal,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = TextoPrincipal,
            LinkColor = Tema.Primaria,
            ActiveLinkColor = Tema.PrimariaEscura,
            VisitedLinkColor = Tema.Primaria,
            LinkBehavior = LinkBehavior.HoverUnderline,
            Location = new Point(0, y),
            Size = new Size(400, 28),
            Cursor = Cursors.Hand
        };

        // Só a palavra de destaque vira link (turquesa); o resto do texto fica em cor normal.
        if (destaque != null)
        {
            int inicio = texto.IndexOf(destaque, StringComparison.Ordinal);
            if (inicio >= 0)
                link.LinkArea = new LinkArea(inicio, destaque.Length);
        }

        link.Click += aoClicar;
        return link;
    }

    private void ConstruirPainelLogin()
    {
        _painelLogin.Controls.Add(CriarTexto("Bem-vindo de volta", FonteTitulo, TextoPrincipal, 0, 42));
        _painelLogin.Controls.Add(CriarTexto("Entre com seu e-mail e senha.", Tema.Subtitulo, TextoSecundario, 46, 26));

        Posicionar(_painelLogin, _loginEmail, 104);
        Posicionar(_painelLogin, _loginSenha, 196);

        _btnEntrar.Location = new Point(0, 306);
        _btnEntrar.Click += async (_, _) => await EntrarAsync();
        _painelLogin.Controls.Add(_btnEntrar);

        _painelLogin.Controls.Add(CriarLink("Ainda não tem conta? Cadastre-se", 368, (_, _) => MostrarCadastro(), "Cadastre-se"));
    }

    private void ConstruirPainelCadastro()
    {
        _painelCadastro.Controls.Add(CriarTexto("Criar conta", FonteTitulo, TextoPrincipal, 0, 42));
        _painelCadastro.Controls.Add(CriarTexto("Cadastre-se como cliente para adotar um animal.", Tema.Subtitulo, TextoSecundario, 46, 26));

        Posicionar(_painelCadastro, _cadNome, 100);
        Posicionar(_painelCadastro, _cadIdade, 190);
        Posicionar(_painelCadastro, _cadEmail, 280);
        Posicionar(_painelCadastro, _cadSenha, 370);

        _btnCriar.Location = new Point(0, 476);
        _btnCriar.Click += async (_, _) => await CadastrarAsync();
        _painelCadastro.Controls.Add(_btnCriar);

        _painelCadastro.Controls.Add(CriarLink("Já tenho conta. Entrar", 538, (_, _) => MostrarLogin(), "Entrar"));
    }

    private void DesenharEsquerda(Graphics g, Size area)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float w = area.Width, h = area.Height, s = DeviceDpi / 96f;

        // Formas suaves ao fundo (um tom de teal mais claro, bem transparente)
        Decoracao.Elipse(g, w * 0.10f, h * 1.02f, w * 0.95f, h * 0.38f, Color.FromArgb(26, 110, 190, 185));
        Decoracao.Elipse(g, w * 1.00f, h * 1.00f, w * 0.40f, h * 0.22f, Color.FromArgb(40, 110, 190, 185));

        // Patinhas grandes no canto superior direito, bem sutis
        Color patinha = Color.FromArgb(55, 110, 190, 190);
        Decoracao.Patinha(g, w * 0.86f, h * 0.20f, w * 0.34f, 14f, patinha);
        Decoracao.Patinha(g, w * 0.70f, h * 0.085f, w * 0.20f, -18f, patinha);

        // Cachorro e gato, só o contorno, na parte de baixo
        Color traco = Color.FromArgb(60, 140, 215, 210);
        Decoracao.Cachorro(g, w * 0.03f, h * 0.74f, w * 0.36f, traco, 3f * s);
        Decoracao.Gato(g, w * 0.46f, h * 0.78f, w * 0.36f, traco, 3f * s);
    }

    private void DesenharDireita(Graphics g, Size area)
    {
        // No cadastro o formulário ocupa quase a altura toda; sem decoração para não haver cortes.
        if (!_painelLogin.Visible) return;

        g.SmoothingMode = SmoothingMode.AntiAlias;
        float w = area.Width, h = area.Height;

        Color forma = Tema.Hex("#E3ECEF");
        Color patinha = Tema.Hex("#C9DADF");

        // Canto superior direito
        Decoracao.Elipse(g, w, 0f, w * 0.30f, h * 0.14f, forma);
        Decoracao.Patinha(g, w * 0.90f, h * 0.085f, w * 0.16f, 15f, patinha);

        // Canto inferior direito
        Decoracao.Elipse(g, w, h, w * 0.34f, h * 0.15f, forma);
        Decoracao.Patinha(g, w * 0.88f, h * 0.93f, w * 0.16f, -12f, patinha);
    }

    private void Centralizar()
    {
        foreach (Panel p in new[] { _painelLogin, _painelCadastro })
            p.Location = new Point(Math.Max(0, (_direita.Width - p.Width) / 2), Math.Max(10, (_direita.Height - p.Height) / 2));
    }

    private void MostrarLogin()
    {
        _painelCadastro.Visible = false;
        _painelLogin.Visible = true;
        AcceptButton = _btnEntrar;
        _direita.Invalidate();
        _loginEmail.FocarCampo();
    }

    private void MostrarCadastro()
    {
        _painelLogin.Visible = false;
        _painelCadastro.Visible = true;
        AcceptButton = _btnCriar;
        _direita.Invalidate();
        _cadNome.FocarCampo();
    }

    // ---------------- Login ----------------

    private async Task EntrarAsync()
    {
        _loginEmail.LimparErro();
        _loginSenha.LimparErro();

        string email = _loginEmail.Texto.Trim();
        string senha = _loginSenha.Texto;
        bool valido = true;

        if (string.IsNullOrWhiteSpace(email)) { _loginEmail.MostrarErro("Informe o e-mail."); valido = false; }
        if (string.IsNullOrWhiteSpace(senha.Trim())) { _loginSenha.MostrarErro("Informe a senha."); valido = false; }
        if (!valido) return;

        _btnEntrar.Enabled = false;
        UseWaitCursor = true;

        try
        {
            Adotante? adotante = await Adotante.Login(email, senha);

            if (adotante == null)
            {
                FormMensagem.Erro(this, "E-mail ou senha incorretos. Confira os dados e tente novamente.", "Não foi possível entrar");
                return;
            }

            UsuarioLogado = adotante;
            Close();
        }
        catch (Exception ex)
        {
            FormMensagem.Erro(this, Mensagens.DeErro(ex));
        }
        finally
        {
            if (!IsDisposed)
            {
                UseWaitCursor = false;
                _btnEntrar.Enabled = true;
            }
        }
    }

    // ---------------- Cadastro ----------------

    private async Task CadastrarAsync()
    {
        foreach (Campo c in new[] { _cadNome, _cadIdade, _cadEmail, _cadSenha })
            c.LimparErro();

        _btnCriar.Enabled = false;
        UseWaitCursor = true;

        try
        {
            var dados = await Formularios.LerCliente(
                _cadNome, _cadIdade, _cadEmail,
                Validacao.IdadeMinimaCadastro
            );

            if (dados == null)
                return;

            
            if (!Adotante.ValidarNome(dados.Value.Nome))
            {
                _cadNome.MostrarErro("O nome não pode conter números.");
                return;
            }

            string senha = _cadSenha.Texto.Replace(" ", "");
            string? erroSenha = await Validacao.SenhaAsync(senha);

            if (erroSenha != null)
                _cadSenha.MostrarErro(erroSenha);

            if (erroSenha != null)
                return;

            var novo = new Adotante(dados.Value.Nome, dados.Value.Idade, dados.Value.Email, senha);
            await Adotante.CadastrarAdotante(novo);

            string email = dados.Value.Email;

            foreach (Campo c in new[] { _cadNome, _cadIdade, _cadEmail, _cadSenha })
                c.Texto = string.Empty;

            _loginEmail.Texto = email;
            _loginSenha.Texto = string.Empty;
            MostrarLogin();
            _loginSenha.FocarCampo();

            Toast.Mostrar(this, "Cliente cadastrado com sucesso! Entre com sua senha para continuar.");
        }
        catch (Exception ex)
        {
            FormMensagem.Erro(this, Mensagens.DeErro(ex));
        }
        finally
        {
            if (!IsDisposed)
            {
                UseWaitCursor = false;
                _btnCriar.Enabled = true;
            }
        }
    }
}
