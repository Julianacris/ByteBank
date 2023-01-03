using System;
namespace ByteBank1
{   
    class DadosConta 
    {
        public readonly string name; 
        public readonly string senha; 
        public readonly string cpf; 
        private double saldo;   
        public DadosConta(string name, string senha, string cpf) 
        {
            this.name = name;
            this.senha = senha;
            this.cpf = cpf;
            this.saldo = 0;
        }
        public void setSaldo(double saldo) 
        {
            this.saldo = saldo;
        }
        public double getSaldo() 
        {
            return this.saldo;
        }
    }
    public class program 
    {
        static void ShowMenu()
        {
            Console.WriteLine("1 - Inserir novo usuário");
            Console.WriteLine("2 - Deletar novo usuário");
            Console.WriteLine("3 - Listar todas as contas registradas");
            Console.WriteLine("4 - Detalhes de um usuário");
            Console.WriteLine("5 - Quantia armazenada no banco");
            Console.WriteLine("6 - Manipular a conta");
            Console.WriteLine("0 - Para sair do programa");
            Console.WriteLine("Digite a opção desejada"); 
        } 
        static void RegistarNovoUsuario(List<DadosConta>contas)
        {
            Console.WriteLine("Digite o cpf: ");
            string cpf = Console.ReadLine();

            //var index = contas.FindIndex(conta => conta.cpf == cpf);
            var index = EncontraIndexContaPorCPF(cpf, contas);
            if (index != -1) 
            {
                Console.WriteLine("Usuário ja Cadastrado, não é possivel abrir uma nova conta.");
                return;
            }
            Console.WriteLine("Digite o nome: ");
            string nome = Console.ReadLine();
            Console.WriteLine("Digite a senha: ");
            string senha = Console.ReadLine();
            DadosConta cliente = new DadosConta(nome, senha, cpf);
            contas.Add(cliente);
        }
        static void DeletarUsuario(List<DadosConta> contas)
        {
            Console.WriteLine("Digite o CPF: ");
            string cpfParaDeletar = Console.ReadLine();
            int IndexParaDeletar = EncontraIndexContaPorCPF(cpfParaDeletar, contas);

            if (IndexParaDeletar == -1)
            {
                Console.WriteLine("Não foi possível deletar esta conta");
                Console.WriteLine("Motivo: Conta não encontrada.");
                return;
            }

            contas.RemoveAt(IndexParaDeletar);

            Console.WriteLine("Conta deletada com sucesso.");
        }
        static void ListarTodasAsContas(List<DadosConta> contas)
        {   
            if (contas.Count == 0)
            {
                Console.WriteLine("Não há contas registradas");
                return;
            }
            for(int i = 0; i < contas.Count; i++)
            {
                ApresentaConta(contas[i]);
            }
        }
        static void ApresentarUsuario(List<DadosConta> contas)
        {
            Console.WriteLine("Digite o CPF: ");
            string cpfParaApresentar = Console.ReadLine();
            int IndexParaApresentar = EncontraIndexContaPorCPF(cpfParaApresentar, contas);

            if (IndexParaApresentar == -1)
            {
                Console.WriteLine("Não foi possível apresentar esta conta");
                Console.WriteLine("Motivo: Conta não encontrada.");
            }
            else
            {
                ApresentaConta(contas[IndexParaApresentar]);
            }
        }
        static void ApresentaConta(DadosConta conta)
        {
            Console.WriteLine($"CPF = {conta.cpf} | Titular = {conta.name} | Saldo = R$ {conta.getSaldo():f2}");
        }
        static double ApresentarSaldo(List<DadosConta> contas)
        {
            double totalSaldos = 0;
            for (int i = 0; i < contas.Count; i++)
            {
                totalSaldos += contas[i].getSaldo();
            }
            Console.WriteLine($"Total acumulado no banco: {totalSaldos:f2}");
            return totalSaldos;
        }
        static void ManipularConta(List<DadosConta>contas)
        {
            Console.WriteLine("Digite o cpf do titular da conta ");
            string cpfInformado = Console.ReadLine();

            var index = EncontraIndexContaPorCPF(cpfInformado, contas);
            if ( index == -1 )
            {
                Console.WriteLine("Titular Não encontrado");
            }
            else
            {
                Console.WriteLine("Informe a senha para acessar a conta:");
                string senha = Console.ReadLine();
                
                if (contas[index].senha == senha )
                {
                    string operacao = "";
                    do
                    {
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine($"Seu saldo atual é de: {contas[index].getSaldo():f2}");
                        Console.WriteLine("1 - Se você deseja efetuar um saque");
                        Console.WriteLine("2 - Se você deseja efetuar um depósito");
                        Console.WriteLine("3 - Se você deseja efetuar uma transferência");
                        Console.WriteLine("0 - Para voltar ao menu anterior");
                        operacao = Console.ReadLine();
                        switch (operacao)
                        {
                            case "1":
                                Saque(contas[index]);
                                break;
                            case "2":
                                Deposito(contas[index]);
                                break;
                            case"3":
                                Transferencia(contas[index], contas);
                                break;
                        }
                    } while (operacao != "0");
                }
                else
                {
                    Console.WriteLine("Senha incorreta");
                }
            }
        }
        static void Deposito( DadosConta Cliente )
        {
            Console.WriteLine("Quanto deseja depositar?");
            double valorDeposito = double.Parse(Console.ReadLine());
            Cliente.setSaldo(valorDeposito+Cliente.getSaldo());
        }
        static void Saque(DadosConta Cliente)
        {
            Console.WriteLine($"Seu saldo atual é de: {Cliente.getSaldo():f2}");
            if (Cliente.getSaldo() == 0)
            {
                Console.WriteLine("Seu saldo atual não permite saques");
            }
            else
            {
                Console.WriteLine("Quanto deseja sacar?");
                double valorSaque = double.Parse(Console.ReadLine());

                double novoSaldo = Cliente.getSaldo() - valorSaque;
                if (novoSaldo < 0) 
                {
                    Console.WriteLine("Saldo insuficiente para efetuar este saque");
                }
                else
                {
                    Cliente.setSaldo(novoSaldo);
                    Console.WriteLine($"Seu saldo atual é de: {Cliente.getSaldo():f2}");
                }
            }
        }

