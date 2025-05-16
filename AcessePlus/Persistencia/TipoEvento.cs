using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class TipoEvento : ConexaoBD
    {
        public const string CamposTabela = "(descricao)";
        public Modelo.TipoEvento ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.TipoEvento();

            modelo.Id = leitor.GetInt32(0);
            modelo.Descricao = leitor.GetString(1);

            return modelo;
        }
        public void Inserir(Modelo.TipoEvento modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO tipo_evento {0}" +
                " VALUES (@descricao);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));

            comando.ExecuteNonQuery();
        }
        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM tipo_evento" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.TipoEvento modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE tipo_evento " +
                " SET descricao = @descricao" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.TipoEvento> BuscarTodos()
        {
            List<Modelo.TipoEvento> modelos = new List<Modelo.TipoEvento>();

            var sql = "SELECT * FROM tipo_evento;";

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
        public Modelo.TipoEvento BuscarPorId(int Id)
        {
            Modelo.TipoEvento modelo = null;

            List<Modelo.TipoEvento> modelos = new List<Modelo.TipoEvento>();

            var sql = "SELECT * FROM tipo_evento WHERE id=@id;";

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
