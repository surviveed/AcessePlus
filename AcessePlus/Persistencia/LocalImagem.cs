using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class LocalImagem : ConexaoBD
    {
        public void Inserir(AcessePlus.Modelo.LocalImagem modelo)
        {
            var sql = @"
                INSERT INTO localimagem (localid, imagem, nomearquivo, ordem, datacadastro)
                VALUES (@localid, @imagem, @nomearquivo, @ordem, @datacadastro);";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("localid", modelo.LocalId);
                comando.Parameters.AddWithValue("imagem", modelo.Imagem);
                comando.Parameters.AddWithValue("nomearquivo", modelo.NomeArquivo ?? (object)DBNull.Value);
                comando.Parameters.AddWithValue("ordem", modelo.Ordem);
                comando.Parameters.AddWithValue("datacadastro", modelo.DataCadastro);

                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.LocalImagem> BuscarPorLocalId(int localId)
        {
            var lista = new List<Modelo.LocalImagem>();
            var sql = @"
                SELECT id, localid, imagem, nomearquivo, ordem, datacadastro 
                FROM localimagem 
                WHERE localid = @localid 
                ORDER BY ordem;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("localid", localId);

                using (var leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        var img = new Modelo.LocalImagem
                        {
                            Id = leitor.GetInt32(0),
                            LocalId = leitor.GetInt32(1),
                            Imagem = (byte[])leitor["imagem"],
                            NomeArquivo = leitor.IsDBNull(3) ? null : leitor.GetString(3),
                            Ordem = leitor.GetInt32(4),
                            DataCadastro = leitor.GetDateTime(5)
                        };
                        lista.Add(img);
                    }
                }
            }

            return lista;
        }

        public List<Modelo.LocalImagem> BuscarTodos()
        {
            var lista = new List<Modelo.LocalImagem>();
            var sql = "SELECT id, localid, imagem, nomearquivo, ordem, datacadastro FROM localimagem ORDER BY ordem;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    var imagem = new Modelo.LocalImagem
                    {
                        Id = reader.GetInt32(0),
                        LocalId = reader.GetInt32(1),
                        Imagem = (byte[])reader["imagem"],
                        NomeArquivo = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Ordem = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        DataCadastro = reader.IsDBNull(5) ? default : reader.GetDateTime(5)
                    };
                    lista.Add(imagem);
                }
            }

            return lista;
        }
    }
}
