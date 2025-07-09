using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class Cidade : ConexaoBD
    {
        public const string CamposTabela = "(id_uf, descricao, codigo_ibge)";

        public Modelo.Cidade ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Cidade
            {
                Id = leitor.GetInt32(0),
                Uf = new Modelo.Uf
                {
                    Id = leitor.GetInt32(1)
                },
                Descricao = leitor.GetString(2),
                CodigoIbge = leitor.GetInt32(3)
            };

            return modelo;
        }

        public void Inserir(Modelo.Cidade modelo)
        {
            var sql = $"INSERT INTO cidade {CamposTabela} VALUES (@id_uf, @descricao, @codigo_ibge);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id_uf", modelo.Uf.Id);
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("codigo_ibge", modelo.CodigoIbge);

                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM cidade WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.Cidade modelo)
        {
            var sql = @"
                UPDATE cidade 
                SET id_uf = @id_uf, descricao = @descricao, codigo_ibge = @codigo_ibge 
                WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id_uf", modelo.Uf.Id);
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("codigo_ibge", modelo.CodigoIbge);
                comando.Parameters.AddWithValue("id", modelo.Id);

                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.Cidade> BuscarTodos()
        {
            var modelos = new List<Modelo.Cidade>();
            var sql = "SELECT * FROM cidade;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            using (var leitor = comando.ExecuteReader())
            {
                while (leitor.Read())
                {
                    modelos.Add(ObterModelo(leitor));
                }
            }

            return modelos;
        }

        public Modelo.Cidade BuscarPorId(int id)
        {
            var sql = "SELECT * FROM cidade WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);

                using (var leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        return ObterModelo(leitor);
                    }
                }
            }

            return null;
        }
    }
}
