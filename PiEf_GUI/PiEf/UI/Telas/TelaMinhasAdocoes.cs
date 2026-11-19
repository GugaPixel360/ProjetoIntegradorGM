using System.Windows.Forms;
using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;

namespace PiEf.UI.Telas;

/// <summary>Histórico de adoções do cliente autenticado.</summary>
public sealed class TelaMinhasAdocoes : TelaBase
{
    private readonly Adotante _usuario;
    private readonly DataGridView _grade = new();
    private readonly Label _vazio = CriarMensagemVazia();

    public TelaMinhasAdocoes(Adotante usuario)
    {
        _usuario = usuario;

        Tema.EstilizarGrade(_grade);
        _grade.Dock = DockStyle.Fill;
        Tema.AdicionarColuna(_grade, "Data", nameof(AdocaoLinha.Data), 1.2f, "dd/MM/yyyy HH:mm");
        Tema.AdicionarColuna(_grade, "Animal", nameof(AdocaoLinha.Animal), 2f);

        var cartao = new Cartao { Dock = DockStyle.Fill };
        cartao.Controls.Add(_grade);
        cartao.Controls.Add(_vazio);
        Controls.Add(cartao);
    }

    public override string Titulo => "Minhas adoções";
    public override string Subtitulo => "Animais que você adotou.";

    public override Task AtualizarAsync() => ExecutarAsync(async () =>
    {
        List<AdocaoLinha> adocoes = await ConsultaService.ListarAdocoesDoCliente(_usuario.Id);
        _grade.DataSource = adocoes;
        _grade.Visible = adocoes.Count > 0;
        _vazio.Text = "Você ainda não possui adoções registradas.";
        _vazio.Visible = adocoes.Count == 0;
    });
}
