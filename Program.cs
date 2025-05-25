using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Trabalho_do_Marcelo
{
    public class Funcionario
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=password_1234;Database=trabalho_marcelo";

        // CREATE - Cadastrar Funcionário
        public void CadastrarFuncionario(string nome, string cargo, decimal salario, DateTime dt_nascimento)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                conexao.Open();
                string sql = "INSERT INTO funcionario (nome, cargo, salario, dt_nascimento) VALUES (@nome, @cargo, @salario, @dt_nascimento)";
                using (var cmd = new NpgsqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("nome", nome);
                    cmd.Parameters.AddWithValue("cargo", cargo);
                    cmd.Parameters.AddWithValue("salario", salario);
                    cmd.Parameters.AddWithValue("dt_nascimento", dt_nascimento);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Funcionário cadastrado com sucesso!");
                }
            }
        }

        // READ - Buscar Funcionário
        public void BuscarFuncionarioPorId(int id)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                conexao.Open();
                string sql = "SELECT * FROM funcionario WHERE id = @id";
                using (var cmd = new NpgsqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["id"]}");
                            Console.WriteLine($"Nome: {reader["nome"]}");
                            Console.WriteLine($"Cargo: {reader["cargo"]}");
                            Console.WriteLine($"Salário: {reader["salario"]}");
                            Console.WriteLine($"Data de Nascimento: {reader["dt_nascimento"]:yyyy-MM-dd}");
                        }
                        else
                        {
                            Console.WriteLine("Funcionário não encontrado.");
                        }
                    }
                }
            }
        }

        // DELETE - Excluir Funcionário
        public void DeletarFuncionario(int id)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // Buscar informações antes de excluir
                string selectSql = "SELECT * FROM funcionario WHERE id = @id";
                using (var selectCmd = new NpgsqlCommand(selectSql, conexao))
                {
                    selectCmd.Parameters.AddWithValue("id", id);

                    using (var reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Funcionário que será excluído:");
                            Console.WriteLine($"ID: {reader["id"]}");
                            Console.WriteLine($"Nome: {reader["nome"]}");
                            Console.WriteLine($"Cargo: {reader["cargo"]}");
                            Console.WriteLine($"Salário: {reader["salario"]}");
                            Console.WriteLine($"Data de Nascimento: {reader["dt_nascimento"]:yyyy-MM-dd}");

                            // 👉 Fecha o reader ANTES de executar o DELETE
                            reader.Close();
                        }
                        else
                        {
                            Console.WriteLine("Funcionário não encontrado. Não é possível excluir.");
                            return;
                        }
                    }
                }

                // Depois de mostrar, faz o DELETE
                string deleteSql = "DELETE FROM funcionario WHERE id = @id";
                using (var deleteCmd = new NpgsqlCommand(deleteSql, conexao))
                {
                    deleteCmd.Parameters.AddWithValue("id", id);
                    deleteCmd.ExecuteNonQuery();
                    Console.WriteLine("Funcionário deletado com sucesso!");
                    Console.WriteLine("\nPressione qualquer tecla para encerrar...");
                    Console.ReadKey();
                }
            }
        }

    }

        internal class Program
    {
        static void Main(string[] args)
        {
            Funcionario dao = new Funcionario();

            // CREATE
            Console.WriteLine("Cadastrar Funcionário");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Cargo: ");
            string cargo = Console.ReadLine();

            Console.Write("Salário: ");
            decimal salario = decimal.Parse(Console.ReadLine());

            Console.Write("Data de nascimento (aaaa-MM-dd): ");
            DateTime dtNascimento = DateTime.Parse(Console.ReadLine());

            dao.CadastrarFuncionario(nome, cargo, salario, dtNascimento);
            Console.WriteLine("Funcionário cadastrado com sucesso!");

            // READ
            Console.WriteLine("\nBuscar Funcionário por ID");
            Console.Write("ID: ");
            int idBuscar = int.Parse(Console.ReadLine());
            dao.BuscarFuncionarioPorId(idBuscar);

            // DELETE
            Console.WriteLine("\nExcluir Funcionário por ID");
            Console.Write("ID: ");
            int idDeletar = int.Parse(Console.ReadLine());
            dao.DeletarFuncionario(idDeletar);
        }
    }
}