        static void Transferencia(DadosConta Cliente, List<DadosConta> contas)
        {
            if (Cliente.getSaldo() == 0)
            {
                Console.WriteLine("Saldo insuficiente para efetuar transação");
            }
            else
            {
                Console.WriteLine("Informe o CPF do destinatário:");
                string CpfDestinatario = Console.ReadLine();
                var index = EncontraIndexContaPorCPF(CpfDestinatario, contas);
                if (index == -1)
                {
                    Console.WriteLine("Destinatário não encontrado");
                    return;
                }
                else
                {
                    var Destinatario = contas[index];
                    Console.WriteLine("Você deseja transferir para:");
                    Console.WriteLine($"{Destinatario.name}?");
                    string ConfirmaDestinatario = "";
                    do
                    {
                        Console.WriteLine("Digite S para Sim ou N para Não");
                        ConfirmaDestinatario = Console.ReadLine();
                    }
                    while (ConfirmaDestinatario != "S" && ConfirmaDestinatario != "N");
                    
                    if ( ConfirmaDestinatario == "S")
                    {
                        Console.WriteLine("Qual valor você deseja transferir?");
                        double valorParaTransferir = double.Parse( Console.ReadLine());
                        if (valorParaTransferir > Cliente.getSaldo())
                        {
                            Console.WriteLine("Saldo insuficiente para efetuar transação");
                        }
                        else
                        {
                            Console.WriteLine("Digite sua senha:");
                            string senha = Console.ReadLine();
                            if (senha == Cliente.senha) 
                            {
                                double saldoAtual = Cliente.getSaldo();
                                Cliente.setSaldo(saldoAtual - valorParaTransferir);

                                double saldoDestinatario = Destinatario.getSaldo();
                                Destinatario.setSaldo(saldoDestinatario + valorParaTransferir);
                            }
                            else
                            {
                                Console.WriteLine("Senha incorreta");
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }

            }
        }

        static int EncontraIndexContaPorCPF( string CPF, List<DadosConta> contas)
        {
            return contas.FindIndex(conta => conta.cpf == CPF);
        }
        

        public static void Main(string[] args)
        {
            Console.WriteLine("Bem vindo ao ByteBank");

            List<DadosConta> contas = new List<DadosConta>();

            int option;
            do
            {
                ShowMenu();
                option = int.Parse(Console.ReadLine());
               
                Console.WriteLine("------------------------------------");
               
               switch (option)
               {
                    case 0:
                    Console.WriteLine("Encerrando o programa...");
                        break;
                       
                    case 1: 
                        RegistarNovoUsuario(contas);
                        break;
                    case 2:
                        DeletarUsuario(contas);
                        break;
                    case 3:
                        ListarTodasAsContas(contas);
                        break;
                    case 4:
                        ApresentarUsuario(contas);
                        break;
                    case 5:
                        ApresentarSaldo(contas); 
                        break;
                    case 6:
                        ManipularConta(contas);
                        break;

               }
                Console.WriteLine("-----------------------------------");

            }while(option != 0);
        }
    }
}