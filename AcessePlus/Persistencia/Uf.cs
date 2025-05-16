using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Uf : ConexaoBD
    {
        public const string CamposTabela = "(id_pais,descricao,codigo_ibge)";
        public Modelo.Uf ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Uf();

            modelo.Id = leitor.GetInt32(0);
            modelo.Pais.Id = leitor.GetInt32(1);
            modelo.Descricao = leitor.GetString(2);
            modelo.CodigoIbge = leitor.GetInt32(3);

            return modelo;
        }
        public void Inserir(Modelo.Uf modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO uf {0}" +
                " VALUES (@id_pais,@descricao,@codigo_ibge);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id_pais", modelo.Pais.Id));
            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("codigo_ibge", modelo.CodigoIbge));

            comando.ExecuteNonQuery();
        }
        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM uf" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.Uf modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE uf " +
                " SET id_pais=@id_pais, descricao = @descricao,codigo_ibge=@codigo_ibge" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id_pais", modelo.Pais.Id));
            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("codigo_ibge", modelo.CodigoIbge));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Uf> BuscarTodos()
        {
            List<Modelo.Uf> modelos = new List<Modelo.Uf>();

            var sql = "SELECT * FROM uf;";

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
        public Modelo.Uf BuscarPorId(int Id)
        {
            Modelo.Uf modelo = null;

            List<Modelo.Uf> modelos = new List<Modelo.Uf>();

            var sql = "SELECT * FROM uf WHERE id=@id;";

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
