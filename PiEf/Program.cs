
while (true)
    {
        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("1 - Listar clientes");
        Console.WriteLine("2 - Cadastrar cliente");
        Console.WriteLine("3 - Excluir cliente");
        Console.WriteLine("4 - Atualizar cliente");
        Console.WriteLine("5 - Cadastrar animal");
        Console.WriteLine("6 - Listar animais disponíveis");
        Console.WriteLine("0 - Sair");

        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                ListarAdotantes();
                break;
            case "2":
                CadastrarAdotante();
                break;
            case "3":
                ExcluirAdotante();
                break;
            case "4":
                AtualizarAdotante();
                break;
            case "5":
                CadastrarAnimal();
            case "0":
                return;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
        