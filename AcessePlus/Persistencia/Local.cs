using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Local : ConexaoBD
    {
        public const string CamposTabela = "(nome, capacidade, endereco,observacoes,id_cidade)";
        public Modelo.Local ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Local();

            modelo.Id = leitor.GetInt32(0);
            modelo.Nome = leitor.GetString(1);
            modelo.Capacidade = leitor.GetInt32(2);
            modelo.Endereco= leitor.GetString(3);
            modelo.Observacoes= leitor.GetString(4);
            modelo.Cidade.Id = leitor.GetInt32(5);

            return modelo;
        }
        public void Inserir(Modelo.Local modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO local {0}" +
                " VALUES (@nome,@capacidade,@endereco,@observacoes,@id_cidade);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("nome", modelo.Nome));
            comando.Parameters.Add(new NpgsqlParameter("capacidade", modelo.Capacidade));
            comando.Parameters.Add(new NpgsqlParameter("endereco", modelo.Endereco));
            comando.Parameters.Add(new NpgsqlParameter("observacoes", modelo.Observacoes));
            comando.Parameters.Add(new NpgsqlParameter("id_cidade", modelo.Cidade.Id));

            comando.ExecuteNonQuery();
        }
        public void Excluir(int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM local" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.Local modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE local " +
                " SET nome = @nome, capacidade = @capacidade, endereco=@endereco, observacoes=@observacoes, id_cidade=@id_cidade" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("nome", modelo.Nome));
            comando.Parameters.Add(new NpgsqlParameter("capacidade", modelo.Capacidade));
            comando.Parameters.Add(new NpgsqlParameter("endereco", modelo.Endereco));
            comando.Parameters.Add(new NpgsqlParameter("observacoes", modelo.Observacoes));
            comando.Parameters.Add(new NpgsqlParameter("id_cidade", modelo.Cidade.Id));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Local> BuscarTodos()
        {
            List<Modelo.Local> modelos = new List<Modelo.Local>();

            var sql = "SELECT * FROM local;";

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
        public Modelo.Local BuscarPorId(int Id)
        {
            Modelo.Local modelo = null;

            List<Modelo.Local> modelos = new List<Modelo.Local>();

            var sql = "SELECT * FROM local WHERE id=@id;";

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
