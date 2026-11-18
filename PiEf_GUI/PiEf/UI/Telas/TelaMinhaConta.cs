using System.Drawing;
using System.Windows.Forms;
using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;
using PiEf.UI.Dialogos;

namespace PiEf.UI.Telas;

/// <summary>Cliente: atualizar os próprios dados ou excluir a própria conta.</summary>
public sealed class TelaMinhaConta : TelaBase
{
    private readonly Adotante _usuario;
    private readonly Campo _nome = new() { Rotulo = "Nome" };
    private readonly Campo _idade = new() { Rotulo = "Idade" };
    private readonly Campo _email = new() { Rotulo = "E-mail" };

    public event EventHandler? DadosAtualizados;
    public event EventHandler? ContaExcluida;

    public TelaMinhaConta(Adotante usuario)
    {
        _usuario = usuario;
        AutoScroll = true;

        // ----- Cartão: meus dados -----
        var cartaoDados = new Cartao { Location = new Point(0, 0), Size = new Size(560, 410) };

        cartaoDados.Controls.Add(new Label
        {
            Text = "Meus dados",
            Font = Tema.SubtituloNegrito,
            ForeColor = Tema.Texto,
            AutoSize = false,
            Location = new Point(28, 22),
            Size = new Size(504, 28)
        });

        int y = 66;
        foreach (Campo c in new[] { _nome, _idade, _email })
        {
            c.Location = new Point(28, y);
            c.Width = 504;
            cartaoDados.Controls.Add(c);
            y += 90;
        }

        var btnSalvar = new BotaoModerno
        {
            Text = "Salvar alterações",
            Icone = Icones.Salvar,
            Size = new Size(200, 44),
            Location = new Point(28, y + 6)
        };
        btnSalvar.Click += async (_, _) => await SalvarAsync();
        cartaoDados.Controls.Add(btnSalvar);

        // ----- Cartão: excluir conta -----
        var cartaoExcluir = new Cartao { Location = new Point(0, 430), Size = new Size(560, 170) };

        cartaoExcluir.Controls.Add(new Label
        {
            Text = "Excluir minha conta",
            Font = Tema.SubtituloNegrito,
            ForeColor = Tema.Perigo,
            AutoSize = false,
            Location = new Point(28, 22),
            Size = new Size(504, 28)
        });

        cartaoExcluir.Controls.Add(new Label
        {
            Text = "Ao excluir a conta, seus dados serão removidos e você será desconectado. " +
                   "Esta ação não pode ser desfeita.",
            Font = Tema.Normal,
            ForeColor = Tema.TextoSuave,
            AutoSize = false,
            Location = new Point(28, 54),
            Size = new Size(504, 44)
        });

        var btnExcluir = new BotaoModerno
        {
            Text = "Excluir conta",
            Icone = Icones.Excluir,
            Estilo = EstiloBotao.Perigo,
            Size = new Size(170, 44),
            Location = new Point(28, 108)
        };
        btnExcluir.Click += async (_, _) => await ExcluirContaAsync();
        cartaoExcluir.Controls.Add(btnExcluir);

        Controls.Add(cartaoDados);
        Controls.Add(cartaoExcluir);
    }

    public override string Titulo => "Minha conta";
    public override string Subtitulo => "Atualize seus dados ou exclua sua conta.";

    public override Task AtualizarAsync()
    {
        _nome.Texto = _usuario.Nome;
        _idade.Texto = _usuario.Idade.ToString();
        _email.Texto = _usuario.Email;
        return Task.CompletedTask;
    }

    private Task SalvarAsync() => ExecutarAsync(async () =>
    {
        _nome.LimparErro(); _idade.LimparErro(); _email.LimparErro();

        var dados = await Formularios.LerCliente(_nome, _idade, _email, Validacao.IdadeMinimaAtualizacao);
        if (dados == null) return;

        var atualizado = new Adotante
        {
            Id = _usuario.Id,
            Nome = dados.Value.Nome,
            Idade = dados.Value.Idade,
            Email = dados.Value.Email,
            Senha = _usuario.Senha,
            TipoUsuario = _usuario.TipoUsuario
        };

        Resultado r = await Operacoes.AtualizarClienteAsync(atualizado);

        if (!r.Sucesso)
        {
            FormMensagem.Erro(FindForm(), r.Mensagem);
            return;
        }

        _usuario.Nome = atualizado.Nome;
        _usuario.Idade = atualizado.Idade;
        _usuario.Email = atualizado.Email;

        Toast.Mostrar(this, "Dados atualizados com sucesso!");
        DadosAtualizados?.Invoke(this, EventArgs.Empty);
    });

    private async Task ExcluirContaAsync()
    {
        bool confirmou = FormMensagem.Confirmar(FindForm(), "Excluir minha conta",
            "Tem certeza que deseja excluir sua conta? Esta ação não pode ser desfeita.",
            "Excluir conta", perigo: true);

        if (!confirmou) return;

        await ExecutarAsync(async () =>
        {
            Resultado r = await Operacoes.ExcluirClienteAsync(_usuario.Id);

            if (!r.Sucesso)
            {
                FormMensagem.Erro(FindForm(), r.Mensagem);
                return;
            }

            FormMensagem.Informar(FindForm(), "Conta excluída com sucesso.");
            ContaExcluida?.Invoke(this, EventArgs.Empty);
        });
    }
}
