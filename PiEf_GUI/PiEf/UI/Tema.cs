using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PiEf.UI;

/// <summary>Identidade visual única do sistema: cores, fontes e estilo de tabelas.</summary>
public static class Tema
{
    // Paleta: petróleo (estrutura), turquesa (ação), âmbar (destaque), neutros frios.
    public static readonly Color Sidebar = Hex("#0F3B40");
    public static readonly Color SidebarHover = Hex("#17505A");
    public static readonly Color SidebarAtivo = Hex("#1D6A6A");
    public static readonly Color Primaria = Hex("#178A83");
    public static readonly Color PrimariaEscura = Hex("#11706A");
    public static readonly Color PrimariaClara = Hex("#E2F3F1");
    public static readonly Color Destaque = Hex("#F2A93B");
    public static readonly Color DestaqueClaro = Hex("#FDF1DC");
    public static readonly Color Fundo = Hex("#F2F5F6");
    public static readonly Color Borda = Hex("#DCE4E8");
    public static readonly Color Texto = Hex("#1A2B31");
    public static readonly Color TextoSuave = Hex("#62747C");
    public static readonly Color Perigo = Hex("#C0412F");
    public static readonly Color PerigoEscuro = Hex("#A33323");
    public static readonly Color PerigoClaro = Hex("#FBEAE7");
    public static readonly Color Sucesso = Hex("#2B8A5A");
    public static readonly Color SucessoClaro = Hex("#E3F4EB");

    public static readonly Font Normal = new("Segoe UI", 10f);
    public static readonly Font Negrito = new("Segoe UI Semibold", 10f);
    public static readonly Font Campo = new("Segoe UI", 10.5f);
    public static readonly Font Pequena = new("Segoe UI", 9f);
    public static readonly Font Subtitulo = new("Segoe UI", 10.5f);
    public static readonly Font SubtituloNegrito = new("Segoe UI Semibold", 12f);
    public static readonly Font Titulo = new("Segoe UI Semibold", 20f);
    public static readonly Font TituloGrande = new("Segoe UI Semibold", 26f);
    public static readonly Font Numero = new("Segoe UI Semibold", 28f);
    public static readonly Font Icone = new("Segoe UI Emoji", 11f);
    public static readonly Font IconeGrande = new("Segoe UI Emoji", 36f);

    public static Color Hex(string cor) => ColorTranslator.FromHtml(cor);

    public static GraphicsPath Arredondado(Rectangle r, int raio)
    {
        int d = raio * 2;
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    /// <summary>Aplica o visual padrão a uma tabela (somente leitura, linha inteira selecionável).</summary>
    public static void EstilizarGrade(DataGridView g)
    {
        g.AutoGenerateColumns = false;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.ReadOnly = true;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.RowHeadersVisible = false;
        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = Color.White;
        g.GridColor = Borda;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 42;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Hex("#EEF3F5"),
            ForeColor = TextoSuave,
            Font = Negrito,
            Padding = new Padding(10, 0, 0, 0),
            SelectionBackColor = Hex("#EEF3F5"),
            SelectionForeColor = TextoSuave,
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };

        g.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Texto,
            Font = Normal,
            Padding = new Padding(10, 0, 0, 0),
            SelectionBackColor = PrimariaClara,
            SelectionForeColor = Texto,
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };

        g.RowTemplate.Height = 44;
    }

    public static void AdicionarColuna(DataGridView g, string titulo, string propriedade, float peso = 1f, string? formato = null)
    {
        var coluna = new DataGridViewTextBoxColumn
        {
            Name = propriedade,
            HeaderText = titulo,
            DataPropertyName = propriedade,
            FillWeight = peso,
            MinimumWidth = 50,
            SortMode = DataGridViewColumnSortMode.NotSortable
        };

        if (formato != null)
            coluna.DefaultCellStyle.Format = formato;

        g.Columns.Add(coluna);
    }
}

/// <summary>Ícones em texto (Segoe UI Emoji): trocar aqui altera o sistema inteiro.</summary>
public static class Icones
{
    public const string Animais = "\U0001F43E";
    public const string Painel = "\U0001F3E0";
    public const string Clientes = "\U0001F465";
    public const string Adocoes = "\u2764";
    public const string Novo = "\u2795";
    public const string Editar = "\u270F";
    public const string Excluir = "\U0001F5D1";
    public const string Buscar = "\U0001F50D";
    public const string Salvar = "\U0001F4BE";
    public const string Sair = "\U0001F6AA";
    public const string Usuario = "\U0001F464";
    public const string Atualizar = "\U0001F504";
    public const string Cancelar = "\u2716";
    public const string Sucesso = "\u2714";
    public const string Aviso = "\u26A0";
    public const string Info = "\u2139";
    public const string Email = "\u2709";
    public const string Cadeado = "\U0001F512";
}
