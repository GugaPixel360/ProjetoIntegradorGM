public class Adotante
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }

     public Adotante(string nome, int idade, string email)
    {
        Nome = nome;
        Idade = idade;
        Email = email;
    }
}