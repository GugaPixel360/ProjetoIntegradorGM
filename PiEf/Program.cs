using PiEf.Models;
Console.Clear();


static async Task MenuInicial()
{
    while (true)
    {
        Console.WriteLine("\n===== SISTEMA DE ADOÇÃO =====");
        Console.WriteLine("1 - Entrar");
        Console.WriteLine("2 - Cadastrar cliente");
        Console.WriteLine("0 - Sair");
        Console.Write("Escolha uma opção: ");

        string opcao = Console.ReadLine()?.Trim() ?? "";

        switch (opcao)
        {
            case "1":
            {
                Console.WriteLine("\n===== LOGIN =====");

                Console.Write("Email: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("Senha: ");
                string senha = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(senha.Trim()))
                {
                    Console.WriteLine("Senha inválida.");
                    break;
                }

                Adotante? adotante = await Adotante.Login(email, senha);

                if (adotante == null)
                {
                    Console.WriteLine("\nEmail ou senha incorretos.");
                }
                else
                {
                    Console.WriteLine($"\nBem-vindo, {adotante.Nome}!");

                    if (adotante.TipoUsuario == "Admin")
                    {
                        await MenuAdmin();
                    }
                    else
                    {
                        await MenuCliente(adotante);
                    }
                }

                break;
            }

            case "2":
            {
                Console.WriteLine("\n===== CADASTRO DE CLIENTE =====");

                Console.Write("Nome: ");
                string nome = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(nome.Trim()))
                {
                    Console.WriteLine("Nome inválido.");
                    break;
                }

                Console.Write("Idade: ");

                if (!int.TryParse(Console.ReadLine()?.Replace(" ", ""), out int idade) || idade < 0)
                {
                    Console.WriteLine("Idade inválida.");
                    break;
                }
                

                Console.Write("Email: ");
                string email = Console.ReadLine()?.Replace(" ", "") ?? "";
                if (!await Adotante.ValidarEmail(email))
                {
                    Console.WriteLine("Email inválido.");
                    break;
                }

                Console.Write("Senha: ");
                string senha = Console.ReadLine()?.Replace(" ", "") ?? "";
                if (!await Adotante.ValidarSenha(senha))
                {
                    Console.WriteLine("Senha inválida.");
                    Console.WriteLine("A senha deve ter pelo menos 6 caracteres, incluindo letras, números e caracteres especiais.");
                    break;
                }

                Adotante novoAdotante = new Adotante(nome, idade, email, senha);

                await Adotante.CadastrarAdotante(novoAdotante);

                Console.WriteLine("\nCliente cadastrado com sucesso!");

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


static async Task MenuAdmin()
{
    while (true)
    {
        Console.WriteLine("\n===== MENU ADMINISTRADOR =====");
        Console.WriteLine("1 - Cadastrar animal");
        Console.WriteLine("2 - Listar animais disponíveis");
        Console.WriteLine("3 - Atualizar animal");
        Console.WriteLine("4 - Excluir animal");
        Console.WriteLine("5 - Adotar animal");
        Console.WriteLine("6 - Listar adoções");
        Console.WriteLine("7 - Atualizar clientes");
        Console.WriteLine("8 - Excluir cliente");
        Console.WriteLine("9 - Listar clientes");
        Console.WriteLine("0 - Voltar ao menu inicial");

        Console.Write("Escolha uma opção: ");
        string opcao = Console.ReadLine() ?? "";

        switch (opcao)
        {
            case "1":
            {
                Console.WriteLine("\n===== CADASTRAR ANIMAL =====");

                Console.Write("Nome: ");
                string nomeC = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(nomeC.Trim()))
                {
                    Console.WriteLine("Nome inválido.");
                    break;
                }

                Console.Write("Espécie: ");
                string especie = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(especie.Trim()))
                {
                    Console.WriteLine("Espécie inválida.");
                    break;
                }

                Console.Write("Raça: ");
                string raca = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(raca.Trim()))
                {
                    Console.WriteLine("Raça inválida.");
                    break;
                }

                Console.Write("Idade: ");
                if (!int.TryParse(Console.ReadLine(), out int idadeC) || idadeC < 0)
                {
                    Console.WriteLine("Idade inválida.");
                    break;
                }

                Console.Write("Sexo: ");
                string sexo = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(sexo.Trim()))
                {
                    Console.WriteLine("Sexo inválido.");
                    break;
                }

                Animal novoAnimal = new Animal(nomeC, especie, raca, idadeC, sexo);

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
                                          $"Idade: {animal.Idade} | Sexo: {animal.Sexo} | ");
                    }
                break;
                }
                

            
            case "3":
                {
                    Console.WriteLine("\n===== ATUALIZAR ANIMAL =====");

                    Console.Write("Digite o ID do animal: ");
                    if (!int.TryParse(Console.ReadLine(), out int idN))
                    {
                        Console.WriteLine("ID inválido.");
                        break;
                    }

                    Console.Write("Novo nome: ");
                    string nomeN = Console.ReadLine() ?? "";

                    Console.Write("Nova espécie: ");
                    string especie = Console.ReadLine() ?? "";

                    Console.Write("Nova raça: ");
                    string raca = Console.ReadLine() ?? "";

                    Console.Write("Nova idade: ");
                    if (!int.TryParse(Console.ReadLine(), out int idadeN) || idadeN < 0)
                    {
                        Console.WriteLine("Idade inválida.");
                        break;
                    }

                    Console.Write("Novo sexo: ");
                    string sexo = Console.ReadLine() ?? "";

                    Animal animalAtualizado = new Animal(nomeN, especie, raca, idadeN, sexo)
                    {
                        Id = idN
                    };

                    await Animal.AtualizarAnimal(animalAtualizado);

                    break;
                }


            
            case "4":
            {
                Console.WriteLine("\n===== EXCLUIR ANIMAL =====");

                Console.Write("Digite o ID do animal que deseja excluir: ");
                await Animal.ListarAnimaisDisponiveis();

                if (!int.TryParse(Console.ReadLine(), out int idE))
                {
                    Console.WriteLine("ID inválido.");
                    break;
                }

                await Animal.ExcluirAnimal(idE);

                break;
            }


            case "5":
            {
                Console.WriteLine("\n===== CADASTRAR ADOÇÃO =====");

                Console.Write("ID do cliente: ");

                if (!int.TryParse(Console.ReadLine(), out int idAdotante))
                {
                    Console.WriteLine("ID do cliente inválido.");
                    break;
                }

                Console.Write("ID do animal: ");

                if (!int.TryParse(Console.ReadLine(), out int idAnimal))
                {
                    Console.WriteLine("ID do animal inválido.");
                    break;
                }

                await Adocao.CadastrarAdocao(idAdotante, idAnimal);

                break;
            }
            
            case "6":
            {
                List<Adocao> adocoes = await Adocao.ListarAdocoes();

                if (adocoes.Count == 0)
                {
                    Console.WriteLine("\nNenhuma adoção cadastrada.");
                }
                else
                {
                    Console.WriteLine("\n===== ADOÇÕES CADASTRADAS =====");

                    foreach (var adocao in adocoes)
                    {
                        Console.WriteLine(
                            $"ID da adoção: {adocao.Id} | " +
                            $"ID do cliente: {adocao.IdAdotante} | " +
                            $"ID do animal: {adocao.IdAnimal} | " +
                            $"Data: {adocao.DataAdocao:dd/MM/yyyy HH:mm}"
                        );
                    }
                }

                break;
            }

            case "7":
            Console.Write("Digite o ID do cliente que deseja atualizar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido!");
                break;
            }

            Console.Write("Novo nome: ");
            string nome = Console.ReadLine() ?? "";

            Console.Write("Nova idade: ");
            if (!int.TryParse(Console.ReadLine(), out int idade) || idade < 18)
            {
                Console.WriteLine("Idade inválida!");
                break;
            }

            Console.Write("Novo e-mail: ");
            string email = Console.ReadLine() ?? "";
            if (!await Adotante.ValidarEmail(email))
            {
                Console.WriteLine("E-mail inválido!");
                break;
            }

            Console.Write("Nova senha: ");
            string senha = Console.ReadLine() ?? "";
            if (!await Adotante.ValidarSenha(senha))
            {
                Console.WriteLine("Senha inválida!");
                break;
            }

            Adotante adotanteAtualizado = new Adotante
            {
                Id = id,
                Nome = nome,
                Idade = idade,
                Email = email,
                Senha = senha
            };

            await Adotante.AtualizarAdotante(adotanteAtualizado);
            break;

            case "8":
                List<Adotante> adotantes = await Adotante.ListarAdotantes();

                if (adotantes.Count == 0)
                {
                    Console.WriteLine("\nNenhum cliente cadastrado.");
                    return;
                }
                else
                {
                    Console.WriteLine("\n===== CLIENTES CADASTRADOS =====");

                    foreach (var adotante in adotantes)
                    {
                        Console.WriteLine(
                            $"ID: {adotante.Id} | " +
                            $"Nome: {adotante.Nome} | " +
                            $"Idade: {adotante.Idade} | " +
                            $"E-mail: {adotante.Email}"
                        );
                    }
                }
                Console.Write("Digite o ID do cliente que deseja excluir: ");

                if (!int.TryParse(Console.ReadLine(), out int idExcluir))
                {
                    Console.WriteLine("ID inválido!");
                    break;
                }

                await Adotante.ExcluirAdotante(idExcluir);
                break;
            
            case "9":
                List<Adotante> adotantesL = await Adotante.ListarAdotantes();

                if (adotantesL.Count == 0)
                {
                    Console.WriteLine("\nNenhum cliente cadastrado.");
                }
                else
                {
                    Console.WriteLine("\n===== CLIENTES CADASTRADOS =====");

                    foreach (var adotante in adotantesL)
                    {
                        Console.WriteLine(
                            $"ID: {adotante.Id} | " +
                            $"Nome: {adotante.Nome} | " +
                            $"Idade: {adotante.Idade} | " +
                            $"E-mail: {adotante.Email}"
                        );
                    }
                }

                break;

            case "0":
                Console.WriteLine("Voltando ao menu inicial...");
                return;

        }
    }
}
static async Task MenuCliente(Adotante adotante)
{
    while (true)
    {
        Console.WriteLine("\n===== MENU DO CLIENTE =====");
        Console.WriteLine("1 - Listar animais disponíveis");
        Console.WriteLine("2 - Adotar animal");
        Console.WriteLine("3 - Atualizar meus dados");
        Console.WriteLine("4 - Excluir minha conta");
        Console.WriteLine("0 - Sair da conta");

        Console.Write("Escolha uma opção: ");
        string opcao = Console.ReadLine() ?? "";

        switch (opcao)
        {
            case "1":
            {
                List<Animal> animais =
                    await Animal.ListarAnimaisDisponiveis();

                if (animais.Count == 0)
                {
                    Console.WriteLine("Nenhum animal disponível.");
                    break;
                }

                foreach (var animal in animais)
                {
                    Console.WriteLine(
                        $"ID: {animal.Id} | Nome: {animal.Nome} | " +
                        $"Espécie: {animal.Especie} | Raça: {animal.Raca}"
                    );
                }

                break;
            }

            case "2":
            {
                Console.Write("Digite o ID do animal que deseja adotar: ");

                if (!int.TryParse(Console.ReadLine(), out int idAnimal))
                {
                    Console.WriteLine("ID inválido.");
                    break;
                }

                await Adocao.CadastrarAdocao(adotante.Id, idAnimal);
                break;
            }
            case "3":
            {
                Console.WriteLine("\n===== ATUALIZAR MEUS DADOS =====");

                Console.Write("Novo nome: ");
                string nome = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Nome inválido.");
                    break;
                }

                Console.Write("Nova idade: ");

                if (!int.TryParse(Console.ReadLine(), out int idade) || idade < 18)
                {
                    Console.WriteLine("Idade inválida.");
                    break;
                }

                Console.Write("Novo email: ");
                string email = Console.ReadLine()?.Trim() ?? "";

                if (!await Adotante.ValidarEmail(email))
                {
                    Console.WriteLine("Email inválido.");
                    break;
                }

                Console.Write("Nova senha: ");
                string senha = Console.ReadLine() ?? "";

                if (!await Adotante.ValidarSenha(senha))
                {
                    Console.WriteLine("Senha inválida.");
                    break;
                }

                Adotante adotanteAtualizado = new Adotante
                {
                    Id = adotante.Id,
                    Nome = nome,
                    Idade = idade,
                    Email = email,
                    Senha = senha,
                    TipoUsuario = adotante.TipoUsuario
                };

                await Adotante.AtualizarAdotante(adotanteAtualizado);

                Console.WriteLine("Dados atualizados com sucesso!");

                adotante.Nome = nome;
                adotante.Idade = idade;
                adotante.Email = email;
                adotante.Senha = senha;

                break;
            }


            case "4":
            {
                Console.WriteLine("\n===== EXCLUIR MINHA CONTA =====");
                Console.Write("Tem certeza que deseja excluir sua conta? (s/n): ");

                string confirmacao = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (confirmacao == "s")
                {
                    await Adotante.ExcluirAdotante(adotante.Id);

                    Console.WriteLine("Conta excluída!");
                    return;
                }
                if (confirmacao == "n")
                {
                    Console.WriteLine("Exclusão cancelada.");
                }
                else
                {
                    Console.WriteLine("Opção inválida.");
                    return;
                }

                break;
            }


            case "0":
                Console.WriteLine("Saindo da conta...");
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
}   

