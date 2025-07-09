using Npgsql;
using Persistencia;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Local : ConexaoBD
    {
        public const string CamposTabela = "(nome, capacidade, endereco, id_cidade, id_tipo_local)";

        public Modelo.Local ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Local
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("id")),
                Nome = leitor.GetString(leitor.GetOrdinal("nome")),
                Capacidade = leitor.GetInt32(leitor.GetOrdinal("capacidade")),
                Endereco = leitor.GetString(leitor.GetOrdinal("endereco"))
            };

            if (PossuiColuna(leitor, "tipo_local_id") && !leitor.IsDBNull(leitor.GetOrdinal("tipo_local_id")) &&
                PossuiColuna(leitor, "tipo_local_descricao") && !leitor.IsDBNull(leitor.GetOrdinal("tipo_local_descricao")))
            {
                modelo.TipoLocal = new Modelo.TipoLocal
                {
                    Id = leitor.GetInt32(leitor.GetOrdinal("tipo_local_id")),
                    Descricao = leitor.GetString(leitor.GetOrdinal("tipo_local_descricao"))
                };
            }

            if (PossuiColuna(leitor, "cidade_id"))
            {
                var cidade = new Modelo.Cidade
                {
                    Id = leitor.GetInt32(leitor.GetOrdinal("cidade_id"))
                };

                if (PossuiColuna(leitor, "cidade_descricao") && !leitor.IsDBNull(leitor.GetOrdinal("cidade_descricao")))
                    cidade.Descricao = leitor.GetString(leitor.GetOrdinal("cidade_descricao"));

                if (PossuiColuna(leitor, "cidade_codigo_ibge") && !leitor.IsDBNull(leitor.GetOrdinal("cidade_codigo_ibge")))
                    cidade.CodigoIbge = leitor.GetInt32(leitor.GetOrdinal("cidade_codigo_ibge"));

                if (PossuiColuna(leitor, "uf_id"))
                {
                    var uf = new Modelo.Uf
                    {
                        Id = leitor.GetInt32(leitor.GetOrdinal("uf_id"))
                    };

                    if (PossuiColuna(leitor, "uf_descricao") && !leitor.IsDBNull(leitor.GetOrdinal("uf_descricao")))
                        uf.Descricao = leitor.GetString(leitor.GetOrdinal("uf_descricao"));

                    if (PossuiColuna(leitor, "uf_codigo_ibge") && !leitor.IsDBNull(leitor.GetOrdinal("uf_codigo_ibge")))
                        uf.CodigoIbge = leitor.GetInt32(leitor.GetOrdinal("uf_codigo_ibge"));

                    if (PossuiColuna(leitor, "pais_id"))
                    {
                        var pais = new Modelo.Pais
                        {
                            Id = leitor.GetInt32(leitor.GetOrdinal("pais_id"))
                        };

                        if (PossuiColuna(leitor, "pais_descricao") && !leitor.IsDBNull(leitor.GetOrdinal("pais_descricao")))
                            pais.Descricao = leitor.GetString(leitor.GetOrdinal("pais_descricao"));

                        if (PossuiColuna(leitor, "pais_codigo_ibge") && !leitor.IsDBNull(leitor.GetOrdinal("pais_codigo_ibge")))
                            pais.CodigoIbge = leitor.GetInt32(leitor.GetOrdinal("pais_codigo_ibge"));

                        uf.Pais = pais;
                    }

                    cidade.Uf = uf;
                }

                modelo.Cidade = cidade;
            }

            return modelo;
        }


        public int Inserir(Modelo.Local modelo)
        {
            var sql = $@"
                INSERT INTO local {CamposTabela}
                VALUES (@nome, @capacidade, @endereco, @id_cidade, @id_tipo_local)
                RETURNING id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("nome", modelo.Nome);
                comando.Parameters.AddWithValue("capacidade", modelo.Capacidade);
                comando.Parameters.AddWithValue("endereco", modelo.Endereco);
                comando.Parameters.AddWithValue("id_cidade", modelo.Cidade.Id);
                comando.Parameters.AddWithValue("id_tipo_local", modelo.TipoLocal.Id);

                var id = comando.ExecuteScalar();
                return Convert.ToInt32(id);
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM local WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.Local modelo)
        {
            var sql = @"
                UPDATE local 
                SET nome = @nome, capacidade = @capacidade, endereco = @endereco, 
                    id_tipo_local = @id_tipo_local, id_cidade = @id_cidade 
                WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("nome", modelo.Nome);
                comando.Parameters.AddWithValue("capacidade", modelo.Capacidade);
                comando.Parameters.AddWithValue("endereco", modelo.Endereco);
                comando.Parameters.AddWithValue("id_tipo_local", modelo.TipoLocal.Id);
                comando.Parameters.AddWithValue("id_cidade", modelo.Cidade.Id);
                comando.Parameters.AddWithValue("id", modelo.Id);

                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.Local> BuscarTodos()
        {
            var modelos = new List<Modelo.Local>();
            var sql = @"
                SELECT 
                    l.id,
                    l.nome,
                    l.capacidade,
                    l.endereco,
                    tl.id AS tipo_local_id,
                    tl.descricao AS tipo_local_descricao,
                    l.id_cidade
                FROM local l
                INNER JOIN tipolocal tl ON l.id_tipo_local = tl.id;";

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

        public Modelo.Local BuscarPorId(int id)
        {
            var sql = @"
            SELECT 
                l.id,
                l.nome,
                l.capacidade,
                l.endereco,
                l.id_cidade,
                -- TipoLocal
                tl.id AS tipo_local_id,
                tl.descricao AS tipo_local_descricao,

                -- Cidade
                c.id AS cidade_id,
                c.descricao AS cidade_descricao,
                c.codigo_ibge AS cidade_codigo_ibge,

                -- UF
                u.id AS uf_id,
                u.descricao AS uf_descricao,
                u.codigo_ibge AS uf_codigo_ibge,

                -- País
                p.id AS pais_id,
                p.descricao AS pais_descricao,
                p.codigo_ibge AS pais_codigo_ibge

            FROM local l
            INNER JOIN tipolocal tl ON l.id_tipo_local = tl.id
            INNER JOIN cidade c ON l.id_cidade = c.id
            INNER JOIN uf u ON c.id_uf = u.id
            INNER JOIN pais p ON u.id_pais = p.id
            WHERE l.id = @id;";

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

        private bool PossuiColuna(IDataRecord leitor, string nomeColuna)
        {
            for (int i = 0; i < leitor.FieldCount; i++)
            {
                if (leitor.GetName(i).Equals(nomeColuna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
