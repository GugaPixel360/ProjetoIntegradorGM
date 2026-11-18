using System.Drawing;
using System.Windows.Forms;

namespace PiEf.UI.Controles;

/// <summary>Panel com buffer duplo (sem cintilação) para desenho personalizado.</summary>
public class PainelSuave : Panel
{
    public PainelSuave()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }
}

/// <summary>Superfície branca com borda fina, usada para agrupar conteúdo.</summary>
public class Cartao : PainelSuave
{
    public Cartao()
    {
        BackColor = Color.White;
        Padding = new Padding(1);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Tema.Borda);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}
