using System.Drawing;
using System.Windows.Forms;
using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;
using PiEf.UI.Dialogos;

namespace PiEf.UI.Telas;

/// <summary>Linha exibida na tabela de animais.</summary>
public sealed class AnimalLinha
{
    public int Id { get; }
    public string Nome { get; }
    public string Especie { get; }
    public string Raca { get; }
    public int Idade { get; }
    public string Sexo { get; }
    public string Status { get; }
    public Animal Origem { get; }

    public AnimalLinha(Animal a)
    {
        Origem = a;
        Id = a.Id;
        Nome = a.Nome;
        Especie = a.Especie;
        Raca = a.Raca;
        Idade = a.Idade;
        Sexo = a.Sexo;
        Status = a.Disponivel ? "Disponível" : "Adotado";
    }
}

/// <summary>
/// Animais. Administrador: cadastrar, editar, excluir e registrar adoção.
/// Cliente: consultar os disponíveis e adotar.
/// </summary>
public sealed class TelaAnimais : TelaBase
{
    private readonly Adotante _usuario;
    private readonly bool _admin;

    private readonly DataGridView _grade = new();
    private readonly Label _vazio = CriarMensagemVazia();
    private readonly CampoBusca _busca = new();
    private readonly CheckBox _mostrarTodos = new();
    private readonly BotaoModernoAtalho _btnNovo;
    private readonly BotaoModernoAtalho _btnEditar;
    private readonly BotaoModernoAtalho _btnExcluir;
    private readonly BotaoModernoAtalho _btnAdotar;

    private List<AnimalLinha> _todos = new();

    public TelaAnimais(Adotante usuario)
    {
        _usuario = usuario;
        _admin = usuario.TipoUsuario == "Admin";

        _busca.Dica = "Buscar por nome, espécie ou raça";
        _busca.TextoAlterado += (_, _) => AplicarFiltro();

        _btnNovo = Botao("Novo animal", Icones.Novo, EstiloBotao.Primario, 140);
        _btnEditar = Botao("Editar", Icones.Editar, EstiloBotao.Secundario, 100);
        _btnExcluir = Botao("Excluir", Icones.Excluir, EstiloBotao.Perigo, 100);
        _btnAdotar = Botao("Adotar", Icones.Adocoes, _admin ? EstiloBotao.Secundario : EstiloBotao.Primario, 110);

        _btnNovo.Click += async (_, _) => await NovoAsync();
        _btnEditar.Click += async (_, _) => await EditarAsync();
        _btnExcluir.Click += async (_, _) => await ExcluirAsync();
        _btnAdotar.Click += async (_, _) => await AdotarAsync();

        Panel barra = CriarBarra(_busca, out FlowLayoutPanel acoes);

        if (_admin)
        {
            // Mantém o filtro fora da linha dos botões para que ele não comprima a busca.
            barra.Height = 92;
            _mostrarTodos.Text = "Incluir adotados";
            _mostrarTodos.Font = Tema.Normal;
            _mostrarTodos.ForeColor = Tema.Texto;
            _mostrarTodos.AutoSize = true;
            _mostrarTodos.Margin = new Padding(0, 20, 6, 0);
            _mostrarTodos.BackColor = Tema.Fundo;
            _mostrarTodos.Cursor = Cursors.Hand;
            _mostrarTodos.CheckedChanged += async (_, _) => await ExecutarAsync(AtualizarAsync);

            acoes.Controls.Add(_mostrarTodos);
            acoes.Controls.Add(_btnNovo);
            acoes.Controls.Add(_btnEditar);
            acoes.Controls.Add(_btnExcluir);
        }

        acoes.Controls.Add(_btnAdotar);

        Tema.EstilizarGrade(_grade);
        _grade.Dock = DockStyle.Fill;
        Tema.AdicionarColuna(_grade, "ID", nameof(AnimalLinha.Id), 0.5f);
        Tema.AdicionarColuna(_grade, "Nome", nameof(AnimalLinha.Nome), 1.6f);
        Tema.AdicionarColuna(_grade, "Espécie", nameof(AnimalLinha.Especie), 1.2f);
        Tema.AdicionarColuna(_grade, "Raça", nameof(AnimalLinha.Raca), 1.4f);
        Tema.AdicionarColuna(_grade, "Idade", nameof(AnimalLinha.Idade), 0.6f);
        Tema.AdicionarColuna(_grade, "Sexo", nameof(AnimalLinha.Sexo), 0.9f);
        Tema.AdicionarColuna(_grade, "Situação", nameof(AnimalLinha.Status), 1f);
        _grade.SelectionChanged += (_, _) => AtualizarBotoes();
        _grade.CellFormatting += FormatarCelula;
        _grade.CellDoubleClick += async (_, e) =>
        {
            if (e.RowIndex < 0) return;
            if (_admin) await EditarAsync(); else await AdotarAsync();
        };

        var cartao = new Cartao { Dock = DockStyle.Fill };
        cartao.Controls.Add(_grade);
        cartao.Controls.Add(_vazio);

        Controls.Add(cartao);
        Controls.Add(barra);

        AtualizarBotoes();
    }

