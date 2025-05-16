using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Evento : ConexaoBD
    {
        public const string CamposTabela = "(nome, descricao, id_local,id_tipo)";
        public Modelo.Evento ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Evento();

            modelo.Id = leitor.GetInt32(0);
            modelo.Nome = leitor.GetString(1);
            modelo.Descricao = leitor.GetString(2);
            modelo.Local.Id = leitor.GetInt32(3);
            modelo.TipoEvento.Id = leitor.GetInt32(4);

            return modelo;
        }
        public void Inserir(Modelo.Evento modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO evento {0}" +
                " VALUES (@nome,@descricao,@id_local,@id_tipo);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("nome", modelo.Nome));
            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("id_local", modelo.Local.Id));
            comando.Parameters.Add(new NpgsqlParameter("id_tipo", modelo.TipoEvento.Id));

            comando.ExecuteNonQuery();
        }
        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM evento" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.Evento modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE evento " +
                " SET nome = @nome, descricao = @descricao, id_local=@id_local, id_tipo=@id_tipo" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("nome", modelo.Nome));
            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("id_local", modelo.Local.Id));
            comando.Parameters.Add(new NpgsqlParameter("id_tipo", modelo.TipoEvento.Id));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Evento> BuscarTodos()
        {
            List<Modelo.Evento> modelos = new List<Modelo.Evento>();

            var sql = "SELECT * FROM evento;";

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
        public Modelo.Evento BuscarPorId(int Id)
        {
            Modelo.Evento modelo = null;

            List<Modelo.Evento> modelos = new List<Modelo.Evento>();

            var sql = "SELECT * FROM evento WHERE id=@id;";

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
