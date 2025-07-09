using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class Evento : ConexaoBD
    {
        public const string CamposTabela = "(nome, descricao, id_local, id_tipo)";

        public Modelo.Evento ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Evento
            {
                Id = leitor.GetInt32(0),
                Nome = leitor.GetString(1),
                Descricao = leitor.GetString(2),
                Local = { Id = leitor.GetInt32(3) },
                TipoEvento = { Id = leitor.GetInt32(4) }
            };

            return modelo;
        }

        public void Inserir(Modelo.Evento modelo)
        {
            var sql = $"INSERT INTO evento {CamposTabela} VALUES (@nome, @descricao, @id_local, @id_tipo);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("nome", modelo.Nome);
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("id_local", modelo.Local.Id);
                comando.Parameters.AddWithValue("id_tipo", modelo.TipoEvento.Id);

                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM evento WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.Evento modelo)
        {
            var sql = @"
                UPDATE evento 
                SET nome = @nome, descricao = @descricao, id_local = @id_local, id_tipo = @id_tipo 
                WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("nome", modelo.Nome);
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("id_local", modelo.Local.Id);
                comando.Parameters.AddWithValue("id_tipo", modelo.TipoEvento.Id);
                comando.Parameters.AddWithValue("id", modelo.Id);

                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.Evento> BuscarTodos()
        {
            var modelos = new List<Modelo.Evento>();
            var sql = "SELECT * FROM evento;";

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

        public Modelo.Evento BuscarPorId(int id)
        {
            var sql = "SELECT * FROM evento WHERE id = @id;";

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
