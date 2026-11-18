using System.Drawing;
using System.Windows.Forms;
using PiEf.Services;
using PiEf.UI.Controles;

namespace PiEf.UI.Telas;

/// <summary>Visão geral do administrador: números reais e adoções mais recentes.</summary>
public sealed class TelaPainel : TelaBase
{
    private readonly CartaoNumero _disponiveis = new("Animais disponíveis", Icones.Animais, Tema.Primaria, Tema.PrimariaClara);
    private readonly CartaoNumero _clientes = new("Clientes cadastrados", Icones.Clientes, Tema.Hex("#B9791A"), Tema.DestaqueClaro);
    private readonly CartaoNumero _adocoes = new("Adoções realizadas", Icones.Adocoes, Tema.Sucesso, Tema.SucessoClaro);
    private readonly DataGridView _grade = new();
    private readonly Label _vazio = CriarMensagemVazia();

    public TelaPainel()
    {
        var indicadores = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 124,
            ColumnCount = 3,
            RowCount = 1,
            BackColor = Tema.Fundo
        };
        for (int i = 0; i < 3; i++)
            indicadores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 3f));
        indicadores.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _disponiveis.Dock = DockStyle.Fill; _disponiveis.Margin = new Padding(0, 0, 16, 0);
        _clientes.Dock = DockStyle.Fill; _clientes.Margin = new Padding(0, 0, 16, 0);
        _adocoes.Dock = DockStyle.Fill; _adocoes.Margin = new Padding(0);
        indicadores.Controls.Add(_disponiveis, 0, 0);
        indicadores.Controls.Add(_clientes, 1, 0);
        indicadores.Controls.Add(_adocoes, 2, 0);

        var titulo = new Label
        {
            Text = "Adoções recentes",
            Dock = DockStyle.Top,
            Height = 56,
            Font = Tema.SubtituloNegrito,
            ForeColor = Tema.Texto,
            TextAlign = ContentAlignment.BottomLeft,
            Padding = new Padding(0, 0, 0, 10)
        };

        Tema.EstilizarGrade(_grade);
        _grade.Dock = DockStyle.Fill;
        Tema.AdicionarColuna(_grade, "Data", nameof(AdocaoLinha.Data), 1.2f, "dd/MM/yyyy HH:mm");
        Tema.AdicionarColuna(_grade, "Cliente", nameof(AdocaoLinha.Cliente), 2f);
        Tema.AdicionarColuna(_grade, "Animal", nameof(AdocaoLinha.Animal), 2f);

        var cartao = new Cartao { Dock = DockStyle.Fill };
        cartao.Controls.Add(_grade);
        cartao.Controls.Add(_vazio);

        // Fill primeiro; depois os Top, de baixo para cima.
        Controls.Add(cartao);
        Controls.Add(titulo);
        Controls.Add(indicadores);
    }

    public override string Titulo => "Painel";
    public override string Subtitulo => "Visão geral do sistema de adoção.";

    public override Task AtualizarAsync() => ExecutarAsync(async () =>
    {
        ResumoPainel resumo = await ConsultaService.ObterResumo();

        _disponiveis.Valor = resumo.AnimaisDisponiveis.ToString();
        _clientes.Valor = resumo.Clientes.ToString();
        _adocoes.Valor = resumo.Adocoes.ToString();

        _grade.DataSource = resumo.Recentes;
        _vazio.Text = "Nenhuma adoção cadastrada.";
        _vazio.Visible = resumo.Recentes.Count == 0;
        _grade.Visible = resumo.Recentes.Count > 0;
    });
}
