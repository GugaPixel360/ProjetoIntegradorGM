using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PiEf.UI.Controles;

public enum EstiloBotao { Primario, Secundario, Perigo, Menu }

/// <summary>Botão plano com cantos arredondados, ícone e estados de hover/foco/desabilitado.</summary>
public class BotaoModerno : Button
{
    private bool _hover;
    private bool _pressionado;
    private EstiloBotao _estilo = EstiloBotao.Primario;
    private string _icone = string.Empty;
    private bool _ativo;

    public EstiloBotao Estilo { get => _estilo; set { _estilo = value; Invalidate(); } }
    public string Icone { get => _icone; set { _icone = value; Invalidate(); } }
    /// <summary>Usado pelo estilo Menu para marcar a tela atual.</summary>
    public bool Ativo { get => _ativo; set { _ativo = value; Invalidate(); } }
    public int Raio { get; set; } = 8;

    public BotaoModerno()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInPaintEvent |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = Tema.Negrito;
        Height = 40;
        Cursor = Cursors.Hand;
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressionado = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _pressionado = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _pressionado = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

    private void ObterCores(out Color fundo, out Color texto, out Color borda)
    {
        borda = Color.Empty;
        switch (Estilo)
        {
            case EstiloBotao.Secundario:
                fundo = _pressionado ? Tema.Hex("#E4EAED") : _hover ? Tema.Hex("#F0F4F6") : Color.White;
                texto = Enabled ? Tema.Texto : Tema.Hex("#A5B3B9");
                borda = Tema.Borda;
                break;

            case EstiloBotao.Perigo:
                fundo = !Enabled ? Tema.Hex("#E3B9B2") : (_pressionado || _hover) ? Tema.PerigoEscuro : Tema.Perigo;
                texto = Color.White;
                break;

            case EstiloBotao.Menu:
                fundo = Ativo ? Tema.SidebarAtivo : _hover ? Tema.SidebarHover : Tema.Sidebar;
                texto = Ativo ? Color.White : Tema.Hex("#C5DADD");
                break;

            default:
                fundo = !Enabled ? Tema.Hex("#B9C7CB") : (_pressionado || _hover) ? Tema.PrimariaEscura : Tema.Primaria;
                texto = Color.White;
                break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Tema.Fundo);

        ObterCores(out Color fundo, out Color texto, out Color borda);
        var r = new Rectangle(0, 0, Width - 1, Height - 1);

        using (var path = Tema.Arredondado(r, Raio))
        {
            using var pincel = new SolidBrush(fundo);
            g.FillPath(pincel, path);

            if (borda != Color.Empty)
            {
                using var pen = new Pen(borda);
                g.DrawPath(pen, path);
            }

            if (Focused && ShowFocusCues && Estilo != EstiloBotao.Menu)
            {
                using var penFoco = new Pen(Tema.Destaque, 2f);
                g.DrawPath(penFoco, path);
            }
        }

        if (Estilo == EstiloBotao.Menu && Ativo)
        {
            using var marca = new SolidBrush(Tema.Destaque);
            g.FillRectangle(marca, 0, 10, 4, Height - 20);
        }

        DesenharConteudo(g, texto);
    }

    private void DesenharConteudo(Graphics g, Color cor)
    {
        var sem = new Size(int.MaxValue, int.MaxValue);
        var medida = TextFormatFlags.NoPadding;

        Size tIcone = string.IsNullOrEmpty(Icone) ? Size.Empty : TextRenderer.MeasureText(g, Icone, Tema.Icone, sem, medida);
        Size tTexto = string.IsNullOrEmpty(Text) ? Size.Empty : TextRenderer.MeasureText(g, Text, Font, sem, medida);

        int espaco = (tIcone.Width > 0 && tTexto.Width > 0) ? 8 : 0;
        int total = tIcone.Width + espaco + tTexto.Width;
        int x = Estilo == EstiloBotao.Menu ? 20 : Math.Max(4, (Width - total) / 2);

        var flags = TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine | TextFormatFlags.Left;

        if (tIcone.Width > 0)
        {
            TextRenderer.DrawText(g, Icone, Tema.Icone, new Rectangle(x, 0, tIcone.Width + 4, Height), cor, flags);
            x += tIcone.Width + espaco;
        }

        if (tTexto.Width > 0)
            TextRenderer.DrawText(g, Text, Font, new Rectangle(x, 0, Math.Max(10, Width - x - 4), Height), cor, flags | TextFormatFlags.EndEllipsis);
    }
}
