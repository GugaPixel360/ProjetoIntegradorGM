using PiEf.Models;

while (true)
{
    Console.WriteLine("\n===== SISTEMA DE ADOÇÃO =====");
    Console.WriteLine("1 - Listar clientes");
    Console.WriteLine("2 - Cadastrar cliente");
    Console.WriteLine("3 - Excluir cliente");
    Console.WriteLine("4 - Atualizar cliente");
    Console.WriteLine("5 - Cadastrar animal");
    Console.WriteLine("6 - Listar animais disponíveis");
    Console.WriteLine("0 - Sair");

    string opcao = Console.ReadLine() ?? "";

    switch (opcao)
    {
        case "1":
    List<Adotante> adotantes = await Adotante.ListarAdotantes();

    if (adotantes.Count == 0)
    {
        Console.WriteLine("Nenhum cliente cadastrado.");
    }
    else
    {
        Console.WriteLine("\n===== CLIENTES CADASTRADOS =====");

        foreach (var a in adotantes)
        {
            Console.WriteLine(
                $"ID: {a.Id} | Nome: {a.Nome} | " +
                $"Idade: {a.Idade} | Email: {a.Email}"
            );
        }
    }
    break;

    case "2":
        Console.Write("Nome: ");
        string nome = Console.ReadLine()!;

        Console.Write("Idade: ");
        int idade = int.Parse(Console.ReadLine()!);

        Console.Write("Email: ");
        string email = Console.ReadLine()!;

        Adotante novoAdotante = new Adotante(nome, idade, email);

        await Adotante.CadastrarAdotante(novoAdotante);
        Console.WriteLine("Cliente cadastrado com sucesso!");
        break;

    case "3":
        Console.Write("Digite o ID do cliente que deseja excluir: ");
        int idExcluir = int.Parse(Console.ReadLine()!);

        await Adotante.ExcluirAdotante(idExcluir);
        break;

    case "4":
        Console.Write("Digite o ID do cliente que deseja atualizar: ");
        int idAtualizar = int.Parse(Console.ReadLine()!);

        Console.Write("Novo nome: ");
        string novoNome = Console.ReadLine()!;

        Console.Write("Nova idade: ");
        int novaIdade = int.Parse(Console.ReadLine()!);

        Console.Write("Novo email: ");
        string novoEmail = Console.ReadLine()!;

        Adotante adotanteAtualizado = new Adotante(novoNome, novaIdade, novoEmail)
        {
            Id = idAtualizar
        };

        await Adotante.AtualizarAdotante(adotanteAtualizado);
        break;

        case "5":
            Console.Write("Nome: ");
            string nomeA = Console.ReadLine()!;

            Console.Write("Espécie: ");
            string especie = Console.ReadLine()!;

            Console.Write("Raça: ");
            string raca = Console.ReadLine()!;

            Console.Write("Idade: ");
            int idadeA = int.Parse(Console.ReadLine()!);

            Console.Write("Sexo: ");
            string sexo = Console.ReadLine()!;

            Animal novoAnimal = new Animal(nomeA, especie, raca, idadeA, sexo);

            await Animal.CadastrarAnimal(novoAnimal);

            Console.WriteLine("Animal cadastrado com sucesso!");
            break;

        case "6":
            List<Animal> animais = await Animal.ListarAnimaisDisponiveis();

            if (animais.Count == 0)
            {
                Console.WriteLine("Nenhum animal disponível.");
            }
            else
            {
                foreach (var animal in animais)
                {
                    Console.WriteLine(
                        $"ID: {animal.Id} | Nome: {animal.Nome} | " +
                        $"Espécie: {animal.Especie} | Raça: {animal.Raca}" +
                        $" | Idade: {animal.Idade} | Sexo: {animal.Sexo}" +
                        $" | Sexo: {animal.Sexo}"
                    );
                }
            }
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}