using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PiEf.UI.Controles;

/// <summary>Caixa de busca com ícone; dispara TextoAlterado a cada digitação.</summary>
public class CampoBusca : PainelSuave
{
    private readonly TextBox _texto = new();
    private bool _foco;

    public event EventHandler? TextoAlterado;

    public string Texto => _texto.Text;
    public string Dica { set => _texto.PlaceholderText = value; }

    public CampoBusca()
    {
        BackColor = Tema.Fundo;
        Size = new Size(320, 42);

        _texto.BorderStyle = BorderStyle.None;
        _texto.Font = Tema.Campo;
        _texto.BackColor = Color.White;
        _texto.ForeColor = Tema.Texto;
        _texto.PlaceholderText = "Buscar...";
        _texto.TextChanged += (_, _) => TextoAlterado?.Invoke(this, EventArgs.Empty);
        _texto.Enter += (_, _) => { _foco = true; Invalidate(); };
        _texto.Leave += (_, _) => { _foco = false; Invalidate(); };

        Controls.Add(_texto);
        Resize += (_, _) => Posicionar();
        Click += (_, _) => _texto.Focus();
        Posicionar();
    }

    private void Posicionar()
    {
        _texto.SetBounds(42, Math.Max(0, (Height - _texto.Height) / 2), Math.Max(20, Width - 54), _texto.Height);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Tema.Fundo);

        var r = new Rectangle(1, 1, Width - 3, Height - 3);
        using var path = Tema.Arredondado(r, 8);
        using var fundo = new SolidBrush(Color.White);
        g.FillPath(fundo, path);
        using var pen = new Pen(_foco ? Tema.Primaria : Tema.Borda, _foco ? 2f : 1f);
        g.DrawPath(pen, path);

        TextRenderer.DrawText(g, Icones.Buscar, Tema.Icone, new Rectangle(12, 0, 28, Height), Tema.TextoSuave,
            TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
    }
}
