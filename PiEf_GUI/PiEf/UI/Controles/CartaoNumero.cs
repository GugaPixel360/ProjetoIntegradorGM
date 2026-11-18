using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PiEf.UI.Controles;

/// <summary>Indicador do painel: ícone, número grande e legenda.</summary>
public sealed class CartaoNumero : Cartao
{
    private readonly string _icone;
    private readonly Color _cor;
    private readonly Color _corClara;
    private readonly Label _valor = new();
    private readonly Label _legenda = new();

    public CartaoNumero(string legenda, string icone, Color cor, Color corClara)
    {
        _icone = icone;
        _cor = cor;
        _corClara = corClara;

        _legenda.Text = legenda;
        _legenda.Font = Tema.Normal;
        _legenda.ForeColor = Tema.TextoSuave;
        _legenda.AutoSize = false;
        _legenda.BackColor = Color.White;

        _valor.Text = "–";
        _valor.Font = Tema.Numero;
        _valor.ForeColor = Tema.Texto;
        _valor.AutoSize = false;
        _valor.BackColor = Color.White;

        Controls.Add(_valor);
        Controls.Add(_legenda);
        Resize += (_, _) => Posicionar();
        Posicionar();
    }

    public string Valor { set => _valor.Text = value; }

    private void Posicionar()
    {
        _valor.SetBounds(96, 22, Math.Max(40, Width - 110), 52);
        _legenda.SetBounds(96, 76, Math.Max(40, Width - 110), 24);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var quadro = new Rectangle(24, 28, 56, 56);
        using (var path = Tema.Arredondado(quadro, 14))
        using (var pincel = new SolidBrush(_corClara))
            g.FillPath(pincel, path);

        TextRenderer.DrawText(g, _icone, Tema.Icone, quadro, _cor,
            TextFormatFlags.NoPadding | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}
