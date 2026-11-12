public class Adotante
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }

     public Cliente(int id, string nome, int idade, string email)
    {
        Id = id;
        Nome = nome;
        Idade = Idade;
        Email = email;
    }

    static void Cadastrar()
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        Console.Write("Idade: ");
        string idade = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
    }

    Adotante a = new Adotante(id, nome, idade, email);
}