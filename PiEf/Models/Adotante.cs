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

// mudar nome dps
class Crud
{
    static List<Adotante> adotantes = new List<Adotante>();
    static int proximoId = 1;
    static void ListarAdotantes()
        {
            if (adotantes.Count == 0)
            {
                Console.WriteLine("Nenhum cliente cadastrado");
                return;
            }

            foreach (var a in adotantes)
            a.();
        }
}



