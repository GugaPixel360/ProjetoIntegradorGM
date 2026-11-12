class Menu
{
     static void Main()
    {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1 - Cadastrar (Create)");
            Console.WriteLine("2 - Listar (Read)");
            Console.WriteLine("3 - Atualizar (Update)");
            Console.WriteLine("4 - Excluir (Delete)");
            Console.WriteLine("0 - Sair");

            int opcao = Console.Write("Escolha: ");

            switch (opcao)
        {
                case 1: Criar();
                case 2: Listar();
                case 3: Atualizar();
                case 4: Excluir();
        }
    }
}