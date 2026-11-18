using PiEf.Models;
using PiEf.Services;
using PiEf.UI.Controles;

namespace PiEf.UI.Dialogos;

/// <summary>Cadastro e edição de animal (usa Animal.CadastrarAnimal / AtualizarAnimal).</summary>
public sealed class FormAnimal : FormDialogo
{
    private readonly Animal? _existente;
    private readonly Campo _nome = new() { Rotulo = "Nome" };
    private readonly Campo _especie = new() { Rotulo = "Espécie" };
    private readonly Campo _raca = new() { Rotulo = "Raça" };
    private readonly Campo _idade = new() { Rotulo = "Idade" };
    private readonly Campo _sexo = new() { Rotulo = "Sexo" };

    public FormAnimal(Animal? existente = null)
        : base(existente == null ? "Novo animal" : "Editar animal",
               existente == null ? "Preencha os dados do animal." : "Altere os dados e salve.",
               existente == null ? "Cadastrar" : "Salvar alterações",
               Icones.Salvar)
    {
        _existente = existente;

        _especie.ConfigurarSelecao(new object[] { "Cachorro", "Gato", "Coelho", "Ave" }, somenteLista: false);

        var opcoesSexo = new List<object> { "Macho", "Fêmea" };
        if (existente != null && !string.IsNullOrWhiteSpace(existente.Sexo) && !opcoesSexo.Contains(existente.Sexo))
            opcoesSexo.Add(existente.Sexo); // preserva valores antigos digitados livremente no console
        _sexo.ConfigurarSelecao(opcoesSexo, somenteLista: true);

        AdicionarCampo(_nome);
        AdicionarCampo(_especie);
        AdicionarCampo(_raca);
        AdicionarCampo(_idade);
        AdicionarCampo(_sexo);
        Finalizar();

        if (existente != null)
        {
            _nome.Texto = existente.Nome;
            _especie.Texto = existente.Especie;
            _raca.Texto = existente.Raca;
            _idade.Texto = existente.Idade.ToString();
            _sexo.Texto = existente.Sexo;
        }
    }

    protected override async Task<bool> SalvarAsync()
    {
        bool valido = true;

        string? erro = Validacao.Obrigatorio(_nome.Texto, "o nome");
        if (erro != null) { _nome.MostrarErro(erro); valido = false; }

        erro = Validacao.Obrigatorio(_especie.Texto, "a espécie");
        if (erro != null) { _especie.MostrarErro(erro); valido = false; }

        erro = Validacao.Obrigatorio(_raca.Texto, "a raça");
        if (erro != null) { _raca.MostrarErro(erro); valido = false; }

        erro = Validacao.Idade(_idade.Texto, 0, out int idade);
        if (erro != null) { _idade.MostrarErro(erro); valido = false; }

        erro = Validacao.Obrigatorio(_sexo.Texto, "o sexo");
        if (erro != null) { _sexo.MostrarErro(erro); valido = false; }

        if (!valido) return false;

        var animal = new Animal(_nome.Texto.Trim(), _especie.Texto.Trim(), _raca.Texto.Trim(), idade, _sexo.Texto.Trim());

        if (_existente == null)
        {
            await Animal.CadastrarAnimal(animal);
            return true;
        }

        animal.Id = _existente.Id;
        Resultado r = await Operacoes.AtualizarAnimalAsync(animal);

        if (!r.Sucesso)
        {
            FormMensagem.Erro(this, r.Mensagem);
            return false;
        }

        return true;
    }
}
