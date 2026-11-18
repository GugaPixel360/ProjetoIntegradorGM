using System.Drawing;
using System.Windows.Forms;
using PiEf.UI.Controles;

namespace PiEf.UI.Dialogos;

/// <summary>Base dos formulários modais (título, campos empilhados e botões Salvar/Cancelar).</summary>
public abstract class FormDialogo : Form
{
    private readonly BotaoModerno _btnConfirmar = new();
    private readonly BotaoModerno _btnCancelar = new();
    private readonly List<Campo> _campos = new();
    private int _y = 100;
    private bool _bloqueado;

    protected FormDialogo(string titulo, string subtitulo, string textoConfirmar, string icone)
    {
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

        Controls.Add(new Label
        {
            Text = titulo,
            Font = Tema.Titulo,
            ForeColor = Tema.Texto,
            AutoSize = false,
            Location = new Point(28, 22),
            Size = new Size(404, 38)
        });

        Controls.Add(new Label
        {
            Text = subtitulo,
            Font = Tema.Subtitulo,
            ForeColor = Tema.TextoSuave,
            AutoSize = false,
            Location = new Point(28, 62),
            Size = new Size(404, 24)
        });

        _btnConfirmar.Text = textoConfirmar;
        _btnConfirmar.Icone = icone;
        _btnConfirmar.Size = new Size(160, 42);
        _btnConfirmar.Click += async (_, _) => await ConfirmarAsync();

        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Estilo = EstiloBotao.Secundario;
        _btnCancelar.Size = new Size(110, 42);
        _btnCancelar.DialogResult = DialogResult.Cancel;

        Controls.Add(_btnConfirmar);
        Controls.Add(_btnCancelar);

        AcceptButton = _btnConfirmar;
        CancelButton = _btnCancelar;
    }

    protected IReadOnlyList<Campo> Campos => _campos;

    protected void AdicionarCampo(Campo campo)
    {
        campo.Location = new Point(28, _y);
        campo.Width = 404;
        Controls.Add(campo);
        _campos.Add(campo);
        _y += campo.Height + 4;
    }

    /// <summary>Deve ser chamado pelo formulário filho depois de adicionar todos os campos.</summary>
    protected void Finalizar()
    {
        int yBotoes = _y + 16;
        _btnConfirmar.Location = new Point(460 - 28 - _btnConfirmar.Width, yBotoes);
        _btnCancelar.Location = new Point(_btnConfirmar.Left - 10 - _btnCancelar.Width, yBotoes);
        ClientSize = new Size(460, yBotoes + 42 + 28);
    }

    protected void DesabilitarConfirmacao() { _bloqueado = true; _btnConfirmar.Enabled = false; }

    /// <summary>Valida e grava. Retorna true para fechar o formulário com sucesso.</summary>
    protected abstract Task<bool> SalvarAsync();

    private async Task ConfirmarAsync()
    {
        _btnConfirmar.Enabled = false;
        try
        {
            foreach (var c in _campos) c.LimparErro();

            if (await SalvarAsync())
                DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            FormMensagem.Erro(this, PiEf.Services.Mensagens.DeErro(ex));
        }
        finally
        {
            if (!IsDisposed && !_bloqueado) _btnConfirmar.Enabled = true;
        }
    }
}
