using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;


namespace ProjetoEcommerce.Repositorio
{
    // Define a classe responsável por interagir com os dados de clientes no banco de dados
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        // Declara uma variável privada somente leitura para armazenar a string de conexão com o MySQL
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");


        // Método para cadastrar um novo cliente no banco de dados
        public void Cadastrar(Produto produto)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into Produto (NomeProd, DescProd, QuantProd, PrecoProd) values (@nome, @descricao, @quantidade, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.NomeProd;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.DescProd;
                cmd.Parameters.Add("@quantidade", MySqlDbType.VarChar).Value = produto.QuantProd;
                cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.PrecoProd;
                cmd.ExecuteNonQuery();
                conexao.Close();
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
                    MySqlCommand cmd = new MySqlCommand("Update Produto set NomeProd=@nome, DescProd=@descricao, QuantProd=@quantidade, PrecoProd=@preco " + " where CodCli=@codigo ", conexao);
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.NomeProd;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.DescProd;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.VarChar).Value = produto.QuantProd;
                    cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.PrecoProd;
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
                MySqlCommand cmd = new MySqlCommand("SELECT * from Produto", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Produtolist.Add(
                                new Produto
                                {
                                    CodProd = Convert.ToInt32(dr["CodProd"]), // Converte o valor da coluna "codigo" para inteiro
                                    NomeProd = ((string)dr["NomeProd"]), // Converte o valor da coluna "nome" para string
                                    DescProd = ((string)dr["DescProd"]), // Converte o valor da coluna "telefone" para string
                                    QuantProd = ((string)dr["QuantProd"]),
                                    PrecoProd = ((string)dr["PrecoProd"]),// Converte o valor da coluna "email" para string
                                });
                }
                return Produtolist;
            }
        }

        public Cliente ObterProduto(int Codigo)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar um registro da tabela 'cliente' com base no código
                MySqlCommand cmd = new MySqlCommand("SELECT * from Produto where CodProd=@codigo ", conexao);

                // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Declara um leitor de dados do MySQL
                MySqlDataReader dr;
                // Cria um novo objeto Cliente para armazenar os resultados
                Produto produto = new Produto();

                /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // Lê os resultados linha por linha
                while (dr.Read())
                {
                    // Preenche as propriedades do objeto Cliente com os valores da linha atual
                    produto.CodProd = Convert.ToInt32(dr["CodProd"]);//propriedade Codigo e convertendo para int
                    produto.NomeProd = (string)(dr["NomeCli"]); // propriedade Nome e passando string
                    produto.DescProd = (string)(dr["DescProd"]); //propriedade telefone e passando string
                    produto.QuantProd = (string)(dr["QuantProd"]); //propriedade email e passando string
                    produto.PrecoProd = (string)(dr["PrecoProd"]); //propriedade email e passando string

                }
                // Retorna o objeto Cliente encontrado (ou um objeto com valores padrão se não encontrado)
                return produto;
            }
        }


        // Método para excluir um cliente do banco de dados pelo seu código (ID)
        public void Excluir(int CodProd)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();

                // Cria um novo comando SQL para deletar um registro da tabela 'cliente' com base no código
                MySqlCommand cmd = new MySqlCommand("delete from produtos where CodProd=@codigo", conexao);

                // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", CodProd);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close(); // Fecha  a conexão com o banco de dados
            }
        }
    }
}
