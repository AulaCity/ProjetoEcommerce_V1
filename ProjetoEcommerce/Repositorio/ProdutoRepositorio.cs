using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public void Cadastrar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                string query = @"INSERT INTO Produto (NomeProd, DescProd, QuantProd, PrecoProd) 
                                 VALUES (@nome, @descricao, @quantidade, @preco)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.NomeProd;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.DescProd;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.QuantProd;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.PrecoProd;

                    cmd.ExecuteNonQuery();

                    conexao.Close();

                }
            }
        }

        public bool Atualizar(Produto produto)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'cliente' com base no código
                    MySqlCommand cmd = new MySqlCommand("UPDATE Produto SET NomeProd=@nome,DescProd=@descricao,QuantProd=@quantidade,PrecoProd=@preco WHERE CodProd=@codigo", conexao);

                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.NomeProd;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.DescProd;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.QuantProd;
                    cmd.Parameters.Add("@preco", MySqlDbType.de).Value = produto.PrecoProd;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada

                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Produto> TodosProdutos()
        {
            List<Produto> Produtolist = new List<Produto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = "SELECT * FROM Produto";
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        Produtolist.Add(new Produto
                        {
                            CodProd = Convert.ToInt32(dr["CodProd"]),
                            NomeProd = dr["NomeProd"].ToString(),
                            DescProd = dr["DescProd"].ToString(),
                            QuantProd = Convert.ToInt32(dr["QuantProd"]),
                            PrecoProd = dr["PrecoProd"].ToString()
                        });
                    }
                }
            }

            return Produtolist;
        }

        public Produto ObterProduto(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = "SELECT * FROM Produto WHERE CodProd = @codigo";
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@codigo", Codigo);

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dr.Read())
                        {
                            return new Produto
                            {
                                CodProd = Convert.ToInt32(dr["CodProd"]),
                                NomeProd = dr["NomeProd"].ToString(),
                                DescProd = dr["DescProd"].ToString(),
                                QuantProd = Convert.ToInt32(dr["QuantProd"]),
                                PrecoProd = dr["PrecoProd"].ToString()
                            };
                        }
                    }
                }
            }

            return null; // Retorna null se não encontrar
        }

        public void Excluir(int CodProd)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = "DELETE FROM Produto WHERE CodProd = @codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@codigo", CodProd);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}