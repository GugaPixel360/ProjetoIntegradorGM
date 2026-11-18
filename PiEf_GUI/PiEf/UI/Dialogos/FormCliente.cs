using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;

namespace PiEf.UI.Dialogos;

/// <summary>Edição de cliente pelo administrador (usa Adotante.AtualizarAdotante).</summary>
public sealed class FormCliente : FormDialogo
{
    private readonly Adotante _cliente;
    private readonly Campo _nome = new() { Rotulo = "Nome" };
    private readonly Campo _idade = new() { Rotulo = "Idade" };
    private readonly Campo _email = new() { Rotulo = "E-mail" };

    public FormCliente(Adotante cliente)
        : base("Editar cliente", "Altere os dados e salve.", "Salvar alterações", Icones.Salvar)
    {
        _cliente = cliente;

        AdicionarCampo(_nome);
        AdicionarCampo(_idade);
        AdicionarCampo(_email);
        Finalizar();

        _nome.Texto = cliente.Nome;
        _idade.Texto = cliente.Idade.ToString();
        _email.Texto = cliente.Email;
    }

    protected override async Task<bool> SalvarAsync()
    {
        var dados = await Formularios.LerCliente(_nome, _idade, _email, Validacao.IdadeMinimaAtualizacao);
        if (dados == null) return false;

        var atualizado = new Adotante
        {
            Id = _cliente.Id,
            Nome = dados.Value.Nome,
            Idade = dados.Value.Idade,
            Email = dados.Value.Email,
            Senha = _cliente.Senha,
            TipoUsuario = _cliente.TipoUsuario
        };

        Resultado r = await Operacoes.AtualizarClienteAsync(atualizado);

        if (!r.Sucesso)
        {
            FormMensagem.Erro(this, r.Mensagem);
            return false;
        }

        return true;
    }
}
