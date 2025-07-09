using Npgsql;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class TipoLocal : ConexaoBD
    {
        public const string CamposTabela = "(descricao)";

        public Modelo.TipoLocal ObterModelo(NpgsqlDataReader leitor)
        {
            return new Modelo.TipoLocal
            {
                Id = leitor.GetInt32(0),
                Descricao = leitor.GetString(1)
            };
        }

        public int Inserir(Modelo.TipoLocal modelo)
        {
            var sql = $@"
                INSERT INTO tipolocal {CamposTabela}
                VALUES (@descricao)
                RETURNING id;
            ";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                var id = comando.ExecuteScalar();
                return Convert.ToInt32(id);
            }
        }

        public void Excluir(int id)
        {
            var sql = "DELETE FROM tipolocal WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("id", id);
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Modelo.TipoLocal modelo)
        {
            var sql = "UPDATE tipolocal SET descricao = @descricao WHERE id = @id;";

            using (var conexao = GetConnection())
            using (var comando = new NpgsqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("descricao", modelo.Descricao);
                comando.Parameters.AddWithValue("id", modelo.Id);
                comando.ExecuteNonQuery();
            }
        }

        public List<Modelo.TipoLocal> BuscarTodos()
        {
            var modelos = new List<Modelo.TipoLocal>();
            var sql = "SELECT * FROM tipolocal;";

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

        public Modelo.TipoLocal BuscarPorId(int id)
        {
            var sql = "SELECT * FROM tipolocal WHERE id = @id;";

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
