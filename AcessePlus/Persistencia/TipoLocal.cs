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
            var modelo = new Modelo.TipoLocal();

            modelo.Id = leitor.GetInt32(0);
            modelo.Descricao = leitor.GetString(1);

            return modelo;
        }

        public int Inserir(Modelo.TipoLocal modelo)
        {
            var sql = $@"
                INSERT INTO tipolocal {CamposTabela}
                VALUES (@descricao)
                RETURNING id;
            ";

            using var comando = new NpgsqlCommand(sql, Conexao);
            comando.Parameters.AddWithValue("descricao", modelo.Descricao);

            var id = comando.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DELETE FROM tipolocal WHERE id = @id");

            using var comando = new NpgsqlCommand(sb.ToString(), Conexao);
            comando.Parameters.AddWithValue("id", Id);

            comando.ExecuteNonQuery();
        }

        public void Atualizar(Modelo.TipoLocal modelo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE tipolocal SET descricao = @descricao WHERE id = @id");

            using var comando = new NpgsqlCommand(sb.ToString(), Conexao);
            comando.Parameters.AddWithValue("descricao", modelo.Descricao);
            comando.Parameters.AddWithValue("id", modelo.Id);

            comando.ExecuteNonQuery();
        }

        public List<Modelo.TipoLocal> BuscarTodos()
        {
            List<Modelo.TipoLocal> modelos = new List<Modelo.TipoLocal>();

            var sql = "SELECT * FROM tipolocal;";

            using var comando = new NpgsqlCommand(sql, Conexao);
            using var leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                var modelo = ObterModelo(leitor);
                modelos.Add(modelo);
            }

            return modelos;
        }

        public Modelo.TipoLocal BuscarPorId(int Id)
        {
            Modelo.TipoLocal modelo = null;

            var sql = "SELECT * FROM tipolocal WHERE id = @id;";

            using var comando = new NpgsqlCommand(sql, Conexao);
            comando.Parameters.AddWithValue("id", Id);

            using var leitor = comando.ExecuteReader();

            if (leitor.Read())
            {
                modelo = ObterModelo(leitor);
            }

            return modelo;
        }
    }
}
