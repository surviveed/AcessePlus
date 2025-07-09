using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class LocalUsuario
    {
        public void Inserir(int idUsuario, int idLocal, string permissaoExtra = null)
        {
            using var con = ConexaoBD.GetConnection();
            var cmd = new NpgsqlCommand(
                "INSERT INTO usuario_local (id_usuario, id_local, permissao_extra) VALUES (@id_usuario, @id_local, @permissao_extra)", con);

            cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@id_local", idLocal);
            cmd.Parameters.AddWithValue("@permissao_extra", (object?)permissaoExtra ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public List<Modelo.LocalUsuario> ListarPorUsuario(int idUsuario)
        {
            using var con = ConexaoBD.GetConnection();
            var cmd = new NpgsqlCommand(
                "SELECT * FROM usuario_local WHERE id_usuario = @id_usuario", con);
            cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

            using var reader = cmd.ExecuteReader();
            var lista = new List<Modelo.LocalUsuario>();

            while (reader.Read())
            {
                lista.Add(new Modelo.LocalUsuario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                    IdLocal = Convert.ToInt32(reader["id_local"]),
                    PermissaoExtra = reader["permissao_extra"]?.ToString()
                });
            }

            return lista;
        }

        public void Remover(int idUsuario, int idLocal)
        {
            using var con = ConexaoBD.GetConnection();
            var cmd = new NpgsqlCommand(
                "DELETE FROM usuario_local WHERE id_usuario = @id_usuario AND id_local = @id_local", con);

            cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@id_local", idLocal);

            cmd.ExecuteNonQuery();
        }
    }
}
