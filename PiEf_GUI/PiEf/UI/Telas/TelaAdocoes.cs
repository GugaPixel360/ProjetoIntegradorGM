using System.Windows.Forms;
using PiEf.Services;
using PiEf.UI.Controles;
using PiEf.UI.Dialogos;

namespace PiEf.UI.Telas;

/// <summary>Administrador: histórico de adoções e registro de nova adoção.</summary>
public sealed class TelaAdocoes : TelaBase
{
    private readonly DataGridView _grade = new();
    private readonly Label _vazio = CriarMensagemVazia();
    private readonly CampoBusca _busca = new();
    private List<AdocaoLinha> _todas = new();

    public TelaAdocoes()
    {
        _busca.Dica = "Buscar por cliente ou animal";
        _busca.TextoAlterado += (_, _) => AplicarFiltro();

        var btnNova = Botao("Nova adoção", Icones.Adocoes, EstiloBotao.Primario, 160);
        btnNova.Click += async (_, _) => await NovaAsync();

        Panel barra = CriarBarra(_busca, out FlowLayoutPanel acoes);
        acoes.Controls.Add(btnNova);

        Tema.EstilizarGrade(_grade);
        _grade.Dock = DockStyle.Fill;
        Tema.AdicionarColuna(_grade, "ID", nameof(AdocaoLinha.Id), 0.5f);
        Tema.AdicionarColuna(_grade, "Data", nameof(AdocaoLinha.Data), 1.3f, "dd/MM/yyyy HH:mm");
        Tema.AdicionarColuna(_grade, "Cliente", nameof(AdocaoLinha.Cliente), 2f);
        Tema.AdicionarColuna(_grade, "ID cliente", nameof(AdocaoLinha.IdCliente), 0.8f);
        Tema.AdicionarColuna(_grade, "Animal", nameof(AdocaoLinha.Animal), 2f);
        Tema.AdicionarColuna(_grade, "ID animal", nameof(AdocaoLinha.IdAnimal), 0.8f);

        var cartao = new Cartao { Dock = DockStyle.Fill };
        cartao.Controls.Add(_grade);
        cartao.Controls.Add(_vazio);

        Controls.Add(cartao);
        Controls.Add(barra);
    }

    public override string Titulo => "Adoções";
    public override string Subtitulo => "Histórico de adoções registradas.";

    public override Task AtualizarAsync() => ExecutarAsync(async () =>
    {
        _todas = await ConsultaService.ListarAdocoesDetalhadas();
        AplicarFiltro();
    });

    private void AplicarFiltro()
    {
        string termo = _busca.Texto.Trim();

        List<AdocaoLinha> lista = termo.Length == 0
            ? _todas
            : _todas.Where(l =>
                    l.Cliente.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                    l.Animal.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

        _grade.DataSource = lista;
        _grade.Visible = lista.Count > 0;
        _vazio.Visible = lista.Count == 0;
        _vazio.Text = _todas.Count == 0
            ? "Nenhuma adoção cadastrada."
            : "Nenhuma adoção encontrada para a busca.";
    }

    private async Task NovaAsync()
    {
        using var dlg = new FormAdocao();
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        Toast.Mostrar(this, "Adoção cadastrada com sucesso!");
        await AtualizarAsync();
    }
}
