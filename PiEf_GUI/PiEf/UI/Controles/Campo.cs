using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PiEf.UI.Controles;

/// <summary>Item de lista suspensa que guarda o objeto real do banco junto com o texto exibido.</summary>
public sealed class ItemSelecao
{
    public object Valor { get; }
    public string Texto { get; }
    public ItemSelecao(object valor, string texto) { Valor = valor; Texto = texto; }
    public override string ToString() => Texto;
}

/// <summary>Campo de formulário: rótulo + entrada (texto, senha ou lista) + mensagem de erro/ajuda.</summary>
public class Campo : UserControl
{
    private readonly Label _rotulo = new();
    private readonly PainelSuave _caixa = new();
    private readonly TextBox _texto = new();
    private readonly ComboBox _combo = new();
    private readonly Label _rodape = new();
    private bool _foco;
    private bool _temErro;
    private string _ajuda = string.Empty;
    private readonly Label _icone = new();

    public Campo()
    {
        BackColor = Color.White;
        Size = new Size(380, 86);

        _rotulo.AutoSize = false;
        _rotulo.Font = Tema.Negrito;
        _rotulo.ForeColor = Tema.Texto;
        _icone.AutoSize = false;
        _icone.Font = Tema.Icone;
        _icone.ForeColor = Tema.Primaria;
        _icone.TextAlign = ContentAlignment.MiddleLeft;
        _icone.Visible = false;

        _caixa.BackColor = Color.White;
        _caixa.Paint += DesenharCaixa;
        _caixa.Click += (_, _) => _texto.Focus();

        _texto.BorderStyle = BorderStyle.None;
        _texto.Font = Tema.Campo;
        _texto.BackColor = Color.White;
        _texto.ForeColor = Tema.Texto;
        _texto.Enter += (_, _) => { _foco = true; _caixa.Invalidate(); };
        _texto.Leave += (_, _) => { _foco = false; _caixa.Invalidate(); };
        _texto.TextChanged += (_, _) => { if (_temErro) LimparErro(); };

        _combo.Font = Tema.Campo;
        _combo.Visible = false;
        _combo.TextChanged += (_, _) => { if (_temErro) LimparErro(); };
        _combo.SelectedIndexChanged += (_, _) => { if (_temErro) LimparErro(); };

        _rodape.AutoSize = false;
        _rodape.Font = Tema.Pequena;

        _caixa.Controls.Add(_texto);
        Controls.Add(_rotulo);
        Controls.Add(_caixa);
        Controls.Add(_combo);
        Controls.Add(_rodape);
        Controls.Add(_icone);

        Resize += (_, _) => Disposicao();
        Disposicao();
        AtualizarRodape();
    }

    public string Rotulo { get => _rotulo.Text; set => _rotulo.Text = value; }
        /// <summary>Ícone opcional exibido ao lado do rótulo.</summary>
    public string Icone
    {
        get => _icone.Text;
        set { _icone.Text = value; _icone.Visible = !string.IsNullOrEmpty(value); Disposicao(); }
    }
    public string Texto
    {
        get => _combo.Visible ? _combo.Text : _texto.Text;
        set { if (_combo.Visible) _combo.Text = value; else _texto.Text = value; }
    }

    /// <summary>Esconde os caracteres digitados.</summary>
    public bool Senha { get => _texto.UseSystemPasswordChar; set => _texto.UseSystemPasswordChar = value; }

    /// <summary>Texto de exemplo dentro do campo vazio.</summary>
    public string Dica { set => _texto.PlaceholderText = value; }

    /// <summary>Texto de orientação exibido abaixo do campo enquanto não houver erro.</summary>
    public string Ajuda { get => _ajuda; set { _ajuda = value; AtualizarRodape(); } }

    public object? ItemSelecionado => _combo.SelectedItem;

    public void ConfigurarSelecao(IEnumerable<object> itens, bool somenteLista)
    {
        _combo.Items.Clear();
        _combo.Items.AddRange(itens.ToArray());
        _combo.DropDownStyle = somenteLista ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
        _caixa.Visible = false;
        _texto.Visible = false;
        _combo.Visible = true;
        Disposicao();
    }

    public void SelecionarItem(Predicate<object> criterio)
    {
        for (int i = 0; i < _combo.Items.Count; i++)
        {
            if (_combo.Items[i] != null && criterio(_combo.Items[i]!))
            {
                _combo.SelectedIndex = i;
                return;
            }
        }
    }

    public void MostrarErro(string? mensagem)
    {
        _temErro = !string.IsNullOrEmpty(mensagem);
        _rodape.Text = mensagem ?? string.Empty;
        _rodape.ForeColor = _temErro ? Tema.Perigo : Tema.TextoSuave;
        if (!_temErro) _rodape.Text = _ajuda;
        _caixa.Invalidate();
    }

    public void LimparErro() => MostrarErro(null);

    public void FocarCampo()
    {
        if (_combo.Visible) _combo.Focus(); else _texto.Focus();
    }

    private void AtualizarRodape()
    {
        if (!_temErro)
        {
            _rodape.Text = _ajuda;
            _rodape.ForeColor = Tema.TextoSuave;
        }
    }

    private void Disposicao()
    {
        int w = Width;
        int recuo = _icone.Visible ? 28 : 0;
        _icone.SetBounds(0, 0, 26, 20);
        _rotulo.SetBounds(recuo, 0, w - recuo, 20);
        _caixa.SetBounds(0, 22, w, 42);
        _texto.Width = w - 24;
        _texto.Location = new Point(12, Math.Max(0, (_caixa.Height - _texto.Height) / 2));
        _combo.SetBounds(0, 22 + Math.Max(0, (42 - _combo.Height) / 2), w, _combo.Height);
        _rodape.SetBounds(0, 66, w, 18);
    }

    private void DesenharCaixa(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);

        var r = new Rectangle(1, 1, _caixa.Width - 3, _caixa.Height - 3);
        using var path = Tema.Arredondado(r, 8);
        using var fundo = new SolidBrush(Color.White);
        g.FillPath(fundo, path);

        Color cor = _temErro ? Tema.Perigo : _foco ? Tema.Primaria : Tema.Borda;
        using var pen = new Pen(cor, (_foco || _temErro) ? 2f : 1f);
        g.DrawPath(pen, path);
    }
}
