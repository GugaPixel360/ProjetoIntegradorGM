using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;

namespace PiEf.UI.Dialogos;

/// <summary>Registro de adoção pelo administrador: escolhe cliente e animal reais do banco.</summary>
public sealed class FormAdocao : FormDialogo
{
    private readonly Animal? _animalPreSelecionado;
    private readonly Campo _cliente = new() { Rotulo = "Cliente" };
    private readonly Campo _animal = new() { Rotulo = "Animal disponível" };

    public FormAdocao(Animal? animalPreSelecionado = null)
        : base("Nova adoção", "Escolha quem está adotando e qual animal.", "Registrar adoção", Icones.Adocoes)
    {
        _animalPreSelecionado = animalPreSelecionado;

        _cliente.ConfigurarSelecao(Array.Empty<object>(), somenteLista: true);
        _animal.ConfigurarSelecao(Array.Empty<object>(), somenteLista: true);

        AdicionarCampo(_cliente);
        AdicionarCampo(_animal);
        Finalizar();
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);

        try
        {
            List<Adotante> clientes = await Adotante.ListarAdotantes();
            List<Animal> animais = await Animal.ListarAnimaisDisponiveis();

            _cliente.ConfigurarSelecao(
                clientes.Select(c => (object)new ItemSelecao(c, $"{c.Nome}  ({c.Email})")),
                somenteLista: true);

            _animal.ConfigurarSelecao(
                animais.Select(a => (object)new ItemSelecao(a, $"{a.Nome}  –  {a.Especie}, {a.Raca}")),
                somenteLista: true);

            if (clientes.Count == 0)
                _cliente.MostrarErro("Nenhum cliente cadastrado.");

            if (animais.Count == 0)
                _animal.MostrarErro("Nenhum animal disponível no momento.");

            if (_animalPreSelecionado != null)
            {
                int id = _animalPreSelecionado.Id;
                _animal.SelecionarItem(o => o is ItemSelecao i && i.Valor is Animal a && a.Id == id);
            }

            if (clientes.Count == 0 || animais.Count == 0)
                DesabilitarConfirmacao();
        }
        catch (Exception ex)
        {
            FormMensagem.Erro(this, Mensagens.DeErro(ex));
            DialogResult = DialogResult.Cancel;
        }
    }

    protected override async Task<bool> SalvarAsync()
    {
        bool valido = true;

        if (_cliente.ItemSelecionado is not ItemSelecao itemCliente)
        {
            _cliente.MostrarErro("Selecione o cliente.");
            valido = false;
            itemCliente = null!;
        }

        if (_animal.ItemSelecionado is not ItemSelecao itemAnimal)
        {
            _animal.MostrarErro("Selecione o animal.");
            valido = false;
            itemAnimal = null!;
        }

        if (!valido) return false;

        int idCliente = ((Adotante)itemCliente.Valor).Id;
        int idAnimal = ((Animal)itemAnimal.Valor).Id;

        Resultado r = await Operacoes.CadastrarAdocaoAsync(idCliente, idAnimal);

        if (!r.Sucesso)
        {
            FormMensagem.Erro(this, r.Mensagem);
            return false;
        }

        return true;
    }
}
