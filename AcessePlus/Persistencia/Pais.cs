using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class Pais : ConexaoBD
    {
        public const string CamposTabela = "(descricao, codigo_ibge)";

        public Modelo.Pais ObterModelo(NpgsqlDataReader leitor)
        {
            return new Modelo.Pais
            {
                Id = leitor.GetInt32(0),
                Descricao = leitor.GetString(1),
                CodigoIbge = leitor.GetInt32(2)
            };
        }

        public void Inserir(Modelo.Pais modelo)
        {
            var sql = $"INSERT INTO pais {CamposTabela} VALUES (@descricao, @codigo_ibge);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("codigo_ibge", modelo.CodigoIbge);
                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM pais WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.Pais modelo)
        {
            var sql = @"
                UPDATE pais 
                SET descricao = @descricao, codigo_ibge = @codigo_ibge 
                WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("codigo_ibge", modelo.CodigoIbge);
                comando.Parameters.AddWithValue("id", modelo.Id);
                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.Pais> BuscarTodos()
        {
            var modelos = new List<Modelo.Pais>();
            var sql = "SELECT * FROM pais;";

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

        public Modelo.Pais BuscarPorId(int id)
        {
            var sql = "SELECT * FROM pais WHERE id = @id;";

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
