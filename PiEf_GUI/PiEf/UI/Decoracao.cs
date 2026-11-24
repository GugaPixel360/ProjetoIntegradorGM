using System.Drawing;
using System.Drawing.Drawing2D;

namespace PiEf.UI;

/// <summary>Desenhos decorativos (patinhas, cachorro, gato e formas suaves) da tela de login.</summary>
public static class Decoracao
{
    public static void Elipse(Graphics g, float cx, float cy, float rx, float ry, Color cor)
    {
        using var pincel = new SolidBrush(cor);
        g.FillEllipse(pincel, cx - rx, cy - ry, rx * 2f, ry * 2f);
    }

    /// <summary>Patinha centrada em (cx, cy). "tamanho" é a largura aproximada em pixels.</summary>
    public static void Patinha(Graphics g, float cx, float cy, float tamanho, float graus, Color cor)
    {
        GraphicsState estado = g.Save();
        g.TranslateTransform(cx, cy);
        g.RotateTransform(graus);
        float u = tamanho / 100f;
        g.ScaleTransform(u, u);

        using var pincel = new SolidBrush(cor);
        g.FillEllipse(pincel, -28f, -4f, 56f, 46f);   // almofada
        g.FillEllipse(pincel, -50f, -20f, 22f, 30f);  // dedos
        g.FillEllipse(pincel, -26f, -46f, 22f, 32f);
        g.FillEllipse(pincel, 4f, -46f, 22f, 32f);
        g.FillEllipse(pincel, 28f, -20f, 22f, 30f);

        g.Restore(estado);
    }

    /// <summary>Gato de frente, só o contorno. (x, y) é o canto superior esquerdo.</summary>
    public static void Gato(Graphics g, float x, float y, float tamanho, Color cor, float espessuraPx)
    {
        GraphicsState estado = g.Save();
        g.TranslateTransform(x, y);
        float u = tamanho / 100f;
        g.ScaleTransform(u, u);

        using var caneta = new Pen(cor, espessuraPx / u)
        {
            LineJoin = LineJoin.Round,
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        using var pincel = new SolidBrush(cor);

        g.DrawEllipse(caneta, 12, 34, 76, 56);                                               // cabeça
        g.DrawLines(caneta, new PointF[] { new(18, 52), new(22, 10), new(46, 36) });         // orelha esquerda
        g.DrawLines(caneta, new PointF[] { new(82, 52), new(78, 10), new(54, 36) });         // orelha direita
        g.FillEllipse(pincel, 33, 55, 8, 10);                                                // olhos
        g.FillEllipse(pincel, 59, 55, 8, 10);
        g.FillPolygon(pincel, new PointF[] { new(46, 68), new(54, 68), new(50, 73) });       // nariz
        g.DrawLine(caneta, 50, 73, 50, 78);
        g.DrawArc(caneta, 41, 72, 9, 9, 0, 120);                                             // boca
        g.DrawArc(caneta, 50, 72, 9, 9, 60, 120);
        g.DrawLine(caneta, 12, 64, -4, 60);                                                  // bigodes
        g.DrawLine(caneta, 12, 70, -4, 72);
        g.DrawLine(caneta, 88, 64, 104, 60);
        g.DrawLine(caneta, 88, 70, 104, 72);
        g.DrawCurve(caneta, new PointF[] { new(24, 84), new(16, 94), new(12, 106) });        // ombros
        g.DrawCurve(caneta, new PointF[] { new(76, 84), new(84, 94), new(88, 106) });

        g.Restore(estado);
    }

    /// <summary>Cachorro de frente, só o contorno. (x, y) é o canto superior esquerdo.</summary>
    public static void Cachorro(Graphics g, float x, float y, float tamanho, Color cor, float espessuraPx)
    {
        GraphicsState estado = g.Save();
        g.TranslateTransform(x, y);
        float u = tamanho / 100f;
        g.ScaleTransform(u, u);

        using var caneta = new Pen(cor, espessuraPx / u)
        {
            LineJoin = LineJoin.Round,
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        using var pincel = new SolidBrush(cor);

        g.DrawEllipse(caneta, 28, 30, 50, 54);   // cabeça
        g.DrawEllipse(caneta, 8, 34, 28, 50);    // orelha caída esquerda
        g.DrawEllipse(caneta, 68, 36, 22, 40);   // orelha direita
        g.FillEllipse(pincel, 41, 50, 6, 8);     // olhos
        g.FillEllipse(pincel, 59, 50, 6, 8);
        g.FillEllipse(pincel, 47, 62, 12, 9);    // nariz
        g.DrawLine(caneta, 53, 71, 53, 76);
        g.DrawArc(caneta, 44, 72, 9, 8, 0, 120); // boca
        g.DrawArc(caneta, 53, 72, 9, 8, 60, 120);
        g.DrawCurve(caneta, new PointF[] { new(34, 84), new(26, 96), new(22, 108) });        // ombros
        g.DrawCurve(caneta, new PointF[] { new(72, 84), new(80, 96), new(84, 108) });

        g.Restore(estado);
    }
}