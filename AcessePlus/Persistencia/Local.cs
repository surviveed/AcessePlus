using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Local : ConexaoBD
    {
        public const string CamposTabela = "(nome, capacidade, endereco, id_cidade, id_tipo_local)";
        public Modelo.Local ObterModelo(NpgsqlDataReader leitor)
        {
            var modelo = new Modelo.Local();

            modelo.Id = leitor.GetInt32(leitor.GetOrdinal("id"));
            modelo.Nome = leitor.GetString(leitor.GetOrdinal("nome"));
            modelo.Capacidade = leitor.GetInt32(leitor.GetOrdinal("capacidade"));
            modelo.Endereco = leitor.GetString(leitor.GetOrdinal("endereco"));

            modelo.TipoLocal = new Modelo.TipoLocal
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("tipo_local_id")),
                Descricao = leitor.GetString(leitor.GetOrdinal("tipo_local_descricao"))
            };

            modelo.Cidade = new Modelo.Cidade
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("id_cidade"))
            };

            return modelo;
        }


        public int Inserir(Modelo.Local modelo)
        {
            var sql = $@"
        INSERT INTO local {CamposTabela}
        VALUES (@nome,@capacidade,@endereco,@id_cidade,@id_tipo_local)
        RETURNING id;";

            using var comando = new NpgsqlCommand(sql, Conexao);
            comando.Parameters.AddWithValue("nome", modelo.Nome);
            comando.Parameters.AddWithValue("capacidade", modelo.Capacidade);
            comando.Parameters.AddWithValue("endereco", modelo.Endereco);
            comando.Parameters.AddWithValue("id_cidade", modelo.Cidade.Id);
            comando.Parameters.AddWithValue("id_tipo_local", modelo.TipoLocal.Id);

            var id = comando.ExecuteScalar();
            return Convert.ToInt32(id);
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
                " SET nome = @nome, capacidade = @capacidade, endereco=@endereco, id_tipo_local=@id_tipo_local, id_cidade=@id_cidade" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("nome", modelo.Nome));
            comando.Parameters.Add(new NpgsqlParameter("capacidade", modelo.Capacidade));
            comando.Parameters.Add(new NpgsqlParameter("endereco", modelo.Endereco));
            comando.Parameters.Add(new NpgsqlParameter("id_tipo_local", modelo.TipoLocal.Id));
            comando.Parameters.Add(new NpgsqlParameter("id_cidade", modelo.Cidade.Id));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Local> BuscarTodos()
        {
            List<Modelo.Local> modelos = new List<Modelo.Local>();

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