    public override string Titulo => _admin ? "Animais" : "Animais disponíveis";
    public override string Subtitulo => _admin
        ? "Cadastre, edite e acompanhe os animais do abrigo."
        : "Escolha um animal para adotar.";

    public override Task AtualizarAsync() => ExecutarAsync(async () =>
    {
        List<Animal> animais = (_admin && _mostrarTodos.Checked)
            ? await ConsultaService.ListarTodosAnimais()
            : await Animal.ListarAnimaisDisponiveis();

        _todos = animais.OrderBy(a => a.Id).Select(a => new AnimalLinha(a)).ToList();
        AplicarFiltro();
    });

    private void AplicarFiltro()
    {
        string termo = _busca.Texto.Trim();

        List<AnimalLinha> lista = termo.Length == 0
            ? _todos
            : _todos.Where(l =>
                    l.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                    l.Especie.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                    l.Raca.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

        _grade.DataSource = lista;
        _grade.Visible = lista.Count > 0;
        _vazio.Visible = lista.Count == 0;
        _vazio.Text = _todos.Count == 0
            ? "Nenhum animal disponível no momento."
            : "Nenhum animal encontrado para a busca.";

        AtualizarBotoes();
    }

    private AnimalLinha? Selecionado => _grade.Visible ? _grade.CurrentRow?.DataBoundItem as AnimalLinha : null;

    private void AtualizarBotoes()
    {
        AnimalLinha? sel = Selecionado;
        _btnEditar.Enabled = sel != null;
        _btnExcluir.Enabled = sel != null;
        _btnAdotar.Enabled = sel != null && sel.Origem.Disponivel;
    }

    private void FormatarCelula(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.CellStyle == null) return;
        if (_grade.Columns[e.ColumnIndex].Name != nameof(AnimalLinha.Status)) return;

        bool disponivel = Convert.ToString(e.Value) == "Disponível";
        e.CellStyle.ForeColor = disponivel ? Tema.Sucesso : Tema.TextoSuave;
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        e.CellStyle.Font = Tema.Negrito;
    }

    private async Task NovoAsync()
    {
        using var dlg = new FormAnimal();
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        Toast.Mostrar(this, "Animal cadastrado com sucesso!");
        await AtualizarAsync();
    }

    private async Task EditarAsync()
    {
        AnimalLinha? sel = Selecionado;
        if (sel == null) return;

        using var dlg = new FormAnimal(sel.Origem);
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        Toast.Mostrar(this, "Animal atualizado com sucesso!");
        await AtualizarAsync();
    }

    private async Task ExcluirAsync()
    {
        AnimalLinha? sel = Selecionado;
        if (sel == null) return;

        bool confirmou = FormMensagem.Confirmar(FindForm(), "Excluir animal",
            $"Deseja excluir \"{sel.Nome}\" (ID {sel.Id})? Esta ação não pode ser desfeita.",
            "Excluir", perigo: true);
        if (!confirmou) return;

        await ExecutarAsync(async () =>
        {
            Resultado r = await Operacoes.ExcluirAnimalAsync(sel.Id);

            if (r.Sucesso) Toast.Mostrar(this, r.Mensagem);
            else FormMensagem.Erro(FindForm(), r.Mensagem);

            await AtualizarAsync();
        });
    }

    private async Task AdotarAsync()
    {
        AnimalLinha? sel = Selecionado;
        if (sel == null || !sel.Origem.Disponivel) return;

        if (_admin)
        {
            using var dlg = new FormAdocao(sel.Origem);
            if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

            Toast.Mostrar(this, "Adoção cadastrada com sucesso!");
            await AtualizarAsync();
            return;
        }

        bool confirmou = FormMensagem.Confirmar(FindForm(), "Confirmar adoção",
            $"Deseja adotar \"{sel.Nome}\" ({sel.Especie}, {sel.Raca})?", "Adotar");
        if (!confirmou) return;

        await ExecutarAsync(async () =>
        {
            Resultado r = await Operacoes.CadastrarAdocaoAsync(_usuario.Id, sel.Id);

            if (r.Sucesso) Toast.Mostrar(this, r.Mensagem);
            else FormMensagem.Erro(FindForm(), r.Mensagem);

            await AtualizarAsync();
        });
    }
}
