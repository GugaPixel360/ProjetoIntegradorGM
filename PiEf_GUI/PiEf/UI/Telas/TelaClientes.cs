using System.Windows.Forms;
using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;
using PiEf.UI.Dialogos;

namespace PiEf.UI.Telas;

public sealed class ClienteLinha
{
    public int Id { get; }
    public string Nome { get; }
    public int Idade { get; }
    public string Email { get; }
    public string Perfil { get; }
    public Adotante Origem { get; }

    public ClienteLinha(Adotante a)
    {
        Origem = a;
        Id = a.Id;
        Nome = a.Nome;
        Idade = a.Idade;
        Email = a.Email;
        Perfil = a.TipoUsuario == "Admin" ? "Administrador" : "Cliente";
    }
}

/// <summary>Administrador: listar, editar e excluir clientes.</summary>
public sealed class TelaClientes : TelaBase
{
    private readonly Adotante _usuarioLogado;
    private readonly DataGridView _grade = new();
    private readonly Label _vazio = CriarMensagemVazia();
    private readonly CampoBusca _busca = new();
    private readonly BotaoModernoAtalho _btnEditar;
    private readonly BotaoModernoAtalho _btnExcluir;
    private List<ClienteLinha> _todos = new();

    public TelaClientes(Adotante usuarioLogado)
    {
        _usuarioLogado = usuarioLogado;

        _busca.Dica = "Buscar por nome ou e-mail";
        _busca.TextoAlterado += (_, _) => AplicarFiltro();

        _btnEditar = Botao("Editar", Icones.Editar, EstiloBotao.Secundario, 110);
        _btnExcluir = Botao("Excluir", Icones.Excluir, EstiloBotao.Perigo, 110);
        _btnEditar.Click += async (_, _) => await EditarAsync();
        _btnExcluir.Click += async (_, _) => await ExcluirAsync();

        Panel barra = CriarBarra(_busca, out FlowLayoutPanel acoes);
        acoes.Controls.Add(_btnEditar);
        acoes.Controls.Add(_btnExcluir);

        Tema.EstilizarGrade(_grade);
        _grade.Dock = DockStyle.Fill;
        Tema.AdicionarColuna(_grade, "ID", nameof(ClienteLinha.Id), 0.5f);
        Tema.AdicionarColuna(_grade, "Nome", nameof(ClienteLinha.Nome), 2f);
        Tema.AdicionarColuna(_grade, "Idade", nameof(ClienteLinha.Idade), 0.6f);
        Tema.AdicionarColuna(_grade, "E-mail", nameof(ClienteLinha.Email), 2.4f);
        Tema.AdicionarColuna(_grade, "Perfil", nameof(ClienteLinha.Perfil), 1.1f);
        _grade.SelectionChanged += (_, _) => AtualizarBotoes();
        _grade.CellDoubleClick += async (_, e) => { if (e.RowIndex >= 0) await EditarAsync(); };

        var cartao = new Cartao { Dock = DockStyle.Fill };
        cartao.Controls.Add(_grade);
        cartao.Controls.Add(_vazio);

        Controls.Add(cartao);
        Controls.Add(barra);

        AtualizarBotoes();
    }

    public override string Titulo => "Clientes";
    public override string Subtitulo => "Consulte, atualize e exclua clientes cadastrados.";

    public override Task AtualizarAsync() => ExecutarAsync(async () =>
    {
        List<Adotante> lista = await Adotante.ListarAdotantes();
        _todos = lista.OrderBy(a => a.Id).Select(a => new ClienteLinha(a)).ToList();
        AplicarFiltro();
    });

    private void AplicarFiltro()
    {
        string termo = _busca.Texto.Trim();

        List<ClienteLinha> lista = termo.Length == 0
            ? _todos
            : _todos.Where(l =>
                    l.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                    l.Email.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

        _grade.DataSource = lista;
        _grade.Visible = lista.Count > 0;
        _vazio.Visible = lista.Count == 0;
        _vazio.Text = _todos.Count == 0
            ? "Nenhum cliente cadastrado."
            : "Nenhum cliente encontrado para a busca.";

        AtualizarBotoes();
    }

    private ClienteLinha? Selecionado => _grade.Visible ? _grade.CurrentRow?.DataBoundItem as ClienteLinha : null;

    private void AtualizarBotoes()
    {
        bool tem = Selecionado != null;
        _btnEditar.Enabled = tem;
        _btnExcluir.Enabled = tem;
    }

    private async Task EditarAsync()
    {
        ClienteLinha? sel = Selecionado;
        if (sel == null) return;

        using var dlg = new FormCliente(sel.Origem);
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        Toast.Mostrar(this, "Cliente atualizado com sucesso!");
        await AtualizarAsync();
    }

    private async Task ExcluirAsync()
    {
        ClienteLinha? sel = Selecionado;
        if (sel == null) return;

        if (sel.Id == _usuarioLogado.Id)
        {
            FormMensagem.Aviso(FindForm(), "Você não pode excluir a conta com a qual está conectado.");
            return;
        }

        bool confirmou = FormMensagem.Confirmar(FindForm(), "Excluir cliente",
            $"Deseja excluir \"{sel.Nome}\" (ID {sel.Id})? Esta ação não pode ser desfeita.",
            "Excluir", perigo: true);
        if (!confirmou) return;

        await ExecutarAsync(async () =>
        {
            Resultado r = await Operacoes.ExcluirClienteAsync(sel.Id);

            if (r.Sucesso) Toast.Mostrar(this, r.Mensagem);
            else FormMensagem.Erro(FindForm(), r.Mensagem);

            await AtualizarAsync();
        });
    }
}
