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
            Console.Write("Nome: ");
            string nome = Console.ReadLine()!;

            Console.Write("Espécie: ");
            string especie = Console.ReadLine()!;

            Console.Write("Raça: ");
            string raca = Console.ReadLine()!;

            Console.Write("Idade: ");
            int idade = int.Parse(Console.ReadLine()!);

            Console.Write("Sexo: ");
            string sexo = Console.ReadLine()!;

            Animal novoAnimal = new Animal(nome, especie, raca, idade, sexo);

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