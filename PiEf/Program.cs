using PiEf.Models;


static async Task MenuInicial()
{
    while (true)
    {
        Console.WriteLine("\n===== SISTEMA DE ADOÇÃO =====");
        Console.WriteLine("1 - Entrar");
        Console.WriteLine("2 - Cadastrar cliente");
        Console.WriteLine("3 - Listar clientes");
        Console.WriteLine("0 - Sair");
        Console.Write("\nEscolha uma opção: ");

        string opcao = Console.ReadLine() ?? "";

        switch (opcao)
        {
            case "1":
            {
                Console.WriteLine("\n===== LOGIN =====");

                Console.Write("Email: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("Senha: ");
                string senha = Console.ReadLine() ?? "";

                Adotante? adotante = await Adotante.Login(email, senha);

                if (adotante == null)
                {
                    Console.WriteLine("\nEmail ou senha incorretos.");
                }
                else
                {
                    Console.WriteLine($"\nBem-vindo, {adotante.Nome}!");

                    await MenuPrincipal();
                }

                break;
            }

            case "2":
            {
                Console.WriteLine("\n===== CADASTRO DE CLIENTE =====");

                Console.Write("Nome: ");
                string nome = Console.ReadLine() ?? "";

                Console.Write("Idade: ");

                if (!int.TryParse(Console.ReadLine(), out int idade) || idade < 0)
                {
                    Console.WriteLine("Idade inválida.");
                    break;
                }

                Console.Write("Email: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("Senha: ");
                string senha = Console.ReadLine() ?? "";

                Adotante novoAdotante = new Adotante(nome, idade, email, senha);

                await Adotante.CadastrarAdotante(novoAdotante);

                Console.WriteLine("\nCliente cadastrado com sucesso!");

                break;
            }

            case "3":
            {
                List<Adotante> adotantes = await Adotante.ListarAdotantes();

                if (adotantes.Count == 0)
                {
                    Console.WriteLine("\nNenhum cliente cadastrado.");
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
            }

            case "0":
                Console.WriteLine("Encerrando o sistema...");
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
}


await MenuInicial();


static async Task MenuPrincipal()
{
    while (true)
    {
        Console.WriteLine("\n===== MENU PRINCIPAL =====");
        Console.WriteLine("1 - Cadastrar animal");
        Console.WriteLine("2 - Listar animais disponíveis");
        Console.WriteLine("3 - Atualizar animal");
        Console.WriteLine("4 - Excluir animal");
        Console.WriteLine("5 - Adotar animal");
        Console.WriteLine("6 - Listar adoções");
        Console.WriteLine("0 - Voltar ao menu inicial");

        Console.Write("\nEscolha uma opção: ");
        string opcao = Console.ReadLine() ?? "";

        switch (opcao)
        {
            case "1":
            {
                Console.WriteLine("\n===== CADASTRAR ANIMAL =====");

                Console.Write("Nome: ");
                string nome = Console.ReadLine() ?? "";

                Console.Write("Espécie: ");
                string especie = Console.ReadLine() ?? "";

                Console.Write("Raça: ");
                string raca = Console.ReadLine() ?? "";

                Console.Write("Idade: ");
                if (!int.TryParse(Console.ReadLine(), out int idade) || idade < 0)
                {
                    Console.WriteLine("Idade inválida.");
                    break;
                }

                Console.Write("Sexo: ");
                string sexo = Console.ReadLine() ?? "";

                Animal novoAnimal = new Animal(nome, especie, raca, idade, sexo);

                await Animal.CadastrarAnimal(novoAnimal);

                Console.WriteLine("\nAnimal cadastrado com sucesso!");
                break;
            }


            case "2":
                {
                    List<Animal> animais = await Animal.ListarAnimaisDisponiveis();

                    foreach (var animal in animais)
                    {
                        Console.WriteLine($"ID: {animal.Id} | Nome: {animal.Nome} | " +
                                          $"Espécie: {animal.Especie} | Raça: {animal.Raca} | " +
                                          $"Idade: {animal.Idade} | Sexo: {animal.Sexo} | " +
                                          $"Disponível: {(animal.Disponivel ? "Sim" : "Não")}");
                    }
                break;
                }
                

            
            case "3":
                {
                    Console.WriteLine("\n===== ATUALIZAR ANIMAL =====");

                    Console.Write("Digite o ID do animal: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("ID inválido.");
                        break;
                    }

                    Console.Write("Novo nome: ");
                    string nome = Console.ReadLine() ?? "";

                    Console.Write("Nova espécie: ");
                    string especie = Console.ReadLine() ?? "";

                    Console.Write("Nova raça: ");
                    string raca = Console.ReadLine() ?? "";

                    Console.Write("Nova idade: ");
                    if (!int.TryParse(Console.ReadLine(), out int idade) || idade < 0)
                    {
                        Console.WriteLine("Idade inválida.");
                        break;
                    }

                    Console.Write("Novo sexo: ");
                    string sexo = Console.ReadLine() ?? "";

                    Animal animalAtualizado = new Animal(nome, especie, raca, idade, sexo)
                    {
                        Id = id
                    };

                    await Animal.AtualizarAnimal(animalAtualizado);

                    break;
                }


            
            case "4":
            {
                Console.WriteLine("\n===== EXCLUIR ANIMAL =====");

                Console.Write("Digite o ID do animal que deseja excluir: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido.");
                    break;
                }

                await Animal.ExcluirAnimal(id);

                break;
            }


            case "5":
                Console.WriteLine("\nCadastro de adoção ainda não implementado.");
                break;

            case "6":
                Console.WriteLine("\nListagem de adoções ainda não implementada.");
                break;

            case "0":
                return;

            default:
                Console.WriteLine("\nOpção inválida.");
                break;
        }
    }
}
