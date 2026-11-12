namespace PiEf.Models;

namespace PiEf.Models;

public class Animal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string Raca { get; set; } = string.Empty;

    public int Idade { get; set; }

    public string Sexo { get; set; } = string.Empty;

    public bool Disponivel { get; set; } = true;


    public Animal (string nome, string especie, string raca, int idade, string sexo)
    {
        Nome = nome;
        Especie = especie;
        Raca = raca;
        Idade = idade;
        Sexo = sexo;
    }
}













