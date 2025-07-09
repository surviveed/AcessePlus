using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class Avaliacao : ConexaoBD
    {
        public const string CamposTabela = "(comentario, tipo_acessibilidade, tipo, id_local, id_usuario)";

        public Modelo.Avaliacao ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Avaliacao();

            modelo.Id = leitor.GetInt32(0);
            modelo.Comentario = leitor.GetString(1);
            modelo.TipoAcessibilidade_Enum = (Modelo.Avaliacao.eTipoAcessibilidade)leitor.GetChar(2);
            modelo.Tipo_Enum = (Modelo.Avaliacao.eTipo)leitor.GetChar(3);
            modelo.Local.Id = leitor.GetInt32(4);
            modelo.Usuario.Id = leitor.GetInt32(5);

            return modelo;
        }

        public void Inserir(Modelo.Avaliacao modelo)
        {
            var sql = $"INSERT INTO avaliacao {CamposTabela} VALUES (@comentario, @tipo_acessibilidade, @tipo, @id_local, @id_usuario);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("comentario", modelo.Comentario);
                comando.Parameters.AddWithValue("tipo_acessibilidade", modelo.TipoAcessibilidade);
                comando.Parameters.AddWithValue("tipo", modelo.Tipo);
                comando.Parameters.AddWithValue("id_local", modelo.Local.Id);
                comando.Parameters.AddWithValue("id_usuario", modelo.Usuario.Id);

                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM avaliacao WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.Avaliacao modelo)
        {
            var sql = @"
                UPDATE avaliacao 
                SET comentario = @comentario, 
                    tipo_acessibilidade = @tipo_acessibilidade, 
                    tipo = @tipo, 
                    id_local = @id_local, 
                    id_usuario = @id_usuario
                WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("comentario", modelo.Comentario);
                comando.Parameters.AddWithValue("tipo_acessibilidade", modelo.TipoAcessibilidade);
                comando.Parameters.AddWithValue("tipo", modelo.Tipo);
                comando.Parameters.AddWithValue("id_local", modelo.Local.Id);
                comando.Parameters.AddWithValue("id_usuario", modelo.Usuario.Id);
                comando.Parameters.AddWithValue("id", modelo.Id);

                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.Avaliacao> BuscarTodos()
        {
            var modelos = new List<Modelo.Avaliacao>();
            var sql = "SELECT * FROM avaliacao;";

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

        public Modelo.Avaliacao BuscarPorId(int id)
        {
            var sql = "SELECT * FROM avaliacao WHERE id = @id;";

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

        public List<Modelo.Avaliacao> BuscarPorIdLocal(int idLocal)
        {
            var modelos = new List<Modelo.Avaliacao>();
            var sql = "SELECT * FROM avaliacao WHERE id_local = @id_local;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id_local", idLocal);

                using (var leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        modelos.Add(ObterModelo(leitor));
                    }
                }
            }

            return modelos;
        }
    }
}
