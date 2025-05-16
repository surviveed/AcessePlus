using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Cidade : ConexaoBD
    {
        public const string CamposTabela = "(id_uf, descricao, codigo_ibge)";
        public Modelo.Cidade ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Cidade();

            modelo.Id = leitor.GetInt32(0);
            modelo.Uf.Id = leitor.GetInt32(1);
            modelo.Descricao = leitor.GetString(2);
            modelo.CodigoIbge = leitor.GetInt32(3);

            return modelo;
        }
        public void Inserir(Modelo.Cidade modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO cidade {0}" +
                " VALUES (@id_uf,@descricao,@codigo_ibge);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id_uf", modelo.Uf.Id));
            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("codigo_ibge", modelo.CodigoIbge));

            comando.ExecuteNonQuery();
        }
        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM cidade" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.Cidade modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE cidade " +
                " SET id_uf = @id_uf, descricao = @descricao, codigo_ibge=@codigo_ibge" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id_uf", modelo.Uf.Id));
            comando.Parameters.Add(new NpgsqlParameter("descricao", modelo.Descricao));
            comando.Parameters.Add(new NpgsqlParameter("codigo_ibge", modelo.CodigoIbge));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Cidade> BuscarTodos()
        {
            List<Modelo.Cidade> modelos = new List<Modelo.Cidade>();

            var sql = "SELECT * FROM cidade;";

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
        public Modelo.Cidade BuscarPorId(int Id)
        {
            Modelo.Cidade modelo = null;

            List<Modelo.Cidade> modelos = new List<Modelo.Cidade>();

            var sql = "SELECT * FROM cidade WHERE id=@id;";

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
