using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class Uf : ConexaoBD
    {
        public const string CamposTabela = "(id_pais, descricao, codigo_ibge)";

        public Modelo.Uf ObterModelo(NpgsqlDataReader leitor)
        {
            return new Modelo.Uf
            {
                Id = leitor.GetInt32(0),
                Pais = new Modelo.Pais
                {
                    Id = leitor.GetInt32(1)
                },
                Descricao = leitor.GetString(2),
                CodigoIbge = leitor.GetInt32(3)
            };
        }

        public void Inserir(Modelo.Uf modelo)
        {
            var sql = $"INSERT INTO uf {CamposTabela} VALUES (@id_pais, @descricao, @codigo_ibge);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id_pais", modelo.Pais.Id);
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("codigo_ibge", modelo.CodigoIbge);
                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM uf WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.Uf modelo)
        {
            var sql = @"
                UPDATE uf 
                SET id_pais = @id_pais, descricao = @descricao, codigo_ibge = @codigo_ibge 
                WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id_pais", modelo.Pais.Id);
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("codigo_ibge", modelo.CodigoIbge);
                comando.Parameters.AddWithValue("id", modelo.Id);
                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.Uf> BuscarTodos()
        {
            var modelos = new List<Modelo.Uf>();
            var sql = "SELECT * FROM uf;";

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

        public Modelo.Uf BuscarPorId(int id)
        {
            var sql = "SELECT * FROM uf WHERE id = @id;";

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
