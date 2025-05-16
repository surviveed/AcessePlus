using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Pais : ConexaoBD
    {
        public const string CamposTabela = "(descricao, codigo_ibge)";
        public Modelo.Pais ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Pais();

            modelo.Id = leitor.GetInt32(0);
            modelo.Descricao = leitor.GetString(1);
            modelo.CodigoIbge = leitor.GetInt32(2);

            return modelo;
        }
        public void Inserir(Modelo.Pais modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO pais {0}" +
                " VALUES (@descricao,@codigo_ibge);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("codigo_ibge", modelo.CodigoIbge));

            comando.ExecuteNonQuery();
        }
        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM pais" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.Pais modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE pais " +
                " SET descricao = @descricao, codigo_ibge = @codigo_ibge" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("codigo_ibge", modelo.CodigoIbge));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Pais> BuscarTodos()
        {
            List<Modelo.Pais> modelos = new List<Modelo.Pais>();

            var sql = "SELECT * FROM pais;";

            NpgsqlCommand comando = new NpgsqlCommand(sql, Conexao);

            NpgsqlDataReader leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                var modelo = ObterModelo(leitor);

                modelos.Add(modelo);
            }
            leitor.Close();

            return modelos;
        }
        public Modelo.Pais BuscarPorId(int Id)
        {
            Modelo.Pais modelo = null;

            List<Modelo.Pais> modelos = new List<Modelo.Pais>();

            var sql = "SELECT * FROM pais WHERE id=@id;";

            NpgsqlCommand comando = new NpgsqlCommand(sql, Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            NpgsqlDataReader leitor = comando.ExecuteReader();

            if (leitor.Read())
            {
                modelo = ObterModelo(leitor);

                modelos.Add(modelo);
            }
            leitor.Close();

            return modelo;
        }
    }
}
