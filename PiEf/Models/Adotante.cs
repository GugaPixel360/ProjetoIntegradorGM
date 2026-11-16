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

    public static class Crud
{
    public static List<Adotante> adotantes = new List<Adotante>();
    public static int proximoId = 1;

        public static void ListarAdotantes()
        {
            if (adotantes.Count == 0)
            {
                Console.WriteLine("Nenhum cliente cadastrado");
                return;
            }

            foreach (var a in adotantes)
            a.();
        }

        public  static void CadastrarAdotante()
        {
            Console.WriteLine("Digite o nome do cliente:");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite a idade do cliente:");
            int idade = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o email do cliente:");
            string email = Console.ReadLine();

            Adotante adotante = new Adotante(nome, idade, email);
            adotantes.Add(adotante);
            proximoId++;

            Console.WriteLine("Cliente cadastrado com sucesso!");
        }

        public static void ExcluirAdotante()
        {
            Console.WriteLine("Digite o ID do cliente que deseja excluir:");
            int id = int.Parse(Console.ReadLine());

            Adotante adotante = adotantes.FirstOrDefault(a => a.Id == id);
            if (adotante == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            adotantes.Remove(adotante);
            Console.WriteLine("Cliente excluído com sucesso!");
        }

        public static void AtualizarAdotante()
        {
            Console.WriteLine("Digite o ID do cliente que deseja atualizar:");
            int id = int.Parse(Console.ReadLine());

            Adotante adotante = adotantes.FirstOrDefault(a => a.Id == id);
            if (adotante == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            Console.WriteLine("Digite o novo nome do cliente:");
            adotante.Nome = Console.ReadLine();

            Console.WriteLine("Digite a nova idade do cliente:");
            adotante.Idade = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o novo email do cliente:");
            adotante.Email = Console.ReadLine();

            Console.WriteLine("Cliente atualizado com sucesso!");
        }

    }
}

