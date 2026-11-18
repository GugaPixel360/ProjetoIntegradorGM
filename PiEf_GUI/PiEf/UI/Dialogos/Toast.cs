using System.Drawing;
using System.Windows.Forms;

namespace PiEf.UI.Dialogos;

public enum TipoMensagem { Sucesso, Erro, Aviso, Info }

/// <summary>Aviso rápido no canto da janela (não bloqueia e some sozinho).</summary>
public sealed class Toast : Form
{
    private static Toast? _atual;

    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 3500 };
    private readonly string _mensagem;
    private readonly TipoMensagem _tipo;

    private Toast(string mensagem, TipoMensagem tipo)
    {
        _mensagem = mensagem;
        _tipo = tipo;

        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.White;
        DoubleBuffered = true;

        Size texto = TextRenderer.MeasureText(mensagem, Tema.Normal, new Size(290, 0),
            TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
        Size = new Size(400, Math.Max(64, texto.Height + 30));

        _timer.Tick += (_, _) => { _timer.Stop(); Close(); };
        Click += (_, _) => Close();
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE: não rouba o foco do usuário
            return cp;
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _timer.Start();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timer.Stop();
        _timer.Dispose();
        if (ReferenceEquals(_atual, this)) _atual = null;
        base.OnFormClosed(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;

        (Color cor, Color clara, string icone) = _tipo switch
        {
            TipoMensagem.Sucesso => (Tema.Sucesso, Tema.SucessoClaro, Icones.Sucesso),
            TipoMensagem.Erro => (Tema.Perigo, Tema.PerigoClaro, Icones.Cancelar),
            TipoMensagem.Aviso => (Tema.Destaque, Tema.DestaqueClaro, Icones.Aviso),
            _ => (Tema.Primaria, Tema.PrimariaClara, Icones.Info)
        };

        using (var borda = new Pen(Tema.Borda))
            g.DrawRectangle(borda, 0, 0, Width - 1, Height - 1);
        using (var barra = new SolidBrush(cor))
            g.FillRectangle(barra, 0, 0, 5, Height);

        var quadro = new Rectangle(22, (Height - 36) / 2, 36, 36);
        using (var pincel = new SolidBrush(clara))
            g.FillEllipse(pincel, quadro);
        TextRenderer.DrawText(g, icone, Tema.Icone, quadro, cor,
            TextFormatFlags.NoPadding | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        TextRenderer.DrawText(g, _mensagem, Tema.Normal, new Rectangle(72, 0, 310, Height), Tema.Texto,
            TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    public static void Mostrar(Control? origem, string mensagem, TipoMensagem tipo = TipoMensagem.Sucesso)
    {
        Form? dono = origem as Form ?? origem?.FindForm();
        if (dono == null || dono.IsDisposed) return;

        if (_atual != null && !_atual.IsDisposed) _atual.Close();

        var toast = new Toast(mensagem, tipo);
        toast.Location = new Point(dono.Right - toast.Width - 28, dono.Bottom - toast.Height - 40);
        _atual = toast;
        toast.Show(dono);
    }
}
