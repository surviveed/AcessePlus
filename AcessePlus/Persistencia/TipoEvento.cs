using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class TipoEvento : ConexaoBD
    {
        public const string CamposTabela = "(descricao)";

        public Modelo.TipoEvento ObterModelo(NpgsqlDataReader leitor)
        {
            return new Modelo.TipoEvento
            {
                Id = leitor.GetInt32(0),
                Descricao = leitor.GetString(1)
            };
        }

        public void Inserir(Modelo.TipoEvento modelo)
        {
            var sql = $"INSERT INTO tipo_evento {CamposTabela} VALUES (@descricao);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM tipo_evento WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.TipoEvento modelo)
        {
            var sql = "UPDATE tipo_evento SET descricao = @descricao WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("id", modelo.Id);
                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.TipoEvento> BuscarTodos()
        {
            var modelos = new List<Modelo.TipoEvento>();
            var sql = "SELECT * FROM tipo_evento;";

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

        public Modelo.TipoEvento BuscarPorId(int id)
        {
            var sql = "SELECT * FROM tipo_evento WHERE id = @id;";

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
