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
                    cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.PrecoProd;

                    cmd.ExecuteNonQuery();

                    conexao.Close();

                }
            }
        }

        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    string query = @"UPDATE Produto SET NomeProd = @nome, DescProd = @descricao, QuantProd = @quantidade, PrecoProd = @preco WHERE CodProd = @codigo";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                        cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.NomeProd;
                        cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.DescProd;
                        cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.QuantProd;
                        cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.PrecoProd;

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        return linhasAfetadas > 0;
                    }
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