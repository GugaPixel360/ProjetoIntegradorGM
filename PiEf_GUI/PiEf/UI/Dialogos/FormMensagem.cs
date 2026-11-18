using System.Drawing;
using System.Windows.Forms;
using PiEf.UI.Controles;

namespace PiEf.UI.Dialogos;

/// <summary>Caixa de diálogo padronizada para confirmações, erros e avisos.</summary>
public sealed class FormMensagem : Form
{
    private readonly TipoMensagem _tipo;

    private FormMensagem(string titulo, string mensagem, TipoMensagem tipo, string textoConfirmar, string? textoCancelar, bool perigo)
    {
        _tipo = tipo;

        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode = AutoScaleMode.Dpi;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        Font = Tema.Normal;
        Text = titulo;

        const int larguraTexto = 340;

        var lblTitulo = new Label
        {
            Text = titulo,
            Font = Tema.SubtituloNegrito,
            ForeColor = Tema.Texto,
            AutoSize = false,
            Location = new Point(92, 26),
            Size = new Size(larguraTexto, 28)
        };

        Size medida = TextRenderer.MeasureText(mensagem, Tema.Normal, new Size(larguraTexto, 0),
            TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

        var lblMensagem = new Label
        {
            Text = mensagem,
            Font = Tema.Normal,
            ForeColor = Tema.TextoSuave,
            AutoSize = false,
            Location = new Point(92, 58),
            Size = new Size(larguraTexto, medida.Height + 10)
        };

        int yBotoes = Math.Max(lblMensagem.Bottom, 90) + 22;

        var btnConfirmar = new BotaoModerno
        {
            Text = textoConfirmar,
            Estilo = perigo ? EstiloBotao.Perigo : EstiloBotao.Primario,
            DialogResult = DialogResult.OK,
            Size = new Size(120, 40)
        };

        BotaoModerno? btnCancelar = null;
        if (textoCancelar != null)
        {
            btnCancelar = new BotaoModerno
            {
                Text = textoCancelar,
                Estilo = EstiloBotao.Secundario,
                DialogResult = DialogResult.Cancel,
                Size = new Size(110, 40)
            };
        }

        int direita = 460 - 28;
        btnConfirmar.Location = new Point(direita - btnConfirmar.Width, yBotoes);
        if (btnCancelar != null)
            btnCancelar.Location = new Point(btnConfirmar.Left - 10 - btnCancelar.Width, yBotoes);

        Controls.Add(lblTitulo);
        Controls.Add(lblMensagem);
        Controls.Add(btnConfirmar);
        if (btnCancelar != null) Controls.Add(btnCancelar);

        // Em ações destrutivas, "Enter" cancela (mais seguro); nos demais casos, confirma.
        AcceptButton = (perigo && btnCancelar != null) ? btnCancelar : btnConfirmar;
        CancelButton = btnCancelar ?? btnConfirmar;

        ClientSize = new Size(460, yBotoes + 40 + 24);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        (Color cor, Color clara, string icone) = _tipo switch
        {
            TipoMensagem.Sucesso => (Tema.Sucesso, Tema.SucessoClaro, Icones.Sucesso),
            TipoMensagem.Erro => (Tema.Perigo, Tema.PerigoClaro, Icones.Cancelar),
            TipoMensagem.Aviso => (Tema.Destaque, Tema.DestaqueClaro, Icones.Aviso),
            _ => (Tema.Primaria, Tema.PrimariaClara, Icones.Info)
        };

        var quadro = new Rectangle(28, 26, 48, 48);
        using (var pincel = new SolidBrush(clara))
            g.FillEllipse(pincel, quadro);
        TextRenderer.DrawText(g, icone, Tema.Icone, quadro, cor,
            TextFormatFlags.NoPadding | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private static DialogResult Exibir(FormMensagem f, IWin32Window? dono)
        => dono == null ? f.ShowDialog() : f.ShowDialog(dono);

    public static bool Confirmar(IWin32Window? dono, string titulo, string mensagem,
        string textoConfirmar = "Confirmar", bool perigo = false)
    {
        using var f = new FormMensagem(titulo, mensagem, perigo ? TipoMensagem.Aviso : TipoMensagem.Info,
            textoConfirmar, "Cancelar", perigo);
        return Exibir(f, dono) == DialogResult.OK;
    }

    public static void Erro(IWin32Window? dono, string mensagem, string titulo = "Não foi possível concluir")
    {
        using var f = new FormMensagem(titulo, mensagem, TipoMensagem.Erro, "Entendi", null, false);
        Exibir(f, dono);
    }

    public static void Aviso(IWin32Window? dono, string mensagem, string titulo = "Atenção")
    {
        using var f = new FormMensagem(titulo, mensagem, TipoMensagem.Aviso, "Entendi", null, false);
        Exibir(f, dono);
    }

    public static void Informar(IWin32Window? dono, string mensagem, string titulo = "Tudo certo")
    {
        using var f = new FormMensagem(titulo, mensagem, TipoMensagem.Sucesso, "OK", null, false);
        Exibir(f, dono);
    }
}
